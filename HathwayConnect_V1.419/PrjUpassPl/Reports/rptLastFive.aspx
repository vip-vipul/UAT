<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptLastFive.aspx.cs" Inherits="PrjUpassPl.Reports.rptLastFive" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .topHead
        {
            background: #E5E5E5;
            width: 96.5%;
        }
        .topHead table td
        {
            font-size: 12px;
            font-weight: bold;
        }
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
        }
        
        .tabody
        {
            padding: 10px;
            border: 1px solid #094791;
            background: #ffffff;
            width: 96.5%;
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
        .popBack
        {
            background: white; /* IE 8 */
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=50)"; /* IE 5-7 */
            filter: alpha(opacity=50); /* Netscape */
            -moz-opacity: 0.5; /* Safari 1.x */
            -khtml-opacity: 0.5; /* Good browsers */
            opacity: 0.5;
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
        .ui-autocomplete.ui-widget
        {
            font-family: Verdana,Arial,sans-serif;
            font-size: 10px;
        }
        
        .tabs
        {
            width: 100%;
            display: inline-block;
        }
        
        /*----- Tab Links -----*/
        /* Clearfix */
        .tab-links:after
        {
            display: block;
            clear: both;
            content: '';
        }
        
        .tab-links li
        {
            margin: 0px 5px;
            float: left;
            list-style: none;
        }
        
        .tab-links a
        {
            padding: 9px 15px;
            display: inline-block;
            border-radius: 3px 3px 0px 0px;
            background: red;
            font-size: 16px;
            font-weight: 600;
            color: #4c4c4c;
            transition: all linear 0.15s;
        }
        
        .tab-links a:hover
        {
            background: #a7cce5;
            text-decoration: none;
        }
        
        li.active a, li.active a:hover
        {
            background: #fff;
            color: #4c4c4c;
        }
        
        /*----- Content of Tabs -----*/
        .tab-content
        {
            padding: 15px;
            border-radius: 3px;
            box-shadow: -1px 1px 1px rgba(0,0,0,0.15);
            background: #fff;
        }
        
        .tab
        {
            display: none;
        }
        
        .tab.active
        {
            display: block;
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
            <asp:Panel runat="server" ID="pnlTransItemDet">
                <div class="maindive">
                <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <table width="100%" align="center" id="tbldet" runat="server">
                        <tr>
                            <th align="center">
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label1" ForeColor="Black" Font-Bold="True" Font-Size="9pt" Font-Names="Tahoma"
                                                runat="server" Text="LCO Name"></asp:Label>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;
                                            <asp:Label ID="Label3" ForeColor="Black" Font-Bold="True" Font-Size="9pt" Font-Names="Tahoma"
                                                runat="server" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlLco" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </th>
                        </tr>
                      <%--  <tr>
                            <th align="center">
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label2" ForeColor="Black" Font-Bold="true" Font-Size="9pt" Font-Names="Tahoma"
                                                runat="server" Text="User Name"></asp:Label>
                                        </td>
                                        <td align="left">
                                            &nbsp;
                                            <asp:Label ID="Label4" ForeColor="Black" Font-Bold="True" Font-Size="9pt" Font-Names="Tahoma"
                                                runat="server" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlUser" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </th>
                        </tr>--%>
                        <tr>
                            <th align="center">
                                <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button" UseSubmitBehavior="false"
                                    OnClick="btnSubmit_Click" />
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
                    <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                    <table align="left">
                        <tr>
                            <td align="left">
                                <asp:Button runat="server" ID="btn_genExl" Text="Generate Excel" CssClass="button" style="width:120px;"
                                    UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="griddiv">
                                    <asp:GridView runat="server" ID="grdTransDet" CssClass="Grid" AutoGenerateColumns="false"
                                        ShowFooter="true" OnRowDataBound="grdTransDet_RowDataBound" OnSorting="grdTransDet_Sorting">
                                        <FooterStyle CssClass="GridFooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Customer ID" DataField="custid" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" FooterText="" />
                                            <asp:BoundField HeaderText="VC" DataField="vc" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Plan Name" DataField="plnname" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Plan Type" DataField="plntyp" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Transaction Type" DataField="flag" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Reason" DataField="reason" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="User ID" DataField="uname" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="User Name" DataField="userowner" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Transaction Date & Time" DataField="tdt" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="MRP" DataField="custprice" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Amount Deducted" DataField="amtdd" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Expiry date" DataField="expdt" HeaderStyle-HorizontalAlign="Center"
                                                ControlStyle-Width="75pt" ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Pay Term" DataField="payterm" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Balance" DataField="bal" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="LCO Name" DataField="lconame" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="JV Name" DataField="jvname" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="ERP LCO A/C" DataField="erplco_ac" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Distributor" DataField="distname" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Sub Distributor" DataField="subdist" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="City" DataField="city" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="State" DataField="state" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
               <%-- <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                     <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
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
