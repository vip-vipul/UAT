<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransBalanceManagementPages.aspx.cs" Inherits="PrjUpassPl.Transaction.TransBalanceManagementPages" %>

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
            pad: </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:Panel ID="pnlView" runat="server">
        <div class="maindive">
            <table border="0" align="center" style="margin-top: 90px;">
                <tr>
                    <td colspan="4" class="tdclass">
                        <a href="../Transaction/TransHwayUserCreditLimit_New.aspx">
                            <img src="../IconImage/balance allocation Icon.jpg" alt="" border="0" height="130"
                                width="130"></img></a>
                    </td>
                    <td colspan="4" class="tdclass">
                        <a href="../Transaction/transhwaybilldetails.aspx">
                            <img src="../IconImage/Account Invoice Details.jpg" alt="" border="0" height="130"
                                width="130"></img></a>
                    </td>
                    <td colspan="4" class="tdclass">
                        <a href="../Transaction/HwayTransLcoOnlinePayment.aspx">
                            <img src="../IconImage/online payment Icon.jpg" alt="" border="0" height="130" width="130"></img></a>
                    </td>
                    <td colspan="4" class="tdclass">
                        <a href="../Transaction/transhwaylcoinvoicedetails.aspx">
                            <img src="../IconImage/LCO Invoice Details.jpg" alt="" border="0" height="130" width="130"></img></a>
                    </td>
                    <td colspan="4" class="tdclass">
                        <a href="../Reports/rptLCOAllDetailsNew.aspx">
                            <img src="../IconImage/Transaction Summary.jpg" alt="" border="0" height="130" width="130"></img></a>
                    </td>
                    <td colspan="4" class="tdclass">
                        <a href="../Transaction/HwayTransLCOSTBInventoryConformation.aspx">
                            <img src="../IconImage/online payment Icon.jpg" alt="" border="0" height="130" width="130"></img></a>
                    </td>
                    <td colspan="4" class="tdclass">
                        <a href="../Transaction/frmNewSTBSP.aspx">
                            <img src="../IconImage/Account Invoice Details.jpg" alt="" border="0" height="130" width="130"></img></a>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
