<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransHwayLcoReprintReceipt.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayLcoReprintReceipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
        }
    </script>
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
            width: 840px;
        }
        .delInfoContent
        {
            width: 96%;
        }
        .scroller
        {
            overflow: auto;
            max-height: 250px;
        }
        .completionList
        {
            border: solid 1px Gray;
            margin: 0px;
            padding: 3px;
            height: 120px;
            overflow: auto;
            background-color: #FFFFFF;
        }
        .listItem
        {
            color: #191919;
        }
        .itemHighlighted
        {
            background-color: #ADD6FF;
        }
        .style67
        {
            width: 290px;
        }
        .style68
        {
            width: 127px;
            margin-left: 40px;
        }
        .style69
        {
            width: 130px;
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
        Sys.Browser.WebKit = {};
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }
        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
            document.getElementById("imgrefresh2").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
            document.getElementById("imgrefresh2").style.visibility = 'hidden';
        }
        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey(parseInt('<%= rbtnsearch.SelectedValue %>'));
        }
    </script>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
            <div>
                <table cellspacing="0" cellpadding="0" align="center" border="0" width="80%">
                    <tr>
                        <td align="center">
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center">
                                        <div class="delInfo">
                                            <table runat="server" align="center" width="100%" id="tbl1" border="0">
                                                <tr>
                                                    <td align="right" width="100">
                                                        <asp:Label ID="lblDist" runat="server" Text="Search By"></asp:Label>
                                                        &nbsp;
                                                        <asp:Label ID="Label24" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:RadioButtonList ID="rbtnsearch" AutoPostBack="true" runat="server" OnSelectedIndexChanged="rbtnsearch_SelectedIndexChanged"
                                                            RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Receipt No." Selected="True" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Lco Code" Value="1"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <%--<td id="tdBankName" runat="server" visible="false">
                                                        <asp:DropDownList ID="ddlBankName" Width="130" runat="server">
                                                        </asp:DropDownList>
                                                    </td>--%>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtLCOSearch" runat="server" Style="resize: none;" Width="110px" onkeyup="SetContextKey()"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1" UseContextKey = "true"
                                                            CompletionInterval="100" EnableCaching="true" CompletionSetCount="3" TargetControlID="txtLCOSearch"
                                                            FirstRowSelected="false" ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="autocomplete"
                                                            CompletionListItemCssClass="autocompleteItem" CompletionListHighlightedItemCssClass="autocompleteItemHover"
                                                            OnClientItemSelected="ClientItemSelected">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hfCustomerId" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divdet" runat="server">
                                            <div class="delInfo">
                                                <%--<cc1:Accordion ID="DetailsAccordion" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                                                            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                                            FadeTransitions="true" SuppressHeaderPostbacks="true" TransitionDuration="250"
                                                            FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None">
                                                            <Panes>
                                                                <cc1:AccordionPane ID="DetailsAccordionPane" runat="server">
                                                                    <Header>
                                                                        <a href="#" class="href">LCO & Payment Details</a></Header>
                                                                    <Content>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td style="vertical-align: top;" width="50%">
                                                                                    <table class="delInfoContent" style="padding-right: 50px; margin-top: 1px;">
                                                                                        <tr>
                                                                                            <td colspan="3" align="left">
                                                                                                <b>
                                                                                                    <asp:Label runat="server" ID="Label5" Text="LCO Details:"></asp:Label>
                                                                                                </b>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <hr />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style69">
                                                                                                <asp:Label runat="server" ID="Label21" Text="LCO Code"></asp:Label>
                                                                                            </td>
                                                                                            <td width="10px">
                                                                                                <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblLcoCode" Text=""></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="Label4" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style69">
                                                                                                <asp:Label runat="server" ID="Label12" Text="LCO Name"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblLcoName" Text=""></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="Label9" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style69">
                                                                                                <asp:Label runat="server" ID="Label6" Text="Address"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label runat="server" ID="Label8" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left" colspan="2">
                                                                                                <asp:Label runat="server" ID="lblLcoAddr"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style69">
                                                                                                <asp:Label runat="server" ID="Label15" Text="Mobile No."></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label runat="server" ID="Label2" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblLcoMobno" Text=""></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="Label14" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style69">
                                                                                                <asp:Label runat="server" ID="Label7" Text="Email"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label runat="server" ID="Label19" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblLcoEmail" Text=""></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="Label18" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td style="vertical-align: top;">
                                                                                    <table class="delInfoContent" style="margin-top: 2px;">
                                                                                        <tr>
                                                                                            <td colspan="3" align="left">
                                                                                                <b>
                                                                                                    <asp:Label runat="server" ID="lblpayment" Text="Payment Details:"></asp:Label>
                                                                                                </b>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="3">
                                                                                                <hr />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style68">
                                                                                                <asp:Label runat="server" ID="lblamount" Text="Amount"></asp:Label>
                                                                                            </td>
                                                                                            <td width="10px">
                                                                                                <asp:Label runat="server" ID="Label111" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblLcoAmt" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style68">
                                                                                                <asp:Label runat="server" ID="lblpaymode" Text="Pay Mode"></asp:Label>
                                                                                            </td>
                                                                                            <td width="10px">
                                                                                                <asp:Label runat="server" ID="Label131" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblLcoPaymode" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style68">
                                                                                                <asp:Label runat="server" ID="lblbank" Text="Bank"></asp:Label>
                                                                                            </td>
                                                                                            <td width="10px">
                                                                                                <asp:Label runat="server" ID="Label102" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblLcoBank" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style68">
                                                                                                <asp:Label runat="server" ID="lblbranch" Text="Branch"></asp:Label>
                                                                                            </td>
                                                                                            <td width="10px">
                                                                                                <asp:Label runat="server" ID="Label101" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblLcoBranch" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style68">
                                                                                                <asp:Label runat="server" ID="Label23" Text="Cheque No"></asp:Label>
                                                                                            </td>
                                                                                            <td width="10px">
                                                                                                <asp:Label runat="server" ID="Label25" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblChequeNo" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="style68">
                                                                                                <asp:Label runat="server" ID="Label27" Text="Cheque Date"></asp:Label>
                                                                                            </td>
                                                                                            <td width="10px">
                                                                                                <asp:Label runat="server" ID="Label28" Text=":"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label runat="server" ID="lblChequeDate" Text=""></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </Content>
                                                                </cc1:AccordionPane>
                                                            </Panes>
                                                        </cc1:Accordion>--%>
                                                <asp:GridView runat="server" ID="grdPaymentDet" CssClass="Grid" AutoGenerateColumns="false"
                                                    ShowFooter="true" AllowPaging="true" PageSize="10" 
                                                    OnRowCommand="grdPaymentDet_RowCommand" onrowdatabound="grdPaymentDet_RowDataBound">
                                                    <%-- OnRowDataBound="grdTransDet_RowDataBound" --%>
                                                    <FooterStyle CssClass="GridFooter" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Receipt No" DataField="receiptno" />
                                                        <asp:BoundField HeaderText="Lco Name" DataField="lconame" />
                                                        <asp:BoundField HeaderText="Mobile No" DataField="mobno" />
                                                        <asp:BoundField HeaderText="Amount" DataField="amt" ItemStyle-HorizontalAlign="Right" />
                                                        <asp:BoundField HeaderText="Paymode" DataField="paymode" />
                                                        <asp:BoundField HeaderText="Bank" DataField="bnknm" />
                                                        <asp:BoundField HeaderText="Cheque No" DataField="chqddno" />
                                                        <asp:BoundField HeaderText="Cheque Date" DataField="chequedt" />
                                                        <asp:BoundField HeaderText="Payment Date" DataField="paydt" />
                                                        <asp:BoundField HeaderText="Company" DataField="company" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbReprint" runat="server" Text="Reprint" CommandName="Reprint"></asp:LinkButton>
                                                                <asp:Label ID ="lblrev" runat ="server" Visible="false" Text="Reversed"></asp:Label>
                                                                <asp:HiddenField ID="hflcocode" runat="server" Value='<%# Eval("lcocode")%>' />
                                                                <asp:HiddenField ID="hfaddress" runat="server" Value='<%# Eval("address")%>' />
                                                                <asp:HiddenField ID="hfremrk" runat="server" Value='<%# Eval("remrk")%>' />
                                                                <asp:HiddenField ID="hfamt" runat="server" Value='<%# Eval("amt")%>' />
                                                                <asp:HiddenField ID="hfname" runat="server" Value='<%# Eval("name")%>' />
                                                                <asp:HiddenField ID="hfisactive" runat="server" Value='<%# Eval("isactive")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <%--<cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender2" runat="server"
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
                                        </cc1:UpdatePanelAnimationExtender>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <%-- ----------------------------------------------------Loader------------------------------------------------------------------ --%>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." />
            </div>
        <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rbtnsearch" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <%--<cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server"
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
    </cc1:UpdatePanelAnimationExtender>--%>
</asp:Content>
