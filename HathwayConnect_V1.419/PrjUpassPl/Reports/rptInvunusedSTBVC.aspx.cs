using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Reports;
using System.Data;
using System.Collections;
using System.IO;

namespace PrjUpassPl.Reports
{
    public partial class rptInvunusedSTBVC : System.Web.UI.Page
    {
        string username = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Inventory Unused STB/VC Report";
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            binddata();
        }
        private Hashtable getLedgerParamsData()
        {
            string search_type = ddlPackage.SelectedValue.ToString();
            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("Type", search_type);
            return htSearchParams;
        }

        protected void binddata()
        {

            Hashtable htAddPlanParams = getLedgerParamsData();
            string username ;

            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            DataTable dt = new DataTable();

            Cls_Business_rptInvUnusedSTBVC objTran = new Cls_Business_rptInvUnusedSTBVC();
            dt = objTran.GetDetails(htAddPlanParams, username);
            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }


            if (dt.Rows.Count == 0)
            {
                btnGenerateExcel.Visible = false;
                grdExpiry.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btnGenerateExcel.Visible = true;
                grdExpiry.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdExpiry.DataSource = dt;
                grdExpiry.DataBind();
               // ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdExpiry.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");

            }
        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }
        protected void ExportExcel()
        {
            
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["searched_trans"];
            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "Inv_UnusedSTB" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                        + "STB No/VC ID" + Convert.ToChar(9)
                        + "Lco code" + Convert.ToChar(9)
                        + "Type" + Convert.ToChar(9)
                        + "Box Type" + Convert.ToChar(9)
                        + "Scheme Name" + Convert.ToChar(9)
                        + "Plan Name" + Convert.ToChar(9)
                        + "Activation Allowed" + Convert.ToChar(9)
                        + "Termination Allowed" + Convert.ToChar(9)
                        + "Plan Change Allowed" + Convert.ToChar(9)
                        + "Inserted By" + Convert.ToChar(9)
                        + "Inserted Date" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["stbno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Type"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Boxtype"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["schname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["planname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["activation_allowed"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["termination_allowed"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["plan_change_allowed"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["insby"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["insdate"].ToString() + Convert.ToChar(9);
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

                dt.Dispose();
                Response.Redirect("../MyExcelFile/" + "Inv_UnusedSTB" + datetime + ".xls");
                
            }


            if (dt.Rows.Count == 0)
            {
                grdExpiry.Visible = false;
                lblSearchMsg.Text = "No data found";
            }

        }

    }
}