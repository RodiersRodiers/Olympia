
Namespace OBJOlympia

    Public Class Filtering

        Public Shared Function filterOutAsciAbove255(ByVal strSource As String) As String
            '// filterOutAsciAbove255
            If strSource = "" Then
                Return ""
            Else
                Dim strNewString As New System.Text.StringBuilder()
                Dim dataArray() As Char = strSource.ToCharArray
                filterOutAsciAbove255Extracted(strSource, strNewString, dataArray)
                Return strNewString.ToString
            End If
        End Function

        '// filterOutAsciAbove255Extracted
        Private Shared Sub filterOutAsciAbove255Extracted(ByVal strSource As String, ByVal strNewString As System.Text.StringBuilder, ByVal dataArray As Char())
            For i As Integer = 0 To dataArray.Length - 1 Step 1
                Dim myInt As Integer = Convert.ToInt32(dataArray(i))

                Conversies(myInt)
                If (myInt <= 255 And myInt >= 0) Then
                    strNewString.Append(strSource(i))
                Else
                    filterOutSelectCase(strNewString, myInt)
                End If
            Next
        End Sub

        Private Shared Sub Conversies(ByRef myInt As Integer)
            'Conversies           
            Select Case True
                Case myInt = 8217 ' For conversion of second reversed ' to a normal ' (come with e.g. WORD)
                    myInt = 39
                Case myInt = 8364 ' eurosign unicode
                    myInt = 128
            End Select
        End Sub

        Private Shared Sub filterOutSelectCase(ByVal strNewString As System.Text.StringBuilder, ByVal myInt As Integer)
            Select Case myInt
                Case 8216  ' Filtering out the ' from WORD (more can be added)
                    strNewString.Append("''")
                Case Else
                    strNewString.Append("?")
            End Select
        End Sub

        Public Shared Function doRemoveVBRLFs(ByVal strTekst As String) As String
            '//doRemoveVBRLFs
            Return strTekst.Replace(vbCrLf, "$§$")
        End Function

        Public Shared Function doSetupVBRLFs(ByVal strTekst As String) As String
            '//doSetupVBRLFs
            Return strTekst.Replace("$§$", vbCrLf)
        End Function

        Public Shared Function escape_magic_quotes(ByVal strUnescaped As String) As String
            '//escape_magic_quotes
            If strUnescaped <> "" Then
                Return strUnescaped.Replace("'", "''")
            Else
                Return ""
            End If
        End Function

    End Class

End Namespace
