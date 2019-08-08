using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PrjUpassBLL.Transaction;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using PrjUpassDAL.Helper;
using System.Data.OleDb;
using System.Globalization;
using PrjUpassPl.Helper;

namespace PrjUpassPl.Transaction
{
    public partial class TransOsdBmailNotification : System.Web.UI.Page
    {
        string oper_id = "";
        string username = "";
        string catid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["RightsKey"] = "N";
            if (Request.QueryString["flag"] != null && Request.QueryString["flag"] != "")
            {
                ViewState["Flag"] = Request.QueryString["flag"].ToString();
                Master.PageHeading = "SMS Notification";
                RadIncExc.Items.FindByText("All").Attributes.Add("style", "display:none");
               // RadIncExc.Items.FindByText("One By One").Attributes.Add("style", "display:none");
            }
            else
            {
                ViewState["Flag"] = "OSD";
                Master.PageHeading = "BMAIL Notification";
                RadIncExc.Items.FindByText("All").Attributes.Add("style", "display:block");
               // RadIncExc.Items.FindByText("One By One").Attributes.Add("style", "display:block");
            }
            if (!IsPostBack)
            {
                fillCombo();
                AddDataInGrid();
                visibleControl();
               
                Trlco.Visible = true;
            }


        }
        private void visibleControl()
        {
            txtFrom.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtTo.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            string flag = ViewState["Flag"].ToString().ToUpper();

           
            if (flag == "SMS")
            {
                Tr5.Visible = false;
                Tr6.Visible = false;
                TrNotification.Visible = false;
                TrFrequency.Visible = false;
                TrToDate.Visible = false;
                TrFromDate.Visible = false;
              //  Trlco.Visible = true;


            }
            else
            {

                TrToDate.Visible = true;
                TrFromDate.Visible = true;
                TrNotification.Visible = true;
                TrFrequency.Visible = true;

               // Trlco.Visible = false;

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searhParam = txtSearchParam.Text;
            string search_type = rdoSearchParamType.SelectedValue.ToString();
            string oper_id = "";
            string user_brmpoid = "";
            string category = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["category"] != null && Session["user_brmpoid"] != null)
            {
                username = Convert.ToString(Session["username"]);
                oper_id = ddlLco.SelectedValue;//Convert.ToString(Session["operator_id"]);
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
                category = Convert.ToString(Session["category"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            string response_params = user_brmpoid + "$" + searhParam + "$SW";
            if (search_type == "0")
            {
                //if VC ID
                response_params += "$V";
            }

            string apiResponse = callAPI(response_params, "12");
            // string apiResponse = "083248297$$$VIRENDRA CABIN$10$0$0.0.0.1 /account 2444513730 25$0.0.0.1 /account 2444513730 25$*$*$*$*$*$0.0.0.1 /account 35057619 4$1083248297$0.0.0.1 /service/catv 2444560382 109!5949035292573986!	00:1E:6B:58:4B:96!0.0.0.1 /plan 6605694 0|2015-05-20T00:00:00Z|2016-05-20T00:00:00Z|0.0.0.1 /deal 6603390 2|51755601|0.0.0.1 /purchased_product 2443703322 18|1~0.0.0.1 /plan 1630152209 0|2015-11-13T00:00:00Z|2016-01-13T00:00:00Z|0.0.0.1 /deal 1630152545 0|75567960|0.0.0.1 /purchased_product 3533988819 8|1~0.0.0.1 /plan 2637459089 2|2015-12-09T00:00:00Z|2016-01-09T00:00:00Z|0.0.0.1 /deal 2637458785 2|76314930|0.0.0.1 /purchased_product 3633878545 4|1~0.0.0.1 /plan 1976357466 0|2015-11-04T00:00:00Z|2016-01-04T00:00:00Z|0.0.0.1 /deal 1976360330 0|75338697|0.0.0.1 /purchased_product 3499937641 8|1~0.0.0.1 /plan 2637458385 0|2015-11-07T00:00:00Z|2015-12-09T19:36:12Z|0.0.0.1 /deal 2637457553 2|75438125|0.0.0.1 /purchased_product 3504290442 9|3~0.0.0.1 /plan 1761922741 0|2015-05-20T00:00:00Z|2015-12-09T19:34:42Z|0.0.0.1 /deal 1761922229 0|51755429|0.0.0.1 /purchased_product 2444559105 22|3~0.0.0.1 /plan 1976358042 0|2015-11-04T00:00:00Z|2015-11-04T19:26:18Z|0.0.0.1 /deal 1976359178 0|75338562|0.0.0.1 /purchased_product 3499784957 7|3~0.0.0.1 /plan 1976360218 0|2015-11-04T00:00:00Z|2015-11-04T19:20:18Z|0.0.0.1 /deal 1976357898 0|75328648|0.0.0.1 /purchased_product 3499413473 7|3~0.0.0.1 /plan 1630154257 0|2015-11-04T00:00:00Z|2015-11-04T19:18:40Z|0.0.0.1 /deal 1630154593 0|75338200|0.0.0.1 /purchased_product 3499763102 7|3~0.0.0.1 /plan 1630152209 0|2015-11-04T00:00:00Z|2015-11-04T19:03:06Z|0.0.0.1 /deal 1630152545 0|75337783|0.0.0.1 /purchased_product 3499836356 7|3~0.0.0.1 /plan 1630154257 0|2015-11-04T00:00:00Z|2015-11-04T18:55:54Z|0.0.0.1 /deal 1630154593 0|75337997|0.0.0.1 /purchased_product 3499803652 7|3~0.0.0.1 /plan 1630154257 0|2015-11-04T00:00:00Z|2015-11-04T18:51:09Z|0.0.0.1 /deal 1630154593 0|75337876|0.0.0.1 /purchased_product 3499940350 7|3~0.0.0.1 /plan 1630154257 0|2015-11-04T00:00:00Z|2015-11-04T18:42:54Z|0.0.0.1 /deal 1630154593 0|75332694|0.0.0.1 /purchased_product 3499587537 7|3~0.0.0.1 /plan 1630154257 0|2015-11-04T00:00:00Z|2015-11-04T18:17:06Z|0.0.0.1 /deal 1630154593 0|75330971|0.0.0.1 /purchased_product 3499680435 7|3~0.0.0.1 /plan 1630154257 0|2015-11-04T00:00:00Z|2015-11-04T17:16:06Z|0.0.0.1 /deal 1630154593 0|75330456|0.0.0.1 /purchased_product 3499630372 7|3~0.0.0.1 /plan 1630154257 0|2015-11-04T00:00:00Z|2015-11-04T16:53:46Z|0.0.0.1 /deal 1630154593 0|75328807|0.0.0.1 /purchased_product 3499478939 7|3~0.0.0.1 /plan 1630154257 0|2015-11-04T00:00:00Z|2015-11-04T15:48:25Z|0.0.0.1 /deal 1630154593 0|75328668|0.0.0.1 /purchased_product 3499373617 7|3~0.0.0.1 /plan 2522383467 0|2015-11-04T00:00:00Z|2015-11-04T15:43:38Z|0.0.0.1 /deal 2522383275 0|75328616|0.0.0.1 /purchased_product 3499547059 7|3~0.0.0.1 /plan 2522383467 0|2015-11-04T00:00:00Z|2015-11-04T15:42:09Z|0.0.0.1 /deal 2522383275 0|75328578|0.0.0.1 /purchased_product 3499430936 7|3~0.0.0.1 /plan 1976375541 0|2015-08-28T00:00:00Z|2015-09-28T00:00:00Z|0.0.0.1 /deal 1976375125 0|71724329|0.0.0.1 /purchased_product 2889793663 8|3~0.0.0.1 /plan 1976374133 0|2015-08-28T00:00:00Z|2015-09-28T00:00:00Z|0.0.0.1 /deal 1976373589 0|71711079|0.0.0.1 /purchased_product 2889704376 8|3~0.0.0.1 /plan 1976374517 0|2015-08-28T00:00:00Z|2015-09-28T00:00:00Z|0.0.0.1 /deal 1976377173 0|71495032|0.0.0.1 /purchased_product 2885810499 8|3~0.0.0.1 /plan 411108344 0|2015-05-20T00:00:00Z|2015-08-20T00:00:00Z|0.0.0.1 /deal 411107832 0|51755617|0.0.0.1 /purchased_product 2444535214 12|3~0.0.0.1 /plan 411109368 0|2015-05-20T00:00:00Z|2015-08-20T00:00:00Z|0.0.0.1 /deal 411108856 0|51755617|0.0.0.1 /purchased_product 2444535406 12|3~0.0.0.1 /plan 1738959814 0|2015-05-20T00:00:00Z|2015-06-20T00:00:00Z|0.0.0.1 /deal 1738959174 2|51755640|0.0.0.1 /purchased_product 2444644762 8|3~0.0.0.1 /plan 1738958790 0|2015-05-20T00:00:00Z|2015-06-20T00:00:00Z|0.0.0.1 /deal 1738958150 2|51755640|0.0.0.1 /purchased_product 2444647514 8|3~0.0.0.1 /plan 1738960838 0|2015-05-20T00:00:00Z|2015-06-20T00:00:00Z|0.0.0.1 /deal 1738960198 2|51755640|0.0.0.1 /purchased_product 2444645722 8|3~0.0.0.1 /plan 1738956838 0|2015-05-20T00:00:00Z|2015-06-20T00:00:00Z|0.0.0.1 /deal 1738957638 2|51755640|0.0.0.1 /purchased_product 2444646618 8|3!10100!HD!0";
            try
            {
                if (apiResponse != "")
                {
                    List<string> lstResponse = new List<string>();
                    lstResponse = apiResponse.Split('$').ToList();
                    string cust_id = lstResponse[0]; //account_no
                    ViewState["customer_no"] = cust_id;
                    string cust_name = lstResponse[3];
                    ViewState["customer_name"] = cust_name;
                    string cust_addr = lstResponse[4];
                    ViewState["accountPoid"] = lstResponse[6];
                    string lco_poid = lstResponse[13];

                    //DATA ACCESS VALIDATION----------------------------------------------------------------------------------VALIDATION
                    if (category != "10") //No validation for national level user
                    {
                        Cls_Validation obj = new Cls_Validation();
                        //bool validate_cust_access = obj.CustDataAccess(username, oper_id, lco_poid); rimesh
                        //if (validate_cust_access == false)
                        //{                           
                        //    pnlCustDetails.Visible = false;                            
                        //    return;
                        //}
                    }
                    else
                    {   //if user is not a LCO, then get city using brmpoid 
                        Cls_Business_TxnAssignPlan objCity = new Cls_Business_TxnAssignPlan();
                        string city = objCity.GetCityFromBrmPoid(Session["username"].ToString(), lco_poid);
                        ViewState["cityid"] = city;
                    }


                    string cust_services = lstResponse[15];
                    string[] service_arr = cust_services.Split('^');
                    DataTable dtStbs = new DataTable();
                    dtStbs.Columns.Add(new DataColumn("STB_NO"));
                    dtStbs.Columns.Add(new DataColumn("VC_ID"));
                    dtStbs.Columns.Add(new DataColumn("SERVICE_STRING"));
                    dtStbs.Columns.Add(new DataColumn("Status"));

                    DataTable sortedDT = new DataTable();

                    foreach (string service in service_arr)
                    {
                        string stb_no = service.Split('!')[1];
                        string vc_id = service.Split('!')[2];
                        string stb_status = service.Split('!')[4];
                        if (stb_status == "10103")
                        {
                            continue; //if status is terminated
                        }
                        if (stb_no == "" || vc_id == "")
                        {
                            continue;
                        }
                        DataRow drStbRow = dtStbs.NewRow();
                        drStbRow["STB_NO"] = stb_no;
                        drStbRow["VC_ID"] = vc_id;
                        drStbRow["SERVICE_STRING"] = service;
                        drStbRow["Status"] = stb_status;
                        dtStbs.Rows.Add(drStbRow);

                        DataView dv = dtStbs.DefaultView;
                        dv.Sort = "Status Asc";
                        sortedDT = dv.ToTable();
                    }
                    if (sortedDT.Rows.Count == 0)
                    {
                        return;
                    }
                    grdStb.DataSource = sortedDT;
                    grdStb.DataBind();

                    //ticking first stb value and bind all fields with details
                    ((CheckBox)(grdStb.Rows[0].FindControl("chkStb"))).Checked = true;
                    HiddenField hdnDefaultService = ((HiddenField)(grdStb.Rows[0].FindControl("hdnServiceStr")));

                    lblAcno.Text = cust_id;
                    lblCustName.Text = cust_name;
                    tblCustDetails.Visible = true;
                    btnCancel.Visible = true;
                    btnAdd.Visible = true;
                }
                else
                {

                }

            }
            catch (Exception)
            {
                tblCustDetails.Visible = false;
            }
        }

        public String callAPI(string Request, string request_code)
        {
            try
            {

                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
               HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                 //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21//TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                 //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost//TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/testhwayobrmcallservice/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                myRequest.Timeout = 90000;
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                using (HttpWebResponse responseFromSender = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (StreamReader responseReader = new StreamReader(responseFromSender.GetResponseStream()))
                    {
                        fromSender = responseReader.ReadToEnd();
                    }
                }
                String Res = fromSender.Split('%')[0];
                return Res;
            }
            catch (Exception ex)
            {
                //FileLogText("Admin", "callAPI", " Error:" + ex.Message.Trim(), "");
                return "1$---$" + ex.Message.Trim();
            }
        }

        private void FileLogText1(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\HwayObrm_Web_" + filename + ".txt", true);
                sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(strRequest.Trim());
                sw.WriteLine(strResponse.Trim());
                sw.WriteLine("************************************************************************************************************************");
            }
            catch (Exception ex)
            {
                //Response.Write("Error while writing logs : " + ex.Message.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        protected void chkStb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gv = (GridViewRow)chk.NamingContainer;
            if (grdStb.Rows.Count == 1)
            {
                chk.Checked = true;
                return;
            }
            int rownumber = gv.RowIndex;
            if (chk.Checked)
            {
                int i;
                for (i = 0; i <= grdStb.Rows.Count - 1; i++)
                {
                    if (i != rownumber)
                    {
                        CheckBox chkcheckbox = ((CheckBox)(grdStb.Rows[i].FindControl("chkStb")));
                        chkcheckbox.Checked = false;
                    }
                }
                HiddenField hdnServiceString = (HiddenField)grdStb.Rows[rownumber].FindControl("hdnServiceStr");
                string service_str = hdnServiceString.Value;
            }
            else
            {
                chk.Checked = true;
            }
        }

        private void AddDataInGrid()
        {
            string flag = ViewState["Flag"].ToString().ToUpper();

            DataTable dt = new DataTable();
            DataRow dr;
            dt.TableName = "OSD";
            if (flag == "SMS")
            {
                dt.Columns.Add(new DataColumn("LCO Name", typeof(string)));

            }

            dt.Columns.Add(new DataColumn("Account", typeof(string)));
            dt.Columns.Add(new DataColumn("VC/MAC ID", typeof(string)));
            dt.Columns.Add(new DataColumn("Customer Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Template", typeof(string)));
            if (flag != "SMS")
            {

                dt.Columns.Add(new DataColumn("Frequency", typeof(string)));
                dt.Columns.Add(new DataColumn("Duration", typeof(Int32)));
                dt.Columns.Add(new DataColumn("Position", typeof(Int32)));
            }
            dt.Columns.Add(new DataColumn("Notification", typeof(string)));
            dt.Columns.Add(new DataColumn("Action", typeof(string)));
            dt.Columns.Add(new DataColumn("LCO ID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Action2", typeof(string)));

            dr = dt.NewRow();
            dt.Rows.Add(dr);
            ViewState["OSD"] = dt;
        }
        protected void grdAcountDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (TableCell tableCell in e.Row.Cells)
            {
                DataControlFieldCell cell = (DataControlFieldCell)tableCell;
                if (cell.ContainingField.HeaderText == "LCO ID")
                {
                    cell.Visible = false;
                }
                if (ddlNotification.SelectedValue == "OSD")
                {
                    if (cell.ContainingField.HeaderText == "Position")
                    {
                        cell.Visible = false;
                    }
                }
                else
                {
                    if (cell.ContainingField.HeaderText == "Duration")
                    {
                        cell.Visible = false;
                    }
                }
                if (cell.ContainingField.HeaderText == "Action2")
                {
                    cell.Visible = false;
                }
                if (cell.ContainingField.HeaderText == "Action")
                {
                    cell.Visible = false;
                }

                //Action2
            }
        }
        private void AddNewDataInGrid(string vc)
        {
            Int32 operator_id = Convert.ToInt32(Session["operator_id"]);
            if (ddlNotification.SelectedValue == "0" && ddlNotification.Visible == true)
            {
                lblmsg.Text = "Please select Notification";
                return;
            }
            if (ddlNotification.SelectedValue == "OSD")
            {
                if (ddlDuration.SelectedValue == "0" && ddlDuration.Visible == true)
                {
                    lblmsg.Text = "Please select Duration";
                    return;
                }
            }
            else
            {
                if (ddlPosition.SelectedValue == "0" && ddlPosition.Visible == true)
                {
                    lblmsg.Text = "Please select Position";
                    return;
                }
            }
            if (ddlFrequency.SelectedValue == "0" && ddlFrequency.Visible == true)
            {
                lblmsg.Text = "Please select Frequency";
                return;
            }

            if (ddlLco.SelectedValue == "-1" && ddlLco.Visible == true)
            {
                lblmsg.Text = "Please select LCO";
                return;
            }
            if (ViewState["OSD"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["OSD"];
                DataRow drCurrentRow = null;
                string flag = ViewState["Flag"].ToString().ToUpper();
                string Notification = "";

                if (dtCurrentTable.Rows.Count > 0)
                {

                    divAcountDetails.Visible = true;

                    if (dtCurrentTable.Select("'VC/MAC ID' = '" + vc + "'").Length > 0)
                    {
                        lblSearcherror.Text = "vc already Added";
                        return;
                    }
                    else
                    {
                        lblSearcherror.Text = "";
                    }

                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {

                        drCurrentRow = dtCurrentTable.NewRow();
                        if (flag == "SMS")
                        {
                            drCurrentRow["LCO Name"] = ddlLco.SelectedItem;
                            drCurrentRow["LCO ID"] = ddlLco.SelectedValue;

                        }
                        else
                        {
                            drCurrentRow["LCO ID"] = operator_id;
                        }
                        drCurrentRow["Account"] = lblAcno.Text;
                        drCurrentRow["VC/MAC ID"] = vc;
                        drCurrentRow["Customer Name"] = lblCustName.Text;
                        drCurrentRow["Template"] = dllTemplate.SelectedItem;

                        if (flag != "SMS")
                        {
                            drCurrentRow["Frequency"] = ddlFrequency.SelectedValue;
                            drCurrentRow["Duration"] = Convert.ToInt32(ddlDuration.SelectedValue);
                            drCurrentRow["Position"] = Convert.ToInt32(ddlPosition.SelectedValue);
                            drCurrentRow["Notification"] = ddlNotification.SelectedValue.ToUpper(); ;
                        }
                        else
                        {
                            drCurrentRow["Notification"] = "SMS";
                        }
                        drCurrentRow["Action"] = RadIncExc.SelectedItem;
                        drCurrentRow["Action2"] = RadIncExc.SelectedValue;

                    }
                    if (dtCurrentTable.Rows[0][0].ToString() == "")
                    {
                        dtCurrentTable.Rows[0].Delete();
                        dtCurrentTable.AcceptChanges();
                    }


                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["OSD"] = dtCurrentTable;

                    grdAcountDetails.DataSource = dtCurrentTable;
                    grdAcountDetails.DataBind();
                }
                else
                {
                    divAcountDetails.Visible = false;
                }
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            getSelectedvc();
        }
        private void getSelectedvc()
        {
            try
            {
                for (int i = 0; i < grdStb.Rows.Count; i++)
                {
                    CheckBox chk = (CheckBox)grdStb.Rows[i].Cells[1].FindControl("chkStb");

                    if (chk.Checked)
                    {
                        AddNewDataInGrid(grdStb.Rows[i].Cells[1].Text.Trim());
                        tblCustDetails.Visible = false;
                        btnAdd.Visible = false;
                        txtSearchParam.Text = "";
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        public DataTable exceldata(string filePath)
        {
            DataTable dt = new DataTable();
            using (TextReader tr = File.OpenText(filePath))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    string[] items = line.Split('\t');
                    if (dt.Columns.Count == 0)
                    {
                        // Create the data columns for the data table based on the number of items
                        // on the first line of the file
                        for (int i = 0; i < items.Length; i++)
                            dt.Columns.Add(new DataColumn("Column" + i, typeof(string)));
                    }
                    dt.Rows.Add(items);
                }
            }

            return dt;

        }

        //protected void fillCombo()
        //{
        //    string username, catid, operator_id;
        //    if (Session["username"] != null || Session["operator_id"] != null)
        //    {
        //        username = Session["username"].ToString();
        //        catid = Convert.ToString(Session["category"]);
        //        operator_id = Convert.ToString(Session["operator_id"]);
        //    }
        //    else
        //    {
        //        Session.Abandon();
        //        Response.Redirect("~/Login.aspx");
        //        return;
        //    }
        //    DataSet dsLco;
        //    if (catid == "0")
        //    {
        //        dsLco = Cls_Helper.Comboupdate(" aoup_lcopre_lco_det order by var_lcomst_name asc", "num_lcomst_operid", "var_lcomst_name");
        //    }
        //    else
        //    {
        //        dsLco = Cls_Helper.Comboupdate(" aoup_lcopre_lco_det WHERE num_lcomst_operid='" + operator_id + "' order by var_lcomst_name asc", "num_lcomst_operid", "var_lcomst_name");
        //    }

        //    ddlLco.DataSource = dsLco;
        //    ddlLco.DataTextField = "var_lcomst_name";
        //    ddlLco.DataValueField = "num_lcomst_operid";
        //    ddlLco.DataBind();
        //    //dsLco.Dispose();
        //    if (catid == "0")
        //    {
        //        ddlLco.Items.Insert(0, new ListItem("Select LCO", "-1"));
        //    }
        //    else
        //    {
        //        Message("SMS");
        //    }
        //}

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

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,num_lcomst_operid lcocode ";
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
                   

                }
                else
                {
                    //  lblmsg.Text = "No LCO Details Found";
                    // divdet.Visible = false;
                    // Paydet.Visible = false;
                    // btnSubmit.Visible = false;
                    // pnllco.Visible = false;
                }

                if (catid == "0")
                {
                    ddlLco.Items.Insert(0, new ListItem("Select LCO", "-1"));
                }
                else
                {
                    Message("SMS");
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


        private string getAllValueToSubmit()
        {
            string flag = ViewState["Flag"].ToString().ToUpper();

            string strdata = "";

            try
            {

                for (int i = 0; i < grdAcountDetails.Rows.Count; i++)
                {
                    string tempstr = "";
                    if (flag == "OSD")
                    {
                        tempstr = grdAcountDetails.Rows[i].Cells[0].Text + "$" + grdAcountDetails.Rows[i].Cells[1].Text + "$" + grdAcountDetails.Rows[i].Cells[2].Text + "$" + grdAcountDetails.Rows[i].Cells[3].Text + "$" + grdAcountDetails.Rows[i].Cells[4].Text + "$" + grdAcountDetails.Rows[i].Cells[5].Text + "$" + grdAcountDetails.Rows[i].Cells[6].Text + "$" + grdAcountDetails.Rows[i].Cells[7].Text + "$" + grdAcountDetails.Rows[i].Cells[10].Text + "$" + grdAcountDetails.Rows[i].Cells[9].Text;
                    }
                    else
                    {
                        tempstr = grdAcountDetails.Rows[i].Cells[1].Text + "$" + grdAcountDetails.Rows[i].Cells[2].Text + "$" + grdAcountDetails.Rows[i].Cells[3].Text + "$" + grdAcountDetails.Rows[i].Cells[4].Text + "$$0$0$" + grdAcountDetails.Rows[i].Cells[5].Text + "$" + grdAcountDetails.Rows[i].Cells[8].Text + "$" + grdAcountDetails.Rows[i].Cells[7].Text;
                    }
                    if (strdata.Length == 0)
                    {
                        strdata = tempstr;
                    }
                    else
                    {
                        strdata += "#" + tempstr;
                    }
                }

                return strdata;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            visibleControl();
            lblmsg.Text = "";
            lblSearcherror.Text = "";
            divAcountDetails.Visible = false;
            tblCustDetails.Visible = false;
            btnAdd.Visible = false;
            ddlNotification.SelectedValue = "0";
            Tr5.Visible = false;
            Tr6.Visible = false;
            ddlFrequency.SelectedValue = "0";
            TrTemplate.Visible = false;
            txtSearchParam.Text = "";
            RadIncExc.ClearSelection();
            RadIncExc.Enabled = true;
            btnReset.Visible = false;
            divSearchHolder.Visible = false;
            TrFullmsg.Visible = false;
            string flag = ViewState["Flag"].ToString().ToUpper();
            if (flag == "SMS")
            {
                fillCombo();
            }

            AddDataInGrid();
            ddlNotification.Enabled = true;
            ddlFrequency.Enabled = true;
            ddlDuration.Enabled = true;
            ddlPosition.Enabled = true;
            dllTemplate.Enabled = true;
            visibleControl();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            txtSearchParam.Text = "";
            string operator_id = Convert.ToString(Session["operator_id"]);
            string flag = ViewState["Flag"].ToString().ToUpper();
            string type = "";
            string data = "";

            if (RadIncExc.SelectedValue == "A")
            {
                if (flag != "OSD")
                {
                    type = "SMS";
                    operator_id = ddlLco.SelectedValue.ToString();
                }
                else
                {
                    type = ddlNotification.SelectedValue.ToString();
                }

                data = "0$0$$" + dllTemplate.SelectedItem.ToString() + "$" + ddlFrequency.SelectedValue.ToString() + "$" + ddlDuration.SelectedValue.ToString() + "$" + ddlPosition.SelectedValue.ToString() + "$" + type + "$A$" + operator_id + "";
                callprocedureMst(data);
            }
            else if (RadIncExc.SelectedValue == "I")
            {
                data = getAllValueToSubmit();
                callprocedureMst(data);
            }
            else if (RadIncExc.SelectedValue == "B")
            {
            }

        }
        protected void Message(string FLAG)
        {
            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue; //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            DataSet dsMessage;

            string TYPE = "";
            if (FLAG == "SMS")
            {
                TYPE = "SMS";
                lblMessage.Text = "Message";
                operator_id = ddlLco.SelectedValue;
            }
            else
            {
                TYPE = ddlNotification.SelectedValue.ToUpper();
                lblMessage.Text = "Template";
            }

            dsMessage = Cls_Helper.Comboupdate(@" view_lcopre_notification WHERE ""type""='" + TYPE + "' and lcoid='" + operator_id + "' order by msg asc", "id", "msg");

            if (dsMessage.Tables[0].Rows != null && dsMessage.Tables[0].Rows.Count > 0)
            {

                dllTemplate.DataSource = dsMessage;
                dllTemplate.DataTextField = "msg";
                dllTemplate.DataValueField = "id";
                dllTemplate.DataBind();
                dllTemplate.Items.Insert(0, new ListItem("Select Message"));

                dllTemplate.SelectedIndex = -1;
                dsMessage.Dispose();

                SetFullMsg(dllTemplate.SelectedItem.ToString());
                TrFullmsg.Visible = true;
                TrTemplate.Visible = true;
            }
            else
            {
                TrFullmsg.Visible = false;
                TrTemplate.Visible = false;
            }
        }
        //dllTemplate_SelectedIndexChanged
        protected void dllTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFullMsg(dllTemplate.SelectedItem.ToString());
        }
        private void SetFullMsg(string msg)
        {
            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue;//Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            DataSet dsMessage;
            string FLAG = ViewState["Flag"].ToString().ToUpper();
            string TYPE = "";
            if (FLAG == "SMS")
            {
                TYPE = "SMS";
                lblMessage.Text = "Message";
                operator_id = ddlLco.SelectedValue;
            }
            else
            {
                TYPE = ddlNotification.SelectedValue.ToUpper();
                lblMessage.Text = "Template";
            }
            string FullMsg = "";
            FullMsg = Cls_Helper.fnGetScalar(@" select full_msg from  view_lcopre_notification WHERE ""type""='" + TYPE + "' AND  msg='" + msg + "'  and lcoid='" + operator_id + "'");
            lblFullMsg.Text = FullMsg;
        }
        private void callprocedureMst(string data)
        {
            Cls_BLL_TransOsdBmailNotification obj = new Cls_BLL_TransOsdBmailNotification();

            username = Convert.ToString(Session["username"]);
            string pro_output = "";

            string FromTime = txtFrom.Text + " " + ddlFromHour.SelectedValue + ":" + ddlfromMinute.SelectedValue + ":00 " + ddlFromAmPm.SelectedValue;
            DateTime dtfromtime = Convert.ToDateTime(FromTime);

            string ToTime = txtTo.Text + " " + ddlToHour.SelectedValue + ":" + ddlToMinue.SelectedValue + ":00 " + ddlToAmPm.SelectedValue;
            DateTime dtTotime = Convert.ToDateTime(ToTime);


            pro_output = obj.SaveOsdBMail(username, data, dtfromtime.ToString(), dtTotime.ToString());
            if (pro_output.Split('$')[0] == "9999")
            {
                visibleControl();
                lblmsg.Text = pro_output.Split('$')[1].ToString();
                lblSearcherror.Text = "";
                divAcountDetails.Visible = false;
                tblCustDetails.Visible = false;
                btnAdd.Visible = false;
                btnCancel.Visible = false;
                ddlNotification.SelectedValue = "0";
                Tr5.Visible = false;
                Tr6.Visible = false;
                ddlFrequency.SelectedValue = "0";
                TrTemplate.Visible = false;
                txtSearchParam.Text = "";
                RadIncExc.ClearSelection();
                RadIncExc.Enabled = true;
                btnReset.Visible = false;
                divSearchHolder.Visible = false;
                TrFullmsg.Visible = false;
                string flag = ViewState["Flag"].ToString().ToUpper();
                if (flag == "SMS")
                {
                    fillCombo();
                }

                AddDataInGrid();
                ddlNotification.Enabled = true;
                ddlFrequency.Enabled = true;
                ddlDuration.Enabled = true;
                ddlPosition.Enabled = true;
                dllTemplate.Enabled = true;
                visibleControl();


            }
            else
            {
                lblmsg.Text = pro_output.Split('$')[1].ToString();// "File Upload : Failed";
            }
        }

        protected void grdAcountDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void ddlLco_SelectedIndexChanged(object sender, EventArgs e)
        {
            Message("SMS");
        }
        protected void ddlNotification_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNotification.SelectedValue != "OSD" && ddlNotification.SelectedValue != "BMAIL")
            {
                TrTemplate.Visible = false;
            }
            Message("OSD");
            if (ddlNotification.SelectedValue == "OSD")
            {
                Tr6.Visible = false;
                Tr5.Visible = true;

            }
            else
            {
                Tr5.Visible = false;
                Tr6.Visible = true;

            }
        }

        protected void RadIncExc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string flag = ViewState["Flag"].ToString().ToUpper();


                if (flag != "SMS")
                {
                    if (txtFrom.Text == "")
                    {
                        lblmsg.Text = "Please select from date";
                        RadIncExc.ClearSelection();
                        return;
                    }
                    if (txtTo.Text == "")
                    {
                        lblmsg.Text = "Please select to date";
                        RadIncExc.ClearSelection();
                        return;
                    }
                    if (Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")) > Convert.ToDateTime(txtFrom.Text))
                    {
                        lblmsg.Text = "From date must be Greater than current date";
                        RadIncExc.ClearSelection();
                        return;
                    }
                    if (Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")) > Convert.ToDateTime(txtTo.Text))
                    {
                        lblmsg.Text = "To date must be Greater than current date";
                        RadIncExc.ClearSelection();
                        return;
                    }

                    if (Convert.ToDateTime(txtFrom.Text) > Convert.ToDateTime(txtTo.Text))
                    {
                        lblmsg.Text = "From date must be less than to date";
                        RadIncExc.ClearSelection();
                        return;
                    }

                }
                if (ddlLco.SelectedValue == "-1" && ddlLco.Visible == true)
                {
                    lblmsg.Text = "Please select LCO";
                    RadIncExc.ClearSelection();
                    return;
                }

                if (ddlNotification.SelectedValue == "0" && ddlNotification.Visible == true)
                {
                    lblmsg.Text = "Please select notification";
                    RadIncExc.ClearSelection();
                    return;
                }
                if (TrTemplate.Visible == false)
                {
                    lblmsg.Text = "There is no predifined message for this LCO";
                    RadIncExc.ClearSelection();
                    return;
                }
                if (ddlNotification.SelectedValue == "OSD")
                {
                    if (ddlDuration.SelectedValue == "0" && ddlDuration.Visible == true)
                    {
                        lblmsg.Text = "Please select duration";
                        RadIncExc.ClearSelection();
                        return;
                    }
                }
                else
                {
                    if (ddlPosition.SelectedValue == "0" && ddlPosition.Visible == true)
                    {
                        lblmsg.Text = "Please select position";
                        RadIncExc.ClearSelection();
                        return;
                    }
                }
                if (ddlFrequency.SelectedValue == "0" && ddlFrequency.Visible == true)
                {
                    lblmsg.Text = "Please select frequency";
                    RadIncExc.ClearSelection();
                    return;
                }


                if (RadIncExc.SelectedValue == "I")
                {
                    ddlNotification.Enabled = false;
                }
                else
                {
                    ddlNotification.Enabled = true;
                    dllTemplate.Enabled = true;
                    ddlFrequency.Enabled = true;
                    ddlDuration.Enabled = true;
                    ddlPosition.Enabled = true;

                }
                ddlLco.Enabled = false;
                lblmsg.Text = "";
                btnReset.Visible = true;
                RadIncExc.Enabled = false;

                if (RadIncExc.SelectedValue == "A")
                {
                    ddlNotification.Enabled = false;
                    dllTemplate.Enabled = false;
                    ddlFrequency.Enabled = false;
                    ddlDuration.Enabled = false;
                    ddlPosition.Enabled = false;


                    divAcountDetails.Visible = true;
                    Table1.Visible = false;

                    Button2.Visible = false;
                    tblCustDetails.Visible = false;
                    divSearchHolder.Visible = false;

                }
                else if (RadIncExc.SelectedValue == "B")
                {
                    dvBulk.Visible = true;
                    divSearchHolder.Visible = false;
                    if (flag == "SMS")
                    {
                        excelSMS.Visible = true;
                        excelOSD.Visible = false;
                    }
                    else
                    {
                        excelOSD.Visible = true;
                        excelSMS.Visible = false;
                    }


                }
                else
                {
                    dvBulk.Visible = false;
                    divSearchHolder.Visible = true;
                }


            }
            catch (Exception ex)
            {

            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            string flag = ViewState["Flag"].ToString().ToUpper();
            visibleControl();
            TrFullmsg.Visible = false;
            lblmsg.Text = "";
            lblSearcherror.Text = "";
            divAcountDetails.Visible = false;
            tblCustDetails.Visible = false;
            btnAdd.Visible = false;
            ddlNotification.SelectedValue = "0";
            ddlFrequency.SelectedValue = "0";
            TrTemplate.Visible = false;
            txtSearchParam.Text = "";
            Tr5.Visible = false;
            Tr6.Visible = false;
            RadIncExc.ClearSelection();
            RadIncExc.Enabled = true;
            btnReset.Visible = false;
            divSearchHolder.Visible = false;
            AddDataInGrid();
            ddlLco.Enabled = true;
            if (flag == "SMS")
            {
                fillCombo();
            }
            ddlFrequency.Enabled = true;
            ddlDuration.Enabled = true;
            ddlPosition.Enabled = true;
            dllTemplate.Enabled = true;
            ddlNotification.Enabled = true;
            dvBulk.Visible = false;
            lblStatusHeading.Text = "";
            lblStatus.Text = "";

        }
        protected bool FileUpload()
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
                return false;
            }

            //check - file extension
            if (fupData.PostedFile.ContentType != "text/plain")
            {
                lblStatusHeading.Text = "File Upload : Failed";
                lblStatus.Text = "Only tab deliminated text(.txt) files are allowed";
                return false;
            }

            //check - file length
            if (fupData.PostedFile.ContentLength == 0)
            {
                lblStatusHeading.Text = "File Upload : Failed";
                lblStatus.Text = "File does not have contents";
                return false;
            }

            ViewState["FileName"] = fupData.PostedFile.FileName;
            //check - directory and save file in diectory
            string directoryPath = string.Format(@"D:/DataUpload/Hwaysmsosdbmail/{0}", username.Trim());
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

                return false;
            }
            return true;
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (!FileUpload())
            {
                return;
            }
            Cls_Presentation_Helper helper = new Cls_Presentation_Helper();
            DateTime date = DateTime.Now;
            string cur_timestamp = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
            string cur_time = DateTime.Now.ToString("dd-MMM-yyyy_hh:mm:ss");

            string operator_id = Convert.ToString(Session["operator_id"]);
            string table_columns = "";
            string flag = ViewState["Flag"].ToString().ToUpper();

            string FromTime = txtFrom.Text + " " + ddlFromHour.SelectedValue + ":" + ddlfromMinute.SelectedValue + ":00 " + ddlFromAmPm.SelectedValue;
            DateTime dtfromtime = Convert.ToDateTime(FromTime);

            string ToTime = txtTo.Text + " " + ddlToHour.SelectedValue + ":" + ddlToMinue.SelectedValue + ":00 " + ddlToAmPm.SelectedValue;
            DateTime dtTotime = Convert.ToDateTime(ToTime);



            if (flag != "SMS")
            {
                table_columns = "( var_lcopre_notif_vcid, num_lcopre_notif_mobileno  constant \"" + 0 + "\", var_lcopre_notif_msg_header  constant \"" + dllTemplate.SelectedItem + "\", var_lcopre_notif_fullmsg  constant \"" + lblFullMsg.Text + "\", var_lcopre_notif_notification  constant \"" + ddlNotification.SelectedValue + "\", var_lcopre_notif_frequency  constant \"" + ddlFrequency.SelectedValue + "\" ,num_lcopre_notif_postition constant \"" + ddlPosition.SelectedValue + "\", num_lcopre_notif_duration constant \"" + ddlDuration.SelectedValue + "\", var_lcopre_notif_insby constant \"" + username + "\",  dat_lcopre_notif_insdt  constant \"" + cur_timestamp + "\",var_lcopre_notif_lcoid constant \"" + operator_id + "\",dat_lcopre_from  constant \"" + dtfromtime.ToString() + "\",dat_lcopre_to  constant \"" + dtTotime.ToString() + "\")";

            }
            else
            {
                operator_id = ddlLco.SelectedValue.ToString();
                table_columns = "( var_lcopre_notif_vcid, num_lcopre_notif_mobileno, var_lcopre_notif_msg_header  constant \"" + dllTemplate.SelectedItem + "\", var_lcopre_notif_fullmsg  constant \"" + lblFullMsg.Text + "\", var_lcopre_notif_notification  constant SMS, var_lcopre_notif_frequency  constant \"" + 0 + "\" ,num_lcopre_notif_postition constant \"" + 0 + "\", num_lcopre_notif_duration constant \"" + 0 + "\", var_lcopre_notif_insby constant \"" + username + "\",  dat_lcopre_notif_insdt  constant \"" + cur_timestamp + "\",var_lcopre_notif_lcoid constant \"" + operator_id + "\",dat_lcopre_from  constant \"" + dtfromtime.ToString() + "\",dat_lcopre_to  constant \"" + dtTotime.ToString() + "\")";

            }


            //string uploadResult = helper.cDataUpload("D:\\DataUpload\\Hwaysmsosdbmail\\" + username + "\\" + ViewState["FileName"].ToString(),
            //                                         "D:\\DataUpload\\Hwaysmsosdbmail\\" + username + "\\smsosdbmailCTL.txt",
            //                                         "D:\\DataUpload\\Hwaysmsosdbmail\\" + username + "\\smsosdbmailLOG.log",
            //                                         table_columns, "UPASS_LOCAL", "SYSTEM", "cba", "aoup_lcopre_notification_tmp", "");


            //string uploadResult = helper.cDataUpload("D:\\DataUpload\\Hwaysmsosdbmail\\" + username + "\\" + ViewState["FileName"].ToString(),
            //                                     "D:\\DataUpload\\Hwaysmsosdbmail\\" + username + "\\smsosdbmailCTL.txt",
            //                                     "D:\\DataUpload\\Hwaysmsosdbmail\\" + username + "\\smsosdbmailLOG.log",
            //                                     table_columns, "UPASSDB", "UPASS", "cba", "aoup_lcopre_notification_tmp", "");

            string uploadResult = helper.cDataUpload("D:\\DataUpload\\Hwaysmsosdbmail\\" + username + "\\" + ViewState["FileName"].ToString(),
                                               "D:\\DataUpload\\Hwaysmsosdbmail\\" + username + "\\smsosdbmailCTL.txt",
                                               "D:\\DataUpload\\Hwaysmsosdbmail\\" + username + "\\smsosdbmailLOG.log",
                                               table_columns, "upassdb", "upass", "cba", "aoup_lcopre_notification_tmp", "");





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
            Cls_BLL_TransOsdBmailNotification obj = new Cls_BLL_TransOsdBmailNotification();
            username = Convert.ToString(Session["username"]);
            string pro_output = obj.SaveOsdBMailbULK(username);
            if (pro_output.Split('#')[0] == "9999")
            {
                lblStatus.Text = pro_output.Split('$')[1].ToString();
            }
            else
            {
                lblStatus.Text = pro_output.Split('$')[1].ToString();
            }
        }

    }
}