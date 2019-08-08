using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Reports;
using System.Data;
using System.IO;

namespace PrjUpassPl.Reports
{
    public partial class rptPlanDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["RightsKey"] = null;
        }

        protected void clear_msg()
        {
            lblMessage.Text = "";
        }

        protected void btnPlanDl_Click(object sender, EventArgs e)
        {
            clear_msg();
            Cls_Business_rptPlanDetails obj = new Cls_Business_rptPlanDetails();
            DataTable dtPlanDet = obj.getPlanDetails();
            if (dtPlanDet == null)
            {
                lblMessage.Text = "Something went wrong...";
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
                        + "Name" + Convert.ToChar(9)
                        + "Type" + Convert.ToChar(9)
                        + "Plan Poid" + Convert.ToChar(9)
                        + "Deal Poid" + Convert.ToChar(9)
                        + "Product Poid" + Convert.ToChar(9)
                        + "Customer Price" + Convert.ToChar(9)
                        + "Lco Price" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9)
                        + "DAS Area" + Convert.ToChar(9);

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
                                 + dtPlanDet.Rows[i]["AREA"].ToString() + Convert.ToChar(9);
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