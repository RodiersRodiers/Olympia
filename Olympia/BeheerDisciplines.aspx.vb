Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Class BeheerDisciplines
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private ResultCount As Integer
    Private strDeleteConfirm, strDeleteError, strDeleteOk, strDBError, strPagingTot, strHeaderTitle, strPagingRecordsFound, strInsertBeschrijving, strUpdateOk, _
        strUpdateError, strPrimaryKeyAllreadyExists, strAddError, strAddOk, strCompleted As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FillUpStringFields()
        If Not IsPostBack Then
            LoadDataLabel1()
            ValidateToegang()
            SetMultiLanguages()
        End If
    End Sub

    Private Sub ValidateToegang()
        Dim i As Integer
        i = myBalOlympia.CheckToegangGebruiker(Session("Gebruiker"), 21)
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing
            Case Rechten_Lid.lezen
                btnINSERTAdd.Visible = False
            Case Else
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Beheer Disciplines toegang geweigerd (Domein: 21) "
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

    Private Sub SetMultiLanguages()

        btnINSERTAdd.Text = "Toevoegen Discipline"
        btnINSERTCancel.Text = "Annuleren"
        btnINSERTAddchild.Text = "Toevoegen Groep"
        btnINSERTCancelchild.Text = "Annuleren"
        lnkbCloseChild.Text = "Close"
    End Sub

    Private Sub FillUpStringFields() 'Fill up the strings
        Dim mygebruiker As Gebruikers = myBalOlympia.GetGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"

        lblPageTitle.Text = "Beheer > Disciplines"
        strHeaderTitle = "Overzicht Disciplines"

        strInsertBeschrijving = "Beschrijving Verplicht "

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


#Region "Disciplines"
    Private Function getDefaultSortExpressionLabel1() As String
        If ViewState("SortExpression") Is Nothing Then
            Return "Beschrijving asc"
        Else
            Return String.Format("{0} {1}", ViewState("SortExpression"), ViewState("SortDirection"))
        End If
    End Function

    Private Sub dtgLabel1_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgLabel1.ItemCommand
        Try
            Dim i_Result As Integer
            Select Case e.CommandName
                Case "INSERT"
                    hideLabel2Table()
                    Dim txtInsertLabel1Beschrijving As TextBox = e.Item.FindControl("txtInsertLabel1Beschrijving")
                    Dim txtInsertLabel1ID As TextBox = e.Item.FindControl("txtInsertLabel1ID")
                    Dim mydiscipline As New pic_Disciplines With {
                        .Id = txtInsertLabel1ID.Text,
                        .beschrijving = txtInsertLabel1Beschrijving.Text
                    }

                    Try
                        i_Result = myBalOlympia.InsertDiscipline(mydiscipline)
                        dtgLabel1.ShowFooter = False
                        btnINSERTAdd.Visible = True
                        btnINSERTCancel.Visible = False

                        If i_Result >= 1 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strAddOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                            LoadDataLabel1()
                        End If
                    Catch ex As Exception
                        If ex.Message = "119" Then
                            ' UC_Message.setMessage(strDuplicate, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        Else
                            '  UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        End If
                    End Try

                Case "DELETE"
                    Dim lblLabel1ID As Label = e.Item.FindControl("lblLabel1ID") 'Label
                    Dim mydiscipline As New pic_Disciplines With {
                        .Id = lblLabel1ID.Text
                    }
                    i_Result = myBalOlympia.DeleteDiscipline(mydiscipline)

                    LoadDataLabel1()
                    ' UC_MessageChild.setMessage(strDeleteOk, CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                    hideLabel2Table()

                Case "DETAIL"
                    Dim lblLabel1ID As Label = e.Item.FindControl("lblLabel1ID")
                    Dim lblLabel1Beschrijving As LinkButton = e.Item.FindControl("lblLabel1Beschrijving")
                    ViewState("IdDiscipline") = lblLabel1ID.Text
                    lblSUBSUB.Text = "Discipline : " & lblLabel1Beschrijving.Text
                    lblIDDisc.Text = lblLabel1ID.Text
                    LoadDataLabel2(lblLabel1ID.Text)
                    showLabel2Table()
                Case "CANCEL"
                    dtgLabel1.ShowFooter = False
                    LoadDataLabel1()
                    btnINSERTAdd.Visible = True

            End Select

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub
    Private Sub dtgLabel1_EditCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgLabel1.EditCommand
        dtgLabel1.EditItemIndex = e.Item.ItemIndex
        LoadDataLabel1()
        hideLabel2Table()
        btnINSERTAdd.Visible = False
    End Sub

    Private Sub dtgLabel1_UpdateCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgLabel1.UpdateCommand
        Dim i_Result As Integer
        Try
            Dim lblEditLabel1ID As Label = e.Item.FindControl("lblEditLabel1ID")
            Dim lblEditFeitencatID As Label = e.Item.FindControl("lblEditFeitencatID")
            Dim txtEditLabel1Beschrijving As TextBox = e.Item.FindControl("txtEditLabel1Beschrijving")
            Dim myDiscipline As New pic_Disciplines With {
                .Id = lblEditLabel1ID.Text,
                .beschrijving = txtEditLabel1Beschrijving.Text
            }
            i_Result = myBalOlympia.UpdateDiscipline(myDiscipline)
            If i_Result = 1 Then
                ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strUpdateOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
            End If
        Catch ex As Exception
            ' UC_Message.setMessage(strUpdateError, CustomMessage.TypeMessage.Fataal, ex)
        End Try

        btnINSERTAdd.Visible = True
        dtgLabel1.EditItemIndex = -1
        hideLabel2Table()
        LoadDataLabel1()
    End Sub

    Private Sub dtgLabel1_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgLabel1.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lblLabel1Beschrijving As LinkButton = e.Item.FindControl("lblLabel1Beschrijving")
                Dim lblLabel1ID As Label = e.Item.FindControl("lblLabel1ID")
                Dim cbeDelete As AjaxControlToolkit.ConfirmButtonExtender = e.Item.FindControl("cbeDelete")
                cbeDelete.ConfirmText = strDeleteConfirm

                Dim myDiscipline As pic_Disciplines = e.Item.DataItem()
                lblLabel1Beschrijving.Text = myDiscipline.beschrijving
                lblLabel1ID.Text = myDiscipline.Id
            End If

            If e.Item.ItemType = ListItemType.EditItem Then
                Dim lblEditLabel1ID As Label = e.Item.FindControl("lblEditLabel1ID")
                Dim txtEditLabel1Beschrijving As TextBox = e.Item.FindControl("txtEditLabel1Beschrijving")
                Dim myDiscipline As pic_Disciplines = e.Item.DataItem()
                lblEditLabel1ID.Text = myDiscipline.Id
                txtEditLabel1Beschrijving.Text = myDiscipline.beschrijving
            End If

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTCancel.Click
        dtgLabel1.ShowFooter = False
        btnINSERTAdd.Visible = True
        btnINSERTCancel.Visible = False
    End Sub

    Private Sub btnINSERTAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTAdd.Click
        dtgLabel1.ShowFooter = True
        btnINSERTAdd.Visible = False
        btnINSERTCancel.Visible = True
        hideLabel2Table()
        btnINSERTAddchild.Visible = False
    End Sub

    Private Sub LoadDataLabel1()
        Try
            Dim myList As List(Of pic_Disciplines) = myBalOlympia.getDisciplines(getDefaultSortExpressionLabel1)
            dtgLabel1.DataSource = myList
            dtgLabel1.DataBind()
            ResultCount = myList.Count
        Catch ex As Exception
            'UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Sub dtgLabel1_CancelCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgLabel1.CancelCommand
        dtgLabel1.EditItemIndex = -1
        LoadDataLabel1()
    End Sub
