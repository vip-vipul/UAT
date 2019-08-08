<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master"
    CodeBehind="transRupassDistLimit.aspx.cs" Inherits="PrjUpassPl.Transaction.transRupassDistLimit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Distributor Balance</title>
    <link href="../Menu/menu.css" type="text/css" rel="stylesheet" />
    <link href="../Stylesheet/stylSVC.css" type="text/css" rel="Stylesheet" />
    <link href="../Stylesheet/stylesheet_moc.css" type="text/css" rel="Stylesheet" />
    <link href="../CSS/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
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
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <script language="javascript">
        function dovalid1() {
            //	    if (document.getElementById("<%=txtCashAmt.ClientID %>").value = "")
            //		{
            //			alert("Please Enter Amount");
            //			document.getElementById("<%=txtCashAmt.ClientID %>").focus();
            //			return false;
            //		}

            //		if (isNaN(document.getElementById("<%=txtCashAmt.ClientID %>").value))
            //		{
            //			alert("Please Enter Proper Amount");
            //			document.getElementById("<%=txtCashAmt.ClientID %>").select();
            //			return false;
            //		}	
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
            <div class="delInfo topHead">
                <table class='delInfoContent'>
                    <tr>
                        <td align="right" width="130px">
                            Distributor Name:
                        </td>
                        <td align="left" width="180px">
                            <asp:Label ID="lblDistName" runat="server" Text="Mahim Cable"></asp:Label>
                        </td>
                        <td align="right" width="50px">
                            User:
                        </td>
                        <td align="left">
                            <asp:Label ID="Label22" runat="server" Text="Keshaw Rane"></asp:Label>
                        </td>
                        <td align="right" width="130px">
                            Available Balance:
                        </td>
                        <td align="left" width="67px">
                            <asp:Label ID="lblAvailBal" runat="server" Text="10000"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div class="delInfo">
                <table runat="server" align="center" width="100%" border="0">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblDist" runat="server" Text="Search LCO By"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0">Code</asp:ListItem>
                                <asp:ListItem Value="1">Name</asp:ListItem>
                                <asp:ListItem Value="2">Email</asp:ListItem>
                                <asp:ListItem Value="3">Mobile No.</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLCOSearch" runat="server" Style="resize: none;" Width="180px"></asp:TextBox>
                            <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1"
                                CompletionInterval="100" EnableCaching="true" CompletionSetCount="3" TargetControlID="txtLCOSearch"
                                FirstRowSelected="false" ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="completionList"
                                CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo">
                <table width="100%">
                    <tr>
                        <td colspan="3" align="left">
                            <b>
                                <asp:Label runat="server" ID="Label4" Text="LCO Details:"></asp:Label>
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
                            <asp:Label runat="server" ID="Label21" Text="LCO Code"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label runat="server" ID="lblCustNo" Text="1013426081"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label runat="server" ID="Label12" Text="LCO Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label runat="server" ID="lblCustName" Text="ranjan singh"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="130px">
                            <asp:Label runat="server" ID="Label5" Text="LCO Address"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label6" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label runat="server" ID="lblCustAddr" Text="38 C, LUKARGANJ, Mumbai, Maharashtra"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="130px">
                            <asp:Label runat="server" ID="Label15" Text="Mobile No."></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label16" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label runat="server" ID="Label17" Text="8087381812"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="130px">
                            <asp:Label runat="server" ID="Label18" Text="Email"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label19" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label runat="server" ID="Label20" Text="ranjan.singh@gmail.com"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo">
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
                    <tr>
                        <td width="100px" align="left" class="style68">
                            <asp:Label ID="lblCashAmt" runat="server" Text="Amount"></asp:Label>
                        </td>
                        <td class="style68">
                            <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left" class="style68">
                            <asp:TextBox ID="txtCashAmt" runat="server" Style="resize: none;" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="ERP Receipt No"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text=":"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" Style="resize: none;" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tdmode" runat="server">
                        <td align="left" width="100px">
                            <asp:Label ID="lbldepmode" runat="server" Text="Payment Mode"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="clndepmode" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:RadioButton ID="radCash" runat="server" GroupName="grpPayMode" Text="Cash" AutoPostBack="True" />
                            <asp:RadioButton ID="radCheque" runat="server" GroupName="grpPayMode" Text="Cheque"
                                AutoPostBack="True" Checked="True" />
                            <asp:RadioButton ID="radNEFT" runat="server" GroupName="grpPayMode" Text="DD" AutoPostBack="True" />
                            <asp:RadioButton ID="radRTGS" runat="server" GroupName="grpPayMode" Text="RTGS" AutoPostBack="True"
                                Visible="false" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="Label10" runat="server" placeholder="Cheque No."></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tdbnnm" runat="server">
                        <td align="left">
                            <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                        </td>
                        <td>
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
                            <asp:TextBox ID="txtBankName" Visible="false" runat="server" Style="resize: none;"
                                Width="180px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                TargetControlID="txtBankName" ValidChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-">
                            </cc1:FilteredTextBoxExtender>
                            <asp:CheckBox ID="ckhother" Text="Others" AutoPostBack="true" runat="server" Visible="False" />
                        </td>
                        <td align="left">
                            <asp:Label ID="lblbranchnm" runat="server" Text="Branch Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtbranchnm" runat="server" Style="resize: none;" Width="180px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender188" runat="server" FilterType="Custom"
                                TargetControlID="txtbranchnm" ValidChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr id="Tr1" runat="server">
                        <td align="left">
                            <asp:Label ID="lblReferenceNo" runat="server" Text="Remark"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtReferenceNo" runat="server" Style="resize: none;" Width="98%"
                                TextMode="MultiLine"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                TargetControlID="txtReferenceNo" ValidChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-">
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
                                class="button" Width="60"></asp:Button>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSubmit" TabIndex="2" runat="server" Font-Bold="True" Text="Submit"
                                class="button" Width="60"></asp:Button>
                        </td>
                    </tr>
                </table>
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
