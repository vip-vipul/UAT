<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rcptPaymentReceiptInvoice.aspx.cs"
    Inherits="PrjUpassPl.Reports.rcptPaymentReceiptInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            try {
                num = $("#<%= lblAmout1.ClientID %>").text();
                
            }
            catch (err) {
                num = $("#<%= lblAmount.ClientID %>").text();
            }
            if ((num = num.toString()).length > 9) return 'overflow';
            n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
            if (!n) return;
            var str = '';
            str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'Crore ' : '';
            str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'Lakh ' : '';
            str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'Thousand ' : '';
            str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'Hundred ' : '';
            str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + 'only ' : '';
            try
            {
            $("#<%= lblAmtInWords.ClientID %>").text(str);
            $("#<%= lblAmtInWords2.ClientID %>").text(str);
            }
            catch(err)
            {
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
                                        ST No. :
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
                                        LCO Code :
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
                                        LCO Name :
                                        <asp:Label runat="server" ID="lblLcoName" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        <asp:Label runat="server" ID="lblCashier" Text="Cashier Name :"></asp:Label>
                                        <asp:Label runat="server" ID="lblCashierName" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">

                                        LCO Address :
                                        <asp:Label runat="server" ID="lblAddress13" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="width: 100%; text-align: center;">
                                <h4>
                                    Payment Details</h4>
                            </div>
                            <table id="trCash" runat="server" width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                                <tr>
                                    <td style="padding-left: 10px; width: 31%;border-right:0px;">
                                        Base Amount(Rs.)
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">
                                        :
                                    </td>
                                    <td style="border-left: 0px;width:19%;">
                                        <asp:Label runat="server" ID="lblBaseAmt" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;border-right:0px;width:25%;">
                                        Mode
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">
                                        :
                                    </td>
                                    <td style="border-left:0px;width:25%;">
                                        <asp:Label runat="server" ID="lblPaymentMode" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Service Tax @
                                        <asp:Label runat="server" ID="lblST" Text=""></asp:Label>%
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">
                                        :
                                    </td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblSTAmt" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Cheque/DD/Ref/RR No.
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lblChequeDDNo" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Education Cess @
                                        <asp:Label runat="server" ID="lblEC" Text=""></asp:Label>%
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblECAmt" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Bank Name
                                        
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lblBankName" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Higher Education Cess @
                                        <asp:Label runat="server" ID="lblHEC" Text=""></asp:Label>%
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblHECAmt" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;border-right:0px;" id="tdOnline1" runat="server">
                                        Online Receipt No.
                                    </td>
                                    <td style="border-left:0px;border-right:0px;"  id="tdOnline2" runat="server">:</td>
                                    <td style="border-left:0px;"  id="tdOnline3" runat="server">
                                        <asp:Label runat="server" ID="lblonlineReceipt" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trET">
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Entertainment Tax @
                                        <asp:Label runat="server" ID="lblET" Text=""></asp:Label>%
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblETAmt" Text=""></asp:Label>
                                    </td>
                                   
                                    <td style="padding-left: 10px;border-right:0px;" id="TRmpos12" runat="server" visible="false">
                                        R.R. No.
                                    </td>
                                    <td style="border-left:0px;border-right:0px;"  id="TRmpos13" runat="server"  visible="false">:</td>
                                    <td style="border-left:0px;"  id="TRmpos14" runat="server"  visible="false">
                                        <asp:Label runat="server" ID="lblrr1" Text=""></asp:Label>
                                    </td>
                                </tr>
                                   
                                   
                                  <tr runat="server" id="TRmpos11">
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Auth No.
                                        
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblauth1" Text=""></asp:Label>
                                    </td>
                                   
                                    <td style="padding-left: 10px;border-right:0px;" id="td11" runat="server">
                                        MPOS User Id
                                    </td>
                                    <td style="border-left:0px;border-right:0px;"  id="td12" runat="server">:</td>
                                    <td style="border-left:0px;"  id="td13" runat="server">
                                        <asp:Label runat="server" ID="lblmpos1" Text=""></asp:Label>
                                    </td>

                                </tr>


                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        <b>Total Amount</b>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" Font-Bold="true" ID="lblAmount" Text=""></asp:Label>
                                    </td>
                                    <td colspan="3" style="padding-left: 10px;">
                                    </td>
                                </tr>
                            </table>
                            <table  id="trOnline" runat="server"  width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                            <tr>
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
                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        <b>Total Amount</b>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" Font-Bold="true" ID="lblAmout1" Text=""></asp:Label>
                                    </td>
                                   <td style="padding-left: 10px;border-right:0px;">
                                        Bank Name
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lblBankName3" Text=""></asp:Label>
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
                                        <br />
                                        <asp:Label runat="server" ID="lblData" Text="" Font-Bold="True" ForeColor="#FF3300"></asp:Label>
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
            <div style="margin-top: 10px; display:none;">
                <table width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;
                    font-size: 12px; font-family: Arial;">
                    <tr>
                        <td style="height: 50px;" align="center">
                            <h4><u><asp:Label ID="lblReceiptTitle1" runat="server"></asp:Label></u></h4>
                            <asp:Label ID="lblCompanyName2" runat="server" Font-Bold="true" Text=""></asp:Label>
                            <div style="width: 100%; font-size: 10px; text-align: center;">
                                Branch Address:
                                <asp:Label runat="server" ID="lblAddress22" Text=""></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 10px; border-bottom: 0px;">
                            <table width="100%" style="border: 1px solid black; border-bottom: 0px; border-collapse: collapse;">
                                <tr>
                                    <td align="left" width="33.34%">
                                       &nbsp;&nbsp; CIN :
                                        <asp:Label runat="server" ID="lblCIN2" Text=""></asp:Label>
                                    </td>
                                    <td align="center" width="33.34%">
                                        PAN :
                                        <asp:Label runat="server" ID="lblPAN2" Text=""></asp:Label>
                                    </td>
                                    <td align="center" width="33.34%">
                                        ST No. :
                                        <asp:Label runat="server" ID="lblSTNO2" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                                <tr>
                                    <td style="padding-left: 10px; width: 50%;">
                                        Receipt No. :
                                        <asp:Label runat="server" ID="lblRcptNo2" Text=""></asp:Label>
                                    </td>
                                   <td style="padding-left: 10px;">
                                        LCO Code :
                                        <asp:Label runat="server" ID="lblLcoCode2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                 <td style="padding-left: 10px;">
                                        Date :
                                        <asp:Label runat="server" ID="lblRcptDate2" Text=""></asp:Label>
                                    </td>
                                    
                                    <td style="padding-left: 10px;">
                                        LCO Name :
                                        <asp:Label runat="server" ID="lblLcoName2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        <asp:Label runat="server" ID="lblCashier1" Text="Cashier Name :"></asp:Label>
                                        <asp:Label runat="server" ID="lblCashierName2" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        Company Address :
                                        <asp:Label runat="server" ID="lblAddress23" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="width: 100%; text-align: center;">
                                <h4>
                                    Payment Details</h4>
                            </div>
                            <table id="trCash1" runat="server" width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                                <tr>
                                    <td style="padding-left: 10px; width: 31%;border-right:0px;">
                                        Base Amount(Rs.)
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">
                                        :
                                    </td>
                                    <td style="border-left: 0px;width:19%">
                                        <asp:Label runat="server" ID="lblBaseAmt2" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;border-right:0px;width:25%">
                                        Mode
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">
                                        :
                                    </td>
                                    <td style="border-left:0px;width:25%">
                                        <asp:Label runat="server" ID="lblPaymentMode2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Service Tax @
                                        <asp:Label runat="server" ID="lblST2" Text=""></asp:Label>%
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">
                                        :
                                    </td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblSTAmt2" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Cheque/DD/Ref No.
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lblChequeDDNo2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Education Cess @
                                        <asp:Label runat="server" ID="lblEC2" Text=""></asp:Label>%
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblECAmt2" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Bank Name
                                        
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lblBankName2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Higher Education Cess @
                                        <asp:Label runat="server" ID="lblHEC2" Text=""></asp:Label>%
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblHECAmt2" Text=""></asp:Label>
                                    </td>
                                   <td style="padding-left: 10px;border-right:0px;" id="tdOnline4" runat="server">
                                        Online Receipt No.
                                    </td>
                                    <td style="border-left:0px;border-right:0px;"  id="tdOnline5" runat="server">:</td>
                                    <td style="border-left:0px;"  id="tdOnline6" runat="server">
                                        <asp:Label runat="server" ID="lblonlineReceipt1" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="tr1">
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Entertainment Tax @
                                        <asp:Label runat="server" ID="lblET2" Text=""></asp:Label>%
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblETAmt2" Text=""></asp:Label>
                                    </td>
                                   
                                    <td style="padding-left: 10px;border-right:0px;" id="TRmpos22" runat="server" visible="false"  >
                                        R.R. No.
                                    </td>
                                    <td style="border-left:0px;border-right:0px;"  id="TRmpos23" runat="server" visible="false" >:</td>
                                    <td style="border-left:0px;"  id="TRmpos24" runat="server" visible="false" >
                                        <asp:Label runat="server" ID="lblrr2" Text=""></asp:Label>
                                    </td>

                                </tr>

                                  <tr runat="server" id="TRmpos21" visible="false">
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Auth No.
                                       
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td  style="border-left: 0px;">
                                        <asp:Label runat="server" ID="lblAuth2" Text=""></asp:Label>
                                    </td>
                                   
                                    <td style="padding-left: 10px;border-right:0px;" id="td8" runat="server">
                                        MPOS User Id
                                    </td>
                                    <td style="border-left:0px;border-right:0px;"  id="td9" runat="server">:</td>
                                    <td style="border-left:0px;"  id="td10" runat="server">
                                        <asp:Label runat="server" ID="lblmpos2" Text=""></asp:Label>
                                    </td>

                                </tr>


                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        <b>Total Amount</b>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" Font-Bold="true" ID="lblAmount2" Text=""></asp:Label>
                                    </td>
                                    <td colspan="3" style="padding-left: 10px;">
                                    </td>
                                </tr>
                            </table>
                             <table  id="trOnline1" runat="server" width="100%" border="1" bordercolor="Black" style="border-collapse: collapse;">
                            <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Mode
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;width:25%;">
                                        <asp:Label runat="server" ID="lblPaymentMode3" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        
                                         <asp:Label runat="server" ID="lblOnlinereceipt5" Text=""></asp:Label>
                                    </td>
                                    <td style="border-left:0px;border-right:0px;"  id="td1" runat="server">:</td>
                                    <td style="border-left:0px;"  id="td4" runat="server">
                                        <asp:Label runat="server" ID="lblonlineR2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        <b>Total Amount</b>
                                    </td>
                                    <td style="border-left: 0px;border-right: 0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" Font-Bold="true" ID="lblAmout2" Text=""></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;border-right:0px;">
                                        Bank Name
                                    </td>
                                    <td style="border-left:0px;border-right:0px;">:</td>
                                    <td style="border-left:0px;">
                                        <asp:Label runat="server" ID="lblBankName4" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr style="border: 0px;">
                                    <td style="padding-left: 10px;">
                                        Amount in words :
                                        <asp:Label runat="server" ID="lblAmtInWords2" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        Remark :
                                        <asp:Label runat="server" ID="lblRemark2" Text=""></asp:Label>
                                       <%-- <br />
                                         <asp:Label runat="server" ID="lblData1" Text="" Font-Bold="True" ForeColor="#FF3300"></asp:Label>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 50px; padding: 10px; border-top: 0px;" align="center" valign="bottom">
                            <asp:Label runat="server" ID="lblSignature1" Text="This is system generated receipts, no signature is required"></asp:Label>   
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 10px;">
                            Registered Address :
                            <asp:Label runat="server" ID="lblAddress2" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-size: x-small;">
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
