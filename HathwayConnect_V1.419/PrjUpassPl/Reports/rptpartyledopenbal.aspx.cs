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
    public partial class rptpartyledopenbal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {           
            Master.PageHeading = "Bal. on Migration";
            if (!IsPostBack)
            {
                
                loadcity();
            }
        }
        private Hashtable getLedgerParamsData()
        {
            string city = "";
            if (ddladdcity.SelectedValue == "0")
            {
                city = "All";
            }
            else
            {
                city = ddladdcity.SelectedValue.ToString();
            }
           

            Hashtable htSearchParams = new Hashtable(); 
            htSearchParams.Add("city", city.ToString());

            return htSearchParams;
        }

        protected void loadcity()
        {


            string catid = Convert.ToString(Session["category"]);
            string username = Session["username"].ToString();
            string where_str = " WHERE q.var_city_companycode='HWP' and a.num_usermst_cityid = q.num_city_id";
           /* if (catid == "3")
            {
                where_str += " AND a.var_usermst_username ='" + username.ToString() + "'";
            }*/
            DataSet ds = Cls_Helper.Comboupdate(" aoup_lcopre_user_det a, aoup_lcopre_city_def q " + where_str + " group by q.num_city_id,q.var_city_name ORDER BY q.var_city_name", "num_city_id", "var_city_name");
            ddladdcity.DataSource = ds;
            ddladdcity.DataTextField = "var_city_name";
            ddladdcity.DataValueField = "num_city_id";
            ddladdcity.DataBind();
            ddladdcity.Dispose();
            ddladdcity.Items.Insert(0, new ListItem("All", "0"));
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            if (ddladdcity.SelectedIndex == 0)
            {
                grdExpiry.Visible = false;
                bindexcel();
            }
            else
            {
                binddata();
            }
        }
        
        protected void binddata()
        {
            Hashtable htAddPlanParams = getLedgerParamsData();
            Cls_Business_rptpartyledopenbal obj = new Cls_Business_rptpartyledopenbal();
            DataTable dtPlanDet = obj.getpartyledDet(htAddPlanParams);

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

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdExpiry.ClientID + "', 400, 1200 , 37 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");
            }

        }
        protected void bindexcel()
        {
            Hashtable htAddPlanParams = getLedgerParamsData();
            Cls_Business_rptpartyledopenbal obj = new Cls_Business_rptpartyledopenbal();
            DataTable dtPlanDet = obj.getpartyledDet(htAddPlanParams);
            if (dtPlanDet == null)
            {
                lblMessage.Text = "Something went wrong...";
            }
            else
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;
                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "Lcobalancemigrat_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "Opening Balance" + Convert.ToChar(9)
                        + "Date" + Convert.ToChar(9);

                    while (j < dtPlanDet.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dtPlanDet.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["var_partled_lcocode"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["var_partled_lconame"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["num_partled_openinbal"].ToString() + Convert.ToChar(9)
                                + dtPlanDet.Rows[i]["partyleddate"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "Lcobalancemigrat_" + datetime + ".xls");
            }
        }

        protected void grdExpiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdExpiry.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void grdExpiry_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddladdcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (ddladdcity.SelectedIndex == 0)
            {
                grdExpiry.Visible = false;
            }
        }
    }
}