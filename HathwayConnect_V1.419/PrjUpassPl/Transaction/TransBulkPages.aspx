<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TransBulkPages.aspx.cs" Inherits="PrjUpassPl.Transaction.TransBulkPages" %>
<%@ mastertype virtualpath="~/MasterPage.Master" %>
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
            padding-bottom: 0px;
            padding-top: 0px;
        }
        .delInfo33
        {
            padding: 5px;
            border: 1px solid #094791;
            width: 1055px;
            height:auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
 <asp:Panel ID="pnlView" runat="server">
      <div class="maindive">
       <table width="100%" style="    padding-left: 66px;">
       <tr>
        <td >
           <asp:LinkButton ID="lnkDetail"  Height="18px" runat="server" style="display: inline-block; font-weight: bold; height: 18px; padding: 10px;"
                                        Text="Bulk Transaction" OnClick="lnkatag_Click" ></asp:LinkButton>
        
        <asp:LinkButton ID="lnkDetail1" Height="18px" runat="server" Style="display: inline-block; font-weight: bold; height: 18px; padding: 10px; "
                                        Text="Bulk  Scheduler Transaction"  OnClick="lnkatag1_Click"></asp:LinkButton>
        </td>
        </tr>
       </table>
       <div class="delInfo33">
       <table border="0" align="center" style="margin-top: 40px;">
        
        <tr id="tr1" runat="server" visible="true">
          <td  class="tdclass" style=" padding-left: 10px;">
                <a href="../Transaction/TransHwayBulkOperation.aspx">
                    <img src="../IconImage/Bulk Transaction.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
            <td  class="tdclass" >
                <a href="../Transaction/TransHwayBulkRenewal.aspx">
                    <img src="../IconImage/Bulk Renewal.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
          <td colspan="4" class="tdclass" >
                <a href="../Transaction/TransHwayBulkOperation.aspx?Bulk=Change">
                    <img src="../IconImage/Bulk Change Plan.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>

             <td colspan="4" class="tdclass">
                <a href="../Transaction/TransHwayBulkActDct.aspx">
                    <img src="../IconImage/Bulk Discount.jpg" alt="" border="0" height="130" width="130"></img></a>
                    <br />
                    <asp:Label ID="Label2" Text="Bulk Active/Deactive" runat="server"> </asp:Label>
            </td>
            <td colspan="4" class="tdclass">
                <a href="../Transaction/TransHwayBulkAutoRenewal.aspx">
                    <img src="../IconImage/Bulk Discount.jpg" alt="" border="0" height="130" width="130"></img></a>
                    <br />
                    <asp:Label ID="Label5" Text="Bulk Auto Renewal" runat="server"> </asp:Label>
            </td>
          <td colspan="4" class="tdclass">
                <a href="../Transaction/TransHwayBulkCustumerAddEcaf.aspx">
                    <img src="../IconImage/Bulk Discount.jpg" alt="" border="0" height="130" width="130"></img></a>
                    <br />
                    <asp:Label ID="Label4" Text="Bulk Customer Creation" runat="server"> </asp:Label>
            </td>
          </tr>
        <tr id="tr2" runat="server" visible="false">
        
        <%--<td colspan="4" class="tdclass">
                <a href="../Transaction/TransHwayBulkActivation.aspx">
                    <img src="../IconImage/Bulk Discount.jpg" alt="" border="0" height="130" width="130"></img></a>
                    <br />
                    <asp:Label ID="Label3" Text="Bulk Change Transaction" runat="server"> </asp:Label>
            </td>
              <td colspan="4" class="tdclass">
                <a href="../Transaction/TransHwayBulkActivation.aspx?Bulk=Change">
                    <img src="../IconImage/Bulk Change Plan.jpg" alt="" border="0" height="130" width="130"></img></a>
                    <br />
                    <asp:Label ID="Label4" Text="Bulk Scheduler Change Transaction" runat="server"> </asp:Label>
            </td>--%>
        <td colspan="4" class="tdclass">
                <a href="../Transaction/TransHwayBulkActDctScheduler.aspx">
                    <img src="../IconImage/Bulk Change Plan.jpg" alt="" border="0" height="130" width="130"></img></a>
                    <br />
                    <asp:Label ID="Label1" Text="Bulk Active/Deactive Scheduler" runat="server"> </asp:Label>
            </td>
            <td colspan="4" class="tdclass">
          <a href="../Transaction/tranBulkschedulerReport.aspx">
                    <img src="../Img/Report.jpg" alt="" border="0" height="130" width="130"></img></a>
              
            </td>
        </tr>
    </table>
    </div>
        </div>
    </asp:Panel>
</asp:Content>
