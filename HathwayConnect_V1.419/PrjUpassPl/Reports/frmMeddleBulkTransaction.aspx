<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmMeddleBulkTransaction.aspx.cs" Inherits="PrjUpassPl.Reports.frmMeddleBulkTransaction" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">

     function ShowModalPopup() {
         document.getElementById('<%=btnRefresh.ClientID %>').click();
     }

     function goBack() {
         window.location.href = "../Transaction/TransHwayBulkCustumerAddEcaf.aspx";
         return false;
     }
    </script>
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
        .nontrColor
        {
            border: 0px solid #094791;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
  <asp:UpdatePanel runat="server" ID="GridBulkStstus">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlRegisterLCO">
                <div class="maindive">
                    <div style="float: right">
                        <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                            Back</button>
                    </div>
                    <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
                    <div id="divtextbox" class="delInfo1" runat="server">
                        <table runat="server" align="center" width="700px" id="tbl1" border="0">
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Enter Bulk Unique ID"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtUniqueid" runat="server" Width="80%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnSearch" runat="server" ValidationGroup="1" Text="Search" OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Label ID="lblerrormsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Button ID="Button1" runat="server" ValidationGroup="1" Text="Search" Style="display: none"
                        OnClick="btnSubmit_Click" />
                    <div id="DivShowValue" runat="server">
                        <table class="Grid">
                            <tr>
                                <th>
                                    Unique Id
                                </th>
                                <th>
                                    Total Transactions
                                </th>
                                <th>
                                    Successful Transactions
                                </th>
                                <th>
                                    Failed Transactions
                                </th>
                                <th>
                                    Pending Transactions
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSumFile" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lblSumTotal" runat="server" Text="0" OnClick="lblSumTotal_Click"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lblSumSuccess" runat="server" Text="0" OnClick="lblSumSuccess_Click"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lblSumFailure" runat="server" Text="0" OnClick="lblSumFailure_Click"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lblRemaing" runat="server" Text="0" OnClick="lblRemaing_Click"></asp:LinkButton>
                                </td>
                            </tr>
                          
                        </table>
                       <table>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="Label4" runat="server" Text="Page will be refreshed automatically after 30 second..."></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" 
                                        onclick="btnRefresh_Click">
                                    </asp:Button>
                                </td>
                            </tr>
                            
                        </table>
                    </div>
                    <div class="griddiv">
                        <div id="DivGried" runat="server">
                            <asp:Label ID="lbSearchMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            <asp:GridView runat="server" ID="grdBulkstatus" CssClass="Grid" AutoGenerateColumns="false"
                                OnPageIndexChanging="grdBulkstatus_PageIndexChanging" ShowFooter="true" AllowPaging="true"
                                PageSize="10">
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Unique ID" DataField="uniqueID" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="STB No" DataField="STBNO" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="VC ID" DataField="VCID" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Bulk Status" DataField="Status" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                                <PagerSettings Mode="Numeric" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
