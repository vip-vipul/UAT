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
using System.Configuration;
using System.Data.OracleClient;
using PrjUpassDAL.Helper;
using PrjUpassBLL.Master;
namespace PrjUpassPl.Transaction
{
    public partial class TransHwayBulkCustumerAddEcaf : System.Web.UI.Page
    {
        string username;
        string Unique_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Bulk Ecaf";
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

            if (!IsPostBack)
            {
                GetUserCity();
                GetDetails();
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (ddlpin.SelectedItem.ToString() == "-- Select Zip Code --")
            {
                msgbox("Please Select Zip Code");
                return;
            }
            if (ddlstreet.SelectedItem.ToString() == "-- Select Street --")
            {
                msgbox("Please Select Street");
                return;
            }
            if (ddllocation.SelectedItem.ToString() == "-- Select Location --")
            {
                msgbox("Please Select Location");
                return;
            }

            string pincodecode = "";
            string areacode = "";
            string streetcode = "";
            string locationcode = "";
            string buildingcode = "";

            string pincode = "";
            string area = "";
            string street = "";
            string location = "";
            string building = "";

            pincodecode = ddlpin.SelectedValue.ToString();
            areacode = ddlarea.SelectedValue.ToString();
            streetcode = ddlstreet.SelectedValue.ToString();
            locationcode = ddllocation.SelectedValue.ToString();
            buildingcode = ddlbuilding.SelectedValue.ToString();


            pincode = ddlpin.SelectedItem.ToString();
            area = ddlarea.SelectedItem.ToString();
            street = ddlstreet.SelectedItem.ToString();
            location = ddllocation.SelectedItem.ToString();
            building = ddlbuilding.SelectedItem.ToString();

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
            //string directoryPath = string.Format(@"D:/DataUpload/HathwayAutoRenewal/{0}", username.Trim());
            //string filePath = directoryPath + "/" + fupData.PostedFile.FileName;
            //try
            //{
            //    if (!Directory.Exists(directoryPath))
            //    {
            //        Directory.CreateDirectory(directoryPath);
            //    }
            //    fupData.SaveAs(filePath);
            //}
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

                return;
            }

            Cls_Presentation_Helper helper = new Cls_Presentation_Helper();
            DateTime date = DateTime.Now;
            string cur_timestamp = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
            string cur_time = DateTime.Now.ToString("dd-MMM-yyyy_hh:mm:ss");
            file_name = fupData.PostedFile.FileName;

            Random random = new Random();
            Unique_id = username + "_" + cur_time + "_" + random.Next(1000, 9999);
            ViewState["upload_id"] = Unique_id;


            string table_columns = "( VAR_CUST_STB, VAR_CUST_VC,VAR_CUST_APPLICANTTITLE,VAR_CUST_APPLICANTFIRSTNM,VAR_CUST_APPLICANTMEDDILNM,VAR_CUST_APPLICANTLASTNM,";
            table_columns += " VAR_CUST_MOBILENO,VAR_CUST_LANDLINE,VAR_CUST_EMAIL,VAR_CUST_FLATNO,VAR_CUST_ADDRESS1,VAR_CUST_CITY,var_cust_planname,VAR_CUST_ZIPCODE constant \"" + pincodecode + "\",VAR_CUST_AREA constant \"" + area + "\",";
            table_columns += " VAR_CUST_AREACODE constant \"" + areacode + "\",VAR_CUST_STREET constant \"" + street + "\",VAR_CUST_LOCATION constant \"" + location + "\",VAR_CUST_BUILDING constant \"" + building + "\",VAR_CUST_UNIQUNO constant \"" + Unique_id + "\",VAR_CUST_LCOCODE constant \"" + username + "\",var_cust_state constant \"" + ViewState["State"] + "\")";

            string uploadResult = helper.cDataUpload("D:\\DataUpload\\Hathway\\" + username + "\\" + fupData.PostedFile.FileName,
                                                     "D:\\DataUpload\\Hathway\\" + username + "\\HathwayBulkEcafCTL.txt",
                                                     "D:\\DataUpload\\Hathway\\" + username + "\\HathwayBulkEcafLOG.log",
                                                     table_columns, "UPASSDB", "UPASS", "cba", "aoup_crf_cust_bulk_Raw", ""
                                                     );

