<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="mstLCOAdminPages.aspx.cs" Inherits="PrjUpassPl.Master.mstLCOAdminPages" %>
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
                <a href="../Master/frmLCOPreUserDefine.aspx">
                    <img src="../IconImage/User Creation Icon.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
            <td colspan="4" class="tdclass">
                <a href="../Master/TransAssignBtnRights.aspx">
                    <img src="../IconImage/User Creation Icon.jpg" alt="" border="0" height="130" width="130"></img></a>
                    <br />
                    <asp:Label ID="lblTest" runat="server" Text="User Rights Update"></asp:Label>
            </td>
            <td colspan="4" class="tdclass">
                <a href="../Transaction/transUserAccountMapping.aspx">
                    <img src="../IconImage/User Creation Icon.jpg" alt="" border="0" height="130" width="130"></img></a>
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="User Account Mapping"></asp:Label>
            </td>
              <td colspan="4" class="tdclass">
                <a href="../Reports/rptLCOAllDetails.aspx">
                    <img src="../IconImage/LCO Details.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>

            <td colspan="4" class="tdclass" style="display:none">
                <a href="../Master/frmOtherLcoDetails.aspx">
                    <img src="../IconImage/LCO Other Details.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
         <td colspan="4" class="tdclass">
                <a href="frmgstn.aspx" target="_blank">
                    <img src="../IconImage/LCO GSTN.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
        <td colspan="4" class="tdclass">
                <a href="mstComplaintRegistration.aspx">
                    <img src="../IconImage/User Creation Icon.jpg" alt="" border="0" height="130" width="130"></img></a>
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Complaint Registration"></asp:Label>
            </td>
        </tr>
        
   
    </table>
        </div>
    </asp:Panel>
</asp:Content>
