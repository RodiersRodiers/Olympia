Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Public Class GebruikerDetail
    Inherits Page
    Private myBalOlympia As New Olympia.BALOlympia.BalGebruikers
    Private ResultCount As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        setMultiLanguages()
        If Not IsPostBack Then
            Dim idlid As Integer = Request.QueryString("ID_lid")
            ViewState("ID_Lid") = idlid
            initPicklists()
            LoadData()
        End If
    End Sub

    Private Sub initPicklists()
        Try
            cbGeslacht.Items.Clear()
            cbGeslacht.Items.Add(New ListItem("M", 1))
            cbGeslacht.Items.Add(New ListItem("V", 2))
        Catch ex As Exception
            ' UC_Message.setMessage(strDBError, CustomMessage.TypeMessage.Fataal, ex)
        End Try
    End Sub
    Private Sub setMultiLanguages()
        Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"

        lblPageTitle.Text = "Gebruikers > Beheer Algemeen"
        lblEmail.Text = "Email"
        lblNaam.Text = "Naam"
        lblVoornaam.Text = "Voornaam"
        lblGeslacht.Text = "Geslacht"
        lblGSM.Text = "GSM"
        lblGebDatum.Text = "Geb Datum"
        lblGemeente.Text = "Gemeente"
        lblPostcode.Text = "Postcode"
        lblStraat.Text = "Straat"
        lblHuisnr.Text = "HuisNr"
        lblRekNr.Text = "RekNr"
        lblInfo.Text = "Info"
        btn_Changepw.Text = "Verander Paswoord"
        btn_Annuleer.Text = "Annuleer"
        btn_Opslaan.Text = "Opslaan"
        btn_Edit.Text = "aanpassen"
        btnRechten.Text = "Rechten"
        btnOverzicht.Text = "Overzicht"

    End Sub

    Private Sub LoadData()
        Try

            If ViewState("ID_Lid") > 0 Then
                Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(ViewState("ID_Lid"))
                txtNaam.Text = mygebruiker.Naam
                txtVoornaam.Text = mygebruiker.Voornaam
                txtEmail.Text = mygebruiker.Email
                txtGSM.Text = mygebruiker.GSM

                If mygebruiker.GebDatum = myBalOlympia.getEmptyDate Then
                    txtGebDatum.Text = ""
                Else
                    txtGebDatum.Text = mygebruiker.GebDatum.ToString("dd/MM/yyyy")
                End If

                txtRekNr.Text = mygebruiker.Rekeningnummer
                txtGemeente.Text = mygebruiker.Gemeente
                txtpostcode.Text = mygebruiker.Postcode
                txtStraat.Text = mygebruiker.Straat
                txtHuisnr.Text = mygebruiker.Huisnr
                txtareaInfo.InnerText = mygebruiker.Info

                If mygebruiker.Geslacht = "M" Then
                    cbGeslacht.SelectedValue = 1
                Else
                    cbGeslacht.SelectedValue = 2
                End If

            End If
        Catch ex As Exception
            MsgBox("Fout in het ophalen gebruiker !")
        End Try
    End Sub

    Private Sub btn_Opslaan_Click(sender As Object, e As EventArgs) Handles btn_Opslaan.Click
        Dim i_Result As Integer
        Dim mygebruiker As New Gebruikers With {
            .Naam = txtNaam.Text,
            .Voornaam = txtVoornaam.Text,
            .Email = txtEmail.Text,
            .GSM = txtGSM.Text
        }

        If txtGebDatum.Text <> "" Then
            mygebruiker.GebDatum = txtGebDatum.Text
        Else
            mygebruiker.GebDatum = myBalOlympia.getEmptyDate
        End If

        mygebruiker.Geslacht = cbGeslacht.SelectedItem.Text
        mygebruiker.Rekeningnummer = txtRekNr.Text
        mygebruiker.Gemeente = txtGemeente.Text
        mygebruiker.Postcode = txtpostcode.Text
        mygebruiker.Straat = txtStraat.Text
        mygebruiker.Huisnr = txtHuisnr.Text
        mygebruiker.Info = txtareaInfo.InnerText.ToString
        mygebruiker.IdLid = ViewState("ID_Lid")
        Try
            Dim mylogging As New Logging
            mylogging.Gebruiker.IdLid = ViewState("ID_Lid")
            mylogging.EventLogging = "Aanpassen gebruiker " & txtNaam.Text & " " & txtVoornaam.Text
            mylogging.Type = TypeLogging.gebruikers

            myBalOlympia.InsertLogging(mylogging)
            If mygebruiker.IdLid > 0 Then
                i_Result = myBalOlympia.UpdateGebruiker(mygebruiker)
            Else
                i_Result = myBalOlympia.InsertGebruiker(mygebruiker)
            End If
            If i_Result = 1 Then
                ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strUpdateOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
            End If
        Catch ex As Exception
            MsgBox("Fout in het opslaan gebruiker !")
        End Try
    End Sub

    Private Sub btn_Changepw_Click(sender As Object, e As EventArgs) Handles btn_Changepw.Click

    End Sub

    Private Sub btnRechten_Click(sender As Object, e As EventArgs) Handles btnRechten.Click
        Response.Redirect("GebruikersRechten.aspx?ID_lid=" & ViewState("ID_Lid"))
    End Sub


    Private Sub btnOverzicht_Click(sender As Object, e As EventArgs) Handles btnOverzicht.Click
        Response.Redirect("GebruikersOverzicht.aspx?ID_lid=" & ViewState("ID_Lid"))
    End Sub
End Class