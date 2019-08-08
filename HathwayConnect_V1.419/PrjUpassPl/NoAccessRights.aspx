<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoAccessRights.aspx.cs" Inherits="PrjUpassPl.NoAccessRights" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <h1 style="color:Red;">
                You are not authorized to access this page!
            </h1>
           <%-- <asp:HyperLink ID="lnkPrevPage" NavigateUrl="~/Transaction/Home.aspx" runat="server">Click Here</asp:HyperLink>
            &nbsp;to move back to home page.--%>
        </center>
    </div>
    </form>
</body>
</html>
