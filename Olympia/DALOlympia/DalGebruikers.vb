Imports Olympia.OBJOlympia
Imports System.Data.OleDb
Imports MySql.Data.MySqlClient

Namespace DALOlympia

    Public Class DalGebruikers : Inherits DALBase
        Private strSQL As New StringBuilder
        Dim i As Integer = 0

        Public Shared Function ImportExceltoDatatable(filepath As String) As DataTable
            ' string sqlquery= "Select * From [SheetName$] Where YourCondition";
            Dim dt As New DataTable
            Try
                Dim ds As New DataSet()
                Dim constring As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filepath & ";Extended Properties=""Excel 12.0;HDR=YES;"""
                Dim con As New OleDbConnection(constring & "")

                con.Open()

                Dim myTableName = con.GetSchema("Tables").Rows(0)("TABLE_NAME")
                Dim sqlquery As String = String.Format("SELECT * FROM [{0}]", myTableName) ' "Select * From " & myTableName  
                Dim da As New OleDbDataAdapter(sqlquery, con)
                da.Fill(ds)
                dt = ds.Tables(0)
                Return dt
            Catch ex As Exception
                MsgBox(Err.Description, MsgBoxStyle.Critical)
                Return dt
            End Try
        End Function


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
                DoParameterAdd("@Email", mygebruiker.Email, MySqlDbType.String)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function Changepw(ByVal mygebruiker As Gebruikers) As Integer
            strSQL.Remove(0, strSQL.Length)
            strSQL.Append("update Gebruikers set password = ? where ID_Lid = ? ")
            Try
                DoParameterAdd("@Paswoord", mygebruiker.Paswoord, MySqlDbType.String)
                DoParameterAdd("@IdLid", mygebruiker.IdLid, MySqlDbType.Int32)
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
                DoParameterAdd("@IdLid", mygebruiker.IdLid, MySqlDbType.Int32)
                DoParameterAdd("@Paswoord", mygebruiker.Paswoord, MySqlDbType.String)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

