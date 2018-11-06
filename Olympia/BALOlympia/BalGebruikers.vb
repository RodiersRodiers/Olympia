Imports Olympia.OBJOlympia
Imports Olympia.DALOlympia

Namespace BALOlympia

    Public Class BalGebruikers
        Private myDalGebruikers As New DalGebruikers

        Public Shared Function CheckStringNull(ByVal obj As Object, Optional ByVal def As String = "") As String
            If (obj Is Nothing) Or (obj Is DBNull.Value) Then Return def
            Return CStr(obj).Trim()
        End Function

        Public Shared Function CheckIntNull(ByVal obj As Object) As Object
            If (obj.Equals(DBNull.Value)) Then Return Nothing
            Return obj
        End Function

        Public Function IsEmptyDate(ByVal dt As Date) As Boolean
            If dt = New Date(1900, 1, 1) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function GetEmptyDate() As Date
            Dim myDate As New Date(1900, 1, 1)
            Return myDate
        End Function
        Public Function Changepw(ByVal mygebruiker As Gebruikers) As Integer
            Try
                Return myDalGebruikers.Changepw(mygebruiker)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

        Public Function GetAuthGebruiker(ByVal mygebruiker As Gebruikers) As Integer
            Try
                Return myDalGebruikers.GetAuthGebruiker(mygebruiker)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

        Public Function Checkemail(ByVal mygebruiker As Gebruikers) As Integer
            Try
                Return myDalGebruikers.checkemail(mygebruiker)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

        Public Function ReadFileExcell(ByVal strPath As String) As List(Of Gebruikers)
            Dim myList As New List(Of Gebruikers)
            Try

                For Each myRow As DataRow In myDalGebruikers.readFileExcell(strPath).Rows
                    Dim mygebruiker As New Gebruikers
                    ' Import Naam
                    If myRow.ItemArray.Count >= 1 Then
                        If myRow(2).ToString = "" Then
                            mygebruiker.Naam = ""
                        Else
                            mygebruiker.Naam = myRow(2).ToString
                        End If
                    End If

                    ' Import Voornaam
                    If myRow.ItemArray.Count >= 2 Then
                        If myRow(3).ToString = "" Then
                            mygebruiker.Voornaam = ""
                        Else
                            mygebruiker.Voornaam = myRow(3).ToString
                        End If
                    End If

                    ' Import Geb Datum
                    If myRow.ItemArray.Count >= 2 Then
                        If myRow(4).ToString = "" Then
                            mygebruiker.GebDatum = ""
                        Else
                            mygebruiker.GebDatum = myRow(4)
                        End If
                    End If

                    ' Import Geslacht
                    If myRow.ItemArray.Count >= 2 Then
                        If myRow(5).ToString = "" Then
                            mygebruiker.Geslacht = ""
                        Else
                            mygebruiker.Geslacht = myRow(5).ToString
                        End If
                    End If

                    ' Import email
                    If myRow.ItemArray.Count >= 3 Then
                        If myRow(6).ToString = "" Then
                            mygebruiker.Email = ""
                        Else
                            mygebruiker.Email = myRow(6).ToString
                        End If
                    End If

                    ' Import gsm
                    If myRow.ItemArray.Count >= 4 Then
                        If myRow(7).ToString = "" Then
                            mygebruiker.GSM = ""
                        Else
                            mygebruiker.GSM = myRow(7).ToString
                        End If
                    End If

                    ' Import RekNr
                    If myRow.ItemArray.Count >= 5 Then
                        If myRow(8).ToString = "" Then
                            mygebruiker.Rekeningnummer = ""
                        Else
                            mygebruiker.Rekeningnummer = myRow(8).ToString
                        End If
                    End If

                    ' Import gemeente
                    If myRow.ItemArray.Count >= 6 Then
                        If myRow(13).ToString = "" Then
                            mygebruiker.Gemeente = ""
                        Else
                            mygebruiker.Gemeente = myRow(13).ToString
                        End If
                    End If


                    ' Required fields
                    If mygebruiker.Naam <> "" Then
                        ' Only Naam is required.
                        myList.Add(mygebruiker)
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally

            End Try
            Return myList
        End Function

