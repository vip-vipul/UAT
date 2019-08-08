<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransHwayOsdBmailFlagMap.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayOsdBmailFlagMap" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">

function back()
    {
        window.location="../Transaction/TransNotificationPages.aspx";
    }
        function CheckAllA(chk) {

            var grid = document.getElementById("<%= grddt.ClientID %>");
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[12];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            cell.childNodes[j].checked = chk.checked;
                        }
                    }
                }
            }
        }
        function CheckAllD(chk) {

            var grid = document.getElementById("<%= grddt.ClientID %>");
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[12];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            cell.childNodes[j].checked = chk.checked;
                        }
                    }
                }
            }
        }

        function RevokeConfirmation() {
            var grid = document.getElementById("<%= grddt.ClientID %>");
            var TotalTrueRows = 0;
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[12];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            if (cell.childNodes[j].checked) {
                                TotalTrueRows = TotalTrueRows + 1;
                            }
                        }
                    }
                }
            }
            if (TotalTrueRows > 0) {
                if (confirm("Are you sure, you want to Save selected Transactions ?") == true)
                    return true;
                else
                    return false;
            }
            else {
                alert("Please select atleast one record");
                return false;
            }

        }
		    
		    
    </script>
    <script type="text/javascript">
        function CheckOne() {

            var grid = document.getElementById("<%= grddt.ClientID %>");
            var TottalRows = 0;
            var TotalTrueRows = 0;
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[12];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            TottalRows = TottalRows + 1;
                            if (cell.childNodes[j].checked) {
                                TotalTrueRows = TotalTrueRows + 1;
                            }
                        }
                    }
                }
            }
            if (TottalRows == TotalTrueRows) {
                cell = grid.rows[0].cells[12];

                for (j = 0; j < cell.childNodes.length; j++) {

                    if (cell.childNodes[j].type == "checkbox") {
                        cell.childNodes[j].checked = true;
                    }
                }
            }
            else {
                cell = grid.rows[0].cells[12];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        cell.childNodes[j].checked = false;
                    }
                }
            }
        }
        function CheckOne1() {
            var grid = document.getElementById("<%= grddt.ClientID %>");
            var TottalRows = 0;
            var TotalTrueRows = 0;
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[12];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            TottalRows = TottalRows + 1;
                            if (cell.childNodes[j].checked) {
                                TotalTrueRows = TotalTrueRows + 1;
                            }
                        }
                    }
                }
            }
            if (TottalRows == TotalTrueRows) {
                cell = grid.rows[0].cells[12];

                for (j = 0; j < cell.childNodes.length; j++) {

                    if (cell.childNodes[j].type == "checkbox") {
                        cell.childNodes[j].checked = true;
                    }
                }
            }
            else {
                cell = grid.rows[0].cells[12];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        cell.childNodes[j].checked = false;
                    }
                }
            }
        }
        function goBack() {
            window.history.back();
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

        function InProgress() {

            document.getElementById("imgrefresh2").style.visibility = 'visible';
        }
        function onComplete() {

            document.getElementById("imgrefresh2").style.visibility = 'hidden';
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div id="imgrefresh2" class="loader transparent">
                    <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                        AlternateText="Loading ..." ToolTip="Loading ..." />
                </div>
                <div class="maindive">
                <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <div class="tblSearchItm" id="Parent" runat="server" style="width: 30%;">
                        <table width="100%">
                           
                            <tr>
                                <td align="left" class="cal_image_holder">
                                   LCO </td>
                                <td>
                                    :</td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlLco" AutoPostBack="true" runat="server" Height="19px" Style="resize: none;"
                                                            Width="304px">
                                                            </asp:DropDownList></td>
                            </tr>
                           
                            <tr>
                                <td align="left" class="cal_image_holder">
                                    From Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td align="left">
                                    <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1" AutoPostBack="true"></asp:TextBox>
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
                                    <asp:TextBox runat="server" ID="txtTo" BorderWidth="1" AutoPostBack="true"></asp:TextBox>
                                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    Notification Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td align="left" class="cal_image_holder">
                                    <asp:DropDownList ID="ddlType" runat="server" Height="19px" Style="resize: none;"
                                        Width="150px">
                                        <asp:ListItem Text="Select All" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="BMAIL" Value="BMAIL"></asp:ListItem>
                                    </asp:DropDownList>
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
                            <tr align="center">
                                <td colspan="6">
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="griddiv">
                    
                    <table>
                    
                        <tr id="parentgrid" runat="server">
                            <td align="left">
                                
                                        <asp:GridView runat="server" ID="grdExpiry" CssClass="Grid" AutoGenerateColumns="false"
                                            ShowFooter="true" OnRowCommand="grdExpiry_RowCommand" AllowPaging="true" PageSize="10"
                                            OnPageIndexChanging="grdExpiry_PageIndexChanging">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reference Id" ItemStyle-HorizontalAlign="Left" FooterText="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkreferenceid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"referenceid")%>'
                                                            CommandName="referenceid"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Reference ID" DataField="referenceid" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Count" DataField="count" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Inserted By" DataField="insby" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Inserted Date" DataField="insdt" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerSettings Mode="Numeric" />
                                        </asp:GridView>
                                  
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                
                                    <div id="Child" runat="server">
                                        <asp:GridView runat="server" ID="grddt" CssClass="Grid" AutoGenerateColumns="false"
                                            ShowFooter="true" AllowPaging="true" PageSize="15" OnPageIndexChanging="grddt_PageIndexChanging"
                                            OnRowDataBound="grddt_RowDataBound">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Account No" DataField="ACCNO" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="VC ID" DataField="VCID" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Customer Name" DataField="CUSTNAME" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Notification" DataField="NOTIFTYPE" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Template Message" DataField="TEMPLATE_MSG" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Full Message" DataField="FULL_MSG" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Frequency" DataField="FREQUENCY" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Duration" DataField="DURATION" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Position" DataField="POSITION" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Inserted By" DataField="INSBY" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Inserted Date" DataField="INSDT" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left">
                                                    <FooterStyle HorizontalAlign="left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblAllRenew" runat="server" Text="Activation"></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox runat="server" ID="mainCB" onclick="javascript:CheckAllA(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="cbRenew" OnClick="CheckOne();" Checked='<%# bool.Parse(Eval("activeflag").ToString() == "Y" ? "True": "False") %>' />
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblAllDelete" runat="server" Text="Deactivation"></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox runat="server" ID="maindelete"
                                                            onclick="javascript:CheckAllD(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="cbdelete" OnClick="CheckOne1();" Checked='<%# bool.Parse(Eval("deleteflag").ToString() == "Y" ? "True": "False") %>' />
                                                    <asp:HiddenField ID="hdntransid" runat="server" Value='<%# Eval("TRANSID")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="Numeric" />
                                        </asp:GridView>
                                    </div>
                                
                            </td>
                        </tr>
                        <tr align="center">
                            <td>
                                <asp:Label ID="lblSave" ForeColor="Red" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr align="center">
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="Submit" CssClass="button" OnClick="btnSave_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnCancel" Text="Back" CssClass="button" UseSubmitBehavior="false"
                                    OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                   </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:UpdatePanelAnimationExtender ID="updPlanAniEx" runat="server" TargetControlID="UpdatePanel1">
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
