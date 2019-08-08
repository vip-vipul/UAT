using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;
using PrjUpassBLL.Master;

namespace PrjUpassPl.Master
{
    public partial class mstHwayLCOMsgBroadcast : System.Web.UI.Page
    {
        //static string type;
        string catid;
        string operid;
        string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (RadSearchby.SelectedValue.ToString() == "0")
            //{
            //    type = "0";
            //}
            //else
            //{
            //    type = "1";
            //}

            if (Session["username"] != null && Session["category"] != null && Session["operator_id"] != null) {
               
                catid = Session["category"].ToString();
                operid = Session["operator_id"].ToString();
                username = Session["username"].ToString();
            }

            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchOperators(string prefixText, int count, string contextKey)
        {
            string Str = prefixText.Trim();
            double Num;
            bool isNum = double.TryParse(Str, out Num);
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            string catid = "";
            string operid = "";
            if (HttpContext.Current.Session["category"] != null && HttpContext.Current.Session["operator_id"] != null)
            {
                catid = HttpContext.Current.Session["category"].ToString();
                operid = HttpContext.Current.Session["operator_id"].ToString();
            }
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            str = "SELECT lconame, lcoid, lcocode, operid ";
            str += " FROM view_lcopre_msglco_search ";
            str += " WHERE lcoid is not null ";
            if (contextKey == "0")
            {
                str += "  and upper(lcocode) like upper('" + prefixText.ToString() + "%')";
            }
            else if (contextKey == "1")
            {
                str += "  and upper(lconame) like  upper('" + prefixText.ToString() + "%')";
            }
            if (catid == "2")
            {
                str += " and PARENTID='" + operid.ToString() + "'  ";
            }
            else if (catid == "5")
            {
                str += " and DISTID='" + operid.ToString() + "'  ";
            }
            else if (catid == "10")
            {
                str += " and HO_OPERID='" + operid.ToString() + "'  ";
            }
            
            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (contextKey == "0")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["lcocode"].ToString(), dr["operid"].ToString());
                    Operators.Add(item);
                }
                else if (contextKey == "1")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                            dr["lconame"].ToString(), dr["operid"].ToString());
                    Operators.Add(item);
                }
            }
            con.Close();
            con.Dispose();
            return Operators;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string lco_oper_id = hdnLcoOperId.Value;
            if (lco_oper_id == "") {
                lblResponseMsg.Text = "Search and select LCO first and try again";
                return;
            }
            string message = txtBrodcastermsg.Text;
            string flag = "D";
            if (rbtnFlag.SelectedValue == "1")
            {
                flag = "C";
            }
            Hashtable ht = new Hashtable();
            ht["message"] = message;
            ht["operator"] = lco_oper_id;
            ht["flag"] = flag;
            Cls_Business_MstHwaymsgBrodcaster objBroad = new Cls_Business_MstHwaymsgBrodcaster();
            string response = "";
            if (rbtnFlag.SelectedValue == "1" && txtBrodcastermsg.Text.Trim() == "")
            {
                response = "Message concated successfully";
            }
            else
            {
                response = objBroad.SetLCOBrodcastMsg(username, ht);
            }
            //
            if (response == "ex_occured")
            {
                lblResponseMsg.Text = "Something went wrong while setting message";
            }
            else {
                lblResponseMsg.Text = response;
                txtBrodcastermsg.Text = "";
                txtLCOSearch.Text = "";
                hdnLcoOperId.Value = "";
                lblSearchedValue.Text = "";
                lblSearchType.Text = "";
            }
        }
    }
}