using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PrjUpassPl.Helper;
using PrjUpassBLL.Transaction;
using System.Text;
using System.Net;
using System.Data;
using PrjUpassDAL.Transaction;
using PrjUpassPl.BulkUploadService;
using System.Collections;
using PrjUpassDAL.Authentication;
using PrjUpassBLL.Reports;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayBulkAutoRenewal : System.Web.UI.Page
    {
        string username;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Bulk Auto Renewal";
            if (Session["username"] != null)
            {
                username = Session["username"].ToString().Trim();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            Session["RightsKey"] = "N";

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //clearing
            lblStatusHeading.Text = "";
            lblStatus.Text = "";

            DataTable temp = new DataTable();

            temp.Dispose();
            string file_name = "";


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
            else
            {
                if (!SecurityValidation.SizeUploadValidation(fupData))
                {
                    lblStatusHeading.Text = "File Upload : Failed";
                    lblStatus.Text = "Please upload file less than 5MB";
                    return;
                }
            }

            //check - file length
            if (fupData.PostedFile.ContentLength == 0)
            {
                lblStatusHeading.Text = "File Upload : Failed";
                lblStatus.Text = "File does not have contents";
                return;
            }

            //check - directory and save file in diectory
            string directoryPath = string.Format(@"D:/DataUpload/HathwayAutoRenewal/{0}", username.Trim());
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

                return;
            }

            Cls_Presentation_Helper helper = new Cls_Presentation_Helper();
            DateTime date = DateTime.Now;
            string cur_timestamp = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
            string cur_time = DateTime.Now.ToString("dd-MMM-yyyy_hh:mm:ss");
            file_name = fupData.PostedFile.FileName;



            string table_columns = "( var_lcopre_ecs_accno, var_lcopre_ecs_vcid,var_lcopre_ecs_plan,var_lcopre_ecs_lcocode,var_lcopre_ecs_flag,DAT_ECS_VALIDUPTO)";

            string uploadResult = helper.cDataUpload("D:\\DataUpload\\HathwayAutoRenewal\\" + username + "\\" + fupData.PostedFile.FileName,
                                                     "D:\\DataUpload\\HathwayAutoRenewal\\" + username + "\\HathwayAutoRenewalCTL.txt",
                                                     "D:\\DataUpload\\HathwayAutoRenewal\\" + username + "\\HathwayAutoRenewalLOG.log",
                                                     table_columns, "upassdb", "upass", "cba", "aoup_lcopre_ecs_raw", ""
                                                     );

            if (uploadResult == "0")
            {
                callprocedureTemp();
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

        private void callprocedureTemp()
        {
            Cls_Business_TransBulk_AutoRenewal obj = new Cls_Business_TransBulk_AutoRenewal();

            string pro_output = obj.bulkAutoRenewalTemp(username);
            if (pro_output.Split('#')[0] == "9999")
            {
                callprocedureMst();

            }
            else
            {
                lblStatusHeading.Text = pro_output.Split('#')[1].ToString();// "File Upload : Failed";
            }
        }

        private void callprocedureMst()
        {
            Cls_Business_TransBulk_AutoRenewal obj = new Cls_Business_TransBulk_AutoRenewal();
            string pro_output = obj.bulkAutoRenewalMst(username);
            if (pro_output.Split('#')[0] == "9999")
            {
                lblStatusHeading.Text = "File Upload : Successful";
                lblStatus.Text = "";
            }
            else
            {
                lblStatusHeading.Text = pro_output.Split('#')[1].ToString();// "File Upload : Failed";
            }
        }

        public void msgbox(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
        }
    }
}