#Region "Logging"

        Public Function InsertLogging(ByVal myLogging As Logging) As Integer
            Try
                Return myDalGebruikers.InsertLogging(myLogging)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

        Public Function GetLogging(ByVal sort As String, ByVal startDate As Date, ByVal endDate As Date, ByVal intType As String, ByVal txtInfo As String) As List(Of Logging)
            Dim myList As New List(Of Logging)

            Try
                Dim dt As DataTable = myDalGebruikers.getLogging(sort, startDate, endDate, intType, txtInfo)
                For Each myRow As DataRow In dt.Rows
                    Dim myLogging As New Logging
                    myLogging.Gebruiker.Naam = myRow("Naam").ToString
                    myLogging.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myLogging.Datum = myRow("datum")
                    myLogging.EventLogging = myRow("Event").ToString
                    myLogging.Type = myRow("Type").ToString
                    myList.Add(myLogging)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

#End Region

#Region "Gebruikers"

        Public Function GetGebruiker(ByVal IDLid As Integer) As Gebruikers
            Dim mygebruiker As New Gebruikers
            Try
                Dim dt As DataTable = myDalGebruikers.getGebruiker(IDLid)
                For Each myRow As DataRow In dt.Rows
                    mygebruiker.Naam = myRow("Naam").ToString
                    mygebruiker.Voornaam = myRow("Voornaam").ToString
                    mygebruiker.Email = myRow("Email").ToString
                    mygebruiker.GSM = myRow("GSM").ToString
                    mygebruiker.Paswoord = myRow("Password").ToString
                    mygebruiker.Rekeningnummer = myRow("Rekeningnr").ToString
                    mygebruiker.Geslacht = myRow("Geslacht").ToString
                    mygebruiker.Gemeente = myRow("Gemeente").ToString
                    mygebruiker.Postcode = myRow("Postcode").ToString
                    mygebruiker.Straat = myRow("Straat").ToString
                    mygebruiker.Huisnr = myRow("huisnr").ToString
                    mygebruiker.Info = myRow("Info").ToString
                    mygebruiker.GebDatum = myRow("GebDatum")
                    mygebruiker.IdLid = myRow("ID_Lid")
                Next
            Catch ex As Exception
                Throw
            End Try
            Return mygebruiker
        End Function

        Public Function GetTrainers(ByVal sort As String) As List(Of Gebruikers)
            Dim myList As New List(Of Gebruikers)
            Try
                Dim dt As DataTable = myDalGebruikers.getTrainers(sort)
                For Each myRow As DataRow In dt.Rows
                    Dim mygebruiker As New Gebruikers With {
                        .Naam = myRow("Naam").ToString,
                        .Voornaam = myRow("Voornaam").ToString,
                        .VolledigeNaam = myRow("Naam").ToString & " " & myRow("Voornaam").ToString,
                        .IdLid = myRow("ID_Lid")
                    }
                    myList.Add(mygebruiker)
                Next
            Catch ex As Exception
                Throw
            End Try
            Return myList
        End Function

        Public Function GetGebruikers(ByVal sort As String, ByVal filter As String) As List(Of Gebruikers)
            Dim myList As New List(Of Gebruikers)
            Try
                Dim dt As DataTable = myDalGebruikers.getGebruikers(sort, filter)
                For Each myRow As DataRow In dt.Rows
                    Dim mygebruiker As New Gebruikers With {
                        .Naam = myRow("Naam").ToString,
                        .Voornaam = myRow("Voornaam").ToString,
                        .Email = myRow("Email").ToString,
                        .GSM = myRow("GSM").ToString,
                        .Paswoord = myRow("Password").ToString,
                        .IdLid = myRow("ID_Lid")
                    }
                    myList.Add(mygebruiker)
                Next
            Catch ex As Exception
                Throw
            End Try
            Return myList
        End Function
        Public Function InsertGebruiker(ByVal mygebruiker As Gebruikers) As Integer
            Try
                Return myDalGebruikers.InsertGebruiker(mygebruiker)
            Catch ex As Exception
                Throw
            Finally
            End Try
        End Function

        Public Function UpdateGebruiker(ByVal mygebruiker As Gebruikers) As Integer
            Try
                Return myDalGebruikers.UpdateGebruiker(mygebruiker)
            Catch ex As Exception
                Throw
            Finally
            End Try
        End Function

        Public Function DeleteGebruiker(ByVal mygebruiker As Integer) As Integer

            Try
                Return myDalGebruikers.DeleteGebruiker(mygebruiker)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

        Public Sub DoImportGebruikers(ByVal mylistgebruikers As List(Of Gebruikers))
            For Each item As Gebruikers In mylistgebruikers
                Dim Naam As String = item.Naam
                Dim Voornaam As String = item.Voornaam
                Dim gebDatum As Date = item.GebDatum
                Dim i As Integer = 0

                i = myDalGebruikers.CheckGebruiker(Naam, Voornaam, gebDatum) 'kijken of de entiteit nog moet worden aangemaakt

                If i = -1 Then ' nieuwe entiteit aanmaken
                    Try
                        i = myDalGebruikers.InsertGebruiker(item)
                    Catch ex As Exception
                        Throw
                    End Try
                End If
            Next
        End Sub

        Public Function CheckToegangGebruiker(ByVal ID_Lid As Integer, ByVal pagina As Integer) As Integer

            Try
                Return myDalGebruikers.CheckToegangGebruiker(ID_Lid, pagina)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

