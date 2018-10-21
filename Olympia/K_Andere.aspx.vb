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
            Dim myList As List(Of Handelingen) = myBalOlympia.Gethandelingbygebruiker(getDefaultSortExpressionLabel1, ViewState("ID_Lid"), Vergoedingen.Andere, strfilter)
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
                    myhandeling.Discipline.Id = drpInsertGroep.SelectedValue
                    myhandeling.Groep.Id = 0
                    myhandeling.Actie.Id = Vergoedingen.Andere
                    myhandeling.Datum = txtInsertDatum.Text
                    myhandeling.Info = txtInsertInfo.InnerText
                    myhandeling.Aantal = txtInsertaantal.text
                    myhandeling.Gebruiker.IdLid = ViewState("ID_Lid")

                    Try
                        i_Result = myBalOlympia.Inserthandeling(myhandeling)
                        dtgrid.ShowFooter = False
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
                        .Id = dtgrid.DataKeys(e.Item.ItemIndex)
                    }
                    i_Result = myBalOlympia.Deletehandeling(myhandeling)

                    LoadData(txtfilter.Text)
                    ' UC_MessageChild.setMessage(strDeleteOk, CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))

                Case "UPDATE"
                    Try
                        Dim myhandeling As New Handelingen With {
                            .Id = dtgrid.DataKeys(e.Item.ItemIndex)
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
                    dtgrid.EditItemIndex = -1
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
                lblgroep.Text = myhandeling.Discipline.beschrijving
                lblinfo.Text = myhandeling.Info
                lblAantal.Text = myhandeling.Aantal
            End If

            If e.Item.ItemType = ListItemType.EditItem Then
                Dim txteditDatum As TextBox = e.Item.FindControl("txteditDatum")
                Dim txteditinfo As HtmlTextArea = e.Item.FindControl("txteditinfo")
                Dim txteditaantal As TextBox = e.Item.FindControl("txteditaantal")
                txteditDatum.Text = myhandeling.Datum
                txteditinfo.InnerText = myhandeling.Info
                txteditaantal.Text = myhandeling.Aantal
                Dim drpEditGroep As DropDownList = e.Item.FindControl("drpEditGroep")
                DoFillUpDropDown(e.Item.FindControl("drpEditGroep"), myBalOlympia.getDisciplines("beschrijving").ToArray, "Id", "Beschrijving", myhandeling.Discipline.Id, "")
            End If

            If e.Item.ItemType = ListItemType.Footer Then
                Dim drpInsertGroep As DropDownList = e.Item.FindControl("drpInsertGroep")
                DoFillUpDropDown(e.Item.FindControl("drpInsertGroep"), myBalOlympia.getDisciplines("beschrijving").ToArray, "Id", "Beschrijving", "", "")
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
