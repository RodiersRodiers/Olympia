<%@ Page Language="VB" ValidateRequest="true" AutoEventWireup="false" CodeBehind="Aanwezigheden.aspx.vb" Inherits="Olympia.Aanwezigheden" %>

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
                                <a onclick="window.location.href='BeheerVergoedingen.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Vergoedingen</a>   </div>

            <a onclick="window.location.href='login.aspx'" href="#" class="w3-bar-item w3-button w3-padding">Uitloggen</a>
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
                                <td colspan="2" style="align-content: flex-start">
                                    <asp:Label ID="lblPageTitle" CssClass="PageTitle" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblkeuze" runat="server"></asp:Label>
                                    <asp:ComboBox ID="cbDiscipline" runat="server" AutoPostBack="true"></asp:ComboBox> &nbsp;
                                    <asp:ComboBox ID="cbGroep" Width="200" Font-Bold="true" runat="server" AutoPostBack="true"></asp:ComboBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txtDatum"  BorderStyle="Double" BorderColor="blue" Width="140" Font-Size="X-Large" Font-Bold="true" ForeColor="blue" runat="server" AutoPostBack="true" ></asp:TextBox>
                                    <asp:CalendarExtender ID="calDocumentDatum" runat="server" TargetControlID="txtDatum"
                                                        Animated="false" Format="dd/MM/yyyy" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="align-content: flex-start">
                                    <asp:Button ID="btnINSERTAdd" runat="server" />
                                    <asp:Button ID="btnINSERTCancel" Visible="false" runat="server" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="align-content: flex-start">
                                    <%--  <uc3:UCMessage ID="UC_Message" runat="server" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td>

                                    <asp:DataGrid ID="dtgrid" runat="server" AutoGenerateColumns="false" AlternatingItemStyle-BackColor="Wheat" CssClass="navigateable" GridLines="Horizontal" Width="95%" PageSize="50" AllowPaging="true" BorderWidth="1" AllowCustomPaging="false" PagerStyle-Visible="false" AllowSorting="true" DataKeyField="id" ShowFooter="false">
                                        <ItemStyle CssClass="datagridItem" />
                                        <FooterStyle CssClass="datagridItem" />
                                        <Columns>

                                            <asp:TemplateColumn Visible="false" HeaderStyle-CssClass="datagridHeaderNonSort" HeaderText="ID1">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" SortExpression="ID_Actie" Visible="true" HeaderText="functie" ItemStyle-Width="80" HeaderStyle-Width="80" FooterStyle-Width="80"
                                                HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfunctie" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" SortExpression="naam" Visible="true" HeaderText="Lid" ItemStyle-Width="150" HeaderStyle-Width="150" FooterStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllid" runat="server"></asp:Label>
                                                    <asp:Label ID="lblIDLid" Visible="false" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" SortExpression="info" HeaderText="aanwezig" ItemStyle-Width="60" HeaderStyle-Width="60" FooterStyle-Width="60"
                                                HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="lblaanwezig" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkbEditaanwezig" runat="server" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="chkbInsertaanwezig" runat="server" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                               <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" SortExpression="opmerking" HeaderText="Opmerking" ItemStyle-Width="60" HeaderStyle-Width="60" FooterStyle-Width="60"
                                                HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:label ID="lblopmerking" runat="server"></asp:label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:textbox ID="txtEditopmerking" runat="server" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:textbox ID="txtInsertopmerking" runat="server" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn  HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-Width="60" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkbEdit" CssClass="activation" ToolTip="Edit" runat="Server" Text="<img src='../images/edit.gif' alt='edit' border=0>" CommandName="EDIT" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkbUpdate" ToolTip="Update" runat="Server" Text="<img src='../images/update.gif' alt='update' border=0>" CommandName="UPDATE" />
                                                    <asp:LinkButton ID="lnkbCancel" ToolTip="Cancel" runat="Server" Text="<img src='../images/cancel.gif' alt='edit' border=0>" CommandName="CANCEL" />
                                                </EditItemTemplate>
                                            </asp:TemplateColumn>
                                            
                                        </Columns>
                                    </asp:DataGrid>
                                </td>
                            </tr>


                        </table>
                        <br />
                        <br />
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
