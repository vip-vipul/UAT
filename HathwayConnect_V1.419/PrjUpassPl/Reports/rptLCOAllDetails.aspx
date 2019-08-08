<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptLCOAllDetails.aspx.cs" Inherits="PrjUpassPl.Reports.rptLCOAllDetails" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 780px;
            margin: 5px;
            background-color: White;
            margin-top: 8px;
            height: 415px;
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

        function InProgress() {

            document.getElementById("imgrefresh2").style.visibility = 'visible';
        }
        function onComplete() {

            document.getElementById("imgrefresh2").style.visibility = 'hidden';
        }

        function goBack() {
            window.history.back();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                
                <div class="maindive">
                    <div style="float: right">
                        <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                            Back</button>
                    </div>
                    <div runat="server" id="divLcoAllSearch">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSearchlbl" runat="server" Text="LCO :"></asp:Label>
                                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            </td>
                            <td align="right">
                                &nbsp;
                            </td>
                            <td align="left">
                                <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtSearch"
                            FilterType="UppercaseLetters, LowercaseLetters, Numbers">
                        </cc1:FilteredTextBoxExtender>--%>
                                <asp:DropDownList ID="ddlLco" AutoPostBack="true" runat="server" Height="19px" Style="resize: none;"
                                    Width="304px">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hfLCOCode" runat="server" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="button" UseSubmitBehavior="false"
                                    OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSearch"
                                    Display="Dynamic" runat="server" ErrorMessage="Enter LCO Code or Name and try again"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                    </table>
                </div>
                    <div style="padding: 100px; height: 210px; margin-top: -100px; margin-left: -80px;
                        margin-right: -80px">
                        <cc1:Accordion ID="LCOAccordion" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                            FadeTransitions="true" SuppressHeaderPostbacks="true" TransitionDuration="250"
                            FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None" Visible="false">
                            <Panes>
                                <cc1:AccordionPane ID="LCOAccordionPane" runat="server">
                                    <Header>
                                        <a href="#" class="href">LCO Details</a></Header>
                                    <Content>
                                        <table width="100%" height="110%">
                                            <tr>
                                                <td align="left" width="120px">
                                                    <asp:Label runat="server" ID="Label21" Font-Bold="true" Text="LCO Code"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblLCONo"></asp:Label>
                                                </td>
                                                <td align="left" width="60px">
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label6" Text="Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label9" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblLCOName"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="120px" height="20px">
                                                    <asp:Label runat="server" ID="Label15" Font-Bold="true" Text="Mobile No."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label16" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblmobno"></asp:Label>
                                                </td>
                                                <td align="left" width="60px">
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label18" Text="Email"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label19" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblEmail"></asp:Label>
                                                    <asp:HiddenField ID="hdnLcoId" runat="server" Value="" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="120px" height="20px">
                                                    <asp:Label runat="server" ID="Label7" Font-Bold="true" Text="Address"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label8" Text=":"></asp:Label>
                                                </td>
                                                <td align="left" colspan="4">
                                                    <asp:Label runat="server" ID="lblLCOAddr"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="120px" height="20px">
                                                    <asp:Label runat="server" ID="Label1" Font-Bold="true" Text="City"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label2" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblcity"></asp:Label>
                                                </td>
                                                <td align="left" width="60px">
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label4" Text="State"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label12" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblstate"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="120px" height="20px">
                                                    <asp:Label runat="server" ID="Label3" Font-Bold="true" Text="Distributor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lbldist"></asp:Label>
                                                </td>
                                                <td align="left" width="120px">
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label20" Text="Sub-Distributor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label22" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblsubdist"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="122px" height="20px">
                                                    <asp:Label runat="server" ID="Label10" Font-Bold="true" Text="ERP LCO A/C"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label17" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblerpaccno"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label14" Text="JV number"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label23" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lbljvno"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" height="20px">
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label28" Text="Universe Renewal"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label29" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblecsstatus"></asp:Label>
                                                </td>
                                                <td align="left" width="120px" height="20px">
                                                    <asp:Label runat="server" ID="Label24" Font-Bold="true" Text="Current Balance"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:Label runat="server" ID="lblCurrBalance"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label26" Text="Allocated Balance"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label25" Text=":"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblAllocatedBal"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label27" Text="Unallocated Balance"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label30" Text=":"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblUnallocatedBal"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label31" Text="Area Manager"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label32" Text=":"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblAreaM" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label34" Text="Executive"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label35" Text=":"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblExec"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label37" Text="P&T License Expiry Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label38" Text=":"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPtexdt" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" Font-Bold="true" ID="Label40" Text="Interconnect Agreement Expiry Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label41" Text=":"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblIntagreedt"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </Content>
                                </cc1:AccordionPane>
                            </Panes>
                        </cc1:Accordion>
                        <div class="griddiv">
                            <cc1:TabContainer ID="DetailsTab" Width="800" runat="server" Visible="false" ScrollBars="Auto"
                                ActiveTabIndex="1">
                                <cc1:TabPanel ID="tbpLastFive" runat="server" HeaderText="Last 5 Transactions">
                                    <ContentTemplate>
                                        <asp:Label ID="lbllastfive" ForeColor="Red" runat="server"></asp:Label>
                                        <asp:GridView ID="grdLastFive" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                                            ShowFooter="True" Width="100%" AllowSorting="True" OnRowDataBound="grdLastFive_RowDataBound"
                                            OnSorting="grdLastFive_Sorting" EnableModelValidation="True" Visible="false">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Customer ID" DataField="custid" FooterText="Total">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Customer Name" DataField="custname">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="VC ID" DataField="vc">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Plan Name" DataField="plnname">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Plan Type" DataField="plntyp">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Transaction Type" DataField="flag">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Reason" DataField="reason">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="MRP" DataField="custprice">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Amount Deducted" DataField="amtdd">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Balance" DataField="bal">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Pay Term" DataField="payterm">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="User ID" DataField="uname">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="User Name" DataField="userowner">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Transaction Date & Time" DataField="tdt">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Expiry Date" DataField="expdt">
                                                    <ControlStyle Width="75pt" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tbpRcptEntry" runat="server" HeaderText="Receipt Entry Details">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltop" ForeColor="Red" runat="server" Text=""></asp:Label>
                                        <asp:GridView ID="grdTopup" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                                            ShowFooter="true" Width="100%" AllowSorting="true" OnRowDataBound="grdTopup_RowDataBound"
                                            OnSorting="grdTopup_Sorting" Visible="false">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Date & Time" DataField="dtttime" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="left" FooterText="Total" />
                                                <asp:BoundField HeaderText="Amount" DataField="amt" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                                <asp:BoundField HeaderText="Mode Of Payment" DataField="paymode" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="left" />
                                                <asp:BoundField HeaderText="ERP Receipt No." DataField="erprcptno" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="left" />
                                                <asp:BoundField HeaderText="UPASS Transaction ID" DataField="rcptno" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="left" />
                                                <asp:BoundField HeaderText="Finance User Id" DataField="finuid" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="left" />
                                                <asp:BoundField HeaderText="Finance User Name" DataField="fiuname" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="left" />
                                                <asp:BoundField HeaderText="Action" DataField="action" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <%--  <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="LCO Name" DataField="lconame" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="JV Name" DataField="jvname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />--%>
                                                <%--<asp:BoundField HeaderText="ERP LCO A/C" DataField="erplco_ac" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />--%>
                                                <%-- <asp:BoundField HeaderText="Distributor" DataField="distname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Sub Distributor" DataField="subdist" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="City" DataField="city" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="State" DataField="state" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />--%>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tbpRcptRev" runat="server" HeaderText="Receipt Reversal Details">
                                    <ContentTemplate>
                                        <asp:Label ID="lblrevrs" ForeColor="Red" runat="server" Text=""></asp:Label>
                                        <asp:GridView ID="grdReversal" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                                            ShowFooter="true" Width="100%" AllowSorting="true" OnRowDataBound="grdReversal_RowDataBound"
                                            OnSorting="grdReversal_Sorting">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="100px" ItemStyle-HorizontalAlign="center" FooterText="Total" />
                            <asp:BoundField HeaderText="Company Code" DataField="companycode" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="100px" ItemStyle-HorizontalAlign="center" />--%>
                                                <asp:BoundField HeaderText="Voucher NO." DataField="voucherno" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="150px" ItemStyle-HorizontalAlign="center" />
                                                <asp:BoundField HeaderText="Amount" DataField="amount" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                                <asp:BoundField HeaderText="Reason" DataField="reasonname" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="250px" ItemStyle-HorizontalAlign="center" />
                                                <asp:BoundField HeaderText="Remark" DataField="lcopayremark" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                                                <asp:BoundField HeaderText="Inserted By" DataField="insby" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                                                <asp:BoundField HeaderText="Inserted Date" DataField="date1" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="130px" ItemStyle-HorizontalAlign="center" />
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tbpPartyLed" runat="server" HeaderText="Party Leadger Details">
                                    <ContentTemplate>
                                        <asp:Label ID="lblPartyLed" ForeColor="Red" runat="server"></asp:Label>
                                        <asp:GridView runat="server" ID="grdPartyLed" CssClass="Grid" AutoGenerateColumns="False"
                                            ShowFooter="True" EnableModelValidation="True" OnRowDataBound="grdPartyLed_RowDataBound">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="LCO Code" DataField="lcocode" FooterText="Total">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="LCO Name" DataField="lconame">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Opening Balance" DataField="openinbal">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Debit" DataField="drlimit">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Credit" DataField="crlimit">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Closing Balance" DataField="closingbal">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                        <asp:GridView runat="server" ID="grdPartyLedDet" CssClass="Grid" AutoGenerateColumns="False"
                                            ShowFooter="True" EnableModelValidation="True" OnRowDataBound="grdPartyLedDet_RowDataBound">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="15pt" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Ledger Date" DataField="dt1">
                                                    <ControlStyle Width="15pt" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Ledger Type" DataField="ltype">
                                                    <ControlStyle Width="15pt" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Remark" DataField="premark">
                                                    <ControlStyle Width="15pt" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Debit" DataField="drlimit">
                                                    <ControlStyle Width="15pt" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Credit" DataField="crlimit">
                                                    <ControlStyle Width="15pt" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Balance" DataField="balance">
                                                    <ControlStyle Width="15pt" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tbpService" runat="server" HeaderText="Service Status Details">
                                    <ContentTemplate>
                                        <asp:Label ID="lblServiceData" ForeColor="Red" runat="server"></asp:Label>
                                        <asp:GridView runat="server" ID="grdactdact" CssClass="Grid" AutoGenerateColumns="False"
                                            ShowFooter="True" EnableModelValidation="True">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="STBNO" HeaderText="STB Number" SortExpression="lcocode">
                                                    <ControlStyle Width="65pt" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CUSTNO" HeaderText="Customer Code" SortExpression="lcocode">
                                                    <ControlStyle Width="65pt" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ADDR" HeaderText="Address" SortExpression="crlimit" Visible="False">
                                                    <ControlStyle Width="35pt" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Account Poid" DataField="ACCPOID">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Service Poid" DataField="SVCPOID">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="VC Id" DataField="VCID">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Status" DataField="STATUS">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Transaction By" DataField="TRANSBY">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Transaction Date" DataField="TRANSDT">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Order Id" DataField="ORDERID">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Reason" DataField="reasonname">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tbpUserDet" runat="server" HeaderText="User Details">
                                    <ContentTemplate>
                                        <asp:Label ID="lblUserDet" ForeColor="Red" runat="server"></asp:Label>
                                        <asp:GridView ID="grdUserDet" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                                            ShowFooter="True" Width="100%" AllowSorting="True" EnableModelValidation="True">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="User Id" DataField="username">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <%# Eval("fname").ToString() %>&nbsp;<%# Eval("mname").ToString()%>&nbsp;<%# Eval("lname").ToString()%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="Address" DataField="addr">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                                                <%--  <asp:BoundField HeaderText="Pincode" DataField="code">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                                                <asp:BoundField HeaderText="BrmPoid" DataField="brmpoid">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%-- <asp:BoundField HeaderText="State" DataField="ststeid">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="City" DataField="cityid">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                                                <asp:BoundField HeaderText="Email" DataField="email">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Mobile No" DataField="mobno">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Account No" DataField="accno">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Balance" DataField="balance">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Inserted By" DataField="insby">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Inserted Date" DataField="insdt">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>
                    </div>
                </div>
                <%--<table width="100%">
            <tr>
                <td align="left" class="style67">
                    <asp:Button runat="server" ID="btngrnExel" Text="Generate Excel" CssClass="button"
                        Visible="false" UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" />
                </td>
                <td class="style68">
                </td>
                <td>
                </td>
            </tr>
        </table>--%>
                <%--<table width="100%">
            <tr>
                <td align="left" class="style67" colspan="3">
                    <asp:Label ID="Label1" runat="server" Font-Names="Tahoma" Font-Bold="true" Font-Size="9pt"
                        Visible="false" Text="LCO Details : "></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style67" colspan="3">
                    <asp:Label ID="lbllcodet" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <asp:GridView ID="grdLcoDet" runat="server" AutoGenerateColumns="false" CssClass="Grid"
            ShowFooter="true" Width="100%" AllowSorting="true" OnRowDataBound="grdLcoDet_RowDataBound"
            OnSorting="grdLcoDet_Sorting">
            <FooterStyle CssClass="GridFooter" />
            <Columns>
                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left" FooterText="Total" />
                <asp:BoundField HeaderText="LCO Name" DataField="lconame" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left" />
                <asp:BoundField HeaderText="Awailable Balance" DataField="awailbal" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
            </Columns>
        </asp:GridView>--%>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <%-- <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:ImageButton ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()">
                </asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
           <%-- <asp:PostBackTrigger ControlID="rdolstSubsSearch" />--%>
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
