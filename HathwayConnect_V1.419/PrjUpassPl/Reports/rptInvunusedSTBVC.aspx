<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptInvunusedSTBVC.aspx.cs" Inherits="PrjUpassPl.Reports.rptInvunusedSTBVC" %>
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
    function goBack() {
        window.location.href = "../Reports/rptnoncasreport.aspx";
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
        <script type="text/javascript">

            function InProgress() {

                document.getElementById("imgrefresh2").style.visibility = 'visible';
            }
            function onComplete() {

                document.getElementById("imgrefresh2").style.visibility = 'hidden';
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                <div style="float:right">
                <button onclick="return goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <div class="tblSearchItm" style="width: 30%;">
                       <table width="120%">
                           <tr>
                                <td align="right">
                                    <asp:Label ID="lblUser" runat="server" Text="Search By :"></asp:Label>
                                </td>
                                <td>
                               
                               <asp:DropDownList ID="ddlPackage" runat="server" >
                                        <asp:ListItem Text="STB No" Value="STB"></asp:ListItem>
                                        <asp:ListItem Text="VC ID" Value="VC"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="padding-left: 60px" align="center">
                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button"
                                        OnClick="btnSubmit_Click" OnClientClick="return checkvalid();" />
                                        &nbsp;
                                         <asp:Button runat="server" ID="Button2" Text="Reset" CssClass="button" 
                                        />
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
                                <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" style="  width: 100px;" CssClass="button"
                                    UseSubmitBehavior="false" Visible="false" OnClick="btnGenerateExcel_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                              


<div id="DivRoot" runat="server" align="left" style="width: 100%;display:none">
                       
                        <div style="overflow: scroll;width:100%" onscroll="OnScrollDiv(this)" id="DivMainContent" >

                                    <asp:GridView runat="server" ID="grdExpiry" CssClass="Grid" AutoGenerateColumns="false"
                                        ShowFooter="true"  AllowPaging="true" PageSize="100">
                                        <FooterStyle CssClass="GridFooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="STB/VC" DataField="stbno" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Lco Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            
                                            <asp:BoundField HeaderText="Type" DataField="Type" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Box Type" DataField="Boxtype" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Scheme Name" DataField="schname" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Plan Name" DataField="planname" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Activation Allowed" DataField="activation_allowed" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Termination Allowed" DataField="termination_allowed" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="Plan Change Allowed" DataField="plan_change_allowed" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Inserted By" DataField="insby" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Inserted Date" DataField="insdate" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left"  />
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
                </div>
            </asp:Panel>
                   <div id="imgrefresh2" class="loader transparent">
                <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                    <asp:ImageButton ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()">
                </asp:ImageButton>
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
