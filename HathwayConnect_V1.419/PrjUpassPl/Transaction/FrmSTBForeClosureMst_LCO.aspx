<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="FrmSTBForeClosureMst_LCO.aspx.cs" Inherits="PrjUpassPl.Transaction.FrmSTBForeClosureMst_LCO" %>
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

         window.location.href = "../Transaction/frmSTBForeClosureList_LCO.aspx";
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="maindive">
            <div style="float: right">
            <button onclick="return back()" id="btnreturnBulkOperation" runat="server" style="margin-right: 5px;
                margin-top: -15px;" class="button">
                Back</button>
        </div>
                <asp:Label ID="lblResponse" runat="server" ForeColor="Red" Text=""></asp:Label>

                <asp:Panel ID="pnlView" runat="server" ScrollBars="Auto" style="padding-top: 13px;">
                <asp:Label ID="lblmsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                <div>
                    <div class="delInfo">
                        <table id="Table1" runat="server" align="center" width="100%" border="0">
                            <tr>
                                <td align="right">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblDist" runat="server" Text="Work Order"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="Label22" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearch" runat="server" Width="200px" onkeydown="SetContextKey()"
                                        ReadOnly="true"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divdet" runat="server" visible="false" style="padding-left: 35px;">
                        <div class="delInfo" id="Custdet">
                            <table width="100%" cellpadding="2">
                                <tr>
                                    <td colspan="6" align="left">
                                        <b>
                                            <asp:Label runat="server" ID="Label14" Text="Details"></asp:Label>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td width="160px" align="left">
                                        <asp:Label ID="lbldislcocode" runat="server" Text=""></asp:Label>
                                        &nbsp;
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbllcocode" runat="server" Font-Bold="true" BackColor="#4682B4" Width="100px"
                                            ForeColor="White"></asp:Label>
                                        <%--<asp:Label ID="lbllcocode" runat="server" style=" background-color: #e6d5b8;" Font-Bold="True"></asp:Label>--%>
                                    </td>
                                    <td width="120px" align="left">
                                        <asp:Label ID="lbldislconame" runat="server" Text="LCO Name"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbllconame" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="50px" align="left">
                                        <asp:Label ID="Label8" runat="server" Text="Amount" Visible="False"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label9" runat="server" Text=":" Visible="False"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblamount" runat="server" Visible="False"></asp:Label>
                                    </td>
                                    <td width="120px" align="left">
                                        <asp:Label ID="Label6" runat="server" Text="Type" Visible="False"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label7" runat="server" Text=":" Visible="False"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbltype" runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="160px" align="left">
                                        <asp:Label ID="Label2" runat="server" Text="Cashier"></asp:Label>
                                        &nbsp;
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblcashier" runat="server"></asp:Label>
                                    </td>
                                    <td width="50px" align="left">
                                        <asp:Label ID="Label12" runat="server" Text="Box Type"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label16" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblboxtype" runat="server" Font-Bold="true" BackColor="#4682B4" Width="100px"
                                            ForeColor="White"></asp:Label>
                                        <%--<asp:Label ID="lblboxtype" runat="server" style=" background-color: #e6d5b8;" Font-Bold="True"></asp:Label>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="50px" align="left">
                                        <asp:Label ID="Label10" runat="server" Text="STB Count"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label11" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblstbcount" runat="server" Font-Bold="true" BackColor="#4682B4" Width="100px"
                                            ForeColor="White"></asp:Label>
                                        <%--<asp:Label ID="lblstbcount" runat="server" style=" background-color: #e6d5b8;" Font-Bold="True"></asp:Label>--%>
                                    </td>
                                    <td width="120px" align="left">
                                        <asp:Label ID="Label13" runat="server" Text="Pending Count"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label15" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblpendingcount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="120px" align="left">
                                        <asp:Label ID="Label17" runat="server" Text="Scheme"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label18" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblscheme" runat="server" Font-Bold="true" BackColor="#4682B4" Width="100px"
                                            ForeColor="White"></asp:Label>
                                        <%--<asp:Label ID="lblscheme" runat="server" style=" background-color: #e6d5b8;" Font-Bold="True"></asp:Label>--%>
                                    </td>
                                    <td align="left" class="style68">
                                        <asp:Label ID="Label19" runat="server" Text="STB Rate"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label20" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblRateSTB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="style67">
                                        <asp:Label ID="Label21" runat="server" Text="STB Discount"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label24" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblDiscSTB" runat="server"></asp:Label>
                                    </td>
                                    <td align="left" class="style68">
                                        <asp:Label ID="Label25" runat="server" Text="STB Net"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label26" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblNetSTB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="style67">
                                        <asp:Label ID="Label28" runat="server" Text="LCO Rate"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label29" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblRateLCO" runat="server"></asp:Label>
                                    </td>
                                    <td align="left" class="style68">
                                        <asp:Label ID="Label31" runat="server" Text="LCO Discount"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label32" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblDiscLCO" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="style67">
                                        <asp:Label ID="Label34" runat="server" Text="LCO Net"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label35" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblNetLCO" runat="server"></asp:Label>
                                    </td>
                                    <td align="left" class="style68">
                                        <asp:Label ID="Label37" runat="server" Text="Total Net"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label40" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblTotalNet" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="delInfo">
                            <table width="100%">
                                <tr>
                                    <td align="left" width="50px">
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" Text="Total :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblTotalCount" runat="server" Font-Bold="true" Text="4"></asp:Label>
                                    </td>
                                    <td align="right" width="80px">
                                        <asp:Label ID="lblAllocated" runat="server" Font-Bold="true" Text="Allocated :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkAllocated" runat="server" Font-Bold="true" OnClick="lnkReceiptno_click"></asp:LinkButton>
                                    </td>
                                    <td align="right" width="120px">
                                        <asp:Label ID="Label36" runat="server" Font-Bold="true" Text="Foreclosure :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblforeclosure" runat="server" Font-Bold="true" Text="4"></asp:Label>
                                    </td>
                                    <td align="right" width="120px">
                                        <asp:Label ID="Label39" runat="server" Font-Bold="true" Text="Faulty :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblfaulty" runat="server" Font-Bold="true" Text="4"></asp:Label>
                                    </td>
                                    <td align="right" width="80px">
                                        <asp:Label ID="lblBalance" runat="server" Font-Bold="true" Text="Balance :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblBalanceCount" runat="server" Font-Bold="true" Text=""></asp:Label>
                                    </td>
                                    <%--<td colspan="6" align="left">
                        <asp:GridView runat="server" ID="grdSTBCount" CssClass="Grid" AutoGenerateColumns="false"
                            AllowPaging="true" PageSize="100" Width="100%">
                            <FooterStyle CssClass="GridFooter" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Total" DataField="Total" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Allocated">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkReceiptno" runat="server" Text='<%# Eval("Allocated") %>' OnClick="lnkReceiptno_click"></asp:LinkButton>
                                        HiddenField ID="hdnreceiptno" runat="server" Value='<%# Eval("receiptno").ToString()%>' /><asp:
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Balance" DataField="Balance" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                            <PagerSettings Mode="Numeric" />
                        </asp:GridView>
                    </td>--%>
                                </tr>
                            </table>
                        </div>
                        <div class="delInfo">
                            <table width="100%">
                                <tr>
                                    <td align="left" class="style67">
                                        <asp:Label ID="Label5" runat="server" Text="No. of Boxes"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label27" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtnoofbox" runat="server" Style="resize: none;" Width="200" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td align="left" class="style68">
                                        <asp:Label ID="Label30" runat="server" Text="Select Reason"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label33" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlreason" runat="server" Height="19px" Style="resize: none;"
                                            Width="205px">
                                            <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Boxes Not available in warehouse"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Need to purchase STB in new scheme"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Need to change STB type"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <%--    <input value="Test" name="Test" type="button" tabindex="2" />--%>
                                    <asp:Button ID="btnSubmit" TabIndex="3" runat="server" Font-Bold="True" Text="Submit"
                                        class="button" Width="70" OnClick="btnSubmit_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <!---------------------------cnfmPopup------------------------>
            <cc1:ModalPopupExtender ID="popMsgBox" runat="server" BehaviorID="mpeMsgBox" TargetControlID="hdnMsgBox"
                PopupControlID="pnlMsgBox">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnMsgBox" runat="server" />
            <asp:Panel ID="pnlMsgBox" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;
                display: none">
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
                                Are you sure ?
                                <br />
                                Do you want to submit the data ?
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
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
                                <%--     <input id="Button3" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closemsgbox();" />--%>
                                <asp:Button ID="Button2" runat="server" CssClass="button" Text="Close" Width="100px"
                                    OnClick="btnClodeMsg1_click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="popGridBox" runat="server" BehaviorID="mpeGridBox" TargetControlID="hdnGridBox"
                PopupControlID="pnlGideBox">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnGridBox" runat="server" />
            <asp:Panel ID="pnlGideBox" runat="server" CssClass="Popup" Style="width: 430px; height: 240px;
                display: none">
                <asp:Image ID="Image1" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" onclick="closeGridPopup();" ImageUrl="~/Images/closebtn.png" />
                <%-- display: none; --%>
                <center>
                    <br />
                    <asp:Panel ID="pnl" runat="server" ScrollBars="Auto" Style="width: 430px; height: 160px;">
                        <asp:GridView ID="grdSTBDetails" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <HeaderStyle Width="40px" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="STB" DataField="STB" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Scheme" DataField="Scheme" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <br />
                    <input id="Button1" class="button" runat="server" type="button" value="Close" style="width: 100px;"
                        onclick="closeGridPopup();" />
                </center>
            </asp:Panel>
            <!---------------------------cnfmPopup------------------------>
            <cc1:ModalPopupExtender ID="popMsg" runat="server" BehaviorID="mpeMsg" TargetControlID="hdnPop2"
                PopupControlID="pnlMessage">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnPop2" runat="server" />
            <asp:Panel ID="pnlMessage" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;
                display: none;">
                <%-- display: none; --%>
                <asp:Image ID="imgClose2" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" onclick="closeMsgPopup();" ImageUrl="~/Images/closebtn.png" />
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
                                <asp:Label ID="lblPopupResponse" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <%--   <input id="btnClodeMsg" class="button" runat="server" type="button" value="Close"
                                    style="width: 100px;" onclick="closeMsgPopup();" />--%>
                                <asp:Button ID="Button3" runat="server" CssClass="button" Text="Close" Width="100px"
                                    OnClick="btnClodeMsg1_click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>

             </ContentTemplate>
             </asp:UpdatePanel>  
</asp:Content>
