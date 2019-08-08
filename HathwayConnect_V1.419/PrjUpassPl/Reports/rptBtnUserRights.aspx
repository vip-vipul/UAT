<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptBtnUserRights.aspx.cs" Inherits="PrjUpassPl.Reports.rptBtnUserRights" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            font-size: 12px;
            font-weight: bold;
        }
        
        .header
        {
            background: lightgrey;
        }
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
     function back() {

         window.location.href = "../Reports/rptnoncasreport.aspx";
         return false;
     }
     function InProgress() {
         document.getElementById("imgrefresh").style.visibility = 'visible';
     }
     function onComplete() {
         document.getElementById("imgrefresh").style.visibility = 'hidden';
     }
     function closeMsgPopup() {
         $find("popCheques").hide();
         return false;

     }

     function closeMsgPopupnew() {
         $find("mpeMsg").hide();
         return false;
     }

    </script>

       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="maindive">
            <div style="float: right">
            <button onclick="return back()" id="btnreturnBulkOperation" runat="server" style="margin-right: 5px;
                margin-top: -15px;" class="button">
                Back</button>
        </div>
                <asp:Label ID="lblResponse" runat="server" ForeColor="Red" Text=""></asp:Label>
               
                <div>
                    LCO :
                    <asp:DropDownList ID="ddlLco" runat="server" AutoPostBack="true" Height="19px" Style="resize: none;"
                        Width="304px" >
                    </asp:DropDownList>
                </div>
                <br />
                <div class="griddiv"  runat="server" id="div1" >
                    <div class="delInfo" style="padding: 10px; width: 70%">
                        <asp:GridView ID="grdUsers" Width="100%" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                            >
                            <Columns>
                            <asp:TemplateField HeaderText="User ID">
                            <ItemTemplate >
                            <asp:LinkButton ID="lnkUserID" runat="server" Text='<%# Eval("username").ToString() %>' OnClick="lnkUserID_Click"></asp:LinkButton>
                            <asp:HiddenField id="hdnUserID" runat="server" Value='<%# Eval("username").ToString() %>' />
                            <asp:HiddenField id="hdnUserName" runat="server" Value='<%# Eval("Name").ToString() %>' />
                            </ItemTemplate>
                            </asp:TemplateField>
                                <asp:BoundField HeaderText="Name" DataField="Name" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Address" DataField="Addr" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Email ID" DataField="email" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Mobile No" DataField="MobNo" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <div class="delInfo" style="padding: 10px; width: 90%" runat="server" visible="false" id="div3">
                                         <asp:GridView runat="server" ID="grdUsers_2" CssClass="Grid" AutoGenerateColumns="false"
                                    ShowFooter="true" AllowPaging="true" PageSize="100" >
                                    <FooterStyle CssClass="GridFooter" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VAR_ACCESS_USERNAME" HeaderText="User Id" SortExpression="VAR_ACCESS_USERNAME"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"  />
                                        <asp:BoundField DataField="PlanAdd" HeaderText="Plan Add" SortExpression="PlanAdd"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"  />
                                        <asp:BoundField HeaderText="Plan Renew" DataField="PlanRenew" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Plan Change" DataField="PlanChange" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Plan Cancel" DataField="PlanCancel" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Discount" DataField="Discount" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Retrack" DataField="Retrack" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Customer Modify" DataField="CustModify" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="STB Swap" DataField="STBSwap" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="AutoRenew" DataField="AutoRenew" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Activate/Deactivate" DataField="Deactivate" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Terminate" DataField="Div_TERMINATE" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Foc Pack" DataField="FocPack" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                        
                                    </Columns>
                                </asp:GridView>
       
            </div>
            <%-- -----------------------------------Loader--------------------------- --%>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
            </div>

     
        </ContentTemplate>
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
