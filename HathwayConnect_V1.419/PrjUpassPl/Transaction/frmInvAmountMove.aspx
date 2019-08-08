<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmInvAmountMove.aspx.cs" Inherits="PrjUpassPl.Transaction.frmInvAmountMove" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="../CSS/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .topHead
        {
            background: #E5E5E5;
        }
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            margin: 10px;
            width: 900px;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .scroller
        {
            overflow: auto;
            max-height: 250px;
        }
        .Popup
        {
            display: inline-block;
            background-color: #FFFFFF;
            border: 2px solid #094791; /*box-shadow: 8px 8px 5px #888888;*/
            -moz-box-shadow: 3px 3px 4px #444;
            -webkit-box-shadow: 3px 3px 4px #444;
            box-shadow: 3px 3px 4px #444;
            -ms-filter: "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#444444')";
            filter: progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#444444');
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


        function closeMsgPopupnew() {
            $find("mpeMsg").hide();
            return false;
        }

        function closeMsgPopupnew1() {
            debugger;
            $("#<%=btnSearch.ClientID%>").click();
            $find("mpeMsg1").hide();

            return false;
        }
        function closeModifyCustConfirmPopup() {
            $find("mpeModifyConfirm").hide();
            return false;
        }


        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div class="maindive">
            <asp:Panel ID="pnlView" runat="server" ScrollBars="Auto">
                <asp:Label ID="lblmsg11" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                <div>
                    <div class="delInfo" >
                        <table id="Table1" runat="server" align="center" width="100%" border="0">
                            
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label66" runat="server" Text="LCO Code :"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearch" runat="server" Width="200px" enable="false"></asp:TextBox>
                                    &nbsp;
                                    <asp:HiddenField ID="hfCustomerId" runat="server" />
                                    <asp:Button ID="btnSearch" runat="server" Height="23px" Text="Search" OnClick="btnSearch_Click" enable="false" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divdet" runat="server">
                        <div class="delInfo" id="Lcodet">
                            <cc1:Accordion ID="LCOAccordion" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                FadeTransitions="true" SuppressHeaderPostbacks="true" TransitionDuration="250"
                                FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None" Width="871px">
                                <Panes>
                                    <cc1:AccordionPane ID="LCOAccordionPane" runat="server">
                                        <Header>
                                            <a href="#" class="href">
                                                <asp:Label ID="Label32" runat="server" Text="LCO Details"></asp:Label></a></Header>
                                        <Content>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="3" align="left">
                                                        <b>
                                                            <asp:Label runat="server" ID="Label4" Text="LCO Details:"></asp:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <hr />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="75px">
                                                        <asp:Label runat="server" ID="Label1" Text="LCO Code"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label2" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label runat="server" ID="lblCustNo"></asp:Label>
                                                    </td>
                                                    <td align="left" width="47px">
                                                        Name
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label6" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label runat="server" ID="lblCustName"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="75px">
                                                        <asp:Label runat="server" ID="Label9" Text="Mobile No."></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label14" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label runat="server" ID="lblmobno"></asp:Label>
                                                    </td>
                                                    <td align="left" width="47px">
                                                        <asp:Label runat="server" ID="Label38" Text="Email"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label39" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label runat="server" ID="lblEmail"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="75px">
                                                        <asp:Label runat="server" ID="Label7" Text="Address"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label8" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label runat="server" ID="lblCustAddr"></asp:Label>
                                                    </td>
                                                    <td align="left" width="47px">
                                                        <asp:Label runat="server" ID="Label41" Text="Inventory Balance"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label42" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label runat="server" ID="lblInvBalance" Font-Bold="true" BackColor="#4682B4" Width="100px"
                                                            ForeColor="White"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </Content>
                                    </cc1:AccordionPane>
                                </Panes>
                            </cc1:Accordion>
                        </div>
                        <div class="delInfo" id="STBdet">
                            <table width="100%" cellpadding="2">
                                <tr>
                                    <td colspan="2" align="left">
                                        <b>
                                            <asp:Label runat="server" ID="Label5" Text="Transfer Amount Details"></asp:Label>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                
                                <tr>
                                    <td align="left" width="142px">
                                        <asp:Label ID="Label10" runat="server" Text="LCO Balance"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label12" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        :
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:TextBox ID="txtLCOBanalce" runat="server" Style="resize: none;" Width="200" MaxLength="12"
                                             Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtLCOBanalce"
                                            ValidChars="1234567890.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="142px">
                                        <asp:Label ID="Label11" runat="server" Text="Transfer Amount "></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label46" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        :
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:TextBox ID="txtAmount" runat="server" Style="resize: none;" Width="200" MaxLength="12"
                                             ></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtAmount"
                                            ValidChars="1234567890.">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="142px" align="left">
                                        <asp:Label ID="Label55" runat="server" Text="Mobile No"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label56" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label57" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10" Width="200" onkeydown="return ((event.keyCode>=48 && event.keyCode<=57) || event.keyCode==8 || (event.keyCode>=96 && event.keyCode<=107));"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="142px" align="left">
                                        <asp:Label ID="Label20" runat="server" Text="Remark"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label51" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label21" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:TextBox ID="txtRemark" MaxLength="100" runat="server" Width="200" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom"
                                            TargetControlID="txtRemark" ValidChars=" -_">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSubmit" TabIndex="2" runat="server" Font-Bold="True" Text="Submit"
                                        class="button" Width="65" OnClick="btnSubmit_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            </div>
            
            <cc1:ModalPopupExtender ID="popupModifyConfirm" runat="server" BehaviorID="mpeModifyConfirm"
                TargetControlID="hdnModifyConfirm" PopupControlID="pnlModifyConfirm">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnModifyConfirm" runat="server" />
            <asp:Panel ID="pnlModifyConfirm" runat="server" CssClass="Popup" Style="width: 430px;
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
                            <td>
                                <asp:Label ID="Label102" runat="server" Text="LCO Balance  "></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblLCOBalance"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label91" runat="server" Text="Pre Inventory Balance "></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblInventoryBalance"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label89" runat="server" Text="Post Inventory Balance  "></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label ID="lblPostInvBalance" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Transfer Amount "></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblMovingAmount"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Remark "></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblRemark"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Button ID="btnModifyConfirm" runat="server" CssClass="button" Text="Confirm"
                                    Width="100px" OnClick="btnModifyConfirm_click" />
                                &nbsp;&nbsp;
                                <input id="Button6" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closeModifyCustConfirmPopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
           
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
                                <asp:Label ID="lblPopupResponse" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <input id="btnClodeMsg" class="button" runat="server" type="button" value="Close"
                                    style="width: 100px;" onclick="closeMsgPopupnew();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>

                   <cc1:ModalPopupExtender ID="popMsg1" runat="server" BehaviorID="mpeMsg1" TargetControlID="hdnPop1"
                PopupControlID="pnlMessage1">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnPop1" runat="server" />
            <asp:Panel ID="pnlMessage1" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;
                display: none;">
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
                                <asp:Label ID="lblmsgsuc1" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <input id="Button1" class="button" runat="server" type="button" value="Close"
                                    style="width: 100px;" onclick="closeMsgPopupnew1();" />
                                    <%--<asp:Button ID="btnmsgClose" class="button" runat="server" style="width: 100px;" OnClick="closeMsgPopupnew1" />--%>
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
