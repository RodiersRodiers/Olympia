Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Partial Class Start
    Inherits Page
    Private myBalOlympia As New Olympia.BALOlympia.BalGebruikers
    Private ResultCount As Integer
    Private strHeaderTitle, strDBError, strPagingTot, strPagingRecordsFound As String

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles form1.Load
        setMultiLanguages()
        validateToegang()
    End Sub

    Private Sub validateToegang()
        pagAanwezigheden.Visible = False
        pagGebruikers.Visible = False
        myBtn1.Visible = False
        beheer.Visible = False
        myBtn2.Visible = False
        vergoeding.Visible = False

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

    Private Sub setMultiLanguages()
        Dim mygebruiker As Gebruikers = myBalOlympia.getGebruiker(Session("Gebruiker"))
        lbllogin.Text = "u bent ingelogd als " & mygebruiker.Naam & " " & mygebruiker.Voornaam & " (" & mygebruiker.GebDatum & ")"
        lblPageTitle.Text = "Welkom"
    End Sub

End Class