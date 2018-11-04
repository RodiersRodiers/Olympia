
Imports System.IO
Imports System.Security.Cryptography
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace OBJOlympia

    Public Class Tools

        Implements IDisposable

        Public Sub Dispose() Implements IDisposable.Dispose
            If m_des IsNot Nothing Then
                m_des.Dispose()
                m_des = Nothing
            End If
        End Sub

        Public Shared Function ValidEmailAddress(ByVal emailAddress As String, ByRef errorMessage As String) As Boolean
            If emailAddress.Length = 0 Then
                errorMessage = "E-mail Verplicht !"
                Return False
            End If
            If emailAddress.IndexOf("@") > -1 Then
                If (emailAddress.IndexOf(".", emailAddress.IndexOf("@")) > emailAddress.IndexOf("@")) AndAlso emailAddress.Split(".").Length > 0 AndAlso emailAddress.Split(".")(1) <> "" Then
                    errorMessage = ""
                    Return True
                End If
            End If
            errorMessage = "E-mail is niet geldig !"
            Return False
        End Function

        Private Shared Function GetStrResult(ByVal Bytes As Long) As String
            Dim strResult As String
            If (Bytes >= 1073741824) Then
                strResult = String.Format("{0:##.##} GB", Decimal.Divide(Bytes, 1073741824))
            ElseIf (Bytes >= 1048576) Then
                strResult = String.Format("{0:##.##} MB", Decimal.Divide(Bytes, 1048576))
            ElseIf (Bytes >= 1024) Then
                strResult = String.Format("{0:##.##} KB", Decimal.Divide(Bytes, 1024))
            ElseIf (Bytes > 0 And Bytes < 1024) Then
                strResult = String.Format("{0:##.##} Bytes", Decimal.Divide(Bytes, 1073741824))
            Else
                strResult = "0 Bytes"
            End If
            Return strResult
        End Function

        Public Shared Function GetFileSize(ByVal strPathFile As String) As String
            Dim Bytes As Long = New FileInfo(strPathFile).Length
            Dim strResult As String = ""

            strResult = GetStrResult(Bytes)
            Return strResult
        End Function

        Public Shared Sub doUpdateComboBox(ByVal myCB As AjaxControlToolkit.ComboBox, ByVal IntValue As Integer)
            myCB.SelectedIndex = -1
            For Each myListItem As ListItem In myCB.Items
                If IntValue = myListItem.Value Then
                    myListItem.Selected = True
                    Exit For
                End If
            Next
        End Sub

        Public Shared Sub doUpdateComboBoxRefTab(ByVal myCB As AjaxControlToolkit.ComboBox, ByVal strValue As String)
            myCB.SelectedIndex = -1
            For Each myListItem As ListItem In myCB.Items
                If strValue = myListItem.Value Then
                    myListItem.Selected = True
                    Exit For
                End If
            Next
        End Sub

        Private Shared Function ListbItemContains(target As ListBox, item As ListItem) As Boolean
            Dim blnGevonden As Boolean = False
            For Each myListItem As ListItem In target.Items
                If myListItem.Value = item.Value Then
                    blnGevonden = True
                    Exit For
                End If
            Next
            Return blnGevonden
        End Function

        Private Shared Sub doMoveListbItemExtr(ByVal lstbSource As ListBox, ByVal lstbTarget As ListBox, ByVal itemArray As ArrayList, ByRef blnGevonden As Boolean, ByVal Int As Integer)
            If Not ListbItemContains(lstbTarget, (lstbSource.Items(Int))) Then
                lstbTarget.Items.Add(lstbSource.Items(Int))
                itemArray.Add(lstbSource.Items(Int))
            Else
                blnGevonden = True
            End If
        End Sub

        Public Shared Sub doMoveListbItem(ByVal lstbSource As ListBox, ByVal lstbTarget As ListBox)
            If lstbSource.SelectedIndex <> -1 Then
                Dim itemArray As New ArrayList

                Dim blnGevonden As Boolean = False
                For Each Int As Integer In lstbSource.GetSelectedIndices
                    doMoveListbItemExtr(lstbSource, lstbTarget, itemArray, blnGevonden, Int)
                Next

                If Not blnGevonden Then
                    For Each Int As ListItem In itemArray
                        lstbSource.Items.Remove(Int)
                    Next
                End If

            End If
        End Sub

        Public Shared Function convertDatumToDbTimeStamp(ByVal dteIn As DateTime) As String
            Return dteIn.ToString("yyyy-MM-dd HH:mm:ss")
        End Function


        Public Shared Function doRemoveVBRLFs(ByVal strTekst As String) As String
            '//doRemoveVBRLFs
            Return strTekst.Replace(vbCrLf, "$§$")
        End Function

        Public Shared Function doSetupVBRLFs(ByVal strTekst As String) As String
            '//doSetupVBRLFs
            Return strTekst.Replace("$§$", vbCrLf)
        End Function

