
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptINVPrePartyLedger.aspx.cs" Inherits="PrjUpassPl.Reports.rptINVPrePartyLedger" %>

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
           return false;
       }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                    <div style="float: right">
                        <button onclick="return goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                            Back</button>
                    </div>
                    <div class="tblSearchItm" style="width: 30%;">
                        <table width="100%">
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    LCO :
                                    <asp:DropDownList ID="ddlLco" AutoPostBack="true" runat="server" Height="19px" Style="resize: none;"
                                        Width="304px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
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
                    <table>
                        <tr>
                            <td style="width: 100px" align="left">
                                <asp:Button runat="server" ID="btn_genExl" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Button runat="server" Width="60px" ID="btnAll" Text="All" CssClass="button"
                                    OnClick="btnAll_Click" Visible="false" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div class="griddiv">
                                    <asp:GridView runat="server" ID="grdLcoPartyLedger" CssClass="Grid" AutoGenerateColumns="false"
                                        ShowFooter="true" OnRowCommand="grdLcoPartyLedger_RowCommand" AllowPaging="true"
                                        PageSize="100" OnRowDataBound="grdLcoPartyLedger_RowDataBound" OnSorting="grdLcoPartyLedger_Sorting"
                                        OnPageIndexChanging="grdLcoPartyLedger_PageIndexChanging">
                                        <FooterStyle CssClass="GridFooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:HyperLinkField DataTextField="lconame" DataNavigateUrlFields="lconame" SortExpression="lconame"
                                ControlStyle-Width="95pt" HeaderStyle-HorizontalAlign="Center" Target="_blank"
                                  ItemStyle-HorizontalAlign="left" DataNavigateUrlFormatString="rptAddPlanTransUSER.aspx?lcoid={0}&amp;lconame={1}"
                                HeaderText="LCO Name" FooterText="Total" />--%>
                                            <asp:TemplateField HeaderText="LCO Name" ItemStyle-HorizontalAlign="Left" FooterText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"lconame")%>'
                                                        CommandName="LcoName1"></asp:LinkButton>
                                                    <asp:Label ID="lblOperid1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"lcoid")%>'
                                                        Visible="false"></asp:Label><asp:Label ID="lblolconame" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"lconame")%>'
                                                            Visible="false"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="lcocode" HeaderText="LCO Code" SortExpression="lcocode"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="65pt" />
                                            <asp:BoundField DataField="crlimit" HeaderText="Balance" SortExpression="crlimit"
                                                HeaderStyle-HorizontalAlign="Center" Visible="false" ControlStyle-Width="35pt"
                                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Opening Balance" DataField="openinbal" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Debit" DataField="drlimit" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Credit" DataField="crlimit" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Closing Balance" DataField="closingbal" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
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
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <%-- <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />
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
