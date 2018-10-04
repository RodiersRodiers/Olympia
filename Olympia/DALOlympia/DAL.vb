
Imports Olympia.OBJOlympia
Imports System.Data.OleDb
Imports System.Data.Odbc

Namespace DALOlympia

    Public Class DALBase

        Public conn As New OdbcConnection("Driver={MySQL ODBC 5.1 Driver};Server=185.41.126.25;Port=9151;Database=rodierscurato;User=RodiersRodiers;Password=%Roller75!;Option=3;")

        Private myListParamColl As New List(Of OdbcParameter)

        Public Sub DoParameterAdd(ByVal strName As String, ByVal objValue As Object, ByVal myDBType As Integer)
            Dim myparam As OdbcParameter = New OdbcParameter(strName, myDBType)
            If objValue Is Nothing Then
                objValue = ""
            End If
            If myDBType = OdbcType.NVarChar Then
                Dim strObjectValue As String = TryCast(objValue, String)

                strObjectValue = strObjectValue.Replace("<", "&lt;").Replace(">", "&gt;")
                strObjectValue = Filtering.filterOutAsciAbove255(strObjectValue)
                myparam.OdbcType = OdbcType.NVarChar
                myparam.Direction = ParameterDirection.Input
                myparam.Value = strObjectValue
            Else
                myparam.OdbcType = OdbcType.Int
                myparam.Direction = ParameterDirection.Input
                myparam.Value = objValue
            End If

            If myListParamColl Is Nothing Then
                myListParamColl = New List(Of OdbcParameter)
            End If
            myListParamColl.Add(myparam)
        End Sub

        'Public Function doParameterGet(ByVal strParameterName As String) As Object
        '    Dim objValue As Object = Nothing

        '    For Each myParam As SqlParameter In myListParamColl
        '        If myParam.Value = strParameterName Then
        '            If myParam.DbType = SqlDbType.NVarChar Then
        '                objValue = myParam.Value.ToString
        '            Else
        '                objValue = myParam.Value
        '            End If
        '        End If
        '    Next
        '    Return objValue
        'End Function

        Public Sub GenerateDebugSQL(ByVal strSQL As String)
            ' /// Generate full output string
            Dim i As Integer = 0
            Dim strOutput As New Text.StringBuilder
            Dim myParameter As OdbcParameter
            If Not myListParamColl Is Nothing Then
                Dim s() As String = strSQL.Split("?")
                For j As Integer = 0 To s.Count - 2
                    myParameter = returnParameter(i)
                    If myParameter.OdbcType = OdbcType.NVarChar Then
                        strOutput.Append(s(j) & "'" & myParameter.Value & "' ")
                    Else
                        strOutput.Append(s(j) & "" & myParameter.Value & " ")
                    End If
                    i = i + 1
                Next
            Else
                strOutput.Append(strSQL)
            End If
            Debug.WriteLine("SQL -> " & strOutput.ToString)
        End Sub

        Private Function ReturnParameter(ByVal i As Integer) As OdbcParameter
            Dim myReturnParameter As New OdbcParameter
            Dim ii As Integer = 0
            For Each myParameter As OdbcParameter In myListParamColl
                If i = ii Then
                    myReturnParameter = myParameter
                    Exit For
                End If
                ii = ii + 1
            Next
            Return myReturnParameter
        End Function

        Public Function ReturnDALDataTable(ByVal strSQL As String) As DataTable
            Dim mydt As New DataTable
            Dim myCommand As New OdbcCommand(strSQL, conn)
            Dim myDataAdapter As New OdbcDataAdapter(myCommand)

            Try
                If Not myListParamColl Is Nothing Then
                    For Each myParam As OdbcParameter In myListParamColl
                        myCommand.Parameters.Add(myParam)
                    Next
                End If

                GenerateDebugSQL(strSQL)
                conn.Open()
                myDataAdapter.Fill(mydt)
                conn.Close()
            Catch ex As Exception
                Debug.WriteLine(ex.Message & " " & ex.StackTrace)
                Throw
            Finally
            End Try
            If Not myListParamColl Is Nothing Then
                myListParamColl = New List(Of OdbcParameter)
            End If
            Return mydt
        End Function

        Public Function ExecuteDALCommand(ByVal strSQL As String) As Integer
            Dim myCommand As New OdbcCommand(strSQL, conn)
            Dim myDataAdapter As New OdbcDataAdapter(myCommand)
            Dim intResult As Integer = 0
            Try
                If Not myListParamColl Is Nothing Then
                    For Each myParam As OdbcParameter In myListParamColl
                        myCommand.Parameters.Add(myParam)
                    Next
                End If

                generateDebugSQL(strSQL)
                conn.Open()
                intResult = myCommand.ExecuteNonQuery.ToString
                conn.Close()
            Catch ex As Exception
                Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace))
                Throw
            Finally
            End Try
            If Not myListParamColl Is Nothing Then
                myListParamColl = New List(Of OdbcParameter)
            End If
            Return intResult
        End Function

        Public Function ExecuteDALScalar(ByVal strSQL As String) As Integer
            Dim myCommand As New OdbcCommand(strSQL, conn)
            Dim intResult As Integer = 0

            Try
                If Not myListParamColl Is Nothing Then
                    For Each myParam As OdbcParameter In myListParamColl
                        myCommand.Parameters.Add(myParam)
                    Next
                End If

                generateDebugSQL(strSQL)
                conn.Open()
                intResult = myCommand.ExecuteScalar
                conn.Close()
            Catch ex As Exception
                Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace))
                Throw
            Finally
            End Try
            If Not myListParamColl Is Nothing Then
                myListParamColl = New List(Of OdbcParameter)
            End If
            Return intResult
        End Function

        Private Function AttachTransToConn()
            Return conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)
        End Function

        Public Function SetExcelConnectionString(ByVal FileName As String, ByVal Header As UseHeader, ByVal IMEX As ExcelImex)

            Dim Mode As String = CInt(IMEX).ToString
            Dim Builder As New OleDbConnectionStringBuilder With {.DataSource = FileName}
            If IO.Path.GetExtension(FileName).ToUpper = ".XLSX" Then
                Builder.Provider = "Microsoft.ACE.OLEDB.12.0"
                Builder.Add("Extended Properties", "Excel 12.0;IMEX=" & Mode & ";HDR=" & Header.ToString & ";")
            Else
                Builder.Provider = "Microsoft.Jet.OLEDB.4.0"
                Builder.Add("Extended Properties", "Excel 8.0;IMEX=" & Mode & ";HDR=" & Header.ToString & ";")
            End If
            Dim strconnectionExcel As String = Builder.ConnectionString
            Return strconnectionExcel

        End Function

    End Class

End Namespace

