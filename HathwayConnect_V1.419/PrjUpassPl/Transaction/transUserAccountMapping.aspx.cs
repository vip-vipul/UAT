using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassPl.Helper;
using System.IO;
using PrjUpassBLL.Transaction;
using PrjUpassBLL.Reports;

namespace PrjUpassPl.Transaction
{
    public partial class transUserAccountMapping : System.Web.UI.Page
    {
        string username;
        string upload_id;
        string operator_id = "";
        string category_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "User Account Mapping";
            if (!IsPostBack)
            {
                if (Session["username"] != null)
                {
                    operator_id = Convert.ToString(Session["operator_id"]);
                    category_id = Convert.ToString(Session["category"]);
                    username = Session["username"].ToString().Trim();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                Session["RightsKey"] = "N";
                BindLCO();
            }
        }
        protected void rblMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblMode.SelectedValue == "0")
            {
                tr3.Visible = false;
                tr1.Visible = true;
                tr2.Visible = true;
                tr4.Visible = true;
                td1.Visible = false;
                lblStatus.Text = "";
                lblStatusHeading.Text = "";
                Operation.Visible = false;
                btnUpload.Text = "Submit";
            }
            else if (rblMode.SelectedValue == "1")
            {
                tr3.Visible = true;
                tr1.Visible = false;
                tr2.Visible = false;
                tr4.Visible = false;
                td1.Visible = true;
                Operation.Visible = true;
                lblStatus.Text = "";
                lblStatusHeading.Text = "";
                btnUpload.Text = "Upload";
            }
        }

        public void BindLCO()
        {

            cls_Business_rptLcowiseUserdetails objDetails = new cls_Business_rptLcowiseUserdetails();
            DataTable dt = objDetails.getLcoSub(Session["username"].ToString().Trim(), category_id, operator_id);
            ddlLco.DataSource = dt;
            ddlLco.DataTextField = "username";
            ddlLco.DataValueField = "username";
            ddlLco.DataBind();
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (rblMode.SelectedValue == "0")
            {
                if (txtAccountNo.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("N", txtAccountNo.Text);

                    if (valid == "")
                    {
                        String IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (IPAddress == null)
                        {
                            IPAddress = Request.ServerVariables["REMOTE_ADDR"];
                        }
                        string IPAdd = Convert.ToString(IPAddress);
                        Cls_Business_UserAccountMap obj = new Cls_Business_UserAccountMap();

                        string pro_output = obj.UserAccountMap(Session["username"].ToString().Trim(), IPAdd, ddlLco.SelectedValue, txtAccountNo.Text, rblAction.SelectedValue.ToString(), "S");
                        if (pro_output.Split('#')[0] == "9999")
                        {
                            lblStatusHeading.Text = "Successful";
                            lblStatus.Text = "";
                        }
                        else
                        {
                            lblStatusHeading.Text = pro_output.Split('#')[1].ToString();// "File Upload : Failed";
                        }
                        txtAccountNo.Text = "";
                        ddlLco.SelectedIndex = 0;
                        rblAction.SelectedValue = "Y";
                    }
                    else
                    {
                        lblStatusHeading.Text = valid.ToString();   
                        return;
                    }
                }
            }
            else
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

                if (fupData.PostedFile.ContentLength == 0)
                {
                    lblStatusHeading.Text = "File Upload : Failed";
                    lblStatus.Text = "File does not have contents";
                    return;
                }

                //check - directory and save file in diectory
                string directoryPath = string.Format(@"D:/DataUpload/HathwayBulk/{0}", Session["username"].ToString().Trim());
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

                string table_columns = "( var_user_acc_username, var_user_acc_accountno, Var_user_acc_activeflag)";

                string uploadResult = helper.cDataUpload("D:\\DataUpload\\HathwayBulk\\" + Session["username"].ToString().Trim() + "\\" + fupData.PostedFile.FileName,
                                                         "D:\\DataUpload\\HathwayBulk\\" + Session["username"].ToString().Trim() + "\\HathwayUserAccMappingCTL.txt",
                                                         "D:\\DataUpload\\HathwayBulk\\" + Session["username"].ToString().Trim() + "\\HathwayUserAccMappingLOG.log",
                                                         table_columns, "Upassdb", "UPASS", "cba", "AOUP_LCOPRE_USER_ACC_MAP_raw", ""
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
                        lblStatus.Visible = true;
                    }
                    else
                    {
                        lblStatus.Text = uploadResult.ToString();
                        lblStatus.Visible = true;
                    }
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
            string IPAdd = Convert.ToString(IPAddress);
            Cls_Business_UserAccountMap obj = new Cls_Business_UserAccountMap();


            string pro_output = obj.UserAccountMap(Session["username"].ToString().Trim(), IPAdd, "", "", "", "B");
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