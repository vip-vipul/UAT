<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="FrmWarehouseBulkProcess.aspx.cs" Inherits="PrjUpassPl.Transaction.FrmWarehouseBulkProcess" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
           <div class="maindive">
                <div class="delInfo">
<asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
  <div id="DivShowValue" runat="server">
                        <table class="Grid">
                            <tr>
                                <th>
                                  Unique Id
                                </th>
                                <th>
                                    Total STB/VC
                                </th>
                                <th>
                                    Success
                                </th>
                                <th>
                                    Fail
                                </th>
                                <th>
                                    Pending Varification
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
</div>
</div>
</asp:Content>
