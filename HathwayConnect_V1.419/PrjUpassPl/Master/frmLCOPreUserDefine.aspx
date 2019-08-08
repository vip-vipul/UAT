<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLCOPreUserDefine.aspx.cs"
    Inherits="PrjUpassPl.Master.frmLCOPreUserDefine" MasterPageFile="~/MasterPage.Master" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            margin: 10px;
            
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
    <script type="text/javascript" language="javascript">
      function back()
    {
    
        window.location.href="../Master/mstLCOAdminPages.aspx";
        return false;
    }
        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey(parseInt('<%= RadSearchby.SelectedValue %>'));
        }
        function goBack() {
            window.history.back();
        }
    </script>
    
        <asp:Panel runat="server" ID="pnlRegisterLCO">
        
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblResponseMsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="maindive">
            <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
            <table>
                <tr>
               
                    <td align="center">
                     <div class="griddiv">
                        <div class="delInfo" id="divsearchLco" runat="server">
                        
                            <table runat="server" align="center" width="500px" id="tbl1" border="0">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblUser" runat="server" Text="Search LCO By"></asp:Label>
                                        <asp:Label ID="Label59" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="RadSearchby" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true">
                                            <%--OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged1"--%>
                                            <asp:ListItem Value="0">CODE</asp:ListItem>
                                            <asp:ListItem Value="1" Selected="True">Name</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtLCOSearch" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                            Width="150px"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1"
                                            UseContextKey="true" CompletionInterval="100" EnableCaching="true" CompletionSetCount="3"
                                            TargetControlID="txtLCOSearch" FirstRowSelected="false" ID="AutoCompleteExtender1"
                                            runat="server" CompletionListCssClass="autocomplete" CompletionListItemCssClass="autocompleteItem"
                                            CompletionListHighlightedItemCssClass="autocompleteItemHover">
                                        </cc1:AutoCompleteExtender>
                                        <asp:HiddenField ID="hfLCOCode" runat="server" />
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                            
                        </div>
                        <%--<div id="divdet" runat="server">--%>
                        <asp:Panel runat="server" ID="pnlDetails">
                            <div class="delInfo">
                            
                                <table width="97%">
                                    <tr>
                                        <td colspan="6" align="left">
                                            <b>
                                                <asp:Label runat="server" ID="Label5" Text="LCO Details:"></asp:Label>
                                            </b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" width="80px">
                                            <asp:Label runat="server" ID="Label21" Text="LCO Code"></asp:Label>
                                        </td>
                                        <td width="10px">
                                            <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label runat="server" ID="lblLCOCode" Text=""></asp:Label>
                                        </td>
                                        <td align="left" width="80px">
                                            <asp:Label runat="server" ID="Label12" Text="LCO Name"></asp:Label>
                                        </td>
                                        <td width="10px">
                                            <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label runat="server" ID="lblLCOName" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                
                            </div>
                            <div class="delInfo">
                                <table width="97%">
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
                                        <td align="left">
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
                                        <%--<td align="left">
                                        <asp:Label ID="Label28" runat="server" Text="User Name"></asp:Label>
                                        <asp:Label ID="Label33" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label29" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtUserName" runat="server" Style="resize: none;" MaxLength="100"
                                            BorderWidth="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUserName"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter User Name"></asp:RequiredFieldValidator>
                                       
                                    </td>--%>
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
                                            <asp:Label ID="Label29" runat="server" Text="JV No."></asp:Label>
                                            <asp:Label ID="Label31" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label32" runat="server" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtJvNo" runat="server" Style="resize: none;" MaxLength="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtJvNo"
                                                ValidationGroup="lco" Display="none" ErrorMessage="Enter JV ID"></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtJvNo"
                                                FilterType="Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label28" runat="server" Text="Direct No."></asp:Label>
                                            <asp:Label ID="Label33" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label36" runat="server" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtDirectNo" runat="server" Style="resize: none;" MaxLength="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDirectNo"
                                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Direct ID"></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtDirectNo"
                                                FilterType="Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="delInfo">
                                <table width="97%">
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
                                        <td align="left" width="90px;">
                                            <asp:Label ID="Label42" runat="server" Text="Account No"></asp:Label>
                                            <asp:Label ID="Label41" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label43" runat="server" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtAccNO" runat="server" Style="resize: none;" MaxLength="100"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtAccNO"
                                                ValidationGroup="lco" Display="none" ErrorMessage="Enter Account No"></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtBrmId"
                                                FilterType="LowercaseLetters, Numbers, Custom" ValidChars="./ ">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="delInfo">
                                <table width="97%">
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
                                            <asp:Label ID="Label47" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" Width="130px"
                                                OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" width="90px">
                                            <asp:Label ID="Label15" runat="server" Text="City"></asp:Label>
                                            <asp:Label ID="Label48" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:UpdatePanel runat="server" ID="uplCity">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlCity" runat="server" Width="130px">
                                                        <asp:ListItem Value="0">Select City</asp:ListItem>
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
                                <table width="97%">
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
                                            <asp:Label ID="Label51" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtEmail" runat="server" Style="resize: none;"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtEmail"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter Email"></asp:RequiredFieldValidator>--%>
                                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>--%>
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
                        </div>
                    </td>
                    
                </tr>
            </table>
            
            </div>
            
        </asp:Panel>
    
</asp:Content>
