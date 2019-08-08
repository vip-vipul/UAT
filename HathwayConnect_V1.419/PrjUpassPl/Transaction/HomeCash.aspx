<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="HomeCash.aspx.cs" Inherits="PrjUpassPl.Transaction.HomeCash" %>


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
            padding-left: 30px;
            padding-bottom: 20px;
            padding-top: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <table border="0" align="center" style="margin-top: 90px;">
        <tr>
          <td colspan="4" class="tdclass">
                <a href="../Transaction/frmCustomerSearch.aspx">
                    <img src="../Img/Customer Details.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
              <td colspan="4" class="tdclass">
                <a href="../Transaction/frmCustomerSearch.aspx">
                    <img src="../Img/Customer Details.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
          
         
            <td colspan="4" class="tdclass">
                <a href="../Transaction/TransHwayLcoPayment.aspx">
                    <img src="../Img/Balance.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
            <td colspan="4" class="tdclass">
          <a href="../Reports/rptnoncasreport.aspx">
                    <img src="../Img/Report.jpg" alt="" border="0" height="130" width="130"></img></a>
              
            </td>
         
          
        </tr>
        <%--     <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/LCO Admin.jpg" alt="" border=0 height=100 width=100></img></td>
        <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/Pack Management.jpg" alt="" border=0 height=100 width=100></img></td>
        <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/Balance.jpg" alt="" border=0 height=100 width=100></img></td>
        <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/Bulk.jpg" alt="" border=0 height=100 width=100></img></td>
        <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/Customer Details.jpg" alt="" border=0 height=100 width=100></img></td></tr>
        --%>
    </table>
</asp:Content>
