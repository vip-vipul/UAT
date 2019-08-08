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
    public partial class rptNotificationSent : System.Web.UI.Page
    {
        string operid;
        string username;
        string catid;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            //((ScriptManager)Master.FindControl("ScriptManager1")).RegisterPostBackControl(ExportToExcel);
            Master.PageHeading = "Notification Sent Report";
            if (!IsPostBack)
            {
                // Session["RightsKey"] = null;
                Session["RightsKey"] = "N";
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();

                FillLcoDetails();
            }

        }

        protected void FillLcoDetails()
        {
            string str = "";
            string operator_id = "";
            string category_id = "";
            if (Session["operator_id"] != null && Session["category"] != null)
            {
                operator_id = Convert.ToString(Session["operator_id"]);
                category_id = Convert.ToString(Session["category"]);
            }
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,var_lcomst_code lcocode ";
                str += "     FROM aoup_lcopre_lco_det a ,aoup_operator_def c,aoup_user_def u ";
                str += "  WHERE a.num_lcomst_operid = c.num_oper_id and  a.num_lcomst_operid=u.num_user_operid and u.var_user_username=a.var_lcomst_code  ";
                if (category_id == "11")
                {
                    str += "  and c.num_oper_clust_id =" + operator_id;
                }
                else if (category_id == "3")
                {
                    str += "and a.num_lcomst_operid =  " + operator_id + " ";
                }
                else
                {

                    //  lblmsg.Text = "No LCO Details Found";
                    // Paydet.Visible = false;
                    // btnSubmit.Visible = false;
                    //  divdet.Visible = false;
                    // pnllco.Visible = false;
                    return;
                }
                DataTable tbllco = GetResult(str);

                if (tbllco.Rows.Count > 0)
                {
                    // pnllco.Visible = true;
                    ddlLco.DataTextField = "name";
                    ddlLco.DataValueField = "lcocode";

                    ddlLco.DataSource = tbllco;
                    ddlLco.DataBind();
                    //if (category_id == "11")
                    //{
                    //    ddlLco.Items.Insert(0, new ListItem("Select LCO", "0"));
                    //}
                    //else if (category_id == "3")
                    //{
                    //    //ddllco_SelectedIndexChanged(null, null);
                    //}
                }
                else
                {
                    //  lblmsg.Text = "No LCO Details Found";
                    // divdet.Visible = false;
                    // Paydet.Visible = false;
                    // btnSubmit.Visible = false;
                    // pnllco.Visible = false;
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }

        }
        public DataTable GetResult(String Query)
        {
            DataTable MstTbl = new DataTable();


            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();

            OracleCommand Cmd = new OracleCommand(Query, con);
            OracleDataAdapter AdpData = new OracleDataAdapter();
            AdpData.SelectCommand = Cmd;
            AdpData.Fill(MstTbl);

            con.Close();

            return MstTbl;
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
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
                string search_type = RadSearchby.SelectedValue.ToString();
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

                    str = "select " + hdnslctcolumns.Value + " from view_lcopre_notification_sent a where 0=0   ";
                    str += @" and  a.""Lco Code""= '" + ddlLco.SelectedValue + "' ";
                    if (txtsearchpara.Text.Length > 0)
                    {
                        if (search_type == "0" && txtsearchpara.Text.Length < 11)
                        {
                            string valid = SecurityValidation.chkData("N", txtsearchpara.Text);
                            if (valid == "")
                                str += @" and a.""Account No""= '" + txtsearchpara.Text + "' ";
                            else
                            {
                                lblSearchMsg.Text = valid.ToString();
                                return null;
                            }
                            // str += @" and a.""Account No""= '" + txtsearchpara.Text + "' ";

                        }
                        else if (search_type == "1")
                        {
                            string valid = SecurityValidation.chkData("T", txtsearchpara.Text);
                            if (valid == "")
                                str += @" and a.""VC/MAC Id""= '" + txtsearchpara.Text + "'";
                            else
                            {
                                lblSearchMsg.Text = valid.ToString();
                                return null;
                            }
                            //    str += @" and a.""VC/MAC Id""= '" + txtsearchpara.Text + "'";

                        }

                    }
                    str += " and trunc(a.INSDT) >= '" + from + "' and trunc(a.INSDT) <= '" + to + "'";
                    if (ddlType.SelectedValue != "0")
                    {
                        str += @" and a.""Type""='" + ddlType.SelectedValue + "'";
                    }
                    if (ddlLco.SelectedValue != "0")
                    {
                        str += @" and  upper(a.""Lco Code"")= upper('" + ddlLco.SelectedValue.ToString().Trim() + "') ";
                    }
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
            string search_type = RadSearchby.SelectedValue.ToString();
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
            Response.AddHeader("content-disposition", "attachment;filename=notificationSent.xls");
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
                if (RadSearchby.SelectedValue == "2")
                {
                    lblSearchMsg.Text = "";
                    txtsearchpara.Text = Session["username"].ToString();
                    // ViewState["islco"] = "1";
                    //btnGenerateExcel.Visible = true;
                    txtsearchpara.Enabled = false;
                    //btnSearch.Visible = false;
                    // grd.Visible = false;
                }
                else
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
}