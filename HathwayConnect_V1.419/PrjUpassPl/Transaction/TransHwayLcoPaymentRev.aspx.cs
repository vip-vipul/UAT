using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using PrjUpassBLL.Transaction;
using PrjUpassDAL.Helper;
using PrjUpassDAL.Authentication;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayLcoPaymentRev : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        string operid;
        string username;
        string catid;
        //static string type;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["operator_id"] != null)
                {
                    Session["RightsKey"] = null;
                    rbtnsearch.SelectedIndex = 0;
                    operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    catid = Convert.ToString(Session["category"]);
                    reset();
                    DataSet ds = Cls_Helper.Comboupdate("aoup_bank_def order by var_bank_name asc ", "num_bank_id", "var_bank_name");
                    ddlBankName.DataSource = ds;
                    ddlBankName.DataTextField = "var_bank_name";
                    ddlBankName.DataValueField = "num_bank_id";
                    ddlBankName.DataBind();
                    ds.Dispose();
                    ddlBankName.Items.Insert(0, "Select Bankname");
                    //if (rbtnsearch.SelectedValue == "0")
                    //{
                    //    type = "0";
                    //}
                    //else
                    //{
                    //    type = "1";
                    //}
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                txtChequeBounceDate.Attributes.Add("readonly", "readonly");
            }
            //   btnSubmit.Attributes.Add("onclick", "javascript:return dovalid1()");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchOperators(string prefixText, int count, string contextKey)
        {
            string Str = prefixText.Trim();
            double Num;
            bool isNum = double.TryParse(Str, out Num);
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            string catid = "";
            string operid = "";
            if (HttpContext.Current.Session["category"] != null && HttpContext.Current.Session["operator_id"] != null)
            {
                catid = HttpContext.Current.Session["category"].ToString();
                operid = HttpContext.Current.Session["operator_id"].ToString();
            }

            if (contextKey == "0")
            {
                str = "  select VAR_LCOMST_CODE, VAR_LCOMST_NAME, VAR_LCOMST_ADDRESS, NUM_LCOMST_MOBILENO, ";
                str += " VAR_LCOMST_EMAIL, NUM_LCOPAY_AMOUNT, VAR_LCOPAY_PAYMODE, BNKNM, VAR_LCOPAY_BRANCH,";
                str += " VAR_LCOPAY_RECEIPTNO, OPERID, OPERCATEGORY, PARENTID, DISTID from view_lcopre_payment_rev";
                str += " where upper(VAR_LCOPAY_RECEIPTNO) Like upper('" + prefixText + "%')";

                if (catid == "2")
                {
                    str += " and parentid='" + operid.ToString() + "'  ";
                }
                if (catid == "5")
                {
                    str += " and distid='" + operid.ToString() + "'  ";
                }
                else if (catid == "10")
                {

                    str += " and HOID='" + operid.ToString() + "'  ";
                }
                OracleCommand cmd = new OracleCommand(str, con);

                con.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["VAR_LCOPAY_RECEIPTNO"].ToString(), dr["VAR_LCOPAY_RECEIPTNO"].ToString());
                    Operators.Add(item);
                }
                //string[] prefixTextArray = Operators.ToArray<string>();
                con.Close();
                con.Dispose();
                return Operators;
            }
            else
            {
                return null;
            }



            //string[] responseStr = null;
            //List<string> Operators = new List<string>();
            //Operators.Add("ranjan singh");
            //Operators.Add("ranjit mahajan");
            //Operators.Add("rinky sharma");
            //Operators.Add("rukhsana shaikh");
            //Operators.Add("ryan oberoy");

            //string[] prefixTextArray = Operators.ToArray<string>();
            //return prefixTextArray;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
            //Response.Redirect("TransHwayLcoPaymentRev.aspx");
        }

        protected void reset()
        {
            lblLcoAmt.Text = "";
            lblLcoPaymode.Text = "";
            lblLcoBank.Text = "";
            lblLcoBranch.Text = "";
            lblLcoCode.Text = "";
            lblLcoName.Text = "";
            lblLcoAddr.Text = "";
            lblLcoMobno.Text = "";
            lblLcoEmail.Text = "";
            txtRemark.Text = "";
            lblmsg.Text = "";
            txtChequeBounceDate.Text = "";
            divdet.Visible = false;
            ddlReason.Items.Clear();
            ddlReason.Items.Insert(0, new ListItem("Select Reason", "0"));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (rbtnsearch.SelectedValue.ToString() == "1")
            {
                if (ddlBankName.SelectedIndex == 0)
                {
                    msgbox("Please Select Bank Name", ddlBankName);
                    return;
                }
            }
            if (txtLCOSearch.Text.Trim() != "")
            {
                string customerId = Request.Form[hfCustomerId.UniqueID];
                string customerName = Request.Form[txtLCOSearch.UniqueID];
                Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
                // string[] responseStr = obj.getLcodetails(username, txtLCOSearch.Text.Trim(), RadSearchby.SelectedValue, operid);
                string loggedInUser = "";
                string operator_id = "";
                string search_type = "";
                if (Session["username"] != null && Session["operator_id"] != null)
                {
                    loggedInUser = Session["username"].ToString();
                    operator_id = Session["operator_id"].ToString();
                }
                if (rbtnsearch.SelectedValue == "0")
                {
                    search_type = "0";
                }
                else
                {
                    search_type = "1";
                }
                string[] responceStr = obj.GetLcopaymentDetails(loggedInUser, txtLCOSearch.Text.Trim(), catid, operator_id, search_type, ddlBankName.SelectedValue.ToString());
                if (responceStr.Length != 0)
                {
                    lblLcoCode.Text = responceStr[0].Trim();
                    lblLcoName.Text = responceStr[1].Trim();
                    lblLcoAddr.Text = responceStr[2].Trim();
                    lblLcoMobno.Text = responceStr[3].Trim();
                    lblLcoEmail.Text = responceStr[4].Trim();

                    lblLcoAmt.Text = responceStr[5].Trim();
                    lblLcoPaymode.Text = (responceStr[6].Trim() == "C") ? "Cash" : ((responceStr[6].Trim() == "Q") ? "Cheque" : ((responceStr[6].Trim() == "DD") ? "Demand Draft" : ((responceStr[6].Trim() == "N") ? "NEFT" : "")));
                    lblLcoBank.Text = responceStr[7].Trim();
                    lblLcoBranch.Text = responceStr[8].Trim();

                    lblChequeNo.Text = responceStr[9].Trim();
                    lblChequeDate.Text = responceStr[10].Trim();
                    ViewState["ReceiptNo"] = responceStr[11].Trim();


                    DataSet ds = Cls_Helper.Comboupdate("aoup_reasons_def", "num_reason_id", "var_reason_name");
                    ddlReason.DataSource = ds;
                    ddlReason.DataTextField = "var_reason_name";
                    ddlReason.DataValueField = "num_reason_id";
                    ddlReason.DataBind();
                    ds.Dispose();
                    ddlReason.Items.Insert(0, "Select Reason");

                    divdet.Visible = true;
                    DetailsAccordion.SelectedIndex = -1;
                }
                else
                {
                    if (rbtnsearch.SelectedValue.ToString() == "0")
                    {
                        msgbox("Invalid Receipt", txtLCOSearch);
                        return;
                    }
                    else
                    {
                        msgbox("Invalid Cheque/DD No.", txtLCOSearch);
                        return;
                    }
                }
            }
            else
            {
                if (rbtnsearch.SelectedValue.ToString() == "0")
                {
                    msgbox("Please Enter Receipt No.", txtLCOSearch);
                    return;
                }
                else if (rbtnsearch.SelectedValue.ToString() == "1")
                {
                    msgbox("Please Enter Cheque No.", txtLCOSearch);
                    return;
                }
            }
        }

        public void msgbox(string message, Control ctrl)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
            ctrl.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);

            if (txtLCOSearch.Text.Trim() == "")
            {
                if (rbtnsearch.SelectedValue.ToString() == "0")
                {
                    msgbox("Please Enter Receipt No.", txtLCOSearch);
                    return;
                }
                else if (rbtnsearch.SelectedValue.ToString() == "1")
                {
                    msgbox("Please Enter Cheque No.", txtLCOSearch);
                    return;
                }
            }
            else if (ddlReason.SelectedIndex == 0)
            {
                msgbox("Please select Reason", ddlReason);
                return;
            }
            if (txtChequeBounceDate.Text == "")
            {
                msgbox("Please Select Cheque Bounce Date", txtChequeBounceDate);
                return;
            }
            else if (txtRemark.Text.Trim() == "")
            {
                msgbox("Please enter remark", txtRemark);
                return;
            }
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
            //   ht.Add("LcoCode", Convert.ToInt32(lblLcoCode.Text.Trim()));
            ht.Add("LcoCode", lblLcoCode.Text.Trim());
            ht.Add("ReceiptNo", ViewState["ReceiptNo"].ToString());
            ht.Add("Amount", Convert.ToInt32(lblLcoAmt.Text.Trim()));
            ht.Add("Remark", txtRemark.Text.Trim());
            ht.Add("Reason", ddlReason.SelectedValue.ToString());
            ht.Add("ChequeBounceDate", txtChequeBounceDate.Text.ToString());
            ht.Add("IP", Ip);

            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
            string response = obj.LcoPaymentRev(ht);
            reset();
            if (response == "ex_occured")
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            lblmsg.Text = response;

        }

        protected void rbtnsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtnsearch.SelectedValue == "0")
            {

                tdBankName.Visible = false;
                //type = "0";
            }
            else if (rbtnsearch.SelectedValue == "1")
            {

                tdBankName.Visible = true;
                //type = "1";
            }
        }
    }
}