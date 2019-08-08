using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Transaction
{
    public partial class rcptPaymentReceiptInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rcpt_no = "";
            string rcpt_Online_no = "";
            string date = "";
            string cashier_name = "";
            string address = "";
            string company = "";
            string lco_code = "";
            string lco_name = "";
            string amount = "";
            string pay_mode = "";
            string cheque_no = "";
            string bank_name = "";
            string remark = "";
            string username = "";

            string base_amt = "";
            string st = "";
            string st_smt = "";
            string ec = "";
            string ec_amt = "";
            string hec = "";
            string hec_amt = "";
            string et = "";
            string et_amt = "";

            if (Session["username"] != null && Session["username"] != "")
            {
                username = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            if (Session["rcpt_pt_rcptno1"] == null || Session["rcpt_pt_date1"] == null || Session["rcpt_pt_lcocd1"] == null || Session["rcpt_pt_amt1"] == null)
            {
                if (Session["rcpt_pt_paymode1"] != null)
                {
                    if (Session["rcpt_pt_paymode1"].ToString() == "Online")
                    {
                        Response.Redirect("~/Transaction/TransHwayOnlinePayResponse.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/Transaction/TransHwayLcoPayment.aspx");
                    }
                }
            }


            if (Session["rcpt_pt_rcptno1"] != null)
            {
                rcpt_no = Session["rcpt_pt_rcptno1"].ToString();
            }
            if (Session["rcpt_pt_rcptno2"] != null)
            {
                rcpt_Online_no = Session["rcpt_pt_rcptno2"].ToString();
            }
            if (Session["rcpt_pt_date1"] != null)
            {
                date = Session["rcpt_pt_date1"].ToString();
            }
            if (Session["rcpt_pt_cashiername"] != null)
            {
                cashier_name = Session["rcpt_pt_cashiername"].ToString();
            }
            if (Session["rcpt_pt_address"] != null)
            {
                address = Session["rcpt_pt_address"].ToString();
            }
            if (Session["rcpt_pt_company"] != null)
            {
                company = Session["rcpt_pt_company"].ToString();
            }
            if (Session["rcpt_pt_lcocd1"] != null)
            {
                lco_code = Session["rcpt_pt_lcocd1"].ToString();
            }
            if (Session["rcpt_pt_lconm1"] != null)
            {
                lco_name = Session["rcpt_pt_lconm1"].ToString();
            }
            if (Session["rcpt_pt_amt1"] != null)
            {
                amount = Session["rcpt_pt_amt1"].ToString();
            }
            if (Session["rcpt_pt_paymode1"] != null)
            {
                pay_mode = Session["rcpt_pt_paymode1"].ToString();
            }
            if (Session["rcpt_pt_cheqno1"] != null)
            {
                cheque_no = Session["rcpt_pt_cheqno1"].ToString();
            }
            if (Session["rcpt_pt_bnknm1"] != null)
            {
                bank_name = Session["rcpt_pt_bnknm1"].ToString();
            }
            if (Session["rcpt_pt_premark1"] != null)
            {
                remark = Session["rcpt_pt_premark1"].ToString();
            }
            //if (Session["rcpt_pt_details"] != null)
            //{
            //    details = Session["rcpt_pt_details"].ToString();
            //}
            //Session["cashier"] = Session["name"].ToString();
            if (pay_mode.Trim() == "Cash")
            {
                bank_name = "N/A";
                cheque_no = "N/A";
            }
            if (pay_mode.Trim() == "Online")
            {
                tdOnline1.Visible = true;
                tdOnline2.Visible = true;
                tdOnline3.Visible = true;
                tdOnline4.Visible = true;
                tdOnline5.Visible = true;
                tdOnline6.Visible = true;
                lblonlineReceipt1.Text = rcpt_Online_no;
                lblonlineReceipt.Text = rcpt_Online_no;
                lblonlineR2.Text = rcpt_Online_no;
                lblonlineR1.Text = rcpt_Online_no;
                trOnline1.Visible = true;
                trOnline.Visible = true;
                trCash.Visible = false;
                trCash1.Visible = false;
                lblReceiptTitle.Text = "RECEIPT";

                lblReceiptTitle1.Text = "RECEIPT";
                lblCashier.Visible = false;
                lblCashierName.Visible = false;
                lblCashier1.Visible = false;
                lblCashierName2.Visible = false;
            }
            else
            {

                tdOnline1.Visible = false;
                tdOnline2.Visible = false;
                tdOnline3.Visible = false;
                tdOnline4.Visible = false;
                tdOnline5.Visible = false;
                tdOnline6.Visible = false;
                lblonlineReceipt.Text = "N/A";
                lblCashierName.Visible = true;
                lblCashier.Visible = true;
                lblCashier1.Visible = true;
                lblCashierName2.Visible = true;
                lblReceiptTitle.Text = "RECEIPT cum INVOICE";
                lblReceiptTitle1.Text = "RECEIPT cum INVOICE";
                trOnline1.Visible = false;
                trCash1.Visible = true;
                trCash.Visible = true;
            }
            lblCompanyName.Text = company;
            lblCompanyName2.Text = company;
            lblRemark.Text = remark;
            lblRemark2.Text = remark;
            lblBankName.Text = bank_name;
            lblBankName2.Text = bank_name;
            lblChequeDDNo.Text = cheque_no;
            lblChequeDDNo2.Text = cheque_no;
            lblPaymentMode.Text = pay_mode;
            lblPaymentMode2.Text = pay_mode;
            lblAmount.Text = amount;
            lblAmount2.Text = amount;
            lblAmout2.Text = amount;
            hdnAmount.Value = amount;
            lblAmout1.Text = amount;
            lblLcoCode.Text = lco_code;
            lblLcoCode2.Text = lco_code;
            lblLcoName.Text = lco_name;
            lblLcoName2.Text = lco_name;
            lblAddress.Text = address;
            lblAddress2.Text = address;
            lblCashierName.Text = cashier_name;
            lblCashierName2.Text = cashier_name;
            lblRcptDate.Text = date;
            lblRcptDate2.Text = date;
            lblRcptNo.Text = rcpt_no;
            lblRcptNo2.Text = rcpt_no;

            Cls_BLL_TransHwayLcoPayment objpan = new Cls_BLL_TransHwayLcoPayment();
            string[] details = objpan.getPanDetails(username, company);
            if (details != null)
            {
                try
                {
                    lblCIN.Text = details[0];
                    lblCIN2.Text = details[0];
                    lblPAN.Text = details[1];
                    lblPAN2.Text = details[1];
                    lblSTNO.Text = details[2];
                    lblSTNO2.Text = details[2];
                    lblCompanyName.Text = details[3];
                    lblCompanyName2.Text = details[3];
                    address = details[3];
                    lblAddress12.Text = address;
                    lblAddress13.Text = address;
                    lblAddress22.Text = address;
                    lblAddress23.Text = address;

                }
                catch (Exception ex)
                {

                }
            }

            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
            string[] tax_det = obj.getTaxDetails(username, rcpt_no);
            base_amt = tax_det[8];
            st = tax_det[2];
            st_smt = tax_det[3];
            ec = tax_det[4];
            ec_amt = tax_det[5];
            hec = tax_det[6];
            hec_amt = tax_det[7];
            et = tax_det[0];
            et_amt = tax_det[1];

            lblBaseAmt.Text = base_amt;
            lblBaseAmt2.Text = base_amt;
            lblST.Text = (Convert.ToDouble(st) * 100).ToString();
            lblST2.Text = (Convert.ToDouble(st) * 100).ToString();
            lblSTAmt.Text = st_smt;
            lblSTAmt2.Text = st_smt;
            lblHEC.Text = (Convert.ToDouble(hec) * 100).ToString();
            lblHEC2.Text = (Convert.ToDouble(hec) * 100).ToString();
            lblHECAmt.Text = hec_amt;
            lblHECAmt2.Text = hec_amt;
            lblEC.Text = (Convert.ToDouble(ec) * 100).ToString();
            lblEC2.Text = (Convert.ToDouble(ec) * 100).ToString();
            lblECAmt.Text = ec_amt;
            lblECAmt2.Text = ec_amt;
            lblET.Text = (Convert.ToDouble(et) * 100).ToString();
            lblET2.Text = (Convert.ToDouble(et) * 100).ToString();
            lblETAmt.Text = et_amt;
            lblETAmt2.Text = et_amt;



            if (et == "0")
            {
                tr1.Visible = false;
                trET.Visible = false;
            }
            else
            {
                tr1.Visible = true;
                trET.Visible = true;
            }
        }
    }
}