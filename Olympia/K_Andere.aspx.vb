Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Class K_Andere
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private ResultCount As Integer
    Private strDeleteConfirm, strDeleteError, strDeleteOk, strDBError, strPagingTot, strHeaderTitle, strPagingRecordsFound, strInsertBeschrijving, strUpdateOk, _
        strUpdateError, strPrimaryKeyAllreadyExists, strAddError, strAddOk, strCompleted As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        fillUpStringFields()

        If Not IsPostBack Then
            Dim idlid As Integer = Session("gebruiker")
            ViewState("ID_Lid") = idlid
            loadDBPicklists()
            validateToegang()
            LoadData(txtfilter.Text)
        End If
    End Sub

    Private Sub validateToegang()
        Dim i As Integer
        i = myBalOlympia.CheckToegangGebruiker(Session("Gebruiker"), 14)
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing
            Case Rechten_Lid.lezen
                btnINSERTAdd.Visible = False
            Case Else
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Vergoedingen Andere toegang geweigerd (Domein: 14) "
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

    Private Sub fillUpStringFields() 'Fill up the strings
        Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"

        lblPageTitle.Text = "Andere Onkosten"
        lblFilter.Text = "Filter"
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

    Private Sub loadDBPicklists()
        Try
            'Trainers
            cbopsteller.DataSource = myBalOlympia.GetGebruikers("naam ASC", "")
            cbopsteller.DataTextField = "Naam"
            cbopsteller.DataValueField = "IDLid"
            cbopsteller.DataBind()

        Catch ex As Exception
            'UC_Message.setMessage(strDbError, CustomMessage.TypeMessage.Fataal, ex)
        End Try
    End Sub

    Private Sub LoadData(ByVal strfilter As String)
        Try
            Dim myList As List(Of Handelingen) = myBalOlympia.GetHandelingAnderebygebruiker(getDefaultSortExpression, ViewState("ID_Lid"), strfilter)
            dtgDataGrid.DataSource = myList
            dtgDataGrid.DataBind()
            ResultCount = myList.Count
            doShowPage()
        Catch ex As Exception
            'UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Sub dtgDataGrid_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgDataGrid.ItemCommand
        Try
            Dim i_Result As Integer
            Select Case e.CommandName
                Case "INSERT"
                    Dim myhandeling As New Handelingen
                    Dim txtInsertDatum As TextBox = e.Item.FindControl("txtInsertDatum")
                    Dim drpInsertGroep As DropDownList = e.Item.FindControl("drpInsertGroep")
                    Dim txtInsertInfo As HtmlTextArea = e.Item.FindControl("txtInsertInfo")
                    Dim txtInsertaantal As TextBox = e.Item.FindControl("txtInsertaantal")
                    myhandeling.Discipline.Id = drpInsertGroep.SelectedValue
                    myhandeling.Groep.Id = 0
                    myhandeling.Actie.Id = Vergoedingen.Andere
                    myhandeling.Datum = txtInsertDatum.Text
                    myhandeling.Info = txtInsertInfo.InnerText
                    myhandeling.Aantal = txtInsertaantal.Text
                    myhandeling.Gebruiker.IdLid = ViewState("ID_Lid")

                    Try
                        i_Result = myBalOlympia.Inserthandeling(myhandeling)
                        dtgDataGrid.ShowFooter = False
                        btnINSERTAdd.Visible = True
                        btnINSERTCancel.Visible = False

                        If i_Result >= 1 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strAddOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                            LoadData(txtfilter.Text)
                        End If
                    Catch ex As Exception
                        If ex.Message = "119" Then
                            ' UC_Message.setMessage(strDuplicate, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        Else
                            '  UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        End If
                    End Try

                Case "DELETE"
                    Dim myhandeling As New Handelingen With {
                        .Id = dtgDataGrid.DataKeys(e.Item.ItemIndex)
                    }
                    i_Result = myBalOlympia.Deletehandeling(myhandeling)

                    LoadData(txtfilter.Text)
                    ' UC_MessageChild.setMessage(strDeleteOk, CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))

                Case "UPDATE"
                    Try
                        Dim myhandeling As New Handelingen With {
                            .Id = dtgDataGrid.DataKeys(e.Item.ItemIndex)
                        }
                        Dim txteditDatum As TextBox = e.Item.FindControl("txteditDatum")
                        Dim drpEditGroep As DropDownList = e.Item.FindControl("drpEditGroep")
                        Dim txteditinfo As HtmlTextArea = e.Item.FindControl("txteditinfo")
                        Dim txteditaantal As TextBox = e.Item.FindControl("txteditaantal")

                        myhandeling.Datum = txteditDatum.Text
                        myhandeling.Discipline.Id = drpEditGroep.SelectedValue
                        myhandeling.Groep.Id = 0
                        myhandeling.Actie.Id = Vergoedingen.Andere
                        myhandeling.Info = txteditinfo.InnerText
                        myhandeling.Aantal = txteditaantal.Text
                        i_Result = myBalOlympia.Updatehandeling(myhandeling)
                        If i_Result = 1 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strUpdateOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                        End If
                    Catch ex As Exception
                        ' UC_Message.setMessage(strUpdateError, CustomMessage.TypeMessage.Fataal, ex)
                    End Try

                    btnINSERTAdd.Visible = True
                    dtgDataGrid.EditItemIndex = -1
                    LoadData(txtfilter.Text)

                Case "CANCEL"
                    dtgDataGrid.EditItemIndex = -1
                    dtgDataGrid.ShowFooter = False
                    LoadData(txtfilter.Text)
                    btnINSERTAdd.Visible = True

                Case "EDIT"
                    dtgDataGrid.EditItemIndex = e.Item.ItemIndex
                    LoadData(txtfilter.Text)
                    btnINSERTAdd.Visible = False

            End Select

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Sub dtgDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgDataGrid.ItemDataBound
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
                lblgroep.Text = myhandeling.Discipline.beschrijving
                lblinfo.Text = myhandeling.Info
                lblAantal.Text = myhandeling.Aantal
                If myhandeling.Validate = False Then
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
                DoFillUpDropDown(e.Item.FindControl("drpEditGroep"), myBalOlympia.GetDisciplines("beschrijving").ToArray, "Id", "Beschrijving", myhandeling.Discipline.Id, "")
            End If

            If e.Item.ItemType = ListItemType.Footer Then
                Dim drpInsertGroep As DropDownList = e.Item.FindControl("drpInsertGroep")
                DoFillUpDropDown(e.Item.FindControl("drpInsertGroep"), myBalOlympia.GetDisciplines("beschrijving").ToArray, "Id", "Beschrijving", "", "")
            End If

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
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

    Private Function getDefaultSortExpression() As String
        If ViewState("SortExpression") Is Nothing Then
            Return "datum desc"
        Else
            Return String.Format("{0} {1}", ViewState("SortExpression"), ViewState("SortDirection"))
        End If
    End Function
    Private Sub setRowsDataGridItemVisible(ByVal blnVisible As Boolean)
        For Each myDatagridItem As DataGridItem In dtgDataGrid.Items
            If myDatagridItem.ItemType = ListItemType.Item Or myDatagridItem.ItemType = ListItemType.AlternatingItem Then
                myDatagridItem.Visible = blnVisible
            End If
        Next
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTCancel.Click
        dtgDataGrid.ShowFooter = False
        btnINSERTAdd.Visible = True
        btnINSERTCancel.Visible = False
        setRowsDataGridItemVisible(True)
    End Sub

    Private Sub BtnINSERTAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTAdd.Click
        dtgDataGrid.ShowFooter = True
        btnINSERTAdd.Visible = False
        btnINSERTCancel.Visible = True
        setRowsDataGridItemVisible(False)
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

