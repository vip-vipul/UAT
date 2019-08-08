<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptLcowiseAwailBalDet.aspx.cs" Inherits="PrjUpassPl.Reports.rptLcowiseAwailBalDet" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
           
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <div class="tblSearchItm">
                        <table width="100%">
                           <tr>
                                <td align="right" width="53%">
                                    <%--<asp:RadioButtonList ID="rdolstSubsSearch" AutoPostBack="true" runat="server" Font-Names="Tahoma"
                                        Font-Size="9pt" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdolstSubsSearch_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="0">LCO Code</asp:ListItem>
                                        <asp:ListItem Value="1">LCO Name</asp:ListItem>
                                    </asp:RadioButtonList>--%>
                                    <asp:DropDownList ID="ddlLco" AutoPostBack="true" runat="server" Height="19px" Style=" resize: none;"
                                        Width="304px">
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                    &nbsp;
                                    <asp:HiddenField ID="hfLCOCode" runat="server" />
                                    <asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="button" UseSubmitBehavior="false"
                                        OnClick="btnSearch_Click" />
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
                                    Visible="false" UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" />
                            </td>
                            <td class="style68">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    
<div id="DivRoot" runat="server" align="left" style="width: 100%;display:none">
                        <div style="overflow: hidden;width:100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll;width:100%" onscroll="OnScrollDiv(this)" id="DivMainContent" 


                    <asp:GridView ID="grdAwailBal" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                        ShowFooter="true" Width="100%" AllowSorting="true" AllowPaging="true" PageSize="100"
                        OnRowDataBound="grdAwailBal_RowDataBound" OnSorting="grdAwailBal_Sorting" OnPageIndexChanging="grdAwailBal_PageIndexChanging">
                        <FooterStyle CssClass="GridFooter" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" FooterText="" />
                            <asp:BoundField HeaderText="LCO Name" DataField="lconame" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" ItemStyle-Width="200px" />
                            <asp:BoundField HeaderText="Total Balance" DataField="actuallim" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Allocated Balance" DataField="allocatedlimit" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Unallocated Balance" DataField="availablelimit" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Last Transaction Date" DataField="last_transdt" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Company Name" DataField="companyname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Distributor" DataField="distributor" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Sub Distributor" DataField="subdistributor" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="State" DataField="statename" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="City" DataField="cityname" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField HeaderText="DAS Area" DataField="DASAREA" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" />
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
                     <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
            </div>
        <%--</ContentTemplate>
        <Triggers>
            
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
            <asp:PostBackTrigger ControlID="btngrnExel" />
           
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
    </cc1:UpdatePanelAnimationExtender>--%>
</asp:Content>
