<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="EcafPages.aspx.cs" Inherits="PrjUpassPl.Reports.EcafPages" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                            <table id="MasterBody_xDlstState" rules="rows" style="font-size: Medium; width: 100%;"
                                cellspacing="1" cellpadding="1" border="1">
                                <tbody>
                                    <tr>
                                        <td style="font-weight: bold;">
                                            <table class="dList1" width="100%" cellspacing="1" cellpadding="2" height="20px">
                                                <tbody>
                                                    <tr>
                                                        <td width="400" valign="top" align="left">
                                                            <a id="Hyperlink3_0" class="l1" onmouseout=" this.className='l1'" onmouseover="this.className='l2'"
                                                                href="../Master/mstecafcustomerdetails.aspx"><font size="2"><b>ECAF Entry Form </b>
                                                                </font></a>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td style="font-weight: bold;">
                                            <table class="dList1" width="100%" cellspacing="1" cellpadding="2" height="20px">
                                                <tbody>
                                                    <tr>
                                                        <td width="400" valign="top" align="left">
                                                            <a id="MasterBody_xDlstState_Hyperlink3_2" class="l1" onmouseout=" this.className='l1'"
                                                                onmouseover="this.className='l2'" href="rptEcafCustDetails.aspx"><font size="2"><b>ECAF
                                                                    Report </b></font></a>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight: bold;">
                                            <table class="dList1" width="100%" cellspacing="1" cellpadding="2" height="20px">
                                                <tbody>
                                                    <tr>
                                                        <td width="400" valign="top" align="left">
                                                            <a id="A1" class="l1" onmouseout=" this.className='l1'" onmouseover="this.className='l2'"
                                                                href="../Master/mstecafstbtransfer.aspx"><font size="2"><b>STB Transfer</b> </font>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td style="font-weight: bold;">
                                            <table class="dList1" width="100%" cellspacing="1" cellpadding="2" height="20px">
                                                <tbody>
                                                    <tr>
                                                        <td width="400" valign="top" align="left">
                                                            <a id="A2" class="l1" onmouseout=" this.className='l1'" onmouseover="this.className='l2'"
                                                                href="../Master/mstecafstbtranslcoapplist.aspx"><font size="2"><b>STB Transfer LCO Approval
                                                                </b></font></a>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
