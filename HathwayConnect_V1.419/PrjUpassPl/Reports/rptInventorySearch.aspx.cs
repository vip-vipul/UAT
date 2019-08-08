using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OracleClient;
using System.IO;
using System.Net;
using System.Text;
using System.Data;

namespace PrjUpassPl.Reports
{
    public partial class rptInventorySearch : System.Web.UI.Page
    {
        string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Inventory Search Report";

            if (!IsPostBack)
            {
                // Session["RightsKey"] = null;
                Session["RightsKey"] = "N";

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
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

        protected void binddata()
        {
            string search_type = RadSearchby.SelectedValue.ToString();
            try
            {
                lblSearchMsg.Text = "";
                string FLD_FLAGS = "";
                String DEVICE_ID = "";
                string reqcode = "";
                string req = "";
                String StrPriErroResponse = "";
                String[] final_obrm_status;
                string APISERIAL_NO = "";
                string APIVENDOR_WARRANTY_END = "";
                string PAIWARRANTY_END = "";
                string APIACCOUNT_NO = "";
                string APICATEGORY = "";
                string APICOMPANY = "";
                string APIDEVICE_ID = "";
                string APIDEVICE_TYPE = "";
                string APIMANUFACTURER = "";
                string APIMODEL = "";
                string APISOURCE = "";
                string APISTATE_ID = "";
                string poid = "";
                string DELIVERY_STATUS = "";
                if (txtsearchpara.Text.Trim() != "")
                {
                    DEVICE_ID = txtsearchpara.Text.Trim();
                    FLD_FLAGS = "121";

                    if (search_type == "120")
                    { FLD_FLAGS = "120"; }
                    else
                    { FLD_FLAGS = "121"; }
                    reqcode = "40";
                    req = Session["username"].ToString() + "$" + DEVICE_ID + "$" + FLD_FLAGS;
                    StrPriErroResponse = callAPI(req, reqcode);
                    final_obrm_status = StrPriErroResponse.Split('$');
                    DataTable dt = new DataTable();
                    dt.Clear();

                    if (final_obrm_status[0].ToString() == "0")
                    {
                        APISERIAL_NO = final_obrm_status[1].ToString();
                        APIVENDOR_WARRANTY_END = final_obrm_status[2].ToString();
                        PAIWARRANTY_END = final_obrm_status[3].ToString();
                        APIACCOUNT_NO = final_obrm_status[4].ToString();
                        APICATEGORY = final_obrm_status[5].ToString();
                        APICOMPANY = final_obrm_status[6].ToString();
                        APIDEVICE_ID = final_obrm_status[7].ToString();
                        APIDEVICE_TYPE = final_obrm_status[8].ToString();
                        APIMANUFACTURER = final_obrm_status[9].ToString();
                        APIMODEL = final_obrm_status[10].ToString();
                        APISOURCE = final_obrm_status[11].ToString();
                        APISTATE_ID = final_obrm_status[12].ToString().Trim();
                        poid = final_obrm_status[13].ToString().Trim();
                        DELIVERY_STATUS = final_obrm_status[14].ToString().Trim();
                        dt.Columns.Add("SerialNo");
                        dt.Columns.Add("VendorWarrantyEnd");
                        dt.Columns.Add("WarrantyEnd");
                        dt.Columns.Add("AccountNo");
                        dt.Columns.Add("Category");
                        dt.Columns.Add("Company");
                        dt.Columns.Add("DeviceId");
                        dt.Columns.Add("DeviceType");
                        dt.Columns.Add("ManufacturerDetails");
                        dt.Columns.Add("Model");
                        dt.Columns.Add("Source");
                        dt.Columns.Add("StateId");
                        dt.Columns.Add("POId");
                        dt.Columns.Add("DeliveryStatus");

                        DataRow r1 = dt.NewRow();
                        r1[0] = APISERIAL_NO;
                        r1[1] = APIVENDOR_WARRANTY_END;
                        r1[2] = PAIWARRANTY_END;
                        r1[3] = APIACCOUNT_NO;
                        r1[4] = APICATEGORY;
                        r1[5] = APICOMPANY;
                        r1[6] = APIDEVICE_ID;
                        r1[7] = APIDEVICE_TYPE;
                        r1[8] = APIMANUFACTURER;
                        r1[9] = APIMODEL;
                        r1[10] = APISOURCE;
                        r1[12] = poid;
                        string strState = "";
                        if (APISTATE_ID == "1")
                            strState = "GOOD";
                        else if (APISTATE_ID == "2")
                            strState = "ALLOCATED";
                        else if (APISTATE_ID == "3")
                            strState = "FAULTY";
                        else if (APISTATE_ID == "4")
                            strState = "REPAIRING";
                        else if (APISTATE_ID == "5")
                            strState = "PRE-ACTIVE";
                        else if (APISTATE_ID == "6")
                            strState = "DEAD";

                        r1[11] = strState;

                        string strerror = "";
                        if (DELIVERY_STATUS == "2")
                            strerror = "This Device is undelivered. It can not be activated";
                        else if (DELIVERY_STATUS == "3")
                            strerror = "This Device is Faulty. It can not be activated";
                        else if (DELIVERY_STATUS == "4")
                            strerror = "This Device is Lost in Transit. It can not be activated";
                        else
                            strerror = "";

                        r1[13] = strerror;
                        dt.Rows.Add(r1);
                        ViewState["invtsearched_Details"] = dt;

                        grdtransdet.DataSource = dt;
                        grdtransdet.DataBind();
                        btnGenerateExcel.Visible = true;
                    }
                    else
                    {
                        lblSearchMsg.Text = "Error while calling Obrm OP Search";
                        btnGenerateExcel.Visible = false;
                        grdtransdet.DataSource = null;
                        grdtransdet.DataBind();
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                btnGenerateExcel.Visible = false;
                grdtransdet.DataSource = null;
                grdtransdet.DataBind();
                Response.Write("Error : " + ex.Message.Trim());
                return;
            }
        }

        public String callAPI(string Request, string request_code)
        {
            try
            {
                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21//TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
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
                return "1$---$" + ex.Message.Trim();
            }
        }

        protected void ExportExcel()
        {
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
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["invtsearched_Details"];

            if (dt == null)
            {
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "InventorySearch_" + datetime + ".xls");
                try
                {

                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                        + "Serial No." + Convert.ToChar(9)
                        + "Vendor Warranty End" + Convert.ToChar(9)
                        + "Warranty End" + Convert.ToChar(9)
                        + "Account No" + Convert.ToChar(9)
                        + "Category" + Convert.ToChar(9)
                        + "Company" + Convert.ToChar(9)
                         + "Device Id" + Convert.ToChar(9)
                          + "Device Type" + Convert.ToChar(9)
                        + "Manufacturer Details" + Convert.ToChar(9)
                        + "Model" + Convert.ToChar(9)
                        + "Source" + Convert.ToChar(9)
                        + "Device State" + Convert.ToChar(9)
                        + "PO Id" + Convert.ToChar(9);
                    //+ "DeliveryStatus" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + "'" + dt.Rows[i]["SerialNo"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["VendorWarrantyEnd"].ToString() + Convert.ToChar(9)
                                 + dt.Rows[i]["WarrantyEnd"].ToString() + Convert.ToChar(9)
                                  + dt.Rows[i]["AccountNo"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Category"].ToString() + Convert.ToChar(9)
                                 + dt.Rows[i]["Company"].ToString() + Convert.ToChar(9)
                                 + "'" + dt.Rows[i]["DeviceId"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["DeviceType"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["ManufacturerDetails"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Model"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Source"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["StateId"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["POId"].ToString() + Convert.ToChar(9);
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

                dt.Dispose();
                Response.Redirect("../MyExcelFile/" + "InventorySearch_" + datetime + ".xls");


            }

        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }
    }
}