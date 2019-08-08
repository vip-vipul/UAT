<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rcptPaymentReceiptInvoicePIS.aspx.cs" Inherits="PrjUpassPl.Transaction.rcptPaymentReceiptInvoicePIS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Payment Receipt</title>
    <script type="text/javascript" src="../JS/jquery.min.js"></script>
    <script type="text/javascript">
        function printRcpt() {
            var divName = "<%= pnlRcpt.ClientID %>";
            var printContents = document.getElementById(divName).innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;
            window.print();
            document.body.innerHTML = originalContents;
        }

        function inWords() {
            var a = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine ', 'Ten ', 'Eleven ', 'Twelve ', 'Thirteen ', 'Fourteen ', 'Fifteen ', 'Sixteen ', 'Seventeen ', 'Eighteen ', 'Nineteen '];
            var b = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
            var num;

            num = $("#<%= lblNetAmount.ClientID %>").text();

            if (num == '') {
                num = $("#<%= lblAmout1.ClientID %>").text();

            }
            if ((num = num.toString()).length > 9) return 'overflow';
            n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
            if (!n) return;
            var str = '';
            str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'Crore ' : '';
            str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'Lakh ' : '';
            str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'Thousand ' : '';
            str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'Hundred ' : '';
            str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + ' only ' : '';
            try {
                $("#<%= lblAmtInWords.ClientID %>").text(str);
            }
            catch (err) {
            }

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
            background-color: #FFFFFF; /*border: solid 1px black;*/
            margin: 0px; /* this affects the margin on the content before sending to printer */
        }
    </style>
