﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="rptNotificationSent.aspx.cs" Inherits="PrjUpassPl.Reports.rptNotificationSent" %>

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
                    <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                        Back</button>
                </div>
                <asp:Panel runat="server" ID="pnlSearch">
                    <asp:HiddenField ID="hdnslctcolumns" runat="server" Value='"Transaction Id", "Reference Id", "Account No", "VC/MAC Id", "STB", "Customer No", "Mobile No", "Lco Code", "Lco Name", "Type", "TEMPLATE", "MESSAGE", "ACTION", "Insert By","Insert Date", "FLAG"' />
                    <div class="delInfo1">
                        <table runat="server" align="center" width="100%" id="tbl1" border="0">
                            
                               <tr>
                                            <td align="left" class="cal_image_holder">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; LCO Name&nbsp; :
                                                <asp:DropDownList ID="ddlLco" runat="server"  Height="19px" 
                                                    Style="resize: none;" Width="304px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                            
                            
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;From Date :
                                    <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>
                                    <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;&nbsp;&nbsp; To Date :
                                    <asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>
                                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Type :
                                    <asp:DropDownList ID="ddlType" runat="server" Height="19px" Style="resize: none;"
                                        Width="150px">
                                        <asp:ListItem Text="Select All" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="BMAIL" Value="BMAIL"></asp:ListItem>
                                        <asp:ListItem Text="SMS" Value="SMS"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table style="width: 78%">
                                        <tr>
                                            <td align="right">
                                                Search By:
                                            </td>
                                            <td style="width:29%">
                                                <asp:RadioButtonList ID="RadSearchby" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                                    onclick="Showexcel()" OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Account No.</asp:ListItem>
                                                    <asp:ListItem Value="1">VC Id</asp:ListItem>
                                                    <asp:ListItem Value="2">LCO Code</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtsearchpara" runat="server"  Width="90px"
                                                    Height="15px" onkeydown="SetContextKey()"></asp:TextBox>
                                                     <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                    CssClass="button" OnClientClick="Showexcel()" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
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
                    <table>
                        <tr>
                            <td align="left">
                                <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                    Width="110px" OnClientClick="hideexcel()" UseSubmitBehavior="false" OnClick="btnGenerateExcel_Click"
                                    Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div class="griddiv">
                                    <asp:GridView ID="grd" runat="server" CssClass="Grid" ShowFooter="true" Width="100%"
                                        AllowPaging="true" PageSize="100" OnPageIndexChanging="grd_PageIndexChanging">
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="imgrefresh2" class="loader transparent">
                <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
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
