<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Register.aspx.vb" Inherits="Olympia.RegisterOlympia" %>

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
<body>
    <form id="form1" runat="server">

        <table width="100%" style="align-content: center">
            <tr>
                <td valign="top" align="center" >
                    <br />
                    <img src="images/groep.png" alt="" style="width: 240px; height: 200px;" />
                    <br />
                    <br />
                    <h2>Registreren</h2>
                    <table width="25%" style="align-content: center">


                        <tr>
                            <td align="right">

                                <asp:Label ID="lblEmail" Font-Bold="True" runat="server" AssociatedControlID="txtEmail"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" TabIndex="2" MaxLength="255"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" Display="None" ControlToValidate="txtEmail"
                                    SetFocusOnError="true">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">

                                <asp:Label ID="lblPaswoord" Font-Bold="True" runat="server" AssociatedControlID="txtPaswoord"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPaswoord" runat="server" Width="100%" TabIndex="2" MaxLength="255"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtPaswoord" runat="server" Display="None" ControlToValidate="txtPaswoord"
                                    SetFocusOnError="true">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">

                                <asp:Label ID="lblPaswoordConfirm" Font-Bold="True" runat="server" AssociatedControlID="txtPaswoordConfirm"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPaswoordConfirm" runat="server" Width="100%" TabIndex="2" MaxLength="255"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtPaswoordConfirm" runat="server" Display="None" ControlToValidate="txtPaswoordConfirm"
                                    SetFocusOnError="true">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">

                                <asp:Label ID="lblNaam" Font-Bold="True" runat="server" AssociatedControlID="txtNaam"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtNaam" runat="server" Width="100%" TabIndex="2" MaxLength="255"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtNaam" runat="server" Display="None" ControlToValidate="txtNaam"
                                    SetFocusOnError="true">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">

                                <asp:Label ID="lblVoornaam" Font-Bold="True" runat="server" AssociatedControlID="txtVoornaam"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtVoornaam" runat="server" Width="100%" TabIndex="2" MaxLength="255"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtVoornaam" runat="server" Display="None" ControlToValidate="txtVoornaam"
                                    SetFocusOnError="true">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="left">
                                <asp:Button ID="btn_Registreer" runat="server" />

                            </td>
                            <td align="right">
                                <asp:Button ID="btn_Annuleer" runat="server" />

                            </td>
                        </tr>
                    </table>
</td>
       </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <strong>claimer</strong><br />
                            <sub>Registraties kunnen voorwerp uitmaken van controle op echtheid.
Door gebruik te maken van de databank erkent de gebruiker impliciet zich te houden aan deze voorwaarden en de regels van toepassing op de wetgeving GDPR.</sub>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:LinkButton ID="lnkbprivacy" runat="server" />
                        </td>
                    </tr>

        </table>
    </form>
</body>
</html>