#Region "DATAGRID - Sorting" ' To move to original class

    Private Sub dtgDataGrid_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgDataGrid.SortCommand
        If ViewState("SortExpression") = String.Empty Then
            ViewState("SortExpression") = e.SortExpression
            ViewState("SortDirection") = "ASC"
        Else
            If ViewState("SortExpression") <> e.SortExpression Then
                ViewState("SortExpression") = e.SortExpression
                ViewState("SortDirection") = "ASC"
            Else
                ' Clicked on same column, toggle sort order
                If ViewState("SortDirection") = "ASC" Then
                    ViewState("SortDirection") = "DESC"
                Else
                    ViewState("SortDirection") = "ASC"
                End If
            End If
        End If
        UpdateColumnHeaders(dtgDataGrid)
        LoadData(txtfilter.Text)
    End Sub

    Sub UpdateColumnHeaders(ByVal dg As DataGrid)
        Dim c As DataGridColumn

        For Each c In dg.Columns
            'Clear any <img> tags that might be present
            c.HeaderText = Regex.Replace(c.HeaderText, "\s<.*>", String.Empty)
            c.HeaderStyle.CssClass = "datagridHeaderNonSort"
            Dim strSortExpr As String = ViewState("SortExpression")
            Dim strSortDir As String = ViewState("SortDirection")

            If c.SortExpression = strSortExpr Then
                c.HeaderStyle.CssClass = "datagridHeaderSort"
                If strSortDir = "ASC" Then
                    c.HeaderText &= " <img src=""../../images/up.gif"" border=""0"">"
                Else
                    c.HeaderText &= " <img src=""../../images/down.gif"" border=""0"">"
                End If
            End If
        Next
    End Sub
