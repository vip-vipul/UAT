﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" 
CodeBehind="rptExpired.aspx.cs" Inherits="PrjUpassPl.Reports.rptExpired" %>

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
    function goBack() {
        window.location.href = "../Reports/rptnoncasreport.aspx";
        return false;
    }
    function InProgress() {

        document.getElementById("imgrefresh2").style.visibility = 'visible';
    }
    function onComplete() {

        document.getElementById("imgrefresh2").style.visibility = 'hidden';
    }
        
        
    </script>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <ContentTemplate>
    <div class="maindive">
    <div style="float:right">
                <button onclick="return goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
       
          
                <asp:Panel runat="server" ID="pnlSearch">
                    <br />
                    <div class="tblSearchItm" style="width: 50%;">
                        <table width="100%">
                        <tr>
                         <td align="right">
                                LCO :
                                </td>
                                <td colspan="3">
                                <asp:DropDownList ID="ddlLco" runat="server" Height="19px" 
                                    Style="resize: none;" Width="304px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                          <tr>
                                <td align="right" >
                                     Search by  :
                                     </td>
                                     <td colspan="4">
                                     <asp:RadioButtonList ID="rodserch" runat="server" RepeatDirection="Horizontal" style="float:left">
                                     <asp:ListItem Selected="True" Text="Account" Value="ACC"></asp:ListItem>
                                     <asp:ListItem  Text="VC/MAC" Value="VC"></asp:ListItem>
                                    </asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   
                                    <asp:TextBox runat="server" ID="txtaccvc" BorderWidth="1" style="float:left" MaxLength="20"></asp:TextBox> 
                                </td>
                            </tr>
                        <tr>
                                <td align="right" class="cal_image_holder">
                                    From Date :
                                    </td>
                                    <td>
                                    <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>                                   
                                    <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                                <td align="right" class="cal_image_holder">
                                   To Date :
                                    </td>
                                    <td>
                                    <asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>                                   
                                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                          
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="center">
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
                            <td align="left">
                                <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" OnClick="btnGenerateExcel_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            <div class="griddiv">
                                <asp:GridView runat="server" ID="grdExpiry" CssClass="Grid" AutoGenerateColumns="false"
                                    ShowFooter="true" OnRowCommand="grdExpiry_RowCommand" AllowPaging="true" PageSize="100"
                                    OnPageIndexChanging="grdExpiry_PageIndexChanging">
                                    <%--OnRowCommand="grdLcoPartyLedger_RowCommand" OnRowDataBound="grdLcoPartyLedger_RowDataBound"
                        OnSorting="grdLcoPartyLedger_Sorting"--%>
                                    <FooterStyle CssClass="GridFooter" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Account Number" DataField="account_no" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderText="VC Id" ItemStyle-HorizontalAlign="Left" FooterText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LBvc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"vc")%>'
                                                    CommandName="vcid"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnfullname" runat="server" Value='<%# Eval("fullname")%>' />
                                                <asp:HiddenField ID="HdnAddress" runat="server" Value='<%# Eval("address")%>' />
                                                <%--<asp:Label ID="lblOperid1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"lco_code")%>'
                                        Visible="false"></asp:Label><asp:Label ID="lblolconame" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"lco_name")%>'
                                            Visible="false"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Customer Name" DataField="fullname" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Address" DataField="address" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="LCO Name" DataField="lco_name" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Lco Code" DataField="lco_code" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Mobile" DataField="mobile" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Package" DataField="planname" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="City" DataField="cityname" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Plan Type" DataField="plantype" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" />
                                        <asp:BoundField HeaderText="Disconnect date" DataField="enddate" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <PagerSettings Mode="Numeric" />
                                </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                </div>
                <%-- ---------------------------------------------------ADDON POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popExp" runat="server" BehaviorID="mpeExp" TargetControlID="hdnPop3"
                    PopupControlID="pnlExp" CancelControlID="imgClose3">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnPop3" runat="server" />
                <asp:Panel ID="pnlExp" runat="server" CssClass="Popup" Style="width: 650px; height: 250px;
                    display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="imgClose3" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" onclick="closeExpPopup();" ImageUrl="~/Images/closebtn.png" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;&nbsp;Expiry Details
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table width="90%">
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="Label21" Font-Bold="true" Text="Account No."></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label143" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblAccNo" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="label1" runat="server" Font-Bold="true" Text="VC No."></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblVCNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="Label12" Font-Bold="true" Text="LCO Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lbllcoName" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="Label7" Font-Bold="true" Text="Full Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblfullname"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="Label13" Font-Bold="true" Text="Address"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lbladdress"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="130px">
                                    <asp:Label runat="server" ID="Label3" Font-Bold="true" Text="Mobile"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblMobile" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="130px">
                                    <asp:Label runat="server" ID="Label5" Font-Bold="true" Text="Plan name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblplan" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="label9" runat="server" Font-Bold="true" Text="Plan type"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label10" runat="server" Text=":"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblplantype" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="100px">
                                    <asp:Label runat="server" ID="lblServiceStat" Font-Bold="true" Text="Disconnect date"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblEnddate" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <div id="imgrefresh2" class="loader transparent">                   
                      <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
                    
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
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
    </div>
</asp:Content>