#Region "Initializing"

        ''' <returns></returns>
        Public Shared Function getEmptyDate() As Date
            Return New Date(1900, 1, 1)
        End Function

        Public Shared Function doChacheDate(ByVal mydate As Date) As String
            Return mydate.ToString("yyyy-MM-dd")
        End Function

        Public Shared Function doChacheDateTime(ByVal mydate As Date) As String
            Return mydate.ToString("yyyy-MM-dd HH:mm:ss")
        End Function

#End Region

#Region "GetDatum"

        Public Shared Function getDatum(ByVal strDatum As String, ByVal strUur As String, ByVal dtEmpty As Date) As Date
            Try
                Dim dt As New Date
                If strDatum <> "" Then
                    dt = CType(strDatum, Date)
                    dt = getDatumExtracted(strUur, dt)
                Else
                    dt = dtEmpty
                End If

                Return dt
            Catch ex As Exception
                Throw
            End Try
        End Function

        Private Shared Sub getDatumExtractedExtracted(ByVal intHH As Integer, ByVal intMM As Integer)
            If (intHH > 24 Or intHH < 0) Then
                Throw New Exception("Error_UUR1")
            End If
            If (intMM > 59 Or intMM < 0) Then
                Throw New Exception("Error_MIN1")
            End If
        End Sub


        Private Shared Function getDatumExtracted(ByVal strUur As String, ByRef dt As Date) As Date
            If (strUur = "" Or strUur = "__:__:__" Or strUur = "__:__") Then
                dt.AddTicks(0)
            Else
                Dim str() As String = strUur.Split(":")
                Dim intHH As Integer = CType(str(0), Integer)
                Dim intMM As Integer = CType(str(1), Integer)

                getDatumExtractedExtracted(intHH, intMM)

                dt = getDatum(dt, str)
            End If
            Return dt
        End Function

        Private Shared Function getDatum(ByRef dt As Date, ByVal myStr As String()) As Date
            Try
                dt = dt.AddHours(CType(myStr(0), Double))
                dt = dt.AddMinutes(CType(myStr(1), Double))

                If myStr.Count = 3 Then
                    ' ==== Kan zijn dat er ook met seconden wordt gewerkt !! bv bij Vatting > Gebeurtenissen
                    dt = dt.AddSeconds(CType(myStr(2), Double))
                End If
                Return getDatumExtr_Minutes(dt, myStr)
            Catch ex As Exception
                Throw New Exception("Error_UUR2")
            End Try
        End Function

        Private Shared Function getDatumExtr_Minutes(ByRef dt As Date, ByVal myStr As String()) As Date
            If (myStr(0) = "00" Or myStr(0) = "24") And myStr(1) = "00" And dt <> getEmptyDate() Then
                dt = dt.AddMinutes(1)
            End If
            Return dt
        End Function

#End Region

#Region "ImageConverting"

        Public Shared Function convertImage(ByVal myRealImage As System.Drawing.Image) As Bitmap
            If myRealImage Is Nothing Then
                Return Nothing
            End If

            Dim intW As Integer = myRealImage.Width
            Dim intH As Integer = myRealImage.Height

            Dim myNewBitmap As New Bitmap(intW, intH, PixelFormat.Format32bppPArgb)
            Using canvas As Graphics = Graphics.FromImage(myNewBitmap)
                canvas.InterpolationMode = Drawing2D.InterpolationMode.High
                canvas.DrawImageUnscaled(myRealImage, 0, 0, intW, intH)
            End Using

            myRealImage.Dispose()

            Return myNewBitmap
        End Function

#End Region

#Region "DES (Encryption/Decrypting)"


        ' define the triple des provider
        Private m_des As New TripleDESCryptoServiceProvider

        ' define the string handler
        Private m_utf8 As New UTF8Encoding

        Private ReadOnly m_key() As Byte = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24}
        Private ReadOnly m_iv() As Byte = {8, 7, 6, 5, 4, 3, 2, 1}

        Public Function Encrypt(ByVal input() As Byte) As Byte()
            Return Transform(input, m_des.CreateEncryptor(m_key, m_iv))
        End Function

        Public Function Decrypt(ByVal input() As Byte) As Byte()
            Return Transform(input, m_des.CreateDecryptor(m_key, m_iv))
        End Function

        Public Function Encrypt(ByVal text As String) As String
            Dim strReturn As String = ""
            Try
                If text <> "" Then
                    Dim input() As Byte = m_utf8.GetBytes(text)
                    Dim output() As Byte = Transform(input, _
                                    m_des.CreateEncryptor(m_key, m_iv))

                    strReturn = Convert.ToBase64String(output)

                End If
            Catch ex As Exception
                Throw
            End Try
            Return strReturn
        End Function

        Public Function Decrypt(ByVal text As String) As String
            If text <> "" Then
                Dim input() As Byte = Convert.FromBase64String(text)
                Dim output() As Byte = Transform(input, m_des.CreateDecryptor(m_key, m_iv))
                Return m_utf8.GetString(output)
            Else
                Return ""
            End If
        End Function

        Private Shared Function Transform(ByVal input() As Byte, ByVal CryptoTransform As ICryptoTransform) As Byte()
            ' create the necessary streams
            Dim memStream As MemoryStream = New MemoryStream
            Dim cryptStream As CryptoStream = New  _
                CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write)
            ' transform the bytes as requested
            cryptStream.Write(input, 0, input.Length)
            cryptStream.FlushFinalBlock()
            ' Read the memory stream and convert it back into byte array
            memStream.Position = 0
            Dim result(CType(memStream.Length - 1, Int32)) As Byte
            memStream.Read(result, 0, CType(result.Length, Int32))
            ' close and release the streams
            memStream.Close()
            cryptStream.Close()
            ' hand back the encrypted buffer
            Return result
        End Function
#End Region

    End Class

End Namespace