#Region "Logging"

        Public Function InsertLogging(ByVal myLogging As Logging) As Integer
            strSQL.Remove(0, strSQL.Length)
            strSQL.Append("INSERT into logging (Datum, ID_Lid, Event, type) VALUES (now(), ?, ?, ?) ")

            Try
                DoParameterAdd("@ID_Lid", myLogging.Gebruiker.IdLid, MySqlDbType.Int32)
                DoParameterAdd("@EventLogging", myLogging.EventLogging, MySqlDbType.String)
                DoParameterAdd("@Type", myLogging.Type, MySqlDbType.Int32)

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
                    DoParameterAdd("@intType", intType, MySqlDbType.Int32)
                End If

                If Not strInfo = "" Then
                    strSQL.Append(" AND ( Event LIKE ? ")
                    DoParameterAdd("@strInfo", "%" & strInfo & "%", MySqlDbType.String)
                    strSQL.Append(" or naam LIKE ? ")
                    DoParameterAdd("@naam", "%" & strInfo & "%", MySqlDbType.String)
                    strSQL.Append(" or voornaam LIKE ? ) ")
                    DoParameterAdd("@voornaam", "%" & strInfo & "%", MySqlDbType.String)
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
                DoParameterAdd("@idlid", ID_Lid, MySqlDbType.Int32)

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

        Public Function GetGebruikersOpenHandeling(ByVal sort As String, ByVal strfilter As String, ByVal intopen As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from Gebruikers g ")
                strSQL.Append("left join handelingen h on h.ID_Lid = g.ID_Lid ")
                strSQL.Append("where 1=1 ")
                If intopen = -1 Then
                    strSQL.Append(" AND valid = 0 ")
                End If
                If Not strfilter Like "" Then
                    strSQL.Append(" AND (naam like ? or voornaam like ? ")
                    DoParameterAdd("@naam", "%" & strfilter & "%", MySqlDbType.String)
                    DoParameterAdd("@voornaam", "%" & strfilter & "%", MySqlDbType.String)
                End If
                strSQL.Append("group by g.ID_Lid, valid ")
                strSQL.Append("order by " & sort)
                mydt = ReturnDALDataTable(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function GetGebruikers(ByVal sort As String, ByVal strfilter As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from Gebruikers where active=1 ")
                If Not strfilter Like "" Then
                    strSQL.Append(" AND (naam like ? or voornaam like ? ")
                    DoParameterAdd("@naam", "%" & strfilter & "%", MySqlDbType.String)
                    DoParameterAdd("@voornaam", "%" & strfilter & "%", MySqlDbType.String)
                End If
                strSQL.Append(" order by " & sort)
                mydt = ReturnDALDataTable(strSQL.ToString)
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
                DoParameterAdd("@Email", mygebruiker.Email, MySqlDbType.String)
                DoParameterAdd("@Naam", mygebruiker.Naam, MySqlDbType.String)
                DoParameterAdd("@Voornaam", mygebruiker.Voornaam, MySqlDbType.String)
                DoParameterAdd("@GebDatum", mygebruiker.GebDatum, MySqlDbType.Date)
                DoParameterAdd("@geslacht", mygebruiker.Geslacht, MySqlDbType.String)
                DoParameterAdd("@gsm", mygebruiker.GSM, MySqlDbType.String)
                DoParameterAdd("@Paswoord", mygebruiker.Paswoord, MySqlDbType.String)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@idlid", ID_Lid, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function UpdateGebruiker(ByVal mygebruiker As Gebruikers) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("update gebruikers set email=?, naam=?,voornaam=?,gemeente=?,postcode=?,gsm=?,straat=?,Huisnr=?,Rekeningnr=?,info=?,gebdatum=?,geslacht= ? where ID_lid = ? ")
                DoParameterAdd("@Email", mygebruiker.Email, MySqlDbType.String)
                DoParameterAdd("@naam", mygebruiker.Naam, MySqlDbType.String)
                DoParameterAdd("@voornaam", mygebruiker.Voornaam, MySqlDbType.String)
                DoParameterAdd("@gemeente", mygebruiker.Gemeente, MySqlDbType.String)
                DoParameterAdd("@postcode", mygebruiker.Postcode, MySqlDbType.String)
                DoParameterAdd("@gsm", mygebruiker.GSM, MySqlDbType.String)
                DoParameterAdd("@straat", mygebruiker.Straat, MySqlDbType.String)
                DoParameterAdd("@huisnummer", mygebruiker.Huisnr, MySqlDbType.String)
                DoParameterAdd("@rekeningnummer", mygebruiker.Rekeningnummer, MySqlDbType.String)
                DoParameterAdd("@info", mygebruiker.Info, MySqlDbType.String)
                DoParameterAdd("@gebdatum", mygebruiker.GebDatum, MySqlDbType.Date)
                DoParameterAdd("@geslacht", mygebruiker.Geslacht, MySqlDbType.String)
                DoParameterAdd("@ID_lid", mygebruiker.IdLid, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function CheckGebruiker(ByVal Naam As String, ByVal Voornaam As String, ByVal gebDatum As Date) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("select * from gebruikers where naam = ? AND voornaam = ? AND gebdatum= ? ")
                DoParameterAdd("@naam", Naam, MySqlDbType.String)
                DoParameterAdd("@voornaam", Voornaam, MySqlDbType.String)
                DoParameterAdd("@gebdatum", gebDatum, MySqlDbType.String)

                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function CheckToegangGebruiker(ByVal ID_Lid As Integer, ByVal pagina As Integer) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("select Validate from Rechten where ID_Lid = ? AND ID_actie = ?")
                DoParameterAdd("@idlid", ID_Lid, MySqlDbType.Int32)
                DoParameterAdd("@IDActie", pagina, MySqlDbType.Int32)
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
                DoParameterAdd("@idlid", ID_Lid, MySqlDbType.Int32)

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
        Public Function GetActieIdbyBeschrijving(ByVal strbeschrijving As String) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT ID from PIC_Acties where beschrijving = ? ")
                DoParameterAdd("@strbeschrijving", strbeschrijving, MySqlDbType.String)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function
        Public Function GetActiesWedJur(ByVal sort As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT * from PIC_Acties where id = 11 or id = 12 order by " & sort)

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

        Public Function GetDisciplineIdbyBeschrijving(ByVal strbeschrijving As String) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT ID from PIC_Disciplines where active=1 and beschrijving = ? ")
                DoParameterAdd("@strbeschrijving", strbeschrijving, MySqlDbType.String)
                i = ExecuteDALScalar(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function


        Public Function GetDisciplinebyGroep(ByVal idgroep As Integer) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT ID_Discipline from PIC_trainingsgroepen where active=1 and ID = ? ")
                DoParameterAdd("@ID", idgroep, MySqlDbType.Int32)
                i = ExecuteDALScalar(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

        Public Function InsertDiscipline(ByVal myDiscipline As pic_Disciplines) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("INSERT INTO PIC_Disciplines (Beschrijving) values (?) ")
                DoParameterAdd("@Beschrijving", myDiscipline.beschrijving, MySqlDbType.String)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@Beschrijving", myDiscipline.beschrijving, MySqlDbType.String)
                DoParameterAdd("@ID_Discipline", myDiscipline.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@ID_Discipline", myDiscipline.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@myIdDiscipline", myIdDiscipline, MySqlDbType.Int32)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function GetTrainingsgroepen(ByVal sort As String) As DataTable
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
                DoParameterAdd("@Beschrijving", myTrainingsGroep.beschrijving, MySqlDbType.String)
                DoParameterAdd("@ID_Discipline", myTrainingsGroep.Discipline.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@Beschrijving", myTrainingsGroep.beschrijving, MySqlDbType.String)
                DoParameterAdd("@ID_Groep", myTrainingsGroep.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@ID_Groep", myTrainingsGroep.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@IDGROEP", idGroep, MySqlDbType.Int32)

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
                DoParameterAdd("@IDGROEP", idGroep, MySqlDbType.Int32)
                If Not strdatum Like "" Then
                    strSQL.Append("AND datum like @strdatum) ")
                    DoParameterAdd("@strdatum", strdatum, MySqlDbType.Date)
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
                DoParameterAdd("@ID_Lid", myAanwezigheid.Gebruiker.IdLid, MySqlDbType.Int32)
                DoParameterAdd("@Datum", myAanwezigheid.Datum, MySqlDbType.Date)
                DoParameterAdd("@ID_Groep", myAanwezigheid.Groep.Id, MySqlDbType.Int32)
                DoParameterAdd("@Opmerking", myAanwezigheid.Opmerking, MySqlDbType.String)
                DoParameterAdd("@aanwezig", myAanwezigheid.Aanwezig, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@opmerking", myAanwezigheid.Opmerking, MySqlDbType.String)
                DoParameterAdd("@Aanwezig", myAanwezigheid.Aanwezig, MySqlDbType.Int32)
                DoParameterAdd("@ID", myAanwezigheid.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@ID", myAanwezigheid.Id, MySqlDbType.Int32)

                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return i
        End Function

#End Region

#Region "Handelingen"

        Public Function GetRapportAllhandelingenbygebruiker(ByVal idlid As Integer, ByVal datumlaag As Date, ByVal datumhoog As Date) As DataTable
            Dim mydt As New DataTable

            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT km, v.bedrag as Bedraglg, H.ID, dagvm,dagnm,dagav,valid,h.bedrag,h.Datum,D.id as disciplineid, D.beschrijving as disciplinebeschrijving, A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid, G.naam, G.voornaam, h.info, H.aantal, ")
                strSQL.Append("T.id as groepid, t.beschrijving as groepbeschrijving ")
                strSQL.Append("from Handelingen H ")
                strSQL.Append("join gebruikers G on H.id_lid=G.id_lid ")
                strSQL.Append("join vergoedingen V on G.id_lid = V.id_Lid ")
                strSQL.Append("left join PIC_Disciplines D on D.id = H.id_Discipline ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=H.id_groep ")
                strSQL.Append("left join PIC_acties A on A.id=H.id_actie ")
                strSQL.Append("where h.id_lid = ? ")
                strSQL.Append("And ( h.datum between ? and ? ) ")
                DoParameterAdd("@idlid", idlid, MySqlDbType.Int32)
                DoParameterAdd("@datumlaag", datumlaag.ToString("yyyy-MM-dd"), MySqlDbType.String)
                DoParameterAdd("@datumhoog", datumhoog.ToString("yyyy-MM-dd"), MySqlDbType.String)

                mydt = ReturnDALDataTable(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function GetAllhandelingenbygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String, ByVal datumlaag As Date, ByVal datumhoog As Date) As DataTable
            Dim mydt As New DataTable
            Dim dtEmptyDate As Date = ConvertDatumToDbTimeStamp(GetEmptyDate)
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT H.ID, valid,h.bedrag,h.Datum,D.id as disciplineid, D.beschrijving as disciplinebeschrijving, A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid, G.naam, G.voornaam, h.info, H.aantal, ")
                strSQL.Append("T.id as groepid, t.beschrijving as groepbeschrijving ")
                strSQL.Append("from Handelingen H ")
                strSQL.Append("join gebruikers G on H.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Disciplines D on D.id = H.id_Discipline ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=H.id_groep ")
                strSQL.Append("left join PIC_acties A on A.id=H.id_actie ")
                strSQL.Append("where h.id_lid = ? ")
                DoParameterAdd("@idlid", idlid, MySqlDbType.Int32)

                If Not datumlaag = dtEmptyDate Then
                    strSQL.Append("And ( h.datum between ? and ? )   ")
                    DoParameterAdd("@datumlaag", datumlaag.ToString("yyyy-MM-dd"), MySqlDbType.String)
                    DoParameterAdd("@datumhoog", datumhoog.ToString("yyyy-MM-dd"), MySqlDbType.String)
                End If

                If Not strfilter Like "" Then
                    strSQL.Append(" AND h.info like ? ")
                    DoParameterAdd("@info", "%" & strfilter & "%", MySqlDbType.String)
                End If
                strSQL.Append("order by " & sort)

                mydt = ReturnDALDataTable(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function Gethandelingbygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT dagvm, dagnm, dagav, H.ID, valid,h.bedrag,h.Datum,D.id as disciplineid, D.beschrijving as disciplinebeschrijving,T.id as groepid,t.beschrijving as groepbeschrijving, A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid, G.naam, G.voornaam, h.info, H.aantal ")
                strSQL.Append("from Handelingen H ")
                strSQL.Append("join gebruikers G on H.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=H.id_groep ")
                strSQL.Append("left join PIC_Disciplines D on D.id = H.id_Discipline ")
                strSQL.Append("left join PIC_acties A on A.id=H.id_actie ")
                strSQL.Append("where 1=1  ")
                strSQL.Append("AND h.id_lid = ? ")
                DoParameterAdd("@idlid", idlid, MySqlDbType.Int32)
                If Not strfilter Like "" Then
                    strSQL.Append(" AND h.info like ? ")
                    DoParameterAdd("@info", "%" & strfilter & "%", MySqlDbType.String)
                End If

                strSQL.Append("order by " & sort)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function GethandelingWedJurybygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT dagvm, dagnm, dagav, H.ID, valid,h.bedrag,h.Datum,D.id as disciplineid, D.beschrijving as disciplinebeschrijving,T.id as groepid,t.beschrijving as groepbeschrijving, A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid, G.naam, G.voornaam, h.info, H.aantal ")
                strSQL.Append("from Handelingen H ")
                strSQL.Append("join gebruikers G on H.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=H.id_groep ")
                strSQL.Append("left join PIC_Disciplines D on D.id = H.id_Discipline ")
                strSQL.Append("left join PIC_acties A on A.id=H.id_actie ")
                strSQL.Append("where 1=1 AND (h.ID_actie = 11 or H.ID_actie =12) ")
                strSQL.Append("AND h.id_lid = ? ")
                DoParameterAdd("@idlid", idlid, MySqlDbType.Int32)
                If Not strfilter Like "" Then
                    strSQL.Append(" AND h.info like ? ")
                    DoParameterAdd("@info", "%" & strfilter & "%", MySqlDbType.String)
                End If

                strSQL.Append("order by " & sort)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function

        Public Function GethandelingVerplaatsingbygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT dagvm, dagnm, dagav, H.ID, valid,h.bedrag,h.Datum,D.id as disciplineid, D.beschrijving as disciplinebeschrijving,T.id as groepid,t.beschrijving as groepbeschrijving, A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid, G.naam, G.voornaam, h.info, H.aantal ")
                strSQL.Append("from Handelingen H ")
                strSQL.Append("join gebruikers G on H.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=H.id_groep ")
                strSQL.Append("left join PIC_Disciplines D on D.id = H.id_Discipline ")
                strSQL.Append("left join PIC_acties A on A.id=H.id_actie ")
                strSQL.Append("where 1=1 AND h.ID_actie = 13 ")
                strSQL.Append("AND h.id_lid = ? ")
                DoParameterAdd("@idlid", idlid, MySqlDbType.Int32)
                If Not strfilter Like "" Then
                    strSQL.Append(" AND h.info like ? ")
                    DoParameterAdd("@info", "%" & strfilter & "%", MySqlDbType.String)
                End If

                strSQL.Append("order by " & sort)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function
        Public Function GetHandelingAnderebygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT H.ID, valid,h.bedrag,h.Datum,D.id as disciplineid, D.beschrijving as disciplinebeschrijving,T.id as groepid,t.beschrijving as groepbeschrijving, A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid, G.naam, G.voornaam, h.info, H.aantal ")
                strSQL.Append("from Handelingen H ")
                strSQL.Append("join gebruikers G on H.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=H.id_groep ")
                strSQL.Append("left join PIC_Disciplines D on D.id = H.id_Discipline ")
                strSQL.Append("left join PIC_acties A on A.id=H.id_actie ")
                strSQL.Append("where 1=1  ")
                strSQL.Append("AND h.id_lid = ? and H.id_actie = 14 ")
                DoParameterAdd("@idlid", idlid, MySqlDbType.Int32)
                If Not strfilter Like "" Then
                    strSQL.Append(" AND h.info like ? ")
                    DoParameterAdd("@info", "%" & strfilter & "%", MySqlDbType.String)
                End If

                strSQL.Append("order by " & sort)
                mydt = ReturnDALDataTable(strSQL.ToString)

            Catch ex As Exception
                Throw
            End Try

            Return mydt
        End Function
        Public Function GetHandelingLesgeverVergoedingbygebruiker(ByVal sort As String, ByVal idlid As Integer, ByVal strfilter As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT H.ID, valid,h.bedrag,h.Datum,D.id as disciplineid, D.beschrijving as disciplinebeschrijving,T.id as groepid,t.beschrijving as groepbeschrijving, A.id as actieid, A.beschrijving as actiebeschrijving,G.ID_Lid as gebruikerid, G.naam, G.voornaam, h.info, H.aantal ")
                strSQL.Append("from Handelingen H ")
                strSQL.Append("join gebruikers G on H.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=H.id_groep ")
                strSQL.Append("left join PIC_Disciplines D on D.id = H.id_Discipline ")
                strSQL.Append("left join PIC_acties A on A.id=H.id_actie ")
                strSQL.Append("where 1=1  ")
                strSQL.Append("AND h.id_lid = ? and H.id_actie = 15 ")
                DoParameterAdd("@idlid", idlid, MySqlDbType.Int32)
                If Not strfilter Like "" Then
                    strSQL.Append(" AND h.info like ? ")
                    DoParameterAdd("@info", "%" & strfilter & "%", MySqlDbType.String)
                End If

                strSQL.Append("order by " & sort)
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
                strSQL.Append("INSERT INTO Handelingen (ID_Lid, Datum, ID_Groep,ID_Discipline, ID_Actie, Info, Aantal, dagvm, dagnm, dagav) values (?,?,?,?,?,?,?,?,?,?)")
                DoParameterAdd("@ID_Lid", myHandeling.Gebruiker.IdLid, MySqlDbType.Int32)
                DoParameterAdd("@Datum", myHandeling.Datum, MySqlDbType.Date)
                DoParameterAdd("@ID_Groep", myHandeling.Groep.Id, MySqlDbType.Int32)
                DoParameterAdd("@ID_Discipline", myHandeling.Discipline.Id, MySqlDbType.Int32)
                DoParameterAdd("@ID_Actie", myHandeling.Actie.Id, MySqlDbType.Int32)
                DoParameterAdd("@Info", myHandeling.Info, MySqlDbType.String)
                DoParameterAdd("@Aantal", myHandeling.Aantal, MySqlDbType.String)
                DoParameterAdd("@dagvm", myHandeling.dagVM, MySqlDbType.Int32)
                DoParameterAdd("@dagnm", myHandeling.dagNM, MySqlDbType.Int32)
                DoParameterAdd("@dagav", myHandeling.dagAV, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function UpdateHandeling(ByVal myHandeling As Handelingen) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("UPDATE Handelingen SET Datum = ?, ID_Discipline = ? ,ID_Groep= ? ,Info= ? , aantal= ?, dagvm=?,dagnm=?,dagav=?, valid=? where ID = ? ")
                DoParameterAdd("@Datum", myHandeling.Datum, MySqlDbType.Date)
                DoParameterAdd("@ID_Discipline", myHandeling.Discipline.Id, MySqlDbType.Int32)
                DoParameterAdd("@ID_Groep", myHandeling.Groep.Id, MySqlDbType.Int32)
                DoParameterAdd("@Info", myHandeling.Info, MySqlDbType.String)
                DoParameterAdd("@Aantal", myHandeling.Aantal, MySqlDbType.String)
                DoParameterAdd("@dagvm", myHandeling.dagVM, MySqlDbType.Int32)
                DoParameterAdd("@dagnm", myHandeling.dagNM, MySqlDbType.Int32)
                DoParameterAdd("@dagav", myHandeling.dagAV, MySqlDbType.Int32)
                DoParameterAdd("@validate", myHandeling.Validate, MySqlDbType.Int32)
                DoParameterAdd("@ID", myHandeling.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)

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
                DoParameterAdd("@ID_Handeling", myHandeling.Id, MySqlDbType.Int32)

                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@idlid", idlid, MySqlDbType.Int32)
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
                DoParameterAdd("@ID_Lid", myrechten.Gebruiker.IdLid, MySqlDbType.Int32)
                DoParameterAdd("@ID_Actie", myrechten.Actie.Id, MySqlDbType.Int32)
                DoParameterAdd("@ID_Groep", myrechten.Groep.Id, MySqlDbType.Int32)
                DoParameterAdd("@Validate", myrechten.Validate, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
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
                DoParameterAdd("@ID", myrechten.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
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

                DoParameterAdd("@ID_Actie", myrechten.Actie.Id, MySqlDbType.Int32)
                DoParameterAdd("@ID_Groep", myrechten.Groep.Id, MySqlDbType.Int32)
                DoParameterAdd("@Validate", myrechten.Validate, MySqlDbType.Int32)
                DoParameterAdd("@ID", myrechten.Id, 10)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function
#End Region

#Region "vergoedingen"


        Public Function GetLesgeverVergoeding(ByVal sort As String) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT naam, voornaam, gebdatum,r.bedrag,r.datum, T.id as groepid,T.beschrijving as groepbeschrijving, ")
                strSQL.Append("G.ID_Lid as gebruikerid  ")
                strSQL.Append("from vergoedingen r")
                strSQL.Append("left join gebruikers G on R.id_lid=G.id_lid ")
                strSQL.Append("left join PIC_Trainingsgroepen T on T.id=R.id_groep ")
                mydt = ReturnDALDataTable(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return mydt
        End Function

        Public Function InsertLesgeverVergoeding(ByVal myvergoeding As Lesgeververgoeding) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("INSERT into vergoedingen (ID_Lid, datum, bedrag) VALUES (?,?,?)")
                DoParameterAdd("@ID_Lid", myvergoeding.Gebruiker.IdLid, MySqlDbType.Int32)
                DoParameterAdd("@datum", myvergoeding.Datum, MySqlDbType.Date)
                DoParameterAdd("@bedrag", myvergoeding.Bedrag, MySqlDbType.Decimal)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function UpdateLesgeverVergoeding(ByVal myvergoeding As Lesgeververgoeding) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("Update vergoedingen set ID_Lid=?, datum=?, bedrag=? where id = ?")
                DoParameterAdd("@ID_Lid", myvergoeding.Gebruiker.IdLid, MySqlDbType.Int32)
                DoParameterAdd("@datum", myvergoeding.Datum, MySqlDbType.Date)
                DoParameterAdd("@bedrag", myvergoeding.Bedrag, MySqlDbType.Decimal)
                DoParameterAdd("@ID", myvergoeding.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function DeleteLesgeverVergoeding(ByVal myvergoeding As Lesgeververgoeding) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("Delete from vergoedingen where id = ?")
                DoParameterAdd("@ID", myvergoeding.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

#End Region
#Region "Boodschappen"

        Public Function GetBoodschappenByLid(ByVal sort As String, ByVal idlid As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT datum, zender, ontvanger, inhoud, gelezen  ")
                strSQL.Append("from Boodschappen B ")
                strSQL.Append("left join gebruikers G on B.ontvanger=G.id_lid ")
                strSQL.Append("where B.ontvanger = ? ")
                DoParameterAdd("@idlid", idlid, MySqlDbType.Int32)
                mydt = ReturnDALDataTable(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return mydt
        End Function

        Public Function GetBoodschappenById(ByVal id As Integer) As DataTable
            Dim mydt As New DataTable
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("SELECT naam, voornaam,  ")
                strSQL.Append("G.ID_Lid as gebruikerid from Boodschappen R ")
                strSQL.Append("left join gebruikers G on R.id_lid=G.id_lid ")

                strSQL.Append("where R.ID_Lid = ? ")
                DoParameterAdd("@idlid", id, MySqlDbType.Int32)
                mydt = ReturnDALDataTable(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return mydt
        End Function

        Public Function InsertBoodschap(ByVal myboodschap As Boodschappen) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("INSERT into Boodschappen (datum,zender,ontvanger,inhoud) VALUES (?,?,?,?)")
                DoParameterAdd("@datum", myboodschap.Datum, MySqlDbType.DateTime)
                DoParameterAdd("@zender", myboodschap.Zender.IdLid, MySqlDbType.Int32)
                DoParameterAdd("@ontvanger", myboodschap.Ontvanger.IdLid, MySqlDbType.Int32)
                DoParameterAdd("@inhoud", myboodschap.Inhoud, MySqlDbType.VarChar)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function DeleteBoodschap(ByVal myboodschap As Integer) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("Delete from Boodschappen where ID = ? ")
                DoParameterAdd("@ID", myboodschap, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function

        Public Function UpdateBoodschap(ByVal myboodschap As Boodschappen) As Integer
            Dim i As Integer = 0
            Try
                strSQL.Remove(0, strSQL.Length)
                strSQL.Append("update Boodschappen set inhoud=?, gelezen=? where ID = ? ")

                DoParameterAdd("@inhoud", myboodschap.Inhoud, MySqlDbType.Int32)
                DoParameterAdd("@gelezen", myboodschap.gelezen, MySqlDbType.Int32)
                DoParameterAdd("@ID", myboodschap.Id, MySqlDbType.Int32)
                i = ExecuteDALCommand(strSQL.ToString)
            Catch ex As Exception
                Throw
            End Try
            Return i
        End Function
#End Region

    End Class

End Namespace