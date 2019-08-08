<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptObrmCustMast.aspx.cs" Inherits="PrjUpassPl.Reports.rptObrmCustMast" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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


        // --Gridview HEader TEsting

        function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
            var tbl = document.getElementById(gridId);
            if (tbl) {
                var DivHR = document.getElementById('DivHeaderRow');
                var DivMC = document.getElementById('DivMainContent');
                var DivFR = document.getElementById('DivFooterRow');

                //*** Set divheaderRow Properties ****
                DivHR.style.height = (parseFloat(headerHeight) + 4.5) + 'px';
                DivHR.style.width = (parseInt(width) - 16) + 'px';
                DivHR.style.position = 'relative';
                DivHR.style.top = '0px';
                DivHR.style.zIndex = '10';
                DivHR.style.verticalAlign = 'top';

                //*** Set divMainContent Properties ****
                DivMC.style.width = width + 'px';
                DivMC.style.height = height + 'px';
                DivMC.style.position = 'relative';
                DivMC.style.top = -headerHeight + 'px';
                DivMC.style.zIndex = '1';

                //*** Set divFooterRow Properties ****
                DivFR.style.width = (parseInt(width) - 16) + 'px';
                DivFR.style.position = 'relative';
                DivFR.style.top = -headerHeight + 'px';
                DivFR.style.verticalAlign = 'top';
                DivFR.style.paddingtop = '2px';

                if (isFooter) {
                    var tblfr = tbl.cloneNode(true);
                    tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
                    var tblBody = document.createElement('tbody');
                    tblfr.style.width = '100%';
                    tblfr.cellSpacing = "0";
                    tblfr.border = "0px";
                    tblfr.rules = "none";
                    //*****In the case of Footer Row *******
                    tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
                    tblfr.appendChild(tblBody);
                    DivFR.appendChild(tblfr);
                }
                //****Copy Header in divHeaderRow****
                DivHR.appendChild(tbl.cloneNode(true));
            }
        }



        function OnScrollDiv(Scrollablediv) {
            document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
            document.getElementById('DivFooterRow').scrollLeft = Scrollablediv.scrollLeft;
        }

        //


    </script>
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
                                    <asp:Label ID="lblUser" runat="server" Text="Search LCO By"></asp:Label>
                                    <asp:Label ID="Label37" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    <asp:Label ID="Label44" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="RadSearchby" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                        onclick="Showexcel()" OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Selected="True">Account No.</asp:ListItem>
                                        <asp:ListItem Value="1">VC Id</asp:ListItem>
                                        <asp:ListItem Value="2">LCO Code</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsearchpara" runat="server" Style="resize: none;" Width="150px" MaxLength="50"
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
                                        OnClientClick="hideexcel()" UseSubmitBehavior="false" OnClick="btnGenerateExcel_Click"
                                        Visible="false" Width="95px" />
                                    <asp:Button runat="server" ID="btnCustDetail" Text="Customer Details" CssClass="button"
                                        UseSubmitBehavior="false" Visible="false" OnClick="btnCustDetail_Click" />
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
                                CssClass="Grid" OnPageIndexChanging="grdtransdet_PageIndexChanging" PageSize="100"
                                ShowFooter="true">
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="account_no" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                                        HeaderText="Account Number" ItemStyle-HorizontalAlign="Right" Visible="true" />
                                    <%--<asp:TemplateField HeaderText="VC Id" ItemStyle-HorizontalAlign="Left" FooterText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBvc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"vc")%>'
                                                CommandName="vcid"></asp:LinkButton>
                                            <asp:HiddenField ID="hdnfullname" runat="server" Value='<%# Eval("fullname")%>' />
                                            <asp:HiddenField ID="HdnAddress" runat="server" Value='<%# Eval("address")%>' />
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="vc" HeaderStyle-HorizontalAlign="Center" HeaderText="VC Id"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="mac" HeaderStyle-HorizontalAlign="Center" HeaderText="MAC"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="stb" HeaderStyle-HorizontalAlign="Center" HeaderText="STB"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="first_name" HeaderStyle-HorizontalAlign="Center" HeaderText="First Name"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="middle_name" HeaderStyle-HorizontalAlign="Center" HeaderText="Middle Name"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="last_name" HeaderStyle-HorizontalAlign="Center" HeaderText="Last Name"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="address" FooterStyle-Width="200px" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="200px" HeaderText=" Address" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="200px" Visible="true" />
                                    <asp:BoundField DataField="lco_name" HeaderStyle-HorizontalAlign="Center" HeaderText="LCO Name"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="lco_code" HeaderStyle-HorizontalAlign="Center" HeaderText="LCO Code"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="mobile" HeaderStyle-HorizontalAlign="Center" HeaderText="Mobile"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="planname" HeaderStyle-HorizontalAlign="Center" HeaderText="Package"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="city" HeaderStyle-HorizontalAlign="Center" HeaderText="City"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="state" HeaderStyle-HorizontalAlign="Center" HeaderText="State"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="zip" HeaderStyle-HorizontalAlign="Center" HeaderText="Zip"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="customer_type" HeaderStyle-HorizontalAlign="Center" HeaderText="Customer Type"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="cust_category" HeaderStyle-HorizontalAlign="Center" HeaderText="Customer Category"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="agreement_no" HeaderStyle-HorizontalAlign="Center" HeaderText="Agreement No"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="productname" HeaderStyle-HorizontalAlign="Center" HeaderText="Product Name"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="connection_type" HeaderStyle-HorizontalAlign="Center"
                                        HeaderText="Connection Type" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="homephone" HeaderStyle-HorizontalAlign="Center" HeaderText="Home Phone"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="workphone" HeaderStyle-HorizontalAlign="Center" HeaderText="Work Phone"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <%-- <asp:BoundField HeaderText="Plan Type" DataField="plantype" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" />--%>
                                    <asp:BoundField DataField="startdate" HeaderStyle-HorizontalAlign="Center" HeaderText="Start Date"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="enddate" HeaderStyle-HorizontalAlign="Center" HeaderText="End Date"
                                        ItemStyle-HorizontalAlign="Left" Visible="true" />
                                    <asp:BoundField DataField="custprice" HeaderStyle-HorizontalAlign="Center" HeaderText="MRP"
                                        ItemStyle-HorizontalAlign="Right" Visible="true" />
                                    <asp:BoundField DataField="renewflagstatus" HeaderStyle-HorizontalAlign="Center"
                                        HeaderText="Renewal Status" ItemStyle-HorizontalAlign="Left" Visible="true" />
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
            <asp:PostBackTrigger ControlID="btnCustDetail" />
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
