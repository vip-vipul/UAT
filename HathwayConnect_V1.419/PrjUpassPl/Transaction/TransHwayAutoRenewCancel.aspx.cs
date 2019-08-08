using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;
using PrjUpassPl.Helper;
using PrjUpassBLL.Reports;
using System.IO;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayAutoRenewCancel : System.Web.UI.Page
    {

        string username, catid, operator_id;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session["RightsKey"] = null; 
                grdLcoCustEcsDetails.PageIndex = 0;

                if (Session["username"] != null || Session["operator_id"] != null)
                {
                    username = Session["username"].ToString();
                    catid = Convert.ToString(Session["category"]);
                    operator_id = Convert.ToString(Session["operator_id"]);

                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                    return;
                }



                //setting page heading
                //Master.PageHeading = "User Details Report(Lco wise)";

                BindData();

            }
        }

        protected void BindData()
        {

            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptCustEcsDetails objTran = new Cls_Business_RptCustEcsDetails();
            DataTable dt = objTran.getCustEcsDetails(username, catid, operator_id,"Y","");

            if (dt.Rows.Count == 0)
            {
                //btnAll.Visible = false;
                //grdLcodet.Visible = false;
                //lblSearchMsg.Text = "No data found";
                //btngrnExel.Visible = false;
                grdLcoCustEcsDetails.DataSource = null;
                grdLcoCustEcsDetails.DataBind();
                btnSubmit.Visible = false;
            }
            else
            {
                //btnAll.Visible = true;
                //btngrnExel.Visible = true;
                //grdLcodet.Visible = true;
                //btngrnExel.Visible = true;
                //lblSearchMsg.Text = "";
                //ViewState["searched_Lco"] = dt;
                grdLcoCustEcsDetails.DataSource = dt;
                grdLcoCustEcsDetails.DataBind();

            }
        }

        protected void grdLcoCustEcsDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable;
            string sort_direction;
            if (ViewState[e.SortExpression.ToString()] == null)
            {
                ViewState[e.SortExpression.ToString()] = Convert.ToString(e.SortDirection);
                sort_direction = Convert.ToString(e.SortDirection);
            }
            else
            {
                if (ViewState[e.SortExpression.ToString()].ToString() == "Ascending")
                {
                    ViewState[e.SortExpression.ToString()] = "Descending";
                    sort_direction = "Descending";
                }
                else
                {
                    ViewState[e.SortExpression.ToString()] = "Ascending";
                    sort_direction = "Ascending";
                }
            }

            if (ViewState["searched_trans"] != null)
            {
                dataTable = (DataTable)ViewState["searched_trans"];
            }
            else
            {
                return;
            }
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + SetSortDirection(sort_direction);
                grdLcoCustEcsDetails.DataSource = dataTable;
                grdLcoCustEcsDetails.DataBind();
            }
        }

        protected string SetSortDirection(string sortDirection)
        {
            if (sortDirection == "Ascending")
            {
                return "DESC";
            }
            else
            {
                return "ASC";
            }
        }

        protected void grdLcoCustEcsDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLcoCustEcsDetails.PageIndex = e.NewPageIndex;
            BindData();
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            
            
            foreach (GridViewRow row in grdLcoCustEcsDetails.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkautorenew") as CheckBox);
                    if (chkRow.Checked)
                    {
                        cnt++;
                    }
                }
            }
            if (cnt == 0)
            {
                lblPopupFinalConfMsg.Text = "Please Select Atleast One Plan To Cancel!";
                Button1.Value = "OK";
                btnPopupConfYes.Visible = false;
                
            }
            else
            {
                btnPopupConfYes.Visible = true;
                Button1.Value = "Cancel";
                lblPopupFinalConfMsg.Text = "Are you sure you want to cancel selected plans!";
                
            }

            popFinalConf.Show();


            //if (cnt == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select Atleast One Plan To Cancel!');", true);
            //    pnlFinalConfirm.Visible = false;
            //    return;

            //}
            //else
            //{
            //    pnlFinalConfirm.Visible = true;
            //    popFinalConf.Show();
            //}
            

        }



        protected void btnPopupConfYes_Click(object sender, EventArgs e)
        {
            ProceedCancel();
        }

        public void ProceedCancel()
        {
            string CustId = "";
            string VcId = "";
            string PlanName = "";
            DataTable dt = new DataTable();

            dt.Columns.Add("CustId");
            dt.Columns.Add("VcId");
            dt.Columns.Add("PlanName");

            foreach (GridViewRow row in grdLcoCustEcsDetails.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkautorenew") as CheckBox);
                    if (chkRow.Checked)
                    {
                        CustId = row.Cells[1].Text;
                        VcId = row.Cells[2].Text;
                        PlanName = row.Cells[7].Text;

                        dt.Rows.Add(CustId, VcId, PlanName);


                    }
                }
            }

            /* string str = "";
             for (int i = 0; i < dt.Rows.Count; i++)
             {
                 str += dt.Rows[i][0].ToString() + "$"+ dt.Rows[i][1].ToString() + "$" + dt.Rows[i][2].ToString() + "$";
             }
             string strvalue = str.TrimEnd('$');

             Hashtable ht = new Hashtable();
             ht.Add("username", username);
             ht.Add("strvalue", strvalue);

             Cls_BLL_AutoRenewCancel obj = new Cls_BLL_AutoRenewCancel();
             string msg = obj.AutoRenewCancel(username, ht);*/

            int cnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string str = " update aoup_lcopre_ecs_det set  var_ecs_isactive ='C' ";
                str += " where var_ecs_vcid ='" + dt.Rows[i][1].ToString() + "'";
                str += " and var_ecs_planname ='" + dt.Rows[i][2].ToString() + "'";
                str += " and var_ecs_isactive ='" + "Y" + "'";
                cnt++;
                Cls_Helper obj = new Cls_Helper();
                int returnval = obj.insertQry(str);
            }
                     
            BindData();

        }
    }
}
