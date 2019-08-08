<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmSTBForeClosureList_LCO.aspx.cs" Inherits="PrjUpassPl.Transaction.frmSTBForeClosureList_LCO" %>

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
    <script type="text/javascript">
        function closemsgbox() {

            $find("mpeMsgBox").hide();

        }
        function closeMsgPopup() {
            $find("mpeMsg").hide();
            return false;
        }
        function closeGridPopup() {
            $find("mpeGridBox").hide();
            return false;
        } 
    </script>
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
            <div style="float: right">
            <button onclick="return back()" id="btnreturnBulkOperation" runat="server" style="margin-right: 5px;
                margin-top: -15px;" class="button">
                Back</button>
        </div>
                <asp:Label ID="lblResponse" runat="server" ForeColor="Red" Text=""></asp:Label>
               
                <div>
                    LCO :
                    <asp:DropDownList ID="ddlLco" runat="server" AutoPostBack="true" Height="19px" Style="resize: none;"
                        Width="304px" OnSelectedIndexChanged="ddlLco_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                
                <br />
                <div class="griddiv">
                    <div class="delInfo" style="padding: 10px; width: 70%">
           <table width="100%" cellpadding="2">
                <tr>
                    <td colspan="6" align="left">
                        <asp:GridView runat="server" ID="grdCashcollect" CssClass="Grid" AutoGenerateColumns="false"
                            AllowPaging="true" PageSize="100" Width="100%">
                            <FooterStyle CssClass="GridFooter" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:BoundField HeaderText="Receipt No." DataField="receiptno" HeaderStyle-HorizontalAlign="Center"
                                  ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Pay Mode" DataField="paymentmode" HeaderStyle-HorizontalAlign="Center"
                                    Visible="false" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Transaction Type" DataField="xtype" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Transaction Sub Type" DataField="TRANSSUBTYPE1" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Box Type" DataField="BOXTYPE" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Scheme" DataField="SCHEME" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Discount" DataField="lcodiscount" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                <asp:BoundField HeaderText="Amount" DataField="totalnet" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                <asp:TemplateField HeaderText="ForeClosure">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkReceiptno" runat="server" Text='Foreclosure'
                                            OnClick="lnkReceiptno_click"></asp:LinkButton>
                                        <asp:HiddenField ID="hdnreceiptno" runat="server" Value='<%# Eval("receiptno").ToString()%>' />
                                        <asp:HiddenField ID="hdnsubtype" runat="server" Value='<%# Eval("subtypex").ToString()%>' />
                                        <asp:HiddenField ID="hdntype" runat="server" Value='<%# Eval("transtype").ToString()%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="Numeric" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
                    </div>
                </div>
            </div>
            <%-- -----------------------------------Loader--------------------------- --%>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
            </div>
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
