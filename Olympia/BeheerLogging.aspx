<%@ Page Language="VB" ValidateRequest="true" AutoEventWireup="false" CodeBehind="BeheerLogging.aspx.vb"
    Inherits="Olympia.BeheerLogging" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="uc3" TagName="UCMessage" Src="CustomMessage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="UCScript" Src="CustomScriptControl.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Curato</title>
    <script src="https://www.w3schools.com/lib/w3.js"></script>
    <meta charset="UTF-8" />
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Styles.css" type="text/css" />
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
            <a onclick="window.location.href='aanwezigheden.aspx'" id="pagAanwezigheden"  visible="false" runat="server" href="#" class="w3-bar-item w3-button">Aanwezigheden</a>

            <a onclick="w3.toggleShow('#vergoeding')" href="#" class="w3-button w3-block w3-left-align" visible="false" runat="server" id="myBtn1">Vergoedingen <i class="fa fa-caret-down"></i></a>
            <div id="vergoeding"  visible="false" runat="server" class="w3-bar-block w3-padding-large w3-medium ">
                <a onclick="window.location.href='K_Jury.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Jury</a>
                <a onclick="window.location.href='K_Wedstrijd.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Wedstrijd</a>
                <a onclick="window.location.href='K_Verplaatsing.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Verplaatsing</a>
                <a onclick="window.location.href='K_Andere.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Andere</a>
            </div>

   <a onclick="window.location.href='BeheerGebruikers.aspx'" href="#" id="pagGebruikers" visible="false" runat="server" class="w3-bar-item w3-button">Gebruikers</a>

            <a onclick="w3.toggleShow('#beheer')"  href="#" class="w3-button w3-block w3-left-align" visible="false" runat="server" id="myBtn2">Beheer <i class="fa fa-caret-down"></i></a>
            <div id="beheer"  visible="false" runat="server" class="w3-bar-block w3-padding-large w3-medium">
                <a onclick="window.location.href='beheerdisciplines.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Trainingsgroepen</a>
                <a onclick="window.location.href='beheerlogging.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Logging</a>
                <a onclick="window.location.href='Importgegevens.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Import</a>
                <a onclick="window.location.href='beheerandere.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Andere</a>
            </div>

            <a onclick="window.location.href='meldingen.aspx'" href="#" class="w3-bar-item w3-button">Meldingen</a>
            <a onclick="window.location.href='privacy.aspx'" href="#" class="w3-bar-item w3-button w3-padding">Privacy</a>
                            <a onclick="window.location.href='login.aspx'" href="#" class="w3-bar-item w3-button w3-padding">Uitloggen</a>
        </div>
    </nav>

    <!-- !PAGE CONTENT! -->
    <div class="w3-main" style="margin-left: 250px">
        <form id="form1" runat="server">
            <uc1:UCScript ID="myUCScript1" runat="server" />
            <table style="width: 100%">
        <tr>
            <td style="vertical-align:top; align-content:center">
                <!-- CONTENT -->
                <table style="width:85%" >
                    <tr>
                            <td  style=" align-content:flex-start">
                            <asp:Label ID="lblPageTitle" CssClass="PageTitle" runat="server"></asp:Label><br />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                       <td  style=" align-content:flex-end">
                            <asp:Panel ID="lnkbAdminTitle" runat="server" CssClass="CollapsPanelHeader">
                                <asp:LinkButton ID="lnkbAdmin" CausesValidation="false" runat="server" />
                                <asp:LinkButton ID="btnShowDatagrid" runat="server" Visible="false" />
                                <asp:LinkButton ID="btnShowRapport" runat="server" />
                            </asp:Panel>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td  style=" align-content:flex-start">
                            <table width="100%">
                                <tr>
                                    <td align="left" width="150">
                                        <asp:Label ID="lblPeriode" runat="server"></asp:Label>:
                                    </td>
                                    <td  style=" align-content:flex-start">
                                        <asp:TextBox ID="txtStart" Width="100" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fte1" runat="server" FilterMode="InvalidChars"
                                            InvalidChars="<>&-" TargetControlID="txtStart" />
                                        <asp:CalendarExtender ID="calExtStart" runat="server" TargetControlID="txtStart"
                                            Animated="false" Format="dd/MM/yyyy" />
                                        <asp:Label ID="lblPeriodeTot" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtEnd" Width="100" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fte7" runat="server" FilterMode="InvalidChars"
                                            InvalidChars="<>&-" TargetControlID="txtEnd" />
                                        <asp:CalendarExtender ID="calExtEnd" runat="server" TargetControlID="txtEnd"
                                            Animated="true" Format="dd/MM/yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td  style=" align-content:flex-start">
                                        <asp:Label ID="lblinfo" runat="server"></asp:Label>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInfo" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fte2" runat="server" FilterMode="InvalidChars"
                                            InvalidChars="<>&" TargetControlID="txtInfo" />
                                    </td>
                                </tr>
                                <tr>
                                   <td  style=" align-content:flex-start">
                                        <asp:Label ID="lblzoektype" runat="server"></asp:Label>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cbTypes" AutoPostBack="true" runat="server">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnzoek" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnwisfilter" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td  style=" align-content:flex-start">
                            <asp:datagrid ID="dtgDataGrid" runat="server" AutoGenerateColumns="false" CssClass="navigateable" GridLines="Horizontal"
                                Width="100%" PageSize="20" AllowPaging="true" BorderWidth="0" AllowCustomPaging="false"
                                PagerStyle-Visible="false" AllowSorting="true" DataKeyField="datum" ShowFooter="false">
                                <ItemStyle CssClass="datagridItem" />
                                <FooterStyle CssClass="datagridItem" />
                                <Columns>
                                    <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderSort" SortExpression="Datum"
                                        ItemStyle-Width="150" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Top"
                                        HeaderStyle-HorizontalAlign="center" HeaderText="Datum">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDatum" Font-Size="Smaller" runat="server"></asp:Label>
                                                                                 </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" ItemStyle-VerticalAlign="Top"
                                        SortExpression="Naam" HeaderText="Gebruiker">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGebruiker" Font-Size="Smaller" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" ItemStyle-VerticalAlign="Top"
                                        HeaderText="Event">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEvent" Font-Size="Smaller" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" ItemStyle-VerticalAlign="Top"
                                        SortExpression="Type" HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" Font-Size="Smaller" Font-Bold="true" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:datagrid>
                            <table  style="width:100%" id="tblFooter" visible="false" runat="server">
                                <tr>
                                    <td valign="top">
                                        <table width="100%" class="datagridPaging">
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
                                                    <asp:FilteredTextBoxExtender ID="fte99" runat="server" FilterMode="ValidChars"
                                                        FilterType="Numbers" TargetControlID="txtCurrentPage" />
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
                    <tr>
                        <td colspan="2" align="left">
                            <uc3:UCMessage ID="UC_Message" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
  </div>
    <div class="w3-black w3-center w3-padding-16"><b>Powered by Graecas Inc.</b></div>
    <script>
        document.getElementById("myBtn1").click();
        document.getElementById("myBtn2").click();
    </script>

</body>
</html>
