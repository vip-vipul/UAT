<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="PrjUpassPl.ErrorPage" %>

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
                Sorry, something went wrong while processing your request!
            </h1>
            <asp:HyperLink ID="lnkPrevPage" runat="server">Click Here</asp:HyperLink>
            &nbsp;to move back to previous page.
        </center>
    </div>
    </form>
</body>
</html>
