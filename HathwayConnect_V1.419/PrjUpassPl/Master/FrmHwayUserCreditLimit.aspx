<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmHwayUserCreditLimit.aspx.cs"
Inherits="PrjUpassPl.Master.FrmHwayUserCreditLimit" MasterPageFile="~/MasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Distributor Balance</title>
    <link href="../CSS/main.css" rel="stylesheet" type="text/css" />
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
            width: 60%;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript" language="javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
        }
    </script>
    <asp:Panel ID="pnlView" runat="server" ScrollBars="Auto">
        <div>
            <%--  <table cellspacing="0" cellpadding="0" align="center" border="0">
                <tr>
                    <td align="center"> --%>
            <%--   <table align="center" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left"> --%>
            
            <div class="delInfo">
                <table id="Table1" runat="server" align="center" width="100%" border="0">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblDist" runat="server" Text="Search USER By"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="RadSearchby" AutoPostBack="true" runat="server" 
                                RepeatDirection="Horizontal" 
                                onselectedindexchanged="RadSearchby_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Code</asp:ListItem>
                                <asp:ListItem Value="1">Name</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtUSERSearch" runat="server" Width="200px"></asp:TextBox>
                            &nbsp;
                            <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1"
                                CompletionInterval="100" EnableCaching="true" CompletionSetCount="3" TargetControlID="txtUSERSearch"
                                FirstRowSelected="false" ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="autocomplete"
                                CompletionListItemCssClass="autocompleteItem" CompletionListHighlightedItemCssClass="autocompleteItemHover"
                                OnClientItemSelected="ClientItemSelected">
                            </cc1:AutoCompleteExtender>
                            <asp:HiddenField ID="hfCustomerId" runat="server" />
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divdet" runat="server">
                <div class="delInfo" id="USERdet">
                    <table width="100%">
                        <tr>
                            <td colspan="3" align="left">
                                <b>
                                    <asp:Label runat="server" ID="Label4" Text="USER Details:"></asp:Label>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="80px">
                                <asp:Label runat="server" ID="Label21" Text="USER Code"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblCustNo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="80px">
                                <asp:Label runat="server" ID="Label12" Text="Name"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblCustName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="80px">
                                <asp:Label runat="server" ID="Label5" Text="Address"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label6" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblCustAddr"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="80px">
                                <asp:Label runat="server" ID="Label15" Text="Mobile No."></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label16" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblmobno"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="80px">
                                <asp:Label runat="server" ID="Label18" Text="Email"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label19" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblEmail"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="delInfo" id="Paydet">
                    <table width="96%" cellpadding="2">
                        <tr>
                            <td colspan="6" align="left">
                                <b>
                                    <asp:Label runat="server" ID="Label14" Text="Payment Details:"></asp:Label>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table width="96%">
                        <tr>
                            <td width="120px" align="left">
                                <asp:Label ID="lblCashAmt" runat="server" Text="Amount"></asp:Label>
                            </td>
                            <td width="5px">
                                <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtCashAmt" runat="server" Style="resize: none;" Width="180px"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtCashAmt"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <table width="96%">
                        <tr id="tdmode" runat="server">
                            <td align="left" width="120px">
                                <asp:Label ID="lbldepmode" runat="server" Text="Payment Mode"></asp:Label>
                            </td>
                            <td width="5px">
                                <asp:Label ID="clndepmode" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" width="180px">
                                <asp:RadioButtonList ID="RBPaymode" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="C">Cash</asp:ListItem>
                                    <asp:ListItem Value="Q" Selected="True">Cheque</asp:ListItem>
                                    <asp:ListItem Value="DD">DD</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtChqDDno" runat="server" placeholder="Cheque No."></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtWatermark" runat="server" TargetControlID="txtChqDDno"
                                    WatermarkText="Cheque/DD Number" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtChqDDno"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <table width="96%">
                        <tr id="tdbnnm" runat="server">
                            <td align="left" width="120px">
                                <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                            </td>
                            <td width="5px">
                                <asp:Label ID="ClnBankName" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlBankName" runat="server" Style="resize: none;">
                                    <asp:ListItem Text="Select Bank Name" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Axis Bank" Value="Axis Bank"></asp:ListItem>
                                    <asp:ListItem Text="Ratnakar Bank" Value="Ratnakar Bank"></asp:ListItem>
                                    <asp:ListItem Text="Kotak Bank" Value="Kotak Bank"></asp:ListItem>
                                    <asp:ListItem Text="Yes Bank" Value="Yes Bank"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblbranchnm" runat="server" Text="Branch Name"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtbranchnm" runat="server" Style="resize: none;" Width="180px"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender188" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom"
                                    TargetControlID="txtbranchnm" ValidChars=" -_">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <table width="96%">
                        <tr id="Tr1" runat="server">
                            <td align="left" width="120px">
                                <asp:Label ID="lblReferenceNo" runat="server" Text="Remark"></asp:Label>
                            </td>
                            <td width="5px">
                                <asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" colspan="4">
                                <asp:TextBox ID="txtRemark" runat="server" Style="resize: none;" Width="480px" TextMode="MultiLine"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom"
                                    TargetControlID="txtRemark" ValidChars=" -_">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Label ID="lblmsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="delInfo">
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnCancel" TabIndex="2" runat="server" Font-Bold="True" Text="Cancel"
                                    class="button" Width="60" onclick="btnCancel_Click"></asp:Button>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnSubmit" TabIndex="2" runat="server" Font-Bold="True" Text="Submit"
                                    class="button" Width="60"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <%--  </td>
                            </tr>
                        </table>--%>
            <%--   </td>
                </tr>
            </table> --%>
        </div>
    </asp:Panel>
</asp:Content>
