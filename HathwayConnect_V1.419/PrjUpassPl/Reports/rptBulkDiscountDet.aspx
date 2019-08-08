<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptBulkDiscountDet.aspx.cs" Inherits="PrjUpassPl.Reports.rptBulkDiscountDet" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.GridFooter
        {
            border: 1px solid #094791;
            border-radius: 0px;
            color: White;
            background: #094791;
            width: 100px;
        }*/
        
        .cal_image_holder
        {
            width: 7%;
        }
        .Grid th a
        {
            color: #ffffff;
            cursor: pointer;
        }
        /*.style67
        {
            width: 142px;
        }
        .style68
        {
            width: 975px;
        }*/
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
             return false;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
            <div class="maindive">
            <div style="float:right">
                <button onclick="return goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
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
                            <td style="padding-left: 50px" align="center">
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button" UseSubmitBehavior="false"
                                    OnClick="btnSubmit_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <table width="100%">
                    <tr>
                        <td align="left" class="style67">
                            <asp:Button runat="server" ID="btngrnExel" Text="Generate CSV" CssClass="button"
                                UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" />
                            &nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                UseSubmitBehavior="false" align="left" OnClick="btnGenerateExcel_Click" />
                        </td>
                        <td class="style68">
                            <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                            <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <div class="griddiv">
                <asp:GridView ID="grdDiscount" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                    ShowFooter="true" Width="100%" AllowSorting="true" AllowPaging="true" PageSize="100"
                    OnSorting="grdDiscount_Sorting" OnPageIndexChanging="grdDiscount_PageIndexChanging">
                    <FooterStyle CssClass="GridFooter" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Account No." DataField="accno" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="VC/MAC Id" DataField="vc" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="First Name" DataField="fname" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="Last Name" DataField="lname" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="Address" DataField="address" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="Zip" DataField="zip" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="City" DataField="city" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="State" DataField="state" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="Customer Type" DataField="custtype" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="Connection Type" DataField="conntype" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="Mobile" DataField="mobile" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="LCO Name" DataField="lconame" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="Discount Amount" DataField="discountamt" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Discount Type" DataField="DISCOUNTTYPE" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Requested By" DataField="requestedby" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Reason" DataField="reason" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="Expiry Date" DataField="expirydt" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                        <asp:BoundField HeaderText="Inserted By" DataField="insby" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Inserted Date" DataField="insdt" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                    </Columns>
                </asp:GridView>
                </div>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
             <%-- <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:PostBackTrigger ControlID="btngrnExel" />
            <asp:PostBackTrigger ControlID="btnGenerateExcel" />
            <%--<asp:AsyncPostBackTrigger ControlID="" EventName="Click" />--%>
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
