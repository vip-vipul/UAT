<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="FrmAutoEcsLcoConfig.aspx.cs" Inherits="PrjUpassPl.Master.FrmAutoEcsLcoConfig" %>

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
        .style67
        {
            width: 132px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript" language="javascript"><a href="../Transaction/">../Transaction/</a>
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
                   
                    <tr>
                        <td align="left" width="90px;">
                            <asp:Label ID="Label2" runat="server" Text="LCO Code"></asp:Label>
                          
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            
                            <asp:Label ID="lblLcoCode" runat="server"></asp:Label>
                        
                        </td>
                        <td align="left">
                            <asp:Label ID="Label4" runat="server" Text="LCO Name"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            
                            <asp:Label ID="lblLcoName" runat="server"></asp:Label>
                         
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label6" runat="server" Text="First Name"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                           
                            <asp:Label ID="LblFristName" runat="server" Text="First Name"></asp:Label>
                           
                        </td>
                        <td align="left">
                            <asp:Label ID="Label8" runat="server" Text="Middle Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="LblMidName" runat="server" Text="Middle Name"></asp:Label>
                          
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
                            <asp:Label ID="LblLastname" runat="server" Text="Last Name"></asp:Label>
                           
                        </td>
                        <td align="left">
                            <asp:Label ID="lblDirect" Text="Direct" runat="server"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="Label39" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                         <asp:Label ID="lblDirectNos" runat="server" Text="Direct"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label31" Text="JV" runat="server"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="Label36" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                        <asp:Label ID="LblJvNos" runat="server" Text="JV"></asp:Label>
                        </td>
                        <td align="left">
                            
                        </td>
                        <td>
                            
                        </td>
                        <td align="left">
                            
                        </td>
                    </tr>
                </table>
            </div>
          
      <div id="div1" runat="server">
            <div class="delInfo">
                <table width="95%">
                   
                  
                   
                   
                    <tr>
                        <td align="left" class="style67">
                            <asp:Label ID="Label38" Text="Ecs Status" runat="server"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="Label20" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                           
                            <asp:CheckBox ID="chkecsstatus" runat="server" />
                           
                        </td>
                        <td align="left">
                            
                        </td>
                        <td>
                            
                        </td>
                        <td align="left">
                           
                          
                        </td>
                    </tr>
                
                  
                </table>
            </div>
          
    
         
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

