Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia
Imports System.IO
Imports System.Data.OleDb

Partial Class Importgegevens
    Inherits Page
    Private myBalOlympia As New Olympia.BALOlympia.BalGebruikers
    Private myDalOlympia As New Olympia.DALOlympia.DalGebruikers
    Private ResultCount As Integer
    Private strTekst1, strTekst2, strTelRep, strGeimporteerd, strImportEmtpy, strTekst3, strTekst6, _
        strImportError, strImportOk, strverwerkingstijd, strTekst4, strTekst5, strSuccesOpslaan, strDBError, strDBErrorVerwerken, _
        strEmptyDossierNaam, strEmptyDocNr, strEmptyEenheid, strEmptyEntiteit, strEmptyOnderwerp, strSheet99Error As String

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles form1.Load
        fillUpStringFields()
        If Not IsPostBack() Then
            validateToegang()
            setMultiLanguages()
            initPicklists()
        End If
    End Sub

    Private Sub validateToegang()
        Dim i As Integer
        i = myBalOlympia.CheckToegangGebruiker(Session("Gebruiker"), 23)
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing
            Case Rechten_Lid.lezen
                ' btnINSERTAdd.Visible = False
            Case Else
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Beheer Import toegang geweigerd (Domein: 23) "
                mylogging.Type = TypeLogging.vergoedingen
                myBalOlympia.InsertLogging(mylogging)
                Response.Redirect("start.aspx?Error=AD")
        End Select

        Dim mygebruikersToegangen As New List(Of Rechten)
        mygebruikersToegangen = myBalOlympia.CheckToegangenGebruiker(Session("Gebruiker"))
        Dim mytoegang As New Rechten
        For Each mytoegang In mygebruikersToegangen
            Select Case mytoegang.Actie.beschrijving
                Case "pagAanwezigheden"
                    pagAanwezigheden.Visible = True
                Case "pagGebruikers"
                    pagGebruikers.Visible = True
                Case "pagBeheer"
                    myBtn1.Visible = True
                    beheer.Visible = True
                Case "pagVergoedingen"
                    myBtn2.Visible = True
                    vergoeding.Visible = True
            End Select
        Next
    End Sub

    Private Sub fillUpStringFields() 'Fill up the strings
        Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"

        strDBError = "Fout bij het ophalen van de gegevens"
        strDBErrorVerwerken = "Fout bij het verwerken van de gegevens"
        strSuccesOpslaan = "entiteiten werden geïmporteerd in"
        strTekst1 = "externe entiteiten gevonden"
        strTekst2 = "Er zijn geen externe entiteiten gekoppeld"
        strTekst3 = "Er zijn geen gegevens om te importeren"
        strTekst4 = "Er is geen bestand gevonden"
        strTekst5 = "Er is geen directory gevonden"
        strTekst6 = "Geen werkblad gevonden met de naam 1, gelieve te hernoemen."
        strverwerkingstijd = "Verwerkingstijd"
        strImportOk = "Importeren van gegevens succesvol !"
        strTelRep = "TelRep"
        strImportEmtpy = "Er werden geen gegevens verwerkt, gelieve items te selecteren en te linken aan een entiteit"
        strImportError = "Er is een fout opgetreden tijdens het importeren !"
        strGeimporteerd = " geïmporteerd"
        strEmptyEntiteit = "Gelieve een entiteit te selecteren"
        strEmptyDossierNaam = "Gelieve een dossiernaam te selecteren"
        strEmptyDocNr = "Gelieve een documentnr in te vullen"
        strEmptyEenheid = "Gelieve een eenheid te selecteren"
        strEmptyOnderwerp = "Gelieve een onderwerp in te vullen"
        strSheet99Error = "Er is een fout opgetreden. Mogelijks is de naam van het werkblad in Excell verschillende van 1"
    End Sub

    Private Sub setMultiLanguages() ' Fill up the object according to the language
        lblPageTitle.Text = "BEHEER > Import gegevens"
        lblType.Text = "Type"
        btnImport.Text = "Import"
        btnLoadList.Text = "Load File"
    End Sub

    Private Sub initPicklists()
        Try
            cbType.Items.Clear()
            cbType.Items.Add(New ListItem("", 0))
            cbType.Items.Add(New ListItem("andere", TypeImportExt.Trainingsgroepen))
            cbType.Items.Add(New ListItem("Gebruikers", TypeImportExt.Gebruikers))
            cbType.Items.Add(New ListItem("Wedstrijden", TypeImportExt.Wedstrijden))
            If cbType.SelectedValue = 0 Then
                btnLoadList.Visible = False
            Else
                btnLoadList.Visible = True
            End If
        Catch ex As Exception
            ' UC_Message.setMessage(strDBError, CustomMessage.TypeMessage.Fataal, ex)
        End Try
    End Sub

    Private Sub cbType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbType.SelectedIndexChanged
        If cbType.SelectedValue = 0 Then
            btnLoadList.Visible = False
        Else
            Select Case cbType.SelectedItem.Value
                Case TypeImportExt.Trainingsgroepen
                    lblinfo.Text = "Import Trainingsgroepen vanuit excell , tabblad heeft naam 1"
                Case TypeImportExt.Gebruikers
                    lblinfo.Text = "Import lijst Gebruikers vanuit excell , tabblad heeft naam 1"
                Case TypeImportExt.Wedstrijden
                    lblinfo.Text = "Import lijst Wedstrijden vanuit excell bestand , tabblad heeft naam 1"
            End Select
            btnLoadList.Visible = True
        End If
    End Sub

    Private Sub loadDataExtr_Personen(ByVal strPath As String)
        If Directory.Exists(strPath) Then
            'read file (first 10)
            Dim intAantal As Integer = 0

            Dim myFile As String = ("import.xls")
            Dim myfileimport As New String(strPath & " " & myFile)

            'Lezen van de file
            Dim myListgebruikers As New List(Of Gebruikers)(myBalOlympia.readFileExcell(myfileimport))

            If myListgebruikers.Count > 0 Then
                dtgDataGrid.DataSource = myListgebruikers
                dtgDataGrid.DataBind()
                btnImport.Visible = True
                ResultCount = myListgebruikers.Count
            Else
                '  UC_Message.setMessage(strTekst4, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
            End If
        Else
            ' UC_Message.setMessage(strTekst5, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End If
    End Sub

    Private Sub dtgDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgDataGrid.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lblNaam As Label = e.Item.FindControl("lblNaam")
                Dim lblVoornaam As Label = e.Item.FindControl("lblVoornaam")
                Dim lblGebDatum As Label = e.Item.FindControl("lblGebDatum")
                Dim lblgeslacht As Label = e.Item.FindControl("lblgeslacht")
                Dim lblEmail As Label = e.Item.FindControl("lblEmail")
                Dim lblGsm As Label = e.Item.FindControl("lblGsm")
                Dim lblRekNr As Label = e.Item.FindControl("lblRekNr")
                Dim lblGemeente As Label = e.Item.FindControl("lblGemeente")

                Dim mygebruikersOverzicht As Gebruikers = e.Item.DataItem
                lblNaam.Text = mygebruikersOverzicht.Naam
                lblVoornaam.Text = mygebruikersOverzicht.Voornaam
                lblGebDatum.Text = mygebruikersOverzicht.GebDatum
                lblgeslacht.Text = mygebruikersOverzicht.Geslacht
                lblEmail.Text = mygebruikersOverzicht.Email
                lblGsm.Text = mygebruikersOverzicht.GSM
                lblRekNr.Text = mygebruikersOverzicht.Rekeningnummer
                lblGemeente.Text = mygebruikersOverzicht.Gemeente
            End If

        Catch ex As Exception
            Response.Write(" " & ex.StackTrace)
        End Try
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Try

            Dim dateStart As Date = Date.Now
            Dim myList As New List(Of Gebruikers)

            For Each myrow As DataGridItem In dtgDataGrid.Items
                Dim sub_DateStart = Date.Now

                Dim lblNaam As Label = myrow.FindControl("lblNaam")
                Dim lblVoornaam As Label = myrow.FindControl("lblVoornaam")
                Dim lblGebDatum As Label = myrow.FindControl("lblGebDatum")
                Dim lblgeslacht As Label = myrow.FindControl("lblgeslacht")
                Dim lblEmail As Label = myrow.FindControl("lblEmail")
                Dim lblGsm As Label = myrow.FindControl("lblGsm")
                Dim lblRekNr As Label = myrow.FindControl("lblRekNr")
                Dim lblGemeente As Label = myrow.FindControl("lblGemeente")

                Dim mygebruiker As New Gebruikers With {
                    .Naam = lblNaam.Text,
                    .Voornaam = lblVoornaam.Text,
                    .GebDatum = lblGebDatum.Text,
                    .Geslacht = lblgeslacht.Text,
                    .Email = lblEmail.Text,
                    .GSM = lblGsm.Text,
                    .Rekeningnummer = lblRekNr.Text,
                    .Gemeente = lblGemeente.Text,
                    .Paswoord = "pw"
                }
                myList.Add(mygebruiker)

            Next

            myBalOlympia.doImportGebruikers(myList)

            If myList.Count > 0 Then
                Try
                    Dim mylogging As New Logging
                    mylogging.Gebruiker.IdLid = Session("Gebruiker")
                    mylogging.EventLogging = "Import " & myList.Count & " gebruikers"
                    mylogging.Type = 1

                    myBalOlympia.InsertLogging(mylogging)
                Catch ex As Exception
                    'UC_Message.setMessage(strImportError, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                End Try
            Else
                'UC_Message.setMessage(strImportEmtpy, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
            End If

        Catch ex As Exception
            'UC_Message.setMessage(ex.Message & " " & ex.StackTrace, CustomMessage.TypeMessage.Fataal, ex)
        End Try
    End Sub

    Private Sub btnLoadList_Click(sender As Object, e As EventArgs) Handles btnLoadList.Click
        Select Case cbType.SelectedItem.Value
            Case TypeImportExt.Trainingsgroepen

            Case TypeImportExt.Gebruikers
                loadDataExtr_Personen("c:/")
            Case TypeImportExt.Wedstrijden

        End Select

    End Sub
End Class
