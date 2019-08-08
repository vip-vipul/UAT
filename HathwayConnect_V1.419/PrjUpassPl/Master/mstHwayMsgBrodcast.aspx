<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="mstHwayMsgBrodcast.aspx.cs" Inherits="PrjUpassPl.Master.mstHwayMsgBrodcast" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:Panel runat="server" ID="pnlRegisterLCO">
        <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
        <div class="delInfo1">
            <table runat="server" align="center" width="700px" id="tbl1" border="0">
                <tr>
                    <td align="right">
                        <asp:Label ID="Label1" runat="server" Text="Enter Brodcaster Message"></asp:Label>
                    </td>
                    <td>
                     <%--<asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>--%>
                    </td>
                    <td align="center">
                        <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtBrodcastermsg" runat="server"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="1"
                            ControlToValidate="txtBrodcastermsg" ErrorMessage="Please Enter Brodcaster Message" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label3" runat="server" Text="Brodcaster Message Type"></asp:Label>
                    </td>
                     <td>
                     <asp:Label ID="Label6" runat="server" Text="" ForeColor="Red"></asp:Label>
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
                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="1" Text="Submit" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
