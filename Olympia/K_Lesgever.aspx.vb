Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Class K_Lesgever
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private ResultCount As Integer
    Private strDeleteConfirm, strDeleteError, strDeleteOk, strDBError, strPagingTot, strHeaderTitle, strPagingRecordsFound, strInsertBeschrijving, strUpdateOk,
        strUpdateError, strPrimaryKeyAllreadyExists, strAddError, strAddOk, strCompleted As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FillUpStringFields()

        If Not IsPostBack Then
            Dim idlid As Integer = Session("gebruiker")
            ViewState("ID_Lid") = idlid
            LoadDBPicklists()
            validateToegang()
            LoadData(txtfilter.Text)
        End If
    End Sub

    Private Sub validateToegang()
        Dim i As Integer
        i = myBalOlympia.CheckToegangGebruiker(Session("Gebruiker"), 15) 'Lesgever
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing
                cbopsteller.SelectedValue = Session("Gebruiker")
            Case Rechten_Lid.lezen
                btnINSERTAdd.Visible = False
            Case Else
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Vergoeding Lesgevers toegang geweigerd (Domein: 15) "
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
                btnINSERTAdd.Visible = False
            Case Else
                cbopsteller.Enabled = False
                cbopsteller.SelectedValue = Session("Gebruiker")
        End Select

    End Sub

    Private Sub LoadDBPicklists()
        Try
            'Trainers
            cbopsteller.DataSource = myBalOlympia.GetGebruikers("naam ASC", "")
            cbopsteller.DataTextField = "Naam"
            cbopsteller.DataValueField = "IDLid"
            cbopsteller.DataBind()
            'cbopsteller.Items.Insert(0, New ListItem("", 0)) 'Standaard invoegen op de eerste plaats

        Catch ex As Exception
            'UC_Message.setMessage(strDbError, CustomMessage.TypeMessage.Fataal, ex)
        End Try
    End Sub

    Private Sub FillUpStringFields() 'Fill up the strings
        Dim mygebruiker As Gebruikers = myBalOlympia.GetGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"

        lblPageTitle.Text = "Vergoeding Lesgever"
        lblFilter.Text = "Filter"
        lblOpsteller.Text = "Gebruiker"
        btnFilter.Text = "Zoek"
        btnWisFilter.Text = "Wis Filter"
        strInsertBeschrijving = "Beschrijving Verplicht "
        btnINSERTAdd.Text = "Toevoegen"
        btnINSERTCancel.Text = "Annuleren"
        strCompleted = "record(s) bijgewerkt"
        strAddOk = "Toevoeging succesvol uitgevoerd"
        strAddError = "Er is een fout opgetreden tijdens de toevoeging !"
        strUpdateOk = "Update succesvol uitgevoerd"
        strUpdateError = "Er is een fout opgetreden tijdens de Update !"
        strDeleteError = "Er is een fout opgetreden tijdens het verwijderen !"
        strDeleteOk = "Item succesvol verwijderd"
        strDeleteConfirm = "Bent u zeker dat dit record wilt verwijderen ?"
        strPrimaryKeyAllreadyExists = "Discipline bestaat reeds"
        strDBError = "Fout in het ophalen van de gegevens"
    End Sub

    Private Sub DoFillUpDropDown(ByRef drpSource As DropDownList, ByVal myOb() As Object, ByVal strDataValueField As String, ByVal strDataTextField As String, ByVal selectedItem As String, ByVal strDefaultTekst As String)
        drpSource.DataSource = myOb
        drpSource.DataValueField = strDataValueField
        drpSource.DataTextField = strDataTextField
        drpSource.DataBind()

        If selectedItem <> "" Then
            For Each item As ListItem In drpSource.Items
                If item.Value = selectedItem Then
                    item.Selected = True
                    Exit For
                End If
            Next
        Else
            If strDefaultTekst <> "" Then
                drpSource.Items.Insert(0, New ListItem(strDefaultTekst, ""))
            End If
        End If
    End Sub

    Private Function getDefaultSortExpressionLabel1() As String
        If ViewState("SortExpression") Is Nothing Then
            Return "datum asc"
        Else
            Return String.Format("{0} {1}", ViewState("SortExpression"), ViewState("SortDirection"))
        End If
    End Function

    Private Sub LoadData(ByVal strfilter As String)
        Try
            Dim myList As List(Of Handelingen) = myBalOlympia.GetLesgeverVergoedingbygebruiker(getDefaultSortExpressionLabel1, cbopsteller.SelectedValue, Vergoedingen.Lesgever, strfilter)
            dtgrid.DataSource = myList
            dtgrid.DataBind()
            ResultCount = myList.Count
        Catch ex As Exception
            'UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Sub dtgLabel1_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgrid.ItemCommand
        Try
            Dim i_Result As Integer
            Select Case e.CommandName
                Case "INSERT"
                    Dim myhandeling As New Handelingen
                    Dim txtInsertDatum As TextBox = e.Item.FindControl("txtInsertDatum")
                    Dim drpInsertGroep As DropDownList = e.Item.FindControl("drpInsertGroep")
                    Dim txtInsertInfo As HtmlTextArea = e.Item.FindControl("txtInsertInfo")
                    Dim txtInsertaantal As TextBox = e.Item.FindControl("txtInsertaantal")
                    myhandeling.Groep.Id = drpInsertGroep.SelectedValue
                    myhandeling.Discipline.Id = myBalOlympia.GetDisciplinebyGroep(drpInsertGroep.SelectedValue)
                    myhandeling.Actie.Id = Vergoedingen.Lesgever
                    myhandeling.Datum = txtInsertDatum.Text
                    myhandeling.Info = txtInsertInfo.InnerText
                    myhandeling.Aantal = txtInsertaantal.Text
                    myhandeling.Gebruiker.IdLid = cbopsteller.SelectedValue


                    Try
                        i_Result = myBalOlympia.Inserthandeling(myhandeling)
                        If i_Result > 0 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strAddOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                            Dim mylogging As New Logging
                            mylogging.Gebruiker.IdLid = Session("Gebruiker")
                            mylogging.EventLogging = "Insert Vergoeding Lesgever - " & txtInsertDatum.Text & ": " & drpInsertGroep.Text
                            mylogging.Type = 2
                            myBalOlympia.InsertLogging(mylogging)
                            LoadData(txtfilter.Text)
                        End If
                    Catch ex As Exception

                    End Try
                    dtgrid.ShowFooter = False
                    btnINSERTAdd.Visible = True
                    btnINSERTCancel.Visible = False

                Case "DELETE"
                    Dim myhandeling As New Handelingen
                    myhandeling.Id = dtgrid.DataKeys(e.Item.ItemIndex)
                    Dim txtDatum As Label = e.Item.FindControl("lblDatum")
                    Dim txtInfo As Label = e.Item.FindControl("lblInfo")
                    myhandeling.Datum = txtDatum.Text
                    myhandeling.Info = txtInfo.Text
                    Try
                        i_Result = myBalOlympia.Deletehandeling(myhandeling)
                        If i_Result > 0 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strAddOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                            Dim mylogging As New Logging
                            mylogging.Gebruiker.IdLid = Session("Gebruiker")
                            mylogging.EventLogging = "Delete Vergoeding Lesgever - " & txtDatum.Text & ": " & txtInfo.Text
                            mylogging.Type = 2
                            myBalOlympia.InsertLogging(mylogging)
                            LoadData(txtfilter.Text)
                        End If
                    Catch ex As Exception

                    End Try

                Case "UPDATE"
                    Dim myhandeling As New Handelingen
                    myhandeling.Id = dtgrid.DataKeys(e.Item.ItemIndex)
                    Dim txteditDatum As TextBox = e.Item.FindControl("txteditDatum")
                    Dim drpEditGroep As DropDownList = e.Item.FindControl("drpEditGroep")
                    Dim txteditinfo As HtmlTextArea = e.Item.FindControl("txteditinfo")
                    Dim txteditaantal As TextBox = e.Item.FindControl("txteditaantal")
                    myhandeling.Datum = txteditDatum.Text
                    myhandeling.Groep.Id = drpEditGroep.SelectedValue
                    myhandeling.Discipline.Id = myBalOlympia.GetDisciplinebyGroep(drpEditGroep.SelectedValue)
                    myhandeling.Actie.Id = Vergoedingen.Lesgever
                    myhandeling.Info = txteditinfo.InnerText
                    myhandeling.Aantal = txteditaantal.Text
                    Try
                        i_Result = myBalOlympia.Updatehandeling(myhandeling)
                        If i_Result > 0 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strAddOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                            Dim mylogging As New Logging
                            mylogging.Gebruiker.IdLid = Session("Gebruiker")
                            mylogging.EventLogging = "Update Vergoeding Lesgever - " & txteditDatum.Text & ": " & drpEditGroep.Text
                            mylogging.Type = 2
                            myBalOlympia.InsertLogging(mylogging)
                        End If
                    Catch ex As Exception
                        ' UC_Message.setMessage(strUpdateError, CustomMessage.TypeMessage.Fataal, ex)
                    End Try
                    dtgrid.EditItemIndex = -1
                    dtgrid.ShowFooter = False
                    btnINSERTAdd.Visible = True
                    LoadData(txtfilter.Text)

                Case "CANCEL"
                    dtgrid.EditItemIndex = -1
                    dtgrid.ShowFooter = False
                    LoadData(txtfilter.Text)
                    btnINSERTAdd.Visible = True

                Case "EDIT"
                    dtgrid.EditItemIndex = e.Item.ItemIndex
                    LoadData(txtfilter.Text)
                    btnINSERTAdd.Visible = False
            End Select

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Sub dtgLabel1_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgrid.ItemDataBound
        Try
            Dim myhandeling As Handelingen = e.Item.DataItem()
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lblId As Label = e.Item.FindControl("lblId")
                Dim lblLidID As Label = e.Item.FindControl("lblLidID")
                Dim lblDatum As Label = e.Item.FindControl("lblDatum")
                Dim lblgroep As Label = e.Item.FindControl("lblgroep")
                Dim lblinfo As Label = e.Item.FindControl("lblinfo")
                Dim lblAantal As Label = e.Item.FindControl("lblAantal")
                Dim cbeDelete As AjaxControlToolkit.ConfirmButtonExtender = e.Item.FindControl("cbeDelete")
                cbeDelete.ConfirmText = strDeleteConfirm
                lblId.Text = myhandeling.Id
                lblLidID.Text = myhandeling.Gebruiker.IdLid
                lblDatum.Text = myhandeling.Datum
                lblgroep.Text = myhandeling.Groep.beschrijving
                lblinfo.Text = myhandeling.Info
                lblAantal.Text = myhandeling.Aantal
                If myhandeling.Validate = 0 Then
                    e.Item.BackColor = Drawing.Color.LightPink
                Else
                    e.Item.FindControl("lnkbEdit").Visible = False
                    e.Item.FindControl("btnDelete").Visible = False
                End If
            End If

            If e.Item.ItemType = ListItemType.EditItem Then
                Dim txteditDatum As TextBox = e.Item.FindControl("txteditDatum")
                Dim txteditinfo As HtmlTextArea = e.Item.FindControl("txteditinfo")
                Dim txteditaantal As TextBox = e.Item.FindControl("txteditaantal")
                txteditDatum.Text = myhandeling.Datum
                txteditinfo.InnerText = myhandeling.Info
                txteditaantal.Text = myhandeling.Aantal
                Dim drpEditGroep As DropDownList = e.Item.FindControl("drpEditGroep")
                DoFillUpDropDown(e.Item.FindControl("drpEditGroep"), myBalOlympia.GetTrainingsgroepen("beschrijving").ToArray, "Id", "Beschrijving", myhandeling.Discipline.Id, "")
            End If

            If e.Item.ItemType = ListItemType.Footer Then
                Dim drpInsertGroep As DropDownList = e.Item.FindControl("drpInsertGroep")
                DoFillUpDropDown(e.Item.FindControl("drpInsertGroep"), myBalOlympia.GetTrainingsgroepen("beschrijving").ToArray, "Id", "Beschrijving", "", "")
            End If

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTCancel.Click
        dtgrid.ShowFooter = False
        btnINSERTAdd.Visible = True
        btnINSERTCancel.Visible = False
    End Sub

    Private Sub BtnINSERTAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTAdd.Click
        dtgrid.ShowFooter = True
        btnINSERTAdd.Visible = False
        btnINSERTCancel.Visible = True
    End Sub

    Private Sub Cbopsteller_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbopsteller.SelectedIndexChanged
        LoadData(txtfilter.Text)
    End Sub

    Private Sub BtnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        LoadData(txtfilter.Text)
    End Sub

    Private Sub BtnWisFilter_Click(sender As Object, e As EventArgs) Handles btnWisFilter.Click
        txtfilter.Text = ""
        LoadData(txtfilter.Text)
    End Sub
End Class