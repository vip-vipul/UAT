<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmLCODetailsUpdate.aspx.cs" Inherits="PrjUpassPl.Master.frmLCODetailsUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .delInfo
        {
            /*padding: 10px;
            border: 1px solid #094791;*/
            width: 95%;
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
    <script type="text/javascript" language="javascript">
        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey(parseInt('<%= RadSearchby.SelectedValue %>'));
        }
    </script>
    <asp:Panel runat="server" ID="pnlRegisterLCO">
        <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
        <table width="700px">
            <tr>
                <td align="center">
                    <div class="delInfo" id="divsearchLco" runat="server">
                        <table runat="server" align="center" width="500px" id="tbl1" border="0">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblUser" runat="server" Text="Search LCO By"></asp:Label>
                                    <asp:Label ID="Label59" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="RadSearchby" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"><%--OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged1"--%>
                                        <asp:ListItem Value="0" Selected="True">CODE</asp:ListItem>
                                        <asp:ListItem Value="1">Name</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtLCOSearch" runat="server" onkeydown = "SetContextKey()" Style="resize: none;" Width="150px"></asp:TextBox>
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
                                <tr>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label1" Text="Address"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label2" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="5">
                                        <asp:Label runat="server" ID="lblAddress" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label4" Text="JV"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label6" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblJV" Text=""></asp:Label>
                                    </td>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label8" Text="Direct"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label9" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblDirect" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label7" Text="Distributor"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label10" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblDistributor" Text=""></asp:Label>
                                    </td>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label15" Text="Sub Distributor"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label16" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblSubDistributor" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label14" Text="State"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label17" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblState" Text=""></asp:Label>
                                    </td>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label24" Text="City"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label25" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblCity" Text=""></asp:Label>
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
                                        <asp:Label ID="Label50" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label45" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtMobile" runat="server" Style="resize: none;" MaxLength="10"></asp:TextBox>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtMobile"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter Mobile"></asp:RequiredFieldValidator>--%>
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
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtEmail"
                                            ValidationGroup="lco" Display="none" ErrorMessage="Enter Email"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
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
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
