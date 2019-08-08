<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptSelfCareTransaction.aspx.cs" Inherits="PrjUpassPl.Reports.rptSelfCareTransaction" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
<script type="text/javascript">

    function InProgress() {

        document.getElementById("imgrefresh2").style.visibility = 'visible';
    }
    function onComplete() {

        document.getElementById("imgrefresh2").style.visibility = 'hidden';
    }
    function goBack() {
        window.location.href = "../Reports/rptnoncasreport.aspx";
    }
    </script>

  
    <div class="maindive">
     <div style="float: right">
            <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                Back</button>
        </div>
          <%--<div>
<h2>SelfCare Transaction Report</h2>
</div>--%>

        <asp:Panel runat="server" ID="pnlSearch">
            <div class="tblSearchItm" style="width: 30%;">
                <table width="100%">
                   
                    <tr>
                        <td align="center" class="cal_image_holder">
                            From Date :
                            <asp:TextBox ID="txtFrom" runat="server" BorderWidth="1"></asp:TextBox>
                            <%--</td>
                    <td class="cal_image_holder" align="left">--%>
                            <asp:Image ID="imgFrom" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                            <cc1:CalendarExtender ID="calFrom" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgFrom"
                                TargetControlID="txtFrom">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="cal_image_holder">
                            &nbsp;&nbsp; To Date :
                            <asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>
                            <%--</td>
                    <td class="cal_image_holder" align="left">--%>
                            <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                            <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                Format="dd-MMM-yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td style="padding-left: 50px" align="center">
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button" UseSubmitBehavior="false"
                                OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <table width="100%">
                <tr>
                    <td align="left" class="style67" style="margin-left: 1%;">
                       <%-- <asp:Button runat="server" ID="btngrnExel" Text="Generate CSV" CssClass="button"
                            UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" />
                        &nbsp;&nbsp;--%>
                        <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                            UseSubmitBehavior="false" align="left" OnClick="btnGenerateExcel_Click" Visible="false" />
                    </td>
                    <td align="center" class="style68">
                            <asp:Label ID="lbldaterange" runat="server"></asp:Label>                            
                        </td>
                        <td>
                        </td>
                    <td class="style68">
                        <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                        <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            
                    <asp:GridView ID="grdSelfCareReport" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                        ShowFooter="true" Width="100%" AllowSorting="true" AllowPaging="true" PageSize="5"
                        OnRowDataBound="grdSelfCareReport_RowDataBound" OnSorting="grdSelfCareReport_Sorting"
                        OnPageIndexChanging="grdSelfCareReport_PageIndexChanging">
                        <FooterStyle CssClass="GridFooter" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Transaction ID" DataField="transactionid" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" FooterText="" />
                            <asp:BoundField HeaderText="Transaction Status" DataField="pgtransactionstatus" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Amount" DataField="amount" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="PGTransaction Id" DataField="pgtransactionid" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Acount No" DataField="accountno" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Payment Mode" DataField="paymentmode" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Email" DataField="email" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="PGRequest DateTime" DataField="pgrequestdatetime" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="PG Response DateTime" DataField="pgresponsedatetime" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Receipt No" DataField="receiptno" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Subscription Date" DataField="subscriptiondate" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Plan Expiry Date" DataField="planexpirydate" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Subscribed Transaction Status" DataField="subscribedtransactionstatus" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Transaction Description" DataField="transactiondescription" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Pack Name" DataField="packname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Plan Name" DataField="planname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Product Name" DataField="productname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Plan Call Type" DataField="plancalltype" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="IsLive" DataField="islive" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Platform Type" DataField="platformtype" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                  <asp:BoundField HeaderText="Device ID" DataField="deviceid" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                  <asp:BoundField HeaderText="City" DataField="city" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                  <asp:BoundField HeaderText="LCO Name" DataField="lco_name" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="Pack Amount" DataField="packamount" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="Refund Amount" DataField="refundamount" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="Payable Amount" DataField="payableamount" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="Discount Amount" DataField="discountamount" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="Hathway Share Percentage" DataField="hathwaysharepercentage" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="Actual Hathway Share Amount" DataField="actualhathwayshareamount" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="Actual Coshare Amount" DataField="actuallcoshareamount" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="LCO PG Name" DataField="lcopgname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="BRM POID" DataField="brm_poid" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="LCO Code" DataField="lco_code" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="JV" DataField="jv" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="Company" DataField="company" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                           <asp:BoundField HeaderText="Transaction Date" DataField="transaction_date" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />

                        </Columns>
             </asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>
