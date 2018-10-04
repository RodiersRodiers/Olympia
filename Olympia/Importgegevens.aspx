<%@ Page Language="VB" ValidateRequest="true" CodeBehind="Importgegevens.aspx.vb"
    Inherits="Olympia.Importgegevens" %>

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
        </div>
    </nav>

    <!-- !PAGE CONTENT! -->
    <div class="w3-main" style="margin-left: 250px">
        <form id="form1" runat="server">
            <uc1:UCScript ID="myUCScript1" runat="server" />
            <table style="width: 100%">
                <tr>
                    <td style="vertical-align: top; align-content: center">
                        <!-- CONTENT -->
                        <table style="width: 85%">
                            <tr>
                                <td style="align-content: flex-start">
                                    <asp:Label ID="lblPageTitle" CssClass="PageTitle" runat="server"></asp:Label><br />
                                    <br />
                         
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblType" Font-Bold="True" runat="server"></asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:ComboBox ID="cbType" AutoCompleteMode="Suggest" DropDownStyle="DropDownList"
                                                    AppendDataBoundItems="False" Width="200" AutoPostBack="true" runat="server">
                                                </asp:ComboBox>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="btnLoadList" runat="server" />
                                                <asp:Button ID="btnImport" runat="server" Visible="false" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblinfo" ForeColor="blue" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>

                                        <tr>
                                            <td colspan="3">


                                                <asp:DataGrid ID="dtgDataGrid" SelectedItemStyle-BackColor="#cccccc"
                                                    runat="server" AutoGenerateColumns="false" CssClass="navigateable" GridLines="Horizontal"
                                                    Width="100%" PageSize="100" AllowPaging="true" BorderWidth="0" AllowCustomPaging="false"
                                                    AllowSorting="true" ShowFooter="false" PagerStyle-Visible="false">
                                                    <ItemStyle CssClass="datagridItem" Wrap="false" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderSort" ItemStyle-Width="100"
                                                            HeaderStyle-HorizontalAlign="center" HeaderText="Naam" ItemStyle-HorizontalAlign="left" SortExpression="Naam">
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

                                                                   <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" SortExpression="gebdatum"
                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150" ItemStyle-Width="150"
                                                            ItemStyle-HorizontalAlign="left" HeaderText="GebDatum">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGebdatum" Font-Bold="true" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                                   <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" SortExpression="geslacht"
                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150" ItemStyle-Width="150"
                                                            ItemStyle-HorizontalAlign="left" HeaderText="geslacht">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGeslacht" Font-Bold="true" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="150" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="left"
                                                            HeaderText="Email">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail" Font-Size="Smaller" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="150" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="left"
                                                            HeaderText="gsm">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblgsm" Font-Size="Smaller" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="150" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="left"
                                                            HeaderText="RekNr">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRekNr" Font-Size="Smaller" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="150" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="left"
                                                            HeaderText="Gemeente">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGemeente" Font-Size="Smaller" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                    </Columns>
                                                </asp:DataGrid>

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            
                        </table>
                    </td>
                </tr>
            </table>
            <div id="progressbar_1" style="width: 0%; background-color: #51575d; height: 15px;">
            </div>
        </form>


        <!-- End page content -->
        <div class="w3-black w3-center w3-padding-24">Powered by <a href="https://www.turnkringolympia.be" title="Olympia TKOZ" target="_blank" class="w3-hover-opacity">Olympia TKOZ</a></div>
    </div>

    <script>

        document.getElementById("myBtn1").click();
        document.getElementById("myBtn2").click();

    </script>

</body>
</html>
