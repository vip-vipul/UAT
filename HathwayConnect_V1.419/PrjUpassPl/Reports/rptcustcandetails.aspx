<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptcustcandetails.aspx.cs" Inherits="PrjUpassPl.Reports.rptcustcandetails" %>
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
    <div class="maindive">
     <asp:Panel runat="server" ID="pnlTransItemDet">
                    <div  style="width: 100%;">
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
                                <td style="padding-left: 60px" align="center">
                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button" 
                                        onclick="btnSubmit_Click"/>
                                        &nbsp;
                                         </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>                           
                            </table>
                        <table width="100%">
                            <tr>
                            <td>
                                   <asp:Button runat="server" ID="btn_genExcel" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" align="left" onclick="btn_genExcel_Click" /></td>
                            </tr>
                            <tr>
                            <td align="left">
                            <div class="griddiv">
                        <asp:GridView runat="server" ID="grdTransDet" CssClass="Grid"  AutoGenerateColumns="false"
                            ShowFooter="true" AllowPaging="true" PageSize="100"
                            OnPageIndexChanging="grdTransDet_PageIndexChanging">                          
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
                                <asp:BoundField HeaderText="VC ID" DataField="vc" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Plan Name" DataField="plnname" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Plan Type" DataField="plntyp" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Transaction Type" DataField="flag" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Reason" DataField="reason" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="User ID" DataField="uname" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="User Name" DataField="userowner" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Transaction Date & Time" DataField="tdt" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="MRP" DataField="custprice" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="left" />
                                <asp:BoundField HeaderText="Amount Deducted" DataField="amtdd" HeaderStyle-HorizontalAlign="Center"
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
                                     <asp:BoundField HeaderText="OBRM Status" DataField="OBRMSTATUS" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                        </asp:GridView>
                    </div>    
                            </td>
                            </tr>
                        </table>
                    </div>                   
     </asp:Panel>   
                </div>
</asp:Content>
