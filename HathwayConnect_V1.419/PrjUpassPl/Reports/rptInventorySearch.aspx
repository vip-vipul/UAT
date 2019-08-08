<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptInventorySearch.aspx.cs" Inherits="PrjUpassPl.Reports.rptInventorySearch" %>


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
        }

        function closePopup() {
            $find("mpeConfirmation").hide();
            return false;
        }
        function closeExpPopup() {
            $find("mpeExp").hide();
            return false;
        }

        function hideexcel() {
            var btn = document.getElementById('<%=btnGenerateExcel.ClientID%>');
            btn.style.visibility = 'hidden';
        }
        function Showexcel() {
            var btn = document.getElementById('<%=btnGenerateExcel.ClientID%>');
            btn.style.visibility = 'visible';
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="maindive">
                <div style="float: right">
                    <asp:Label ID="lblLastRefreshTime" runat="server" Text="" Style="margin-right: 50px;
                        margin-top: -15px;"></asp:Label>
                    <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                        Back</button>
                </div>
                <br />
                <asp:Panel runat="server" ID="pnlSearch">
                    <div class="delInfo">
                        <table runat="server" align="center" width="650px" id="tbl1" border="0">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblUser" runat="server" Text="Search By"></asp:Label>
                                    <asp:Label ID="Label37" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    <asp:Label ID="Label44" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="RadSearchby" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                        onclick="Showexcel()" >
                                        <asp:ListItem Value="121" Selected="True">Mac Id/ VM.</asp:ListItem>
                                        <asp:ListItem Value="121">STB No.</asp:ListItem>
                                        <asp:ListItem Value="120">VC Id</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsearchpara" runat="server" Style="resize: none;" Width="150px"
                                        onkeydown="SetContextKey()"></asp:TextBox>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                        CssClass="button" OnClientClick="Showexcel()" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:RadioButtonList ID="radCMRdate" runat="server" RepeatDirection="Horizontal" Visible="false">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                        UseSubmitBehavior="false" OnClick="btnGenerateExcel_Click"
                                        Visible="false" Width="105px" />
                                    
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--<table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                                &nbsp;
                                <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>--%>
                    <!--Gridview header Freeze By sanket 15.07.2016-->
                    <div id="DivRoot" runat="server" align="left" style="width: 100%">
                        <div style="overflow: hidden; width: 100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll; width: 100%" onscroll="OnScrollDiv(this)" id="DivMainContent">
                            <!--TEst GridView-->
                            <asp:GridView ID="grdtransdet" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                                CssClass="Grid"  PageSize="100"
                                ShowFooter="true">
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SerialNo" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                                        HeaderText="Serial No" ItemStyle-HorizontalAlign="Right" Visible="true" />
                                   
                                    <asp:BoundField DataField="VendorWarrantyEnd" HeaderStyle-HorizontalAlign="Center" HeaderText="Vendor Warranty End"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="WarrantyEnd" HeaderStyle-HorizontalAlign="Center" HeaderText="Warranty End"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="AccountNo" HeaderStyle-HorizontalAlign="Center" HeaderText="Account No"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="Category" HeaderStyle-HorizontalAlign="Center" HeaderText="Category"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="Company" HeaderStyle-HorizontalAlign="Center" HeaderText="Company"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="DeviceId" HeaderStyle-HorizontalAlign="Center" HeaderText="Device Id"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="DeviceType" FooterStyle-Width="200px" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="200px" HeaderText=" Device Type" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="200px" Visible="true" />
                                    <asp:BoundField DataField="ManufacturerDetails" HeaderStyle-HorizontalAlign="Center" HeaderText="Manufacturer Details"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="Model" HeaderStyle-HorizontalAlign="Center" HeaderText="Model"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="Source" HeaderStyle-HorizontalAlign="Center" HeaderText="Source"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="StateId" HeaderStyle-HorizontalAlign="Center" HeaderText="Device State"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="POId" HeaderStyle-HorizontalAlign="Center" HeaderText="PO Id"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="DeliveryStatus" HeaderStyle-HorizontalAlign="Center" HeaderText="Delivery Status"
                                        ItemStyle-HorizontalAlign="Left" Visible="false" />
                                </Columns>
                                <PagerSettings Mode="Numeric" />
                            </asp:GridView>
                        </div>
                        <div id="DivFooterRow" style="overflow: hidden">
                        </div>
                    </div>
                    <!--TEst Grid-->
                    <asp:Button runat="server" ID="Button1" Text="Customer Details" CssClass="button"
                        UseSubmitBehavior="false" Visible="false" Width="105px" />
                </asp:Panel>
            </div>
            <div id="imgrefresh2" class="loader transparent">
                <%--  <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:ImageButton ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()">
                </asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <%-- <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="btnGenerateExcel" />
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
