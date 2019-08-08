using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;
using System.Data;

namespace PrjUpassPl.Reports
{
    public partial class frmMiddlePageBulkTransaction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["RightsKey"] = "N";
            Master.PageHeading = "Bulk Upload Process";
            if(!IsPostBack)
            {
                if (Request.QueryString["uniqueid"] != null)
                {
                    string uniqueid = Request.QueryString["uniqueid"].ToString();
                    ViewState["uniqueid"] = uniqueid;
                    btnRefresh.Attributes.Add("style", "display:none");
                    lnkShwoAll.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "javascript:ShowModalPopup();", true);
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            callprocedure();
        }
        private DataTable generateDataTable(string[] err_data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("custno");
            dt.Columns.Add("vc");
            dt.Columns.Add("lcocode");
            dt.Columns.Add("plan");
            dt.Columns.Add("action");
            dt.Columns.Add("err");
            for (int i = 0; i < err_data.Length; i++)
            {
                string[] err_det = err_data[i].Split('$');
                try
                {
                    DataRow dr = dt.NewRow();
                    dr["custno"] = err_det[0];
                    dr["vc"] = err_det[1];
                    dr["lcocode"] = err_det[2];
                    dr["plan"] = err_det[3];
                    dr["action"] = err_det[4];
                    dr["err"] = err_det[8];
                    dt.Rows.Add(dr);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
            return dt;
        }

        private void callprocedure()
        {
            Cls_Business_TransHwayBulkOperation obj = new Cls_Business_TransHwayBulkOperation();
            string pro_output = obj.bulkUploadTemp(Session["lco_username"].ToString(), ViewState["uniqueid"].ToString());
            if (pro_output.Split('#')[0] == "9999")
            {
                lblStatusHeading.Text = "File Upload : Successful";
                lblStatus.Text = "";
                //btnBeginTrans.Visible = true;
                Response.Redirect("../Reports/rptBulkTransactionProc.aspx?uniqueid=" + ViewState["uniqueid"].ToString());

            }
            else if (pro_output.Split('#')[0] == "-301")
            {
                lblStatusHeading.Text = "File verification : Failed";
                lnkShwoAll.Visible = true;
                lblStatus.Text = pro_output.Split('#')[1];
            }
            else if (pro_output.Split('#')[0] == "-300")
            {
                lblStatusHeading.Text = "File verification : Failed";
                lnkShwoAll.Visible = true;
                lblStatus.Text = "Sorry ";
            }
            else
            {
                lblStatusHeading.Text = "File verification : Failed";
                lnkShwoAll.Visible = false;
                lblStatus.Text = pro_output.Split('#')[1];
            }
        }

        protected void lnkShwoAll_Click(object sender, EventArgs e)
        {
            Cls_Business_TransHwayBulkOperation obj = new Cls_Business_TransHwayBulkOperation();
            string pro_output = obj.bulkUploadDuplicate(Session["username"].ToString(), ViewState["uniqueid"].ToString());
            string raw_message = pro_output.Split('#')[1];
            if (raw_message.Contains('~'))
            {
                string[] err_data = raw_message.TrimEnd('~').Split('~');
                DataTable dtErrData = generateDataTable(err_data);
                rptErrData.DataSource = dtErrData;
                rptErrData.DataBind();
                pnlErrData.Visible = true;
            }

        }
    }
}