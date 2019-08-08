<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="Transactionprovi.aspx.cs" Inherits="PrjUpassPl.Transaction.Transactionprovi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../template1/tabcontent.css" rel="stylesheet" type="text/css" />
    <script src="../JS/tabcontent.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <div style="background-color: White;">
        <div style="width: 500px; margin: 0 auto; padding: 120px 0 40px;">
            <ul class="tabs" data-persist="true">
                <li><a href="#view1">Lorem</a></li><li><a href="#view2">TV'2'</a></li>
            </ul>
            <div class="tabcontents">
                <div id="view1">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <label>
                                    Customer No.</label>
                            </td>
                            <td>
                                <label>
                                    :</label>
                            </td>
                            <td>
                                <label>
                                    083248297</label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <input id="hdnStbNo" type="hidden" value="" />
                                <input id="hdnStbNo" type="hidden" value="" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <label runat="server" id="Label120">
                                    Customer Name</label>
                            </td>
                            <td>
                                <label id="Label6" runat="server">
                                    :</label>
                            </td>
                            <td align="left">
                                <label runat="server" id="lblCustN">
                                    VIRENDRA CABIN</label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="130px">
                                <label id="Label3" text="Customer Mobile">
                                    Customer Mobile</label>
                            </td>
                            <td>
                                <label id="Label31">
                                    :</label>
                            </td>
                            <td align="left">
                                <label runat="server" id="Label32">
                                    9768334055</label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="130px">
                                <label runat="server" id="Label5" text="Customer Address">
                                </label>
                            </td>
                            <td>
                                <label id="Label11" runat="server" text=":">
                                </label>
                            </td>
                            <td align="left">
                                <label runat="server" id="lblCustAddr1">
                                    10</label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="130px">
                                <label runat="server" id="lblServiceStat">
                                    Email Id</label>
                            </td>
                            <td>
                                <label id="Label4" runat="server" text=":">
                                </label>
                            </td>
                            <td align="left">
                                <label runat="server" id="lblCustAddr">
                                    Pankaj@gmail.com</label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="view2">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <label id="Label21">
                                    VC Number</label>
                            </td>
                            <td>
                                <label id="Label1" runat="server">
                                    :</label>
                            </td>
                            <td>
                                <label runat="server" id="lblCustNo">
                                    kjdf</label>
                    </table>
                </div>
            </div>
            <%--<div style="width: 500px; margin: 0 auto; padding: 120px 0 40px;">
        <ul class="tabs" data-persist="true">
            <li><a href="#view1">Lorem</a></li>
            <li><a href="#view2">Using other templates</a></li>
            <li><a href="#view3">Advanced</a></li>
        </ul>
        <div class="tabcontents">
            <div id="view1">
                <b>Lorem Issum</b>
                <p>Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...</p>
                
            </div>
            <div id="view2">
                <b>Switch to other templates</b>
                <p>Open this page with Notepad, and update the CSS link to:</P>
                <p>template1 ~ 6.</p>                
            </div>
            <div id="view3">
                <b>Advanced</b>
                <p>If you expect a more feature-rich version of the tabber, you can use the advanced version of the script, 
                    <a href="http://www.menucool.com/jquery-tabs">McTabs - jQuery Tabs</a>:</p>
                <ul>
                    <li>URL support: a hash id in the URL can select a tab</li>
                    <li>Bookmark support: select a tab via bookmark anchor</li>
                    <li>Select tabs by mouse over</li>
                    <li>Auto advance</li>
                    <li>Smooth transitional effect</li>
                    <li>Content can retrieved from other documents or pages through Ajax</li>
                    <li>... and more</li>     
                </ul>
            </div>
        </div>
    </div>--%>
        </div>
</asp:Content>
