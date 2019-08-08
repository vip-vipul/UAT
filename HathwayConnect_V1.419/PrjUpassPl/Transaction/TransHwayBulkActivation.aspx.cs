using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Transaction;
using PrjUpassPl.Helper;
using System.IO;
using System.Data.OracleClient;
using System.Configuration;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayBulkActivation : System.Web.UI.Page
    {
        string username;
        string upload_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Bulk"] != null)  // Added by Vivek Singh on 27-Jun-2016
            {
                bulkchange.Visible = true;
                bulktransaction.Visible = false;
                Master.PageHeading = "Bulk Scheduler Change Transaction";
                //change
                change.Visible = true;
                ViewState["Bulk"] = "C";
            }
            else
            {
                bulkchange.Visible = false;
                bulktransaction.Visible = true;
                Master.PageHeading = "Bulk Scheduler Transaction";
                //operation
                Operation.Visible = true;
                ViewState["Bulk"] = "B";
            }

            Master.PageHeading = "Bulk Scheduler Transaction";
            if (!IsPostBack)
            {
                if (Session["username"] != null)
                {
                    username = Session["username"].ToString().Trim();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                if (ViewState["upload_id"] != null)
                {
                    upload_id = ViewState["upload_id"].ToString();
                }
                else
                {
                    lnkfileStatus.Visible = false;
                    //btnBeginTrans.Visible = false;

                }

                fillCombo();
                // getBalance();
            }
        }

        public void getBalance()
        {
            //string RequTomor = "";
            //string RequToday = "";
            //string AvalBalance = "";
            //Cls_Business_TransHwayBulkOperation obj = new Cls_Business_TransHwayBulkOperation();
            //obj.GetAvalBalance(Session["lco_username"].ToString(), out AvalBalance);
            //obj.GetRequToday(Session["lco_username"].ToString(), out RequToday);
            //obj.GetRequTomor(Session["lco_username"].ToString(), out RequTomor);
            //lblAvailBalance.Text = AvalBalance;
            //lblRequToday.Text = RequToday;
            //lblRequTomor.Text = RequTomor;
        }

        protected void ddlLco_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["lco_username"] = ddlLco.SelectedValue;
        }

        protected void fillCombo()
        {
            string str = "";

            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
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
                    Session["lco_username"] = ddlLco.SelectedValue;

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
                Response.Write("Error while online payment : " + ex.Message.ToString());
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //clearing
            lblStatusHeading.Text = "";
            lblStatus.Text = "";
            // btnBeginTrans.Visible = false;
            pnlErrData.Visible = false;
            if (ViewState["Bulk"].ToString() == "C")
            {
                change.Visible = true;
            }
            else
            {
                Operation.Visible = true;
            }

            // pnlSummary.Visible = false;
            DataTable temp = new DataTable();
            rptErrData.DataSource = temp;
            rptErrData.DataBind();
            temp.Dispose();
            string file_name = "";

            //check - file selected
            if (!fupData.HasFile)
            {
                lblStatusHeading.Text = "File Upload : Failed";
                lblStatus.Text = "Select non-empty tab deliminated file first and try again";
                return;
            }

            //check - file extension
            if (fupData.PostedFile.ContentType != "text/plain")
            {
                lblStatusHeading.Text = "File Upload : Failed";
                lblStatus.Text = "Only tab deliminated text(.txt) files are allowed";
                return;
            }

            //check - file length
            if (fupData.PostedFile.ContentLength == 0)
            {
                lblStatusHeading.Text = "File Upload : Failed";
                lblStatus.Text = "File does not have contents";
                return;
            }
            if (Session["username"] != null)
            {
                username = ddlLco.SelectedValue;//Session["username"].ToString().Trim();
            }
            //check - directory and save file in diectory
            string directoryPath = string.Format(@"D:/DataUpload/Hathway/{0}", username.Trim());
            //string directoryPath = string.Format(@"E:/DataUpload/Hathway/{0}", username.Trim());
            string filePath = directoryPath + "/" + fupData.PostedFile.FileName;
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                fupData.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                lblStatusHeading.Text = "File Upload : Failed";
                lblStatus.Text = ex.Message.ToString();
                // btnBeginTrans.Visible = false;
                return;
            }

            Cls_Presentation_Helper helper = new Cls_Presentation_Helper();
            DateTime date = DateTime.Now;
            string cur_timestamp = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");

            string cur_time = DateTime.Now.ToString("dd-MMM-yyyy_hh:mm:ss");
            file_name = fupData.PostedFile.FileName;
            Random random = new Random();
            upload_id = username + "_" + cur_time + "_" + random.Next(1000, 9999);
            ViewState["upload_id"] = upload_id;
            string table_columns = "( var_lcopre_bulk_custid, var_lcopre_bulk_vcid, var_lcopre_bulk_lcocode, var_lcopre_bulk_planname,var_lcopre_bulk_action, var_lcopre_bulk_renewflag, VAR_LCOPRE_BULK_DATE , var_lcopre_bulk_insby constant \"" + username + "\", var_lcopre_bulk_useruniqueid constant \"" + upload_id + "\", dat_lcopre_bulk_date constant \"" + cur_timestamp + "\",  var_lcopre_bulk_file constant \"" + file_name + "\",var_two_step_proc_flag constant \"" + "Y" + "\",var_lcopre_bulk_flag,var_lcopre_bulk_new_planname,var_lcopre_bulk_foc_1,var_lcopre_bulk_foc_2,var_lcopre_bulk_foc_3)";

            string uploadResult = helper.cDataUpload("D:\\DataUpload\\Hathway\\" + username + "\\" + fupData.PostedFile.FileName,
                                                   "D:\\DataUpload\\Hathway\\" + username + "\\HathwayCTL.txt",
                                                   "D:\\DataUpload\\Hathway\\" + username + "\\HathwayLOG.log",
                                                   table_columns, "UPASSDB", "UPASS", "cba", "aoup_lcopre_bulk_actvate_raw",
                                                   upload_id);




            if (uploadResult == "0")
            {
                  callprocedure();
            }
            else
            {
                if (uploadResult == "")
                {
                    lblStatus.Text = "Error While Uploading...";
                }
                else
                {
                    lblStatus.Text = uploadResult.ToString();
                }
            }
        }

        private void callprocedure()
        {
            Cls_Business_TransHwayBulkOperation obj = new Cls_Business_TransHwayBulkOperation();
            string pro_output = obj.bulkUploadActTemp(username, upload_id);
            if (pro_output.Split('#')[0] == "9999")
            {
                lblStatusHeading.Text = "File Upload : Successful";
                lblStatus.Text = "";
                //btnBeginTrans.Visible = true;
             //   lnkfileStatus.Visible = true;
                //    Response.Redirect("../Reports/rptBulkActTransactionProc.aspx?uniqueid=" + upload_id);
            }
            else
            {
                lblStatusHeading.Text = "File Upload : Failed";
                string raw_message = pro_output.Split('#')[1];
                lnkfileStatus.Visible = false;
                string message = "";
                if (raw_message.Contains('~'))
                {
                    string[] err_data = raw_message.TrimEnd('~').Split('~');
                    DataTable dtErrData = generateDataTable(err_data);
                    rptErrData.DataSource = dtErrData;
                    rptErrData.DataBind();
                    pnlErrData.Visible = true;
                    if (ViewState["Bulk"].ToString() == "C")
                    {
                        change.Visible = false;
                    }
                    else
                    {
                        Operation.Visible = false;
                    }
                }
                else
                {
                    message = raw_message.Replace("$", "").Replace("-", "");
                }

                lblStatus.Text = message;
            }
        }

        public void msgbox(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
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

        protected void lnkfileStatus_Click(object sender, EventArgs e)
        {
            // Response.Redirect("rptBulkTransactionProc.aspx?uniqueid='" + unique_id + "'");
            //   Response.Redirect("../Reports/rptBulkActTransactionProc.aspx?uniqueid=" + ViewState["upload_id"]);

        }




    }
}