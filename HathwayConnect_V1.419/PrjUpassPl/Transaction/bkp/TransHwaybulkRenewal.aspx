<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransHwaybulkRenewal.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwaybulkRenewal" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    
    

        function FunOnload() {
            if (Divgrd.innerText != "") {
                var id = "1";
                FunChkDisable(id);
            }
        }




        function FunChkDisable(id) {

            var gv = document.getElementById("<%= grdExpiry.ClientID %>");

            var chkrow;
            if (gv.rows.length > 0) {
                var chk = gv.rows[0].cells[13];
                for (i = 0; i < gv.rows.length - 1; i++) {
                    cell = gv.rows[i].cells[13];

                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            if (id == "1") {
                                cell.childNodes[j].disabled = false;
                            }
                            else if (id == "2") {
                                cell.childNodes[j].disabled = false;
                            }
                            else if (id == "3") {
                                cell.childNodes[j].disabled = true;
                            }
                            else if (id == "4") {
                                cell.childNodes[j].checked = true;
                            }
                            else if (id == "5") {
                                cell.childNodes[j].checked = false;
                            }


                        }
                    }
                }
            }

        }



        function Check_ClickS(objRef) {
            var row = objRef.parentNode.parentNode;
            var GridView = row.parentNode;
            var inputList = GridView.getElementsByTagName("*");
            for (var i = 0; i < inputList.length; i++) {

                var ChkAutorenw = "";
                if (inputList[i].id.indexOf("cbRenew") != -1) {
                    Chkrenew = inputList[i];
                }
                if (inputList[i].id.indexOf("cbAutoRenew") != -1) {
                    ChkAutorenw = inputList[i];
                }
                if (ChkAutorenw != "") {
                    if (Chkrenew.checked) {
                        ChkAutorenw.disabled = false;


                    }
                    else {

                        //                         document.getElementById("<%= HidChRenewalValue.ClientID %>").value = "N";
                        ChkAutorenw.disabled = true;
                        ChkAutorenw.checked = false;
                    }
                }

            }



        }






        $(document).ready(function () {

            FunOnload();
            $("#<%=grdExpiry.ClientID%> input[id*='cbRenew']:checkbox").click(function () {
                //Get number of checkboxes in list either checked or not checked
                var totalCheckboxes = $("#<%=grdExpiry.ClientID%> input[id*='cbRenew']:checkbox").size();
                //Get number of checked checkboxes in list
                var checkedCheckboxes = $("#<%=grdExpiry.ClientID%> input[id*='cbRenew']:checkbox:checked").size();
                //Check / Uncheck top checkbox if all the checked boxes in list are checked
                $("#<%=grdExpiry.ClientID%> input[id*='cbAllrenew']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);

            });

            $("#<%=grdExpiry.ClientID%> input[id*='cbAllrenew']:checkbox").click(function () {
                //Check/uncheck all checkboxes in list according to main checkbox 

                $("#<%=grdExpiry.ClientID%> input[id*='cbRenew']:checkbox").attr('checked', $(this).is(':checked'));
                //  $("#<%=grdExpiry.ClientID%> input[id*='cbAutoRenew']:checkbox").attr('disable', $(this).is(':checked'));


                var status = $("#<%=grdExpiry.ClientID%> input[id*='cbRenew']:checkbox").is(':checked');
                if (status == true) {
                    var id = "2";
                }
                else {
                    var id = "3";
                }

                FunChkDisable(id)

                //                    Check_panRenew(id);

            });




        });


        function FunChkAllAutoRenewal(Objpre) {

            if (Objpre.checked) {
                var id = "4";
            }
            else {
                var id = "5";

            }
            FunChkDisable(id)




        }

        function Check_CbAuto(objRef) {

            return true;
        }



        function closePopup() {
            $find("mpeConfirmation").hide();
            return false;
        }
        function closeExpPopup() {
            $find("mpeExp").hide();
            return false;
        }
        function closeFinalConfPopup() {
            $find("mpeFinalConf").hide();
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


function back1()
    {
        window.location.href="../Transaction/TransBulkPages.aspx";
        return false;
    }
        function InProgress() {

            document.getElementById("imgrefresh2").style.visibility = 'visible';
        }
        function onComplete() {

            document.getElementById("imgrefresh2").style.visibility = 'hidden';
        }

        function goBack() {
            window.history.back();
        }
    </script>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <asp:Panel runat="server" ID="pnlSearch">
    <div class="maindive">
     <div style="float:right">
                <button onclick="return back1()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
        <div class="tblSearchItm" style="width: 30%;">
            <table width="100%">
                <tr>
                    <td align="left" class="cal_image_holder">
                        From Date
                    </td>
                    <td>
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1" AutoPostBack="true" OnTextChanged="txtFrom_TextChanged"></asp:TextBox>
                        <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                        <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                            Format="dd-MMM-yyyy">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        To Date
                    </td>
                    <td>
                        :
                    </td>
                    <td align="left" class="cal_image_holder">
                        <asp:TextBox runat="server" ID="txtTo" BorderWidth="1" AutoPostBack="true" OnTextChanged="txtTo_TextChanged"></asp:TextBox>
                        <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                        <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                            Format="dd-MMM-yyyy">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updPlan" runat="server">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:RadioButton runat="server" ID="rbALL" GroupName="type" Checked="true" Text="ALL"
                                                OnCheckedChanged="rbALL_CheckedChanged" />
                                                 &nbsp;&nbsp;
                                            <asp:RadioButton runat="server" ID="rbB" GroupName="type" Text="Basic" OnCheckedChanged="rbB_CheckedChanged" />
                                            &nbsp;&nbsp;
                                            <asp:RadioButton runat="server" ID="rbAD" GroupName="type" Text="AddOn" OnCheckedChanged="rbAD_CheckedChanged" />
                                            &nbsp;&nbsp;
                                            <asp:RadioButton runat="server" ID="rbAL" GroupName="type" Text="Al-a-Carte" OnCheckedChanged="rbAL_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trType">
                                        <td align="center" valign="middle">
                                            Plan Name :
                                            <asp:DropDownList ID="ddlPlanname" runat="server" Width="230px">
                                                <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtFrom" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtTo" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="rbALL" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="rbAD" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="rbAL" EventName="CheckedChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
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
        <table>
            <%--<tr>
                        <td align="left">
                            <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                UseSubmitBehavior="false" OnClick="btnGenerateExcel_Click" />
                        </td>
                    </tr>--%>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnProceed" Text="Proceed" CssClass="button" UseSubmitBehavior="false"
                        OnClick="btnProceed_Click" Visible="false" />
                </td>
            </tr>
            <tr>
                <td align="left">
                <div id="Div1">
                    <div id="Divgrd" class="griddiv">
                        <asp:GridView runat="server" ID="grdExpiry" CssClass="Grid" AutoGenerateColumns="false"
                            ShowFooter="true" OnRowCommand="grdExpiry_RowCommand" AllowPaging="true" PageSize="10"
                            OnPageIndexChanging="grdExpiry_PageIndexChanging">
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
                                <asp:BoundField HeaderText="Account Number" DataField="account_no" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="VC Id" ItemStyle-HorizontalAlign="Left" FooterText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LBvc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"vc")%>'
                                            CommandName="vcid"></asp:LinkButton>
                                        <asp:HiddenField ID="hdnfullname" runat="server" Value='<%# Eval("fullname")%>' />
                                        <asp:HiddenField ID="HdnAddress" runat="server" Value='<%# Eval("address")%>' />
                                        <asp:HiddenField ID="hdnBrmpoid" runat="server" Value='<%# Eval("brmpoid")%>' />
                                        <%--<asp:Label ID="lblOperid1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"lco_code")%>'
                                        Visible="false"></asp:Label><asp:Label ID="lblolconame" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"lco_name")%>'
                                            Visible="false"></asp:Label>--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Customer Name" DataField="fullname" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Address" DataField="address" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="LCO Name" DataField="lco_name" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Lco Code" DataField="lco_code" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Mobile" DataField="mobile" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Package" DataField="planname" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="City" DataField="cityname" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Plan Type" DataField="plantype" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="End Date" DataField="enddate" HeaderStyle-HorizontalAlign="Center"
                                    Visible="true" ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAllRenew" runat="server" Text="Select All"></asp:Label>
                                        <br />
                                        <%--<asp:CheckBox ID="cbAllrenew" AutoPostBack="true" OnCheckedChanged="CHCKEDCHANGEDALL"  runat="server"  Checked="false" />--%>
                                        <asp:CheckBox ID="cbAllrenew" runat="server" onclick="FunChkDisable(this);" /><%--Add by pankaj  20150627--%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%-- <asp:CheckBox ID="cbRenew" runat="server" Checked="false" OnCheckedChanged="ChckedChanged" AutoPostBack="true"/>--%>
                                        <%--<asp:HiddenField ID="hdnfullname" runat="server" Value='<%# Eval("fullname")%>' />--%>
                                        <asp:CheckBox ID="cbRenew" runat="server" onclick="Check_ClickS(this);" />
                                        <%--Add by pankaj vivek 20150627--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%--Add by pankaj vivek 20150627--%>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAllAutoRenew" runat="server" Text="Auto Renew"></asp:Label>
                                        <br />
                                        <asp:CheckBox ID="cbAllAutorenew" runat="server" onclick="FunChkAllAutoRenewal(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbAutoRenew" runat="server" onclick="Check_CbAuto(this);" />
                                        <%--<asp:HiddenField ID="hdnfullname" runat="server" Value='<%# Eval("fullname")%>' />--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="Numeric" />
                        </asp:GridView>
                    </div>
                    </div>
                </td>
            </tr>
        </table>
        </div>
    </asp:Panel>
    <%-- ---------------------------------------------------ADDON POPUP-------------------------------------------------- --%>
    <cc1:ModalPopupExtender ID="popExp" runat="server" BehaviorID="mpeExp" TargetControlID="hdnPop3"
        PopupControlID="pnlExp" CancelControlID="imgClose3">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnPop3" runat="server" />
    <asp:HiddenField ID="HidDisvalue" runat="server" Value="0" />
    <asp:HiddenField ID="HidChRenewalValue" runat="server" Value="N" />
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
                        <asp:Label runat="server" ID="lblServiceStat" Font-Bold="true" Text="End date"></asp:Label>
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
    <%-- ----------------------------------------------------Confirmation------------------------------------------------ --%>
    <cc1:ModalPopupExtender ID="popFinalConf" runat="server" BehaviorID="mpeFinalConf"
        TargetControlID="hdnPop7" PopupControlID="pnlFinalConfirm">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnPop7" runat="server" />
    <asp:Panel ID="pnlFinalConfirm" runat="server" CssClass="Popup" Style="width: 430px;
        height: 160px;">
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
                        <asp:Label ID="lblPopupFinalConfMsg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnPopupConfYes" runat="server" CssClass="button" Text="Yes" Width="100px"
                            OnClick="btnPopupConfYes_Click" />
                        &nbsp;&nbsp;
                        <input id="Button1" class="button" runat="server" type="button" value="No" style="width: 100px;"
                            onclick="closeFinalConfPopup();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <div id="imgrefresh2" class="loader transparent">
        <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
            AlternateText="Loading ..." ToolTip="Loading ..." />
    </div>
    <%--</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <%--<cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server"
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
    <cc1:UpdatePanelAnimationExtender ID="updPlanAniEx" runat="server" TargetControlID="updPlan">
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
