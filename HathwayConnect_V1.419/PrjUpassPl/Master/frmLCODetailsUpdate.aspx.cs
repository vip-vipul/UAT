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
    public partial class frmLCODetailsUpdate : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
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

                    RadSearchby.SelectedValue = "0";
                    pnlDetails.Visible = false;

                    if (catid == "3")
                    {
                        divsearchLco.Visible = false;
                        loadLCOSectionLco();
                    }
                    else
                    {
                        divsearchLco.Visible = true;
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

        protected void loadLCOSectionLco()
        {
            
            btnSearch.Enabled = false;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                string username = Convert.ToString(Session["username"]);
                string catid = Convert.ToString(Session["category"]);
                string operator_id = Convert.ToString(Session["operator_id"]);
                Cls_Business_MstLCOUpdateDetails obj = new Cls_Business_MstLCOUpdateDetails();
                Hashtable htResponse = obj.GetTransationsLCo(username, catid, operator_id);

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
                    lblLCOCode.Text = "";
                    lblLCOName.Text = "";
                    txtEmail.Text = "";
                    txtMobile.Text = "";
                    lblSubDistributor.Text = "";
                    lblState.Text = "";
                    lblDistributor.Text = "";
                    lblDirect.Text = "";
                    lblCity.Text = "";
                    lblAddress.Text = "";
                    lblJV.Text = "";
                    lblResponseMsg.Text = "No data found...";
                    pnlDetails.Visible = false;
                }
                else
                {
                    lblLCOCode.Text = dt.Rows[0]["lcocode"].ToString();
                    lblLCOName.Text = dt.Rows[0]["lconame"].ToString();

                    lblAddress.Text = dt.Rows[0]["var_usermst_address"].ToString();
                    lblCity.Text = dt.Rows[0]["city"].ToString();
                    lblDirect.Text = dt.Rows[0]["directno"].ToString();
                    lblDistributor.Text = dt.Rows[0]["distributor"].ToString();
                    lblJV.Text = dt.Rows[0]["jvno"].ToString();
                    lblState.Text = dt.Rows[0]["state"].ToString();
                    lblSubDistributor.Text = dt.Rows[0]["subdistributor"].ToString();
                   
                    txtEmail.Text = dt.Rows[0]["var_usermst_email"].ToString();
                  
                    txtMobile.Text = dt.Rows[0]["num_usermst_mobileno"].ToString();

                    Session["LcoCode"] = lblLCOCode.Text.Trim().ToString();
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
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                string username = Convert.ToString(Session["username"]);
                string catid = Convert.ToString(Session["category"]);
                string operator_id = Convert.ToString(Session["operator_id"]);
                Cls_Business_MstLCOUpdateDetails obj = new Cls_Business_MstLCOUpdateDetails();
                Hashtable htResponse = obj.GetTransations(htLCOParams, username, catid, operator_id);

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
                    lblLCOCode.Text = "";
                    lblLCOName.Text = "";
                    txtEmail.Text = "";
                    txtMobile.Text = "";
                    lblSubDistributor.Text = "";
                    lblState.Text = "";
                    lblDistributor.Text = "";
                    lblDirect.Text = "";
                    lblCity.Text = "";
                    lblAddress.Text = "";
                    lblJV.Text = "";
                    lblResponseMsg.Text = "No data found...";
                    pnlDetails.Visible = false;
                }
                else
                {
                    lblLCOCode.Text = dt.Rows[0]["lcocode"].ToString();
                    lblLCOName.Text = dt.Rows[0]["lconame"].ToString();
                    txtEmail.Text = dt.Rows[0]["email"].ToString();

                    txtMobile.Text = dt.Rows[0]["mobileno"].ToString();

                    lblAddress.Text = dt.Rows[0]["addr"].ToString();
                    lblCity.Text = dt.Rows[0]["city"].ToString();
                    lblState.Text = dt.Rows[0]["state"].ToString();
                    lblDistributor.Text = dt.Rows[0]["distname"].ToString();
                    lblSubDistributor.Text = dt.Rows[0]["subdist"].ToString();
                    lblDirect.Text = dt.Rows[0]["directno"].ToString();
                    lblJV.Text = dt.Rows[0]["jvno"].ToString();

                    Session["LcoCode"] = lblLCOCode.Text.Trim().ToString();
                    //ViewState["lcoid2"] = dt.Rows[0]["lcoid"].ToString();
                    //ViewState["searched_trans"] = dt;
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        protected void reset()
        {
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                string catid = Convert.ToString(Session["category"]);
                txtLCOSearch.Text = "";

                lblResponseMsg.Text = "";

                if (!(catid == "3"))
                {
                    pnlDetails.Visible = false;
                    lblLCOCode.Text = "";
                    lblLCOName.Text = "";

                    txtMobile.Text = "";
                    txtEmail.Text = "";

                    lblJV.Text = "";
                    lblDirect.Text = "";
                    lblCity.Text = "";
                    lblState.Text = "";
                    lblAddress.Text = "";
                    lblDistributor.Text = "";
                    lblSubDistributor.Text = "";

                }
                else if (catid == "3")
                {
                    divsearchLco.Visible = false;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            string mobileno = txtMobile.Text;
            ht["mobileno"] = mobileno;
            string email = txtEmail.Text;
            ht["email"] = email;
            Cls_Business_MstLCOUpdateDetails obj = new Cls_Business_MstLCOUpdateDetails();
             string lcocode = "";
             string username = "";
             if (Session["LcoCode"] != null)
            {
                lcocode = Convert.ToString(Session["LcoCode"]);
                username = Convert.ToString(Session["username"]);
                
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
             string response = obj.UpdateLCOData(username, lcocode, ht);
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