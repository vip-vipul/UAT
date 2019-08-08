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
using System.Threading;
using System.Drawing;

namespace PrjUpassPl.Reports
{
    public partial class RptCustEcsDet : System.Web.UI.Page
    {
        string username,catid,operator_id;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Customer auto renewal report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = "A";
                grd.PageIndex = 0;

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
            string hdnslctcols = hdnslctcolumns.Value;
            Cls_Business_RptCustEcsDetails objTran = new Cls_Business_RptCustEcsDetails();
            DataTable dt = objTran.getCustEcsDetails(username, catid, operator_id,"",hdnslctcols);

            if (dt.Rows.Count == 0)
            {
                
                //btnAll.Visible = false;
                //grdLcodet.Visible = false;
                btnGenerateExcel.Visible = false;
                lblSearchMsg.Text = "No data found";
               
                //btngrnExel.Visible = false;
                //grdLcoCustEcsDetails.DataSource = "No data found";
                //grdLcoCustEcsDetails.DataBind();
            }
            else
            {
                ViewState["grddata"] = dt;
                //btnAll.Visible = true;
                //btngrnExel.Visible = true;
                //grdLcodet.Visible = true;
                //btngrnExel.Visible = true;
                //lblSearchMsg.Text = "";
                //ViewState["
                btnGenerateExcel.Visible = true;
                grd.DataSource = dt;
                grd.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grd.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");

            }
        }
       
        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
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
                grd.DataSource = dataTable;
                grd.DataBind();
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

             protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

             protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
             {
                 grd.PageIndex = e.NewPageIndex;
                 BindData();
             }

             protected void ExportExcel()
             {

                 Response.Clear();
                 Response.Buffer = true;
                 Response.AddHeader("content-disposition", "attachment;filename=Customer_Details.xls");
                 Response.Charset = "";
                 Response.ContentType = "application/vnd.ms-excel";
                 using (StringWriter sw = new StringWriter())
                 {
                     HtmlTextWriter hw = new HtmlTextWriter(sw);

                     GridView GridView1 = new GridView();
                     GridView1.DataSource = (DataTable)ViewState["grddata"];
                     GridView1.DataBind();
                     //To Export all pages
                     GridView1.AllowPaging = false;

                     //string hdnslctcols = hdnslctcolumns.Value;

                     //Cls_Business_RptCustEcsDetails objTran = new Cls_Business_RptCustEcsDetails();
                     //DataTable dt = objTran.getCustEcsDetails(username, catid, operator_id, "", hdnslctcols);

                     grd.HeaderRow.BackColor = Color.White;
                     foreach (TableCell cell in grd.HeaderRow.Cells)
                     {
                         cell.BackColor = grd.HeaderStyle.BackColor;
                     }
                     foreach (GridViewRow row in GridView1.Rows)
                     {
                         row.BackColor = Color.White;
                         foreach (TableCell cell in row.Cells)
                         {
                             if (row.RowIndex % 2 == 0)
                             {
                                 cell.BackColor = grd.AlternatingRowStyle.BackColor;
                             }
                             else
                             {
                                 cell.BackColor = grd.RowStyle.BackColor;
                             }
                             cell.CssClass = "textmode";
                         }
                     }

                     GridView1.RenderControl(hw);
                     string style = @"<style> .textmode { } </style>";
                     Response.Write(style);
                     Response.Output.Write(sw.ToString());
                     Response.Flush();
                     Response.End();
                 }

             }

             public override void VerifyRenderingInServerForm(Control control)
             {
                 /* Verifies that the control is rendered */
             }
    }
}