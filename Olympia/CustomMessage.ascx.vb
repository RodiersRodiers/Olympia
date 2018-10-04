
Imports Olympia.OBJOlympia
Imports Olympia.BALOlympia


Partial Class CustomMessage
    Inherits UserControl

    Enum TypeMessage
        Fataal = 1
        Fout = 2
        Waarschuwing = 3
        Bevestiging = 4
        Info = 5
    End Enum

    ''' <summary>
    ''' Handles the Load event of the Page control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'TBLMessage.Visible = False

        If IsPostBack Then
            pnlMessage.Visible = False
            'pnlMessage.Style.Add("display", "none")
            setMultilanguages()
        End If
    End Sub

    ''' <summary>
    ''' Sets the multilanguages.
    ''' </summary>
    Private Sub setMultiLanguages() ' Fill up the object according to the language
        imgMsgLeft.ImageUrl = "images/msgLeft.gif"
        imgMsgRight.ImageUrl = "images/msgRight.gif"
    End Sub

    ''' <summary>
    ''' Sets the message.
    ''' </summary>
    ''' <param name="strMyMessage">The string my message.</param>
    ''' <param name="mytypeMessage">The mytype message.</param>
    ''' <param name="myException">My exception.</param>
    ''' <param name="stayVisible">if set to <c>true</c> [stay visible].</param>
    Public Sub setMessage(ByVal strMyMessage As String, ByVal mytypeMessage As TypeMessage, ByVal myException As Exception, Optional ByVal stayVisible As Boolean = False)
        If Not strMyMessage = "" Then

            pnlMessage.Visible = True
            lblMessage.Text = strMyMessage
            Select Case mytypeMessage
                Case TypeMessage.Fataal
                    imgMessage.ImageUrl = "images/error.gif"
                    imgMessage.ToolTip = "Een fatale fout heeft zich voorgedaan."
                    lblMessage.CssClass = "MsgNegative"
                    'LOGGING
                    If myException.Message <> "VALIDATION" Then
                        sendLogging(String.Format("{0}<BR>{1}", myException.Message, Left(myException.StackTrace, 5000)))
                    End If
                Case TypeMessage.Fout
                    imgMessage.ImageUrl = "images/error.gif"
                    imgMessage.ToolTip = "Een fout heeft zich voorgedaan."
                    lblMessage.CssClass = "MsgNegative"
                    'LOGGING
                    If myException.Message <> "VALIDATION" Then
                        sendLogging(String.Format("{0}<BR>{1}", myException.Message, Left(myException.StackTrace, 5000)))
                    End If
                Case TypeMessage.Waarschuwing
                    imgMessage.ImageUrl = "images/warning.gif"
                    imgMessage.ToolTip = "Waarschuwing"
                    lblMessage.CssClass = "MsgNegative"
                    'LOGGING
                    If myException.Message <> "VALIDATION" Then
                        sendLogging(String.Format("{0}<BR>{1}", myException.Message, Left(myException.StackTrace, 5000)))
                    End If
                Case TypeMessage.Bevestiging
                    imgMessage.ImageUrl = "images/Confirmation.gif"
                    imgMessage.ToolTip = "Bevestiging"
                Case TypeMessage.Info
                    imgMessage.ImageUrl = "images/Info.gif"
                    imgMessage.ToolTip = "Info"
                Case TypeMessage.Info
            End Select

            TBLMessage.Visible = True
            If Not stayVisible Then
                AnimationExtender1.Animations = "<OnLoad><Sequence><FadeOut Duration=""5"" Fps=""50"" /></Sequence></OnLoad>"
            End If

            strMyMessage = ""
        Else
            pnlMessage.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btnClose control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnClose.Click
        TBLMessage.Visible = False
    End Sub

    ''' <summary>
    ''' Sends the logging.
    ''' </summary>
    ''' <param name="strTekst">The string tekst.</param>
    Private Sub sendLogging(ByRef strTekst As String)
        'Dim myLogging As New Logging
        'myLogging.Gebruiker.IdLid = Session("Gebruiker")
        'myLogging.EventLogging = strTekst
        'myLogging.Type = 1
        'Try
        '    Dim myBal As New Olympia.BALOlympia.BalGebruikers
        '    myBal.InsertLogging(myLogging)
        'Catch ex As Exception
        '    Throw
        'End Try
    End Sub
End Class