#End Region

#Region "Groepen"

    Private Sub LoadDataLabel2(ByVal iddisc As Integer)
        Dim myList As List(Of pic_Trainingsgroepen)
        myList = myBalOlympia.GetTrainingsGroepenbyDiscipine("beschrijving asc", iddisc)
        dtgLabel2.DataSource = myList
        dtgLabel2.DataBind()
    End Sub

    Private Sub dtgLabel2_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgLabel2.ItemCommand
        Try
            Dim i_Result As Integer
            ViewState("IdDiscipline") = lblIDDisc.Text
            Select Case e.CommandName
                Case "INSERT"
                    Dim txtInsertLabel2Beschrijving As TextBox = e.Item.FindControl("txtInsertLabel2Beschrijving")

                    Dim myTrainingsgroep As New pic_Trainingsgroepen
                    myTrainingsgroep.Discipline.Id = lblIDDisc.Text
                    myTrainingsgroep.beschrijving = txtInsertLabel2Beschrijving.Text

                    dtgLabel2.ShowFooter = False
                    btnINSERTAddchild.Visible = True
                    btnINSERTCancelchild.Visible = False

                    Try
                        i_Result = myBalOlympia.InsertTrainingsgroep(myTrainingsgroep)
                        If i_Result >= 1 Then
                            ' UC_MessageChild.setMessage(String.Format("{0} ({1} {2} )", strAddOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                            LoadDataLabel2(lblIDDisc.Text)
                        End If
                    Catch ex As Exception
                    End Try



                Case "DELETE"
                    Dim myTrainingsgroep As New pic_Trainingsgroepen
                    Dim mylblEditLabel2ID As Label = e.Item.FindControl("lblLabel2ID")
                    myTrainingsgroep.Id = mylblEditLabel2ID.Text
                    i_Result = myBalOlympia.DeleteTrainingsgroep(myTrainingsgroep)

                    LoadDataLabel2(lblIDDisc.Text)
                    ' UC_MessageChild.setMessage(strDeleteOk, CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))

                Case "CANCEL"
                    dtgLabel2.EditItemIndex = -1
                    Dim iddisc As Integer = 0
                    LoadDataLabel2(iddisc)
                    dtgLabel2.ShowFooter = False
                    btnINSERTAdd.Visible = True
                    btnINSERTAddchild.Visible = True
                    btnINSERTCancelchild.Visible = False

            End Select
        Catch ex As Exception
            ' UC_MessageChild.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, ex)
        End Try
    End Sub

    Private Sub dtgLabel2_EditCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgLabel2.EditCommand
        dtgLabel2.EditItemIndex = e.Item.ItemIndex
        LoadDataLabel2(ViewState("IdDiscipline"))
        btnINSERTAddchild.Visible = False
    End Sub

    Private Sub dtgLabel2_UpdateCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgLabel2.UpdateCommand
        Dim i_Result As Integer
        Try
            Dim lblEditLabel2ID As Label = e.Item.FindControl("lblEditLabel2ID")
            Dim txtEditLabel2Beschrijving As TextBox = e.Item.FindControl("txtEditLabel2Beschrijving")
            Dim myTrainingsgroep As New pic_Trainingsgroepen With {
                .Id = lblEditLabel2ID.Text
            }
            myTrainingsgroep.Discipline.Id = ViewState("IdDiscipline")
            myTrainingsgroep.beschrijving = txtEditLabel2Beschrijving.Text
            i_Result = myBalOlympia.UpdateTrainingsgroep(myTrainingsgroep)
            If i_Result = 1 Then
                ' UC_MessageChild.setMessage(String.Format("{0} ({1} {2} )", strUpdateOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
            End If
        Catch ex As Exception
            ' UC_MessageChild.setMessage(strUpdateError, CustomMessage.TypeMessage.Fataal, ex)
        End Try

        btnINSERTAddchild.Visible = True
        dtgLabel2.EditItemIndex = -1
        LoadDataLabel2(ViewState("IdDiscipline"))
    End Sub

    Private Sub btnINSERTAdddtgLabel2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTAddchild.Click
        dtgLabel2.ShowFooter = True
        btnINSERTAddchild.Visible = False
        btnINSERTCancelchild.Visible = True
        btnINSERTAdd.Visible = False
    End Sub
    Private Sub dtgLabel2_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgLabel2.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lblLabel2ID As Label = e.Item.FindControl("lblLabel2ID")
                Dim lblLabel2Beschrijving As Label = e.Item.FindControl("lblLabel2Beschrijving")
                Dim myTrainingsgroep As pic_Trainingsgroepen = e.Item.DataItem()
                lblLabel2Beschrijving.Text = myTrainingsgroep.beschrijving
                lblLabel2ID.Text = myTrainingsgroep.Id

            End If

            If e.Item.ItemType = ListItemType.EditItem Then
                Dim lblEditLabel2ID As Label = e.Item.FindControl("lblEditLabel2ID")
                Dim txtEditLabel2Beschrijving As TextBox = e.Item.FindControl("txtEditLabel2Beschrijving")
                Dim myTrainingsgroep As pic_Trainingsgroepen = e.Item.DataItem()
                lblEditLabel2ID.Text = myTrainingsgroep.Id
                txtEditLabel2Beschrijving.Text = myTrainingsgroep.beschrijving
            End If

        Catch ex As Exception
            Response.Write(" " & ex.StackTrace)
        End Try
    End Sub
    Private Sub hideLabel2Table()
        childTable.Visible = False
        dtgLabel2.SelectedIndex = -1
        btnINSERTAdd.Visible = True
        btnINSERTAddchild.Visible = False
        Dim script As String = "<script>  $(document).ready(function(){ $('#childTable').hide();});</script>"
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "Entity", script, False)
        dtgLabel2.EditItemIndex = -1
    End Sub
    Private Sub btnCloseLabel2_Click(sender As Object, e As EventArgs) Handles lnkbCloseChild.Click
        hideLabel2Table()
    End Sub
    Private Sub showLabel2Table()
        childTable.Visible = True
        dtgLabel2.SelectedIndex = -1
        btnINSERTAddchild.Visible = True
        btnINSERTAdd.Visible = False
        lnkbCloseChild.Visible = True
        Dim script As String = "<script>  $(document).ready(function(){ $('#childTable').show();});</script>"
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "Entity", script, False)
    End Sub
#End Region

#Region "DATAGRID - Sorting" ' To move to original class

    Private Sub dtgLabel1_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgLabel1.SortCommand
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
        UpdateColumnHeaders(dtgLabel1)
        LoadDataLabel1()
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

    Private Sub dtgLabel2_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgLabel2.SortCommand
        If ViewState("SortExpressionChild") = String.Empty Then
            ViewState("SortExpressionChild") = e.SortExpression
            ViewState("SortDirectionChild") = "ASC"
        Else
            If ViewState("SortExpressionChild") <> e.SortExpression Then
                ViewState("SortExpressionChild") = e.SortExpression
                ViewState("SortDirectionChild") = "ASC"
            Else
                ' Clicked on same column, toggle sort order
                If ViewState("SortDirectionChild") = "ASC" Then
                    ViewState("SortDirectionChild") = "DESC"
                Else
                    ViewState("SortDirectionChild") = "ASC"
                End If
            End If
        End If
        UpdateColumnHeaders(dtgLabel2)
        LoadDataLabel2(ViewState("IdDiscipline"))
    End Sub


#End Region

End Class
