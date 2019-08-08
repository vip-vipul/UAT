<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptnoncasreport.aspx.cs" Inherits="PrjUpassPl.Reports.rptnoncasreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ mastertype virtualpath="~/MasterPage.Master" %>
    <style type="text/css">
        .l1
        {
            text-decoration: none;
            cursor: hand;
        }
        .l2
        {
            text-decoration: underline;
            cursor: hand;
        }
        table.list
        {
            font-family: Verdana;
            font-size: larger;
        }
        
        
        
        table.dList1
        {
            font-family: Verdana;
            font-size: large;
            width: 100%;
            background-color: black;
            border-width: 0;
        }
        table.dList1 td
        {
            background-color: white;
        }
        table.dList1 td.srno
        {
            background-color: black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:Panel ID="pnlView" runat="server">
        <div class="maindive">
            <table width="100%" align="center" border="0" style="vertical-align: middle; padding-top: 2px">
                <tr>
                    <td align="left">
                    <div class="griddiv">
                        <asp:DataList ID="xDlstState" runat="server" CellPadding="1" CellSpacing="1" Font-Size="Medium"
                            GridLines="Horizontal" RepeatColumns="2" Width="100%">
                            <ItemStyle Font-Bold="True"></ItemStyle>
                            <ItemTemplate>
                                <table cellpadding="2" cellspacing="1" class="dList1" width="100%" height="20px">
                                    <tr>
                                        <td align="left" valign="top" width="400">
                                            <asp:HyperLink ID="Hyperlink3" runat="server" CssClass="l1" NavigateUrl='<%#DataBinder.Eval(Container.DataItem,"var_frm_file")%>'
                                                onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
															<font size="2"><b>
																	<%# DataBinder.Eval(Container.DataItem, "var_frm_name")%>
																</b></font>
                                            </asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
