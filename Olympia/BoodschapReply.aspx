<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BoodschapReply.aspx.vb" Inherits="Olympia.BoodschapReply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%@ register tagprefix="uc1" tagname="UCScript" src="../../CustomScriptControl.ascx" %>
<%@ Register TagPrefix="uc2" TagName="UCMessage" Src="../../CustomMessage.ascx" %>
<head runat="server">
    <title></title>
    <script src="../../scripts/jquery.js" type="text/javascript"></script>
    <link href="../../Styles.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin: 0px;">
    <form id="form1" runat="server">
    <uc1:UCScript ID="myUCScript1" runat="server" />
    <table width="100%" cellspacing="0" cellpadding="0">
        <tr>
            <td colspan="3" style="background-image: url('../../images/entiteiten/entiteitenHeader.gif');
                background-repeat: repeat;">
                <table cellspacing="0" cellpadding="0" width="100%" height="56" border="0" width="100%">
                    <tr>
                        <td width="150px" style="background-image: url('../../images/entiteiten/entiteitenHeaderLeft.gif');
                            background-repeat: no-repeat; background-position: left;">
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblVattingNummer" Font-Size="13" ForeColor="white" Font-Bold="true"
                                runat="server" />
                        </td>
                        <td align="right" valign="bottom" style="background-image: url('../../images/entiteiten/entiteitenHeaderRight.gif');
                            background-repeat: no-repeat; background-position: top right;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
            </td>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td width="10%">
            </td>
            <td valign="top" align="left">
                <table width="80%" border="0">
                    <tr>
                        <td class="instruction" width="100">
                            <asp:Label ID="lblAfzender" Font-Bold="true" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAfzender2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="instruction">
                            <asp:Label ID="lblOnderwerp" Font-Bold="true" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOnderwerp2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="instruction" colspan="2">
                            <asp:Label ID="lblInhoud" Font-Bold="true" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" width="100%" colspan="2">
                            <textarea id="txtAreaInhoud" onkeypress="textCounter(this,this.form.counter,19500);"
                                name="txtInhoud" runat="server" rows="10" cols="100"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmaxkar" Visible="false" runat="server"></asp:Label>
                            <input type="text" style="font-size: smaller;" name="counter" maxlength="3" size="15"
                                value="19500" onblur="textCounter(this.form.counter,this,19500);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="padding-top: 10px;">
                            <asp:LinkButton ID="lnkReply" CssClass="clickMeButton" Width="200px" runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnkDoosturen" CssClass="clickMeButton" Width="200px" runat="server"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="10%">

            </td>
        </tr>
        <tr>
            <td colspan=3>
            <uc2:UCMessage ID="UCMessage1" runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
