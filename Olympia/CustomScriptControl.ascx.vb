
Public Class CustomScriptControl
    Inherits UserControl

    ''' <summary>
    ''' Handles the PreRender event of the Page control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.Page.ClientScript.GetPostBackEventReference(UpdatePanel1, String.Empty)
    End Sub

    ''' <summary>
    ''' Handles the Load event of the Page control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Response.Cache.SetCacheability(HttpCacheability.NoCache)

    End Sub


End Class