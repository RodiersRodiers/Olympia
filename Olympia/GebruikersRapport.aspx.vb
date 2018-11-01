Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Class GebruikersRapport
    Inherits Page
    Private myBalOlympia As New BalGebruikers
    Private ResultCount As Integer
    Private strDeleteConfirm, strDeleteError, strDeleteOk, strDBError, strPagingTot, strHeaderTitle, strPagingRecordsFound, strInsertBeschrijving, strUpdateOk,
        strUpdateError, strPrimaryKeyAllreadyExists, strAddError, strAddOk, strCompleted As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        fillUpStringFields()
        If Not IsPostBack Then
            Dim idlid As Integer = Request.QueryString("ID_lid")
            ViewState("ID_Lid") = idlid
            setMultiLanguages()
            validateToegang()
            LoadData("", txtdatumlaag.Text, txtdatumhoog.Text)
        End If
    End Sub
    Private Sub validateToegang()
        Dim i As Integer
        i = myBalOlympia.CheckToegangGebruiker(Session("Gebruiker"), 2) 'gebruikers
        Select Case i
            Case Rechten_Lid.schrijven
                'do nothing

            Case Rechten_Lid.lezen

            Case Else
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = Session("Gebruiker")
                mylogging.EventLogging = "Gebruikersrapport toegang geweigerd (Domein: 2) "
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
    Private Sub fillUpStringFields() 'Fill up the strings
        lblPageTitle.Text = "Gebruikers > Rapport"
        lblFilter.Text = "Filter"
        btnFilter.Text = "Zoek"
        btnWisFilter.Text = "Wis Filter"
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

    Private Sub setMultiLanguages()
        Dim mygebruiker As Gebruikers = myBalOlympia.GetGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"
        Dim mygebruiker2 As Gebruikers = myBalOlympia.GetGebruiker(ViewState("ID_Lid"))
        lblgebruiker.Text = mygebruiker2.Naam & " " & mygebruiker2.Voornaam & " (" & mygebruiker2.GebDatum & ")"
        txtdatumlaag.Text = Now.AddMonths(-1).ToString("dd/MM/yyyy")
        txtdatumhoog.Text = Now().ToString("dd/MM/yyyy")

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
            Return "datum asc"
        Else
            Return String.Format("{0} {1}", ViewState("SortExpression"), ViewState("SortDirection"))
        End If
    End Function

    Private Sub LoadData(ByVal myfilter As String, ByVal datumlaag As Date, ByVal datumhoog As Date)
        Try
            Dim myList As List(Of Handelingen) = myBalOlympia.GetAllhandelingenbygebruiker(getDefaultSortExpression, ViewState("ID_Lid"), "", datumlaag, datumhoog)
            dtgDataGrid.DataSource = myList
            dtgDataGrid.DataBind()
            ResultCount = myList.Count
        Catch ex As Exception
            'UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub

    Private Sub dtgLabel1_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dtgDataGrid.ItemDataBound
        Try
            Dim myhandeling As Handelingen = e.Item.DataItem()
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lblLidID As Label = e.Item.FindControl("lblLidID")
                Dim lblDatum As Label = e.Item.FindControl("lblDatum")
                Dim lblgroep As Label = e.Item.FindControl("lblgroep")
                Dim lblActie As Label = e.Item.FindControl("lblActie")
                Dim lblinfo As Label = e.Item.FindControl("lblinfo")
                Dim lblAantal As Label = e.Item.FindControl("lblAantal")
                Dim lblBedrag As Label = e.Item.FindControl("lblBedrag")
                lblLidID.Text = myhandeling.Gebruiker.IdLid
                lblDatum.Text = myhandeling.Datum
                lblgroep.Text = myhandeling.Discipline.beschrijving
                lblActie.Text = myhandeling.Actie.beschrijving
                lblinfo.Text = myhandeling.Info
                lblBedrag.Text = myhandeling.Bedrag
                lblAantal.Text = myhandeling.Aantal

            End If

        Catch ex As Exception
            ' UC_Message.setMessage(strAddError, CustomMessage.TypeMessage.Fataal, New Exception("VALIDATION"))
        End Try
    End Sub
    Private Sub BtnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        LoadData("", txtdatumlaag.Text, txtdatumhoog.Text)
    End Sub

    Private Sub BtnWisFilter_Click(sender As Object, e As EventArgs) Handles btnWisFilter.Click
        txtdatumlaag.Text = Now.AddMonths(-1).ToString("dd/MM/yyyy")
        txtdatumhoog.Text = Now().ToString("dd/MM/yyyy")
        LoadData("", txtdatumlaag.Text, txtdatumhoog.Text)
    End Sub
End Class