#End Region

#Region "DATAGRID - Paging" ' To move to original class

    Private Sub dtgDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgDataGrid.PageIndexChanged
        dtgDataGrid.CurrentPageIndex = e.NewPageIndex
        LoadData(txtfilter.Text)
        doShowPage()
    End Sub

    Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (dtgDataGrid.CurrentPageIndex < (dtgDataGrid.PageCount - 1)) Then
                    dtgDataGrid.CurrentPageIndex += 1
                End If
            Case "prev" 'The prev button was clicked
                If (dtgDataGrid.CurrentPageIndex > 0) Then
                    dtgDataGrid.CurrentPageIndex -= 1
                End If
            Case "last" 'The Last Page button was clicked
                dtgDataGrid.CurrentPageIndex = (dtgDataGrid.PageCount - 1)
            Case "refresh"  'reload the page
                'do nothing
            Case Else 'The First Page button was clicked
                dtgDataGrid.CurrentPageIndex = Convert.ToInt32(arg)
        End Select
        LoadData("")
    End Sub

    Sub Prev_Buttons()
        Dim PrevSet As String
        If dtgDataGrid.CurrentPageIndex + 1 <> 1 And ResultCount <> -1 Then
            PrevSet = dtgDataGrid.PageSize
            If dtgDataGrid.CurrentPageIndex + 1 = dtgDataGrid.PageCount Then
            End If
        End If
    End Sub

    Sub Next_Buttons()
        Dim NextSet As String
        If dtgDataGrid.CurrentPageIndex + 1 < dtgDataGrid.PageCount Then
            NextSet = dtgDataGrid.PageSize
        End If
        If dtgDataGrid.CurrentPageIndex + 1 = dtgDataGrid.PageCount - 1 Then
            Dim EndCount As Integer
            EndCount = ResultCount - (dtgDataGrid.PageSize * (dtgDataGrid.CurrentPageIndex + 1))
        End If
    End Sub

    Private Sub doShowPage()
        If ResultCount > 0 Then
            txtCurrentPage.Text = dtgDataGrid.CurrentPageIndex + 1
            Dim i_start As Integer = dtgDataGrid.CurrentPageIndex * dtgDataGrid.PageSize
            If i_start = 0 Then
                lblExtraPaging.Text = " / " & dtgDataGrid.PageCount & " (" & ResultCount & " records )"
            Else
                lblExtraPaging.Text = " / " & dtgDataGrid.PageCount & " (" & ResultCount & " records )"
            End If
            txtCurrentPage.Visible = True
            lblExtraPaging.Visible = True
        Else
            txtCurrentPage.Visible = False
            lblExtraPaging.Visible = False
        End If
    End Sub

    Private Sub txtCurrentPage_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCurrentPage.TextChanged
        If IsNumeric(txtCurrentPage.Text) Then
            If txtCurrentPage.Text <= dtgDataGrid.PageCount And txtCurrentPage.Text > 0 Then
                dtgDataGrid.CurrentPageIndex = txtCurrentPage.Text - 1
                LoadData(txtfilter.Text)
                doShowPage()
            Else
                If txtCurrentPage.Text = 0 Then
                    txtCurrentPage.Text = 1
                Else
                    txtCurrentPage.Text = dtgDataGrid.CurrentPageIndex + 1
                End If
            End If
        Else
            txtCurrentPage.Text = dtgDataGrid.CurrentPageIndex + 1
        End If
    End Sub

#End Region
End Class
