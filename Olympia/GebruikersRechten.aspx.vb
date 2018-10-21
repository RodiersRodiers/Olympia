Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Public Class GebruikersRechten
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private ResultCount As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        fillUpStringFields()
        If Not IsPostBack Then
            Dim idlid As Integer = Request.QueryString("ID_lid")
            ViewState("ID_Lid") = idlid
            setMultiLanguages()
            LoadData(idlid)
        End If
    End Sub
    Private Sub fillUpStringFields()

    End Sub
    Private Sub setMultiLanguages()
        Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"
        Dim mygebruiker2 As Gebruikers = myBalOlympia.getGebruiker(ViewState("ID_Lid"))
        lblgebruiker.Text = mygebruiker2.Naam & " " & mygebruiker2.Voornaam & " (" & mygebruiker2.GebDatum & ")"
        lblPageTitle.Text = "Gebruikers > Beheer Rechten"
        btnINSERTAdd.Text = "Toevoegen"
        btnINSERTCancel.Text = "Annuleren"
    End Sub

    Private Sub LoadData(ByVal idlid As Integer)
        Try
            Dim myList As New List(Of Rechten)
            myList = myBalOlympia.getRechtenGebruiker(getDefaultSortExpressionLabel1, idlid)

            dtgLabel1.DataSource = myList
            dtgLabel1.DataBind()
            ResultCount = myList.Count
        Catch ex As Exception
            'UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Function getDefaultSortExpressionLabel1() As String
        If ViewState("SortExpression") Is Nothing Then
            Return "ID_Groep asc"
        Else
            Return String.Format("{0} {1}", ViewState("SortExpression"), ViewState("SortDirection"))
        End If
    End Function

    Private Sub dtgLabel1_ItemCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles dtgLabel1.ItemCommand
        Try
            Dim i_Result As Integer
            Select Case e.CommandName
                Case "INSERT"
                    Dim myrecht As New Rechten
                    Dim drpInsertGroep As DropDownList = e.Item.FindControl("drpInsertGroep")
                    Dim drpInsertActie As DropDownList = e.Item.FindControl("drpInsertActie")
                    Dim drpInsertToelating As DropDownList = e.Item.FindControl("drpInsertToelating")

                    myrecht.Validate = drpInsertToelating.SelectedItem.Value
                    myrecht.Groep.Id = drpInsertGroep.SelectedItem.Value
                    myrecht.Actie.Id = drpInsertActie.SelectedItem.Value
                    myrecht.Gebruiker.IdLid = ViewState("ID_Lid")

                    Try
                        i_Result = myBalOlympia.InsertRechtenGebruiker(myrecht)
                        dtgLabel1.ShowFooter = False
                        btnINSERTAdd.Visible = True
                        btnINSERTCancel.Visible = False
                        LoadData(ViewState("ID_Lid"))
                        If i_Result >= 1 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strAddOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))

                        End If
                    Catch ex As Exception
                        If ex.Message = "119" Then
                            ' UC_Message.setMessage(strDuplicate, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        Else
                            '  UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        End If
                    End Try

                Case "UPDATE"
                    Try
                        Dim myrecht As New Rechten With {
                            .Id = dtgLabel1.DataKeys(e.Item.ItemIndex)
                        }
                        Dim drpEditGroep As DropDownList = e.Item.FindControl("drpEditGroep")
                        Dim drpEditActie As DropDownList = e.Item.FindControl("drpEditActie")
                        Dim drpEditToelating As DropDownList = e.Item.FindControl("drpEditToelating")

                        myrecht.Validate = drpEditToelating.SelectedValue
                        myrecht.Groep.Id = drpEditGroep.SelectedValue
                        myrecht.Actie.Id = drpEditActie.SelectedValue

                        i_Result = myBalOlympia.UpdateRechtenGebruiker(myrecht)
                        If i_Result = 1 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strUpdateOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                        End If
                    Catch ex As Exception
                        ' UC_Message.setMessage(strUpdateError, CustomMessage.TypeMessage.Fataal, ex)
                    End Try

                    btnINSERTAdd.Visible = True
                    dtgLabel1.EditItemIndex = -1
                    LoadData(ViewState("ID_Lid"))

                Case "DELETE"
                    Dim myrecht As New Rechten With {
                        .Id = dtgLabel1.DataKeys(e.Item.ItemIndex)
                    }
                    i_Result = myBalOlympia.DeleteRechtenGebruiker(myrecht)
                    LoadData(ViewState("ID_Lid"))
                    ' UC_MessageChild.setMessage(strDeleteOk, CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))

                Case "CANCEL"
                    dtgLabel1.EditItemIndex = -1
                    dtgLabel1.ShowFooter = False
                    LoadData(ViewState("ID_Lid"))
                    btnINSERTAdd.Visible = True

                Case "EDIT"
                    dtgLabel1.EditItemIndex = e.Item.ItemIndex
                    LoadData(ViewState("ID_Lid"))
                    btnINSERTAdd.Visible = False
            End Select

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Sub dtgLabel1_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgLabel1.ItemDataBound

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lblId As Label = e.Item.FindControl("lblId")
            Dim lblLidID As Label = e.Item.FindControl("lblLidID")
            Dim lblactie As Label = e.Item.FindControl("lblactie")
            Dim lblgroep As Label = e.Item.FindControl("lblgroep")
            Dim lblToelating As Label = e.Item.FindControl("lblToelating")
            Dim cbeDelete As AjaxControlToolkit.ConfirmButtonExtender = e.Item.FindControl("cbeDelete")
            cbeDelete.ConfirmText = "Bent u zeker dat u deze record wilt wissen ?"
            Dim myrecht As Rechten = e.Item.DataItem
            lblId.Text = myrecht.Id
            lblLidID.Text = myrecht.Gebruiker.IdLid
            lblactie.Text = myrecht.Actie.beschrijving
            lblgroep.Text = myrecht.Groep.beschrijving

            Select Case myrecht.Validate
                Case 1
                    lblToelating.Text = "Schrijven"
                    lblToelating.CssClass = "RCCUOK"
                Case 2
                    lblToelating.Text = "Enkel Lezen"
                    lblToelating.CssClass = "RCCUDONE"
                Case 3
                    lblToelating.Text = "Geen Toegang"
                    lblToelating.CssClass = "CTIIncomplete"
            End Select
        End If

        If e.Item.ItemType = ListItemType.EditItem Then
            Dim drpEditGroep As DropDownList = e.Item.FindControl("drpEditGroep")
            Dim drpEditActie As DropDownList = e.Item.FindControl("drpEditActie")
            Dim drpEditToelating As DropDownList = e.Item.FindControl("drpEditToelating")
            Dim myrecht As Rechten = e.Item.DataItem
            DoFillUpDropDown(e.Item.FindControl("drpEditGroep"), myBalOlympia.getAllTrainingsgroepen("beschrijving").ToArray, "Id", "Beschrijving", myrecht.Groep.Id, "")
            DoFillUpDropDown(e.Item.FindControl("drpEditActie"), myBalOlympia.getActies("beschrijving").ToArray, "Id", "Beschrijving", myrecht.Actie.Id, "")

            drpEditToelating.Items.Clear()
            drpEditToelating.Items.Add(New ListItem("Schrijven", 1))
            drpEditToelating.Items.Add(New ListItem("Enkel Lezen", 2))
            drpEditToelating.Items.Add(New ListItem("Geen Toegang", 3))
            drpEditToelating.SelectedItem.Value = myrecht.Validate

        End If

        If e.Item.ItemType = ListItemType.Footer Then
            Dim drpInsertGroep As DropDownList = e.Item.FindControl("drpInsertGroep")
            Dim drpInsertActie As DropDownList = e.Item.FindControl("drpInsertActie")
            Dim drpInsertToelating As DropDownList = e.Item.FindControl("drpInsertToelating")
            DoFillUpDropDown(e.Item.FindControl("drpInsertGroep"), myBalOlympia.getAllTrainingsgroepen("beschrijving").ToArray, "Id", "Beschrijving", "", "")
            DoFillUpDropDown(e.Item.FindControl("drpInsertActie"), myBalOlympia.getActies("beschrijving").ToArray, "Id", "Beschrijving", "", "")

            drpInsertToelating.Items.Clear()
            drpInsertToelating.Items.Add(New ListItem("Schrijven", 1))
            drpInsertToelating.Items.Add(New ListItem("Enkel Lezen", 2))
            drpInsertToelating.Items.Add(New ListItem("Geen Toegang", 3))

        End If

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

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTCancel.Click
        dtgLabel1.ShowFooter = False
        btnINSERTAdd.Visible = True
        btnINSERTCancel.Visible = False
    End Sub

    Private Sub btnINSERTAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTAdd.Click
        dtgLabel1.ShowFooter = True
        btnINSERTAdd.Visible = False
        btnINSERTCancel.Visible = True
    End Sub

End Class