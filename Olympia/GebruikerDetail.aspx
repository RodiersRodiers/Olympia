<%@ Page Language="VB" ValidateRequest="true" AutoEventWireup="false" CodeBehind="GebruikerDetail.aspx.vb" Inherits="Olympia.GebruikerDetail" %>


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
            <asp:Label ID="lbllogin" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
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
                <a onclick="window.location.href='BeheerVergoedingen.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Vergoedingen</a>
            </div>

            <a onclick="window.location.href='login.aspx'" href="#" class="w3-bar-item w3-button w3-padding">Uitloggen</a>
        </div>
    </nav>

    <!-- !PAGE CONTENT! -->
    <div class="w3-main" style="margin-left: 250px">
        <form id="form1" runat="server">
            <uc1:UCScript ID="myUCScript1" runat="server" />
            <table style="width: 100%">
                <tr>
                    <td>
                        <!-- CONTENT -->
                        <table style="width: 95%">
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblPageTitle" CssClass="PageTitle" runat="server"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblgebruiker" Font-Bold="true" runat="server"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" class="w3-gray ">
                                    <asp:Button ID="btnDetail" runat="server" ForeColor="Blue" />
                                    <asp:Button ID="btnRechten" runat="server" />
                                    <asp:Button ID="btnOverzicht" runat="server" />
                                    <asp:Button ID="btnrapport" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblNaam" ForeColor="Red" runat="server" AssociatedControlID="txtNaam"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNaam" Font-Bold="True" runat="server" Width="175" TabIndex="2" MaxLength="150"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtNaam" runat="server" Display="None" ControlToValidate="txtNaam"
                                        SetFocusOnError="true">*</asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblVoornaam" ForeColor="Red" runat="server" AssociatedControlID="txtVoornaam"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVoornaam" Font-Bold="True" runat="server" Width="150" TabIndex="2" MaxLength="150"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtVoornaam" runat="server" Display="None" ControlToValidate="txtVoornaam"
                                        SetFocusOnError="true">*</asp:RequiredFieldValidator>
                                    &nbsp;
                                    <asp:Label ID="lblGebDatum" runat="server" AssociatedControlID="txtGebDatum"></asp:Label>
                                    &nbsp;
                                    <asp:TextBox ID="txtGebDatum" runat="server" Font-Bold="True" Width="95" TabIndex="2" MaxLength="50"></asp:TextBox>
                                    <asp:CalendarExtender ID="calDocumentDatum" runat="server" TargetControlID="txtGebDatum"
                                        Animated="false" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="rqfv_Datum" runat="server" CssClass="hidden" ControlToValidate="txtGebDatum"
                                        InitialValue="" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender runat="Server" CssClass="CustomValidatorCalloutStyle"
                                        ID="ValidatorCalloutExtender2" TargetControlID="rqfv_Datum" Width="200px" HighlightCssClass="ajax__validatorcallout_hightlight"
                                        WarningIconImageUrl="../images/validatorCallOutWarning.gif" CloseImageUrl="../images/Close.gif" />
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblGeslacht" runat="server"></asp:Label>
                                    &nbsp;
                                     <asp:ComboBox ID="cbGeslacht" AutoCompleteMode="Suggest" DropDownStyle="DropDownList"
                                         AppendDataBoundItems="False" Width="25" runat="server">
                                     </asp:ComboBox>
                                </td>
                            </tr>

                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblEmail" ForeColor="Red" runat="server" AssociatedControlID="txtEmail"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" Font-Bold="True" runat="server" Width="175" TabIndex="2" MaxLength="50"></asp:TextBox>
                                </td>
                             <td style="text-align: right">
                                    <asp:Label ID="lblGSM" runat="server"></asp:Label>
                                  </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtGSM" runat="server" Width="175" TabIndex="2" MaxLength="50"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                
                                    <asp:Label ID="lblRekNr" runat="server"></asp:Label>&nbsp;
                                
                                    <asp:TextBox ID="txtRekNr" runat="server" Width="200" TabIndex="2" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblGemeente" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGemeente" runat="server" Width="175" TabIndex="2" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblPostcode" runat="server" AssociatedControlID="txtpostcode"></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtpostcode" runat="server" Width="50" TabIndex="2" MaxLength="10"></asp:TextBox>
                                    &nbsp;
                                    <asp:Label ID="lblStraat" runat="server" AssociatedControlID="txtStraat"></asp:Label>
                                    &nbsp;
                                    <asp:TextBox ID="txtStraat" runat="server" Width="175" TabIndex="2" MaxLength="100"></asp:TextBox>
                                    &nbsp;
                                    <asp:Label ID="lblHuisnr" runat="server" AssociatedControlID="txtHuisnr"></asp:Label>
                                    &nbsp;
                                    <asp:TextBox ID="txtHuisnr" runat="server" Width="50" TabIndex="2" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblInfo" runat="server" AssociatedControlID="txtareaInfo"></asp:Label>
                                </td>
                                <td colspan="3" style="text-align: left">
                                    <textarea id="txtareaInfo" runat="server" cols="75" rows="2"></textarea>
                                    <br />
                                </td>
                            </tr>


                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btn_Opslaan" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btn_Annuleer" Visible="false" runat="server" />
                                    <asp:Button ID="btn_Changepw" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" class="w3-gray ">Lesgeververgoedingen
                                </td>
                            </tr>

                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="btnINSERTAdd" runat="server" />
                                    <asp:Button ID="btnINSERTCancel" Visible="false" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:DataGrid ID="dtgDataGrid" RowHighlightColor="#cccccc" RowClickColor="#ebe0fb"
                                        RowSelectionEnabled="true" HorizontalAlign="Center" RowClickEventCommandName="dtgDataGrid_EditCommand"
                                        runat="server" AutoGenerateColumns="false" CssClass="navigateable" GridLines="Horizontal"
                                        Width="90%" PageSize="40" AllowPaging="true" BorderWidth="1" AllowCustomPaging="false"
                                        PagerStyle-Visible="false" AllowSorting="true" AlternatingItemStyle-BackColor="WhiteSmoke" DataKeyField="ID" ShowFooter="false">
                                        <ItemStyle CssClass="datagridItem" />
                                        <FooterStyle CssClass="datagridItem" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-Width="80" SortExpression="Datum" HeaderText="Datum">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatum" Width="80" runat="server"></asp:Label>
                                                    <asp:Label ID="lblLidID" Visible="false" runat="server"></asp:Label>
                                                    <asp:Label ID="lblID" Visible="false" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:label ID="txtEditDatum" Width="85" runat="server"></asp:label>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtInsertDatum" Width="85" runat="server"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calDocumentDatum" runat="server" TargetControlID="txtInsertDatum"
                                                        Animated="false" Format="dd/MM/yyyy" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" SortExpression="bedrag" HeaderStyle-Width="60" HeaderText="bedrag">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbedrag" Width="60" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txteditbedrag" Width="60" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtInsertbedrag" Width="60" runat="server"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderStyle-CssClass="datagridHeaderNonSort" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" SortExpression="km" HeaderStyle-Width="60" HeaderText="Km">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKm" Width="60" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txteditKm" Width="60" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtInsertKm" Width="60" runat="server"></asp:TextBox>
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
                                                    <textarea id="txtInsertinfo" runat="server" cols="30" rows="2"></textarea>
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
                                                    <asp:ConfirmButtonExtender ID="cbeDelete" runat="server" TargetControlID="btnDelete" ConfirmText="Bent u zeker dat u deze discipline wilt verwijderen ?" />
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
                                <td colspan="4">
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
            <b onclick="window.location.href='privacy.aspx'">Privacy</b>&nbsp;&nbsp;&nbsp;
        </div>

    </div>

    <script>
        document.getElementById("myBtn2").click();
        document.getElementById("myBtn1").click();
    </script>

</body>
</html>
