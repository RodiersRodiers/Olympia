<%@ Page Language="VB" ValidateRequest="true" AutoEventWireup="false" CodeBehind="K_Lesgever.aspx.vb" Inherits="Olympia.K_Lesgever" %>

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
                               <a onclick="window.location.href='BeheerVergoedingen.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Vergoedingen</a>    </div>

                <a onclick="window.location.href='login.aspx'" href="#" class="w3-bar-item w3-button w3-padding">Uitloggen</a>
        </div>
    </nav>

    <!-- !PAGE CONTENT! -->
    <div class="w3-main" style="margin-left: 250px">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>

            <table style="width: 100%">
                <tr>
                    <td style="vertical-align: top; align-content: center">
                        <!-- CONTENT -->
                        <table style="width: 85%">
                            <tr>
                                <td style="align-content: flex-start">
                                    <asp:Label ID="lblPageTitle" CssClass="PageTitle" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table >
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOpsteller" Font-Bold="true" runat="server" />
                                                &nbsp;
                                                <asp:ComboBox ID="cbopsteller" AutoPostBack="true" runat="server"></asp:ComboBox>
                                            </td>
                                            <td width="50"></td>
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
                                            <td colspan="2" >
                                                <asp:Button ID="btnINSERTAdd" runat="server" />
                                                <asp:Button ID="btnINSERTCancel" Visible="false" runat="server" />
                                                <br />
                                            </td>
                                            <td><font color="blue"> Info = persoonlijke opmerking <br />
                                             Duur  = in minuten = bijv. 1.5 uur = 90</font></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>


                            <tr>
                                <td colspan="2" style="align-content: flex-start">
                                    <%--  <uc3:UCMessage ID="UC_Message" runat="server" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                                                       <asp:DataGrid ID="dtgDataGrid" SelectedItemStyle-BackColor="#cccccc"
                                        runat="server" AutoGenerateColumns="false" CssClass="navigateable" GridLines="Horizontal"
                                        Width="95%" PageSize="25" AllowPaging="true" BorderWidth="0" AllowCustomPaging="false"
                                        AllowSorting="true" ShowFooter="false" PagerStyle-Visible="false">
                                        <ItemStyle CssClass="datagridItem" Wrap="false" />
                                        <Columns>
                                              <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-Width="85" SortExpression="Datum" HeaderText="Dag">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDag" Width="80" runat="server"></asp:Label>
                                               
                                                </ItemTemplate>
                                                <EditItemTemplate>
                               
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                       
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-Width="85" SortExpression="Datum" HeaderText="Datum">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatum" Width="85" runat="server"></asp:Label>
                                                    <asp:hiddenfield ID="lblLidID" runat="server"></asp:hiddenfield>
                                                    <asp:hiddenfield ID="lblID" runat="server"></asp:hiddenfield>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                     <asp:hiddenfield ID="lblEditID" runat="server"></asp:hiddenfield>
                                                    <asp:TextBox ID="txtEditDatum" Width="85" runat="server"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calDocumentDatum" runat="server" TargetControlID="txtEditDatum"
                                                        Animated="false" Format="dd/MM/yyyy" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtInsertDatum" Width="85" runat="server"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calDocumentDatum" runat="server" TargetControlID="txtInsertDatum"
                                                        Animated="false" Format="dd/MM/yyyy" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" SortExpression="ID_Discipline" HeaderText="Groep">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgroep" Width="200" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="drpEditGroep" Width="200" runat="server"></asp:DropDownList>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="drpInsertGroep" Width="200" runat="server"></asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" SortExpression="aantal" HeaderStyle-Width="70" HeaderText="Duur">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblaantal" Width="50" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txteditaantal" Width="50" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtInsertaantal" Width="50" runat="server"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" SortExpression="info" HeaderStyle-Width="300" HeaderText="Info">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblinfo" Width="300" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <textarea id="txtEditInfo" runat="server" cols="30" rows="2"></textarea>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <textarea id="txtInsertInfo" runat="server" cols="30" rows="2"></textarea>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" Visible="true" HeaderStyle-Width="60" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkbEdit" CssClass="activation" ToolTip="Edit" runat="Server" Text="<img src='../images/edit.gif' alt='edit' border=0>" CommandName="EDIT" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkbUpdate" ToolTip="Update" runat="Server" Text="<img src='../images/update.gif' alt='update' border=0>" CommandName="UPDATE" />
                                                    <asp:LinkButton ID="lnkbCancel" ToolTip="Cancel" runat="Server" Text="<img src='../images/cancel.gif' alt='edit' border=0>" CommandName="CANCEL" />
                                                </EditItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" Visible="true" HeaderStyle-Width="60" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ConfirmButtonExtender ID="cbeDelete" runat="server" TargetControlID="btnDelete" ConfirmText="Bent u zeker dat u deze rij wilt verwijderen ?" />
                                                    <asp:LinkButton ID="btnDelete" ToolTip="Verwijderen" runat="server" Text="<img src='../images/database_delete.png' alt='Delete' border=0>" CommandName="DELETE" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="ImagebtnAdd" ImageUrl="../images/add.gif" runat="server" ToolTip="Add" CommandName="INSERT" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    </td>
                            </tr>
                            <tr>
                               <td valign="top">
                                    <table width="95%" class="datagridPaging">
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