#End Region

#Region "Rechten Gebruikers"

        Public Function GetRechtenGebruiker(ByVal sort As String, ByVal IDLid As Integer) As List(Of Rechten)
            Dim mylist As New List(Of Rechten)

            Try
                Dim dt As DataTable = myDalGebruikers.getRechtenGebruiker(sort, IDLid)
                For Each myRow As DataRow In dt.Rows
                    Dim myrecht As New Rechten With {
                        .Id = myRow("id")
                    }
                    myrecht.Groep.Id = myRow("groepid")
                    myrecht.Groep.beschrijving = myRow("groepbeschrijving").ToString
                    myrecht.Gebruiker.IdLid = myRow("gebruikerid")
                    myrecht.Gebruiker.Naam = myRow("Naam").ToString
                    myrecht.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myrecht.Actie.Id = myRow("actieid")
                    myrecht.Actie.beschrijving = myRow("actiebeschrijving").ToString
                    myrecht.Validate = myRow("Validate")

                    mylist.Add(myrecht)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return mylist
        End Function

        Public Function InsertRechtenGebruiker(ByVal myrecht As Rechten) As Integer

            Try
                Return myDalGebruikers.InsertRechtenGebruiker(myrecht)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

        Public Function UpdateRechtenGebruiker(ByVal myrecht As Rechten) As Integer

            Try
                Return myDalGebruikers.UpdateRechtenGebruiker(myrecht)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

        Public Function DeleteRechtenGebruiker(ByVal myrecht As Rechten) As Integer

            Try
                Return myDalGebruikers.DeleteRechtenGebruiker(myrecht)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

#End Region

#Region "Acties"
        Public Function GetActies(ByVal sort As String) As List(Of pic_Acties)
            Dim myList As New List(Of pic_Acties)

            Try
                Dim dt As DataTable = myDalGebruikers.getActies(sort)
                For Each myRow As DataRow In dt.Rows
                    Dim myactie As New pic_Acties
                    myactie.Id = myRow("Id").ToString
                    myactie.beschrijving = myRow("beschrijving").ToString
                    myList.Add(myactie)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function GetActieIdbyBeschrijving(ByVal strbeschrijving As String) As Integer
            Dim i As Integer

            Try
                i = myDalGebruikers.GetActieIdbyBeschrijving(strbeschrijving)

            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

        Public Function GetActiesWedJur(ByVal sort As String) As List(Of pic_Acties)
            Dim myList As New List(Of pic_Acties)

            Try
                Dim dt As DataTable = myDalGebruikers.GetActiesWedJur(sort)
                For Each myRow As DataRow In dt.Rows
                    Dim myactie As New pic_Acties
                    myactie.Id = myRow("Id").ToString
                    myactie.beschrijving = myRow("beschrijving").ToString
                    myList.Add(myactie)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function CheckToegangenGebruiker(ByVal ID_Lid As Integer) As List(Of Rechten)
            Dim mylist As New List(Of Rechten)
            Try
                Dim dt As DataTable = myDalGebruikers.CheckToegangenGebruiker(ID_Lid)
                For Each myRow As DataRow In dt.Rows
                    Dim myToegang As New Rechten
                    myToegang.Gebruiker.IdLid = myRow("ID_Lid")
                    myToegang.Actie.Id = myRow("ID_Actie")
                    myToegang.Validate = myRow("Validate")
                    myToegang.Actie.beschrijving = myRow("menu")
                    mylist.Add(myToegang)
                Next
            Catch ex As Exception
                Throw
            Finally

            End Try
            Return mylist
        End Function

#End Region

