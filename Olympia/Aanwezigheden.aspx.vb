Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Class Aanwezigheden
    Inherits Page
    Private myBalOlympia As New Olympia.BALOlympia.BalGebruikers
    Private ResultCount As Integer
    Private strDeleteConfirm, strDeleteError, strDeleteOk, strDBError, strPagingTot, strHeaderTitle, strPagingRecordsFound, strInsertBeschrijving, strUpdateOk, _
        strUpdateError, strPrimaryKeyAllreadyExists, strAddError, strAddOk, strCompleted As String

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        fillUpStringFields()
        Dim idlid As Integer = Session("gebruiker")
        ViewState("ID_Lid") = idlid
        If Not IsPostBack Then
            validateToegang()
            loadDBPicklists()
            setMultiLanguages()
        End If
    End Sub

    Private Sub validateToegang()
        Dim i As Integer
        i = myBalOlympia.CheckToegangGebruiker(Session("gebruiker"), 1)
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing
            Case Rechten_Lid.lezen
                btnINSERTAdd.Visible = False
            Case Else
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("gebruiker")
                mylogging.EventLogging = "Aanwezigheden toegang geweigerd (Domein: 1) "
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

    Private Sub loadDBPicklists()
        'get disciplines
        cbDiscipline.DataSource = myBalOlympia.getDisciplines("Id ASC")
        cbDiscipline.DataTextField = "Beschrijving"
        cbDiscipline.DataValueField = "Id"
        cbDiscipline.DataBind()
        cbDiscipline.Items.Insert(0, New ListItem("", 0)) 'Standaard invoegen op de eerste plaats
    End Sub

    Private Sub cbDiscipline_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbDiscipline.SelectedIndexChanged
        'get groepen
        cbGroep.DataSource = myBalOlympia.getTrainingsGroepenbyDiscipine("Id ASC", cbDiscipline.SelectedValue)
        cbGroep.DataTextField = "Beschrijving"
        cbGroep.DataValueField = "Id"
        cbGroep.DataBind()
        cbGroep.Items.Insert(0, New ListItem("", 0)) 'Standaard invoegen op de eerste plaats
    End Sub

    Private Sub setMultiLanguages()

    End Sub

    Private Sub fillUpStringFields() 'Fill up the strings
        Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"
        lblPageTitle.Text = "Aanwezigheden"
        lblkeuze.Text = "Groep : "
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

    Private Function getDefaultSortExpressionLabel1() As String
        If ViewState("SortExpression") Is Nothing Then
            Return "naam asc"
        Else
            Return String.Format("{0} {1}", ViewState("SortExpression"), ViewState("SortDirection"))
        End If
    End Function

    Private Sub LoadData()
        Try
            Dim myList As List(Of Aanwezigheid) = myBalOlympia.getAanwezighedenbyGroep(getDefaultSortExpressionLabel1, cbGroep.SelectedValue, txtDatum.Text)
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
                    Dim lblIDLid As Label = e.Item.FindControl("lblIDLid")
                    Dim lblinsertinfo As Label = e.Item.FindControl("lblinsertinfo")
                    Dim chkbInsertaanwezig As CheckBox = e.Item.FindControl("chkbInsertaanwezig")
                    Dim myAanwezigheid As Aanwezigheid = e.Item.DataItem()

                    If txtDatum.Text Like "" Then
                        MsgBox("Datum verplicht !")
                        Exit Sub
                    Else
                        myAanwezigheid.Datum = txtDatum.Text
                    End If

                    myAanwezigheid.Gebruiker.IdLid = lblIDLid.Text
                    myAanwezigheid.Groep.Id = cbGroep.SelectedValue
                    myAanwezigheid.Opmerking = lblinsertinfo.Text
                    myAanwezigheid.Aanwezig = chkbInsertaanwezig.Checked
                    Try
                        i_Result = myBalOlympia.InsertAanwezigheid(myAanwezigheid)
                        dtgrid.ShowFooter = False
                        btnINSERTAdd.Visible = True
                        btnINSERTCancel.Visible = False

                        If i_Result >= 1 Then
                            ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strAddOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
                            LoadData()
                        End If
                    Catch ex As Exception
                        If ex.Message = "119" Then
                            ' UC_Message.setMessage(strDuplicate, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        Else
                            '  UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fout, New Exception("VALIDATION"))
                        End If
                    End Try

                Case "DELETE"
                    Dim lblID As Label = e.Item.FindControl("lblID") 'Label
                    Dim myAanwezigheid As New Aanwezigheid With {
                        .Id = lblID.Text
                    }
                    i_Result = myBalOlympia.DeleteAanwezigheid(myAanwezigheid)

                    LoadData()
                    ' UC_MessageChild.setMessage(strDeleteOk, CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))

                Case "CANCEL"
                    dtgrid.ShowFooter = False
                    LoadData()
                    btnINSERTAdd.Visible = True

            End Select

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub
    Private Sub dtgLabel1_EditCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgrid.EditCommand
        dtgrid.EditItemIndex = e.Item.ItemIndex
        LoadData()
        btnINSERTAdd.Visible = False
    End Sub

    Private Sub dtgLabel1_UpdateCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgrid.UpdateCommand
        Dim i_Result As Integer
        Try
            Dim lblid As Label = e.Item.FindControl("lblid")
            Dim txteditopmerking As TextBox = e.Item.FindControl("txteditopmerking")
            Dim chkbEditaanwezig As CheckBox = e.Item.FindControl("chkbEditaanwezig")
            Dim myAanwezigheid As Aanwezigheid = e.Item.DataItem()
            myAanwezigheid.Id = lblid.Text
            myAanwezigheid.Opmerking = txteditopmerking.Text
            myAanwezigheid.Aanwezig = chkbEditaanwezig.Checked
            i_Result = myBalOlympia.UpdateAanwezigheid(myAanwezigheid)
            If i_Result = 1 Then
                ' UC_Message.setMessage(String.Format("{0} ({1} {2} )", strUpdateOk, i_Result, strCompleted), CustomMessage.TypeMessage.Bevestiging, New Exception("VALIDATION"))
            End If
        Catch ex As Exception
            ' UC_Message.setMessage(strUpdateError, CustomMessage.TypeMessage.Fataal, ex)
        End Try

        btnINSERTAdd.Visible = True
        dtgrid.EditItemIndex = -1
        LoadData()
    End Sub

    Private Sub dtgLabel1_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgrid.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                Dim lblID As Label = e.Item.FindControl("lblID")
                Dim lblfunctie As Label = e.Item.FindControl("lblfunctie")
                Dim lbllid As Label = e.Item.FindControl("lbllid")
                Dim lblOpmerking As Label = e.Item.FindControl("lblOpmerking")
                Dim lblaanwezig As CheckBox = e.Item.FindControl("lblaanwezig")
                Dim lnkbEdit As LinkButton = e.Item.FindControl("lnkbEdit")
                Dim btnDelete As LinkButton = e.Item.FindControl("btnDelete")

                Dim myAanwezigheid As Aanwezigheid = e.Item.DataItem()
                lblID.Text = myAanwezigheid.Gebruiker.IdLid
                If myAanwezigheid.Functie = 30 Then
                    lblfunctie.Text = "Gymnast"
                Else
                    lblfunctie.Text = "Trainer"
                End If

                lbllid.Text = myAanwezigheid.Gebruiker.Naam & " " & myAanwezigheid.Gebruiker.Voornaam & " (" & myAanwezigheid.Gebruiker.GebDatum & ")"

                lblaanwezig.Checked = myAanwezigheid.Aanwezig
                lblOpmerking.Text = myAanwezigheid.Opmerking

                If txtDatum.Text Like "" Then
                    lblaanwezig.Visible = False
                    lblOpmerking.Visible = False
                    lnkbEdit.Visible = False
                End If


            End If

            If e.Item.ItemType = ListItemType.EditItem Then
                Dim lblID As Label = e.Item.FindControl("lblID")
                Dim lblfunctie As Label = e.Item.FindControl("lblfunctie")
                Dim lbllid As Label = e.Item.FindControl("lbllid")
                Dim txteditopmerking As Label = e.Item.FindControl("txteditopmerking")
                Dim chkbEditaanwezig As CheckBox = e.Item.FindControl("chkbEditaanwezig")
                Dim myAanwezigheid As Aanwezigheid = e.Item.DataItem()
                lblID.Text = myAanwezigheid.Gebruiker.IdLid
                If myAanwezigheid.Functie = 30 Then
                    lblfunctie.Text = "Gymnast"
                Else
                    lblfunctie.Text = "Trainer"
                End If

                lbllid.Text = myAanwezigheid.Gebruiker.Naam & " " & myAanwezigheid.Gebruiker.Voornaam & " (" & myAanwezigheid.Gebruiker.GebDatum & ")"

                chkbEditaanwezig.Checked = myAanwezigheid.Aanwezig
                txteditopmerking.Text = myAanwezigheid.Opmerking
            End If

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTCancel.Click
        dtgrid.ShowFooter = False
        btnINSERTAdd.Visible = True
        btnINSERTCancel.Visible = False
    End Sub

    Private Sub btnINSERTAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnINSERTAdd.Click
        dtgrid.ShowFooter = True
        btnINSERTAdd.Visible = False
        btnINSERTCancel.Visible = True
    End Sub

    Private Sub dtgLabel1_CancelCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dtgrid.CancelCommand
        dtgrid.EditItemIndex = -1
        LoadData()
    End Sub


    Private Sub cbGroep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbGroep.SelectedIndexChanged
        LoadData()
    End Sub

    Private Sub txtDatum_TextChanged(sender As Object, e As EventArgs) Handles txtDatum.TextChanged
        LoadData()
    End Sub
End Class
