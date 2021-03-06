﻿<%@ Page Language="vb" ValidateRequest="true" AutoEventWireup="false" CodeBehind="BeheerGebruikers.aspx.vb" Inherits="Olympia.BeheerGebruikers" %>

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
            <a onclick="window.location.href='aanwezigheden.aspx'" id="pagAanwezigheden" visible="false" runat="server" href="#" class="w3-bar-item w3-button">Aanwezigheden</a>

            <a onclick="w3.toggleShow('#vergoeding')" href="#" class="w3-button w3-block w3-left-align" visible="false" runat="server" id="myBtn1">Vergoedingen <i class="fa fa-caret-down"></i></a>
            <div id="vergoeding" visible="false" runat="server" class="w3-bar-block w3-padding-large w3-medium ">
                <a id="v_lesgever" runat="server" onclick="window.location.href='K_Lesgever.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Lesgever</a>
               <a id="v_wedstrijd" runat="server" onclick="window.location.href='K_Wedstrijd.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Wedstrijd</a>
                <a id="v_verplaatsing" runat="server" onclick="window.location.href='K_Verplaatsing.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Verplaatsing</a>
                <a id="v_andere" runat="server" onclick="window.location.href='K_Andere.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Andere</a>
            </div>

            <a onclick="window.location.href='BeheerGebruikers.aspx'" href="#" id="pagGebruikers" visible="false" runat="server" class="w3-bar-item w3-button">Gebruikers</a>

            <a onclick="w3.toggleShow('#beheer')" href="#" class="w3-button w3-block w3-left-align" visible="false" runat="server" id="myBtn2">Beheer <i class="fa fa-caret-down"></i></a>
            <div id="beheer" visible="false" runat="server" class="w3-bar-block w3-padding-large w3-medium">
                <a onclick="window.location.href='beheerdisciplines.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Trainingsgroepen</a>
                <a onclick="window.location.href='beheerlogging.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Logging</a>
                <a onclick="window.location.href='Importgegevens.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Import</a>
                                <a onclick="window.location.href='BeheerVergoedingen.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Vergoedingen</a>        </div>

            <a onclick="window.location.href='login.aspx'" href="#" class="w3-bar-item w3-button w3-padding">Uitloggen</a>
        </div>
    </nav>

    <!-- !PAGE CONTENT! -->
    <div class="w3-main" style="margin-left: 250px">
        <form id="form1" runat="server">
      <asp:scriptmanager ID="ScriptManager1" runat="server" EnablePageMethods = "true"> </asp:scriptmanager>
            <table style="width: 100%">
                <tr>
                    <td style="vertical-align: top; align-content: center">
                        <!-- CONTENT -->
                        <table style="width: 85%">
                            <tr>
                                <td style="align-content: flex-start">
                                    <asp:Label ID="lblPageTitle" CssClass="PageTitle" runat="server"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblresult" runat="server"></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btn_nieuw" runat="server" />
                                             </td>

                                            <td width="250" align="center">
                                                <asp:Label ID="lblOpen" runat="server"></asp:Label>
                                                <asp:CheckBox ID="ckbxOpen" runat="server" AutoPostBack="true" />
                                            </td>
                                            <td style="background-color: ButtonFace">
                                                <asp:Label ID="lblFilter" runat="server"></asp:Label>
                                                <asp:TextBox ID="txtfilter" runat="server"></asp:TextBox>&nbsp;
                                                <asp:FilteredTextBoxExtender ID="fte2" runat="server" FilterMode="InvalidChars"
                                                    InvalidChars="<>&" TargetControlID="txtfilter" />
                                                <asp:Button ID="btnFilter" runat="server" />
                                                <asp:Button ID="btnWisFilter" runat="server" />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                                                                    <td colspan="3" align="center">
                                            <asp:Label ID="lblGebruiker" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtGebruiker" Width="300" runat="server" Font-Bold="true" Font-Size="Larger"
                                                TabIndex="2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="fte1" runat="server" FilterMode="InvalidChars" InvalidChars="<>&" TargetControlID="txtGebruiker"></asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" Enabled="true" runat="server" ServiceMethod="getGebruikersSuggest"
                                                ServicePath="~/OlympiaService.asmx" TargetControlID="txtGebruiker" MinimumPrefixLength="2"
                                                CompletionInterval="50" CompletionSetCount="20" CompletionListItemCssClass="autocomplete_listItem"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                            </asp:AutoCompleteExtender>

                                        </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td style="align-content: flex-start">
                                    <asp:DataGrid ID="dtgDataGrid" SelectedItemStyle-BackColor="#cccccc"
                                        runat="server" AutoGenerateColumns="false" CssClass="navigateable" GridLines="Horizontal"
                                        Width="100%" PageSize="20" AllowPaging="true" BorderWidth="0" AllowCustomPaging="false"
                                        AllowSorting="true" ShowFooter="false" AlternatingItemStyle-BackColor="WhiteSmoke" PagerStyle-Visible="false">
                                        <ItemStyle CssClass="datagridItem" Wrap="false" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" ItemStyle-Width="30"
                                                HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkOpen" runat="Server" CssClass="activation" CommandName="OPEN"
                                                        Text="<img src='../images/SnelZoeken.gif' width='19' height='19' alt='Open' border=0>" />
                                                    <asp:Label ID="lblId" Visible="true" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderSort" ItemStyle-Width="100"
                                                HeaderStyle-HorizontalAlign="center" HeaderText="Naam" ItemStyle-HorizontalAlign="center" SortExpression="Naam">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNaam" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" SortExpression="Voornaam"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150" ItemStyle-Width="150"
                                                ItemStyle-HorizontalAlign="left" HeaderText="Voornaam">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVoornaam" Font-Bold="true" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="150" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="left"
                                                SortExpression="Email" HeaderText="Email">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmail" Font-Size="Smaller" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="100" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="left"
                                                SortExpression="GSM" HeaderText="GSM">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGSM" Font-Size="Smaller" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" ItemStyle-Width="30"
                                                HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="Server" CssClass="activation" CommandName="Delete"
                                                        Text="<img src='../images/delete.gif' width='19' height='19' alt='Open' border=0>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>

                                        </Columns>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table width="100%" class="datagridPaging">
                                        <tr>
                                            <td style="width: 33%; vertical-align: middle; align-content: flex-start" class="datagridBleuHook">&nbsp;
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
                                            <td style="width: 33%; vertical-align: bottom" align="right" class="datagridBleuHook">&nbsp;
                                <asp:Label ID="lblPagina" Font-Size="Smaller" runat="server"></asp:Label>&nbsp;
                                        <asp:TextBox ID="txtCurrentPage" Font-Size="Smaller" Width="20" MaxLength="4" runat="server"></asp:TextBox>

                                                <asp:Label ID="lblExtraPaging" Font-Size="Smaller" runat="server"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <%-- <uc3:ucmessage id="UC_Message" runat="server" />--%>
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
