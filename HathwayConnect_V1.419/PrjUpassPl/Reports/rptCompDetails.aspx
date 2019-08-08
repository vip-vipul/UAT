<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptCompDetails.aspx.cs" Inherits="PrjUpassPl.Reports.rptCompDetails" %>

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
                   <asp:HiddenField ID="hdnslctcolumns" runat="server" Value='"LCO Code", "LCO Name", "JV Name", "ERP LCO A/C", "Distributor", "Sub distributor", 
                                                                        "State", "City", "Company Name", "Account No", "First Name", "Middle Name", 
                                                                        "Last Name", EMAIL, MOBILE, ADDRESS, "Old First Name", "Old Middle Name", 
                                                                        "Old Last Name", "Old Email", "Old Mobile", "Old Address", "Term Accepted", 
                                                                        "Insert By", "Insert Date"'/>
                    <div class="delInfo1">
                        <table runat="server" align="center" width="100%" id="tbl1" border="0">
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
                                <td align="center">
                                    <table style="width: 78%">
                                        <tr>
                                            <td align="center">
                                                Account No.
                                                 <asp:TextBox ID="txtsearchpara" runat="server"  Width="100px"
                                                    Height="15px" ></asp:TextBox>
                                                     <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                    CssClass="button"  />
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
                                    Width="110px" UseSubmitBehavior="false" OnClick="btnExport_Click"
                                    Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div class="griddiv">
                                  <asp:GridView ID="GridCompList" runat="server" AlternatingRowStyle-CssClass="GrdAltRow"
                            AutoGenerateColumns="false" CssClass="Grid" Width="98%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false"
                                    ItemStyle-Width="5px" ControlStyle-Font-Bold="false">
                                    <HeaderStyle Wrap="false" />
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ControlStyle Font-Bold="False" />
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="cmpno" HeaderText="Comp. No." ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left"
                                    ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="custnm" HeaderText="Name" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left"
                                    ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="custno" HeaderText="Mobile No" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="right" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="cmpdesc" HeaderText="Description" HeaderStyle-Width="240px"
                                    ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="cmptype" HeaderText="Comp. Type" ItemStyle-Width="150px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="cmpsubtype" HeaderText="Sub Type" ItemStyle-Width="9px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="cmpstatus" HeaderText="Status" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="srvst" HeaderText="Service Status" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="regdt" HeaderText="Comp. Date" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="assgnuser" HeaderText="Assign User" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="userremark" HeaderText="User Remark" ItemStyle-Width="110px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="remarkdate" HeaderText="Remark Date" DataFormatString="{0:dd-MM-yyyy}"
                                    ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="source" HeaderText="Source" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                    
                                       <asp:BoundField DataField="Flag" HeaderText="Flag" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                       <asp:BoundField DataField="lcocode" HeaderText="LCO Code" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                       <asp:BoundField DataField="companyname" HeaderText="Company Name" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="callername" HeaderText="Caller Name" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="callerno" HeaderText="Caller No" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="alternateno" HeaderText="Alternate No" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                            </Columns>
                        </asp:GridView>
                                  <%--  <asp:GridView ID="grd" runat="server" CssClass="Grid" ShowFooter="true" Width="100%"
                                        AllowPaging="true" PageSize="100" OnPageIndexChanging="grd_PageIndexChanging">
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>--%>
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
