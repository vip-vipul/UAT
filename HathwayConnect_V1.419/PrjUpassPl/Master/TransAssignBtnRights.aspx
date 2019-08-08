<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TransAssignBtnRights.aspx.cs" Inherits="PrjUpassPl.Master.TransAssignBtnRights" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            font-size: 12px;
            font-weight: bold;
        }
        
        .header
        {
            background: lightgrey;
        }
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
     function back() {

         window.location.href = "../Master/mstLCOAdminPages.aspx";
         return false;
     }
     function InProgress() {
         document.getElementById("imgrefresh").style.visibility = 'visible';
     }
     function onComplete() {
         document.getElementById("imgrefresh").style.visibility = 'hidden';
     }
     function closeMsgPopup() {
         $find("popCheques").hide();
         return false;

     }

     function closeMsgPopupnew() {
         $find("mpeMsg").hide();
         return false;
     }

    </script>

       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="maindive">
            <div style="float: right">
            <button onclick="return back()" id="btnreturnBulkOperation" runat="server" style="margin-right: 5px;
                margin-top: -15px;" class="button">
                Back</button>
        </div>
                <asp:Label ID="lblResponse" runat="server" ForeColor="Red" Text=""></asp:Label>
               
                <div>
                    LCO :
                    <asp:DropDownList ID="ddlLco" runat="server" AutoPostBack="true" Height="19px" Style="resize: none;"
                        Width="304px" >
                    </asp:DropDownList>
                </div>
                <br />
                <div class="griddiv"  runat="server" id="div1" >
                    <div class="delInfo" style="padding: 10px; width: 70%">
                        <asp:GridView ID="grdUsers" Width="100%" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                            >
                            <Columns>
                            <asp:TemplateField HeaderText="User ID">
                            <ItemTemplate >
                            <asp:LinkButton ID="lnkUserID" runat="server" Text='<%# Eval("username").ToString() %>' OnClick="lnkUserID_Click"></asp:LinkButton>
                            <asp:HiddenField id="hdnUserID" runat="server" Value='<%# Eval("username").ToString() %>' />
                            <asp:HiddenField id="hdnUserName" runat="server" Value='<%# Eval("Name").ToString() %>' />
                            </ItemTemplate>
                            </asp:TemplateField>
                                <asp:BoundField HeaderText="Name" DataField="Name" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Address" DataField="Addr" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Email ID" DataField="email" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Mobile No" DataField="MobNo" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="delInfo" style="padding: 10px; width: 70%" runat="server" visible="false" id="div3">
                <table>
                        <tr>
                        <td>
                        User ID:
                        <asp:Label ID="lblLcoID" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                       &nbsp;&nbsp;&nbsp; User Name:
                        <asp:Label ID="lblLcoName" runat="server" Text=""></asp:Label>
                        </td>
                        </tr>
                        <tr>
                        </table>
                        <hr />
                        <table>
                        </tr>
                        <tr>
                        <td >
                        <asp:CheckBox runat="server" ID="chkUserAccMap" Text="User Account Mapping" />
                        </td>
                        <td >
                        
                        </td>
                        </tr>
                        </table>
                </div>
                <br />
                <div class="delInfo" style="padding: 10px; width: 70%" runat="server" visible="false" id="div2">
                        <table>
                        <tr>
                        <th>
                        Pack Management Rights
                        </th>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkAdd" Text="Add Plan" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkRenew" Text="Renew Plan" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkChange" Text="Change Plan" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkCancel" Text="Cancel Plan" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkDiscount" Text="Discount" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkRetrack" Text="Retrack" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkCustModify" Text="Customer Modify" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkSTBSwap" Text="STB Swap" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkAutoRenew" Text="Auto Renew" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkDeactivate" Text="Activate/Deactivate" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkTerminate" Text="Terminate" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkFocPack" Text="FOC Pack" />
                        </td>
                        </tr>
                        </table>
                    </div>
            
                <div class="delInfo" style="padding: 10px; width: 70%" runat="server" visible="false" id="div5">
                        <table>
                        <tr>
                        <th align="left">
                        Tiles Rights
                        </th>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkDashboard" Text="Dashboard" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="ChkPackManagement" Text="Pack Management" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkBalanceManagement" Text="Balance Management" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkBulkManagement" Text="Bulk Management" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkMassenger" Text="Messenger" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkReports" Text="Reports" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkInventoryManagement" Text="Inventory Management" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkEcaf" Text="Ecaf" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkNotification" Text="Notification" />
                        </td>
                        <td>
                        <asp:CheckBox runat="server" ID="chkLCOAdmin" Text="LCO Admin" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:CheckBox runat="server" ID="chkLegal" Text="Legal" />
                        </td>
                        </tr>
                       <tr >
                       <td colspan="2" align="center">
                       <br />
                       <asp:Button runat="server" ID="Button2" Text="Submit" OnClick="btnSubmit_Click" />
                       </td>
                       </tr>
                        </table>
                    </div>
            
            </div>
            <%-- -----------------------------------Loader--------------------------- --%>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
            </div>

                 <cc1:ModalPopupExtender ID="popMsg" runat="server" BehaviorID="mpeMsg" TargetControlID="hdnPop5"
                PopupControlID="pnlMessage">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnPop5" runat="server" />
            <asp:Panel ID="pnlMessage" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;
                display: none;">
                <%-- display: none; --%>
                <asp:Image ID="imgClose2" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" onclick="closeMsgPopupNew();" ImageUrl="~/Images/closebtn.png" />
                <center>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Message
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
                                <asp:Label ID="lblPopupResponse" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="btnClodeMsg1" runat="server" CssClass="button" Text="Close" Width="100px"
                                    OnClick="btnClodeMsg1_click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="popCheques" runat="server" BehaviorID="popCheques" TargetControlID="hdnPop2"
                PopupControlID="pnlCheque" DropShadow="true" BackgroundCssClass="transparent">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnPop2" runat="server" />
            <asp:Panel ID="pnlCheque" runat="server" CssClass="Popup" Style="width: 350px; height: 200px;
                display: none;">
                <asp:Image ID="Image1" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" onclick="closeMsgPopup();" ImageUrl="~/Images/closebtn.png" />
                <div class="body">
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;&nbsp;Confirmation
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table align="center">
                            <tr>
                                <td width="100%">
                                    Are You Sure Want to Submit Data ?
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnSubmitBankDet" runat="server" OnClick="btnSubmitConfirm_Click"
                                        CssClass="button" Text="Confirm" Width="100px" />
                                    <asp:Button ID="Button1" runat="server" CssClass="button" Text="Cancel" Width="100px"
                                        OnClientClick="closeMsgPopup();return false;" />
                                </td>
                            </tr>
                        </table>
                    </center>
            </asp:Panel>

        </ContentTemplate>
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