#Region "Disciplines"
        Public Function GetDisciplines(ByVal sort As String) As List(Of pic_Disciplines)
            Dim myList As New List(Of pic_Disciplines)

            Try
                Dim dt As DataTable = myDalGebruikers.getDisciplines(sort)
                For Each myRow As DataRow In dt.Rows
                    Dim mydiscipline As New pic_Disciplines With {
                        .Id = myRow("Id").ToString,
                        .beschrijving = myRow("beschrijving").ToString,
                        .active = myRow("active").ToString
                    }

                    myList.Add(mydiscipline)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function GetDisciplineIdbyBeschrijving(ByVal strbeschrijving As String) As Integer
            Dim i As Integer

            Try
                i = myDalGebruikers.GetDisciplineIdbyBeschrijving(strbeschrijving)

            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

        Public Function GetDisciplinebyGroep(ByVal idGroep As Integer) As Integer
            Dim i As Integer

            Try
                i = myDalGebruikers.GetDisciplinebyGroep(idGroep)

            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function
        Public Function InsertDiscipline(ByVal myDiscipline As pic_Disciplines) As Integer
            Try
                Return myDalGebruikers.insertDiscipline(myDiscipline)
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Function

        Public Function UpdateDiscipline(ByVal myDiscipline As pic_Disciplines) As Integer
            Try
                Return myDalGebruikers.UpdateDiscipline(myDiscipline)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function DeleteDiscipline(ByVal myDiscipline As pic_Disciplines) As Integer
            Try
                Return myDalGebruikers.DeleteDiscipline(myDiscipline)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function
#End Region

#Region "Trainingsgroepen"
        Public Function GetTrainingsGroepenbyDiscipine(ByVal sort As String, ByVal myIdDiscipline As Integer) As List(Of pic_Trainingsgroepen)
            Dim myList As New List(Of pic_Trainingsgroepen)

            Try
                Dim dt As DataTable = myDalGebruikers.getTrainingsGroepenbyDiscipine(sort, myIdDiscipline)
                For Each myRow As DataRow In dt.Rows
                    Dim myTrainingsgroepen As New pic_Trainingsgroepen With {
                        .Id = myRow("Id").ToString,
                        .beschrijving = myRow("beschrijving").ToString,
                        .active = myRow("active").ToString
                    }

                    myList.Add(myTrainingsgroepen)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function GetTrainingsgroepen(ByVal sort As String) As List(Of pic_Trainingsgroepen)
            Dim myList As New List(Of pic_Trainingsgroepen)

            Try
                Dim dt As DataTable = myDalGebruikers.GetTrainingsgroepen(sort)
                For Each myRow As DataRow In dt.Rows
                    Dim myTrainingsgroepen As New pic_Trainingsgroepen With {
                        .Id = myRow("Id").ToString,
                        .beschrijving = myRow("beschrijving").ToString,
                        .active = myRow("active").ToString
                    }

                    myList.Add(myTrainingsgroepen)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function InsertTrainingsgroep(ByVal myTrainingsgroepen As pic_Trainingsgroepen) As Integer
            Try
                Return myDalGebruikers.InsertTrainingsgroep(myTrainingsgroepen)
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Function

        Public Function UpdateTrainingsgroep(ByVal myTrainingsgroepen As pic_Trainingsgroepen) As Integer
            Try
                Return myDalGebruikers.UpdateTrainingsgroep(myTrainingsgroepen)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function DeleteTrainingsgroep(ByVal myTrainingsgroepen As pic_Trainingsgroepen) As Integer
            Try
                Return myDalGebruikers.DeleteTrainingsgroep(myTrainingsgroepen)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function
#End Region

