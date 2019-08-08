using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassBLL.Master;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassPl.Master
{
    public partial class FrmAutoEcsLcoConfig : System.Web.UI.Page
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
                Session["RightsKey"] = "N";
                operid = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                divdetails.Visible = false;

               
            }
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

            str = "SELECT a.var_lcomst_code,a.var_lcomst_name,a.var_lcomst_firstname, ";
            str += " a.var_lcomst_middlename,a.var_lcomst_lastname,a.num_lcomst_directno,";
            str += " a.num_lcomst_jvno,a.var_lcomst_ecsstatus ";
            str += " from aoup_lcopre_lco_det a ";
         

            if (contextKey == "0")
            {
                str += " where upper(a.var_lcomst_code) like upper('" + prefixText.ToString() + "%')";
            }
            else if (contextKey == "1")
            {
                str += " where upper(a.var_lcomst_name) like  upper('" + prefixText.ToString() + "%')";
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
                        dr["var_lcomst_code"].ToString(), dr["var_lcomst_code"].ToString());
                    Operators.Add(item);
                }
                else if (contextKey == "1")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                            dr["var_lcomst_name"].ToString(), dr["var_lcomst_name"].ToString());
                    Operators.Add(item);
                }
            }
            con.Close();
            con.Dispose();
            return Operators;
        }

      

      

       

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            string code = lblLcoCode.Text;
            ht["Lcocode"] = code;
            string name = lblLcoName.Text;
            ht["Lconame"] = name;
            string fname = LblFristName.Text;
            ht["fname"] = fname;
            string mname = LblMidName.Text;
            ht["mname"] = mname;
            string lname = LblLastname.Text;
            ht["lname"] = lname;
            Cls_Business_MstLcoAutoEcsConfiguration obj = new Cls_Business_MstLcoAutoEcsConfiguration();
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

     

        protected void reset()
        {
            txtLCOSearch.Text = "";
            lblLcoCode.Text = "";
            lblLcoName.Text = "";
            LblFristName.Text = "";
            LblMidName.Text = "";
            LblLastname.Text = "";
            LblJvNos.Text = "";
            lblDirectNos.Text = "";

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
                Cls_Business_MstLcoAutoEcsConfiguration obj = new Cls_Business_MstLcoAutoEcsConfiguration();
                Hashtable htResponse = obj.GetTransations(htLCOParams, username);


               
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
                    lblLcoCode.Text = dt.Rows[0]["var_lcomst_code"].ToString();
                    lblLcoName.Text = dt.Rows[0]["var_lcomst_name"].ToString();
                    LblFristName.Text = dt.Rows[0]["var_lcomst_firstname"].ToString();
                    LblMidName.Text = dt.Rows[0]["var_lcomst_middlename"].ToString();
                    LblLastname.Text = dt.Rows[0]["var_lcomst_lastname"].ToString();
                    lblDirectNos.Text = dt.Rows[0]["num_lcomst_directno"].ToString();
                    LblJvNos.Text = dt.Rows[0]["num_lcomst_jvno"].ToString();
                    if (dt.Rows[0]["var_lcomst_ecsstatus"].ToString() == "Y")
                    {
                        chkecsstatus.Checked = true;
                    }
                    else {
                        chkecsstatus.Checked = false;
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
            lblResponseMsg.Text = "";
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
           
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            divdetails.Visible = false;
            lblResponseMsg.Text = "";
            txtLCOSearch.Text = "";
        }




       

    }
}