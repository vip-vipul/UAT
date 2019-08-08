<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptserviceActDact.aspx.cs" Inherits="PrjUpassPl.Reports.rptserviceActDact" %>

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                <div style="float:right">
                <button onclick="return goBack();"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <div class="tblSearchItm" style="width: 30%;">
                        <table width="100%">
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    From Date :
                                    <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>
                                    <%--</td>
                    <td class="cal_image_holder" align="left">--%>
                                    <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;&nbsp; To Date :
                                    <asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>
                                    <%--</td>
                    <td class="cal_image_holder" align="left">--%>
                                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;&nbsp; Status :
                                    <asp:DropDownList ID="Ddlstatus" runat="server" Width="130px">
                                        <asp:ListItem Selected="True" Text="All" Value="ALL"></asp:ListItem>
                                        <asp:ListItem Text="Activated" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Deactivated" Value="D"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="padding-left: 60px" align="center">
                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button" UseSubmitBehavior="false"
                                        OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                                &nbsp;
                                <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="left" class="style67">
                                <asp:Button runat="server" ID="btngrnExel" Text="Generate Excel" CssClass="button"
                                    Visible="false" UseSubmitBehavior="false" align="left" OnClick="btngrnExel_Click" />
                            </td>
                            <td class="style68">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td align="left">
                            <div id="DivRoot" runat="server" align="left" style="width: 100%;display:none">
                        <div style="overflow: hidden;width:100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll;width:100%" onscroll="OnScrollDiv(this)" id="DivMainContent" 

                                <asp:GridView runat="server" ID="grdactdact" CssClass="Grid" AutoGenerateColumns="false"
                                    ShowFooter="true" AllowPaging="true" PageSize="100" OnPageIndexChanging="grdactdact_PageIndexChanging">
                                    <FooterStyle CssClass="GridFooter" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="STBNO" HeaderText="STB Number" SortExpression="lcocode"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="65pt" />
                                        <asp:BoundField DataField="CUSTNO" HeaderText="Customer Code" SortExpression="lcocode"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="65pt" />
                                        <asp:BoundField DataField="ADDR" HeaderText="Address" SortExpression="crlimit" HeaderStyle-HorizontalAlign="Center"
                                            Visible="false" ControlStyle-Width="35pt" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <%--<asp:BoundField HeaderText="Account Poid" DataField="ACCPOID" HeaderStyle-HorizontalAlign="Center"
                                Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Service Poid" DataField="SVCPOID" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />--%>
                                        <asp:BoundField HeaderText="VC Id" DataField="VCID" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Status" DataField="STATUS" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Reason" DataField="reason" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Transaction By" DataField="TRANSBY" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Transaction Date" DataField="TRANSDT" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Order Id" DataField="ORDERID" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="LCO Code" DataField="lcode" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="LCO Name" DataField="lnaame" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Company Name" DataField="companyname" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Distributor" DataField="distributor" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="left" />
                                        <asp:BoundField HeaderText="Sub Distributor" DataField="subdistributor" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="State" DataField="statename" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="City" DataField="cityname" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                                 </div>
                        <div id="DivFooterRow" style="overflow: hidden">
                        </div>
                    </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />

            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:PostBackTrigger ControlID="btngrnExel" />
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
