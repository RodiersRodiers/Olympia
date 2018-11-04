Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Public Class BoodschapReply
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private strDbError As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        fillUpStringFields()
        If Not IsPostBack() Then
            setMultiLanguages()
            loadBoodschap()
        Else
            txtAreaInhoud.InnerText = Tools.doSetupVBRLFs(txtAreaInhoud.InnerText)
        End If
    End Sub

    Private Sub fillUpStringFields() '''Fill up the strings
        strDbError = "Fout in het ophalen van de gegevens"
    End Sub

    Private Sub setMultiLanguages() ''' Fill up the object according to the language
        lblInhoud.Text = "Inhoud"
        lblOnderwerp.Text = "Onderwerp: "
        lblAfzender.Text = "Afzender : "
        lnkReply.Text = "Beantwoorden "
        lnkDoosturen.Text = "Doorsturen "
        lblmaxkar.Text = "max.karakters"
    End Sub

    Private Sub loadBoodschap()
        Try
            Dim IDBoodschap As Integer = Request.QueryString("ID")
            Dim myboodschap As Boodschappen = myBalOlympia.getBoodschapById(IDBoodschap)
            ViewState("id") = myboodschap.Id
            myboodschap.gelezen = BoodschapStatus.Gelezen
            myBalOlympia.UpdateBoodschap(myboodschap)
            lblAfzender2.Text = String.Format("{0} {1}", myboodschap.Zender.Naam, myboodschap.Zender.Voornaam)
            txtAreaInhoud.InnerText = Tools.doSetupVBRLFs(myboodschap.Inhoud)
            txtAreaInhoud.Attributes.Add("enabled", "false")
        Catch ex As Exception
            'UC_Message.setMessage(strDbError, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
        End Try
    End Sub

    Public Sub lnkReply_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkReply.Click
        Response.Redirect("BoodschapCompose.aspx?replyID=" & ViewState("id"))
    End Sub

    Public Sub lnkTransfer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkDoosturen.Click
        Response.Redirect("BoodschapCompose.aspx?replyID=" & ViewState("id") & "&Noreply=true")
    End Sub

End Class