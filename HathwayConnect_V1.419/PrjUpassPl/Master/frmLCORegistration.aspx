<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmLCORegistration.aspx.cs" Inherits="PrjUpassPl.Master.frmLCORegistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 650px;
            margin: 5px;
        }
        .delInfo1
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 650px;
            margin: 5px;
        }
        .delInfoContent
        {
            width: 95%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript" language="javascript">
        function SetContextKey() {
            var search_type = parseInt('<%= RadSearchby.SelectedValue %>');
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey(search_type);
        }
    </script>
    <asp:Panel runat="server" ID="pnlRegisterLCO">
        <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
        <div class="delInfo1">
            <table runat="server" align="center" width="500px" id="tbl1" border="0">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblUser" runat="server" Text="Search LCO By"></asp:Label>
                        <asp:Label ID="Label37" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        <asp:Label ID="Label44" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="RadSearchby" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged1">
                            <asp:ListItem Value="0" Selected="True">CODE</asp:ListItem>
                            <asp:ListItem Value="1">Name</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLCOSearch" runat="server" Style="resize: none;" Width="150px"
                            onkeydown="SetContextKey()"></asp:TextBox>
                        <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1"
                            UseContextKey="true" CompletionInterval="100" EnableCaching="true" CompletionSetCount="3"
                            TargetControlID="txtLCOSearch" FirstRowSelected="false" ID="AutoCompleteExtender1"
                            runat="server" CompletionListCssClass="autocomplete" CompletionListItemCssClass="autocompleteItem"
                            CompletionListHighlightedItemCssClass="autocompleteItemHover" CompletionListElementID="LcoListHolder">
                        </cc1:AutoCompleteExtender>
                        <div id="LcoListHolder" runat="server">
                        </div>
                        <asp:HiddenField ID="hfLCOCode" runat="server" />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divdetails" runat="server">
            <div class="delInfo">
                <table width="95%">
                    <tr>
                        <td align="left" colspan="6">
                            <b>
                                <asp:Label ID="Label1" runat="server" Text="LCO Details"></asp:Label>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <%--<tr>
                    <td align="left" width="90px;">
                        <asp:Label ID="Label31" runat="server" Text="MSO Name"></asp:Label>
                        <asp:Label ID="Label40" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label32" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlMSO" runat="server" OnSelectedIndexChanged="ddlMSO_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td align="left">
                        <asp:Label ID="Label36" runat="server" Text="Distributor Name"></asp:Label>
                        <asp:Label ID="Label58" runat="server" ForeColor="Red" Text="*"></asp:Label>

                    </td>
                    <td>
                        <asp:Label ID="Label38" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDistributor" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>--%>
                    <tr>
                        <td align="left" width="90px;">
                            <asp:Label ID="Label2" runat="server" Text="LCO Code"></asp:Label>
                            <%--<asp:Label ID="Label41" runat="server" ForeColor="Red" Text="*"></asp:Label>--%>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <%--<asp:TextBox ID="txtLcoCode" runat="server" MaxLength="10"></asp:TextBox>--%>
                            <asp:Label ID="lblLcoCode" runat="server"></asp:Label>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLcoCode"
                            ValidationGroup="lco" Display="none" ErrorMessage="Enter LCO Code"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtLcoCode"
                            FilterType="Numbers">
                        </cc1:FilteredTextBoxExtender>--%>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label4" runat="server" Text="LCO Name"></asp:Label>
                            <%--<asp:Label ID="Label45" runat="server" ForeColor="Red" Text="*"></asp:Label>--%>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <%--<asp:TextBox ID="txtLcoName" runat="server" MaxLength="100"></asp:TextBox>--%>
                            <asp:Label ID="lblLcoName" runat="server"></asp:Label>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLcoName"
                            ValidationGroup="lco" Display="none" ErrorMessage="Enter LCO Name"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtLcoName"
                            FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" -_.">
                        </cc1:FilteredTextBoxExtender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label6" runat="server" Text="First Name"></asp:Label>
                            <%--<asp:Label ID="Label59" runat="server" ForeColor="Red" Text="*"></asp:Label>--%>
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFirstName"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter First Name"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtFirstName"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" -_">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label8" runat="server" Text="Middle Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMidName" runat="server" MaxLength="100"></asp:TextBox>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMidName" 
                            ValidationGroup="lco" display="none" ErrorMessage="Enter Middle Name"></asp:RequiredFieldValidator>--%>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtMidName"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" -_">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label10" runat="server" Text="Last Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="100"></asp:TextBox>
                            <%-- <asp:RequiredFieldValidrator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtLastName" 
                            ValidationGroup="lco" display="none" ErrorMessage="Enter Last Name"></asp:RequiredFieldValidator> --%>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtLastName"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" -_">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblDirect" Text="Direct" runat="server"></asp:Label>
                            <asp:Label ID="Label57" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label39" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDirect" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtDirect"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Direct ID"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtDirect"
                                FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label31" Text="JV" runat="server"></asp:Label>
                            <asp:Label ID="Label32" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label36" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtJV" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtJV"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter JV ID"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtJV"
                                FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label38" Text="Ecs Status" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label41" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkecsstatus" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo">
                <table width="95%">
                    <tr>
                        <td align="left" colspan="6">
                            <b>
                                <asp:Label ID="Label26" runat="server" Text="Company Details"></asp:Label>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <hr />
                        </td>
                    </tr>
                    <tr align="left">
                        <td align="left" width="120px;">
                            <asp:Label ID="Label27" runat="server" Text="Company"></asp:Label>
                            <asp:Label ID="Label46" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label28" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlCompany" runat="server" Enabled="false" Width="140px">
                            </asp:DropDownList>
                        </td>
                        <td align="left" width="90px;">
                            <asp:Label ID="Label29" runat="server" Text="Distributor"></asp:Label>
                            <asp:Label ID="Label47" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label30" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <%-- <asp:DropDownList ID="ddlDist" runat="server" Width="110px">
                            <asp:ListItem>Select Distributor</asp:ListItem>
                            </asp:DropDownList>--%>
                            <asp:TextBox ID="txtDist" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDist"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Distributor"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtDist"
                                FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="120px;">
                            <asp:Label ID="Label33" runat="server" Text="Sub-Distributor"></asp:Label>
                            <asp:Label ID="Label48" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label34" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <%-- <asp:DropDownList ID="ddlSubDist" runat="server" Width="110px">
                                <asp:ListItem>Select Sub-Distributor</asp:ListItem>
                            </asp:DropDownList>--%>
                            <asp:TextBox ID="txtSubDist" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSubDist"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Sub-Distributor"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtSubDist"
                                FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo">
                <table width="95%">
                    <tr>
                        <td align="left" colspan="6">
                            <b>
                                <asp:Label ID="Label35" runat="server" Text="BRM Details"></asp:Label>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <%-- <td align="left" width="100px;">
                        <asp:Label ID="Label44" runat="server" Text="User ID"></asp:Label>
                        <asp:Label ID="Label49" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label37" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                         <asp:TextBox ID="txtUserId" runat="server" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtUserId"
                            ValidationGroup="lco" Display="none" ErrorMessage="Enter User ID"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtUserId"
                            FilterType="Numbers">
                        </cc1:FilteredTextBoxExtender> 
                    </td> --%>
                        <td align="left" width="100px;">
                            <asp:Label ID="Label42" runat="server" Text="BRM POID"></asp:Label>
                            <asp:Label ID="Label50" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label43" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtBrmId" runat="server" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtBrmId"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter BRM POID"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtBrmId"
                                FilterType="LowercaseLetters, Numbers, Custom" ValidChars="./ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" width="100px;">
                            <asp:Label ID="Lbuser" runat="server" Text="User Name"></asp:Label>
                            <asp:Label ID="Label49" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label67" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtUsername" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtUsername"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter User ID"></asp:RequiredFieldValidator>
                            <cc1:AutoCompleteExtender ServiceMethod="SearchUserName" MinimumPrefixLength="1"
                                CompletionInterval="100" EnableCaching="true" CompletionSetCount="3" TargetControlID="txtUsername"
                                FirstRowSelected="false" ID="AutoCompleteExtender2" runat="server" CompletionListCssClass="autocomplete"
                                CompletionListItemCssClass="autocompleteItem" CompletionListHighlightedItemCssClass="autocompleteItemHover"
                                CompletionListElementID="UserListHolder">
                            </cc1:AutoCompleteExtender>
                            <div id="UserListHolder" runat="server">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo">
                <table width="95%">
                    <tr>
                        <td align="left" colspan="9">
                            <b>
                                <asp:Label ID="Label12" runat="server" Text="Address Details"></asp:Label>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label13" runat="server" Text="Address"></asp:Label>
                            <asp:Label ID="Label51" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
                        </td>
                        <td colspan="7" align="left">
                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="500" Width="99%" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Address"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtAddress"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" -_.:,/">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label17" runat="server" Text="State"></asp:Label>
                            <asp:Label ID="Label52" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label18" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlState" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label15" runat="server" Text="City"></asp:Label>
                            <asp:Label ID="Label53" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label16" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel runat="server" ID="uplCity">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlCity" runat="server" Enabled="false">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlState" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label24" runat="server" Text="Pincode"></asp:Label>
                            <asp:Label ID="Label54" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label25" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPincode" runat="server" MaxLength="6" Width="45px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtPincode"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Pincode"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtPincode"
                                FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo">
                <table width="95%">
                    <tr>
                        <td align="left" colspan="6">
                            <b>
                                <asp:Label ID="Label19" runat="server" Text="Contact Details"></asp:Label>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="100px;">
                            <asp:Label ID="Label20" runat="server" Text="Mobile No."></asp:Label>
                            <asp:Label ID="Label55" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label21" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMobile" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtMobile"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Mobile"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtMobile"
                                FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label22" runat="server" Text="Email"></asp:Label>
                            <asp:Label ID="Label56" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtEmail"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Email"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtEmail"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars="@._">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo" style="text-align: center;">
                <asp:Button ID="btnCancel" CssClass="button" runat="server" Text="Reset" UseSubmitBehavior="false"
                    OnClick="btnCancel_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" UseSubmitBehavior="true"
                    OnClick="btnSubmit_Click" ValidationGroup="lco" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="lco"
                    ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
