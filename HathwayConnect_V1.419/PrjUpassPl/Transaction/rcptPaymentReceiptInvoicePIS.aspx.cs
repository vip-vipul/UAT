using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Transaction
{
    public partial class rcptPaymentReceiptInvoicePIS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String transID = "";
            if (Request.QueryString["transID"] != null)
            {
                // updated by Prashanth.
                //transID= Request.QueryString["transID"].ToString();
                transID = Request.QueryString["transID"].ToString().TrimEnd(',');
            }

            if (Session["username"] != null && Session["username"] != "")
            {

            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            string PISUSER = "";
            try
            {
                PISUSER = Session["username"].ToString();
            }
            catch
            {
                PISUSER = Session["username"].ToString();
            }
            cls_business_rcptpis ob = new cls_business_rcptpis();

            string response = ob.getrcptData(Session["username"].ToString(), transID.TrimStart(','));

            string[] Sortedres = response.Split('~');

            string data = Sortedres[0];
            string msgCode = Sortedres[1];

            if (msgCode == "9999")
            {
                string[] finlRes = data.Split('$');

                string rcpt_no = "";
                string rcpt_Online_no = "";
                string date = "";
                string cashier_name = "";
                string lcoaddress = "";
                string branchsddress = "";
                string company = "";
                string lco_code = "";
                string lco_name = "";
                string amount = "";
                String Discount = "";
                string pay_mode = "";
                string cheque_no = "";
                string bank_name = "";
                string remark = "";

                string base_amt = "";
                string st = "";
                string st_smt = "";
                string ec = "";
                string ec_amt = "";
                string hec = "";
                string hec_amt = "";
                string et = "";
                string et_amt = "";
                String useraddress = "";
                String Cashieruser = Session["username"].ToString();
                rcpt_no = finlRes[0];
                date = finlRes[1];
                cashier_name = Session["username"].ToString();
                useraddress = finlRes[2];
                company = finlRes[3];
                lco_code = finlRes[4];
                lco_name = finlRes[5];
                amount = finlRes[6];
                Discount = finlRes[7];
                pay_mode = finlRes[8];
                cheque_no = finlRes[9];
                bank_name = finlRes[10];
                remark = finlRes[11];
                lbltype.Text = finlRes[13];
                lblstbcount.Text = finlRes[12];
                trlcodet.Visible = false;
                trscheame.Visible = false;
                if (pay_mode == "Cheque")
                {
                    lblchqcaption.Visible = true;
                    lblchqcaption.Text = "**Credit in account will be given subject to clearance of cheque.";
                }
                else if (pay_mode == "DD")
                {
                    lblchqcaption.Visible = true;
                    lblchqcaption.Text = "**Credit in account will be given subject to clearance of Demand Draft.";
                }
                else
                {
                    lblchqcaption.Text = "";
                }
                if (finlRes[14].ToString() == "SPSN")
                {
                    Cashieruser = PISUSER;
                    lbldiscount.Visible = false;
                    lbldiscounttxt.Visible = false;
                    lblCol.Visible = false;
                    trlcodet.Visible = true;
                    trscheame.Visible = true;
                    discountremark.Visible = false;
                    lblNetAmount.Visible = true;
                    lblNet.Visible = true;
                    lblNetCol.Visible = true;
                    lbldistremark.Text = finlRes[20].ToString();
                    lblNetAmount.Text = finlRes[21].ToString();
                    if (finlRes[19].ToString() == "11")
                    {
                        lbllcocodehead.Text = "Distributor Code";
                        lbllcocodetxt.Text = finlRes[16];

                        lbllconamehead.Text = "Name";
                        lbllconametxt.Text = finlRes[15];

                        lblscheame.Text = "Scheme Name";
                        lblscheametxt.Text = finlRes[17];

                        lblscheameamt.Text = "Scheme Rate";
                        lblscheameamttxt.Text = finlRes[18];
                    }
                    else
                    {
                        lbllcocodehead.Text = "Lco Code";
                        lbllcocodetxt.Text = finlRes[16];

                        lbllconamehead.Text = "Name";
                        lbllconametxt.Text = finlRes[15];

                        lblscheame.Text = "Scheme Name";
                        lblscheametxt.Text = finlRes[17];

                        lblscheameamt.Text = "Scheme Rate";
                        lblscheameamttxt.Text = finlRes[18];
                    }
                    hdnAmount.Value = finlRes[6];
                    lblAmout1.Text = finlRes[6];
                    trPaymode.Visible = false;
                    lblbankname.Visible = false;
                    lblBankCol.Visible = false;
                    lblBankName3.Visible = false;
                }

                if (finlRes[14].ToString() == "SPSR")
                {
                    Cashieruser = PISUSER;
                    trlcodet.Visible = true;
                    trscheame.Visible = true;
                    lbldiscount.Visible = false;
                    lbldiscounttxt.Visible = false;
                    lblCol.Visible = false;
                    discountremark.Visible = false;
                    lblNetAmount.Visible = true;
                    lblNet.Visible = true;
                    lblNetCol.Visible = true;
                    lbldistremark.Text = finlRes[20].ToString();
                    lblNetAmount.Text = finlRes[21].ToString();
                    if (finlRes[19].ToString() == "11")
                    {
                        lbllcocodehead.Text = "Distributor Code";
                        lbllcocodetxt.Text = finlRes[16];

                        lbllconamehead.Text = "Name";
                        lbllconametxt.Text = finlRes[15];

                        lblscheame.Text = "Scheme Name";
                        lblscheametxt.Text = finlRes[17];

                        lblscheameamt.Text = "Scheme Rate";
                        lblscheameamttxt.Text = finlRes[18];
                    }
                    else
                    {
                        lbllcocodehead.Text = "Lco Code";
                        lbllcocodetxt.Text = finlRes[16];

                        lbllconamehead.Text = "Name";
                        lbllconametxt.Text = finlRes[15];

                        lblscheame.Text = "Scheme Name";
                        lblscheametxt.Text = finlRes[17];

                        lblscheameamt.Text = "Scheme Rate";
                        lblscheameamttxt.Text = finlRes[18];
                    }
                    hdnAmount.Value = finlRes[6];
                    lblAmout1.Text = finlRes[6];
                    trPaymode.Visible = false;
                    lblbankname.Visible = false;
                    lblBankCol.Visible = false;
                    lblBankName3.Visible = false;
                }

                if (finlRes[14].ToString() == "PPSN")
                {
                    Cashieruser = PISUSER;
                    lbldiscount.Visible = true;
                    lblCol.Visible = true;
                    lbldiscounttxt.Visible = true;
                    discountremark.Visible = true;
                    lblNetAmount.Visible = true;
                    lblNet.Visible = true;
                    lblNetCol.Visible = true;
                    lbldistremark.Text = finlRes[20].ToString();
                    lblNetAmount.Text = finlRes[21].ToString();
                    trlcodet.Visible = true;
                    trscheame.Visible = true;
                    lbllcocodehead.Text = "Account No.";
                    lbllcocodetxt.Text = finlRes[16];

                    lbllconamehead.Text = "VC No.";
                    lbllconametxt.Text = finlRes[15];

                    lblscheame.Text = "Scheme Name";
                    lblscheametxt.Text = finlRes[17];

                    lblscheameamt.Text = "Scheme Rate";
                    lblscheameamttxt.Text = finlRes[18];
                    hdnAmount.Value = finlRes[21].ToString();
                    lblAmout1.Text = finlRes[21].ToString();
                    trPaymode.Visible = false;
                    lblbankname.Visible = false;
                    lblBankCol.Visible = false;
                    lblBankName3.Visible = false;
                }

                if (finlRes[14].ToString() == "PPSR")
                {
                    Cashieruser = PISUSER;
                    lbldiscount.Visible = true;
                    lblCol.Visible = true;
                    lbldiscounttxt.Visible = true;
                    discountremark.Visible = true;
                    lblNetAmount.Visible = true;
                    lblNet.Visible = true;
                    lblNetCol.Visible = true;
                    lbldistremark.Text = finlRes[20].ToString();
                    lblNetAmount.Text = finlRes[21].ToString();
                    trlcodet.Visible = true;
                    trscheame.Visible = true;
                    lbllcocodehead.Text = "Account No.";
                    lbllcocodetxt.Text = finlRes[16];

                    lbllconamehead.Text = "VC No.";
                    lbllconametxt.Text = finlRes[15];

                    lblscheame.Text = "Scheme Name";
                    lblscheametxt.Text = finlRes[17];

                    lblscheameamt.Text = "Scheme Rate";
                    lblscheameamttxt.Text = finlRes[18];
                    hdnAmount.Value = finlRes[21].ToString();
                    lblAmout1.Text = finlRes[21].ToString();
                    trPaymode.Visible = false;
                    lblbankname.Visible = false;
                    lblBankCol.Visible = false;
                    lblBankName3.Visible = false;
                }

                if (finlRes[14].ToString() == "PPCC")
                {
                    lbldiscount.Visible = false;
                    lbldiscounttxt.Visible = false;
                    lblCol.Visible = false;
                    lblstbnohead.Text = "STB No.";
                    trlcodet.Visible = true;
                    lbllcocodehead.Text = "Account No.";
                    lbllcocodetxt.Text = finlRes[16];
                    lblNetAmount.Text = null;
                    lbllconamehead.Text = "VC No.";
                    lbllconametxt.Text = finlRes[15];
                    lblNetAmount.Visible = false;
                    lblNet.Visible = false;
                    lblNetCol.Visible = false;
                    if (finlRes.Length > 20)
                    {

                        hdnAmount.Value = finlRes[21].ToString();
                        lblAmout1.Text = finlRes[21].ToString();
                    }
                    else
                    {
                        hdnAmount.Value = amount;
                        lblAmout1.Text = amount;
                    }
                }

                if (finlRes[14].ToString() == "CBCC")
                {
                    lbldiscount.Visible = false;
                    lbldiscounttxt.Visible = false;
                    trlcodet.Visible = true;
                    lblCol.Visible = false;
                    lbllcocodehead.Text = "Broadcaster Code";
                    lbllcocodetxt.Text = finlRes[16];
                    lblNetAmount.Text = null;
                    lbllconamehead.Text = "Broadcaster Name";
                    lbllconametxt.Text = finlRes[15];
                    lblstbnohead.Text = "Collection Date";

                    lbldiscounttxt.Text = "Broadcaster ERP Code";
                    lbllcocodeheadtxt.Text = "Broadcaster Code :";
                    lbllconameheadtxt.Text = "Broadcaster Name :";
                    lblNetAmount.Visible = false;
                    lblNet.Visible = false;
                    lblNetCol.Visible = false;
                    hdnAmount.Value = finlRes[6];
                    lblAmout1.Text = finlRes[6];
                }

                if (finlRes[14].ToString() == "CACC")
                {
                    lbldiscount.Visible = false;
                    lbldiscounttxt.Visible = false;
                    trlcodet.Visible = true;
                    lblCol.Visible = false;
                    lbllcocodehead.Text = "Advertiser Code";
                    lbllcocodetxt.Text = finlRes[16];
                    lblNetAmount.Visible = false;
                    lblNet.Visible = false;
                    lblNetCol.Visible = false;
                    lblNetAmount.Text = null;
                    lbllconamehead.Text = "Advertiser Name";
                    lbllconametxt.Text = finlRes[15];
                    lblstbnohead.Text = "Collection Date";
                    lblNetAmount.Text = null;
                    lbldiscounttxt.Text = "Advertiser ERP Code";
                    lbllcocodeheadtxt.Text = "Advertiser Code :";
                    lbllconameheadtxt.Text = "Advertiser Name :";
                    hdnAmount.Value = finlRes[6];
                    lblAmout1.Text = finlRes[6];
                }
                if (finlRes[14].ToString() == "PPEC")
                {
                    lbldiscount.Visible = false;
                    lbldiscounttxt.Visible = false;
                    lblCol.Visible = false;
                    lblstbnohead.Text = "Executive Code";
                    lbllcocodeheadtxt.Text = "Executive Code :";
                    lblstbcount.Text = lco_code;
                    lbllconameheadtxt.Text = "Executive Name :";
                    lbllcocodehead.Text = "Executive Code";
                    lbllconamehead.Text = "Executive Name";
                    lblNetAmount.Visible = false;
                    lblNet.Visible = false;
                    lblNetCol.Visible = false;
                    lblNetAmount.Text = null;
                    hdnAmount.Value = finlRes[6].ToString();
                    lblAmout1.Text = finlRes[6].ToString();
                }
                if (finlRes[14].ToString() == "SPLC")
                {
                    lbldiscount.Visible = false;
                    lbldiscounttxt.Visible = false;
                    trlcodet.Visible = true;
                    lblCol.Visible = false;
                    lbldistremark.Text = finlRes[20].ToString();
                    if (finlRes[19].ToString() == "11")
                    {
                        lbllcocodehead.Text = "Distributor Code";
                        lbllcocodetxt.Text = finlRes[16];

                        lbllconamehead.Text = "Name";
                        lbllconametxt.Text = finlRes[15];

                    }
                    else
                    {
                        lbllcocodehead.Text = "Lco Code";
                        lbllcocodetxt.Text = finlRes[16];

                        lbllconamehead.Text = "Name";
                        lbllconametxt.Text = finlRes[15];
                    }
                    lblNetAmount.Visible = false;
                    lblNet.Visible = false;
                    lblNetCol.Visible = false;
                    lblNetAmount.Text = null;
                    hdnAmount.Value = finlRes[6];
                    lblAmout1.Text = finlRes[6].ToString();
                }

                if (pay_mode.Trim() == "Cash")
                {
                    bank_name = "N/A";
                    cheque_no = "N/A";
                }
                if (finlRes[14].ToString() == "PPEC")
                {
                    //bank_name = "N/A";
                    //cheque_no = "N/A";
                    lblPaymentMode4.Text = pay_mode;
                }
                lblPaymentMode4.Text = pay_mode;
                lblonlineR1.Text = cheque_no;
                lblSignature.Visible = true;
                lblCashierName.Visible = true;
                lblCashier.Visible = true;
                lblReceiptTitle.Text = "RECEIPT";
                trOnline.Visible = true;
                lbldiscount.Text = Discount;

                lblCompanyName.Text = company;
                lblRemark.Text = remark;
                lblBankName3.Text = bank_name;
                lblonlineR1.Text = cheque_no;



                // lblNetAmount.Text = amount;
                lblLcoCode.Text = lco_code;
                lblLcoName.Text = lco_name;
                lblAddress.Text = branchsddress;
                lblCashierName.Text = cashier_name;
                lblRcptDate.Text = date;
                lblRcptNo.Text = rcpt_no;

                lblOnlinereceipt2.Text = "Cheque No.";
                lblbankname.Text = "Bank Name";
                if (pay_mode.Trim() == "MPOS")
                {
                    lblOnlinereceipt2.Text = "R.R No.";
                    lblonlineR1.Text = cheque_no;
                    lblbankname.Text = "MPOS User Id";
                }
                else if (pay_mode.Trim() == "Cheque")
                {
                    lblonlineR1.Text = cheque_no;
                }
                else if (pay_mode.Trim() == "DD")
                {
                    lblOnlinereceipt2.Text = "DD No.";
                    lblonlineR1.Text = cheque_no;
                }
                else if (pay_mode.Trim() == "NEFT")
                {
                    lblOnlinereceipt2.Text = "Reference No.";
                    lblonlineR1.Text = cheque_no;
                }

                Cls_BLL_TransHwayLcoPayment objpan = new Cls_BLL_TransHwayLcoPayment();



                string[] details = objpan.getPanDetailsPIS(Cashieruser, company);
                if (details != null)
                {
                    try
                    {
                        lblCIN.Text = details[0];
                        lblPAN.Text = details[1];
                        lblSTNO.Text = details[2];
                        lblCompanyName.Text = details[3];
                        lcoaddress = details[4];
                        branchsddress = details[5];
                        lblAddress12.Text = useraddress;
                        lblAddress13.Text = lcoaddress;
                        lblAddress.Text = branchsddress;
                    }
                    catch (Exception ex)
                    {

                    }
                }

                /*Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
                string[] tax_det = obj.getTaxDetails(cashier_name, rcpt_no);
                base_amt = tax_det[8];
                st = tax_det[2];
                st_smt = tax_det[3];
                ec = tax_det[4];
                ec_amt = tax_det[5];
                hec = tax_det[6];
                hec_amt = tax_det[7];
                et = tax_det[0];
                et_amt = tax_det[1];*/


            }
        }
   
    }
}