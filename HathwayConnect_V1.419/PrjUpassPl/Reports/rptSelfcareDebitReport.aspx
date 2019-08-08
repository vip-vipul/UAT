﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptSelfcareDebitReport.aspx.cs" Inherits="PrjUpassPl.Reports.rptSelfcareDebitReport" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     function customOpen(url) {
         var w = window.open(url, '', 'width=1000,height=600,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
         w.focus();

     }
    </script>
    <style type="text/css">
        /*.GridFooter
        {
            border: 1px solid #094791;
            border-radius: 0px;
            color: White;
            background: #094791;
            width: 100px;
        }*/
        .ajax__calendar_container
        {
        	z-index:100;
        	}
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


         <div class="maindive">
                <div style="float: right">
                    <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                        Back</button>
                </div>
                <asp:Panel runat="server" ID="pnlSearch">
                    <div class="tblSearchItm" style="width: 30%;">
                        <table width="100%">
                           <%-- <tr>
                                <td align="center" class="cal_image_holder">
                                    LCO :
                                    <asp:DropDownList ID="ddlLco" AutoPostBack="true" runat="server" Height="19px" Style="resize: none;"
                                        Width="304px">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
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
                    <table width="100%">
                        <tr>
                            <td align="left" class="style67" style="margin-left: 1%;">
                                <asp:Button runat="server" ID="btngrnExel" Text="Generate CSV" CssClass="button"
                                    UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" Width="100" />
                                &nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" align="left" OnClick="btnGenerateExcel_Click" Width="100" />
                            </td>
                            <td class="style68">
                                <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                                <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <div id="DivRoot" runat="server" align="left" style="width: 100%; display: none">
                        
                        <div style="overflow: scroll; width: 100%" onscroll="OnScrollDiv(this)" id="DivMainContent">
                            <asp:GridView ID="grdselfcaredebit" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                                ShowFooter="True" Width="100%" AllowSorting="True" AllowPaging="True" PageSize="100"
                                OnRowDataBound="grdAddPlantopup_RowDataBound" OnSorting="grdAddPlantopup_Sorting"
                                OnPageIndexChanging="grdAddPlantopup_PageIndexChanging" HeaderStyle-Width="150"
                                EnableModelValidation="True">
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Transaction Id" DataField="num_custpay_transid" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" FooterText="">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Receipt No" DataField="var_custpay_receiptno" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Customer Id" DataField="var_custpay_custid" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField HeaderText="VC Id" DataField="var_custpay_vcid" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField HeaderText="Amount" DataField="amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Inserted By" DataField="var_custpay_insby" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Transaction Date" DataField="dat_custpay_transdt" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="User" DataField="var_custpay_user" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Flag" DataField="var_custpay_page_flag" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left">
                                        <ControlStyle Width="30px" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="DAS Area" DataField="var_custpay_das_area" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField HeaderText="Child Id" DataField="num_custpay_lco_shares" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField HeaderText="Deduct Type" DataField="var_custpay_deducttype" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    
                                <asp:BoundField HeaderText="LCO Code" DataField="var_lcomst_code" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="LCO Name" DataField="var_lcomst_name" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Collection Amount" DataField="collection_amount" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="LCO GSTN" DataField="lco_gstn" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="JV GSTN" DataField="jv_gstn" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                    
                                   
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="DivFooterRow" style="overflow: hidden">
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="imgrefresh2" class="loader transparent">
                <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:ImageButton ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()">
                </asp:ImageButton>
            </div>
</asp:Content>