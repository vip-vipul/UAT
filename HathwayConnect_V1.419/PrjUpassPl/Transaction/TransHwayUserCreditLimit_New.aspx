<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransHwayUserCreditLimit_New.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayUserCreditLimit_New" %>

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

            window.location.href = "../Transaction/TransBalanceManagementPages.aspx";
        }
        function checkZero(ctrl) {
            if (ctrl.value == "") {
                ctrl.value = "0";
            }
        }
        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="maindive">
                <asp:Label ID="lblResponse" runat="server" ForeColor="Red" Text=""></asp:Label>
               
                <div>
                    LCO :
                    <asp:DropDownList ID="ddlLco" runat="server" AutoPostBack="true" Height="19px" Style="resize: none;"
                        Width="304px" OnSelectedIndexChanged="ddlLco_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <asp:HiddenField ID="hdnAvailBal" runat="server" Value="0" />
                <asp:HiddenField ID="hdnAvailCreditBal" runat="server" Value="0" />
                <div id="balBox" style="width: 70%; text-align: center;" runat="server" class="delInfo header">
                    <asp:Label ID="Label1" runat="server" Text="Total Balance"></asp:Label>
                    :
                    <asp:Label runat="server" ID="lbltotalbalance" Text="0"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Label ID="Label3" runat="server" Text="Allocated Balance"></asp:Label>
                    :
                    <asp:Label runat="server" ID="lblallocatedbalance" Text="0"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Label ID="lbl" runat="server" Text="Unallocated Balance"></asp:Label>
                    :
                    <asp:Label runat="server" ID="lblAvailBal" Text="0"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Label ID="Label2" runat="server" Text="Credit Balance"></asp:Label>
                    :
                    <asp:Label runat="server" ID="lblCreditBal" Text="0"></asp:Label>
                </div>
                <br />
                <div class="griddiv">
                    <div class="delInfo" style="padding: 10px; width: 70%">
                        <asp:GridView ID="grdUsers" Width="100%" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                            OnRowDataBound="grdUsers_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="User ID" DataField="username" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Name" DataField="userowner" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Allocated Balance" DataField="curentcreditlimit" ItemStyle-Width="100"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderText="Increase By" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtIncLimit" runat="server" Width="70px" Style="text-align: right;"
                                            onclick="this.select();" onblur="checkZero(this)" MaxLength="5" Text="0"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtIncLimit"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Decrease By" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDecLimit" runat="server" Width="70px" Style="text-align: right;"
                                            onclick="this.select();" onblur="checkZero(this)" MaxLength="5" Text="0"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtDecLimit"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:HiddenField ID="hdnUserId" runat="server" Value='<%# Eval("username").ToString()%>' />
                                        <asp:HiddenField ID="hdnCurCredit" runat="server" Value='<%# Eval("curentcreditlimit").ToString()%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="USERBLCK" ItemStyle-HorizontalAlign="Left"
                                    Visible="true" />
                                <asp:TemplateField HeaderText="Block/Unblock" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:Button ID="btnBlockunblock" runat="server" Text="" OnClick="btnBlockunblock_Click" />
                                        <asp:HiddenField ID="hdnblkUnblck" runat="server" Value='<%# Eval("USERBLCK").ToString()%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnManageCredits" runat="server" Text="Submit" CssClass="button"
                            OnClick="btnManageCredits_Click" />
                        &nbsp;
                        <asp:Button runat="server" ID="btnBck" Text="Back" CssClass="button" UseSubmitBehavior="false"
                            OnClientClick="back();" />
                    </div>
                </div>
            </div>
            <%-- -----------------------------------Loader--------------------------- --%>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
            </div>
            <cc1:ModalPopupExtender ID="popMsgBox" runat="server" BehaviorID="mpeMsgBox" TargetControlID="hdnMsgBox"
                PopupControlID="pnlMsgBox">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnMsgBox" runat="server" />
            <asp:Panel ID="pnlMsgBox" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;">
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
                                User ID :
                                <asp:Label ID="lblpopuserid" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                User Name :
                                <asp:Label ID="lblpopusername" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="lblmsgbox" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:HiddenField ID="hdnpopstatus" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="btncnfmBlck" runat="server" Text="Confirm" OnClick="btncnfmBlck_Click" />
                                &nbsp;&nbsp;
                                <input id="Button3" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closemsgbox();" />
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
