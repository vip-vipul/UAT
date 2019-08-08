<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptBulkEcafReport.aspx.cs" Inherits="PrjUpassPl.Reports.rptBulkEcafReport" %>
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
           return false;
       }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div class="maindive">
        <div style="float:right">
                <button onclick="return goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
            <asp:Panel runat="server" ID="pnlTransItemDet">
                
                    <table width="100%" style="margin-left: 1%;">
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
                            <td colspan="2" align="center">
                                <asp:Button runat="server" ID="btnSubmit" Text="Search" CssClass="button"
                                    UseSubmitBehavior="false" align="left" OnClick="btnSubmit_Click" Width="100"/>
                            </td>
                            </tr>
                    </table>
                    <table width="100%" style="margin-left: 1%;">
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" style="margin-left: 1%;">
                        <tr>
                            <td style="width: 250px">
                                <asp:Button runat="server" ID="btn_genExl" Text="Generate CSV" CssClass="button"
                                    UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" Width="100" Visible="false" />
                                &nbsp; &nbsp;
                                <asp:Button runat="server" ID="btn_genExcel" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" align="left" OnClick="btn_genExcel_Click" Width="100" Visible="false" />
                            </td>
                            <td>
                                <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                                <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div id="DivRoot" runat="server" align="center" style="width: 100%;">
                        
                            <div class="griddiv">
        <asp:GridView runat="server" ID="grdBulkUpload" CssClass="Grid" AutoGenerateColumns="false"
            Visible="false" OnRowCommand="grdBulkUpload_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                        <asp:HiddenField ID="hdnUploadId" runat="server" Value='<%# Eval("uploadid")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Upload Id" DataField="uploadid" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left" FooterText="" />
                <asp:BoundField HeaderText="Status" DataField="status" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left" FooterText="" />
                <asp:TemplateField HeaderText="Total Transaction" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbnTotal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"total")%>'
                            CommandName="total"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Success" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbnSuccess" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"success")%>'
                            CommandName="success"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Failed" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbnFail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"failed")%>'
                            CommandName="fail"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:BoundField HeaderText="Insert Date" DataField="dat_lcopre_bulk_insdt" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left" FooterText="" />
            </Columns>
        </asp:GridView>
        </div>
                       </div>
                        
               
                
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                    <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." 
                    OnClientClick="onComplete()">
                         </asp:ImageButton>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
