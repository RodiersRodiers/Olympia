
Imports System.Drawing
Imports System.Web.UI.WebControls

Namespace OBJOlympia

    Public Class OlympiaDatagrid
        Inherits DataGrid

        Public Property RowHighlightColor() As Color
            Get
                Dim o As Object = ViewState("RowHighlightColor")
                If o Is Nothing Then
                    Return Color.Empty
                Else
                    Return o
                End If
            End Get
            Set(ByVal Value As Color)
                ViewState("RowHighlightColor") = Value
            End Set
        End Property

        Public Property RowClickColor() As Color
            Get
                Dim o As Object = ViewState("RowClickColor")
                If o Is Nothing Then
                    Return Color.Empty
                Else
                    Return o
                End If
            End Get
            Set(ByVal Value As Color)
                ViewState("RowClickColor") = Value
            End Set
        End Property

        Public Property RowClickEventCommandName() As String
            Get
                Dim o As Object = ViewState("RowClickEventCommandName")
                If o Is Nothing Then
                    Return String.Empty
                Else
                    Return o
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("RowClickEventCommandName") = Value
            End Set
        End Property

        Public Property RowSelectionEnabled() As Boolean
            Get
                Dim o As Object = ViewState("RowSelectionEnabled")
                If o Is Nothing Then
                    Return True
                Else
                    Return o
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("RowSelectionEnabled") = Value
            End Set
        End Property