#Region "Aanwezigheden"

        Public Function GetLedenbyGroep(ByVal sort As String, ByVal idgroep As Integer) As List(Of Handelingen)
            Dim myList As New List(Of Handelingen)

            Try
                Dim dt As DataTable = myDalGebruikers.getLedenbyGroep(sort, idgroep)
                For Each myRow As DataRow In dt.Rows
                    Dim myhandeling As New Handelingen
                    myhandeling.Gebruiker.IdLid = myRow("id_lid")
                    myhandeling.Gebruiker.Naam = myRow("Naam").ToString
                    myhandeling.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myhandeling.Gebruiker.GebDatum = myRow("gebdatum")
                    myhandeling.Actie.Id = myRow("id_actie")
                    myList.Add(myhandeling)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function GetAanwezighedenbyGroep(ByVal sort As String, ByVal idgroep As Integer, ByVal strdatum As String) As List(Of Aanwezigheid)
            Dim myList As New List(Of Aanwezigheid)

            Try
                Dim dt As DataTable = myDalGebruikers.getAanwezighedenbyGroep(sort, idgroep, strdatum)
                For Each myRow As DataRow In dt.Rows
                    Dim myAanwezigheid As New Aanwezigheid
                    myAanwezigheid.Gebruiker.IdLid = myRow("idlid")
                    myAanwezigheid.Gebruiker.Naam = myRow("Naam").ToString
                    myAanwezigheid.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myAanwezigheid.Gebruiker.GebDatum = myRow("gebdatum")
                    myAanwezigheid.Functie = myRow("id_actie")

                    If IsDBNull(myRow("datum")) Then
                        myAanwezigheid.Datum = "01/01/1900"
                        myAanwezigheid.Aanwezig = 0
                        myAanwezigheid.Opmerking = ""
                    Else
                        myAanwezigheid.Datum = myRow("Datum").ToString
                        myAanwezigheid.Aanwezig = myRow("aanwezig")
                        myAanwezigheid.Opmerking = myRow("Opmerking").ToString
                    End If
                    myList.Add(myAanwezigheid)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function InsertAanwezigheid(ByVal myAanwezigheid As Aanwezigheid) As Integer
            Try
                Return myDalGebruikers.insertAanwezigheid(myAanwezigheid)
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Function

        Public Function UpdateAanwezigheid(ByVal myAanwezigheid As Aanwezigheid) As Integer
            Try
                Return myDalGebruikers.UpdateAanwezigheid(myAanwezigheid)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function DeleteAanwezigheid(ByVal myAanwezigheid As Aanwezigheid) As Integer
            Try
                Return myDalGebruikers.DeleteAanwezigheid(myAanwezigheid)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function


#End Region

