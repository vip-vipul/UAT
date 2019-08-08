using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Transaction;
using System.IO;

namespace PrjUpassPl.Transaction
{
    public partial class FrmWarehouseBulkProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["uniqueid"] != null)
                {
                    string uniqueid = Request.QueryString["uniqueid"].ToString();
                    ViewState["uniqueid"] = uniqueid;
                    if (uniqueid != "")
                    {
                        SearchbindData(uniqueid);
                    }
                }
            }
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string Unique_id = "";
                if (ViewState["uniqueid"] == null && Convert.ToString(ViewState["uniqueid"]) == "")
                {

                    lblResponseMsg.Text = "Please Eneter unique  Id....";
                    return;
                }
                else
                {
                    if (ViewState["uniqueid"].ToString() != "")
                    {
                        Unique_id = ViewState["uniqueid"].ToString();
                    }

                }

                SearchbindData(Unique_id);



            }
            catch (Exception ex)
            {
                Response.Redirect("../errorPage.aspx");
            }
        }
        protected void SearchbindData(string Unique_id)
        {

            string username, catid, operator_id;
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
            DataTable dt = new DataTable();
            Cls_Business_Warehouse obj = new Cls_Business_Warehouse();

           // dt = objTran.GetDetails(username, operator_id, catid, Unique_id);
            dt = obj.Getbulkprocessdata(Session["username"].ToString(), Unique_id);
            if (dt == null)
            {
                lblResponseMsg.Text = "No Data Found";
            }
            else if (dt.Rows.Count == 0)
            {
                lblResponseMsg.Text = "No Data Found";
                DivShowValue.Visible = false;
            }


            else
            {
                DivShowValue.Visible = true;
                lblSumFile.Text = dt.Rows[0]["upload_id"].ToString();
                lblSumTotal.Text = dt.Rows[0]["totalstb"].ToString();
                lblSumSuccess.Text = dt.Rows[0]["success"].ToString();
                lblSumFailure.Text = dt.Rows[0]["fail"].ToString();
                lblRemaing.Text = dt.Rows[0]["pending"].ToString();


            }
            if (lblRemaing.Text.Trim() == "0")
            {
                lblRemaing.Enabled = false;
            }
            else
            {
                lblRemaing.Enabled = true;
            }
            if (lblSumFailure.Text.Trim() == "0")
            {
                lblSumFailure.Enabled = false;
            }
            else
            {
                lblSumFailure.Enabled = true;
            }
            if (lblSumSuccess.Text.Trim() == "0")
            {
                lblSumSuccess.Enabled = false;
            }
            else
            {
                lblSumSuccess.Enabled = true;
            }
            if (lblSumTotal.Text.Trim() == "0")
            {
                lblSumTotal.Enabled = false;
            }
            else
            {
                lblSumTotal.Enabled = true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string Unique_id = "";
                if ( ViewState["uniqueid"] == null && Convert.ToString(ViewState["uniqueid"]) == "")
                {

                    lblResponseMsg.Text = "Please Eneter unique  Id....";
                    return;
                }
                else
                {
                     if (ViewState["uniqueid"].ToString() != "")
                    {
                        Unique_id = ViewState["uniqueid"].ToString();
                    }

                }

                SearchbindData(Unique_id);



            }
            catch (Exception ex)
            {
                Response.Redirect("../errorPage.aspx");
            }

        }

        protected void lblSumTotal_Click(object sender, EventArgs e)
        {
            ExportExcel(ViewState["uniqueid"].ToString(), "All");
        }

        protected void lblSumSuccess_Click(object sender, EventArgs e)
        {
            ExportExcel(ViewState["uniqueid"].ToString(), "0");
        }

        protected void lblSumFailure_Click(object sender, EventArgs e)
        {
            ExportExcel(ViewState["uniqueid"].ToString(), "1");
        }

        protected void lblRemaing_Click(object sender, EventArgs e)
        {
            ExportExcel(ViewState["uniqueid"].ToString(), "");

        }

        protected void ExportExcel(string UploadID, string Type)
        {
            DataTable dt = null;
            Cls_Business_Warehouse obj = new Cls_Business_Warehouse();
            dt = obj.Getdata(UploadID, Type, Session["username"].ToString());
            if (dt.Rows.Count > 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "BulkProcess_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                    + "Upload ID" + Convert.ToChar(9)
                    + "Work Order No" + Convert.ToChar(9)
                    + "LCO Code" + Convert.ToChar(9)
                    + "Device ID" + Convert.ToChar(9)
                    + "Box Type" + Convert.ToChar(9)
                    + "Type" + Convert.ToChar(9)
                    + "API Status" + Convert.ToChar(9)
                    + "API Message" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Uploadid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["orderno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["deviceid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["boxtype"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["type"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["APIStatus"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["APImsg"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "BulkProcess_" + datetime + ".xls");
            }
            else
            {
                lblResponseMsg.Text = "No data found ...";
            }

        }
    }
}