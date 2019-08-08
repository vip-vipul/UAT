<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransHwayUserCreditLimitRev.aspx.cs"
    MasterPageFile="~/MasterPage.Master" Inherits="PrjUpassPl.Transaction.TransHwayUserCreditLimitRev" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .topHead
        {
            background: #E5E5E5;
        }
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            margin: 10px;
            width: 75%;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .scroller
        {
            overflow: auto;
            max-height: 250px;
        }
        .completionList
        {
            border: solid 1px Gray;
            margin: 0px;
            padding: 3px;
            height: 120px;
            overflow: auto;
            background-color: #FFFFFF;
        }
        .listItem
        {
            color: #191919;
        }
        .itemHighlighted
        {
            background-color: #ADD6FF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
    <div>
        <table cellspacing="0" cellpadding="0" align="center" border="0" width="80%">
            <tr>
                <td align="center">
                    <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="center">
                                <div class="delInfo">
                                    <table runat="server" align="center" width="100%" id="tbl1" border="0">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblUser" runat="server" Text="Search User By"></asp:Label></asp:Label><asp:Label ID="Label6" runat="server" Text="*" ForeColor="red"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left" class="style67">
                                                <asp:RadioButtonList ID="RadSearchby" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                                    OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Selected="True">ID</asp:ListItem>
                                                    <asp:ListItem Value="1">Name</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLCOSearch" runat="server" Style="resize: none;" Width="200px"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1"
                                                    CompletionInterval="100" EnableCaching="true" CompletionSetCount="3" TargetControlID="txtLCOSearch"
                                                    FirstRowSelected="false" ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="autocomplete"
                                                    CompletionListItemCssClass="autocompleteItem" CompletionListHighlightedItemCssClass="autocompleteItemHover">
                                                </cc1:AutoCompleteExtender>
                                                <asp:HiddenField ID="hfUserId" runat="server" />
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divdet" runat="server">
                                    <div class="delInfo">
                                        <table class="delInfoContent">
                                            <tr>
                                                <td colspan="3" align="left">
                                                    <b>
                                                        <asp:Label runat="server" ID="Label5" Text="User Details:"></asp:Label>
                                                    </b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label21" Text="User ID"></asp:Label>
                                                </td>
                                                <td width="10px">
                                                    <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblUserId"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label12" Text="Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblUserName"></asp:Label>
                                                </td>
                                            </tr>
                                            <%--  <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label6" Text="Address"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label8" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblUserAddr" Text="38 C, LUKARGANJ, Mumbai, Maharashtra"></asp:Label>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label15" Text="Mobile No."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label2" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblmobno"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="130px">
                                                    <asp:Label runat="server" ID="Label7" Text="Current balance"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label19" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblCurLimit"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="delInfo">
                                        <table width="96%" cellpadding="2">
                                            <tr>
                                                <td colspan="6" align="left">
                                                    <b>
                                                        <asp:Label runat="server" ID="Label14" Text="Balance Reversal:"></asp:Label>
                                                    </b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="100px" align="left" class="style68">
                                                    <asp:Label ID="Label9" runat="server" Text="Reverse Balance"></asp:Label><asp:Label ID="Label8" runat="server" Text="*" ForeColor="red"></asp:Label>
                                                </td>
                                                <td class="style68">
                                                    <asp:Label ID="Label10" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td align="left" class="style68">
                                                    <asp:TextBox ID="txtNewLimit" runat="server" Style="resize: none;" Width="180px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNewLimit"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="100px" align="left" class="style68">
                                                    <asp:Label ID="Label1" runat="server" Text="Remark"></asp:Label><asp:Label ID="Label16" runat="server" Text="*" ForeColor="red"></asp:Label>
                                                </td>
                                                <td class="style68">
                                                    <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td align="left" class="style68">
                                                    <asp:TextBox ID="txtremark" runat="server" Style="resize: none;" Width="250px" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="delInfo">
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnCancel" TabIndex="2" runat="server" Font-Bold="True" Text="Cancel"
                                                        class="button" Width="60" Height="20px" OnClick="btnCancel_Click"></asp:Button>
                                                    &nbsp;&nbsp;
                                                    <asp:Button ID="btnSubmit" TabIndex="2" runat="server" Font-Bold="True" Text="Submit"
                                                        class="button" Width="60" Height="20px" OnClick="btnSubmit_Click"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
