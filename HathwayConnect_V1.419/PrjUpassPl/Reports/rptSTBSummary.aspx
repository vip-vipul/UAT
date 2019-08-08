<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptSTBSummary.aspx.cs" Inherits="PrjUpassPl.Reports.rptSTBSummary" %>
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

     function closeMsgPopup() {
         $find("popCheques").hide();
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
                                <td align="left">
                                  
                                     LCO Name: <asp:DropDownList ID="ddlLco" runat="server" Height="19px" Style="resize: none;"
                                        Width="304px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                  
                                    Receipt No.:<asp:TextBox runat="server" ID="txtRecptNo" BorderWidth="1"></asp:TextBox>
                                    
                                </td>
                            </tr>
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
                    <div id="DivRoot" runat="server" align="left" style="width: 100%; display: none">
                        <div style="overflow: hidden; width: 100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll; width: 100%" onscroll="OnScrollDiv(this)" id="DivMainContent">
                            <asp:GridView ID="grdBulkProc" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                                ShowFooter="true" Width="100%" AllowSorting="true" AllowPaging="true" PageSize="10"
                                OnPageIndexChanging="grdBulkProc_PageIndexChanging" OnRowCommand="grdBulkProc_RowCommand" DataKeyNames="stb_id">
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Unique ID" DataField="dtttime" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" FooterText="" />--%>
                                    <%--<asp:TemplateField HeaderText="Unique ID" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"uploadid")%>'
                                                CommandName="UniqId"></asp:LinkButton>
                                            <asp:Label ID="lbluid1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"uploadid")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField HeaderText="Receipt No." DataField="receiptno" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="total" DataField="total" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                         
                                          <asp:TemplateField HeaderText="STB Faulty" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSTBFaulty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"stb_faulty")%>'
                                                CommandName="stb_faulty"></asp:LinkButton>
                                            <asp:Label ID="lbSTBFaulty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"stb_faulty")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="VC Faulty" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkVCFaulty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"vc_faulty")%>'
                                                CommandName="vc_faulty"></asp:LinkButton>
                                            <asp:Label ID="lblvcfaulty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"vc_faulty")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="STB Good" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSTBGood" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"stb_good")%>'
                                                CommandName="stb_good"></asp:LinkButton>
                                            <asp:Label ID="lblStbGood" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"stb_good")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="VC Good" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkVCGood" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"vc_good")%>'
                                                CommandName="vc_good"></asp:LinkButton>
                                            <asp:Label ID="lblVDGood" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"vc_good")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                       <asp:TemplateField HeaderText="STB Undelivered" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSTBUndelivered" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"stb_undelivered")%>'
                                                CommandName="stb_undelivered"></asp:LinkButton>
                                            <asp:Label ID="lblSTBUndelivered" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"stb_undelivered")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="VC Undelivered" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkVCUndelivered" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"vc_undelivered")%>'
                                                CommandName="vc_undelivered"></asp:LinkButton>
                                            <asp:Label ID="lblVCUndelivered" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"vc_undelivered")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:BoundField HeaderText="STB Pending Count" DataField="stbpendingcount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />

                               

                                      <asp:BoundField HeaderText="VC Pending Count" DataField="vcpendingcount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />

                                  


                                    <asp:BoundField HeaderText="Scheme Name" DataField="scheme_name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />

                                        <asp:BoundField HeaderText="Scheme Type" DataField="scheme_type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" Visible="false" />

                                    <%--<asp:BoundField HeaderText="STB Faulty" DataField="stb_faulty" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField HeaderText="VC Faulty" DataField="vc_faulty" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="left" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="DivFooterRow" style="overflow: hidden">
                        </div>
                    </div>
                </div>
            </asp:Panel>



                          <cc1:ModalPopupExtender ID="popCheques" runat="server" BehaviorID="popCheques" Drag="true"
        TargetControlID="hdnPDC" PopupControlID="pnlPDC" DropShadow="true">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnPDC" runat="server" />
    <asp:Panel ID="pnlPDC" runat="server" CssClass="Popup" Style="width: auto; height: auto;
        min-width: 700px; min-height: 300px">
        <%-- display: none; --%>
        <asp:Image ID="Image1" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeMsgPopup();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Detail View :
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                        <asp:Label ID="lblPDCMsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="600px">
                <tr>
                    <td align="center" width="100%">
                        <asp:GridView runat="server" ID="grdBulkstatus" CssClass="Grid" AutoGenerateColumns="false"
                                OnPageIndexChanging="grdBulkstatus_PageIndexChanging" ShowFooter="true" AllowPaging="true"
                                PageSize="10">
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="STB" DataField="stb" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="SCHEME" DataField="scheme" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="TYPE" DataField="type" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="STATUS" DataField="status" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                         <asp:BoundField HeaderText="CONFIRM STATUS" DataField="confirmstatus" HeaderStyle-HorizontalAlign="Center"
                                        Visible="false" ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                                <PagerSettings Mode="Numeric" />
                            </asp:GridView>
                    </td>
                </tr>
                <%--<tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnpdcsubmit" runat="server" CssClass="button" Text="Submit" Width="100px"
                         visible="false" />
                        <asp:Button ID="btnpdcClose" runat="server" CssClass="button" Text="Reset" Width="100px"
                         visible="false"    />
                    </td>
                </tr>--%>
            </table>
        </center>
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
            <%--<asp:AsyncPostBackTrigger ControlID="" EventName="Click" />--%>
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
