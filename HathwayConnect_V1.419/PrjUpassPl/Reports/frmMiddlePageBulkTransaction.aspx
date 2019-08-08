<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmMiddlePageBulkTransaction.aspx.cs" Inherits="PrjUpassPl.Reports.frmMiddlePageBulkTransaction" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        
        function ShowModalPopup() {
            document.getElementById('<%=btnRefresh.ClientID %>').click();
        }

        function goBack() {
            window.location.href = "../Transaction/TransHwayBulkOperation.aspx";
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
        <div class="maindive">
        <div style="float:right">
                <button onclick="return goBack()" id="btnreturnBulkOperation" runat="server"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                 <div>
            <br />
            <h3>
                <asp:Label runat="server" ID="lblStatusHeading" Text="Please wait file verification is in progress.."></asp:Label>
            </h3>
            <asp:Label runat="server" ID="lblStatus" Text="" ForeColor="Red"></asp:Label> 
                     <asp:LinkButton ID="lnkShwoAll"  runat="server" Text="Show" onclick="lnkShwoAll_Click"></asp:LinkButton>
            <br />
            <br />
            <br />
             
        </div>
        <asp:Panel ID="pnlErrData" runat="server" Visible="false">
           
            <table class="Grid">
                <tr>
                    <th>
                        Customer No
                    </th>
                    <th>
                        VC
                    </th>
                    <th>
                        LCO Code
                    </th>
                    <th>
                        Plan Name
                    </th>
                    <th>
                        Action
                    </th>
                    <th>
                        Error
                    </th>
                </tr>
                <asp:Repeater ID="rptErrData" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lblCustNo" runat="server" Text='<%#Eval("custno") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVC" runat="server" Text='<%#Eval("vc") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLcoCode" runat="server" Text='<%#Eval("lcocode") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPlan" runat="server" Text='<%#Eval("plan") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAction" runat="server" Text='<%#Eval("action") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("err") %>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </asp:Panel>
            <asp:Button ID="btnRefresh" runat="server" Text="Button" 
                onclick="btnRefresh_Click" />
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

