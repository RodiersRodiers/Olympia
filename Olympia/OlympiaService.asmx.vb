﻿
Imports System.Web.Services
Imports System.ComponentModel
Imports AjaxControlToolkit
Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia

Public Class OlympiaService
    Inherits WebService

    <System.Web.Script.Services.ScriptMethod(), _
   System.Web.Services.WebMethod()> _
    Public Shared Function GetUsersSuggest(ByVal prefixText As String, ByVal count As String) As List(Of String)
        Dim myBalGebruiker As New BalGebruikers
        Dim myUserList As List(Of Gebruikers) = myBalGebruiker.getGebruikers(prefixText, "")

        Dim myList As New List(Of String)
        For Each myUser As Gebruikers In myUserList
            myList.Add(AutoCompleteExtender.CreateAutoCompleteItem(myUser.Naam & " " & myUser.Voornaam, myUser.IdLid))
        Next
        Return myList.ToList

    End Function

End Class
