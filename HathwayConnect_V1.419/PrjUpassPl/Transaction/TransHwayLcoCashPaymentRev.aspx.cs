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
    public partial class TransHwayLcoPaymentRev_Replica : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        string operid;
        string username;
        string catid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["operator_id"] != null)
                {
                    Session["RightsKey"] = null;
                    operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    catid = Convert.ToString(Session["category"]);
                    reset();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchOperators(string prefixText, int count)
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

            str = "  select VAR_LCOPAY_RECEIPTNO";
            str += " from VIEW_LCOPRE_CASH_PAYMENT_REV";
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
        protected void reset()
        {
            lblLcoAmt.Text = "";
            lblLcoPaymode.Text = "";
            //lblLcoBank.Text = "";
            //lblLcoBranch.Text = "";
            lblLcoCode.Text = "";
            lblLcoName.Text = "";
            lblLcoAddr.Text = "";
            lblLcoMobno.Text = "";
            lblLcoEmail.Text = "";
            txtRemark.Text = "";
            lblmsg.Text = "";
            divdet.Visible = false;
            ddlReason.Items.Clear();
            ddlReason.Items.Insert(0, new ListItem("Select Reason", "0"));
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
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
                msgbox("Please Enter Receipt No.", txtLCOSearch);
                return;
            }
            else if (ddlReason.SelectedIndex == 0)
            {
                msgbox("Please select Reason", ddlReason);
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
            ht.Add("IP", Ip);

            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
            string response = obj.LcoCashPaymentRevarsal(ht);
            reset();
            if (response == "ex_occured")
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            lblmsg.Text = response;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtLCOSearch.Text.Trim() != "")
            {
                lblmsg.Text = "";
                string customerId = Request.Form[hfCustomerId.UniqueID];
                string customerName = Request.Form[txtLCOSearch.UniqueID];
                Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
                string cat_id = "";
                string oper_id = "";
                string logged_in_user = "";
                if (HttpContext.Current.Session["category"] != null && HttpContext.Current.Session["operator_id"] != null)
                {
                    cat_id = HttpContext.Current.Session["category"].ToString();
                    oper_id = HttpContext.Current.Session["operator_id"].ToString();
                    logged_in_user = HttpContext.Current.Session["username"].ToString();
                }

                string[] responceStr = obj.GetLcoCashpaymentDetails(logged_in_user, txtLCOSearch.Text.Trim(), cat_id, oper_id);
                if (responceStr.Length != 0)
                {
                    lblLcoCode.Text = responceStr[0].Trim();
                    lblLcoName.Text = responceStr[1].Trim();
                    lblLcoAddr.Text = responceStr[2].Trim();
                    lblLcoMobno.Text = responceStr[3].Trim();
                    lblLcoEmail.Text = responceStr[4].Trim();
                    lblLcoAmt.Text = responceStr[5].Trim();
                    lblLcoPaymode.Text = (responceStr[6].Trim() == "C") ? "Cash" : ((responceStr[6].Trim() == "Q") ? "Cheque" : ((responceStr[6].Trim() == "DD") ? "Demand Draft" : ((responceStr[6].Trim() == "N") ? "NEFT" : "")));
                        //(responceStr[6].Trim() == "C") ? "Cash" : ((responceStr[6].Trim() == "Q") ? "Cheque" : "");
                    ViewState["ReceiptNo"] = responceStr[7].Trim();

                    DataSet ds = Cls_Helper.Comboupdate("aoup_reasons_def", "num_reason_id", "var_reason_name");
                    ddlReason.DataSource = ds;
                    ddlReason.DataTextField = "var_reason_name";
                    ddlReason.DataValueField = "num_reason_id";
                    ddlReason.DataBind();
                    ds.Dispose();
                    ddlReason.Items.Insert(0, "Select Reason");
                    divdet.Visible = true;
                }
                else
                {
                    msgbox("Invalid Receipt", txtLCOSearch);
                    reset();
                    return;
                }
            }
            else
            {
                msgbox("Please Enter Receipt No.", txtLCOSearch);
                return;
            }
        }

    }
}