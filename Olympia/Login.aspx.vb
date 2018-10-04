﻿Imports Olympia.OBJOlympia
Imports System.Data.Odbc
Public Class LoginOlympia
    Inherits Page
    Private myBalOlympia As New BALOlympia.BalGebruikers

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        setMultiLanguages()
    End Sub

    Private Sub setMultiLanguages()

        lblGebruiker.Text = "Gebruiker"
        lblPassword.Text = "Paswoord"
        btn_Login.Text = "Login"
        lbtn_Registreer.Text = "Registreer"
        lnkbprivacy.Text = "Privacy Policy"
    End Sub


    Private Sub btn_Login_Click(sender As Object, e As EventArgs) Handles btn_Login.Click
        Dim mygebruiker As New Gebruikers With {
            .Email = txtGebruiker.Text.Trim,
            .Paswoord = txtPassword.Text.Trim
        }

        Dim ii As Integer = myBalOlympia.checkemail(mygebruiker)
        If ii > 0 Then
            mygebruiker.IdLid = ii
            Dim i As Integer = myBalOlympia.GetAuthGebruiker(mygebruiker)
            If i > 0 Then
                Dim mylogging As New Logging
                mylogging.Gebruiker.IdLid = i
                mylogging.EventLogging = "Login"
                mylogging.Type = 1

                Session("gebruiker") = i
                myBalOlympia.InsertLogging(mylogging)
                Response.Redirect("start.aspx", False)
            Else
                MsgBox("Verkeerd paswoord !")
                Return
            End If
        Else
            MsgBox("U bent nog niet gekend, registreer u eerst !")
            Return
        End If


    End Sub

    Private Sub lbtn_Registreer_Click(sender As Object, e As EventArgs) Handles lbtn_Registreer.Click
        Response.Redirect("Register.aspx", False)
    End Sub

    Private Sub lnkbprivacy_Click(sender As Object, e As EventArgs) Handles lnkbprivacy.Click
        Response.Redirect("Privacy.aspx", False)
    End Sub
End Class