<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmLCOPreMSOUserDefine.aspx.cs" Inherits="PrjUpassPl.Master.frmLCOPreMSOUserDefine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .delInfo
        {
            /*padding: 10px;
            border: 1px solid #094791;*/
            width: 700px;
            margin: 5px;
            padding: 10px;
            border: 1px solid #094791;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .style67
        {
            width: 123px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    
    <asp:Panel runat="server" ID="pnlRegisterLCO">
        <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
        <table width="60%">
            <tr>
                <td align="center">
                    <div class="delInfo" id="divsearchLco" runat="server">
                        <table runat="server" align="center" width="100%" id="tbl1" border="0">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblUser" runat="server" Text="Search By Company Name"></asp:Label>
                                    <asp:Label ID="Label59" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                                </td>
                                <td colspan="2" align="right">
                                    <asp:TextBox ID="txtCompanySearch" runat="server" Style="resize: none;" Width="150px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1" 
                                        CompletionInterval="100" EnableCaching="true" CompletionSetCount="3" TargetControlID="txtCompanySearch"
                                        FirstRowSelected="false" ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="autocomplete"
                                        CompletionListItemCssClass="autocompleteItem" CompletionListHighlightedItemCssClass="autocompleteItemHover">
                                    </cc1:AutoCompleteExtender>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                        ValidationGroup="S" />
                                    <asp:RequiredFieldValidator ID="RequiredField1" runat="server" ControlToValidate="txtCompanySearch"
                                        ValidationGroup="S" ErrorMessage="Please Enter Company Name"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--<div id="divdet" runat="server">--%>
                    <asp:Panel runat="server" ID="pnlDetails">
                        <div class="delInfo">
                            <table class="delInfoContent">
                                <tr>
                                    <td colspan="3" align="left">
                                        <b>
                                            <asp:Label runat="server" ID="Label5" Text="Company Details:"></asp:Label>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label runat="server" ID="Label12" Text="Company Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblCompanyName" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="delInfo">
                            <table class="delInfoContent">
                                <tr>
                                    <td colspan="6" align="left">
                                        <b>
                                            <asp:Label runat="server" ID="Label30" Text="User Details:"></asp:Label>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="100">
                                        <asp:Label ID="Label26" runat="server" Text="Login ID"></asp:Label>
                                        <asp:Label ID="Label58" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtUserId" runat="server" Style="resize: none;" MaxLength="25"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserId"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter User ID"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtUserId"
                                            FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars="_">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label6" runat="server" Text="First Name"></asp:Label>
                                        <asp:Label ID="Label34" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFirstName" runat="server" Style="resize: none;" MaxLength="100"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFirstName"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter First Name"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtFirstName"
                                            FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" -_">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label8" runat="server" Text="Middle Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtMidName" runat="server" Style="resize: none;" MaxLength="100"></asp:TextBox>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMidName"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter Middle Name"></asp:RequiredFieldValidator>--%>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtMidName"
                                            FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" -_">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label10" runat="server" Text="Last Name"></asp:Label>
                                        <asp:Label ID="Label38" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtLastName" runat="server" Style="resize: none;" MaxLength="100"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtLastName"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter Last Name"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtLastName"
                                            FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" -_">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:RadioButton ID="rdDirect" runat="server" Checked="true" GroupName="a" Text="Direct" />
                                        <asp:RadioButton ID="rdJV" runat="server" GroupName="a" Text="JV" />
                                        <asp:Label ID="Label39" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtJvDirNo" runat="server" Style="resize: none;" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtJvDirNo"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter Direct Or JV ID"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtJvDirNo"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td colspan="3" align="left">
                                        <asp:RadioButton ID="rbtnCashier" runat="server" Checked="true" GroupName="b" Text="Cashier" />
                                        <asp:RadioButton ID="rbtnFinance" runat="server" GroupName="b" Text="Finance" />
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
                                    <td align="left" width="90px;">
                                        <asp:Label ID="Label44" runat="server" Text="BRM POID"></asp:Label>
                                        <asp:Label ID="Label40" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label37" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtBrmId" runat="server" Style="resize: none;" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtBrmId"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter BRM POID"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtBrmId"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="delInfo">
                            <table class="delInfoContent">
                                <tr>
                                    <td align="left" colspan="9">
                                        <b>
                                            <asp:Label ID="Label2" runat="server" Text="Address Details"></asp:Label>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="90px">
                                        <asp:Label ID="Label4" runat="server" Text="Address"></asp:Label>
                                        <asp:Label ID="Label46" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td colspan="7" align="left">
                                        <asp:TextBox ID="txtAddress" runat="server" Style="resize: none;" MaxLength="500"
                                            Width="99%" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter Address"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtAddress"
                                            FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=" -_.:,/">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="90px">
                                        <asp:Label ID="Label17" runat="server" Text="State"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlState" runat="server" Width="130px" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" width="90px">
                                        <asp:Label ID="Label15" runat="server" Text="City"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel runat="server" ID="uplCity">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCity" runat="server" Width="130px" Enabled="false">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlState" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="left" width="100px">
                                        <asp:Label ID="Label24" runat="server" Text="Pincode"></asp:Label>
                                        <asp:Label ID="Label49" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label25" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtPincode" runat="server" Style="resize: none;" MaxLength="6" Width="45px"></asp:TextBox>
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
                            <table class="delInfoContent">
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
                                        <asp:Label ID="Label50" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label45" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtMobile" runat="server" Style="resize: none;" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtMobile"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter Mobile"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtMobile"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label22" runat="server" Text="Email"></asp:Label>
                                        <asp:Label ID="Label51" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtEmail" runat="server" Style="resize: none;"></asp:TextBox>
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
                        <div class="delInfo">
                            <asp:Button ID="btnCancel" CssClass="button" runat="server" Text="Reset" UseSubmitBehavior="false"
                                OnClick="btnCancel_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" UseSubmitBehavior="true"
                                OnClick="btnSubmit_Click" ValidationGroup="lco" />
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="lco"
                                ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" />
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
