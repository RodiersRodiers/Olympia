<%@ Page Language="VB" ValidateRequest="true" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="Olympia.LoginOlympia" %>

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
                    <h2>Inloggen</h2>

                    Als je nog geen Login hebt  <a><strong><font color="red"> - 
                        <asp:LinkButton ID="LnkbRegistreer" runat="server" />
                        - </font></strong></a>dan eerst !
                </td>
            </tr>
            <tr>
                <td  align="center">
              <table width="25%" style="align-content: center">
            <tr>
                <td align="center">

                    <asp:Label ID="lblGebruiker" Font-Bold="True" runat="server" AssociatedControlID="txtGebruiker"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtGebruiker" runat="server" Width="250" TabIndex="1" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtGebruiker" runat="server" Display="None" ControlToValidate="txtGebruiker"
                        SetFocusOnError="true">*</asp:RequiredFieldValidator>
                </td>
            </tr>
          
            <tr>
                <td align="center">
                    <asp:Label ID="lblPassword" Font-Bold="True" runat="server" AssociatedControlID="txtPassword"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Width="250" TabIndex="2" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtPassword" runat="server" Display="None" ControlToValidate="txtPassword"
                        SetFocusOnError="true">*</asp:RequiredFieldValidator>
                </td>
            </tr>
    </table>
                </td>
            </tr>
            <tr>
                <td  align="center">
                    <asp:Button ID="btn_Login" runat="server" />
                </td>
            </tr>
            <tr>
                 <td  align="center">
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


