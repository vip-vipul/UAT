<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptLCOPlanShare.aspx.cs" Inherits="PrjUpassPl.Reports.WebForm1" %>
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
    </script>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>        
        <div class="maindive"> <br />
            <asp:Panel runat="server" ID="pnlSearch">                
                 <div style="float: Right">
            <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                Back</button>
        </div>  
                    <div class="tblSearchItm" style="width: 50%;">
                             <table width="120%">
                             <tr id="tr1" runat="server" width="50%">
                                <td align="center">
                                    <asp:CheckBox ID="chkdt"  runat="server" Text="From Date" AutoPostBack="True" 
                                        oncheckedchanged="chkdt_CheckedChanged" />
                                    &nbsp;:<asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>
                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                    <asp:Image runat="server" ID="imgFrom" 
                                        ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr id="tr2" runat="server" width="100%">
                                <td align="center">
                                    To Date&nbsp;:<asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                    <asp:Image runat="server" ID="imgTo" 
                                        ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:CheckBox ID="chkcity"  runat="server" Text="City:" 
                                        oncheckedchanged="chkcity_CheckedChanged" AutoPostBack="True" />
                                    &nbsp;<asp:DropDownList ID="ddlcity" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlcity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:CheckBox ID="chkplan" runat="server" Text="Plan:" 
                                        oncheckedchanged="chkplan_CheckedChanged" AutoPostBack="True" />
                                    <asp:DropDownList ID="ddlplan" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlplan_SelectedIndexChanged">
                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Basic" Value="B"></asp:ListItem>
                                        <asp:ListItem Text="Add On" Value="AD"></asp:ListItem>
                                        <asp:ListItem Text="Ala-Carte" Value="AL"></asp:ListItem>
                                    </asp:DropDownList>
                                &nbsp;<asp:CheckBox ID="chkaddmap" runat="server" Text="Addon Mapping" 
                                        oncheckedchanged="chkaddmap_CheckedChanged" AutoPostBack="True" />
                                    &nbsp;<asp:CheckBox ID="chkabcmap" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="chkabcmap_CheckedChanged" Text="Basic Mapping" />
                                    <br />
                                </td>                                
                            </tr>
                            <tr id="tr4" runat="server" width="60%">
                                <td align="center">
                                    <asp:Label ID="lblcity" runat="server" Text="City:" Visible="true"></asp:Label>
                                    <asp:DropDownList ID="ddladdcity" runat="server" 
                                        onselectedindexchanged="ddladdcity_SelectedIndexChanged" Visible="true" 
                                        Height="19px" Width="145px" AutoPostBack="True">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblbasicplan" runat="server"></asp:Label>
&nbsp;<asp:DropDownList ID="ddladdplan" runat="server" Visible="true" Height="16px" Width="200px" 
                                        onselectedindexchanged="ddladdplan_SelectedIndexChanged" 
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                     &nbsp;&nbsp;&nbsp;&nbsp;
                                     <asp:Button ID="btnPlanDl" runat="server" Text="Set Plan Details" 
                                         OnClick="btnPlanDl_Click" Visible="False" />
                                          &nbsp;&nbsp;
                                          <asp:Button ID="btndownPlandl" runat="server" Text="Download Plan Details" onclick="btndownPlandl_Click" 
                                          />
                               
                                    </td>
                            </tr>
                                 <tr>
                                     <td align="center">
                                         <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                                     </td>
                                 </tr>
                                 </table>
                                 </div>
                                  <table>
                        <tr>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div class="griddiv">
                                    <asp:GridView runat="server" ID="grdExpiry" CssClass="Grid" AutoGenerateColumns="false"
                                        ShowFooter="true" AllowPaging="true" PageSize="100"
                                        OnPageIndexChanging="grdExpiry_PageIndexChanging" Visible="False" 
                                        onselectedindexchanged="grdExpiry_SelectedIndexChanged">
                                        <%--OnRowCommand="grdLcoPartyLedger_RowCommand" OnRowDataBound="grdLcoPartyLedger_RowDataBound"
                        OnSorting="grdLcoPartyLedger_Sorting"--%>
                                        <FooterStyle CssClass="GridFooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Plan Name" DataField="var_plan_name" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" 
                                                FooterStyle-HorizontalAlign="Right" >
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Plan Type" DataField="var_plan_plantype" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Plan Poid" DataField="var_plan_planpoid" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Deal Poid" DataField="var_plan_dealpoid" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Product Poid" DataField="var_plan_productpoid" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Customer Price" DataField="num_plan_custprice" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Lco Price" DataField="num_plan_lcoprice" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="City" DataField="var_city_name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField HeaderText="LCO Code" DataField="LCO_Code" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField HeaderText="LCO Name" DataField="LCO_NAME" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerSettings Mode="Numeric" />
                                    </asp:GridView>
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
        <asp:AsyncPostBackTrigger ControlID="btnPlanDl" EventName="Click" />
                <asp:PostBackTrigger ControlID="btndownPlandl" />

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
