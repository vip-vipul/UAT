using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Reports;
using System.Data;
using System.IO;
using System.Collections;
using PrjUpassDAL.Helper;


namespace PrjUpassPl.Reports
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        string username;
        string catid;
        string operator_id;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["RightsKey"] = null;
            Master.PageHeading = "LCO Plan Share Details";
            if (!IsPostBack)
            {

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                loadcity();
                tr4.Visible = false;
            }
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);

            }
        }

        protected void clear_msg()
        {
            lblMessage.Text = "";
        }
        private Hashtable getLedgerParamsData()
        {


            string city = "";
            string plan = "";
            string from = "";
            string to = "";
            string abcmap = "";
            string chkDt = "";
            string chkCity = "";
            string chkPlan = "";
            string addmap = "";
            if (chkcity.Checked)
            {
                city = ddlcity.SelectedValue.ToString();

                chkCity = "1";
            }

            else if (chkplan.Checked)
            {
                plan = ddlplan.SelectedValue.ToString();
                chkPlan = "1";
            }

            else if (chkdt.Checked)
            {
                from = txtFrom.Text;
                to = txtTo.Text;
                chkDt = "1";
            }

            else if (chkaddmap.Checked)
            {
                addmap = "1";
            }

            else if (chkabcmap.Checked)
            {
                abcmap = "1";
            }

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("city", city.ToString());
            htSearchParams.Add("plan", plan.ToString());
            htSearchParams.Add("chkPlan", chkPlan);
            htSearchParams.Add("chkCity", chkCity);
            htSearchParams.Add("chkDt", chkDt);
            htSearchParams.Add("addmap", addmap);
            htSearchParams.Add("abcmap", abcmap);
            htSearchParams.Add("addbasiccity", ddladdcity.SelectedValue.ToString());
            htSearchParams.Add("addbasicplan", ddladdplan.SelectedValue.ToString());


            return htSearchParams;
        }
        protected void btnPlanDl_Click(object sender, EventArgs e)
        {
            binddata();

        }
        protected void loadcity()
        {

            string catid = Convert.ToString(Session["category"]);
            string username = Session["username"].ToString();
            string where_str = " WHERE q.var_city_companycode='HWP' and a.num_usermst_cityid = q.num_city_id";
            if (catid == "3")
            {
                where_str += " AND a.var_usermst_username ='" + username.ToString() + "'";
            }
            DataSet ds = Cls_Helper.Comboupdate(" aoup_lcopre_user_det a, aoup_lcopre_city_def q " + where_str + " group by q.num_city_id,q.var_city_name ORDER BY q.var_city_name", "num_city_id", "var_city_name");
            ddlcity.DataSource = ds;
            ddlcity.DataTextField = "var_city_name";
            ddlcity.DataValueField = "num_city_id";
            ddlcity.DataBind();
            ddlcity.Items.Insert(0, new ListItem("All", "All"));
            ddlcity.Dispose();
        }
        protected void loadaddcity()
        {


            string catid = Convert.ToString(Session["category"]);
            string username = Session["username"].ToString();
            string where_str = " WHERE q.var_city_companycode='HWP' and a.num_usermst_cityid = q.num_city_id";
            if (catid == "3")
            {
                where_str += " AND a.var_usermst_username ='" + username.ToString() + "'";
            }
            DataSet ds = Cls_Helper.Comboupdate(" aoup_lcopre_user_det a, aoup_lcopre_city_def q " + where_str + " group by q.num_city_id,q.var_city_name ORDER BY q.var_city_name", "num_city_id", "var_city_name");
            ddladdcity.DataSource = ds;
            ddladdcity.DataTextField = "var_city_name";
            ddladdcity.DataValueField = "num_city_id";
            ddladdcity.DataBind();
            ddladdcity.Dispose();
        }
        protected void loadbasicplan()
        {
            string username = Session["username"].ToString();
            string where_str = "";
            DataSet ds;
            if (chkaddmap.Checked)
            {
                where_str = "  where num_plan_cityid='" + ddladdcity.SelectedValue.ToString() + "' and var_plan_plantype='B'";
                ds = Cls_Helper.Comboupdate(" aoup_lcopre_lcoplan_def " + where_str + " group by num_plan_plantypeid,var_plan_name ORDER BY var_plan_name", "num_plan_plantypeid", "var_plan_name");
                ddladdplan.DataSource = ds;
                ddladdplan.DataTextField = "var_plan_name";
                ddladdplan.DataValueField = "num_plan_plantypeid";
                ddladdplan.DataBind();
                ddladdplan.Dispose();
            }
            else
            {
                where_str = "  where num_plan_cityid='" + ddladdcity.SelectedValue.ToString() + "' and var_plan_plantype in('AD','GAD','RAD')";
                ds = Cls_Helper.Comboupdate(" aoup_lcopre_lcoplan_def " + where_str + " group by var_plan_name ORDER BY var_plan_name", "0", "var_plan_name");
                ddladdplan.DataSource = ds;
                ddladdplan.DataTextField = "var_plan_name";
                ddladdplan.DataValueField = "var_plan_name";
                ddladdplan.DataBind();
                ddladdplan.Dispose();
            }


        }
        protected void chkaddmap_CheckedChanged(object sender, EventArgs e)
        {
            loadaddcity();
            loadbasicplan();
            grdExpiry.Visible = false;
            if (chkaddmap.Checked)
            {
                chkabcmap.Visible = false;
                lblbasicplan.Text = "Basic Plan";
                tr4.Visible = true;
                btnPlanDl.Visible = true;
                chkcity.Checked = false;
                chkplan.Checked = false;
            }
            else
            {
                chkabcmap.Visible = true;
                tr4.Visible = false;
                btnPlanDl.Visible = false;
            }

        }

        protected void ddladdcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadbasicplan();
        }
        protected void binddata()
        {
            Hashtable htAddPlanParams = getLedgerParamsData();
            Cls_business_lcoshareDef obj = new Cls_business_lcoshareDef();
            DataTable dtPlanDet = obj.getPlanDetails(htAddPlanParams, username, catid);

            if (dtPlanDet == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dtPlanDet.Rows.Count == 0)
            {
                grdExpiry.Visible = false;
                lblMessage.Text = "No data found";
            }
            else
            {
                grdExpiry.Visible = true;
                lblMessage.Text = "";
                ViewState["searched_trans"] = dtPlanDet;
                grdExpiry.DataSource = dtPlanDet;
                grdExpiry.DataBind();
            }



        }

        protected void grdExpiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdExpiry.PageIndex = e.NewPageIndex;
            binddata();
        }




        protected void data()
        {

            if (chkcity.Checked && ddlcity.SelectedIndex == 0 && chkplan.Checked && ddlplan.SelectedIndex == 0)
            {
                btndownPlandl.Visible = true;
                btnPlanDl.Visible = false;
                grdExpiry.Visible = false;
                lblMessage.Visible = true;
                chkaddmap.Visible = true;
                chkabcmap.Visible = true;
                chkabcmap.Checked = false;
                chkaddmap.Checked = false;
                tr4.Visible = false;
            }
            else if (chkcity.Checked && ddlcity.SelectedIndex > 0 && chkplan.Checked && ddlplan.SelectedIndex == 0)
            {
                btndownPlandl.Visible = true;
                btnPlanDl.Visible = false;
                grdExpiry.Visible = false;
                lblMessage.Visible = true;
                chkaddmap.Visible = true;
                chkabcmap.Visible = true;
                chkabcmap.Checked = false;
                chkaddmap.Checked = false;
                tr4.Visible = false;
            }
            else if (chkcity.Checked && ddlcity.SelectedIndex == 0 && chkplan.Checked && ddlplan.SelectedIndex > 0)
            {
                btndownPlandl.Visible = true;
                btnPlanDl.Visible = false;
                grdExpiry.Visible = false;
                lblMessage.Visible = true;
                chkaddmap.Visible = true;
                chkabcmap.Visible = true;
                chkabcmap.Checked = false;
                chkaddmap.Checked = false;
                tr4.Visible = false;
            }
            else if (chkcity.Checked && ddlcity.SelectedIndex == 0)
            {
                btndownPlandl.Visible = true;
                btnPlanDl.Visible = false;
                grdExpiry.Visible = false;
                lblMessage.Visible = true;
                chkaddmap.Visible = true;
                chkabcmap.Visible = true;
                chkabcmap.Checked = false;
                chkaddmap.Checked = false;
                tr4.Visible = false;
            }
            else if (chkplan.Checked && ddlplan.SelectedIndex == 0)
            {
                btndownPlandl.Visible = true;
                btnPlanDl.Visible = false;
                grdExpiry.Visible = false;
                lblMessage.Visible = true;
                chkaddmap.Visible = true;
                chkabcmap.Visible = true;
                chkabcmap.Checked = false;
                chkaddmap.Checked = false;
                tr4.Visible = false;
            }
            else if (chkplan.Checked == false || chkcity.Checked == false && chkplan.Checked == false && chkcity.Checked == false)
            {
                btndownPlandl.Visible = true;
                chkaddmap.Visible = true;
                chkabcmap.Visible = true;
                chkabcmap.Checked = false;
                chkaddmap.Checked = false;
            }
            else if (chkplan.Checked == true || chkcity.Checked == true && chkplan.Checked == true && chkcity.Checked == true)
            {
                btndownPlandl.Visible = true;
                btnPlanDl.Visible = true;
                chkaddmap.Visible = true;
                chkabcmap.Visible = true;
                chkabcmap.Checked = false;
                chkaddmap.Checked = false;
                tr4.Visible = false;
            }

            else
            {
                btndownPlandl.Visible = true;
                btnPlanDl.Visible = true;
                grdExpiry.Visible = true;
                lblMessage.Visible = true;

            }

        }

        protected void chkdt_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkaddmap.Checked && chkdt.Checked)
            //{

            //}
            //else
            //{
            //   data();
            //}

        }

        protected void chkcity_CheckedChanged(object sender, EventArgs e)
        {
            grdExpiry.Visible = false;
            data();
        }

        protected void ddlcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdExpiry.Visible = false;
            data();
        }

        protected void chkplan_CheckedChanged(object sender, EventArgs e)
        {
            grdExpiry.Visible = false;
            data();
        }

        protected void ddlplan_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdExpiry.Visible = false;
            data();
        }

        protected void chkabcmap_CheckedChanged(object sender, EventArgs e)
        {
            loadaddcity();
            loadbasicplan();
            grdExpiry.Visible = false;
            if (chkabcmap.Checked)
            {
                chkaddmap.Visible = false;
                lblbasicplan.Text = "Add On Plan";
                tr4.Visible = true;
                btnPlanDl.Visible = true;
                chkcity.Checked = false;
                chkplan.Checked = false;

            }
            else
            {
                chkaddmap.Visible = true;
                tr4.Visible = false;
                btnPlanDl.Visible = false;
            }

        }

        protected void ddladdplan_SelectedIndexChanged(object sender, EventArgs e)
        {
            // binddata();
        }

        protected void grdExpiry_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btndownPlandl_Click(object sender, EventArgs e)
        {
            clear_msg();
            lblMessage.Text = "";
            if (chkabcmap.Checked == false && chkaddmap.Checked == false && chkcity.Checked == false && chkdt.Checked == false && chkplan.Checked == false)
            {


                lblMessage.Text = "Please Select One Option!!";
                return;

            }

            Hashtable htAddPlanParams = getLedgerParamsData();
            Cls_business_lcoshareDef obj = new Cls_business_lcoshareDef();
            DataTable dtPlanDet = obj.getPlanDetails(htAddPlanParams, username, catid);
            if (dtPlanDet == null)
            {
                lblMessage.Text = "Something went wrong...";
            }
                if(dtPlanDet.Rows.Count == 0)
                {
                    lblMessage.Text = "No Data Found";
                
                
                }
            else
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;
                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PlanDetails_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                        + "Plan Name" + Convert.ToChar(9)
                        + "Plan Type" + Convert.ToChar(9)
                        + "Plan Poid" + Convert.ToChar(9)
                        + "Deal Poid" + Convert.ToChar(9)
                        + "Product Poid" + Convert.ToChar(9)
                        + "Customer Price" + Convert.ToChar(9)
                        + "Lco Price" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9)
                         + "LCO Code" + Convert.ToChar(9)
                          + "LCO Name" + Convert.ToChar(9);
                    
                    while (j < dtPlanDet.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dtPlanDet.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["var_plan_name"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["var_plan_plantype"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["var_plan_planpoid"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["var_plan_dealpoid"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["var_plan_productpoid"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["num_plan_custprice"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["num_plan_lcoprice"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["var_city_name"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["LCO_Code"].ToString() + Convert.ToChar(9)
                                 + dtPlanDet.Rows[i]["LCO_NAME"].ToString() + Convert.ToChar(9);
                            sw.WriteLine(strrow);
                        }
                    }
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception ex)
                {
                    sw.Flush();
                    sw.Close();
                    Response.Write("Error : " + ex.Message.Trim());
                    return;
                }
                //dtPlanDet = null;
                dtPlanDet.Dispose();
                Response.Redirect("../MyExcelFile/" + "PlanDetails_" + datetime + ".xls");
            }
        }

    }
}