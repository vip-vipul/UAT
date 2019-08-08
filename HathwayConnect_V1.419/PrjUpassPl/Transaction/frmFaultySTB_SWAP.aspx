<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmFaultySTB_SWAP.aspx.cs" Inherits="PrjUpassPl.Transaction.frmFaultySTB_SWAP" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            margin: 10px;
            
        }
        .delInfoContent
        {
            width: 95%;
        }
        .style67
        {
            width: 123px;
        }
       
    </style>
   <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 780px;
            margin: 5px;
            background-color: White;
            margin-top: 8px;
            height: 25px;
        }
        .delInfo1
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 780px;
            margin: 5px;
            background-color: White;
            margin-top: 8px;
            height: 365px;
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
        .style68
        {
            width: 184px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript">

        function InProgress() {
            //debugger;
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {

            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
        function CloseSwapPop() {
            $find("mpeSwapAL").hide();
            return false;
        }
        function closeSwapConfirmPopup() {
            $find("mpeSwapModifyConfirm").hide();
            return false;
        }
        function closemsgbox() {
            $find("mpeMsgBox").hide();
            return false;
        }

        function closeMsgPopup() {
            $find("mpeMsg").hide();
            return false;
        }
        function goBack() {
            window.location.href = "../Reports/EcafPages.aspx";
            return false;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            
            <ContentTemplate>
            <div class="maindive">
            <div style="float:right">
                <button onclick="return goBack();"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
            <asp:Panel ID="pnlView" runat="server" ScrollBars="Auto">
                <asp:Label ID="lblmsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                <div>
                    <div class="delInfo">
                        <table id="Table1" runat="server" align="center" width="100%" border="0">
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblDist" runat="server" Text="Search By"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="Label02" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label08" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="RadSearchby" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="0" Selected="True">VC</asp:ListItem>
                                        <asp:ListItem Value="1">Account No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearch" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                                    &nbsp;
                                    <asp:Button ID="Button1" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divdet" runat="server" visible="false">
                        <div class="delInfo1" id="Custdet">
                            <table width="100%" cellpadding="2">
                                <tr>
                                    <td colspan="6" align="left">
                                        <b>
                                            <asp:Label runat="server" ID="Label014" Text="Customer Details"></asp:Label>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                                       <table width="100%" id="tbl" runat="server">
                                        <tr id="trCustNo" runat="server">
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustNumber" Text="Customer A/C No."></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustNo" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trcust1Name" runat="server">
                                            <td align="left">
                                                <asp:Label runat="server" ID="Label12" Text="Customer Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustName" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trVC" runat="server">
                                            <td align="left">
                                                <asp:Label runat="server" ID="Label21" Text="VC/Mac ID"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label72" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblVCID" Text=""></asp:Label> 
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="labelvc" runat="server" Text="STB/Mac ID"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="Label01" runat="server" Text=":"></asp:Label>
                                                &nbsp;
                                                <asp:Label ID="lblStbNo" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trCustMobileNo" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="Label3" Text="Customer Mobile"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lbltxtmobno" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tremail" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label ID="Label9" runat="server" Text="Email Id"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblemail" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trCustAdd" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="Label5" Text="Customer Address"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustAddr" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trvcdet" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="Label73" Text="TV Details"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label74" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:GridView ID="GridVC" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                                    Height="100%" Width="60%" >
                                                    <Columns>
                                                    <asp:BoundField HeaderText="TV" DataField="TV" HeaderStyle-HorizontalAlign="Center"
                                                     Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="VC ID" DataField="VC_ID" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="STB NO" DataField="STB_NO" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="Status" DataField="Status" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="SUSPENSION DATE" DataField="SUSPENSION_DATE" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="BOX TYPE" DataField="BOX_TYPE" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                    <asp:Button ID="btnSwap" runat="server" Text="Faulty" OnClick="btnSwap_click" />  <%-- OnClick="lnkReceiptno_click"--%>
                                                    <asp:HiddenField ID="hdnVC_ID" runat="server" Value='<%# Eval("VC_ID").ToString()%>' />
                                                    <asp:HiddenField ID="hdnSTB_NO" runat="server" Value='<%# Eval("STB_NO").ToString()%>' />
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    
                                    </table>
                     </div>
                        
                    </div>
            
                </div>
            </asp:Panel>
        </div>
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
                                <asp:Button ID="btncnfmBlck" runat="server" Text="Confirm"  />
                                &nbsp;&nbsp;
                                <input id="Button3" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closemsgbox();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            
            
                <%-- ---------------------------------------------------SWAP pop-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="mpeSwapPop" runat="server" BehaviorID="mpeSwapAL"
                    TargetControlID="HdnpnlSwapAL" PopupControlID="pnlSwapAl">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="HdnpnlSwapAL" runat="server" />
                <asp:Panel ID="pnlSwapAl" runat="server" CssClass="Popup" Style="width: 650px;
                    display: none; height: 180px;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image18" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="CloseSwapPop();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label46" runat="server" ForeColor="#094791" Font-Bold="true" Text="Swap"></asp:Label>
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
                                 STB ID :
                                </td>
                                <td align="right">
                                <asp:TextBox ID="txtSwapSTBID" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td align="left">
                                New STB ID :
                                </td>
                                <td align="right">
                                <asp:TextBox ID="txtswapNewSTBID" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            Reason :
                            </td>
                            <td>
                            <asp:DropDownList ID="ddlSawpReason" runat="server">
                            <asp:ListItem  Text=" Select Reason" Value="0"></asp:ListItem>
                            <asp:ListItem  Text="Change of device due to faulty device" Value="Change of device due to faulty device"></asp:ListItem>
                            <asp:ListItem  Text="Change temporary current device to a permanemt device" Value="Change temporary current device to a permanemt device"></asp:ListItem>
                            </asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                            <td colspan="2" align="center">
                            <asp:Label ID="lblPopupResponse1" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                            </tr>
                        </table>
                        <br />
                        <table width="90%">
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="btnswapConf" runat="server" CssClass="button" Text="Submit" Visible="true"
                                        Width="100px" OnClick="btnswapConf_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>


            <cc1:ModalPopupExtender ID="mpSwapConfirm" runat="server" BehaviorID="mpeSwapModifyConfirm"
                TargetControlID="hdnSwapConfirm" PopupControlID="pnlSwapConfirm">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnSwapConfirm" runat="server" />
            <asp:Panel ID="pnlSwapConfirm" runat="server" CssClass="Popup" Style="width: 430px;
                display: none; height: 220px;">
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
                    <table>
                        <tr>
                            <td style=" width: 70px;">
                                <asp:Label ID="Label47" runat="server" Text="STB ID"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label ID="lblSTBID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style=" width: 70px;">
                                <asp:Label ID="Label49" runat="server" Text="New STB ID"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblNewSTBID"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td    style=" width: 70px;">
                                <asp:Label ID="Label50" runat="server" Text="Reason"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblCofReason"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                &nbsp;
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                            <td align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Button ID="btnSawpConfirm" runat="server" CssClass="button" Text="Confirm"
                                    Width="100px" OnClick="btnModifyConfirm_click" /><%-- OnClick="btnModifyConfirm_click" --%>
                                &nbsp;&nbsp;
                                <%--<input id="btnSwapClose" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closeSwapConfirmPopup();" />--%>
                                    <button onclick="closeSwapConfirmPopup();"  style="margin-right:5px;margin-top:-15px; width: 100px;"   class="button">Cancel</button>
                            </td>
                        </tr>
                    </table>
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
                            <button onclick="return goBack();"  style="margin-right:5px;margin-top:-15px;"   class="button">Close</button>
                                <%--<input id="btnClodeMsg" class="button" runat="server" type="button" value="Close"
                                    style="width: 100px;" onclick="" />--%>
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%-- Loader --%>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btncnfmBlck" />
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
