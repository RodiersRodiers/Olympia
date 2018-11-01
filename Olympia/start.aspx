<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="start.aspx.vb" Inherits="Olympia.start" %>
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
                                <a onclick="window.location.href='BeheerVergoedingen.aspx'" href="#" class="w3-bar-item w3-button w3-light-grey"><i class="fa fa-caret-right w3-margin-right"></i>Vergoedingen</a>       

            </div>

            <a onclick="window.location.href='meldingen.aspx'" href="#" class="w3-bar-item w3-button">Meldingen</a>
            <a onclick="window.location.href='login.aspx'" href="#" class="w3-bar-item w3-button w3-padding">Uitloggen</a>
        </div>
    </nav>

    <!-- !PAGE CONTENT! -->
    <div class="w3-main" style="margin-left: 250px">
        <form id="form1" runat="server">
            <uc1:UCScript ID="myUCScript1" runat="server" />
            <table style="width: 100%">
        <tr>
            <td style="vertical-align:top; align-content:center">
                <!-- CONTENT -->
                <table style="width:85%" >
                    <tr>
                            <td  style=" align-content:flex-start">
                            <asp:Label ID="lblPageTitle" CssClass="PageTitle" runat="server"></asp:Label>
                            <br />
                            <br />
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
