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
using System.Data.OracleClient;
using System.Configuration;

namespace PrjUpassPl.Transaction
{
    public partial class ankit : System.Web.UI.Page
    {
        string username;
        string upload_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Bulk"] != null)  // Added by Vivek Singh on 27-Jun-2016
            {
                bulkchange.Visible = true;
                bulktransaction.Visible = false;
                Master.PageHeading = "Bulk Change Transaction";
                //change
                ViewState["Bulk"] = "C";
            }
            else
            {
                bulkchange.Visible = false;
                bulktransaction.Visible = true;
                Master.PageHeading = "Bulk Transaction";
                //operation
                Operation.Visible = true;
                ViewState["Bulk"] = "B";
            }
            Session["RightsKey"] = "N";
            Master.PageHeading = "Bulk Transaction";
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

            }
        }

        
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //clearing
            lblStatusHeading.Text = "";
            lblStatus.Text = "";
            //temp.Dispose();
            string file_name = "";
            username = Session["username"].ToString().Trim();
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
            string table_columns = "(bhubhag_id, bhubhag_name, is_deleted, deleted_on,is_updated, updated_on, updated_by)";

            string uploadResult = helper.cDataUpload("D:\\DataUpload\\Hathway\\" + username + "\\" + fupData.PostedFile.FileName,
                                                   "D:\\DataUpload\\Hathway\\" + username + "\\HathwayCTL.txt",
                                                   "D:\\DataUpload\\Hathway\\" + username + "\\HathwayLOG.log",
                                                  // table_columns, "upassdb", "upass", "cba", "aoup_lcopre_lcoshare_plan",
                                                  table_columns, "ASBSDB", "bhiwandi", "bhiwandi", "bhubhag",
                                                   upload_id);

            if (uploadResult == "0")
            {
                callprocedureMst();
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
        private void callprocedureMst()
        {
            Cls_Business_TransBulk_Discount obj = new Cls_Business_TransBulk_Discount();
            string pro_output = obj.bulkUploadMst(username);
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



    }
}