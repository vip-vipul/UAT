<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmgstn.aspx.cs" Inherits="PrjUpassPl.Master.frmgstn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" action="http://gst.hathway.net:2462/prodgst/review" method="post"  clientidmode="Static">
        <input type="hidden" id="accnumber" name="accountNumber" value="" >
<input type="hidden"  name="action" value="getCustomerinfo" >
    </form>
</body>
</html>
