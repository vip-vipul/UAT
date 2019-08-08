<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptSmsDelivery.aspx.cs" Inherits="PrjUpassPl.Reports.rptSmsDelivery" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function closePopup() {
            $find("mpeConfirmation").hide();
            return false;
        }
        function closeExpPopup() {
            $find("mpeExp").hide();
            return false;
        }
        function goBack() {
            window.location.href = "../Reports/rptnoncasreport.aspx";
            return false;
        }
    </script>
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
    <div class="maindive">
        <div style="float: right">
            <button onclick="return goBack();" style="margin-right: 5px; margin-top: -15px;"
                class="button">
                Back</button>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlSearch">
                    <div class="tblSearchItm" style="width: 30%;">
                        <table width="120%">
                            <tr>
                                    <td>
                                        LCO Name :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLco" runat="server" Height="19px" Style="resize: none;"
                                            Width="304px">
                                        </asp:DropDownList>
                                    </td>
                            </tr>
                            <tr>
                                <td align="center" class="cal_image_holder">
                                     From Date :
                                   
                                </td>
                                <td>
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
                                     To Date :
                                   
                                </td>
                                <td>
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
                                <td style="padding-left: 60px" align="center">
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
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                                &nbsp;
                                <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td align="left">
                                <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" OnClick="btnGenerateExcel_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div class="griddiv">
                                    <asp:GridView runat="server" ID="grdExpiry" CssClass="Grid" AutoGenerateColumns="false"
                                        ShowFooter="true" AllowPaging="true" PageSize="100" OnPageIndexChanging="grdExpiry_PageIndexChanging">
                                        <%--OnRowCommand="grdLcoPartyLedger_RowCommand" OnRowDataBound="grdLcoPartyLedger_RowDataBound"
                        OnSorting="grdLcoPartyLedger_Sorting"--%>
                                        <FooterStyle CssClass="GridFooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="VC/MAC Id" DataField="var_sms_vcid" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Mobile No" DataField="num_sms_clcontact" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Message" DataField="var_sms_message" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Status" DataField="var_sms_status" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Inserted By" DataField="var_sms_username" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Inserted Date" DataField="date_sms_dt" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <PagerSettings Mode="Numeric" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%-- ---------------------------------------------------ADDON POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popExp" runat="server" BehaviorID="mpeExp" TargetControlID="hdnPop3"
                    PopupControlID="pnlExp" CancelControlID="imgClose3">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnPop3" runat="server" />
                <asp:Panel ID="pnlExp" runat="server" CssClass="Popup" Style="width: 650px; height: 250px;
                    display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="imgClose3" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" onclick="closeExpPopup();" ImageUrl="~/Images/closebtn.png" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;&nbsp;Expiry Details
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table width="90%">
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="Label21" Font-Bold="true" Text="Account No."></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label143" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblAccNo" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="label1" runat="server" Font-Bold="true" Text="VC No."></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblVCNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="Label12" Font-Bold="true" Text="LCO Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lbllcoName" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="Label7" Font-Bold="true" Text="Full Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblfullname"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="Label13" Font-Bold="true" Text="Address"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lbladdress"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="130px">
                                    <asp:Label runat="server" ID="Label3" Font-Bold="true" Text="Mobile"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblMobile" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="130px">
                                    <asp:Label runat="server" ID="Label5" Font-Bold="true" Text="Plan name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblplan" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="label9" runat="server" Font-Bold="true" Text="Plan type"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label10" runat="server" Text=":"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblplantype" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="100px">
                                    <asp:Label runat="server" ID="lblServiceStat" Font-Bold="true" Text="End date"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblEnddate" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <div id="imgrefresh2" class="loader transparent">
                    <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                    <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                        AlternateText="Loading ..." ToolTip="Loading ..." />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnGenerateExcel" />
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
    </div>
</asp:Content>
