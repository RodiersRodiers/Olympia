<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="BoodschapCompose.aspx.vb" ValidateRequest="true"
    EnableEventValidation="false" Inherits="Olympia.BoodschapCompose" %>

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
    <script type="text/javascript">
        function closeIt() {
            parent.tb_remove();
            parent.location.reload(1);
        }


        function ace_itemSelectedIdEenheid(sender, e) {
            var hdEenheidID = document.form1.hdCBEenheid;
            hdCBEenheid.value = e.get_value();
        }


        function textCounter(field, countfield, maxlimit) {
            if (field.value.length > maxlimit) {
                field.value = field.value.substring(0, maxlimit);
                return false;
            }
            else {
                countfield.value = maxlimit - field.value.length;
            }
        }

        function eraseEdit() {
            document.getElementById('txtStamnummers').value = '';
            document.getElementById('txtOntvanger').value = '';
        }


        function addRecipient() {
            var hddStamnummer = document.getElementById("cbDossierBeheerder.clientId").value;

            if (document.getElementById("cbDossierBeheerder.clientId%>").selectedIndex > 0) {
                var e = document.getElementById("cbDossierBeheerder.clientId");
                document.getElementById("txtOntvanger.clientId%>").value += e.options[e.selectedIndex].text + ";";
                document.getElementById("txtStamnummers.clientId%>").value += e.options[e.selectedIndex].value + ";";
            } else {
                var e = document.getElementById("cbDossierBeheerder.clientId");
                for (i = 1; i < e.options.length; i++) {
                    if (e.options[i].value != hddStamnummer) {
                        document.getElementById("txtOntvanger.clientId").value += e.options[i].text + ";";
                        document.getElementById("txtStamnummers.clientId").value += e.options[i].value + ";";
                    }
                }
            }
        }
                

    </script>
</head>
<body style="margin: 0px;">
    <form id="form1" name="myForm" runat="server">
       <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>
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
            <td width="5%">
            </td>
            <td valign="top" align="left">
                <table width="100%" border="0">
                    <tr>
                        <td colspan="2">
                            <table border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbleenheid" runat="server">:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTeam" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblGebruiker" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtEenheid" AutoPostBack="true" autocomplete="off" runat="server"></asp:TextBox>
                                        <asp:HiddenField ID="hdCBEenheid" runat="server" />
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" ServiceMethod="GetEenhedenSuggest"
                                            ServicePath="../KISSService.asmx" OnClientItemSelected="ace_itemSelectedIdEenheid"
                                            TargetControlID="txtEenheid" MinimumPrefixLength="1" CompletionInterval="1000"
                                            CompletionSetCount="20" CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                        </asp:AutoCompleteExtender>
                                        <asp:HiddenField ID="hdCustIDEenheid" runat="server" />
                                        <asp:FilteredTextBoxExtender ID="fte6" runat="server" FilterMode="InvalidChars"
                                            InvalidChars="<>&" TargetControlID="txtEenheid" />
                                    </td>
                                    <td>
                                        <asp:ComboBox ID="cbTeam" DropDownStyle="DropDownList" AppendDataBoundItems="false"
                                            AutoCompleteMode="SuggestAppend" AutoPostBack="true" runat="server">
                                        </asp:ComboBox>
                                    </td>
                                    <td>
                                        <asp:ComboBox ID="cbDossierBeheerder" DropDownStyle="DropDownList" AppendDataBoundItems="false"
                                            AutoCompleteMode="SuggestAppend" AutoPostBack="true" runat="server">
                                        </asp:ComboBox>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkAdd" CssClass="clickMeButton" runat="server" OnClientClick="addRecipient(); return false;"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br/>
                        </td>
                    </tr>
                    <tr>
                        <td class="instruction">
                            <asp:Label ID="lblOntvanger" runat="server"></asp:Label>&nbsp;
                            <asp:TextBox ID="txtStamnummers" runat="server" MaxLength="255" CssClass="hidden"></asp:TextBox>
                            <asp:TextBox ID="txtOntvanger" Width="90%" runat="server"></asp:TextBox>
                            <asp:LinkButton Text="<img src='../../images/erase.png' alt='clear' border=0>" ID="lnkbDelete"
                                OnClientClick="eraseEdit();return false;" runat="server"></asp:LinkButton>
                            <asp:HiddenField ID="hddStamnummerSession" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="instruction">
                            <asp:Label ID="lblOnderwerp" runat="server"></asp:Label>&nbsp;
                            <asp:TextBox ID="txtOnderwerp" Width="90%" MaxLength="200" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="fte1" runat="server" FilterMode="InvalidChars"
                                InvalidChars="<>&" TargetControlID="txtOnderwerp">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br>
                        </td>
                    </tr>
                    <tr>
                        <td class="instruction">
                            <asp:Label ID="lblInhoud" Visible="false" runat="server"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <textarea id="txtAreaInhoud" onchange="this.value=this.value.replace(/[<> ]+/ig,' ')"
                                onkeypress="textCounter(this,this.form.counter,19500);" name="txtInhoud" runat="server"
                                rows="10" cols="100"></textarea>
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
                            <asp:LinkButton ID="lnkSend" CssClass="clickMeButton" Width="200px" runat="server"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
