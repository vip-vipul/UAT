<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmNewSTBSP.aspx.cs" Inherits="PrjUpassPl.Transaction.frmNewSTBSP" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>New STB</title>
    <link href="../CSS/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
               .MyCalendar .ajax__calendar_container
        {
            border: 1px solid #376091;
            background-color: #f9f9f9;
            color: #4db1a7;
            margin-top: 7px;
            border-radius: 5px;
        }
        .Grid td {
     padding: 0px !important;
}   
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

        function closeMsgPopup() {
            $find("popCheques2").hide();
            return false;
        }

        function closeMsgPopupnew() {
            $find("mpeMsg").hide();
            return false;
        }

        function closeModifyCustConfirmPopup() {
            $find("mpeModifyConfirm").hide();
            return false;
        }

        function closeShcemeDetPopup() {
            $find("popCheques3").hide();
            return false;
        }
        function closeMsgPopup() {
            $find("popCheques").hide();
            return false;
        }
        function getSelectedBankId(sender, e) {
            $get("<%=hfBankId.ClientID %>").value = e.get_value();
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
                    <div class="delInfo">
                        <table id="Table1" runat="server" align="center" width="100%" border="0">
                            
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label66" runat="server" Text="LCO Code :"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearch" runat="server" Width="200px"></asp:TextBox>
                                    &nbsp;
                                    <asp:HiddenField ID="hfCustomerId" runat="server" />
                                    <asp:Button ID="btnSearch" runat="server" Height="23px" Text="Search" OnClick="btnSearch_Click" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divdet" runat="server" >
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
                                            <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>--%>
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
                                                        <asp:Label runat="server" ID="txtBalance" Font-Bold="true" BackColor="#4682B4" Width="100px"
                                                            ForeColor="White"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>--%>
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
                                            <asp:Label runat="server" ID="Label5" Text="STB Details"></asp:Label>
                                        </b>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label ID="lblchqcaption" runat="server" Text="" ForeColor="Red"></asp:Label>
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
                                    <td width="142px" align="left">
                                        <asp:Label ID="Label58" runat="server" Text="Type"></asp:Label>
                                        <asp:Label ID="Label59" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                        &nbsp;
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label60" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:DropDownList ID="ddltype" runat="server" Height="19px" Style="resize: none;"
                                            Width="205px" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0">--Select type--</asp:ListItem>
                                            <asp:ListItem Value="STB">STB</asp:ListItem>
                                            <asp:ListItem Value="VC">VC</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtOther" runat="server" MaxLength="20" Style="resize: none;" Width="200px"
                                            Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="142px">
                                        <asp:Label ID="Label11" runat="server" Text="No. :"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label46" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        :
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:TextBox ID="txtNoofSTB" runat="server" Style="resize: none;" Width="50px" MaxLength="5"
                                            OnTextChanged="txtNoofSTB_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtNoofSTB"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                        <%--<asp:Button ID="btnAddDetails" runat="server" OnClick="btnAddDetails_Click" Text="Add Details" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="142px" align="left">
                                        <asp:Label ID="Label12" runat="server" Text="Scheme"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label33" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label13" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:DropDownList ID="ddlscheme" runat="server" Height="19px" Style="resize: none;"
                                            Width="205px" OnSelectedIndexChanged="ddlScheme_OnSelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkpdcdetails" runat="server" OnClick="lnkpdcdetails_Click" Text="Enter PDC Details"
                                    Visible="false"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="142px" align="left">
                                        <asp:Label ID="Label52" runat="server" Text="Box Type"></asp:Label>
                                        <%--<asp:Label ID="Label53" runat="server" Text="*" ForeColor="Red"></asp:Label>--%>
                                        &nbsp;
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label54" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:DropDownList ID="ddlboxtype" runat="server" Height="19px" Style="resize: none;"
                                            Width="205px">
                                            <asp:ListItem Selected="True" Value="0">--Select Box type--</asp:ListItem>
                                            <asp:ListItem Value="HD">HD</asp:ListItem>
                                            <asp:ListItem Value="SD">SD</asp:ListItem>
                                            <asp:ListItem Value="PVR">PVR</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trPaymode" runat="server" visible="false">
                                    <td width="142px" align="left">
                                        <asp:Label ID="Label16" runat="server" Text="Payment Mode"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label34" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label17" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="RBPaymode" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                            OnSelectedIndexChanged="RBPaymode_SelectedIndexChanged">
                                            <asp:ListItem Value="C" Selected="True">Cash</asp:ListItem>
                                            <asp:ListItem Value="Q">Cheque</asp:ListItem>
                                            <asp:ListItem Value="M">MPOS</asp:ListItem>
                                            <asp:ListItem Value="D">DD</asp:ListItem>
                                            <asp:ListItem Value="N">NEFT</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="trCheque" runat="server" visible="false">
                                    <td colspan="3">
                                        <table>
                                            <tr>
                                                <td width="142px" align="left">
                                                    <asp:Label ID="Label10" runat="server" Text="Bank Name "></asp:Label>
                                                    &nbsp;
                                                    <asp:Label ID="Label35" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                                <td width="5px">
                                                    <asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <%-- <td align="left">
                                                    <asp:DropDownList ID="ddlBank1" runat="server" Height="19px" Style="resize: none;"
                                                        Width="205px">
                                                    </asp:DropDownList>
                                                </td>--%>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBankName" runat="server" AutoComplete="off" Width="200" MaxLength="30"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ServiceMethod="SearchBankName" MinimumPrefixLength="1"
                                                        OnClientItemSelected="getSelectedBankId" CompletionInterval="100" EnableCaching="true"
                                                        CompletionSetCount="3" TargetControlID="txtBankName" FirstRowSelected="false"
                                                        ID="AutoCompleteExtender2" runat="server" CompletionListCssClass="autocomplete"
                                                        CompletionListItemCssClass="autocompleteItem" CompletionListHighlightedItemCssClass="autocompleteItemHover"
                                                        CompletionListElementID="bankListHolder">
                                                    </cc1:AutoCompleteExtender>
                                                    <div id="bankListHolder">
                                                    </div>
                                                    <asp:HiddenField ID="hfBankId" runat="server" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label24" runat="server" Text="Cheque No. "></asp:Label>
                                                    &nbsp;<asp:Label ID="Label43" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                                <td width="5px">
                                                    <asp:Label ID="Label25" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtChequeNo" MaxLength="6" runat="server" Style="resize: none;"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtChequeNo"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txtRefNo" MaxLength="12" runat="server" Style="resize: none;" Visible="false"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtRefNo"
                                                        FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="142px" align="left">
                                                    <asp:Label ID="Label40" runat="server" Text="Bank Branch"></asp:Label>
                                                    <asp:Label ID="Label103" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                    &nbsp;
                                                </td>
                                                <td width="5px">
                                                    <asp:Label ID="Label47" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtbankbranch" MaxLength="20" runat="server" Style="resize: none;"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label15" runat="server" Text="Cheque Date"></asp:Label>
                                                    &nbsp;<asp:Label ID="Label36" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td width="5px">
                                                    <asp:Label ID="lblDateCol" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtChqDate" MaxLength="30" runat="server" Style="resize: none;"></asp:TextBox>
                                                    <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtChqDate" PopupButtonID="imgFrom"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trMPOS" runat="server" visible="false">
                                    <td colspan="3">
                                        <table>
                                            <tr>
                                                <td width="142px" align="left">
                                                    <asp:Label ID="Label26" runat="server" Text="RR No. "></asp:Label>
                                                    &nbsp;
                                                    <asp:Label ID="Label44" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                                <td width="5px">
                                                    <asp:Label ID="Label27" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtRRNo" MaxLength="20" runat="server" Style="resize: none;"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label28" runat="server" Text="MPOS User Id "></asp:Label>
                                                    &nbsp;<asp:Label ID="Label45" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                                <td width="5px">
                                                    <asp:Label ID="Label29" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtmposuserid" MaxLength="20" runat="server" Style="resize: none;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="142px" align="left">
                                                    <asp:Label ID="Label30" runat="server" Text="Auth Code"></asp:Label>
                                                    &nbsp;
                                                    <asp:Label ID="Label48" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                                <td width="5px">
                                                    <asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtAuthCode" MaxLength="20" runat="server" Style="resize: none;"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" Text="Amount"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDisc" runat="server" Text="Discount"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label19" runat="server" Text="Net Amount"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label74" runat="server" Text="Upfront Amount"></asp:Label>
                                    </td>
                                </tr>


                                <tr>
                                 <td>
                                      Price
                                    </td>
                                    <td>
                                        <asp:Label ID="Label22" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtRateComb" MaxLength="10" runat="server" Style="resize: none;"
                                            Width="100px" ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtRateComb"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDiscountComb" MaxLength="10" runat="server" Style="resize: none;"
                                            Width="100px" ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtDiscountComb"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtNetComb" MaxLength="10" runat="server" Style="resize: none;" Width="100px"
                                            ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txtNetComb"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtUpfrontComb" MaxLength="10" runat="server" Style="resize: none;" Width="100px"
                                            ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txtUpfrontComb"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr id="rowstb" runat="server" style="display:none;">
                                    <td>
                                        STB ACTIVATION
                                    </td>
                                    <td>
                                        <asp:Label ID="Label37" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtRateSTB" MaxLength="10" runat="server" Style="resize: none;"
                                            Width="100px" ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtRateSTB"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDiscountSTB" MaxLength="10" runat="server" Style="resize: none;"
                                            Width="100px" ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtDiscountSTB"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtNetSTB" MaxLength="10" runat="server" Style="resize: none;" Width="100px"
                                            ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtNetSTB"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtSTBUpfront" MaxLength="10" runat="server" Style="resize: none;" Width="100px"
                                            ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtSTBUpfront"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr id="rowsub" runat="server" visible="false">
                                    <td>
                                        SUBSCRIPTION
                                    </td>
                                    <td>
                                        <asp:Label ID="Label68" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtRateLCO" MaxLength="10" runat="server" Style="resize: none;"
                                            Width="100px" ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtRateLCO"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDiscountLCO" MaxLength="10" runat="server" Style="resize: none;"
                                            Width="100px" ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtDiscountLCO"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtNetLCO" MaxLength="10" runat="server" Style="resize: none;" Width="100px"
                                            ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtNetLCO"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtlcoUpfront" MaxLength="10" runat="server" Style="resize: none;" Width="100px"
                                            ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtlcoUpfront"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr id="trpdcamount" runat="server">
                            <td width="142px" align="left">
                                <asp:Label ID="Label67" runat="server" Text="PDC Amount"></asp:Label>
                                &nbsp;
                                <asp:Label ID="Label69" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            </td>
                            <td width="5px">
                                <asp:Label ID="Label71" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtpdcpaidamount" MaxLength="10" runat="server" Style="resize: none;"
                                    Width="100px" ReadOnly="true" Enabled="False"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtpdcpaidamount"
                                    FilterType="Numbers, Custom" ValidChars="-">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                                <tr>
                                    <td width="142px" align="left">
                                        <asp:Label ID="Label75" runat="server" Text="Total Net"></asp:Label>
                                        &nbsp;
                                        <%--<asp:Label ID="Label76" runat="server" ForeColor="Red" Text="*"></asp:Label>--%>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label77" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:TextBox ID="txtTotalNet" MaxLength="10" runat="server" Style="resize: none;"
                                            Width="100px" ReadOnly="true" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtTotalNet"
                                            FilterType="Numbers">
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
            

                                        <cc1:ModalPopupExtender ID="popCheques" runat="server" BehaviorID="popCheques" Drag="true"
        TargetControlID="hdnPDC" PopupControlID="pnlPDC" DropShadow="true">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnPDC" runat="server" />
    <asp:Panel ID="pnlPDC" runat="server" CssClass="Popup" Style="width: auto; height: auto;
        min-width: 700px; min-height: 300px">
        <%-- display: none; --%>
        <asp:Image ID="Image1" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeMsgPopup();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;PDC Details
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                        <asp:Label ID="lblPDCMsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="600px">
                <tr>
                    <td align="center" width="100%">
                        <asp:GridView ID="gridPDCChques" Width="660px" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                            <%--OnRowDeleting="OnRowDeleting"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Cheque No.">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpdcchequeno" MaxLength="6" Height="19px" Width="130px" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtpdcchequeno"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cheque Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPDCChqDate" MaxLength="30" Width="110px" runat="server"></asp:TextBox>
                                        <%--<asp:Image runat="server" ID="imgFromPDC" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <cc1:CalendarExtender runat="server" ID="calFromPDC" TargetControlID="txtPDCChqDate"
                                            PopupButtonID="imgFromPDC" Format="dd-MMM-yyyy">
                                        </cc1:CalendarExtender>--%>

                                        <asp:Image ID="imgFrom" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender ID="CalendarExtender12" runat="server" Format="dd-MMM-yyyy"
                                       CssClass="MyCalendar"  PopupButtonID="imgFrom" TargetControlID="txtPDCChqDate">
                                    </cc1:CalendarExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bank Name">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlpdcbank" runat="server" Width="130px" Height="19px">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpdcamount" Height="19px" Width="130px" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderAmt" runat="server" TargetControlID="txtpdcamount"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:CommandField HeaderText="Remove" ShowDeleteButton="True" ButtonType="Link"/>--%>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnpdcsubmit" runat="server" CssClass="button" Text="Submit" Width="100px"
                            OnClick="btnpdcsubmit_click" />
                        <asp:Button ID="btnpdcClose" runat="server" CssClass="button" Text="Reset" Width="100px"
                            OnClick="btnpdcClose_click" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    

                  <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BehaviorID="ModalPopupExtender1" Drag="true"
        TargetControlID="HiddenField1" PopupControlID="Panel1" DropShadow="true">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:Panel ID="Panel1" runat="server" CssClass="Popup" Style="width: auto; height: auto;
        min-width: 700px; min-height: 300px">
        <%-- display: none; --%>
        <asp:Image ID="Image2" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeMsgPopup();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Wallet PDC Details
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                        <asp:Label ID="lblWPDCmsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="600px">
                <tr>
                    <td align="center" width="100%">
                        <asp:GridView ID="GridView1" Width="660px" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                            <%--OnRowDeleting="OnRowDeleting"--%>
                            <Columns>
                                <%--<asp:TemplateField HeaderText="Cheque No.">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpdcchequeno" MaxLength="6" Height="19px" Width="130px" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtpdcchequeno"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText=" Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPDCChqDate" MaxLength="30" Width="110px" runat="server"></asp:TextBox>
                                        <%--<asp:Image runat="server" ID="imgFromPDC" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <cc1:CalendarExtender runat="server" ID="calFromPDC" TargetControlID="txtPDCChqDate"
                                            PopupButtonID="imgFromPDC" Format="dd-MMM-yyyy">
                                        </cc1:CalendarExtender>--%>

                                        <asp:Image ID="imgFrom" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender ID="CalendarExtender12" runat="server" Format="dd-MMM-yyyy"
                                       CssClass="MyCalendar"  PopupButtonID="imgFrom" TargetControlID="txtPDCChqDate">
                                    </cc1:CalendarExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Category">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlCategory" runat="server" Width="130px" Height="19px">
                                        <asp:ListItem Text="STB ACTIVATION" Value="HINV" />
                                             <asp:ListItem Text="SUBSCRIPTION" Value="SINV" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpdcamount" Height="19px" Width="130px" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderAmt" runat="server" TargetControlID="txtpdcamount"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="Button2" runat="server" CssClass="button" Text="Submit" Width="100px"
                            OnClick="btnpdcsubmit_click" />
                        <asp:Button ID="Button4" runat="server" CssClass="button" Text="Reset" Width="100px"
                            OnClick="btnpdcClose_click" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    




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
                                <asp:Label ID="Label89" runat="server" Text="LCO Code  "></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label ID="lblcust" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label102" runat="server" Text="No. of STB  "></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblnoofstb"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Scheme Name
                            </td>
                            <td align="center">
                                :
                            </td>
                            <td align="left" colspan="2">
                                <asp:Label runat="server" ID="lblSchemeName"></asp:Label>
                            </td>
                        </tr>
                        <tr  style="display:none;">
                            <td align="left">
                                <asp:Label runat="server" ID="lblSTBTotalText" Text=""></asp:Label>
                            </td>
                            <td align="center">
                                :
                            </td>
                            <td align="left" colspan="2">
                                <asp:Label runat="server" ID="lblSTBTotal"></asp:Label>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td align="left">
                                
                                <asp:Label runat="server" ID="lblLCOSubscriptionTotalText" Text=""></asp:Label>
                            </td>
                            <td align="center">
                                :
                            </td>
                            <td align="left" colspan="2">
                                <asp:Label runat="server" ID="lblLCOTotal"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCofAmount" runat="server" Text="Amount "></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblamount1"></asp:Label>
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


                <cc1:ModalPopupExtender ID="popCheques2" runat="server" BehaviorID="Cheques2" Drag="true"
                TargetControlID="hdnPop3" PopupControlID="pnlCheque2" DropShadow="true" BackgroundCssClass="transparent">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnPop3" runat="server" />
            <asp:Panel ID="pnlCheque2" runat="server" CssClass="Popup" Style="width: 650px; height: 400px;
                display: none;">
                <%-- display: none; --%>
                <div class="body">
                <%-- display: none; --%>
     
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;&nbsp;STB Details
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                    <asp:Label ID="lblmsg1" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table width="500px">
                            <tr>
                                <td align="center" width="100%">
                                    <asp:GridView ID="gridCheques" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <HeaderStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="STB No.">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSTBNo" runat="server" Text="" Width="200px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Box Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlboxtype" runat="server" Width="50px" Height="19px" Style="resize: none;">
                                                        <asp:ListItem Selected="True" Value="SD">HD</asp:ListItem>
                                                        <asp:ListItem Value="HD">SD</asp:ListItem>
                                                        <asp:ListItem Value="PVL">PVL</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Repair Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlRepairType" runat="server" Width="100px" Height="19px" Style="resize: none;">
                                                        <asp:ListItem Selected="True" Value="Fatul">Fatul</asp:ListItem>
                                                        <asp:ListItem Value="Card Error">Card Error</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="btnChangePass" runat="server" CssClass="button" Text="Submit" Width="100px"
                                        OnClick="btngrdsubmit_click" />
                                    <asp:Button ID="Button1" runat="server" CssClass="button" Text="Cancel" Width="100px"
                                        OnClientClick="closeMsgPopup();return false;" />
                                </td>
                            </tr>
                        </table>
                    </center>
            </asp:Panel>
      
    <%-- Scheme Details --%>
     <cc1:ModalPopupExtender ID="popCheques3" runat="server" BehaviorID="popCheques3" Drag="true"
                TargetControlID="hdnPop2" PopupControlID="pnlCheque" DropShadow="true" BackgroundCssClass="transparent">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnPop2" runat="server" />
            <asp:Panel ID="pnlCheque" runat="server" CssClass="Popup" Style="width: 650px; height: 400px;
                display: none;">
                <%-- display: none; --%>
                <div class="body">
                           <center>
                    <br />
                         <table width="100%" >
                            <tr>
                                <td align="left" colspan="6" style="color: #094791; font-weight: bold;">
                                    <b>
                                        <asp:Label runat="server" ID="Label070" Text="Scheme Details"></asp:Label>
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
                                <td align="left" class="style71">
                                    <asp:Label ID="lblCashAmt" runat="server" Text="Scheme Name"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label050" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                   <asp:Label ID="lblDetSchemeName" runat="server" Text="*"></asp:Label>
                                </td>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label53" runat="server" Text="Scheme Desc"></asp:Label>
                                    <td width="5px">
                                        <asp:Label ID="Label62" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" class="style68">
                                    <asp:Label ID="lblDetschemedescr" runat="server"  Text="*"></asp:Label>
                                    </td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label70" runat="server" Text="Termination Allowed"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label72" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                    <asp:Label ID="lblDettermallow" runat="server"  Text="*"></asp:Label>
                                </td>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label73" runat="server" Text="Penalty on Foreclosure"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label76" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                    <asp:Label ID="lblDetpenalty" runat="server"  Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label78" runat="server" Text="Plan Change Allow"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label80" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                    <asp:Label ID="lblDetPlanchangeallow" runat="server"  Text="*"></asp:Label>
                                </td>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label81" runat="server" Text="Plan Activation Allow"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label83" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                    <asp:Label ID="lblDetplanactallow" runat="server"  Text="*"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="2">
                            <tr>
                                <td colspan="6" align="left">
                                    <b>
                                        <asp:Label runat="server" ID="Label84" Text="Subscription Details"></asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label85" runat="server" Text="Subscription Payterm"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label87" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                    <asp:Label ID="lblDetsubpayterm" runat="server"  Text="*"></asp:Label>
                                </td>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label88" runat="server" Text="Subscription Plan Allowed"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label92" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                <asp:Label ID="lblDetsubscrplanallow" runat="server"  Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label93" runat="server" Text="Subscription LCO Rate"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label95" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style71">
                                <asp:Label ID="lblDetLCORate" runat="server"  Text="*"></asp:Label>
                                </td>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label96" runat="server" Text="Subscription Discount LCO Rate"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label98" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                <asp:Label ID="lblDetLCODiscount" runat="server"  Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label99" runat="server" Text="Subscription Net LCO Rate"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label101" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                <asp:Label ID="lblDetLCONet" runat="server"  Text="*"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="2">
                            <tr>
                                <td colspan="6" align="left">
                                    <b>
                                        <asp:Label runat="server" ID="Label104" Text="STB Details"></asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label105" runat="server" Text="STB Count"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label107" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                <asp:Label ID="lblDetStbCount" runat="server"  Text="*"></asp:Label>
                                </td>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label108" runat="server" Text="STB status"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label110" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                    <asp:Label ID="lblDetStbStatus" runat="server"  Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label49" runat="server" Text="STB Type"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label50" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                     <asp:Label ID="lblDetBoxType" runat="server"  Text="*"></asp:Label>
                                </td>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label049" runat="server" Text="STB make & model"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Labe50" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                     <asp:Label ID="lblDetstbmakemodule" runat="server"  Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label053" runat="server" Text="STB Rate"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label61" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                    <asp:Label ID="lblDetSTBRate" runat="server"  Text="*"></asp:Label>
                                </td>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label062" runat="server" Text="STB Discount"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label63" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                       <asp:Label ID="lblSTBDiscount" runat="server"  Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="style71">
                                    <asp:Label ID="Label64" runat="server" Text="STB Net"></asp:Label>
                                    </td>
                                <td width="5px">
                                    <asp:Label ID="Label65" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" class="style68">
                                           <asp:Label ID="lblSTBNet" runat="server"  Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            <td align="center" colspan="3">
                                &nbsp;&nbsp;
                                <input id="Button3" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closeShcemeDetPopup();" />
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
            <asp:PostBackTrigger ControlID="btnModifyConfirm" />
            <asp:PostBackTrigger ControlID="RBPaymode" />
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
