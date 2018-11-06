<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="start.aspx.vb" Inherits="Olympia.start" %>
<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="uc3" TagName="UCMessage" Src="CustomMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Curato</title>
    <script src="https://www.w3schools.com/lib/w3.js"></script>
    <meta charset="UTF-8" />
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Styles.css" type="text/css" />
       
    <script src="../Scripts/thickbox.js" type="text/javascript"></script>
    <link href="../Scripts/thickbox.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function tb_init() {
            $(document).click(function (e) {
                e = e || window.event;
                var el = e.target || e.scrElement || null;
                if (el && el.parentNode && !el.className || !/thickbox/.test(el.className))
                    el = el.parentNode;
                if (!el || !el.className || !/thickbox/.test(el.className))
                    return;
                var t = el.title || el.name || null;
                var a = el.href || el.alt;
                var g = el.rel || false;
                tb_show(t, a, g);
                el.blur();
                return false;
            });
        };
        function tb_remove() {
            $("#TB_imageOff").unbind("click");
            $("#TB_closeWindowButton").unbind("click");
            $("#TB_window").fadeOut("fast", function () { $('#TB_window,#TB_overlay,#TB_HideSelect').trigger("unload").unbind().remove(); });
            $("#TB_load").remove();
            if (typeof document.body.style.maxHeight == "undefined") {//if IE 6
                $("body", "html").css({ height: "auto", width: "auto" });
                $("html").css("overflow", "");
            }
            document.onkeydown = "";
            document.onkeyup = "";

            parent.location.reload(1);
            return false;
        };
    </script>

</head>

