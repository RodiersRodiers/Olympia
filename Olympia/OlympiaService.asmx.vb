
Imports System.Web.Services
Imports AjaxControlToolkit
Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia
Imports System.ComponentModel

'To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<Script.Services.ScriptService()>
<WebService(Namespace:="turnkringolympia.be")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class OlympiaService
    Inherits WebService

    Private myBalGebruiker As New BalGebruikers


    <WebMethod()>
    Public Function GetUsersSuggest(ByVal prefixText As String, ByVal count As String) As List(Of String)
        Dim myBalGebruiker As New BalGebruikers
        Dim myUserList As List(Of Gebruikers) = myBalGebruiker.GetGebruikers(prefixText, "")

        Dim myList As New List(Of String)
        For Each myUser As Gebruikers In myUserList
            myList.Add(AutoCompleteExtender.CreateAutoCompleteItem(myUser.Naam & " " & myUser.Voornaam, myUser.IdLid))
        Next
        Return myList.ToList

    End Function

End Class

