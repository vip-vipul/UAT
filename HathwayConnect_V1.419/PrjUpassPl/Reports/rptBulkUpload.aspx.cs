using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using PrjUpassBLL.Reports;
using System.IO;
using System.Configuration;
using System.Data.OracleClient;

namespace PrjUpassPl.Reports
{
    public partial class rptBulkUpload : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Bulk Upload Report";
            txtFrom.Attributes.Add("readonly", "readonly");
            txtTo.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            grdBulkUpload.Visible = false;
            string from = txtFrom.Text;
            string to = txtTo.Text;
            DateTime fromDt;
            DateTime toDt;
            //date validation
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
            {
                fromDt = new DateTime();
                toDt = new DateTime();
                fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    return;
                }
            }
            else
            {
                lblSearchMsg.Text = "From and To date cannot be blank";
                return;
            }

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
            string lco = ddlLco.SelectedValue.ToString().Trim();
            DataTable dt = new DataTable();
            Cls_Business_rptBulkUpload objTran = new Cls_Business_rptBulkUpload();
            dt = objTran.GetBulkDetails(from, to, username, catid, lco);
            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b><b>Upload Details From : </b>" + from + "<b> To : </b>" + to);

            if (dt.Rows.Count == 0)
            {
                grdBulkUpload.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                lblSearchMsg.Text = "";
                grdBulkUpload.DataSource = dt;
                grdBulkUpload.DataBind();

                grdBulkUpload.Visible = true;
            }
        }

        protected void grdBulkUpload_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
            int rowindex = clickedRow.RowIndex;
            string upload_id = ((HiddenField)clickedRow.FindControl("hdnUploadId")).Value;
            string file_name = ((HiddenField)clickedRow.FindControl("hdnFileName")).Value;

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

            generateExcel(upload_id, status_flag, file_name);
        }

        protected void generateExcel(string upload_id, int status_flag, string file_name)
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
            Cls_Business_rptBulkUpload obj = new Cls_Business_rptBulkUpload();
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

            string xls_file_name = file_name;
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
                    String strheader = "Sr. No." + Convert.ToChar(9) +
                                   "Customer No" + Convert.ToChar(9) +
                                   "VC ID" + Convert.ToChar(9) +
                                   "LCO Code" + Convert.ToChar(9) +
                                   "Plan Name" + Convert.ToChar(9) +
                                   "Transaction Type" + Convert.ToChar(9) +
                                   "Upload Id" + Convert.ToChar(9) +
                                   "Date Time" + Convert.ToChar(9) +
                                   "Status" + Convert.ToChar(9) +
                                   "Status Message" + Convert.ToChar(9);
                     while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)+
                                 dt.Rows[i]["cust_no"].ToString() + Convert.ToChar(9) +
                            "'" + dt.Rows[i]["vc_id"].ToString().Trim() + Convert.ToChar(9) +
                            dt.Rows[i]["lco_code"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["plan_name"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["action"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["upload_id"].ToString() + Convert.ToChar(9) +
                            "'" + dt.Rows[i]["upload_date"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["status"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["message"].ToString() + Convert.ToChar(9);
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
            /*StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + xls_file_name + datetime + ".xls");
            try
            {
                int j = 0;
                String strheader = "Sr. No." + Convert.ToChar(9) +
                                   "Customer No" + Convert.ToChar(9) +
                                   "VC ID" + Convert.ToChar(9) +
                                   "LCO Code" + Convert.ToChar(9) +
                                   "Plan Name" + Convert.ToChar(9) +
                                   "Transaction Type" + Convert.ToChar(9) +
                                   "Upload Id" + Convert.ToChar(9) +
                                   "Date Time" + Convert.ToChar(9) +
                                   "Status" + Convert.ToChar(9) +
                                   "Status Message" + Convert.ToChar(9);


                while (j < dt.Rows.Count)
                {
                    sw.WriteLine(strheader);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        j = j + 1;
                        string strrow = j.ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["cust_no"].ToString() + Convert.ToChar(9) +
                            "'" + dt.Rows[i]["vc_id"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["lco_code"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["plan_name"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["action"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["upload_id"].ToString() + Convert.ToChar(9) +
                            "'" + dt.Rows[i]["upload_date"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["status"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["message"].ToString() + Convert.ToChar(9);
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
            Response.Redirect("../MyExcelFile/" + xls_file_name + datetime + ".xls");*/
        }
    }
}