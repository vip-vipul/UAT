<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="mstHwayLCOMsgBroadcast.aspx.cs" Inherits="PrjUpassPl.Master.mstHwayLCOMsgBroadcast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 570px;
            margin: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript" language="javascript">
        function SelectedLcoOperId(sender, e) {
            $get("<%=hdnLcoOperId.ClientID %>").value = e.get_value();
            $get("<%=lblSearchedValue.ClientID %>").innerHTML = e.get_text();
            var selected_rad = "";
            var radioButtonList = document.getElementById("<%=RadSearchby.ClientID%>");
            var listItems = radioButtonList.getElementsByTagName("input");
            for (var i = 0; i < listItems.length; i++) {
                if (listItems[i].checked) {
                    selected_rad = listItems[i].value;
                }
            }
            if (selected_rad == "0") {
                $get("<%=lblSearchType.ClientID %>").innerHTML = "Code";
            } else {
                $get("<%=lblSearchType.ClientID %>").innerHTML = "Name";
            }
            $get("pnlLcoDetails").style.display = "block";
        }

        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey(parseInt('<%= RadSearchby.SelectedValue %>'));
        }
        
    </script>
    <asp:Panel runat="server" ID="pnlLCOMsg">
        <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
        <div class="delInfo">
            <table runat="server" align="center" width="500px" id="Table1" border="0">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblUser" runat="server" Text="Search LCO By"></asp:Label>
                        <asp:Label ID="Label37" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        <asp:Label ID="Label44" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="RadSearchby" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" >
                            <asp:ListItem Value="0" Selected="True">Code</asp:ListItem>
                            <asp:ListItem Value="1">Name</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLCOSearch" runat="server" Style="resize: none;" Width="150px" onkeydown = "SetContextKey()"></asp:TextBox>
                        <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1" OnClientItemSelected="SelectedLcoOperId"
                            CompletionInterval="100" EnableCaching="true" CompletionSetCount="3" TargetControlID="txtLCOSearch" UseContextKey = "true"
                            FirstRowSelected="false" ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="autocomplete"
                            CompletionListItemCssClass="autocompleteItem" CompletionListHighlightedItemCssClass="autocompleteItemHover"
                            CompletionListElementID="LcoListHolder">
                        </cc1:AutoCompleteExtender>
                        <div id="LcoListHolder" runat="server">
                        </div>
                        <asp:HiddenField ID="hdnLcoOperId" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="pnlLcoDetails" class="delInfo" style="display:none;">
            <table width="100%">
                <tr>
                    <td colspan="3" align="left">
                        <b>Seached LCO</b>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />   
                    </td>
                </tr>
                <tr>
                    <td width="50px" align="left">
                        <asp:Label ID="lblSearchType" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblSearchedValue" Font-Bold="true" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="delInfo">
            <table runat="server" align="center" width="100%" id="tbl1" border="0">
                <tr>
                    <td align="left" width="150px">
                        <asp:Label ID="Label1" runat="server" Text="Brodcast Message"></asp:Label>
                        <%--<asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>--%>
                    </td>
                    <td align="center">
                        <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtBrodcastermsg" TextMode="MultiLine" Width="200px" MaxLength="1024"
                            runat="server"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="1"
                            ControlToValidate="txtBrodcastermsg" ErrorMessage="Message cannot be blank" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="150px">
                        <asp:Label ID="Label3" runat="server" Text="Message Type"></asp:Label>
                        <asp:Label ID="Label6" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="rbtnFlag" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Disable Earlier Message" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Concat To Earlier Message" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="1" Text="Submit" 
                            onclick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
    </asp:Panel>
</asp:Content>
