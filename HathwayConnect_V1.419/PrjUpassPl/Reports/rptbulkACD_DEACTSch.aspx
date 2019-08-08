<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptbulkACD_DEACTSch.aspx.cs" Inherits="PrjUpassPl.Reports.rptbulkACD_DEACTSch" %>
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

  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
                function closemsgbox() {

                    $find("mpeMsgBox").hide();

                }
          </script>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                <div style="float:right">
                <button onclick="return goBack();"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <div class="tblSearchItm" style="width: 30%;">
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
                                <td align="center" class="cal_image_holder">
                                    &nbsp;&nbsp; Status :
                                    <asp:DropDownList ID="Ddlstatus" runat="server" Width="130px">
                                        <asp:ListItem Selected="True" Text="All" Value="ALL"></asp:ListItem>
                                        <asp:ListItem Text="Activated" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Deactivated" Value="D"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;&nbsp; Activity Flag :
                                    <asp:RadioButtonList ID="rblFlag" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="1">Enabling</asp:ListItem>
                                    <asp:ListItem Value="2">Disabling</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="padding-left: 60px" align="center">
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
                    <table width="100%">
                        <tr>
                            <td align="left" class="style67">
                                <asp:Button runat="server" ID="btngrnExel" Text="Generate Excel" CssClass="button"
                                    Visible="false" UseSubmitBehavior="false" align="left" OnClick="btngrnExel_Click" />
                            </td>
                            <td class="style68">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td align="left">
                            <div id="DivRoot" runat="server" align="left" style="width: 100%;display:none">
                        <div style="overflow: hidden;width:100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll;width:100%" onscroll="OnScrollDiv(this)" id="DivMainContent" >

                                <asp:GridView runat="server" ID="grdactdact" CssClass="Grid" AutoGenerateColumns="false"
                                    ShowFooter="true" AllowPaging="true" PageSize="100" 
                                    OnPageIndexChanging="grdactdact_PageIndexChanging" onrowcommand="grdactdact_RowCommand" 
                                    >
                                    <FooterStyle CssClass="GridFooter" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="accno" HeaderText="Account No" SortExpression="lcocode"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="65pt" />
                                        <asp:BoundField HeaderText="VC Id" DataField="VCID" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Status" DataField="ACTION" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Reason" DataField="reason" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Process Flag" DataField="PROCESSFLAG" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Scheduler Date" DataField="SCHDATE" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Insert By" DataField="insby" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Insert Date" DataField="insdt" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Disabl By" DataField="DISABLBY" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Disabl Date" DataField="DISABLDATE" HeaderStyle-HorizontalAlign="Center"
                                            Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                        <asp:Label ID="lblbulkid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"BulkID")%>'
                                                Visible="false"></asp:Label>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text='Remove' CommandName="Remove"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
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
                <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />

            </div>
            <!---------------------------cnfmPopup------------------------>
            <cc1:ModalPopupExtender ID="popMsgBox" runat="server" BehaviorID="mpeMsgBox" TargetControlID="hdnMsgBox"
                PopupControlID="pnlMsgBox">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnMsgBox" runat="server" />
            <asp:Panel ID="pnlMsgBox" runat="server" CssClass="Popup" Style="width: 330px; height: 160px;
                display: none">
                <%-- display: none; --%>
                <center>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Confirmation
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
                            <td align="center" colspan="3">
                                Are you sure to Remove ?
                                <br />
                                
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:HiddenField ID="hdnpopstatus" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="btncnfmBlck" runat="server" Text="Confirm" OnClick="btncnfmBlck_Click" />
                                &nbsp;&nbsp;
                                <input id="Button3" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closemsgbox();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
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
    </cc1:UpdatePanelAnimationExtender>
</asp:Content>