#Region "Overridden DataGrid Methods"

        ''' <summary>
        ''' Creates a <see cref="T:System.Web.UI.WebControls.DataGridItem" /> object.
        ''' </summary>
        ''' <param name="itemIndex">The index for the <see cref="T:System.Web.UI.WebControls.DataGridItem" /> object.</param>
        ''' <param name="dataSourceIndex">The index of the data item from the data source.</param>
        ''' <param name="itemType">One of the <see cref="T:System.Web.UI.WebControls.ListItemType" /> values.</param>
        ''' <returns>
        ''' A <see cref="T:System.Web.UI.WebControls.DataGridItem" /> object.
        ''' </returns>
        Protected Overrides Function CreateItem(ByVal itemIndex As Integer, ByVal dataSourceIndex As Integer, ByVal itemType As ListItemType) As DataGridItem
            Dim item As OlympiaDatagridItem = New OlympiaDatagridItem(itemIndex, dataSourceIndex, itemType)

            If (RowSelectionEnabled And itemType <> ListItemType.Header And itemType <> ListItemType.Footer And itemType <> ListItemType.Pager) Then
                item.Attributes.Add("onmouseover", "javascript:kissDtg_changeBackColor(this, true);")
                item.Attributes.Add("onmouseout", "javascript:kissDtg_changeBackColor(this, false);")
                item.Attributes.Add("onmouseover", "javascript:this.style.cursor='default';")
            End If

            Return item
        End Function

        ''' <summary>
        ''' Gets the string script.
        ''' </summary>
        ''' <returns></returns>
        Private Shared Function GetStrScript() As System.Text.StringBuilder
            Dim strScript As New System.Text.StringBuilder
            strScript.Append("<script language=""JavaScript"">" & vbCrLf)
            strScript.Append("<!--" & vbCrLf)
            strScript.Append("var lastColorUsed;" & vbCrLf)
            strScript.Append("function kissDtg_changeBackColor(row, highlight){{" & vbCrLf)
            strScript.Append("  if (highlight)" & vbCrLf)
            strScript.Append(" {{" & vbCrLf)
            strScript.Append("    lastColorUsed = row.style.backgroundColor;" & vbCrLf)
            strScript.Append("    row.style.backgroundColor = '#{0:X2}{1:X2}{2:X2}';}}" & vbCrLf)
            strScript.Append("  else {{" & vbCrLf)
            strScript.Append("    row.style.backgroundColor = lastColorUsed;}}" & vbCrLf)
            strScript.Append("}}// -->" & vbCrLf)
            strScript.Append("</script>")
            Return strScript
        End Function

        ''' <summary>
        ''' Ons the pre render.
        ''' </summary>
        ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        Protected Overrides Sub onPreRender(ByVal e As EventArgs)
            MyBase.OnPreRender(e)

            If Not RowSelectionEnabled Then
                Exit Sub
            End If

            Const SCRIPT_KEY As String = "kissDtgScript"
            Dim strScript As System.Text.StringBuilder = GetStrScript()

            If RowHighlightColor <> Color.Empty And Not Page.ClientScript.IsClientScriptBlockRegistered(SCRIPT_KEY) Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, SCRIPT_KEY, String.Format(strScript.ToString, RowHighlightColor.R, RowHighlightColor.G, RowHighlightColor.B))
            End If

            'add the click client-side event handler, if needed
            If RowClickEventCommandName <> String.Empty And Me.ChildControlsCreated And Controls.Count > 0 Then
                CreateClickEvent()
            End If
        End Sub

        ''' <summary>
        ''' Creates the click event.
        ''' </summary>
        Public Overridable Sub CreateClickEvent()
            For Each dgi As OlympiaDatagridItem In Me.Items
                If dgi.ItemType <> ListItemType.Header And dgi.ItemType <> ListItemType.Footer And dgi.ItemType <> ListItemType.Pager Then
                    'dgi.Attributes("OnDblClick") = Page.ClientScript.GetPostBackClientHyperlink(dgi, RowClickEventCommandName)                    
                    'dgi.Attributes("OnClick") = "javascript:this.style.backgroundColor='" & System.Drawing.ColorTranslator.ToHtml(RowClickColor) & "';"
                    'dgi.Attributes("OnClick") = "selectRow(this);"

                    '                Dim MYLINK As Button = CType(e.Item.Cells(6).Controls(0), Button)
                    'e.Item.Attributes("ondblclick") = ClientScript.GetPostBackClientHyperlink(dtgDataGrid, "dtgDataGrid_EditCommand")

                    'e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';";


                    'e.Item.Attributes.Add("onmouseenter", "highlightRow(this,'red')")
                    ' e.Item.Attributes.Add("onmouseleave", "dehighlightRow(this,'white')")

                End If
            Next

        End Sub

#End Region

        ''' <summary>
        ''' 
        ''' </summary>
        Public Class OlympiaDatagridItem
            Inherits DataGridItem : Event IPostBackEventHandler()

            ''' <summary>
            ''' Initializes a new instance of the <see cref="OlympiaDatagridItem"/> class.
            ''' </summary>
            ''' <param name="itemIndex">The index of the item from the <see cref="P:System.Web.UI.WebControls.DataGrid.Items" /> collection in the <see cref="T:System.Web.UI.WebControls.DataGrid" /> control.</param>
            ''' <param name="dataSetIndex">The index number of the item, from the bound data source, that appears in the <see cref="T:System.Web.UI.WebControls.DataGrid" /> control.</param>
            ''' <param name="itemType">One of the <see cref="T:System.Web.UI.WebControls.ListItemType" /> values.</param>
            Sub New(ByVal itemIndex As Integer, ByVal dataSetIndex As Integer, ByVal itemType As ListItemType)
                MyBase.New(itemIndex, dataSetIndex, itemType)
            End Sub

#Region "IPostBackEventHandler Members"

            ''' <summary>
            ''' Raises the post back event.
            ''' </summary>
            ''' <param name="eventArgument">The event argument.</param>
            Public Sub RaisePostBackEvent(ByVal eventArgument As String)
                Dim commandArgs As CommandEventArgs = New CommandEventArgs(eventArgument, Nothing)
                Dim args As DataGridCommandEventArgs = New DataGridCommandEventArgs(Me, Me, commandArgs)
                MyBase.RaiseBubbleEvent(Me, args)
            End Sub

#End Region

        End Class

    End Class

End Namespace
