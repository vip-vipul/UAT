using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;
using PrjUpassBLL.Master;
using System.Data.OracleClient;
using System.Configuration;

namespace PrjUpassPl.Master
{
    public partial class frmLCOPreMSOUserDefine : System.Web.UI.Page
    {
        string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["operator_id"] != null)
                {
                    Session["RightsKey"] = null;
                    string operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    string catid = Convert.ToString(Session["category"]);
                    pnlDetails.Visible = false;
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

            str = "SELECT a.num_comp_companyid, a.var_comp_companyname, a.num_comp_distid, ";
            str += "  a.var_comp_distname, a.num_comp_subdistid, ";
            str += "  a.var_comp_subdistname, a.var_comp_insby, a.dat_comp_insdt, ";
            str += " a.var_comp_updby, a.dat_comp_upddt, a.num_comp_operid ";
            str += " FROM view_lcopre_company_det a ";
            str += " where upper(a.var_comp_companyname) like  upper('" + prefixText.ToString() + "%')";

            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                    dr["var_comp_companyname"].ToString(), dr["var_comp_companyname"].ToString());
                Operators.Add(item);

            }

            con.Close();
            con.Dispose();
            return Operators;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadMSoSection();
            loadState();
            loadCity();

            string username;
            if (Session["username"] != null)
            {
                username = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

        }

        protected void loadMSoSection()
        {
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                Cls_Business_MstLCOPreMSOUDefine obj = new Cls_Business_MstLCOPreMSOUDefine();
                Hashtable htResponse = obj.GetTransations(txtCompanySearch.Text, username);

                DataTable dt = null;
                if (htResponse["htResponse"] != null)
                {
                    dt = (DataTable)htResponse["htResponse"];
                }

                if (dt == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    lblCompanyName.Text = "";
                    lblResponseMsg.Text = "No data found...";
                    pnlDetails.Visible = false;
                }
                else
                {

                    lblCompanyName.Text = dt.Rows[0]["var_comp_companyname"].ToString();
                    ViewState["state"] = dt.Rows[0]["var_comp_state"].ToString();
                    ViewState["city"] = dt.Rows[0]["var_comp_city"].ToString();
                    lblResponseMsg.Text = "";

                    pnlDetails.Visible = true;

                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void loadCity()
        {

            string where_str = " WHERE var_city_companycode='HWP' AND var_city_name='" + ViewState["city"].ToString() + "'";
            DataSet ds = Cls_Helper.Comboupdate("aoup_lcopre_city_def " + where_str + " ORDER BY var_city_name asc", "num_city_id", "var_city_name");
            ddlCity.DataSource = ds;
            ddlCity.DataTextField = "var_city_name";
            ddlCity.DataValueField = "num_city_id";
            ddlCity.DataBind();
            ddlCity.Dispose();


        }

        protected void loadState()
        {
            DataSet dsStates = Cls_Helper.Comboupdate("AOUP_LCOPRE_STATE_DEF WHERE VAR_PLAN_COMPANYCODE='HWP' and VAR_STATE_NAME='" + ViewState["state"].ToString() + "'  ORDER BY VAR_STATE_NAME asc", "NUM_STATE_ID", "VAR_STATE_NAME");
            ddlState.DataSource = dsStates;
            ddlState.DataTextField = "VAR_STATE_NAME";
            ddlState.DataValueField = "NUM_STATE_ID";
            ddlState.DataBind();
            dsStates.Dispose();

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();

            ht["companyname"] = lblCompanyName.Text;

            string flag = "";
            if (rbtnCashier.Checked == true)
            {
                flag = "C";
            }
            else
            {
                flag = "F";
            }
            ht["flag"] = flag;

            string userid1 = txtUserId.Text;
            ht["loginid"] = userid1;
           
            string fname = txtFirstName.Text;
            ht["fname"] = fname;
            string mname = txtMidName.Text;
            ht["mname"] = mname;
            string lname = txtLastName.Text;
            ht["lname"] = lname;
            string jv = "0";
            string direct = "0";
            if (rdJV.Checked)
            {
                jv = txtJvDirNo.Text;
            }
            else
            {
                direct = txtJvDirNo.Text;
            }
            ht["jv"] = jv;
            ht["direct"] = direct;
            string brmpoid = txtBrmId.Text;
            ht["brmpoid"] = brmpoid;

            string addr = txtAddress.Text;
            ht["addr"] = addr;
            string state = ddlState.SelectedValue.ToString();

            ht["state"] = state;
            string city = ddlCity.SelectedValue.ToString();

            ht["city"] = city;
            string pincode = txtPincode.Text;
            ht["pincode"] = pincode;
            string mobileno = txtMobile.Text;
            ht["mobileno"] = mobileno;
            string email = txtEmail.Text;
            ht["email"] = email;

            Cls_Business_MstLCOPreMSOUDefine obj = new Cls_Business_MstLCOPreMSOUDefine();
            string username = "";
            string compcode = "";
            if (Session["username"] != null)
            {
                username = Convert.ToString(Session["username"]);
                compcode = Convert.ToString(Session["opr_code"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            ht["compcode"] = "HWP";//compcode;
            string response = obj.setUserData(username, ht);
            reset();
            if (response == "ex_occured")
            {
                //exception occured
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            else
            {
                lblResponseMsg.Text = response;
            }
        }

        protected void reset()
        {
            txtCompanySearch.Text = "";
            lblCompanyName.Text = "";
           
            
            
            txtUserId.Text = "";
            txtFirstName.Text = "";
            txtMidName.Text = "";
            txtLastName.Text = "";
            txtJvDirNo.Text = "";
            

            txtAddress.Text = "";
            txtPincode.Text = "";
            txtMobile.Text = "";
            txtEmail.Text = "";

            rdDirect.Checked = true;
            rbtnCashier.Checked = true;
            lblResponseMsg.Text = "";
            ddlCity.Items.Clear();
            ddlState.Items.Clear();

            pnlDetails.Visible = false;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }
    }
}