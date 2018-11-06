Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Public Class BeheerGebruikers
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private ResultCount As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        setMultiLanguages()
        If Not IsPostBack Then
            validateToegang()
            LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
        End If

    End Sub

    Private Sub validateToegang()
        Dim i As Integer
        i = myBalOlympia.CheckToegangGebruiker(Session("Gebruiker"), 2)
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing
            Case Rechten_Lid.lezen
                ' btnINSERTAdd.Visible = False
            Case Else
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Beheer Gebruikers toegang geweigerd (Domein: 2) "
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
    End Sub
    Private Sub setMultiLanguages()
        lblPageTitle.Text = "Beheer > Gebruikers"

        Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"
        lblOpen.Text = "Openstaand"
        lblFilter.Text = "Filter"
        lblGebruiker.Text = "Gebruiker"
        btnFilter.Text = "Zoek"
        btnWisFilter.Text = "Wis Filter"
        btn_nieuw.Text = "Nieuwe gebruiker aanmaken"
        imgbFirstbutton.ToolTip = "Eerste record"
        imgbPrevbutton.ToolTip = "Vorige record"
        imgbNextbutton.ToolTip = "Volgende record"
        imgbLastbutton.ToolTip = "Laatste record"
        imgbFirstbutton.Attributes.Add("onMouseOver", "this.src='../images/firstRecord_over.gif';")
        imgbFirstbutton.Attributes.Add("onMouseLeave", "this.src='../images/firstRecord.gif';")
        imgbPrevbutton.Attributes.Add("onMouseOver", "this.src='../images/previousRecord_over.gif';")
        imgbPrevbutton.Attributes.Add("onMouseLeave", "this.src='../images/previousRecord.gif';")
        imgbNextbutton.Attributes.Add("onMouseOver", "this.src='../images/nextRecord_over.gif';")
        imgbNextbutton.Attributes.Add("onMouseLeave", "this.src='../images/nextRecord.gif';")
        imgbLastbutton.Attributes.Add("onMouseOver", "this.src='../images/lastRecord_over.gif';")
        imgbLastbutton.Attributes.Add("onMouseLeave", "this.src='../images/lastRecord.gif';")
    End Sub
    Private Function getDefaultSortExpression() As String
        If ViewState("SortExpression") Is Nothing Then
            If Not Session("sortDossierOverzicht") Is Nothing Then
                Return Session("sortDossierOverzicht")
            Else
                Return "Naam DESC"
            End If
        Else
            Session("sortDossierOverzicht") = ViewState("SortExpression") & " " & ViewState("SortDirection")
            Return ViewState("SortExpression") & " " & ViewState("SortDirection")
        End If
    End Function

    Private Sub LoadData(ByVal sort As String, ByVal filter As String, ByVal intOpen As Integer)
        Try
            Dim mygebruikersList As New List(Of Gebruikers)
            mygebruikersList = myBalOlympia.GetGebruikersOpenHandeling(sort, filter, intOpen)
            dtgDataGrid.DataSource = mygebruikersList
            dtgDataGrid.DataBind()
            ResultCount = mygebruikersList.Count
            lblresult.Text = ResultCount & " Gebruikers gevonden"
            doShowPage()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub dtgDataGrid_CancelCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgDataGrid.CancelCommand
        dtgDataGrid.EditItemIndex = -1
        LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
    End Sub

    Private Sub dtgDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgDataGrid.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lblId As Label = e.Item.FindControl("lblId")
                Dim lblNaam As Label = e.Item.FindControl("lblNaam")
                Dim lblVoornaam As Label = e.Item.FindControl("lblVoornaam")
                Dim lblEmail As Label = e.Item.FindControl("lblEmail")
                Dim lblGSM As Label = e.Item.FindControl("lblGSM")
                Dim mygebruikersOverzicht As Gebruikers = e.Item.DataItem
                Dim lnkOpen As LinkButton = e.Item.FindControl("lnkOpen")

                If mygebruikersOverzicht.Validate = 0 Then
                    e.Item.BackColor = Drawing.Color.Pink
                End If

                lnkOpen.CommandArgument = mygebruikersOverzicht.IdLid
                lblId.Text = mygebruikersOverzicht.IdLid
                lblNaam.Text = mygebruikersOverzicht.Naam
                lblVoornaam.Text = mygebruikersOverzicht.Voornaam
                lblEmail.Text = mygebruikersOverzicht.Email
                lblGSM.Text = mygebruikersOverzicht.GSM

            End If

        Catch ex As Exception
            Response.Write(" " & ex.StackTrace)
        End Try
    End Sub

    Private Sub dtgDataGrid_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles dtgDataGrid.ItemCommand
        Select Case e.CommandName

            Case "OPEN"
                Dim lblId As Label = e.Item.FindControl("lblId")
                Response.Redirect("GebruikerDetail.aspx?ID_lid=" & lblId.Text)
            Case "Delete"
                Dim lblId As Label = e.Item.FindControl("lblId")
                myBalOlympia.DeleteGebruiker(lblId.Text)
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Gebruiker " & lblId.Text & "verwijderd !"
                mylogging.Type = 1
                myBalOlympia.InsertLogging(mylogging)

        End Select

        LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
    End Sub

    Private Sub btn_nieuw_Click(sender As Object, e As EventArgs) Handles btn_nieuw.Click
        Response.Redirect("GebruikerDetail.aspx?ID_lid=0")
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
    End Sub

    Private Sub btnWisFilter_Click(sender As Object, e As EventArgs) Handles btnWisFilter.Click
        txtfilter.Text = ""
        LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
    End Sub

    Private Sub ckbxOpen_CheckedChanged(sender As Object, e As EventArgs) Handles ckbxOpen.CheckedChanged
        LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
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
        LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
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
        LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
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
        LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
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
                LoadData(getDefaultSortExpression, txtfilter.Text, ckbxOpen.Checked)
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