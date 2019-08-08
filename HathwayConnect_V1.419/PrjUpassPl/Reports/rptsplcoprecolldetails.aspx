<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptsplcoprecolldetails.aspx.cs"
    MasterPageFile="~/MasterPage.Master" Inherits="PrjUpassPl.Reports.rptsplcoprecolldetails" %>

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
<script type="text/javascript">   
    function goBack() {
        window.location.href = "../Reports/rptnoncasreport.aspx";
        return false;
    }
    </script>
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
         function checkvalid() {


             var rbtnvalue = $('#<%=RadSearchby.ClientID %> input[type=radio]:checked').val();

             if (rbtnvalue == 1) {

                 if (document.getElementById("<%=txtsearchpara.ClientID%>").value == "") {
                     alert("Please Enter VC No.!!");
                     return false;
                 }
             }
         }sss
    </script>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div class="maindive">
        <div style="float: right">
            <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                Back</button>
        </div>      
                <asp:Panel runat="server" ID="pnlSearch">
                    <br />
                    <div class="tblSearchItm" style="width: 30%;">
                        <table width="100%">
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;&nbsp;&nbsp; From Date :
                                    <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>
                                    <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;&nbsp;&nbsp; &nbsp;&nbsp; To Date :
                                    <asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>
                                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                        <table width="120%">
                          <tr>
                                <td align="right">
                                    <asp:Label ID="lblUser" runat="server" Text="Search LCO By :"></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="RadSearchby" AutoPostBack="true" runat="server" 
                                        RepeatDirection="Horizontal" 
                                        onselectedindexchanged="RadSearchby_SelectedIndexChanged1">
                                        <asp:ListItem Value="0" style="display:none">Account No.</asp:ListItem>
                                        <asp:ListItem Value="1">VC Id</asp:ListItem>
                                        <asp:ListItem Value="2" style="display:none">LCO Code</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsearchpara" runat="server" Style="resize: none;" Width="180px"
                                        MaxLength="20" onkeydown="SetContextKey()" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>                       
                            <tr>
                                <td style="padding-left: 60px" align="center">
                                    
                                </td>
                                <td>
                                <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button"
                                        OnClick="btnSubmit_Click" OnClientClick="return checkvalid();" />
                                </td>
                                <td align="left">
                                <asp:Button runat="server" ID="Button2" Text="Reset" CssClass="button" 
                                        OnClick="btnreset_Click"/>
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
                            <td align="left">
                                <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                    UseSubmitBehavior="false" OnClick="btnGenerateExcel_Click" Visible="false" Width="113px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                               <div id="DivRoot" runat="server" align="left" style="width: 100%;display:none">
                        <div style="overflow: hidden;width:100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll;width:100%" onscroll="OnScrollDiv(this)" id="DivMainContent" 

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
                                            <asp:BoundField HeaderText="Customer Name" DataField="customer_name" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Entity Code" DataField="entity_code" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="LCO_Name" DataField="lco_name" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="City" DataField="city" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="State" DataField="state" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Area" DataField="area" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Receipt No." DataField="receipt_no" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Amount" DataField="amount" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Receipt Date" DataField="receipt_date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Reversal date" DataField="reversal_date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Created By Username" DataField="created_by_username"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Description" DataField="description" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Customer Type" DataField="customer_type" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Payment Mode" DataField="payment_mode" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Cheque No." DataField="cheque_no" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Cheque Date" DataField="cheque_date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Bank Name" DataField="bank_name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Branch Name" DataField="branch_name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Bank Code" DataField="bank_code" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="Payment Channel" DataField="payment_channel" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Upass Receipt No." DataField="upass_reciept_no" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Reversal Status" DataField="reversal_status" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="JV" DataField="jv" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Distributer" DataField="distributer" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Sub Distributer" DataField="sub_distributer" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Company" DataField="company" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Report Date" DataField="report_date" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="From Date" DataField="from_date" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="To Date" DataField="to_date" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <PagerSettings Mode="Numeric" />
                                    </asp:GridView>
                                </div>
                        <div id="DivFooterRow" style="overflow: hidden">
                        </div>
                    </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
               </div>                
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
</asp:Content>

