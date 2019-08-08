<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptPrePartyLedgerDET.aspx.cs" Inherits="PrjUpassPl.Reports.rptPrePartyLedgerDET" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.GridFooter
        {
            border: 1px solid #094791;
            border-radius: 0px;
            color: White;
            background: #094791;
            width: 100px;
        }*/
        
        
        /*.style67
        {
            width: 142px;
        }
        .style68
        {
            width: 975px;
        }*/
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
             return false;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="maindive">
            <div style="float:right">
                <button onclick="return goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                <asp:Panel runat="server" ID="pnlTransItemDet">
                    <table width="100%">
                        <tr>
                            <th align="left">
                                <asp:Label ID="Label1" ForeColor="Black" Font-Bold="true" Font-Size="9pt" Font-Names="Tahoma"
                                    runat="server" Text="LCO Name :"></asp:Label>&nbsp;
                                <asp:Label ID="lbllconm" ForeColor="Black" Font-Bold="false" Font-Size="9pt" Font-Names="Tahoma"
                                    runat="server"></asp:Label>
                            </th>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
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
                        <asp:GridView runat="server" ID="grdPLcoDet" CssClass="Grid" AutoGenerateColumns="false"
                            ShowFooter="true" OnRowDataBound="grdPLcoDet_RowDataBound" AllowPaging="true"
                            PageSize="100" OnPageIndexChanging="grdPLcoDet_PageIndexChanging">
                            <FooterStyle CssClass="GridFooter" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" FooterText="" />
                                <asp:BoundField HeaderText="LCO Name" DataField="lconame" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Opening Balance" DataField="openinbal" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Debit" DataField="drlimit" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Credit" DataField="crlimit" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Closing Balance" DataField="closingbal" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                <%--<asp:BoundField HeaderText="Ledger Type" DataField="ltype" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left"   />
                <asp:BoundField HeaderText="Ledger Date" DataField="dt1" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Left"   />--%>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:GridView runat="server" ID="grdPartyLedger" CssClass="Grid" AutoGenerateColumns="false"
                            Width="68%" ShowFooter="true" OnRowDataBound="grdPartyLedger_RowDataBound" AllowPaging="true"
                            PageSize="100" OnSorting="grdPartyLedger_Sorting" OnPageIndexChanging="grdPartyLedger_PageIndexChanging">
                            <FooterStyle CssClass="GridFooter" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left" ControlStyle-Width="15pt">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left" FooterText="Total"   />
                <asp:BoundField HeaderText="LCO Name" DataField="lconame" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"   />--%>
                                <asp:BoundField HeaderText="Ledger Date" DataField="dt1" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" ControlStyle-Width="15pt" />
                                <asp:BoundField HeaderText="Ledger Type" DataField="ltype" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" ControlStyle-Width="15pt" />
                                <%--<asp:BoundField HeaderText="Opening Balance" DataField="openinbal" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"   />--%>
                                <asp:BoundField HeaderText="Remark" DataField="premark" HeaderStyle-HorizontalAlign="Center"
                                    ControlStyle-Width="15pt" ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Debit" DataField="drlimit" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ControlStyle-Width="15pt" />
                                <asp:BoundField HeaderText="Credit" DataField="crlimit" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ControlStyle-Width="15pt" />
                                <asp:BoundField HeaderText="Balance" DataField="balance" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ControlStyle-Width="15pt" />
                                    <asp:BoundField HeaderText="DAS Area" DataField="AREA" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ControlStyle-Width="15pt" />
                                <%--<asp:BoundField HeaderText="Closing Balance" DataField="closingbal" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"   />--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
            <div id="imgrefresh2" class="loader transparent">
               <%-- <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                     <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />--%>
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