#Region "Handelingen"

        Public Function GetRapportAllhandelingenbygebruiker(ByVal idlid As Integer, ByVal datumlaag As Date, ByVal datumhoog As Date) As List(Of RapportHandelingen)
            Dim myList As New List(Of RapportHandelingen)

            Try
                Dim dt As DataTable = myDalGebruikers.GetRapportAllhandelingenbygebruiker(idlid, datumlaag, datumhoog)
                For Each myRow As DataRow In dt.Rows
                    Dim myhandeling As New RapportHandelingen
                    myhandeling.Id = myRow("id")
                    myhandeling.Datum = myRow("datum")
                    myhandeling.Groep = myRow("groepbeschrijving").ToString
                    myhandeling.Discipline = myRow("Disciplinebeschrijving").ToString
                    myhandeling.Actie = myRow("actiebeschrijving").ToString
                    myhandeling.Info = myRow("Info").ToString
                    myhandeling.Aantal = myRow("Aantal")
                    myhandeling.Bedrag = myRow("Bedraglg")
                    myhandeling.KM = myRow("km")
                    myhandeling.dagVM = myRow("dagVM")
                    myhandeling.dagNM = myRow("dagNM")
                    myhandeling.dagAV = myRow("dagAV")
                    If IsDBNull(myRow("valid")) Then
                        myhandeling.Validate = 1
                    Else
                        myhandeling.Validate = myRow("valid")
                    End If
                    myList.Add(myhandeling)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function GetAllhandelingenbygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String, ByVal datumlaag As Date, ByVal datumhoog As Date) As List(Of Handelingen)
            Dim myList As New List(Of Handelingen)

            Try
                Dim dt As DataTable = myDalGebruikers.GetAllhandelingenbygebruiker(sort, idlid, strfilter, datumlaag, datumhoog)
                For Each myRow As DataRow In dt.Rows
                    Dim myhandeling As New Handelingen
                    myhandeling.Id = myRow("id")
                    myhandeling.Datum = myRow("datum")
                    myhandeling.Groep.Id = myRow("groepid")
                    myhandeling.Groep.beschrijving = myRow("groepbeschrijving").ToString
                    myhandeling.Discipline.Id = myRow("Disciplineid")
                    myhandeling.Discipline.beschrijving = myRow("Disciplinebeschrijving").ToString
                    myhandeling.Gebruiker.IdLid = myRow("gebruikerid")
                    myhandeling.Gebruiker.Naam = myRow("Naam").ToString
                    myhandeling.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myhandeling.Actie.Id = myRow("actieid")
                    myhandeling.Actie.beschrijving = myRow("actiebeschrijving").ToString
                    myhandeling.Info = myRow("Info").ToString
                    myhandeling.Aantal = myRow("Aantal").ToString
                    myhandeling.Bedrag = myRow("Bedrag").ToString
                    If IsDBNull(myRow("valid")) Then
                        myhandeling.Validate = 1
                    Else
                        myhandeling.Validate = myRow("valid")
                    End If
                    myList.Add(myhandeling)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function GetGebruikersOpenHandeling(ByVal sort As String, ByVal filter As String, ByVal intopen As Integer) As List(Of Gebruikers)
            Dim myList As New List(Of Gebruikers)

            Try
                Dim dt As DataTable = myDalGebruikers.GetGebruikersOpenHandeling(sort, filter, intopen)
                For Each myRow As DataRow In dt.Rows
                    Dim mygebruiker As New Gebruikers
                    mygebruiker.Naam = myRow("Naam").ToString
                    mygebruiker.Voornaam = myRow("Voornaam").ToString
                    mygebruiker.Email = myRow("Email").ToString
                    mygebruiker.GSM = myRow("GSM").ToString
                    mygebruiker.IdLid = myRow("ID_Lid")
                    If IsDBNull(myRow("valid")) Then
                        mygebruiker.Validate = 1
                    Else
                        mygebruiker.Validate = myRow("valid")
                    End If
                    myList.Add(mygebruiker)
                Next

            Catch ex As Exception
                Throw
            End Try
            Return myList
        End Function

        Public Function GetHandelingLesgeverVergoedingbygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As List(Of Handelingen)
            Dim myList As New List(Of Handelingen)

            Try
                Dim dt As DataTable = myDalGebruikers.GetHandelingLesgeverVergoedingbygebruiker(sort, idlid, strfilter)
                For Each myRow As DataRow In dt.Rows
                    Dim myhandeling As New Handelingen
                    myhandeling.Id = myRow("id")
                    myhandeling.Datum = myRow("datum")
                    myhandeling.Discipline.Id = myRow("Disciplineid")
                    myhandeling.Discipline.beschrijving = myRow("Disciplinebeschrijving").ToString
                    myhandeling.Groep.Id = myRow("GroepId").ToString
                    myhandeling.Groep.beschrijving = myRow("GroepBeschrijving").ToString
                    myhandeling.Gebruiker.IdLid = myRow("gebruikerid")
                    myhandeling.Gebruiker.Naam = myRow("Naam").ToString
                    myhandeling.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myhandeling.Actie.Id = myRow("actieid")
                    myhandeling.Actie.beschrijving = myRow("actiebeschrijving").ToString
                    myhandeling.Info = myRow("Info").ToString
                    myhandeling.Aantal = myRow("Aantal").ToString
                    myhandeling.Validate = myRow("Valid")
                    myList.Add(myhandeling)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function
        Public Function GetHandelingAnderebygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As List(Of Handelingen)
            Dim myList As New List(Of Handelingen)

            Try
                Dim dt As DataTable = myDalGebruikers.GetHandelingAnderebygebruiker(sort, idlid, strfilter)
                For Each myRow As DataRow In dt.Rows
                    Dim myhandeling As New Handelingen
                    myhandeling.Id = myRow("id")
                    myhandeling.Datum = myRow("datum")
                    myhandeling.Discipline.Id = myRow("Disciplineid")
                    myhandeling.Discipline.beschrijving = myRow("Disciplinebeschrijving").ToString
                    myhandeling.Groep.Id = myRow("GroepId").ToString
                    myhandeling.Groep.beschrijving = myRow("GroepBeschrijving").ToString
                    myhandeling.Gebruiker.IdLid = myRow("gebruikerid")
                    myhandeling.Gebruiker.Naam = myRow("Naam").ToString
                    myhandeling.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myhandeling.Actie.Id = myRow("actieid")
                    myhandeling.Actie.beschrijving = myRow("actiebeschrijving").ToString
                    myhandeling.Info = myRow("Info").ToString
                    myhandeling.Aantal = myRow("Aantal").ToString
                    myhandeling.Validate = myRow("Valid")
                    myList.Add(myhandeling)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function
        Public Function Gethandelingbygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As List(Of Handelingen)
            Dim myList As New List(Of Handelingen)

            Try
                Dim dt As DataTable = myDalGebruikers.Gethandelingbygebruiker(sort, idlid, strfilter)
                For Each myRow As DataRow In dt.Rows
                    Dim myhandeling As New Handelingen
                    myhandeling.Id = myRow("id")
                    myhandeling.Datum = myRow("datum")
                    myhandeling.Discipline.Id = myRow("Disciplineid")
                    myhandeling.Discipline.beschrijving = myRow("Disciplinebeschrijving").ToString
                    myhandeling.Gebruiker.IdLid = myRow("gebruikerid")
                    myhandeling.Gebruiker.Naam = myRow("Naam").ToString
                    myhandeling.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myhandeling.Actie.Id = myRow("actieid")
                    myhandeling.Actie.beschrijving = myRow("actiebeschrijving").ToString
                    myhandeling.Info = myRow("Info").ToString
                    myhandeling.Aantal = myRow("Aantal").ToString
                    myhandeling.dagVM = myRow("dagvm").ToString
                    myhandeling.dagNM = myRow("dagNM").ToString
                    myhandeling.dagAV = myRow("dagAV").ToString
                    myhandeling.Validate = myRow("Valid")
                    myList.Add(myhandeling)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function GethandelingWedJurybygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As List(Of Handelingen)
            Dim myList As New List(Of Handelingen)

            Try
                Dim dt As DataTable = myDalGebruikers.GethandelingWedJurybygebruiker(sort, idlid, strfilter)
                For Each myRow As DataRow In dt.Rows
                    Dim myhandeling As New Handelingen
                    myhandeling.Id = myRow("id")
                    myhandeling.Datum = myRow("datum")
                    myhandeling.Discipline.Id = myRow("Disciplineid")
                    myhandeling.Discipline.beschrijving = myRow("Disciplinebeschrijving").ToString
                    myhandeling.Gebruiker.IdLid = myRow("gebruikerid")
                    myhandeling.Gebruiker.Naam = myRow("Naam").ToString
                    myhandeling.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myhandeling.Actie.Id = myRow("actieid")
                    myhandeling.Actie.beschrijving = myRow("actiebeschrijving").ToString
                    myhandeling.Info = myRow("Info").ToString
                    myhandeling.Aantal = myRow("Aantal").ToString
                    myhandeling.dagVM = myRow("dagvm").ToString
                    myhandeling.dagNM = myRow("dagNM").ToString
                    myhandeling.dagAV = myRow("dagAV").ToString
                    myhandeling.Validate = myRow("Valid")
                    myList.Add(myhandeling)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function
        Public Function GethandelingVerplaatsingbygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As List(Of Handelingen)
            Dim myList As New List(Of Handelingen)

            Try
                Dim dt As DataTable = myDalGebruikers.GethandelingVerplaatsingbygebruiker(sort, idlid, strfilter)
                For Each myRow As DataRow In dt.Rows
                    Dim myhandeling As New Handelingen
                    myhandeling.Id = myRow("id")
                    myhandeling.Datum = myRow("datum")
                    myhandeling.Discipline.Id = myRow("Disciplineid")
                    myhandeling.Discipline.beschrijving = myRow("Disciplinebeschrijving").ToString
                    myhandeling.Gebruiker.IdLid = myRow("gebruikerid")
                    myhandeling.Gebruiker.Naam = myRow("Naam").ToString
                    myhandeling.Gebruiker.Voornaam = myRow("Voornaam").ToString
                    myhandeling.Actie.Id = myRow("actieid")
                    myhandeling.Actie.beschrijving = myRow("actiebeschrijving").ToString
                    myhandeling.Info = myRow("Info").ToString
                    myhandeling.Aantal = myRow("Aantal").ToString
                    myhandeling.dagVM = myRow("dagvm").ToString
                    myhandeling.dagNM = myRow("dagNM").ToString
                    myhandeling.dagAV = myRow("dagAV").ToString
                    myhandeling.Validate = myRow("Valid")
                    myList.Add(myhandeling)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function Inserthandeling(ByVal myhandeling As Handelingen) As Integer
            Try
                Return myDalGebruikers.insertHandeling(myhandeling)
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Function

        Public Function Updatehandeling(ByVal myhandeling As Handelingen) As Integer
            Try
                Return myDalGebruikers.UpdateHandeling(myhandeling)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function Deletehandeling(ByVal myhandeling As Handelingen) As Integer
            Try
                Return myDalGebruikers.DeleteHandeling(myhandeling)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function
