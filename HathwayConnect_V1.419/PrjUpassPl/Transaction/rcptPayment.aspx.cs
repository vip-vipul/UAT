using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjUpassPl.Transaction
{
    public partial class rcptPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rcpt_no = "";
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
            if (Session["rcpt_pt_rcptno1"] == null || Session["rcpt_pt_date1"] == null || Session["rcpt_pt_lcocd1"] == null || Session["rcpt_pt_amt1"] == null)
            {
                Response.Redirect("~/Transaction/TransHwayLcoPayment.aspx");
            }
            if (Session["rcpt_pt_rcptno1"] != null)
            {
                rcpt_no = Session["rcpt_pt_rcptno1"].ToString();
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
            //Session["cashier"] = Session["name"].ToString();
            if (pay_mode.Trim() == "Cash")
            {
                bank_name = "N/A";
                cheque_no = "N/A";
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
            
            if (!IsPostBack)
            {
                //Session["rcptno1"] = null;
                //Session["premark1"] = null;
                //Session["date1"] = null;
                //Session["cashiername"] = null;
                //Session["address"] = null;
                //Session["company"] = null;
                //Session["lcocd1"] = null;
                //Session["lconm1"] = null;
                //Session["amt1"] = null;
                //Session["paymode1"] = null;
                //Session["bnknm1"] = null;
                //Session["cheqno1"] = null;
            }
        }
    }
}