            if (uploadResult == "0")
            {
                callprocedureTemp();

                Response.Redirect("../Reports/frmMeddleBulkTransaction.aspx?uniqueid=" + Unique_id.Trim());
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
            Cls_BLL_Bulk_Cust_Ecaf obj = new Cls_BLL_Bulk_Cust_Ecaf();

            string pro_output = obj.Bulk_Cust_Ecaf_inst(username, ViewState["upload_id"].ToString() );
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

        public void GetUserCity()
        {
            string strConn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(strConn);
            String str = "";

            str += " select n.var_city_name,a.var_lcomst_dasarea,a.num_lcomst_cityid from aoup_lcopre_lco_det a,aoup_lcopre_city_def n  where a.var_lcomst_code='" + Session["username"].ToString().Trim() + "'";
            str += " and a.num_lcomst_cityid=n.num_city_id";
            conn.Open();
            OracleCommand cmd2 = new OracleCommand(str, conn);
            OracleDataReader dr4 = cmd2.ExecuteReader();

            while (dr4.Read())
            {
                Session["city"] = dr4["var_city_name"].ToString();
                Session["cityID"] = dr4["num_lcomst_cityid"].ToString();
                Session["DASAREA"] = dr4["var_lcomst_dasarea"].ToString();
            }

            dr4.Close();
            conn.Close();
        }

        public void GetDetails()
        {
            String Response = FillAddDeatil("P", Session["city"].ToString());
            ddlpin.Items.Clear();
            DataTable Tblpincode = new DataTable();
            Tblpincode.Columns.Add("ID");
            Tblpincode.Columns.Add("Name");
            ViewState["State"] = Response.Split('*')[1].ToString();
            string[] Responsearr = Response.Split('*')[0].Split('$');

            foreach (string str in Responsearr)
            {
                if (str != "")
                {
                    String[] strarr = str.Split('~');
                    Tblpincode.Rows.Add(strarr[1], strarr[0]);
                }
            }
            ddlpin.Items.Clear();
            if (Tblpincode.Rows.Count > 0)
            {
                ListItem lst = new ListItem();
                ddlpin.DataTextField = "ID";
                ddlpin.DataValueField = "Name";

                ddlpin.DataSource = Tblpincode;
                ddlpin.DataBind();
                ddlpin.Items.Insert(0, new ListItem("-- Select Zip Code --", ""));
            }
        }

        public String FillAddDeatil(String selectionfalg, String Entity)
        {
            Cls_BLL_mstCrf objdet = new Cls_BLL_mstCrf();
            string response = objdet.GetAdddeatils(Session["username"].ToString(), selectionfalg, Entity);
            return response;
        }

        protected void ddlpin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpin.SelectedValue.Trim() != "")
            {
                String Response = FillAddDeatil("A", ddlpin.SelectedValue.Trim());

                DataTable TblArea = new DataTable();
                TblArea.Columns.Add("ID");
                TblArea.Columns.Add("Name");
                string[] Responsearr = Response.Split('*')[0].Split('$');

                foreach (string str in Responsearr)
                {
                    if (str != "")
                    {
                        String[] strarr = str.Split('~');
                        TblArea.Rows.Add(strarr[1], strarr[0]);
                    }
                }
                ddlarea.Items.Clear();
                if (TblArea.Rows.Count > 0)
                {
                    ListItem lst = new ListItem();
                    ddlarea.DataTextField = "Name";
                    ddlarea.DataValueField = "ID";

                    ddlarea.DataSource = TblArea;
                    ddlarea.DataBind();

                }
                ddlstreet.Items.Clear();
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();

                ddlarea.Items.Insert(0, new ListItem("-- Select Area --", ""));
                ddlstreet.Items.Insert(0, new ListItem("-- Select Street --", ""));
            }
            else
            {
                ddlarea.Items.Clear();
                ddlstreet.Items.Clear();
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();
            }
        }

        protected void ddlarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlarea.SelectedValue.Trim() != "")
            {
                String Response = FillAddDeatil("S", ddlarea.SelectedValue.Trim());

                DataTable TblStreet = new DataTable();
                TblStreet.Columns.Add("ID");
                TblStreet.Columns.Add("Name");
                string[] Responsearr = Response.Split('*')[0].Split('$');

                foreach (string str in Responsearr)
                {
                    if (str != "")
                    {
                        String[] strarr = str.Split('~');
                        TblStreet.Rows.Add(strarr[1], strarr[0]);
                    }
                }
                ddlstreet.Items.Clear();
                if (TblStreet.Rows.Count > 0)
                {
                    ddlstreet.DataTextField = "ID";
                    ddlstreet.DataValueField = "Name";

                    ddlstreet.DataSource = TblStreet;
                    ddlstreet.DataBind();
                    ddlstreet.Items.Insert(0, new ListItem("-- Select Street --", ""));
                    ddlstreet_SelectedIndexChanged(null, null);
                }
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();
            }
            else
            {
                ddlstreet.Items.Clear();
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();
            }
        }

        protected void ddlstreet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlstreet.SelectedValue.Trim() != "")
            {
                String Response = FillAddDeatil("L", ddlstreet.SelectedValue.Trim());

                DataTable TblLocation = new DataTable();
                TblLocation.Columns.Add("ID");
                TblLocation.Columns.Add("Name");
                string[] Responsearr = Response.Split('*')[0].Split('$');

                foreach (string str in Responsearr)
                {
                    if (str != "")
                    {
                        String[] strarr = str.Split('~');
                        TblLocation.Rows.Add(strarr[1], strarr[0]);
                    }
                }
                ddllocation.Items.Clear();
                if (TblLocation.Rows.Count > 0)
                {
                    ddllocation.DataTextField = "ID";
                    ddllocation.DataValueField = "Name";

                    ddllocation.DataSource = TblLocation;
                    ddllocation.DataBind();
                    ddllocation.Items.Insert(0, new ListItem("-- Select Location --", ""));
                    ddllocation_SelectedIndexChanged(null, null);
                }
                ddlbuilding.Items.Clear();
            }
            else
            {
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();
            }
        }

        protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddllocation.SelectedValue.Trim() != "")
            {
                String Response = FillAddDeatil("B", ddllocation.SelectedValue.Trim());

                DataTable TblBulding = new DataTable();
                TblBulding.Columns.Add("ID");
                TblBulding.Columns.Add("Name");
                string[] Responsearr = Response.Split('*')[0].Split('$');

                foreach (string str in Responsearr)
                {
                    if (str != "")
                    {
                        String[] strarr = str.Split('~');
                        TblBulding.Rows.Add(strarr[1], strarr[0]);
                    }
                }
                ddlbuilding.Items.Clear();
                if (TblBulding.Rows.Count > 0)
                {
                    ddlbuilding.DataTextField = "ID";
                    ddlbuilding.DataValueField = "Name";

                    ddlbuilding.DataSource = TblBulding;
                    ddlbuilding.DataBind();
                    ddlbuilding.Items.Insert(0, new ListItem("-- Select Bulding --", ""));

                }

            }
            else
            {
                ddlbuilding.Items.Clear();
            }
        }
    }
}