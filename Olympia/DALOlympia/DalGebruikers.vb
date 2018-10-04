Imports Olympia.OBJOlympia
Imports System.Data.SqlClient
Imports System.Data.OleDb

Namespace DALOlympia

    Public Class DalGebruikers : Inherits DALBase
        Private strSQL As New StringBuilder
        Dim i As Integer = 0

        Public Function ReadFileExcell(ByVal strfile As String) As DataTable
            Dim dt As New DataTable
            strSQL.Remove(0, strSQL.Length)

            Const strTableName As String = "1"
            Dim cnnOLEDB As New OleDbConnection(SetExcelConnectionString("import.xls", UseHeader.Yes, ExcelImex.Resolve))
            cnnOLEDB.Open()
            Try

                Dim myOLEDBAdapter As New OleDbDataAdapter("SELECT DISTINCT * FROM [" & strTableName & "$] ", cnnOLEDB)
                myOLEDBAdapter.Fill(dt)
            Catch ex As Exception
                Throw
            End Try
            cnnOLEDB.Close()
            Return dt

        End Function

        Public Function Checkemail(ByVal mygebruiker As Gebruikers) As Integer
            strSQL.Remove(0, strSQL.Length)
            strSQL.Append("SELECT ID_Lid from Gebruikers where email = ? ")

            Try
                DoParameterAdd("@Email", mygebruiker.Email, 13)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function GetAuthGebruiker(ByVal mygebruiker As Gebruikers) As Integer
            strSQL.Remove(0, strSQL.Length)
            strSQL.Append("SELECT * from Gebruikers where ID_Lid = ? and password = ? ")

            Try
                DoParameterAdd("@IdLid", mygebruiker.IdLid, 10)
                DoParameterAdd("@Paswoord", mygebruiker.Paswoord, 13)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

