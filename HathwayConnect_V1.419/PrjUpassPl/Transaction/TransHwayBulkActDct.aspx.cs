using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassPl.Helper;
using System.IO;
using System.Data;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayBulkActDct : System.Web.UI.Page
    {
        string username;
        string upload_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Bulk ACT and DEACT";
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
            else
            {
                if (!SecurityValidation.SizeUploadValidation(fupData))
                {
                    lblStatusHeading.Text = "File Upload : Failed";
                    lblStatus.Text = "Please upload file less than 5MB";
                    return;
                }
            }

           // check - file extension
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
            string directoryPath = string.Format(@"D:/DataUpload/HathwayBulk/{0}", username.Trim());
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
            Random random = new Random();
            upload_id = username + "_" + cur_time + "_" + random.Next(1000, 9999);
            ViewState["upload_id"] = upload_id;

            string table_columns = "( VAR_ACTDCT_ACCNO, VAR_ACTDCT_VCID, VAR_ACTDCT_REASON, VAR_ACTDCT_LCOCODE,var_actdct_action,VAR_ACTDCT_FILENAME constant \"" + file_name + "\",VAR_ACTDCT_FILEUNIQUEID constant \"" + upload_id + "\")";

            string uploadResult = helper.cDataUpload("D:\\DataUpload\\HathwayBulk\\" + username + "\\" + fupData.PostedFile.FileName,
                                                     "D:\\DataUpload\\HathwayBulk\\" + username + "\\HathwayBulkActDctCTL.txt",
                                                     "D:\\DataUpload\\HathwayBulk\\" + username + "\\HathwayBulkActDctLOG.log",
                                                     table_columns, "Upassdb", "UPASS", "cba", "aoup_lcopre_Bulk_act_dct_Raw", ""
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
            String IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (IPAddress == null)
            {
                IPAddress = Request.ServerVariables["REMOTE_ADDR"];

            }
           string IPAdd =Convert.ToString( IPAddress);
            Cls_Business_Bulk_ActDct obj = new Cls_Business_Bulk_ActDct();


            string pro_output = obj.bulkUploadTemp(username, IPAdd, ViewState["upload_id"].ToString());
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
        private void callprocedureMst()
        {
            Cls_Business_Bulk_ActDct obj = new Cls_Business_Bulk_ActDct();
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

        public void msgbox(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
        }
    }
}