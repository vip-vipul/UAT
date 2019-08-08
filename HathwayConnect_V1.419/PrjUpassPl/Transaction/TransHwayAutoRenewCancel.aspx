<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransHwayAutoRenewCancel.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayAutoRenewCancel" %>

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
    function closeFinalConfPopup() {
        $find("mpeFinalConf").hide();
        return false;
    }
</script>
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
    </script>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <table>
                    <%-- <tr>
                        <td>
                            <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                            &nbsp;
                            <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                </table>
                <table>
                    <tr>
                        <td align="left">
                            <asp:GridView runat="server" ID="grdLcoCustEcsDetails" CssClass="Grid" AutoGenerateColumns="false"
                                ShowFooter="true" AllowPaging="true" AllowSorting="true" PageSize="100" OnSorting="grdLcoCustEcsDetails_Sorting"
                                OnPageIndexChanging="grdLcoCustEcsDetails_PageIndexChanging">
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left" ControlStyle-Width="15pt">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="account_id" HeaderText="Account No" SortExpression="account_id"
                                        HeaderStyle-HorizontalAlign="Center" ControlStyle-Width="35pt" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="VC ID" DataField="VC_ID" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Customer Name" DataField="customername" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Cust Address" DataField="cust_address" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Lco Name" DataField="lconame" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="Lco Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Package" DataField="Package" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Plan Type" DataField="PlanType" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="End Date" DataField="enddate" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="AutoRenewal Status" DataField="is_active" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Auto Renew Cancel">
                                       <%-- <HeaderTemplate>
                                            <asp:Label ID="lblAllRenew" runat="server" Text="Select All"></asp:Label>
                                            <br />
                                            <asp:CheckBox ID="chkautorenewHeader" onclick="HeaderchkClick(this);" runat="server" />
                                        </HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkautorenew" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                </table>
               
            </asp:Panel>
            <%-- ----------------------------------------------------Confirmation------------------------------------------------ --%>
    <cc1:ModalPopupExtender ID="popFinalConf" runat="server" BehaviorID="mpeFinalConf"
        TargetControlID="hdnPop7" PopupControlID="pnlFinalConfirm">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnPop7" runat="server" />
    <asp:Panel ID="pnlFinalConfirm" runat="server" CssClass="Popup" Style="width: 430px;
        height: 160px;">
        <%-- display: none; --%>
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Confirmation
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
                    <td align="center" colspan="3">
                        <asp:Label ID="lblPopupFinalConfMsg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnPopupConfYes" runat="server" CssClass="button" Text="Yes" 
                            Width="100px" onclick="btnPopupConfYes_Click"
                            />
                        &nbsp;&nbsp;
                        <input id="Button1" class="button" runat="server" type="button" value="No" style="width: 100px;"
                            onclick="closeFinalConfPopup();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />
            </div>
        </ContentTemplate>
        <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:PostBackTrigger ControlID="btn_genExl" />
        </Triggers>--%>
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
