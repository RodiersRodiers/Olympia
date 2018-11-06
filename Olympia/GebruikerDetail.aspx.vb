Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Public Class GebruikerDetail
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private ResultCount As Integer
    Private strDeleteConfirm As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim idlid As Integer = Request.QueryString("ID_lid")
            ViewState("ID_Lid") = idlid
            SetMultiLanguages()
            InitPicklists()
            validateToegang()
            LoadData()
            LoadData2()
        End If
    End Sub
    Private Sub validateToegang()
        Dim i As Integer
        i = myBalOlympia.CheckToegangGebruiker(Session("Gebruiker"), 2) 'Lesgever
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing

            Case Rechten_Lid.lezen

            Case Else
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Gebruikersdetail toegang geweigerd (Domein: 2) "
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
                    myBtn2.Visible = True
                    beheer.Visible = True
                Case "pagVergoedingen"
                    myBtn1.Visible = True
                    vergoeding.Visible = True
            End Select
        Next

        i = myBalOlympia.CheckToegangGebruiker(Session("Gebruiker"), 20) 'beheerder
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing
            Case Rechten_Lid.lezen

            Case Else

        End Select

    End Sub
    Private Sub InitPicklists()
        Try
            cbGeslacht.Items.Clear()
            cbGeslacht.Items.Add(New ListItem("M", 1))
            cbGeslacht.Items.Add(New ListItem("V", 2))
        Catch ex As Exception
            ' UC_Message.setMessage(strDBError, CustomMessage.TypeMessage.Fataal, ex)
        End Try
    End Sub
    Private Sub SetMultiLanguages()
        Dim mygebruiker As Gebruikers = myBalOlympia.GetGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"
        Dim mygebruiker2 As Gebruikers = myBalOlympia.GetGebruiker(ViewState("ID_Lid"))
        lblgebruiker.Text = mygebruiker2.Naam & " " & mygebruiker2.Voornaam & " (" & mygebruiker2.GebDatum & ")"
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
        btnRechten.Text = "Rechten"
        btnOverzicht.Text = "Handelingen"
        btnrapport.Text = "Rapport"
        btnDetail.Text = "Detail"
        btnINSERTAdd.Text = "Toevoegen"
        btnINSERTCancel.Text = "Annuleren"
        strDeleteConfirm = "Bent u zeker dat dit record wilt verwijderen ?"
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

    Private Sub Btn_Opslaan_Click(sender As Object, e As EventArgs) Handles btn_Opslaan.Click
        Dim i_Result As Integer
        Dim mygebruiker As New Gebruikers
        mygebruiker.Naam = txtNaam.Text
        mygebruiker.Voornaam = txtVoornaam.Text
        mygebruiker.Email = txtEmail.Text
        mygebruiker.GSM = txtGSM.Text

        If txtGebDatum.Text <> "" Then
            mygebruiker.GebDatum = txtGebDatum.Text
        Else
            mygebruiker.GebDatum = myBalOlympia.GetEmptyDate
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

            If mygebruiker.IdLid > 0 Then
                i_Result = myBalOlympia.UpdateGebruiker(mygebruiker)
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Aanpassen gebruiker " & txtNaam.Text & " " & txtVoornaam.Text
                mylogging.Type = TypeLogging.gebruikers
                myBalOlympia.InsertLogging(mylogging)
            Else
                mygebruiker.Paswoord = "pw"
                mygebruiker.Active = 1
                i_Result = myBalOlympia.InsertGebruiker(mygebruiker)
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Toevoegen gebruiker " & txtNaam.Text & " " & txtVoornaam.Text
                mylogging.Type = TypeLogging.gebruikers
                myBalOlympia.InsertLogging(mylogging)
            End If
            If i_Result = 1 Then
                ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strUpdateOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
            End If
        Catch ex As Exception
            MsgBox("Fout in het opslaan gebruiker !")
        End Try
    End Sub

    Private Function getDefaultSortExpression() As String
        If ViewState("SortExpression") Is Nothing Then
            Return "datum desc"
        Else
            Return String.Format("{0} {1}", ViewState("SortExpression"), ViewState("SortDirection"))
        End If
    End Function

    Private Sub LoadData2()
        Try
            Dim myList As List(Of Lesgeververgoeding) = myBalOlympia.GetLesgeverVergoedingen(getDefaultSortExpression, ViewState("ID_Lid"))
            dtgDataGrid.DataSource = myList
            dtgDataGrid.DataBind()
            ResultCount = myList.Count
        Catch ex As Exception
            'UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Sub dtgDataGrid_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgDataGrid.ItemCommand
        Try
            Dim i_Result As Integer
            Select Case e.CommandName
                Case "INSERT"
                    Dim mylsvgd As New Lesgeververgoeding
                    Dim txtInsertDatum As TextBox = e.Item.FindControl("txtInsertDatum")
                    Dim txtInsertKm As TextBox = e.Item.FindControl("txtInsertKm")
                    Dim txtInsertBedrag As TextBox = e.Item.FindControl("txtInsertBedrag")
                    Dim txtInsertinfo As HtmlTextArea = e.Item.FindControl("txtInsertinfo")
                    mylsvgd.Datum = txtInsertDatum.Text
                    mylsvgd.Km = txtInsertKm.Text
                    mylsvgd.Bedrag = txtInsertBedrag.Text
                    mylsvgd.Gebruiker.IdLid = ViewState("ID_Lid")
                    mylsvgd.info = txtInsertinfo.InnerText
                    Try
                        i_Result = myBalOlympia.InsertLesgeverVergoeding(mylsvgd)
                        dtgDataGrid.ShowFooter = False
                        btnINSERTAdd.Visible = True
                        btnINSERTCancel.Visible = False

                        If i_Result >= 1 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strAddOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                            LoadData2()
                        End If
                    Catch ex As Exception
                        If ex.Message = "119" Then
                            ' UC_Message.setMessage(strDuplicate, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        Else
                            '  UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        End If
                    End Try

                Case "DELETE"
                    Dim mylsvgd As New Lesgeververgoeding With {
                        .Id = dtgDataGrid.DataKeys(e.Item.ItemIndex)
                    }
                    i_Result = myBalOlympia.DeleteLesgeverVergoeding(mylsvgd)

                    LoadData2()
                    ' UC_MessageChild.setMessage(strDeleteOk, CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))

                Case "UPDATE"
                    Try
                        Dim mylsvgd As New Lesgeververgoeding
                        Dim txteditkm As TextBox = e.Item.FindControl("txteditKm")
                        Dim txteditBedrag As TextBox = e.Item.FindControl("txteditBedrag")
                        Dim txteditInfo As HtmlTextArea = e.Item.FindControl("txteditInfo")
                        mylsvgd.Id = dtgDataGrid.DataKeys(e.Item.ItemIndex)
                        mylsvgd.Km = txteditkm.Text
                        mylsvgd.Bedrag = txteditBedrag.Text
                        mylsvgd.info = txteditInfo.InnerText

                        i_Result = myBalOlympia.UpdateLesgeverVergoeding(mylsvgd)
                        If i_Result = 1 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strUpdateOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                        End If
                    Catch ex As Exception
                        ' UC_Message.setMessage(strUpdateError, CustomMessage.TypeMessage.Fataal, ex)
                    End Try

                    btnINSERTAdd.Visible = True
                    dtgDataGrid.EditItemIndex = -1
                    LoadData2()

                Case "CANCEL"
                    dtgDataGrid.EditItemIndex = -1
                    dtgDataGrid.ShowFooter = False
                    LoadData2()
                    btnINSERTAdd.Visible = True

                Case "EDIT"
                    dtgDataGrid.EditItemIndex = e.Item.ItemIndex
                    LoadData2()
                    btnINSERTAdd.Visible = False

            End Select

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Sub dtgDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgDataGrid.ItemDataBound
        Try
            Dim mylsvgd As Lesgeververgoeding = e.Item.DataItem()
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lblId As Label = e.Item.FindControl("lblId")
                Dim lblLidID As Label = e.Item.FindControl("lblLidID")
                Dim lblDatum As Label = e.Item.FindControl("lblDatum")
                Dim lblinfo As Label = e.Item.FindControl("lblinfo")
                Dim lblkm As Label = e.Item.FindControl("lblkm")
                Dim lblBedrag As Label = e.Item.FindControl("lblBedrag")
                Dim cbeDelete As AjaxControlToolkit.ConfirmButtonExtender = e.Item.FindControl("cbeDelete")
                cbeDelete.ConfirmText = strDeleteConfirm
                lblId.Text = mylsvgd.Id
                lblLidID.Text = mylsvgd.Gebruiker.IdLid
                lblDatum.Text = mylsvgd.Datum
                lblinfo.Text = mylsvgd.info
                lblBedrag.Text = mylsvgd.Bedrag
                lblkm.Text = mylsvgd.Km

            End If

            If e.Item.ItemType = ListItemType.EditItem Then
                Dim txteditDatum As Label = e.Item.FindControl("txteditDatum")
                Dim txteditinfo As HtmlTextArea = e.Item.FindControl("txteditinfo")
                Dim txteditkm As TextBox = e.Item.FindControl("txteditkm")
                Dim txteditBedrag As TextBox = e.Item.FindControl("txteditBedrag")
                txteditDatum.Text = mylsvgd.Datum
                txteditinfo.InnerText = mylsvgd.info
                txteditkm.Text = mylsvgd.Km
                txteditBedrag.Text = mylsvgd.Bedrag

            End If

            If e.Item.ItemType = ListItemType.Footer Then
            End If

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTCancel.Click
        dtgDataGrid.ShowFooter = False
        btnINSERTAdd.Visible = True
        btnINSERTCancel.Visible = False
    End Sub

    Private Sub btnINSERTAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTAdd.Click
        dtgDataGrid.ShowFooter = True
        btnINSERTAdd.Visible = False
        btnINSERTCancel.Visible = True
    End Sub


    Private Sub Btn_Changepw_Click(sender As Object, e As EventArgs) Handles btn_Changepw.Click

    End Sub

    Private Sub BtnRechten_Click(sender As Object, e As EventArgs) Handles btnRechten.Click
        Response.Redirect("GebruikersRechten.aspx?ID_lid=" & ViewState("ID_Lid"))
    End Sub


    Private Sub BtnOverzicht_Click(sender As Object, e As EventArgs) Handles btnOverzicht.Click
        Response.Redirect("GebruikersOverzicht.aspx?ID_lid=" & ViewState("ID_Lid"))
    End Sub

    Private Sub btnrapport_Click(sender As Object, e As EventArgs) Handles btnrapport.Click
        Response.Redirect("GebruikersRapport.aspx?ID_lid=" & ViewState("ID_Lid"))
    End Sub
    Private Sub btnDetail_Click(sender As Object, e As EventArgs) Handles btnDetail.Click
        Response.Redirect("GebruikerDetail.aspx?ID_lid=" & ViewState("ID_Lid"))
    End Sub
End Class