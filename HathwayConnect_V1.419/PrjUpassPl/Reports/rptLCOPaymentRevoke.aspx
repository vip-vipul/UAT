<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptLCOPaymentRevoke.aspx.cs"
    Inherits="PrjUpassPl.Reports.rptLCOPaymentRevoke" MasterPageFile="~/MasterPage.Master" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .transparent
        {
            /* IE 8 */
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=50)"; /* IE 5-7 */
            filter: alpha(opacity=50); /* Netscape */
            -moz-opacity: 0.5; /* Safari 1.x */
            -khtml-opacity: 0.5; /* Good browsers */
            opacity: 0.5;
        }
        .loader
        {
            position: fixed;
            text-align: center;
            height: 100%;
            width: 100%;
            top: 0;
            right: 0;
            left: 0;
            z-index: 9999999;
            background-color: #FFFFFF;
            opacity: 0.3;
            visibility: hidden;
        }
        .loader img
        {
            padding: 10px;
            position: fixed;
            top: 45%;
            left: 50%;
        }
    </style>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <div class="tblSearchItm" style="width: 30%;">
                        <table width="100%">
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    From Date :
                                    <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>
                                    <%--</td>
                    <td class="cal_image_holder" align="left">--%>
                                    <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                        Format="dd-MMM-yyyy">
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
                                <td style="padding-left: 40px" align="center">
                                    <%-- &nbsp;&nbsp;&nbsp;&nbsp;--%>
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
                            <td align="left" class="style67">
                                <asp:Button runat="server" ID="btngrnExel" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" />
                            </td>
                            <td class="style68">
                                <asp:Label ID="lbldaterange" runat="server"></asp:Label>
                                <%-- <asp:Label ID="lblSearchParams" runat="server" ></asp:Label>
           <asp:Label ID="lblResultCount" runat="server" ></asp:Label>--%>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                   <div id="DivRoot" runat="server" align="left" style="width: 100%;display:none">
                        <div style="overflow: hidden;width:100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll;width:100%" onscroll="OnScrollDiv(this)" id="DivMainContent" 


                        <asp:GridView ID="grdLCOPayRev" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                            ShowFooter="true" AllowSorting="true" AllowPaging="true" PageSize="100" OnPageIndexChanging="grdLCOPayRev_PageIndexChanging"
                            OnRowDataBound="grdLCOPayRev_RowDataBound">
                            <FooterStyle CssClass="GridFooter" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20px"
                                    ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                        <asp:HiddenField ID="hdnChqBouncedDt" runat="server" Value='<%# Eval("dat_lcopay_chequebouncedt").ToString()%>' />
                                        <asp:HiddenField ID="hdnPayMode" runat="server" Value='<%# Eval("PAYMENT_MODE").ToString()%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--  <asp:BoundField HeaderText="Transaction ID" DataField="num_lcopay_transid" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="center"   />--%>
                                <asp:BoundField HeaderText="LCO Code" DataField="var_lcopay_lcocode" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField HeaderText="LCO Name" DataField="var_lcomst_name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="center" />
                                <%--<asp:BoundField HeaderText="Company Code" DataField="var_lcopay_companycode" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                    ItemStyle-HorizontalAlign="center"   />--%>
                                <asp:BoundField HeaderText="Receipt No" DataField="var_lcopay_voucherno" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="150px" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField HeaderText="Payment Mode" DataField="PAYMENT_MODE" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="150px" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField HeaderText="Cheque/Ref/DD No" DataField="cheque_no" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="150px" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField HeaderText="Bank" DataField="bank" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="150px" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField HeaderText="Branch" DataField="branch" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="150px" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField HeaderText="Amount" DataField="num_lcopay_amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="right" />
                                <asp:BoundField HeaderText="Reason" DataField="var_reason_name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="250px" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField HeaderText="Remark" DataField="var_lcopay_remark" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Company Name" DataField="companyname" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Distributor" DataField="distributor" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Sub Distributor" DataField="subdistributor" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="State" DataField="statename" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="City" DataField="cityname" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Payment By" DataField="cashier" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="150px" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField HeaderText="Inserted By" DataField="var_lcopay_insby" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Payment Date" DataField="payment_dt" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Reversal Date" DataField="date1" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="130px" ItemStyle-HorizontalAlign="center" />
                            </Columns>
                        </asp:GridView>
                    
                        </div>
                        <div id="DivFooterRow" style="overflow: hidden">
                        </div>
                    </div>

                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                     <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:PostBackTrigger ControlID="btngrnExel" />
        </Triggers>
    </asp:UpdatePanel>
    <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server"
        TargetControlID="UpdatePanel1">
        <Animations>
            <OnUpdating>
               <Parallel duration="0">
                    <ScriptAction Script="InProgress();" /> 
               </Parallel>
            </OnUpdating>
            <OnUpdated>
               <Parallel duration="0">
                   <ScriptAction Script="onComplete();" /> 
               </Parallel>
            </OnUpdated>
        </Animations>
    </cc1:UpdatePanelAnimationExtender>
</asp:Content>
