<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptBulkActFileProcess.aspx.cs" Inherits="PrjUpassPl.Reports.rptBulkActFileProcess" %>

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
                    <div class="tblSearchItm" style="width: 40%;">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    LCO Name:
                                    <asp:DropDownList ID="ddlLco" runat="server" Height="19px" Style="resize: none;"
                                        Width="304px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    From Date :
                                    <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>
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
                                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:RadioButtonList ID="RadSearchby" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="E" Selected="True">Enable</asp:ListItem>
                                        <asp:ListItem Value="D">Disable</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="padding-left: 50px" align="center">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
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
                    <div id="DivRoot" runat="server" align="left" style="width: 100%; display: none">
                        <div style="overflow: hidden; width: 100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll; width: 100%" onscroll="OnScrollDiv(this)" id="DivMainContent">
                            <asp:GridView ID="grdBulkProc" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                                ShowFooter="true" Width="100%" AllowSorting="true" AllowPaging="true" PageSize="20"
                                OnPageIndexChanging="grdBulkProc_PageIndexChanging" OnRowCommand="grdBulkProc_RowCommand" >
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Unique ID" DataField="dtttime" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" FooterText="" />--%>
                                    <asp:BoundField HeaderText="Unique ID" DataField="uploadid" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="File Name" DataField="filename" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="Date Time" DataField="insdt" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <%--  <asp:BoundField HeaderText="Count" DataField="Total" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />--%>
                                    <asp:TemplateField HeaderText="Count" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Total")%>'
                                                CommandName="Count"></asp:LinkButton>
                                            <asp:Label ID="lbluid1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"uploadid")%>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblprocessdt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"processdate")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Procees Date" DataField="processdate" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="File Status" DataField="status" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="Deleted By" DataField="delby" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="Deleted Date" DataField="deldate" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MMM/yyyy}" 
                                        ItemStyle-HorizontalAlign="left" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="grdBulkProc" />
                            </Triggers>
                            <ContentTemplate>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="DivFooterRow" style="overflow: hidden">
                        </div>
                    </div>
                </div>
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
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="" EventName="Click" />--%>
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
