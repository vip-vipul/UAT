using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Master;
using System.IO;
using PrjUpassBLL.Reports;

namespace PrjUpassPl.Reports
{
    public partial class rptBulkEcafReport : System.Web.UI.Page
    {
        string username, catid, operator_id;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
  Session["RightsKey"] = "N";
            Master.PageHeading = "Bulk Ecaf Report";
            if (!IsPostBack)
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
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                
            }
        }
        protected void binddata()
        {

            DataTable dt = new DataTable();
            string from = txtFrom.Text;
            string to = txtTo.Text;
            Cls_Business_RptBulkTransProces objTran = new Cls_Business_RptBulkTransProces();
            dt = objTran.GetBulkEcafDetails(Session["username"].ToString(), from, to);
            if (dt.Rows.Count == 0)
            {

                grdBulkUpload.DataSource = null;
                grdBulkUpload.DataBind();
                grdBulkUpload.Visible = false;
                lblSearchMsg.Text = "No data found";
             
            }
            else
            {
                //lblSearchParams.Text = "SELECT * from view_lcopre_bulk_ecaf_summary where trunc(insdate)>='" + from + "' and trunc(insdate)<='" + to + "' and insby='" + Session["username"].ToString() + "'";

                grdBulkUpload.DataSource = dt;
                ViewState["Data"] = dt;
                grdBulkUpload.DataBind();
                grdBulkUpload.Visible = true;
             
            }
        }
        protected void btn_genExl_Click(object sender, EventArgs e)
        {
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
            Cls_Business_RptBulkTransProces objTran = new Cls_Business_RptBulkTransProces();
            DataTable dt = null; //check for exception
            string from = txtFrom.Text;
            string to = txtTo.Text;
            dt = objTran.GetBulkEcafDetails(Session["username"].ToString(), from, to);
            ViewState["Data"] = dt;
            if (ViewState["Data"] != null)
            {
                dt = (DataTable)ViewState["Data"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "BulkEcaf_" + datetime + ".csv");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + ","
                        + "Uniqu No" + ","
                        + "STB NO" + ","
                        + "VC ID" + ","
                        + "VC ID" + ","
                        + "First Name" + ","
                        + "Meddil Name" + ","
                        + "Last Name" + ","
                        + "Mobile No" + ","
                        + "Email Id" + ","
                        + "User Name" + ","
                        + "Status" + ","
                        + "Status MSG" + ","
                        + "Transaction by" + ","
                        + "'Transaction Date" ;


                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + ","
                                + dt.Rows[i]["uniquno"].ToString() + ","
                                + dt.Rows[i]["stb"].ToString() + ","
                                + dt.Rows[i]["vc"].ToString() + ","
                                + dt.Rows[i]["firstnm"].ToString() + ","
                                + dt.Rows[i]["meddilnm"].ToString() + ","
                                + dt.Rows[i]["lastnm"].ToString() + ","
                                + dt.Rows[i]["mobileno"].ToString() + ","
                                + dt.Rows[i]["email"].ToString() + ","
                                + dt.Rows[i]["Status"].ToString() + ","
                                + dt.Rows[i]["errormsg"].ToString() + ","
                            + dt.Rows[i]["insby"].ToString() + ","
                            +"'"+ dt.Rows[i]["insdate"].ToString() ;


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
                Response.AddHeader("Content-disposition", "attachment; filename=BulkEcaf_" + datetime + ".csv");
                Response.ContentType = "text/csv";
                Response.Redirect("../MyExcelFile/" + "BulkEcaf_" + datetime + ".csv");
            }

        }
        protected void btn_genExcel_Click(object sender, EventArgs e)
        {
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
            Cls_Business_RptBulkTransProces objTran = new Cls_Business_RptBulkTransProces();
            DataTable dt = null; //check for exception
            string from = txtFrom.Text;
            string to = txtTo.Text;
            dt = objTran.GetBulkEcafDetails(Session["username"].ToString(), from, to);
            ViewState["Data"] = dt;
            if (ViewState["Data"] != null)
            {
                dt = (DataTable)ViewState["Data"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "BulkEcaf_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                        + "Uniqu No" + Convert.ToChar(9)
                        + "STB NO" + Convert.ToChar(9)
                        + "VC ID" + Convert.ToChar(9)
                        + "VC ID" + Convert.ToChar(9)
                        + "First Name" + Convert.ToChar(9)
                        + "Meddil Name" + Convert.ToChar(9)
                        + "Last Name" + Convert.ToChar(9)
                        + "Mobile No" + Convert.ToChar(9)
                        + "Email Id" + Convert.ToChar(9)
                        + "User Name" + Convert.ToChar(9)
                        + "Status" + Convert.ToChar(9)
                        + "Status MSG" + Convert.ToChar(9)
                        + "Transaction by" + Convert.ToChar(9)
                        + "'Transaction Date";


                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["uniquno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["stb"].ToString() + Convert.ToChar(9)
                                +"'"+ dt.Rows[i]["vc"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["firstnm"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["meddilnm"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lastnm"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["mobileno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["email"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Status"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["errormsg"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["insby"].ToString() + Convert.ToChar(9)
                            + "'" + dt.Rows[i]["insdate"].ToString();


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
                Response.AddHeader("Content-disposition", "attachment; filename=BulkEcaf_" + datetime + ".xls");
                Response.ContentType = "text/csv";
                Response.Redirect("../MyExcelFile/" + "BulkEcaf_" + datetime + ".xls");
            }

        }


        protected void grdBulkUpload_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                int rowindex = clickedRow.RowIndex;
                string upload_id = ((HiddenField)clickedRow.FindControl("hdnUploadId")).Value;

                int status_flag = -1;
                if (e.CommandName.Equals("total"))
                {
                    status_flag = 0;
                }
                else if (e.CommandName.Equals("success"))
                {
                    status_flag = 1;
                }
                else if (e.CommandName.Equals("fail"))
                {
                    status_flag = 2;
                }

                generateExcel(upload_id, status_flag);
            }
            catch (Exception ex)
            {
                lblSearchMsg.Text = ex.ToString();
               // lblSearchParams.Text = "select * from view_lcopre_BulkEcaf a where a.UNIQUNO = '" + upload_id + "'";
            }
        }

        protected void generateExcel(string upload_id, int status_flag)
        {
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
            Cls_Business_RptBulkTransProces obj = new Cls_Business_RptBulkTransProces();
            DataTable dt = null;
            dt = obj.GetExcelData(username, upload_id, status_flag);
            
            if (dt == null)
            {
                lblSearchMsg.Text = "Something went wrong while fetching details...";
                return;
            }
            if (dt.Rows.Count == 0)
            {
                lblSearchMsg.Text = "No data found for clicked count figure...";
                return;
            }

            DateTime dd = DateTime.Now;
            string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

            string xls_file_name = "";
            if (status_flag == 0)
            {
                xls_file_name += "_all_";
            }
            else if (status_flag == 1)
            {
                xls_file_name += "_success_";
            }
            else if (status_flag == 2)
            {
                xls_file_name += "_fail_";
            }

            StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + xls_file_name + datetime + ".xls");
            try
            {
                int j = 0;
                String strheader = "Sr. No." + Convert.ToChar(9)
                         + "Unique No" + Convert.ToChar(9)
                         + "STB NO" + Convert.ToChar(9)
                         + "VC ID" + Convert.ToChar(9)
                         + "First Name" + Convert.ToChar(9)
                         + "Meddle Name" + Convert.ToChar(9)
                         + "Last Name" + Convert.ToChar(9)
                         + "Mobile No" + Convert.ToChar(9)
                         + "Email Id" + Convert.ToChar(9)
                         + "Status" + Convert.ToChar(9)
                         + "Status MSG" + Convert.ToChar(9)
                         + "Transaction by" + Convert.ToChar(9)
                         + "'Transaction Date";
                while (j < dt.Rows.Count)
                {
                    sw.WriteLine(strheader);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        j = j + 1;
                        string strrow = j.ToString() + Convert.ToChar(9)
                               + dt.Rows[i]["UNIQUNO"].ToString() + Convert.ToChar(9)
                               +"'" + dt.Rows[i]["stb"].ToString() + Convert.ToChar(9)
                               + "'" + dt.Rows[i]["vc"].ToString() + Convert.ToChar(9)
                               + dt.Rows[i]["firstnm"].ToString() + Convert.ToChar(9)
                               + dt.Rows[i]["meddilnm"].ToString() + Convert.ToChar(9)
                               + dt.Rows[i]["lastnm"].ToString() + Convert.ToChar(9)
                               + dt.Rows[i]["mobileno"].ToString() + Convert.ToChar(9)
                               + dt.Rows[i]["email"].ToString() + Convert.ToChar(9)
                               + dt.Rows[i]["Status"].ToString() + Convert.ToChar(9)
                               + dt.Rows[i]["errormsg"].ToString() + Convert.ToChar(9)
                           + dt.Rows[i]["insby"].ToString() + Convert.ToChar(9)
                           + "'" + dt.Rows[i]["insdate"].ToString();
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
            Response.Redirect("../MyExcelFile/" + xls_file_name + datetime + ".xls");
        }
    

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                string from = txtFrom.Text;
                string to = txtTo.Text;
                lblResultCount.Text = "";
                DateTime fromDt;
                DateTime toDt;
                if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
                {
                    fromDt = new DateTime();
                    toDt = new DateTime();
                    fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                    toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                    if (toDt.CompareTo(fromDt) < 0)
                    {
                        lblSearchMsg.Text = "To date must be later than From date";
                        lblSearchMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else if (Convert.ToDateTime(txtFrom.Text.ToString()) > DateTime.Now.Date)
                    {
                        lblSearchMsg.Text = "You can not select date greater than current date!";
                        return;
                    }
                    else if (Convert.ToDateTime(txtTo.Text.ToString()) > DateTime.Now.Date)
                    {
                        lblSearchMsg.Text = "You can not select date greater than current date!";
                        return;
                    }
                    else
                    {
                        lblSearchMsg.Text = "";
                    }
                }
                binddata();

            }
            catch (Exception ex)
            {
                lblSearchMsg.Text = ex.ToString();
            }
        }
    }
}