<body class="w3-content" style="max-width: 1200px">

    <div class="w3-black w3-center w3-padding-24 ">
        <font size="32"><b>Curato</b></font>
        <br />
        <div class="w3-right">
        <asp:Label ID="lbllogin" runat="server"></asp:Label>&nbsp;
            </div>
    </div>
    <nav class="w3-sidebar w3-bar-block w3-white w3-top" style="z-index: 3; width: 250px" id="mySidebar">
        <div class="w3-padding-16 w3-large w3-text-grey" style="font-weight: bold">
            <img src="images/figuur vrouw.png" alt="" style="width: 230px; height: 200px;" />
            <a onclick="window.location.href='Start.aspx'" href="#" class="w3-bar-item w3-button">Mijn Profiel</a>
            <a onclick="window.location.href='aanwezigheden.aspx'" id="pagAanwezigheden" visible="false" runat="server" href="#" class="w3-bar-item w3-button">Aanwezigheden</a>

            <a onclick="w3.toggleShow('#vergoeding')" href="#" class="w3-button w3-block w3-left-align" visible="false" runat="server" id="myBtn1">Vergoedingen <i class="fa fa-caret-down"></i></a>
            <div id="vergoeding" visible="false" runat="server" class="w3-bar-block w3-padding-large w3-medium ">
                <a id="v_lesgever" runat="server" onclick="window.location.href='K_Lesgever.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Lesgever</a>
                <a id="v_wedstrijd" runat="server" onclick="window.location.href='K_Wedstrijd.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Wedstrijd/Jury</a>
                <a id="v_verplaatsing" runat="server" onclick="window.location.href='K_Verplaatsing.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Verplaatsing</a>
                <a id="v_andere" runat="server" onclick="window.location.href='K_Andere.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Andere</a>
            </div>

            <a onclick="window.location.href='BeheerGebruikers.aspx'" href="#" id="pagGebruikers" visible="false" runat="server" class="w3-bar-item w3-button">Gebruikers</a>

            <a onclick="w3.toggleShow('#beheer')" href="#" class="w3-button w3-block w3-left-align" visible="false" runat="server" id="myBtn2">Beheer <i class="fa fa-caret-down"></i></a>
            <div id="beheer" visible="false" runat="server" class="w3-bar-block w3-padding-large w3-medium">
                <a onclick="window.location.href='beheerdisciplines.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Trainingsgroepen</a>
                <a onclick="window.location.href='beheerlogging.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Logging</a>
                <a onclick="window.location.href='Importgegevens.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Import</a>
                                <a onclick="window.location.href='BeheerVergoedingen.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Vergoedingen</a>       

            </div>

            <a onclick="window.location.href='login.aspx'" href="#" class="w3-bar-item w3-button w3-padding">Uitloggen</a>
        </div>
    </nav>

    <!-- !PAGE CONTENT! -->
    <div class="w3-main" style="margin-left: 250px">
        <form id="form1" runat="server">
           <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>
            <table style="width: 100%">
        <tr>
            <td style="vertical-align:top; align-content:center">
                <!-- CONTENT -->
                <table style="width:85%" >
                    <tr>
                            <td  style=" align-content:flex-start">
                            <asp:Label ID="lblPageTitle" CssClass="PageTitle" runat="server"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left" id="test" runat="server" visible="true">
                            <table id="tblMailClient" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton BackColor="Gray" Font-Bold="true" ForeColor="White" runat="server"
                                            ID="lnkNew"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAantalGebruikers" runat="server"></asp:Label>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3">
                                        <asp:DataGrid ID="dtgDataGrid" RowHighlightColor="#cccccc" RowClickColor="#ebe0fb"
                                            RowSelectionEnabled="true" RowClickEventCommandName="dtgDataGrid_EditCommand"
                                            runat="server" AutoGenerateColumns="false" CssClass="navigateable" GridLines="Horizontal"
                                            Width="100%" PageSize="10" AllowPaging="true" DataKeyField="Id" BorderWidth="0"
                                            AllowCustomPaging="false" AllowSorting="true" ShowFooter="false" PagerStyle-Visible="false">
                                            <ItemStyle CssClass="datagridItem" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-Width="50"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkOpen" runat="Server" autoPostback="true" CommandName="OPEN"
                                                            Text="<img src='../images/SnelZoeken.gif' width='19' height='19' alt='Open' border=0>" />
                                                        <asp:Label ID="lblId" Visible="false" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderSort" ItemStyle-Font-Size="X-Small"
                                                    ItemStyle-Width="125" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    SortExpression="DatumUur">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDatumUur" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" ItemStyle-Font-Size="X-Small"
                                                    SortExpression="g.naam" ItemStyle-Width="200">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAfzender" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" ItemStyle-Font-Size="X-Small"
                                                    SortExpression="onderwerp">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOnderwerp" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-Width="40"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ConfirmButtonExtender ID="cbeDelete" runat="server" TargetControlID="btnDelete"
                                                            ConfirmText="Bent u zeker dat u deze boodschap wilt verwijderen ?" />
                                                        <asp:LinkButton ID="btnDelete" ToolTip="Verwijderen" runat="server" Text="<img src='../images/database_delete.png' border=0>"
                                                            CommandName="DELETE" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-Width="40"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSetStatus" ToolTip="Status veranderen" runat="server" Text="<img src='../images/cancel.gif' border=0>"
                                                            CommandName="STATUSONGELEZEN" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" colspan="3">
                                        <table width="100%" cellspacing="0" cellpadding="0" class="datagridPaging">
                                            <tr>
                                                <td width="33%" valign="middle" align="left" class="datagridBleuHook">
                                                    &nbsp;
                                                </td>
                                                <td align="center" valign="bottom">
                                                    <asp:ImageButton ID="imgbFirstbutton" ImageUrl="../images/firstRecord.gif" CommandArgument="0"
                                                        runat="server" OnClick="PagerButtonClick" />&nbsp;&nbsp;
                                                    <asp:ImageButton ID="imgbPrevbutton" ImageUrl="../images/previousRecord.gif" CommandArgument="prev"
                                                        runat="server" OnClick="PagerButtonClick" />&nbsp;&nbsp;
                                                    <asp:ImageButton ID="imgbNextbutton" ImageUrl="../images/nextRecord.gif" CommandArgument="next"
                                                        runat="server" OnClick="PagerButtonClick" />&nbsp;&nbsp;
                                                    <asp:ImageButton ID="imgbLastbutton" ImageUrl="../images/lastRecord.gif" CommandArgument="last"
                                                        runat="server" OnClick="PagerButtonClick" />
                                                </td>
                                                <td valign="bottom" width="33%" align="right">
                                                    <asp:Label ID="lblPagina" Font-Size="Smaller" runat="server"></asp:Label>&nbsp;
                                                    <asp:TextBox ID="txtCurrentPage" Font-Size="Smaller" Width="20" MaxLength="4" runat="server"></asp:TextBox>
                                                    <asp:Label ID="lblExtraPaging" Font-Size="Smaller" runat="server"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                       
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
        </form>
  </div>

    <div class="w3-black w3-center w3-padding-16">
        <b>Powered by Graecas Inc.</b>   
        <div class="w3-right">
<b onclick="window.location.href='privacy.aspx'" >Privacy</b>&nbsp;
            </div>
        
    </div>

    <script>
        document.getElementById("myBtn2").click();
        document.getElementById("myBtn1").click();
    </script>

</body>
</html>
