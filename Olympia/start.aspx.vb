Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Class Start
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private ResultCount As Integer
    Private strHeaderTitle, strDBError, strPagingTot, strPagingRecordsFound As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim idlid As Integer = Request.QueryString("ID_lid")
            ViewState("ID_Lid") = idlid
            setMultiLanguages()
            validateToegang()
            MyInitPage()
            LoadData(getDefaultSortExpression)
        End If

    End Sub

    Private Sub validateToegang()
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
    End Sub

    Private Sub setMultiLanguages()
        Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"
        lblPageTitle.Text = "Welkom"
        lnkNew.Text = "Nieuw Bericht"
    End Sub

    Private Sub LoadData(ByVal getDefaultSortExpression As String)
        Dim myBoodschappen As List(Of Boodschappen) = myBalOlympia.GetBoodschappenByLid(Session("Gebruiker"), getDefaultSortExpression)
        dtgDataGrid.DataSource = myBoodschappen
        dtgDataGrid.DataBind()
        ResultCount = dtgDataGrid.Items.Count
        doShowPage()
    End Sub

    Private Function getDefaultSortExpression() As String
        If ViewState("SortExpression") Is Nothing Then
            Return "datum desc"
        Else
            Return String.Format("{0} {1}", ViewState("SortExpression"), ViewState("SortDirection"))
        End If
    End Function

    Private Sub MyInitPage()
        lnkNew.Attributes.Add("class", "clickMeButton thickbox")
        lnkNew.Attributes.Add("href", "BoodschapCompose.aspx?KeepThis=true&TB_iframe=true&height=500&width=800")
        lnkNew.Attributes.Add("title", "Versturen boodschap")
    End Sub

    Private Sub DtgDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgDataGrid.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lblDatumUur As Label = e.Item.FindControl("lblDatumUur")
                Dim lblAfzender As Label = e.Item.FindControl("lblAfzender")
                Dim lblOnderwerp As Label = e.Item.FindControl("lblOnderwerp")
                Dim lnkOpen As LinkButton = e.Item.FindControl("lnkOpen")
                Dim lblId As Label = e.Item.FindControl("lblId")
                Dim myBoodschap As Boodschappen = e.Item.DataItem()

                lblDatumUur.Text = Format(myBoodschap.Datum, "dd/MM/yyyy HH:mm")
                lblId.Text = myBoodschap.Id
                lblAfzender.Text = myBoodschap.Zender.Naam + " " + myBoodschap.Zender.Voornaam
                lblOnderwerp.Text = myBoodschap.Inhoud

                lnkOpen.Attributes.Add("class", "activation thickbox")
                lnkOpen.Attributes.Add("href", "BoodschapReply.aspx?ID=" & myBoodschap.Id & "&KeepThis=true&TB_iframe=true&height=500&width=800")
                lnkOpen.Attributes.Add("title", " ")

                If myBoodschap.gelezen = BoodschapStatus.Ongelezen Then
                    e.Item.CssClass = "redDate"
                End If
            End If
        Catch ex As Exception
            Response.Write(" " & ex.StackTrace)
        End Try
    End Sub

    Private Sub DtgDataGrid_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgDataGrid.ItemCommand
        Select Case e.CommandName
            Case "OPEN"
                Dim lblId As Label = e.Item.FindControl("lblId")
                Dim myMsg As Boodschappen = myBalOlympia.getBoodschapById(lblId.Text)
                myMsg.gelezen = BoodschapStatus.Gelezen
                myBalOlympia.UpdateBoodschap(myMsg)

            Case "DELETE"
                Dim lblId As Label = e.Item.FindControl("lblId")
                myBalOlympia.DeleteBoodschap(lblId.Text)

            Case "STATUSONGELEZEN"
                Dim lblId As Label = e.Item.FindControl("lblId")
                Dim myMsg As Boodschappen = myBalOlympia.getBoodschapById(lblId.Text)
                If myMsg.gelezen = BoodschapStatus.Ongelezen Then
                    myMsg.gelezen = BoodschapStatus.Gelezen
                End If
                If myMsg.gelezen = BoodschapStatus.Gelezen Then
                    myMsg.gelezen = BoodschapStatus.Ongelezen
                End If

                myBalOlympia.UpdateBoodschap(myMsg)

                'reload header
                Dim script As String = "<script>$(document).ready(function(){ parent.location.reload(1);});</script>"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "Entity", script, False)
        End Select
        LoadData(getDefaultSortExpression)
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
        LoadData(getDefaultSortExpression)
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
        LoadData(getDefaultSortExpression)
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
                LoadData(getDefaultSortExpression)
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