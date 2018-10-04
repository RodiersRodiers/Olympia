Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Public Class RegisterOlympia
    Inherits Page
    Private myBalOlympia As New Olympia.BALOlympia.BalGebruikers

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        setMultiLanguages()

    End Sub
    Private Sub setMultiLanguages()
        lblEmail.Text = "Email"
        lblNaam.Text = "Naam"
        lblVoornaam.Text = "Voornaam"
        lblPaswoord.Text = "Paswoord"
        lblPaswoordConfirm.Text = "Herhaal Paswoord"
        btn_Annuleer.Text = "Annuleer"
        btn_Registreer.Text = "Registreer"
        lnkbprivacy.Text = "Privacy Policy"
    End Sub

    Private Sub btn_Annuleer_Click(sender As Object, e As EventArgs) Handles btn_Annuleer.Click
        Response.Redirect("Login.aspx", False)
    End Sub

    Private Sub btn_Registreer_Click(sender As Object, e As EventArgs) Handles btn_Registreer.Click
        Dim mygebruiker As New Gebruikers

        If txtNaam.Text = "" Then
            MsgBox("Naam verplicht !")
            Return
        Else
            mygebruiker.Naam = txtNaam.Text
        End If

        If txtVoornaam.Text = "" Then
            MsgBox("Voornaam verplicht !")
            Return
        Else
            mygebruiker.Voornaam = txtVoornaam.Text
        End If

        If txtEmail.Text = "" Then
            MsgBox("Email verplicht !")
            Return
        Else
            Dim stremail As New String("")
            stremail = txtEmail.Text
            If stremail.IndexOf("@") > -1 Then
                If (stremail.IndexOf(".", stremail.IndexOf("@")) > stremail.IndexOf("@")) AndAlso stremail.Split(".").Length > 0 AndAlso stremail.Split(".")(1) <> "" Then
                    mygebruiker.Email = stremail
                Else
                    MsgBox("Email niet geldig !")
                    Return
                End If
            Else
                MsgBox("Email niet geldig !")
                Return
            End If

        End If

        If txtPaswoord.Text = txtPaswoordConfirm.Text Then
            If txtPaswoord.Text.Length > 7 Then
                Dim StrPwd As New String("")
                StrPwd = txtPaswoord.Text

                Dim x As Integer
                Dim bCapital As Boolean = False
                Dim bLower As Boolean = False
                Dim bNumberic As Boolean = False
                Dim iAscii As Integer

                For x = 1 To Len(StrPwd)
                    iAscii = Asc(Mid(StrPwd, x, 1))
                    If Not bCapital Then 'hoofdletter
                        bCapital = CBool(iAscii >= 65 And iAscii <= 90)
                    End If
                    If Not bLower Then 'kleine letter
                        bLower = CBool(iAscii >= 97 And iAscii <= 122)
                    End If
                    If Not bNumberic Then 'cijfers
                        bNumberic = CBool(iAscii >= 48 And iAscii <= 57)
                    End If
                Next x

                If bCapital And bLower And bNumberic Then
                    mygebruiker.Paswoord = txtPaswoord.Text
                Else
                    MsgBox("Paswoord moet minstens 8 tekens met minstens 1 hoofdletter en 1 cijfer bevatten !")
                    Return
                End If
            Else
                MsgBox("Paswoord moet minstens 8 tekens met minstens 1 hoofdletter en 1 cijfer bevatten !")
                Return
            End If

        Else
            MsgBox("Paswoorden komen niet overeen !")
            Return
        End If

            Dim i As New Integer
            Dim ii As New Integer
            i = myBalOlympia.InsertGebruiker(mygebruiker)
            If i = 1 Then
                ii = myBalOlympia.getAuthGebruiker(mygebruiker)
                Dim mylogging As New Logging
            mylogging.Gebruiker.IdLid = ii
                mylogging.EventLogging = "Registratie"
                mylogging.Type = 1

            myBalOlympia.InsertLogging(mylogging)
            Response.Redirect("Login.aspx", False)
            End If
    End Sub

    Private Sub lnkbprivacy_Click(sender As Object, e As EventArgs) Handles lnkbprivacy.Click
        Response.Redirect("Privacy.aspx", False)
    End Sub

End Class