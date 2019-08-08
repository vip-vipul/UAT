<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptPreCustLastTransDet.aspx.cs" Inherits="PrjUpassPl.Reports.rptPreCustLastTransDet" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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
             window.location.href = "../Reports/rptnoncasreport.aspx";
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <div style="width: 30%;">
                        <table width="100%">
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table width="100%">
                        <tr>
                            <td style="width: 100px">
                                <asp:Button runat="server" ID="btn_genExl" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" />
                            </td>
                            <td>
                                <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                                <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div class="griddiv">
                    <asp:GridView ID="grdlasttrans" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                        ShowFooter="true" Width="100%" AllowSorting="true" AllowPaging="true" PageSize="100"
                        OnRowDataBound="grdlasttrans_RowDataBound" OnSorting="grdlasttrans_Sorting" OnPageIndexChanging="grdlasttrans_PageIndexChanging">
                        <FooterStyle CssClass="GridFooter" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Transaction ID" DataField="transid" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" FooterText="" />
                            <asp:BoundField HeaderText="Receipt No." DataField="receiptno" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Customer ID" DataField="custid" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Customer Name" DataField="custname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="VC ID" DataField="vcid" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Plan ID" DataField="planid" HeaderStyle-HorizontalAlign="Center"
                                Visible="false" ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Plan Name" DataField="planname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Plan Type" DataField="plantype" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Customer Price" DataField="custprice" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="LCO Price" DataField="lcoprice" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Expiry" DataField="expirydt" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Payment Term" DataField="payterm" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Balance" DataField="balance" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Company Code" DataField="companycode" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Status" DataField="flag" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Transaction By" DataField="lconame" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Transaction Date" DataField="transdt1" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
               <%-- <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                     <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_genExl" />
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
