using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Reports
{
    public partial class rptBulkActFileProcessRemove : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["RightsKey"] = "N";
            Master.PageHeading = "Bulk Scheduler File Process ";
            if (!IsPostBack)
            {
                if (Session["username"] != null)
                {

                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                Binddata();
            }
        }

        public void Binddata()
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string getexcel = "";
            if (Session["flag"].ToString() == "E")
            {
                getexcel = "select num_lcopre_bulk_id,var_lcopre_bulk_useruniqueid useruniqueid,var_lcopre_bulk_custid custid,var_lcopre_bulk_lcocode,var_lcopre_bulk_planname planname,date_lcobulk_date date1,";
                getexcel += "date_lcobulk_rnewalflag from  aoup_lcopre_bulkact_temp where var_lcopre_bulk_useruniqueid='" + Session["uniqueid"].ToString() + "' and date_lcobulk_date='" + Session["processdt"].ToString() + "'";
            }
            else
            {
                getexcel = "select num_lcopre_bulk_id,var_lcopre_bulk_useruniqueid useruniqueid,var_lcopre_bulk_custid custid,var_lcopre_bulk_lcocode,var_lcopre_bulk_planname planname,date_lcobulk_date date1,";
                getexcel += "date_lcobulk_rnewalflag from  aoup_lcopre_bulkact_temphis where var_lcopre_bulk_useruniqueid='" + Session["uniqueid"].ToString() + "' and date_lcobulk_date='" + Session["processdt"].ToString() + "'";
            }

            OracleCommand cmd = new OracleCommand(getexcel, con);
            OracleDataAdapter DaObj = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();

            DaObj.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                if (Session["flag"].ToString() == "E")
                {
                    grdBulkstatus.Columns[6].Visible = true;
                }
                else
                {
                    grdBulkstatus.Columns[6].Visible = false;
                }
                grdBulkstatus.DataSource = dt;
                grdBulkstatus.DataBind();
                grdBulkstatus.Visible = true;
            }
            else
            {
                grdBulkstatus.Visible = false;
            }
        }
        protected void grdBulkProc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Remove"))
            {
                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    string lblbulkid = ((Label)clickedRow.FindControl("lblbulkid")).Text;
                    Session["Bulk_ID"] = lblbulkid;
                    popMsgBox.Show();



                }
                catch (Exception ex)
                {
                    Response.Write("Error : " + ex.Message.Trim());
                    return;
                }

            }
        }
        protected void btncnfmBlck_Click(object sender, EventArgs e)
        {
            try
            {
                Cls_Business_TransHwayBulkOperation obj = new Cls_Business_TransHwayBulkOperation();
                string pro_output = obj.bulkUploadActTempRemove(Session["Bulk_ID"].ToString(), Session["uniqueid"].ToString(), Session["processdt"].ToString(), Session["username"].ToString().Trim());
                if (pro_output.Split('#')[0] == "9999")
                {
                    lblResponseMsg.Text = "Selected Data Removed Successfully";
                    Binddata();
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error : " + ex.Message.Trim());
                return;
            }
        }

        protected void grdBulkProc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBulkstatus.PageIndex = e.NewPageIndex;
            Binddata();
        }
    }
}