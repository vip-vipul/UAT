<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptAddPlanTransDET_JV.aspx.cs" Inherits="PrjUpassPl.Reports.rptAddPlanTransDET_JV" %>
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
                            <th align="left">
                                <asp:Label ID="Label1" ForeColor="Black" Font-Bold="true" Font-Size="9pt" Font-Names="Tahoma"
                                    runat="server" Text="LCO Name :"></asp:Label>&nbsp;
                                <asp:Label ID="lbllconm" ForeColor="Black" Font-Bold="false" Font-Size="9pt" Font-Names="Tahoma"
                                    runat="server"></asp:Label>
                            </th>
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
                                    UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" Width="100"/>
                                &nbsp; &nbsp;
                                <asp:Button runat="server" ID="btn_genExcel" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" align="left" OnClick="btn_genExcel_Click" Width="100"/>
                            </td>
                            <td>
                                <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                                <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div id="DivRoot" runat="server" align="left" style="width: 100%;display:none">
                        <div style=" overflow: hidden; width: 1184px; height: 59.5px; position: relative; top: 7px; z-index: 10; vertical-align: top;width:100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll;width:100%" onscroll="OnScrollDiv(this)" id="DivMainContent" >

                        <asp:GridView runat="server" ID="grdTransDet" CssClass="Grid"  AutoGenerateColumns="false"
                            ShowFooter="true" AllowPaging="true" PageSize="500" OnSorting="grdTransDet_Sorting"
                            OnPageIndexChanging="grdTransDet_PageIndexChanging">
                            <%-- OnRowDataBound="grdTransDet_RowDataBound" --%>
                            <FooterStyle CssClass="GridFooter" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Customer ID" DataField="custid" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <%-- FooterText="Total" --%>
                                <asp:BoundField HeaderText="Customer Name" DataField="custname" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Customer Address" DataField="custaddr" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="VC ID/MAC ID" DataField="vc" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Plan Name" DataField="plnname" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Plan Type" DataField="plntyp" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Transaction Type" DataField="flag" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Reason" DataField="reason" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="User ID" DataField="insby" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="User Name" DataField="userowner" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Transaction Date & Time" DataField="tdt" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="MRP" DataField="custprice" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="LCO MRP" DataField="LCOMRP" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="LCO DISCOUNT" DataField="LCODISCOUNT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="NET LCO PRICE" DataField="amtdd" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    
                                        
                                <asp:BoundField HeaderText="Expiry Date" DataField="expdt" HeaderStyle-HorizontalAlign="Center"
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
                                    <asp:BoundField HeaderText="DAS Area" DataField="AREA" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                     <asp:BoundField HeaderText="OBRM Status" DataField="OBRMSTATUS" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                     <asp:BoundField HeaderText="Source" DataField="SFLAG" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="LCO Share" DataField="LSHARE" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                     <asp:BoundField HeaderText="LCO Share Type" DataField="LSHARETYPE" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                     <asp:BoundField HeaderText="Discount" DataField="Discount" HeaderStyle-HorizontalAlign="Center"
                                      ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Broadcaster Price" DataField="broad_price" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="JV Price" DataField="jv_price" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="JV Balance" DataField="jv_balance" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />

                            </Columns>
                        </asp:GridView>
                                           </div>
                        <div id="DivFooterRow" style="overflow: hidden">
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
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="btn_genExl" />
            <asp:PostBackTrigger ControlID="btn_genExcel" />
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
