using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OracleClient;
using PrjUpassBLL.Transaction;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassPl.Master
{
    public partial class FrmHwayUserCreditLimit : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        static string operid;
        static string username;
        static string catid;
        static string type;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["operator_id"] != null)
                {
                    Session["RightsKey"] = null;
                    divdet.Visible = false;
                    operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    catid = Convert.ToString(Session["category"]);
                    if (RadSearchby.SelectedValue.ToString() == "0")
                    {
                        type = "0";
                    }
                    else
                    {
                        type = "1";
                    }
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
            if (!isNum)
            {
                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection con = new OracleConnection(strCon);
                string str = "";
                if (type == "1")
                {

                    str = " select MSOID, MSONAME, DISTID, DISTNAME, LCOID, LCONAME, USERNAME, USEROWNER, ";
                    str += "INSDT, TOTLCOCNT,LCOMSTCODE,LCOMSTNAME,LCOMSTADDRESS,LCOMSTMOBILENO,LCOMSTEMAIL,LCOMSTCODE ";
                    str += " from view_hway_userdet ";
                    str += " where upper(LCOMSTNAME) like upper('" + prefixText + "%') ";
                    //    str = " select a.var_lcomst_code, a.var_lcomst_name, a.var_lcomst_address, a.num_lcomst_mobileno, a.var_lcomst_email " +
                    //" from aoup_lcopre_lco_det a where upper(a.var_lcomst_name) like upper('" + prefixText + "%') " +
                    //" and rownum = '1'";
                }
                else if (type == "0")
                {
                    str = " select MSOID, MSONAME, DISTID, DISTNAME, LCOID, LCONAME, USERNAME, USEROWNER, ";
                    str += "INSDT, TOTLCOCNT,LCOMSTCODE,LCOMSTNAME,LCOMSTADDRESS,LCOMSTMOBILENO,LCOMSTEMAIL,LCOMSTCODE ";
                    str += " from view_hway_userdet ";
                    str += " where LCOMSTCODE = '" + prefixText + "' ";

                    //str = " select a.var_lcomst_code, a.var_lcomst_name, a.var_lcomst_address, a.num_lcomst_mobileno, a.var_lcomst_email " +
                    //               " from aoup_lcopre_lco_det a where upper(a.var_lcomst_code) = '" + prefixText + "'" +
                    //               " and rownum = '1'";
                }
                if (catid == "2")
                {
                    str += " and msoid = " + operid;
                }
                else if (catid == "5")
                {
                    str += " and distid = " + operid;
                }
                OracleCommand cmd = new OracleCommand(str, con);

                con.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (type == "1")
                    {
                        string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                            dr["LCOMSTNAME"].ToString(), dr["LCOMSTNAME"].ToString());
                        Operators.Add(item);
                    }
                }
                //string[] prefixTextArray = Operators.ToArray<string>();
                con.Close();
                con.Dispose();
                return Operators;
            }
            else
                return null;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransHwayLcoPayment.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string customerId = Request.Form[hfCustomerId.UniqueID];
            string customerName = Request.Form[txtUSERSearch.UniqueID];
            if (txtUSERSearch.Text.Trim() != "0")
            {
                Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
                string[] responseStr = obj.getLcodetails(username, txtUSERSearch.Text, RadSearchby.SelectedValue, operid,catid);
                if (responseStr.Length != 0)
                {
                    lblCustNo.Text = responseStr[0].Trim();
                    lblCustName.Text = responseStr[1].Trim();
                    lblCustAddr.Text = responseStr[2].Trim();
                    lblmobno.Text = responseStr[3].Trim();
                    lblEmail.Text = responseStr[4].Trim();

                    DataSet ds = Cls_Helper.Comboupdate("aoup_bank_def", "num_bank_id", "var_bank_name");
                    ddlBankName.DataSource = ds;
                    ddlBankName.DataTextField = "var_bank_name";
                    ddlBankName.DataValueField = "num_bank_id";
                    ddlBankName.DataBind();
                    ds.Dispose();
                    ddlBankName.Items.Insert(0, "Select Bankname");
                    divdet.Visible = true;
                }
                else
                {
                    msgbox("No Such LCO Found", txtUSERSearch);
                    return;
                }
            }
            else
            {
                msgbox("Please Select LCO by code or name", txtUSERSearch);
                return;
            }
        }

        public void msgbox(string message,Control ctrl)
        {
            string msg = "<script type=\"text/javascript\">alert(\"" + message + "\");</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", msg);
            ctrl.Focus();
        }

        protected void RadSearchby_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadSearchby.SelectedValue.ToString() == "0")
            {
                type = "0";
            }
            else
            {
                type = "1";
            }
        }
    }
}