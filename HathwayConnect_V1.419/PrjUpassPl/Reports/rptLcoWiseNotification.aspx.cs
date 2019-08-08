using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PrjUpassBLL.Reports;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Threading;
using System.Drawing;
using System.IO;

namespace PrjUpassPl.Reports
{
    public partial class rptLcoWiseNotification : System.Web.UI.Page
    {
        decimal amt = 0;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Notification Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                grdAddPlantopup.PageIndex = 0;

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


        private Hashtable getTopupParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            Hashtable htTopupParams = new Hashtable();
            htTopupParams.Add("from", from);
            htTopupParams.Add("to", to);
            return htTopupParams;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            binddata();


        }

        private DataTable binddata()
        {
            btnGenerateExcel.Visible = false;
            string search_type = RadSearchby.SelectedValue.ToString();
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
                    grdAddPlantopup.Visible = false;
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
                else
                {
                    lblSearchMsg.Text = "";
                    grdAddPlantopup.Visible = true;
                }
            }

            Hashtable htTopupParams = getTopupParamsData();

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
                return null;
            }

            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);


            DataTable DtObj = new DataTable();
            string StrQry = @"SELECT  a.""ACCOUNT NO"", a.vcid, a.""CUSTOMER NAME"", a.""NOTIFICATION TYPE"", a.message, a.frequency,";
            StrQry += @"a.duration, a.position, case when a.action='I' then 'Include' else 'Exclude' end Action, a.""INSERTED BY"",REFERENCEID, ";
            StrQry += @"a.""INSERTED DATE"" FROM view_lcopre_notification_rpt a where ";

            StrQry += " trunc(a.dt) >= '" + from + "' and trunc(a.dt) <= '" + to + "'";
            if (ddlType.SelectedValue != "0")
            {
                StrQry += @" and a.""NOTIFICATION TYPE""='" + ddlType.SelectedValue + "'";
            }
            if (ddlLco.SelectedValue != "0")
            {
                StrQry += @" and a.""INSERTED BY""='" + ddlLco.SelectedValue.ToString().Trim() + "'";
            }
            if (txtsearchpara.Text.ToString().Trim() != "")
            {
                if (search_type == "0" && txtsearchpara.Text.Length < 11)
                {
                    string valid = SecurityValidation.chkData("N", txtsearchpara.Text);
                    if (valid == "")
                        StrQry += @" and a.""ACCOUNT NO""= '" + txtsearchpara.Text.ToString() + "'";
                    else
                    {
                        lblSearchMsg.Text = valid.ToString();
                        return null;
                    }
                }
                else if (search_type == "1")
                {
                    string valid = SecurityValidation.chkData("T", txtsearchpara.Text);
                    if (valid == "")
                        StrQry += @" and a.VCID= '" + txtsearchpara.Text.ToString() + "'";
                    else
                    {
                        lblSearchMsg.Text = valid.ToString();
                        return null;
                    }
                }
            }

            OracleCommand Cmd = new OracleCommand(StrQry, ConObj);
            OracleDataAdapter DaObj = new OracleDataAdapter(Cmd);
            DaObj.Fill(DtObj);




            if (DtObj.Rows.Count == 0)
            {
                grdAddPlantopup.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {

                grdAddPlantopup.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = DtObj;
                grdAddPlantopup.DataSource = DtObj;
                grdAddPlantopup.DataBind();
                btnGenerateExcel.Visible = true;


                return DtObj;
            }
            return null;
        }



        protected void ExportExcel()
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=LCO_notification.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                grdAddPlantopup.AllowPaging = false;
                this.binddata();
                grdAddPlantopup.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grdAddPlantopup.HeaderRow.Cells)
                {
                    cell.BackColor = grdAddPlantopup.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grdAddPlantopup.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grdAddPlantopup.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grdAddPlantopup.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grdAddPlantopup.RenderControl(hw);
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

        protected void grdAddPlantopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAddPlantopup.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void RadSearchby_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

        protected void grdAddPlantopup_Sorting(object sender, GridViewSortEventArgs e)
        {

        }




    }
}