<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TransHwayBulkRenewConf.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayBulkRenewConf" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 500px;
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
        .loader h3
        {
            padding: 10px;
            position: fixed;
            top: 55%;
            left: 30%;
        }
    </style>
    <script type="text/javascript">
        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
        function hideTrans(ctrl) {
            ctrl.style.visibility = 'hidden';
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <asp:Label runat="server" ID="lblConfirmTxt" Text="Are you sure you want to perform following selected renew transaction ?"></asp:Label>
            <br />
            <asp:Label runat="server" ID="lblTransCount"></asp:Label>
            <br />
            <h3>
                <asp:Label runat="server" ID="lblStatusHeading" Text=""></asp:Label>
            </h3>
            <asp:Label runat="server" ID="lblStatus" Text="" ForeColor="Red"></asp:Label>
            <br />
            <asp:Button runat="server" Width="150px" ID="btnBeginTrans" Text="Begin Renewals" Visible="false"
                OnClientClick="hideTrans(this);" OnClick="btnBeginTrans_Click" />
            &nbsp;
            <asp:Button runat="server" Width="150px" ID="btnCancelTransactions" 
                Text="Cancel" Visible="false" onclick="btnCancelTransactions_Click" />
            <asp:GridView runat="server" ID="grdExpiry" CssClass="Grid" AutoGenerateColumns="false"
                ShowFooter="true" AllowPaging="true" PageSize="500">
                <FooterStyle CssClass="GridFooter" />
                <Columns>
                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Account Number" DataField="account_no" HeaderStyle-HorizontalAlign="Center"
                        Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <FooterStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="VC Id" DataField="vc" HeaderStyle-HorizontalAlign="Center"
                        Visible="true" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Lco Code" DataField="lco_code" HeaderStyle-HorizontalAlign="Center"
                        Visible="true" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Package" DataField="planname" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Auto Renew" DataField="AutoRenew" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>


                </Columns>
                <PagerSettings Mode="Numeric" />
            </asp:GridView>
            <%-- ----------------------------------------------------Loader------------------------------------------------------------------ --%>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
                <br />
                <h3>
                    Transactions are in progress, Do not refresh or close the browser</h3>
            </div>
            <asp:Panel ID="pnlSummary" runat="server" Visible="false">
                <table class="Grid">
                    <tr>
                        <th>Final Name</th>
                        <th>Total Transactions</th>
                        <th>Successful Transactions</th>
                        <th>Failed Transactions</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSumFile" runat="server" Text=""></asp:Label>    
                        </td>
                        <td>
                            <asp:LinkButton ID="lblSumTotal" runat="server" Text="0" 
                                onclick="lblSumTotal_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lblSumSuccess" runat="server" Text="0" 
                                onclick="lblSumSuccess_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lblSumFailure" runat="server" Text="0" 
                                onclick="lblSumFailure_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCancelTransactions" />
            <asp:PostBackTrigger ControlID="lblSumTotal" />
            <asp:PostBackTrigger ControlID="lblSumSuccess" />
            <asp:PostBackTrigger ControlID="lblSumFailure" />
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
