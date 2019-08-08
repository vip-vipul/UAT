<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerSearchTest.aspx.cs"
    Inherits="PrjUpassPl.Transaction.CustomerSearchTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LCO Prepaid Module Load Tester</title>
    <link href="../CSS/stylSVC.css" type="text/css" rel="Stylesheet" />
    <link href="../CSS/stylesheet_moc.css" type="text/css" rel="Stylesheet" />
    <link href="../CSS/main.css" rel="stylesheet" type="text/css" />
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
    </script>
    <style type="text/css">
        /*-----------AUTOCOMPLETE-----------*/
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
        }
        .delInfoContent
        {
            width: 100%;
        }
        .scroller
        {
            overflow: auto;
            max-height: 250px;
        }
        .plan_scroller
        {
            overflow: auto;
            max-height: 170px;
        }
        .gridHolder
        {
            width: 75%;
        }
        .stbHolder
        {
            height: 150px;
            overflow-y: auto; /*width: 25%;*/
        }
        .custDetailsHolder
        {
            height: 150px;
            overflow-y: auto; /*width: 85%;*/
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
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="77.5%">
                    <tr>
                        <td align="left" valign="top" id="divCustHolder" runat="server">
                            <div class="delInfo custDetailsHolder" runat="server" id="divSearchHolder">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="Label20" Text="Search By"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdoSearchParamType" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="0">VC/Mac ID</asp:ListItem>
                                                <asp:ListItem Value="1">Account No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="90px" ID="txtSearchParam" MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnSearch" Text="Search" ValidationGroup="searchValidation"
                                                OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="pnlCustDetails" Visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:Label runat="server" ID="Label21" Text="Customer No."></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCustNo" Text="1070414797"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="labelvc" runat="server" Text="STB No."></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                                                &nbsp;
                                                <asp:Label ID="lblStbNo" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label runat="server" ID="Label12" Text="Customer Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustName" Text="PRAFUL DHONGE"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="Label3" Text="Customer Mobile"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="Label32" Text="9890547869"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="Label5" Text="Customer Address"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustAddr" Text="R-103, Thakur Complex, Lower Parel, Mumbai"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="lblServiceStat" Text="Service Status"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lbactive" runat="server" Text="Inactive" Visible="false"></asp:Label>
                                                <asp:Label ID="lbdeactive" runat="server" Text="Active" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <asp:Label runat="server" ID="lblSearchResponse" ForeColor="Red"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvSearchParam" runat="server" ValidationGroup="searchValidation"
                                Display="Dynamic" ErrorMessage="Enter VCID or Account No" ControlToValidate="txtSearchParam"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtSearchParam"
                                FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" valign="top">
                            <div class="delInfo stbHolder" runat="server" id="divStbHolder">
                                <asp:GridView ID="grdStb" Width="70%" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkStb" AutoPostBack="true" OnCheckedChanged="chkStb_CheckedChanged">
                                                </asp:CheckBox>
                                                <asp:HiddenField runat="server" ID="hdnServiceStr" Value='<%# Eval("SERVICE_STRING").ToString()%>' />
                                                <asp:HiddenField runat="server" ID="hdnStbVCID" Value='<%# Eval("VC_ID").ToString()%>' />
                                                <asp:HiddenField runat="server" ID="hdnStbNo" Value='<%# Eval("STB_NO").ToString()%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="VC ID" DataField="VC_ID" ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="bottom" width="80%">
                        </td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="pnlGridHolder" Visible="false">
                    <div class="delInfo gridHolder">
                        <table style="border-collapse: collapse" width="100%">
                            <tr>
                                <td align="left">
                                    <b>
                                        <asp:Label runat="server" ID="lblPlanHeading" Text="Plan Details :"></asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" width="95%">
                                    <b>
                                        <asp:Label ID="lblBasicPlan" runat="server" Text="Basic Plan"></asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="98%">
                                    <div class="plan_scroller">
                                        <asp:GridView ID="grdBasicPlanDetails" Width="100%" CssClass="Grid" runat="server"
                                            AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="625" />
                                                <%-- <asp:BoundField HeaderText="Customer Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left" />--%>
                                                <%--<asp:BoundField HeaderText="MRP" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="100" />--%>
                                                <%--<asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-Width="100"
                                                ItemStyle-HorizontalAlign="Left" />--%>
                                                <asp:BoundField HeaderText="Expiry" DataField="EXPIRY" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="100" />
                                                <%--   <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label30" runat="server" Text='<%# PlanStatusText(Eval("PLAN_STATUS")) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField> --%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <b>
                                        <asp:Label ID="lblAddonPlan" runat="server" Text="Addon Plans"></asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <div class="plan_scroller">
                                        <asp:GridView ID="grdAddOnPlan" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="400px"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="MRP" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="left"
                                                    ItemStyle-Width="100" />
                                                <asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="100" />
                                                <asp:BoundField HeaderText="Expiry" DataField="EXPIRY" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="100" />
                                                <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="100" />
                                                <%--   <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label30" runat="server" Text='<%# PlanStatusText(Eval("PLAN_STATUS")) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField> --%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <b>
                                        <asp:Label ID="lblAlacartePlan" runat="server" Text="A-La-Carte"></asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <div class="plan_scroller">
                                        <asp:GridView ID="grdCarte" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="400px"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="MRP" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="left"
                                                    ItemStyle-Width="100" />
                                                <asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="100" />
                                                <asp:BoundField HeaderText="Expiry" DataField="EXPIRY" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="100" />
                                                <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="100" />
                                                <%--<asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label30" runat="server" Text='<%# PlanStatusText(Eval("PLAN_STATUS")) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <table>
                    <tr>
                        <td>
                            <%--<asp:Button ID="btnSubmit" runat="server" Visible="false" Text="Submit" OnClick="btnSubmit_Click" /> --%>
                            <%--<asp:Button ID="btnReset" runat="server" Visible="false" Text="Reset" OnClick="btnReset_Click" />--%>
                        </td>
                    </tr>
                </table>
                <%-- ---------------------------------------------------MESSAGE POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popMsg" runat="server" BehaviorID="mpeMsg" TargetControlID="hdnPop2"
                    PopupControlID="pnlMessage" CancelControlID="imgClose2">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnPop2" runat="server" />
                <asp:Panel ID="pnlMessage" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;
                    display: none;">
                    <asp:Image ID="imgClose2" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" onclick="closeMsgPopup();" ImageUrl="~/Images/closebtn.png" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;&nbsp;Message
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table width="90%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblPopupResponse" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnCloseMsg" runat="server" OnClientClick="closeMsgPopup();" Width="100px"
                                        Text="OK" />
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
    </div>
    </form>
</body>
</html>
