

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmSelfcare.aspx.cs" Inherits="PrjUpassPl.Transaction.frmSelfcare" %>

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
                        
                            
                                <table cellpadding="2" cellspacing="1" class="dList1" width="100%" height="20px">
                                    <tr>
                                        <td align="left" valign="top" width="400">
                                            <asp:LinkButton ID="lnkEnableSelfcare" runat="server" CssClass="l1" OnClick="lnkEnableSelfcare_Click"
                                                onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
															<font size="2"><b>Enable Selfcare</b></font>
                                            </asp:LinkButton>
                                        </td>

                                        <td align="left" valign="top" width="400">
                                            <asp:HyperLink ID="Hyperlink1" runat="server" CssClass="l1" NavigateUrl=''
                                                onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
															<font size="2"><b>Debit Selfcare Report</b></font>
                                            </asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <cc1:ModalPopupExtender ID="popBasicEnableSelfcare" runat="server" BehaviorID="mpeBasicAdd" TargetControlID="hdnPopEnableSelfcare"
                        PopupControlID="pnlEnableSelfcare">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnPopEnableSelfcare" runat="server" />
                    <asp:Panel ID="pnlEnableSelfcare" runat="server" CssClass="Popup" Style="width: 700px; height: auto;max-height:550px;
                    display: none;">
                        <center>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                        &nbsp;&nbsp;&nbsp;Enable Selfcare
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table width="95%">
                                <tr id="tr10" runat="server">
                                    <td align="left" width="100px">
                                        <asp:Label ID="lbltext" Font-Bold="true" runat="server" Text="Plan Payterm"></asp:Label>
                                    </td>
                                    
                                </tr>

                 </table>
                <br />
                <table width="95%">
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="btnYes" runat="server" Width="60px" Text="Yes" OnClick="btnYes_Click"
                                CommandName="Change" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnNo" runat="server" Width="60px" Text="No" OnClick="btnNo_Click"
                                CommandName="Change" />
                            <%--<asp:Button ID="btnCloseAdd" runat="server" Width="60px" Text="Cancel" 
                                    onclick="btnCloseAdd_Click"/> --%>
                        </td>
                    </tr>
                </table>
                </center> 
                
                </asp:Panel>
</asp:Content>
