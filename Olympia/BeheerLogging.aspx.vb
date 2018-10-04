Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Class BeheerLogging
    Inherits Page
    Private myBalOlympia As New Olympia.BALOlympia.BalGebruikers
    Private ResultCount As Integer
    Private strHeaderTitle, strDBError, strPagingTot, strPagingRecordsFound As String

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles form1.Load
        fillUpStringFields()
        If Not IsPostBack Then
            validateToegang()
            setDefaultDates()
            FillUpLogTypes()
            setMultiLanguages()
        End If
    End Sub

    Private Sub validateToegang()
        Dim i As Integer
        i = myBalOlympia.CheckToegangGebruiker(Session("Gebruiker"), 22)
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing
            Case Rechten_Lid.lezen
                'btnINSERTAdd.Visible = False
            Case Else
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Beheer Logging toegang geweigerd (Domein: 22) "
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

    Private Sub setDefaultDates()
        calExtStart.SelectedDate = Date.Now.AddMonths(-2)
        calExtEnd.SelectedDate = Date.Now.AddDays(1)
    End Sub

    Private Sub FillUpLogTypes()
        cbTypes.Items.Clear()
        cbTypes.Items.Add(New ListItem("Loggin", LogType.Loggin))
        cbTypes.Items.Add(New ListItem("Registratie", LogType.Registratie))
        cbTypes.Items.Add(New ListItem("Beheer", LogType.Beheer))
        cbTypes.Items.Add(New ListItem("AutoProcedures", LogType.AutoProcedures))
    End Sub

    Private Function getDefaultSortExpression() As String
        If ViewState("SortExpression") Is Nothing Then
            Return "Datum DESC"
        Else
            Return String.Format("{0} {1}", ViewState("SortExpression"), ViewState("SortDirection"))
        End If
    End Function

    Private Sub fillUpStringFields() 'Fill up the strings
        Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"

        lblPageTitle.Text = "BEHEER > logging"
        strHeaderTitle = "Overzicht logging"

        dtgDataGrid.Columns(0).HeaderText = "Datum"
        dtgDataGrid.Columns(1).HeaderText = "Gebruiker"
        dtgDataGrid.Columns(2).HeaderText = "Event"
        dtgDataGrid.Columns(3).HeaderText = "Type"

        lblPeriode.Text = "Periode"
        lblPeriodeTot.Text = "tot"
        lblzoektype.Text = "Type"

        strDBError = "Fout in het ophalen van de gegevens"
    End Sub

    Private Sub setMultiLanguages() ' Fill up the object according to the language

        btnzoek.Text = "Zoek"
        btnwisfilter.Text = "wis filter"
        btnShowRapport.Text = "Rapport"
        btnShowDatagrid.Text = "Overzicht"

        txtStart.Attributes.Add("readonly", "true")
        txtEnd.Attributes.Add("readonly", "true")
        lblInfo.Text = "Info"

        imgbFirstbutton.ToolTip = "Eerste record"
        imgbPrevbutton.ToolTip = "Vorige record"
        imgbNextbutton.ToolTip = "Volgende record"
        imgbLastbutton.ToolTip = "Laatste record"
        imgbFirstbutton.Attributes.Add("onMouseOver", "this.src='../../images/firstRecord_over.gif';")
        imgbFirstbutton.Attributes.Add("onMouseLeave", "this.src='../../images/firstRecord.gif';")
        imgbPrevbutton.Attributes.Add("onMouseOver", "this.src='../../images/previousRecord_over.gif';")
        imgbPrevbutton.Attributes.Add("onMouseLeave", "this.src='../../images/previousRecord.gif';")
        imgbNextbutton.Attributes.Add("onMouseOver", "this.src='../../images/nextRecord_over.gif';")
        imgbNextbutton.Attributes.Add("onMouseLeave", "this.src='../../images/nextRecord.gif';")
        imgbLastbutton.Attributes.Add("onMouseOver", "this.src='../../images/lastRecord_over.gif';")
        imgbLastbutton.Attributes.Add("onMouseLeave", "this.src='../../images/lastRecord.gif';")
    End Sub

    Private Function LoadData(ByVal getDefaultSortExpression As String)
        Dim myList As New List(Of Logging)
        Try
            myList = myBalOlympia.getLogging(getDefaultSortExpression, calExtStart.SelectedDate.Value, calExtEnd.SelectedDate.Value, cbTypes.SelectedItem.Value, txtInfo.Text)
        Catch ex As Exception
            UC_Message.setMessage(strDBError, CustomMessage.TypeMessage.Fataal, ex)
        End Try
        Return myList
    End Function

    Private Sub dtgDataGrid_CancelCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgDataGrid.CancelCommand
        dtgDataGrid.EditItemIndex = -1
        btnShowRapport.Visible = True
        LoadData(getDefaultSortExpression)
    End Sub

    Private Sub txtEnd_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEnd.TextChanged
        calExtEnd.SelectedDate = txtEnd.Text
    End Sub

    Private Sub txtStart_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtStart.TextChanged
        calExtStart.SelectedDate = txtStart.Text
    End Sub

    Private Sub setRowsDataGridItemVisible(ByVal blnVisible As Boolean)
        For Each myDatagridItem As DataGridItem In dtgDataGrid.Items
            If myDatagridItem.ItemType = ListItemType.Item Or myDatagridItem.ItemType = ListItemType.AlternatingItem Then
                myDatagridItem.Visible = blnVisible
            End If
        Next
    End Sub

    Private Sub dtgDataGrid_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgDataGrid.ItemCommand
        Try
            Dim i_Result As Integer
            If i_Result >= 1 Then
                UC_Message.setMessage(String.Format("Bewerking succesvol uitgevoerd ({0} record(s) bijgewerkt)", i_Result), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
            End If

            dtgDataGrid.ShowFooter = False
            btnShowRapport.Visible = True
            setRowsDataGridItemVisible(True)
            LoadData(getDefaultSortExpression)
        Catch ex As Exception
            UC_Message.setMessage("Er is een fout opgetreden tijdens de bewerking", CustomMessage.TypeMessage.Fataal, ex)
        End Try
    End Sub

    Private Sub dtgDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgDataGrid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim myDatum As Label = e.Item.FindControl("lblDatum")
            Dim myGebruiker As Label = e.Item.FindControl("lblGebruiker")
            Dim myEvent As Label = e.Item.FindControl("lblEvent")
            Dim myType As Label = e.Item.FindControl("lblType")

            Dim myLogging As Logging = e.Item.DataItem

            Dim dt As Date = myLogging.Datum

            myDatum.Text = dt.ToString("dd/MM/yyyy") & " " & dt.ToString("HH:mm:ss")
            If myLogging.Gebruiker.Naam = "" Then
                myGebruiker.Text = "System"
            Else
                myGebruiker.Text = myLogging.Gebruiker.Naam & " " & myLogging.Gebruiker.Voornaam
            End If

            myEvent.Text = myLogging.EventLogging.Replace(vbCrLf, "").Replace("$§$", "<BR>")

            Dim myLogType As LogType = myLogging.Type
            myType.Text = myLogType.ToString

        End If
    End Sub

    Private Sub LoadDatagrid()
        dtgDataGrid.Visible = True
        btnShowRapport.Visible = True
        btnShowDatagrid.Visible = False
        tblFooter.Visible = True

        Dim myList As List(Of Logging) = LoadData(getDefaultSortExpression)

        Try
            dtgDataGrid.DataSource = myList
            dtgDataGrid.DataBind()
        Catch
            dtgDataGrid.CurrentPageIndex = 0
            dtgDataGrid.DataSource = myList
            dtgDataGrid.DataBind()
        End Try

        ResultCount = myList.Count
        doShowPage()
    End Sub

    Private Sub btnShowDatagrid_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowDatagrid.Click
        LoadDatagrid()
    End Sub

    Protected Sub btnShowRapport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowRapport.Click
        loadReport()
    End Sub

    Private Sub loadReport()
        'ReportViewer1.LocalReport.ReportPath = "app_protected/Beheer/rptLogging.rdlc"
        'ReportViewer1.Visible = True
        'dtgDataGrid.Visible = False
        'btnShowRapport.Visible = False
        'btnShowDatagrid.Visible = True
        'tblFooter.Visible = False
        'tblFooter.Visible = False

        'Try
        '    'set parameters
        '    Dim rpt_parameters As List(Of ReportParameter) = New List(Of ReportParameter)
        '    rpt_parameters.Add(New ReportParameter("HeaderTitle", strHeaderTitle))
        '    ReportViewer1.LocalReport.SetParameters(rpt_parameters)
        '    Dim myGebruiker As Gebruikers = Session("Gebruiker")
        '    rpt_parameters.Add(New ReportParameter("GebruikersNaam", myGebruiker.Naam))
        '    ReportViewer1.LocalReport.SetParameters(rpt_parameters)

        '    Dim dataSource As ReportDataSource
        '    Dim myList As New List(Of Logging)
        '    myList = myBalAdmin.getLoggingRapport(calExtStart.SelectedDate.Value, calExtEnd.SelectedDate.Value, cbTypes.SelectedItem.Value, txtNaam.Text, txtInfo.Text)
        '    dataSource = New ReportDataSource("Loggingrapport", myList)
        '    ReportViewer1.LocalReport.DataSources.Clear()
        '    ReportViewer1.LocalReport.DataSources.Add(dataSource)
        '    ReportViewer1.LocalReport.Refresh()
        'Catch ex As Exception
        '    UC_Message.setMessage(strDBError, CustomMessage.TypeMessage.Fataal, ex)
        'End Try
    End Sub

    Private Sub btnzoek_Click(sender As Object, e As EventArgs) Handles btnzoek.Click
       
            LoadDatagrid()

    End Sub

    Private Sub btnwisfilter_Click(sender As Object, e As EventArgs) Handles btnwisfilter.Click
        txtInfo.Text = ""
        cbTypes.SelectedIndex = 0

        LoadDatagrid()

    End Sub

    Private Sub cbTypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbTypes.SelectedIndexChanged
        LoadDatagrid()
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
        LoadDatagrid()
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
        LoadDatagrid()
        doShowPage()
    End Sub

    Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        'used by external paging UI
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

        'Now, bind the data!
        LoadDatagrid()

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
                lblExtraPaging.Text = " / " & dtgDataGrid.PageCount & " " & "&nbsp;&nbsp;&nbsp;&nbsp;[" & 1 & " " & strPagingTot & " " & (i_start + dtgDataGrid.Items.Count) & "] - " & ResultCount & " " & strPagingRecordsFound & "&nbsp;"
            Else
                lblExtraPaging.Text = " / " & dtgDataGrid.PageCount & " " & "&nbsp;&nbsp;&nbsp;&nbsp;[" & i_start & " " & strPagingTot & " " & (i_start + dtgDataGrid.Items.Count) & "] - " & ResultCount & " " & strPagingRecordsFound & "&nbsp;"
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
                LoadDatagrid()
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
