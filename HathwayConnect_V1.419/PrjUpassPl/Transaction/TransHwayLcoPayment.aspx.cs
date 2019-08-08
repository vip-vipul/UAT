using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OracleClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;
using PrjUpassDAL.Authentication;
using System.IO;
using System.Resources;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayLcoPayment : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        string username;


        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Reciept Entry";
            if (!IsPostBack)
            {
                
                if (Session["operator_id"] != null)
                {
                    Session["RightsKey"] = null;
                    divdet.Visible = false;
                    username = Convert.ToString(Session["username"]);
                  
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                Master.PageHeading = "Recipt Entery";
                divDateBox.Style["display"] = "none";
                divDateLabel.Style["display"] = "none";
                divChqDDno.Style["display"] = "none";
                lblmode.Text = "";
                lblbanknme.Text = "";
                lblbranch.Text = "";
                lblDateStar.Text = "";
            }
            txtFrom.Attributes.Add("readonly", "readonly");
            
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchBankName(string prefixText, int count)
        {
            string Str = prefixText.Trim();

            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            str += " SELECT a.num_bank_id, a.var_bank_bankcd, a.var_bank_name ";
            str += " FROM view_bank_def a ";
            str += " where upper(var_bank_name) like upper('" + prefixText + "%') ";

            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                    dr["var_bank_name"].ToString(), dr["num_bank_id"].ToString());
                Operators.Add(item);
            }
            con.Close();
            con.Dispose();
            return Operators;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> SearchOperators(string prefixText, int count, string contextKey)
        {
            string Str = prefixText.Trim();
            string catid = "";
            string operid = "";
            if (HttpContext.Current.Session["category"] != null && HttpContext.Current.Session["operator_id"] != null)
            {
                catid = HttpContext.Current.Session["category"].ToString();
                operid = HttpContext.Current.Session["operator_id"].ToString();
            }

            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            if (contextKey == "1")
            {
                str = "  SELECT a.lcoid, a.distid, a.msoid, a.hoid, a.lcomstcode, a.lcomstname, ";
                str += "  a.lcomstaddress, a.lcomstmobileno, a.lcomstemail, ";
                str += "  a.currentcreditlimit ";
                str += "  FROM veiw_lcopre_paylco_search a ";
                str += " where upper(a.LCOMSTNAME) like upper('" + prefixText + "%') ";
            }
            else if (contextKey == "0")
            {
                str = "  SELECT a.lcoid, a.distid, a.msoid, a.hoid, a.lcomstcode, a.lcomstname, ";
                str += "  a.lcomstaddress, a.lcomstmobileno, a.lcomstemail, ";
                str += "  a.currentcreditlimit ";
                str += "  FROM veiw_lcopre_paylco_search a ";
                str += " where upper(a.lcomstcode) like upper('" + prefixText + "%') ";
            }
            if (catid == "2")
            {
                str += " and a.msoid = " + operid;
            }
            else if (catid == "5")
            {
                str += " and a.distid = " + operid;
            }
            else if (catid == "10")
            {
                str += " and a.HOID = " + operid;
            }
            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (contextKey == "1")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["LCOMSTNAME"].ToString(), dr["LCOMSTNAME"].ToString());
                    Operators.Add(item);
                }
                else
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["LCOMSTCODE"].ToString(), dr["LCOMSTCODE"].ToString());
                    Operators.Add(item);
                }
            }
            //string[] prefixTextArray = Operators.ToArray<string>();
            con.Close();
            con.Dispose();
            return Operators;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            btnSubmit.Visible = true;
            lblmsg.Text = "";
            if (txtLCOSearch.Text.Trim() != "")
            {
                string customerId = Request.Form[hfCustomerId.UniqueID];
                string customerName = Request.Form[txtLCOSearch.UniqueID];
                Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
                string operator_id = "";
                string category_id = "";
                if (Session["operator_id"] != null && Session["category"] != null)
                {
                    operator_id = Convert.ToString(Session["operator_id"]);
                    category_id = Convert.ToString(Session["category"]);
                }
                string[] responseStr = obj.getLcodetails(username, txtLCOSearch.Text.Trim(), RadSearchby.SelectedValue, operator_id, category_id);
                if (responseStr.Length != 0)
                {
                    lblCustNo.Text = responseStr[0].Trim();
                    lblCustName.Text = responseStr[1].Trim();
                    lblCustAddr.Text = responseStr[2].Trim();
                    lblmobno.Text = responseStr[3].Trim();
                    lblEmail.Text = responseStr[4].Trim();
                    lblCurrBalance.Text = responseStr[5].Trim();

                    divdet.Visible = true;
                    LCOAccordion.Visible = true;
                    LCOAccordion.SelectedIndex = 0;
                }
                else
                {
                    msgbox("No Such LCO Found", txtLCOSearch);
                    LCOAccordion.Visible = false;
                    return;
                }
            }
            else
            {
                msgbox("Please Select LCO by code or name", txtLCOSearch);
                LCOAccordion.Visible = false;
                return;
            }
        }

        public void msgbox(string message, Control ctrl)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
            ctrl.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            lblmsg.Text = "";
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            if (txtLCOSearch.Text.Trim() == "" || lblCustNo.Text.Trim() == "")
            {
                msgbox("Please Select LCO", txtLCOSearch);
                return;
            }
            else if (txtCashAmt.Text.Trim() == "")
            {
                msgbox("Please Enter Amount", txtCashAmt);
                return;
            }
            else if (RBPaymode.SelectedValue == "Q" && txtChqDDno.Text.Trim() == "")
            {
                msgbox("Please Enter Cheque Number", txtChqDDno);
                return;
            }
            else if (RBPaymode.SelectedValue == "DD" && txtChqDDno.Text.Trim() == "")
            {
                msgbox("Please Enter DD Number", txtChqDDno);
                return;
            }
            else if (RBPaymode.SelectedValue == "N" && txtChqDDno.Text.Trim() == "")
            {
                msgbox("Please Enter Reference Number", txtChqDDno);
                return;
            }
            if (RBPaymode.SelectedValue.ToString() == "Q" || RBPaymode.SelectedValue.ToString() == "DD")
            {
                if (txtFrom.Text.Trim() == "")
                {
                    msgbox("Please select Cheque/DD date", txtFrom);
                    return;
                }
                if (txtBankName.Text.Trim() == "" || txtbranchnm.Text.Trim() == "")
                {
                    msgbox("Please select bank and branch", txtBankName);
                    return;
                }
            }
            if (RBPaymode.SelectedValue.ToString() == "N")
            {
                if (txtBankName.Text.Trim() == "")
                {
                    msgbox("Please select bank", txtBankName);
                    return;
                }
            }

            btnSubmit.Visible = false;

            Hashtable ht = new Hashtable();
            string loggedInUser;
            if (Session["username"] != null)
            {
                loggedInUser = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            ht.Add("User", loggedInUser);
            ht.Add("CustCode", lblCustNo.Text.Trim());
            ht.Add("Amount", Convert.ToInt32(txtCashAmt.Text.Trim()));
            ht.Add("PayMode", RBPaymode.SelectedValue.ToString());

            if (RBPaymode.SelectedValue.ToString() == "Q" || RBPaymode.SelectedValue.ToString() == "DD")
            {
                ht.Add("chequeddno", txtChqDDno.Text.Trim());
                ht.Add("CheckDate", Convert.ToDateTime(txtFrom.Text.Trim()));
            }
            else if (RBPaymode.SelectedValue.ToString() == "N")
            {
                ht.Add("chequeddno", txtChqDDno.Text.Trim());
                ht.Add("CheckDate", DateTime.MinValue);
            }

         else
            {
                ht.Add("chequeddno", "N/A");
                ht.Add("CheckDate", DateTime.MinValue);
            }
            ht.Add("BankName", hfBankId.Value.Trim());
            ht.Add("Branch", txtbranchnm.Text.Trim());
            ht.Add("Remark", txtRemark.Text.Trim());
            ht.Add("ReceiptNo", txtReceipt.Text.Trim());
            ht.Add("IP", Ip);
            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
       string response = obj.LcoPayment(ht);
        if (response == "ex_occured")
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            string[] strrnw = new string[4];

            strrnw = response.Split(',');

            lblmsg.Text = strrnw[1];
            //resetForNextTrans();

            if (response.StartsWith("9999"))
            {
                Session["rcpt_pt_rcptno1"] = strrnw[2].ToString();
                Session["rcpt_pt_date1"] = strrnw[3].ToString();
                Session["rcpt_pt_cashiername"] = strrnw[4].ToString();
                Session["rcpt_pt_address"] = strrnw[5].ToString();
                Session["rcpt_pt_company"] = strrnw[6].ToString();
                //Session["rcpt_pt_details"] = strrnw[7].ToString();
                Session["rcpt_pt_lcocd1"] = lblCustNo.Text;
                Session["rcpt_pt_lconm1"] = lblCustName.Text;
                //Session["cashier"] = Session["name"].ToString();
                /*
                    Str += "," + out_BASEamt.ToString();
                    Str += "," + out_ET.ToString();
                    Str += "," + out_ETamt.ToString();
                    Str += "," + out_ST.ToString();
                    Str += "," + out_STamt.ToString();
                    Str += "," + out_EC.ToString();
                    Str += "," + out_ECamt.ToString();
                    Str += "," + out_HEC.ToString();
                    Str += "," + out_HECamt.ToString();
                 
                Session["rcpt_pt_baseamt"] = strrnw[7].ToString();
                Session["rcpt_pt_et"] = strrnw[8].ToString();
                Session["rcpt_pt_etamt"] = strrnw[9].ToString();
                Session["rcpt_pt_st"] = strrnw[10].ToString();
                Session["rcpt_pt_stamt"] = strrnw[11].ToString();
                Session["rcpt_pt_ec"] = strrnw[12].ToString();
                Session["rcpt_pt_ecamt"] = strrnw[13].ToString();
                Session["rcpt_pt_hec"] = strrnw[14].ToString();
                Session["rcpt_pt_hecamt"] = strrnw[15].ToString(); 
                */

                Session["rcpt_pt_amt1"] = txtCashAmt.Text;
                Session["rcpt_pt_paymode1"] = RBPaymode.SelectedItem.ToString();
                Session["rcpt_pt_cheqno1"] = txtChqDDno.Text;
                Session["rcpt_pt_bnknm1"] = txtBankName.Text.Trim();//hfBankId.Value.ToString();
                Session["rcpt_pt_premark1"] = txtRemark.Text.Trim();
                reset();
                lblmsg.Text = response.Split(',')[1];
                Response.Write("<script language='javascript'> window.open('../Transaction/rcptPaymentReceiptInvoice.aspx', 'Print_Receipt','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes,location=no,status=no');</script>");
            }
            else
            {
                lblmsg.Text = response;
            }
        }

        protected void reset()
        {
            txtbranchnm.Text = "";
            txtCashAmt.Text = "";
            txtChqDDno.Text = "";
            txtFrom.Text = "";
            txtLCOSearch.Text = "";
            txtReceipt.Text = "";
            txtRemark.Text = "";
            txtBankName.Text = "";
            RBPaymode.SelectedValue = "C";
            lblmsg.Text = "";
            divdet.Visible = false;
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            reset();
        }

        protected void RBPaymode_SelectedIndexChanged(object sender, EventArgs e)
        {
            divDateBox.Style["display"] = "block";
            divDateLabel.Style["display"] = "block";
            divChqDDno.Style["display"] = "block";
            if (RBPaymode.SelectedValue == "Q" || RBPaymode.SelectedValue == "DD" || RBPaymode.SelectedValue == "N")
            {
                lblmode.Text = "*";
                lblbanknme.Text = "*";
                lblbranch.Text = "*";
                lblDateStar.Text = "*";
                if (RBPaymode.SelectedValue == "Q")
                {
                    //txtWatermark.WatermarkText = "Cheque Number";
                    txtChqDDno.MaxLength = 6;
                }
                else if (RBPaymode.SelectedValue == "N")
                {
                    //txtWatermark.WatermarkText = "Reference No";
                    txtChqDDno.MaxLength = 12;
                    lblbranch.Text = "";
                    divDateBox.Style["display"] = "none";
                    divDateLabel.Style["display"] = "none";
                }
                else if (RBPaymode.SelectedValue == "DD")
                {
                    //txtWatermark.WatermarkText = "DD Number";
                    txtChqDDno.MaxLength = 6;
                }
             
                else
                {
                    txtWatermark.WatermarkText = " ";
                    txtChqDDno.MaxLength = 6;
                }
            }
            else
            {
                lblmode.Text = "";
                lblbanknme.Text = "";
                lblbranch.Text = "";
                lblDateStar.Text = "";
                divDateBox.Style["display"] = "none";
                divDateLabel.Style["display"] = "none";
                divChqDDno.Style["display"] = "none";
            }
        }
    }
}