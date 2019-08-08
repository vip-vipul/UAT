<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="RptCustEcsDetails.aspx.cs" Inherits="PrjUpassPl.Reports.RptCustEcsDet" %>

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
        
        .cal_image_holder
        {
            width: 7%;
        }
        .Grid th a
        {
            color: #ffffff;
            cursor: pointer;
        }
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
         
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="maindive">
                <div style="float: Center">
                    <asp:Panel runat="server" ID="pnlSearch">
                        <asp:HiddenField ID="hdnslctcolumns" runat="server" Value='account_id"Account No", VC_ID"VC_ID", customername"Customer Name", cust_address"Cust Address", lconame"Lco Name", lcocode"Lco Code", Package"Package", PlanType"Plan Type",enddate "End Date",is_active "AutoRenewal Status"' />
                        <div class="delInfo1">
                            <button onclick="goBack()" style="margin-right: -1005px; margin-top: -15px;" class="button">
                                Back</button>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                                Width="110px" OnClick="btnGenerateExcel_Click" Visible="true" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnGenerateExcel" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                 <div id="DivRoot" runat="server" align="left" style="width: 100%;display:none">
                        <div style="overflow: hidden;width:100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll;width:100%" onscroll="OnScrollDiv(this)" id="DivMainContent" 

                                        <asp:GridView ID="grd" runat="server" CssClass="Grid" ShowFooter="true" Width="100%"
                                            AllowPaging="true" PageSize="100" OnPageIndexChanging="grd_PageIndexChanging"
                                            OnSorting="grd_Sorting">
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                     </div>
                        <div id="DivFooterRow" style="overflow: hidden">
                        </div>
                    </div>

                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div id="imgrefresh2" class="loader transparent">
                    <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                    <asp:ImageButton ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                        AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()">
                    </asp:ImageButton>
                </div>
        </ContentTemplate>
        <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:PostBackTrigger ControlID="btn_genExl" />
        </Triggers>--%>
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
