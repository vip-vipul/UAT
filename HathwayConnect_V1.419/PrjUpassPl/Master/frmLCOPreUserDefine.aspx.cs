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
    public partial class frmLCOPreUserDefine : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        string username;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Lco User Define";
            if (!IsPostBack)
            {

                if (Session["operator_id"] != null)
                {


                    Session["pagenos"] = "1";
                    Session["RightsKey"] = null;
                    string operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    string catid = Convert.ToString(Session["category"]);

                    RadSearchby.SelectedValue = "0";
                    pnlDetails.Visible = false;

                    if (catid == "3")
                    {
                        divsearchLco.Visible = false;
                        loadLCOSectionLco();
                        loadLCOState();
                        loadLCOcity();
                        txtAccNO.Enabled = false;
                        txtAddress.Enabled = false;
                        txtBrmId.Enabled = false;
                        txtDirectNo.Enabled = false;
                        txtJvNo.Enabled = false;
                        txtPincode.Enabled = false;
                        ddlCity.Enabled = false;
                        ddlState.Enabled = false;
                    }
                    else
                    {
                        divsearchLco.Visible = true;
                        txtBrmId.Text = "";
                        txtBrmId.Enabled = true;
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
        public static List<string> SearchOperators(string prefixText, int count, string contextKey)
        {
            string Str = prefixText.Trim();
            double Num;
            bool isNum = double.TryParse(Str, out Num);

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

            str = "SELECT lconame, lcoid, lcocode ";
            str += " FROM view_preLCO_lco_det ";

            if (contextKey == "0")
            {
                str += " where upper(lcocode) like upper('" + prefixText.ToString() + "%')";
            }
            else if (contextKey == "1")
            {
                str += " where upper(lconame) like  upper('" + prefixText.ToString() + "%')";
            }
            if (catid == "2")
            {
                str += " and PARENTID='" + operid.ToString() + "'  ";
            }
            else if (catid == "5")
            {
                str += " and DISTID='" + operid.ToString() + "'  ";
            }
            else if (catid == "3")
            {
                str += " and OPERID ='" + operid.ToString() + "'";
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
                        dr["lcocode"].ToString(), dr["lcocode"].ToString());
                    Operators.Add(item);
                }
                else if (contextKey == "1")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                            dr["lconame"].ToString(), dr["lconame"].ToString());
                    Operators.Add(item);
                }
            }
            //string[] prefixTextArray = Operators.ToArray<string>();
            con.Close();
            con.Dispose();
            return Operators;
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


        protected void loadLCOSectionLco()
        {
            // Hashtable htLCOParams = getLCOParamsData();
            btnSearch.Enabled = false;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                string username = Convert.ToString(Session["username"]);
                string catid = Convert.ToString(Session["category"]);
                string operator_id = Convert.ToString(Session["operator_id"]);
                Cls_Business_MstLCOPreUDefine obj = new Cls_Business_MstLCOPreUDefine();
                Hashtable htResponse = obj.GetTransationsLCo(username, catid, operator_id);

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
                    lblLCOCode.Text = "";
                    lblLCOName.Text = "";
                    lblResponseMsg.Text = "No data found...";
                    pnlDetails.Visible = false;
                    return;
                }
                else
                {
                    lblLCOCode.Text = dt.Rows[0]["lcocode"].ToString();
                    lblLCOName.Text = dt.Rows[0]["lconame"].ToString();
                    txtBrmId.Text = dt.Rows[0]["var_usermst_brmpoid"].ToString();
                    txtDirectNo.Text = dt.Rows[0]["directno"].ToString();
                    txtJvNo.Text = dt.Rows[0]["jvno"].ToString();
                    txtEmail.Text = dt.Rows[0]["var_usermst_email"].ToString();
                    txtAddress.Text = dt.Rows[0]["var_usermst_address"].ToString();
                    txtAccNO.Text = dt.Rows[0]["var_usermst_accno"].ToString();
                    txtMobile.Text = dt.Rows[0]["num_usermst_mobileno"].ToString();
                    txtPincode.Text = dt.Rows[0]["var_usermst_code"].ToString();
                    ViewState["state"] = dt.Rows[0]["num_usermst_stateid"].ToString();
                    ViewState["city"] = dt.Rows[0]["num_usermst_cityid"].ToString();

                    ViewState["lcoid2"] = dt.Rows[0]["lcoid"].ToString();
                    ViewState["searched_trans"] = dt;
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

        protected void loadLCOSection()
        {
            Hashtable htLCOParams = getLCOParamsData();
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                string username = Convert.ToString(Session["username"]);
                string catid = Convert.ToString(Session["category"]);
                string operator_id = Convert.ToString(Session["operator_id"]);
                Cls_Business_MstLCOPreUDefine obj = new Cls_Business_MstLCOPreUDefine();
                Hashtable htResponse = obj.GetTransations(htLCOParams, username, catid, operator_id);

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
                    lblLCOCode.Text = "";
                    lblLCOName.Text = "";
                    lblResponseMsg.Text = "No data found...";
                    pnlDetails.Visible = false;
                    return;
                }
                else
                {
                    lblLCOCode.Text = dt.Rows[0]["lcocode"].ToString();
                    lblLCOName.Text = dt.Rows[0]["lconame"].ToString();
                    ViewState["lcoid2"] = dt.Rows[0]["lcoid"].ToString();
                    ViewState["searched_trans"] = dt;
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


        protected void loadLCOState()
        {
            DataSet dsStates = Cls_Helper.Comboupdate("AOUP_LCOPRE_STATE_DEF WHERE VAR_PLAN_COMPANYCODE='HWP' and num_state_id='" + ViewState["state"].ToString() + "' ORDER BY VAR_STATE_NAME", "NUM_STATE_ID", "VAR_STATE_NAME");
            ddlState.DataSource = dsStates;
            ddlState.DataTextField = "VAR_STATE_NAME";
            ddlState.DataValueField = "NUM_STATE_ID";
            ddlState.DataBind();
            dsStates.Dispose();
        }

        protected void loadLCOcity()
        {
            string where_str = " WHERE var_city_companycode='HWP' AND  num_city_id='" + ViewState["city"].ToString() + "'";
            DataSet ds = Cls_Helper.Comboupdate(" aoup_lcopre_city_def " + where_str + " ORDER BY var_city_name", "num_city_id", "var_city_name");
            ddlCity.DataSource = ds;
            ddlCity.DataTextField = "var_city_name";
            ddlCity.DataValueField = "num_city_id";
            ddlCity.DataBind();
            ddlCity.Dispose();
        }

        protected void loadState()
        {
            DataSet dsStates = Cls_Helper.Comboupdate("AOUP_LCOPRE_STATE_DEF WHERE VAR_PLAN_COMPANYCODE='HWP' ORDER BY VAR_STATE_NAME", "NUM_STATE_ID", "VAR_STATE_NAME");
            ddlState.DataSource = dsStates;
            ddlState.DataTextField = "VAR_STATE_NAME";
            ddlState.DataValueField = "NUM_STATE_ID";
            ddlState.DataBind();
            dsStates.Dispose();
            ddlState.Items.Insert(0, new ListItem("Select State", "0"));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadLCOSection();
            loadState();


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

            ViewState["flag"] = "0";
            ViewState["lcoid"] = "0";
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string state_id = ddlState.SelectedValue.ToString();
            if (state_id != "0")
            {
                string where_str = " WHERE var_city_companycode='HWP' AND num_city_stateid= " + state_id;
                DataSet ds = Cls_Helper.Comboupdate(" aoup_lcopre_city_def " + where_str + " ORDER BY var_city_name", "num_city_id", "var_city_name");
                ddlCity.DataSource = ds;
                ddlCity.DataTextField = "var_city_name";
                ddlCity.DataValueField = "num_city_id";
                ddlCity.DataBind();
                ddlCity.Dispose();
            }
            else
            {
                ddlCity.Items.Clear();
            }
            ddlCity.Items.Insert(0, new ListItem("Select City", "0"));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        protected void reset()
        {
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                string username = Convert.ToString(Session["username"]);
                string catid = Convert.ToString(Session["category"]);
                string operator_id = Convert.ToString(Session["operator_id"]);
                txtLCOSearch.Text = "";

                txtUserId.Text = "";

                txtFirstName.Text = "";
                txtMidName.Text = "";
                txtLastName.Text = "";

                lblResponseMsg.Text = "";

                if (!(catid == "3"))
                {
                    pnlDetails.Visible = false;
                    lblLCOCode.Text = "";
                    lblLCOName.Text = "";
                    txtBrmId.Text = "";
                    txtJvNo.Text = "";
                    txtDirectNo.Text = "";
                    txtAccNO.Text = "";
                    txtAddress.Text = "";
                    txtPincode.Text = "";
                    txtMobile.Text = "";
                    txtEmail.Text = "";
                    ddlState.SelectedValue = "0";
                    ddlCity.Items.Clear();
                    ddlCity.Items.Insert(0, new ListItem("Select City", "0"));
                }
                else if (catid == "3")
                {
                    divsearchLco.Visible = false;
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                string username = Convert.ToString(Session["username"]);
                string catid = Convert.ToString(Session["category"]);
                string operator_id = Convert.ToString(Session["operator_id"]);
                Hashtable ht = new Hashtable();
                /*string code = lblLCOCode.Text;
                ht["code"] = code;*/
                string lcoid2 = ViewState["lcoid2"].ToString();
                ht["lcoid2"] = lcoid2;

                string userlevel = "";
                if (catid == "3")
                {
                    userlevel = "E";
                }
                else
                {
                    userlevel = "M";
                }
                ht["userlevel"] = userlevel;


                if (txtUserId.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("T", txtUserId.Text);

                    if (valid == "")
                    {
                        ht["userid12"] = txtUserId.Text;
                    }
                    else
                    {
                        lblResponseMsg.Text = valid +" at Login ID";
                        return;
                    }

                }

                if (txtFirstName.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("T", txtFirstName.Text);

                    if (valid == "")
                    {
                        ht["fname"] = txtFirstName.Text;
                    }
                    else
                    {
                        lblResponseMsg.Text = valid + " at First Name";
                        return;
                    }

                }


                if (txtMidName.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("T", txtMidName.Text);

                    if (valid == "")
                    {
                        ht["mname"] = txtMidName.Text;
                    }
                    else
                    {
                        lblResponseMsg.Text = valid + " at Middle Name";
                        return;
                    }

                }

                if (txtLastName.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("T", txtLastName.Text);

                    if (valid == "")
                    {
                        ht["lname"] = txtLastName.Text;
                    }
                    else
                    {
                        lblResponseMsg.Text = valid + " at Last Name";
                        return;
                    }

                }


                if (txtMobile.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("N", txtMobile.Text);

                    if (valid == "")
                    {
                        ht["mobileno"] = txtMobile.Text;
                    }
                    else
                    {
                        lblResponseMsg.Text = valid + " at Mobile";
                        return;
                    }

                }


                if (txtEmail.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("T", txtEmail.Text);

                    if (valid == "")
                    {
                        ht["email"] = txtEmail.Text;
                    }
                    else
                    {
                        lblResponseMsg.Text = valid + " at Email";
                        return;
                    }

                }




                //string userid1 = txtUserId.Text;
                //ht["userid12"] = userid1;
                string userowner = "";
                ht["userowner"] = userowner;
                //string fname = txtFirstName.Text;
                //ht["fname"] = fname;
                //string mname = txtMidName.Text;
                //ht["mname"] = mname;
                //string lname = txtLastName.Text;
                //ht["lname"] = lname;
                string jv = "0";
                string direct = "0";

                jv = txtJvNo.Text;

                direct = txtDirectNo.Text;

                ht["jv"] = jv;
                ht["direct"] = direct;
                string brmpoid = txtBrmId.Text;
                ht["brmpoid"] = brmpoid;
                string accno = txtAccNO.Text;
                ht["accno"] = accno;
                string addr = txtAddress.Text;
                ht["addr"] = addr;
                string state = ddlState.SelectedValue.ToString();
                if (catid != "3")
                {
                    if (state == "0")
                    {
                        lblResponseMsg.Text = "Select State";
                        return;
                    }
                }
                ht["state"] = state;
                string city = ddlCity.SelectedValue.ToString();
                if (catid != "3")
                {
                    if (city == "0")
                    {
                        lblResponseMsg.Text = "Select City";
                        return;
                    }
                }
                ht["city"] = city;
                string pincode = txtPincode.Text;
                ht["pincode"] = pincode;
                //string mobileno = txtMobile.Text;
                //ht["mobileno"] = mobileno;
                //string email = txtEmail.Text;
                //ht["email"] = email;


                if (ViewState["userid1"] != null)
                {
                    ht["userid1"] = Convert.ToString(ViewState["userid1"]);
                }
                else
                {
                    ht["userid1"] = "0";
                }
                /*if (ViewState["lcoid"] != null)
                {
                    ht["lcoid"] = Convert.ToString(ViewState["lcoid"]);
                }
                else
                {
                    ht["lcoid"] = "0";
                }*/

                if (ViewState["flag"] != null)
                {
                    ht["flag"] = Convert.ToString(ViewState["flag"]);
                }
                else
                {
                    ht["flag"] = "0";
                }

                Cls_Business_MstLCOPreUDefine obj = new Cls_Business_MstLCOPreUDefine();

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
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
        }

        //protected void RadSearchby_SelectedIndexChanged1(object sender, EventArgs e)
        //{
        //    if (RadSearchby.SelectedValue.ToString() == "0")
        //    {
        //        type = "0";
        //    }
        //    else
        //    {
        //        type = "1";
        //    }

        //}
    }
}