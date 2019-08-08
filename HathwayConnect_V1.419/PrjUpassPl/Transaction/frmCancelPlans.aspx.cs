using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Transaction;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassDAL.Helper;
using PrjUpassDAL.Authentication;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.IO.Compression;
using CrystalDecisions.CrystalReports.Engine;

namespace PrjUpassPl.Transaction
{
    public partial class frmCancelPlans : System.Web.UI.Page
    {

        string strerror = "";
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        String CancelSelectedFOCPlan = "";
        static string plantype = "B";
        //static string poidlist;
        //static string basic_poids = "";
        //static string addon_poids = "";
        //static string ala_poids = "";
        //string city = "";
        string oper_id = "";
        string username = "";
        string catid = "";
        DataTable dtAddonPlans;
        DataTable dtAddonPlansreg;
        DataTable dtBasicPlans;
        DataTable dtAlacartePlans;
        DataTable dthathwayspecial;
        DataTable dtAlacartePlansFree;
        static string ErrorMsg = "";
        static string parentsmsg = "";
        DataTable dtBasicFreePlans;
        DataTable dtFOCPlanStatus;
        DataTable dtPlanStatus;
        StringBuilder sb = new StringBuilder();


        private byte[] Compress(byte[] b)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream zs = new GZipStream(ms, CompressionMode.Compress, true);
            zs.Write(b, 0, b.Length);
            zs.Close();
            return ms.ToArray();
        }

        private byte[] Decompress(byte[] b)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream zs = new GZipStream(new MemoryStream(b),
                                           CompressionMode.Decompress, true);
            byte[] buffer = new byte[4096];
            int size;
            while (true)
            {
                size = zs.Read(buffer, 0, buffer.Length);
                if (size > 0)
                    ms.Write(buffer, 0, size);
                else break;
            }
            zs.Close();
            return ms.ToArray();
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            System.Web.UI.PageStatePersister pageStatePersister1 = this.PageStatePersister;
            pageStatePersister1.Load();
            String vState = pageStatePersister1.ViewState.ToString();
            byte[] pBytes = System.Convert.FromBase64String(vState);
            pBytes = Decompress(pBytes);
            LosFormatter mFormat = new LosFormatter();
            Object ViewState = mFormat.Deserialize(System.Convert.ToBase64String(pBytes));
            return new Pair(pageStatePersister1.ControlState, ViewState);
        }

        protected override void SavePageStateToPersistenceMedium(Object pViewState)
        {
            Pair pair1;
            System.Web.UI.PageStatePersister pageStatePersister1 = this.PageStatePersister;
            Object ViewState;
            if (pViewState is Pair)
            {
                pair1 = ((Pair)pViewState);
                pageStatePersister1.ControlState = pair1.First;
                ViewState = pair1.Second;
            }
            else
            {
                ViewState = pViewState;
            }
            LosFormatter mFormat = new LosFormatter();
            StringWriter mWriter = new StringWriter();
            mFormat.Serialize(mWriter, ViewState);
            String mViewStateStr = mWriter.ToString();
            byte[] pBytes = System.Convert.FromBase64String(mViewStateStr);
            pBytes = Compress(pBytes);
            String vStateStr = System.Convert.ToBase64String(pBytes);
            pageStatePersister1.ViewState = vStateStr;
            pageStatePersister1.Save();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (radPlanAD.Checked)
            //{
            //    plantype = "AD";
            //}
            //else
            //{
            //    plantype = "AL";
            //}

            Master.PageHeading = "Manage Expired Plans";
            Session["pagenos"] = "0";


            dtBasicFreePlans = new DataTable();

            dtBasicFreePlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtBasicFreePlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtBasicFreePlans.Columns.Add(new DataColumn("language"));

            dtFOCPlanStatus = new DataTable();
            dtFOCPlanStatus.Columns.Add("PlanName");
            dtFOCPlanStatus.Columns.Add("RenewStatus");

            ///Added by Vivek Singh on 15-Jul-2016
            dtPlanStatus = new DataTable();
            dtPlanStatus.Columns.Add("VCID");
            dtPlanStatus.Columns.Add("PlanName");
            dtPlanStatus.Columns.Add("Status");


            //ViewState["customer_alacarte_plans"] = dtAlacartePlans;

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {
                oper_id = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                GridView grdStb = new GridView();
                dynamiclink.Visible = false;
                ViewState["provi_chkbx_selected_service"] = "0"; // selected service index
                hdntag.Value = "lnkDetail";
                resetSearchBox();

                if (Session["username"] != null && Session["operator_id"] != null)
                {
                    Session["RightsKey"] = "N";
                    Cls_Business_TxnAssignPlan pl1 = new Cls_Business_TxnAssignPlan();


                    string city = "";
                    String dasarea = "";
                    String operid = "";
                    string Flag = "";
                    string JVNO = "";
                    string StateName = "";
                    pl1.GetUserCity(Session["username"].ToString(), out city, out dasarea, out operid, out JVNO, out Flag, out StateName);
                    ViewState["cityid"] = city;
                    Session["dasarea"] = dasarea;
                    Session["lco_operid"] = operid;
                    Session["StateName"] = StateName;
                    Session["lcoid"] = Session["operator_id"];
                    oper_id = Convert.ToString(Session["lcoid"]);
                    ViewState["JVFlag"] = Flag;
                    Session["JVNO"] = JVNO;



                    if (Convert.ToString(Session["category"]) == "11")
                    {
                        lblavbal.Visible = false;
                        avbal.Visible = false;
                    }
                    else
                    {
                        lblavbal.Visible = true;
                        avbal.Visible = true;
                    }
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                /*
                //fetching whole plan data in datatable
                Cls_Business_TxnAssignPlan pl = new Cls_Business_TxnAssignPlan();
                DataTable dt = pl.GetPackagedef(username);
                ViewState["plan_def"] = dt;
                
                
                //table which holds customer plans
                DataTable dtCustPlans = new DataTable();
                dtCustPlans.Columns.Add(new DataColumn("PLAN_ID"));
                dtCustPlans.Columns.Add(new DataColumn("PLAN_NAME"));
                dtCustPlans.Columns.Add(new DataColumn("PLAN_TYPE"));
                dtCustPlans.Columns.Add(new DataColumn("PLAN_POID"));
                dtCustPlans.Columns.Add(new DataColumn("DEAL_POID"));
                dtCustPlans.Columns.Add(new DataColumn("PRODUCT_POID"));
                dtCustPlans.Columns.Add(new DataColumn("CUST_PRICE"));
                dtCustPlans.Columns.Add(new DataColumn("LCO_PRICE"));
                dtCustPlans.Columns.Add(new DataColumn("PAYTERM"));
                dtCustPlans.Columns.Add(new DataColumn("CITYID"));
                dtCustPlans.Columns.Add(new DataColumn("CITY_NAME"));
                dtCustPlans.Columns.Add(new DataColumn("COMPANY_CODE"));
                dtCustPlans.Columns.Add(new DataColumn("INSBY"));
                dtCustPlans.Columns.Add(new DataColumn("INSDT"));
                dtCustPlans.Columns.Add(new DataColumn("ACTIVATION"));
                dtCustPlans.Columns.Add(new DataColumn("EXPIRY"));
                dtCustPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
                dtCustPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
                dtCustPlans.Columns.Add(new DataColumn("PLAN_STATUS"));
               // ViewState["customer_plans"] = dtCustPlans;
                */

                //table which holds basic plans

                // dtBasicPlans.Columns.Add(new DataColumn("PLAN_ID"));

                Cls_Business_TxnAssignPlan objAsPl = new Cls_Business_TxnAssignPlan();
                string responseStr = objAsPl.getDistributorDetails(username, oper_id);

                if (responseStr == "ex_occured")
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }

                string[] arrDistDetails = new string[2];
                arrDistDetails = responseStr.Split('~');
                string cust_name = arrDistDetails[0];
                string avail_bal = arrDistDetails[1];
                string user = arrDistDetails[2];

                double balance = 0;
                lblDistName.Text = cust_name;
                lblAvailBal.Text = avail_bal;
                balance = Convert.ToDouble(lblAvailBal.Text);

                if (balance < 1000)
                {
                    lblAvailBal.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblAvailBal.ForeColor = System.Drawing.Color.Black;

                }
                lbluser.Text = user;

                //fill service Activation info popup reasons
                DataTable dtSerActReasons = objAsPl.getProviReasons(username, "SACT");
                ddlServiceInfoPopupActReason.DataSource = dtSerActReasons;
                ddlServiceInfoPopupActReason.DataValueField = "num_reason_id";
                ddlServiceInfoPopupActReason.DataTextField = "var_reason_name";
                ddlServiceInfoPopupActReason.DataBind();
                //fill service Deactivation info popup reasons
                DataTable dtSerDactReasons = objAsPl.getProviReasons(username, "SDACT");
                ddlServiceInfoPopupDactReason.DataSource = dtSerDactReasons;
                ddlServiceInfoPopupDactReason.DataValueField = "num_reason_id";
                ddlServiceInfoPopupDactReason.DataTextField = "var_reason_name";
                ddlServiceInfoPopupDactReason.DataBind();
                //fill plan cancel reasons
                DataTable dtPlnCnlReasons = objAsPl.getProviReasons(username, "PCL");
                ddlPopupReason.DataSource = dtPlnCnlReasons;
                ddlPopupReason.DataValueField = "num_reason_id";
                ddlPopupReason.DataTextField = "var_reason_name";
                ddlPopupReason.DataBind();
                ListItem default_itm = new ListItem();
                default_itm.Text = "Select Reason";
                default_itm.Value = "0";
                ddlServiceInfoPopupActReason.Items.Insert(0, default_itm);
                ddlServiceInfoPopupDactReason.Items.Insert(0, default_itm);
                ddlPopupReason.Items.Insert(0, default_itm);
                //fill service Deactivation info popup reasons
                DataTable dtReasons = objAsPl.getProviReasons(username, "TERM");
                ddlTERMINATEReason.DataSource = dtReasons;
                ddlTERMINATEReason.DataValueField = "num_reason_id";
                ddlTERMINATEReason.DataTextField = "var_reason_name";
                ddlTERMINATEReason.DataBind();
                ddlTERMINATEReason.Items.Insert(0, default_itm);

                dtReasons = objAsPl.getProviReasons(username, "FSS");
                ddlFaultyReason.DataSource = dtReasons;
                ddlFaultyReason.DataValueField = "num_reason_id";
                ddlFaultyReason.DataTextField = "var_reason_name";
                ddlFaultyReason.DataBind();
                ddlFaultyReason.Items.Insert(0, default_itm);

                dtReasons = objAsPl.getProviReasons(username, "SSMC");
                ddlSawpReason.DataSource = dtReasons;
                ddlSawpReason.DataValueField = "num_reason_id";
                ddlSawpReason.DataTextField = "var_reason_name";
                ddlSawpReason.DataBind();
                ddlSawpReason.Items.Insert(0, default_itm);
                txtSearchParam.Text = Session["customer_no"].ToString();
                btnSearch_Click(null, null);
            }
        }//shri

        protected string getAvailableBal()
        {
            //string username = "";
            string bal = "0";
            if (Session["username"] != null)
            {

                if (catid == "11")
                {
                    username = Convert.ToString(Session["lco_username"]);
                }
                else
                {
                    username = Convert.ToString(Session["username"]);
                }
                Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                bal = obj.getDistBalance(username);
                if (bal == "ex_occured")
                {
                    bal = "0";
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            return bal;
        }//shri



        protected void resetAllGrids()
        {
            /*if (ViewState["customer_basic_plans"] != null)
            {
                DataTable dt = (DataTable)ViewState["customer_basic_plans"];
                dt.Rows.Clear();
                ViewState["customer_basic_plans"] = dt;
            } 
            if (ViewState["customer_addon_plans"] != null)
            {
                DataTable dt = (DataTable)ViewState["customer_addon_plans"];
                dt.Rows.Clear();
                ViewState["customer_addon_plans"] = dt;
            }
            if (ViewState["customer_alacarte_plans"] != null)
            {
                DataTable dt = (DataTable)ViewState["customer_alacarte_plans"];
                dt.Rows.Clear();
                ViewState["customer_alacarte_plans"] = dt;
            }
            if (ViewState["customer_plans"] != null)
            {
                DataTable dt = (DataTable)ViewState["customer_plans"];
                dt.Rows.Clear();
                ViewState["customer_plans"] = dt;
            } */
            try
            {
                dtAddonPlans.Clear();
                dtAlacartePlans.Clear();
                dtBasicPlans.Clear();
                dtBasicFreePlans.Clear();
                dtFOCPlanStatus.Clear();

                dtAddonPlans.Dispose();
                dtAlacartePlans.Dispose();
                dtBasicPlans.Dispose();
                dtBasicFreePlans.Dispose();
                dtFOCPlanStatus.Dispose();

                ViewState["bucket1foc"] = "0";
                ViewState["bucket2foc"] = "0";
            }
            catch { }
        }//shri

        protected string dataTableBuilder(DataTable dt, string[] arr_data, string plan_type_flag)
        {
            string poid_list = "";
            int contvalue = 0;
            foreach (string plan_data in arr_data)
            {
                string[] plan_details_arr = plan_data.Split('$');
                string plan_poid = plan_details_arr[0];
                string plan_name = plan_details_arr[1];
                string plan_custprice = plan_details_arr[2];
                string plan_lcoprice = plan_details_arr[3];
                string plan_activation = plan_details_arr[4];
                string plan_expiry = plan_details_arr[5];
                string plan_dealpoid = plan_details_arr[6];
                string plan_package = plan_details_arr[7];
                string plan_purchase = plan_details_arr[8];
                string plan_status = plan_details_arr[9];
                string plan_renewflag = "";
                string plan_changeflag = "";
                string plan_actionflag = "";
                string Plan_Grace_Date = "";
                string plantype_flag = "";
                // created by vivek 16-nov-2015 
                plan_renewflag = plan_details_arr[10];
                plan_changeflag = plan_details_arr[11];
                plan_actionflag = plan_details_arr[12];
                Plan_Grace_Date = plan_details_arr[13];
                try
                {
                    plantype_flag = plan_details_arr[21];
                }
                catch (Exception)
                {
                    plantype_flag = plan_type_flag;
                }

                if (plan_type_flag.Trim() == "B") //if basic plans, then accept only active poids
                {

                }
                else
                { //if addon and alacarte plans, then accept all poids
                    if (plan_status.Trim() == "Active")
                    {
                        poid_list += "'" + plan_poid + "',";

                    }
                }

                DataRow tempDr = dt.NewRow();
                tempDr["PLAN_POID"] = plan_poid;
                tempDr["PLAN_NAME"] = plan_name;
                tempDr["PLAN_TYPE"] = plantype_flag;
                tempDr["DEAL_POID"] = plan_dealpoid;
                //tempDr["PRODUCT_POID"] = dr_plan[0]["PRODUCT_POID"];
                tempDr["CUST_PRICE"] = plan_custprice;
                tempDr["LCO_PRICE"] = plan_lcoprice;
                //tempDr["PAYTERM"] = dr_plan[0]["PAYTERM"];
                //tempDr["CITYID"] = city;
                //tempDr["CITY_NAME"] = dr_plan[0]["CITY_NAME"];
                //tempDr["COMPANY_CODE"] = dr_plan[0]["COMPANY_CODE"];
                //tempDr["INSDT"] = dr_plan[0]["INSDT"];
                //tempDr["INSBY"] = dr_plan[0]["INSBY"];
                tempDr["ACTIVATION"] = plan_activation;
                tempDr["EXPIRY"] = plan_expiry;
                tempDr["PACKAGE_ID"] = plan_package;
                tempDr["PURCHASE_POID"] = plan_purchase;
                tempDr["PLAN_STATUS"] = plan_status;
                // created by vivek 16-nov-2015 

                tempDr["PLAN_RENEWFLAG"] = plan_renewflag;
                tempDr["PLAN_CHANGEFLAG"] = plan_changeflag;
                tempDr["PLAN_ACTIONFLAG"] = plan_actionflag;
                tempDr["GRACE"] = Plan_Grace_Date;
                if (plan_type_flag.Trim() == "B")
                {
                    if (ViewState["discounttype"].ToString() != "P")
                    {

                        tempDr["DISCOUNT"] = "Rs. " + Convert.ToDouble(ViewState["DiscountAmount"].ToString()).ToString();
                    }
                    else
                    {
                        tempDr["DISCOUNT"] = Convert.ToDouble(ViewState["DiscountAmount"].ToString()).ToString() + " %";
                    }

                    tempDr["alacartebase"] = plan_details_arr[14];
                    tempDr["pincycle"] = plan_details_arr[15];
                    tempDr["alacartebaseprice"] = plan_details_arr[16];
                    tempDr["BD_PRICE"] = plan_details_arr[17];
                    tempDr["SD_Count"] = plan_details_arr[18];
                    tempDr["HD_Count"] = plan_details_arr[19];
                    tempDr["Total_Count"] = plan_details_arr[20];
                }
                else
                {
                    //14
                    tempDr["BD_PRICE"] = plan_details_arr[17];
                    tempDr["SD_Count"] = plan_details_arr[18];
                    tempDr["HD_Count"] = plan_details_arr[19];
                    tempDr["Total_Count"] = plan_details_arr[20];
                }



                dt.Rows.Add(tempDr);
            }
            ViewState["Countvalue"] = contvalue;
            return poid_list;
        }//shri

        protected void bindAllGrids(string service_str)
        {
            try
            {
                nullFOCViewState();
                ViewState["Changefoccheck"] = null;
                //creating plan def datatable using viewstate
                /*   DataTable dtPlanDef = new DataTable();
                   if (ViewState["plan_def"] != null)
                   {
                       dtPlanDef = (DataTable)ViewState["plan_def"];
                   } */

                //separating and manupulating plan poids
                ViewState["ServicePoid"] = service_str.Split('!')[0];

                lblStbNo.Text = service_str.Split('!')[1];
                string stb_no = service_str.Split('!')[1];

                /*string getstb = "select * from aoup_lcopre_stb_hd_pack where var_stb_no='" + stb_no + "' and var_stb_active_flag='Y'";

                DataTable tblstbdethd = GetResult(getstb);
                if (tblstbdethd.Rows.Count > 0)
                {
                    ViewState["stb_no_hd"] = "Y";
                }
                else
                {*/
                ViewState["stb_no_hd"] = "N";
                //}

                lblVCID.Text = service_str.Split('!')[2];
                ViewState["vcid"] = service_str.Split('!')[2];
                HyperLink1.NavigateUrl = "~/Reports/rptEcafCustDetails.aspx?vc=" + lblVCID.Text;
                String CheckMaintv = "";
                if (ViewState["stb_status"] != null)
                {
                    ViewState["stb_status"] = service_str.Split('!')[4];
                    ViewState["vcid"] = service_str.Split('!')[2];
                    ViewState["Device_Type"] = service_str.Split('!')[5];
                    ViewState["Last_status"] = service_str.Split('!')[7];
                    CheckMaintv = service_str.Split('!')[6].ToString();
                    // Session["TVConnection"] = service_str.Split('!')[8].ToString();// -- for TV discount Connection Type
                    //----------    for child TV discount ...
                    try
                    {
                        Session["TVConnection"] = service_str.Split('!')[8].ToString();// -- for TV discount Connection Type
                    }
                    catch (Exception ex)
                    {
                        Session["TVConnection"] = "0";
                    }
                    if (ViewState["Device_Type"].ToString().Contains("HD"))
                    {
                        Session["BoxType"] = "HD";
                    }
                    else
                    {
                        Session["BoxType"] = "SD";
                    }
                }

                //showing service status
                Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                string ServiceStatus = obj.getServiceStatus(service_str.Split('!')[4]);
                if (ServiceStatus == "A") { lbactive.Visible = false; lbdeactive.Visible = false; btnDeact.Visible = true; btnAct.Visible = false; }//changed by shrikant
                else if (ServiceStatus == "IA") { lbactive.Visible = false; lbdeactive.Visible = false; btnDeact.Visible = false; btnAct.Visible = true; }//changed by shrikant
                else if (ServiceStatus == "CL") { lbactive.Visible = false; lbdeactive.Visible = false; btnDeact.Visible = false; btnAct.Visible = false; }
                else if (ServiceStatus == "EX") { lbactive.Visible = false; lbdeactive.Visible = false; btnDeact.Visible = false; btnAct.Visible = false; }

                if (ServiceStatus == "IA")
                {
                    btnOpenAddPopup.Visible = false;
                    allrenewal.Visible = false;
                    Challrenew.Visible = false;

                }
                else
                {
                    //btnOpenAddPopup.Visible = true;
                    btnOpenAddPopup.Visible = false;
                    allrenewal.Visible = true;
                    Challrenew.Visible = true;

                }



                string DeviceDefinitionType = service_str.Split('!')[5]; //--this is device type SD / HD
                if (DeviceDefinitionType == null || DeviceDefinitionType == "")
                {
                    DeviceDefinitionType = "SD";
                }

                ViewState["DeviceDefinitionType"] = DeviceDefinitionType;

                string all_plan_string = service_str.Split('!')[3]; //--this is plan string under service

                string city = "";
                if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
                {
                    city = ViewState["cityid"].ToString();
                }
                string service_data = obj.getCancelServiceDataBL(Session["username"].ToString(), city, all_plan_string, ViewState["customer_no"].ToString());
                //"9999#4223442545$MP PRIME SP 1M$300$105$16-Dec-2016$15-Jan-2017$0.0.0.1 /deal 4223440177 4$104817663$0.0.0.1 /purchased_product 6622635011 8$Expired$Y$SY$EX$NA$$Y~4223442545$MP PRIME SP 1M$300$105$27-Apr-2016$30-Nov-2016$0.0.0.1 /deal 4223440177 4$86187110$0.0.0.1 /purchased_product 4448256640 36$Expired$Y$SY$EX$12-Dec-2016$$N###";//
                string[] service_data_arr = service_data.Split('#');
                if (service_data_arr[0] != "9999")
                {
                    //show only customer information but hide plan details
                    setSearchBox();
                    Detailss.Visible = true;
                    pnlGridHolder.Visible = false;
                    btnReset.Visible = true;
                    lblSearchResponse.Text = "";
                    btnRefreshForm.Visible = false;
                    // needed to add other if condition
                    if (service_data_arr[0] == "-342")
                    {
                        resetSearchBox();
                        Detailss.Visible = false;
                        pnlGridHolder.Visible = false;
                        btnReset.Visible = false;
                        resetAllGrids();
                        msgboxstr("Account not mapped to logged in user");
                        return;
                    }
                    else
                    {
                        msgboxstr("Something went wrong while fetching plan details...");
                        return;
                    }
                }
                else
                {
                    //show only customer information as well as plan details
                    setSearchBox();
                    Detailss.Visible = true;
                    pnlGridHolder.Visible = true;
                    btnReset.Visible = true;
                    lblSearchResponse.Text = "";
                    if (hdntag.Value == "lnkDetail")
                    {
                        dynamiclink.Visible = true;
                        lnkatag_Click(null, null);
                    }
                }
                GetDiscountofLCO();

                //generating basic table
                string basic_data_str = service_data_arr[1];
                string basic_ActivePlan = service_data_arr[7];
                Session["basic_ActivePlan"] = basic_ActivePlan;
                if (basic_ActivePlan.Split('$').Length > 1)
                {
                    ViewState["ActivePOIDs"] = basic_ActivePlan.Split('$')[0];
                    ViewState["BasicExpiry"] = basic_ActivePlan.Split('$')[1];
                }
                else
                {
                    ViewState["ActivePOIDs"] = basic_ActivePlan.Split('$')[0];
                    ViewState["BasicExpiry"] ="";
                }
                    ViewState["BasicPoid"] = "";
                string basic_poids = "";
                if (basic_data_str != null && basic_data_str != "")
                {
                    //DataTable dtBasicPlans = (DataTable)ViewState["customer_basic_plans"];
                    string[] basic_plan_arr = basic_data_str.Split('~');
                    basic_poids = dataTableBuilder(dtBasicPlans, basic_plan_arr, "B");
                    basic_poids = basic_poids.TrimEnd(',');
                    if (basic_poids == "")
                    {
                        basic_poids = "'0'";
                    }
                }
                else
                {
                    basic_poids = "'0'";
                }

                foreach (DataRow dr in dtBasicPlans.Rows)
                {
                    dr["datevalue"] = Convert.ToDateTime(dr["expiry"]);
                }
                ViewState["basic_poids"] = basic_poids;

                if (dtBasicPlans.Rows.Count > 0)
                {
                    DataView dv = dtBasicPlans.DefaultView;
                    dv.Sort = "dateValue desc";
                    DataTable sortedDT = dv.ToTable();

                    dtBasicPlans.Rows.Clear();



                    DataView dv1 = new DataView(sortedDT);
                    dv1.RowFilter = " plan_type='B'";
                    DataTable sortedDTfinalNew = dv1.ToTable();
                    if (sortedDTfinalNew.Rows.Count > 0)
                    {
                        DataView dv2 = new DataView(sortedDTfinalNew);
                        dv2.RowFilter = "expiry='" + sortedDTfinalNew.Rows[0]["expiry"].ToString() + "' ";
                        DataTable sortedDTfinal = dv2.ToTable();

                        if (sortedDTfinal.Rows[0]["plan_status"].ToString().ToUpper() == "EXPIRED")
                        {
                            dtBasicPlans = sortedDT;
                        }
                        /*else
                        {
                            if (sortedDTfinal.Rows.Count >= 1)
                            {
                                foreach (DataRow dr in sortedDTfinal.Rows)
                                {
                                    dtBasicPlans.ImportRow(dr);
                                }
                            }
                        }*/
                    }
                }
                if (dtBasicPlans.Rows.Count > 0)
                {
                    /*if (dtBasicPlans.Rows[0]["pincycle"].ToString() == "N")
                    {
                        dtBasicPlans.Rows.Clear();
                    }*/
                }

                // ViewState["basic_poids"] = basic_poids;
                //generating addon table
                string addon_data_str = service_data_arr[2];
                string addon_poids = "";

                if (addon_data_str != null && addon_data_str != "")
                {
                    //DataTable dtAddonPlans = (DataTable)ViewState["customer_addon_plans"];
                    string[] addon_plan_arr = addon_data_str.Split('~');
                    addon_poids = dataTableBuilder(dtAddonPlans, addon_plan_arr, "AD");
                    addon_poids = addon_poids.TrimEnd(',');

                    //poidlist = addon_poids;

                    //Validation rule : If service has addon pachages then user cannot activate/deactivate the service.-------------------------VALIDATION RULE
                    //btnDeact.Visible = false;
                    //btnAct.Visible = false;
                }
                addon_data_str = service_data_arr[3];
                if (addon_data_str != null && addon_data_str != "")
                {
                    //DataTable dtAddonPlans = (DataTable)ViewState["customer_addon_plans"];
                    string[] addon_plan_arr = addon_data_str.Split('~');
                    if (addon_poids != "")
                    {
                        addon_poids = addon_poids + ",";
                    }
                    addon_poids += dataTableBuilder(dtAddonPlansreg, addon_plan_arr, "AD");

                    addon_poids = addon_poids.TrimEnd(',');
                    if (addon_poids == "")
                    {
                        addon_poids = "'0'";
                    }
                }

                if (addon_poids == "")
                {
                    addon_poids = "'0'";
                }
                ViewState["addon_poids"] = addon_poids;

                /*if (dtAddonPlans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAddonPlans.Rows)
                    {
                        dr["datevalue"] = Convert.ToDateTime(dr["expiry"]);
                    }
                    DataView dvAD = dtAddonPlans.DefaultView;
                    dvAD.Sort = "dateValue desc";
                    DataTable sortedADDT = dvAD.ToTable();
                    dtAddonPlans.Rows.Clear();
                    /*dtAddonPlansreg.Rows.Clear();
                    DataView dvAL2 = new DataView(sortedADDT);
                    dvAL2.RowFilter = "expiry='" + sortedADDT.Rows[0]["expiry"].ToString() + "' ";
                    DataTable sortedALDTfinal2 = dvAD2.ToTable();
                    
                    if (sortedADDT.Rows[0]["plan_status"].ToString().ToUpper() == "EXPIRED")
                    {
                        dtAddonPlans = sortedADDT;
                    }
                }*/

                //generating Hathway Special table
                string hatwayspecial_data_str = service_data_arr[5];
                string hathspecial_poids = "";
                if (hatwayspecial_data_str != null && hatwayspecial_data_str != "")
                {
                    // DataTable dtAlacartePlans = (DataTable)ViewState["customer_alacarte_plans"];
                    string[] hathspecial_plan_arr = hatwayspecial_data_str.Split('~');
                    hathspecial_poids = dataTableBuilder(dthathwayspecial, hathspecial_plan_arr, "HSP");
                    hathspecial_poids = hathspecial_poids.TrimEnd(',');
                    if (hathspecial_poids == "")
                    {
                        hathspecial_poids = "'0'";
                    }
                }
                else
                {
                    hathspecial_poids = "'0'";
                }
                ViewState["hwayspecial_poid"] = hathspecial_poids;


                /*if (dthathwayspecial.Rows.Count > 0)
                {
                    foreach (DataRow dr in dthathwayspecial.Rows)
                    {
                        dr["datevalue"] = Convert.ToDateTime(dr["expiry"]);
                    }
                    DataView dvHSP = dthathwayspecial.DefaultView;
                    dvHSP.Sort = "dateValue desc";
                    DataTable sortedHSPDT = dvHSP.ToTable();

                    dthathwayspecial.Rows.Clear();
                    DataView dvHSP1 = new DataView(sortedHSPDT);
                    dvHSP1.RowFilter = " plan_type='HSP'";
                    DataTable sortedHSPDTfinal = dvHSP1.ToTable();
                    if (sortedHSPDTfinal.Rows.Count > 0)
                    {
                        DataView dvHSP2 = new DataView(sortedHSPDTfinal);
                        dvHSP2.RowFilter = "expiry='" + sortedHSPDTfinal.Rows[0]["expiry"].ToString() + "' ";
                        DataTable sortedHSPDTfinal2 = dvHSP2.ToTable();

                        if (sortedHSPDT.Rows[0]["plan_status"].ToString().ToUpper() == "EXPIRED")
                        {
                            dthathwayspecial = sortedHSPDT;
                        }
                        else
                        {
                            if (sortedHSPDTfinal2.Rows.Count >= 1)
                            {
                                dthathwayspecial.ImportRow(sortedHSPDTfinal2.Rows[0]);
                            }
                        }
                    }
                }*/

                //generating alacarte free table
                string alacartefree_data_str = service_data_arr[6];
                string alafree_poids = "";
                if (alacartefree_data_str != null && alacartefree_data_str != "")
                {
                    // DataTable dtAlacartePlans = (DataTable)ViewState["customer_alacarte_plans"];
                    string[] alacartefree_plan_arr = alacartefree_data_str.Split('~');
                    alafree_poids = dataTableBuilder(dtAlacartePlansFree, alacartefree_plan_arr, "AL");
                    alafree_poids = alafree_poids.TrimEnd(',');
                    if (alafree_poids == "")
                    {
                        alafree_poids = "'0'";
                    }
                }
                else
                {
                    alafree_poids = "'0'";
                }
                ViewState["alafree_poids"] = alafree_poids;


                //generating alacarte table
                string alacarte_data_str = service_data_arr[4];
                string ala_poids = "";
                if (alacarte_data_str != null && alacarte_data_str != "")
                {
                    // DataTable dtAlacartePlans = (DataTable)ViewState["customer_alacarte_plans"];
                    string[] alacarte_plan_arr = alacarte_data_str.Split('~');
                    ala_poids = dataTableBuilder(dtAlacartePlans, alacarte_plan_arr, "AL");
                    ala_poids = ala_poids.TrimEnd(',');
                    if (ala_poids == "")
                    {
                        ala_poids = "'0'";
                    }
                }
                else
                {
                    ala_poids = "'0'";
                }
                ViewState["ala_poids"] = ala_poids;

                /*if (dtAlacartePlans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAlacartePlans.Rows)
                    {
                        dr["datevalue"] = Convert.ToDateTime(dr["expiry"]);
                    }
                    DataView dvAL = dtAlacartePlans.DefaultView;
                    dvAL.Sort = "dateValue desc";
                    DataTable sortedALDT = dvAL.ToTable();
                    dtAlacartePlans.Rows.Clear();
                    /*dtAlacartePlans.Rows.Clear();
                    DataView dvAL2 = new DataView(sortedALDT);
                    dvAL2.RowFilter = "expiry='" + sortedALDT.Rows[0]["expiry"].ToString() + "' ";
                        DataTable sortedALDTfinal2 = dvAL2.ToTable();
                    
                    if (sortedALDT.Rows[0]["plan_status"].ToString().ToUpper() == "EXPIRED")
                        {
                            dtAlacartePlans = sortedALDT;
                        }
                }*/
                Session["alacarte"] = dtAlacartePlans;


                string statusglobelvalue = "N";
                string strAutorenewalOn = "On";
                Cls_Business_TxnAssignPlan objAuto = new Cls_Business_TxnAssignPlan();
                statusglobelvalue = obj.autorenewstatuslco(username);
                if (statusglobelvalue.ToString() == "Y")
                {
                    strAutorenewalOn = "Global On";
                }
                else
                {
                    strAutorenewalOn = "On";
                }


                grdCartefree.DataSource = dtAlacartePlansFree;
                grdCartefree.DataBind();
                if (dtAlacartePlansFree.Rows.Count == 0)
                {
                    //make accordian and label invisible
                    lblAlacartePlanFree.Visible = false;
                    AlacarteAccordionFree.Visible = false;
                }
                else
                {
                    //make accordian and label visible
                    dynamiclink.Visible = true;
                    if (hdntag.Value == "lnkDetail")
                    {
                        lnkatag_Click(null, null);
                    }

                    lblAlacartePlanFree.Visible = true;
                    AlacarteAccordionFree.Visible = true;
                    AlacarteAccordionFree.SelectedIndex = -1;
                }

                bool AutoRenewAlaCartefree;
                double alacartevalue = 0;
                for (int i = 0; i < grdCartefree.Rows.Count; i++)
                {
                    string planname = "";
                    planname = grdCartefree.Rows[i].Cells[0].Text.ToString();
                    HiddenField hdnALPlanPoid = (HiddenField)grdCartefree.Rows[i].Cells[8].FindControl("hdnALPlanPoid");
                    String PlanPoid = hdnALPlanPoid.Value.Trim();
                    AutoRenewAlaCartefree = AutoRenewstatus(ViewState["vcid"].ToString(), ViewState["customer_no"].ToString(), PlanPoid.ToString());
                    alacartevalue += Convert.ToDouble(grdCartefree.Rows[i].Cells[1].Text.ToString());
                    if (AutoRenewAlaCartefree == true)
                    {
                        //((CheckBox)(grdCarte.Rows[i].FindControl("cbAlaAutorenew"))).Checked = true;
                        //((CheckBox)(grdCarte.Rows[i].FindControl("cbAlaAutorenew"))).BackColor = System.Drawing.ColorTranslator.FromHtml("#C5FFD8");

                        grdCartefree.Rows[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#ffe6e6");
                        ((CheckBox)(grdCartefree.Rows[i].FindControl("cbAlaAutorenew"))).Checked = true;
                        ((Label)(grdCartefree.Rows[i].FindControl("lblAutorenew"))).Text = strAutorenewalOn;

                    }
                    else
                    {
                        // ((CheckBox)(grdCarte.Rows[i].FindControl("cbAlaAutorenew"))).Checked = false;
                        ((CheckBox)(grdCartefree.Rows[i].FindControl("cbAlaAutorenew"))).Checked = false;
                        ((Label)(grdCartefree.Rows[i].FindControl("lblAutorenew"))).Text = "Off";
                    }

                }

                ViewState["alacartevalue"] = alacartevalue;

                grdCarte.DataSource = dtAlacartePlans;
                grdCarte.DataBind();
                if (dtAlacartePlans.Rows.Count == 0)
                {
                    //make accordian and label invisible
                    lblAlacartePlan.Visible = false;
                    AlacarteAccordion.Visible = false;
                }
                else
                {
                    //make accordian and label visible
                    dynamiclink.Visible = true;
                    if (hdntag.Value == "lnkDetail")
                    {
                        lnkatag_Click(null, null);
                    }

                    lblAlacartePlan.Visible = true;
                    AlacarteAccordion.Visible = true;
                    AlacarteAccordion.SelectedIndex = -1;
                }

                bool AutoRenewAlaCarte;

                for (int i = 0; i < grdCarte.Rows.Count; i++)
                {
                    string planname = "";
                    planname = grdCarte.Rows[i].Cells[0].Text.ToString();
                    HiddenField hdnALPlanPoid = (HiddenField)grdCarte.Rows[i].Cells[8].FindControl("hdnALPlanPoid");
                    String PlanPoid = hdnALPlanPoid.Value.Trim();
                    AutoRenewAlaCarte = AutoRenewstatus(ViewState["vcid"].ToString(), ViewState["customer_no"].ToString(), PlanPoid.ToString());
                    if (AutoRenewAlaCarte == true)
                    {
                        //((CheckBox)(grdCarte.Rows[i].FindControl("cbAlaAutorenew"))).Checked = true;
                        //((CheckBox)(grdCarte.Rows[i].FindControl("cbAlaAutorenew"))).BackColor = System.Drawing.ColorTranslator.FromHtml("#C5FFD8");

                        grdCarte.Rows[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#ffe6e6");
                        ((CheckBox)(grdCarte.Rows[i].FindControl("cbAlaAutorenew"))).Checked = true;
                        ((Label)(grdCarte.Rows[i].FindControl("lblAutorenew"))).Text = strAutorenewalOn;

                    }
                    else
                    {
                        // ((CheckBox)(grdCarte.Rows[i].FindControl("cbAlaAutorenew"))).Checked = false;
                        ((CheckBox)(grdCarte.Rows[i].FindControl("cbAlaAutorenew"))).Checked = false;
                        ((Label)(grdCarte.Rows[i].FindControl("lblAutorenew"))).Text = "Off";
                    }
                    Button lnkaddplanbaseexpired = (Button)grdCarte.Rows[i].Cells[8].FindControl("lnkALaddplanbaseexpired");
                    Button lnkBRenew = (Button)grdCarte.Rows[i].Cells[8].FindControl("lbALRenewal");
                    Button lnkBChange = (Button)grdCarte.Rows[i].Cells[8].FindControl("lbALChange");
                    Button lnkBCancel = (Button)grdCarte.Rows[i].Cells[8].FindControl("lbALCancel");

                    if (grdCarte.Rows[i].Cells[11].Text.ToString().ToString().ToUpper() == "EXPIRED")
                    {
                        grdCarte.Rows[i].Font.Bold = true;
                        grdCarte.Rows[i].Cells[0].ForeColor = System.Drawing.Color.Red;
                        grdCarte.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Red;
                        grdCarte.Rows[i].Cells[2].ForeColor = System.Drawing.Color.Red;
                        grdCarte.Rows[i].Cells[3].ForeColor = System.Drawing.Color.Red;
                        grdCarte.Rows[i].Cells[4].ForeColor = System.Drawing.Color.Red;
                        grdCarte.Rows[i].Cells[5].ForeColor = System.Drawing.Color.Red;
                        grdCarte.Rows[i].Cells[6].ForeColor = System.Drawing.Color.Red;
                        grdCarte.Rows[i].Cells[7].ForeColor = System.Drawing.Color.Red;
                        grdCarte.Rows[i].Cells[9].ForeColor = System.Drawing.Color.Red;
                        lnkaddplanbaseexpired.Visible = true;
                        //lnkaddplanbaseexpired.Visible = false;
                        lnkBRenew.Visible = false;
                        lnkBChange.Visible = false;
                        lnkBCancel.Visible = false;
                        radPlanBasic.Checked = true;
                    }
                    else
                    {
                        lnkaddplanbaseexpired.Visible = false;
                        lnkBRenew.Visible = true;
                        lnkBChange.Visible = true;
                        lnkBCancel.Visible = true;
                        radPlanBasic.Checked = false;
                    }
                } // done by vivek 2015-06-12

                Session["basic"] = dtBasicPlans;
                ViewState["dtBasicPlans"] = dtBasicPlans;
                grdBasicPlanDetails.DataSource = dtBasicPlans;
                grdBasicPlanDetails.DataBind();
                if (CheckMaintv == "0")
                {
                    ViewState["CheckMaintvBasicCount"] = dtBasicPlans.Rows.Count;
                }
                if (CheckMaintv == "1")
                {
                    ViewState["CheckchildtvBasicCount"] = "YES";
                }
                else
                {
                    ViewState["CheckchildtvBasicCount"] = "NO";
                }




                if (dtBasicPlans.Rows.Count == 0)
                {
                    lblBasicPlan.Visible = false;
                    ViewState["BasicActionFlag"] = null;
                }
                else
                {
                    dynamiclink.Visible = true;

                    if (hdntag.Value == "lnkDetail")
                    {
                        lnkatag_Click(null, null);
                    }
                    HiddenField hdnBasicActionFlag = (HiddenField)grdBasicPlanDetails.Rows[0].FindControl("hdnBasicActionFlag");  // created by vivek 16-nov-2015 
                    ViewState["BasicActionFlag"] = hdnBasicActionFlag.Value;
                    lblBasicPlan.Visible = true;
                }

                bool AutoRenewAlBasic;
                ViewState["Alacartebasevalue"] = "0";
                ViewState["alacartebaseplan"] = "N";
                for (int i = 0; i < grdBasicPlanDetails.Rows.Count; i++)
                {
                    string planname = "";
                    Button lnkAddFOCPack = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkAddFOCPack");
                    Button btnALPack = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("btnALPack");

                    planname = grdBasicPlanDetails.Rows[i].Cells[0].Text.ToString();
                    HiddenField hdnBasicPlanPoid = (HiddenField)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("hdnBasicPlanPoid");
                    ViewState["basic_poids"] = ((HiddenField)(grdBasicPlanDetails.Rows[i].FindControl("hdnBasicPlanPoid"))).Value.Trim();
                    String PlanPoid = hdnBasicPlanPoid.Value.Trim();
                    AutoRenewAlBasic = AutoRenewstatus(ViewState["vcid"].ToString(), ViewState["customer_no"].ToString(), PlanPoid.ToString());
                    if (AutoRenewAlBasic == true)
                    {
                        //grdBasicPlanDetails.Rows[i].Cells[0].Text = grdBasicPlanDetails.Rows[i].Cells[0].Text + "(Autorenewal On)";
                        grdBasicPlanDetails.Rows[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#ffe6e6");
                        ((CheckBox)(grdBasicPlanDetails.Rows[i].FindControl("cbBAutorenew"))).Checked = true;
                        ((Label)(grdBasicPlanDetails.Rows[i].FindControl("lblAutorenew"))).Text = strAutorenewalOn;

                        // ((CheckBox)(grdBasicPlanDetails.Rows[i].FindControl("cbBAutorenew"))).BackColor = System.Drawing.ColorTranslator.FromHtml("#C5FFD8");
                    }
                    else
                    {
                        ((CheckBox)(grdBasicPlanDetails.Rows[i].FindControl("cbBAutorenew"))).Checked = false;
                        ((Label)(grdBasicPlanDetails.Rows[i].FindControl("lblAutorenew"))).Text = "Off";
                    }
                    Button lnkaddplanbaseexpired = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkaddplanbaseexpired");
                    Button lnkBRenew = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkBRenew");
                    Button lnkBChange = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkBChange");
                    Button lnkBCancel = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkBCancel");

                    if (grdBasicPlanDetails.Rows[i].Cells[11].Text.ToString().ToString().ToUpper() == "EXPIRED")
                    {
                        grdBasicPlanDetails.Rows[i].Font.Bold = true;
                        grdBasicPlanDetails.Rows[i].Cells[0].ForeColor = System.Drawing.Color.Red;
                        grdBasicPlanDetails.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Red;
                        grdBasicPlanDetails.Rows[i].Cells[2].ForeColor = System.Drawing.Color.Red;
                        grdBasicPlanDetails.Rows[i].Cells[3].ForeColor = System.Drawing.Color.Red;
                        grdBasicPlanDetails.Rows[i].Cells[4].ForeColor = System.Drawing.Color.Red;
                        grdBasicPlanDetails.Rows[i].Cells[5].ForeColor = System.Drawing.Color.Red;
                        grdBasicPlanDetails.Rows[i].Cells[6].ForeColor = System.Drawing.Color.Red;
                        grdBasicPlanDetails.Rows[i].Cells[7].ForeColor = System.Drawing.Color.Red;
                        grdBasicPlanDetails.Rows[i].Cells[9].ForeColor = System.Drawing.Color.Red;
                        lnkaddplanbaseexpired.Visible = true;
                        //lnkaddplanbaseexpired.Visible = false;
                        lnkBRenew.Visible = false;
                        lnkBChange.Visible = false;
                        lnkBCancel.Visible = false;
                        lnkAddFOCPack.Visible = false;
                        ViewState["BasepalnExpired"] = true;
                        radPlanBasic.Checked = true;
                    }
                    else
                    {
                        lnkaddplanbaseexpired.Visible = false;
                        lnkBRenew.Visible = true;
                        lnkBChange.Visible = true;
                        //lnkBChange.Visible = false;
                        lnkBCancel.Visible = true;
                        //lnkAddFOCPack.Visible = true;
                        lnkAddFOCPack.Visible = false;
                        ViewState["BasepalnExpired"] = false;
                        radPlanBasic.Checked = false;
                    }
                    //FileLogTextChange1(((HiddenField)(grdBasicPlanDetails.Rows[i].FindControl("hdnBasicPlanType"))).Value.Trim(), " hdnBasicPlanType", "", "");
                    if (((HiddenField)(grdBasicPlanDetails.Rows[i].FindControl("hdnBasicPlanType"))).Value.Trim() == "NCF")
                    {
                        lnkaddplanbaseexpired.Visible = false;
                        lnkBRenew.Visible = false;
                        lnkBChange.Visible = false;
                        lnkBCancel.Visible = false;
                    }
                    /*else
                    {
                        lnkBRenew.Visible = true;
                        lnkBChange.Visible = false;
                        lnkBCancel.Visible = true;
                    }*/
                    if (((HiddenField)(grdBasicPlanDetails.Rows[i].FindControl("hdnbasicalacartebase"))).Value.Trim() == "Y")
                    {

                        ViewState["alacartebaseplan"] = ((HiddenField)(grdBasicPlanDetails.Rows[i].FindControl("hdnbasicalacartebase"))).Value.Trim();
                        if (ViewState["alacartebaseplan"] == "N")
                        {
                            ViewState["Alacartebasevalue"] = Convert.ToDouble(grdBasicPlanDetails.Rows[i].Cells[1].Text.ToString());
                        }
                        else
                        {
                            ViewState["Alacartebasevalue"] = Convert.ToDouble(((HiddenField)(grdBasicPlanDetails.Rows[i].FindControl("hdnbasicalacartebaseprice"))).Value.Trim());
                        }

                        radPlanAD.Enabled = false;
                        radPlanADreg.Enabled = false;
                        radhwayspecial.Enabled = false;
                        lnkAddFOCPack.Visible = false;
                        tr1.Visible = false;
                        btnALPack.Visible = false;
                        lnkAddFOCPack.Visible = false;
                    }
                    else
                    {
                        radPlanAD.Enabled = true;
                        radPlanADreg.Enabled = true;
                        radhwayspecial.Enabled = false;
                        tr1.Visible = true;
                        btnALPack.Visible = false;
                        lnkAddFOCPack.Visible = false;
                    }
                }



                /*try
                {
                    if (grdBasicPlanDetails.Rows.Count == 0 &&Grdhathwayspecial.Rows.Count==0)
                    {
                        radPlanAD.Enabled = false;
                        radPlanAL.Enabled = false;
                        btnAutoRenewal.Visible = false;
                    }
                    else
                    {
                        DateTime dtexpiry=new DateTime();
                        if (grdBasicPlanDetails.Rows.Count > 0)
                        {
                             dtexpiry = Convert.ToDateTime(dtBasicPlans.Rows[0]["EXPIRY"]);
                        }
                        else if (Grdhathwayspecial.Rows.Count > 0)
                        {
                            dtexpiry = Convert.ToDateTime(dthathwayspecial.Rows[0]["EXPIRY"]);
                        }
                        if (dtexpiry < DateTime.Now && Convert.ToString( dtexpiry) != "31-DEC-69" && Convert.ToString(dtexpiry) != "30-DEC-30")
                        {
                            // btnOpenAddPopup.Visible = false;
                            radPlanAD.Enabled = false;
                            radPlanAL.Enabled = false;
                            btnAutoRenewal.Visible = false;
                        }
                        else
                        {
                            // btnOpenAddPopup.Visible = true;
                            radPlanAD.Enabled = true;
                            radPlanAL.Enabled = true;
                            //btnAutoRenewal.Visible = true;
                            btnAutoRenewal.Visible = true;
                        }

                    }
                }
                catch (Exception ex)
                {
                }*/


                Session["hathspecial"] = dthathwayspecial;
                Grdhathwayspecial.DataSource = dthathwayspecial;
                Grdhathwayspecial.DataBind();
                if (dthathwayspecial.Rows.Count == 0)
                {
                    //make accordian and label invisible
                    lblhathwayspecial.Visible = false;
                }
                else
                {
                    dynamiclink.Visible = true;
                    //make accordian and label invisible

                    if (hdntag.Value == "lnkDetail")
                    {
                        lnkatag_Click(null, null);
                    }

                    lblhathwayspecial.Visible = true;
                }

                bool AutoRenewhathspecial;

                for (int i = 0; i < Grdhathwayspecial.Rows.Count; i++)
                {
                    string planname = "";
                    planname = Grdhathwayspecial.Rows[i].Cells[0].Text.ToString();
                    HiddenField hdnADPlanPoid = (HiddenField)Grdhathwayspecial.Rows[i].Cells[8].FindControl("hdnADPlanPoid");
                    string PlanPoid = hdnADPlanPoid.Value.Trim();
                    AutoRenewhathspecial = AutoRenewstatus(ViewState["vcid"].ToString(), ViewState["customer_no"].ToString(), PlanPoid.ToString());
                    if (AutoRenewhathspecial == true)
                    {
                        Grdhathwayspecial.Rows[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#ffe6e6");
                        ((CheckBox)(Grdhathwayspecial.Rows[i].FindControl("cbAddonAutorenew"))).Checked = true;
                        ((Label)(Grdhathwayspecial.Rows[i].FindControl("lblAutorenew"))).Text = strAutorenewalOn;

                    }
                    else
                    {
                        ((CheckBox)(Grdhathwayspecial.Rows[i].FindControl("cbAddonAutorenew"))).Checked = false;
                        ((Label)(Grdhathwayspecial.Rows[i].FindControl("lblAutorenew"))).Text = "Off";
                    }
                    Button lnkADaddplanbaseexpired = (Button)Grdhathwayspecial.Rows[i].Cells[8].FindControl("lnkHSPaddplanbaseexpired");
                    Button lnkADRenew = (Button)Grdhathwayspecial.Rows[i].Cells[8].FindControl("lnkADRenew");
                    Button lnkADChange = (Button)Grdhathwayspecial.Rows[i].Cells[8].FindControl("lnkADChange");
                    Button lnkADCancel = (Button)Grdhathwayspecial.Rows[i].Cells[8].FindControl("lnkADCancel");
                    if (Grdhathwayspecial.Rows[i].Cells[11].Text.ToString().ToString().ToUpper() == "EXPIRED")
                    {
                        Grdhathwayspecial.Rows[i].Font.Bold = true;
                        Grdhathwayspecial.Rows[i].Cells[0].ForeColor = System.Drawing.Color.Red;
                        Grdhathwayspecial.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Red;
                        Grdhathwayspecial.Rows[i].Cells[2].ForeColor = System.Drawing.Color.Red;
                        Grdhathwayspecial.Rows[i].Cells[3].ForeColor = System.Drawing.Color.Red;
                        Grdhathwayspecial.Rows[i].Cells[4].ForeColor = System.Drawing.Color.Red;
                        Grdhathwayspecial.Rows[i].Cells[5].ForeColor = System.Drawing.Color.Red;
                        Grdhathwayspecial.Rows[i].Cells[6].ForeColor = System.Drawing.Color.Red;
                        Grdhathwayspecial.Rows[i].Cells[7].ForeColor = System.Drawing.Color.Red;
                        Grdhathwayspecial.Rows[i].Cells[9].ForeColor = System.Drawing.Color.Red;
                        lnkADaddplanbaseexpired.Visible = true;
                        //lnkaddplanbaseexpired.Visible = false;
                        lnkADRenew.Visible = false;
                        lnkADChange.Visible = false;
                        lnkADCancel.Visible = false;
                        ViewState["HSPpalnExpired"] = true;
                        radhwayspecial.Checked = true;
                    }
                    else
                    {
                        lnkADaddplanbaseexpired.Visible = false;
                        lnkADRenew.Visible = true;
                        lnkADChange.Visible = true;
                        //lnkBChange.Visible = false;
                        lnkADCancel.Visible = true;
                        ViewState["HSPpalnExpired"] = false;
                        radhwayspecial.Checked = false;
                    }

                }


                DataTable dtAddonPlansAll = dtAddonPlans.Copy();
                dtAddonPlansAll.Merge(dtAddonPlansreg);

                Session["addon"] = dtAddonPlansAll;
                grdAddOnPlan.DataSource = dtAddonPlans;
                grdAddOnPlan.DataBind();
                if (dtAddonPlans.Rows.Count == 0)
                {
                    //make accordian and label invisible
                    lblAddonPlan.Visible = false;
                    AddonAccordion.Visible = false;
                }
                else
                {
                    dynamiclink.Visible = true;
                    //make accordian and label invisible

                    if (hdntag.Value == "lnkDetail")
                    {
                        lnkatag_Click(null, null);
                    }

                    lblAddonPlan.Visible = true;
                    AddonAccordion.Visible = true;
                    AddonAccordion.SelectedIndex = -1;
                }

                bool AutoRenewAddon;

                for (int i = 0; i < grdAddOnPlan.Rows.Count; i++)
                {
                    string planname = "";
                    planname = grdAddOnPlan.Rows[i].Cells[0].Text.ToString();
                    HiddenField hdnADPlanPoid = (HiddenField)grdAddOnPlan.Rows[i].Cells[8].FindControl("hdnADPlanPoid");
                    string PlanPoid = hdnADPlanPoid.Value.Trim();
                    AutoRenewAddon = AutoRenewstatus(ViewState["vcid"].ToString(), ViewState["customer_no"].ToString(), PlanPoid.ToString());
                    if (AutoRenewAddon == true)
                    {
                        grdAddOnPlan.Rows[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#ffe6e6");
                        ((CheckBox)(grdAddOnPlan.Rows[i].FindControl("cbAddonAutorenew"))).Checked = true;
                        ((Label)(grdAddOnPlan.Rows[i].FindControl("lblAutorenew"))).Text = strAutorenewalOn;
                        //((CheckBox)(grdAddOnPlan.Rows[i].FindControl("cbAddonAutorenew"))).Checked = true;
                        //((CheckBox)(grdCarte.Rows[i].FindControl("cbAddonAutorenew"))).BackColor = System.Drawing.ColorTranslator.FromHtml("#C5FFD8");

                    }
                    else
                    {
                        ((CheckBox)(grdAddOnPlan.Rows[i].FindControl("cbAddonAutorenew"))).Checked = false;
                        ((Label)(grdAddOnPlan.Rows[i].FindControl("lblAutorenew"))).Text = "Off";
                    }
                    Button lnkaddplanbaseexpired = (Button)grdAddOnPlan.Rows[i].Cells[8].FindControl("lnkADaddplanbaseexpired");
                    Button lnkBRenew = (Button)grdAddOnPlan.Rows[i].Cells[8].FindControl("lnkADRenew");
                    Button lnkBChange = (Button)grdAddOnPlan.Rows[i].Cells[8].FindControl("lnkADChange");
                    Button lnkBCancel = (Button)grdAddOnPlan.Rows[i].Cells[8].FindControl("lnkADCancel");

                    if (grdAddOnPlan.Rows[i].Cells[11].Text.ToString().ToString().ToUpper() == "EXPIRED")
                    {
                        grdAddOnPlan.Rows[i].Font.Bold = true;
                        grdAddOnPlan.Rows[i].Cells[0].ForeColor = System.Drawing.Color.Red;
                        grdAddOnPlan.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Red;
                        grdAddOnPlan.Rows[i].Cells[2].ForeColor = System.Drawing.Color.Red;
                        grdAddOnPlan.Rows[i].Cells[3].ForeColor = System.Drawing.Color.Red;
                        grdAddOnPlan.Rows[i].Cells[4].ForeColor = System.Drawing.Color.Red;
                        grdAddOnPlan.Rows[i].Cells[5].ForeColor = System.Drawing.Color.Red;
                        grdAddOnPlan.Rows[i].Cells[6].ForeColor = System.Drawing.Color.Red;
                        grdAddOnPlan.Rows[i].Cells[7].ForeColor = System.Drawing.Color.Red;
                        grdAddOnPlan.Rows[i].Cells[9].ForeColor = System.Drawing.Color.Red;
                        lnkaddplanbaseexpired.Visible = true;
                        //lnkaddplanbaseexpired.Visible = false;
                        lnkBRenew.Visible = false;
                        lnkBChange.Visible = false;
                        lnkBCancel.Visible = false;
                        radPlanBasic.Checked = true;
                    }
                    else
                    {
                        lnkaddplanbaseexpired.Visible = false;
                        lnkBRenew.Visible = true;
                        lnkBChange.Visible = true;
                        lnkBCancel.Visible = true;
                        radPlanBasic.Checked = false;
                    }
                }


                grdAddOnPlanReg.DataSource = dtAddonPlansreg;
                grdAddOnPlanReg.DataBind();
                if (dtAddonPlansreg.Rows.Count == 0)
                {
                    //make accordian and label invisible
                    lblAddonPlanReg.Visible = false;
                    AddonAccordionReg.Visible = false;
                }
                else
                {
                    dynamiclink.Visible = true;
                    //make accordian and label invisible

                    if (hdntag.Value == "lnkDetail")
                    {
                        lnkatag_Click(null, null);
                    }

                    lblAddonPlanReg.Visible = false;
                    AddonAccordionReg.Visible = false;
                    AddonAccordionReg.SelectedIndex = -1;
                }



                //FileLogTextChange("ServiceStatus", "enter ", " Error:" + Request, "");

                //if (lblBasicPlan.Visible == true||lblhathwayspecial.Visible==true)
                //{
                BtnRetract.Visible = true;

                //}
                //else
                //{
                //    BtnRetract.Visible = false;
                //}



                /*if (lblBasicPlan.Visible == true || lblhathwayspecial.Visible == true || lblAddonPlan.Visible == true || lblAlacartePlan.Visible == true)
                {
                    btnCustomerReceipt.Visible = true;
                    btnAutoRenewal.Visible = true;
                }
                else
                {
                    btnCustomerReceipt.Visible = false;
                    btnAutoRenewal.Visible = false;
                }*/

                if (ServiceStatus == "A")
                {
                    //FileLogTextChange("ServiceStatus", "active ", " Error:" + Request, "");
                    //divgriddet.Visible = true;
                    dynamiclink.Visible = true;
                    pnlGridHolder.Visible = true;
                    // Detailss.Visible = true;
                    if (basic_poids == "'0'" && addon_poids == "'0'" && ala_poids == "'0'")
                    {
                        // dynamiclink.Visible = true;
                        if (hdntag.Value == "lnkDetail")
                        {
                            lnkatag_Click(null, null);
                        }
                    }

                    //StatbleDynamicTabs();



                }
                else
                {
                    //FileLogTextChange("ServiceStatus", "Deactive ", " Error:" + Request, "");
                    //divgriddet.Visible = false;
                    //dynamiclink.Visible = true;
                    pnlGridHolder.Visible = true;
                    //if (hdntag.Value == "lnkDetail")

                    StatbleDynamicTabs();

                    Detailss.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // FileLogText("BindAllGrid", "", Session["username"].ToString(), ex.Message.ToString());
            }

        }//shri

        protected bool AutoRenewstatus(string vcid, string customerid, string planname) // done by vivek 2015-06-12
        {
            string statusvalue = "";
            bool IsActive;
            if (Session["lcoid"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
            {
                username = Convert.ToString(Session["username"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
            statusvalue = obj.autorenewstatus(username, vcid, customerid, planname);
            if (statusvalue.ToString() == "Y")
            {
                IsActive = true;
            }
            else
            {
                IsActive = false;
            }
            return IsActive;

        }

        protected void resetSearchBox()
        {
            dynamiclink.Visible = false;
            divCustHolder.Attributes.Remove("width");
            divCustHolder.Attributes["width"] = "126%";


            divSearchHolder.Attributes["class"] = "delInfo";
        }//shri

        protected void setSearchBox()
        {
            divCustHolder.Attributes.Remove("width");
            divCustHolder.Attributes["width"] = "77.5%";

            divSearchHolder.Attributes.Remove("class");
            divSearchHolder.Attributes["class"] = "delInfo";
        }//shri

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search_type2 = rdoSearchParamType.SelectedValue.ToString();

            if (search_type2 == "1")
            {
                string blusername = SecurityValidation.chkData("N", txtSearchParam.Text);
                if (blusername.Length > 0)
                {
                    lblSearchResponse.Text = blusername;
                    return;
                }

                if (txtSearchParam.Text.Length != 10)
                {
                    resetAllGrids();
                    resetSearchBox();
                    //pnlCustDetails.Visible = false;
                    pnlGridHolder.Visible = false;
                    lblSearchResponse.Text = "Please Enter Proper Account No.";
                    return;
                }
                if (txtSearchParam.Text.Substring(0, 1).ToString() != "1")
                {
                    resetAllGrids();
                    resetSearchBox();
                    //pnlCustDetails.Visible = false;
                    pnlGridHolder.Visible = false;
                    lblSearchResponse.Text = "Please Enter Proper Account No.";
                    return;
                }
            }
            else
            {
                string blusername = SecurityValidation.chkData("T", txtSearchParam.Text);
                if (blusername.Length > 0)
                {
                    lblSearchResponse.Text = blusername;
                    return;
                }
            }


            if (sender != null)
            {
                ViewState["provi_chkbx_selected_service"] = "0";
                try
                {
                    if (((System.Web.UI.WebControls.Button)(sender)).Text == "Search")
                    {
                        hdntag.Value = "lnkDetail";
                    }
                }
                catch (Exception ex)
                {
                }

            }
            if (IsSaveplan != "1")
            {
                hdntag.Value = "lnkDetail";
            }
            resetAllGrids();

            dtBasicPlans = new DataTable();
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("DEAL_POID"));
            //dtBasicPlans.Columns.Add(new DataColumn("PRODUCT_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtBasicPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dtBasicPlans.Columns.Add(new DataColumn("HD_Count"));
            dtBasicPlans.Columns.Add(new DataColumn("SD_Count"));
            dtBasicPlans.Columns.Add(new DataColumn("Total_Count"));
            dtBasicPlans.Columns.Add(new DataColumn("BD_PRICE"));
            //---------------------
            //dtBasicPlans.Columns.Add(new DataColumn("INSBY"));
            //dtBasicPlans.Columns.Add(new DataColumn("INSDT"));
            dtBasicPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtBasicPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtBasicPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtBasicPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_STATUS"));

            // created by vivek 16-nov-2015 
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtBasicPlans.Columns.Add(new DataColumn("GRACE"));
            dtBasicPlans.Columns.Add(new DataColumn("DISCOUNT"));
            dtBasicPlans.Columns.Add(new DataColumn("alacartebase"));
            dtBasicPlans.Columns.Add(new DataColumn("pincycle"));
            dtBasicPlans.Columns.Add("datevalue", typeof(DateTime));
            dtBasicPlans.Columns.Add(new DataColumn("alacartebaseprice"));
            //ViewState["customer_basic_plans"] = dtBasicPlans;

            //table which holds addon plans
            dtAddonPlans = new DataTable();
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("DEAL_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAddonPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dtAddonPlans.Columns.Add(new DataColumn("HD_Count"));
            dtAddonPlans.Columns.Add(new DataColumn("SD_Count"));
            dtAddonPlans.Columns.Add(new DataColumn("Total_Count"));
            dtAddonPlans.Columns.Add(new DataColumn("BD_PRICE"));
            //--------------
            dtAddonPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtAddonPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtAddonPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAddonPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_STATUS"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAddonPlans.Columns.Add(new DataColumn("GRACE"));
            dtAddonPlans.Columns.Add("datevalue", typeof(DateTime));

            //table which holds addon plans
            dtAddonPlansreg = new DataTable();
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_POID"));
            dtAddonPlansreg.Columns.Add(new DataColumn("DEAL_POID"));
            dtAddonPlansreg.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAddonPlansreg.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dtAddonPlansreg.Columns.Add(new DataColumn("HD_Count"));
            dtAddonPlansreg.Columns.Add(new DataColumn("SD_Count"));
            dtAddonPlansreg.Columns.Add(new DataColumn("Total_Count"));
            dtAddonPlansreg.Columns.Add(new DataColumn("BD_PRICE"));
            //----------------
            dtAddonPlansreg.Columns.Add(new DataColumn("ACTIVATION"));
            dtAddonPlansreg.Columns.Add(new DataColumn("EXPIRY"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_STATUS"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAddonPlansreg.Columns.Add(new DataColumn("GRACE"));
            dtAddonPlansreg.Columns.Add("datevalue", typeof(DateTime));

            //ViewState["customer_addon_plans"] = dtAddonPlans;

            //table which holds a-la-carte plans

            dtAlacartePlans = new DataTable();
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAlacartePlans.Columns.Add(new DataColumn("DEAL_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAlacartePlans.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dtAlacartePlans.Columns.Add(new DataColumn("HD_Count"));
            dtAlacartePlans.Columns.Add(new DataColumn("SD_Count"));
            dtAlacartePlans.Columns.Add(new DataColumn("Total_Count"));
            dtAlacartePlans.Columns.Add(new DataColumn("BD_PRICE"));
            //-------------
            dtAlacartePlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtAlacartePlans.Columns.Add(new DataColumn("EXPIRY"));
            dtAlacartePlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_STATUS"));

            // created by vivek 16-nov-2015 
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAlacartePlans.Columns.Add(new DataColumn("GRACE"));
            dtAlacartePlans.Columns.Add("datevalue", typeof(DateTime));

            dthathwayspecial = new DataTable();
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_NAME"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_POID"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_TYPE"));
            dthathwayspecial.Columns.Add(new DataColumn("DEAL_POID"));
            dthathwayspecial.Columns.Add(new DataColumn("CUST_PRICE"));
            dthathwayspecial.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dthathwayspecial.Columns.Add(new DataColumn("HD_Count"));
            dthathwayspecial.Columns.Add(new DataColumn("SD_Count"));
            dthathwayspecial.Columns.Add(new DataColumn("Total_Count"));
            dthathwayspecial.Columns.Add(new DataColumn("BD_PRICE"));
            //-----
            dthathwayspecial.Columns.Add(new DataColumn("ACTIVATION"));
            dthathwayspecial.Columns.Add(new DataColumn("EXPIRY"));
            dthathwayspecial.Columns.Add(new DataColumn("PACKAGE_ID"));
            dthathwayspecial.Columns.Add(new DataColumn("PURCHASE_POID"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_STATUS"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dthathwayspecial.Columns.Add(new DataColumn("GRACE"));
            dthathwayspecial.Columns.Add("datevalue", typeof(DateTime));

            dtAlacartePlansFree = new DataTable();
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_POID"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("DEAL_POID"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("CUST_PRICE"));
            //-------Tariff Order new Columns
            dtAlacartePlansFree.Columns.Add(new DataColumn("HD_Count"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("SD_Count"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("Total_Count"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("BD_PRICE"));
            //---------------------------
            dtAlacartePlansFree.Columns.Add(new DataColumn("LCO_PRICE"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("ACTIVATION"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("EXPIRY"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_STATUS"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("GRACE"));
            dtAlacartePlansFree.Columns.Add("datevalue", typeof(DateTime));

            lblSearchResponse.Text = "";
            string searhParam = txtSearchParam.Text;
            string search_type = rdoSearchParamType.SelectedValue.ToString();
            // string username = "";
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
            // string response_params = username + "$" + searhParam + "$SW";
            string response_params = user_brmpoid + "$" + searhParam + "$SW";
            if (search_type == "0")
            {
                //if VC ID
                response_params += "$V";
            }

            // string apiResponse = callAPI(response_params, "6");
            string apiResponse = callAPI(response_params, "12");//
            //string apiResponse = "1007817203$$$HCDPL DEMO DEMO$SUKHALIYA ROAD AREA|SUKHALIYA ROAD AREA|SUKHALIYA ROAD AREA|SUKHALIYA ROAD AREA$1234567890$0.0.0.1 /account 18816628 59$1$0.0.0.1 /payinfo/invoice 18813196 0$$*$*$*$0.0.0.1 /account 7118301 0$1007817203$0.0.0.1 /service/catv 18816236 135!5944627826510297!000063988356!0.0.0.1 /plan 4223442545 0|2016-12-16T00:00:00Z|2017-01-15T05:30:00Z|0.0.0.1 /deal 4223440177 4|104817663|0.0.0.1 /purchased_product 6622635011 8|3|NOT FOUND|pin_cycle_fees~0.0.0.1 /plan 4223442545 4|2016-04-27T00:00:00Z|2016-11-30T05:30:00Z|0.0.0.1 /deal 4223440177 4|86187110|0.0.0.1 /purchased_product 4448256640 36|3|2016-12-12T18:30:00Z|~0.0.0.1 /plan 1849316408 0|2015-05-30T00:00:00Z|2016-04-28T21:14:46Z|0.0.0.1 /deal 1849316312 0|52136364|0.0.0.1 /purchased_product 2466807577 29|3|NOT FOUND|~0.0.0.1 /plan 1849317944 0|2015-02-26T00:00:00Z|2015-05-29T21:44:54Z|0.0.0.1 /deal 1849319384 0|46030710|0.0.0.1 /purchased_product 2055135773 13|3|NOT FOUND|~0.0.0.1 /plan 1849318968 0|2015-02-26T00:00:00Z|2015-02-25T21:41:25Z|0.0.0.1 /deal 1849317432 0|45861661|0.0.0.1 /purchased_product 2046191790 11|3|NOT FOUND|~0.0.0.1 /plan 1849316408 0|2015-01-23T00:00:00Z|2015-02-21T01:52:06Z|0.0.0.1 /deal 1849316312 0|42803927|0.0.0.1 /purchased_product 1854790672 10|3|NOT FOUND|~0.0.0.1 /plan 1818111 0|2013-04-16T00:00:00Z|2015-01-22T15:55:18Z|0.0.0.1 /deal 1816511 0|1263217|0.0.0.1 /purchased_product 18814562 20|3|NOT FOUND|non-cas package~0.0.0.1 /plan 1433679 0|2013-04-16T00:00:00Z|2014-12-31T05:30:00Z|0.0.0.1 /deal 1428989 0|1263216|0.0.0.1 /purchased_product 18816322 50|3|NOT FOUND|stb zero value-removed!10100!SD!0!2015-02-26T08:30:25Z";

            //string apiResponse = callAPI(response_params, "13");
            //string[] req_data_arr = apiResponse.Split(new string[] { "##" }, StringSplitOptions.None);
            //string cust_info_res = req_data_arr[0].TrimEnd('$');
            //string service_info_res = req_data_arr[1];


            try
            {
                if (apiResponse != "")
                {

                    List<string> lstResponse = new List<string>();

                    lstResponse = apiResponse.Split('$').ToList();
                    string cust_id = lstResponse[0]; //account_no
                    ViewState["customer_no"] = cust_id;
                    Session["customer_no"] = cust_id;
                    string cust_name = lstResponse[3];
                    ViewState["customer_name"] = cust_name;
                    Session["customer_name"] = cust_name;
                    string cust_addr = lstResponse[4];
                    ViewState["accountPoid"] = lstResponse[6];
                    string lco_poid = lstResponse[13];

                    string cust_mobile_no = lstResponse[5];
                    ViewState["custemail"] = lstResponse[1].ToString();
                    Session["custemail"] = lstResponse[1].ToString();
                    lblemail.Text = lstResponse[1].ToString();
                    Session["MANUFACTURER"] = lstResponse[17];

                    //DATA ACCESS VALIDATION----------------------------------------------------------------------------------VALIDATION
                    Cls_Validation obj = new Cls_Validation();
                    string validate_cust_accesslco = obj.CustDataAccess(username, oper_id, lco_poid, Session["category"].ToString());



                    if (validate_cust_accesslco.Length == 0)
                    {
                        resetAllGrids();
                        resetSearchBox();
                        //pnlCustDetails.Visible = false;
                        pnlGridHolder.Visible = false;
                        lblSearchResponse.Text = "You have no privileges to access customer information as s/he belongs to other LCO";
                        return;
                    }
                    else
                    {
                        List<string> lcoidAndCity = new List<string>();
                        lcoidAndCity = validate_cust_accesslco.Split('$').ToList();
                        Session["lcoid"] = lcoidAndCity[0];
                        ViewState["cityid"] = lcoidAndCity[1];
                        Session["lco_username"] = lcoidAndCity[2];

                        if (Convert.ToString(Session["category"]) == "11")
                        {
                            lcocodetd.Visible = true;
                            lconametd.Visible = true;
                            lcocobalancetd.Visible = true;
                            lbllcocode.Text = "LCO Code : " + lcoidAndCity[2].ToString();
                            lbllconame.Text = "LCO Name : " + lcoidAndCity[4].ToString();
                            lbllcobalance.Text = "Available Balance : " + lcoidAndCity[3].ToString();
                            Session["dasarea"] = lcoidAndCity[5].ToString();
                        }
                        else
                        {
                            lcocodetd.Visible = false;
                            lconametd.Visible = false;
                            lcocobalancetd.Visible = false;
                            lbllcocode.Text = "";
                            lbllconame.Text = "";
                            lbllcobalance.Text = "";
                        }
                    }

                    string cust_services = lstResponse[15];
                    string[] service_arr = cust_services.Split('^');
                    ViewState["Service_Str"] = null;
                    ViewState["Service_Str"] = cust_services.ToString();
                    string stb_status = "";
                    DataTable dtStbs = new DataTable();
                    dtStbs.Columns.Add(new DataColumn("STB_NO"));
                    dtStbs.Columns.Add(new DataColumn("VC_ID"));
                    dtStbs.Columns.Add(new DataColumn("SERVICE_STRING"));
                    dtStbs.Columns.Add(new DataColumn("Status"));
                    dtStbs.Columns.Add(new DataColumn("PARENT_CHILD_FLAG"));
                    dtStbs.Columns.Add(new DataColumn("TAB_FLAG"));
                    dtStbs.Columns.Add(new DataColumn("Last_Status"));
                    dtStbs.Columns.Add(new DataColumn("Pack_Type"));

                    DataTable sortedDT = new DataTable();
                    string strvcid = "";
                    int k = 1;
                    ViewState["parentsmsg"] = "0";
                    foreach (string service in service_arr)
                    {


                        string stb_no = service.Split('!')[1];

                        string vc_id = service.Split('!')[2];

                        stb_status = service.Split('!')[4];

                        string Pack_Type = service.Split('!')[5];
                        string parent_child_flag = service.Split('!')[6];

                        string Last_Status = service.Split('!')[7];

                        string tab_flag = "";

                        if (parent_child_flag == null || parent_child_flag == "")
                        {
                            parent_child_flag = "1";

                        }
                        if (parent_child_flag == "1")
                        {
                            k = k + 1;
                            tab_flag = "lnkAddon" + k.ToString();
                        }
                        else
                        {
                            tab_flag = "lnkAddon1";
                        }

                        if (stb_status == "10103")
                        {
                            continue; //if status is terminated
                        }
                        if (stb_no == "" || vc_id == "")
                        {
                            continue; //id stan_no or vc_id is blank
                        }
                        DataRow drStbRow = dtStbs.NewRow();
                        drStbRow["STB_NO"] = stb_no;
                        drStbRow["VC_ID"] = vc_id;
                        ViewState["vcid"] = vc_id;
                        strvcid += strvcid + vc_id + ",";

                        drStbRow["SERVICE_STRING"] = service;
                        drStbRow["Status"] = stb_status;
                        ViewState["stb_status"] = stb_status;
                        drStbRow["PARENT_CHILD_FLAG"] = parent_child_flag;
                        drStbRow["TAB_FLAG"] = tab_flag;
                        drStbRow["Last_Status"] = Last_Status;
                        drStbRow["Pack_Type"] = Pack_Type;


                        dtStbs.Rows.Add(drStbRow);
                        DataView dv = dtStbs.DefaultView;
                        dv.Sort = "PARENT_CHILD_FLAG Asc";
                        sortedDT = dv.ToTable();
                    }
                    strvcid = strvcid.TrimEnd(',');
                    if (sortedDT.Rows.Count == 0)
                    {
                        btnReset_Click(sender, e);
                        lblSearchResponse.Text = "No STB found";
                        return;
                    }



                    ViewState["vcdetail"] = sortedDT;

                    DataTable dtVCinfo = new DataTable();

                    if (sortedDT.Rows.Count > 0)
                    {


                        dtVCinfo.Columns.Add(new DataColumn("TV"));
                        dtVCinfo.Columns.Add(new DataColumn("VC_ID"));
                        dtVCinfo.Columns.Add(new DataColumn("STB_NO"));
                        dtVCinfo.Columns.Add(new DataColumn("STATUS"));
                        dtVCinfo.Columns.Add(new DataColumn("BOX_TYPE"));
                        dtVCinfo.Columns.Add(new DataColumn("SUSPENSION_DATE"));

                        string Parent_Flag = "";
                        int J = 0;
                        int parentflagvalue = 0;
                        string ServiceStatus = "";


                        for (int i = 0; i < sortedDT.Rows.Count; i++)
                        {
                            DataRow drvcinfo = dtVCinfo.NewRow();
                            Parent_Flag = sortedDT.Rows[i]["TAB_FLAG"].ToString();
                            Parent_Flag = Parent_Flag.Substring(Parent_Flag.Length - 1);
                            parentflagvalue = Convert.ToInt32(Parent_Flag);
                            parentflagvalue = parentflagvalue - 1;

                            if (parentflagvalue == 0)
                            {
                                drvcinfo["TV"] = "Main TV";
                                ViewState["VC_IDMAIN"] = sortedDT.Rows[i]["VC_ID"].ToString();
                                ViewState["STB_NOMAIN"] = sortedDT.Rows[i]["STB_NO"].ToString();
                            }
                            if (parentflagvalue != 0)
                            {

                                drvcinfo["TV"] = "Addon " + parentflagvalue.ToString();
                            }

                            //drvcinfo["TV"] = sortedDT.Rows[0]["VC_ID"].ToString();
                            drvcinfo["VC_ID"] = sortedDT.Rows[i]["VC_ID"].ToString();
                            drvcinfo["STB_NO"] = sortedDT.Rows[i]["STB_NO"].ToString();

                            ServiceStatus = sortedDT.Rows[i]["Status"].ToString();


                            DateTime dttime = Convert.ToDateTime(sortedDT.Rows[i]["Last_Status"].ToString().Split('T')[0]);
                            drvcinfo["SUSPENSION_DATE"] = dttime.ToString("dd-MMM-yyyy");
                            if (ServiceStatus == "10100")
                            {
                                drvcinfo["SUSPENSION_DATE"] = "";
                                drvcinfo["STATUS"] = "Active";
                            }
                            else
                            {
                                drvcinfo["SUSPENSION_DATE"] = dttime.ToString("dd-MMM-yyyy");
                                drvcinfo["STATUS"] = "In-Active";
                            }
                            drvcinfo["BOX_TYPE"] = sortedDT.Rows[i]["Pack_Type"].ToString();
                            dtVCinfo.Rows.Add(drvcinfo);
                            if (sortedDT.Rows[i]["VC_ID"].ToString() == sortedDT.Rows[i]["STB_NO"].ToString())
                            {
                                btnVCSWAP.Visible = false;
                                btnFaulty.Text = "SWAP STB/MAC";
                                Label1050.Text = "Main VC/MAC :";
                                Label1049.Text = "Main STB/MAC :";
                                Label1048.Text = "Child VC/MAC :";
                                Label1047.Text = "Child STB/MAC :";
                                Label106.Text = "STB/MAC ID :";
                                Label109.Text = "VC/MAC ID :";
                                Label114.Text = "STB/MAC ID ";
                                Label116.Text = "VC/MAC ID ";
                            }
                            else
                            {
                                btnVCSWAP.Visible = false;
                                btnFaulty.Text = "SWAP STB";
                                Label1050.Text = "Main VC :";
                                Label1049.Text = "Main STB :";
                                Label1048.Text = "Child VC :";
                                Label1047.Text = "Child STB :";
                                Label106.Text = "STB ID :";
                                Label109.Text = "VC ID :";
                                Label114.Text = "STB ID ";
                                Label116.Text = "VC ID ";
                            }
                        }


                    }

                    GridVC.DataSource = dtVCinfo;
                    GridVC.DataBind();

                    if (IsSaveplan == "1")
                    {

                        createDynamicTabs(sortedDT);

                    }

                    /*

                                        for (int i = 1; i <= Convert.ToInt32(hdnTabCount.Value); i++)
                                        {
                                            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("MasterBody");
                                            LinkButton lnk = (LinkButton)cph.FindControl("lnkAddon" + i.ToString());
                                            lnk.Visible = false;
                                        }

                                        for (int i = 1; i <= sortedDT.Rows.Count; i++)
                                        {
                                            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("MasterBody");
                                            LinkButton lnk = (LinkButton)cph.FindControl("lnkAddon" + i.ToString());
                                            lnk.Visible = true;
                                        }
                                        */




                    //ticking stb value and bind all fields with details
                    string stb_row_number = "0";


                    //On vc search, same vc must be ticked
                    if (rdoSearchParamType.SelectedValue.ToString() == "0")
                    {
                        int dt_row_index = 0;

                        strerror = sortedDT.Rows.Count.ToString();

                        foreach (DataRow dtRw in sortedDT.Rows)
                        {


                            if (dtRw.ItemArray[1] != null && dtRw.ItemArray[1].ToString().Trim() == txtSearchParam.Text.Trim())
                            {
                                stb_row_number = Convert.ToString(dt_row_index);
                                ViewState["provi_chkbx_selected_service"] = stb_row_number;
                            }
                            dt_row_index++;
                            strerror += "~" + dt_row_index.ToString();
                        }

                        strerror = "10";
                    }

                    if (ViewState["provi_chkbx_selected_service"] != null)
                    {
                        stb_row_number = ViewState["provi_chkbx_selected_service"].ToString();
                    }

                    //if (stb_status == "10100")
                    //{
                    if (hdntag.Value.Trim() != "lnkDetail")
                    {
                        DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value.Trim() + "'").CopyToDataTable();
                        string planstring = myResultSet.Rows[0]["SERVICE_STRING"].ToString();

                        bindAllGrids(planstring);
                    }
                    else
                    {
                        bindAllGrids(sortedDT.Rows[0]["SERVICE_STRING"].ToString());
                    }
                    //}
                    //else
                    //{
                    //    setSearchBox();
                    //    Detailss.Visible = false;

                    //    pnlGridHolder.Visible = false;

                    //    lbactive.Visible = true;
                    //    lbdeactive.Visible = false;
                    //    btnDeact.Visible = false;
                    //    btnAct.Visible = true;
                    //}



                    //bindAllGrids(hdnDefaultService.Value);

                    lblCustNo.Text = cust_id;
                    lblCustName.Text = cust_name;
                    lblCustAddr.Text = cust_addr.Replace('|', ',');
                    lbltxtmobno.Text = cust_mobile_no;

                    ViewState["custaddr"] = lblCustAddr.Text;
                    ViewState["custmob"] = lbltxtmobno.Text;
                    Session["custmob"] = lbltxtmobno.Text;
                    //--------------------------------------------------------comented on 10-Jan-2015
                    //setSearchBox();
                    //pnlCustDetails.Visible = true; 
                    //pnlGridHolder.Visible = true;
                    //btnReset.Visible = true;
                }
                else
                {

                    resetSearchBox();
                    lblSearchResponse.Text = "Failed to receive customer data";
                    Detailss.Visible = false;
                    pnlGridHolder.Visible = false;
                    btnReset.Visible = false;

                }

            }
            catch (Exception ex)
            {

                //FileLogText("Customer data not found", "PAWAN", strerror, ex.Message.ToString());
                resetSearchBox();
                lblSearchResponse.Text = "Customer data not found";
                Detailss.Visible = false;
                pnlGridHolder.Visible = false;
                btnReset.Visible = false;

            }
            finally
            {
                popAdd.Hide();
            }
        }//shri

        protected void GrdAddplanFinal_RowDataBound(object sender, GridViewRowEventArgs e)  // Added By Vivek 15-Feb-2016
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[2].Visible = false;
            }
        }

        protected void ChkPlanAdd_CheckedChanged(Object sender, EventArgs args)
        {
            GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;

            int rowindex = row.RowIndex;

            HiddenField hdnplan = (HiddenField)grdPlanChan.Rows[rowindex].FindControl("hdnplanaddPlanPoid");
            HiddenField hdnplanaddplantype = (HiddenField)grdPlanChan.Rows[rowindex].FindControl("hdnplanaddplantype");
            CheckBox ChkPlanAdd = (CheckBox)grdPlanChan.Rows[rowindex].FindControl("ChkPlanAdd");
            Double grdmrp = Convert.ToDouble(grdPlanChan.Rows[rowindex].Cells[1].Text);
            if (radPlanBasic.Checked == true)
            {

                foreach (GridViewRow gr in grdPlanChan.Rows)
                {
                    HiddenField hdn = (HiddenField)gr.FindControl("hdnBasicPlanPoid");
                    CheckBox ChkPlanAddbase = (CheckBox)gr.FindControl("ChkPlanAdd");

                    if (rowindex == gr.RowIndex)
                    {

                    }
                    else
                    {
                        ChkPlanAddbase.Checked = false;
                    }

                }
            }

            if (ChkPlanAdd.Checked == true)
            {
                Double Total = 0;

                Total = Convert.ToDouble(ViewState["Total"]) + grdmrp;


                ViewState["Total"] = Total;
                lbltotaladd.Text = Total + "/-";

                StatbleDynamicTabs();
                popAdd.Show();
            }
            else
            {
                Double Total = 0;
                Total = Convert.ToDouble(ViewState["Total"]) - grdmrp;
                if (Total < 0)
                {
                    Total = 0;
                }
                ViewState["Total"] = Total;
                lbltotaladd.Text = Total + "/-";

                StatbleDynamicTabs();
                popAdd.Show();
            }

        }


        public String callAPI(string Request, string request_code)
        {
            try
            {
                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
                HttpWebRequest myRequest = null;

                myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request); //-- Deploy Time 
               // myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request); // -- debug time
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
        }//shri

        private void QueryFileLog(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\Search_Qry_" + filename + ".txt", true);
                string ___username = "unknown";
                if (Session["username"] != null && Session["username"].ToString() != "")
                {
                    ___username = Session["username"].ToString();
                }
                sw.WriteLine(Str + ":-" + ___username + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(Str + ":-" + sender + "                      " + DateTime.Now.ToString("HH:mm:ss"));
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

        private void FileLogTextChange1(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            if (!Directory.Exists(@"C:\temp\Logs\HwayChangeLog"))
            {
                Directory.CreateDirectory(@"C:\temp\Logs\HwayChangeLog");
            }
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayChangeLog\HwayObrm_Web_" + filename + ".txt", true);
                sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(strRequest.Trim());
                sw.WriteLine(strResponse.Trim());
                sw.WriteLine("************************************************************************************************************************");
            }
            catch (Exception ex)
            {
                // Response.Write("Error while writing logs : " + ex.Message.ToString());
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            resetSearchBox();
            Detailss.Visible = false;
            pnlGridHolder.Visible = false;
            btnReset.Visible = false;
            lblSearchResponse.Text = "";
            ViewState["provi_chkbx_selected_service"] = "0";
            popAdd.Hide();
        }//shri

        //--------------------------------SERVICE ACT/DEACT-BEGIN-------------------------------
        protected void btnAct_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            setServiceInfoPopup("Inactive");
            ddlServiceInfoPopupDactReason.Visible = false;
            ddlServiceInfoPopupActReason.Visible = true;
            popServiceInfo.Show();
            StatbleDynamicTabs();
            //setServicePopup("This will activate the service.", "Are you sure you want to activate?", "ACT");
            //popService.Show();
        }

        protected void btnDeact_Click(object sender, EventArgs e)
        {
            // lnkatag_Click(null, null);
            setServiceInfoPopup("Active");
            ddlServiceInfoPopupDactReason.Visible = true;
            ddlServiceInfoPopupActReason.Visible = false;
            popServiceInfo.Show();
            StatbleDynamicTabs();
            //setServicePopup("This will deactivate the service.", "Are you sure you want to deactivate?", "DACT");
            //popService.Show();
        }

        //sets service info popup
        protected void setServiceInfoPopup(string status)
        {
            lblServiceInfoPopupStatus.Text = status;
            lblServiceInfoPopupStbNo.Text = lblStbNo.Text;
            ddlServiceInfoPopupActReason.SelectedValue = "0";
            ddlServiceInfoPopupDactReason.SelectedValue = "0";
            if (status == "Inactive")
                hdnServiceInfoPopupFlag.Value = "ACT";
            else
                hdnServiceInfoPopupFlag.Value = "DACT";
        }

        //collects info required for service popup and calls service confirmation popup
        protected void btnServiceInfoSubmit_Click(object sender, EventArgs e)
        {
            // lnkatag_Click(null, null);
            string service_op = hdnServiceInfoPopupFlag.Value;
            string reason_id = "";
            string reason_txt = "";
            string service_popmsg1 = "";
            string service_popmsg2 = "";
            if (service_op == "ACT")
            {
                reason_id = ddlServiceInfoPopupActReason.SelectedValue;
                reason_txt = ddlServiceInfoPopupActReason.SelectedItem.Text;
                service_popmsg1 = "This will activate the service.";
                service_popmsg2 = "Are you sure you want to activate with reason - " + reason_txt + "?";
            }
            else
            {

                reason_id = ddlServiceInfoPopupDactReason.SelectedValue;
                reason_txt = ddlServiceInfoPopupDactReason.SelectedItem.Text;
                service_popmsg1 = "This will deactivate the service.";
                service_popmsg2 = "Are you sure you want to deactivate with reason - " + reason_txt + "?";
            }
            if (reason_id.Trim() == "0")
            {
                // checking whether reason selected or not
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert(\"Select Reason\");", true);
                popServiceInfo.Show();
                StatbleDynamicTabs();
                return;
            }
            setServicePopup(service_popmsg1, service_popmsg2, service_op.Trim());
            popService.Show();
            StatbleDynamicTabs();
        }

        //set service confirmation popup
        protected void setServicePopup(string message1, string message2, string flag)
        {
            lblPopupServiceMsg1.Text = message1;
            lblPopupServiceMsg2.Text = message2;
            hdnPopupServiceFlag.Value = flag;
        }

        protected void btnPopupServiceConfirm_Click(object sender, EventArgs e)
        {
            //  lnkatag_Click(null, null);
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            string service_operation_flag = hdnPopupServiceFlag.Value;
            string username = "";
            string user_brmpoid = "";
            string stb_no = "";
            string reason_id = "";
            string vc_id = "";
            string selected_service_str = "";
            string operation_flag = hdnPopupServiceFlag.Value;
            string req_code = "";
            if (Session["lco_username"] != null && Session["user_brmpoid"] != null)
            {
                username = Convert.ToString(Session["lco_username"]);
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }


            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
            stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
            vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
            selected_service_str = myResultSet.Rows[0]["SERVICE_STRING"].ToString();

            if (operation_flag == "ACT")
            {
                req_code = "10";
                reason_id = ddlServiceInfoPopupActReason.SelectedValue;
            }
            else
            {
                req_code = "9";
                reason_id = ddlServiceInfoPopupDactReason.SelectedValue;
            }
            if (ViewState["accountPoid"] == null && ViewState["ServicePoid"] == null)
            {
                msgboxstr("Failed to get Account POID or Service POID, Please relogin and try again.");
                return;
            }
            //string Request = username + "$" + ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + username + "$" + vc_id;//stb_no;
            string Request = user_brmpoid + "$" + ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + username + "$" + vc_id;//stb_no;
            try
            {
                //activation and deactivation
                string api_response = callAPI(Request, req_code);
                //registering in db
                string[] final_obrm_status = api_response.Split('$');
                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";
                string obrm_orderid = "";
                try
                {
                    if (obrm_status == "0" || obrm_status == "1")
                    {
                        if (operation_flag == "ACT" && obrm_status == "0")
                        {
                            obrm_msg = "Service activated successfully.";
                            obrm_orderid = final_obrm_status[2];
                        }
                        else if (operation_flag == "DACT" && obrm_status == "0")
                        {
                            obrm_msg = "Service deactivated successfully : " + final_obrm_status[2];
                        }
                        else if (operation_flag == "ACT" && obrm_status == "1")
                        {
                            obrm_msg = "Service activation failed on account : " + final_obrm_status[2];
                        }
                        else if (operation_flag == "DACT" && obrm_status == "1")
                        {
                            obrm_msg = "Service deactivation failed on account : " + final_obrm_status[2];
                        }
                        else
                        {
                            obrm_msg = "Service activation / deactivation failed";
                        }
                    }
                    else
                    {
                        obrm_status = "1";
                        obrm_msg = api_response;
                    }
                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    obrm_msg = api_response;
                }

                if (obrm_status == "0")
                { //success from OBRM
                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    Hashtable htServiceData = new Hashtable();
                    htServiceData["username"] = username;
                    htServiceData["stb_no"] = stb_no;
                    htServiceData["vc_id"] = vc_id;
                    htServiceData["cust_no"] = lblCustNo.Text;
                    htServiceData["cust_addr"] = lblCustAddr.Text;
                    htServiceData["orderid"] = obrm_orderid;
                    htServiceData["reason_id"] = reason_id;
                    htServiceData["account_poid"] = ViewState["accountPoid"].ToString();
                    htServiceData["service_poid"] = ViewState["ServicePoid"].ToString();
                    htServiceData["IP"] = Ip;
                    if (operation_flag == "ACT")
                    {
                        htServiceData["status"] = "A";
                    }
                    else
                    {
                        htServiceData["status"] = "D";
                    }
                    string response = obj.serviceStatusUpdateBLL(htServiceData);
                    msgboxstr_refresh(obrm_msg);

                }
                else
                {
                    msgboxstr(obrm_msg);
                }
            }
            catch (Exception ex)
            {
                if (operation_flag == "ACT")
                {
                    msgboxstr("Something went wrong. Service activation failed.");
                }
                else
                {
                    msgboxstr("Something went wrong. Service deactivation failed.");
                }
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-btnPopupServiceConfirm");
                return;
            }
            StatbleDynamicTabs();
        }
        //--------------------------------SERVICE ACT/DEACT-END-------------------------------

        protected string callGetProviConfirm(Hashtable htData, string flag)
        {
            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
            Hashtable ht = new Hashtable();
            ht["username"] = Session["username"].ToString();
            ht["lco_id"] = Session["lcoid"].ToString();
            ht["cust_no"] = lblCustNo.Text;
            ht["vc_id"] = Convert.ToString(ViewState["vcid"]);
            ht["cust_name"] = lblCustName.Text;
            ht["plan_id"] = htData["planpoid"];
            ht["flag"] = flag;
            ht["IP"] = htData["IP"];
            if (htData["expiry"] != null)
            {
                ht["expiry"] = htData["expiry"];
            }
            else
            {
                ht["expiry"] = ""; // add plan
            }
            if (htData["activation"] != null)
            {
                ht["activation"] = htData["activation"];
            }
            else
            {
                ht["activation"] = ""; // add plan
            }

            //---------------------changed on 03/01/2014 to manage existing plans
            if (htData["existing_poids"] != null)
            {
                ht["existing_poids"] = htData["existing_poids"];
            }
            else
            {
                ht["existing_poids"] = ""; // add plan
            }



            string result = obj.getProviConfirm(ht);
            string[] result_arr = result.Split('#');
            if (result_arr[0] != "9999")
            {
                msgboxstr(result_arr[1]);
                return null;
            }
            else
            {
                return result_arr[1];
            }
        }//shri

        protected void lnkADRenew_Click(object sender, EventArgs e)
        {

            TableRow grdAddOnPlan = ((System.Web.UI.WebControls.TableRow)((GridViewRow)(((Button)(sender)).Parent.BindingContainer)));
            HiddenField hdnPlanrenewflag = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanRenewFlag");

            if (hdnPlanrenewflag.Value == "Y") // created by vivek 16-nov-2015                
            {

                if (ViewState["BasicActionFlag"] != null)
                {
                    if (ViewState["BasicActionFlag"].ToString() == "D" || ViewState["BasicActionFlag"].ToString() == "EX")
                    {
                        if (ViewState["BasicActionFlag"].ToString() == "EX")
                        {
                            lblPopupResponse.Text = "Basic Pack is expired,you can't Renew the pack";
                        }
                        else
                        {
                            lblPopupResponse.Text = "Basic Pack is due,you can't Renew the pack";
                        }
                        btnRefreshForm.Visible = false;
                        popMsg.Show();
                        StatbleDynamicTabs();
                        return;
                    }
                }
            }
            else
            {
                btnRefreshForm.Visible = false;
                lblPopupResponse.Text = "You can't Renew the pack";
                popMsg.Show();
                StatbleDynamicTabs();
                return;
            }

            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);

            //   HiddenField hdnPlanId = (HiddenField)grdAddOnPlan.Rows[rindex].FindControl("hdnADPlanId");
            HiddenField hdnPlanName = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanName");
            // HiddenField hdnPlanType = (HiddenField)grdAddOnPlan.Rows[rindex].FindControl("hdnADPlanType");
            HiddenField hdnPlanPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanPoid");
            HiddenField hdnDealPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADDealPoid");
            //  HiddenField hdnProductPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADProductPoid");
            HiddenField hdnCustPrice = (HiddenField)grdAddOnPlan.FindControl("hdnADCustPrice");
            HiddenField hdnLcoPrice = (HiddenField)grdAddOnPlan.FindControl("hdnADLcoPrice");
            HiddenField hdnActivation = (HiddenField)grdAddOnPlan.FindControl("hdnADActivation");
            HiddenField hdnExpiry = (HiddenField)grdAddOnPlan.FindControl("hdnADExpiry");
            HiddenField hdnPackageId = (HiddenField)grdAddOnPlan.FindControl("hdnADPackageId");
            HiddenField hdnPurchasePoid = (HiddenField)grdAddOnPlan.FindControl("hdnADPurchasePoid");
            //check box for Addon Autorenewal
            CheckBox cbAddonAutorenew = (CheckBox)grdAddOnPlan.FindControl("cbAddonAutorenew");
            Hashtable htData = new Hashtable();
            //htData["planid"] = hdnPlanId.Value;
            htData["planname"] = hdnPlanName.Value;
            // htData["plantype"] = hdnPlanType.Value;
            htData["planpoid"] = hdnPlanPoid.Value;
            htData["dealpoid"] = hdnDealPoid.Value;
            htData["custprice"] = hdnCustPrice.Value;
            htData["lcoprice"] = hdnLcoPrice.Value;
            htData["activation"] = hdnActivation.Value;
            htData["expiry"] = hdnExpiry.Value;
            htData["purchasepoid"] = hdnPurchasePoid.Value;
            htData["IP"] = Ip;

            if (cbAddonAutorenew.Checked)
            {
                htData["autorenew"] = "Y";
            }
            else
            {
                htData["autorenew"] = "N";
            }

            string conResult = callGetProviConfirm(htData, "R");
            if (conResult == null)
            {
                popAdd.Hide(); //ajax control toolkit bug quickfix
                StatbleDynamicTabs();
                return;
            }
            else
            {
                htData["discountamt"] = 0;
                GrdRenewConfrim.DataSource = null;
                GrdRenewConfrim.DataBind();
                DataTable dtRenew = new DataTable();
                dtRenew.Columns.Add(new DataColumn("PLAN_NAME"));
                dtRenew.Columns.Add(new DataColumn("CUST_PRICE", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("LCO_PRICE", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("discount", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("netmrp", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("Activation"));
                dtRenew.Columns.Add(new DataColumn("valid_upto"));
                dtRenew.Columns.Add(new DataColumn("plan_poid"));
                dtRenew.Columns.Add(new DataColumn("DEAL_POID"));
                dtRenew.Columns.Add(new DataColumn("plan_type"));
                dtRenew.Columns.Add(new DataColumn("AutoRenew"));

                DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                string stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
                string vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                string selected_service_str = myResultSet.Rows[0]["SERVICE_STRING"].ToString();
                selected_service_str = selected_service_str.Replace("|", "$");
                Cls_Business_TxnAssignPlan objPlan = new Cls_Business_TxnAssignPlan();
                string strPLANPOISs = hdnPlanPoid.Value;
                //FileLogTextChange1("NCF Renew request", selected_service_str, "-------" + strPLANPOISs + "--------", Session["username"].ToString());
                string strNCFPlanList = objPlan.Check_NFCPlan(Session["username"].ToString(), selected_service_str, ViewState["cityid"].ToString(), ViewState["customer_no"].ToString(), Session["operator_id"].ToString(), strPLANPOISs, "R", vc_id);
                string[] strPlanlist = strNCFPlanList.Split('$');
                if (strPlanlist[0] == "9999")
                {
                    if (strPlanlist[1] == "Y")
                    {
                        DataTable dt = new DataTable();
                        dt = objPlan.getNCFPlanDetails(Session["username"].ToString(), strPlanlist[2].ToString(), ViewState["JVFlag"].ToString(), ViewState["cityid"].ToString(), Session["dasarea"].ToString(), Convert.ToString(Session["operator_id"]), Convert.ToString(Session["JVNO"]));
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //htData["planpoid"] += dt.Rows[i][5].ToString() + ",";
                            //htData["planname"] += dt.Rows[i][0].ToString() + ",";
                            //htData["custprice"] += Convert.ToString(Convert.ToDouble(dt.Rows[i][1].ToString()) + Convert.ToDouble(htData["custprice"].ToString()));
                            //htData["lcoprice"] += Convert.ToString(Convert.ToDouble(dt.Rows[i][2].ToString()) + Convert.ToDouble(htData["lcoprice"].ToString())); ;
                            //htData.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), dt.Rows[i][10].ToString(), dt.Rows[i][11].ToString(), dt.Rows[i][12].ToString());
                            dtRenew.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), 0, dt.Rows[i][1].ToString(), "", "", dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), htData["activation"], "N");
                        }
                    }
                }
                dtRenew.Rows.Add(htData["planname"], htData["custprice"], htData["lcoprice"], 0, htData["custprice"], htData["activation"], htData["expiry"], htData["planpoid"], htData["dealpoid"], htData["activation"], "N");

                if (dtRenew.Rows.Count > 0)
                {
                    GrdRenewConfrim.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim2.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim.DataSource = dtRenew;
                    GrdRenewConfrim.DataBind();
                    GrdRenewConfrim2.DataSource = dtRenew;
                    GrdRenewConfrim2.DataBind();

                }
                setPopup("This will renew the plan with following details.", "Are you sure you want to renew?", "R", "AD", htData);
                pop.Show();
                popAdd.Hide(); //ajax control toolkit bug quickfix
                StatbleDynamicTabs();
            }
        }//shri

        protected void lnkADCancel_Click(object sender, EventArgs e)
        {
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);

            TableRow grdAddOnPlan = ((System.Web.UI.WebControls.TableRow)((GridViewRow)(((Button)(sender)).Parent.BindingContainer)));
            //lblchangediscount.Text = "";
            // HiddenField hdnPlanId = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanId");
            HiddenField hdnPlanName = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanName");
            HiddenField hdnPlanType = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanType");
            HiddenField hdnPlanPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanPoid");
            HiddenField hdnDealPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADDealPoid");
            // HiddenField hdnProductPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADProductPoid");
            HiddenField hdnCustPrice = (HiddenField)grdAddOnPlan.FindControl("hdnADCustPrice");
            HiddenField hdnLcoPrice = (HiddenField)grdAddOnPlan.FindControl("hdnADLcoPrice");
            HiddenField hdnActivation = (HiddenField)grdAddOnPlan.FindControl("hdnADActivation");
            HiddenField hdnExpiry = (HiddenField)grdAddOnPlan.FindControl("hdnADExpiry");
            HiddenField hdnPackageId = (HiddenField)grdAddOnPlan.FindControl("hdnADPackageId");
            HiddenField hdnPurchasePoid = (HiddenField)grdAddOnPlan.FindControl("hdnADPurchasePoid");
            HiddenField hdnChannelCount = (HiddenField)grdAddOnPlan.FindControl("hdnChannelCount");
            Session["ChannelCount"] = hdnChannelCount.Value;
            //check box for Addon Autorenewal
            CheckBox cbAddonAutorenew = (CheckBox)grdAddOnPlan.FindControl("cbAddonAutorenew");
            Hashtable htData = new Hashtable();
            //  htData["planid"] = hdnPlanId.Value;
            htData["planname"] = hdnPlanName.Value;
            // htData["plantype"] = hdnPlanType.Value;
            htData["planpoid"] = hdnPlanPoid.Value;
            htData["dealpoid"] = hdnDealPoid.Value;
            htData["custprice"] = hdnCustPrice.Value;
            htData["lcoprice"] = hdnLcoPrice.Value;
            htData["activation"] = hdnActivation.Value;
            htData["expiry"] = hdnExpiry.Value;
            htData["packageid"] = hdnPackageId.Value;
            htData["purchasepoid"] = hdnPurchasePoid.Value;
            htData["IP"] = Ip;

            if (cbAddonAutorenew.Checked)
            {
                htData["autorenew"] = "Y";
            }
            else
            {
                htData["autorenew"] = "N";
            }
            htData["plantypevalue"] = hdnPlanType.Value;
            GrdRenewConfrim.DataSource = null;
            GrdRenewConfrim.DataBind();
            GrdRenewConfrim2.DataSource = null;
            GrdRenewConfrim2.DataBind();
            DataTable dtRenew = new DataTable();
            dtRenew.Columns.Add(new DataColumn("PLAN_NAME"));
            dtRenew.Columns.Add(new DataColumn("CUST_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("LCO_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("discount", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("netmrp", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("Activation"));
            dtRenew.Columns.Add(new DataColumn("valid_upto"));
            dtRenew.Columns.Add(new DataColumn("plan_poid"));
            dtRenew.Columns.Add(new DataColumn("DEAL_POID"));
            dtRenew.Columns.Add(new DataColumn("plan_type"));
            dtRenew.Columns.Add(new DataColumn("AutoRenew"));
            dtRenew.Rows.Add(htData["planname"], htData["custprice"], htData["lcoprice"], 0, htData["custprice"], htData["activation"], htData["expiry"], htData["planpoid"], htData["dealpoid"], htData["activation"], "N");
            string conResult = callGetProviConfirm(htData, "C");
            if (ViewState["rowIndex"] != null)
            {
                //do nothing
                ViewState["transaction_data"] = htData;
            }
            else
            {
                if (conResult == null)
                {
                    popAdd.Hide(); //ajax control toolkit bug quickfix
                    StatbleDynamicTabs();
                    return;
                }
                else
                {

                    GrdRenewConfrim.DataSource = dtRenew;
                    GrdRenewConfrim.DataBind();
                    GrdRenewConfrim2.DataSource = dtRenew;
                    GrdRenewConfrim2.DataBind();
                    string[] conResult_arr = conResult.Split('$');
                    htData["refund_amt"] = conResult_arr[1];
                    htData["days_left"] = conResult_arr[0];
                    try
                    {
                        htData["refund_lcoamt"] = conResult_arr[2];
                    }
                    catch
                    {
                        htData["refund_lcoamt"] = "Rs.0";
                    }
                    if (dtRenew.Rows.Count > 0)
                    {
                        GrdRenewConfrim.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                        GrdRenewConfrim2.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                        GrdRenewConfrim.DataSource = dtRenew;
                        GrdRenewConfrim.DataBind();
                        GrdRenewConfrim2.DataSource = dtRenew;
                        GrdRenewConfrim2.DataBind();
                    }
                    setPopup("This will cancel the plan with following details.", "Are you sure you want to cancel?", "C", "AD", htData);
                    pop.Show();
                    popAdd.Hide(); //ajax control toolkit bug quickfix
                    StatbleDynamicTabs();
                }
            }

        }//shri

        protected void lnkALRenew_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            TableRow grdCarte = ((System.Web.UI.WebControls.TableRow)((GridViewRow)(((Button)(sender)).Parent.BindingContainer)));

            HiddenField hdnALPlanRenewFlag = (HiddenField)grdCarte.FindControl("hdnALPlanRenewFlag");

            if (hdnALPlanRenewFlag.Value == "Y")
            {
                if (ViewState["BasicActionFlag"] != null)
                {

                    if (ViewState["BasicActionFlag"].ToString() == "D" || ViewState["BasicActionFlag"].ToString() == "EX")     // created by vivek 16-nov-2015
                    {
                        if (ViewState["BasicActionFlag"].ToString() == "EX")
                        {
                            lblPopupResponse.Text = "Basic Pack is expired,you can't Renew the pack";
                        }
                        else
                        {
                            lblPopupResponse.Text = "Basic Pack is due,you can't Renew the pack";
                        }
                        btnRefreshForm.Visible = false;
                        popMsg.Show();
                        StatbleDynamicTabs();
                        return;
                    }
                }
            }
            else
            {
                btnRefreshForm.Visible = false;
                lblPopupResponse.Text = "You can't Renew the pack";
                popMsg.Show();
                StatbleDynamicTabs();
                return;
            }

            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);

            // HiddenField hdnPlanId = (HiddenField)grdCarte.FindControl("hdnALPlanId");
            HiddenField hdnPlanName = (HiddenField)grdCarte.FindControl("hdnALPlanName");
            // HiddenField hdnPlanType = (HiddenField)grdCarte.FindControl("hdnALPlanType");
            HiddenField hdnPlanPoid = (HiddenField)grdCarte.FindControl("hdnALPlanPoid");
            HiddenField hdnDealPoid = (HiddenField)grdCarte.FindControl("hdnALDealPoid");
            // HiddenField hdnProductPoid = (HiddenField)grdCarte.FindControl("hdnALProductPoid");
            HiddenField hdnCustPrice = (HiddenField)grdCarte.FindControl("hdnALCustPrice");
            HiddenField hdnLcoPrice = (HiddenField)grdCarte.FindControl("hdnALLcoPrice");
            HiddenField hdnActivation = (HiddenField)grdCarte.FindControl("hdnALActivation");
            HiddenField hdnExpiry = (HiddenField)grdCarte.FindControl("hdnALExpiry");
            HiddenField hdnPackageId = (HiddenField)grdCarte.FindControl("hdnALPackageId");
            HiddenField hdnPurchasePoid = (HiddenField)grdCarte.FindControl("hdnALPurchasePoid");

            //check box for Ala Autorenewal
            CheckBox cbAlaAutorenew = (CheckBox)grdCarte.FindControl("cbAlaAutorenew");
            Hashtable htData = new Hashtable();
            // htData["planid"] = hdnPlanId.Value;
            htData["planname"] = hdnPlanName.Value;
            // htData["plantype"] = hdnPlanType.Value;
            htData["planpoid"] = hdnPlanPoid.Value;
            htData["dealpoid"] = hdnDealPoid.Value;
            htData["custprice"] = hdnCustPrice.Value;
            htData["lcoprice"] = hdnLcoPrice.Value;
            htData["activation"] = hdnActivation.Value;
            htData["expiry"] = hdnExpiry.Value;
            htData["packageid"] = hdnPackageId.Value;
            htData["purchasepoid"] = hdnPurchasePoid.Value;
            htData["IP"] = Ip;

            if (cbAlaAutorenew.Checked)
            {
                htData["autorenew"] = "Y";
            }
            else
            {
                htData["autorenew"] = "N";
            }

            string conResult = callGetProviConfirm(htData, "R");
            if (conResult == null)
            {
                popAdd.Hide(); //ajax control toolkit bug quickfix
                StatbleDynamicTabs();
                return;
            }
            else
            {
                htData["discountamt"] = 0;
                DataTable dtRenew = new DataTable();
                dtRenew.Columns.Add(new DataColumn("PLAN_NAME"));
                dtRenew.Columns.Add(new DataColumn("CUST_PRICE", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("LCO_PRICE", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("discount", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("netmrp", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("Activation"));
                dtRenew.Columns.Add(new DataColumn("valid_upto"));
                dtRenew.Columns.Add(new DataColumn("plan_poid"));
                dtRenew.Columns.Add(new DataColumn("DEAL_POID"));
                dtRenew.Columns.Add(new DataColumn("plan_type"));
                dtRenew.Columns.Add(new DataColumn("AutoRenew"));


                DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                string stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
                string vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                string selected_service_str = myResultSet.Rows[0]["SERVICE_STRING"].ToString();
                selected_service_str = selected_service_str.Replace("|", "$");
                Cls_Business_TxnAssignPlan objPlan = new Cls_Business_TxnAssignPlan();
                string strPLANPOISs = hdnPlanPoid.Value;
                string strNCFPlanList = objPlan.Check_NFCPlan(Session["username"].ToString(), selected_service_str, ViewState["cityid"].ToString(), ViewState["customer_no"].ToString(), Session["operator_id"].ToString(), strPLANPOISs, "R", vc_id);
                string[] strPlanlist = strNCFPlanList.Split('$');
                if (strPlanlist[0] == "9999")
                {
                    if (strPlanlist[1] == "Y")
                    {
                        DataTable dt = new DataTable();
                        dt = objPlan.getNCFPlanDetails(Session["username"].ToString(), strPlanlist[2].ToString(), ViewState["JVFlag"].ToString(), ViewState["cityid"].ToString(), Session["dasarea"].ToString(), Convert.ToString(Session["operator_id"]), Convert.ToString(Session["JVNO"]));
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //htData["planpoid"] += dt.Rows[i][5].ToString() + ",";
                            //htData["planname"] += dt.Rows[i][0].ToString() + ",";
                            // htData["custprice"] += Convert.ToString(Convert.ToDouble(dt.Rows[i][1].ToString()) + Convert.ToDouble(htData["custprice"].ToString()));
                            // htData["lcoprice"] += Convert.ToString(Convert.ToDouble(dt.Rows[i][2].ToString()) + Convert.ToDouble(htData["lcoprice"].ToString())); ;
                            //htData.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), dt.Rows[i][10].ToString(), dt.Rows[i][11].ToString(), dt.Rows[i][12].ToString());
                            dtRenew.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), 0, dt.Rows[i][1].ToString(), "", "", dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), htData["activation"], "N");
                        }
                    }
                }
                dtRenew.Rows.Add(htData["planname"], htData["custprice"], htData["lcoprice"], 0, htData["custprice"], htData["activation"], htData["expiry"], htData["planpoid"], htData["dealpoid"], htData["activation"], "N");
                if (dtRenew.Rows.Count > 0)
                {
                    GrdRenewConfrim.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim2.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim.DataSource = dtRenew;
                    GrdRenewConfrim.DataBind();
                    GrdRenewConfrim2.DataSource = dtRenew;
                    GrdRenewConfrim2.DataBind();
                }
                setPopup("This will renew the plan with following details.", "Are you sure you want to renew?", "R", "AL", htData);
                pop.Show();
                popAdd.Hide(); //ajax control toolkit bug quickfix
                StatbleDynamicTabs();
            }
        }//shri

        protected void lnkALCancel_Click(object sender, EventArgs e)
        {
            // lnkatag_Click(null, null);
            //lblchangediscount.Text = "";
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            int rindex = (((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex;
            TableRow grdCarte = ((System.Web.UI.WebControls.TableRow)((GridViewRow)(((Button)(sender)).Parent.BindingContainer)));
            // HiddenField hdnPlanId = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALPlanId");
            HiddenField hdnPlanName = (HiddenField)grdCarte.FindControl("hdnALPlanName");
            // HiddenField hdnPlanType = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALPlanType");
            HiddenField hdnPlanPoid = (HiddenField)grdCarte.FindControl("hdnALPlanPoid");
            HiddenField hdnDealPoid = (HiddenField)grdCarte.FindControl("hdnALDealPoid");
            // HiddenField hdnProductPoid = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALProductPoid");
            HiddenField hdnCustPrice = (HiddenField)grdCarte.FindControl("hdnALCustPrice");
            HiddenField hdnLcoPrice = (HiddenField)grdCarte.FindControl("hdnALLcoPrice");
            HiddenField hdnActivation = (HiddenField)grdCarte.FindControl("hdnALActivation");
            HiddenField hdnExpiry = (HiddenField)grdCarte.FindControl("hdnALExpiry");
            HiddenField hdnPackageId = (HiddenField)grdCarte.FindControl("hdnALPackageId");
            HiddenField hdnPurchasePoid = (HiddenField)grdCarte.FindControl("hdnALPurchasePoid");
            HiddenField hdnChannelCount = (HiddenField)grdCarte.FindControl("hdnChannelCount");
            Session["ChannelCount"] = hdnChannelCount.Value;
            //check box for Ala Autorenewal
            CheckBox cbAlaAutorenew = (CheckBox)grdCarte.FindControl("cbAlaAutorenew");
            Hashtable htData = new Hashtable();
            // htData["planid"] = hdnPlanId.Value;
            htData["planname"] = hdnPlanName.Value;
            // htData["plantype"] = hdnPlanType.Value;
            htData["planpoid"] = hdnPlanPoid.Value;
            htData["dealpoid"] = hdnDealPoid.Value;
            htData["custprice"] = hdnCustPrice.Value;
            htData["lcoprice"] = hdnLcoPrice.Value;
            htData["activation"] = hdnActivation.Value;
            htData["expiry"] = hdnExpiry.Value;
            htData["packageid"] = hdnPackageId.Value;
            htData["purchasepoid"] = hdnPurchasePoid.Value;
            htData["IP"] = Ip;

            if (cbAlaAutorenew.Checked)
            {
                htData["autorenew"] = "Y";
            }
            else
            {
                htData["autorenew"] = "N";
            }
            GrdRenewConfrim.DataSource = null;
            GrdRenewConfrim.DataBind();
            GrdRenewConfrim2.DataSource = null;
            GrdRenewConfrim2.DataBind();
            DataTable dtRenew = new DataTable();
            dtRenew.Columns.Add(new DataColumn("PLAN_NAME"));
            dtRenew.Columns.Add(new DataColumn("CUST_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("LCO_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("discount", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("netmrp", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("Activation"));
            dtRenew.Columns.Add(new DataColumn("valid_upto"));
            dtRenew.Columns.Add(new DataColumn("plan_poid"));
            dtRenew.Columns.Add(new DataColumn("DEAL_POID"));
            dtRenew.Columns.Add(new DataColumn("plan_type"));
            dtRenew.Columns.Add(new DataColumn("AutoRenew"));
            dtRenew.Rows.Add(htData["planname"], htData["custprice"], htData["lcoprice"], 0, htData["custprice"], htData["activation"], htData["expiry"], htData["planpoid"], htData["dealpoid"], htData["activation"], "N");

            string conResult = callGetProviConfirm(htData, "C");
            if (conResult == null)
            {
                popAdd.Hide(); //ajax control toolkit bug quickfix
                StatbleDynamicTabs();
                return;
            }
            else
            {

                GrdRenewConfrim.DataSource = dtRenew;
                GrdRenewConfrim.DataBind();
                GrdRenewConfrim2.DataSource = dtRenew;
                GrdRenewConfrim2.DataBind();
                string[] conResult_arr = conResult.Split('$');
                htData["refund_amt"] = conResult_arr[1];
                htData["days_left"] = conResult_arr[0];

                try
                {
                    htData["refund_lcoamt"] = conResult_arr[2];
                }
                catch
                {
                    htData["refund_lcoamt"] = "Rs.0";
                }
                if (dtRenew.Rows.Count > 0)
                {
                    GrdRenewConfrim.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim2.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim.DataSource = dtRenew;
                    GrdRenewConfrim.DataBind();
                    GrdRenewConfrim2.DataSource = dtRenew;
                    GrdRenewConfrim2.DataBind();
                }
                setPopup("This will cancel the plan with following details.", "Are you sure you want to cancel?", "C", "AL", htData);
                pop.Show();
                popAdd.Hide(); //ajax control toolkit bug quickfix
                StatbleDynamicTabs();
            }

        }//shri

        #region Date Alignment 08072019
        // -- Need to add new Method for if basic plan having then show that expiry date align
        protected void btnAddPlan1_Click(object sender, EventArgs e)
        {
            hdnButtonValue.Value = "BTNADDPLAN";
            if ((ViewState["BasicExpiry"]).ToString() != "")
            {
                // popAlignDate.Show();
                msgAlignDate("Do you want to align the expiry date with your Basic Plan ?");
            }
            else
            {
                btnAddPlan_Click(null, null);
            }
        }

        public void msgAlignDate(string message)
        {
            // lnkatag_Click(null, null);
            lblALignDate.Text = message;
            //  btnRefreshForm.Visible = false;
            popAlignDate.Show();
            //  popAlignDate.Hide(); //ajax control toolkit bug quickfix
            StatbleDynamicTabs();
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            ViewState["Alignflag"] = "Y";

            if (hdnButtonValue.Value.ToUpper() == "BTNADDPLAN")
            {
                btnAddPlan_Click(null, null);
            }
            else if (hdnButtonValue.Value.ToUpper() == "BTNRENSUBMIT")
            {
                btnRenSubmit_Click(null, null);
            }
            popAdd.Hide();
            popAlignDate.Hide();
            StatbleDynamicTabs();
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            ViewState["Alignflag"] = "N";
            if (hdnButtonValue.Value.ToUpper() == "BTNADDPLAN")
            {
                btnAddPlan_Click(null, null);
            }
            else if (hdnButtonValue.Value.ToUpper() == "BTNRENSUBMIT")
            {
                btnRenSubmit_Click(null, null);
            }
            popAdd.Hide();
            popAlignDate.Hide();
            StatbleDynamicTabs();
        }

        protected void btnRenSubmit_Click1(object sender, EventArgs e)
        {
            hdnButtonValue.Value = "BTNRENSUBMIT";

            if ((ViewState["BasicExpiry"]).ToString() != "")
            {
                // popAlignDate.Show();
                msgAlignDate("Do you want to align the expiry date with your Basic Plan ?");

            }
            else
            {
                btnRenSubmit_Click(null, null);
            }
        }
        #endregion

        //---


        protected void btnAddPlan_Click(object sender, EventArgs e)
        {

            string strPLANPOISs = "";
            ViewState["TblPlanAddfinal"] = null;
            GrdaddplanConfrim.DataSource = null;
            GrdaddplanConfrim.DataBind();
            nullFOCViewState();
            lbltotaladd.Text = "0.00/-";
            hdntotaladdamount.Value = "0";
            ViewState["Total"] = "0";
            DataTable TblPlanAddfinal = new DataTable();
            TblPlanAddfinal.Columns.Add("plan_name");
            TblPlanAddfinal.Columns.Add("cust_price", typeof(double));
            TblPlanAddfinal.Columns.Add("lco_price", typeof(double));
            TblPlanAddfinal.Columns.Add("BC_price", typeof(double));
            TblPlanAddfinal.Columns.Add("discount", typeof(double));
            TblPlanAddfinal.Columns.Add("netmrp", typeof(double));
            TblPlanAddfinal.Columns.Add("ChannelCount", typeof(double));
            TblPlanAddfinal.Columns.Add("plan_poid");
            TblPlanAddfinal.Columns.Add("deal_poid");
            TblPlanAddfinal.Columns.Add("productid");
            TblPlanAddfinal.Columns.Add("plan_type");
            TblPlanAddfinal.Columns.Add("autorenew");
            TblPlanAddfinal.Columns.Add("Message");
            TblPlanAddfinal.Columns.Add("Code");
            TblPlanAddfinal.Columns.Add("foctype");

            //-----NCF-------
            DataTable TblNCFPlanAddfinal = new DataTable();
            TblNCFPlanAddfinal.Columns.Add("plan_name");
            TblNCFPlanAddfinal.Columns.Add("cust_price", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("lco_price", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("BC_price", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("discount", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("netmrp", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("ChannelCount", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("plan_poid");
            TblNCFPlanAddfinal.Columns.Add("deal_poid");
            TblNCFPlanAddfinal.Columns.Add("productid");
            TblNCFPlanAddfinal.Columns.Add("plan_type");
            TblNCFPlanAddfinal.Columns.Add("autorenew");
            TblNCFPlanAddfinal.Columns.Add("Message");
            TblNCFPlanAddfinal.Columns.Add("Code");
            TblNCFPlanAddfinal.Columns.Add("foctype");

            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            Hashtable htData = new Hashtable();
            htData["IP"] = Ip;
            ViewState["SearchedPoid"] = null;
            ViewState["SearchedPlanName"] = null;
            foreach (GridViewRow gr in grdPlanChan.Rows)
            {
                HiddenField hdnplanaddPlanPoid = (HiddenField)gr.Cells[2].FindControl("hdnplanaddPlanPoid");
                HiddenField hdnplanaddDealPoid = (HiddenField)gr.Cells[2].FindControl("hdnplanaddDealPoid");
                HiddenField hdnplanaddproducteId = (HiddenField)gr.Cells[2].FindControl("hdnplanaddproducteId");
                HiddenField hdnplanaddplantype = (HiddenField)gr.Cells[2].FindControl("hdnplanaddplantype");
                HiddenField hdnplanaddplanLCOPrice = (HiddenField)gr.Cells[2].FindControl("hdnplanaddplanLCOPrice");
                HiddenField hdnplanaddplanBCPrice = (HiddenField)gr.Cells[2].FindControl("hdnplanaddplanBCPrice");
                //CheckBox ChkPlanAddRenew = (CheckBox)gr.Cells[9].FindControl("ChkPlanAddRenew");
                CheckBox ChkPlanAdd = (CheckBox)gr.Cells[2].FindControl("ChkPlanAdd");
                try
                {
                    if (ChkPlanAdd.Checked)
                    {
                        String Plan_name = HttpUtility.HtmlDecode(gr.Cells[0].Text.Trim());
                        Double lcoprice = Convert.ToDouble(hdnplanaddplanLCOPrice.Value);
                        Double BCprice = Convert.ToDouble(hdnplanaddplanBCPrice.Value);
                        Double custprice = Convert.ToDouble(gr.Cells[1].Text.Trim());
                        Double SDCount = Convert.ToDouble(gr.Cells[5].Text.Trim());
                        Double HDCount = Convert.ToDouble(gr.Cells[6].Text.Trim()) * 2;
                        Double ChannelCount = SDCount + HDCount;
                        strPLANPOISs += hdnplanaddPlanPoid.Value + ",";

                        if (hdnplanaddplantype.Value.Trim() == "B")
                        {
                            ViewState["BasciaddcancelFOC"] = "Y";

                        }
                        else
                        {
                            ViewState["BasciaddcancelFOC"] = null;
                        }

                        ViewState["SearchedPoid"] = hdnplanaddPlanPoid.Value.Trim();
                        ViewState["SearchedPlanName"] = Plan_name;
                        htData["planpoid"] = hdnplanaddPlanPoid.Value.Trim();

                        if (hdnplanaddplantype.Value.Trim() == "GAD")
                        {
                            string addon_poids = "'0'";
                            if (ViewState["addon_poids"] != null && ViewState["addon_poids"] != "")
                            {
                                addon_poids = ViewState["addon_poids"].ToString();
                            }
                            htData["existing_poids"] = addon_poids;
                        }
                        if (hdnplanaddplantype.Value.Trim() == "RAD")
                        {
                            string addon_poidsReg = "'0'";
                            if (ViewState["addon_poidsReg"] != null && ViewState["addon_poidsReg"] != "")
                            {
                                addon_poidsReg = ViewState["addon_poidsReg"].ToString();
                            }
                            htData["existing_poids"] = addon_poidsReg;
                        }
                        if (hdnplanaddplantype.Value.Trim() == "AL")
                        {
                            string ala_poids = "'0'";
                            if (ViewState["ala_poids"] != null && ViewState["ala_poids"] != "")
                            {
                                ala_poids = ViewState["addon_poids"].ToString();
                            }
                            htData["existing_poids"] = ala_poids;
                        }

                        htData["autorenew"] = "N";
                       
                        string conResult = callGetProviConfirmAddplan(htData, "A");

                        string[] result_arr = conResult.Split('#');
                        string[] result_arr2 = result_arr[1].Split('$');
                        if (result_arr[0] != "9999")
                        {
                            //result_arr[1];
                            TblPlanAddfinal.Rows.Add(Plan_name, custprice, lcoprice, BCprice, 0, 0, ChannelCount, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                          hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), result_arr[1].ToString(), result_arr[0].ToString(), "");
                        }
                        else
                        {
                            try
                            {
                                if (result_arr2[1].ToString() != "")
                                {
                                    TblPlanAddfinal.Rows.Add(Plan_name, result_arr2[1].ToString(), result_arr2[2].ToString(), BCprice, Convert.ToDouble(result_arr2[0]), custprice - Convert.ToDouble(result_arr2[0]), ChannelCount, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                                     hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), "Valid Plan for Add", "9999", "");
                                }
                            }
                            catch (Exception)
                            {
                                TblPlanAddfinal.Rows.Add(Plan_name, 0, 0, BCprice, Convert.ToDouble(result_arr2[0]), custprice - Convert.ToDouble(0), ChannelCount, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                                  hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), "Valid Plan for Add", "9999", "");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msgboxstr(ex.ToString());
                }
            }

            /*if (radPlanBasic.Checked == true)
            {
                if (TblPlanAddfinal.Rows.Count > 0)
                {
                    ViewState["TblPlanAddfinal"] = TblPlanAddfinal;
                    fillFreePlanGrid(ViewState["SearchedPoid"].ToString(), "N");
                    if (grdFreePlan.Rows.Count > 0)
                    {

                        PopUpFreePlan.Show();
                        StatbleDynamicTabs();
                        return;
                    }

                }
                else
                {
                    msgboxstr("Please select Plan");
                }
            }
            else
            {*/
            try
            {
                DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                string stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
                string vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                string selected_service_str = myResultSet.Rows[0]["SERVICE_STRING"].ToString();
                selected_service_str = selected_service_str.Replace("|", "$");
                ViewState["strNCFPlanListNew"] = null;
                Cls_Business_TxnAssignPlan objPlan = new Cls_Business_TxnAssignPlan();
                //FileLogTextChange1(vc_id, "Calling NCF Procedure", selected_service_str, strPLANPOISs);
                string strNCFPlanList = objPlan.Check_NFCPlan(Session["username"].ToString(), selected_service_str, ViewState["cityid"].ToString(), ViewState["customer_no"].ToString(), Session["operator_id"].ToString(), strPLANPOISs, "A", vc_id);
                //FileLogTextChange1(vc_id, "Calling NCF Procedure", strNCFPlanList, "");
                string[] strPlanlist = strNCFPlanList.Split('$');
                if (strPlanlist[0] == "9999")
                {
                    if (strPlanlist[1] == "Y")
                    {
                        if (strPlanlist[3] == "B")
                        {
                            DataTable dt = new DataTable();
                            dt = objPlan.getNCFPlanDetails(Session["username"].ToString(), strPlanlist[2].ToString(), ViewState["JVFlag"].ToString(), ViewState["cityid"].ToString(), Session["dasarea"].ToString(), Convert.ToString(Session["operator_id"]), Convert.ToString(Session["JVNO"]));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                ViewState["strNCFPlanListNew"] = strPlanlist[2].ToString();
                                Double SDcount = Convert.ToDouble(dt.Rows[i][13].ToString());
                                Double HDcount = Convert.ToDouble(dt.Rows[i][14].ToString()) * 2;
                                double BC_PRICE = Convert.ToDouble(dt.Rows[i][15].ToString());
                                Double Channelcount = SDcount + HDcount;
                                //--- Calculating Align Date Price here 
                                Hashtable htDataNCF = new Hashtable();
                                htDataNCF["IP"] = Ip;
                                htDataNCF["planpoid"] = dt.Rows[i][5].ToString();
                                htDataNCF["existing_poids"] = "'0'";
                                htDataNCF["autorenew"] = "N";
                                
                                string conResult = callGetProviConfirmAddplan(htDataNCF, "A");

                                string[] result_arr = conResult.Split('#');
                                string[] result_arr2 = result_arr[1].Split('$');

                                if (result_arr[0] != "9999")
                                {
                                    //result_arr[1];
                                    TblPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), BC_PRICE, dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), dt.Rows[i][10].ToString(), dt.Rows[i][11].ToString(), dt.Rows[i][12].ToString());
                                }
                                else
                                {
                                    try
                                    {
                                        if (result_arr2[1].ToString() != "")
                                        {
                                            TblPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), result_arr2[1].ToString(), result_arr2[2].ToString(), BC_PRICE, Convert.ToDouble(result_arr2[0]),
                                                Convert.ToDouble(dt.Rows[i][4].ToString()) - Convert.ToDouble(result_arr2[0]), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), "Valid Plan for Add", result_arr[0].ToString(), dt.Rows[i][12].ToString());
                                            //TblPlanAddfinal.Rows.Add(Plan_name, result_arr2[1].ToString(), result_arr2[2].ToString(), BCprice, Convert.ToDouble(result_arr2[0]), custprice - Convert.ToDouble(result_arr2[0]), ChannelCount, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                                            // hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), result_arr[1].ToString(), result_arr[0].ToString(), "");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        //TblPlanAddfinal.Rows.Add(Plan_name, 0, 0, BCprice, Convert.ToDouble(result_arr2[0]), custprice - Convert.ToDouble(0), ChannelCount, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                                        //  hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), result_arr[1].ToString(), result_arr[0].ToString(), "");
                                        TblPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), 0, 0, BC_PRICE, Convert.ToDouble(result_arr2[0]),
                                               Convert.ToDouble(dt.Rows[i][4].ToString()) - Convert.ToDouble(result_arr2[0]), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), "Valid Plan for Add", result_arr[0].ToString(), dt.Rows[i][12].ToString());
                                    }
                                }
                                //TblPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), BC_PRICE, dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), dt.Rows[i][10].ToString(), dt.Rows[i][11].ToString(), dt.Rows[i][12].ToString());
                            }
                        }
                        else
                        {
                            DataTable dt = new DataTable();
                            dt = objPlan.getNCFPlanDetails(Session["username"].ToString(), strPlanlist[2].ToString(), ViewState["JVFlag"].ToString(), ViewState["cityid"].ToString(), Session["dasarea"].ToString(), Convert.ToString(Session["operator_id"]), Convert.ToString(Session["JVNO"]));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                ViewState["strNCFPlanListNew"] = strPlanlist[2].ToString();
                                Double SDcount = Convert.ToDouble(dt.Rows[i][13].ToString());
                                Double HDcount = Convert.ToDouble(dt.Rows[i][14].ToString()) * 2;
                                double BC_PRICE = Convert.ToDouble(dt.Rows[i][15].ToString());
                                Double Channelcount = SDcount + HDcount;
                                //--- Calculating Align Date Price here 
                                Hashtable htDataNCF2 = new Hashtable();
                                htDataNCF2["IP"] = Ip;
                                htDataNCF2["planpoid"] = dt.Rows[i][5].ToString();
                                htDataNCF2["existing_poids"] = "'0'";
                                htDataNCF2["autorenew"] = "N";
                               
                                string conResult = callGetProviConfirmAddplan(htDataNCF2, "A");

                                string[] result_arr = conResult.Split('#');
                                string[] result_arr2 = result_arr[1].Split('$');

                                if (result_arr[0] != "9999")
                                {
                                    TblNCFPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), BC_PRICE, dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), dt.Rows[i][10].ToString(), dt.Rows[i][11].ToString(), dt.Rows[i][12].ToString());
                                }
                                else
                                {
                                    try
                                    {
                                        if (result_arr2[1].ToString() != "")
                                        {
                                            TblNCFPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), result_arr2[1].ToString(), result_arr2[2].ToString(), BC_PRICE, Convert.ToDouble(result_arr2[0]),
                                                Convert.ToDouble(dt.Rows[i][4].ToString()) - Convert.ToDouble(result_arr2[0]), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), "Valid Plan for Add", result_arr[0].ToString(), dt.Rows[i][12].ToString());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        TblNCFPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), 0, 0, BC_PRICE, Convert.ToDouble(result_arr2[0]),
                                               Convert.ToDouble(dt.Rows[i][4].ToString()) - Convert.ToDouble(result_arr2[0]), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), "Valid Plan for Add", result_arr[0].ToString(), dt.Rows[i][12].ToString());
                                    }
                                }

                              //  TblNCFPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), BC_PRICE, dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), dt.Rows[i][10].ToString(), dt.Rows[i][11].ToString(), dt.Rows[i][12].ToString());
                            }
                            for (int i = 0; i < TblPlanAddfinal.Rows.Count; i++)
                            {
                                TblNCFPlanAddfinal.Rows.Add(TblPlanAddfinal.Rows[i][0].ToString(), TblPlanAddfinal.Rows[i][1].ToString(), TblPlanAddfinal.Rows[i][2].ToString(), TblPlanAddfinal.Rows[i][3].ToString(), TblPlanAddfinal.Rows[i][4].ToString(), TblPlanAddfinal.Rows[i][5].ToString(), TblPlanAddfinal.Rows[i][6].ToString(), TblPlanAddfinal.Rows[i][7].ToString(), TblPlanAddfinal.Rows[i][8].ToString(), TblPlanAddfinal.Rows[i][9].ToString(), TblPlanAddfinal.Rows[i][10].ToString(), TblPlanAddfinal.Rows[i][11].ToString(), TblPlanAddfinal.Rows[i][12].ToString(), TblPlanAddfinal.Rows[i][13].ToString(), TblPlanAddfinal.Rows[i][13].ToString());
                            }
                            TblPlanAddfinal.Clear();
                        }
                    }
                    Session["ChannelCount"] = strPlanlist[4].ToString();
                }
                else
                {
                    Session["ChannelCount"] = lblChannelcount.Text;
                }

                GrdaddplanConfrim.DataSource = null;
                GrdaddplanConfrim.DataBind();
                if (TblNCFPlanAddfinal.Rows.Count > 0 || TblPlanAddfinal.Rows.Count > 0)
                {

                    //GrdaddplanConfrim.Columns[2].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("LCO_PRICE")).Sum().ToString();
                    //GrdaddplanConfrim.Columns[3].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("discount")).Sum().ToString();
                    //GrdaddplanConfrim.Columns[4].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("netmrp")).Sum().ToString();
                    if (TblPlanAddfinal.Rows.Count > 0)
                    {
                        GrdaddplanConfrim.Columns[1].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                        GrdaddplanConfrim.Columns[2].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("LCO_PRICE")).Sum().ToString();
                        GrdaddplanConfrim.Columns[3].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("BC_PRICE")).Sum().ToString();
                        GrdaddplanConfrim.Columns[7].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("ChannelCount")).Sum().ToString();
                        GrdaddplanConfrim.DataSource = TblPlanAddfinal;
                        GrdaddplanConfrim.DataBind();
                    }
                    else if (TblNCFPlanAddfinal.Rows.Count > 0)
                    {
                        GrdaddplanConfrim.Columns[1].FooterText = TblNCFPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                        GrdaddplanConfrim.Columns[2].FooterText = TblNCFPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("LCO_PRICE")).Sum().ToString();
                        GrdaddplanConfrim.Columns[3].FooterText = TblNCFPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("BC_PRICE")).Sum().ToString();
                        GrdaddplanConfrim.Columns[7].FooterText = TblNCFPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("ChannelCount")).Sum().ToString();
                        GrdaddplanConfrim.DataSource = TblNCFPlanAddfinal;
                        GrdaddplanConfrim.DataBind();
                    }
                    popaddplanconfirm.Show();
                }
                else
                {
                    msgboxstr("Please select Plan");
                }
            }
            catch (Exception ex)
            {
                msgboxstr(ex.ToString());
            }
            //}
            StatbleDynamicTabs();
        }

        protected void GrdaddplanConfrim_RowDataBound(object sender, GridViewRowEventArgs e)  // Added By Vivek 15-Feb-2016
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblautorenew = (Label)e.Row.Cells[3].FindControl("lblautorenew");
                if (lblautorenew.Text.Trim() == "Y")
                {
                    lblautorenew.Text = "YES";
                }
                else if (lblautorenew.Text.Trim() == "N")
                {
                    lblautorenew.Text = "NO";
                }
                else
                {
                    lblautorenew.Text = "-";
                }
            }

        }

        protected string callGetProviConfirmAddplan(Hashtable htData, string flag)
        {
            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
            Hashtable ht = new Hashtable();
            ht["username"] = username;
            ht["lco_id"] = oper_id;
            ht["cust_no"] = ViewState["customer_no"].ToString();
            ht["vc_id"] = ViewState["vcid"].ToString();
            ht["cust_name"] = lblCustName.Text;
            ht["plan_id"] = htData["planpoid"];
            ht["flag"] = flag;
            ht["IP"] = htData["IP"];
            if (htData["expiry"] != null)
            {
                ht["expiry"] = htData["expiry"];
            }
            else
            {
                ht["expiry"] = ""; // add plan
            }
            if (htData["activation"] != null)
            {
                ht["activation"] = htData["activation"];
            }
            else
            {
                ht["activation"] = ""; // add plan
            }

            //---------------------changed on 03/01/2014 to manage existing plans
            if (htData["existing_poids"] != null)
            {
                ht["existing_poids"] = htData["existing_poids"];
            }
            else
            {
                ht["existing_poids"] = ""; // add plan
            }

            //------------08072019

            if (ViewState["Alignflag"] != null)
            {
                ht["Alignflag"] = ViewState["Alignflag"];
            }
            else
            {
                ht["Alignflag"] = ""; // add alignment flag
            }


            if (ViewState["BasicExpiry"] != null)
            {
                ht["BasicExpiry"] = ViewState["BasicExpiry"];
            }
            else
            {
                ht["BasicExpiry"] = ""; // add basic plan's expiry date
            }



            if (ViewState["BasicPoid"] != null)
            {
                ht["BasicPoid"] = ViewState["BasicPoid"];
            }
            else
            {
                ht["BasicPoid"] = ""; // basic plan's id
            }
            //---- for Validate Active Channels
            try
            {
                ht["ActivePlanPoid"] = Convert.ToString(ViewState["ActivePOIDs"]).Split('~')[0];
            }
            catch (Exception)
            {
                ht["ActivePlanPoid"] = "";
            }
            //--------------------



            string result = obj.getProviConfirm(ht);
            return result;
        }

        protected void btnaddplanConfirm_Click(object sender, EventArgs e)
        {

            /*if (ViewState["freeplanpoid"] != null)  // added by vivek 04-Jan-2016
            {
                if (ViewState["BasicPlanAlreadyThere"] != null)
                {
                    if (ViewState["CancelSelectedFOCPlanId"] != null)   // added by vivek 15-Jan-2016
                    {
                        SendFOCCancelRequestOneByOne();
                    }
                }
            }


            if (ViewState["BasciaddcancelFOC"] != null)
            {
                if (ViewState["BasciaddcancelFOC"].ToString() == "Y")
                {
                    SendFOCCancelRequestOneByOne();
                }
            }

            if (ViewState["cancelfocpack"] != null)
            {
                SendFOCCancelRequestOneByOne();
            }
            */
            try
            {
                ViewState["cancelfocpack"] = null;
                ViewState["PlanPoids"] = "";
                ViewState["ValidsPlanPoids"] = "";
                foreach (GridViewRow gr in GrdaddplanConfrim.Rows)
                {
                    HiddenField hdnplanaddconfPlanPoid = (HiddenField)gr.Cells[3].FindControl("hdnplanaddconfPlanPoid");
                    HiddenField hdnplanaddconfautorenew = (HiddenField)gr.Cells[3].FindControl("hdnplanaddconfautorenew");
                    HiddenField hdnplanaddconfplantype = (HiddenField)gr.Cells[3].FindControl("hdnplanaddconfplantype");
                    HiddenField hdnfoctype = (HiddenField)gr.Cells[3].FindControl("hdnfoctype");
                    HiddenField hdnCode = (HiddenField)gr.Cells[3].FindControl("hdnCode");//
                    if (hdnCode.Value == "9999")
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("planpoid", hdnplanaddconfPlanPoid.Value.Trim());

                        ht.Remove("planname");
                        ht.Add("planname", HttpUtility.HtmlDecode(gr.Cells[0].Text));
                        /*if (HttpUtility.HtmlDecode(gr.Cells[0].Text).Contains("NCF"))
                        {
                            //Session["PlanPoidsNEw"] = hdnplanaddconfPlanPoid.Value.Trim() + "," + ViewState["PlanPoids"];
                            //ViewState["PlanPoids"] = Session["PlanPoidsNEw"];
                            //Session["PlanPoidsNEw"] = "";
                        
                            if ( ViewState["strNCFPlanListNew"] != null)
                            {
                                ViewState["PlanPoids"] = ViewState["strNCFPlanListNew"].ToString() + "," + ViewState["PlanPoids"];
                                ViewState["strNCFPlanListNew"] = null;
                            }
                        }
                        else
                        {*/
                        ViewState["PlanPoids"] += hdnplanaddconfPlanPoid.Value.Trim() + ",";
                        //}

                        if (!HttpUtility.HtmlDecode(gr.Cells[0].Text).Contains("NCF 1"))
                        {
                            ViewState["ValidsPlanPoids"] = hdnplanaddconfPlanPoid.Value + "," + ViewState["ValidsPlanPoids"];
                        }
                        ViewState["transaction_data"] = ht;
                        hdnPopupAction.Value = "A";

                        if (hdnplanaddconfplantype.Value == "B")
                        {
                            ViewState["basic_poids"] = hdnplanaddconfPlanPoid.Value.Trim();
                        }
                        if (hdnplanaddconfplantype.Value == "AD" && HttpUtility.HtmlDecode(gr.Cells[0].Text).Contains("FREE"))
                        {

                            if (hdnfoctype.Value == "N")
                            {
                                ViewState["bucket1foc"] = Convert.ToInt32(ViewState["bucket1foc"]) + 1;
                            }
                            else if (hdnfoctype.Value == "Y")
                            {
                                ViewState["bucket2foc"] = Convert.ToInt32(ViewState["bucket2foc"]) + 1;
                            }
                        }
                        hdnPopupAutoRenew.Value = hdnplanaddconfautorenew.Value.Trim();

                        //DataRow row2 = dtFOCPlanStatus.NewRow();
                        //row2["PlanName"] = HttpUtility.HtmlDecode(gr.Cells[0].Text);
                        //row2["RenewStatus"] = ViewState["ErrorMessage"];
                        //dtFOCPlanStatus.Rows.Add(row2);
                    }
                }
                if (ViewState["PlanPoids"] == null)
                {
                    msgboxstr(" Please select valid Plan's for Process ...");
                    return;
                }

                processTransactionValidfirst();

                if (ViewState["ErrorMessage"] == null)
                {
                    ProcessTransactionAPICALL();
                    if (ViewState["ErrorMessage"] == null)
                    {
                        ProcessTransactionFinal();

                        lblAllStatus.Text = "Add Plan Pack Status";
                        //Gridrenew.DataSource = dtFOCPlanStatus;
                        //Gridrenew.DataBind();
                        Gridrenew.Visible = false;
                        lblPlanStatus.Text = hdnBasicPoidAddResponse.Value;
                        popallrenewal.Show();
                        //renewFOCAfterBasicRenew();
                        StatbleDynamicTabs();
                        string updated_bal = getAvailableBal();
                        if (Convert.ToString(Session["category"]) == "11")
                        {
                            lbllcobalance.Text = "Available Balance : " + updated_bal;
                        }
                        else
                        {
                            lblAvailBal.Text = updated_bal;
                        }
                    }
                    else
                    {
                        msgboxstr(ViewState["ErrorMessage"].ToString());
                    }
                }
                else
                {
                    msgboxstr(ViewState["ErrorMessage"].ToString());
                }
                ViewState["TblPlanAddfinal"] = null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void setPopup(string message1, string message2, string flag, string grid, Hashtable ht)
        {
            ViewState["transaction_data"] = null;
            lblPopupText1.Text = message1;
            lblPopupText2.Text = message2;
            hdnPopupAction.Value = flag;
            hdnPopupType.Value = grid;
            hdnPopupAutoRenew.Value = ht["autorenew"].ToString();
            lblPopupCustNo.Text = lblCustNo.Text;
            //lblPopupAmount.Text = ht["lcoprice"].ToString();
            lblPopupAmount.Text = ht["custprice"].ToString();
            lblpopuplcoamt.Text = ht["lcoprice"].ToString();
            lblPopupPlanName.Text = ht["planname"].ToString();
            /* if (flag == "R")
             {
                 lblPopupDiscount.Text = ht["discountamt"].ToString();
                 RenewDiscount.Visible = true;
             }
             else
             {*/
            lblPopupDiscount.Text = "";
            RenewDiscount.Visible = false;
            //}
            ht["trans_flag"] = flag;

            if (ht["activation"] != null && ht["expiry"] != null)
            {
                lblPopupFrom.Text = ht["activation"].ToString();
                lblPopupTo.Text = ht["expiry"].ToString();
                lblPopupFrom.Visible = true;
                lblPopupTo.Visible = true;
                trExp.Visible = true;
                trAct.Visible = true;
            }
            else
            {
                lblPopupFrom.Text = "";
                lblPopupTo.Text = "";
                lblPopupFrom.Visible = false;
                lblPopupTo.Visible = false;
                trExp.Visible = false;
                trAct.Visible = false;
            }
            //hiding reason row on popup
            if (flag == "C")
            {
                if (ht["autorenew"] == "Y" && ViewState["alacartebaseplan"].ToString() == "N")
                {
                    trPopupAutorenew.Visible = true;
                    lblPopupAutorenew.Text = "This plan will not autorenew.";
                }
                else
                {
                    trPopupAutorenew.Visible = false;
                    lblPopupAutorenew.Text = "";
                }
                trPopupCancelReason.Visible = true;
                trPopupCancelDaysLeft.Visible = true;
                trPopupCancellcoRefund.Visible = true;
                ddlPopupReason.SelectedValue = "0";
                lblPopupDaysleft.Text = Convert.ToString(ht["days_left"]);
                trPopupCancelRefund.Visible = true;
                lblPopupRefund.Text = Convert.ToString(ht["refund_amt"]);
                lblPopuplcoRefund.Text = Convert.ToString(ht["refund_lcoamt"]);
            }
            else
            {
                if (ht["autorenew"] == "Y")
                {
                    trPopupAutorenew.Visible = true;
                    lblPopupAutorenew.Text = "This plan will autorenew.";
                }
                else
                {
                    trPopupAutorenew.Visible = false;
                    lblPopupAutorenew.Text = "";
                }
                trPopupCancelReason.Visible = false;
                trPopupCancelDaysLeft.Visible = false;
                trPopupCancellcoRefund.Visible = false;
                lblPopupDaysleft.Text = "";
                trPopupCancelRefund.Visible = false;
                lblPopupRefund.Text = "";
                lblPopuplcoRefund.Text = "";
            }
            tblRenew.Visible = false;
            ViewState["transaction_data"] = ht;
        }//shri

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchOperators(string prefixText, int count, string plan_type)
        {
            return null;
            //string Str = prefixText.Trim();
            //double Num;
            //bool isNum = double.TryParse(Str, out Num);
            //if (!isNum)
            //{
            //    string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            //    OracleConnection con = new OracleConnection(strCon);
            //    string str = "";
            //    if (plan_type == "AD")
            //    {
            //        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
            //            " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_fetch a " +
            //            " where a.cityid ='" + city + "' and a.plan_type='AD'" +
            //            " and upper(a.plan_name) like upper('" + prefixText + "%') " +
            //            " and a.plan_poid not in (" + addon_poids + ")" +
            //            " and  a.plan_poid2 in (" + basic_poids + ")";
            //        //str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, " +
            //        //    " a.lco_price, a.payterm, a.cityid, a.city_name, a.company_code, a.insby, a.insdt " +
            //        //    " FROM view_lcopre_plan_fetch a" +
            //        //    " where a.cityid ='" + city + "' and a.plan_type='" + plantype + "' " +
            //        //    " and upper(a.plan_name) like upper('" + prefixText + "%')" +
            //        //    " and a.plan_poid not in (" + addon_poids + ")";
            //    }
            //    else if (plan_type == "AL")
            //    {
            //        //str = " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
            //        //" FROM view_lcopre_plan_fetchold a " +
            //        //" where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + prefixText + "%')  " +
            //        //" and a.plan_poid not in (" + ala_poids + ") " +
            //        //" ) minus  " +
            //        //" (" +
            //        //" SELECT  a.plan_id,a.plan_name , a.cust_price, a.plan_poid, 'AL', a.deal_poid, a.lco_price from view_lcopre_plan_fetchold a " +
            //        //" where a.plan_id in  (  Select c.num_channel_planid  from view_lcopre_plan_fetchold a , aoup_lcopre_planchnnel_map b, aoup_lcopre_channel_def c  " +
            //        //" where a.plan_type in ('B','AD') " +
            //        //" and a.plan_id=b.num_planchannel_plan_id " +
            //        //" and b.num_planchannel_channel_id = c.num_channel_id " +
            //        //" and c.num_channel_planid is not null " +
            //        //" and a.plan_poid  in (" + ala_poids + ")  ) ) ";

            //        str =  " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
            //               " FROM view_lcopre_plan_fetchnew a " +
            //               " where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + prefixText + "%')  " +
            //               " and a.plan_poid not in (" + ala_poids + ") " +
            //               " ) minus  " +
            //               " ( " +
            //               " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice " +
            //               " FROM aoup_lcopre_plan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_plan_def c " +
            //               "  where a.var_plan_name=b.var_plan_name " +
            //               " and c.var_plan_proviid=b.var_plan_provi " +
            //               "  AND b.var_plan_city=a.num_plan_cityid " +
            //               " and a.var_plan_plantype in ('AD','B') " +
            //               " and c.var_plan_plantype='AL' " +
            //               " and a.var_plan_planpoid in(" + ala_poids + ") " +
            //               " and c.num_plan_cityid='" + city + "' " +
            //               " ) ";
            //    }
            //    OracleCommand cmd = new OracleCommand(str, con);
            //    con.Open();
            //    List<string> Operators = new List<string>();
            //    OracleDataReader dr = cmd.ExecuteReader();
            //    //string item = "";
            //    while (dr.Read())
            //    {
            //        //item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
            //        //    dr["plan_name"].ToString(), dr["plan_id"].ToString());
            //        //Operators.Add(item);
            //        Operators.Add(string.Format("{0}-{1}", dr["plan_name"].ToString(), dr["plan_id"].ToString()));
            //    }
            //    con.Close();
            //    return Operators;
            //}
            //else
            //    return null;
        }

        public void msgbox(string message, Control ctrl)
        {
            //lnkatag_Click(null, null);
            lblPopupResponse.Text = message;
            btnRefreshForm.Visible = true;
            btnClodeMsg.Visible = false;
            popMsg.Show();
            popAdd.Hide(); //ajax control toolkit bug quickfix
            ctrl.Focus();
            StatbleDynamicTabs();
        }

        public void msgboxstr(string message)
        {
            // lnkatag_Click(null, null);
            lblPopupResponse.Text = message;
            btnRefreshForm.Visible = false;
            popMsg.Show();
            popAdd.Hide(); //ajax control toolkit bug quickfix
            StatbleDynamicTabs();
        }

        public void msgboxstr_refresh(string message)
        {
            lblPopupResponse.Text = message;
            btnRefreshForm.Visible = true;
            btnClodeMsg.Visible = false;
            imgClose2.Visible = false;
            btnRefreshForm.Visible = true;
            popMsg.Show();
            popAdd.Hide(); //ajax control toolkit bug quickfix
            StatbleDynamicTabs();
            string updated_bal = getAvailableBal();
            if (Convert.ToString(Session["category"]) == "11")
            {
                lbllcobalance.Text = "Available Balance : " + updated_bal;
            }
            else
            {
                lblAvailBal.Text = updated_bal;
            }
        }

        protected void btnsearchplan_Click(object sender, EventArgs e)
        {
            bool checkLcosgare = GetLCOSharesPrice();

            if (checkLcosgare == true)
            {
                //cbAddPlanAutorenew.Checked = false;
                //trAddplanAutorenew.Visible = true;
                popAdd.Show();
                return;
            }

            //  lnkatag_Click(null, null);
            //if (grdPlanChan.SelectedItem.Text.Trim() != "")
            //{
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            string search_text = "";//grdPlanChan.SelectedItem.Text.Trim();
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string basic_poids = "'0'";
            if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            {
                basic_poids = ViewState["basic_poids"].ToString();
            }
            string addon_poids = "'0'";
            if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
            {
                addon_poids = ViewState["addon_poids"].ToString();
            }
            string ala_poids = "'0'";
            if (ViewState["ala_poids"] != null && ViewState["ala_poids"].ToString() != "")
            {
                ala_poids = ViewState["ala_poids"].ToString();
            }

            string PlanType = "";
            if (radPlanAD.Checked == true)
            {
                PlanType = "GAD";
            }
            else if (radPlanADreg.Checked == true)
            {
                PlanType = "RAD";
            }
            // if (RadPlantype.SelectedValue == "AD")
            if (radPlanAD.Checked)
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                       " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_jvplan_fetch a " +
                       " where a.cityid ='" + city + "' and a.plan_type_new='GAD'" +   //a.plan_type in ('RAD','GAD') need to remove
                       " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                       " and a.plan_poid not in (" + addon_poids + ")" +
                       " and  a.plan_poid2 in (" + basic_poids + ")";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_fetch a " +
                        " where a.cityid ='" + city + "' and a.plan_type_new='GAD'" +   //a.plan_type in ('RAD','GAD') need to remove
                        " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        " and a.plan_poid not in (" + addon_poids + ")" +
                        " and  a.plan_poid2 in (" + basic_poids + ")";
                }

            }
            else if (radPlanADreg.Checked)
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                       " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_jvplan_fetch a " +
                       " where a.cityid ='" + city + "' and a.plan_type_new='RAD'" +   //a.plan_type in ('RAD','GAD') need to remove
                       " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                       " and a.plan_poid not in (" + addon_poids + ")" +
                       " and  a.plan_poid2 in (" + basic_poids + ")";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_fetch a " +
                        " where a.cityid ='" + city + "' and a.plan_type_new='RAD'" +   //a.plan_type in ('RAD','GAD') need to remove
                        " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        " and a.plan_poid not in (" + addon_poids + ")" +
                        " and  a.plan_poid2 in (" + basic_poids + ")";
                }

            }
            else if (radPlanAL.Checked)
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                             " FROM view_lcopre_plan_jvfetchnew a " +
                            " where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + search_text + "%')  " +
                             " and a.plan_poid not in (" + ala_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                             " ) minus  " +
                             " ( " +
                             " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice " +
                             " FROM aoup_lcopre_jvplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_jvplan_def c " +
                             "  where a.var_plan_name=b.var_plan_name " +
                             " and c.var_plan_proviid=b.var_plan_provi " +
                             "  AND b.var_plan_city=a.num_plan_cityid " +
                             " and a.var_plan_plantype in ('AD','B') " +
                             " and c.var_plan_plantype='AL' " +
                             " and a.var_plan_planpoid in(" + ala_poids + ") and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                             " and c.num_plan_cityid='" + city + "' " +
                             " ) ";

                }
                else
                {
                    str = " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                           " FROM view_lcopre_plan_fetchnew a " +
                          " where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + search_text + "%')  " +
                           " and a.plan_poid not in (" + ala_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                           " ) minus  " +
                           " ( " +
                           " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice " +
                           " FROM aoup_lcopre_plan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_plan_def c " +
                           "  where a.var_plan_name=b.var_plan_name " +
                           " and c.var_plan_proviid=b.var_plan_provi " +
                           "  AND b.var_plan_city=a.num_plan_cityid " +
                           " and a.var_plan_plantype in ('AD','B') " +
                           " and c.var_plan_plantype='AL' " +
                           " and a.var_plan_planpoid in(" + ala_poids + ") and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                           " and c.num_plan_cityid='" + city + "' " +
                           " ) ";
                }
            }
            else if (radPlanBasic.Checked)
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                       " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_fetch_jvbasic a " +
                       " where a.cityid ='" + city + "' and a.plan_type='B'" +
                       " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                       " and a.plan_poid not in (" + basic_poids + ")";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_fetch_basic a " +
                        " where a.cityid ='" + city + "' and a.plan_type='B'" +
                        " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        " and a.plan_poid not in (" + basic_poids + ")";

                }


            }
            OracleCommand cmd = new OracleCommand(str, con);
            con.Open();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //lblplanname.Text = dr["plan_name"].ToString();
                //lblplanamt.Text = dr["lco_price"].ToString();
                //lblplanamt.Text = dr["cust_price"].ToString();
                ViewState["SearchedPoid"] = dr["plan_poid"].ToString();
                //-------------Kiran
                ViewState["SearchedPlanid"] = dr["plan_id"].ToString();
                ViewState["SearchedPlanName"] = dr["plan_name"].ToString();
                ViewState["SearchedPlanType"] = dr["plan_type"].ToString();
                ViewState["SearchedDealPoid"] = dr["deal_poid"].ToString();
                ViewState["SearchedCustPrice"] = dr["cust_price"].ToString();
                ViewState["SearchedLcoPrice"] = dr["lco_price"].ToString();
                // ViewState["SearchedActivation"] = dr["var_plan_planpoid"].ToString();
                // ViewState["SearchedExpiry"] = dr["var_plan_planpoid"].ToString();
                //=======================
            }
            if (!dr.HasRows)
            {
                if (radPlanAD.Checked)
                {
                    msgbox("No Such Plan Found", grdPlanChan);
                }
                if (radPlanAL.Checked)
                {
                    msgbox("No Such channel Found", grdPlanChan);
                }
                if (radPlanBasic.Checked)
                {
                    msgbox("No Such channel Found", grdPlanChan);
                }
                // trAddplanAutorenew.Visible = false;
            }
            else
            {
                //cbAddPlanAutorenew.Checked = false;
                //trAddplanAutorenew.Visible = true;
                popAdd.Show();
            }
            con.Close();
            //}
            //else
            //{
            //    msgbox("Please Enter Plan Name", grdPlanChan);
            //}
        }//shri

        public bool GetLCOSharesPrice()
        {

            bool CheckData = false;


            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            string search_text = "";// grdPlanChan.SelectedItem.Text.Trim();
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string basic_poids = "'0'";
            if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            {
                basic_poids = ViewState["basic_poids"].ToString();
            }
            string addon_poids = "'0'";
            if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
            {
                addon_poids = ViewState["addon_poids"].ToString();
            }
            string ala_poids = "'0'";
            if (ViewState["ala_poids"] != null && ViewState["ala_poids"].ToString() != "")
            {
                ala_poids = ViewState["ala_poids"].ToString();
            }

            // if (RadPlantype.SelectedValue == "AD")
            if (radPlanAD.Checked)
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                      " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcojvplan_fetch a " +
                      " where a.cityid ='" + city + "' and a.PLAN_TYPE_NEW='GAD'" +
                      " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "' and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                      " and a.plan_poid not in (" + addon_poids + ")";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fetch a " +
                        " where a.cityid ='" + city + "' and a.PLAN_TYPE_NEW='GAD'" +
                        " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "' and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                        " and a.plan_poid not in (" + addon_poids + ")";

                }
            }
            else if (radPlanADreg.Checked)
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                    " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcojvplan_fetch a " +
                    " where a.cityid ='" + city + "' and a.plan_type_new='RAD'" +
                    " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "' and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                    " and a.plan_poid not in (" + addon_poids + ")";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fetch a " +
                        " where a.cityid ='" + city + "' and a.plan_type_new='RAD'" +
                        " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "' and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                        " and a.plan_poid not in (" + addon_poids + ")";

                }
            }
            else if (radPlanAL.Checked)
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                           " FROM view_lcopre_lcoplan_jvfetchnew a " +
                          " where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + search_text + "%')  " +
                           " and a.lcocode='" + Session["lcoid"].ToString() + "' and a.plan_poid not in (" + ala_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                           " ) minus  " +
                           " ( " +
                           " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice " +
                           " FROM aoup_lcopre_lcojvplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_lcojvplan_def c " +
                           "  where a.var_plan_name=b.var_plan_name " +
                           " and c.var_plan_proviid=b.var_plan_provi " +
                           "  AND b.var_plan_city=a.num_plan_cityid " +
                           " and a.var_plan_plantype in ('AD','B') " +
                           " and c.var_plan_plantype='AL' " +
                           " and a.var_plan_planpoid in(" + ala_poids + ") and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                            " and a.var_plan_lcocode='" + Session["lcoid"].ToString() + "'" +
                           " and c.num_plan_cityid='" + city + "' " +
                           " ) ";
                }
                else
                {
                    str = " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                           " FROM view_lcopre_lcoplan_fetchnew a " +
                          " where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + search_text + "%')  " +
                           " and a.lcocode='" + Session["lcoid"].ToString() + "' and a.plan_poid not in (" + ala_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                           " ) minus  " +
                           " ( " +
                           " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice " +
                           " FROM aoup_lcopre_lcoplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_lcoplan_def c " +
                           "  where a.var_plan_name=b.var_plan_name " +
                           " and c.var_plan_proviid=b.var_plan_provi " +
                           "  AND b.var_plan_city=a.num_plan_cityid " +
                           " and a.var_plan_plantype in ('AD','B') " +
                           " and c.var_plan_plantype='AL' " +
                           " and a.var_plan_planpoid in(" + ala_poids + ") and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                            " and a.var_plan_lcocode='" + Session["lcoid"].ToString() + "'" +
                           " and c.num_plan_cityid='" + city + "' " +
                           " ) ";
                }
            }
            else if (radPlanBasic.Checked)
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                       " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fch_JVbasi a " +
                       " where a.cityid ='" + city + "' and a.plan_type='B'" +
                       " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "' and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                       " and a.plan_poid not in (" + basic_poids + ")";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fetch_basi a " +
                        " where a.cityid ='" + city + "' and a.plan_type='B'" +
                        " and upper(a.plan_name) like upper('" + search_text + "') and a.dasarea='" + Session["dasarea"].ToString() + "' and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                        " and a.plan_poid not in (" + basic_poids + ")";
                }

            }
            OracleCommand cmd = new OracleCommand(str, con);
            con.Open();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //lblplanname.Text = dr["plan_name"].ToString();
                //lblplanamt.Text = dr["lco_price"].ToString();
                //lblplanamt.Text = dr["cust_price"].ToString();
                ViewState["SearchedPoid"] = dr["plan_poid"].ToString();
                //-------------Kiran
                ViewState["SearchedPlanid"] = dr["plan_id"].ToString();
                ViewState["SearchedPlanName"] = dr["plan_name"].ToString();
                ViewState["SearchedPlanType"] = dr["plan_type"].ToString();
                ViewState["SearchedDealPoid"] = dr["deal_poid"].ToString();
                ViewState["SearchedCustPrice"] = dr["cust_price"].ToString();
                ViewState["SearchedLcoPrice"] = dr["lco_price"].ToString();
                // ViewState["SearchedActivation"] = dr["var_plan_planpoid"].ToString();
                // ViewState["SearchedExpiry"] = dr["var_plan_planpoid"].ToString();
                //=======================
            }

            if (!dr.HasRows)
            {
                CheckData = false;
            }
            else
            {
                CheckData = true;
            }
            con.Close();

            return CheckData;
        }

        //sets final confirmation box
        protected void setFinalConfirmation()
        {
            Hashtable htData;
            string message = "Are you sure you want to ";
            if (ViewState["transaction_data"] != null)
            {
                htData = ViewState["transaction_data"] as Hashtable;
            }
            else
            {
                return;
            }
            if (htData["autorenew"].ToString() == "N")
            {
                trpopFinalConfAutorenewMsg.Visible = false;
            }
            if (htData["trans_flag"].ToString() == "A")
            {
                if (htData["autorenew"].ToString() == "Y")
                {
                    lblPopupAutoRenewMsg.Text = "This plan will autorenew.";
                }
                message += " add the plan - " + htData["planname"] + ".";
            }
            if (htData["trans_flag"].ToString() == "R")
            {
                if (htData["autorenew"].ToString() == "Y")
                {
                    lblPopupAutoRenewMsg.Text = "This plan will autorenew.";
                }
                message += " renew the plan  ";// + htData["planname"] + ".";
            }
            if (htData["trans_flag"].ToString() == "C")
            {
                if (htData["autorenew"].ToString() == "Y" && ViewState["alacartebaseplan"].ToString() == "N")
                {
                    lblPopupAutoRenewMsg.Text = "This plan will not autorenew.";
                }
                message += " cancel the plan - " + htData["planname"];//+ ".\nRefund amount is " + htData["refund_amt"] + " and " + htData["days_left"] + " days left.";
            }
            lblPopupFinalConfMsg.Text = message;
        }//shri

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            setFinalConfirmation();
            //--------------------Check reason is selected or not in case of cancel operation-----------------------
            Hashtable htData;
            if (ViewState["transaction_data"] != null)
            {
                htData = ViewState["transaction_data"] as Hashtable;
                if (htData["trans_flag"].ToString() == "C")
                {
                    if (ddlPopupReason.SelectedValue.Trim() == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert(\"Select Reason\");", true);
                        pop.Show();
                        StatbleDynamicTabs();
                        return;
                    }
                }
            }
            else
            {
                StatbleDynamicTabs();
                return;
            }
            //-----------------------------------------------Reason checking end---------------------------------------

            popFinalConf.Show();
            StatbleDynamicTabs();
        }//shri

        protected void btnALPack_Click(object sender, EventArgs e)  //Added By Vivek 12-Feb-2016
        {
            nullFOCViewState();
            lbltoatlalbaseremain.Text = (Convert.ToDouble(ViewState["Alacartebasevalue"]) - Convert.ToDouble(ViewState["alacartevalue"])).ToString("0.00");
            double Total = Convert.ToDouble(ViewState["alacartevalue"]);
            ViewState["Total"] = ViewState["alacartevalue"];
            btnalfreeadd.Visible = false;
            btnalfreeclose.Visible = false;

            lblALtotal.Text = Total.ToString("0.00") + "/-";

            hdnalacartebaseaddplanpoid.Value = "";

            int rindex = (((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex;

            HiddenField hdnBasicPlanPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanPoid");

            fillFreeALPlanGrid(hdnBasicPlanPoid.Value.ToString());

            if (grdALfree.Rows.Count > 0)
            {
                ViewState["BasicPlanAlreadyThere"] = "Y";
                btnalfreeadd.Visible = true;
                btnalfreeclose.Visible = true;
                PopUpALFreePlan.Show();
                StatbleDynamicTabs();

                return;
            }
            else
            {
                lblPopupResponse.Text = "A-La-Carte Free Channels Plan not found!";
                btnRefreshForm.Visible = false;
                btnalfreeadd.Visible = false;
                btnalfreeclose.Visible = false;
                popMsg.Show();
                StatbleDynamicTabs();
                PopUpALFreePlan.Hide();
                return;
            }
        }

        public void fillFreeALPlanGrid(String Basicpoid)
        {
            ViewState["Addplanpoids"] = "";
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string str = "";
            string basic_poids = "'0'";
            if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            {
                basic_poids = ViewState["basic_poids"].ToString();
            }
            string ala_poids = "'0'";
            if (ViewState["ala_poidsFree"] != null && ViewState["ala_poidsFree"].ToString() != "")
            {
                ala_poids = ViewState["ala_poidsFree"].ToString();
            }
            string DeviceDefinitionType = "SD";
            string hd_where_clause = "";
            if (ViewState["DeviceDefinitionType"] != null && ViewState["DeviceDefinitionType"] != "")
            {
                DeviceDefinitionType = ViewState["DeviceDefinitionType"].ToString();
            }
            if (!DeviceDefinitionType.Contains("HD"))
            {
                hd_where_clause = " and a.device_type <> 'HD' ";
            }
            //ddlPlanChan.Items.Clear();
            try
            {
                if (Session["JVFlagS"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,'N' ALACARTEBASE,base_price  " +
                                       " FROM view_lcopre_JVplan_fchnew_free a " +
                                      " where a.plan_type='AL' and a.cityid ='" + city + "' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                       " and a.plan_poid not in (" + ala_poids + ") " +
                                       " and a.plan_name like '%FREE%'" +// and a.payterm ='1'
                                       "  order by plan_name asc";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,'N' ALACARTEBASE,base_price  " +
                                       " FROM view_lcopre_plan_fetchnew_free a " +
                                      " where a.plan_type='AL' and a.cityid ='" + city + "' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                       " and a.plan_poid not in (" + ala_poids + ") " +
                                       " and a.plan_name like '%FREE%'" +// and a.payterm ='1'
                                       "  order by plan_name asc";
                }

                DataTable TblPlans = GetResult(str);
                if (TblPlans.Rows.Count > 0)
                {
                    Label7.Visible = false;
                    btnAddPlan.Visible = true;
                    grdALfree.DataSource = TblPlans;
                    grdALfree.DataBind();
                    //ViewState["TblPlans"] = TblPlans;
                }
                else
                {
                    grdALfree.DataSource = null;
                    grdALfree.DataBind();
                }
                // QueryFileLog("Query Log", "Addon Query : ", str, ad_qry_test_plan);
            }
            catch (Exception ex)
            {
                grdALfree.DataSource = null;
                grdALfree.DataBind();
            }
        }

        protected void btnAddALFreePlan_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow gr in grdALfree.Rows)
            {
                CheckBox ChkPlanAdd = (CheckBox)gr.Cells[3].FindControl("cbALFreePlan");


                if (ChkPlanAdd.Checked == true)
                {
                    HiddenField hdnplanaddconfPlanPoid = (HiddenField)gr.Cells[3].FindControl("hdnALFreePlanPoid");
                    Hashtable ht = new Hashtable();
                    ht.Add("planpoid", hdnplanaddconfPlanPoid.Value.Trim());
                    ht.Add("planname", gr.Cells[0].Text);
                    ViewState["transaction_data"] = ht;
                    hdnPopupAction.Value = "A";

                    hdnPopupAutoRenew.Value = "N";
                    processTransaction();



                    DataRow row2 = dtFOCPlanStatus.NewRow();
                    row2["PlanName"] = HttpUtility.HtmlDecode(gr.Cells[0].Text);
                    row2["RenewStatus"] = ViewState["ErrorMessage"];
                    dtFOCPlanStatus.Rows.Add(row2);
                }
            }

            lblAllStatus.Text = "Add Plan Pack Status";
            Gridrenew.DataSource = dtFOCPlanStatus;
            Gridrenew.DataBind();
            popallrenewal.Show();
            StatbleDynamicTabs();
            string updated_bal = getAvailableBal();
            if (Convert.ToString(Session["category"]) == "11")
            {
                lbllcobalance.Text = "Available Balance : " + updated_bal;
            }
            else
            {
                lblAvailBal.Text = updated_bal;
            }
        }

        protected void cbALFreePlan_Changed(object sender, EventArgs e)
        {
            GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;

            int rowindex = row.RowIndex;

            HiddenField hdnplan = (HiddenField)grdALfree.Rows[rowindex].FindControl("hdnALFreePlanPoid");
            CheckBox ChkPlanAdd = (CheckBox)grdALfree.Rows[rowindex].FindControl("cbALFreePlan");
            Double grdmrp = Convert.ToDouble(grdALfree.Rows[rowindex].Cells[1].Text);

            lbltotalbaseprice.Text = Convert.ToDouble(ViewState["Alacartebasevalue"]).ToString();
            if (ChkPlanAdd.Checked == true)
            {
                String albasealapoid = hdnalacartebaseaddplanpoid.Value;
                albasealapoid = albasealapoid + hdnplan.Value.Trim() + ",";
                hdnalacartebaseaddplanpoid.Value = albasealapoid;

                Double Total = 0;
                Double Alacartebasemrpotal = 0;


                Total = Convert.ToDouble(ViewState["Total"]) + grdmrp;
                ViewState["Total"] = Total;
                Alacartebasemrpotal += Total;

                lblALtotal.Text = Total + "/-";

                lbltoatlalbaseremain.Text = (Convert.ToDouble(ViewState["Alacartebasevalue"]) - Total).ToString("0.00");
                StatbleDynamicTabs();
                PopUpALFreePlan.Show();

                if (ViewState["alacartebaseplan"].ToString() == "Y")
                {
                    if (Alacartebasemrpotal > Convert.ToDouble(ViewState["Alacartebasevalue"]))
                    {
                        albasealapoid = hdnalacartebaseaddplanpoid.Value;
                        albasealapoid = albasealapoid.Replace(hdnplan.Value.Trim() + ",", "");
                        hdnalacartebaseaddplanpoid.Value = albasealapoid;

                        Total = 0;
                        Total = Convert.ToDouble(ViewState["Total"]) - grdmrp;
                        if (Total < 0)
                        {
                            Total = 0;
                        }

                        lblALtotal.Text = Total + "/-";
                        ViewState["Total"] = Total;
                        lbltoatlalbaseremain.Text = (Convert.ToDouble(ViewState["Alacartebasevalue"]) - Total).ToString("0.00");
                        lblalacartetext.Text = "Crossed the maximum allotted limit for A-la-Carte channels.";
                        popalacrtebaseplan.Show();
                        PopUpALFreePlan.Hide();
                        ChkPlanAdd.Checked = false;
                        StatbleDynamicTabs();
                        return;
                    }
                }

            }
            else
            {
                String albasealapoid = hdnalacartebaseaddplanpoid.Value;
                albasealapoid = albasealapoid.Replace(hdnplan.Value.Trim() + ",", "");
                hdnalacartebaseaddplanpoid.Value = albasealapoid;

                Double Total = 0;
                Total = Convert.ToDouble(ViewState["Total"]) - grdmrp;
                if (Total < 0)
                {
                    Total = 0;
                }
                lbltoatlalbaseremain.Text = (Convert.ToDouble(ViewState["Alacartebasevalue"]) - Total).ToString("0.00");
                lblALtotal.Text = Total + "/-";
                ViewState["Total"] = Total;
                StatbleDynamicTabs();
                PopUpALFreePlan.Show();
            }
        }

        protected void resetAddPopup()
        {
            ViewState["BasicPlanChangeWithFOC"] = null;
            AddExpiredplan.Visible = true;
            //radPlanBasic.Visible = false;
            radPlanAD.Checked = false;
            radPlanADreg.Checked = false;
            radPlanAL.Checked = false;
            radhwayspecial.Checked = false;
            lbltotaladd.Text = "0.00/-";
            hdntotaladdamount.Value = "0";
            ViewState["Total"] = "0";
            /*  if (Convert.ToString(ViewState["alacartebaseplan"]) == "Y")
              {
                  if (ViewState["alacartevalue"] == null)
                  {
                      lbltotaladd.Text = ViewState["alacartevalue"].ToString() + "/-";
                      hdntotaladdamount.Value = ViewState["alacartevalue"].ToString();
                      ViewState["Total"] = ViewState["alacartevalue"].ToString();
                  }
                  else
                  {
                      lbltotaladd.Text = "0.00/-";
                      hdntotaladdamount.Value = "0";
                      ViewState["Total"] = "0";
                  }

                  radPlanAD.Checked = false;
                  radPlanBasic.Checked = false;
                  radPlanAL.Checked = true;

                  radPlanAL_CheckedChanged(radPlanAL, new EventArgs());
                  plantype = "AL";
                  radPlanBasic.Enabled = false;
                  radPlanAD.Enabled = false;
                  radPlanADreg.Enabled = false;
                  radhwayspecial.Enabled = false;
                  tr1.Visible = false;


              }
              else*/
            if (grdBasicPlanDetails.Rows.Count > 0 && (Boolean)ViewState["BasepalnExpired"] == false || Grdhathwayspecial.Rows.Count > 0)
            {
                Boolean ChkPlanType = false;
                foreach (GridViewRow gr in grdBasicPlanDetails.Rows)
                {
                    HiddenField hdnplanaddconfPlanPoid = (HiddenField)gr.Cells[12].FindControl("hdnBasicPlanType");
                    // FileLogTextChange1("Plan Type", hdnplanaddconfPlanPoid.Value, "", "");
                    if (hdnplanaddconfPlanPoid.Value == "B")
                    {
                        ChkPlanType = true;
                    }
                }
                if (ChkPlanType == false)
                {
                    if (Grdhathwayspecial.Rows.Count > 0)
                    {
                        radPlanAD.Checked = false;
                        radPlanBasic.Enabled = false;
                        radhwayspecial.Enabled = false;
                        radPlanAL.Enabled = true;
                        radPlanAD.Enabled = true;
                        radPlanAD.Checked = true;
                        radPlanAD_CheckedChanged(radPlanAD, new EventArgs());
                        plantype = "HSP";
                    }
                    else
                    {
                        radPlanAD.Enabled = true;
                        radPlanAL.Enabled = true;
                        radhwayspecial.Enabled = true;
                        radhwayspecial.Checked = false;
                        radPlanAD.Checked = false;
                        radPlanAL.Checked = false;
                        radPlanBasic.Checked = true;
                        radPlanBasic.Enabled = true;
                        radPlanBasic_CheckedChanged(radPlanBasic, new EventArgs());
                        plantype = "B";
                    }
                }
                else
                {
                    radPlanAD.Checked = false;
                    radPlanBasic.Enabled = false;
                    radhwayspecial.Enabled = false;
                    radPlanAL.Enabled = true;
                    radPlanAD.Enabled = true;
                    radPlanAD.Checked = true;
                    radPlanAD_CheckedChanged(radPlanAD, new EventArgs());
                    plantype = "HSP";
                }
            }
            else
            {
                radPlanAD.Enabled = true;
                radPlanAL.Enabled = true;
                radhwayspecial.Enabled = true;
                radhwayspecial.Checked = false;
                radPlanAD.Checked = false;
                radPlanAL.Checked = false;
                radPlanBasic.Checked = true;
                radPlanBasic.Enabled = true;
                radPlanBasic_CheckedChanged(radPlanBasic, new EventArgs());
                plantype = "B";
            }
            if (grdPlanChan.Rows.Count > 0)
            {
                Label7.Visible = false;
                btnAddPlan.Visible = true;
            }
            else
            {
                Label7.Visible = true;
                btnAddPlan.Visible = false;
            }
            //lblplanname.Text = "";
            //lblplanamt.Text = "";
            if (ViewState["SearchedPoid"] != null)
            {
                ViewState.Remove("SearchedPoid");
            }
            if (ViewState["SearchedPlanid"] != null)
            {
                ViewState.Remove("SearchedPlanid");
            }
            if (ViewState["SearchedPlanName"] != null)
            {
                ViewState.Remove("SearchedPlanName");
            }
            if (ViewState["SearchedPlanType"] != null)
            {
                ViewState.Remove("SearchedPlanType");
            }
            if (ViewState["SearchedDealPoid"] != null)
            {
                ViewState.Remove("SearchedDealPoid");
            }
            if (ViewState["SearchedCustPrice"] != null)
            {
                ViewState.Remove("SearchedCustPrice");
            }
            if (ViewState["SearchedLcoPrice"] != null)
            {
                ViewState.Remove("SearchedLcoPrice");
            }
            // ScriptManager.RegisterClientScriptBlock(this, GetType(), "jsBindAutocomplete", "bindAutocomplete('AD');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "jsBindAutocomplete", "bindAutocomplete('AD');", true);

        }//shri

        protected void Button2_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            resetAddPopup();
            popAdd.Show();
        }

        protected void btnOpenAddPopup_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            //if (ViewState["BasicActionFlag"] != null)
            //{
            //    if (ViewState["BasicActionFlag"].ToString() == "EX")     // created by vivek 16-nov-2015
            //    {
            //        btnRefreshForm.Visible = false;
            //        lblPopupResponse.Text = "Basic Pack is expired,you can't add the new plans";
            //        popMsg.Show();
            //        StatbleDynamicTabs();
            //        return;
            //    }
            //}

            resetAddPopup();
            //popAdd.Show();
            //StatbleDynamicTabs();
        }//shri
        public string IsSaveplan = "0";
        protected void btnRefreshForm_Click(object sender, EventArgs e)
        {
            // lnkatag_Click(null, null);
            //refreshing page to get updated service details
            btnRefreshForm.Visible = false;
            btnClodeMsg.Visible = true;
            imgClose2.Visible = true;
            radPlanBasic.Checked = true;
            radPlanAD.Checked = false;
            radPlanAL.Checked = false;
            IsSaveplan = "1";
            btnSearch_Click(null, new EventArgs());
            IsSaveplan = "0";
            //StatbleDynamicTabs();
        }

        protected void btnPopupFinalConfYes_Click(object sender, EventArgs e)
        {

            /*if (ViewState["CheckMaintvBasicCount"].ToString() == "0" && ViewState["CheckchildtvBasicCount"].ToString() == "YES")
            {
                btnRefreshForm.Visible = false;
                lblPopupResponse.Text = "Base Package in Main Connection is Inactive";
                popMsg.Show();
                StatbleDynamicTabs();
                return;
            }*/


            if (ViewState["freeplanpoid"] != null)  // added by vivek 04-Jan-2016
            {
                if (ViewState["BasicPlanAlreadyThere"] != null)
                {
                    if (ViewState["CancelSelectedFOCPlanId"] != null)   // added by vivek 15-Jan-2016
                    {
                        if (ViewState["alacartechangenew"] == null || ViewState["alacartechangenew"].ToString() == "N")
                        {
                            SendFOCCancelRequestOneByOne();
                        }
                    }
                }

                Hashtable htPlanData = new Hashtable();
                string[] planpoid = ViewState["freeplanpoid"].ToString().Split(',');
                string[] Planname = ViewState["freeplanname"].ToString().Split(',');
                string[] changebuketcount = ViewState["changebuketcount"].ToString().Split(',');
                for (int i = 0; i < planpoid.Length; i++)
                {
                    hdnPopupAutoRenew.Value = "N";
                    hdnPopupAction.Value = "A";


                    if (i == 1)
                    {
                        if (ViewState["BasicPlanAlreadyThere"] == null && ViewState["ChangedBasicPlanFOCChange"] == null)
                        {
                            if (hdnBasicPoidAddResponse.Value != "Success")
                            {
                                msgboxstr(ViewState["ErrorMessage"].ToString());
                                popFinalConf.Hide();
                                StatbleDynamicTabs();
                                return;
                            }
                            //else
                            //{
                            //    SendFOCCancelRequestOneByOne(); // added by vivek 19-Jan-2016                                
                            //}
                        }
                    }


                    htPlanData.Clear();
                    htPlanData.Add("planpoid", planpoid[i].ToString().Trim());
                    htPlanData.Remove("planname");
                    htPlanData.Add("planname", Planname[i].ToString().Trim());
                    ViewState["transaction_data"] = htPlanData;
                    try
                    {
                        ViewState["bucket1foc"] = changebuketcount[i].ToString().Trim();
                    }
                    catch { }
                    processTransaction();

                    DataRow row1 = dtFOCPlanStatus.NewRow();
                    row1["PlanName"] = Planname[i].ToString();
                    row1["RenewStatus"] = ViewState["ErrorMessage"];
                    dtFOCPlanStatus.Rows.Add(row1);
                }

                Gridrenew.DataSource = dtFOCPlanStatus;
                Gridrenew.DataBind();
                lblAllStatus.Text = "Pack Status";
                Gridrenew.HeaderRow.Cells[1].Text = "Status";
                popMsg.Hide();
                popallrenewal.Show();
            }
            //Vivek End Here

            else
            {
                if (hdnPopupAction.Value.ToString() == "C")//Added by Vivek Singh on 14-Jul-2016 for cancellation of all the plan if basic plan get cancelled   
                {
                    Hashtable htPlanData = new Hashtable();
                    if (ViewState["transaction_data"] != null)
                    {
                        htPlanData = ViewState["transaction_data"] as Hashtable;
                        processTransaction();
                    }

                    if (htPlanData["plantypevalue"] != null)
                    {
                        if (htPlanData["plantypevalue"].ToString() == "B" || htPlanData["plantypevalue"].ToString() == "HSP")
                        {
                            string _tvType = "";
                            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                            string _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                            if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                            {
                                _tvType = "MAIN";
                            }
                            else
                            {
                                _tvType = "CHILD";
                            }

                            Cancellation_Process(htPlanData["planpoid"].ToString(), _tvType, htPlanData["planname"].ToString(), _vc_id);
                        }
                        else
                        {
                            processTransaction();
                        }
                    }

                    else
                    {
                        processTransaction();
                    }
                }
                else
                {
                    Hashtable htPlanData = ViewState["transaction_data"] as Hashtable;

                    foreach (GridViewRow gr in GrdRenewConfrim2.Rows)
                    {
                        HiddenField hdnplanaddconfPlanPoid = (HiddenField)gr.Cells[6].FindControl("hdnplanrenewconfPlanPoid");

                        processTransactionRenew(hdnplanaddconfPlanPoid.Value);
                    }


                }
            }
            renewFOCAfterBasicRenew();
            StatbleDynamicTabs();
            string updated_bal = getAvailableBal();
            if (Convert.ToString(Session["category"]) == "11")
            {
                lbllcobalance.Text = "Available Balance : " + updated_bal;
            }
            else
            {
                lblAvailBal.Text = updated_bal;
            }
        }//shri

        private void renewFOCAfterBasicRenew()
        {
            try
            {
                if (ViewState["renewfoc"].ToString() == "Y")
                {
                    int count = 0;
                    foreach (GridViewRow gvrow in grdBasicPlanDetails.Rows)
                    {
                        CheckBox chk = (CheckBox)gvrow.FindControl("cbBasicrenew");
                        if (chk != null & chk.Checked)
                        {
                            HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                            if (hdnBasicPlanName.Value.ToString().ToUpper().Contains("FREE"))
                            {
                                ((CheckBox)gvrow.FindControl("cbAddonrenew")).Checked = true;
                                count++;
                            }
                        }
                    }
                    if (count > 0)
                    {
                        btnRenSubmit_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }


        // for Renewal
        protected void processTransactionRenew(string Poid)
        {
            try
            {
                ViewState["ErrorMessage"] = null;
                //gathering data
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string transaction_action = hdnPopupAction.Value; //A-Add , C-Cancel, R-Renew
                string transaction_type = hdnPopupType.Value; //grid from where function is called 

                string plan_poid = "";
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string _STB_NO = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";
                string _foccount = "";
                string maintStatus = "INACTIVE";
                if (Session["username"] != null && Session["operator_id"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["lcoid"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }


                try
                {
                    DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                    DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();

                    DataTable myResultSet_flag = sortedDT.Select("PARENT_CHILD_FLAG='0'").CopyToDataTable();

                    _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                    _STB_NO = myResultSet.Rows[0]["STB_NO"].ToString();
                    if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                    {
                        _tvType = "MAIN";
                    }
                    else
                    {
                        _tvType = "CHILD";
                    }
                    if (myResultSet_flag.Rows[0]["Status"].ToString() == "10100")
                    {
                        maintStatus = "ACTIVE";
                    }
                    else
                    {
                        maintStatus = "INACTIVE";
                    }

                    if (ViewState["AddedFOC"] == null)
                    {
                        ViewState["AddedFOC"] = "0";
                    }
                }
                catch (Exception ex)
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";
                    msgboxstr("Something went wrong while transaction. Plan data not found.");
                    Cls_Security objSecurity = new Cls_Security();
                    objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransaction");
                    return;
                }

                if (ViewState["transaction_data"] != null)
                {
                    htPlanData = ViewState["transaction_data"] as Hashtable;
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";
                    msgboxstr("Something went wrong while transaction. Plan data not found.");
                    return;
                }

                //processing 
                string Request = "";
                //validating...
                if (transaction_action == "A")
                {
                    //if add action
                    // plan_poid = ViewState["SearchedPoid"].ToString();
                    plan_poid = htPlanData["planpoid"].ToString();
                    activation_date = "";
                    expiry_date = "";
                }
                else
                {
                    //if cancel and renew
                    plan_poid = Poid;//htPlanData["planpoid"].ToString();
                    activation_date = htPlanData["activation"].ToString();
                    expiry_date = htPlanData["expiry"].ToString();
                    if (transaction_action == "C")
                    {
                        reason_id = ddlPopupReason.SelectedValue;
                        reason_text = ddlPopupReason.SelectedItem.Text;
                    }
                }

                if (ViewState["ServicePoid"] != null && ViewState["accountPoid"] != null)
                {
                    ht.Add("username", _username);
                    ht.Add("lcoid", _oper_id);
                    ht.Add("custid", lblCustNo.Text.Trim());
                    ht.Add("vcid", _vc_id);
                    ht.Add("STBNO", _STB_NO);
                    ht.Add("custname", lblCustName.Text.Trim());
                    ht.Add("cust_addr", lblCustAddr.Text.Trim());
                    ht.Add("planid", plan_poid);
                    ht.Add("flag", transaction_action);
                    ht.Add("expdate", expiry_date);
                    ht.Add("actidate", activation_date);
                    ht.Add("request", Request);
                    ht.Add("reason_id", reason_id);
                    ht.Add("IP", Ip);

                    ht.Add("MainTVStatus", maintStatus);
                    ht.Add("TVType", _tvType);
                    ht.Add("DeviceType", ViewState["Device_Type"].ToString());


                    ht.Add("FOCCount", ViewState["AddedFOC"].ToString());

                    ht.Add("BasicPoid", ViewState["basic_poids"].ToString());
                    ht.Add("addon_poids", Convert.ToString(ViewState["addon_poids"]).Replace("'", ""));
                    ht.Add("Speacial", "N");
                    ht.Add("bucket1foc", Convert.ToString(ViewState["bucket1foc"]));
                    ht.Add("bucket2foc", Convert.ToString(ViewState["bucket2foc"]));
                    if (grdAddOnPlan.Rows.Count > 0)
                    {
                        if (grdBasicPlanDetails.Rows.Count > 0)
                        {
                            foreach (GridViewRow grb in grdBasicPlanDetails.Rows)
                            {
                                string plannameb = "";
                                plannameb = grb.Cells[0].Text.ToString();

                                if (plannameb.Contains("BASIC"))
                                {
                                    foreach (GridViewRow gr in grdAddOnPlan.Rows)
                                    {
                                        string planname = "";
                                        planname = gr.Cells[0].Text.ToString();

                                        if (planname.Contains("SPECIAL"))
                                        {

                                            ht.Remove("Speacial");
                                            ht.Add("Speacial", "Y");
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    string response = obj.ValidateProvTrans(ht);

                    string[] res = response.Split('$');
                    if (res[0] != "9999")
                    {

                        if (Convert.ToString(htPlanData["planname"]).Contains("FREE"))
                        {
                            ht.Add("mrp", 1);
                            response = obj.ValidateProvTransFoc2(ht);
                            res = response.Split('$');
                            if (res[0] != "9999")
                            {
                                ViewState["ErrorMessage"] = res[1].ToString();
                                msgboxstr(res[1]);
                                return;
                            }
                            else
                            {
                                request_id = res[1];
                            }
                        }
                        else
                        {
                            ViewState["ErrorMessage"] = res[1].ToString();
                            msgboxstr(res[1]);
                            return;
                        }
                    }
                    else
                    {
                        request_id = res[1];
                    }
                }
                else
                {
                    ViewState["ErrorMessage"] = "something went wrong, Service or account details not found...Please relogin";
                    msgboxstr("something went wrong, Service or account details not found...Please relogin");
                    return;
                }

                //OBRM call to get response...
                // Request = _username + "$" + Request;
                if (transaction_action == "A")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid;
                }
                else if (transaction_action == "C")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["packageid"] + "$" + htPlanData["dealpoid"];
                }
                else if (transaction_action == "R")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["purchasepoid"];
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction.";
                    msgboxstr("Something went wrong while transaction.");
                    return;
                }


                Request = _user_brmpoid + "$" + Request;
                string req_code = "";
                if (transaction_action == "A")
                {
                    req_code = "5";
                }
                else if (transaction_action == "R")
                {
                    req_code = "7";
                }
                else if (transaction_action == "C")
                {
                    req_code = "8";
                }
                //FileLogTextChange("Admin", "MainCancelRequest", " Error:" + Request, "");
                string api_response = callAPI(Request, req_code);
                //string api_response = "0$ACCOUNT - Service add plan completed successfully$0.0.0.1 /account 81788441 9$0.0.0.1 /service/catv 81788185 39";
                string[] final_obrm_status = api_response.Split('$');
                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";

                try
                {
                    if (obrm_status == "0" || obrm_status == "1")
                    {
                        obrm_msg = final_obrm_status[2];
                        ViewState["OBRMMessage"] = "";
                    }
                    else
                    {
                        obrm_status = "1";
                        obrm_msg = api_response;
                    }
                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    ViewState["ErrorMessage"] = api_response;
                    obrm_msg = api_response;
                }

                ht.Add("obrmsts", obrm_status);
                ht.Add("request_id", request_id);
                ht.Add("response", api_response);
                //----------    for child TV discount ... added by RP on 09052019
                ht.Add("BoxType", Session["BoxType"]);
                ht.Add("TVConnection", Session["TVConnection"]);
                //--------
                Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
                string resp = obj1.ProvTransRes(ht); // "9999";
                string[] finalres = resp.Split('$');
                if (finalres[0] == "9999")
                {
                    try
                    {
                        string autorenew_flag = hdnPopupAutoRenew.Value;//Y-Autorenew, N-no Autorenew
                        ht.Add("autorenew", autorenew_flag);
                        string Expdate = "";
                        //FileLogText("ECS-Flag", username, autorenew_flag, "");//

                        if (transaction_action != "C" && autorenew_flag == "Y")
                        {
                            string response_params = _user_brmpoid + "$" + lblCustNo.Text + "$SW$" + _vc_id + "$" + plan_poid;
                            //FileLogText("ECS-Request", username, response_params, "");//
                            //apiResponse =  Service_poid + "$" + account_poid + "$" + Plan_Poid + "$" + Pur_expdt + "$" + Pur_strtdt + "$" + Deal_poid + "$" + Pkg_id + "$" + Pur_Poid;
                            string apiResponse = callAPI(response_params, "14");
                            //FileLogText("ECS-Api", username, apiResponse, "");//
                            if (apiResponse.Split('$')[0] != "*")
                            {
                                Expdate = apiResponse.Split('$')[3];
                                //FileLogText("ECS-Expdate", username, Expdate, "");//
                            }
                            ht.Remove("expdate");
                            ht.Add("expdate", Expdate);
                            string resp1 = obj1.ProvECSSingle(ht);
                            //FileLogText("ECS-!C", username, resp1, "");//
                        }
                        else if (transaction_action == "C")
                        {
                            string resp1 = obj1.ProvECSSingle(ht);
                            //FileLogText("ECS-C", username, resp1, "");//
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewState["ErrorMessage"] = ex.ToString();
                        //FileLogText("ECS", username, ex.ToString(), "");
                    }
                    msgboxstr_refresh("Transaction successful : " + obrm_msg);//
                    hdnBasicPoidAddResponse.Value = "Success";  //Added by vivek 05-Jan-2016
                    ViewState["ErrorMessage"] = "Transaction successful : " + obrm_msg;//
                }
                else
                {
                    if (obrm_status == "0")
                    {
                        msgboxstr_refresh("Transaction successful by OBRM but failure at Atyeti : " + finalres[1]);
                        hdnBasicPoidAddResponse.Value = "Transaction successful by OBRM but failure at Atyeti : " + finalres[1];
                        ViewState["ErrorMessage"] = "Transaction successful by OBRM but failure at Atyeti : " + finalres[1];
                    }
                    else
                    {
                        msgboxstr("Transaction failed : " + finalres[1] + " : " + obrm_msg);
                        hdnBasicPoidAddResponse.Value = "Transaction failed : " + finalres[1] + " : " + obrm_msg;
                        ViewState["ErrorMessage"] = "Transaction failed : " + finalres[1] + " : " + obrm_msg;
                    }
                }

                //gettting balance after above 3 process
                string updated_bal = getAvailableBal();
                if (Convert.ToString(Session["category"]) == "11")
                {
                    lbllcobalance.Text = "Available Balance : " + updated_bal;
                }
                else
                {
                    lblAvailBal.Text = updated_bal;
                }
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                ViewState["ErrorMessage"] = ex.Message.ToString();
                objSecurity.InsertIntoDb(Session["lco_username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransaction");
                return;
            }
        }

        protected void processTransaction()
        {
            try
            {
                ViewState["ErrorMessage"] = null;
                //gathering data
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string transaction_action = hdnPopupAction.Value; //A-Add , C-Cancel, R-Renew
                string transaction_type = hdnPopupType.Value; //grid from where function is called 

                string plan_poid = "";
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";
                string _foccount = "";
                string maintStatus = "INACTIVE";
                if (Session["username"] != null && Session["operator_id"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["lcoid"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }


                try
                {
                    DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                    DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();

                    DataTable myResultSet_flag = sortedDT.Select("PARENT_CHILD_FLAG='0'").CopyToDataTable();

                    _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                    if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                    {
                        _tvType = "MAIN";
                    }
                    else
                    {
                        _tvType = "CHILD";
                    }
                    if (myResultSet_flag.Rows[0]["Status"].ToString() == "10100")
                    {
                        maintStatus = "ACTIVE";
                    }
                    else
                    {
                        maintStatus = "INACTIVE";
                    }

                    if (ViewState["AddedFOC"] == null)
                    {
                        ViewState["AddedFOC"] = "0";
                    }
                }
                catch (Exception ex)
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";
                    msgboxstr("Something went wrong while transaction. Plan data not found.");
                    Cls_Security objSecurity = new Cls_Security();
                    objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransaction");
                    return;
                }

                if (ViewState["transaction_data"] != null)
                {
                    htPlanData = ViewState["transaction_data"] as Hashtable;
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";
                    msgboxstr("Something went wrong while transaction. Plan data not found.");
                    return;
                }

                //processing 
                string Request = "";
                //validating...
                if (transaction_action == "A")
                {
                    //if add action
                    // plan_poid = ViewState["SearchedPoid"].ToString();
                    plan_poid = htPlanData["planpoid"].ToString();
                    activation_date = "";
                    expiry_date = "";
                }
                else
                {
                    //if cancel and renew
                    plan_poid = htPlanData["planpoid"].ToString();
                    activation_date = htPlanData["activation"].ToString();
                    expiry_date = htPlanData["expiry"].ToString();
                    if (transaction_action == "C")
                    {
                        reason_id = ddlPopupReason.SelectedValue;
                        reason_text = ddlPopupReason.SelectedItem.Text;
                    }
                }

                if (ViewState["ServicePoid"] != null && ViewState["accountPoid"] != null)
                {
                    ht.Add("username", _username);
                    ht.Add("lcoid", _oper_id);
                    ht.Add("custid", lblCustNo.Text.Trim());
                    ht.Add("vcid", _vc_id);
                    ht.Add("custname", lblCustName.Text.Trim());
                    ht.Add("cust_addr", lblCustAddr.Text.Trim());
                    ht.Add("planid", plan_poid);
                    ht.Add("flag", transaction_action);
                    ht.Add("expdate", expiry_date);
                    ht.Add("actidate", activation_date);
                    ht.Add("request", Request);
                    ht.Add("reason_id", reason_id);
                    ht.Add("IP", Ip);

                    ht.Add("MainTVStatus", maintStatus);
                    ht.Add("TVType", _tvType);
                    ht.Add("DeviceType", ViewState["Device_Type"].ToString());


                    ht.Add("FOCCount", ViewState["AddedFOC"].ToString());

                    ht.Add("BasicPoid", ViewState["basic_poids"].ToString());
                    ht.Add("addon_poids", ViewState["addon_poids"].ToString().Replace("'", ""));

                    ht.Add("Speacial", "N");
                    ht.Add("bucket1foc", Convert.ToString(ViewState["bucket1foc"]));
                    ht.Add("bucket2foc", Convert.ToString(ViewState["bucket2foc"]));
                    if (grdAddOnPlan.Rows.Count > 0)
                    {
                        if (grdBasicPlanDetails.Rows.Count > 0)
                        {
                            foreach (GridViewRow grb in grdBasicPlanDetails.Rows)
                            {
                                string plannameb = "";
                                plannameb = grb.Cells[0].Text.ToString();

                                if (plannameb.Contains("BASIC"))
                                {
                                    foreach (GridViewRow gr in grdAddOnPlan.Rows)
                                    {
                                        string planname = "";
                                        planname = gr.Cells[0].Text.ToString();

                                        if (planname.Contains("SPECIAL"))
                                        {

                                            ht.Remove("Speacial");
                                            ht.Add("Speacial", "Y");
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    string response = obj.ValidateProvTrans(ht);

                    string[] res = response.Split('$');
                    if (res[0] != "9999")
                    {

                        if (Convert.ToString(htPlanData["planname"]).Contains("FREE"))
                        {
                            ht.Add("mrp", 1);
                            response = obj.ValidateProvTransFoc2(ht);
                            res = response.Split('$');
                            if (res[0] != "9999")
                            {
                                ViewState["ErrorMessage"] = res[1].ToString();
                                msgboxstr(res[1]);
                                return;
                            }
                            else
                            {
                                request_id = res[1];
                            }
                        }
                        else
                        {
                            ViewState["ErrorMessage"] = res[1].ToString();
                            msgboxstr(res[1]);
                            return;
                        }
                    }
                    else
                    {
                        request_id = res[1];
                    }
                }
                else
                {
                    ViewState["ErrorMessage"] = "something went wrong, Service or account details not found...Please relogin";
                    msgboxstr("something went wrong, Service or account details not found...Please relogin");
                    return;
                }

                //OBRM call to get response...
                // Request = _username + "$" + Request;
                if (transaction_action == "A")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + Session["ChannelCount"].ToString();
                }
                else if (transaction_action == "C")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["packageid"] + "$" + htPlanData["dealpoid"] + "$" + Session["ChannelCount"].ToString();
                }
                else if (transaction_action == "R")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["purchasepoid"];
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction.";
                    msgboxstr("Something went wrong while transaction.");
                    return;
                }


                Request = _user_brmpoid + "$" + Request;
                string req_code = "";
                if (transaction_action == "A")
                {
                    req_code = "5";
                }
                else if (transaction_action == "R")
                {
                    req_code = "7";
                }
                else if (transaction_action == "C")
                {
                    req_code = "8";
                }
                //FileLogTextChange("Admin", "MainCancelRequest", " Error:" + Request, "");
                string api_response = callAPI(Request, req_code);
                //string api_response = "0$ACCOUNT - Service add plan completed successfully$0.0.0.1 /account 81788441 9$0.0.0.1 /service/catv 81788185 39";
                string[] final_obrm_status = api_response.Split('$');
                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";

                try
                {
                    if (obrm_status == "0" || obrm_status == "1")
                    {
                        obrm_msg = final_obrm_status[2];
                        ViewState["OBRMMessage"] = "";
                    }
                    else
                    {
                        obrm_status = "1";
                        obrm_msg = api_response;
                    }
                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    ViewState["ErrorMessage"] = api_response;
                    obrm_msg = api_response;
                }

                ht.Add("obrmsts", obrm_status);
                ht.Add("request_id", request_id);
                ht.Add("response", api_response);
                //----------    for child TV discount ... added by RP on 09052019
                ht.Add("BoxType", Session["BoxType"]);
                ht.Add("TVConnection", Session["TVConnection"]);
                //-
                Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
                string resp = obj1.ProvTransRes(ht); // "9999";
                string[] finalres = resp.Split('$');
                if (finalres[0] == "9999")
                {
                    try
                    {
                        string autorenew_flag = hdnPopupAutoRenew.Value;//Y-Autorenew, N-no Autorenew
                        ht.Add("autorenew", autorenew_flag);
                        string Expdate = "";
                        //FileLogText("ECS-Flag", username, autorenew_flag, "");//

                        if (transaction_action != "C" && autorenew_flag == "Y")
                        {
                            string response_params = _user_brmpoid + "$" + lblCustNo.Text + "$SW$" + _vc_id + "$" + plan_poid;
                            //FileLogText("ECS-Request", username, response_params, "");//
                            //apiResponse =  Service_poid + "$" + account_poid + "$" + Plan_Poid + "$" + Pur_expdt + "$" + Pur_strtdt + "$" + Deal_poid + "$" + Pkg_id + "$" + Pur_Poid;
                            string apiResponse = callAPI(response_params, "14");
                            //FileLogText("ECS-Api", username, apiResponse, "");//
                            if (apiResponse.Split('$')[0] != "*")
                            {
                                Expdate = apiResponse.Split('$')[3];
                                //FileLogText("ECS-Expdate", username, Expdate, "");//
                            }
                            ht.Remove("expdate");
                            ht.Add("expdate", Expdate);
                            string resp1 = obj1.ProvECSSingle(ht);
                            //FileLogText("ECS-!C", username, resp1, "");//
                        }
                        else if (transaction_action == "C")
                        {
                            string resp1 = obj1.ProvECSSingle(ht);
                            //FileLogText("ECS-C", username, resp1, "");//
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewState["ErrorMessage"] = ex.ToString();
                        //FileLogText("ECS", username, ex.ToString(), "");
                    }
                    msgboxstr_refresh("Transaction successful : " + obrm_msg);
                    hdnBasicPoidAddResponse.Value = "Success";  //Added by vivek 05-Jan-2016
                    ViewState["ErrorMessage"] = "Transaction successful : " + obrm_msg;
                }
                else
                {
                    if (obrm_status == "0")
                    {
                        msgboxstr_refresh("Transaction successful by OBRM but failure at Atyeti : " + finalres[1]);
                        hdnBasicPoidAddResponse.Value = "Transaction successful by OBRM but failure at Atyeti : " + finalres[1];
                        ViewState["ErrorMessage"] = "Transaction successful by OBRM but failure at Atyeti : " + finalres[1];
                    }
                    else
                    {
                        msgboxstr("Transaction failed : " + finalres[1] + " : " + obrm_msg);
                        hdnBasicPoidAddResponse.Value = "Transaction failed : " + finalres[1] + " : " + obrm_msg;
                        ViewState["ErrorMessage"] = "Transaction failed : " + finalres[1] + " : " + obrm_msg;
                    }
                }

                //gettting balance after above 3 process
                string updated_bal = getAvailableBal();
                if (Convert.ToString(Session["category"]) == "11")
                {
                    lbllcobalance.Text = "Available Balance : " + updated_bal;
                }
                else
                {
                    lblAvailBal.Text = updated_bal;
                }
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                ViewState["ErrorMessage"] = ex.Message.ToString();
                objSecurity.InsertIntoDb(Session["lco_username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransaction");
                return;
            }
        }

        // MultiPlan validation ....
        protected void processTransactionValidfirst()
        {
            try
            {
                ViewState["ErrorMessage"] = null;
                //gathering data
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string transaction_action = hdnPopupAction.Value; //A-Add , C-Cancel, R-Renew
                string transaction_type = hdnPopupType.Value; //grid from where function is called 

                string plan_poid = "";
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";
                string _foccount = "";
                string maintStatus = "INACTIVE";
                if (Session["username"] != null && Session["operator_id"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["lcoid"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }


                try
                {
                    DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                    DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();

                    DataTable myResultSet_flag = sortedDT.Select("PARENT_CHILD_FLAG='0'").CopyToDataTable();

                    _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                    if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                    {
                        _tvType = "MAIN";
                    }
                    else
                    {
                        _tvType = "CHILD";
                    }
                    if (myResultSet_flag.Rows[0]["Status"].ToString() == "10100")
                    {
                        maintStatus = "ACTIVE";
                    }
                    else
                    {
                        maintStatus = "INACTIVE";
                    }

                    if (ViewState["AddedFOC"] == null)
                    {
                        ViewState["AddedFOC"] = "0";
                    }
                }
                catch (Exception ex)
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";
                    msgboxstr("Something went wrong while transaction. Plan data not found.");
                    Cls_Security objSecurity = new Cls_Security();
                    objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransaction");
                    return;
                }

                if (ViewState["transaction_data"] != null)
                {
                    htPlanData = ViewState["transaction_data"] as Hashtable;
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";
                    msgboxstr("Something went wrong while transaction. Plan data not found.");
                    return;
                }

                //processing 
                string Request = "";
                //validating...
                if (transaction_action == "A")
                {
                    //if add action
                    // plan_poid = ViewState["SearchedPoid"].ToString();
                    plan_poid = htPlanData["planpoid"].ToString();
                    activation_date = "";
                    expiry_date = "";
                }
                else
                {
                    //if cancel and renew
                    plan_poid = htPlanData["planpoid"].ToString();
                    activation_date = htPlanData["activation"].ToString();
                    expiry_date = htPlanData["expiry"].ToString();
                    if (transaction_action == "C")
                    {
                        reason_id = ddlPopupReason.SelectedValue;
                        reason_text = ddlPopupReason.SelectedItem.Text;
                    }
                }

                if (ViewState["ServicePoid"] != null && ViewState["accountPoid"] != null)
                {
                    ht.Add("username", _username);
                    ht.Add("lcoid", _oper_id);
                    ht.Add("custid", lblCustNo.Text.Trim());
                    ht.Add("vcid", _vc_id);
                    ht.Add("custname", lblCustName.Text.Trim());
                    ht.Add("cust_addr", lblCustAddr.Text.Trim());
                    ht.Add("planid", plan_poid);
                    ht.Add("flag", transaction_action);
                    ht.Add("expdate", expiry_date);
                    ht.Add("actidate", activation_date);
                    ht.Add("request", Request);
                    ht.Add("reason_id", reason_id);
                    ht.Add("IP", Ip);

                    ht.Add("MainTVStatus", maintStatus);
                    ht.Add("TVType", _tvType);
                    ht.Add("DeviceType", ViewState["Device_Type"].ToString());


                    ht.Add("FOCCount", ViewState["AddedFOC"].ToString());

                    ht.Add("BasicPoid", ViewState["basic_poids"].ToString());
                    ht.Add("addon_poids", ViewState["addon_poids"].ToString().Replace("'", ""));

                    ht.Add("Speacial", "N");
                    ht.Add("bucket1foc", Convert.ToString(ViewState["bucket1foc"]));
                    ht.Add("bucket2foc", Convert.ToString(ViewState["bucket2foc"]));
                    try
                    {
                        ht["AlignExpiryFlag"] = ViewState["Alignflag"].ToString();
                        ht["BasicPlanExpairy"] = ViewState["BasicExpiry"];
                        ht["BasicExpairyPlanID"] = ViewState["BasicPoid"];
                    }
                    catch (Exception)
                    {
                        ht["AlignExpiryFlag"] = "N";
                        ht["BasicPlanExpairy"] = "";
                        ht["BasicExpairyPlanID"] = "";
                    }
                    if (grdAddOnPlan.Rows.Count > 0)
                    {
                        if (grdBasicPlanDetails.Rows.Count > 0)
                        {
                            foreach (GridViewRow grb in grdBasicPlanDetails.Rows)
                            {
                                string plannameb = "";
                                plannameb = grb.Cells[0].Text.ToString();

                                if (plannameb.Contains("BASIC"))
                                {
                                    foreach (GridViewRow gr in grdAddOnPlan.Rows)
                                    {
                                        string planname = "";
                                        planname = gr.Cells[0].Text.ToString();

                                        if (planname.Contains("SPECIAL"))
                                        {

                                            ht.Remove("Speacial");
                                            ht.Add("Speacial", "Y");
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();

                    string response = obj.ValidateProvTrans(ht);

                    string[] res = response.Split('$');
                    if (res[0] != "9999")
                    {

                        if (Convert.ToString(htPlanData["planname"]).Contains("FREE"))
                        {
                            ht.Add("mrp", 1);
                            response = obj.ValidateProvTransFoc2(ht);
                            res = response.Split('$');
                            if (res[0] != "9999")
                            {
                                ViewState["ErrorMessage"] = res[1].ToString();
                                msgboxstr(res[1]);
                                return;
                            }
                            else
                            {
                                request_id = res[1];
                            }
                        }
                        else
                        {
                            ViewState["ErrorMessage"] = res[1].ToString();
                            msgboxstr(res[1]);
                            return;
                        }
                    }
                    else
                    {
                        ViewState["transaction_dataValid"] = ht;
                        request_id = res[1];
                    }
                }
                else
                {
                    ViewState["ErrorMessage"] = "something went wrong, Service or account details not found...Please relogin";
                    msgboxstr("something went wrong, Service or account details not found...Please relogin");
                    return;
                }
                //ht.Add("request_id", request_id);

            }
            catch (Exception ex)
            {
                ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";

                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransactionValidfirst");
                msgboxstr("Something went wrong while transaction. Plan data not found.");
                return;
            }
        }
        // MultiPlan Call API ....
        public void ProcessTransactionAPICALL()
        {

            try
            {
                Hashtable htPlanData = ViewState["transaction_dataValid"] as Hashtable;
                string transaction_action = htPlanData["flag"].ToString();
                string _user_brmpoid = Session["user_brmpoid"].ToString();
                htPlanData["PlanPoids"] = ViewState["PlanPoids"].ToString().TrimEnd(',');
                string AlignExpiryFlag = htPlanData["AlignExpiryFlag"].ToString();
                //OBRM call to get response...
                string Request = "";
                if (transaction_action == "A")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + htPlanData["PlanPoids"].ToString() + "$" + Session["ChannelCount"].ToString() + "$" + AlignExpiryFlag;
                }
                else if (transaction_action == "C")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + htPlanData["PlanPoids"].ToString() + "$" + htPlanData["packageid"] + "$" + htPlanData["dealpoid"] + "$" + Session["ChannelCount"].ToString();
                }
                else if (transaction_action == "R")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + htPlanData["PlanPoids"].ToString() + "$" + htPlanData["purchasepoid"];
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction.";
                    msgboxstr("Something went wrong while transaction.");
                    return;
                }


                Request = _user_brmpoid + "$" + Request;
                string req_code = "";
                if (transaction_action == "A")
                {
                    req_code = "5";
                }
                else if (transaction_action == "R")
                {
                    req_code = "7";
                }
                else if (transaction_action == "C")
                {
                    req_code = "8";
                }
                //FileLogTextChange("Admin", "MainCancelRequest", " Error:" + Request, "");
                string api_response = callAPI(Request, req_code);
                string[] final_obrm_status = api_response.Split('$');
                /* try {
                   string str=  final_obrm_status[1].ToString();
                 }
                 catch(Exception ex) 
                 {
                  api_response = callAPI(Request, req_code);
                   final_obrm_status = api_response.Split('$');
                 }
                 try
                 {
                     string str = final_obrm_status[1].ToString();
                 }
                 catch (Exception ex)
                 {
                     api_response = callAPI(Request, req_code);
                     final_obrm_status = api_response.Split('$');
                 }*/
                //string api_response = "0$ACCOUNT - Service add plan completed successfully$0.0.0.1 /account 81788441 9$0.0.0.1 /service/catv 81788185 39";

                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";

                try
                {
                    if (obrm_status == "0")//|| obrm_status == "1"
                    {
                        obrm_msg = final_obrm_status[2];
                        ViewState["OBRMMessage"] = "";
                    }
                    else
                    {
                        obrm_status = "1";
                        obrm_msg = final_obrm_status[2];//api_response;
                        hdnBasicPoidAddResponse.Value = "Transaction failed : " + obrm_msg;
                        ViewState["ErrorMessage"] = "Transaction failed : " + obrm_msg;
                        msgboxstr("Transaction failed From OBRM:  " + obrm_msg);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    ViewState["ErrorMessage"] = "Transaction failed From OBRM:  " + api_response;
                    obrm_msg = api_response;
                    msgboxstr("Transaction failed From OBRM:  " + obrm_msg);
                    return;
                }

                htPlanData.Add("obrmsts", obrm_status);
                //       ht.Add("request_id", request_id);
                htPlanData.Add("response", api_response);
                ViewState["transaction_dataValid"] = htPlanData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                ViewState["ErrorMessage"] = ex.Message.ToString();

                objSecurity.InsertIntoDb(Session["lco_username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransactionAPICALL");
                msgboxstr("Something went wrong while transaction API. :" + ViewState["ErrorMessage"]);
                return;
            }
        }
        // Transaction Process Final Stage.
        public void ProcessTransactionFinal()
        {
            string PlanPOIDs = "";
            string obrm_msg = "";
            string obrm_status = "";
            Hashtable ht = ViewState["transaction_dataValid"] as Hashtable;
            PlanPOIDs = ht["PlanPoids"].ToString();
            obrm_msg = ht["response"].ToString();
            obrm_status = ht["obrmsts"].ToString();

            string transaction_action = ht["flag"].ToString();
            string _vc_id = "";
            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();

            DataTable myResultSet_flag = sortedDT.Select("PARENT_CHILD_FLAG='0'").CopyToDataTable();

            _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
            string plan_poid = "";
            string _user_brmpoid = Session["user_brmpoid"].ToString();
            ht.Add("request_id", "0");
            //----------    for child TV discount ... added by RP on 09052019
            ht.Add("BoxType", Session["BoxType"]);
            ht.Add("TVConnection", Session["TVConnection"]);
            //-----------
            Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
            string resp = obj1.ProvTransRes(ht); // "9999";
            string[] finalres = resp.Split('$');
            if (finalres[0] == "9999")
            {
                try
                {
                    string autorenew_flag = hdnPopupAutoRenew.Value;//Y-Autorenew, N-no Autorenew
                    ht.Add("autorenew", autorenew_flag);
                    string Expdate = "";
                    //FileLogText("ECS-Flag", username, autorenew_flag, "");//

                    if (transaction_action != "C" && autorenew_flag == "Y")
                    {
                        string[] plan_poidlist = PlanPOIDs.Split(',');
                        for (int i = 0; i < plan_poidlist.Length; i++)
                        {
                            string response_params = _user_brmpoid + "$" + lblCustNo.Text + "$SW$" + _vc_id + "$" + plan_poidlist[i].ToString();
                            //FileLogText("ECS-Request", username, response_params, "");//
                            //apiResponse =  Service_poid + "$" + account_poid + "$" + Plan_Poid + "$" + Pur_expdt + "$" + Pur_strtdt + "$" + Deal_poid + "$" + Pkg_id + "$" + Pur_Poid;
                            string apiResponse = callAPI(response_params, "14");
                            //FileLogText("ECS-Api", username, apiResponse, "");//
                            if (apiResponse.Split('$')[0] != "*")
                            {
                                Expdate = apiResponse.Split('$')[3];
                                //FileLogText("ECS-Expdate", username, Expdate, "");//
                            }
                            ht.Remove("expdate");
                            ht.Add("expdate", Expdate);
                            string resp1 = obj1.ProvECSSingle(ht);
                            //FileLogText("ECS-!C", username, resp1, "");// 
                        }
                    }
                    else if (transaction_action == "C")
                    {
                        string resp1 = obj1.ProvECSSingle(ht);
                        //FileLogText("ECS-C", username, resp1, "");//
                    }
                }
                catch (Exception ex)
                {
                    ViewState["ErrorMessage"] = ex.ToString();
                    //FileLogText("ECS", username, ex.ToString(), "");
                }
                //msgboxstr_refresh("Transaction successful : " + obrm_msg);
                hdnBasicPoidAddResponse.Value = "Transaction successful : " + obrm_msg.Split('$')[1];
                ViewState["ErrorMessage"] = "Transaction successful : " + obrm_msg.Split('$')[1];
            }
            else
            {
                if (obrm_status == "0")
                {
                    //msgboxstr_refresh("Transaction successful by OBRM but failure at UPASS : " + finalres[1]);
                    hdnBasicPoidAddResponse.Value = "Transaction successful by OBRM but failure at Atyeti : " + finalres[1];
                    ViewState["ErrorMessage"] = "Transaction successful by OBRM but failure at Atyeti : " + finalres[1];
                }
                else
                {
                    //msgboxstr("Transaction failed : " + finalres[1] + " : " + obrm_msg);
                    hdnBasicPoidAddResponse.Value = "Transaction failed : " + finalres[1] + " : " + obrm_msg.Split('$')[1];
                    ViewState["ErrorMessage"] = "Transaction failed : " + finalres[1] + " : " + obrm_msg.Split('$')[1];
                }
            }

            //gettting balance after above 3 process
            //string updated_bal = getAvailableBal();
            //if (Convert.ToString(Session["category"]) == "11")
            //{
            //   lbllcobalance.Text = "Available Balance : " + updated_bal;
            //}
            //else
            //{
            //  lblAvailBal.Text = updated_bal;
            //}
        }

        protected void grdPlanChan_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            btnsearchplan_Click(btnsearchplan, new EventArgs());
            StatbleDynamicTabs();
        }//shri

        protected void radPlanAll_CheckedChanged(object sender, EventArgs e) //added by vivek 04-Jan-2016
        {
            lbltotaladd.Text = "0.00/-";
            hdntotaladdamount.Value = "0";
            ViewState["Total"] = "0";
            Label7.Visible = false;
            string str = "";
            string city = "";
            pnlBC.Visible = true;
            lblBC.Visible = false;
            ddlBC.Visible = false;
            btnsearchfilter2.Visible = false;
            pnlAL.Visible = false;
            string DeviceDefinitionType = "SD";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string basic_poids = "'0'";
            if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            {
                basic_poids = ViewState["basic_poids"].ToString();
                basic_poids = "'" + basic_poids + "'";
            }
            if (basic_poids != "'0'")
            {

            }
            string addon_poids = "'0'";
            if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
            {
                addon_poids = ViewState["addon_poids"].ToString();
            }
            string hsp_poids = "'0'";
            if (ViewState["hwayspecial_poid"] != null && ViewState["hwayspecial_poid"].ToString() != "")
            {
                hsp_poids = ViewState["hwayspecial_poid"].ToString();
            }
            if (ViewState["DeviceDefinitionType"] != null && ViewState["DeviceDefinitionType"] != "")
            {
                DeviceDefinitionType = ViewState["DeviceDefinitionType"].ToString();
            }
            string PlanTypenew = "";
            if (radPlanAD.Checked == true)
            {
                PlanTypenew = "GAD";
            }
            else if (radPlanADreg.Checked == true)
            {
                PlanTypenew = "RAD";
            }

            grdPlanChan.DataSource = null;
            grdPlanChan.DataBind();
            try
            {
                string hd_where_clause = "";
                if (!DeviceDefinitionType.Contains("HD"))
                {
                    hd_where_clause = " and a.device_type <> 'HD' ";
                }

                string[] addon_poidsarr = addon_poids.Replace("'", "").Split(',');

                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,BC_PRICE, broad_name, genre_type,var_plan_freeflag from(";
                    str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOJVPLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "'";// and a.PLAN_TYPE='" + PlanTypenew + "' ";
                    //if (basic_poids != "'0'")
                    {
                        str += " and a.plan_type not in ('B','HSP')";
                    }
                    str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    //str += " and var_plan_freeflag <> 'Y' ";
                    str += " and lcocode=" + Session["lcoid"].ToString() + hd_where_clause;
                    //str += " and ( addon_plantype is null ";
                    //foreach (string st in addon_poidsarr)
                    //{ 
                    // str += " or INSTR(addon_plantype,'" + st + ",')>0 ";
                    //}
                    //str += " ) ";
                    //str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                    str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_JVPLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "'";// and a.PLAN_TYPE='" + PlanTypenew + "' ";
                    //if (basic_poids != "'0'")
                    {
                        str += " and a.plan_type not in ('B','HSP')";
                    }
                    str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    //str += " and a.plan_poid2 in (" + basic_poids + ")";
                    //str += " and var_plan_freeflag <> 'Y'" ;
                    //str += " and ( addon_plantype is null ";
                    //foreach (string st in addon_poidsarr)
                    //{
                    // str += " or INSTR(addon_plantype,'" + st + ",')>0 ";
                    //}
                    //str += " ) ";
                    str += hd_where_clause;//" and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                    str += " and not EXISTS (select * from VIEW_LCOJVPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                    str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
                    // str += " order by plan_name asc";

                }
                else
                {

                    str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,BC_PRICE, broad_name, genre_type,var_plan_freeflag from(";
                    str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOPLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "' ";//and a.PLAN_TYPE='" + PlanTypenew + "' ";
                    //if (basic_poids != "'0'")
                    {
                        str += " and a.plan_type not in ('B','HSP')";
                    }
                    str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    //str += " and var_plan_freeflag <> 'Y' ";
                    str += " and lcocode=" + Session["lcoid"].ToString() + hd_where_clause;
                    //str += " and ( addon_plantype is null ";
                    //foreach (string st in addon_poidsarr)
                    //{
                    // str += " or INSTR(addon_plantype,'" + st + ",')>0 ";
                    //}
                    //str += " ) ";
                    // str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                    str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_PLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "' ";//and a.PLAN_TYPE='" + PlanTypenew + "' ";
                    //if (basic_poids != "'0'")
                    {
                        str += " and a.plan_type not in ('B','HSP')";
                    }
                    str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    //str += " and a.plan_poid2 in (" + basic_poids + ")";
                    //str += " and var_plan_freeflag <> 'Y'";
                    //str += " and ( addon_plantype is null ";
                    //foreach (string st in addon_poidsarr)
                    //{
                    // str += " or INSTR(addon_plantype,'" + st + ",')>0 ";
                    //}
                    //str += " ) ";
                    str += hd_where_clause;// +" and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                    str += " and not EXISTS (select * from VIEW_LCOPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                    str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
                    //str += " order by plan_name asc";

                }
                //FileLogTextChange1(str, "ALL String", "", "");
                DataTable TblPlans = GetResult(str);
                ViewState["PLanData"] = TblPlans;
                if (TblPlans.Rows.Count > 0)
                {
                    Label7.Visible = false;
                    btnAddPlan.Visible = true;
                    grdPlanChan.DataSource = TblPlans;
                    grdPlanChan.DataBind();

                    // -- for NTO filter
                    //ddlBC2.DataSource = null;
                    //ddlBC2.DataBind();
                    ListItem default_itmBC = new ListItem();
                    default_itmBC.Text = "All";
                    default_itmBC.Value = "0";
                    string StrQry = "SELECT  distinct(a.BROAD_NAME) BROAD_NAME,a.BROAD_NAME BROAD_value FROM  view_lcopre_plan_fetchnew a WHERE a.plan_type not in ('B','HSP') AND a.cityid = '" + city + "'  AND a.dasarea = '" + Session["dasarea"].ToString() + "' and broad_name is not null";
                    //FileLogTextChange1(StrQry, "BC in ALL String", "", "");
                    Cls_Helper ObjHelper = new Cls_Helper();
                    DataTable dtBCData = null;
                    dtBCData = ObjHelper.GetDataTable(StrQry);
                    ddlBC2.DataSource = dtBCData;
                    ddlBC2.DataValueField = "BROAD_value";
                    ddlBC2.DataTextField = "BROAD_NAME";
                    ddlBC2.DataBind();
                    ddlBC2.Items.Insert(0, default_itmBC);
                    string StrQry2 = "SELECT  distinct(a.genre_type) genre_type,a.genre_type genre_value FROM  view_lcopre_plan_fetchnew a WHERE a.plan_type not in ('B','HSP') AND a.cityid = '" + city + "' AND a.dasarea = '" + Session["dasarea"].ToString() + "' and genre_type is not null";
                    DataTable dtGenerData = null;
                    dtGenerData = ObjHelper.GetDataTable(StrQry2);
                    ddlGener.DataSource = dtGenerData;
                    ddlGener.DataValueField = "genre_value";
                    ddlGener.DataTextField = "genre_type";
                    ddlGener.DataBind();
                    ddlGener.Items.Insert(0, default_itmBC);

                }
                else
                {
                    Label7.Visible = true;
                    btnAddPlan.Visible = false;
                    grdPlanChan.DataSource = null;
                    grdPlanChan.DataBind();
                }
                // QueryFileLog("Query Log", "Addon Query : ", str, ad_qry_test_plan);
            }
            catch (Exception ex)
            {
                Label7.Visible = true;
                btnAddPlan.Visible = false;
            }
            popAdd.Show();
            StatbleDynamicTabs();
        }

        protected void radhwayspecial_CheckedChanged(object sender, EventArgs e)
        {
            lbltotaladd.Text = "0.00/-";
            hdntotaladdamount.Value = "0";
            ViewState["Total"] = "0";
            Label7.Visible = false;
            string str = "";
            string city = "";
            string DeviceDefinitionType = "SD";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string basic_poids = "'0'";
            if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            {
                basic_poids = ViewState["basic_poids"].ToString();
            }
            string addon_poids = "'0'";
            if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
            {
                addon_poids = ViewState["addon_poids"].ToString();
            }

            string hsp_poids = "'0'";
            if (ViewState["hwayspecial_poid"] != null && ViewState["hwayspecial_poid"].ToString() != "")
            {
                hsp_poids = ViewState["hwayspecial_poid"].ToString();
            }
            if (ViewState["DeviceDefinitionType"] != null && ViewState["DeviceDefinitionType"] != "")
            {
                DeviceDefinitionType = ViewState["DeviceDefinitionType"].ToString();
            }
            string PlanTypenew = "'HSP'";
            if (basic_poids == "'0'")
            {
                PlanTypenew = "'HSP'";
            }
            pnlBC.Visible = true;
            lblBC.Visible = false;
            ddlBC.Visible = false;
            btnsearchfilter2.Visible = false;
            pnlAL.Visible = false;
            grdPlanChan.DataSource = null;
            grdPlanChan.DataBind();
            try
            {
                string hd_where_clause = "";
                if (!DeviceDefinitionType.Contains("HD"))
                {
                    hd_where_clause = " and a.device_type <> 'HD' ";
                }
                /*
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt from(";
                    str += "  SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cntv  FROM view_lcopre_lcojvplan_fetch a " +
                                " where a.cityid ='" + city + "' and a.PLAN_TYPE='" + PlanTypenew + "' " +
                                " and a.plan_poid not in (" + hsp_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                " and var_plan_freeflag <> 'Y' " +
                                  hd_where_clause +
                                 " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "' and lcocode=" + Session["lcoid"].ToString() +
                            " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt  FROM view_lcopre_jvplan_fetch a " +
                                " where a.cityid ='" + city + "' and a.PLAN_TYPE='" + PlanTypenew + "' " +   //a.plan_type in ('RAD','GAD')  need to remove
                        //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                                " and a.plan_poid not in (" + hsp_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        // " and  a.plan_poid2 in (" + basic_poids + ")" +
                                " and var_plan_freeflag <> 'Y' " +
                                  hd_where_clause +
                                 " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'" +
                                 " and not EXISTS (select * from  view_lcopre_lcojvplan_fetch where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") " +
                                " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
                }
                else
                {
                    str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt from(";
                    str += "  SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt  FROM view_lcopre_lcoplan_fetch a " +
                                " where a.cityid ='" + city + "' and a.PLAN_TYPE='" + PlanTypenew + "' " +
                                " and a.plan_poid not in (" + hsp_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                " and var_plan_freeflag <> 'Y' " +
                                  hd_where_clause +
                                 " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "' and lcocode=" + Session["lcoid"].ToString() +
                            " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt  FROM view_lcopre_plan_fetch a " +
                                " where a.cityid ='" + city + "' and a.PLAN_TYPE='" + PlanTypenew + "' " +   //a.plan_type in ('RAD','GAD')  need to remove
                        //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                                " and a.plan_poid not in (" + hsp_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        //" and  a.plan_poid2 in (" + basic_poids + ")" +
                                " and var_plan_freeflag <> 'Y' " +
                                  hd_where_clause +
                                 " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'" +
                                 " and not EXISTS (select * from  view_lcopre_lcoplan_fetch where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") " +
                                 " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
                }*/
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,bc_price, broad_name, genre_type,var_plan_freeflag from(";
                    str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOJVPLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "'and a.PLAN_TYPE in(" + PlanTypenew + ") ";
                    str += " and a.plan_poid not in (" + addon_poids + "," + hsp_poids + "," + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    //str += " and var_plan_freeflag <> 'Y' ";
                    str += " and lcocode=" + Session["lcoid"].ToString() + hd_where_clause;
                    //str += " and ( addon_plantype is null ";
                    //foreach (string st in addon_poidsarr)
                    //{
                    // str += " or INSTR(addon_plantype,'" + st + ",')>0 ";
                    //}
                    //str += " ) ";
                    //str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                    str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_JVPLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "' and a.PLAN_TYPE in(" + PlanTypenew + ") ";
                    str += " and a.plan_poid not in (" + addon_poids + "," + hsp_poids + "," + basic_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" + hd_where_clause;
                    //str += " and a.plan_poid2 in (" + basic_poids + ")";
                    //str += " and var_plan_freeflag <> 'Y'";
                    //str += " and ( addon_plantype is null ";
                    //foreach (string st in addon_poidsarr)
                    //{
                    // str += " or INSTR(addon_plantype,'" + st + ",')>0 ";
                    //}
                    //str += " ) ";
                    //str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                    str += " and not EXISTS (select * from VIEW_LCOJVPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                    str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
                    // str += " order by plan_name asc";

                }
                else
                {

                    str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,bc_price, broad_name, genre_type,var_plan_freeflag from(";
                    str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOPLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "' and a.PLAN_TYPE in (" + PlanTypenew + ") ";
                    str += " and a.plan_poid not in (" + addon_poids + "," + hsp_poids + "," + basic_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    // str += " and var_plan_freeflag <> 'Y' ";
                    str += " and lcocode=" + Session["lcoid"].ToString() + hd_where_clause;
                    //str += " and ( addon_plantype is null ";
                    //foreach (string st in addon_poidsarr)
                    //{
                    // str += " or INSTR(addon_plantype,'" + st + ",')>0 ";
                    //}
                    //str += " ) ";
                    //str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                    str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_PLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "' and a.PLAN_TYPE in(" + PlanTypenew + ") ";
                    str += " and a.plan_poid not in (" + addon_poids + "," + hsp_poids + "," + basic_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" + hd_where_clause;
                    //str += " and a.plan_poid2 in (" + basic_poids + ")";
                    //str += " and var_plan_freeflag <> 'Y'";
                    //str += " and ( addon_plantype is null ";
                    //foreach (string st in addon_poidsarr)
                    //{
                    // str += " or INSTR(addon_plantype,'" + st + ",')>0 ";
                    //}
                    //str += " ) ";
                    //str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                    str += " and not EXISTS (select * from VIEW_LCOPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                    str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
                    //str += " order by plan_name asc";

                }
                //FileLogTextChange1(str, "HwaySpecial String", "", "");
                DataTable TblPlans = GetResult(str);

                if (TblPlans.Rows.Count > 0)
                {
                    Label7.Visible = false;
                    btnAddPlan.Visible = true;
                    grdPlanChan.DataSource = TblPlans;
                    grdPlanChan.DataBind();
                }
                else
                {
                    Label7.Visible = true;
                    btnAddPlan.Visible = false;
                    grdPlanChan.DataSource = null;
                    grdPlanChan.DataBind();
                }
                // QueryFileLog("Query Log", "Addon Query : ", str, ad_qry_test_plan);
            }
            catch (Exception ex)
            {
                Label7.Visible = true;
                btnAddPlan.Visible = false;
            }
            popAdd.Show();
            StatbleDynamicTabs();
        }

        protected void radPlanAD_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lbltotaladd.Text = "0.00/-";
                hdntotaladdamount.Value = "0";
                ViewState["Total"] = "0";
                Label7.Visible = false;
                string str = "";
                string city = "";
                string DeviceDefinitionType = "SD";
                if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
                {
                    city = ViewState["cityid"].ToString();
                }
                string basic_poids = "'0'";
                if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
                {
                    basic_poids = ViewState["basic_poids"].ToString();
                }
                string addon_poids = "'0'";
                if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
                {
                    addon_poids = ViewState["addon_poids"].ToString();
                }
                if (ViewState["DeviceDefinitionType"] != null && ViewState["DeviceDefinitionType"] != "")
                {
                    DeviceDefinitionType = ViewState["DeviceDefinitionType"].ToString();
                }
                string PlanTypenew = "";
                if (radPlanAD.Checked == true)
                {
                    PlanTypenew = "AD";
                }
                else if (radPlanADreg.Checked == true)
                {
                    PlanTypenew = "RAD";
                }

                grdPlanChan.DataSource = null;
                grdPlanChan.DataBind();
                try
                {
                    string hd_where_clause = "";
                    if (!DeviceDefinitionType.Contains("HD"))
                    {
                        hd_where_clause = " and a.device_type <> 'HD' ";
                    }

                    if (rdbplanpayterm.SelectedValue.ToString().Trim() != "12")
                    {
                        if (ViewState["stb_no_hd"].ToString() == "Y")
                        {
                            hd_where_clause = " and a.device_type <> 'HD' ";
                        }

                    }
                    string[] addon_poidsarr = addon_poids.Replace("'", "").Split(',');

                    if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, ";
                        str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_lcojvplan_fetch a ";
                        str += "  where a.cityid ='" + city + "' and a.PLAN_TYPE='" + PlanTypenew + "' ";
                        str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                        // str += "  and var_plan_freeflag <> 'Y' ";
                        str += "  and lcocode=" + Session["lcoid"].ToString() + hd_where_clause;
                        str += "  and (  addon_plantype is null ";
                        foreach (string st in addon_poidsarr)
                        {
                            str += " or INSTR(addon_plantype,'" + st + ",')>0   ";
                        }
                        str += " ) ";
                        //str += "  and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                        str += "  union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, ";
                        str += "  a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_jvplan_fetch a ";
                        str += "  where a.cityid ='" + city + "' and a.PLAN_TYPE='" + PlanTypenew + "' ";
                        str += "  and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" + hd_where_clause;
                        //str += " and  a.plan_poid2 in (" + basic_poids + ")";
                        //str += "  and var_plan_freeflag <> 'Y'";
                        str += "  and (  addon_plantype is null ";
                        foreach (string st in addon_poidsarr)
                        {
                            str += " or INSTR(addon_plantype,'" + st + ",')>0   ";
                        }
                        str += " ) ";
                        //str += "  and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                        str += "  and not EXISTS (select * from  view_lcopre_lcojvplan_fetch where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                        str += "  order by plan_name asc";

                    }
                    else
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, ";
                        str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_lcoplan_fetch a ";
                        str += "  where a.cityid ='" + city + "' and a.PLAN_TYPE='" + PlanTypenew + "' ";
                        str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                        //str += "  and var_plan_freeflag <> 'Y' ";
                        str += "  and lcocode=" + Session["lcoid"].ToString() + hd_where_clause;
                        str += "  and (  addon_plantype is null ";
                        foreach (string st in addon_poidsarr)
                        {
                            str += " or INSTR(addon_plantype,'" + st + ",')>0   ";
                        }
                        str += " ) ";
                        //str += "  and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                        str += "  union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, ";
                        str += "  a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_plan_fetch a ";
                        str += "  where a.cityid ='" + city + "' and a.PLAN_TYPE='" + PlanTypenew + "' ";
                        str += "  and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" + hd_where_clause;
                        //str += " and  a.plan_poid2 in (" + basic_poids + ")";
                        //str += "  and var_plan_freeflag <> 'Y'";
                        str += "  and (  addon_plantype is null ";
                        foreach (string st in addon_poidsarr)
                        {
                            str += " or INSTR(addon_plantype,'" + st + ",')>0   ";
                        }
                        str += " ) ";
                        //str += "  and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                        str += "  and not EXISTS (select * from  view_lcopre_lcoplan_fetch where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                        str += "  order by plan_name asc";

                    }

                    DataTable TblPlans = GetResult(str);
                    ViewState["PLanData"] = TblPlans;
                    //FileLogTextChange1(str, "AD String", "", "");
                    if (TblPlans.Rows.Count > 0)
                    {
                        Label7.Visible = false;
                        btnAddPlan.Visible = true;
                        grdPlanChan.DataSource = TblPlans;
                        grdPlanChan.DataBind();
                        // -- for NTO filter
                        ddlBC.DataSource = null;
                        ddlBC.DataBind();
                        ListItem default_itmBC = new ListItem();
                        default_itmBC.Text = "All";
                        default_itmBC.Value = "0";
                        string StrQry = "SELECT  distinct(a.BROAD_NAME) BROAD_NAME,a.BROAD_NAME BROAD_value FROM  view_lcopre_plan_fetchnew a WHERE a.plan_type not in ('B','HSP','AL') AND a.cityid = '" + city + "'  AND a.dasarea = '" + Session["dasarea"].ToString() + "' and broad_name is not null";
                        Cls_Helper ObjHelper = new Cls_Helper();
                        DataTable dtBCData = null;
                        dtBCData = ObjHelper.GetDataTable(StrQry);
                        ddlBC.DataSource = dtBCData;
                        ddlBC.DataValueField = "BROAD_value";
                        ddlBC.DataTextField = "BROAD_NAME";
                        ddlBC.DataBind();
                        ddlBC.Items.Insert(0, default_itmBC);
                        /*string StrQry2 = "SELECT  distinct(a.genre_type) genre_type,a.genre_type genre_value FROM  view_lcopre_plan_fetchnew a WHERE a.plan_type not in ('B','HSP') AND a.cityid = '" + city + "' AND a.payterm = '" + rdbplanpayterm.SelectedValue.ToString().Trim() + "' AND a.dasarea = '" + Session["dasarea"].ToString() + "'";
                        DataTable dtGenerData = null;
                        dtGenerData = ObjHelper.GetDataTable(StrQry2);
                        ddlGener.DataSource = dtGenerData;
                        ddlGener.DataValueField = "genre_value";
                        ddlGener.DataTextField = "genre_type";
                        ddlGener.DataBind();
                        ddlGener.Items.Insert(0, default_itmBC);*/
                        pnlBC.Visible = true;
                        ddlBC.SelectedValue = "0";
                        lblBC.Visible = true;
                        ddlBC.Visible = true;
                        btnsearchfilter2.Visible = true;
                        pnlAL.Visible = false;

                    }
                    else
                    {
                        Label7.Visible = true;
                        btnAddPlan.Visible = false;
                        grdPlanChan.DataSource = null;
                        grdPlanChan.DataBind();
                    }
                    // QueryFileLog("Query Log", "Addon Query : ", str, ad_qry_test_plan);
                }
                catch (Exception ex)
                {
                    //FileLogTextChange1("", "AD String", "", ex.ToString());
                    Label7.Visible = true;
                    btnAddPlan.Visible = false;
                }
            }
            catch (Exception ex)
            {
                FileLogTextChange1("", "AD String", "", ex.ToString());

            }
            popAdd.Show();
            StatbleDynamicTabs();
        }//shri

        protected void radPlanAL_CheckedChanged(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            //lblplanname.Text = "";
            //lblplanamt.Text = "";
            //trAddplanAutorenew.Visible = false;
            //cbAddPlanAutorenew.Checked = false;
            lbltotaladd.Text = "0.00/-";
            hdntotaladdamount.Value = "0";
            ViewState["Total"] = "0";
            Label7.Visible = false;
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string str = "";
            string basic_poids = "'0'";
            if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            {
                basic_poids = ViewState["basic_poids"].ToString();
            }
            string ala_poids = "'0'";
            if (ViewState["ala_poids"] != null && ViewState["ala_poids"].ToString() != "")
            {
                ala_poids = ViewState["ala_poids"].ToString();
            }
            string hd_where_clause = "";
            string DeviceDefinitionType = "SD";
            if (ViewState["DeviceDefinitionType"] != null && ViewState["DeviceDefinitionType"] != "")
            {
                DeviceDefinitionType = ViewState["DeviceDefinitionType"].ToString();
            }
            grdPlanChan.DataSource = null;
            grdPlanChan.DataBind();
            try
            {

                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    if (!DeviceDefinitionType.Contains("HD"))
                    {
                        hd_where_clause = " and a.device_type <> 'HD' ";
                    }
                    str = " SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.BROAD_NAME, a.GENRE_TYPE,a.var_plan_freeflag " +
                                          " FROM view_lcopre_lcoplan_jvfetchnew a " +
                                          " where a.plan_type='AL' and a.cityid ='" + city + "' " +
                                          " and a.plan_poid not in (" + ala_poids + ") " +
                                          " and a.dasarea='" + Session["dasarea"].ToString() + "' and lcocode=" + Session["lcoid"].ToString() +// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "' 
                                          " union select * from ( (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.BROAD_NAME, a.GENRE_TYPE,a.var_plan_freeflag " +
                                          " FROM view_lcopre_plan_jvfetchnew a " +
                                          " where a.plan_type='AL' and a.cityid ='" + city + "' " +
                                          " and a.plan_poid not in (" + ala_poids + ") " + hd_where_clause +
                                          " and a.dasarea='" + Session["dasarea"].ToString() + "'" +// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "' 
                                          " and not EXISTS (select * from  view_lcopre_lcoplan_jvfetchnew where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") " +
                                          " ) minus  " +
                                          " ( " +
                                          " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice,a.var_plan_productpoid product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.num_plan_broadprice, a.var_plan_broad_name, a.var_plan_genre_type,a.var_plan_freeflag" +
                                          " FROM aoup_lcopre_jvplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_jvplan_def c " +
                                          "  where a.var_plan_name=b.var_plan_name " +
                                          " and c.var_plan_proviid=b.var_plan_provi " +
                                          "  AND b.var_plan_city=a.num_plan_cityid " +
                                          " and a.var_plan_plantype in ('AD','B') " +
                                          " and c.var_plan_plantype='AL' " +
                                          " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                                          " and c.num_plan_cityid='" + city + "' " +
                                          " and not EXISTS (select * from  aoup_lcopre_lcojvplan_def where var_plan_name=c.var_plan_name and  var_plan_planpoid=c.var_plan_planpoid and var_plan_lcocode=" + Session["lcoid"].ToString() + ")" +
                                          " ) ) order by plan_name asc";
                }
                else
                {
                    if (!DeviceDefinitionType.Contains("HD"))
                    {
                        hd_where_clause = " and a.var_plan_devicetype <> 'HD' ";
                    }
                    str = " SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.BROAD_NAME, a.GENRE_TYPE,a.var_plan_freeflag " +
                                      " FROM view_lcopre_lcoplan_fetchnew a " +
                                     " where a.plan_type='AL' and a.cityid ='" + city + "' " +
                                     " and a.plan_poid not in (" + ala_poids + ") " +
                                      " and a.dasarea='" + Session["dasarea"].ToString() + "' and lcocode=" + Session["lcoid"].ToString() +// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'
                                   " union select * from ( (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.BROAD_NAME, a.GENRE_TYPE,a.var_plan_freeflag " +
                                      " FROM view_lcopre_plan_fetchnew a " +
                                     " where a.plan_type='AL' and a.cityid ='" + city + "' " +
                                     " and a.plan_poid not in (" + ala_poids + ") " + hd_where_clause +
                                      " and a.dasarea='" + Session["dasarea"].ToString() + "'" +// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "' 
                                      " and not EXISTS (select * from  view_lcopre_lcoplan_fetchnew where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") " +
                                      " ) minus  " +
                                      " ( " +
                                      " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice,a.var_plan_productpoid product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.num_plan_broadprice, a.var_plan_broad_name, a.var_plan_genre_type,a.var_plan_freeflag" +
                                      " FROM aoup_lcopre_plan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_plan_def c " +
                                      "  where a.var_plan_name=b.var_plan_name " +
                                      " and c.var_plan_proviid=b.var_plan_provi " +
                                      "  AND b.var_plan_city=a.num_plan_cityid " +
                                      " and a.var_plan_plantype in ('AD','B') " +
                                      " and c.var_plan_plantype='AL' " +
                                      " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                                      " and c.num_plan_cityid='" + city + "' " +
                                      " and not EXISTS (select * from  aoup_lcopre_lcoplan_def where var_plan_name=c.var_plan_name and  var_plan_planpoid=c.var_plan_planpoid and var_plan_lcocode=" + Session["lcoid"].ToString() + ")" +
                                      " ) ) order by plan_name asc";

                }

                DataTable TblPlans = GetResult(str);
                ViewState["PLanData"] = TblPlans;
                if (TblPlans.Rows.Count > 0)
                {
                    Label7.Visible = false;
                    btnAddPlan.Visible = true;
                    grdPlanChan.DataSource = TblPlans;
                    grdPlanChan.DataBind();
                    // -- for NTO filter
                    //ddlBC2.DataSource = null;
                    //ddlBC2.DataBind();
                    ListItem default_itmBC = new ListItem();
                    default_itmBC.Text = "All";
                    default_itmBC.Value = "0";
                    string StrQry = "SELECT  distinct(a.BROAD_NAME) BROAD_NAME,a.BROAD_NAME BROAD_value FROM  view_lcopre_plan_fetchnew a WHERE a.plan_type in ('AL') AND a.cityid = '" + city + "'  AND a.dasarea = '" + Session["dasarea"].ToString() + "' and broad_name is not null";
                    // FileLogTextChange1(StrQry, "BC in AL String", "", "");
                    Cls_Helper ObjHelper = new Cls_Helper();
                    DataTable dtBCData = null;
                    dtBCData = ObjHelper.GetDataTable(StrQry);
                    ddlBC2.DataSource = dtBCData;
                    ddlBC2.DataValueField = "BROAD_value";
                    ddlBC2.DataTextField = "BROAD_NAME";
                    ddlBC2.DataBind();
                    ddlBC2.Items.Insert(0, default_itmBC);

                    string StrQry2 = "SELECT  distinct(a.genre_type) genre_type,a.genre_type genre_value FROM  view_lcopre_plan_fetchnew a WHERE a.plan_type in ('AL') AND a.cityid = '" + city + "'  AND a.dasarea = '" + Session["dasarea"].ToString() + "' and genre_type is not null";
                    DataTable dtGenerData = null;
                    dtGenerData = ObjHelper.GetDataTable(StrQry2);
                    ddlGener.DataSource = dtGenerData;
                    ddlGener.DataValueField = "genre_value";
                    ddlGener.DataTextField = "genre_type";
                    ddlGener.DataBind();
                    ddlGener.Items.Insert(0, default_itmBC);
                    pnlBC.Visible = true;
                    lblBC.Visible = false;
                    ddlBC.Visible = false;
                    pnlAL.Visible = true;
                    ddlBC2.SelectedValue = "0";
                    ddlSDHD.SelectedValue = "0";
                    ddlPAYFREE.SelectedValue = "0";
                    ddlGener.SelectedValue = "0";
                    btnsearchfilter2.Visible = false;
                }
                else
                {
                    Label7.Visible = true;
                    btnAddPlan.Visible = false;
                    grdPlanChan.DataSource = null;
                    grdPlanChan.DataBind();
                }
                // QueryFileLog("Query Log", "A-la-Carte Query : ", str, plan_testqry_str);
            }
            catch (Exception ex)
            {
                Label7.Visible = true;
                btnAddPlan.Visible = false;
            }
            popAdd.Show();
            StatbleDynamicTabs();
        }//shri

        protected void grdStb_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.Cells.Count > 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string parent_child_flag = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "PARENT_CHILD_FLAG"));
                    if (parent_child_flag == "0")
                    {
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#C5FFD8");
                    }
                }
            }
        }

        protected void cbAlaAutorenew_Clicked(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            bool AutoRenewAlaCarte;
            for (int i = 0; i < grdCarte.Rows.Count; i++)
            {
                string planname = "";
                planname = grdCarte.Rows[i].Cells[0].Text.ToString();

                AutoRenewAlaCarte = AutoRenewstatus(ViewState["vcid"].ToString(), ViewState["customer_no"].ToString(), planname.ToString());
                if (AutoRenewAlaCarte == true)
                {
                    ((CheckBox)(grdCarte.Rows[i].FindControl("cbAlaAutorenew"))).Checked = true;
                }
            } // done by vivek 20150612
            StatbleDynamicTabs();
        }

        protected void cbAddonAutorenew_Clicked(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            bool AutoRenewAddon;

            for (int i = 0; i < grdAddOnPlan.Rows.Count; i++)
            {
                string planname = "";
                planname = grdAddOnPlan.Rows[i].Cells[0].Text.ToString();

                AutoRenewAddon = AutoRenewstatus(ViewState["vcid"].ToString(), ViewState["customer_no"].ToString(), planname.ToString());
                if (AutoRenewAddon == true)
                {
                    ((CheckBox)(grdAddOnPlan.Rows[i].FindControl("cbAddonAutorenew"))).Checked = true;

                }
            }
            StatbleDynamicTabs();
        } // done by vivek 20150612

        protected void lnkBChange_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            ViewState["ChangedBasicPlanFOCChange"] = null;
            ViewState["cancelfocpack"] = "Y";
            ViewState["BasicPlanChangeWithFOC"] = "Y";
            //lblchangediscount.Text = "";
            ViewState["bucket1foc"] = "0";
            ViewState["bucket2foc"] = "0";
            trpayterm.Visible = true;
            rbtnPayterm.ClearSelection();

            rbtnPayterm.Items[0].Selected = true;

            ViewState["basicindex"] = (((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex;

            ViewState["PayTerm"] = rbtnPayterm.SelectedValue.ToString();
            //popChangePayTerm.Hide();
            popchange.Show();


            int rindex = (((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex;
            HiddenField hdnBasicPlanName = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanName");
            HiddenField hdnBasicPlanPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanPoid");
            HiddenField hdnBasicDealPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicDealPoid");
            HiddenField hdnBasicPackageId = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPackageId");
            HiddenField hdnBasicActivation = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicActivation");



            lblOldPlan.Text = hdnBasicPlanName.Value;



            //lblChangePlan.Text = "";
            //lblChangePlanMRP.Text = "";
            //lblChangeplanLCO.Text = "";
            HiddenField hdnBasicExpiry = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicExpiry");
            ViewState["expdt"] = hdnBasicExpiry.Value;
            ViewState["Planid"] = hdnBasicPlanPoid.Value;
            ViewState["Packageid"] = hdnBasicPackageId.Value;
            ViewState["Dealid"] = hdnBasicDealPoid.Value;
            ViewState["ActDate"] = hdnBasicActivation.Value;
            ddlChangeplan("B", ViewState["PayTerm"].ToString());

            StatbleDynamicTabs();
            // popChangePayTerm.Show();

        }

        protected void lnkADChange_Click(object sender, EventArgs e)
        {
            //lblchangediscount.Text = "";
            ViewState["ChangedBasicPlanFOCChange"] = "Add On";
            ViewState["BasicPlanChangeWithFOC"] = "N";
            // lnkatag_Click(null, null);

            TableRow grdAddOnPlan = ((System.Web.UI.WebControls.TableRow)((GridViewRow)(((Button)(sender)).Parent.BindingContainer)));

            HiddenField hdnPlanchnageflag = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanChangeFlag");
            //lblchangediscount.Text = "";
            ViewState["PayTerm"] = "";

            if (hdnPlanchnageflag.Value == "Y")  // created by vivek 16-nov-2015
            {
                if (ViewState["BasicActionFlag"] != null)
                {

                    if (ViewState["BasicActionFlag"].ToString() == "EX")
                    {
                        lblPopupResponse.Text = "Basic Pack is Expired,you can't Change the pack";
                        btnRefreshForm.Visible = false;
                        popMsg.Show();
                        StatbleDynamicTabs();
                        return;
                    }
                }

            }
            else
            {
                btnRefreshForm.Visible = false;
                lblPopupResponse.Text = "You can't Change the pack";
                popMsg.Show();
                StatbleDynamicTabs();
                return;
            }


            trpayterm.Visible = false;
            popchange.Show();

            HiddenField hdnADPlanName = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanName");
            HiddenField hdnADPlanPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanPoid");
            HiddenField hdnADPlanType = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanType");
            HiddenField hdnADDealPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADDealPoid");
            HiddenField hdnADPackageId = (HiddenField)grdAddOnPlan.FindControl("hdnADPackageId");
            HiddenField hdnADActivation = (HiddenField)grdAddOnPlan.FindControl("hdnADActivation");

            ViewState["Planid"] = hdnADPlanPoid.Value;
            ViewState["Packageid"] = hdnADPackageId.Value;
            ViewState["Dealid"] = hdnADDealPoid.Value;
            ViewState["ActDate"] = hdnADActivation.Value;


            lblOldPlan.Text = hdnADPlanName.Value;
            ddlChangeplan(hdnADPlanType.Value, "");
            //lblChangePlan.Text = "";
            //lblChangePlanMRP.Text = "";
            //lblChangeplanLCO.Text = "";
            HiddenField hdnADexp = (HiddenField)grdAddOnPlan.FindControl("hdnADExpiry");
            ViewState["expdt"] = hdnADexp.Value;
            StatbleDynamicTabs();

        }

        protected void lnkALChange_Click(object sender, EventArgs e)
        {
            ViewState["ChangedBasicPlanFOCChange"] = "Ala-Carte";
            ViewState["BasicPlanChangeWithFOC"] = "N";
            //lnkatag_Click(null, null);
            int rindex = (((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex;

            HiddenField hdnALPlanChangeFlag = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALPlanChangeFlag");
            //lblchangediscount.Text = "";
            ViewState["PayTerm"] = "";

            if (hdnALPlanChangeFlag.Value == "Y")
            {
                if (ViewState["BasicActionFlag"] != null)
                {

                    if (ViewState["BasicActionFlag"].ToString() == "EX")     // created by vivek 16-nov-2015
                    {
                        lblPopupResponse.Text = "Basic Pack is Expired,you can't Change the pack";
                        btnRefreshForm.Visible = false;
                        popMsg.Show();
                        StatbleDynamicTabs();
                        return;
                    }
                }

            }
            else
            {
                btnRefreshForm.Visible = false;
                lblPopupResponse.Text = "You can't Change the pack";
                popMsg.Show();
                StatbleDynamicTabs();
                return;
            }



            trpayterm.Visible = false;
            popchange.Show();

            HiddenField hdnALPlanName = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALPlanName");
            HiddenField hdnALPlanPoid = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALPlanPoid");
            HiddenField hdnALPackageId = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALPackageId");
            HiddenField hdnALDealPoid = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALDealPoid");
            HiddenField hdnALActivation = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALActivation");
            ViewState["Planid"] = hdnALPlanPoid.Value;
            ViewState["Packageid"] = hdnALPackageId.Value;
            ViewState["Dealid"] = hdnALDealPoid.Value;
            ViewState["ActDate"] = hdnALActivation.Value;

            lblOldPlan.Text = hdnALPlanName.Value;
            ddlChangeplan("AL", "");
            //lblChangePlan.Text = "";
            //lblChangePlanMRP.Text = "";
            //lblChangeplanLCO.Text = "";
            HiddenField hdnALexp = (HiddenField)grdCarte.Rows[rindex].FindControl("hdnALExpiry");
            ViewState["expdt"] = hdnALexp.Value;
            StatbleDynamicTabs();

        }


        public void ddlChangeplanLco(string planType, string PayTerm)
        {

            try
            {
                ViewState["planoflcoshare"] = "'0',";
                DataTable Tblchangeplan = (DataTable)ViewState["Tblchangeplan"];

                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection con = new OracleConnection(strCon);
                string str = "";

                string city = "";
                if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
                {
                    city = ViewState["cityid"].ToString();
                }
                string basic_poids = "'0'";
                if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
                {
                    basic_poids = ViewState["basic_poids"].ToString();
                }
                string addon_poids = "'0'";
                if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
                {
                    addon_poids = ViewState["addon_poids"].ToString();
                }
                string addon_poidsReg = "'0'";
                if (ViewState["addon_poidsReg"] != null && ViewState["addon_poidsReg"].ToString() != "")
                {
                    addon_poidsReg = ViewState["addon_poidsReg"].ToString();
                }
                string ala_poids = "'0'";
                if (ViewState["ala_poids"] != null && ViewState["ala_poids"].ToString() != "")
                {
                    ala_poids = ViewState["ala_poids"].ToString();
                }

                string hsp_poids = "'0'";
                if (ViewState["hwayspecial_poid"] != null && ViewState["hwayspecial_poid"].ToString() != "")
                {
                    hsp_poids = ViewState["hwayspecial_poid"].ToString();
                }
                ViewState["PlanType"] = planType;

                string planid = ViewState["Planid"].ToString();
                string proviId = "";
                string strproviId = "";
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    strproviId = " select var_plan_proviid  from aoup_lcopre_jvplan_def ";
                    strproviId += " where var_plan_planpoid ='" + planid + "' and var_plan_dasarea='" + Session["dasarea"].ToString() + "'";
                }
                else
                {
                    strproviId = " select var_plan_proviid  from aoup_lcopre_plan_def ";
                    strproviId += " where var_plan_planpoid ='" + planid + "' and var_plan_dasarea='" + Session["dasarea"].ToString() + "'";
                }
                OracleCommand cmdprovi = new OracleCommand(strproviId, con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OracleDataReader drprovi = cmdprovi.ExecuteReader();

                if (drprovi.HasRows)
                {
                    while (drprovi.Read())
                    {
                        proviId = drprovi["var_plan_proviid"].ToString();
                    }
                }

                con.Close();
                string whestring = "";
                if (ViewState["parentsmsg"].ToString() == "0")
                {
                    whestring = "and  a.plan_name not like '%ADDITIONAL%'";
                }
                else if (ViewState["parentsmsg"].ToString() == "1")
                {
                    whestring = "and  a.plan_name  like '%ADDITIONAL%'";
                }
                string whestringDevicetype = "";
                string whestringDevicetypeAl = "";
                if (ViewState["Device_Type"] != null)
                {
                    if (ViewState["Device_Type"].ToString() != "HD")
                    {
                        whestringDevicetype += " and a.device_type = ('" + Convert.ToString(ViewState["Device_Type"]) + "')";
                        whestringDevicetypeAl += " and a.var_plan_devicetype = ('" + Convert.ToString(ViewState["Device_Type"]) + "')";
                    }
                }


                string hd_where_clause = "";
                if (planType == "AD")
                {
                    if (PayTerm != "12")
                    {
                        if (ViewState["stb_no_hd"].ToString() == "Y")
                        {
                            hd_where_clause = " and a.device_type <> 'HD' ";
                        }

                    }
                }

                //ddlPlanChange.Items.Clear();

                if (planType == "B")
                {
                    if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                     " a.cityid, a.company_code, a.insby, a.insdt,a.ALACARTEBASE  FROM view_lcopre_lcoplan_fch_JVbasi a " +
                     " where a.cityid ='" + city + "' and a.plan_type='B' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                     " and a.plan_poid not in (" + basic_poids + ")  and a.lcocode='" + Session["lco_operid"].ToString() + "'" +
                            //" and a.payterm =" + PayTerm + "" +
                        whestring + //whestringDevicetype +
                      " order by a.plan_name asc";
                    }
                    else
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                      " a.cityid, a.company_code, a.insby, a.insdt,a.ALACARTEBASE  FROM view_lcopre_lcoplan_fetch_basi a " +
                      " where a.cityid ='" + city + "' and a.plan_type='B' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                      " and a.plan_poid not in (" + basic_poids + ")  and a.lcocode='" + Session["lco_operid"].ToString() + "'" +
                            //" and a.payterm =" + PayTerm + "" +
                         whestring + //whestringDevicetype +
                       " order by a.plan_name asc";
                    }


                }

                // Added by Vivek 07-03-2016 to get Old FOC Plan Payterm

                string AddOnpayterm = "";

                if (lblOldPlan.Text.ToString().Trim().Contains("1M") == true)
                {
                    AddOnpayterm = "1";
                }
                else if (lblOldPlan.Text.ToString().Trim().Contains("3M") == true)
                {
                    AddOnpayterm = "3";
                }
                else if (lblOldPlan.Text.ToString().Trim().Contains("6M") == true)
                {
                    AddOnpayterm = "6";
                }
                else if (lblOldPlan.Text.ToString().Trim().Contains("12M") == true)
                {
                    AddOnpayterm = "12";
                }

                if (planType == "AD")
                {

                    if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        if (lblOldPlan.Text.ToString().ToUpper().Contains("FREE") == true)
                        {
                            str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                          " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_JVfetchnew a " +
                          " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                                //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                          " and a.plan_poid not in (" + addon_poids + ")" +
                       " and  a.plan_poid2 in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                // " and payterm ='" + AddOnpayterm + "' " +
                                //" and a.plan_proviid='" + proviId + "'" +
                           " order by a.plan_name asc";
                        }
                        else
                        {
                            str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                           " a.cityid, a.company_code, a.insby, a.insdt  FROM  view_lcopre_lcoplan_JVfetchnew a " +
                           " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                                //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                           " and a.plan_poid not in (" + addon_poids + ")" +
                        " and  a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        " and a.PROVIID='" + proviId + "'" +
                            " order by a.plan_name asc";
                        }
                    }
                    else
                    {
                        if (lblOldPlan.Text.ToString().ToUpper().Contains("FREE") == true)
                        {
                            str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                          " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fetchnew a " +
                          " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                                //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                          " and a.plan_poid not in (" + addon_poids + ")" +
                       " and  a.plan_poid2 in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                // " and payterm ='" + AddOnpayterm + "' " +
                                //" and a.plan_proviid='" + proviId + "'" +
                           " order by a.plan_name asc";
                        }
                        else
                        {
                            str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                           " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fetchnew a " +
                           " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                                //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                           " and a.plan_poid not in (" + addon_poids + ")" +
                        " and  a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        " and a.PROVIID='" + proviId + "'" +
                            " order by a.plan_name asc";
                        }
                    }

                }
                else if (planType == "HSP")
                {
                    if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                      " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_JVfetchnew a " +
                      " where a.cityid ='" + city + "' and a.plan_type='HSP'" +
                            //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                      " and a.plan_poid not in (" + hsp_poids + ")" +
                   " and   a.dasarea='" + Session["dasarea"].ToString() + "'" +
                   " and a.PROVIID='" + proviId + "'" +
                       " order by a.plan_name asc";
                    }

                    else
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                       " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fetchnew a " +
                       " where a.cityid ='" + city + "' and a.plan_type='HSP'" +
                            //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                       " and a.plan_poid not in (" + hsp_poids + ")" +
                    " and   a.dasarea='" + Session["dasarea"].ToString() + "'" +
                    " and a.PROVIID='" + proviId + "'" +
                        " order by a.plan_name asc";
                    }

                }
                else if (planType == "AL")
                {
                    if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        str = " select * from ( (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                             " FROM view_lcopre_lcoplan_JVfetchnew a " +
                             " where a.plan_type='AL' and a.cityid ='" + city + "' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                            //and upper(a.plan_name) like upper('" + prefixText + "%')  " +
                             " and a.plan_poid not in (" + ala_poids + ")  and a.lcocode='" + Session["lco_operid"].ToString() + "'" +
                             " and a.proviid ='" + proviId + "'" + //whestringDevicetype +
                             " ) minus  " +
                             " ( " +
                             " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice  " +
                             " FROM aoup_lcopre_lcojvplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_lcojvplan_def c " +
                             "  where a.var_plan_name=b.var_plan_name " +
                             " and c.var_plan_proviid=b.var_plan_provi " +
                             "  AND b.var_plan_city=a.num_plan_cityid " +
                              " and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                             " and a.var_plan_plantype in ('RAD','GAD','B') " +
                             " and c.var_plan_plantype='AL' " +
                             " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_lcocode='" + Session["lco_operid"].ToString() + "'" +
                             " and c.num_plan_cityid='" + city + "' " +
                             " and c.var_plan_proviid ='" + proviId + "'" +
                             " ) ) order by plan_name asc";
                    }
                    else
                    {
                        str = " select * from ( (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                                " FROM view_lcopre_lcoplan_fetchnew a " +
                                " where a.plan_type='AL' and a.cityid ='" + city + "' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                            //and upper(a.plan_name) like upper('" + prefixText + "%')  " +
                                " and a.plan_poid not in (" + ala_poids + ")  and a.lcocode='" + Session["lco_operid"].ToString() + "'" +
                                " and a.proviid ='" + proviId + "'" + //whestringDevicetype +
                                " ) minus  " +
                                " ( " +
                                " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice  " +
                                " FROM aoup_lcopre_lcoplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_lcoplan_def c " +
                                "  where a.var_plan_name=b.var_plan_name " +
                                " and c.var_plan_proviid=b.var_plan_provi " +
                                "  AND b.var_plan_city=a.num_plan_cityid " +
                                 " and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                                " and a.var_plan_plantype in ('RAD','GAD','B') " +
                                " and c.var_plan_plantype='AL' " +
                                " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_lcocode='" + Session["lco_operid"].ToString() + "'" +
                                " and c.num_plan_cityid='" + city + "' " +
                                " and c.var_plan_proviid ='" + proviId + "'" +
                                " ) ) order by plan_name asc";
                    }
                }
                OracleCommand cmd = new OracleCommand(str, con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                // con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                string plan_testqry_str = "";
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["plan_type"].ToString() == "B")
                        {
                            Double DiscountAmount = Convert.ToDouble(ViewState["DiscountAmount"].ToString());
                            if (ViewState["discounttype"].ToString() == "P")
                            {
                                double Customerprice = 0;
                                if (dr["cust_price"].ToString() != "")
                                {
                                    Customerprice = Convert.ToDouble(dr["cust_price"].ToString());
                                }
                                DiscountAmount = Customerprice / 100 * DiscountAmount;

                            }

                            Double custprice = Convert.ToDouble(dr["cust_price"]) - DiscountAmount;

                            if (custprice < 0)
                            {
                                custprice = 0;
                            }

                            Tblchangeplan.Rows.Add(dr["plan_name"].ToString(), Convert.ToDouble(dr["cust_price"]), Convert.ToDouble(dr["lco_price"]), DiscountAmount,
                               custprice, dr["plan_poid"].ToString(), dr["deal_poid"].ToString(), dr["plan_type"].ToString());
                        }

                        else
                        {
                            Tblchangeplan.Rows.Add(dr["plan_name"].ToString(), Convert.ToDouble(dr["cust_price"]), Convert.ToDouble(dr["lco_price"]), 0,
                            0, dr["plan_poid"].ToString(), dr["deal_poid"].ToString(), dr["plan_type"].ToString());
                        }

                        ViewState["planoflcoshare"] += "'" + dr["plan_poid"].ToString() + "',";
                        //ListItem lst = new ListItem();
                        //lst.Text = dr["plan_name"].ToString();
                        //lst.Value = dr["plan_name"].ToString();
                        //ddlPlanChange.Items.Add(lst);
                        //plan_testqry_str += dr["plan_name"].ToString() + ",";

                    }
                    //ddlPlanChange.Items.Insert(0, new ListItem("Select Channel", "0"));



                    if (Tblchangeplan.Rows.Count > 0)
                    {
                        ViewState["Tblchangeplan"] = Tblchangeplan;
                    }
                    else
                    {
                    }
                }
                else
                {
                    //ListItem lst = new ListItem();
                    //lst.Text = "No channels found";
                    //lst.Value = "0";
                    //ddlPlanChange.Items.Add(lst);
                }

                ViewState["planoflcoshare"] = ViewState["planoflcoshare"].ToString().Substring(0, ViewState["planoflcoshare"].ToString().Length - 1);
            }
            catch (Exception ex)
            {
            }

        }

        public void ddlChangeplan(string planType, string PayTerm)
        {

            try
            {
                ViewState["alacartechangenew"] = "N";
                ViewState["Tblchangeplan"] = null;
                lblchangeplannotfount.Visible = false;

                GrdchangePlan.DataSource = null;
                GrdchangePlan.DataBind();

                DataTable Tblchangeplan = new DataTable();

                Tblchangeplan.Columns.Add("plan_name");
                Tblchangeplan.Columns.Add("cust_price", typeof(double));
                Tblchangeplan.Columns.Add("lco_price", typeof(double));
                Tblchangeplan.Columns.Add("discount", typeof(double));
                Tblchangeplan.Columns.Add("netmrp", typeof(double));
                Tblchangeplan.Columns.Add("plan_poid");
                Tblchangeplan.Columns.Add("deal_poid");
                Tblchangeplan.Columns.Add("plan_type");
                Tblchangeplan.Columns.Add("ALACARTEBASE");

                ViewState["Tblchangeplan"] = Tblchangeplan;

                ddlChangeplanLco(planType, PayTerm);

                Tblchangeplan = (DataTable)ViewState["Tblchangeplan"];

                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection con = new OracleConnection(strCon);
                string str = "";

                string city = "";
                if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
                {
                    city = ViewState["cityid"].ToString();
                }
                string basic_poids = "'0'";
                if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
                {
                    basic_poids = ViewState["basic_poids"].ToString();
                }
                string addon_poids = "'0'";
                if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
                {
                    addon_poids = ViewState["addon_poids"].ToString();
                }
                string addon_poidsReg = "'0'";
                if (ViewState["addon_poidsReg"] != null && ViewState["addon_poidsReg"].ToString() != "")
                {
                    addon_poidsReg = ViewState["addon_poidsReg"].ToString();
                }
                string ala_poids = "'0'";
                if (ViewState["ala_poids"] != null && ViewState["ala_poids"].ToString() != "")
                {
                    ala_poids = ViewState["ala_poids"].ToString();
                }

                string hsp_poids = "'0'";
                if (ViewState["hwayspecial_poid"] != null && ViewState["hwayspecial_poid"].ToString() != "")
                {
                    hsp_poids = ViewState["hwayspecial_poid"].ToString();
                }

                string ala_poidsFree = "'0'";
                if (ViewState["ala_poidsFree"] != null && ViewState["ala_poidsFree"].ToString() != "")
                {
                    ala_poidsFree = ViewState["ala_poidsFree"].ToString();
                }

                ViewState["PlanType"] = planType;

                string planid = ViewState["Planid"].ToString();
                string proviId = "";
                string strproviId = "";
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    strproviId += " select var_plan_proviid  from aoup_lcopre_jvplan_def ";
                    strproviId += " where var_plan_planpoid ='" + planid + "' and var_plan_dasarea='" + Session["dasarea"].ToString() + "'";
                }
                else
                {
                    strproviId += " select var_plan_proviid  from aoup_lcopre_plan_def ";
                    strproviId += " where var_plan_planpoid ='" + planid + "' and var_plan_dasarea='" + Session["dasarea"].ToString() + "'";
                }
                OracleCommand cmdprovi = new OracleCommand(strproviId, con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OracleDataReader drprovi = cmdprovi.ExecuteReader();

                if (drprovi.HasRows)
                {
                    while (drprovi.Read())
                    {
                        proviId = drprovi["var_plan_proviid"].ToString();
                    }
                }

                con.Close();
                string whestring = "";
                /*if (ViewState["parentsmsg"].ToString() == "0")
                {
                    whestring = "and  a.plan_name not like '%ADDITIONAL%'";
                }
                else if (ViewState["parentsmsg"].ToString() == "1")
                {
                    whestring = "and  a.plan_name  like '%ADDITIONAL%'";
                }*/
                string whestringDevicetype = "";
                string whestringDevicetypeAl = "";
                if (ViewState["Device_Type"] != null)
                {
                    if (ViewState["Device_Type"].ToString().Contains("HD"))
                    {

                    }
                    else
                    {
                        whestringDevicetype += " and a.device_type = ('SD')";
                        whestringDevicetypeAl += " and a.var_plan_devicetype = ('SD')";
                    }
                }

                string hd_where_clause = "";
                if (planType == "AD")
                {
                    if (PayTerm != "12")
                    {
                        if (ViewState["stb_no_hd"].ToString() == "Y")
                        {
                            hd_where_clause = " and a.device_type <> 'HD' ";
                        }

                    }
                }
                //ddlPlanChange.Items.Clear();

                if (planType == "B")
                {
                    /*if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                     " a.cityid, a.company_code, a.insby, a.insdt,a.ALACARTEBASE  FROM VIEW_LCOPRE_PPRENEWAL_JVFETCH a " +
                     " where a.cityid ='" + city + "' and a.plan_type='B' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                     " and a.plan_poid not in (" + basic_poids + ")" +
                     " and a.plan_poid not in (" + ViewState["planoflcoshare"].ToString() + ")" +
                     " and a.payterm =" + PayTerm + "" +
                        whestring + whestringDevicetype +
                      " order by a.plan_name asc";
                    }
                    else
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                      " a.cityid, a.company_code, a.insby, a.insdt,a.ALACARTEBASE  FROM VIEW_LCOPRE_PPRENEWAL_FETCH a " +
                      " where a.cityid ='" + city + "' and a.plan_type='B' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                      " and a.plan_poid not in (" + basic_poids + ")" +
                      " and a.plan_poid not in (" + ViewState["planoflcoshare"].ToString() + ")" +
                      " and a.payterm =" + PayTerm + "" +
                         whestring + whestringDevicetype +
                       " order by a.plan_name asc";
                    }*/
                    if (ViewState["JVFlag"].ToString() == "Y")
                    {

                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                       " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag,a.ALACARTEBASE  FROM view_lcopre_lcoplan_fch_JVbasi a " +
                                       " where a.cityid =" + city + " and a.plan_type='B'" +
                                       " and a.plan_poid not in (" + basic_poids + ")  and lcocode=" + Session["lcoid"].ToString() + " and  a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                       whestring + hd_where_clause +
                                       " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                       " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag,a.ALACARTEBASE  FROM view_lcopre_plan_fetch_JVbasic a " +
                                       " where a.cityid =" + city + " and a.plan_type='B'" +
                                       " and a.plan_poid not in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                       whestring + hd_where_clause +
                                       " and not EXISTS (select * from  view_lcopre_lcoplan_fch_JVbasi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")  " +
                                       " order by plan_name asc";

                    }
                    else
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                   " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag,a.ALACARTEBASE  FROM view_lcopre_lcoplan_fetch_basi a " +
                                   " where a.cityid =" + city + " and a.plan_type='B'" +
                            //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                                   " and a.plan_poid not in (" + basic_poids + ")  and lcocode=" + Session["lcoid"].ToString() + " and  a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                   whestring + hd_where_clause +
                                   " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                   " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag,a.ALACARTEBASE  FROM view_lcopre_plan_fetch_basic a " +
                                   " where a.cityid =" + city + " and a.plan_type='B'" +
                            //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                            " and a.plan_poid not in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                            whestring + hd_where_clause +
                            " and not EXISTS (select * from  view_lcopre_lcoplan_fetch_basi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")  " +
                            " order by plan_name asc";
                    }



                }

                // Added by Vivek 07-03-2016 to get Old FOC Plan Payterm

                string AddOnpayterm = "";

                if (lblOldPlan.Text.ToString().Trim().Contains("1M") == true)
                {
                    AddOnpayterm = "1";
                }
                else if (lblOldPlan.Text.ToString().Trim().Contains("3M") == true)
                {
                    AddOnpayterm = "3";
                }
                else if (lblOldPlan.Text.ToString().Trim().Contains("6M") == true)
                {
                    AddOnpayterm = "6";
                }
                else if (lblOldPlan.Text.ToString().Trim().Contains("12M") == true)
                {
                    AddOnpayterm = "12";
                }

                if (planType == "AD")
                {
                    if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        if (lblOldPlan.Text.ToString().ToUpper().Contains("FREE") == true)
                        {
                            str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                          " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_jvfetch_free a " +
                          " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                                //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                          " and a.plan_poid not in (" + addon_poids + ")" +
                       " and  a.plan_poid2 in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                       " and var_plan_freeflag='Y' " +
                                // " and payterm ='" + AddOnpayterm + "' " +
                                //" and a.plan_proviid='" + proviId + "'" +
                           " order by a.plan_name asc";
                        }
                        else
                        {
                            str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                           " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_jvfetch_free a " +
                           " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                                //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                           " and a.plan_poid not in (" + addon_poids + ")" +
                            " and a.plan_poid not in (" + ViewState["planoflcoshare"].ToString() + ") " +
                        " and  a.plan_poid2 in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        " and a.plan_proviid='" + proviId + "'" +
                        " and var_plan_freeflag='N' " +
                            " order by a.plan_name asc";
                        }
                    }
                    else
                    {
                        if (lblOldPlan.Text.ToString().ToUpper().Contains("FREE") == true)
                        {
                            str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                          " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_fetch_Free a " +
                          " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                                //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                          " and a.plan_poid not in (" + addon_poids + ")" +
                       " and  a.plan_poid2 in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                       " and var_plan_freeflag='Y' " +
                                //" and payterm ='" + AddOnpayterm + "' " +
                                //" and a.plan_proviid='" + proviId + "'" +
                           " order by a.plan_name asc";
                        }
                        else
                        {
                            str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                           " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_fetch_Free a " +
                           " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                                //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                           " and a.plan_poid not in (" + addon_poids + ")" +
                            " and a.plan_poid not in (" + ViewState["planoflcoshare"].ToString() + ") " +
                        " and  a.plan_poid2 in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        " and a.plan_proviid='" + proviId + "'" +
                        " and var_plan_freeflag='N' " +
                            " order by a.plan_name asc";
                        }
                    }

                }
                else if (planType == "HSP")
                {
                    /*if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                      " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_jvfetch_Free a " +
                      " where a.cityid ='" + city + "' and a.plan_type='HSP'" +
                            //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                      " and a.plan_poid not in (" + hsp_poids + ")" +
                       " and a.plan_poid not in (" + ViewState["planoflcoshare"].ToString() + ") " +
                   " and  a.plan_poid2 in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                   " and a.plan_proviid='" + proviId + "'" +
                   " and var_plan_freeflag='N' " +
                       " order by a.plan_name asc";
                    }
                    else
                    {
                        str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                       " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_fetch_Free a " +
                       " where a.cityid ='" + city + "' and a.plan_type='HSP'" +
                            //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                       " and a.plan_poid not in (" + hsp_poids + ")" +
                        " and a.plan_poid not in (" + ViewState["planoflcoshare"].ToString() + ") " +
                    " and  a.plan_poid2 in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                    " and a.plan_proviid='" + proviId + "'" +
                    " and var_plan_freeflag='N' " +
                        " order by a.plan_name asc";
                    }
                    */
                    string PlanTypenew = "'HSP'";
                    if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,bc_price, broad_name, genre_type,var_plan_freeflag from(";
                        str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                        str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOJVPLAN_FETCH_ALL a ";
                        str += " where a.cityid ='" + city + "'and a.PLAN_TYPE in(" + PlanTypenew + ") ";
                        str += " and a.plan_poid not in (" + addon_poids + "," + hsp_poids + "," + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'";
                        str += " and lcocode=" + Session["lcoid"].ToString() + hd_where_clause;
                        // str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                        str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                        str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_JVPLAN_FETCH_ALL a ";
                        str += " where a.cityid ='" + city + "' and a.PLAN_TYPE in(" + PlanTypenew + ") ";
                        str += " and a.plan_poid not in (" + addon_poids + "," + hsp_poids + "," + basic_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" + hd_where_clause;
                        //str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                        str += " and not EXISTS (select * from VIEW_LCOJVPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                        str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";

                    }
                    else
                    {

                        str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,bc_price, broad_name, genre_type,var_plan_freeflag from(";
                        str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                        str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOPLAN_FETCH_ALL a ";
                        str += " where a.cityid ='" + city + "' and a.PLAN_TYPE in (" + PlanTypenew + ") ";
                        str += " and a.plan_poid not in (" + addon_poids + "," + hsp_poids + "," + basic_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                        str += " and lcocode=" + Session["lcoid"].ToString() + hd_where_clause;
                        //str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                        str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                        str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_PLAN_FETCH_ALL a ";
                        str += " where a.cityid ='" + city + "' and a.PLAN_TYPE in(" + PlanTypenew + ") ";
                        str += " and a.plan_poid not in (" + addon_poids + "," + hsp_poids + "," + basic_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'" + hd_where_clause;
                        //str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                        str += " and not EXISTS (select * from VIEW_LCOPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                        str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";

                    }

                }
                else if (planType == "AL")
                {
                    if (ViewState["JVFlag"].ToString() == "Y")
                    {
                        str = " select * from ( (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                                " FROM view_lcopre_plan_JVfetchnew a " +
                                " where a.plan_type='AL' and a.cityid ='" + city + "' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                            //and upper(a.plan_name) like upper('" + prefixText + "%')  " +
                                " and a.plan_poid not in (" + ala_poids + ") " +
                                 " and a.plan_poid not in (" + ViewState["planoflcoshare"].ToString() + ")" +
                                " and a.proviid ='" + proviId + "'" + //whestringDevicetype +
                                " ) minus  " +
                                " ( " +
                                " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice  " +
                                " FROM aoup_lcopre_jvplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_jvplan_def c " +
                                "  where a.var_plan_name=b.var_plan_name " +
                                " and c.var_plan_proviid=b.var_plan_provi " +
                                "  AND b.var_plan_city=a.num_plan_cityid " +
                                 " and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                                " and a.var_plan_plantype in ('RAD','GAD','B') " +
                                " and c.var_plan_plantype='AL' " +
                                " and a.var_plan_planpoid in(" + ala_poids + ") " +
                                 " and a.var_plan_planpoid not in (" + ViewState["planoflcoshare"].ToString() + ")" +
                                " and c.num_plan_cityid='" + city + "' " +
                                " and c.var_plan_proviid ='" + proviId + "'" +
                                " ) ) order by plan_name asc";
                    }
                    else
                    {
                        str = " select * from ( (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                                " FROM view_lcopre_plan_fetchnew a " +
                                " where a.plan_type='AL' and a.cityid ='" + city + "' and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                            //and upper(a.plan_name) like upper('" + prefixText + "%')  " +
                                " and a.plan_poid not in (" + ala_poids + ") " +
                                 " and a.plan_poid not in (" + ViewState["planoflcoshare"].ToString() + ")" +
                                " and a.proviid ='" + proviId + "'" + //whestringDevicetype +
                                " ) minus  " +
                                " ( " +
                                " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice  " +
                                " FROM aoup_lcopre_plan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_plan_def c " +
                                "  where a.var_plan_name=b.var_plan_name " +
                                " and c.var_plan_proviid=b.var_plan_provi " +
                                "  AND b.var_plan_city=a.num_plan_cityid " +
                                 " and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                                " and a.var_plan_plantype in ('RAD','GAD','B') " +
                                " and c.var_plan_plantype='AL' " +
                                " and a.var_plan_planpoid in(" + ala_poids + ") " +
                                 " and a.var_plan_planpoid not in (" + ViewState["planoflcoshare"].ToString() + ")" +
                                " and c.num_plan_cityid='" + city + "' " +
                                " and c.var_plan_proviid ='" + proviId + "'" +
                                " ) ) order by plan_name asc";
                    }
                }
                OracleCommand cmd = new OracleCommand(str, con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                // con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                string plan_testqry_str = "";
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["plan_type"].ToString() == "B")
                        {
                            Double DiscountAmount = Convert.ToDouble(ViewState["DiscountAmount"].ToString());
                            if (ViewState["discounttype"].ToString() == "P")
                            {
                                double Customerprice = 0;
                                if (dr["cust_price"].ToString() != "")
                                {
                                    Customerprice = Convert.ToDouble(dr["cust_price"].ToString());
                                }
                                DiscountAmount = Customerprice / 100 * DiscountAmount;

                            }
                            Double custprice = Convert.ToDouble(dr["cust_price"]) - DiscountAmount;

                            if (custprice < 0)
                            {
                                custprice = 0;
                            }

                            Tblchangeplan.Rows.Add(dr["plan_name"].ToString(), Convert.ToDouble(dr["cust_price"]), Convert.ToDouble(dr["lco_price"]), DiscountAmount,
                               custprice, dr["plan_poid"].ToString(), dr["deal_poid"].ToString(), dr["plan_type"].ToString(), dr["ALACARTEBASE"].ToString());
                        }

                        else
                        {
                            Tblchangeplan.Rows.Add(dr["plan_name"].ToString(), Convert.ToDouble(dr["cust_price"]), Convert.ToDouble(dr["lco_price"]), 0,
                            0, dr["plan_poid"].ToString(), dr["deal_poid"].ToString(), dr["plan_type"].ToString(), "N");
                        }
                        //ListItem lst = new ListItem();
                        //lst.Text = dr["plan_name"].ToString();
                        //lst.Value = dr["plan_name"].ToString();
                        //ddlPlanChange.Items.Add(lst);
                        //plan_testqry_str += dr["plan_name"].ToString() + ",";

                    }
                    //ddlPlanChange.Items.Insert(0, new ListItem("Select Channel", "0"));


                }
                if (Tblchangeplan.Rows.Count > 0)
                {
                    GrdchangePlan.DataSource = Tblchangeplan;
                    GrdchangePlan.DataBind();
                    lblchangeplannotfount.Visible = false;
                }
                else
                {
                    lblchangeplannotfount.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblchangeplannotfount.Visible = true;
                //FileLogTextChange("admin", "ChangePlan", ex.Message, "");
            }

        }

        protected void ddlPlanChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            ddlChangePlanSelectindex(ViewState["PlanType"].ToString(), ViewState["PayTerm"].ToString());
            StatbleDynamicTabs();
        }

        public void ddlChangePlanSelectindex(string planType, string payterm)
        {
            bool CechStatus = getChangePlanPrice(planType, payterm);

            if (CechStatus == true)
            {
                popchange.Show();
                return;
            }

            // lnkatag_Click(null, null);

            //if (ddlPlanChange.SelectedItem.Text.Trim() != "")
            //{
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            string search_text = "";// ddlPlanChange.SelectedItem.Text.Trim();
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string basic_poids = "'0'";
            if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            {
                basic_poids = ViewState["basic_poids"].ToString();
            }
            string addon_poids = "'0'";
            if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
            {
                addon_poids = ViewState["addon_poids"].ToString();
            }
            string ala_poids = "'0'";
            if (ViewState["ala_poids"] != null && ViewState["ala_poids"].ToString() != "")
            {
                ala_poids = ViewState["ala_poids"].ToString();
            }

            string hd_where_clause = "";
            if (planType == "AD")
            {
                if (payterm != "12")
                {
                    if (ViewState["stb_no_hd"].ToString() == "Y")
                    {
                        hd_where_clause = " and a.device_type <> 'HD' ";
                    }

                }
            }

            if (planType == "B")
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
               " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_pprenewal_JVfetch a " +
               " where a.cityid ='" + city + "' and a.plan_type='B'" +
                " and upper(a.plan_name) like upper('" + search_text + "') " +
               " and a.plan_poid not in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        // " and a.payterm ='" + payterm + "' " +
                " order by a.plan_name asc";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                  " a.cityid, a.company_code, a.insby, a.insdt  FROM VIEW_LCOPRE_PPRENEWAL_FETCH a " +
                  " where a.cityid ='" + city + "' and a.plan_type='B'" +
                   " and upper(a.plan_name) like upper('" + search_text + "') " +
                  " and a.plan_poid not in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        //" and a.payterm ='" + payterm + "' " +
                   " order by a.plan_name asc";

                }



            }

            else if (planType == "AD")
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_jvfetch_free a " +
                        " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                        " and upper(a.plan_name) like upper('" + search_text + "') " +
                        " and a.plan_poid not in (" + addon_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                      " and  a.plan_poid2 in (" + basic_poids + ")";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_plan_fetch_Free a " +
                        " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                        " and upper(a.plan_name) like upper('" + search_text + "') " +
                        " and a.plan_poid not in (" + addon_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                      " and  a.plan_poid2 in (" + basic_poids + ")";
                }

            }
            else if (planType == "AL")
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                           " FROM view_lcopre_plan_JVfetchnew a " +
                           " where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + search_text + "%')  " +
                           " and a.plan_poid not in (" + ala_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                           " ) minus  " +
                           " ( " +
                           " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice " +
                           " FROM aoup_lcopre_jvplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_jvplan_def c " +
                           "  where a.var_plan_name=b.var_plan_name " +
                           " and c.var_plan_proviid=b.var_plan_provi " +
                           "  AND b.var_plan_city=a.num_plan_cityid " +
                           " and a.var_plan_plantype in ('AD','B') " +
                           " and c.var_plan_plantype='AL' " +
                           " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                           " and c.num_plan_cityid='" + city + "' " +
                           " ) ";
                }
                else
                {
                    str = " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                           " FROM view_lcopre_plan_fetchnew a " +
                           " where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + search_text + "%')  " +
                           " and a.plan_poid not in (" + ala_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                           " ) minus  " +
                           " ( " +
                           " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice " +
                           " FROM aoup_lcopre_plan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_plan_def c " +
                           "  where a.var_plan_name=b.var_plan_name " +
                           " and c.var_plan_proviid=b.var_plan_provi " +
                           "  AND b.var_plan_city=a.num_plan_cityid " +
                           " and a.var_plan_plantype in ('AD','B') " +
                           " and c.var_plan_plantype='AL' " +
                           " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                           " and c.num_plan_cityid='" + city + "' " +
                           " ) ";
                }
            }
            OracleCommand cmd = new OracleCommand(str, con);
            con.Open();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Double DiscountAmount = 0;

                if (dr["plan_type"].ToString() == "B")
                {
                    DiscountAmount = Convert.ToDouble(ViewState["DiscountAmount"].ToString());
                    if (ViewState["discounttype"].ToString() == "P")
                    {
                        double Customerprice = 0;
                        if (dr["cust_price"].ToString() != "")
                        {
                            Customerprice = Convert.ToDouble(dr["cust_price"].ToString());
                        }
                        DiscountAmount = Customerprice / 100 * DiscountAmount;

                    }
                }

                if (DiscountAmount > Convert.ToDouble(dr["cust_price"].ToString()))
                {
                    DiscountAmount = Convert.ToDouble(dr["cust_price"].ToString());
                }

                if (dr["plan_name"].ToString().Contains("FREE"))
                {
                    DiscountAmount = 0;
                }

                //lblchangediscount.Text = DiscountAmount.ToString();
                //lblChangePlan.Text = dr["plan_name"].ToString();

                //lblChangePlanMRP.Text = dr["cust_price"].ToString();
                //lblChangeplanLCO.Text = dr["lco_price"].ToString();
                //if (lblChangePlanMRP.Text != "")
                //{
                //    Hidchangplan.Value = lblChangePlanMRP.Text;
                //}
                ViewState["Newplan_Poid"] = dr["plan_poid"].ToString();



            }
            if (!dr.HasRows)
            {
                if (planType == "B")
                {
                    //msgbox("No Such channel Found", ddlPlanChange);
                }
                if (planType == "AD")
                {
                    //msgbox("No Such Plan Found", ddlPlanChange);
                }
                if (planType == "AL")
                {
                    //msgbox("No Such channel Found", ddlPlanChange);
                }

            }
            else
            {

                popchange.Show();
            }

            con.Close();
            //}
        }

        public bool getChangePlanPrice(string planType, string payterm)
        {
            bool checkstatus = false;

            //if (ddlPlanChange.SelectedItem.Text.Trim() != "")
            //{
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            string search_text = "";// ddlPlanChange.SelectedItem.Text.Trim();
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string basic_poids = "'0'";
            if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            {
                basic_poids = ViewState["basic_poids"].ToString();
            }
            string addon_poids = "'0'";
            if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
            {
                addon_poids = ViewState["addon_poids"].ToString();
            }
            string ala_poids = "'0'";
            if (ViewState["ala_poids"] != null && ViewState["ala_poids"].ToString() != "")
            {
                ala_poids = ViewState["ala_poids"].ToString();
            }


            if (planType == "B")
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                 " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fch_JVbasi a " +
                 " where a.cityid ='" + city + "' and a.plan_type='B'" +
                  " and upper(a.plan_name) like upper('" + search_text + "') " +
                 " and a.plan_poid not in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "' and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                        //" and a.payterm ='" + payterm + "' " +
                  " order by a.plan_name asc";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                  " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fetch_basi a " +
                  " where a.cityid ='" + city + "' and a.plan_type='B'" +
                   " and upper(a.plan_name) like upper('" + search_text + "') " +
                  " and a.plan_poid not in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "' and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                        // " and a.payterm ='" + payterm + "' " +
                   " order by a.plan_name asc";
                }
            }

            else if (planType == "AD")
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                      " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fch_JVfree a " +
                      " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                      " and upper(a.plan_name) like upper('" + search_text + "') " +
                      " and a.plan_poid not in (" + addon_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'  and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                    " and  a.plan_poid2 in (" + basic_poids + ")";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt  FROM view_lcopre_lcoplan_fetch_free a " +
                        " where a.cityid ='" + city + "' and a.plan_type='AD'" +
                        " and upper(a.plan_name) like upper('" + search_text + "') " +
                        " and a.plan_poid not in (" + addon_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'  and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                      " and  a.plan_poid2 in (" + basic_poids + ")";
                }

            }
            else if (planType == "AL")
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                          " FROM view_lcopre_lcoplan_JVfetchnew a " +
                          " where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + search_text + "%')  " +
                          " and a.plan_poid not in (" + ala_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'  and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                          " ) minus  " +
                          " ( " +
                          " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice " +
                          " FROM aoup_lcopre_lcojvplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_lcojvplan_def c " +
                          "  where a.var_plan_name=b.var_plan_name " +
                          " and c.var_plan_proviid=b.var_plan_provi " +
                          "  AND b.var_plan_city=a.num_plan_cityid " +
                          " and a.var_plan_plantype in ('AD','B') " +
                          " and c.var_plan_plantype='AL' " +
                          " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                          " and a.var_plan_lcocode='" + Session["lcoid"].ToString() + "'" +
                          " and c.num_plan_cityid='" + city + "' " +
                          " ) ";
                }
                else
                {
                    str = " (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price " +
                           " FROM view_lcopre_lcoplan_fetchnew a " +
                           " where a.plan_type='AL' and a.cityid ='" + city + "' and upper(a.plan_name) like upper('" + search_text + "%')  " +
                           " and a.plan_poid not in (" + ala_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'  and a.lcocode='" + Session["lcoid"].ToString() + "'" +
                           " ) minus  " +
                           " ( " +
                           " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice " +
                           " FROM aoup_lcopre_lcoplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_lcoplan_def c " +
                           "  where a.var_plan_name=b.var_plan_name " +
                           " and c.var_plan_proviid=b.var_plan_provi " +
                           "  AND b.var_plan_city=a.num_plan_cityid " +
                           " and a.var_plan_plantype in ('AD','B') " +
                           " and c.var_plan_plantype='AL' " +
                           " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                           " and a.var_plan_lcocode='" + Session["lcoid"].ToString() + "'" +
                           " and c.num_plan_cityid='" + city + "' " +
                           " ) ";
                }
            }
            OracleCommand cmd = new OracleCommand(str, con);
            con.Open();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Double DiscountAmount = 0;

                if (dr["plan_type"].ToString() == "B")
                {
                    DiscountAmount = Convert.ToDouble(ViewState["DiscountAmount"].ToString());
                    if (ViewState["discounttype"].ToString() == "P")
                    {
                        double Customerprice = 0;
                        if (dr["cust_price"].ToString() != "")
                        {
                            Customerprice = Convert.ToDouble(dr["cust_price"].ToString());
                        }
                        DiscountAmount = Customerprice / 100 * DiscountAmount;

                    }
                }

                if (DiscountAmount > Convert.ToDouble(dr["cust_price"].ToString()))
                {
                    DiscountAmount = Convert.ToDouble(dr["cust_price"].ToString());
                }

                if (dr["plan_name"].ToString().Contains("FREE"))
                {
                    DiscountAmount = 0;
                }

                //lblchangediscount.Text = DiscountAmount.ToString();

                //lblChangePlan.Text = dr["plan_name"].ToString();

                //lblChangePlanMRP.Text = dr["cust_price"].ToString();
                //lblChangeplanLCO.Text = dr["lco_price"].ToString();
                //if (lblChangePlanMRP.Text != "")
                //{
                //    Hidchangplan.Value = lblChangePlanMRP.Text;
                //}
                ViewState["Newplan_Poid"] = dr["plan_poid"].ToString();



            }

            if (!dr.HasRows)
            {
                checkstatus = false;

            }
            else
            {
                checkstatus = true;
                popchange.Show();
            }

            con.Close();
            // }

            return checkstatus;


        }

        protected void btnalacartechangeconfirm_Click(object sender, EventArgs e)
        {
            ViewState["NewPlanName"] = null;
            ViewState["Newplan_Poid"] = null;
            int checngcount = 0;
            StatbleDynamicTabs();
            foreach (GridViewRow gr in GrdchangePlan.Rows)
            {
                RadioButton rbtnchangeplanselect = (RadioButton)gr.Cells[5].FindControl("rbtnchangeplanselect");
                HiddenField hdnplanaddconfPlanPoid = (HiddenField)gr.Cells[5].FindControl("hdnplanaddconfPlanPoid");
                HiddenField hdnalacartechange = (HiddenField)gr.Cells[5].FindControl("hdnalacartechange");

                if (rbtnchangeplanselect.Checked == true)
                {
                    checngcount++;
                    Hidchangplan.Value = gr.Cells[1].Text.Trim();

                    Hidchangplanlco.Value = gr.Cells[2].Text.Trim();

                    ViewState["Newplan_Poid"] = hdnplanaddconfPlanPoid.Value.Trim();
                    ViewState["NewPlanName"] = HttpUtility.HtmlDecode(gr.Cells[0].Text.Trim());
                    // lnkatag_Click(null, null);


                    Cls_Data_Auth auth = new Cls_Data_Auth();
                    string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                    Hashtable htData = new Hashtable();


                    string _username = Session["lco_username"].ToString();
                    string _oper_id = Session["lcoid"].ToString();
                    htData.Add("username", _username);
                    htData.Add("lcoid", _oper_id);


                    if (ViewState["Planid"] != null)
                    {
                        htData["planid"] = ViewState["Planid"].ToString();
                    }
                    else
                    {
                        htData["planid"] = null;
                    }



                    htData.Add("expirydt", ViewState["expdt"].ToString());
                    htData.Add("actdt", ViewState["ActDate"].ToString());
                    htData.Add("in_custid", ViewState["customer_no"].ToString());
                    htData.Add("in_vcid", lblVCID.Text.ToString());
                    htData.Add("in_flag", "CH");


                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    string conResult = obj.getRefund(htData);
                    string[] result_arr = conResult.Split('#');

                    string[] amt = result_arr[1].Split('$');
                    //  9999#27$45$54$

                    if (result_arr[0] != "9999")
                    {
                        msgboxstr(result_arr[1]);
                    }
                    else
                    {
                        htData.Add("RefundAmt", amt[2]);
                        htData.Add("RefundAmtlco", amt[4]);
                        setChangePopup("This will change the plan with following details.", "Are you sure you want to change?", "R", "AD", htData);

                        MPEConfirmation.Show();
                        popchange.Hide(); //ajax control toolkit bug quickfix

                    }
                }
            }

            if (checngcount == 0)
            {
                msgboxstr("Please select Plan to change");
            }
        }
        protected void btnChangePlan_Click(object sender, EventArgs e)
        {
            ViewState["NewPlanName"] = null;
            ViewState["Newplan_Poid"] = null;
            ViewState["alacartechangebase"] = null;
            int checngcount = 0;
            foreach (GridViewRow gr in GrdchangePlan.Rows)
            {
                RadioButton rbtnchangeplanselect = (RadioButton)gr.Cells[5].FindControl("rbtnchangeplanselect");
                HiddenField hdnplanaddconfPlanPoid = (HiddenField)gr.Cells[5].FindControl("hdnplanaddconfPlanPoid");
                HiddenField hdnalacartechange = (HiddenField)gr.Cells[5].FindControl("hdnalacartechange");
                HiddenField hdnplanaddconfplantype = (HiddenField)gr.Cells[5].FindControl("hdnplanaddconfplantype");

                if (rbtnchangeplanselect.Checked == true)
                {
                    checngcount++;
                    Hidchangplan.Value = gr.Cells[1].Text.Trim();

                    Hidchangplanlco.Value = gr.Cells[2].Text.Trim();
                    ViewState["alacartechangebase"] = hdnplanaddconfplantype.Value;
                    if (hdnalacartechange.Value.Trim() == "Y")
                    {
                        popalacartebasechange.Show();
                        ViewState["alacartechangenew"] = hdnalacartechange.Value.Trim();

                        return;
                    }
                    ViewState["Newplan_Poid"] = hdnplanaddconfPlanPoid.Value.Trim();
                    ViewState["NewPlanName"] = HttpUtility.HtmlDecode(gr.Cells[0].Text.Trim());
                    // lnkatag_Click(null, null);


                    Cls_Data_Auth auth = new Cls_Data_Auth();
                    string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                    Hashtable htData = new Hashtable();


                    string _username = Session["username"].ToString();
                    string _oper_id = Session["lcoid"].ToString();
                    htData.Add("username", _username);
                    htData.Add("lcoid", _oper_id);


                    if (ViewState["Planid"] != null)
                    {
                        htData["planid"] = ViewState["Planid"].ToString();
                    }
                    else
                    {
                        htData["planid"] = null;
                    }



                    htData.Add("expirydt", ViewState["expdt"].ToString());
                    htData.Add("actdt", ViewState["ActDate"].ToString());
                    htData.Add("in_custid", ViewState["customer_no"].ToString());
                    htData.Add("in_vcid", lblVCID.Text.ToString());
                    htData.Add("in_flag", "CH");


                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    string conResult = obj.getRefund(htData);
                    string[] result_arr = conResult.Split('#');

                    string[] amt = result_arr[1].Split('$');
                    //  9999#27$45$54$

                    if (result_arr[0] != "9999")
                    {
                        msgboxstr(result_arr[1]);
                    }
                    else
                    {
                        htData.Add("RefundAmt", amt[2]);
                        htData.Add("RefundAmtlco", amt[4]);
                        setChangePopup("This will change the plan with following details.", "Are you sure you want to change?", "R", "AD", htData);

                        MPEConfirmation.Show();
                        popchange.Hide(); //ajax control toolkit bug quickfix
                        StatbleDynamicTabs();
                    }
                }
            }

            if (checngcount == 0)
            {
                msgboxstr("Please select Plan to change");
            }
        }
        //protected void btnChangePlan_Click(object sender, EventArgs e)
        //{
        //    if (ddlPlanChange.SelectedValue == "0")
        //    {
        //        msgboxstr("Please select Plan");
        //        return;
        //    }

        //    // lnkatag_Click(null, null);
        //    Cls_Data_Auth auth = new Cls_Data_Auth();
        //    string Ip = auth.GetIPAddress(HttpContext.Current.Request);
        //    Hashtable htData = new Hashtable();


        //    string _username = Session["username"].ToString();
        //    string _oper_id = Session["lcoid"].ToString();
        //    htData.Add("username", _username);
        //    htData.Add("lcoid", _oper_id);

        //    if (ViewState["Planid"] != null)
        //    {
        //        htData["planid"] = ViewState["Planid"].ToString();
        //    }
        //    else
        //    {
        //        htData["planid"] = null;
        //    }



        //    htData.Add("expirydt", ViewState["expdt"].ToString());
        //    htData.Add("in_custid", ViewState["customer_no"].ToString());
        //    htData.Add("in_vcid", lblVCID.Text.ToString());
        //   //htData.Add("in_flag", "CH");//need to remove

        //    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
        //    string conResult = obj.getRefund(htData);
        //    string[] result_arr = conResult.Split('#');

        //    string[] amt = result_arr[1].Split('$');
        //    //  9999#27$45$54$

        //    if (result_arr[0] != "9999")
        //    {
        //        msgboxstr(result_arr[1]);
        //    }
        //    else
        //    {
        //        htData.Add("RefundAmt", amt[2]);
        //        setChangePopup("This will change the plan with following details.", "Are you sure you want to change?", "R", "AD", htData);

        //        MPEConfirmation.Show();
        //        popchange.Hide(); //ajax control toolkit bug quickfix
        //        StatbleDynamicTabs();
        //    }
        //}


        protected void btnChangePlanConfirmation_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            ViewState["ErrorMessage"] = "";

            if (ViewState["ChangedBasicPlanFOCChange"] == null)
            {
                nullFOCViewState();
                ViewState["ChangedBasicPlanFOCChange"] = "Yes";
                fillFreePlanGrid(ViewState["Newplan_Poid"].ToString(), "N");        //Added By vivek 16-Feb-2016

                if (ViewState["Changefoccheck"] != null)
                {
                    return;
                }
                if (grdFreePlan.Rows.Count > 0)
                {
                    PopUpFreePlan.Show();
                    ViewState["ChangedBasicPlanFOCChange"] = "Yes";   //Change FOC Pack While Changing Basic Pack...  
                    StatbleDynamicTabs();
                    return;
                }
            }
            string _username = "";
            string _oper_id = "";
            if (Session["username"] != null && Session["lcoid"] != null)
            {
                _username = Session["username"].ToString();
                _oper_id = Session["lcoid"].ToString();

            }

            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
            string addplanbalancecheck = obj.Funaddplanbalancecheck(_username, _oper_id, ViewState["Newplan_Poid"].ToString());

            // FileLogTextChange("Admin", "Funaddplanbalancecheck", " Response:" + addplanbalancecheck, "");
            string[] res = addplanbalancecheck.Split('$');


            if (res[0] == "9999")
            {

                //FileLogTextChange("Admin", "Funaddplanbalancecheck", " Response:" + 9999, "");


                bool status = processChangePlanTransaction("CH", ViewState["Planid"].ToString(), ViewState["expdt"].ToString(), "", "");

                DataRow row1 = dtFOCPlanStatus.NewRow();
                row1["PlanName"] = ViewState["OldPlanName"].ToString();
                row1["RenewStatus"] = ViewState["ErrorMessage"];
                dtFOCPlanStatus.Rows.Add(row1);

                if (status == true)
                {
                    bool statusadd = processChangePlanTransaction("A", ViewState["Newplan_Poid"].ToString(), ViewState["expdt"].ToString(), "", "");


                    if (statusadd == true)
                    {
                        ViewState["basic_poids"] = ViewState["Newplan_Poid"];
                    }
                    DataRow row2 = dtFOCPlanStatus.NewRow();
                    row2["PlanName"] = ViewState["NewPlanName"].ToString();
                    row2["RenewStatus"] = ViewState["ErrorMessage"];
                    dtFOCPlanStatus.Rows.Add(row2);

                    if (ViewState["Changefoccheck"] != null)
                    {
                        for (int i = 0; i < grdAddOnPlanReg.Rows.Count; i++)
                        {
                            String planexist = ((HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPlanPoid")).Value;
                            HiddenField hdnPlanName = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPlanName");
                            if (hdnPlanName.Value.Contains("FREE"))
                            {
                                CancelSelectedFOCPlan += planexist + ",";
                            }
                        }
                        ViewState["CancelSelectedFOCPlanId"] = CancelSelectedFOCPlan.TrimEnd(',');
                    }

                    if (ViewState["alacartechangenew"].ToString() == "N")
                    {
                        if (ViewState["BasicPlanChangeWithFOC"] != null)
                        {
                            if (ViewState["BasicPlanChangeWithFOC"].ToString() == "Y")
                            {
                                SendFOCCancelRequestOneByOne();         //Added By vivek 18-Feb-2016 for Canceling Old Basic Plan - FOC Pack   
                                ViewState["BasicPlanChangeWithFOC"] = null;
                                ViewState["CancelSelectedFOCPlanId"] = null;
                            }
                        }
                    }



                    if (ViewState["OldPlanName"].ToString().Contains("BASIC SERVICE TIER") == true || ViewState["OldPlanName"].ToString().Contains("PRIME") == true)
                    {
                        string _tvType = "";
                        DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                        DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                        string _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                        if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                        {
                            _tvType = "MAIN";
                        }
                        else
                        {
                            _tvType = "CHILD";
                        }
                        for (int rowindexi = 1; rowindexi <= grdAddOnPlan.Rows.Count; rowindexi++)
                        {
                            int rowindex = rowindexi - 1;
                            HiddenField hdnPlanName = (HiddenField)grdAddOnPlan.Rows[rowindex].FindControl("hdnADPlanName");

                            HiddenField hdnPlanPoid = (HiddenField)grdAddOnPlan.Rows[rowindex].FindControl("hdnADPlanPoid");
                            HiddenField hdnDealPoid = (HiddenField)grdAddOnPlan.Rows[rowindex].FindControl("hdnADDealPoid");
                            HiddenField hdnCustPrice = (HiddenField)grdAddOnPlan.Rows[rowindex].FindControl("hdnADCustPrice");
                            HiddenField hdnLcoPrice = (HiddenField)grdAddOnPlan.Rows[rowindex].FindControl("hdnADLcoPrice");
                            HiddenField hdnActivation = (HiddenField)grdAddOnPlan.Rows[rowindex].FindControl("hdnADActivation");
                            HiddenField hdnExpiry = (HiddenField)grdAddOnPlan.Rows[rowindex].FindControl("hdnADExpiry");
                            HiddenField hdnPackageId = (HiddenField)grdAddOnPlan.Rows[rowindex].FindControl("hdnADPackageId");
                            HiddenField hdnPurchasePoid = (HiddenField)grdAddOnPlan.Rows[rowindex].FindControl("hdnADPurchasePoid");

                            if (hdnPlanName.Value.Trim().Contains("SPECIAL") == true)
                            {
                                ViewState["changec"] = "CH";
                                Cancellation_ProcessSpecial(hdnPlanPoid.Value.Trim(), "SPECIAL", hdnPlanName.Value.Trim(), _vc_id);
                                break;
                            }
                        }
                    }

                    if (ViewState["alacartebaseplan"].ToString() == "Y" && ViewState["alacartechangebase"].ToString() == "B")
                    {
                        Cls_Data_Auth auth = new Cls_Data_Auth();
                        string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                        foreach (GridViewRow gr in grdCartefree.Rows)
                        {

                            HiddenField hdnPlanName = (HiddenField)gr.FindControl("hdnALPlanName");
                            HiddenField hdnPlanPoid = (HiddenField)gr.FindControl("hdnALPlanPoid");
                            HiddenField hdnDealPoid = (HiddenField)gr.FindControl("hdnALDealPoid");
                            HiddenField hdnCustPrice = (HiddenField)gr.FindControl("hdnALCustPrice");
                            HiddenField hdnLcoPrice = (HiddenField)gr.FindControl("hdnALLcoPrice");
                            HiddenField hdnActivation = (HiddenField)gr.FindControl("hdnALActivation");
                            HiddenField hdnExpiry = (HiddenField)gr.FindControl("hdnALExpiry");
                            HiddenField hdnPackageId = (HiddenField)gr.FindControl("hdnALPackageId");
                            HiddenField hdnPurchasePoid = (HiddenField)gr.FindControl("hdnALPurchasePoid");

                            if (hdnPlanName.Value.Trim().Contains("FREE"))
                            {
                                CheckBox cbAlaAutorenew = (CheckBox)gr.FindControl("cbAlaAutorenew");
                                Hashtable htData = new Hashtable();
                                htData["planname"] = hdnPlanName.Value;
                                htData["planpoid"] = hdnPlanPoid.Value;
                                htData["dealpoid"] = hdnDealPoid.Value;
                                htData["custprice"] = hdnCustPrice.Value;
                                htData["lcoprice"] = hdnLcoPrice.Value;
                                htData["activation"] = hdnActivation.Value;
                                htData["expiry"] = hdnExpiry.Value;
                                htData["packageid"] = hdnPackageId.Value;
                                htData["purchasepoid"] = hdnPurchasePoid.Value;
                                htData["IP"] = Ip;

                                if (cbAlaAutorenew.Checked)
                                {
                                    htData["autorenew"] = "Y";
                                }
                                else
                                {
                                    htData["autorenew"] = "N";
                                }

                                ViewState["transaction_data"] = htData;

                                DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                                DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                                DataTable myResultSet_flag = sortedDT.Select("PARENT_CHILD_FLAG='0'").CopyToDataTable();

                                String _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                                String _tvType = "";
                                String maintStatus = "";
                                if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                                {
                                    _tvType = "MAIN";
                                }
                                else
                                {
                                    _tvType = "CHILD";
                                }
                                if (myResultSet_flag.Rows[0]["Status"].ToString() == "10100")
                                {
                                    maintStatus = "ACTIVE";
                                }
                                else
                                {
                                    maintStatus = "INACTIVE";
                                }

                                processTransaction_cancellation(_vc_id, _tvType, maintStatus, ViewState["ServicePoid"].ToString());

                                DataRow row3 = dtFOCPlanStatus.NewRow();
                                row3["PlanName"] = hdnPlanName.Value;
                                row3["RenewStatus"] = ViewState["ErrorMessage"];
                                dtFOCPlanStatus.Rows.Add(row3);
                            }
                        }

                    }

                    if (ViewState["alacartechangenew"].ToString() == "Y" && ViewState["alacartechangebase"].ToString() == "B")
                    {
                        Cls_Data_Auth auth = new Cls_Data_Auth();
                        string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                        foreach (GridViewRow gr in grdAddOnPlan.Rows)
                        {
                            HiddenField hdnPlanName = (HiddenField)gr.FindControl("hdnADPlanName");
                            HiddenField hdnPlanPoid = (HiddenField)gr.FindControl("hdnADPlanPoid");
                            HiddenField hdnDealPoid = (HiddenField)gr.FindControl("hdnADDealPoid");
                            HiddenField hdnCustPrice = (HiddenField)gr.FindControl("hdnADDealPoid");
                            HiddenField hdnLcoPrice = (HiddenField)gr.FindControl("hdnADLcoPrice");
                            HiddenField hdnActivation = (HiddenField)gr.FindControl("hdnADActivation");
                            HiddenField hdnExpiry = (HiddenField)gr.FindControl("hdnADExpiry");
                            HiddenField hdnPackageId = (HiddenField)gr.FindControl("hdnADPackageId");
                            HiddenField hdnPurchasePoid = (HiddenField)gr.FindControl("hdnADPurchasePoid");

                            CheckBox cbAlaAutorenew = (CheckBox)gr.FindControl("cbAddonrenew");
                            Hashtable htData = new Hashtable();
                            htData["planname"] = hdnPlanName.Value;
                            htData["planpoid"] = hdnPlanPoid.Value;
                            htData["dealpoid"] = hdnDealPoid.Value;
                            htData["custprice"] = hdnCustPrice.Value;
                            htData["lcoprice"] = hdnLcoPrice.Value;
                            htData["activation"] = hdnActivation.Value;
                            htData["expiry"] = hdnExpiry.Value;
                            htData["packageid"] = hdnPackageId.Value;
                            htData["purchasepoid"] = hdnPurchasePoid.Value;
                            htData["IP"] = Ip;

                            if (cbAlaAutorenew.Checked)
                            {
                                htData["autorenew"] = "Y";
                            }
                            else
                            {
                                htData["autorenew"] = "N";
                            }

                            ViewState["transaction_data"] = htData;

                            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                            DataTable myResultSet_flag = sortedDT.Select("PARENT_CHILD_FLAG='0'").CopyToDataTable();

                            String _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                            String _tvType = "";
                            String maintStatus = "";
                            if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                            {
                                _tvType = "MAIN";
                            }
                            else
                            {
                                _tvType = "CHILD";
                            }
                            if (myResultSet_flag.Rows[0]["Status"].ToString() == "10100")
                            {
                                maintStatus = "ACTIVE";
                            }
                            else
                            {
                                maintStatus = "INACTIVE";
                            }

                            processTransaction_cancellation(_vc_id, _tvType, maintStatus, ViewState["ServicePoid"].ToString());

                            DataRow row3 = dtFOCPlanStatus.NewRow();
                            row3["PlanName"] = hdnPlanName.Value;
                            row3["RenewStatus"] = ViewState["ErrorMessage"];
                            dtFOCPlanStatus.Rows.Add(row3);
                        }

                        foreach (GridViewRow gr in grdAddOnPlanReg.Rows)
                        {
                            HiddenField hdnPlanName = (HiddenField)gr.FindControl("hdnADPlanName");
                            HiddenField hdnPlanPoid = (HiddenField)gr.FindControl("hdnADPlanPoid");
                            HiddenField hdnDealPoid = (HiddenField)gr.FindControl("hdnADDealPoid");
                            HiddenField hdnCustPrice = (HiddenField)gr.FindControl("hdnADDealPoid");
                            HiddenField hdnLcoPrice = (HiddenField)gr.FindControl("hdnADLcoPrice");
                            HiddenField hdnActivation = (HiddenField)gr.FindControl("hdnADActivation");
                            HiddenField hdnExpiry = (HiddenField)gr.FindControl("hdnADExpiry");
                            HiddenField hdnPackageId = (HiddenField)gr.FindControl("hdnADPackageId");
                            HiddenField hdnPurchasePoid = (HiddenField)gr.FindControl("hdnADPurchasePoid");

                            CheckBox cbAlaAutorenew = (CheckBox)gr.FindControl("cbAddonrenew");
                            Hashtable htData = new Hashtable();
                            htData["planname"] = hdnPlanName.Value;
                            htData["planpoid"] = hdnPlanPoid.Value;
                            htData["dealpoid"] = hdnDealPoid.Value;
                            htData["custprice"] = hdnCustPrice.Value;
                            htData["lcoprice"] = hdnLcoPrice.Value;
                            htData["activation"] = hdnActivation.Value;
                            htData["expiry"] = hdnExpiry.Value;
                            htData["packageid"] = hdnPackageId.Value;
                            htData["purchasepoid"] = hdnPurchasePoid.Value;
                            htData["IP"] = Ip;

                            if (cbAlaAutorenew.Checked)
                            {
                                htData["autorenew"] = "Y";
                            }
                            else
                            {
                                htData["autorenew"] = "N";
                            }

                            ViewState["transaction_data"] = htData;

                            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                            DataTable myResultSet_flag = sortedDT.Select("PARENT_CHILD_FLAG='0'").CopyToDataTable();

                            String _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                            String _tvType = "";
                            String maintStatus = "";
                            if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                            {
                                _tvType = "MAIN";
                            }
                            else
                            {
                                _tvType = "CHILD";
                            }
                            if (myResultSet_flag.Rows[0]["Status"].ToString() == "10100")
                            {
                                maintStatus = "ACTIVE";
                            }
                            else
                            {
                                maintStatus = "INACTIVE";
                            }

                            processTransaction_cancellation(_vc_id, _tvType, maintStatus, ViewState["ServicePoid"].ToString());

                            DataRow row3 = dtFOCPlanStatus.NewRow();
                            row3["PlanName"] = hdnPlanName.Value;
                            row3["RenewStatus"] = ViewState["ErrorMessage"];
                            dtFOCPlanStatus.Rows.Add(row3);
                        }


                        foreach (GridViewRow gr in Grdhathwayspecial.Rows)
                        {
                            HiddenField hdnPlanName = (HiddenField)gr.FindControl("hdnADPlanName");
                            HiddenField hdnPlanPoid = (HiddenField)gr.FindControl("hdnADPlanPoid");
                            HiddenField hdnDealPoid = (HiddenField)gr.FindControl("hdnADDealPoid");
                            HiddenField hdnCustPrice = (HiddenField)gr.FindControl("hdnADDealPoid");
                            HiddenField hdnLcoPrice = (HiddenField)gr.FindControl("hdnADLcoPrice");
                            HiddenField hdnActivation = (HiddenField)gr.FindControl("hdnADActivation");
                            HiddenField hdnExpiry = (HiddenField)gr.FindControl("hdnADExpiry");
                            HiddenField hdnPackageId = (HiddenField)gr.FindControl("hdnADPackageId");
                            HiddenField hdnPurchasePoid = (HiddenField)gr.FindControl("hdnADPurchasePoid");

                            CheckBox cbAlaAutorenew = (CheckBox)gr.FindControl("cbAddonrenew");
                            Hashtable htData = new Hashtable();
                            htData["planname"] = hdnPlanName.Value;
                            htData["planpoid"] = hdnPlanPoid.Value;
                            htData["dealpoid"] = hdnDealPoid.Value;
                            htData["custprice"] = hdnCustPrice.Value;
                            htData["lcoprice"] = hdnLcoPrice.Value;
                            htData["activation"] = hdnActivation.Value;
                            htData["expiry"] = hdnExpiry.Value;
                            htData["packageid"] = hdnPackageId.Value;
                            htData["purchasepoid"] = hdnPurchasePoid.Value;
                            htData["IP"] = Ip;

                            if (cbAlaAutorenew.Checked)
                            {
                                htData["autorenew"] = "Y";
                            }
                            else
                            {
                                htData["autorenew"] = "N";
                            }

                            ViewState["transaction_data"] = htData;

                            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                            DataTable myResultSet_flag = sortedDT.Select("PARENT_CHILD_FLAG='0'").CopyToDataTable();

                            String _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                            String _tvType = "";
                            String maintStatus = "";
                            if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                            {
                                _tvType = "MAIN";
                            }
                            else
                            {
                                _tvType = "CHILD";
                            }
                            if (myResultSet_flag.Rows[0]["Status"].ToString() == "10100")
                            {
                                maintStatus = "ACTIVE";
                            }
                            else
                            {
                                maintStatus = "INACTIVE";
                            }

                            processTransaction_cancellation(_vc_id, _tvType, maintStatus, ViewState["ServicePoid"].ToString());

                            DataRow row3 = dtFOCPlanStatus.NewRow();
                            row3["PlanName"] = hdnPlanName.Value;
                            row3["RenewStatus"] = ViewState["ErrorMessage"];
                            dtFOCPlanStatus.Rows.Add(row3);
                        }

                    }


                    if (statusadd == true)
                    {
                        if (ViewState["freeplanpoid"] != null)
                        {
                            btnPopupFinalConfYes_Click(null, null);               //Added By vivek 18-Feb-2016 for Adding New Basic Plan - FOC Pack
                        }

                        // msgboxstr("Plan has been successfully changed."); 
                        Gridrenew.DataSource = dtFOCPlanStatus;
                        Gridrenew.DataBind();
                        lblAllStatus.Text = "Pack Status";
                        Gridrenew.HeaderRow.Cells[1].Text = "Change Status";
                        popMsg.Hide();
                        popallrenewal.Show();

                    }
                    else
                    {
                        Gridrenew.DataSource = dtFOCPlanStatus;
                        Gridrenew.DataBind();
                        lblAllStatus.Text = "Pack Status";
                        Gridrenew.HeaderRow.Cells[1].Text = "Change Status";
                        popMsg.Hide();
                        popallrenewal.Show();
                        // msgboxstr("Cancel plan changed successfully but New Plan not added ");
                    }

                    nullFOCViewState();

                }
                else
                {
                    msgboxstr(ViewState["ErrorMessage"] + ",plan could not be changed.");
                    nullFOCViewState();
                }
            }
            else
            {


                DataRow row1 = dtFOCPlanStatus.NewRow();
                //FileLogTextChange("Admin", "Funaddplanbalancecheck", " Response:else", "");
                row1["PlanName"] = res[1].ToString();
                row1["RenewStatus"] = res[2].ToString();
                dtFOCPlanStatus.Rows.Add(row1);



                Gridrenew.DataSource = dtFOCPlanStatus;
                Gridrenew.DataBind();

                //Added by Vivek 18-Feb-2016
                Gridrenew.HeaderRow.Cells[1].Text = "Change Pack Status";
                lblAllStatus.Text = "Change Pack Status";

                popallrenewal.Show();

            }
            StatbleDynamicTabs();
            // processChangePlanTransaction("A", ViewState["Newplan_Poid"].ToString());

        }

        protected void setChangePopup(string message1, string message2, string flag, string grid, Hashtable ht)
        {
            try
            {

                lblPopupText1.Text = message1;
                lblPopupText2.Text = message2;
                lblConfirmOldPlan.Text = lblOldPlan.Text;
                ViewState["OldPlanName"] = lblOldPlan.Text;
                //ViewState["NewPlanName"] = lblChangePlan.Text;
                lblConfirmNewPlan.Text = Convert.ToString(ViewState["NewPlanName"]);
                lblConfirmRefundAmount.Text = ht["RefundAmt"].ToString();

                decimal Hidchangplans = decimal.Parse(Hidchangplan.Value);
                decimal lblConfirmRefundAmountvalue = decimal.Parse(lblConfirmRefundAmount.Text);


                payamount.Text = (Hidchangplans - lblConfirmRefundAmountvalue).ToString();

                lblConfirmRefundlcoAmount.Text = ht["RefundAmtlco"].ToString();
                decimal Hidchangplanslco = decimal.Parse(Hidchangplanlco.Value);
                decimal lblConfirmRefundAmountvaluelco = decimal.Parse(lblConfirmRefundlcoAmount.Text);
                payamountLCO.Text = (Hidchangplanslco - lblConfirmRefundAmountvaluelco).ToString();
            }
            catch (Exception ex)
            {

            }


            //hdnPopupType.Value = grid;
        }

        protected bool processChangePlanTransaction(string transaction_action, string planpoid, string expdate, string purchage_id, string activationdate)
        {
            try
            {
                //gathering data
                string ChangeStatus = "";
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string plan_poid = planpoid;
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string _STB_NO = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";
                string oldaction = transaction_action;
                string maintStatus = "INACTIVE";

                if (Session["username"] != null && Session["operator_id"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["lcoid"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }

                try
                {
                    DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                    DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();

                    DataTable myResultSet_flag = sortedDT.Select("PARENT_CHILD_FLAG='0'").CopyToDataTable();

                    _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                    _STB_NO = myResultSet.Rows[0]["STB_NO"].ToString();
                    if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                    {
                        _tvType = "MAIN";
                    }
                    else
                    {
                        _tvType = "CHILD";
                    }
                    if (myResultSet_flag.Rows[0]["Status"].ToString() == "10100")
                    {
                        maintStatus = "ACTIVE";
                    }
                    else
                    {
                        maintStatus = "INACTIVE";
                    }

                    if (ViewState["AddedFOC"] == null)
                    {
                        ViewState["AddedFOC"] = "0";
                    }
                }
                catch (Exception ex)
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";
                    msgboxstr("Something went wrong while transaction. Plan data not found.");
                    Cls_Security objSecurity = new Cls_Security();
                    objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-processChangePlanTransaction");
                    return false;
                }


                //processing 
                string Request = "";
                //validating...
                if (transaction_action == "A")
                {
                    activation_date = "";
                    expiry_date = "";
                }
                else if (transaction_action == "CH")
                {

                    activation_date = ViewState["ActDate"].ToString();
                    expiry_date = ViewState["expdt"].ToString();

                }
                else if (transaction_action == "R")
                {

                    activation_date = activationdate;
                    expiry_date = expdate;

                }

                if (ViewState["ServicePoid"] != null && ViewState["accountPoid"] != null)
                {
                    if (transaction_action == "A")
                    {
                        Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid;
                    }
                    else if (transaction_action == "CH")
                    {
                        Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + ViewState["Packageid"] + "$" + ViewState["Dealid"];
                        // Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["packageid"] + "$" + htPlanData["dealpoid"];
                    }
                    else if (transaction_action == "R")
                    {
                        Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + purchage_id;
                    }






                    ht.Add("username", _username);
                    ht.Add("lcoid", _oper_id);
                    ht.Add("custid", lblCustNo.Text.Trim());
                    ht.Add("vcid", _vc_id);
                    ht.Add("STBNO", _STB_NO);
                    ht.Add("custname", lblCustName.Text.Trim());
                    ht.Add("cust_addr", lblCustAddr.Text.Trim());
                    ht.Add("planid", plan_poid);
                    ht.Add("flag", transaction_action);
                    ht.Add("expdate", expiry_date);
                    ht.Add("actidate", activation_date);
                    ht.Add("request", Request);
                    ht.Add("reason_id", reason_id);
                    ht.Add("IP", Ip);

                    ht.Add("MainTVStatus", maintStatus);
                    ht.Add("TVType", _tvType);
                    ht.Add("DeviceType", ViewState["Device_Type"].ToString());

                    ht.Add("FOCCount", ViewState["AddedFOC"].ToString());

                    ht.Add("BasicPoid", ViewState["basic_poids"].ToString());
                    ht.Add("addon_poids", Convert.ToString(ViewState["addon_poids"]).Replace("'", ""));
                    ht.Add("bucket1foc", Convert.ToString(ViewState["bucket1foc"]));
                    ht.Add("bucket2foc", Convert.ToString(ViewState["bucket2foc"]));
                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    string response = obj.ValidateProvTrans(ht);
                    //FileLogTextChange("Admin", "ValidateProvTransChange" + transaction_action, " Error:" + response, "");
                    string[] res = response.Split('$');

                    if (res[0] != "9999")
                    {
                        if (transaction_action == "C")
                        {
                            msgboxstr(res[1]);

                        }
                        ViewState["ErrorMessage"] = "something went wrong, Error from Atyeti:" + res[1];
                        return false;
                    }
                    else
                    {
                        request_id = res[1];
                    }
                }
                else
                {
                    if (transaction_action == "CH")
                    {
                        msgboxstr("something went wrong, Service or account details not found...Please relogin");
                    }
                    ViewState["ErrorMessage"] = "something went wrong,Error from Atyeti:Service or account details not found...Please relogin";
                    return false;
                }

                //OBRM call to get response...
                // Request = _username + "$" + Request;

                //FileLogTextChange("Admin", "ChangeRequestCancel" + transaction_action, " Error:" + Request, "");
                string req_code = "";
                if (transaction_action == "A")
                {
                    req_code = "5";
                }

                else if (transaction_action == "CH")
                {
                    req_code = "8";
                }
                else if (transaction_action == "R")
                {
                    req_code = "7";
                }
                Request = _user_brmpoid + "$" + Request;
                string api_response = callAPI(Request, req_code);
                //string api_response = "0$ACCOUNT - Service add plan completed successfully$0.0.0.1 /account 81788441 9$0.0.0.1 /service/catv 81788185 39";
                string[] final_obrm_status = api_response.Split('$');
                // FileLogTextChange("Admin", "callAPIChange " + transaction_action, " Error:" + api_response, "");
                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";

                try
                {
                    if (obrm_status == "0" || obrm_status == "1")
                    {
                        obrm_msg = final_obrm_status[2];
                    }
                    else
                    {
                        obrm_status = "1";
                        obrm_msg = api_response;

                    }
                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    obrm_msg = api_response;
                    ViewState["ErrorMessage"] = api_response.ToString();
                }

                ht.Add("obrmsts", obrm_status);
                ht.Add("request_id", request_id);
                ht.Add("response", api_response);

                Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
                string resp = obj1.ProvTransRes(ht); // "9999";
                //FileLogTextChange("Admin", "ProvTransResChange" + transaction_action, " Error:" + resp, "");
                string[] finalres = resp.Split('$');
                if (finalres[0] == "9999")
                {
                    if (transaction_action == "CH")
                    {
                        msgboxstr_refresh("Transaction successful : " + obrm_msg);
                    }
                    ViewState["ErrorMessage"] = "Transaction successful : " + obrm_msg;
                }
                else
                {
                    if (obrm_status == "0")
                    {
                        if (transaction_action == "CH")
                        {
                            msgboxstr_refresh("Transaction successful by OBRM but failure at Atyeti : " + finalres[1]);


                        }
                        ViewState["ErrorMessage"] = "Transaction successful by OBRM but failure at Atyeti : " + finalres[1];
                    }
                    else
                    {
                        if (transaction_action == "CH")
                        {
                            msgboxstr("Transaction failed : " + finalres[1] + " : " + obrm_msg);
                            ViewState["ErrorMessage"] = "Transaction failed from OBRM";
                        }
                        ViewState["ErrorMessage"] = "Transaction failed from OBRM.";
                    }
                    string updated_bal = getAvailableBal();
                    lblAvailBal.Text = updated_bal;
                    return false;

                }

                string updated_bal1 = getAvailableBal();
                lblAvailBal.Text = updated_bal1;
                return true;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                ViewState["ErrorMessage"] = ex.Message;
                objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransaction");
                // FileLogTextChange("Admin", "ExceptionChange" + transaction_action, " Error:" + ex.Message, "");
                return false;
            }
        }

        protected void lnkBRenew_Click(object sender, EventArgs e)
        {
            // lnkatag_Click(null, null);
            if (ViewState["BasicActionFlag"] != null)
            {

                if (ViewState["BasicActionFlag"].ToString() == "ND")     // created by vivek 16-nov-2015
                {
                    btnRefreshForm.Visible = false;
                    lblPopupResponse.Text = "Basic Pack is not due,you can't Renew the pack";
                    popMsg.Show();
                    StatbleDynamicTabs();
                    return;
                }
            }

            int rindex = (((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex;
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);

            //   HiddenField hdnPlanId = (HiddenField)grdAddOnPlan.Rows[rindex].FindControl("hdnADPlanId");
            HiddenField hdnBasicPlanName = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanName");
            HiddenField hdnBasicPlanPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanPoid");
            HiddenField hdnBasicDealPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicDealPoid");
            HiddenField hdnBasicCustPrice = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicCustPrice");
            HiddenField hdnBasicLcoPrice = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicLcoPrice");
            HiddenField hdnBasicActivation = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicActivation");
            //  HiddenField hdnProductPoid = (HiddenField)grdAddOnPlan.Rows[rindex].FindControl("hdnADProductPoid");
            HiddenField hdnBasicExpiry = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicExpiry");
            HiddenField hdnBasicPackageId = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPackageId");
            HiddenField hdnBasicPurchasePoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPurchasePoid");
            HiddenField hdnBasicPlanStatus = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanStatus");

            Hashtable htData = new Hashtable();

            htData["planname"] = hdnBasicPlanName.Value;

            htData["planpoid"] = hdnBasicPlanPoid.Value;
            htData["dealpoid"] = hdnBasicDealPoid.Value;
            htData["custprice"] = hdnBasicCustPrice.Value;
            htData["lcoprice"] = hdnBasicLcoPrice.Value;
            htData["activation"] = hdnBasicActivation.Value;
            htData["expiry"] = hdnBasicExpiry.Value;
            htData["purchasepoid"] = hdnBasicPurchasePoid.Value;
            htData["IP"] = Ip;
            htData["Plan_Type"] = "B";
            htData["autorenew"] = "N";

            string conResult = callGetProviConfirm(htData, "R");
            if (conResult == null)
            {
                popAdd.Hide(); //ajax control toolkit bug quickfix
                StatbleDynamicTabs();
                return;
            }
            else
            {
                htData["discountamt"] = 0;
                //Double DiscountAmount = 0;

                //DiscountAmount = Convert.ToDouble(ViewState["DiscountAmount"].ToString());
                //if (ViewState["discounttype"].ToString() == "P")
                //{
                //    double Customerprice = 0;
                //    if (htData["custprice"].ToString() != "")
                //    {
                //        Customerprice = Convert.ToDouble(htData["custprice"].ToString());
                //    }
                //    DiscountAmount = Customerprice / 100 * DiscountAmount;

                //}


                //if (DiscountAmount > Convert.ToDouble(htData["custprice"].ToString()))
                //{
                //    DiscountAmount = Convert.ToDouble(htData["custprice"].ToString());
                //}

                //htData["discountamt"] = DiscountAmount.ToString();
                DataTable dtRenew = new DataTable();
                dtRenew.Columns.Add(new DataColumn("PLAN_NAME"));
                dtRenew.Columns.Add(new DataColumn("CUST_PRICE", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("LCO_PRICE", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("discount", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("netmrp", typeof(double)));
                dtRenew.Columns.Add(new DataColumn("Activation"));
                dtRenew.Columns.Add(new DataColumn("valid_upto"));
                dtRenew.Columns.Add(new DataColumn("plan_poid"));
                dtRenew.Columns.Add(new DataColumn("DEAL_POID"));
                dtRenew.Columns.Add(new DataColumn("plan_type"));
                dtRenew.Columns.Add(new DataColumn("AutoRenew"));

                DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                string stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
                string vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                string selected_service_str = myResultSet.Rows[0]["SERVICE_STRING"].ToString();
                selected_service_str = selected_service_str.Replace("|", "$");
                Cls_Business_TxnAssignPlan objPlan = new Cls_Business_TxnAssignPlan();
                string strPLANPOISs = hdnBasicPlanPoid.Value;
                //FileLogTextChange1("request", selected_service_str, "-------" + strPLANPOISs + "--------", Session["username"].ToString());
                string strNCFPlanList = objPlan.Check_NFCPlan(Session["username"].ToString(), selected_service_str, ViewState["cityid"].ToString(), ViewState["customer_no"].ToString(), Session["operator_id"].ToString(), strPLANPOISs, "R", vc_id);
                string[] strPlanlist = strNCFPlanList.Split('$');
                if (strPlanlist[0] == "9999")
                {
                    if (strPlanlist[1] == "Y")
                    {
                        DataTable dt = new DataTable();
                        dt = objPlan.getNCFPlanDetails(Session["username"].ToString(), strPlanlist[2].ToString(), ViewState["JVFlag"].ToString(), ViewState["cityid"].ToString(), Session["dasarea"].ToString(), Convert.ToString(Session["operator_id"]), Convert.ToString(Session["JVNO"]));
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //htData["planpoid"] += dt.Rows[i][5].ToString() + ",";
                            //htData["planname"] += dt.Rows[i][0].ToString()+",";
                            //htData["custprice"] +=dt.Rows[i][1].ToString()+",";
                            //htData["lcoprice"] += dt.Rows[i][2].ToString() + ",";
                            //htData["dealpoid"] += dt.Rows[i][6].ToString() + ",";
                            //htData["dealpoid"] += dt.Rows[i][6].ToString() + ",";
                            //htData["Plan_Type"] += "B"+",";
                            //htData["autorenew"] += "N"+",";
                            dtRenew.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), 0, dt.Rows[i][1].ToString(), "", "", dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), htData["activation"], "N");
                        }
                    }
                }
                dtRenew.Rows.Add(htData["planname"], htData["custprice"], htData["lcoprice"], 0, htData["custprice"], htData["activation"], htData["expiry"], htData["planpoid"], htData["dealpoid"], htData["activation"], "N");
                if (dtRenew.Rows.Count > 0)
                {
                    GrdRenewConfrim.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim2.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim.DataSource = dtRenew;
                    GrdRenewConfrim.DataBind();
                    GrdRenewConfrim2.DataSource = dtRenew;
                    GrdRenewConfrim2.DataBind();
                }
                setPopup("This will renew the plan with following details.", "Are you sure you want to renew?", "R", "B", htData);
                pop.Show();
                popAdd.Hide(); //ajax control toolkit bug quickfix
                StatbleDynamicTabs();
            }
        }

        protected void rbtnPayterm_SelectedIndexChanged(object sender, EventArgs e) // created  by vivek 13-nov-2015
        {


            try
            {
                //lnkatag_Click(null, null);

                ViewState["PayTerm"] = rbtnPayterm.SelectedValue.ToString();
                //popChangePayTerm.Hide();
                popchange.Show();
                lbltotaladd.Text = "0.00/-";
                hdntotaladdamount.Value = "0";
                ViewState["Total"] = "0"; ;

                int rindex = Convert.ToInt32(ViewState["basicindex"].ToString()); // (((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer))).RowIndex;
                HiddenField hdnBasicPlanName = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanPoid");
                HiddenField hdnBasicDealPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicDealPoid");
                HiddenField hdnBasicPackageId = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPackageId");
                HiddenField hdnBasicActivation = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicActivation");



                lblOldPlan.Text = hdnBasicPlanName.Value;



                //lblChangePlan.Text = "";
                //lblChangePlanMRP.Text = "";
                //lblChangeplanLCO.Text = "";
                HiddenField hdnBasicExpiry = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicExpiry");
                ViewState["expdt"] = hdnBasicExpiry.Value;
                ViewState["Planid"] = hdnBasicPlanPoid.Value;
                ViewState["Packageid"] = hdnBasicPackageId.Value;
                ViewState["Dealid"] = hdnBasicDealPoid.Value;
                ViewState["ActDate"] = hdnBasicActivation.Value;
                ddlChangeplan("B", ViewState["PayTerm"].ToString());
                StatbleDynamicTabs();

            }
            catch (Exception ex)
            {
                //FileLogTextChange("admin", "Payterm", ex.Message, "");
            }
        }

        protected void btnOpenRenewNowPopup_Click(object sender, EventArgs e) // created  by vivek 13-nov-2015
        {
            try
            {
                // lnkatag_Click(null, null);
                rbtnRenewNow.ClearSelection();
                rbtnRenewNow.Items[0].Selected = true;
                PopUpRenewNow.Show();
                if (((DataTable)Session["basic"]).Rows.Count > 0)
                {
                    btnRenewNow.Visible = true;
                    btnCancelRenew.Visible = true;

                }
                grdRenewNow.DataSource = (DataTable)Session["basic"];
                grdRenewNow.DataBind();
                StatbleDynamicTabs();
            }
            catch (Exception ex)
            {
                //FileLogTextChange("admin", "RenewNowOpen", ex.Message, "");
            }
        }

        protected void rbtnRenewNow_SelectedIndexChanged(object sender, EventArgs e) // created  by vivek 13-nov-2015
        {
            try
            {
                //lnkatag_Click(null, null);
                PopUpRenewNow.Show();
                if (rbtnRenewNow.SelectedValue.ToString() == "B")
                {
                    //DataTable dt = grdBasicPlanDetails.DataSource as DataTable;
                    if (((DataTable)Session["basic"]).Rows.Count > 0)
                    {
                        btnRenewNow.Visible = true;
                        btnCancelRenew.Visible = true;
                    }
                    grdRenewNow.DataSource = (DataTable)Session["basic"];
                    grdRenewNow.DataBind();
                }
                else if (rbtnRenewNow.SelectedValue.ToString() == "AD")
                {
                    if (((DataTable)Session["addon"]).Rows.Count > 0)
                    {
                        btnRenewNow.Visible = true;
                        btnCancelRenew.Visible = true;
                    }
                    grdRenewNow.DataSource = (DataTable)Session["addon"];
                    grdRenewNow.DataBind();
                }
                else if (rbtnRenewNow.SelectedValue.ToString() == "AL")
                {
                    if (((DataTable)Session["alacarte"]).Rows.Count > 0)
                    {
                        btnRenewNow.Visible = true;
                        btnCancelRenew.Visible = true;
                    }
                    grdRenewNow.DataSource = (DataTable)Session["alacarte"];
                    grdRenewNow.DataBind();
                }
                else if (rbtnRenewNow.SelectedValue.ToString() == "HSP")
                {
                    if (((DataTable)Session["hathspecial"]).Rows.Count > 0)
                    {
                        btnRenewNow.Visible = true;
                        btnCancelRenew.Visible = true;
                    }
                    grdRenewNow.DataSource = (DataTable)Session["hathspecial"];
                    grdRenewNow.DataBind();
                }
                StatbleDynamicTabs();
            }
            catch (Exception ex)
            {
                //FileLogTextChange("admin", "RenewNow", ex.Message, "");
            }
        }

        protected void btnRenewNow_Click(object sender, EventArgs e) // created  by vivek 13-nov-2015
        {
            //lnkatag_Click(null, null);

            string message = "Are you sure you want to renew the selected plans ";
            lblRenewNowmsg.Text = message;
            // PopUpRenewNow.Hide();
            PopUpRenewNowConfirm.Show();
            StatbleDynamicTabs();
        }

        protected void btnRenewNowConfirm_Click(object sender, EventArgs e)
        {
            // lnkatag_Click(null, null);
            StatbleDynamicTabs();
        }

        protected void grdBasicPlanDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string DelhiCities = "";
            try
            {
                DelhiCities = ConfigurationSettings.AppSettings["DelhiCities"].ToString().Trim();
            }
            catch
            { }
            string expirydt = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                expirydt = DataBinder.Eval(e.Row.DataItem, "EXPIRY").ToString();

                Button lbBRenew = e.Row.FindControl("lnkBRenew") as Button;
                Button lbBChange = e.Row.FindControl("lnkBChange") as Button;
                Button lbBCancel = e.Row.FindControl("lnkBCancel") as Button;
                Button btnDiscnt = e.Row.FindControl("btnDiscnt") as Button;
                CheckBox cbBAutorenew = e.Row.FindControl("cbBAutorenew") as CheckBox;
                Button lnkAddFOCPack = e.Row.FindControl("lnkAddFOCPack") as Button;
                Button btnALPack = e.Row.FindControl("btnALPack") as Button;
                CheckBox cbBasicrenew = e.Row.FindControl("cbBasicrenew") as CheckBox;

                lnkAddFOCPack.Visible = false;
                btnALPack.Visible = false;

                if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30" || ViewState["stb_status"].ToString() == "10102")
                {
                    lbBRenew.Visible = false;
                    lbBChange.Visible = false;
                    lbBCancel.Visible = false;
                    cbBAutorenew.Enabled = false;
                    lnkAddFOCPack.Visible = false;
                    cbBasicrenew.Enabled = false;
                    if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30")
                    {
                        e.Row.Cells[5].Text = "NA";
                        e.Row.Cells[10].Visible = false;
                        grdBasicPlanDetails.HeaderRow.Cells[10].Visible = false;
                    }
                    if (ViewState["stb_status"].ToString() == "10102")
                    {
                        //  e.Row.Cells[7].Text = "Any Action Not Allow";
                        e.Row.Cells[5].Visible = false;
                        grdBasicPlanDetails.HeaderRow.Cells[10].Visible = false;
                    }

                }

                if (DelhiCities != "")
                {
                    String[] DelhiCitiesarr = DelhiCities.Split(',');
                    foreach (string str in DelhiCitiesarr)
                    {
                        if (str != "")
                        {
                            if (str == ViewState["cityid"].ToString())
                            {
                                lbBRenew.Visible = false;
                                lbBChange.Visible = false;
                                lbBCancel.Visible = false;
                                btnDiscnt.Visible = false;
                                cbBAutorenew.Enabled = false;
                                cbBasicrenew.Enabled = false;
                            }
                        }
                    }
                }
                HiddenField hdnbasicalacartebase = e.Row.FindControl("hdnbasicalacartebase") as HiddenField;
                if (hdnbasicalacartebase.Value == "Y")
                {
                    lnkAddFOCPack.Visible = false;
                    //btnALPack.Visible = true;
                    btnALPack.Visible = false;
                }
                else
                {
                    //lnkAddFOCPack.Visible = true;
                    lnkAddFOCPack.Visible = false;
                    btnALPack.Visible = false;
                }


                //if (ViewState["stb_status"] != null && ViewState["stb_status"] == "")
                //{
                //    if (ViewState["stb_status"] == "")
                //    {

                //        LinkButton lbBRenew = e.Row.FindControl("lnkBRenew") as LinkButton;
                //        LinkButton lbBChange = e.Row.FindControl("lnkBChange") as LinkButton;
                //        LinkButton lbBCancel = e.Row.FindControl("lnkBCancel") as LinkButton;
                //        CheckBox cbBAutorenew = e.Row.FindControl("cbBAutorenew") as CheckBox;
                //        LinkButton lnkAddFOCPack = e.Row.FindControl("lnkAddFOCPack") as LinkButton;

                //        lbBRenew.Visible = false;
                //        lbBChange.Visible = false;
                //        lbBCancel.Visible = false;
                //        cbBAutorenew.Enabled = false;
                //        lnkAddFOCPack.Visible = false;

                //    }


                //}

            }
        }

        protected void Grdhathwayspecial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string expirydt = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                expirydt = DataBinder.Eval(e.Row.DataItem, "EXPIRY").ToString();

                if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30" || ViewState["stb_status"].ToString() == "10102")
                {
                    Button lbADDRenewal = e.Row.FindControl("lnkADRenew") as Button;
                    Button lbADDCancel = e.Row.FindControl("lnkADCancel") as Button;
                    Button lbADDChange = e.Row.FindControl("lnkADChange") as Button;
                    CheckBox cbAddonAutorenew = e.Row.FindControl("cbAddonAutorenew") as CheckBox;
                    CheckBox cbAddonrenew = e.Row.FindControl("cbAddonrenew") as CheckBox;
                    lbADDRenewal.Visible = false;
                    lbADDCancel.Visible = false;
                    if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30")
                    {
                        lbADDChange.Visible = true;
                        e.Row.Cells[4].Text = "NA";
                        e.Row.Cells[9].Visible = false;
                        grdAddOnPlan.HeaderRow.Cells[9].Visible = false;
                    }
                    if (ViewState["stb_status"].ToString() == "10102")
                    {
                        lbADDChange.Visible = false;
                        cbAddonAutorenew.Enabled = false;
                        cbAddonrenew.Enabled = false;

                        e.Row.Cells[9].Visible = false;
                        grdAddOnPlan.HeaderRow.Cells[9].Visible = false;
                    }




                }
            }
        }

        protected void grdAddOnPlan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string expirydt = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                expirydt = DataBinder.Eval(e.Row.DataItem, "EXPIRY").ToString();

                if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30" || ViewState["stb_status"].ToString() == "10102")
                {
                    Button lbADDRenewal = e.Row.FindControl("lnkADRenew") as Button;
                    Button lbADDCancel = e.Row.FindControl("lnkADCancel") as Button;
                    Button lbADDChange = e.Row.FindControl("lnkADChange") as Button;
                    CheckBox cbAddonAutorenew = e.Row.FindControl("cbAddonAutorenew") as CheckBox;
                    CheckBox cbAddonrenew = e.Row.FindControl("cbAddonrenew") as CheckBox;
                    lbADDRenewal.Visible = false;
                    lbADDCancel.Visible = false;
                    if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30")
                    {
                        lbADDChange.Visible = true;
                        e.Row.Cells[4].Text = "NA";
                        e.Row.Cells[9].Visible = false;
                        grdAddOnPlan.HeaderRow.Cells[9].Visible = false;
                    }
                    if (ViewState["stb_status"].ToString() == "10102")
                    {
                        lbADDChange.Visible = false;
                        cbAddonAutorenew.Enabled = false;
                        cbAddonrenew.Enabled = false;

                        e.Row.Cells[9].Visible = false;
                        grdAddOnPlan.HeaderRow.Cells[9].Visible = false;
                    }

                }
            }
        }

        protected void grdAddOnPlanReg_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string expirydt = "";
            string planname = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                expirydt = DataBinder.Eval(e.Row.DataItem, "EXPIRY").ToString();
                planname = e.Row.Cells[0].Text.Trim();
                Button lbADDRenewal = e.Row.FindControl("lnkADRenew") as Button;
                Button lbADDCancel = e.Row.FindControl("lnkADCancel") as Button;
                Button lbADDChange = e.Row.FindControl("lnkADChange") as Button;
                if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30" || ViewState["stb_status"].ToString() == "10102")
                {

                    CheckBox cbAddonAutorenew = e.Row.FindControl("cbAddonAutorenew") as CheckBox;
                    CheckBox cbAddonrenew = e.Row.FindControl("cbAddonrenew") as CheckBox;
                    lbADDRenewal.Visible = false;
                    lbADDCancel.Visible = false;
                    if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30")
                    {
                        lbADDChange.Visible = true;
                        e.Row.Cells[4].Text = "NA";
                        e.Row.Cells[9].Visible = false;
                        grdAddOnPlanReg.HeaderRow.Cells[9].Visible = false;
                    }
                    if (ViewState["stb_status"].ToString() == "10102")
                    {
                        lbADDChange.Visible = false;
                        cbAddonAutorenew.Enabled = false;
                        cbAddonrenew.Enabled = false;

                        e.Row.Cells[9].Visible = false;
                        grdAddOnPlanReg.HeaderRow.Cells[9].Visible = false;
                    }

                }
                if (planname.Contains("FREE"))
                {
                    lbADDChange.Visible = false;
                }
            }
        }

        protected void grdCartefree_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string expirydt = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                expirydt = DataBinder.Eval(e.Row.DataItem, "EXPIRY").ToString();

                if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30" || ViewState["stb_status"].ToString() == "10102")
                {
                    Button lbALCancel = e.Row.FindControl("lnkALCancel") as Button;
                    CheckBox cbAlaAutorenew = e.Row.FindControl("cbAlaAutorenew") as CheckBox;
                    CheckBox chkalRenew = e.Row.FindControl("chkalRenew") as CheckBox;

                    lbALCancel.Visible = false;

                    if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30")
                    {
                        e.Row.Cells[4].Text = "NA";
                        e.Row.Cells[9].Visible = false;
                        grdCarte.HeaderRow.Cells[9].Visible = false;
                    }
                    if (ViewState["stb_status"].ToString() == "10102")
                    {
                        cbAlaAutorenew.Enabled = false;
                        chkalRenew.Enabled = false;
                        //e.Row.Cells[7].Text = "Any Action Not Allow";
                        e.Row.Cells[9].Visible = false;
                        grdCarte.HeaderRow.Cells[9].Visible = false;
                    }
                }
            }

        }

        protected void grdCarte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string expirydt = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                expirydt = DataBinder.Eval(e.Row.DataItem, "EXPIRY").ToString();

                if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30" || ViewState["stb_status"].ToString() == "10102")
                {
                    Button lbALRenewal = e.Row.FindControl("lnkALRenew") as Button;
                    Button lbALCancel = e.Row.FindControl("lnkALCancel") as Button;
                    Button lbALChange = e.Row.FindControl("lnkALChange") as Button;
                    CheckBox cbAlaAutorenew = e.Row.FindControl("cbAlaAutorenew") as CheckBox;
                    CheckBox chkalRenew = e.Row.FindControl("chkalRenew") as CheckBox;
                    lbALRenewal.Visible = false;
                    lbALCancel.Visible = false;

                    if (expirydt == "31-DEC-69" && expirydt == "30-DEC-30")
                    {
                        lbALChange.Visible = false;
                        e.Row.Cells[4].Text = "NA";
                        e.Row.Cells[9].Visible = false;
                        grdCarte.HeaderRow.Cells[9].Visible = false;
                    }
                    if (ViewState["stb_status"].ToString() == "10102")
                    {
                        lbALChange.Visible = false;
                        cbAlaAutorenew.Enabled = false;
                        chkalRenew.Enabled = false;
                        //e.Row.Cells[7].Text = "Any Action Not Allow";
                        e.Row.Cells[9].Visible = false;
                        grdCarte.HeaderRow.Cells[9].Visible = false;


                    }






                }
            }







        }

        //protected void Lkretct_Click(object sender, EventArgs e)
        //{

        //    string service_popmsg1 = "";
        //    string service_popmsg2 = "";
        //    string service_op = "Retract";
        //    service_popmsg1 = "This will Retract the service.";
        //    service_popmsg2 = "Are you sure you want to Retract?";

        //    setRetractServicePopup(service_popmsg1, service_popmsg2, service_op.Trim());
        //    popretrctservice.Show();







        //}

        //set service confirmation popup

        protected void setRetractServicePopup(string message1, string message2, string flag)
        {

            try
            {
                Lblmsg1.Text = message1;
                Lblmsg2.Text = message2;
                hdnPopupServiceFlag.Value = flag;

            }
            catch (Exception ex)
            {

                //FileLogText("Admin", "setRetractServicePopup", " Error:" + ex.Message.Trim(), "");
            }
        }

        protected void btnpopretracservice_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);


            string username = "";
            string user_brmpoid = "";
            string stb_no = "";
            string vc_id = "";
            string selected_service_str = "";
            string Request = "";
            string CheckStatus = "";

            string req_code = "";

            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
            stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
            vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
            selected_service_str = myResultSet.Rows[0]["SERVICE_STRING"].ToString();



            var str = ":";
            if (vc_id.Any(str.Contains))
            {

                CheckStatus = "MAC";
            }
            else
            {
                CheckStatus = "VC";
            }




            if (Session["username"] != null && Session["user_brmpoid"] != null)
            {
                if (catid == "11")
                {
                    username = Convert.ToString(Session["lco_username"]);
                }
                else
                {
                    username = Convert.ToString(Session["username"]);
                }
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            req_code = "15";


            if (ViewState["accountPoid"] == null && ViewState["ServicePoid"] == null)
            {
                msgboxstr("Failed to get Account POID or Service POID, Please relogin and try again.");
                return;
            }
            if (Session["MANUFACTURER"].ToString() != "VM")
            {
                if (CheckStatus.ToString() == "VC")
                {
                    Request = user_brmpoid + "$" + ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + username + "$" + vc_id + "$" + "VC";//stb_no;Session["MANUFACTURER"]
                }
                else if (CheckStatus.ToString() == "MAC")
                {
                    Request = user_brmpoid + "$" + ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + username + "$" + vc_id + "$" + "MAC";//stb_no;
                }
            }
            else
            {
                Request = user_brmpoid + "$" + ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + username + "$" + vc_id + "$" + "VM";//stb_no;
            }

            /*if (CheckStatus.ToString() == "VC")
            {
                Request = user_brmpoid + "$" + ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + username + "$" + vc_id + "$" + "VC";//stb_no;
            }
            else if (CheckStatus.ToString() == "MAC")
            {
                Request = user_brmpoid + "$" + ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + username + "$" + vc_id + "$" + "MAC";//stb_no;
            }*/
            try
            {
                //activation and deactivation
                string api_response = callAPI(Request, req_code);
                //registering in db
                string[] final_obrm_status = api_response.Split('$');
                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";
                string obrm_orderid = "";
                try
                {
                    if (obrm_status == "0" || obrm_status == "1")
                    {
                        if (obrm_status == "0")
                        {
                            obrm_msg = "Service retrack successfully, order id is : " + final_obrm_status[2];
                            obrm_orderid = final_obrm_status[2];
                        }

                        else if (obrm_status == "1")
                        {
                            obrm_msg = "Service retrack failed on account : " + final_obrm_status[2];
                        }


                    }
                    msgboxstr_refresh(obrm_msg);
                    StatbleDynamicTabs();

                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    obrm_msg = api_response;
                    msgboxstr_refresh(obrm_msg);
                }



            }
            catch (Exception ex)
            {

            }

        }

        protected void lnkaddplanbaseexpired_Click(object sender, EventArgs e)
        {
            TableRow grdAddOnPlan = ((System.Web.UI.WebControls.TableRow)((GridViewRow)(((Button)(sender)).Parent.BindingContainer)));
            HiddenField hdnBasicPlanPoid = (HiddenField)grdAddOnPlan.FindControl("hdnBasicPlanPoid");

            AddExpiredplan.Visible = false;
            lbltotaladd.Text = "0.00/-";
            ViewState["Addplanpoids"] = "";
            ViewState["Total"] = "0";
            Label7.Visible = false;
            string str = "";
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string basic_poids = "'0'";
            //if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            //{
            basic_poids = hdnBasicPlanPoid.Value;
            //}
            try
            {
                /*if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                  " a.cityid, a.company_code, a.insby, a.insdt,a.alacartebase  FROM view_lcopre_lcoplan_fch_JVbasi a " +
                                  " where a.cityid ='" + city + "' and a.plan_type='B' and a.DASAREA ='" + Session["dasarea"].ToString() + "'  and lcocode=" + Session["lcoid"].ToString() + "" +
                                  " and a.plan_poid  in (" + basic_poids + ") " +
                                  " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                  " a.cityid, a.company_code, a.insby, a.insdt,a.alacartebase  FROM view_lcopre_plan_fetch_JVbasic a " +
                                  " where a.cityid ='" + city + "' and a.plan_type='B' and a.DASAREA ='" + Session["dasarea"].ToString() + "'" +
                                  " and a.plan_poid  in (" + basic_poids + ") " +
                                  " and not EXISTS (select * from  view_lcopre_lcoplan_fch_JVbasi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") ";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt,a.alacartebase  FROM view_lcopre_lcoplan_fetch_basi a " +
                        " where a.cityid ='" + city + "' and a.plan_type='B' and a.DASAREA ='" + Session["dasarea"].ToString() + "'  and lcocode=" + Session["lcoid"].ToString() + "" +
                        " and a.plan_poid  in (" + basic_poids + ") " +
                        " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt,a.alacartebase  FROM view_lcopre_plan_fetch_basic a " +
                        " where a.cityid ='" + city + "' and a.plan_type='B' and a.DASAREA ='" + Session["dasarea"].ToString() + "'" +
                        " and a.plan_poid  in (" + basic_poids + ") " +
                        " and not EXISTS (select * from  view_lcopre_lcoplan_fetch_basi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") ";
                }*/
                if (ViewState["JVFlag"].ToString() == "Y")
                {

                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                   " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_lcoplan_fch_JVbasi a " +
                                   " where a.cityid =" + city + " and a.plan_type='B'" +
                                   " and a.plan_poid  in (" + basic_poids + ")  and lcocode=" + Session["lcoid"].ToString() + " and jvno='" + Convert.ToString(Session["JVNO"]) + "' and  a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                   " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                   " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_plan_fetch_JVbasic a " +
                                   " where a.cityid =" + city + " and a.plan_type='B'" +
                                   " and a.plan_poid  in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "' and jvno='" + Convert.ToString(Session["JVNO"]) + "'" +
                                   " and not EXISTS (select * from  view_lcopre_lcoplan_fch_JVbasi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")  " +
                                   " order by plan_name asc";

                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                               " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_lcoplan_fetch_basi a " +
                               " where a.cityid =" + city + " and a.plan_type='B'" +
                               " and a.plan_poid  in (" + basic_poids + ")  and lcocode=" + Session["lcoid"].ToString() + " and  a.dasarea='" + Session["dasarea"].ToString() + "'" +
                               " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                               " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_plan_fetch_basic a " +
                               " where a.cityid =" + city + " and a.plan_type='B'" +
                        " and a.plan_poid  in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        " and not EXISTS (select * from  view_lcopre_lcoplan_fetch_basi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")  " +
                        " order by plan_name asc";
                }

                DataTable TblPlans = GetResult(str);
                if (TblPlans.Rows.Count > 0)
                {
                    grdPlanChan.DataSource = TblPlans;
                    grdPlanChan.DataBind();
                    ViewState["TblPlans"] = TblPlans;
                    if (grdPlanChan.Rows.Count > 0)
                    {
                        for (int i = 0; i < grdPlanChan.Rows.Count; i++)
                        {
                            ((CheckBox)(grdPlanChan.Rows[i].FindControl("ChkPlanAdd"))).Checked = true;
                            ((CheckBox)(grdPlanChan.Rows[i].FindControl("ChkPlanAdd"))).Enabled = false;

                            lbltotaladd.Text = grdPlanChan.Rows[i].Cells[2].Text + "/-";
                            Int32 SDCount = Convert.ToInt32(grdPlanChan.Rows[i].Cells[5].Text);
                            Int32 HDCount = Convert.ToInt32(grdPlanChan.Rows[i].Cells[6].Text);
                            lblChannelcount.Text = Convert.ToString(SDCount + HDCount);
                        }
                    }
                }
                else
                {
                    grdPlanChan.DataSource = null;
                    grdPlanChan.DataBind();
                }

                if (grdPlanChan.Rows.Count > 0)
                {
                    Label7.Visible = false;
                    btnAddPlan.Visible = true;
                }
                else
                {
                    Label7.Visible = true;
                    btnAddPlan.Visible = false;
                }
                StatbleDynamicTabs();
            }
            catch (Exception ex)
            {
                Label7.Visible = true;
            }
            popAdd.Show();
        }

        protected void lnkHSPaddplanbaseexpired_Click(object sender, EventArgs e)
        {
            AddExpiredplan.Visible = false;
            TableRow grdAddOnPlan = ((System.Web.UI.WebControls.TableRow)((GridViewRow)(((Button)(sender)).Parent.BindingContainer)));
            HiddenField hdnADPlanPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanPoid");
            lbltotaladd.Text = "0.00/-";
            ViewState["Addplanpoids"] = "";
            ViewState["Total"] = "0";
            Label7.Visible = false;
            string str = "";
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string hwayspecial_poid = "'0'";
            //if (ViewState["hwayspecial_poid"] != null && ViewState["hwayspecial_poid"].ToString() != "")
            //{
            hwayspecial_poid = hdnADPlanPoid.Value;
            //}
            try
            {
                /*if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                  " a.cityid, a.company_code, a.insby, a.insdt,a.alacartebase  FROM view_lcopre_lcoplan_fch_JVbasi a " +
                                  " where a.cityid ='" + city + "' and a.plan_type='B' and a.DASAREA ='" + Session["dasarea"].ToString() + "'  and lcocode=" + Session["lcoid"].ToString() + "" +
                                  " and a.plan_poid  in (" + basic_poids + ") " +
                                  " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                  " a.cityid, a.company_code, a.insby, a.insdt,a.alacartebase  FROM view_lcopre_plan_fetch_JVbasic a " +
                                  " where a.cityid ='" + city + "' and a.plan_type='B' and a.DASAREA ='" + Session["dasarea"].ToString() + "'" +
                                  " and a.plan_poid  in (" + basic_poids + ") " +
                                  " and not EXISTS (select * from  view_lcopre_lcoplan_fch_JVbasi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") ";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt,a.alacartebase  FROM view_lcopre_lcoplan_fetch_basi a " +
                        " where a.cityid ='" + city + "' and a.plan_type='B' and a.DASAREA ='" + Session["dasarea"].ToString() + "'  and lcocode=" + Session["lcoid"].ToString() + "" +
                        " and a.plan_poid  in (" + basic_poids + ") " +
                        " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                        " a.cityid, a.company_code, a.insby, a.insdt,a.alacartebase  FROM view_lcopre_plan_fetch_basic a " +
                        " where a.cityid ='" + city + "' and a.plan_type='B' and a.DASAREA ='" + Session["dasarea"].ToString() + "'" +
                        " and a.plan_poid  in (" + basic_poids + ") " +
                        " and not EXISTS (select * from  view_lcopre_lcoplan_fetch_basi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") ";
                }*/
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,bc_price, broad_name, genre_type,var_plan_freeflag from(";
                    str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOJVPLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "' ";
                    str += " and a.plan_poid in (" + hwayspecial_poid + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    str += " and lcocode=" + Session["lcoid"].ToString();
                    str += " and jvno='" + Convert.ToString(Session["JVNO"]) + "'";// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'
                    str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_JVPLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "'  ";
                    str += " and a.plan_poid in (" + hwayspecial_poid + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    str += " and jvno='" + Convert.ToString(Session["JVNO"]) + "'";// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'
                    str += " and not EXISTS (select * from VIEW_LCOJVPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                    str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";

                }
                else
                {

                    str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,bc_price, broad_name, genre_type,var_plan_freeflag from(";
                    str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOPLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "' ";
                    str += " and a.plan_poid  in (" + hwayspecial_poid + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    str += " and lcocode=" + Session["lcoid"].ToString();
                    str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,bc_price, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_PLAN_FETCH_ALL a ";
                    str += " where a.cityid ='" + city + "' ";
                    str += " and a.plan_poid  in (" + hwayspecial_poid + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    str += " and not EXISTS (select * from VIEW_LCOPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                    str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";

                }

                DataTable TblPlans = GetResult(str);
                if (TblPlans.Rows.Count > 0)
                {
                    grdPlanChan.DataSource = TblPlans;
                    grdPlanChan.DataBind();
                    ViewState["TblPlans"] = TblPlans;
                    if (grdPlanChan.Rows.Count > 0)
                    {
                        for (int i = 0; i < grdPlanChan.Rows.Count; i++)
                        {
                            ((CheckBox)(grdPlanChan.Rows[i].FindControl("ChkPlanAdd"))).Checked = true;
                            ((CheckBox)(grdPlanChan.Rows[i].FindControl("ChkPlanAdd"))).Enabled = false;

                            lbltotaladd.Text = grdPlanChan.Rows[i].Cells[2].Text + "/-";
                            Int32 SDCount = Convert.ToInt32(grdPlanChan.Rows[i].Cells[5].Text);
                            Int32 HDCount = Convert.ToInt32(grdPlanChan.Rows[i].Cells[6].Text);
                            lblChannelcount.Text = Convert.ToString(SDCount + HDCount);
                        }
                    }
                }
                else
                {
                    grdPlanChan.DataSource = null;
                    grdPlanChan.DataBind();
                }

                if (grdPlanChan.Rows.Count > 0)
                {
                    Label7.Visible = false;
                    btnAddPlan.Visible = true;
                }
                else
                {
                    Label7.Visible = true;
                    btnAddPlan.Visible = false;
                }
                StatbleDynamicTabs();
            }
            catch (Exception ex)
            {
                Label7.Visible = true;
            }
            popAdd.Show();
        }

        protected void lnkADaddplanbaseexpired_Click(object sender, EventArgs e)
        {
            AddExpiredplan.Visible = false;
            TableRow grdAddOnPlan = ((System.Web.UI.WebControls.TableRow)((GridViewRow)(((Button)(sender)).Parent.BindingContainer)));
            HiddenField hdnADPlanPoid = (HiddenField)grdAddOnPlan.FindControl("hdnADPlanPoid");
            lbltotaladd.Text = "0.00/-";
            ViewState["Total"] = "0";
            Label7.Visible = false;
            string str = "";
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string addon_poids = "'0'";
            //if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
            //{
            addon_poids = hdnADPlanPoid.Value;
            //}
            try
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_lcojvplan_fetch a ";
                    str += "  where a.cityid ='" + city + "' and a.PLAN_TYPE='AD' ";
                    str += " and a.plan_poid in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "' and jvno='" + Convert.ToString(Session["JVNO"]) + "'";
                    str += "  and lcocode=" + Session["lcoid"].ToString();
                    str += "  union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, ";
                    str += "  a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_jvplan_fetch a ";
                    str += "  where a.cityid ='" + city + "' and a.PLAN_TYPE='AD' ";
                    str += "  and a.plan_poid  in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "' and jvno='" + Convert.ToString(Session["JVNO"]) + "'";
                    str += "  and not EXISTS (select * from  view_lcopre_lcojvplan_fetch where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                    str += "  order by plan_name asc";

                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, ";
                    str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_lcoplan_fetch a ";
                    str += "  where a.cityid ='" + city + "' and a.PLAN_TYPE='AD' ";
                    str += " and a.plan_poid  in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    str += "  and lcocode=" + Session["lcoid"].ToString();
                    str += "  union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, ";
                    str += "  a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_plan_fetch a ";
                    str += "  where a.cityid ='" + city + "' and a.PLAN_TYPE='AD' ";
                    str += "  and a.plan_poid  in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                    str += "  and not EXISTS (select * from  view_lcopre_lcoplan_fetch where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                    str += "  order by plan_name asc";

                }

                DataTable TblPlans = GetResult(str);
                if (TblPlans.Rows.Count > 0)
                {
                    grdPlanChan.DataSource = TblPlans;
                    grdPlanChan.DataBind();
                    ViewState["TblPlans"] = TblPlans;
                    if (grdPlanChan.Rows.Count > 0)
                    {
                        for (int i = 0; i < grdPlanChan.Rows.Count; i++)
                        {
                            ((CheckBox)(grdPlanChan.Rows[i].FindControl("ChkPlanAdd"))).Checked = true;
                            ((CheckBox)(grdPlanChan.Rows[i].FindControl("ChkPlanAdd"))).Enabled = false;

                            lbltotaladd.Text = grdPlanChan.Rows[i].Cells[2].Text + "/-";
                            Int32 SDCount = Convert.ToInt32(grdPlanChan.Rows[i].Cells[5].Text);
                            Int32 HDCount = Convert.ToInt32(grdPlanChan.Rows[i].Cells[6].Text);
                            lblChannelcount.Text = Convert.ToString(SDCount + HDCount);
                        }
                    }
                }
                else
                {
                    grdPlanChan.DataSource = null;
                    grdPlanChan.DataBind();
                }

                if (grdPlanChan.Rows.Count > 0)
                {
                    Label7.Visible = false;
                    btnAddPlan.Visible = true;
                }
                else
                {
                    Label7.Visible = true;
                    btnAddPlan.Visible = false;
                }
                StatbleDynamicTabs();
            }
            catch (Exception ex)
            {
                Label7.Visible = true;
            }
            popAdd.Show();
        }

        protected void lnkALaddplanbaseexpired_Click(object sender, EventArgs e)
        {
            AddExpiredplan.Visible = false;
            TableRow grdAddOnPlan = ((System.Web.UI.WebControls.TableRow)((GridViewRow)(((Button)(sender)).Parent.BindingContainer)));
            HiddenField hdnALPlanPoid = (HiddenField)grdAddOnPlan.FindControl("hdnALPlanPoid");
            lbltotaladd.Text = "0.00/-";
            ViewState["Addplanpoids"] = "";
            ViewState["Total"] = "0";
            Label7.Visible = false;
            string str = "";
            string city = "";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string ala_poids = "'0'";
            //if (ViewState["ala_poids"] != null && ViewState["ala_poids"].ToString() != "")
            //{
            ala_poids = hdnALPlanPoid.Value;
            //}
            try
            {
                if (ViewState["JVFlag"].ToString() == "Y")
                {
                    str = " SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.BROAD_NAME, a.GENRE_TYPE,a.var_plan_freeflag " +
                                          " FROM view_lcopre_lcoplan_jvfetchnew a " +
                                          " where a.plan_type='AL' and a.cityid ='" + city + "' " +
                                          " and a.plan_poid  in (" + ala_poids + ") " +
                                          " and jvno='" + Convert.ToString(Session["JVNO"]) + "'  and a.dasarea='" + Session["dasarea"].ToString() + "' and lcocode=" + Session["lcoid"].ToString() +// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'
                                          " union select * from ( (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.BROAD_NAME, a.GENRE_TYPE,a.var_plan_freeflag " +
                                          " FROM view_lcopre_plan_jvfetchnew a " +
                                          " where a.plan_type='AL' and a.cityid ='" + city + "' " +
                                          " and a.plan_poid  in (" + ala_poids + ") " +
                                          " and jvno='" + Convert.ToString(Session["JVNO"]) + "'  and a.dasarea='" + Session["dasarea"].ToString() + "'" +// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'
                                          " and not EXISTS (select * from  view_lcopre_lcoplan_jvfetchnew where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") " +
                                          " ) minus  " +
                                          " ( " +
                                          " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice,a.var_plan_productpoid product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.num_plan_broadprice, a.var_plan_broad_name, a.var_plan_genre_type,a.var_plan_freeflag" +
                                          " FROM aoup_lcopre_jvplan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_jvplan_def c " +
                                          "  where a.var_plan_name=b.var_plan_name " +
                                          " and c.var_plan_proviid=b.var_plan_provi " +
                                          "  AND b.var_plan_city=a.num_plan_cityid " +
                                          " and a.var_plan_plantype in ('AD','B') " +
                                          " and c.var_plan_plantype='AL' " +
                                          " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                                          " and c.num_plan_cityid='" + city + "' " +
                                          " and not EXISTS (select * from  aoup_lcopre_lcojvplan_def where var_plan_name=c.var_plan_name and  var_plan_planpoid=c.var_plan_planpoid and var_plan_lcocode=" + Session["lcoid"].ToString() + ")" +
                                          " ) ) order by plan_name asc";
                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.BROAD_NAME, a.GENRE_TYPE,a.var_plan_freeflag " +
                                      " FROM view_lcopre_lcoplan_fetchnew a " +
                                     " where a.plan_type='AL' and a.cityid ='" + city + "' " +
                                     " and a.plan_poid in (" + ala_poids + ") " +
                                      " and a.dasarea='" + Session["dasarea"].ToString() + "' and lcocode=" + Session["lcoid"].ToString() +// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "' 
                                   " union select * from ( (SELECT a.plan_id, a.plan_name, a.cust_price, a.plan_poid, a.plan_type, a.deal_poid, a.lco_price,a.product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.BROAD_NAME, a.GENRE_TYPE,a.var_plan_freeflag " +
                                      " FROM view_lcopre_plan_fetchnew a " +
                                     " where a.plan_type='AL' and a.cityid ='" + city + "' " +
                                     " and a.plan_poid  in (" + ala_poids + ") " +
                                      " and a.dasarea='" + Session["dasarea"].ToString() + "'" +// and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "' 
                                      " and not EXISTS (select * from  view_lcopre_lcoplan_fetchnew where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ") " +
                                      " ) minus  " +
                                      " ( " +
                                      " SELECT c.num_plan_id,c.var_plan_name,c.num_plan_custprice,c.var_plan_planpoid,c.var_plan_plantype,c.var_plan_dealpoid,c.num_plan_lcoprice,a.var_plan_productpoid product_poid,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.num_plan_broadprice, a.var_plan_broad_name, a.var_plan_genre_type,a.var_plan_freeflag" +
                                      " FROM aoup_lcopre_plan_def a,aoup_lcopre_plan_channel b ,aoup_lcopre_plan_def c " +
                                      "  where a.var_plan_name=b.var_plan_name " +
                                      " and c.var_plan_proviid=b.var_plan_provi " +
                                      "  AND b.var_plan_city=a.num_plan_cityid " +
                                      " and a.var_plan_plantype in ('AD','B') " +
                                      " and c.var_plan_plantype='AL' " +
                                      " and a.var_plan_planpoid in(" + ala_poids + ")  and a.var_plan_dasarea='" + Session["dasarea"].ToString() + "'" +
                                      " and c.num_plan_cityid='" + city + "' " +
                                      " and not EXISTS (select * from  aoup_lcopre_lcoplan_def where var_plan_name=c.var_plan_name and  var_plan_planpoid=c.var_plan_planpoid and var_plan_lcocode=" + Session["lcoid"].ToString() + ")" +
                                      " ) ) order by plan_name asc";

                }

                DataTable TblPlans = GetResult(str);
                if (TblPlans.Rows.Count > 0)
                {
                    grdPlanChan.DataSource = TblPlans;
                    grdPlanChan.DataBind();
                    ViewState["TblPlans"] = TblPlans;
                    if (grdPlanChan.Rows.Count > 0)
                    {
                        for (int i = 0; i < grdPlanChan.Rows.Count; i++)
                        {
                            ((CheckBox)(grdPlanChan.Rows[i].FindControl("ChkPlanAdd"))).Checked = true;
                            ((CheckBox)(grdPlanChan.Rows[i].FindControl("ChkPlanAdd"))).Enabled = false;

                            lbltotaladd.Text = grdPlanChan.Rows[i].Cells[2].Text + "/-";
                            Int32 SDCount = Convert.ToInt32(grdPlanChan.Rows[i].Cells[5].Text);
                            Int32 HDCount = Convert.ToInt32(grdPlanChan.Rows[i].Cells[6].Text);
                            lblChannelcount.Text = Convert.ToString(SDCount + HDCount);
                        }
                    }
                }
                else
                {
                    grdPlanChan.DataSource = null;
                    grdPlanChan.DataBind();
                }

                if (grdPlanChan.Rows.Count > 0)
                {
                    Label7.Visible = false;
                    btnAddPlan.Visible = true;
                }
                else
                {
                    Label7.Visible = true;
                    btnAddPlan.Visible = false;
                }
                StatbleDynamicTabs();
            }
            catch (Exception ex)
            {
                Label7.Visible = true;
            }
            popAdd.Show();
        }


        protected void radPlanBasic_CheckedChanged(object sender, EventArgs e)   //added by vivek 04-Jan-2016
        {
            // lnkatag_Click(null, null);
            //lblplanname.Text = "";
            //lblplanamt.Text = "";
            //trAddplanAutorenew.Visible = false;
            //cbAddPlanAutorenew.Checked = false;
            lbltotaladd.Text = "0.00/-";
            hdntotaladdamount.Value = "0";
            ViewState["Total"] = "0";
            Label7.Visible = false;
            string str = "";
            string city = "";
            pnlBC.Visible = true;
            lblBC.Visible = false;
            ddlBC.Visible = false;
            btnsearchfilter2.Visible = false;
            pnlAL.Visible = false;
            string DeviceDefinitionType = "SD";
            if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
            {
                city = ViewState["cityid"].ToString();
            }
            string basic_poids = "'0'";
            if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
            {
                basic_poids = ViewState["basic_poids"].ToString();
            }
            string addon_poids = "'0'";
            if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
            {
                addon_poids = ViewState["addon_poids"].ToString();
            }
            if (ViewState["DeviceDefinitionType"] != null && ViewState["DeviceDefinitionType"] != "")
            {
                DeviceDefinitionType = ViewState["DeviceDefinitionType"].ToString();
            }
            string whestring = "";
            grdPlanChan.DataSource = null;
            grdPlanChan.DataBind();
            try
            {
                if (ViewState["parentsmsg"].ToString() == "0")
                {
                    whestring = "and  a.plan_name not like '%ADDITIONAL%'";
                }
                else if (ViewState["parentsmsg"].ToString() == "1")
                {
                    whestring = "and  a.plan_name not like '%ADDITIONAL%'";
                }
                whestring += " and a.payterm='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "' ";
                string hd_where_clause = "";
                if (!DeviceDefinitionType.Contains("HD"))
                {
                    hd_where_clause = " and a.device_type <> 'HD' ";
                }
                if (ViewState["JVFlag"].ToString() == "Y")
                {

                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                   " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_lcoplan_fch_JVbasi a " +
                                   " where a.cityid =" + city + " and a.plan_type='B'" +
                                   " and a.plan_poid not in (" + basic_poids + ")  and lcocode=" + Session["lcoid"].ToString() + " and  a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                   whestring + hd_where_clause +
                                   " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                                   " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_plan_fetch_JVbasic a " +
                                   " where a.cityid =" + city + " and a.plan_type='B'" +
                                   " and a.plan_poid not in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                                   whestring + hd_where_clause +
                                   " and not EXISTS (select * from  view_lcopre_lcoplan_fch_JVbasi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")  " +
                                   " order by plan_name asc";

                }
                else
                {
                    str = " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                               " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_lcoplan_fetch_basi a " +
                               " where a.cityid =" + city + " and a.plan_type='B'" +
                        //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                               " and a.plan_poid not in (" + basic_poids + ")  and lcocode=" + Session["lcoid"].ToString() + " and  a.dasarea='" + Session["dasarea"].ToString() + "'" +
                               whestring + hd_where_clause +
                               " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price,  a.lco_price, a.payterm, " +
                               " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,a.num_plan_sd_cnt,a.num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag  FROM view_lcopre_plan_fetch_basic a " +
                               " where a.cityid =" + city + " and a.plan_type='B'" +
                        //" and upper(a.plan_name) like upper('" + prefixText + "%') " +
                        " and a.plan_poid not in (" + basic_poids + ")  and a.dasarea='" + Session["dasarea"].ToString() + "'" +
                        whestring + hd_where_clause +
                        " and not EXISTS (select * from  view_lcopre_lcoplan_fetch_basi where plan_name=a.plan_name and  plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")  " +
                        " order by plan_name asc";
                }

                DataTable TblPlans = GetResult(str);
                if (TblPlans.Rows.Count > 0)
                {
                    grdPlanChan.DataSource = TblPlans;
                    grdPlanChan.DataBind();
                }
                else
                {
                    Label7.Visible = true;
                    grdPlanChan.DataSource = null;
                    grdPlanChan.DataBind();
                }
                // QueryFileLog("Query Log", "Addon Query : ", str, ad_qry_test_plan);
                StatbleDynamicTabs();
            }
            catch (Exception ex)
            {
                Label7.Visible = true;
            }
            popAdd.Show();
        }

        private void createDynamicTabs(DataTable dt)
        {
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                LinkButton link = new LinkButton();
                HtmlGenericControl li = new HtmlGenericControl("li");
                if (i == 1)
                {
                    link.Text = "Main TV";
                }
                else
                {
                    link.Text = "Addon" + (i - 1).ToString();
                }

                link.Attributes.Add("Width", "10px");
                link.Attributes.Add("height", "15px");
                link.OnClientClick = "clickre('lnkAddon" + i.ToString() + "')";
                link.ID = "lnkAddon" + i.ToString();
                li.Controls.Add(link);
                ulFiles.Controls.Add(li);
            }
            if (Challrenew.Checked == true)
            {
                Challrenew.Checked = false;
            }


        }

        protected void lnkatag_Click(object sender, EventArgs e)
        {
            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
            // createDynamicTabs(sortedDT);

            if (hdntag.Value == "lnkAddon1")
            {
                ViewState["parentsmsg"] = "0";
            }
            else
            {
                ViewState["parentsmsg"] = "1";

            }

            for (int i = 1; i <= sortedDT.Rows.Count; i++)
            {

                ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("MasterBody");
                LinkButton lnk = (LinkButton)cph.FindControl("lnkAddon" + i.ToString());
                if (lnk == null)
                {
                    createDynamicTabs(sortedDT);
                    lnk = (LinkButton)cph.FindControl("lnkAddon" + i.ToString());
                }
                if (lnk.Visible)
                {
                    lnk.ForeColor = Color.White;
                    lnk.BackColor = Color.Red;
                    lnk.Style.Add("Height", "12px");
                }


            }
            lnkDetail.ForeColor = Color.White;
            lnkDetail.BackColor = Color.Red;

            ContentPlaceHolder cph1 = (ContentPlaceHolder)this.Master.FindControl("MasterBody");
            LinkButton lnk1 = (LinkButton)cph1.FindControl(hdntag.Value);

            lnk1.ForeColor = Color.Red;
            lnk1.BackColor = Color.White;

            if (hdntag.Value == "lnkDetail")
            {
                planDetail.Visible = false;
                trCustAdd.Visible = true;
                trCustMobileNo.Visible = true;
                tremail.Visible = true;
                trServiceStatus.Visible = false;
                BtnRetract.Visible = false;
                trVC.Visible = false;
                trCustNo.Visible = true;
                trcust1Name.Visible = true;
                trvcdet.Visible = true;
                trModify.Visible = true;
            }
            else
            {
                trcust1Name.Visible = false;
                trCustNo.Visible = false;
                trVC.Visible = true;
                planDetail.Visible = true;
                trCustAdd.Visible = false;
                trCustMobileNo.Visible = false;
                tremail.Visible = false;
                //trServiceStatus.Visible = true;
                trServiceStatus.Visible = false;
                trvcdet.Visible = false;
                BtnRetract.Visible = true;
                trModify.Visible = false;


                getvcValue(hdntag.Value);
            }

        }

        private void getvcValue(string strtab)
        {
            DataTable sortedDT = (DataTable)ViewState["vcdetail"];

            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + strtab + "'").CopyToDataTable();
            string planstring = myResultSet.Rows[0]["SERVICE_STRING"].ToString();

            resetAllGrids();

            dtBasicPlans = new DataTable();
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("DEAL_POID"));
            //dtBasicPlans.Columns.Add(new DataColumn("PRODUCT_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtBasicPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dtBasicPlans.Columns.Add(new DataColumn("HD_Count"));
            dtBasicPlans.Columns.Add(new DataColumn("SD_Count"));
            dtBasicPlans.Columns.Add(new DataColumn("Total_Count"));
            dtBasicPlans.Columns.Add(new DataColumn("BD_PRICE"));
            //---------------------
            //dtBasicPlans.Columns.Add(new DataColumn("PAYTERM"));
            //dtBasicPlans.Columns.Add(new DataColumn("CITYID"));
            //dtBasicPlans.Columns.Add(new DataColumn("CITY_NAME"));
            //dtBasicPlans.Columns.Add(new DataColumn("COMPANY_CODE"));
            //dtBasicPlans.Columns.Add(new DataColumn("INSBY"));
            //dtBasicPlans.Columns.Add(new DataColumn("INSDT"));
            dtBasicPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtBasicPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtBasicPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtBasicPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_STATUS"));

            // created by vivek 16-nov-2015 
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtBasicPlans.Columns.Add(new DataColumn("GRACE"));
            dtBasicPlans.Columns.Add(new DataColumn("DISCOUNT"));
            dtBasicPlans.Columns.Add(new DataColumn("alacartebase"));
            dtBasicPlans.Columns.Add(new DataColumn("pincycle"));
            dtBasicPlans.Columns.Add("datevalue", typeof(DateTime));
            dtBasicPlans.Columns.Add(new DataColumn("alacartebaseprice"));
            //ViewState["customer_basic_plans"] = dtBasicPlans;

            //table which holds addon plans
            dtAddonPlans = new DataTable();
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("DEAL_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAddonPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dtAddonPlans.Columns.Add(new DataColumn("HD_Count"));
            dtAddonPlans.Columns.Add(new DataColumn("SD_Count"));
            dtAddonPlans.Columns.Add(new DataColumn("Total_Count"));
            dtAddonPlans.Columns.Add(new DataColumn("BD_PRICE"));
            //--------------
            dtAddonPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtAddonPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtAddonPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAddonPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_STATUS"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAddonPlans.Columns.Add(new DataColumn("GRACE"));


            //table which holds addon plans
            dtAddonPlansreg = new DataTable();
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_POID"));
            dtAddonPlansreg.Columns.Add(new DataColumn("DEAL_POID"));
            dtAddonPlansreg.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAddonPlansreg.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dtAddonPlansreg.Columns.Add(new DataColumn("HD_Count"));
            dtAddonPlansreg.Columns.Add(new DataColumn("SD_Count"));
            dtAddonPlansreg.Columns.Add(new DataColumn("Total_Count"));
            dtAddonPlansreg.Columns.Add(new DataColumn("BD_PRICE"));
            //----------------
            dtAddonPlansreg.Columns.Add(new DataColumn("ACTIVATION"));
            dtAddonPlansreg.Columns.Add(new DataColumn("EXPIRY"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_STATUS"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAddonPlansreg.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAddonPlansreg.Columns.Add(new DataColumn("GRACE"));


            //ViewState["customer_addon_plans"] = dtAddonPlans;

            //table which holds a-la-carte plans

            dtAlacartePlans = new DataTable();
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAlacartePlans.Columns.Add(new DataColumn("DEAL_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAlacartePlans.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dtAlacartePlans.Columns.Add(new DataColumn("HD_Count"));
            dtAlacartePlans.Columns.Add(new DataColumn("SD_Count"));
            dtAlacartePlans.Columns.Add(new DataColumn("Total_Count"));
            dtAlacartePlans.Columns.Add(new DataColumn("BD_PRICE"));
            //-------------
            dtAlacartePlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtAlacartePlans.Columns.Add(new DataColumn("EXPIRY"));
            dtAlacartePlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_STATUS"));

            // created by vivek 16-nov-2015 
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAlacartePlans.Columns.Add(new DataColumn("GRACE"));


            dthathwayspecial = new DataTable();
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_NAME"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_POID"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_TYPE"));
            dthathwayspecial.Columns.Add(new DataColumn("DEAL_POID"));
            dthathwayspecial.Columns.Add(new DataColumn("CUST_PRICE"));
            dthathwayspecial.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dthathwayspecial.Columns.Add(new DataColumn("HD_Count"));
            dthathwayspecial.Columns.Add(new DataColumn("SD_Count"));
            dthathwayspecial.Columns.Add(new DataColumn("Total_Count"));
            dthathwayspecial.Columns.Add(new DataColumn("BD_PRICE"));
            //-----
            dthathwayspecial.Columns.Add(new DataColumn("ACTIVATION"));
            dthathwayspecial.Columns.Add(new DataColumn("EXPIRY"));
            dthathwayspecial.Columns.Add(new DataColumn("PACKAGE_ID"));
            dthathwayspecial.Columns.Add(new DataColumn("PURCHASE_POID"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_STATUS"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dthathwayspecial.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dthathwayspecial.Columns.Add(new DataColumn("GRACE"));


            dtAlacartePlansFree = new DataTable();
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_POID"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("DEAL_POID"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("LCO_PRICE"));
            //-------Tariff Order new Columns
            dtAlacartePlansFree.Columns.Add(new DataColumn("HD_Count"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("SD_Count"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("Total_Count"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("BD_PRICE"));
            //---------------------------
            dtAlacartePlansFree.Columns.Add(new DataColumn("ACTIVATION"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("EXPIRY"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_STATUS"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAlacartePlansFree.Columns.Add(new DataColumn("GRACE"));
            bindAllGrids(planstring);
        }

        protected void BtnRetract_Click(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            try
            {
                string service_popmsg1 = "";
                string service_popmsg2 = "";
                string service_op = "Retract";
                service_popmsg1 = "This will Retrack the service.";
                service_popmsg2 = "Are you sure you want to Retrack?";

                setRetractServicePopup(service_popmsg1, service_popmsg2, service_op.Trim());
                popretrctservice.Show();
                StatbleDynamicTabs();

            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
        }



        protected void lnkBCancel_Click(object sender, EventArgs e)
        {

            //lnkatag_Click(null, null);
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            int rindex = (((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex;
            // HiddenField hdnPlanId = (HiddenField)grdAddOnPlan.Rows[rindex].FindControl("hdnADPlanId");
            HiddenField hdnBasicPlanName = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanName");
            // HiddenField hdnPlanType = (HiddenField)grdAddOnPlan.Rows[rindex].FindControl("hdnADPlanType");
            HiddenField hdnBasicPlanPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanPoid");
            HiddenField hdnBasicDealPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicDealPoid");
            // HiddenField hdnProductPoid = (HiddenField)grdAddOnPlan.Rows[rindex].FindControl("hdnADProductPoid");
            HiddenField hdnBasicCustPrice = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicCustPrice");
            HiddenField hdnBasicLcoPrice = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicLcoPrice");
            HiddenField hdnBasicActivation = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicActivation");
            HiddenField hdnBasicExpiry = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicExpiry");
            HiddenField hdnBasicPackageId = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPackageId");
            HiddenField hdnBasicPurchasePoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPurchasePoid");
            HiddenField hdnChannelCount = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnChannelCount");
            Session["ChannelCount"] = hdnChannelCount.Value;
            //check box for Addon Autorenewal
            CheckBox cbBAutorenew = (CheckBox)grdBasicPlanDetails.Rows[rindex].FindControl("cbBAutorenew");
            Hashtable htData = new Hashtable();
            //  htData["planid"] = hdnPlanId.Value;
            htData["planname"] = hdnBasicPlanName.Value;
            // htData["plantype"] = hdnPlanType.Value;
            htData["planpoid"] = hdnBasicPlanPoid.Value;
            htData["dealpoid"] = hdnBasicDealPoid.Value;
            htData["custprice"] = hdnBasicCustPrice.Value;
            htData["lcoprice"] = hdnBasicLcoPrice.Value;
            htData["activation"] = hdnBasicActivation.Value;
            htData["expiry"] = hdnBasicExpiry.Value;
            htData["packageid"] = hdnBasicPackageId.Value;
            htData["purchasepoid"] = hdnBasicPurchasePoid.Value;
            htData["IP"] = Ip;

            if (cbBAutorenew.Checked)
            {
                htData["autorenew"] = "Y";
            }
            else
            {
                htData["autorenew"] = "N";
            }
            htData["plantypevalue"] = "B"; //Added By Vivek Singh on 11-Jul-2016
            GrdRenewConfrim.DataSource = null;
            GrdRenewConfrim.DataBind();
            GrdRenewConfrim2.DataSource = null;
            GrdRenewConfrim2.DataBind();
            DataTable dtRenew = new DataTable();
            dtRenew.Columns.Add(new DataColumn("PLAN_NAME"));
            dtRenew.Columns.Add(new DataColumn("CUST_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("LCO_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("discount", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("netmrp", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("Activation"));
            dtRenew.Columns.Add(new DataColumn("valid_upto"));
            dtRenew.Columns.Add(new DataColumn("plan_poid"));
            dtRenew.Columns.Add(new DataColumn("DEAL_POID"));
            dtRenew.Columns.Add(new DataColumn("plan_type"));
            dtRenew.Columns.Add(new DataColumn("AutoRenew"));
            dtRenew.Rows.Add(htData["planname"], htData["custprice"], htData["lcoprice"], 0, htData["custprice"], htData["activation"], htData["expiry"], htData["planpoid"], htData["dealpoid"], htData["activation"], "N");
            string conResult = callGetProviConfirm(htData, "C");
            if (conResult == null)
            {
                popAdd.Hide(); //ajax control toolkit bug quickfix
                StatbleDynamicTabs();
                return;
            }
            else
            {

                GrdRenewConfrim.DataSource = dtRenew;
                GrdRenewConfrim.DataBind();
                GrdRenewConfrim2.DataSource = dtRenew;
                GrdRenewConfrim2.DataBind();
                string[] conResult_arr = conResult.Split('$');
                htData["refund_amt"] = conResult_arr[1];
                htData["days_left"] = conResult_arr[0];

                try
                {
                    htData["refund_lcoamt"] = conResult_arr[2];
                }
                catch
                {
                    htData["refund_lcoamt"] = "Rs.0";
                }
                if (dtRenew.Rows.Count > 0)
                {
                    GrdRenewConfrim.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim2.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                    GrdRenewConfrim.DataSource = dtRenew;
                    GrdRenewConfrim.DataBind();
                    GrdRenewConfrim2.DataSource = dtRenew;
                    GrdRenewConfrim2.DataBind();
                }
                setPopup("This will cancel the plan with following details.", "Are you sure you want to cancel?", "C", "B", htData);
                pop.Show();
                popAdd.Hide(); //ajax control toolkit bug quickfix
            }
            StatbleDynamicTabs();

        }


        protected void cbBAutorenew_Clicked(object sender, EventArgs e)
        {
            //lnkatag_Click(null, null);
            bool AutoRenewbasic;

            for (int i = 0; i < grdBasicPlanDetails.Rows.Count; i++)
            {
                string planname = "";
                planname = grdBasicPlanDetails.Rows[i].Cells[0].Text.ToString();

                AutoRenewbasic = AutoRenewstatus(ViewState["vcid"].ToString(), ViewState["customer_no"].ToString(), planname.ToString());
                if (AutoRenewbasic == true)
                {
                    ((CheckBox)(grdAddOnPlan.Rows[i].FindControl("cbBAutorenew"))).Checked = true;

                }
            }
            StatbleDynamicTabs();
        } // done by vivek 20150612

        protected void btnRenSubmit_Click(object sender, EventArgs e)
        {
            string strPLANPOISs = "";
            ViewState["TblPlanAddfinal"] = null;
            GrdaddplanConfrim.DataSource = null;
            GrdaddplanConfrim.DataBind();
            nullFOCViewState();
            lbltotaladd.Text = "0.00/-";
            hdntotaladdamount.Value = "0";
            ViewState["Total"] = "0";
            DataTable TblPlanAddfinal = new DataTable();
            TblPlanAddfinal.Columns.Add("plan_name");
            TblPlanAddfinal.Columns.Add("cust_price", typeof(double));
            TblPlanAddfinal.Columns.Add("lco_price", typeof(double));
            TblPlanAddfinal.Columns.Add("BC_price", typeof(double));
            TblPlanAddfinal.Columns.Add("discount", typeof(double));
            TblPlanAddfinal.Columns.Add("netmrp", typeof(double));
            TblPlanAddfinal.Columns.Add("ChannelCount", typeof(double));
            TblPlanAddfinal.Columns.Add("plan_poid");
            TblPlanAddfinal.Columns.Add("deal_poid");
            TblPlanAddfinal.Columns.Add("productid");
            TblPlanAddfinal.Columns.Add("plan_type");
            TblPlanAddfinal.Columns.Add("autorenew");
            TblPlanAddfinal.Columns.Add("Message");
            TblPlanAddfinal.Columns.Add("Code");
            TblPlanAddfinal.Columns.Add("foctype");

            //-----NCF-------
            DataTable TblNCFPlanAddfinal = new DataTable();
            TblNCFPlanAddfinal.Columns.Add("plan_name");
            TblNCFPlanAddfinal.Columns.Add("cust_price", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("lco_price", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("BC_price", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("discount", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("netmrp", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("ChannelCount", typeof(double));
            TblNCFPlanAddfinal.Columns.Add("plan_poid");
            TblNCFPlanAddfinal.Columns.Add("deal_poid");
            TblNCFPlanAddfinal.Columns.Add("productid");
            TblNCFPlanAddfinal.Columns.Add("plan_type");
            TblNCFPlanAddfinal.Columns.Add("autorenew");
            TblNCFPlanAddfinal.Columns.Add("Message");
            TblNCFPlanAddfinal.Columns.Add("Code");
            TblNCFPlanAddfinal.Columns.Add("foctype");

            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            Hashtable htData = new Hashtable();
            htData["IP"] = Ip;
            ViewState["SearchedPoid"] = null;
            ViewState["SearchedPlanName"] = null;
            string strPOID = "";
            if (grdAddOnPlan.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in grdAddOnPlan.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("cbAddonrenew");
                    if (chk != null & chk.Checked)
                    {
                        string strBrodPrice = gvrow.Cells[5].Text.ToString();
                        HiddenField hdnplanaddPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                        HiddenField hdnplanaddDealPoid = (HiddenField)gvrow.FindControl("hdnADDealPoid");
                        HiddenField hdnplanaddproducteId = (HiddenField)gvrow.FindControl("hdnADPurchasePoid");
                        HiddenField hdnplanaddplantype = (HiddenField)gvrow.FindControl("hdnADPlanType");
                        HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                        HiddenField hdnplanaddplanBCPrice = (HiddenField)gvrow.FindControl("hdnADCustPrice");
                        HiddenField hdnplanaddplanLCOPrice = (HiddenField)gvrow.FindControl("hdnADLcoPrice");
                        HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                        strPOID = hdnplanaddPlanPoid.Value + "," + strPOID;
                        ViewState["SearchedPoid"] = hdnplanaddPlanPoid.Value.Trim();
                        ViewState["SearchedPlanName"] = hdnADPlanName.Value;
                        htData["planpoid"] = hdnplanaddPlanPoid.Value.Trim();
                        htData["autorenew"] = "N";
                        string conResult = callGetProviConfirmAddplan(htData, "A");

                        string[] result_arr = conResult.Split('#');
                        string[] result_arr2 = result_arr[1].Split('$');
                        if (result_arr[0] != "9999")
                        {
                            //result_arr[1];
                            TblPlanAddfinal.Rows.Add(hdnADPlanName.Value, hdnplanaddplanBCPrice.Value, hdnplanaddplanLCOPrice.Value, strBrodPrice, 0, 0, hdnChannelCount.Value, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                          hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), result_arr[1].ToString(), result_arr[0].ToString(), "");
                        }
                        else
                        {
                            //TblPlanAddfinal.Rows.Add(hdnADPlanName.Value, hdnplanaddplanBCPrice.Value, hdnplanaddplanLCOPrice.Value, strBrodPrice, Convert.ToDouble(result_arr[1]), Convert.ToDouble(hdnplanaddplanBCPrice.Value) - Convert.ToDouble(result_arr[1]), hdnChannelCount.Value, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                            // hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), "Valid Plan for Add", result_arr[0].ToString(), "");
                        
                            try
                            {
                                if (result_arr2[1].ToString() != "")
                                {
                                    TblPlanAddfinal.Rows.Add(hdnADPlanName.Value, result_arr2[1].ToString(), result_arr2[2].ToString(), strBrodPrice, Convert.ToDouble(result_arr2[0]), Convert.ToDouble(hdnplanaddplanBCPrice.Value) - Convert.ToDouble(result_arr2[0]), hdnChannelCount.Value, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                                     hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), "Valid Plan for Add", result_arr[0].ToString(), "");
                                }
                            }
                            catch (Exception)
                            {
                                TblPlanAddfinal.Rows.Add(hdnADPlanName.Value, 0, 0, strBrodPrice, Convert.ToDouble(result_arr2[0]), Convert.ToDouble(hdnplanaddplanBCPrice.Value) - Convert.ToDouble(0), hdnChannelCount.Value, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                                  hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), "Valid Plan for Add", result_arr[0].ToString(), "");
                            }
                        }

                    }
                }
            }
            if (grdCarte.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in grdCarte.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkalRenew");
                    if (chk != null & chk.Checked)
                    {
                        string strBrodPrice = gvrow.Cells[5].Text.ToString();
                        HiddenField hdnplanaddPlanPoid = (HiddenField)gvrow.FindControl("hdnALPlanPoid");
                        HiddenField hdnplanaddDealPoid = (HiddenField)gvrow.FindControl("hdnALDealPoid");
                        HiddenField hdnplanaddproducteId = (HiddenField)gvrow.FindControl("hdnALPurchasePoid");
                        HiddenField hdnplanaddplantype = (HiddenField)gvrow.FindControl("hdnALPlanType");
                        HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnALPlanName");
                        HiddenField hdnplanaddplanBCPrice = (HiddenField)gvrow.FindControl("hdnALCustPrice");
                        HiddenField hdnplanaddplanLCOPrice = (HiddenField)gvrow.FindControl("hdnALLcoPrice");
                        HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                        strPOID = hdnplanaddPlanPoid.Value + "," + strPOID;
                        ViewState["SearchedPoid"] = hdnplanaddPlanPoid.Value.Trim();
                        ViewState["SearchedPlanName"] = hdnADPlanName.Value;
                        htData["planpoid"] = hdnplanaddPlanPoid.Value.Trim();
                        htData["autorenew"] = "N";
                        string conResult = callGetProviConfirmAddplan(htData, "A");
                        string[] result_arr = conResult.Split('#');
                        string[] result_arr2 = result_arr[1].Split('$');
                        if (result_arr[0] != "9999")
                        {
                            //result_arr[1];
                            TblPlanAddfinal.Rows.Add(hdnADPlanName.Value, hdnplanaddplanBCPrice.Value, hdnplanaddplanLCOPrice.Value, strBrodPrice, 0, 0, hdnChannelCount.Value, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                          hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), result_arr[1].ToString(), result_arr[0].ToString(), "");
                        }
                        else
                        {
                            //TblPlanAddfinal.Rows.Add(hdnADPlanName.Value, hdnplanaddplanBCPrice.Value, hdnplanaddplanLCOPrice.Value, strBrodPrice, Convert.ToDouble(result_arr[1]), Convert.ToDouble(hdnplanaddplanBCPrice.Value) - Convert.ToDouble(result_arr[1]), hdnChannelCount.Value, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                            // hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), "Valid Plan for Add", result_arr[0].ToString(), "");
                            try
                            {
                                if (result_arr2[1].ToString() != "")
                                {
                                    TblPlanAddfinal.Rows.Add(hdnADPlanName.Value, result_arr2[1].ToString(), result_arr2[2].ToString(), strBrodPrice, Convert.ToDouble(result_arr2[0]), Convert.ToDouble(hdnplanaddplanBCPrice.Value) - Convert.ToDouble(result_arr2[0]), hdnChannelCount.Value, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                                     hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), "Valid Plan for Add", result_arr[0].ToString(), "");
                                }
                            }
                            catch (Exception)
                            {
                                TblPlanAddfinal.Rows.Add(hdnADPlanName.Value, 0, 0, strBrodPrice, Convert.ToDouble(result_arr2[0]), Convert.ToDouble(hdnplanaddplanBCPrice.Value) - Convert.ToDouble(0), hdnChannelCount.Value, hdnplanaddPlanPoid.Value.Trim(), hdnplanaddDealPoid.Value.Trim(),
                                  hdnplanaddproducteId.Value.Trim(), hdnplanaddplantype.Value.Trim(), htData["autorenew"].ToString(), "Valid Plan for Add", result_arr[0].ToString(), "");
                            }
                        }
                    }
                }
            }
            try
            {
                DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                string stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
                string vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                string selected_service_str = myResultSet.Rows[0]["SERVICE_STRING"].ToString();
                selected_service_str = selected_service_str.Replace("|", "$");
                ViewState["strNCFPlanListNew"] = null;
                Cls_Business_TxnAssignPlan objPlan = new Cls_Business_TxnAssignPlan();
                //FileLogTextChange1(vc_id, "Calling NCF Procedure", selected_service_str, strPLANPOISs);
                string strNCFPlanList = objPlan.Check_NFCPlan(Session["username"].ToString(), selected_service_str, ViewState["cityid"].ToString(), ViewState["customer_no"].ToString(), Session["operator_id"].ToString(), strPOID, "A", vc_id);
                //FileLogTextChange1(vc_id, "Calling NCF Procedure", strNCFPlanList, "");
                string[] strPlanlist = strNCFPlanList.Split('$');
                if (strPlanlist[0] == "9999")
                {
                    if (strPlanlist[1] == "Y")
                    {
                        if (strPlanlist[3] == "B")
                        {
                            DataTable dt = new DataTable();
                            dt = objPlan.getNCFPlanDetails(Session["username"].ToString(), strPlanlist[2].ToString(), ViewState["JVFlag"].ToString(), ViewState["cityid"].ToString(), Session["dasarea"].ToString(), Convert.ToString(Session["operator_id"]), Convert.ToString(Session["JVNO"]));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                ViewState["strNCFPlanListNew"] = strPlanlist[2].ToString();
                                Double SDcount = Convert.ToDouble(dt.Rows[i][13].ToString());
                                Double HDcount = Convert.ToDouble(dt.Rows[i][14].ToString()) * 2;
                                double BC_PRICE = Convert.ToDouble(dt.Rows[i][15].ToString());
                                Double Channelcount = SDcount + HDcount;
                                TblPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), BC_PRICE, dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), "Valid Plan for Add", dt.Rows[i][11].ToString(), dt.Rows[i][12].ToString());
                            }
                        }
                        else
                        {
                            DataTable dt = new DataTable();
                            dt = objPlan.getNCFPlanDetails(Session["username"].ToString(), strPlanlist[2].ToString(), ViewState["JVFlag"].ToString(), ViewState["cityid"].ToString(), Session["dasarea"].ToString(), Convert.ToString(Session["operator_id"]), Convert.ToString(Session["JVNO"]));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                ViewState["strNCFPlanListNew"] = strPlanlist[2].ToString();
                                Double SDcount = Convert.ToDouble(dt.Rows[i][13].ToString());
                                Double HDcount = Convert.ToDouble(dt.Rows[i][14].ToString()) * 2;
                                double BC_PRICE = Convert.ToDouble(dt.Rows[i][15].ToString());
                                Double Channelcount = SDcount + HDcount;
                                TblNCFPlanAddfinal.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), BC_PRICE, dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), Channelcount, dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), dt.Rows[i][8].ToString(), dt.Rows[i][9].ToString(), "Valid Plan for Add", "9999", dt.Rows[i][12].ToString());
                            }
                            for (int i = 0; i < TblPlanAddfinal.Rows.Count; i++)
                            {
                                TblNCFPlanAddfinal.Rows.Add(TblPlanAddfinal.Rows[i][0].ToString(), TblPlanAddfinal.Rows[i][1].ToString(), TblPlanAddfinal.Rows[i][2].ToString(), TblPlanAddfinal.Rows[i][3].ToString(), TblPlanAddfinal.Rows[i][4].ToString(), TblPlanAddfinal.Rows[i][5].ToString(), TblPlanAddfinal.Rows[i][6].ToString(), TblPlanAddfinal.Rows[i][7].ToString(), TblPlanAddfinal.Rows[i][8].ToString(), TblPlanAddfinal.Rows[i][9].ToString(), TblPlanAddfinal.Rows[i][10].ToString(), TblPlanAddfinal.Rows[i][11].ToString(), TblPlanAddfinal.Rows[i][12].ToString(), TblPlanAddfinal.Rows[i][13].ToString(), TblPlanAddfinal.Rows[i][13].ToString());
                            }
                            TblPlanAddfinal.Clear();
                        }
                    }
                    Session["ChannelCount"] = strPlanlist[4].ToString();
                }
                else
                {
                    Session["ChannelCount"] = lblChannelcount.Text;
                }

                GrdaddplanConfrim.DataSource = null;
                GrdaddplanConfrim.DataBind();
                if (TblNCFPlanAddfinal.Rows.Count > 0 || TblPlanAddfinal.Rows.Count > 0)
                {

                    //GrdaddplanConfrim.Columns[2].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("LCO_PRICE")).Sum().ToString();
                    //GrdaddplanConfrim.Columns[3].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("discount")).Sum().ToString();
                    //GrdaddplanConfrim.Columns[4].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("netmrp")).Sum().ToString();
                    if (TblPlanAddfinal.Rows.Count > 0)
                    {
                        GrdaddplanConfrim.Columns[1].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                        GrdaddplanConfrim.Columns[2].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("LCO_PRICE")).Sum().ToString();
                        //GrdaddplanConfrim.Columns[3].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("BC_PRICE")).Sum().ToString();
                        //GrdaddplanConfrim.Columns[4].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("ChannelCount")).Sum().ToString();
                        GrdaddplanConfrim.DataSource = TblPlanAddfinal;
                        GrdaddplanConfrim.DataBind();
                    }
                    else if (TblNCFPlanAddfinal.Rows.Count > 0)
                    {
                        GrdaddplanConfrim.Columns[1].FooterText = TblNCFPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                        GrdaddplanConfrim.Columns[2].FooterText = TblNCFPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("LCO_PRICE")).Sum().ToString();
                        //GrdaddplanConfrim.Columns[3].FooterText = TblNCFPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("BC_PRICE")).Sum().ToString();
                        //GrdaddplanConfrim.Columns[4].FooterText = TblNCFPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("ChannelCount")).Sum().ToString();
                        GrdaddplanConfrim.DataSource = TblNCFPlanAddfinal;
                        GrdaddplanConfrim.DataBind();
                    }
                    popaddplanconfirm.Show();
                }
                else
                {
                    msgboxstr("Please select Plan");
                }
            }
            catch (Exception ex)
            {
                msgboxstr(ex.ToString());
            }
            //}
            StatbleDynamicTabs();
        }
        public void oldallRenewal()
        {
            popupRenewAll.Show();
            string strgetbasic = "";
            string strgetaddon = "";
            string strgetalcarte = "";
            string strHathwayspecial = "";
            string strgetalldet = "";
            string strNCFRenewal = "";
            string strPOID = "";
            DataTable dtRenew = new DataTable();
            dtRenew.Columns.Add(new DataColumn("PLAN_NAME"));
            dtRenew.Columns.Add(new DataColumn("CUST_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("LCO_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("discount", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("Activation"));
            dtRenew.Columns.Add(new DataColumn("valid_upto"));
            if (grdBasicPlanDetails.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in grdBasicPlanDetails.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("cbBasicrenew");
                    if (chk != null & chk.Checked)
                    {
                        //Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["purchasepoid"];
                        HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnBasicPlanPoid");
                        HiddenField hdnBasicPurchasePoid = (HiddenField)gvrow.FindControl("hdnBasicPurchasePoid");
                        HiddenField hdnBasicExpiry = (HiddenField)gvrow.FindControl("hdnBasicExpiry");
                        HiddenField hdnBasicActivation = (HiddenField)gvrow.FindControl("hdnBasicActivation");
                        HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnBasicPlanName");
                        HiddenField hdnBasicActionFlag = (HiddenField)gvrow.FindControl("hdnBasicActionFlag");
                        HiddenField hdnBasicPlanType = (HiddenField)gvrow.FindControl("hdnBasicPlanType");
                        HiddenField hdnBasicCustPrice = (HiddenField)gvrow.FindControl("hdnBasicCustPrice");
                        HiddenField hdnBasicLcoPrice = (HiddenField)gvrow.FindControl("hdnBasicLcoPrice");
                        if (hdnBasicPlanType.Value.ToString() != "NCF")
                        {
                            if (hdnBasicActivation.Value.ToString().ToUpper() == "ACTIVE")
                            {
                                strPOID = hdnBasicPlanPoid.Value + "," + strPOID;
                                strgetbasic += hdnBasicPlanPoid.Value.ToString() + "$" + hdnBasicPurchasePoid.Value.ToString() + "$" + hdnBasicExpiry.Value.ToString() + "$" + hdnBasicActivation.Value.ToString() + "$" + hdnBasicPlanName.Value.ToString() + "$" + hdnBasicActionFlag.Value.ToString() + "$" + "B" + "~";
                                dtRenew.Rows.Add(hdnBasicPlanName.Value, hdnBasicCustPrice.Value, hdnBasicLcoPrice.Value, 0, hdnBasicActivation.Value, hdnBasicExpiry.Value);
                            }
                        }
                    }
                }
                strgetbasic = strgetbasic.ToString().TrimEnd('~');
            }
            if (grdAddOnPlan.Rows.Count > 0)
            {


                foreach (GridViewRow gvrow in grdAddOnPlan.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("cbAddonrenew");
                    if (chk != null & chk.Checked)
                    {
                        HiddenField hdnPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                        HiddenField hdnADPurchasePoid = (HiddenField)gvrow.FindControl("hdnADPurchasePoid");
                        HiddenField hdnADExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                        HiddenField hdnADActivation = (HiddenField)gvrow.FindControl("hdnADActivation");
                        HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                        HiddenField hdnADPlanRenewFlag = (HiddenField)gvrow.FindControl("hdnADPlanRenewFlag");
                        HiddenField hdnADCustPrice = (HiddenField)gvrow.FindControl("hdnADCustPrice");
                        HiddenField hdnADLcoPrice = (HiddenField)gvrow.FindControl("hdnADLcoPrice");
                        strPOID = hdnPlanPoid.Value + "," + strPOID;
                        strgetaddon += hdnPlanPoid.Value.ToString() + "$" + hdnADPurchasePoid.Value.ToString() + "$" + hdnADExpiry.Value.ToString() + "$" + hdnADActivation.Value.ToString() + "$" + hdnADPlanName.Value.ToString() + "$" + hdnADPlanRenewFlag.Value.ToString() + "$" + "AD" + "~";
                        dtRenew.Rows.Add(hdnADPlanName.Value, hdnADCustPrice.Value, hdnADLcoPrice.Value, 0, hdnADActivation.Value, hdnADExpiry.Value);
                    }
                }
                strgetaddon = strgetaddon.ToString().TrimEnd('~');
            }

            if (Grdhathwayspecial.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in Grdhathwayspecial.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("cbAddonrenew");
                    if (chk != null & chk.Checked)
                    {
                        HiddenField hdnPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                        HiddenField hdnADPurchasePoid = (HiddenField)gvrow.FindControl("hdnADPurchasePoid");
                        HiddenField hdnADExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                        HiddenField hdnADActivation = (HiddenField)gvrow.FindControl("hdnADActivation");
                        HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                        HiddenField hdnADPlanRenewFlag = (HiddenField)gvrow.FindControl("hdnADPlanRenewFlag");
                        HiddenField hdnADCustPrice = (HiddenField)gvrow.FindControl("hdnADCustPrice");
                        HiddenField hdnADLcoPrice = (HiddenField)gvrow.FindControl("hdnADLcoPrice");
                        if (hdnADActivation.Value.ToString().ToUpper() == "ACTIVE")
                        {
                            strPOID = hdnPlanPoid.Value + "," + strPOID;
                            strHathwayspecial += hdnPlanPoid.Value.ToString() + "$" + hdnADPurchasePoid.Value.ToString() + "$" + hdnADExpiry.Value.ToString() + "$" + hdnADActivation.Value.ToString() + "$" + hdnADPlanName.Value.ToString() + "$" + hdnADPlanRenewFlag.Value.ToString() + "$" + "HSP" + "~";
                            dtRenew.Rows.Add(hdnADPlanName.Value, hdnADCustPrice.Value, hdnADLcoPrice.Value, 0, hdnADActivation.Value, hdnADExpiry.Value);
                        }
                    }
                }
                strHathwayspecial = strHathwayspecial.ToString().TrimEnd('~');
            }


            if (grdCarte.Rows.Count > 0)
            {


                foreach (GridViewRow gvrow in grdCarte.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkalRenew");
                    if (chk != null & chk.Checked)
                    {
                        //Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["purchasepoid"];
                        HiddenField hdnALPlanPoid = (HiddenField)gvrow.FindControl("hdnALPlanPoid");
                        HiddenField hdnALPurchasePoid = (HiddenField)gvrow.FindControl("hdnALPurchasePoid");
                        HiddenField hdnALExpiry = (HiddenField)gvrow.FindControl("hdnALExpiry");
                        HiddenField hdnALActivation = (HiddenField)gvrow.FindControl("hdnALActivation");
                        HiddenField hdnALPlanName = (HiddenField)gvrow.FindControl("hdnALPlanName");
                        HiddenField hdnALPlanRenewFlag = (HiddenField)gvrow.FindControl("hdnALPlanRenewFlag");
                        HiddenField hdnALCustPrice = (HiddenField)gvrow.FindControl("hdnALCustPrice");
                        HiddenField hdnALLcoPrice = (HiddenField)gvrow.FindControl("hdnALLcoPrice");
                        strPOID = hdnALPlanPoid.Value + "," + strPOID;
                        strgetalcarte += hdnALPlanPoid.Value.ToString() + "$" + hdnALPurchasePoid.Value.ToString() + "$" + hdnALExpiry.Value.ToString() + "$" + hdnALActivation.Value.ToString() + "$" + hdnALPlanName.Value.ToString() + "$" + hdnALPlanRenewFlag.Value.ToString() + "$" + "AL" + "~";
                        dtRenew.Rows.Add(hdnALPlanName.Value, hdnALCustPrice.Value, hdnALLcoPrice.Value, 0, hdnALActivation.Value, hdnALExpiry.Value);
                    }
                }
                strgetalcarte = strgetalcarte.ToString().TrimEnd('~');
            }


            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
            string stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
            string vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
            string selected_service_str = myResultSet.Rows[0]["SERVICE_STRING"].ToString();
            selected_service_str = selected_service_str.Replace("|", "$");
            Cls_Business_TxnAssignPlan objPlan = new Cls_Business_TxnAssignPlan();
            string strPLANPOISs = strPOID.TrimEnd(',');
            string strNCFPlanList = objPlan.Check_NFCPlan(Session["username"].ToString(), selected_service_str, ViewState["cityid"].ToString(), ViewState["customer_no"].ToString(), Session["operator_id"].ToString(), strPLANPOISs, "R", vc_id);
            string[] strPlanlist = strNCFPlanList.Split('$');
            if (strPlanlist[0] == "9999")
            {
                if (strPlanlist[1] == "Y")
                {
                    DataTable dt = new DataTable();
                    dt = objPlan.getNCFPlanDetails(Session["username"].ToString(), strPlanlist[2].ToString(), ViewState["JVFlag"].ToString(), ViewState["cityid"].ToString(), Session["dasarea"].ToString(), Convert.ToString(Session["operator_id"]), Convert.ToString(Session["JVNO"]));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strNCFRenewal += dt.Rows[i][5].ToString() + "$" + dt.Rows[i][7].ToString() + "$" + "" + "$" + "" + "$" + dt.Rows[i][0].ToString() + "$" + "N" + "$" + "B" + "~";
                        //dtRenew.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), 0, dt.Rows[i][1].ToString(), "", "", dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), htData["activation"], "N");
                        dtRenew.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), 0, "", "");
                    }
                }
            }
            if (strNCFRenewal != "")
            {
                strNCFRenewal = strNCFRenewal.ToString().TrimEnd('~');

            }
            if (strgetbasic != "")
            {
                strgetalldet = strgetbasic + "#";
            }
            if (strgetaddon != "")
            {
                strgetalldet += strgetaddon + "#";
            }
            if (strgetalcarte != "")
            {
                strgetalldet += strgetalcarte + "#";
            }
            if (strHathwayspecial != "")
            {
                strgetalldet += strHathwayspecial + "#";
            }
            if (strNCFRenewal != "")
            {
                strgetalldet += strNCFRenewal + "#";
            }
            Session["strgetalldet"] = strgetalldet;
            if (dtRenew.Rows.Count > 0)
            {
                GrdRenewConfrim.Columns[1].FooterText = dtRenew.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();

                GrdAllRenewConfirm.DataSource = dtRenew;
                GrdAllRenewConfirm.DataBind();
                PnlAllRenewConfirm.Visible = true;
            }

        }

        protected void BtnAutoRenewAll_Click(object sender, EventArgs e)
        {
            /*
            string strgetbasic = "";
            string strgetaddon = "";
            string strgetalcarte = "";
            string strHathwayspecial = "";
            string strgetalldet = "";
            string strNCFRenewal = "";
            string strPOID = "";
            if (grdBasicPlanDetails.Rows.Count > 0)
            {


                foreach (GridViewRow gvrow in grdBasicPlanDetails.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("cbBasicrenew");
                    if (chk != null & chk.Checked)
                    {
                        //Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["purchasepoid"];
                        HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnBasicPlanPoid");
                        HiddenField hdnBasicPurchasePoid = (HiddenField)gvrow.FindControl("hdnBasicPurchasePoid");
                        HiddenField hdnBasicExpiry = (HiddenField)gvrow.FindControl("hdnBasicExpiry");
                        HiddenField hdnBasicActivation = (HiddenField)gvrow.FindControl("hdnBasicActivation");
                        HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnBasicPlanName");
                        HiddenField hdnBasicActionFlag = (HiddenField)gvrow.FindControl("hdnBasicActionFlag");
                       /* HiddenField hdnBasicPlanType = (HiddenField)gvrow.FindControl("hdnBasicPlanType");
                        if (hdnBasicPlanType.Value.ToString() != "NCF")
                        {
                            strPOID = hdnBasicPlanPoid.Value + "," + strPOID;
                            strgetbasic += hdnBasicPlanPoid.Value.ToString() + "$" + hdnBasicPurchasePoid.Value.ToString() + "$" + hdnBasicExpiry.Value.ToString() + "$" + hdnBasicActivation.Value.ToString() + "$" + hdnBasicPlanName.Value.ToString() + "$" + hdnBasicActionFlag.Value.ToString() + "$" + "B" + "~";
                        //}

                    }
                }
                strgetbasic = strgetbasic.ToString().TrimEnd('~');
            }




            if (grdAddOnPlan.Rows.Count > 0)
            {


                foreach (GridViewRow gvrow in grdAddOnPlan.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("cbAddonrenew");
                    if (chk != null & chk.Checked)
                    {
                        HiddenField hdnPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                        HiddenField hdnADPurchasePoid = (HiddenField)gvrow.FindControl("hdnADPurchasePoid");
                        HiddenField hdnADExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                        HiddenField hdnADActivation = (HiddenField)gvrow.FindControl("hdnADActivation");
                        HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                        HiddenField hdnADPlanRenewFlag = (HiddenField)gvrow.FindControl("hdnADPlanRenewFlag");
                        strPOID = hdnPlanPoid.Value + "," + strPOID;
                        strgetaddon += hdnPlanPoid.Value.ToString() + "$" + hdnADPurchasePoid.Value.ToString() + "$" + hdnADExpiry.Value.ToString() + "$" + hdnADActivation.Value.ToString() + "$" + hdnADPlanName.Value.ToString() + "$" + hdnADPlanRenewFlag.Value.ToString() + "$" + "AD" + "~";

                    }
                }
                strgetaddon = strgetaddon.ToString().TrimEnd('~');
            }

            if (Grdhathwayspecial.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in Grdhathwayspecial.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("cbAddonrenew");
                    if (chk != null & chk.Checked)
                    {
                        HiddenField hdnPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                        HiddenField hdnADPurchasePoid = (HiddenField)gvrow.FindControl("hdnADPurchasePoid");
                        HiddenField hdnADExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                        HiddenField hdnADActivation = (HiddenField)gvrow.FindControl("hdnADActivation");
                        HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                        HiddenField hdnADPlanRenewFlag = (HiddenField)gvrow.FindControl("hdnADPlanRenewFlag");
                        strPOID = hdnPlanPoid.Value + "," + strPOID;
                        strHathwayspecial += hdnPlanPoid.Value.ToString() + "$" + hdnADPurchasePoid.Value.ToString() + "$" + hdnADExpiry.Value.ToString() + "$" + hdnADActivation.Value.ToString() + "$" + hdnADPlanName.Value.ToString() + "$" + hdnADPlanRenewFlag.Value.ToString() + "$" + "HSP" + "~";

                    }
                }
                strHathwayspecial = strHathwayspecial.ToString().TrimEnd('~');
            }


            if (grdCarte.Rows.Count > 0)
            {


                foreach (GridViewRow gvrow in grdCarte.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkalRenew");
                    if (chk != null & chk.Checked)
                    {
                        //Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["purchasepoid"];
                        HiddenField hdnALPlanPoid = (HiddenField)gvrow.FindControl("hdnALPlanPoid");
                        HiddenField hdnALPurchasePoid = (HiddenField)gvrow.FindControl("hdnALPurchasePoid");
                        HiddenField hdnALExpiry = (HiddenField)gvrow.FindControl("hdnALExpiry");
                        HiddenField hdnALActivation = (HiddenField)gvrow.FindControl("hdnALActivation");
                        HiddenField hdnALPlanName = (HiddenField)gvrow.FindControl("hdnALPlanName");
                        HiddenField hdnALPlanRenewFlag = (HiddenField)gvrow.FindControl("hdnALPlanRenewFlag");
                        strPOID = hdnALPlanPoid.Value + "," + strPOID;
                        strgetalcarte += hdnALPlanPoid.Value.ToString() + "$" + hdnALPurchasePoid.Value.ToString() + "$" + hdnALExpiry.Value.ToString() + "$" + hdnALActivation.Value.ToString() + "$" + hdnALPlanName.Value.ToString() + "$" + hdnALPlanRenewFlag.Value.ToString() + "$" + "AL" + "~";

                    }
                }
                strgetalcarte = strgetalcarte.ToString().TrimEnd('~');
            }


            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
            string stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
            string vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
            string selected_service_str = myResultSet.Rows[0]["SERVICE_STRING"].ToString();
            selected_service_str = selected_service_str.Replace("|", "$");
            Cls_Business_TxnAssignPlan objPlan = new Cls_Business_TxnAssignPlan();
            string strPLANPOISs = strPOID.TrimEnd(',');
            string strNCFPlanList = objPlan.Check_NFCPlan(Session["username"].ToString(), selected_service_str, ViewState["cityid"].ToString(), ViewState["customer_no"].ToString(), Session["operator_id"].ToString(), strPLANPOISs, "R", vc_id);
            string[] strPlanlist = strNCFPlanList.Split('$');
            if (strPlanlist[0] == "9999")
            {
                if (strPlanlist[1] == "Y")
                {
                    DataTable dt = new DataTable();
                    dt = objPlan.getNCFPlanDetails(Session["username"].ToString(), strPlanlist[2].ToString(), ViewState["JVFlag"].ToString(), ViewState["cityid"].ToString(), Session["dasarea"].ToString(), Convert.ToString(Session["operator_id"]));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strNCFRenewal = dt.Rows[i][5].ToString() + "$" + dt.Rows[i][7].ToString() + "$" + "" + "$" + "" + "$" + dt.Rows[i][0].ToString() + "$" + "" + "B" + "~";
                        //dtRenew.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), 0, dt.Rows[i][1].ToString(), "", "", dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString(), htData["activation"], "N");
                    }
                }
            }
            if (strNCFRenewal != "")
            {
                strNCFRenewal.TrimEnd('~');
            }



            if (strgetbasic != "")
            {
                strgetalldet = strgetbasic + "#";
            }
            if (strgetaddon != "")
            {
                strgetalldet += strgetaddon + "#";
            }
            if (strgetalcarte != "")
            {
                strgetalldet += strgetalcarte + "#";
            }
            if (strHathwayspecial != "")
            {
                strgetalldet += strHathwayspecial + "#";
            }
            if (strNCFRenewal != "")
            {
                strgetalldet += strNCFRenewal + "#";
            }
            */
            string strgetalldet = Session["strgetalldet"].ToString();
            if (strgetalldet != "")
            {
                strgetalldet = strgetalldet.ToString().TrimEnd('#');
                string[] Arr_renew = strgetalldet.Split('#');
                DataTable dt = new DataTable();
                dt.Columns.Add("PlanName");
                dt.Columns.Add("RenewStatus");



                for (int i = 0; i <= Arr_renew.Length - 1; i++)
                {
                    string[] arr_planrenewdet = Arr_renew[i].Split('~');
                    for (int k = 0; k <= arr_planrenewdet.Length - 1; k++)
                    {
                        string[] arr_planrenew = arr_planrenewdet[k].Split('$');
                        string plan_poid = arr_planrenew[0].ToString();
                        string plan_purchage_poid = arr_planrenew[1].ToString();
                        string expiray = arr_planrenew[2].ToString();
                        string actdaet = arr_planrenew[3].ToString();
                        string planname = arr_planrenew[4].ToString();

                        bool status = false;

                        bool statsren = fungetrebewdet(arr_planrenew[5].ToString(), arr_planrenew[6].ToString());

                        if (statsren == true)
                        {
                            status = processChangePlanTransaction("R", plan_poid, expiray, plan_purchage_poid, actdaet);
                        }

                        if (status == true)
                        {
                            DataRow row1 = dt.NewRow();
                            row1["PlanName"] = planname;
                            row1["RenewStatus"] = ViewState["ErrorMessage"];
                            dt.Rows.Add(row1);

                        }
                        else if (status == false)
                        {
                            DataRow row1 = dt.NewRow();
                            row1["PlanName"] = planname;
                            row1["RenewStatus"] = ViewState["ErrorMessage"];
                            dt.Rows.Add(row1);

                        }
                    }
                }
                Gridrenew.DataSource = dt;
                Gridrenew.DataBind();
                Gridrenew.Visible = true;
                lblPlanStatus.Text = "";
                //Added by Vivek 18-Feb-2016
                Gridrenew.HeaderRow.Cells[1].Text = "Renew Status";
                lblAllStatus.Text = "Renewal Pack Status";

                popallrenewal.Show();


                // lnkatag_Click(null, null);
            }
            StatbleDynamicTabs();
        }

        public bool fungetrebewdet(string hdnPlanrenewflag, string plantype)
        {



            if (plantype == "B")
            {
                if (ViewState["BasicActionFlag"] != null)
                {

                    if (ViewState["BasicActionFlag"].ToString() == "ND")     // created by vivek 16-nov-2015
                    {

                        ViewState["ErrorMessage"] = "Basic Pack is not due,you can't Renew the pack";
                        return false;

                    }
                }

            }
            else if (plantype == "AD")
            {
                if (hdnPlanrenewflag == "Y") // created by vivek 16-nov-2015                
                {

                    if (ViewState["BasicActionFlag"] != null)
                    {
                        if (ViewState["BasicActionFlag"].ToString() == "D" || ViewState["BasicActionFlag"].ToString() == "EX")
                        {
                            if (ViewState["BasicActionFlag"].ToString() == "EX")
                            {
                                ViewState["ErrorMessage"] = "Basic Pack is expired,you can't Renew the pack";
                                return false;
                            }
                            else
                            {
                                ViewState["ErrorMessage"] = "Basic Pack is due,you can't Renew the pack";
                                return false;
                            }

                        }
                    }
                }
                else
                {
                    ViewState["ErrorMessage"] = "You can't Renew the pack";
                    return false;

                }


            }
            else if (plantype == "AL")
            {

                if (hdnPlanrenewflag == "Y")
                {
                    if (ViewState["BasicActionFlag"] != null)
                    {

                        if (ViewState["BasicActionFlag"].ToString() == "D" || ViewState["BasicActionFlag"].ToString() == "EX")     // created by vivek 16-nov-2015
                        {
                            if (ViewState["BasicActionFlag"].ToString() == "EX")
                            {
                                ViewState["ErrorMessage"] = "Basic Pack is expired,you can't Renew the pack";
                                return false;
                            }
                            else
                            {
                                ViewState["ErrorMessage"] = "Basic Pack is due,you can't Renew the pack";
                                return false;
                            }

                        }
                    }
                }
                else
                {

                    ViewState["ErrorMessage"] = "You can't Renew the pack";
                    return false;

                }
            }

            return true;
        }


        protected void btnAddFreePlan_Click(object sender, EventArgs e) // Added by vivek 04-Jan-2016
        {

            int count = 0;
            DataTable TblPlanAddfinal = (DataTable)ViewState["TblPlanAddfinal"];

            if (TblPlanAddfinal == null)
            {
                GrdaddplanConfrim.DataSource = null;
                GrdaddplanConfrim.DataBind();

                TblPlanAddfinal = new DataTable();
                TblPlanAddfinal.Columns.Add("plan_name");
                TblPlanAddfinal.Columns.Add("cust_price", typeof(double));
                TblPlanAddfinal.Columns.Add("lco_price", typeof(double));
                TblPlanAddfinal.Columns.Add("discount", typeof(double));
                TblPlanAddfinal.Columns.Add("netmrp", typeof(double));
                TblPlanAddfinal.Columns.Add("plan_poid");
                TblPlanAddfinal.Columns.Add("deal_poid");
                TblPlanAddfinal.Columns.Add("productid");
                TblPlanAddfinal.Columns.Add("plan_type");
                TblPlanAddfinal.Columns.Add("autorenew");
                TblPlanAddfinal.Columns.Add("Message");
                TblPlanAddfinal.Columns.Add("Code");
                TblPlanAddfinal.Columns.Add("foctype");
            }
            string planname = "";
            string planpoid = "";
            string bucketcount = "";


            for (int i = 0; i <= grdFreePlan.Rows.Count - 1; i++)
            {
                CheckBox chk = ((CheckBox)(grdFreePlan.Rows[i].FindControl("cbFreePlan")));
                if (chk.Checked == true)
                {
                    count = count + 1;
                }
            }


            if (count > Convert.ToInt32(ViewState["plancount"].ToString()))
            {
                lblFOCMsg.Text = "You can only select " + ViewState["plancount"].ToString() + " Plan ";
                popFOCMsg.Show();
                StatbleDynamicTabs();
                return;
            }

            ViewState["AddedFOC"] = count;
            String Language = "";
            if (ViewState["freeplanpoid"] != null)
            {
                planpoid = ViewState["freeplanpoid"].ToString();
            }
            if (ViewState["freeplanname"] != null)
            {
                planname = ViewState["freeplanname"].ToString();
            }

            if (ViewState["foc_count"] != null)
            {
                count = Convert.ToInt32(ViewState["foc_count"]);
            }

            int bucket1foc = 0;
            int bucket2foc = 0;
            for (int i = 0; i <= grdFreePlan.Rows.Count - 1; i++)
            {
                CheckBox chk = ((CheckBox)(grdFreePlan.Rows[i].FindControl("cbFreePlan")));
                if (chk.Checked == true)
                {
                    count = count + 1;
                    planname = planname + " " + grdFreePlan.Rows[i].Cells[0].Text.ToString() + ",";
                    planpoid = planpoid + " " + ((HiddenField)(grdFreePlan.Rows[i].FindControl("hdnFreePlanPoid"))).Value.ToString() + ",";
                    Language = Language + " '" + ((HiddenField)(grdFreePlan.Rows[i].FindControl("hdnfreeplanlanguage"))).Value.ToString() + "',";

                    if (ViewState["Foc2Checked"].ToString() == "N")
                    {
                        bucket1foc++;
                        ViewState["changebuketcount"] = Convert.ToString(ViewState["changebuketcount"]) + "" + bucket1foc + ",";
                    }
                    else
                    {
                        ViewState["changebuketcount"] = Convert.ToString(ViewState["changebuketcount"]) + "" + "1,";
                    }

                    if (TblPlanAddfinal != null)
                    {

                        TblPlanAddfinal.Rows.Add(grdFreePlan.Rows[i].Cells[0].Text.ToString(), 0, 0, 0, 0, ((HiddenField)(grdFreePlan.Rows[i].FindControl("hdnFreePlanPoid"))).Value.ToString(), "", "", "AD", "NL", "9999", "", ViewState["Foc2Checked"].ToString());


                    }
                }
            }



            Language = Language.TrimEnd(',');
            string msg = "";
            ViewState["CheckFOC2"] = "N";
            ViewState["TblPlanAddfinal"] = TblPlanAddfinal;
            if (ViewState["Foc2Checked"].ToString() == "N")
            {
                ViewState["freeplanpoid"] = planpoid;
                ViewState["freeplanname"] = planname;
                if (ViewState["Newplan_Poid"] != null)
                {
                    ViewState["foc_count"] = count;
                    fillFreePlanGridFOC2(ViewState["Newplan_Poid"].ToString(), "Y", Language);
                }
                else if (ViewState["basic_poids"].ToString().Replace("'", "") != "0")
                {
                    ViewState["foc_count"] = count;
                    fillFreePlanGridFOC2(ViewState["basic_poids"].ToString(), "Y", Language);
                }
                else
                {
                    ViewState["foc_count"] = count;
                    fillFreePlanGridFOC2(ViewState["SearchedPoid"].ToString(), "Y", Language);
                }


                if (ViewState["CheckFOC2"].ToString() == "Y")
                {
                    PopUpFreePlan.Show();
                    StatbleDynamicTabs();
                    return;
                }
            }


            if (ViewState["BasicPlanAlreadyThere"] != null)      //Only FOC Plan Added From BasicPlan GridView HyperLink
            {
                if (count != 0)
                {
                    ViewState["freeplanpoid"] = planpoid.TrimEnd(',');  //Only Free Plan Added
                    ViewState["freeplanname"] = planname;
                    msg = "Are you sure you want to add the following plans - " + planname + "?";
                }
                else
                {
                    lblPopupResponse.Text = "No Eligible Free Regional Pack Selected !";
                    btnRefreshForm.Visible = false;
                    popMsg.Show();
                    StatbleDynamicTabs();
                    return;
                }
            }

            else if (ViewState["ChangedBasicPlanFOCChange"] != null)  //Change Plan From OLD Basic To New Basic Plan When Happening 
            {
                if (count != 0)
                {
                    ViewState["freeplanpoid"] = planpoid.TrimEnd(',');
                    ViewState["freeplanname"] = planname.TrimEnd(',');
                }
                else
                {
                    ViewState["freeplanpoid"] = null;
                    ViewState["freeplanname"] = null;
                }

                btnChangePlanConfirmation_Click(null, null);
                return;
            }

            else    // Basic + FOC Plan Added From Add New Plan DropDown For Basic Plan
            {
                if (count != 0)
                {
                    ViewState["freeplanpoid"] = ViewState["basicplanpoid"].ToString() + "," + planpoid.TrimEnd(',');   //Bsic + Free Plan Added
                    ViewState["freeplanname"] = ViewState["SearchedPlanName"].ToString() + "," + planname;
                    msg = "Are you sure you want to add the following plans - " + ViewState["SearchedPlanName"].ToString() + "," + planname + "?";
                }
                else
                {
                    ViewState["freeplanpoid"] = ViewState["basicplanpoid"].ToString();   //Only Basic Plan Added
                    ViewState["freeplanname"] = ViewState["SearchedPlanName"].ToString();
                    msg = "Are you sure you want to add the following plans - " + ViewState["SearchedPlanName"].ToString() + "?";
                }
            }

            //lblPopupFinalConfMsg.Text = msg;

            //popFinalConf.Show();
            GrdaddplanConfrim.DataSource = null;
            GrdaddplanConfrim.DataBind();
            if (TblPlanAddfinal.Rows.Count > 0)
            {
                GrdaddplanConfrim.Columns[1].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                GrdaddplanConfrim.Columns[2].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("LCO_PRICE")).Sum().ToString();
                GrdaddplanConfrim.Columns[3].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("discount")).Sum().ToString();
                GrdaddplanConfrim.Columns[4].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("netmrp")).Sum().ToString();
                GrdaddplanConfrim.DataSource = TblPlanAddfinal;
                GrdaddplanConfrim.DataBind();


                popaddplanconfirm.Show();
            }
            else
            {
                msgboxstr("Please select Plan");
            }
            StatbleDynamicTabs();
        }

        public void fillFreePlanGridFOC2(string basicplan_poid, string DisplayAlreadyFOCPlan, String Language) // Added by vivek 04-Jan-2016
        {
            ViewState["CheckFOC2"] = "N";
            ViewState["Foc2Checked"] = "Y";
            lblFreePlan.Text = "";
            Session["DisplayAlreadyFOCPlan"] = DisplayAlreadyFOCPlan;
            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
            string StrPlanDetails = obj.getFreePlanDetails2(username, basicplan_poid, ViewState["ala_poids"].ToString(), Language);
            string[] StrPlanDetailsArray = StrPlanDetails.Split('#');
            string[] pl = StrPlanDetailsArray[1].Split('~');



            if (StrPlanDetailsArray[0] == "9999" && pl[1].ToString() != "")
            {
                lbleligfreeplan.Text = "Regional Pack – FOC2";
                grdFreePlan.DataSource = null;
                grdFreePlan.DataBind();

                string strplan = StrPlanDetailsArray[1];
                string[] planarry = strplan.Split('~');
                FreePlandataTableBuilder(dtBasicFreePlans, planarry);
                grdFreePlan.DataSource = dtBasicFreePlans;
                grdFreePlan.DataBind();
                ViewState["basicplanpoid"] = basicplan_poid;
                if (grdFreePlan.Rows.Count < 0)
                {
                    btnAddFreePlan.Visible = false;
                    btnCancelFreePlan.Visible = false;
                    lblFreePlan.Text = "";
                    lblNoOfFOCPlan.Text = "";

                }
                else
                {
                    ViewState["CheckFOC2"] = "Y";
                    grdFreePlan.Visible = true;
                    btnAddFreePlan.Visible = true;
                    btnCancelFreePlan.Visible = true;
                    lblNoOfFOCPlan.Text = "You Can Select Maximum " + ViewState["plancount"].ToString() + " FOC Plan";

                    if (CancelSelectedFOCPlan.ToString().Trim() != "")
                    {
                        if (ViewState["cancelfocpack"] == null)
                        {
                            if (CancelSelectedFOCPlan.ToString().Trim() != "")
                            {
                                ViewState["CancelSelectedFOCPlanId"] = CancelSelectedFOCPlan.TrimEnd(',');
                            }

                        }
                        else
                        {
                            ViewState["CancelSelectedFOCPlanId"] = null;
                        }
                    }
                    CancelSelectedFOCPlan = "";
                }
            }
            else
            {
                grdFreePlan.DataSource = null;
                grdFreePlan.DataBind();
                grdFreePlan.Visible = false;
                btnAddFreePlan.Visible = false;
                btnCancelFreePlan.Visible = false;
                lblNoOfFOCPlan.Text = "";
                lblFreePlan.Text = StrPlanDetailsArray[1].ToString();

            }

            Session["DisplayAlreadyFOCPlan"] = null;

        }

        public void fillFreePlanGrid(string basicplan_poid, string DisplayAlreadyFOCPlan) // Added by vivek 04-Jan-2016
        {
            try
            {
                ViewState["Foc2Checked"] = "N";
                lblFreePlan.Text = "";
                Session["DisplayAlreadyFOCPlan"] = DisplayAlreadyFOCPlan;
                Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                string StrPlanDetails = obj.getFreePlanDetails(username, basicplan_poid, ViewState["ala_poids"].ToString());
                string[] StrPlanDetailsArray = StrPlanDetails.Split('#');
                string[] pl = StrPlanDetailsArray[1].Split('~');

                if (StrPlanDetailsArray[0] == "9999" && pl[1].ToString() != "")
                {
                    grdFreePlan.DataSource = null;
                    grdFreePlan.DataBind();
                    lbleligfreeplan.Text = "Regional Pack – FOC1";

                    string strplan = StrPlanDetailsArray[1];
                    string[] planarry = strplan.Split('~');
                    FreePlandataTableBuilder(dtBasicFreePlans, planarry);
                    ViewState["basicplanpoid"] = basicplan_poid;

                    if (StrPlanDetailsArray[1].Split('~')[0].ToString() == "0" && Convert.ToInt32(ViewState["SpecialFOCCount"]) == 0)
                    {
                        if (ViewState["ChangedBasicPlanFOCChange"] != null)  //Change Plan From Old Basic To New Basic Plan When Happening i.e (Change Hyper Link in BasicPlan GridView)
                        {
                            ViewState["Changefoccheck"] = "YES";
                            btnChangePlanConfirmation_Click(null, null);
                            return;
                        }
                        else
                        {
                            btnAddFreePlan_Click(null, null);
                            return;
                        }
                    }

                    grdFreePlan.DataSource = dtBasicFreePlans;
                    grdFreePlan.DataBind();

                    if (grdFreePlan.Rows.Count < 0)
                    {
                        btnAddFreePlan.Visible = false;
                        btnCancelFreePlan.Visible = false;
                        lblFreePlan.Text = "";
                        lblNoOfFOCPlan.Text = "";
                    }
                    else
                    {

                        grdFreePlan.Visible = true;
                        btnAddFreePlan.Visible = true;
                        btnCancelFreePlan.Visible = true;
                        lblNoOfFOCPlan.Text = "You Can Select Maximum " + ViewState["plancount"].ToString() + " FOC Plan";

                        if (ViewState["cancelfocpack"] == null)
                        {
                            if (CancelSelectedFOCPlan.ToString().Trim() != "")
                            {
                                ViewState["CancelSelectedFOCPlanId"] = CancelSelectedFOCPlan.TrimEnd(',');
                            }

                            CancelSelectedFOCPlan = "";
                        }
                        else
                        {
                            ViewState["CancelSelectedFOCPlanId"] = null;
                        }

                    }
                }
                else
                {
                    grdFreePlan.DataSource = null;
                    grdFreePlan.DataBind();
                    grdFreePlan.Visible = false;
                    btnAddFreePlan.Visible = false;
                    btnCancelFreePlan.Visible = false;
                    lblNoOfFOCPlan.Text = "";
                    lblFreePlan.Text = StrPlanDetailsArray[1].ToString();

                    if (ViewState["TblPlanAddfinal"] != null)
                    {
                        DataTable TblPlanAddfinal = (DataTable)ViewState["TblPlanAddfinal"];
                        if (TblPlanAddfinal.Rows.Count > 0)
                        {
                            GrdaddplanConfrim.Columns[1].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("CUST_PRICE")).Sum().ToString();
                            GrdaddplanConfrim.Columns[2].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("LCO_PRICE")).Sum().ToString();
                            GrdaddplanConfrim.Columns[3].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("discount")).Sum().ToString();
                            GrdaddplanConfrim.Columns[4].FooterText = TblPlanAddfinal.AsEnumerable().Select(x => x.Field<Double>("netmrp")).Sum().ToString();
                            GrdaddplanConfrim.DataSource = TblPlanAddfinal;
                            GrdaddplanConfrim.DataBind();


                            popaddplanconfirm.Show();
                        }
                    }
                }

                Session["DisplayAlreadyFOCPlan"] = null;
            }
            catch (Exception ex)
            {
                msgboxstr(ex.Message);
            }
        }

        public int getsepcialfoccount(String BaseplanName)
        {
            try
            {
                int sepcialfoccout = 0;
                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection con = new OracleConnection(strCon);
                con.Open();
                String Strdiscount = "select nvl(var_specialfoc_allow,0) FOCCOUNT" +
                             " from aoup_lcopre_packvalid_config " +
                                " where num_city_id=" + ViewState["cityid"] + " " +
                                " AND instr('" + BaseplanName + "',upper(var_pack_type))<>0 and var_plan_das_area='" + Session["dasarea"] + "' and var_plan_foctype='FOC1'";
                OracleCommand cmd1 = new OracleCommand(Strdiscount, con);
                OracleDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    sepcialfoccout = Convert.ToInt32(dr1["FOCCOUNT"].ToString());

                }

                if (!dr1.HasRows)
                {
                    sepcialfoccout = 0;
                    //lbldiscountamt.Text = "Not Configured";
                }
                con.Close();
                dr1.Dispose();

                return sepcialfoccout;
            }
            catch
            {
                return 0;
            }

        }

        protected void FreePlandataTableBuilder(DataTable dt, string[] arr_data)   // added by vivek 04-Jan-2016
        {
            string count = arr_data[0];

            if (ViewState["BasicPlanChangeWithFOC"] == null)
            {
                if (grdAddOnPlan.Rows.Count > 0)
                {
                    if (grdBasicPlanDetails.Rows.Count > 0)
                    {
                        foreach (GridViewRow grb in grdBasicPlanDetails.Rows)
                        {
                            string plannameb = "";
                            plannameb = grb.Cells[0].Text.ToString();

                            if (plannameb.Contains("BASIC"))
                            {
                                foreach (GridViewRow gr in grdAddOnPlan.Rows)
                                {
                                    string planname = "";
                                    planname = gr.Cells[0].Text.ToString();

                                    if (planname.Contains("SPECIAL"))
                                    {
                                        int sepciafoccount = getsepcialfoccount(grb.Cells[0].Text.ToString());
                                        count = (Convert.ToInt32(count) + sepciafoccount).ToString();

                                        break;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            ViewState["SpecialFOCCount"] = count;

            ViewState["plancount"] = count;
            string plan = arr_data[1];
            string[] planfetch = plan.Split('!');

            foreach (string plan_data in planfetch)
            {
                string[] plan_details_arr = plan_data.Split('$');

                string plan_poid = plan_details_arr[0];
                string plan_name = plan_details_arr[1];
                String language = plan_details_arr[2];

                DataRow tempDr = dt.NewRow();

                tempDr["PLAN_POID"] = plan_poid;
                tempDr["PLAN_NAME"] = plan_name;
                tempDr["language"] = language;
                dt.Rows.Add(tempDr);
            }

        }

        protected void lnkAddFOC_Click(object sender, EventArgs e)  //Added By Vivek 12-Feb-2016
        {
            nullFOCViewState();
            ViewState["foc_count"] = null;
            ViewState["BasciaddcancelFOC"] = null;
            ViewState["cancelfocpack"] = "Y";
            ViewState["BasicPlanChangeWithFOC"] = null;
            ViewState["Newplan_Poid"] = null;
            grdFreePlan.DataSource = null;
            grdFreePlan.DataBind();
            ViewState["TblPlanAddfinal"] = null;
            int indexno = (((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex;

            int rindex = (((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex;

            HiddenField hdnBasicPlanPoid = (HiddenField)grdBasicPlanDetails.Rows[rindex].FindControl("hdnBasicPlanPoid");

            fillFreePlanGrid(hdnBasicPlanPoid.Value.ToString(), "Y");

            if (grdFreePlan.Rows.Count > 0)
            {
                /*  for (int i = 0; i < grdFreePlan.Rows.Count; i++)
                  {
                      CheckBox chkFreeFOCExist = ((CheckBox)grdFreePlan.Rows[i].FindControl("cbFreePlan"));
                      if (chkFreeFOCExist.Checked == true)
                      {
                          string FOCID = ((HiddenField)grdFreePlan.Rows[i].FindControl("hdnFreePlanPoid")).Value;
                          CancelSelectedFOCPlan = FOCID + "," + CancelSelectedFOCPlan;
                      }
                  }*/

                ViewState["BasicPlanAlreadyThere"] = "Y";

                PopUpFreePlan.Show();
                StatbleDynamicTabs();

                return;
            }
            else
            {

                lblPopupResponse.Text = "No Eligible Free Regional Pack Found!";
                btnRefreshForm.Visible = false;
                popMsg.Show();
                StatbleDynamicTabs();

                return;
            }
        }

        protected void grdFreePlan_RowDataBound(object sender, GridViewRowEventArgs e)  // Added By Vivek 15-Feb-2016
        {
            string planexist, FreePlanPoid, FreePlanName, Flag = "";


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                FreePlanPoid = ((HiddenField)e.Row.FindControl("hdnFreePlanPoid")).Value;
                FreePlanName = e.Row.Cells[0].Text.ToString();

                CheckBox chk = ((CheckBox)e.Row.FindControl("cbFreePlan"));

                for (int i = 0; i < grdAddOnPlanReg.Rows.Count; i++)
                {
                    planexist = ((HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPlanPoid")).Value;
                    if (FreePlanPoid == planexist)
                    {
                        Flag = "Y";
                    }
                }

                if (Flag == "Y")
                {
                    chk.Checked = true;
                    CancelSelectedFOCPlan = FreePlanPoid + "," + CancelSelectedFOCPlan;
                }
                else
                {
                    chk.Checked = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                string PlanFreeAddonId = "";
                for (int k = 0; k < grdAddOnPlan.Rows.Count; k++)
                {
                    string PlanFreeAddon = ((HiddenField)grdAddOnPlan.Rows[k].FindControl("hdnADPlanName")).Value;
                    PlanFreeAddonId = ((HiddenField)grdAddOnPlan.Rows[k].FindControl("hdnADPlanPoid")).Value;
                    if (PlanFreeAddon.ToUpper().Contains("FREE") == true)
                    {
                        if (CancelSelectedFOCPlan.Contains(PlanFreeAddonId) == false)
                        {
                            CancelSelectedFOCPlan = PlanFreeAddonId + "," + CancelSelectedFOCPlan;
                        }
                    }
                }
            }
        }

        public void SendFOC_CancelRequest(String FOCPlanId) // Added By Vivek 15-Feb-2016
        {
            string value, PlanName = "";
            int i;
            for (i = 0; i < grdAddOnPlanReg.Rows.Count; i++)
            {
                value = ((HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPlanPoid")).Value.ToString();
                if (value == FOCPlanId)
                {
                    PlanName = ((HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPlanName")).Value.ToString();

                    Cls_Data_Auth auth = new Cls_Data_Auth();
                    string Ip = auth.GetIPAddress(HttpContext.Current.Request);

                    // lblchangediscount.Text = "";
                    // HiddenField hdnPlanId = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPlanId");
                    HiddenField hdnPlanName = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPlanName");
                    // HiddenField hdnPlanType = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPlanType");
                    HiddenField hdnPlanPoid = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPlanPoid");
                    HiddenField hdnDealPoid = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADDealPoid");
                    // HiddenField hdnProductPoid = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADProductPoid");
                    HiddenField hdnCustPrice = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADCustPrice");
                    HiddenField hdnLcoPrice = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADLcoPrice");
                    HiddenField hdnActivation = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADActivation");
                    HiddenField hdnExpiry = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADExpiry");
                    HiddenField hdnPackageId = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPackageId");
                    HiddenField hdnPurchasePoid = (HiddenField)grdAddOnPlanReg.Rows[i].FindControl("hdnADPurchasePoid");

                    //check box for Addon Autorenewal
                    CheckBox cbAddonAutorenew = (CheckBox)grdAddOnPlanReg.Rows[i].FindControl("cbAddonAutorenew");
                    Hashtable htData = new Hashtable();
                    //  htData["planid"] = hdnPlanId.Value;
                    htData["planname"] = hdnPlanName.Value;
                    // htData["plantype"] = hdnPlanType.Value;
                    htData["planpoid"] = hdnPlanPoid.Value;
                    htData["dealpoid"] = hdnDealPoid.Value;
                    htData["custprice"] = hdnCustPrice.Value;
                    htData["lcoprice"] = hdnLcoPrice.Value;
                    htData["activation"] = hdnActivation.Value;
                    htData["expiry"] = hdnExpiry.Value;
                    htData["packageid"] = hdnPackageId.Value;
                    htData["purchasepoid"] = hdnPurchasePoid.Value;
                    htData["IP"] = Ip;

                    if (cbAddonAutorenew.Checked)
                    {
                        htData["autorenew"] = "Y";
                    }
                    else
                    {
                        htData["autorenew"] = "N";
                    }
                    htData["plantypevalue"] = "GAD";
                    ViewState["transaction_data"] = htData;
                    break;
                }
            }

            ViewState["rowIndex"] = i;
            hdnPopupAction.Value = "C";

            processTransaction();

            DataRow row1 = dtFOCPlanStatus.NewRow();
            row1["PlanName"] = PlanName;
            row1["RenewStatus"] = ViewState["ErrorMessage"];
            dtFOCPlanStatus.Rows.Add(row1);

            DataRow row2 = dtPlanStatus.NewRow();

            row2["VCID"] = Convert.ToString(ViewState["VCFREE"]);
            row2["PlanName"] = PlanName;
            row2["Status"] = ViewState["ErrorMessage"].ToString();
            dtPlanStatus.Rows.Add(row2);

            popMsg.Hide();
        }

        private void StatbleDynamicTabs()
        {
            try
            {
                if ((DataTable)ViewState["vcdetail"] != null)
                {

                    ContentPlaceHolder cph1 = (ContentPlaceHolder)this.Master.FindControl("MasterBody");
                    LinkButton lnk1 = (LinkButton)cph1.FindControl("lnkAddon1");
                    if (lnk1 == null)
                    {
                        createDynamicTabs((DataTable)ViewState["vcdetail"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void nullFOCViewState()  //Added By Vivek 16-Feb-2016
        {
            ViewState["freeplanpoid"] = null;
            ViewState["freeplanname"] = null;
            ViewState["CancelSelectedFOCPlanId"] = null;
            ViewState["BasicPlanAlreadyThere"] = null;
            ViewState["ChangedBasicPlanFOCChange"] = null;
            ViewState["rowIndex"] = null;
            ViewState["SpecialFOCCount"] = null;
            ViewState["changec"] = null;
            ViewState["AddedFOC"] = null;
            ViewState["bucket1foc"] = "0";
            ViewState["bucket2foc"] = "0";
        }

        public void SendFOCCancelRequestOneByOne() // Added by Vivek 19-Feb-2016
        {
            if (ViewState["CancelSelectedFOCPlanId"] != null)
            {
                string[] CancelPlanPoid = ViewState["CancelSelectedFOCPlanId"].ToString().Split(',');
                for (int j = 0; j < CancelPlanPoid.Length; j++)
                {
                    if (CancelPlanPoid[j].ToString() != "")
                    {
                        SendFOC_CancelRequest(CancelPlanPoid[j].ToString());
                    }
                }
            }
            else
            {
                string PlanFreeAddonId = "";
                for (int k = 0; k < grdAddOnPlanReg.Rows.Count; k++)
                {
                    string PlanFreeAddon = ((HiddenField)grdAddOnPlanReg.Rows[k].FindControl("hdnADPlanName")).Value;
                    PlanFreeAddonId = ((HiddenField)grdAddOnPlanReg.Rows[k].FindControl("hdnADPlanPoid")).Value;
                    if (PlanFreeAddon.ToUpper().Contains("FREE") == true)
                    {
                        SendFOC_CancelRequest(PlanFreeAddonId.ToString());
                    }
                }
            }
        }


        protected void btnModifyCust_Click(object sender, EventArgs e)
        {
            string searhParam = lblCustNo.Text.Trim();
            string oper_id = "";
            string user_brmpoid = "";
            lblModifyError.Text = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
            {
                if (catid == "11")
                {
                    username = Convert.ToString(Session["lco_username"]);
                }
                else
                {
                    username = Convert.ToString(Session["username"]);
                }
                oper_id = Convert.ToString(Session["lcoid"]);
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            string response_params = user_brmpoid + "$" + searhParam + "$SW";


            string apiResponse = callAPI(response_params, "19");

            try
            {
                if (apiResponse != "")
                {
                    List<string> lstResponse = new List<string>();
                    lstResponse = apiResponse.Split('$').ToList();
                    string cust_id = lstResponse[0];
                    ViewState["customer_no"] = cust_id;
                    string cust_firstname = lstResponse[1];
                    ViewState["customer_firstname"] = cust_firstname;
                    string cust_lastname = lstResponse[2];
                    ViewState["customer_Lastname"] = cust_lastname;
                    string cust_Middlename = lstResponse[3];
                    ViewState["customer_Middlename"] = cust_Middlename;
                    string cust_addr = lstResponse[4];
                    ViewState["ModifyAddress"] = cust_addr;
                    string cust_Mobile = lstResponse[10];
                    ViewState["ModifyMobile"] = cust_Mobile;
                    string cust_Email = lstResponse[9];
                    ViewState["ModifyEmail"] = cust_Email;



                    ViewState["ModifyCity"] = lstResponse[5];
                    ViewState["ModifyState"] = lstResponse[6];
                    ViewState["ModifyAreaName"] = lstResponse[7];
                    ViewState["ModifyArea"] = lstResponse[8];

                    ViewState["ModifyBuilding"] = lstResponse[11];
                    ViewState["ModifyLocation"] = lstResponse[12];
                    ViewState["ModifyStreet"] = lstResponse[13];
                    ViewState["ModifyAccess"] = lstResponse[14];
                    ViewState["ModifySerialNumber"] = lstResponse[15];
                    ViewState["ModifyaccountObj"] = lstResponse[16];
                    ViewState["ModifyDeliveryPre"] = lstResponse[17];
                    ViewState["ModifyPayingObj"] = lstResponse[18];

                    //---Service IDs------------

                    string accountPoid = lstResponse[19];
                    ViewState["ModifyPoid"] = accountPoid;
                    string cust_services = lstResponse[20];
                    string[] service_arr = cust_services.Split('^');
                    ViewState["ModifyServicePoid"] = service_arr[0].Split('!')[0];
                    ViewState["ModifyZip"] = lstResponse[21];

                    ViewState["ModifyAddress1"] = lstResponse[22];
                    ViewState["MANUFACTURER"] = lstResponse[23];


                    //Session["user_brmpoid"]

                    //-------------------------
                    txtModifyFirstName.Text = cust_firstname;
                    txtModifylastName.Text = cust_lastname;
                    txtModifyMiddleName.Text = cust_Middlename;
                    txtModifymobile.Text = cust_Mobile;
                    txtModifyEmail.Text = cust_Email;
                    txtmodifyAddress.Text = cust_addr;
                    string str = "";

                    str = "select SUBSTR(city_code,0,2)||'|'||city_code||'|'||city_code areacode  from reports.geo_master@caslive";
                    str += " where area_name='" + lstResponse[7] + "' and city_name='" + lstResponse[5] + "' and state_name='" + lstResponse[6] + "'";
                    str += " and location_name ='" + lstResponse[12] + "' and street_name='" + lstResponse[13] + "' and rownum=1";
                    if (ViewState["ModifyArea"].ToString().Length < 3)
                    {
                        try
                        {
                            ViewState["ModifyArea"] = Cls_Helper.fnGetScalar(str);
                        }
                        catch (Exception)
                        {
                            ViewState["ModifyArea"] = "NA";
                        }
                    }


                }
            }
            catch (Exception ex)
            {
            }
            popupModifyCust.Show();
            StatbleDynamicTabs();
        }
        protected void rdbplanpayterm_SelectedIndexChanged(object sender, EventArgs e)
        {

            ViewState["planpayterm"] = rdbplanpayterm.SelectedValue.ToString();
            if (radPlanBasic.Checked)
            {
                radPlanBasic_CheckedChanged(radPlanBasic, new EventArgs());
            }
            if (radPlanAD.Checked)
            {
                radPlanAD_CheckedChanged(radPlanAD, new EventArgs());
            }
            if (radPlanADreg.Checked)
            {
                radPlanAD_CheckedChanged(radPlanADreg, new EventArgs());
            }
            if (radPlanAL.Checked)
            {
                radPlanAL_CheckedChanged(radPlanAL, new EventArgs());
            }
            if (radhwayspecial.Checked)
            {
                radhwayspecial_CheckedChanged(radhwayspecial, new EventArgs());
            }
            popAdd.Show();
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string blusername = SecurityValidation.chkData("T", txtModifyFirstName.Text + " " + txtModifyMiddleName.Text + " " + txtModifylastName.Text + " " + txtModifyEmail.Text + " " + txtmodifyAddress.Text);
            if (blusername.Length > 0)
            {
                lblModifyError.Text = blusername;
                popupModifyCust.Show();
                StatbleDynamicTabs();
                return;
            }

            blusername = SecurityValidation.chkData("N", txtModifymobile.Text);
            if (blusername.Length > 0)
            {
                lblModifyError.Text = blusername;
                popupModifyCust.Show();
                StatbleDynamicTabs();
                return;
            }

            lblModifyError.Text = "";
            if (ViewState["ModifyArea"].ToString() == "NA")
            {
                lblModifyError.Text = "Area code not found";
                popupModifyCust.Show();
                StatbleDynamicTabs();
                return;
            }

            string Address, FristName, MiddleName, lastname, txtmob, txtEmail = "";
            string oldAddress, oldFristName, oldMiddleName, oldlastname, oldtxtmob, oldtxtEmail = "";
            Address = txtmodifyAddress.Text.Trim();
            FristName = txtModifyFirstName.Text.Trim();
            MiddleName = txtModifyMiddleName.Text.Trim();
            lastname = txtModifylastName.Text.Trim();
            txtmob = txtModifymobile.Text.Trim();
            txtEmail = txtModifyEmail.Text.Trim();

            /*
            ViewState["customer_firstname"];
            ViewState["customer_Lastname"];
            ViewState["customer_Middlename"];
            ViewState["ModifyAddress"];
            ViewState["ModifyMobile"];
            ViewState["ModifyEmail"];
            */

            oldAddress = Convert.ToString(ViewState["ModifyAddress"]);
            oldFristName = Convert.ToString(ViewState["customer_firstname"]);
            oldMiddleName = Convert.ToString(ViewState["customer_Middlename"]);
            oldlastname = Convert.ToString(ViewState["customer_Lastname"]);
            oldtxtmob = Convert.ToString(ViewState["ModifyMobile"]);
            oldtxtEmail = Convert.ToString(ViewState["ModifyEmail"]);

            string obrm_status = "";
            string responsemodify_params = Session["user_brmpoid"].ToString() + "$" + ViewState["customer_no"].ToString() + "$" + ViewState["ModifyPoid"].ToString() + "$" + ViewState["ModifyServicePoid"].ToString() + "$" + Address + "$" + FristName + "$" + lastname + "$" + txtmob;
            responsemodify_params += "$" + ViewState["ModifyZip"].ToString() + "$" + ViewState["ModifyCity"].ToString() + "$" + ViewState["ModifyStreet"].ToString() + "$" + ViewState["ModifyAreaName"].ToString() + "$" + ViewState["ModifyBuilding"].ToString() + "$" + ViewState["ModifyLocation"].ToString();
            responsemodify_params += "$" + txtEmail + "$" + ViewState["ModifyAccess"].ToString() + "$" + ViewState["ModifySerialNumber"].ToString() + "$" + ViewState["ModifyDeliveryPre"].ToString() + "$" + ViewState["ModifyPayingObj"].ToString();
            responsemodify_params += "$" + ViewState["ModifyArea"].ToString() + "$" + ViewState["ModifyState"].ToString() + "$" + MiddleName + "$" + ViewState["ModifyAddress1"].ToString() + "$" + ViewState["MANUFACTURER"].ToString();
            try
            {
                string account = ViewState["customer_no"].ToString();
                string term = "N";
                string lcocode = Convert.ToString(Session["lcoid"]);
                if (cboCheckModify.Checked)
                {
                    term = "Y";
                }
                Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                string conResult = obj.saveModifiesCust(username, account, FristName, MiddleName, lastname, txtEmail, txtmob, Address, term, lcocode, oldFristName, oldMiddleName, oldlastname, oldtxtEmail, oldtxtmob, oldAddress);
                string[] result_arr = conResult.Split('#');

                if (result_arr[0] == "9999")
                {
                    string modifyapi_resp = callAPI(responsemodify_params, "20");
                    //FileLogText("Request from obrm", username, responsemodify_params, "");
                    string[] final_obrm_status = modifyapi_resp.Split('$');
                    obrm_status = final_obrm_status[1];
                    string obrm_msg = final_obrm_status[2];



                    if (obrm_status == "0")
                    {
                        btnClodeMsg.Value = "OK";
                        msgboxstr("Customer detail Modified successfully!");
                        btnSearch_Click(null, null);
                        btnRefreshForm.Visible = false;

                    }
                    else
                    {

                    }
                }
                else
                {
                    btnClodeMsg.Value = "OK";
                    msgboxstr(result_arr[1]);
                    btnSearch_Click(null, null);
                    btnRefreshForm.Visible = false;
                }

            }
            catch (Exception ex)
            {

            }



        }


        protected void btnAutoRenew_Click(object sender, EventArgs e)
        {
            /*try
            {
                string strPlanPoid = "";
                foreach (GridViewRow gvrow in grdAutoRenewal.Rows)
                {

                    HiddenField hdnALPlanPoid = (HiddenField)gvrow.FindControl("hdnPlanPoid");
                    if (((CheckBox)gvrow.FindControl("chkAutoRenew")).Checked)
                    {

                        if (strPlanPoid == "")
                        {
                            strPlanPoid = hdnALPlanPoid.Value;
                        }
                        else
                        {
                            strPlanPoid = strPlanPoid + "," + hdnALPlanPoid.Value;
                        }
                    }

                }
                processTransactionAutoRenew(strPlanPoid);
            }
            catch (Exception ex)
            {

            }
            StatbleDynamicTabs();
            lnkatag_Click(null, null);*/
            int count2 = 0;

            int countpre = Convert.ToInt32(ViewState["countpre"]);
            foreach (GridViewRow gvrow in grdAutoRenewal.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkAutoRenew");
                if (countpre == 0)
                {
                    if (chk != null & chk.Checked)
                    {
                        HiddenField hdnAutoStatus = (HiddenField)gvrow.FindControl("hdnAutoStatus");
                        if (hdnAutoStatus.Value == "0")
                        {
                            ((CheckBox)gvrow.FindControl("chkAutoRenew")).Checked = true;
                            count2++;
                        }
                    }
                }
                else
                {
                    if (chk != null & chk.Checked)
                    {
                        HiddenField hdnAutoStatus = (HiddenField)gvrow.FindControl("hdnAutoStatus");
                        if (hdnAutoStatus.Value == "1")
                        {
                            ((CheckBox)gvrow.FindControl("chkAutoRenew")).Checked = true;
                            count2++;
                        }
                    }
                }

            }

            if (count2 >= countpre)
            {
                Lblautorenewconfirm.Text = "This will add selected Plans for Autorenewal";
            }
            if (count2 == 0)
            {
                Lblautorenewconfirm.Text = "This will exclude plans for Autorenewal";
            }
            popupautorenewalconfirm.Show();
        }
        protected void btnautorenewcancel_Click(object sender, EventArgs e)
        {
            popupautorenewalconfirm.Hide();
            choAutorenewAll.Checked = false;
        }

        protected void processTransactionAutoRenew(string plan_poid, string Expdate, string Status)
        {
            try
            {
                ViewState["ErrorMessage"] = null;
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData
                    = new Hashtable();
                string transaction_action = "R";
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                if (Session["username"] != null && Session["lcoid"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["lcoid"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }

                DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();

                _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();



                ht.Add("username", _username);
                ht.Add("lcoid", _oper_id);
                ht.Add("custid", lblCustNo.Text.Trim());
                ht.Add("vcid", _vc_id);
                ht.Add("custname", lblCustName.Text.Trim());
                ht.Add("cust_addr", lblCustAddr.Text.Trim());
                ht.Add("planid", plan_poid);
                ht.Add("flag", transaction_action);
                ht.Add("request", Request);
                ht.Add("reason_id", reason_id);
                ht.Add("IP", Ip);
                Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();

                ht.Add("obrmsts", "0");
                ht.Add("request_id", request_id);
                ht.Add("response", "");

                try
                {
                    string autorenew_flag = hdnPopupAutoRenew.Value;
                    ht.Add("autorenew", "Y");
                    /*string Expdate = "";

                    List<string> lstpoid = new List<string>();
                    lstpoid = plan_poid.Split(',').ToList();

                    foreach (string poid in lstpoid)
                    {
                        if (poid != "")
                        {
                            string response_params = _user_brmpoid + "$" + lblCustNo.Text + "$SW$" + _vc_id + "$" + poid;
                            string strexp = getAutorenewExpiry(response_params);

                            if (Expdate == "")
                            {
                                Expdate = strexp;
                                //activation_date = apiResponse.Split('$')[4];
                            }
                            else
                            {
                                Expdate = Expdate + "," + strexp;
                                //activation_date = activation_date + "," + apiResponse.Split('$')[4];
                            }
                        }
                    }*/

                    ht.Add("expdate", Expdate);
                    ht.Add("Status", Status);
                    ht.Add("actidate", activation_date);
                    string resp1 = obj.ProvECS(ht);
                    //FileLogText("ECS-!C", username, resp1, "");//

                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
            }

        }
        protected void btnAutoRenewal_Click(object sender, EventArgs e)
        {
            DataTable dtPlan = new DataTable();
            dtPlan.Columns.Add("PlanName");
            dtPlan.Columns.Add("RenewStatus");
            dtPlan.Columns.Add("PLAN_POID");
            dtPlan.Columns.Add("EXPIRY");
            foreach (GridViewRow gvrow in grdBasicPlanDetails.Rows)
            {
                string strrenewalstatus = "0";
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnBasicPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnBasicPlanPoid");
                CheckBox chk = (CheckBox)gvrow.FindControl("cbBAutorenew");
                HiddenField hdnBasicExpiry = (HiddenField)gvrow.FindControl("hdnBasicExpiry");
                HiddenField hdnBasicPlanType = (HiddenField)gvrow.FindControl("hdnBasicPlanType");
                HiddenField hdnBasicActivation = (HiddenField)gvrow.FindControl("hdnBasicActivation");

                if (chk.Checked)
                {
                    strrenewalstatus = "1";
                }
                DataRow dr = dtPlan.NewRow();
                dr["PlanName"] = hdnBasicPlanName.Value;
                dr["RenewStatus"] = strrenewalstatus;
                dr["PLAN_POID"] = hdnBasicPlanPoid.Value;
                dr["EXPIRY"] = hdnBasicExpiry.Value;
                if (hdnBasicPlanType.Value == "B")
                {
                    if (hdnBasicActivation.Value.ToString().ToUpper() == "ACTIVE")
                    {
                        dtPlan.Rows.Add(dr);
                    }
                }
            }
            foreach (GridViewRow gvrow in Grdhathwayspecial.Rows)
            {
                string strrenewalstatus = "0";
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                CheckBox chk = (CheckBox)gvrow.FindControl("cbAddonAutorenew");
                HiddenField hdnADExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                HiddenField hdnADActivation = (HiddenField)gvrow.FindControl("hdnADActivation");
                if (chk.Checked)
                {
                    strrenewalstatus = "1";
                }
                DataRow dr = dtPlan.NewRow();
                dr["PlanName"] = hdnBasicPlanName.Value;
                dr["RenewStatus"] = strrenewalstatus;
                dr["PLAN_POID"] = hdnBasicPlanPoid.Value;
                dr["EXPIRY"] = hdnADExpiry.Value;
                if (hdnADActivation.Value.ToString().ToUpper() == "ACTIVE")
                {
                    dtPlan.Rows.Add(dr);
                }
            }
            foreach (GridViewRow gvrow in grdAddOnPlan.Rows)
            {
                string strrenewalstatus = "0";
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                CheckBox chk = (CheckBox)gvrow.FindControl("cbAddonAutorenew");
                HiddenField hdnADExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                if (chk.Checked)
                {
                    strrenewalstatus = "1";
                }
                DataRow dr = dtPlan.NewRow();
                dr["PlanName"] = hdnBasicPlanName.Value;
                dr["RenewStatus"] = strrenewalstatus;
                dr["PLAN_POID"] = hdnBasicPlanPoid.Value;
                dr["EXPIRY"] = hdnADExpiry.Value;
                dtPlan.Rows.Add(dr);

            }

            foreach (GridViewRow gvrow in grdAddOnPlanReg.Rows)
            {
                string strrenewalstatus = "0";
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                CheckBox chk = (CheckBox)gvrow.FindControl("cbAddonAutorenew");
                HiddenField hdnADExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                if (chk.Checked)
                {
                    strrenewalstatus = "1";
                }
                DataRow dr = dtPlan.NewRow();
                dr["PlanName"] = hdnBasicPlanName.Value;
                dr["RenewStatus"] = strrenewalstatus;
                dr["PLAN_POID"] = hdnBasicPlanPoid.Value;
                dr["EXPIRY"] = hdnADExpiry.Value;
                dtPlan.Rows.Add(dr);

            }

            foreach (GridViewRow gvrow in grdCarte.Rows)
            {
                string strrenewalstatus = "0";
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnALPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnALPlanPoid");
                CheckBox chk = (CheckBox)gvrow.FindControl("cbAlaAutorenew");
                HiddenField hdnALExpiry = (HiddenField)gvrow.FindControl("hdnALExpiry");
                if (chk.Checked)
                {
                    strrenewalstatus = "1";
                }
                DataRow dr = dtPlan.NewRow();
                dr["PlanName"] = hdnBasicPlanName.Value;
                dr["RenewStatus"] = strrenewalstatus;
                dr["PLAN_POID"] = hdnBasicPlanPoid.Value;
                dr["EXPIRY"] = hdnALExpiry.Value;
                dtPlan.Rows.Add(dr);

            }

            grdAutoRenewal.DataSource = dtPlan;
            grdAutoRenewal.DataBind();
            int count1 = 0;
            foreach (GridViewRow gvrow in grdAutoRenewal.Rows)
            {

                HiddenField hdnAutoStatus = (HiddenField)gvrow.FindControl("hdnAutoStatus");
                if (hdnAutoStatus.Value == "1")
                {
                    ((CheckBox)gvrow.FindControl("chkAutoRenew")).Checked = true;
                    ViewState["countpre"] = count1++;
                }

            }

            if (grdAutoRenewal.Rows.Count == count1)
            {
                choAutorenewAll.Checked = true;
            }
            else
            {
                choAutorenewAll.Checked = false;
            }
            popAutoRenewal.Show();
            StatbleDynamicTabs();
        }
        protected void Btnautorenewsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string strPlanPoid = "";
                string strExpiry = "";
                string strStatus = "";
                foreach (GridViewRow gvrow in grdAutoRenewal.Rows)
                {

                    HiddenField hdnALPlanPoid = (HiddenField)gvrow.FindControl("hdnPlanPoid");
                    HiddenField hdnExpiry = (HiddenField)gvrow.FindControl("hdnExpiry");

                    if (((CheckBox)gvrow.FindControl("chkAutoRenew")).Checked)
                    {

                        if (strStatus == "")
                        {
                            strStatus = "Y";
                        }
                        else
                        {
                            strStatus = strStatus + "," + "Y";
                        }
                    }
                    else
                    {
                        if (strStatus == "")
                        {
                            strStatus = "N";

                        }
                        else
                        {
                            strStatus = strStatus + "," + "N";
                        }
                    }
                    if (strPlanPoid == "")
                    {
                        strExpiry = hdnExpiry.Value;
                        strPlanPoid = hdnALPlanPoid.Value;
                    }
                    else
                    {
                        strExpiry = strExpiry + "," + hdnExpiry.Value;
                        strPlanPoid = strPlanPoid + "," + hdnALPlanPoid.Value;
                    }

                }
                processTransactionAutoRenew(strPlanPoid, strExpiry, strStatus);
            }
            catch (Exception ex)
            {

            }
            StatbleDynamicTabs();
            lnkatag_Click(null, null);
        }

        private string getAutorenewExpiry(string apistring)
        {
            string expiry = "";
            string apiResponse = callAPI(apistring, "14");

            try
            {
                expiry = apiResponse.Split('$')[3];
            }
            catch (Exception ex)
            {
                getAutorenewExpiry(apistring);
            }
            return expiry;

        }

        public void Cancellation_Process(string Planpoid, string CancelTvType, string planname, string childvc_check) //Added by Vivek Singh on 12-Jul-2016
        {
            // dtFOCPlanStatus.Clear();
            dtPlanStatus.Clear();
            int Error_break = 0;

            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();

            if (ViewState["Service_Str"] != null)
            {
                string cust_services = ViewState["Service_Str"].ToString();
                string[] service_arr = cust_services.Split('^');
                int k = 0;

                int a = service_arr.Length; //3

                string[] updated_service_arr = new string[a]; //3
                foreach (string SerivceIndexs in service_arr)
                {

                    string Tv_Type_check = SerivceIndexs.Split('!')[6];
                    if (Tv_Type_check == "0")
                    {
                        updated_service_arr[a - 1] = SerivceIndexs;
                    }
                    else
                    {
                        updated_service_arr[k] = SerivceIndexs;
                        k = k + 1;
                    }
                }

                /* foreach (string service in service_arr)
                 {*/

                foreach (string service in updated_service_arr)
                {
                    if (Error_break > 0)
                    {
                        break;
                    }
                    string service_poid = service.Split('!')[0];
                    string vc = service.Split('!')[2];
                    string stb_status = service.Split('!')[4];
                    string Device_Type = service.Split('!')[5];
                    string Tv_Type = service.Split('!')[6];

                    //if (planname.Contains("SPECIAL") == false)
                    //{

                    if (CancelTvType == "CHILD")
                    {
                        if (Tv_Type == "0")
                        {
                            continue;
                        }
                        else if (Tv_Type == "1")
                        {
                            if (childvc_check != vc)
                            {
                                continue;
                            }
                        }
                    }

                    // }

                    //---
                    if (childvc_check == vc)
                    {
                        string all_plan_string = service.Split('!')[3];
                        string service_data = "";
                        if (planname.Contains("SPECIAL") == false)
                        {
                            service_data = obj.getvcwise_planstr(Session["lco_username"].ToString(), all_plan_string, Planpoid);
                        }
                        else
                        {
                            service_data = obj.getvcwise_spl_planstr(Session["lco_username"].ToString(), all_plan_string);
                        }
                        string[] service_data_arr = service_data.Split('#');
                        if (service_data_arr[0] != "9999")
                        {
                            continue;
                        }
                        else if (service_data_arr[0] == "9999" && service_data_arr[1].Replace("~", "") != "")
                        {

                            string data_str = service_data_arr[1];

                            string[] data_arr = data_str.Split('~');
                            foreach (string plan_data in data_arr)
                            {
                                if (plan_data == "")
                                {
                                    continue;
                                }
                                Tv_Type = service.Split('!')[6];
                                string[] plan_details_arr = plan_data.Split('$');

                                Hashtable htData = new Hashtable();
                                if (plan_details_arr[9].ToString() == "Active")
                                {
                                    htData["planname"] = plan_details_arr[1].ToString();
                                    htData["planpoid"] = plan_details_arr[0].ToString();
                                    htData["dealpoid"] = plan_details_arr[6].ToString();
                                    htData["custprice"] = plan_details_arr[2].ToString();
                                    htData["lcoprice"] = plan_details_arr[3].ToString();
                                    htData["activation"] = plan_details_arr[4].ToString();
                                    htData["expiry"] = plan_details_arr[5].ToString();
                                    htData["packageid"] = plan_details_arr[7].ToString();
                                    htData["purchasepoid"] = plan_details_arr[8].ToString();
                                    htData["IP"] = "";
                                    htData["autorenew"] = "N";
                                    string conResult = callGetProviConfirm_CancellationProcess(htData, "C");
                                    string[] result_arr = conResult.Split('#');
                                    if (result_arr[0].ToString() != "9999")
                                    {
                                        /* DataRow row1 = dtFOCPlanStatus.NewRow();
                                         row1["PlanName"] = htData["planname"].ToString();
                                         row1["RenewStatus"] = result_arr[1].ToString();
                                         dtFOCPlanStatus.Rows.Add(row1);*/

                                        DataRow row1 = dtPlanStatus.NewRow();
                                        row1["VCID"] = vc;
                                        row1["PlanName"] = htData["planname"].ToString();
                                        row1["Status"] = result_arr[1].ToString();
                                        dtPlanStatus.Rows.Add(row1);

                                        if (CancelTvType == "MAIN")
                                        {
                                            if (Tv_Type == "1")
                                            {
                                                Error_break = Error_break + 1;
                                                break;
                                            }
                                        }
                                        continue;
                                    }
                                    else
                                    {
                                        string[] conResult_arr = result_arr[1].Split('$');
                                        htData["refund_amt"] = conResult_arr[1];
                                        htData["days_left"] = conResult_arr[0];
                                        ViewState["transaction_data"] = htData;
                                        hdnPopupAction.Value = "C";
                                        if (Tv_Type.ToString() == "0")
                                        {
                                            Tv_Type = "MAIN";

                                        }
                                        else
                                        {
                                            Tv_Type = "CHILD";
                                        }
                                        processTransaction_cancellation(vc, Tv_Type, stb_status, service_poid);

                                        /* DataRow row2 = dtFOCPlanStatus.NewRow();
                                        row2["PlanName"] = htData["planname"].ToString();
                                        row2["RenewStatus"] = ViewState["ErrorMessage"];
                                        dtFOCPlanStatus.Rows.Add(row2);*/

                                        DataRow row2 = dtPlanStatus.NewRow();
                                        ViewState["VCFREE"] = vc;
                                        row2["VCID"] = vc;
                                        row2["PlanName"] = htData["planname"].ToString();
                                        row2["Status"] = ViewState["ErrorMessage"].ToString();
                                        dtPlanStatus.Rows.Add(row2);
                                    }

                                    if (CancelTvType == "MAIN")
                                    {
                                        if (Tv_Type == "CHILD")
                                        {
                                            if (ViewState["ErrorMessage"].ToString().Contains("Transaction successful :") == false)
                                            {
                                                Error_break = Error_break + 1;
                                                break;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            /*   Gridrenew.DataSource = dtFOCPlanStatus;
               Gridrenew.DataBind();
               lblAllStatus.Text = "Pack Status";
               Gridrenew.HeaderRow.Cells[1].Text = "Status";
               popMsg.Hide();
               popallrenewal.Show();*/

            grdAllCancel.DataSource = dtPlanStatus;
            grdAllCancel.DataBind();
            popMsg.Hide();
            popallCancel.Show();
            StatbleDynamicTabs();
        }

        protected string callGetProviConfirm_CancellationProcess(Hashtable htData, string flag) //Added by Vivek Singh on 12-Jul-2016
        {
            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
            Hashtable ht = new Hashtable();
            ht["username"] = username;
            ht["lco_id"] = oper_id;
            ht["cust_no"] = lblCustNo.Text;
            ht["vc_id"] = hdnVCNo.Value;
            ht["cust_name"] = lblCustName.Text;
            ht["plan_id"] = htData["planpoid"];
            ht["flag"] = flag;
            ht["IP"] = htData["IP"];
            if (htData["expiry"] != null)
            {
                ht["expiry"] = htData["expiry"];
            }
            else
            {
                ht["expiry"] = "";
            }
            if (htData["activation"] != null)
            {
                ht["activation"] = htData["activation"];
            }
            else
            {
                ht["activation"] = ""; // add plan
            }

            ht["existing_poids"] = htData["existing_poids"];

            string result = obj.getProviConfirm(ht);


            return result.ToString();

        }

        protected void processTransaction_cancellation(string Vcid, string Tv_Type, string Status, string Servicepoid)  //Added by Vivek Singh on 12-Jul-2016
        {
            try
            {
                ViewState["ErrorMessage"] = null;
                //gathering data
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string transaction_action = "C";


                string plan_poid = "";
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";

                string maintStatus = "INACTIVE";
                if (Session["username"] != null && Session["lcoid"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["lcoid"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }

                _vc_id = Vcid;

                _tvType = Tv_Type;

                if (Status == "10100")
                {
                    maintStatus = "ACTIVE";
                }
                else
                {
                    maintStatus = "INACTIVE";
                }

                ViewState["AddedFOC"] = "0";

                if (ViewState["transaction_data"] != null)
                {
                    htPlanData = ViewState["transaction_data"] as Hashtable;
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";

                    return;
                }

                //processing 
                string Request = "";


                plan_poid = htPlanData["planpoid"].ToString();
                activation_date = htPlanData["activation"].ToString();
                expiry_date = htPlanData["expiry"].ToString();
                if (transaction_action == "C")
                {
                    reason_id = ddlPopupReason.SelectedValue;
                    reason_text = ddlPopupReason.SelectedItem.Text;
                }
                //------------
                Cls_Business_TxnAssignPlan obj3 = new Cls_Business_TxnAssignPlan();
                string responseCount = obj3.ChannelCount(_username, plan_poid);
                string ChannelCount = responseCount.Split('~')[2];
                //--------------


                if (ViewState["ServicePoid"] != null && ViewState["accountPoid"] != null)
                {

                    ht.Add("username", _username);
                    ht.Add("lcoid", _oper_id);
                    ht.Add("custid", lblCustNo.Text.Trim());
                    ht.Add("vcid", _vc_id);
                    ht.Add("custname", lblCustName.Text.Trim());
                    ht.Add("cust_addr", lblCustAddr.Text.Trim());
                    ht.Add("planid", plan_poid);
                    ht.Add("flag", transaction_action);
                    ht.Add("expdate", expiry_date);
                    ht.Add("actidate", activation_date);
                    ht.Add("request", Request);
                    ht.Add("reason_id", reason_id);
                    ht.Add("IP", Ip);

                    ht.Add("MainTVStatus", maintStatus);
                    ht.Add("TVType", _tvType);
                    ht.Add("DeviceType", ViewState["Device_Type"].ToString());
                    ht.Add("FOCCount", 0);
                    ht.Add("BasicPoid", ViewState["basic_poids"].ToString());
                    ht.Add("addon_poids", Convert.ToString(ViewState["addon_poids"]).Replace("'", ""));
                    ht.Add("bucket1foc", "0");
                    ht.Add("bucket2foc", "0");
                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    string response = obj.ValidateProvTrans(ht);

                    string[] res = response.Split('$');
                    if (res[0] != "9999")
                    {

                        if (transaction_action == "C")
                        {

                            if (Convert.ToString(htPlanData["planname"]).Contains("FREE"))
                            {
                                ht.Add("mrp", 1);
                                response = obj.ValidateProvTransFoc2(ht);
                                res = response.Split('$');
                                if (res[0] != "9999")
                                {
                                    ViewState["ErrorMessage"] = res[1].ToString();
                                    return;
                                }
                                else
                                {
                                    if (res[1] == "A")
                                    {
                                        transaction_action = "A";
                                    }
                                    else
                                    {
                                        request_id = res[1];
                                    }
                                }
                            }
                            else
                            {
                                ViewState["ErrorMessage"] = res[1].ToString();
                                return;
                            }
                        }
                        else
                        {
                            ViewState["ErrorMessage"] = res[1].ToString();
                            return;
                        }
                    }
                    else
                    {
                        if (res[1] == "A")
                        {
                            transaction_action = "A";
                        }
                        else
                        {
                            request_id = res[1];
                        }
                    }
                }
                else
                {
                    ViewState["ErrorMessage"] = "something went wrong, Service or account details not found...Please relogin";

                    return;
                }

                if (transaction_action == "C")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + Servicepoid.ToString() + "$" + plan_poid + "$" + htPlanData["packageid"] + "$" + htPlanData["dealpoid"] + "$" + ChannelCount;
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction.";
                    return;
                }
                Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();

                Request = _user_brmpoid + "$" + Request;

                //string resp1 = obj1.Cust_Trans_B4OBRM(ht);

                string api_response = callAPI(Request, "8");
                //string api_response = "0$ACCOUNT - Service add plan completed successfully$0.0.0.1 /account 81788441 9$0.0.0.1 /service/catv 81788185 39";
                string[] final_obrm_status = api_response.Split('$');
                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";

                try
                {
                    if (obrm_status == "0" || obrm_status == "1")
                    {
                        obrm_msg = final_obrm_status[2];
                    }
                    else
                    {
                        obrm_status = "1";
                        obrm_msg = api_response;
                    }
                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    ViewState["ErrorMessage"] = api_response;
                    obrm_msg = api_response;
                }

                ht.Add("obrmsts", obrm_status);
                ht.Add("request_id", request_id);
                ht.Add("response", api_response);


                string resp = obj1.ProvTransRes(ht); // "9999";
                string[] finalres = resp.Split('$');
                if (finalres[0] == "9999")
                {
                    string autorenew_flag = hdnPopupAutoRenew.Value;
                    ht.Add("autorenew", "N");


                    if (transaction_action != "C")
                    {
                        ht.Remove("expdate");
                        ht.Add("expdate", expiry_date);
                        string resp1 = obj1.ProvECSSingle(ht);
                    }
                    else if (transaction_action == "C")
                    {
                        string resp1 = obj1.ProvECSSingle(ht);
                    }

                    ViewState["ErrorMessage"] = "Transaction successful : " + obrm_msg;
                    ViewState["renewfoc"] = "Y";

                    if (transaction_action == "C" && Convert.ToString(ViewState["changec"]) != "CH")
                    {
                        foreach (GridViewRow gr in grdBasicPlanDetails.Rows)
                        {
                            HiddenField hdnBasicPlanName = (HiddenField)gr.FindControl("hdnBasicPlanName");
                            if (hdnBasicPlanName.Value.Contains("BASIC"))
                            {
                                if (htPlanData["planname"].ToString().Contains("SPECIAL"))
                                {
                                    foreach (GridViewRow gr1 in grdAddOnPlanReg.Rows)
                                    {
                                        HiddenField hdnADPlanName = (HiddenField)gr1.FindControl("hdnADPlanName");
                                        HiddenField hdnADPlanPoid = (HiddenField)gr1.FindControl("hdnADPlanPoid");
                                        if (hdnADPlanName.Value.Contains("FREE"))
                                        {
                                            SendFOC_CancelRequest(hdnADPlanPoid.Value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (obrm_status == "0")
                    {
                        ViewState["ErrorMessage"] = "Transaction successful by OBRM but failure at Atyeti : " + finalres[1];
                        ViewState["renewfoc"] = "Y";
                    }
                    else
                    {
                        ViewState["ErrorMessage"] = "Transaction failed : " + finalres[1] + " : " + obrm_msg;
                        ViewState["renewfoc"] = "N";
                    }
                }

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                ViewState["ErrorMessage"] = ex.Message.ToString();
                objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-processTransaction_cancellation");
                return;
            }
        }

        public void Cancellation_ProcessSpecial(string Planpoid, string CancelTvType, string planname, string childvc_check) //Added by Pawan Yadav on 21-Jul-2016
        {
            // dtFOCPlanStatus.Clear();
            int Error_break = 0;

            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();

            if (ViewState["Service_Str"] != null)
            {
                string cust_services = ViewState["Service_Str"].ToString();
                string[] service_arr = cust_services.Split('^');
                int k = 0;

                int a = service_arr.Length; //3

                string[] updated_service_arr = new string[a]; //3
                foreach (string SerivceIndexs in service_arr)
                {

                    string Tv_Type_check = SerivceIndexs.Split('!')[6];
                    if (Tv_Type_check == "0")
                    {
                        updated_service_arr[a - 1] = SerivceIndexs;
                    }
                    else
                    {
                        updated_service_arr[k] = SerivceIndexs;
                        k = k + 1;
                    }
                }

                /* foreach (string service in service_arr)
                 {*/

                foreach (string service in updated_service_arr)
                {
                    if (Error_break > 0)
                    {
                        break;
                    }
                    string service_poid = service.Split('!')[0];
                    string vc = service.Split('!')[2];
                    string stb_status = service.Split('!')[4];
                    string Device_Type = service.Split('!')[5];
                    string Tv_Type = service.Split('!')[6];

                    //if (planname.Contains("SPECIAL") == false)
                    //{


                    if (CancelTvType == "SPECIAL")
                    {

                        if (childvc_check != vc)
                        {
                            continue;
                        }
                    }

                    // }

                    string all_plan_string = service.Split('!')[3];
                    string service_data = "";
                    if (planname.Contains("SPECIAL") == true)
                    {

                        service_data = obj.getvcwise_spl_planstr(Session["username"].ToString(), all_plan_string);
                    }
                    string[] service_data_arr = service_data.Split('#');
                    if (service_data_arr[0] != "9999")
                    {
                        continue;
                    }
                    else if (service_data_arr[0] == "9999" && service_data_arr[1].Replace("~", "") != "")
                    {

                        string data_str = service_data_arr[1];

                        string[] data_arr = data_str.Split('~');
                        foreach (string plan_data in data_arr)
                        {
                            if (plan_data == "")
                            {
                                continue;
                            }
                            Tv_Type = service.Split('!')[6];
                            string[] plan_details_arr = plan_data.Split('$');

                            Hashtable htData = new Hashtable();
                            if (plan_details_arr[9].ToString() == "Active")
                            {
                                if (plan_details_arr[1].ToString().Contains("SPECIAL") == false)
                                {
                                    continue;
                                }

                                htData["planname"] = plan_details_arr[1].ToString();
                                htData["planpoid"] = plan_details_arr[0].ToString();
                                htData["dealpoid"] = plan_details_arr[6].ToString();
                                htData["custprice"] = plan_details_arr[2].ToString();
                                htData["lcoprice"] = plan_details_arr[3].ToString();
                                htData["activation"] = plan_details_arr[4].ToString();
                                htData["expiry"] = plan_details_arr[5].ToString();
                                htData["packageid"] = plan_details_arr[7].ToString();
                                htData["purchasepoid"] = plan_details_arr[8].ToString();
                                htData["IP"] = "";
                                htData["autorenew"] = "N";
                                string conResult = callGetProviConfirm_CancellationProcess(htData, "C");
                                string[] result_arr = conResult.Split('#');
                                if (result_arr[0].ToString() != "9999")
                                {
                                    /* DataRow row1 = dtFOCPlanStatus.NewRow();
                                     row1["PlanName"] = htData["planname"].ToString();
                                     row1["RenewStatus"] = result_arr[1].ToString();
                                     dtFOCPlanStatus.Rows.Add(row1);*/

                                    DataRow row1 = dtFOCPlanStatus.NewRow();
                                    row1["PlanName"] = htData["planname"].ToString();
                                    row1["RenewStatus"] = result_arr[1].ToString();
                                    dtFOCPlanStatus.Rows.Add(row1);

                                    if (CancelTvType == "MAIN")
                                    {
                                        if (Tv_Type == "1")
                                        {
                                            Error_break = Error_break + 1;
                                            break;
                                        }
                                    }
                                    continue;
                                }
                                else
                                {
                                    string[] conResult_arr = result_arr[1].Split('$');
                                    htData["refund_amt"] = conResult_arr[1];
                                    htData["days_left"] = conResult_arr[0];
                                    ViewState["transaction_data"] = htData;
                                    hdnPopupAction.Value = "C";
                                    if (Tv_Type.ToString() == "0")
                                    {
                                        Tv_Type = "MAIN";

                                    }
                                    else
                                    {
                                        Tv_Type = "CHILD";
                                    }
                                    processTransaction_cancellation(vc, Tv_Type, stb_status, service_poid);

                                    /* DataRow row2 = dtFOCPlanStatus.NewRow();
                                    row2["PlanName"] = htData["planname"].ToString();
                                    row2["RenewStatus"] = ViewState["ErrorMessage"];
                                    dtFOCPlanStatus.Rows.Add(row2);*/

                                    DataRow row2 = dtFOCPlanStatus.NewRow();
                                    row2["PlanName"] = htData["planname"].ToString();
                                    row2["RenewStatus"] = ViewState["ErrorMessage"].ToString();
                                    dtFOCPlanStatus.Rows.Add(row2);
                                }

                                if (CancelTvType == "MAIN")
                                {
                                    if (Tv_Type == "CHILD")
                                    {
                                        if (ViewState["ErrorMessage"].ToString().Contains("Transaction successful :") == false)
                                        {
                                            Error_break = Error_break + 1;
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

        }

        public DataTable GetResult(String Query)
        {
            DataTable MstTbl = new DataTable();


            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            OracleCommand Cmd = new OracleCommand(Query, con);
            OracleDataAdapter AdpData = new OracleDataAdapter();
            AdpData.SelectCommand = Cmd;
            AdpData.Fill(MstTbl);

            con.Close();

            return MstTbl;
        }

        //Discount

        protected void btnDiscnt_Click(object sender, EventArgs e) //
        {

            lblResponseMsg.Text = "";
            txtdisamt.Text = "";
            txtexpdt.Text = "";
            txtReason.Text = "";
            ddlbillno.SelectedIndex = 0;
            div2.Visible = true;
            div3.Visible = true;
            lcodet.Visible = true;

            string strConn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(strConn);
            string str2 = "";


            lblCustName1.Text = ViewState["customer_name"].ToString();
            lblCustNo1.Text = ViewState["customer_no"].ToString();
            lblVCID1.Text = lblVCID.Text;
            lblStbNo1.Text = lblStbNo.Text;
            lbltxtmobno1.Text = ViewState["custmob"].ToString();
            lblCustAddr1.Text = ViewState["custaddr"].ToString();
            lblemail1.Text = ViewState["custemail"].ToString();

            str2 += " SELECT  * ";
            str2 += " FROM aoup_lcopre_discount  ";
            str2 += " where var_lcopre_discount_lcocode='" + Session["lco_username"].ToString() + "' and var_lcopre_discount_vcid ='" + ViewState["vcid"].ToString() + "'  and var_lcopre_discount_accno='" + ViewState["customer_no"].ToString() + "'";

            conn.Open();
            OracleCommand cmd3 = new OracleCommand(str2, conn);
            OracleDataReader dr5 = cmd3.ExecuteReader();

            while (dr5.Read())
            {
                txtdisamt.Text = dr5["num_lcopre_discount_amt"].ToString();

                if (dr5["var_lcopre_discount_type"].ToString() == "A")
                {
                    ddlbillno.SelectedIndex = 0;
                }
                else
                {
                    ddlbillno.SelectedIndex = 1;
                }

                txtexpdt.Text = Convert.ToDateTime(dr5["dat_lcopre_discount_expirydt"].ToString()).ToString("dd-MMM-yyyy");
                txtReason.Text = dr5["var_lcopre_discount_reason"].ToString();
            }

            dr5.Close();
            conn.Close();
            mpeDiscnt.Show();
            StatbleDynamicTabs();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            StatbleDynamicTabs();
            if (txtdisamt.Text == "")
            {
                lblResponseMsg.Text = "Please Enter Discount Amount...!!";
                lblResponseMsg.Visible = true;
                mpeDiscnt.Show();
                return;
            }

            else if (txtexpdt.Text == "")
            {
                lblResponseMsg.Text = "Please Enter Valid Upto Date....!!";
                lblResponseMsg.Visible = true;
                mpeDiscnt.Show();
                return;
            }
            else if (txtReason.Text == "")
            {
                lblResponseMsg.Text = "Please Enter Reasons....!!";
                lblResponseMsg.Visible = true;
                mpeDiscnt.Show();
                return;
            }

            if (Convert.ToDateTime(txtexpdt.Text.Trim()) < DateTime.Now)
            {
                lblResponseMsg.Text = "You can not select Valid Upto Date smaller than current date!";
                mpeDiscnt.Show();
                return;
            }

            Hashtable htlcodata = getData();
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

            Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
            string pro_output, errcode, errmsg = "";
            string[] output;
            pro_output = obj1.InsertDiscount(htlcodata, username, operator_id, catid);
            output = pro_output.Split('$');
            errcode = output[0].ToString();
            errmsg = output[1].ToString();
            if (errcode == "9999")
            {
                GetDiscountofLCOSuccess();
                msgboxstr("Discount Amount Allocated Successfully");
            }

            else
            {
                msgboxstr(errmsg);
            }

        }

        private Hashtable getData()
        {

            string username = ""; ;

            if (catid == "11")
            {
                username = Convert.ToString(Session["lco_username"]);
            }
            else
            {
                username = Convert.ToString(Session["username"]);
            }

            string vcid = lblVCID1.Text.ToString().Trim();
            string amount = txtdisamt.Text.ToString().Trim();
            string distype = ddlbillno.SelectedValue.ToString().Trim();
            string expdt = txtexpdt.Text.ToString().Trim();
            string reason = txtReason.Text.ToString().Trim();


            Hashtable htdata = new Hashtable();
            htdata.Add("username", username);
            htdata.Add("vcid", vcid);
            htdata.Add("amount", amount);
            htdata.Add("distype", distype);
            htdata.Add("expdt", expdt);
            htdata.Add("reason", reason);
            return htdata;

        }

        public void GetDiscountofLCO()
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();
            String Strdiscount = "select var_lcopre_discount_type distype,nvl(num_lcopre_discount_amt,0) discountamt from aoup_lcopre_discount "
                                + "where var_lcopre_discount_lcocode='" + Session["lco_username"].ToString() + "' "
            + "and var_lcopre_discount_accno='" + ViewState["customer_no"].ToString() + "' and var_lcopre_discount_vcid='" + ViewState["vcid"].ToString() + "'";
            OracleCommand cmd1 = new OracleCommand(Strdiscount, con);
            OracleDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                ViewState["discounttype"] = dr1["distype"].ToString();
                ViewState["DiscountAmount"] = dr1["discountamt"].ToString();

            }

            if (!dr1.HasRows)
            {
                ViewState["DiscountAmount"] = "0";
                ViewState["discounttype"] = "";
                //lbldiscountamt.Text = "Not Configured";
            }
            con.Close();
            dr1.Dispose();

        }


        public void GetDiscountofLCOSuccess()
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();
            String Strdiscount = "select var_lcopre_discount_type distype,nvl(num_lcopre_discount_amt,0) discountamt from aoup_lcopre_discount "
                                + "where var_lcopre_discount_lcocode='" + Session["lco_username"].ToString() + "' "
            + "and var_lcopre_discount_accno='" + ViewState["customer_no"].ToString() + "' and var_lcopre_discount_vcid='" + ViewState["vcid"].ToString() + "'";
            OracleCommand cmd1 = new OracleCommand(Strdiscount, con);
            OracleDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                ViewState["discounttype"] = dr1["distype"].ToString();
                ViewState["DiscountAmount"] = dr1["discountamt"].ToString();

                DataTable dtBasicPlans = (DataTable)ViewState["dtBasicPlans"];

                if (dtBasicPlans.Rows.Count > 0)
                {
                    if (ViewState["discounttype"].ToString() != "P")
                    {

                        dtBasicPlans.Rows[0]["DISCOUNT"] = "Rs. " + Convert.ToDouble(ViewState["DiscountAmount"].ToString()).ToString();
                    }
                    else
                    {
                        dtBasicPlans.Rows[0]["DISCOUNT"] = Convert.ToDouble(ViewState["DiscountAmount"].ToString()).ToString() + " %";
                    }

                    grdBasicPlanDetails.DataSource = dtBasicPlans;
                    grdBasicPlanDetails.DataBind();

                    for (int i = 0; i < grdBasicPlanDetails.Rows.Count; i++)
                    {
                        Button lnkaddplanbaseexpired = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkaddplanbaseexpired");
                        Button lnkBRenew = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkBRenew");
                        Button lnkBChange = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkBChange");
                        Button lnkBCancel = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkBCancel");
                        Button lnkAddFOCPack = (Button)grdBasicPlanDetails.Rows[i].Cells[8].FindControl("lnkAddFOCPack");

                        if (grdBasicPlanDetails.Rows[i].Cells[9].Text.ToString().ToString().ToUpper() == "EXPIRED")
                        {
                            grdBasicPlanDetails.Rows[i].Font.Bold = true;
                            grdBasicPlanDetails.Rows[i].Cells[0].ForeColor = System.Drawing.Color.Red;
                            grdBasicPlanDetails.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Red;
                            grdBasicPlanDetails.Rows[i].Cells[2].ForeColor = System.Drawing.Color.Red;
                            grdBasicPlanDetails.Rows[i].Cells[3].ForeColor = System.Drawing.Color.Red;
                            grdBasicPlanDetails.Rows[i].Cells[4].ForeColor = System.Drawing.Color.Red;
                            grdBasicPlanDetails.Rows[i].Cells[5].ForeColor = System.Drawing.Color.Red;
                            grdBasicPlanDetails.Rows[i].Cells[6].ForeColor = System.Drawing.Color.Red;
                            grdBasicPlanDetails.Rows[i].Cells[7].ForeColor = System.Drawing.Color.Red;
                            grdBasicPlanDetails.Rows[i].Cells[9].ForeColor = System.Drawing.Color.Red;
                            //lnkaddplanbaseexpired.Visible = true;
                            lnkaddplanbaseexpired.Visible = false;
                            lnkBRenew.Visible = false;
                            lnkBChange.Visible = false;
                            lnkBCancel.Visible = false;
                            lnkAddFOCPack.Visible = false;
                            ViewState["BasepalnExpired"] = true;
                            radPlanBasic.Checked = true;
                        }
                        else
                        {
                            lnkaddplanbaseexpired.Visible = false;
                            lnkBRenew.Visible = true;
                            //lnkBChange.Visible = true;
                            lnkBChange.Visible = false;
                            lnkBCancel.Visible = true;
                            // lnkAddFOCPack.Visible = true;
                            lnkAddFOCPack.Visible = false;
                            ViewState["BasepalnExpired"] = false;
                            radPlanBasic.Checked = false;
                        }

                    }

                }

            }

            if (!dr1.HasRows)
            {
                ViewState["DiscountAmount"] = "0";
                ViewState["discounttype"] = "";
                //lbldiscountamt.Text = "Not Configured";
            }
            con.Close();
            dr1.Dispose();

        }

        //protected void lnkpopOpen_Click(object sender, EventArgs e)
        //{
        //    int index = (((GridViewRow)(((ImageButton)(sender)).Parent.BindingContainer))).RowIndex;
        //    ViewState["indexBase"] = index.ToString();
        //    int rowindex = Convert.ToInt32((((GridViewRow)(((ImageButton)(sender)).Parent.BindingContainer))).RowIndex);
        //    string expirydt = grdBasicPlanDetails.Rows[rowindex].Cells[4].Text.Trim();
        //    //btnRenewPop.Visible = true;
        //    //btncancelPop.Visible = true;
        //    //btnChangepop.Visible = true;
        //    //btnFOCPack.Visible = true;
        //    //if (expirydt == "01-JAN-70" || ViewState["stb_status"].ToString() == "10102")
        //    //{
        //    //    btnRenewPop.Visible = false;
        //    //    btncancelPop.Visible = false;
        //    //    btnChangepop.Visible = false;
        //    //    btnFOCPack.Visible = false;
        //    //}

        //    //if (grdBasicPlanDetails.Rows[rowindex].Cells[9].Text.ToString().ToString().ToUpper() == "EXPIRED")
        //    //{
        //    //    btnRenewPop.Visible = false;
        //    //    btncancelPop.Visible = false;
        //    //    btnChangepop.Visible = false;
        //    //    btnFOCPack.Visible = false;
        //    //}
        //    mpeActions.Show();
        //    StatbleDynamicTabs();
        //}
        //protected void lnkADActionPop_Click(object sender, EventArgs e)
        //{

        //    ViewState["indexADDon"] = (((GridViewRow)(((ImageButton)(sender)).Parent.BindingContainer))).RowIndex;
        //    int rowindex = Convert.ToInt32((((GridViewRow)(((ImageButton)(sender)).Parent.BindingContainer))).RowIndex);
        //    string expirydt = grdAddOnPlan.Rows[rowindex].Cells[4].Text.Trim();
        //    btnAdREnewPop.Visible = true;
        //    btnADCancelPop.Visible = true;
        //    btnADChangePop.Visible = true;

        //    if (expirydt == "01-JAN-70" || ViewState["stb_status"].ToString() == "10102")
        //    {
        //        btnAdREnewPop.Visible = false;
        //        btnADCancelPop.Visible = false;
        //        if (expirydt == "01-JAN-70")
        //        {
        //            btnADChangePop.Visible = true;
        //        }
        //        if (ViewState["stb_status"].ToString() == "10102")
        //        {
        //            btnADChangePop.Visible = false;
        //        }
        //    }
        //    mpeActionsAddon.Show();
        //    StatbleDynamicTabs();
        //}
        ////    mpeActionsAL.Show();

        //protected void lnkALOpenPop_Click(object sender, EventArgs e)
        //{

        //    ViewState["indexALA"] = (((GridViewRow)(((ImageButton)(sender)).Parent.BindingContainer))).RowIndex;
        //    int rowindex = Convert.ToInt32((((GridViewRow)(((ImageButton)(sender)).Parent.BindingContainer))).RowIndex);
        //    string expirydt = grdCarte.Rows[rowindex].Cells[4].Text.Trim();
        //    btnALREnewPop.Visible = true;
        //    btnALCancelPop.Visible = true;
        //    btnALChangePop.Visible = true;

        //    if (expirydt == "01-JAN-70" || ViewState["stb_status"].ToString() == "10102")
        //    {
        //        btnALREnewPop.Visible = false;
        //        btnALCancelPop.Visible = false;
        //        if (expirydt == "01-JAN-70")
        //        {
        //            btnALChangePop.Visible = true;
        //        }
        //        if (ViewState["stb_status"].ToString() == "10102")
        //        {
        //            btnALChangePop.Visible = false;
        //        }
        //    }
        //    mpeActionsAL.Show();
        //    StatbleDynamicTabs();

        //}



        protected void btnSwap_click(object sender, EventArgs e)
        {
            int rowindex = Convert.ToInt32((((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex);
            string VC_ID = ((HiddenField)GridVC.Rows[rowindex].FindControl("hdnVC_ID")).Value;
            string STB_NO = ((HiddenField)GridVC.Rows[rowindex].FindControl("hdnSTB_NO")).Value;

            string VC_IDMAIN = ViewState["VC_IDMAIN"].ToString();
            string STB_NOMAIN = ViewState["STB_NOMAIN"].ToString();

            txtSwapChildTV.Text = VC_ID;
            txtswapMainTV.Text = VC_IDMAIN;
            txtSwapChildSTB.Text = "";
            txtswapMainSTB.Text = STB_NOMAIN;
            ddlSawpReason.SelectedValue = "0";
            ddlSwapChildTV.Items.Clear();
            foreach (GridViewRow row in GridVC.Rows)
            {
                if (row.Cells[0].Text != "Main TV")
                {
                    string strVC = row.Cells[1].Text; //Where Cells is the column. Just changed the index of cells
                    string strSTB = row.Cells[2].Text + "$" + row.Cells[3].Text;
                    ddlSwapChildTV.Items.Add(new ListItem(strVC.ToString(), strSTB.ToString()));
                }
                else
                {
                    lblActMain.Text = row.Cells[3].Text;
                }
            }
            ddlSwapChildTV.Items.Insert(0, "Select VC/MAC");
            //ddlSwapChildTV.SelectedValue = "0";
            lblActChild.Text = "";
            lblPopupResponse1.Text = "";
            mpeSwapPop.Show();
            StatbleDynamicTabs();
        }

        protected void ddlSwapChildTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] strSTB_ = ddlSwapChildTV.SelectedValue.Split('$');
            txtSwapChildSTB.Text = strSTB_[0].ToString();
            lblActChild.Text = strSTB_[1].ToString();
            mpeSwapPop.Show();
            StatbleDynamicTabs();
        }
        protected void btnswapConf_Click(object sender, EventArgs e)
        {

            if (ddlSwapChildTV.SelectedValue == "0" || ddlSwapChildTV.SelectedValue == "Select VC")
            {
                lblPopupResponse1.Text = "Please Select Child VC To SWAP.";
                mpeSwapPop.Show();
                StatbleDynamicTabs();
                return;
            }
            if (ddlSawpReason.SelectedValue == "0")
            {
                lblPopupResponse1.Text = "Please Select Reason.";
                mpeSwapPop.Show();
                StatbleDynamicTabs();
                return;
            }
            if (lblActChild.Text != "Active")
            {
                lblPopupResponse1.Text = "Child TV STB is Suspended";
                mpeSwapPop.Show();
                StatbleDynamicTabs();
                return;
            }
            if (lblActMain.Text != "Active")
            {
                lblPopupResponse1.Text = "Main TV STB is Suspended";
                mpeSwapPop.Show();
                StatbleDynamicTabs();
                return;
            }

            ViewState["SWAPChildVC"] = ddlSwapChildTV.SelectedItem.ToString();
            ViewState["SWAPMainVC"] = txtswapMainTV.Text;
            ViewState["SWAPReason"] = ddlSawpReason.SelectedValue;
            ViewState["SWAPChildSTB"] = txtSwapChildSTB.Text;
            ViewState["SWAPMainSTB"] = txtswapMainSTB.Text;
            lblCofChildVC.Text = ViewState["SWAPChildVC"].ToString();
            lblCofMainVC.Text = ViewState["SWAPMainVC"].ToString();
            lblCofChildSTB.Text = ViewState["SWAPChildSTB"].ToString();
            lblCofMainSTB.Text = ViewState["SWAPMainSTB"].ToString();
            lblCofReason.Text = ddlSawpReason.SelectedItem.Text; ;
            if (lblCofMainVC.Text == lblCofMainSTB.Text)
            {
                Label1050.Text = "Main VC/MAC :";
                Label1049.Text = "Main STB/MAC :";
                Label1048.Text = "Child VC/MAC :";
                Label1047.Text = "Child STB/MAC :";
            }
            else
            {
                Label1050.Text = "Main VC :";
                Label1049.Text = "Main STB :";
                Label1048.Text = "Child VC :";
                Label1047.Text = "Child STB :";
            }
            mpSwapConfirm.Show();
            StatbleDynamicTabs();
        }

        protected void btnSWAPConfirmation1_Click(object sender, EventArgs e)
        {
            string user_brmpoid = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
            {
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            // string response_params = username + "$" + searhParam + "$SW";
            string response_params = user_brmpoid + "$" + lblCofChildVC.Text + "$SW";

            //if VC ID
            response_params += "$V";

            // string apiResponse = callAPI(response_params, "6");
            string SERVICE_OBJ = "";
            string DEVICE_ID = "";
            string apiResponse = callAPI(response_params, "12");//
            try
            {
                if (apiResponse != "")
                {

                    List<string> lstResponse = new List<string>();

                    lstResponse = apiResponse.Split('$').ToList();
                    ViewState["accountPoid"] = lstResponse[6];
                    string cust_services = lstResponse[15];
                    string[] service_arr = cust_services.Split('^');
                    ViewState["Service_Str"] = null;
                    ViewState["Service_Str"] = cust_services.ToString();

                    ViewState["parentsmsg"] = "0";
                    foreach (string service in service_arr)
                    {
                        string parent_child_flag = service.Split('!')[6];
                        string vc_id = service.Split('!')[2];
                        if (parent_child_flag == "0")
                        {
                            Session["SERVICE_STRING"] = service;
                            SERVICE_OBJ = service.Split('!')[0];

                        }
                        if (vc_id == lblCofChildVC.Text)
                        {
                            DEVICE_ID = service.Split('!')[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatbleDynamicTabs();
                lblPopupResponse.Text = "Transaction Failed from OBRM ";
                popMsg.Show();
            }


            try
            {
                string response_params2 = Session["username"] + "$" + ViewState["accountPoid"] + "$" + DEVICE_ID + "$" + SERVICE_OBJ;//GetHwayOBRMPARENTCHILDSTB_SWAP_Action

                string apiResponse2 = callAPI(response_params2, "35");
                string[] GetAPIresponse = apiResponse2.Split('$');
                if (GetAPIresponse[0] == "0")
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("username", Session["username"].ToString());
                    ht.Add("AccountNo", lblCustNo.Text);
                    ht.Add("FromVC", lblCofChildVC.Text);
                    ht.Add("FromSTB", lblCofChildSTB.Text);
                    ht.Add("TOVC", lblCofMainVC.Text);
                    ht.Add("TOSTB", lblCofMainSTB.Text);
                    ht.Add("Reason", lblCofReason.Text);
                    ht.Add("Type", "C2M");
                    ht.Add("Status", "Success");
                    ht.Add("CustomerName", lblCustName.Text);
                    ht.Add("MobileNo", lbltxtmobno.Text);
                    ht.Add("EmailID", lblemail.Text);
                    ht.Add("CustomerAdd", lblCustAddr.Text);
                    ht.Add("OperID", Convert.ToString(Session["operator_id"]));
                    Cls_Business_Faulty_Swap obj = new Cls_Business_Faulty_Swap();
                    string response = obj.getSTB_Swap(ht);
                    string[] Getresponse1 = response.Split('#');
                    if (Getresponse1[0] == "9999")
                    {
                        mpSwapConfirm.Hide();
                        lblPopupResponse4.Text = Getresponse1[1].ToString();
                        MPOPMsg.Show();
                    }
                    else
                    {
                        mpSwapConfirm.Hide();
                        lblPopupResponse4.Text = Getresponse1[1].ToString();
                        MPOPMsg.Show();
                    }
                }
                else
                {
                    lblPopupResponse.Text = "Failed from OBRM :" + GetAPIresponse[1].ToString();
                    popMsg.Show();
                }
                StatbleDynamicTabs();
            }
            catch (Exception ex)
            {
                StatbleDynamicTabs();
                lblPopupResponse.Text = "Transaction Failed from OBRM ";
                popMsg.Show();
            }
        }

        protected void btnSWAPConfirmationClose_Click(object sender, EventArgs e)
        {
            lblCofChildVC.Text = "";
            lblCofChildSTB.Text = "";
            lblCofMainVC.Text = "";
            lblCofMainSTB.Text = "";
            lblCofReason.Text = "";
            StatbleDynamicTabs();
            MPESwapConfirm1.Hide();
        }
        protected void btnModifyConfirm_click(object sender, EventArgs e)
        {
            MPESwapConfirm1.Show();
            StatbleDynamicTabs();
            //}
            //else
            //{
            //    mpSwapConfirm.Hide();
            //    StatbleDynamicTabs();
            //    lblPopupResponse.Text = Getresponse[1].ToString();
            //    popMsg.Show();
            //}
        }

        private int _counter;

        protected void GridVC_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            DataTable DT = (DataTable)ViewState["vcdetail"];
            if (DT.Rows.Count > 1)
            {
                GridVC.Columns[6].Visible = true;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_counter == 0)
                    {
                        var Button = e.Row.FindControl("btnSwap")
                            as Button;
                        Button.Visible = true;
                        _counter++;
                    }
                }
            }
            else
            {
                GridVC.Columns[6].Visible = false;
            }

        }

        protected void BtnFaulty_Click(object sender, EventArgs e)
        {
            txtfaultySTBID.Text = lblStbNo.Text;
            txtfaultyNewSTBID.Text = "";
            ddlFaultyReason.SelectedValue = "0";
            POPFaulty.Show();
            lblPopupResponse2.Text = "";
            if (btnFaulty.Text == "SWAP STB/MAC")
            {
                lblFaultySTB_ID.Text = "STB/MAC ID";
                lblFaultyNewSTB_ID.Text = "NEW STB/MAC ID";
                Session["SwapWise"] = "mac";
            }
            else
            {
                lblFaultySTB_ID.Text = "STB ID";
                lblFaultyNewSTB_ID.Text = "NEW STB ID";
                Session["SwapWise"] = "stb";
            }
            StatbleDynamicTabs();

        }
        protected void btnFaultyConf_Click(object sender, EventArgs e)
        {
            string blusername = SecurityValidation.chkData("N", txtfaultyNewSTBID.Text);
            if (blusername.Length > 0)
            {
                lblPopupResponse2.Text = blusername;
                POPFaulty.Show();
                StatbleDynamicTabs();
                return;
            }

            if (txtfaultyNewSTBID.Text == "")
            {
                lblPopupResponse2.Text = "Please Enter New STB No.";
                POPFaulty.Show();
                StatbleDynamicTabs();
                return;
            }
            if (ddlFaultyReason.SelectedValue == "0")
            {
                lblPopupResponse2.Text = "Please Select Reason.";
                POPFaulty.Show();
                StatbleDynamicTabs();
                return;
            }

            ViewState["FaultySTBID"] = txtfaultySTBID.Text;
            ViewState["FaultyNewSTBID"] = txtfaultyNewSTBID.Text;
            ViewState["FaultyReason"] = ddlFaultyReason.SelectedValue.ToString();

            Cls_Business_Faulty_Swap obj3 = new Cls_Business_Faulty_Swap();
            string OutStatus = "";
            //-- Replace this method to call API to validate STB
            obj3.GetPlanTo_Swap(Session["username"].ToString(), txtfaultySTBID.Text, txtfaultyNewSTBID.Text, Session["SwapWise"].ToString(), out OutStatus);
            //-
            string[] Getresponse = OutStatus.Split('$');
            if (Getresponse[0] == "9999")
            {
                lblSTBID.Text = ViewState["FaultySTBID"].ToString();
                lblNewSTBID.Text = ViewState["FaultyNewSTBID"].ToString();
                lblFaultyCofReason.Text = ddlFaultyReason.SelectedItem.Text;// ViewState["FaultyReason"].ToString();
                mpeFaulty.Show();
                if (Session["SwapWise"].ToString() == "stb" || Session["SwapWise"].ToString() == "mac")
                {
                    if (btnFaulty.Text == "SWAP STB/MAC")
                    {
                        lblCofSTB_ID.Text = "STB/MAC ID";
                        lblCofNEWSTB_ID.Text = "New STB/MAC ID";

                    }
                    else
                    {
                        lblCofSTB_ID.Text = "STB ID";
                        lblCofNEWSTB_ID.Text = "New STB ID";
                    }
                }
                else
                {

                    lblCofSTB_ID.Text = "VC ID";
                    lblCofNEWSTB_ID.Text = "NEW VC ID";

                }
                StatbleDynamicTabs();
            }
            else
            {
                StatbleDynamicTabs();
                POPFaulty.Show();
                lblPopupResponse2.Text = Getresponse[1].ToString();
                return;
            }

        }

        protected void btnFaultyModifyConfirm_click(object sender, EventArgs e)
        {
            POPFaulty.Hide();
            mpeFaultyConfirm.Show();
            StatbleDynamicTabs();
        }
        protected void btnFaultyModifyConfirm1_click(object sender, EventArgs e)
        {
            string user_brmpoid = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
            {
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            if (Session["SwapWise"].ToString() == "mac")
            {
                Session["SwapWise"] = "stb";
            }
            try
            {

                string response_params2 = Session["username"] + "$" + lblSTBID.Text + "$" + lblNewSTBID.Text + "$" + Session["SwapWise"].ToString();//GetHwayOBRMPARENTCHILDSTB_SWAP_Action

                string apiResponse2 = callAPI(response_params2, "38");
                string[] GetAPIresponse = apiResponse2.Split('$');
                if (GetAPIresponse[0] == "0")
                {
                    if (Session["SwapWise"].ToString() == "stb")
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("username", Session["username"].ToString());
                        ht.Add("AccountNo", lblCustNo.Text);
                        ht.Add("FromVC", "");
                        ht.Add("FromSTB", lblNewSTBID.Text);
                        ht.Add("TOVC", lblVCID.Text);
                        ht.Add("TOSTB", lblSTBID.Text);
                        ht.Add("Reason", lblFaultyCofReason.Text);
                        ht.Add("Type", "FSS");
                        ht.Add("Status", "");
                        ht.Add("CustomerName", lblCustName.Text);
                        ht.Add("MobileNo", lbltxtmobno.Text);
                        ht.Add("EmailID", lblemail.Text);
                        ht.Add("CustomerAdd", lblCustAddr.Text);
                        ht.Add("OperID", Convert.ToString(Session["operator_id"]));
                        Cls_Business_Faulty_Swap obj = new Cls_Business_Faulty_Swap();
                        string response = obj.getSTB_Swap(ht);
                        string[] Getresponse = response.Split('#');
                        if (Getresponse[0] == "9999")
                        {
                            mpSwapConfirm.Hide();
                            lblPopupResponse.Text = Getresponse[1].ToString();
                            popMsg.Show();
                            StatbleDynamicTabs();
                        }
                        else
                        {
                            mpSwapConfirm.Hide();
                            lblPopupResponse.Text = Getresponse[1].ToString();
                            popMsg.Show();
                            StatbleDynamicTabs();
                        }
                    }
                    else if (Session["SwapWise"].ToString() == "vc")
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("username", Session["username"].ToString());
                        ht.Add("AccountNo", lblCustNo.Text);
                        ht.Add("FromVC", lblNewSTBID.Text);
                        ht.Add("FromSTB", "");
                        ht.Add("TOVC", lblSTBID.Text);
                        ht.Add("TOSTB", lblStbNo.Text);
                        ht.Add("Reason", lblFaultyCofReason.Text);
                        ht.Add("Type", "FVS");
                        ht.Add("Status", "");
                        ht.Add("CustomerName", lblCustName.Text);
                        ht.Add("MobileNo", lbltxtmobno.Text);
                        ht.Add("EmailID", lblemail.Text);
                        ht.Add("CustomerAdd", lblCustAddr.Text);
                        ht.Add("OperID", Convert.ToString(Session["operator_id"]));
                        Cls_Business_Faulty_Swap obj = new Cls_Business_Faulty_Swap();
                        string response = obj.getSTB_Swap(ht);
                        string[] Getresponse = response.Split('#');
                        if (Getresponse[0] == "9999")
                        {
                            mpSwapConfirm.Hide();
                            lblPopupResponse.Text = Getresponse[1].ToString();
                            popMsg.Show();
                            StatbleDynamicTabs();
                        }
                        else
                        {
                            mpSwapConfirm.Hide();
                            lblPopupResponse.Text = Getresponse[1].ToString();
                            popMsg.Show();
                            StatbleDynamicTabs();
                        }
                    }
                }
                else
                {
                    lblPopupResponse.Text = "failed from obrm :" + GetAPIresponse[1].ToString();
                    popMsg.Show();
                    StatbleDynamicTabs();
                }
            }
            catch (Exception ex)
            {
                StatbleDynamicTabs();
                lblPopupResponse.Text = "Transaction Failed from OBRM ";
                popMsg.Show();
                //throw;
            }
        }
        protected void btnFaultyConfirmationClose_Click(object sender, EventArgs e)
        {
            mpeFaultyConfirm.Hide();
            StatbleDynamicTabs();
        }
        protected void BtnVCSWAP_Click(object sender, EventArgs e)
        {
            txtfaultySTBID.Text = lblVCID.Text;
            txtfaultyNewSTBID.Text = "";
            ddlFaultyReason.SelectedValue = "0";
            POPFaulty.Show();
            lblPopupResponse2.Text = "";
            lblFaultySTB_ID.Text = "VC ID";
            lblFaultyNewSTB_ID.Text = "NEW VC ID";
            Session["SwapWise"] = "vc";


            StatbleDynamicTabs();

        }
        // --- added By RP ON 12.12.2017
        protected void btnTerminate_Click(object sender, EventArgs e)
        {
            lblTerminateSTBNO.Text = lblStbNo.Text;
            lblTerminateVCID.Text = lblVCID.Text;
            ddlTERMINATEReason.SelectedValue = "0";
            PopTERMINATE.Show();
            lblPopupResTerminate.Text = "";
            StatbleDynamicTabs();

        }
        protected void btnTerminateConf_Click(object sender, EventArgs e)
        {

            if (ddlTERMINATEReason.SelectedValue == "0")
            {
                lblPopupResTerminate.Text = "Please Select Reason.";
                PopTERMINATE.Show();
                StatbleDynamicTabs();
                return;
            }


            ViewState["Terminate_STB"] = lblTerminateSTBNO.Text;
            ViewState["Terminate_VC"] = lblTerminateVCID.Text;
            ViewState["Terminate_Reason"] = ddlTERMINATEReason.SelectedItem.Text;
            lblTerminate_STB.Text = ViewState["Terminate_STB"].ToString();
            lblTerminate_VC.Text = ViewState["Terminate_VC"].ToString();
            lblTerminate_Reason.Text = ViewState["Terminate_Reason"].ToString();
            PopTERMINATE.Hide();
            popConfTERMINATEAL.Show();
            StatbleDynamicTabs();
        }

        protected void btnTERMINATEALModifyConfirm2_click(object sender, EventArgs e)
        {
            string user_brmpoid = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
            {
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            // string response_params = username + "$" + searhParam + "$SW";
            string response_params = user_brmpoid + "$" + lblTerminate_VC.Text + "$SW";

            //if VC ID
            response_params += "$V";

            // string apiResponse = callAPI(response_params, "6");
            string SERVICE_OBJ = "";
            string DEVICE_ID = "";
            string apiResponse = callAPI(response_params, "12");//
            try
            {
                if (apiResponse != "")
                {

                    List<string> lstResponse = new List<string>();

                    lstResponse = apiResponse.Split('$').ToList();
                    ViewState["accountPoid"] = lstResponse[6];
                    string cust_services = lstResponse[15];
                    string[] service_arr = cust_services.Split('^');
                    ViewState["Service_Str"] = null;
                    ViewState["Service_Str"] = cust_services.ToString();

                    ViewState["parentsmsg"] = "0";
                    foreach (string service in service_arr)
                    {
                        string parent_child_flag = service.Split('!')[6];
                        string vc_id = service.Split('!')[2];
                        if (parent_child_flag == "0")
                        {


                        }
                        if (vc_id == lblTerminateVCID.Text)
                        {
                            SERVICE_OBJ = service.Split('!')[0];
                            DEVICE_ID = service.Split('!')[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatbleDynamicTabs();
                lblPopupResponse.Text = "Transaction Failed from OBRM ";
                popMsg.Show();
            }


            try
            {
                string response_params2 = Session["username"].ToString() + "$" + ViewState["accountPoid"] + "$" + SERVICE_OBJ;//GetHwayOBRMPARENTCHILDSTB_SWAP_Action
                ViewState["ServicePoidTer"] = SERVICE_OBJ;

                string apiResponse2 = callAPI(response_params2, "39");
                string obrm_orderid = "";
                string[] GetAPIresponse = apiResponse2.Split('$');
                if (GetAPIresponse[0] == "0")
                {
                    Cls_Data_Auth auth = new Cls_Data_Auth();
                    string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                    Hashtable ht = new Hashtable();
                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    Hashtable htServiceData = new Hashtable();
                    htServiceData["username"] = username;
                    htServiceData["stb_no"] = lblTerminateSTBNO.Text;
                    htServiceData["vc_id"] = lblTerminateVCID.Text;
                    htServiceData["cust_no"] = lblCustNo.Text;
                    htServiceData["cust_addr"] = lblCustAddr.Text;
                    htServiceData["orderid"] = obrm_orderid;
                    htServiceData["reason_id"] = ddlTERMINATEReason.SelectedValue;
                    htServiceData["account_poid"] = ViewState["accountPoid"].ToString();
                    htServiceData["service_poid"] = ViewState["ServicePoidTer"].ToString();
                    htServiceData["IP"] = Ip;
                    htServiceData["status"] = "T";
                    string response = obj.serviceStatusUpdateBLL(htServiceData);
                    string[] Getresponse1 = response.Split('$');
                    if (Getresponse1[0] == "9999")
                    {
                        popConfTERMINATEAL2.Hide();
                        StatbleDynamicTabs();
                        if (lblTerminateSTBNO.Text == lblTerminateVCID.Text)
                        {
                            lblPopupResponse4.Text = "MAC_ID Depairing is Successful";
                        }
                        else
                        {
                            lblPopupResponse4.Text = "STB and VC Depairing is Successful";
                        }
                        MPOPMsg.Show();
                    }
                    else
                    {
                        popConfTERMINATEAL2.Hide();
                        StatbleDynamicTabs();
                        lblPopupResponse4.Text = Getresponse1[1].ToString();
                        MPOPMsg.Show();
                    }
                }
                else
                {
                    popConfTERMINATEAL2.Hide();
                    StatbleDynamicTabs();
                    lblPopupResponse.Text = "Failed from OBRM :" + GetAPIresponse[1].ToString();
                    popMsg.Show();

                }

            }
            catch (Exception ex)
            {
                StatbleDynamicTabs();
                lblPopupResponse.Text = "Transaction Failed from OBRM ";
                popMsg.Show();
                // throw;
            }
        }


        protected void btnTERMINATEALConfirmationClose1_Click(object sender, EventArgs e)
        {
            popConfTERMINATEAL2.Hide();
            mpSwapConfirm.Hide();
            StatbleDynamicTabs();
        }
        protected void btnTerminateModifyConfirm_click(object sender, EventArgs e)
        {
            popConfTERMINATEAL2.Show();
            StatbleDynamicTabs();
            //}
            //else
            //{
            //    mpSwapConfirm.Hide();
            //    StatbleDynamicTabs();
            //    lblPopupResponse.Text = Getresponse[1].ToString();
            //    popMsg.Show();
            //}
        }
        protected void btnFaultyclose_click(object sender, EventArgs e)
        {
            StatbleDynamicTabs();
            mpeFaulty.Hide();
        }
        protected void btnTerminatClose_click(object sender, EventArgs e)
        {
            StatbleDynamicTabs();
            popConfTERMINATEAL.Hide();
        }
        protected void BtnRequestfrm_Click(object sender, EventArgs e)
        {
            DataTable sortedDT = (DataTable)ViewState["vcdetail"];
            DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
            string stb_no = myResultSet.Rows[0]["STB_NO"].ToString();
            string vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
            Session["vc_id"] = vc_id;

            //Response.Write("<script>window.open ('../Transaction/OnlineReqform.aspx','_blank');</script>");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "window.open('OnlineReqform.aspx?formrequestdetails=','_blank');", true);
            //Response.Redirect("~/Transaction/OnlineReqform.aspx");
        }

        protected void btnQuickpay_Click(object sender, EventArgs e)
        {
            Quickpayurl(Session["username"].ToString(), Session["operator_id"].ToString(), Session["customer_no"].ToString(), Session["custmob"].ToString());
        }
        private string CallHttpWebRequest(string URL)
        {
            string strreturn = "";
            string sAddress = URL;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sAddress);
            req.Accept = "text/xml,text/plain,text/html";
            req.Method = "GET";
            HttpWebResponse result = (HttpWebResponse)req.GetResponse();
            Stream ReceiveStream = result.GetResponseStream();
            StreamReader reader = new StreamReader(ReceiveStream, System.Text.Encoding.ASCII);
            string respHTML = reader.ReadToEnd();
            reader.Close();
            return strreturn = respHTML;
        }

        public string Quickpayurl(string username, string Oper_ID, string AccountNo, string MobileNo)
        {
            String Response = "";
            try
            {
                Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                string data = username + "||" + AccountNo + "||" + MobileNo;
                //FileLogText("quickpayurl first :-", data, "", "");
                string date = DateTime.Now.ToString("yyddMMHH");
                string strstring = AccountNo + date;
                string strURL = CallHttpWebRequest("http://tinyurl.com/api-create.php?url=http://selfcare.hathway.com/smslogin/" + strstring);
                string finalURL = "Dear Subscriber, To renew your Cable TV subscription Click " + strURL;
                string conResult = obj.quickpayurl(username, AccountNo, MobileNo, finalURL);
                string[] result_arr = conResult.Split('#');
                data = username + "||" + AccountNo + "||" + MobileNo + "||" + result_arr;
                //FileLogText("quickpayurl after :-", data, "", "");
                if (result_arr[0] == "9999")
                {
                    Response = "1$Success ..";
                }
                else
                {
                    Response = "0$Getting Error";
                }
            }
            catch (Exception ex)
            {
                return Response = "0$Getting Error";
            }
            return Response;
        }

        protected void btnsearchfilter_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["PLanData"];
            DataView dv = new DataView(dt);
            string strQuery = "";
            string str2 = "";
            if (radPlanAL.Checked == true)
            {
                /* string str = "";
                 string city = "";
                 if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
                 {
                     city = ViewState["cityid"].ToString();
                 }
                 string basic_poids = "'0'";
                 if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
                 {
                     basic_poids = ViewState["basic_poids"].ToString();
                     basic_poids = "'" + basic_poids + "'";
                 }
                 if (basic_poids != "'0'")
                 {

                 }
                 string addon_poids = "'0'";
                 if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
                 {
                     addon_poids = ViewState["addon_poids"].ToString();
                 }
                 string hsp_poids = "'0'";
                 if (ViewState["hwayspecial_poid"] != null && ViewState["hwayspecial_poid"].ToString() != "")
                 {
                     hsp_poids = ViewState["hwayspecial_poid"].ToString();
                 }
                
                 string PlanTypenew = "";
                 if (radPlanAD.Checked == true)
                 {
                     PlanTypenew = "GAD";
                 }
                 else if (radPlanADreg.Checked == true)
                 {
                     PlanTypenew = "RAD";
                 }
                 string[] addon_poidsarr = addon_poids.Replace("'", "").Split(',');
                 if (ViewState["JVFlag"].ToString() == "Y")
                 {
                     str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,BC_PRICE, broad_name, genre_type,var_plan_freeflag from(";
                     str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                     str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOJVPLAN_FETCH_ALL a ";
                     str += " where a.cityid ='" + city + "'";// and a.PLAN_TYPE='" + PlanTypenew + "' ";
                     {
                         str += " and a.plan_type not in ('B','HSP')";
                     }
                     str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                     str += " and lcocode=" + Session["lcoid"].ToString(); //+ hd_where_clause;
                     str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                     str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                     str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_JVPLAN_FETCH_ALL a ";
                     str += " where a.cityid ='" + city + "'";// and a.PLAN_TYPE='" + PlanTypenew + "' ";
                     {
                         str += " and a.plan_type not in ('B','HSP')";
                     }
                     str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                     str +=  " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                     str += " and not EXISTS (select * from VIEW_LCOJVPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                     str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
                  
                 }
                 else
                 {

                     str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,BC_PRICE, broad_name, genre_type,var_plan_freeflag from(";
                     str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                     str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOPLAN_FETCH_ALL a ";
                     str += " where a.cityid ='" + city + "' ";//and a.PLAN_TYPE='" + PlanTypenew + "' ";
                     {
                         str += " and a.plan_type not in ('B','HSP')";
                     }
                     str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                     str += " and lcocode=" + Session["lcoid"].ToString() ;//+ hd_where_clause;
                     str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                     str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                     str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_PLAN_FETCH_ALL a ";
                     str += " where a.cityid ='" + city + "' ";//and a.PLAN_TYPE='" + PlanTypenew + "' ";
                     {
                         str += " and a.plan_type not in ('B','HSP')";
                     }
                     str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                     str +=  " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                     str += " and not EXISTS (select * from VIEW_LCOPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                     str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
                    
                 }
                 */

                if (ddlBC2.SelectedValue != "0")
                {
                    if (ddlBC2.SelectedItem.Text.ToString() != "")
                    {
                        strQuery = "   broad_name='" + ddlBC2.SelectedItem.Text.ToString() + "'  ";
                    }
                }
                if (ddlSDHD.SelectedValue != "0")
                {
                    if (strQuery != "")
                    {
                        strQuery += " and ";
                    }
                    strQuery += "  var_plan_devicetype='" + ddlSDHD.SelectedValue + "' ";
                }

                if (ddlPAYFREE.SelectedValue != "0")
                {
                    if (strQuery != "")
                    {
                        strQuery += " and ";
                    }
                    strQuery += " var_plan_freeflag='" + ddlPAYFREE.SelectedValue + "' ";
                }
                if (ddlGener.SelectedValue != "0")
                {
                    if (strQuery != "")
                    {
                        strQuery += " and ";
                    }
                    if (ddlGener.SelectedItem.Text.ToString() != "")
                    {
                        strQuery += " genre_type='" + ddlGener.SelectedItem.Text.ToString() + "' ";
                    }
                }
                // str2 = "select * from (" + str + ") where 1=1 " + strQuery;
            }
            else if (radPlanAll.Checked == true)
            {
                /* string str = "";
             string city = "";
                 string DeviceDefinitionType = "SD";
             if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
             {
                 city = ViewState["cityid"].ToString();
             }
             string basic_poids = "'0'";
             if (ViewState["basic_poids"] != null && ViewState["basic_poids"].ToString() != "")
             {
                 basic_poids = ViewState["basic_poids"].ToString();
                 basic_poids = "'" + basic_poids + "'";
             }
             if (basic_poids != "'0'")
             { 
            
             }
             string addon_poids = "'0'";
             if (ViewState["addon_poids"] != null && ViewState["addon_poids"].ToString() != "")
             {
                 addon_poids = ViewState["addon_poids"].ToString();
             }
             string hsp_poids = "'0'";
             if (ViewState["hwayspecial_poid"] != null && ViewState["hwayspecial_poid"].ToString() != "")
             {
                 hsp_poids = ViewState["hwayspecial_poid"].ToString();
             }
             if (ViewState["DeviceDefinitionType"] != null && ViewState["DeviceDefinitionType"] != "")
             {
                 DeviceDefinitionType = ViewState["DeviceDefinitionType"].ToString();
             }
             string PlanTypenew = "";
             if (radPlanAD.Checked == true)
             {
                 PlanTypenew = "GAD";
             }
             else if (radPlanADreg.Checked == true)
             {
                 PlanTypenew = "RAD";
             }

                 if (ViewState["JVFlag"].ToString() == "Y")
                 {
                     str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,BC_PRICE, broad_name, genre_type,var_plan_freeflag from(";
                     str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                     str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOJVPLAN_FETCH_ALL a ";
                     str += " where a.cityid ='" + city + "'";// and a.PLAN_TYPE='" + PlanTypenew + "' ";
                     {
                         str += " and a.plan_type not in ('B','HSP')";
                     }
                     str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                     str += " and lcocode=" + Session["lcoid"].ToString() 
                     str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                     str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                     str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_JVPLAN_FETCH_ALL a ";
                     str += " where a.cityid ='" + city + "'";// and a.PLAN_TYPE='" + PlanTypenew + "' ";
                     {
                         str += " and a.plan_type not in ('B','HSP')";
                     }
                     str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                     str +=  " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                     str += " and not EXISTS (select * from VIEW_LCOJVPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                     str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
                 
                 }
                 else
                 {

                     str = "SELECT plan_id, plan_name, plan_type, plan_poid, deal_poid,product_poid, cust_price, lco_price, payterm, cityid,company_code, insby, insdt, var_plan_devicetype,num_plan_sd_cnt, num_plan_hd_cnt,BC_PRICE, broad_name, genre_type,var_plan_freeflag from(";
                     str += " SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                     str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,a.BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_LCOPLAN_FETCH_ALL a ";
                     str += " where a.cityid ='" + city + "' ";//and a.PLAN_TYPE='" + PlanTypenew + "' ";
                     {
                         str += " and a.plan_type not in ('B','HSP')";
                     }
                     str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                     str += " and lcocode=" + Session["lcoid"].ToString();
                     str += " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                     str += " union SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, a.product_poid, a.cust_price, a.lco_price, a.payterm, ";
                     str += " a.cityid, a.company_code, a.insby, a.insdt,a.var_plan_devicetype,num_plan_sd_cnt,num_plan_hd_cnt,BC_PRICE, a.broad_name, a.genre_type,a.var_plan_freeflag FROM VIEW_PLAN_FETCH_ALL a ";
                     str += " where a.cityid ='" + city + "' ";//and a.PLAN_TYPE='" + PlanTypenew + "' ";
                     {
                         str += " and a.plan_type not in ('B','HSP')";
                     }
                     str += " and a.plan_poid not in (" + addon_poids + ") and a.dasarea='" + Session["dasarea"].ToString() + "'";
                     str +=  " and a.payterm ='" + rdbplanpayterm.SelectedValue.ToString().Trim() + "'";
                     str += " and not EXISTS (select * from VIEW_LCOPLAN_FETCH_ALL where plan_name=a.plan_name and plan_poid=a.plan_poid and lcocode=" + Session["lcoid"].ToString() + ")";
                     str += " )where (plAN_name not like'%NCF%' ) order by (case when plan_type='B' then 1 else 2 end) asc";
            
                 }*/

                if (ddlBC2.SelectedValue != "0")
                {
                    if (ddlBC2.SelectedItem.Text.ToString() != "")
                    {
                        strQuery = "   broad_name='" + ddlBC2.SelectedItem.Text.ToString() + "'  ";
                    }
                }
                if (ddlSDHD.SelectedValue != "0")
                {
                    if (strQuery != "")
                    {
                        strQuery += " and ";
                    }
                    strQuery += "  var_plan_devicetype='" + ddlSDHD.SelectedValue + "' ";
                }

                if (ddlPAYFREE.SelectedValue != "0")
                {
                    if (strQuery != "")
                    {
                        strQuery += " and ";
                    }
                    strQuery += " var_plan_freeflag='" + ddlPAYFREE.SelectedValue + "' ";
                }
                if (ddlGener.SelectedValue != "0")
                {
                    if (strQuery != "")
                    {
                        strQuery += " and ";
                    }
                    if (ddlGener.SelectedItem.Text.ToString() != "")
                    {
                        strQuery += " genre_type='" + ddlGener.SelectedItem.Text.ToString() + "' ";
                    }
                }
                //str2 = "select * from (" + str + ") where 1=1 " + strQuery;
            }
            else if (radPlanAD.Checked == true)
            {
                if (ddlBC.SelectedValue != "0")
                {
                    if (ddlBC.SelectedItem.Text.ToString() != "")
                    {
                        strQuery = " broad_name='" + ddlBC.SelectedItem.Text.ToString() + "'  ";
                    }
                }
            }
            //FileLogTextChange1("filter", "", "", strQuery);
            if (strQuery != "")
            {
                dv.RowFilter = strQuery;

                grdPlanChan.DataSource = dv;
                grdPlanChan.DataBind();
            }
            else
            {
                grdPlanChan.DataSource = dv;
                grdPlanChan.DataBind();
            }
            //(grdPlanChan.DataSource as DataTable).DefaultView.RowFilter = string.Format(strQuery);
            popAdd.Show();
            StatbleDynamicTabs();
            //Response.Redirect("~/Transaction/OnlineReqform.aspx");
        }

        protected void btnCustomerReceipt_Click(object sender, EventArgs e)
        {

            string VC_ID = lblVCID.Text;

            DataSet Customer_Receipt = new DataSet();
            DataTable PlanDetails = new DataTable();
            PlanDetails.Columns.Add(new DataColumn("PlanName"));
            PlanDetails.Columns.Add(new DataColumn("PlanType"));
            PlanDetails.Columns.Add(new DataColumn("LCOPrice", typeof(double)));
            PlanDetails.Columns.Add(new DataColumn("CustPrice", typeof(double)));
            PlanDetails.Columns.Add(new DataColumn("ChannelCount"));
            foreach (GridViewRow gvrow in grdBasicPlanDetails.Rows)
            {
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnBasicPlanName");
                HiddenField hdnBasicCustPrice = (HiddenField)gvrow.FindControl("hdnBasicCustPrice");
                HiddenField hdnBasicLcoPrice = (HiddenField)gvrow.FindControl("hdnBasicLcoPrice");
                HiddenField hdnBasicPlanType = (HiddenField)gvrow.FindControl("hdnBasicPlanType");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                DataRow dr = PlanDetails.NewRow();
                dr["PlanName"] = hdnBasicPlanName.Value;
                dr["PlanType"] = hdnBasicPlanType.Value;
                dr["LCOPrice"] = hdnBasicLcoPrice.Value;
                dr["CustPrice"] = hdnBasicCustPrice.Value;
                dr["ChannelCount"] = hdnChannelCount.Value;
                PlanDetails.Rows.Add(dr);

            }
            foreach (GridViewRow gvrow in Grdhathwayspecial.Rows)
            {
                HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                HiddenField hdnADCustPrice = (HiddenField)gvrow.FindControl("hdnADCustPrice");
                HiddenField hdnADLcoPrice = (HiddenField)gvrow.FindControl("hdnADLcoPrice");
                HiddenField hdnADPlanType = (HiddenField)gvrow.FindControl("hdnADPlanType");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                DataRow dr = PlanDetails.NewRow();
                dr["PlanName"] = hdnADPlanName.Value;
                dr["PlanType"] = hdnADPlanType.Value;
                dr["LCOPrice"] = hdnADLcoPrice.Value;
                dr["CustPrice"] = hdnADCustPrice.Value;
                dr["ChannelCount"] = hdnChannelCount.Value;
                PlanDetails.Rows.Add(dr);
            }
            foreach (GridViewRow gvrow in grdAddOnPlan.Rows)
            {
                HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                HiddenField hdnADCustPrice = (HiddenField)gvrow.FindControl("hdnADCustPrice");
                HiddenField hdnADLcoPrice = (HiddenField)gvrow.FindControl("hdnADLcoPrice");
                HiddenField hdnADPlanType = (HiddenField)gvrow.FindControl("hdnADPlanType");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                DataRow dr = PlanDetails.NewRow();
                dr["PlanName"] = hdnADPlanName.Value;
                dr["PlanType"] = hdnADPlanType.Value;
                dr["LCOPrice"] = hdnADLcoPrice.Value;
                dr["CustPrice"] = hdnADCustPrice.Value;
                dr["ChannelCount"] = hdnChannelCount.Value;
                PlanDetails.Rows.Add(dr);

            }

            foreach (GridViewRow gvrow in grdAddOnPlanReg.Rows)
            {
                HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                HiddenField hdnADCustPrice = (HiddenField)gvrow.FindControl("hdnADCustPrice");
                HiddenField hdnADLcoPrice = (HiddenField)gvrow.FindControl("hdnADLcoPrice");
                HiddenField hdnADPlanType = (HiddenField)gvrow.FindControl("hdnADPlanType");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                DataRow dr = PlanDetails.NewRow();
                dr["PlanName"] = hdnADPlanName.Value;
                dr["PlanType"] = hdnADPlanType.Value;
                dr["LCOPrice"] = hdnADLcoPrice.Value;
                dr["CustPrice"] = hdnADCustPrice.Value;
                dr["ChannelCount"] = hdnChannelCount.Value;
                PlanDetails.Rows.Add(dr);
            }

            foreach (GridViewRow gvrow in grdCarte.Rows)
            {
                HiddenField hdnADPlanName = (HiddenField)gvrow.FindControl("hdnALPlanName");
                HiddenField hdnADCustPrice = (HiddenField)gvrow.FindControl("hdnALCustPrice");
                HiddenField hdnADLcoPrice = (HiddenField)gvrow.FindControl("hdnALLcoPrice");
                HiddenField hdnADPlanType = (HiddenField)gvrow.FindControl("hdnALPlanType");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                DataRow dr = PlanDetails.NewRow();
                dr["PlanName"] = hdnADPlanName.Value;
                dr["PlanType"] = hdnADPlanType.Value;
                dr["LCOPrice"] = hdnADLcoPrice.Value;
                dr["CustPrice"] = hdnADCustPrice.Value;
                dr["ChannelCount"] = hdnChannelCount.Value;
                PlanDetails.Rows.Add(dr);
            }
            Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
            string conResult = obj1.LCODet(Session["username"].ToString());
            string[] result_arr = conResult.Split('^');


            String downloadpath = "Customer_Receipt_" + DateTime.Now.ToString("dd-MM-yyhhmmss") + ".pdf";
            String ReportPath = Server.MapPath("Customer_Receipt.rpt");
            String ExportPath = Server.MapPath("..\\MyExcelFile\\") + downloadpath;
            ReportDocument rpt = new ReportDocument();

            Customer_Receipt.Tables.Add(PlanDetails);
            PlanDetails.TableName = "PlanDetails";
            rpt.Load(ReportPath);
            rpt.SetDataSource(Customer_Receipt);
            rpt.SetParameterValue("LCOName", Convert.ToString(result_arr[0]));
            rpt.SetParameterValue("CIN", Convert.ToString(result_arr[1]));
            rpt.SetParameterValue("PAN", Convert.ToString(result_arr[2]));
            rpt.SetParameterValue("GSTN", Convert.ToString(result_arr[3]));

            rpt.SetParameterValue("AccountNo", Convert.ToString(Session["customer_no"]));
            rpt.SetParameterValue("VCID", VC_ID);
            rpt.SetParameterValue("CustMobileNo", Convert.ToString(ViewState["custmob"]));
            rpt.SetParameterValue("CustName", Convert.ToString(ViewState["customer_name"]));
            rpt.SetParameterValue("CustAddress", Convert.ToString(ViewState["custaddr"]));
            rpt.SetParameterValue("CustEmail", Convert.ToString(ViewState["custemail"]));
            rpt.SetParameterValue("RegisteredOffice", Convert.ToString(result_arr[4]));
            rpt.SetParameterValue("BranchOffice", Convert.ToString(result_arr[5]));
            rpt.SetParameterValue("LCOPhoneNo", Convert.ToString(result_arr[6]));
            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, ExportPath);
            rpt.Close();
            rpt.Dispose();
            StatbleDynamicTabs();

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + downloadpath);
            Response.TransmitFile(ExportPath);
            Response.End();

        }
        //---- All Cancel
        protected void btnBulkCancel_Click(object sender, EventArgs e)
        {
            DataTable dtRenew = new DataTable();
            dtRenew.Columns.Add(new DataColumn("PLAN_NAME"));
            dtRenew.Columns.Add(new DataColumn("CUST_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("LCO_PRICE", typeof(double)));
            dtRenew.Columns.Add(new DataColumn("ChannelCount"));
            dtRenew.Columns.Add(new DataColumn("PackageId"));
            dtRenew.Columns.Add(new DataColumn("Activation"));
            dtRenew.Columns.Add(new DataColumn("valid_upto"));
            dtRenew.Columns.Add(new DataColumn("plan_poid"));
            dtRenew.Columns.Add(new DataColumn("DEAL_POID"));
            dtRenew.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtRenew.Columns.Add(new DataColumn("PurchasePoid"));

            foreach (GridViewRow gvrow in grdBasicPlanDetails.Rows)
            {
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnBasicPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnBasicPlanPoid");
                HiddenField hdnBasicDealPoid = (HiddenField)gvrow.FindControl("hdnBasicDealPoid");
                HiddenField hdnBasicCustPrice = (HiddenField)gvrow.FindControl("hdnBasicCustPrice");
                HiddenField hdnBasicLcoPrice = (HiddenField)gvrow.FindControl("hdnBasicLcoPrice");
                HiddenField hdnBasicActivation = (HiddenField)gvrow.FindControl("hdnBasicActivation");
                HiddenField hdnBasicExpiry = (HiddenField)gvrow.FindControl("hdnBasicExpiry");
                HiddenField hdnBasicPackageId = (HiddenField)gvrow.FindControl("hdnBasicPackageId");
                HiddenField hdnBasicPurchasePoid = (HiddenField)gvrow.FindControl("hdnBasicPurchasePoid");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                HiddenField hdnPlanType = (HiddenField)gvrow.FindControl("hdnBasicPlanType");
                if (hdnBasicActivation.Value.ToString().ToUpper() == "ACTIVE")
                {
                    dtRenew.Rows.Add(hdnBasicPlanName.Value, hdnBasicCustPrice.Value, hdnBasicLcoPrice.Value, hdnChannelCount.Value, hdnBasicPackageId.Value, hdnBasicActivation.Value, hdnBasicExpiry.Value, hdnBasicPlanPoid.Value, hdnBasicDealPoid.Value, hdnPlanType.Value, hdnBasicPurchasePoid.Value);
                }
            }
            foreach (GridViewRow gvrow in Grdhathwayspecial.Rows)
            {
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                HiddenField hdnBasicDealPoid = (HiddenField)gvrow.FindControl("hdnADDealPoid");
                HiddenField hdnBasicCustPrice = (HiddenField)gvrow.FindControl("hdnADCustPrice");
                HiddenField hdnBasicLcoPrice = (HiddenField)gvrow.FindControl("hdnADLcoPrice");
                HiddenField hdnBasicActivation = (HiddenField)gvrow.FindControl("hdnADActivation");
                HiddenField hdnBasicExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                HiddenField hdnBasicPackageId = (HiddenField)gvrow.FindControl("hdnADPackageId");
                HiddenField hdnBasicPurchasePoid = (HiddenField)gvrow.FindControl("hdnADPurchasePoid");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                HiddenField hdnPlanType = (HiddenField)gvrow.FindControl("hdnADPlanType");
                if (hdnBasicActivation.Value.ToString().ToUpper() == "ACTIVE")
                {
                    dtRenew.Rows.Add(hdnBasicPlanName.Value, hdnBasicCustPrice.Value, hdnBasicLcoPrice.Value, hdnChannelCount.Value, hdnBasicPackageId.Value, hdnBasicActivation.Value, hdnBasicExpiry.Value, hdnBasicPlanPoid.Value, hdnBasicDealPoid.Value, hdnPlanType.Value, hdnBasicPurchasePoid.Value);
                }
            }

            foreach (GridViewRow gvrow in grdAddOnPlan.Rows)
            {
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                HiddenField hdnBasicDealPoid = (HiddenField)gvrow.FindControl("hdnADDealPoid");
                HiddenField hdnBasicCustPrice = (HiddenField)gvrow.FindControl("hdnADCustPrice");
                HiddenField hdnBasicLcoPrice = (HiddenField)gvrow.FindControl("hdnADLcoPrice");
                HiddenField hdnBasicActivation = (HiddenField)gvrow.FindControl("hdnADActivation");
                HiddenField hdnBasicExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                HiddenField hdnBasicPackageId = (HiddenField)gvrow.FindControl("hdnADPackageId");
                HiddenField hdnBasicPurchasePoid = (HiddenField)gvrow.FindControl("hdnADPurchasePoid");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                HiddenField hdnPlanType = (HiddenField)gvrow.FindControl("hdnADPlanType");
                dtRenew.Rows.Add(hdnBasicPlanName.Value, hdnBasicCustPrice.Value, hdnBasicLcoPrice.Value, hdnChannelCount.Value, hdnBasicPackageId.Value, hdnBasicActivation.Value, hdnBasicExpiry.Value, hdnBasicPlanPoid.Value, hdnBasicDealPoid.Value, hdnPlanType.Value, hdnBasicPurchasePoid.Value);
            }

            foreach (GridViewRow gvrow in grdAddOnPlanReg.Rows)
            {
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnADPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnADPlanPoid");
                HiddenField hdnBasicDealPoid = (HiddenField)gvrow.FindControl("hdnADDealPoid");
                HiddenField hdnBasicCustPrice = (HiddenField)gvrow.FindControl("hdnADCustPrice");
                HiddenField hdnBasicLcoPrice = (HiddenField)gvrow.FindControl("hdnADLcoPrice");
                HiddenField hdnBasicActivation = (HiddenField)gvrow.FindControl("hdnADActivation");
                HiddenField hdnBasicExpiry = (HiddenField)gvrow.FindControl("hdnADExpiry");
                HiddenField hdnBasicPackageId = (HiddenField)gvrow.FindControl("hdnADPackageId");
                HiddenField hdnBasicPurchasePoid = (HiddenField)gvrow.FindControl("hdnADPurchasePoid");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                HiddenField hdnPlanType = (HiddenField)gvrow.FindControl("hdnADPlanType");
                dtRenew.Rows.Add(hdnBasicPlanName.Value, hdnBasicCustPrice.Value, hdnBasicLcoPrice.Value, hdnChannelCount.Value, hdnBasicPackageId.Value, hdnBasicActivation.Value, hdnBasicExpiry.Value, hdnBasicPlanPoid.Value, hdnBasicDealPoid.Value, hdnPlanType.Value, hdnBasicPurchasePoid.Value);
            }

            foreach (GridViewRow gvrow in grdCarte.Rows)
            {
                HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnALPlanName");
                HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnALPlanPoid");
                HiddenField hdnBasicDealPoid = (HiddenField)gvrow.FindControl("hdnALDealPoid");
                HiddenField hdnBasicCustPrice = (HiddenField)gvrow.FindControl("hdnALCustPrice");
                HiddenField hdnBasicLcoPrice = (HiddenField)gvrow.FindControl("hdnALLcoPrice");
                HiddenField hdnBasicActivation = (HiddenField)gvrow.FindControl("hdnALActivation");
                HiddenField hdnBasicExpiry = (HiddenField)gvrow.FindControl("hdnALExpiry");
                HiddenField hdnBasicPackageId = (HiddenField)gvrow.FindControl("hdnALPackageId");
                HiddenField hdnBasicPurchasePoid = (HiddenField)gvrow.FindControl("hdnALPurchasePoid");
                HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                HiddenField hdnPlanType = (HiddenField)gvrow.FindControl("hdnALPlanType");
                dtRenew.Rows.Add(hdnBasicPlanName.Value, hdnBasicCustPrice.Value, hdnBasicLcoPrice.Value, hdnChannelCount.Value, hdnBasicPackageId.Value, hdnBasicActivation.Value, hdnBasicExpiry.Value, hdnBasicPlanPoid.Value, hdnBasicDealPoid.Value, hdnPlanType.Value, hdnBasicPurchasePoid.Value);
            }

            grdBulkCancel.DataSource = dtRenew;
            grdBulkCancel.DataBind();
            foreach (GridViewRow gvrow in grdBulkCancel.Rows)
            {
                HiddenField hdnPlanType = (HiddenField)gvrow.FindControl("hdnPlanType");
                CheckBox ch = (CheckBox)gvrow.FindControl("chkBulkCancel");
                if (hdnPlanType.Value == "NCF")
                {
                    ch.Enabled = false;
                }
                ch.Checked = true;
            }
            choBulkCancel.Checked = true;
            popBulkCancel.Show();
            StatbleDynamicTabs();

        }

        protected void lnkView_click(object sender, EventArgs e)
        {

            int rindex = (((GridViewRow)(((CheckBox)(sender)).Parent.BindingContainer))).RowIndex;
            CheckBox chkBulkCancel = (CheckBox)grdBulkCancel.Rows[rindex].FindControl("chkBulkCancel");
            if (chkBulkCancel.Checked == true)
            {
                HiddenField hdnPlanType = (HiddenField)grdBulkCancel.Rows[rindex].FindControl("hdnPlanType");
                if (hdnPlanType.Value == "B" || hdnPlanType.Value == "HSP")
                {
                    foreach (GridViewRow gvrow in grdBulkCancel.Rows)
                    {
                        HiddenField hdnPlanType2 = (HiddenField)gvrow.FindControl("hdnPlanType");
                        if (hdnPlanType2.Value == "NCF")
                        {
                            CheckBox ch = (CheckBox)gvrow.FindControl("chkBulkCancel");
                            ch.Checked = true;
                            ch.Enabled = false;
                        }
                    }
                }
            }
            else if (chkBulkCancel.Checked == false)
            {
                HiddenField hdnPlanType = (HiddenField)grdBulkCancel.Rows[rindex].FindControl("hdnPlanType");
                if (hdnPlanType.Value == "B" || hdnPlanType.Value == "HSP")
                {
                    foreach (GridViewRow gvrow in grdBulkCancel.Rows)
                    {
                        HiddenField hdnPlanType2 = (HiddenField)gvrow.FindControl("hdnPlanType");
                        if (hdnPlanType2.Value == "NCF")
                        {
                            CheckBox ch = (CheckBox)gvrow.FindControl("chkBulkCancel");
                            ch.Checked = false;
                            ch.Enabled = false;
                        }
                    }
                }
            }
            popBulkCancel.Show();
            StatbleDynamicTabs();
        }

        protected void btnCancelAll_Click(object sender, EventArgs e)
        {
            try
            {
                string _tvType = "";
                int Error_break = 0;
                DataTable sortedDT = (DataTable)ViewState["vcdetail"];
                DataTable myResultSet = sortedDT.Select("TAB_FLAG='" + hdntag.Value + "'").CopyToDataTable();
                string _vc_id = myResultSet.Rows[0]["VC_ID"].ToString();
                string service_poid = "", vc = "", stb_status = "";
                string cust_services = ViewState["Service_Str"].ToString();
                string[] service_arr = cust_services.Split('^');
                int k = 0;

                int a = service_arr.Length; //3

                string[] updated_service_arr = new string[a]; //3
                foreach (string SerivceIndexs in service_arr)
                {

                    string Tv_Type_check = SerivceIndexs.Split('!')[6];
                    if (Tv_Type_check == "0")
                    {
                        updated_service_arr[a - 1] = SerivceIndexs;
                    }
                    else
                    {
                        updated_service_arr[k] = SerivceIndexs;
                        k = k + 1;
                    }
                }
                foreach (string service in updated_service_arr)
                {
                    if (Error_break > 0)
                    {
                        break;
                    }
                    vc = service.Split('!')[2];
                    if (vc == _vc_id)
                    {
                        service_poid = service.Split('!')[0];
                        stb_status = service.Split('!')[4];
                    }
                }
                foreach (GridViewRow gvrow in grdBulkCancel.Rows)
                {
                    CheckBox seleced = (CheckBox)gvrow.FindControl("chkBulkCancel");
                    if (seleced.Checked == true)
                    {
                        Hashtable htData = new Hashtable();
                        HiddenField hdnBasicPlanName = (HiddenField)gvrow.FindControl("hdnPlanName");
                        HiddenField hdnBasicPlanPoid = (HiddenField)gvrow.FindControl("hdnPlanPoid");
                        HiddenField hdnBasicDealPoid = (HiddenField)gvrow.FindControl("hdnDealPoid");
                        HiddenField hdnBasicCustPrice = (HiddenField)gvrow.FindControl("hdnCustPrice");
                        HiddenField hdnBasicLcoPrice = (HiddenField)gvrow.FindControl("hdnLcoPrice");
                        HiddenField hdnBasicActivation = (HiddenField)gvrow.FindControl("hdnActivation");
                        HiddenField hdnBasicExpiry = (HiddenField)gvrow.FindControl("hdnExpiry");
                        HiddenField hdnBasicPackageId = (HiddenField)gvrow.FindControl("hdnPackageId");
                        HiddenField hdnBasicPurchasePoid = (HiddenField)gvrow.FindControl("hdnPurchasePoid");
                        HiddenField hdnChannelCount = (HiddenField)gvrow.FindControl("hdnChannelCount");
                        HiddenField hdnPlanType = (HiddenField)gvrow.FindControl("hdnPlanType");
                        htData["planname"] = hdnBasicPlanName.Value;
                        htData["planpoid"] = hdnBasicPlanPoid.Value;
                        htData["dealpoid"] = hdnBasicDealPoid.Value;
                        htData["custprice"] = hdnBasicCustPrice.Value;
                        htData["lcoprice"] = hdnBasicLcoPrice.Value;
                        htData["activation"] = hdnBasicActivation.Value;
                        htData["expiry"] = hdnBasicExpiry.Value;
                        htData["packageid"] = hdnBasicPackageId.Value;
                        htData["purchasepoid"] = hdnBasicPurchasePoid.Value;
                        htData["IP"] = "";
                        htData["autorenew"] = "N";
                        string conResult = callGetProviConfirm_CancellationProcess(htData, "C");
                        string[] result_arr = conResult.Split('#');
                        if (result_arr[0].ToString() != "9999")
                        {
                            /* DataRow row1 = dtFOCPlanStatus.NewRow();
                             row1["PlanName"] = htData["planname"].ToString();
                             row1["RenewStatus"] = result_arr[1].ToString();
                             dtFOCPlanStatus.Rows.Add(row1);*/

                            DataRow row1 = dtPlanStatus.NewRow();
                            row1["VCID"] = _vc_id;
                            row1["PlanName"] = htData["planname"].ToString();
                            row1["Status"] = result_arr[1].ToString();
                            dtPlanStatus.Rows.Add(row1);

                            continue;
                        }
                        else
                        {
                            string[] conResult_arr = result_arr[1].Split('$');
                            htData["refund_amt"] = conResult_arr[1];
                            htData["days_left"] = conResult_arr[0];
                            ViewState["transaction_data"] = htData;
                            hdnPopupAction.Value = "C";
                            if (myResultSet.Rows[0]["TAB_FLAG"].ToString() == "lnkAddon1")
                            {
                                _tvType = "MAIN";
                            }
                            else
                            {
                                _tvType = "CHILD";
                            }
                            processTransaction_cancellation(_vc_id, _tvType, stb_status, service_poid);

                            DataRow row2 = dtPlanStatus.NewRow();
                            ViewState["VCFREE"] = _vc_id;
                            row2["VCID"] = _vc_id;
                            row2["PlanName"] = htData["planname"].ToString();
                            row2["Status"] = ViewState["ErrorMessage"].ToString();
                            dtPlanStatus.Rows.Add(row2);
                        }
                    }
                }
                grdAllCancel.DataSource = dtPlanStatus;
                grdAllCancel.DataBind();
                popMsg.Hide();
                popallCancel.Show();
                StatbleDynamicTabs();
            }
            catch (Exception ex)
            {

            }
        }

        protected void choBulkCancel_click(object sender, EventArgs e)
        {
            if (choBulkCancel.Checked == true)
            {
                foreach (GridViewRow gvrow in grdBulkCancel.Rows)
                {
                    CheckBox ch = (CheckBox)gvrow.FindControl("chkBulkCancel");
                    ch.Checked = true;
                }
            }
            else if (choBulkCancel.Checked == false)
            {
                foreach (GridViewRow gvrow in grdBulkCancel.Rows)
                {
                    CheckBox ch = (CheckBox)gvrow.FindControl("chkBulkCancel");
                    ch.Checked = false;
                }
            }
            popBulkCancel.Show();
            StatbleDynamicTabs();
        }

        //---
    }
}