#End Region

#Region "Vergoedingen"
        Public Function GetAllLesgeverVergoedingen(ByVal sort As String) As List(Of Lesgeververgoeding)
            Dim myList As New List(Of Lesgeververgoeding)

            Try
                Dim dt As DataTable = myDalGebruikers.GetAllLesgeverVergoedingen(sort)
                For Each myRow As DataRow In dt.Rows
                    Dim myvergoeding As New Lesgeververgoeding
                    myvergoeding.Id = myRow("Id").ToString
                    myvergoeding.Gebruiker.IdLid = myRow("beschrijving").ToString
                    myvergoeding.Bedrag = myRow("beschrijving").ToString
                    myvergoeding.Datum = myRow("active").ToString


                    myList.Add(myvergoeding)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function GetLesgeverVergoedingen(ByVal sort As String, ByVal idlid As Integer) As List(Of Lesgeververgoeding)
            Dim myList As New List(Of Lesgeververgoeding)

            Try
                Dim dt As DataTable = myDalGebruikers.GetLesgeverVergoedingen(sort, idlid)
                For Each myRow As DataRow In dt.Rows
                    Dim myvergoeding As New Lesgeververgoeding
                    myvergoeding.Id = myRow("idvergoeding")
                    myvergoeding.Gebruiker.IdLid = myRow("gebruikerid")
                    myvergoeding.Bedrag = myRow("Bedrag").ToString
                    myvergoeding.Datum = myRow("Datum").ToString
                    myvergoeding.Km = myRow("km").ToString
                    myvergoeding.info = myRow("info").ToString

                    myList.Add(myvergoeding)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function InsertLesgeverVergoeding(ByVal myvergoeding As Lesgeververgoeding) As Integer
            Try
                Return myDalGebruikers.InsertLesgeverVergoeding(myvergoeding)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function UpdateLesgeverVergoeding(ByVal myvergoeding As Lesgeververgoeding) As Integer
            Try
                Return myDalGebruikers.UpdateLesgeverVergoeding(myvergoeding)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function DeleteLesgeverVergoeding(ByVal myvergoeding As Lesgeververgoeding) As Integer
            Try
                Return myDalGebruikers.DeleteLesgeverVergoeding(myvergoeding)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function
