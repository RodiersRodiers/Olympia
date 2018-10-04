<%@ Control Language="VB" AutoEventWireup="false" Inherits="Olympia.CustomMessage" CodeBehind="CustomMessage.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:AnimationExtender ID="AnimationExtender1" Enabled="true" runat="server"
    TargetControlID="TBLMessage" BehaviorID="controlledAnimation" />
<asp:Panel ID="pnlMessage" runat="server" Visible="false">
    <table width="100%" cellspacing="0" cellpadding="0" border="0" id="TBLMessage" runat="server"
        visible="false">
        <tr>
            <td align="right" width="10px">
                <asp:Image ID="imgMsgLeft" runat="server" />
            </td>
            <td align="center" class="TR_Msg">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Image ID="imgMessage" runat="server" ImageUrl="/images/lamp.gif" />
            </td>
            <td class="TR_Msg" width="100%">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
            <td class="TR_Msg">
                <asp:ImageButton ID="btnClose" CausesValidation="false" ImageUrl="images/close.gif"
                    runat="server" />
            </td>
            <td align="left">
                <asp:Image ID="imgMsgRight" runat="server" />
            </td>
        </tr>
    </table>
</asp:Panel>
