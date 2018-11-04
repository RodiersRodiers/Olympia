Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Public Class BoodschapCompose
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private strOrigBoodschap As String
    Private boolIsReply = False
    Private boolIsDoorstuur = False
    Private strDbError As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim idlid As Integer = Request.QueryString("ID_lid")
        ViewState("ID_Lid") = idlid
        fillUpStrings()
        If Not IsPostBack Then
            setMultiLanguages()
            If Not Request.QueryString("replyID") Is Nothing Then
                If Not Request.QueryString("Noreply") Is Nothing Then
                    boolIsDoorstuur = True
                Else
                    boolIsReply = True
                End If
                ViewState("originalID") = Request.QueryString("replyID")
                If boolIsReply Then
                    loadOriginal()
                End If
                If boolIsDoorstuur Then
                    loadScaledOriginal()
                End If
            End If
        End If
    End Sub

    Private Sub loadOriginal()
        Try
            Dim originalMsg As Boodschappen = myBalOlympia.getBoodschapById(ViewState("originalID"))
            txtAreaInhoud.InnerText = strOrigBoodschap & vbCrLf & Tools.doSetupVBRLFs(originalMsg.Inhoud) & vbCrLf
            txtOntvanger.Text = originalMsg.Zender.Voornaam & ";"
            txtStamnummers.Text = originalMsg.Zender.Naam + ";"
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub loadScaledOriginal()
        Try
            Dim originalMsg As Boodschappen = myBalOlympia.getBoodschapById(ViewState("originalID"))
            txtAreaInhoud.InnerText = strOrigBoodschap & vbCrLf & Tools.doSetupVBRLFs(originalMsg.Inhoud) & vbCrLf
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub fillUpStrings()
        strOrigBoodschap = "Originele Boodschap :"
        strDbError = "Fout in het ophalen van de gegevens"
    End Sub

    Private Sub setMultiLanguages() ''' Fill up the object according to the language
        lblInhoud.Text = "Inhoud: "
        lblOnderwerp.Text = "Onderwerp: "
        lblOntvanger.Text = "Ontvanger: "
        lbleenheid.Text = "Discipline"
        lblTeam.Text = "Groep"
        lblGebruiker.Text = "Gebruiker"
        lnkSend.Text = "Zend"
        lnkAdd.Text = "Toevoegen"
        lblmaxkar.Text = "max.karakters"

        txtOntvanger.Attributes.Add("disabled", "true")
    End Sub

    Public Sub lnkSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkSend.Click
        Try
            Dim myBoodschap As New Boodschappen
            Dim myGebruiker As Gebruikers = Session("Gebruiker")
            Dim i_result As Integer = 0
            Dim inputOntvangers As String = txtStamnummers.Text
            Dim inputArray As String() = Split(inputOntvangers, ";")

            myBoodschap.Datum = Date.Now()
            myBoodschap.Zender = Session("Gebruiker")
            myBoodschap.Inhoud = Tools.doRemoveVBRLFs(txtAreaInhoud.InnerText)

            Dim myListOfOntvangers As New List(Of Integer)
            For i = 0 To UBound(inputArray)
                If Not inputArray(i) = "" Then
                    If Not myBalOlympia.GetGebruiker(inputArray(i)) Is Nothing Then
                        myListOfOntvangers.Add(inputArray(i))
                    End If
                End If
            Next
            myBoodschap.gelezen = BoodschapStatus.Ongelezen

            i_result = myBalOlympia.InsertBoodschap(myBoodschap, myListOfOntvangers)
            If i_result > 0 Then
                Dim script As String = "<script>$(document).ready(function(){ setTimeout('closeIt()',2000 );});</script>"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "Entity", script, False)
            Else
                'error
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class