#End Region


#Region "Boodschappen"
        Public Function getBoodschapById(ByVal IDBoodschap As Integer) As Boodschappen
            Dim myboodschap As New Boodschappen
            Try
                Dim dt As DataTable = myDalGebruikers.GetBoodschappenById(IDBoodschap)
                For Each myRow As DataRow In dt.Rows
                    myboodschap.Id = myRow("Id")
                    myboodschap.Datum = myRow("Datum").ToString
                    myboodschap.Zender.IdLid = myRow("Zender")
                    myboodschap.Ontvanger.IdLid = myRow("Ontvanger")
                    myboodschap.Inhoud = myRow("Inhoud").ToString
                    myboodschap.gelezen = myRow("gelezen")
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myboodschap
        End Function

        Public Function GetBoodschappenByLid(ByVal idlid As Integer, ByVal sort As String) As List(Of Boodschappen)
            Dim myList As New List(Of Boodschappen)

            Try
                Dim dt As DataTable = myDalGebruikers.GetBoodschappenByLid(sort, idlid)
                For Each myRow As DataRow In dt.Rows
                    Dim myboodschap As New Boodschappen
                    myboodschap.Id = myRow("Id")
                    myboodschap.Datum = myRow("Datum").ToString
                    myboodschap.Zender.IdLid = myRow("Zender")
                    myboodschap.Ontvanger.IdLid = myRow("Ontvanger")
                    myboodschap.Inhoud = myRow("Inhoud").ToString
                    myboodschap.gelezen = myRow("gelezen")
                    myList.Add(myboodschap)
                Next
            Catch ex As Exception
                Throw
            End Try

            Return myList
        End Function

        Public Function InsertBoodschap(ByVal myBoodschap As Boodschappen, ByVal myOntvangers As List(Of Integer)) As Integer
            Try
                For Each myOnt As Integer In myOntvangers
                    myBoodschap.Ontvanger.IdLid = myOnt
                    myDalGebruikers.InsertBoodschap(myBoodschap)
                Next
            Catch ex As Exception
                Throw
                Return -1
            Finally
            End Try
            Return 1
        End Function

        Public Function UpdateBoodschap(ByVal myboodschap As Boodschappen) As Integer
            Try
                Return myDalGebruikers.UpdateBoodschap(myboodschap)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function DeleteBoodschap(ByVal idboodschap As Integer) As Integer
            Try
                Return myDalGebruikers.DeleteBoodschap(idboodschap)
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function
#End Region

    End Class

End Namespace
