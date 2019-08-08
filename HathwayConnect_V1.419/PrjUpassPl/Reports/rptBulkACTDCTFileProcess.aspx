<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptBulkACTDCTFileProcess.aspx.cs" Inherits="PrjUpassPl.Reports.rptBulkACTDCTFileProcess" %>
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
                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button" 
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
                            <td align="left" class="style67">
                                <asp:Button runat="server" ID="btngrnExel" Text="Generate Excel" CssClass="button"
                                    Visible="false" UseSubmitBehavior="false" align="left" 
                                    OnClick="btngrnExel_Click" Width="116px" />
                            </td>
                            <td class="style68">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <div id="DivRoot" runat="server" align="left" style="width: 100%; display: none">
                        <div style="overflow: hidden; width: 100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll; width: 100%" onscroll="OnScrollDiv(this)" id="DivMainContent">
                            <asp:GridView ID="grdBulkProc" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                                ShowFooter="true" Width="100%" AllowSorting="true" AllowPaging="true" PageSize="20"
                                OnPageIndexChanging="grdBulkProc_PageIndexChanging" >
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Unique ID" DataField="dtttime" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" FooterText="" />--%>
                                    <asp:BoundField HeaderText="LCO Code" DataField="LCOCode" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="Account No." DataField="ACC_NO" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="VC ID" DataField="vc_id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="Inserted By" DataField="Insby" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                        <asp:BoundField HeaderText="Inserted Date" DataField="InsDate" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="Action" DataField="Action" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="File Status" DataField="Processflag" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                        <asp:BoundField HeaderText="OBRM Status" DataField="OBRMStatus" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="OBRM Date" DataField="OBRMDate" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="Reason" DataField="Reason" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />

                                </Columns>
                            </asp:GridView>
                        </div>
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
           <asp:PostBackTrigger ControlID="btngrnExel"/>
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
