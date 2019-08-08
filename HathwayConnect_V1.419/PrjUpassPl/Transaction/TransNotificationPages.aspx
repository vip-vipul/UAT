<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransNotificationPages.aspx.cs" Inherits="PrjUpassPl.Transaction.TransNotificationPages" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         .blue
        {
            color: DarkBlue;
        }
        .red
        {
            color: Red;
        }
        .logo
        {
            font-size: 50px;
        }
        .subt
        {
            font-size: 25px;
        }
        .tdclass
        {
            padding-left: 75px;
            padding-bottom: 20px;
            padding-top: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:Panel ID="pnlView" runat="server">
        <div class="maindive">
              <table border="0" align="center" style="margin-top: 90px;">
        <tr>
            <td colspan="4" class="tdclass">
                <a href="../Transaction/TransOsdBmailNotification.aspx?flag=sms">
                    <img src="../IconImage/sms Icon.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
            <td colspan="4" class="tdclass">
                <a href="../Transaction/TransHwayOsdBmailFlagMap.aspx">
                    <img src="../IconImage/OSD Bmail Deactivation.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
            <td colspan="4" class="tdclass">
                <a href="../Transaction/TransOsdBmailNotification.aspx">
                    <img src="../IconImage/osdbmailIcon.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
         
        
        </tr>
        
   
    </table>
        </div>
    </asp:Panel>
</asp:Content>