#Region "Logging"

        Public Function InsertLogging(ByVal myLogging As Logging) As Integer
            strSQL.Remove(0, strSQL.Length)
            strSQL.Append("INSERT into logging (Datum, ID_Lid, Event, type) VALUES({fn NOW()}, ?, ?, ? )")

            Try
                DoParameterAdd("@ID_Lid", myLogging.Gebruiker.IdLid, 10)
                DoParameterAdd("@EventLogging", myLogging.EventLogging, 13)
                DoParameterAdd("@Type", myLogging.Type, 10)

                i = executeDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function GetLogging(ByVal sort As String, ByVal startDate As Date, ByVal endDate As Date, ByVal intType As Integer, ByVal strInfo As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from logging L ")
                strSQL.Append("join gebruikers g on g.ID_Lid=l.ID_Lid ")
                strSQL.Append("WHERE 1=1 And (datum BETWEEN '" & startDate.ToString("yyyy-MM-dd") & "' AND '" & endDate.ToString("yyyy-MM-dd") & "' ) ")


                If intType > 0 Then
                    strSQL.Append(" AND Type = ? ")
                    DoParameterAdd("@intType", intType, 10)
                End If

                If Not strInfo = "" Then
                    strSQL.Append(" AND Event LIKE ? ")
                    DoParameterAdd("@strInfo", "%" & strInfo & "%", 13)
                End If

                If sort <> "" Then
                    strSQL.Append(" ORDER BY " & sort)
                End If

                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt

        End Function
#End Region

#Region "Gebruikers"

        Public Function GetGebruiker(ByVal ID_Lid As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from Gebruikers where active=1 and ID_Lid = ? ")
                DoParameterAdd("@idlid", ID_Lid, 10)

                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try
            Return mydt
        End Function

        Public Function GetTrainers(ByVal sort As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from Gebruikers g join Rechten r on g.ID_Lid = r.ID_Lid where active=1 and id_actie=31 order by " & sort)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try
            Return mydt
        End Function

        Public Function GetGebruikers(ByVal sort As String, ByVal filter As String) As DataTable
            Dim mydt As New DataTable
            Try
                If filter Like "" Then
                    strSQL.Remove(0, strSQL.Length)
                    strSQL.Append("SELECT * from Gebruikers where active=1 order by " & sort)
                    mydt = ReturnDALDataTable(strSQL.ToString)
                Else
                    strSQL.Remove(0, strSQL.Length)
                    strSQL.Append("SELECT * from Gebruikers where active=1 AND (naam like '%" & filter & "%' or voornaam like '%" & filter & "%' ) order by " & sort)
                    mydt = ReturnDALDataTable(strSQL.ToString)
                End If
            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function InsertGebruiker(ByVal mygebruiker As Gebruikers) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("INSERT into gebruikers (email, naam, voornaam, gebdatum, geslacht,gsm,password) VALUES (?,?,?,?,?,?,?)")
                DoParameterAdd("@Email", mygebruiker.Email, 13)
                DoParameterAdd("@Naam", mygebruiker.Naam, 13)
                DoParameterAdd("@Voornaam", mygebruiker.Voornaam, 13)
                DoParameterAdd("@GebDatum", mygebruiker.GebDatum, 23)
                DoParameterAdd("@geslacht", mygebruiker.Geslacht, 13)
                DoParameterAdd("@gsm", mygebruiker.GSM, 13)
                DoParameterAdd("@Paswoord", mygebruiker.Paswoord, 13)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function DeleteGebruiker(ByVal ID_Lid As Integer) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("update gebruikers set Active = 0 where ID_lid = ?")
                DoParameterAdd("@idlid", ID_Lid, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function UpdateGebruiker(ByVal mygebruiker As Gebruikers) As Integer
            Dim i As Integer = 0

            Dim MyCmd As New SqlCommand("update gebruikers set email=@email, naam=@naam,voornaam=@voornaam,gemeente=@gemeente,postcode=@postcode,straat=@straat,huisnummer=@huisnummer,rekeningnummer=@rekeningnummer,info=@info,gebdatum=@gebdatum,geslacht= @geslacht where ID_lid = @ID_Lid ")
            Try

                MyCmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = mygebruiker.Email
                MyCmd.Parameters.Add("@Naam", SqlDbType.NVarChar).Value = mygebruiker.Naam
                MyCmd.Parameters.Add("@Voornaam", SqlDbType.NVarChar).Value = mygebruiker.Voornaam
                MyCmd.Parameters.Add("@gemeente", SqlDbType.NVarChar).Value = mygebruiker.Gemeente
                MyCmd.Parameters.Add("@postcode", SqlDbType.NVarChar).Value = mygebruiker.Postcode
                MyCmd.Parameters.Add("@straat", SqlDbType.NVarChar).Value = mygebruiker.Straat
                MyCmd.Parameters.Add("@huisnummer", SqlDbType.NVarChar).Value = mygebruiker.Huisnr
                MyCmd.Parameters.Add("@rekeningnummer", SqlDbType.NVarChar).Value = mygebruiker.Rekeningnummer
                MyCmd.Parameters.Add("@info", SqlDbType.NVarChar).Value = mygebruiker.Info
                MyCmd.Parameters.Add("@GebDatum", SqlDbType.Date).Value = mygebruiker.GebDatum
                MyCmd.Parameters.Add("@geslacht", SqlDbType.NVarChar).Value = mygebruiker.Geslacht
                MyCmd.Parameters.Add("@ID_Lid", SqlDbType.Int).Value = mygebruiker.IdLid

                conn.Open()
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            conn.Close()
            Return i
        End Function

        Public Function CheckGebruiker(ByVal Naam As String, ByVal Voornaam As String, ByVal gebDatum As Date) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("select * from gebruikers where naam = ? AND voornaam = ? AND gebdatum= ? ")
                DoParameterAdd("@naam", Naam, 13)
                DoParameterAdd("@voornaam", Voornaam, 13)
                DoParameterAdd("@gebdatum", gebDatum, 13)

                conn.Open()
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            conn.Close()
            Return i
        End Function

        Public Function CheckToegangGebruiker(ByVal ID_Lid As Integer, ByVal pagina As Integer) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("select Validate from Rechten where ID_Lid = ? AND ID_actie = ?")
                DoParameterAdd("@idlid", ID_Lid, 10)
                DoParameterAdd("@IDActie", pagina, 10)
                i = ExecuteDALScalar(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function CheckToegangenGebruiker(ByVal ID_Lid As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("select ID_Lid, ID_actie, Validate, menu from Rechten r join PIC_Acties a on a.id = r.ID_Actie where ID_Lid = ?")
                DoParameterAdd("@idlid", ID_Lid, 10)

                mydt = ReturnDALDataTable(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return mydt
        End Function

#End Region

        Public Function GetActies(ByVal sort As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from PIC_Acties order by " & sort)

                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

#Region "Disciplines"

        Public Function GetDisciplines(ByVal sort As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from PIC_Disciplines where active=1 order by " & sort)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function InsertDiscipline(ByVal myDiscipline As pic_Disciplines) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("INSERT INTO PIC_Disciplines (Beschrijving) values (?) ")
                DoParameterAdd("@Beschrijving", myDiscipline.beschrijving, 13)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

        Public Function UpdateDiscipline(ByVal myDiscipline As pic_Disciplines) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("UPDATE PIC_Disciplines SET Beschrijving = ? where ID_Discipline = ? ")
                DoParameterAdd("@Beschrijving", myDiscipline.beschrijving, 13)
                DoParameterAdd("@ID_Discipline", myDiscipline.Id, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

        Public Function DeleteDiscipline(ByVal myDiscipline As pic_Disciplines) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("UPDATE PIC_Disciplines SET active = 0 WHERE Id_discipline = ? ")
                DoParameterAdd("@ID_Discipline", myDiscipline.Id, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

#End Region

#Region "TrainingsGroepen"

        Public Function GetTrainingsGroepenbyDiscipine(ByVal sort As String, ByVal myIdDiscipline As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from PIC_Trainingsgroepen where active=1 and ID_Discipline = ? order by " & sort)
                DoParameterAdd("@myIdDiscipline", myIdDiscipline, 10)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function GetAllTrainingsGroepen(ByVal sort As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from PIC_Trainingsgroepen where active=1 order by " & sort)
                mydt = ReturnDALDataTable(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function InsertTrainingsgroep(ByVal myTrainingsGroep As pic_Trainingsgroepen) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("INSERT INTO PIC_Trainingsgroepen (Beschrijving, ID_Discipline) values (?, ?) ")
                DoParameterAdd("@Beschrijving", myTrainingsGroep.beschrijving, 13)
                DoParameterAdd("@ID_Discipline", myTrainingsGroep.Discipline.Id, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

        Public Function UpdateTrainingsgroep(ByVal myTrainingsGroep As pic_Trainingsgroepen) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("UPDATE PIC_Trainingsgroepen SET Beschrijving = @? where ID = @? ")
                DoParameterAdd("@Beschrijving", myTrainingsGroep.beschrijving, 13)
                DoParameterAdd("@ID_Groep", myTrainingsGroep.Id, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

        Public Function DeleteTrainingsgroep(ByVal myTrainingsGroep As pic_Trainingsgroepen) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("UPDATE PIC_Trainingsgroepen SET active = 0 WHERE Id = ? ")
                DoParameterAdd("@ID_Groep", myTrainingsGroep.Id, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

#End Region

#Region "Aanwezigheden"

        Public Function GetLedenbyGroep(ByVal sort As String, ByVal idGroep As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("select g.id_lid,naam, voornaam, gebdatum, id_actie from Rechten r ")
                strSQL.Append("left join gebruikers g on r.id_lid = g.id_lid ")
                strSQL.Append("where ID_Groep = @IDGROEP and (id_actie =30 or id_actie=31) ")
                strSQL.Append("order by " & sort)
                DoParameterAdd("@IDGROEP", idGroep, 10)

            Catch ex As Exception
                Throw
            End Try
            Return mydt
        End Function

        Public Function GetAanwezighedenbyGroep(ByVal sort As String, ByVal idGroep As Integer, ByVal strdatum As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("select g.ID_Lid as idlid,* from Aanwezigheden a ")
                strSQL.Append("right join gebruikers g on g.ID_Lid = a.ID_Lid ")
                strSQL.Append("left join rechten r on g.ID_Lid = r.ID_Lid ")
                strSQL.Append("where r.ID_Groep = ? and (r.id_actie =30 or r.id_actie=31) ")
                DoParameterAdd("@IDGROEP", idGroep, 10)
                If Not strdatum Like "" Then
                    strSQL.Append("AND datum like @strdatum) ")
                    DoParameterAdd("@strdatum", strdatum, 23)
                End If

                strSQL.Append("order by " & sort)
                i = ExecuteDALScalar(strSQL.ToString)

            Catch ex As Exception
                    Throw
                End Try
            Return mydt
        End Function

        Public Function InsertAanwezigheid(ByVal myAanwezigheid As Aanwezigheid) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("INSERT INTO Aanwezigheden (ID_Lid, Datum, ID_Groep, Opmerking, Aanwezig) values (?,?,?,?,?) ")
                DoParameterAdd("@ID_Lid", myAanwezigheid.Gebruiker.IdLid, 10)
                DoParameterAdd("@Datum", myAanwezigheid.Datum, 23)
                DoParameterAdd("@ID_Groep", myAanwezigheid.Groep.Id, 10)
                DoParameterAdd("@Opmerking", myAanwezigheid.Opmerking, 13)
                DoParameterAdd("@aanwezig", myAanwezigheid.Aanwezig, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function UpdateAanwezigheid(ByVal myAanwezigheid As Aanwezigheid) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("UPDATE Aanwezigheden SET opmerking=?, Aanwezig=? where ID = ? ")
                DoParameterAdd("@opmerking", myAanwezigheid.Opmerking, 13)
                DoParameterAdd("@Aanwezig", myAanwezigheid.Aanwezig, 10)
                DoParameterAdd("@ID", myAanwezigheid.Id, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

        Public Function DeleteAanwezigheid(ByVal myAanwezigheid As Aanwezigheid) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("Delete from Aanwezigheden WHERE ID = ? ")
                DoParameterAdd("@ID", myAanwezigheid.Id, 10)

                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

#End Region

#Region "Handelingen"

        Public Function GetAllhandelingenbygebruiker(ByVal sort As String, ByVal idlid As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT H.ID, h.Validate,h.bedrag,h.Datum,D.id as disciplineid, D.beschrijving as disciplinebeschrijving,T.id as groepid,t.beschrijving as groepbeschrijving, A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid, G.naam, G.voornaam, h.info, H.aantal ")
                strSQL.Append("from Handelingen H ")
                strSQL.Append("join gebruikers G on H.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=H.id_groep ")
                strSQL.Append("left join PIC_Disciplines D on D.id = H.id_Discipline ")
                strSQL.Append("left join PIC_acties A on A.id=H.id_actie ")
                strSQL.Append("where h.id_lid = ? ")
                strSQL.Append("order by " & sort)
                DoParameterAdd("@idlid", idlid, 10)
                mydt = ReturnDALDataTable(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function Gethandelingbygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal idactie As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT H.ID, h.Validate,h.bedrag,h.Datum,D.id as disciplineid, D.beschrijving as disciplinebeschrijving,T.id as groepid,t.beschrijving as groepbeschrijving, A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid, G.naam, G.voornaam, h.info, H.aantal ")
                strSQL.Append("from Handelingen H ")
                strSQL.Append("join gebruikers G on H.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=H.id_groep ")
                strSQL.Append("left join PIC_Disciplines D on D.id = H.id_Discipline ")
                strSQL.Append("left join PIC_acties A on A.id=H.id_actie ")
                strSQL.Append("where h.id_lid = ? and H.id_actie = ? ")
                strSQL.Append("order by " & sort)
                DoParameterAdd("@idlid", idlid, 10)
                DoParameterAdd("@idactie", idactie, 10)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function InsertHandeling(ByVal myHandeling As Handelingen) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("INSERT INTO Handelingen (ID_Lid, Datum, ID_Groep,ID_Discipline, ID_Actie, Info, Aantal) values (?,?,?,?,?,?,?)")
                DoParameterAdd("@ID_Lid", myHandeling.Gebruiker.IdLid, 10)
                DoParameterAdd("@Datum", myHandeling.Datum, 23)
                DoParameterAdd("@ID_Groep", myHandeling.Groep.Id, 10)
                DoParameterAdd("@ID_Discipline", myHandeling.Discipline.Id, 10)
                DoParameterAdd("@ID_Actie", myHandeling.Actie.Id, 10)
                DoParameterAdd("@Info", myHandeling.Info, 13)
                DoParameterAdd("@Aantal", myHandeling.Aantal, 13)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function UpdateHandeling(ByVal myHandeling As Handelingen) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("UPDATE Handelingen SET Datum = ?, ID_Discipline = ? ,ID_Groep= ? ,Info= ? , aantal= ? where ID = ? ")
                DoParameterAdd("@Datum", myHandeling.Datum, 23)
                DoParameterAdd("@ID_Discipline", myHandeling.Discipline.Id, 10)
                DoParameterAdd("@ID_Groep", myHandeling.Groep.Id, 10)
                DoParameterAdd("@Info", myHandeling.Info, 13)
                DoParameterAdd("@Aantal", myHandeling.Aantal, 13)
                DoParameterAdd("@ID", myHandeling.Id, 10)
                i = ExecuteDALScalar(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function DeleteHandeling(ByVal myHandeling As Handelingen) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("Delete from Handelingen WHERE ID = ? ")
                DoParameterAdd("@ID_Handeling", myHandeling.Id, 10)

                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

#End Region

#Region "Rechten Gebruikers"

        Public Function GetRechtenGebruiker(ByVal sort As String, ByVal idlid As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT naam, voornaam, R.validate,r.id, T.id as groepid,T.beschrijving as groepbeschrijving, ")
                strSQL.Append("A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid from Rechten R ")
                strSQL.Append("left join gebruikers G on R.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=R.id_groep ")
                strSQL.Append("left join PIC_acties A on A.id=R.id_actie ")
                strSQL.Append("where R.ID_Lid = ? ")
                DoParameterAdd("@idlid", idlid, 10)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try
            Return mydt
        End Function

        Public Function InsertRechtenGebruiker(ByVal myrechten As Rechten) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("INSERT into Rechten (ID_Lid, ID_Actie, ID_Groep, Validate) VALUES (?,?,?,?)")
                DoParameterAdd("@ID_Lid", myrechten.Gebruiker.IdLid, 10)
                DoParameterAdd("@ID_Actie", myrechten.Actie.Id, 10)
                DoParameterAdd("@ID_Groep", myrechten.Groep.Id, 10)
                DoParameterAdd("@Validate", myrechten.Validate, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function DeleteRechtenGebruiker(ByVal myrechten As Rechten) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("Delete from Rechten where ID = ? ")
                DoParameterAdd("@ID", myrechten.Id, 10)
                i = ExecuteDALScalar(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function UpdateRechtenGebruiker(ByVal myrechten As Rechten) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("update Rechten set ID_Actie=?, ID_Groep=? ,Validate=? where ID = ? ")

                DoParameterAdd("@ID_Actie", myrechten.Actie.Id, 10)
                DoParameterAdd("@ID_Groep", myrechten.Groep.Id, 10)
                DoParameterAdd("@Validate", myrechten.Validate, 10)
                DoParameterAdd("@ID", myrechten.Id, 10)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function
#End Region

    End Class

End Namespace