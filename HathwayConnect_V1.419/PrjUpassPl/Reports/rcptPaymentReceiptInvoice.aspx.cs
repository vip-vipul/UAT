using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Reports
{
    public partial class rcptPaymentReceiptInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
            String useraddress = "";

            if (Session["username"] != null && Session["username"] != "")
            {
                username = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            if (Session["rcpt_pt_receiptno"] == null || Session["rcpt_pt_datetime"] == null || Session["rcpt_pt_Lcocode"] == null || Session["rcpt_pt_Amount"] == null)
            {
                if (Session["rcpt_pt_Paymentmode"] != null)
                {
                    if (Session["rcpt_pt_Paymentmode"].ToString() == "Online")
                    {
                        Response.Redirect("~/Transaction/TransHwayOnlinePayResponse.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/Transaction/TransHwayLcoPayment.aspx");
                    }
                }
            }

            if (Session["data"] != null )
            {
                lblData.Text = Session["data"].ToString();
                Session["data"] = null;

            }


            if (Session["rcpt_pt_receiptno"] != null)
            {
                rcpt_no = Session["rcpt_pt_receiptno"].ToString();
            }
            if (Session["rcpt_pt_receiptonlineno"] != null)
            {
                rcpt_Online_no = Session["rcpt_pt_receiptonlineno"].ToString();
            }
            if (Session["rcpt_pt_datetime"] != null)
            {
                date = Session["rcpt_pt_datetime"].ToString();
            }
            if (Session["rcpt_pt_cashiername"] != null)
            {
                cashier_name = Session["rcpt_pt_cashiername"].ToString();
            }
            if (Session["rcpt_pt_address"] != null)
            {
                useraddress = Session["rcpt_pt_address"].ToString();
            }
            if (Session["rcpt_pt_company"] != null)
            {
                company = Session["rcpt_pt_company"].ToString();
            }
            if (Session["rcpt_pt_Lcocode"] != null)
            {
                lco_code = Session["rcpt_pt_Lcocode"].ToString();
            }
            if (Session["rcpt_pt_Lconame"] != null)
            {
                lco_name = Session["rcpt_pt_Lconame"].ToString();
            }
            if (Session["rcpt_pt_Amount"] != null)
            {
                amount = Session["rcpt_pt_Amount"].ToString();
            }
            if (Session["rcpt_pt_Paymentmode"] != null)
            {
                pay_mode = Session["rcpt_pt_Paymentmode"].ToString();
            }
            if (Session["rcpt_pt_ChequeNo"] != null)
            {
                cheque_no = Session["rcpt_pt_ChequeNo"].ToString();
            }
            if (Session["rcpt_pt_Bankname"] != null)
            {
                bank_name = Session["rcpt_pt_Bankname"].ToString();
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
            /*
            Session["rcpt_rr"]
            Session["rcpt_auth"] 
            Session["rcpt_mpos"] 
            */
            //if (Session["rcpt_rr"] != null)
            //{
            //    if (pay_mode.Trim() == "MPOS")
            //    {
            //        lblrr1.Text = Session["rcpt_rr"].ToString();
            //        lblrr2.Text = Session["rcpt_rr"].ToString();
            //        lblauth1.Text = Session["rcpt_auth"].ToString();
            //        lblAuth2.Text = Session["rcpt_auth"].ToString();
            //        lblmpos1.Text = Session["rcpt_mpos"].ToString();
            //        lblmpos2.Text = Session["rcpt_mpos"].ToString();

            //        TRmpos21.Visible = true;
            //        TRmpos22.Visible = true;
            //        TRmpos24.Visible = true;
            //        TRmpos23.Visible = true;
                    

            //        TRmpos11.Visible = true;
            //        TRmpos12.Visible = true;
            //        TRmpos13.Visible = true;
            //        TRmpos14.Visible = true;
            //    }
            //}

            if (pay_mode.Trim() == "Cash")
            {
                bank_name = "N/A";
                cheque_no = "N/A";
            }
            if (pay_mode.Trim() == "Online")
            {
                lblOnlinereceipt2.Text = "Online Receipt No.";
                lblOnlinereceipt5.Text = "Online Receipt No.";
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
                lblReceiptTitle.Text = "DUPLICATE RECEIPT";
                lblSignature.Visible = false;
                lblSignature1.Visible = false;
                lblReceiptTitle1.Text = "DUPLICATE RECEIPT";
                lblCashier.Visible = false;
                lblCashierName.Visible = false;
                lblCashier1.Visible = false;
                lblCashierName2.Visible = false;
            }
            else
            {
                lblOnlinereceipt2.Text = "Cheque No.";
                lblOnlinereceipt5.Text = "Cheque No.";
                lblonlineR1.Text = cheque_no;
                lblonlineR2.Text = cheque_no;
                lblSignature.Visible = true;
                lblSignature1.Visible = true;
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
                lblReceiptTitle.Text = "DUPLICATE RECEIPT";
                lblReceiptTitle1.Text = "DUPLICATE RECEIPT";
                trOnline1.Visible = true;
                trOnline.Visible = true;
                trCash.Visible = false;
                trCash1.Visible = false;
            }
           
            lblCompanyName.Text = company;
            lblCompanyName2.Text = company;
            lblRemark.Text = remark;
            lblRemark2.Text = remark;
            lblBankName.Text = bank_name;
            lblBankName2.Text = bank_name;
            lblBankName4.Text = bank_name;
            lblBankName3.Text = bank_name;
            lblChequeDDNo.Text = cheque_no;
            lblChequeDDNo2.Text = cheque_no;
            lblonlineR1.Text = cheque_no;
            lblonlineR2.Text = cheque_no;
            lblPaymentMode.Text = pay_mode;
            lblPaymentMode2.Text = pay_mode;
            lblPaymentMode4.Text = pay_mode;
            lblPaymentMode3.Text = pay_mode;
            lblAmount.Text = amount;
            lblAmount2.Text = amount;
            lblAmout2.Text = amount;
            hdnAmount.Value = amount;
            lblAmout1.Text = amount;
            lblLcoCode.Text = lco_code;
            lblLcoCode2.Text = lco_code;
            lblLcoName.Text = lco_name;
            lblLcoName2.Text = lco_name;
            lblAddress.Text = branchsddress;
            lblAddress2.Text = branchsddress;
            lblCashierName.Text = cashier_name;
            lblCashierName2.Text = cashier_name;
            lblRcptDate.Text = date;
            lblRcptDate2.Text = date;
            lblRcptNo.Text = rcpt_no;
            lblRcptNo2.Text = rcpt_no;

            if (pay_mode.Trim() == "MPOS")
            {
                lblOnlinereceipt2.Text = "R.R No.";
                lblOnlinereceipt5.Text = "R.R No.";
                lblonlineR1.Text = Session["rcpt_rr"].ToString();
                lblonlineR2.Text = Session["rcpt_rr"].ToString();
                bank_name = "N/A";
            }

            Cls_BLL_TransHwayLcoPayment objpan = new Cls_BLL_TransHwayLcoPayment();



            string[] details = objpan.getPanDetails(Session["rcpt_pt_Lcocode"].ToString(), company);
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
                    lcoaddress = details[4];
                    branchsddress = details[5];
                    lblAddress12.Text = details[6];
                    lblAddress13.Text = lcoaddress;
                    lblAddress22.Text = details[6];
                    lblAddress23.Text = lcoaddress;
                    lblAddress.Text = branchsddress;
                    lblAddress2.Text = branchsddress;
                }
                catch (Exception ex)
                {

                }
            }

            //Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
            //string[] tax_det = obj.getTaxDetails(username, rcpt_no);
            //base_amt = tax_det[8];
            //st = tax_det[2];
            //st_smt = tax_det[3];
            //ec = tax_det[4];
            //ec_amt = tax_det[5];
            //hec = tax_det[6];
            //hec_amt = tax_det[7];
            //et = tax_det[0];
            //et_amt = tax_det[1];

            //lblBaseAmt.Text = base_amt;
            //lblBaseAmt2.Text = base_amt;
            //lblST.Text = (Convert.ToDouble(st) * 100).ToString();
            //lblST2.Text = (Convert.ToDouble(st) * 100).ToString();
            //lblSTAmt.Text = st_smt;
            //lblSTAmt2.Text = st_smt;
            //lblHEC.Text = (Convert.ToDouble(hec) * 100).ToString();
            //lblHEC2.Text = (Convert.ToDouble(hec) * 100).ToString();
            //lblHECAmt.Text = hec_amt;
            //lblHECAmt2.Text = hec_amt;
            //lblEC.Text = (Convert.ToDouble(ec) * 100).ToString();
            //lblEC2.Text = (Convert.ToDouble(ec) * 100).ToString();
            //lblECAmt.Text = ec_amt;
            //lblECAmt2.Text = ec_amt;
            //lblET.Text = (Convert.ToDouble(et) * 100).ToString();
            //lblET2.Text = (Convert.ToDouble(et) * 100).ToString();
            //lblETAmt.Text = et_amt;
            //lblETAmt2.Text = et_amt;



            //if (et == "0")
            //{
            //    tr1.Visible = false;
            //    trET.Visible = false;
            //}
            //else
            //{
            //    tr1.Visible = true;
            //    trET.Visible = true;
            //}
        }
    }
}