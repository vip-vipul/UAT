<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransHwayLcoPayment.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayLcoPayment" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Distributor Balance</title>
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
            margin: 15px;
            width: 880px;
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
        .style67
        {
            width: 145px;
        }
        .style68
        {
            width: 104px;
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
            .maindive
        {
          background-color:white;
        height:610px;
        margin-top:-18px;
         margin-bottom:100px;
         margin-left:100px;
         margin-right:101px;
       
       
            
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript" language="javascript">
        function hideButton() {
            $("#<%= btnSubmit.ClientID %>").hide();
        }
        function getSelectedBankId(sender, e) {
            $get("<%=hfBankId.ClientID %>").value = e.get_value();
        }
        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
            $('#<%= txtCashAmt.ClientID %>').keyup(function () {
                inWords($('#<%= txtCashAmt.ClientID %>').val());
            });
        }
        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey(parseInt('<%= RadSearchby.SelectedValue %>'));
        }
        //        var a = ['', 'one ', 'two ', 'three ', 'four ', 'five ', 'six ', 'seven ', 'eight ', 'nine ', 'ten ', 'eleven ', 'twelve ', 'thirteen ', 'fourteen ', 'fifteen ', 'sixteen ', 'seventeen ', 'eighteen ', 'nineteen '];
        //        var b = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];
        var a = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine ', 'Ten ', 'Eleven ', 'Twelve ', 'Thirteen ', 'Fourteen ', 'Fifteen ', 'Sixteen ', 'Seventeen ', 'Eighteen ', 'Nineteen '];
        var b = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];

        $(function () {
            $('#<%= txtCashAmt.ClientID %>').keyup(function () {
                inWords($('#<%= txtCashAmt.ClientID %>').val());
            });
        });

        function inWords(num) {
            if ((num = num.toString()).length > 9) return 'overflow';
            n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
            if (!n) return;
            var str = '';
            str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'Crore ' : '';
            str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'Lakh ' : '';
            str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'Thousand ' : '';
            str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'Hundred ' : '';
            str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + 'only ' : '';
            $('#<%= lblamtinword.ClientID %>').text(str)
        }

    </script>
     <div class="maindive">
    <asp:Panel ID="pnlView" runat="server" ScrollBars="Auto">
    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblmsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
            </ContentTemplate>
            <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            </Triggers>--%>
        </asp:UpdatePanel>
       
       
            <div class="delInfo">
                <table id="Table1" runat="server" align="center" width="100%" border="0" >
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblDist" runat="server" Text="Search LCO By"></asp:Label>
                            &nbsp;
                            <asp:Label ID="Label22" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="RadSearchby" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="True">
                                <%-- OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged"  --%>
                                <asp:ListItem Value="0" Selected="True">Code</asp:ListItem>
                                <asp:ListItem Value="1">Name</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLCOSearch" runat="server" Width="200px" onkeydown = "SetContextKey()"></asp:TextBox>
                            &nbsp;
                            <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1"
                                UseContextKey="true" CompletionInterval="100" EnableCaching="true" CompletionSetCount="3"
                                TargetControlID="txtLCOSearch" FirstRowSelected="false" ID="AutoCompleteExtender1"
                                runat="server" CompletionListCssClass="autocomplete" CompletionListItemCssClass="autocompleteItem"
                                CompletionListHighlightedItemCssClass="autocompleteItemHover">
                            </cc1:AutoCompleteExtender>
                            <asp:HiddenField ID="hfCustomerId" runat="server" />
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
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
                        FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None">
                        <Panes>
                            <cc1:AccordionPane ID="LCOAccordionPane" runat="server">
                                <Header>
                                    <a href="#" class="href">LCO Details</a></Header>
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
                                            <td colspan="6" width="400px">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="80px">
                                                <asp:Label runat="server" ID="Label21" Text="LCO Code"></asp:Label>
                                            </td>
                                            <td align="left" width="10px">
                                                <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                                            </td>
                                            <td align="left" width="325px">
                                                <asp:Label runat="server" ID="lblCustNo"></asp:Label>
                                            </td>
                                            <td align="right" width="80px">
                                                <asp:Label runat="server" ID="Label24" Text="Name"></asp:Label>
                                            </td>
                                             <td>
                                                <asp:Label runat="server" ID="Label26" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustName"></asp:Label>
                                            </td>
                                        </tr>

                                          <tr>
                                             <td align="left" width="80px">
                                                <asp:Label runat="server" ID="Label5" Text="Address"></asp:Label>
                                            </td>
                                            <td align="left" width="10px">
                                                <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                                            </td>
                                            <td align="left" width="325px">
                                                 <asp:Label runat="server" ID="lblCustAddr"></asp:Label>
                                            </td>
                                            <td align="right" width="80px">
                                                    <asp:Label runat="server" ID="Label15" Text="Mobile No."></asp:Label>
                                            </td>
                                             <td>
                                                  <asp:Label runat="server" ID="Label16" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                 <asp:Label runat="server" ID="lblmobno"></asp:Label>
                                            </td>
                                        </tr>


                                         <tr>
                                             <td align="left" width="80px">
                                                <asp:Label runat="server" ID="Label18" Text="Email"></asp:Label>
                                            </td>
                                            <td align="left" width="10px">
                                               <asp:Label runat="server" ID="Label19" Text=":"></asp:Label>
                                            </td>
                                            <td align="left" width="325px">
                                                  <asp:Label runat="server" ID="lblEmail"></asp:Label>
                                            </td>
                                            <td align="right" width="130px">
                                              <asp:Label runat="server" ID="Label10" Text="Current Balance"></asp:Label>
                                            </td>
                                             <td>
                                            <asp:Label runat="server" ID="Label17" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                 <asp:Label runat="server" ID="lblCurrBalance"></asp:Label>
                                            </td>
                                        </tr>




                                    
                                
                                        <tr>
                                            <td align="left" width="80px">
                                               
                                            </td>
                                            <td>
                                              
                                            </td>
                                            <td align="left">
                                                
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
                <div class="delInfo" id="Paydet">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="2">
                                <tr>
                                    <td colspan="6" align="left">
                                        <b>
                                            <asp:Label runat="server" ID="Label14" Text="Payment Details:"></asp:Label>
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
                                    <td width="120px" align="left">
                                        <asp:Label ID="lblCashAmt" runat="server" Text="Amount"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label20" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCashAmt" runat="server" Style="resize: none;" Width="200" MaxLength="9"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtCashAmt"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="120px" align="left">
                                        <asp:Label ID="Label8" runat="server" Text="ERP Receipt No."></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label9" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtReceipt" runat="server" Style="resize: none;" Width="200"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtReceipt"
                                            ValidChars="0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="120px" align="left">
                                        <asp:Label ID="Label23" runat="server" Text="Amount In Words"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label25" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="4">
                                        <asp:Label ID="lblamtinword" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr id="tdmode" runat="server">
                                    <td align="left" width="120px">
                                        <asp:Label ID="lbldepmode" runat="server" Text="Payment Mode"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="lblmode" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="clndepmode" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" width="180px">
                                        <asp:RadioButtonList ID="RBPaymode" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="RBPaymode_SelectedIndexChanged"
                                            AutoPostBack="True">
                                            <asp:ListItem Value="C" Selected="True">Cash</asp:ListItem>
                                            <asp:ListItem Value="Q">Cheque</asp:ListItem>
                                            <asp:ListItem Value="DD">DD</asp:ListItem>
                                            <asp:ListItem Value="N">NEFT</asp:ListItem>
                                            <asp:ListItem Value="O">Online</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="left">
                                        <div runat="server" id="divChqDDno">
                                            <asp:TextBox ID="txtChqDDno" MaxLength="6" runat="server" Width="120"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtWatermark" runat="server" TargetControlID="txtChqDDno"
                                                WatermarkText="Cheque/DD/Ref No" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtChqDDno"
                                                FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" ">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                    </td>
                                    <td align="left" width="70px">
                                        <div runat="server" id="divDateLabel">
                                            <asp:Label ID="lblDate" runat="server" Text="Date :"></asp:Label>
                                            <asp:Label ID="lblDateStar" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                    </td>
                                    <td align="left">
                                        <div runat="server" id="divDateBox">
                                            <asp:TextBox runat="server" ID="txtFrom" Width="80px"></asp:TextBox>
                                            <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                            <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr id="tdbnnm" runat="server">
                                    <td align="left" width="120px">
                                        <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="lblbanknme" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="ClnBankName" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtBankName" runat="server" AutoComplete="off" Width="200"></asp:TextBox>
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
                                        <asp:Label ID="lblbranchnm" runat="server" Text="Branch Name"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="lblbranch" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtbranchnm" runat="server" Style="resize: none;" Width="200"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender188" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom"
                                            TargetControlID="txtbranchnm" ValidChars=" -_">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr id="Tr1" runat="server">
                                    <td align="left" width="120px">
                                        <asp:Label ID="lblReferenceNo" runat="server" Text="Remark"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="4">
                                        <asp:TextBox ID="txtRemark" runat="server" Style="resize: none;" Width="200" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom"
                                            TargetControlID="txtRemark" ValidChars=" -_">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </div>
                <div class="delInfo">
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSubmit" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <%--<asp:Button ID="btnCancel" TabIndex="2" runat="server" Font-Bold="True" Text="Cancel"
                                            class="button" Width="60" OnClick="btnCancel_Click1"></asp:Button>
                                        &nbsp;&nbsp;--%>
                                        <asp:Button ID="btnSubmit" TabIndex="2" runat="server" Font-Bold="True" Text="Submit"
                                            class="button" Width="60" OnClientClick="hideButton();" OnClick="btnSubmit_Click">
                                        </asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <%-- ----------------------------------------------------Loader------------------------------------------------------------------ --%>
        <div id="imgrefresh" class="loader transparent">
            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                ToolTip="Loading ..." Style="" />
        </div>
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
        <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender2" runat="server"
            TargetControlID="UpdatePanel2">
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
    </asp:Panel>
</asp:Content>
