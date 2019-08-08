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
    public partial class rptCustomerModify : System.Web.UI.Page
    {
        string operid;
        string username;
        string catid;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            //((ScriptManager)Master.FindControl("ScriptManager1")).RegisterPostBackControl(ExportToExcel);
            Master.PageHeading = "Customer Modification Report";
            if (!IsPostBack)
            {
                // Session["RightsKey"] = null;
                Session["RightsKey"] = "N";
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grd.DataSource = null;
            grd.DataBind();
            btnGenerateExcel.Visible = false;
            // string username = "";
            string oper_id = "";
            string user_brmpoid = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
            {
                username = Convert.ToString(Session["username"]);
                oper_id = Convert.ToString(Session["operator_id"]);
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            binddata();

        }

        private DataTable getData()
        {
            try
            {
                string search_type = "0";//RadSearchby.SelectedValue.ToString();
                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();

                string from = txtFrom.Text;
                string to = txtTo.Text;

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
                        return null;
                    }
                    else if (Convert.ToDateTime(txtFrom.Text.ToString()) > DateTime.Now.Date)
                    {
                        lblSearchMsg.Text = "You can not select date greater than current date!";
                        return null;
                    }
                    else if (Convert.ToDateTime(txtTo.Text.ToString()) > DateTime.Now.Date)
                    {
                        lblSearchMsg.Text = "You can not select date greater than current date!";
                        return null;
                    }


                    OracleConnection con = new OracleConnection(strCon);
                    string str = "";

                    str = "select " + hdnslctcolumns.Value + " from view_lcopre_Cust_Modify a where 0=0   ";
                    str += @" and  a.""Insert By""= '" + username + "' ";
                    if (txtsearchpara.Text.Length > 0)
                    {
                        if (search_type == "0")
                        {
                            string valid = SecurityValidation.chkData("N", txtsearchpara.Text);

                            if (valid == "")
                                str += @" and a.""Account No""= '" + txtsearchpara.Text + "' ";
                            else
                            {
                                lblSearchMsg.Text = valid.ToString();
                                return null;
                            }
                          //  str += @" and a.""Account No""= '" + txtsearchpara.Text + "' ";
                            
                        }
                        else if (search_type == "1")
                        {
                            str += @" and a.""VC/MAC Id""= '" + txtsearchpara.Text + "'";
                           // str += @" and  upper(a.""Lco Code"")= upper('" + username + "') ";
                        }
                        else if (search_type == "2")
                        {
                            str += @" and a.""Lco Code""= '" + txtsearchpara.Text + "'";
                        }
                    }
                    str += " and trunc(a.INSDT) >= '" + from + "' and trunc(a.INSDT) <= '" + to + "'";
                   
                    DataTable DtObj = new DataTable();

                    OracleCommand Cmd = new OracleCommand(str, con);
                    OracleDataAdapter DaObj = new OracleDataAdapter(Cmd);
                    DaObj.Fill(DtObj);
                    return DtObj;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        protected void binddata()
        {
            string search_type = "0";// RadSearchby.SelectedValue.ToString();
            try
            {

                DataTable DtObj = new DataTable();
                DtObj = getData();
                if (DtObj != null)
                {
                    if (DtObj.Rows.Count == 0)
                    {
                        btnGenerateExcel.Visible = false;
                        grd.Visible = false;
                        lblSearchMsg.Text = "No data found";
                    }
                    else
                    {
                        btnGenerateExcel.Visible = true;
                        grd.Visible = true;
                        lblSearchMsg.Text = "";
                        grd.DataSource = DtObj;
                        grd.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {

                Response.Write("Error : " + ex.Message.Trim());
                return;
            }



        }
        protected void ExportExcel()
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=CustModify.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                grd.AllowPaging = false;
                this.getData();
                grd.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grd.HeaderRow.Cells)
                {
                    cell.BackColor = grd.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grd.Rows)
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

                grd.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

        }


        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            binddata();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }


        protected void RadSearchby_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["category"].ToString() == "3")
            {
                
                    lblSearchMsg.Text = "";
                   // ViewState["islco"] = "0";
                    btnSearch.Visible = true;
                    grd.Visible = false;
                    txtsearchpara.Text = "";
                    txtsearchpara.Enabled = true;
                    btnGenerateExcel.Visible = false;
                
            }
        }

    }
}