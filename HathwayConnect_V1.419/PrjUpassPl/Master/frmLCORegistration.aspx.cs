using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Master;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;

namespace PrjUpassPl.Master
{
    public partial class frmLCORegistration : System.Web.UI.Page
    {
        string operid;
        string username;
        string catid;
        //static string type;
        //string city;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                operid = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                divdetails.Visible = false;

                //if (RadSearchby.SelectedValue.ToString() == "0")
                //{
                //    type = "0";
                //}
                //else
                //{
                //    type = "1";
                //}

                ViewState["flag"] = "0";
                ViewState["lcoid"] = "0";
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchUserName(string prefixText, int count)
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            string fetched_city = "";
            if (HttpContext.Current.Session["fetched_mstlcoreg_city"] != null)
            {
                fetched_city = HttpContext.Current.Session["fetched_mstlcoreg_city"].ToString();
            }
            str += " SELECT a.var_usermst_user_name FROM hway_user_master a where upper(a.var_usermst_city) = upper('" + fetched_city + "') ";
            str += " and upper(var_usermst_user_name) like upper('" + prefixText + "%') ";

            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                    dr["var_usermst_user_name"].ToString(), dr["var_usermst_user_name"].ToString());
                Operators.Add(item);
            }
            con.Close();
            con.Dispose();
            return Operators;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchOperators(string prefixText, int count, string contextKey)
        {
            string Str = prefixText.Trim();
            double Num;
            bool isNum = double.TryParse(Str, out Num);
            //if (!isNum)
            //{
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";

            str = "SELECT a.var_hway_brm_poid, a.var_hway_lco_code, a.var_hway_lco_name, ";
            str += " a.var_hway_first_name, a.var_hway_middle_name, ";
            str += " a.var_hway_last_name, a.var_hway_address, a.var_hway_zipcode, ";
            str += " a.var_hway_city, a.var_hway_state, a.var_hway_email, ";
            str += " a.var_hway_phone, a.var_hway_company, a.var_hway_jv, ";
            str += " a.var_hway_dt, a.var_hway_sdt, a.var_hway_area, ";
            str += " a.var_hway_pref_dom, a.var_hway_ent_tax_no, ";
            str += " a.var_hway_erp_control_acct_id, a.var_hway_pan_no, ";
            str += " a.var_hway_st_reg_no, a.var_hway_vat_tax_no, ";
            str += " a.var_hway_report_date, a.var_hway_pp_type ";
            str += " FROM view_hway_lco_master a  ";

            if (contextKey == "0")
            {
                str += " where upper(a.var_hway_lco_code) like upper('" + prefixText.ToString() + "%')";
            }
            else if (contextKey == "1")
            {
                str += " where upper(a.var_hway_lco_name) like  upper('" + prefixText.ToString() + "%')";
            }
            str += " and rownum <= 50";
            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (contextKey == "0")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["var_hway_lco_code"].ToString(), dr["var_hway_lco_code"].ToString());
                    Operators.Add(item);
                }
                else if (contextKey == "1")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                            dr["var_hway_lco_name"].ToString(), dr["var_hway_lco_name"].ToString());
                    Operators.Add(item);
                }
            }
            con.Close();
            con.Dispose();
            return Operators;
        }

        protected void loadCompanySection()
        {
            if (Session["username"] != null && Session["category"] != null && Session["operator_id"] != null)
            {
                string username = Convert.ToString(Session["username"]);
                string operid = Session["operator_id"].ToString();
                string catid = Session["category"].ToString();

                Cls_Business_MstLCORegistration obj = new Cls_Business_MstLCORegistration();
                DataSet dsCompData = obj.getCompDataBll(username, catid, operid, ViewState["companyname"].ToString());
                if (dsCompData != null)
                {
                    ddlCompany.DataSource = dsCompData.Tables[0];
                    ddlCompany.DataTextField = "COMP_NAME";
                    ddlCompany.DataValueField = "COMP_ID";
                    ddlCompany.DataBind();
                    //ddlCompany.Items.Insert(0, new ListItem("Select Company", "0"));

                    //ddlDist.DataSource = dsCompData.Tables[1];
                    //ddlDist.DataTextField = "DIST_NAME";
                    //ddlDist.DataValueField = "DIST_ID";
                    //ddlDist.DataBind();
                    //ddlDist.Items.Insert(0, new ListItem("Select Distributor", "0"));

                    //ddlSubDist.DataSource = dsCompData.Tables[2];
                    //ddlSubDist.DataTextField = "SUBDIST_NAME";
                    //ddlSubDist.DataValueField = "SUBDIST_ID";
                    //ddlSubDist.DataBind();
                    //ddlSubDist.Items.Insert(0, new ListItem("Select Subdistributor", "0"));

                    //ddlMSO.DataSource = dsCompData.Tables[3];
                    //ddlMSO.DataTextField = "var_oper_opername";
                    //ddlMSO.DataValueField = "num_oper_id";
                    //ddlMSO.DataBind();
                    //ddlMSO.Items.Insert(0,new ListItem("Select MSO", "0"));

                    //ddlDistributor.DataSource = dsCompData.Tables[4];
                    //ddlDistributor.DataTextField = "var_oper_opername";
                    //ddlDistributor.DataValueField = "num_oper_id";
                    //ddlDistributor.DataBind();
                    //ddlDistributor.Items.Insert(0, new ListItem("Select Distributor", "0"));
                }
                else
                {
                    //exception occured
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void loadState()
        {
            DataSet dsStates = Cls_Helper.Comboupdate(" AOUP_LCOPRE_STATE_DEF WHERE VAR_PLAN_COMPANYCODE='HWP' and var_state_name='" + ViewState["state"].ToString() + "' order by VAR_STATE_NAME asc", "NUM_STATE_ID", "VAR_STATE_NAME");
            ddlState.DataSource = dsStates;
            ddlState.DataTextField = "VAR_STATE_NAME";
            ddlState.DataValueField = "NUM_STATE_ID";
            ddlState.DataBind();
            dsStates.Dispose();
            // ddlState.Items.Insert(0, new ListItem("Select State", "0"));
        }

        protected void loadCity()
        {

            DataSet dsStates = Cls_Helper.Comboupdate(" aoup_lcopre_city_def WHERE var_city_companycode='HWP' and var_city_name='" + ViewState["city"].ToString() + "' order by var_city_name asc", "num_city_id", "var_city_name");
            ddlCity.DataSource = dsStates;
            ddlCity.DataTextField = "var_city_name";
            ddlCity.DataValueField = "num_city_id";
            ddlCity.DataBind();
            ddlCity.Dispose();
            //ddlCity.Items.Insert(0, new ListItem("Select City", "0"));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            string code = lblLcoCode.Text;
            ht["code"] = code;
            string name = lblLcoName.Text;
            ht["name"] = name;
            string fname = txtFirstName.Text;
            ht["fname"] = fname;
            string mname = txtMidName.Text;
            ht["mname"] = mname;
            string lname = txtLastName.Text;
            ht["lname"] = lname;
            string jv = "0";
            string direct = "0";

            jv = txtJV.Text;


            direct = txtDirect.Text;

            ht["jv"] = jv;
            ht["direct"] = direct;
            string company = ddlCompany.SelectedValue.ToString();

            ht["company"] = company;

            string distributor = txtDist.Text.ToString();

            ht["distributor"] = distributor;

            string subdistributor = txtSubDist.Text.ToString();

            ht["subdistributor"] = subdistributor;
            string userid = "";//txtUserId.Text;
            ht["userid"] = userid;
            string brmpoid = txtBrmId.Text;
            ht["brmpoid"] = brmpoid;

            ht["LcoUserName"] = txtUsername.Text;
            string addr = txtAddress.Text;
            ht["addr"] = addr;
            string state = ddlState.SelectedValue.ToString();

            ht["state"] = state;
            string city = ddlCity.SelectedValue.ToString();

            ht["city"] = city;
            string pin = txtPincode.Text;
            ht["pin"] = pin;
            string mobile = txtMobile.Text;
            ht["mobile"] = mobile;
            string email = txtEmail.Text;
            ht["email"] = email;


            if (ViewState["lcoid"] != null)
            {
                ht["lcoid"] = Convert.ToString(ViewState["lcoid"]);
            }
            else
            {
                ht["lcoid"] = "0";
            }

            if (ViewState["flag"] != null)
            {
                ht["flag"] = Convert.ToString(ViewState["flag"]);
            }
            else
            {
                ht["flag"] = "0";
            }

            Cls_Business_MstLCORegistration obj = new Cls_Business_MstLCORegistration();
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
            ht["compcode"] = compcode;
            //ht["MSO"] = ddlMSO.SelectedValue.ToString();
            //ht["Distributor"] = ddlDistributor.SelectedValue.ToString();
            if (chkecsstatus.Checked == true)
            {
                ht["ecssattus"] = "Y";
            }
            else
            {
                ht["ecssattus"] = "N";
            }
            string response = obj.setLCOData(username, ht);
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        protected void reset()
        {

            lblLcoCode.Text = "";
            lblLcoName.Text = "";
            txtFirstName.Text = "";
            txtMidName.Text = "";
            txtLastName.Text = "";
            txtJV.Text = "";
            txtDirect.Text = "";

            txtBrmId.Text = "";
            txtUsername.Text = "";
            txtAddress.Text = "";
            txtPincode.Text = "";
            txtMobile.Text = "";
            txtEmail.Text = "";
            // ddlState.SelectedValue = "0";
            // ddlCompany.SelectedValue = "0";
            txtDist.Text = "";
            txtSubDist.Text = "";
            chkecsstatus.Checked = false;
            divdetails.Visible = false;
            lblResponseMsg.Text = "";

        }

        private Hashtable getLCOParamsData()
        {
            string lcocd = "";
            string lconm = "";
            if (RadSearchby.SelectedValue.ToString() == "0")
            {
                lcocd = txtLCOSearch.Text;
            }
            else if (RadSearchby.SelectedValue.ToString() == "1")
            {
                lconm = txtLCOSearch.Text;
            }

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("lcocd", lcocd);
            htSearchParams.Add("lconm", lconm);
            return htSearchParams;
        }

        protected void loadLCOSection()
        {
            Hashtable htLCOParams = getLCOParamsData();
            if (Session["username"] != null && Session["operator_id"] != null)
            {
                string username = Convert.ToString(Session["username"]);
                string catid = Convert.ToString(Session["category"]);
                string operator_id = Convert.ToString(Session["operator_id"]);
                Cls_Business_MstLCORegistration obj = new Cls_Business_MstLCORegistration();
                Hashtable htResponse = obj.GetTransations(htLCOParams, username);


                OracleConnection conn = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
                DataTable dts = new DataTable();
                try
                {
                    conn.Open();
                    string StrQry;
                    StrQry = " Select a.var_lcomst_ecsstatus from aoup_lcopre_lco_det a ";

                    if (RadSearchby.SelectedValue.ToString() == "0")
                    {
                        StrQry += " where upper(a.var_lcomst_code)  like upper('" + txtLCOSearch.Text + "%') ";
                    }
                    else if (RadSearchby.SelectedValue.ToString() == "1")
                    {
                        StrQry += " where upper(a.var_lcomst_name) like upper('" + txtLCOSearch.Text + "%') ";
                    }

                    OracleDataAdapter da = new OracleDataAdapter(StrQry, conn);
                    da.Fill(dts);
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
                catch (Exception ex)
                {
                }
                DataTable dt = null; //check for exception
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

                    lblResponseMsg.Text = "No data found...";
                    divdetails.Visible = false;
                }
                else
                {
                    divdetails.Visible = true;
                    lblLcoCode.Text = dt.Rows[0]["var_hway_lco_code"].ToString();
                    lblLcoName.Text = dt.Rows[0]["var_hway_lco_name"].ToString();
                    txtFirstName.Text = dt.Rows[0]["var_hway_first_name"].ToString();
                    txtMidName.Text = dt.Rows[0]["var_hway_middle_name"].ToString();
                    txtLastName.Text = dt.Rows[0]["var_hway_last_name"].ToString();
                    txtDirect.Text = dt.Rows[0]["var_hway_dt"].ToString();
                    txtJV.Text = dt.Rows[0]["var_hway_jv"].ToString();
                    txtDist.Text = dt.Rows[0]["var_hway_dt"].ToString();
                    txtSubDist.Text = dt.Rows[0]["var_hway_sdt"].ToString();
                    txtBrmId.Text = dt.Rows[0]["var_hway_brm_poid"].ToString();
                    txtAddress.Text = dt.Rows[0]["var_hway_address"].ToString();
                    ViewState["state"] = dt.Rows[0]["var_hway_state"].ToString();
                    ViewState["city"] = dt.Rows[0]["var_hway_city"].ToString();
                    Session["fetched_mstlcoreg_city"] = dt.Rows[0]["var_hway_city"].ToString();
                    ViewState["companyname"] = dt.Rows[0]["var_hway_company"].ToString();
                    loadState();
                    loadCity();
                    loadCompanySection();
                    txtPincode.Text = dt.Rows[0]["var_hway_zipcode"].ToString();
                    txtUsername.Text = "";
                    txtMobile.Text = dt.Rows[0]["var_hway_phone"].ToString();
                    txtEmail.Text = dt.Rows[0]["var_hway_email"].ToString();
                    lblResponseMsg.Text = "";
                    if (dts.Rows.Count != 0)
                    {
                        if (dts.Rows[0]["var_lcomst_ecsstatus"].ToString() == "Y")
                        {
                            chkecsstatus.Checked = true;
                        }
                    }
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadLCOSection();

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

        protected void RadSearchby_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //if (RadSearchby.SelectedValue.ToString() == "0")
            //{
            //    type = "0";
            //}
            //else
            //{
            //    type = "1";
            //}
        }

    }
}