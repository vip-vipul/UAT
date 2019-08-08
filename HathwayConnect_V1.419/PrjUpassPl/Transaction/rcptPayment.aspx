<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rcptPayment.aspx.cs" Inherits="PrjUpassPl.Transaction.rcptPayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html moznomarginboxes mozdisallowselectionprint xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Receipt</title>
    <script type="text/javascript">
        function printRcpt() {
            var divName = "<%= pnlRcpt.ClientID %>";
            var printContents = document.getElementById(divName).innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;
            window.print();
            document.body.innerHTML = originalContents;
        }

        var BM = 2; // button middle
        var BR = 3; // button right

        function mouseDown(e) {
            try { if (event.button == BM || event.button == BR) { return false; } }
            catch (e) { if (e.which == BR) { return false; } }
        }
        document.oncontextmenu = function () { return false; }
        document.onmousedown = mouseDown;
    </script>
    <style type="text/css" media="print">
        @page
        {
            size: auto; /* auto is the current printer page size */
            margin: 5mm; /* this affects the margin in the printer settings */
        }
        body
        {
            background-color: #FFFFFF;
            /*border: solid 1px black;*/
            margin: 0px; /* this affects the margin on the content before sending to printer */
        } 
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" value="Print" class="button" onclick="printRcpt();" />
        <asp:Panel ID="pnlRcpt" runat="server" Style="padding: 20px; width: 750px; font-size: 12px;
            font-family: Arial;">
            <div style="margin-bottom: 10px; width: 100%;">
                <table width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;
                    font-size: 12px; font-family: Arial;">
                    <tr>
                        <td style="height: 50px;" align="center">
                            <h3>
                                <u>RECEIPT</u></h3>
                            <asp:Label ID="lblCompanyName" runat="server" Text=""></asp:Label>
                            <div style="width: 100%; padding-left: 20px; font-size: 10px; text-align: left;">
                                LCO COPY</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 10px;">
                            <table width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                                <tr>
                                    <td style="padding-left: 10px; width: 50%;">
                                        Receipt No. :
                                        <asp:Label runat="server" ID="lblRcptNo" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        Date :
                                        <asp:Label runat="server" ID="lblRcptDate" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        LCO Code :
                                        <asp:Label runat="server" ID="lblLcoCode" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        LCO Name :
                                        <asp:Label runat="server" ID="lblLcoName" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 10px;">
                                        Cashier Name :
                                        <asp:Label runat="server" ID="lblCashierName" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="width: 100%; text-align: center;">
                                <h3>
                                    Payment Details</h3>
                            </div>
                            <table width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                                <tr>
                                    <td style="padding-left: 10px; width: 50%;">
                                        Paid Amount(Rs.) :
                                        <asp:Label runat="server" ID="lblAmount" Text=""></asp:Label>/-
                                    </td>
                                    <td style="padding-left: 10px;">
                                        By :
                                        <asp:Label runat="server" ID="lblPaymentMode" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        Cheque/DD/Ref No. :
                                        <asp:Label runat="server" ID="lblChequeDDNo" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        Bank Name :
                                        <asp:Label runat="server" ID="lblBankName" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 10px;">
                                        Remark :
                                        <asp:Label runat="server" ID="lblRemark" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 50px; padding: 10px;" align="right" valign="bottom">
                            Receiver's Signature
                        </td>
                    </tr>
                    <tr>
                        <td>
                            *Office Address :
                            <asp:Label runat="server" ID="lblAddress" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-size:x-small;">
                            Powered By UPASS : http://www.upass.cc
                        </td>
                    </tr>
                </table>
            </div>
            <hr />
            <div style="margin-top: 10px;">
                <table width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;
                    font-size: 12px; font-family: Arial;">
                    <tr>
                        <td style="height: 50px;" align="center">
                            <h3>
                                <u>RECEIPT</u></h3>
                            <asp:Label ID="lblCompanyName2" runat="server" Text=""></asp:Label>
                            <div style="width: 100%; padding-left: 20px; font-size: 10px; text-align: left;">
                                COMPANY COPY</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 10px;">
                            <table width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                                <tr>
                                    <td style="padding-left: 10px; width: 50%;">
                                        Receipt No. :
                                        <asp:Label runat="server" ID="lblRcptNo2" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        Date :
                                        <asp:Label runat="server" ID="lblRcptDate2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        LCO Code :
                                        <asp:Label runat="server" ID="lblLcoCode2" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        LCO Name :
                                        <asp:Label runat="server" ID="lblLcoName2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 10px;">
                                        Cashier Name :
                                        <asp:Label runat="server" ID="lblCashierName2" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="width: 100%; text-align: center;">
                                <h3>
                                    Payment Details</h3>
                            </div>
                            <table width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                                <tr>
                                    <td style="padding-left: 10px; width: 50%;">
                                        Paid Amount(Rs.) :
                                        <asp:Label runat="server" ID="lblAmount2" Text=""></asp:Label>/-
                                    </td>
                                    <td style="padding-left: 10px;">
                                        By :
                                        <asp:Label runat="server" ID="lblPaymentMode2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        Cheque/DD No. :
                                        <asp:Label runat="server" ID="lblChequeDDNo2" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        Bank Name :
                                        <asp:Label runat="server" ID="lblBankName2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 10px;">
                                        Remark :
                                        <asp:Label runat="server" ID="lblRemark2" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 50px; padding: 10px;" align="right" valign="bottom">
                            Receiver's Signature
                        </td>
                    </tr>
                    <tr>
                        <td>
                            *Office Address :
                            <asp:Label runat="server" ID="lblAddress2" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-size:x-small;">
                            Powered By UPASS : http://www.upass.cc
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
