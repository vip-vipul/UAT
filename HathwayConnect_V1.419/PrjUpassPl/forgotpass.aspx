<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgotpass.aspx.cs" Inherits="PrjUpassPl.forgotpass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>password</title>
    <link href="CSS/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <table align="center" style="padding-right: 50px;">
            <tr>
                <td align="right">
                    <asp:Label ID="lbluser" runat="server" Text="UserName"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtname" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr align="center">
                <td colspan="3" align="center">
                    <br />
                    <asp:Button ID="btnreset" runat="server" Text="Reset" class="button" Width="60" Font-Bold="true"
                        OnClick="btnreset_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