</head>
<body onload="inWords()">
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
                            <h4><u><asp:Label ID="lblReceiptTitle" runat="server"></asp:Label></u></h4>
                            <asp:Label ID="lblCompanyName" runat="server" Font-Bold="true" Text=""></asp:Label>
                            <div style="width: 100%; font-size: 10px; text-align: center;">
                                Branch Address:
                                <asp:Label runat="server" ID="lblAddress12" Text=""></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 10px; border-bottom: 0px;">
                            <table width="100%" style="border: 1px solid black; border-bottom: 0px; border-collapse: collapse;">
                                <tr>
                                    <td align="left" width="33.34%">
                                      &nbsp;&nbsp;  CIN :
                                        <asp:Label runat="server" ID="lblCIN" Text=""></asp:Label>
                                    </td>
                                    <td align="center" width="33.34%">
                                        PAN :
                                        <asp:Label runat="server" ID="lblPAN" Text=""></asp:Label>
                                    </td>
                                    <td align="center" width="33.34%">
                                        GSTN No. :
                                        <asp:Label runat="server" ID="lblSTNO" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                                <tr>
                                    <td style="padding-left: 10px; width: 50%;">
                                        Receipt No. :
                                        <asp:Label runat="server" ID="lblRcptNo" Text=""></asp:Label>
                                    </td>
                                     <td style="padding-left: 10px;">
                                         <asp:Label ID="lbllcocodeheadtxt" runat="server" Text="LCO Code :"></asp:Label>
                                        <asp:Label runat="server" ID="lblLcoCode" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                <td style="padding-left: 10px;">
                                        Date :
                                        <asp:Label runat="server" ID="lblRcptDate" Text=""></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdnAmount"/>
                                    </td>
                                   
                                    <td style="padding-left: 10px;">
                                    <asp:Label ID="lbllconameheadtxt" runat="server" Text=" LCO Name :"></asp:Label>
                                        <asp:Label runat="server" ID="lblLcoName" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        <asp:Label runat="server" ID="lblCashier" Text="Cashier ID :"></asp:Label>
                                        <asp:Label runat="server" ID="lblCashierName" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">

                                        Company Address :
                                        <asp:Label runat="server" ID="lblAddress13" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="width: 100%; text-align: center;">
                                <h4>
                                    Payment Details
                                     <br />
                                         <asp:Label ID="lbltype" runat="server" Text=""></asp:Label></h4>
                                <p>
                                    
                                </p>
                            </div>
                            <table  id="trOnline" runat="server"  width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                            <tr runat="server" id="trPaymode">
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Mode
                                        
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;width:25%;">
                                        <asp:Label runat="server" ID="lblPaymentMode4" Text=""></asp:Label>
                                    </td>
                                       <td style="padding-left: 10px;border-right:0px;">
                                        
                                         <asp:Label runat="server" ID="lblOnlinereceipt2" Text=""></asp:Label>
                                    </td>
                                    <td style="border-left:0px;border-right:0px;"  id="td2" runat="server">:</td>
                                    <td style="border-left:0px;"  id="td3" runat="server">
                                        <asp:Label runat="server" ID="lblonlineR1" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="padding-left: 10px;border-right:0px;">
                                        <b>
                                            <asp:Label ID="lblstbnohead" runat="server" Text="No. of STB"></asp:Label>
                                       </b>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" Font-Bold="true" ID="lblstbcount" Text=""></asp:Label>
                                    </td>
                                   <td style="padding-left: 10px;border-right:0px;">
                                       <asp:Label ID="lblbankname" runat="server" Text="Bank Name"></asp:Label>
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">
                                    <asp:Label ID="lblBankCol" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lblBankName3" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trscheame">
                                    <td style="padding-left: 10px;border-right:0px;">
                                        <asp:Label ID="lblscheame" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lblscheametxt" Text=""></asp:Label>
                                    </td>
                                   <td style="padding-left: 10px;border-right:0px;">
                                         <asp:Label ID="lblscheameamt" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lblscheameamttxt" Text=""></asp:Label>
                                    </td>
                                </tr>
                                 <tr runat="server" id="trlcodet">
                                    <td style="padding-left: 10px;border-right:0px;">
                                        <asp:Label ID="lbllcocodehead" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lbllcocodetxt" Text=""></asp:Label>
                                    </td>
                                   <td style="padding-left: 10px;border-right:0px;">
                                         <asp:Label ID="lbllconamehead" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lbllconametxt" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        <b>
                                            <asp:Label ID="lbldiscounttxt" runat="server" Text="Discount"></asp:Label></b>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;"><asp:Label runat="server" Font-Bold="true" ID="lblCol" Text=":"></asp:Label></td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" Font-Bold="true" ID="lbldiscount" Text=""></asp:Label>
                                    </td>
                                   <td style="padding-left: 10px;border-right:0px;">
                                        <b>Total Amount</b>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" Font-Bold="true" ID="lblAmout1" Text=""></asp:Label>
                                    </td>
                                </tr>
                            <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;"></td>
                                    <td style="border-left:0px;">
                                        
                                    </td>
                                   <td style="padding-left: 10px;border-right:0px;">
                                        <b>
                                        <asp:Label runat="server" Font-Bold="true" ID="lblNet" Text="Net Amount"></asp:Label>
                                        </b>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">
                                    <asp:Label runat="server" Font-Bold="true" ID="lblNetCol" Text=":"></asp:Label>
                                    </td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" Font-Bold="true" ID="lblNetAmount" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table width="100%">
                                <tr style="border: 0px;">
                                    <td style="padding-left: 10px;">
                                        Amount in words :
                                        <asp:Label runat="server" ID="lblAmtInWords" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        Remark :
                                        <asp:Label runat="server" ID="lblRemark" Text=""></asp:Label>
                                        <asp:Label runat="server" ID="lblData" Text="" Font-Bold="True" ForeColor="#FF3300"></asp:Label>
                                    </td>
                                </tr>

                                <tr id="discountremark" runat="server" visible="false">
                                    <td style="padding-left: 10px;">
                                        <asp:Label runat="server" ID="Label1" Text=""></asp:Label>
                                        <asp:Label runat="server" ID="lbldistremark" Text="" Font-Bold="True" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                <asp:Label ID="lblchqcaption" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 50px; padding: 10px; border-top: 0px;" align="center" valign="bottom">
                           <asp:Label runat="server" ID="lblSignature" Text="This is system generated receipts, no signature is required"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 10px;">
                            Registered Address :
                            <asp:Label runat="server" ID="lblAddress" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-size: x-small;">
                            Powered By UPASS : http://www.upass.cc
                        </td>
                    </tr>
                </table>
            </div>
            <hr />
           
        </asp:Panel>
    </div>
    </form>
</body>
</html>
