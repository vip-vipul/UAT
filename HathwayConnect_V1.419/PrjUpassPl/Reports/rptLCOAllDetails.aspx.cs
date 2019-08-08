using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;
using PrjUpassPl.Helper;
using PrjUpassBLL.Reports;
using System.IO;
using System.Configuration;
using System.Data.OracleClient;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Reports
{
    public partial class rptLCOAllDetails : System.Web.UI.Page
    {
        decimal awailbal = 0;
        decimal amtdd = 0;
        decimal bal = 0;
        decimal amt = 0;
        decimal revamt = 0;
        string type;
        string operid;
        string username;
        string catid;

        decimal led_opnbal = 0;
        decimal led_debit = 0;
        decimal led_credit = 0;
        decimal led_closebal = 0;

        decimal led_opnbal1 = 0;
        decimal led_debit1 = 0;
        decimal led_credit1 = 0;
        decimal led_closebal1 = 0;
        string opertorid = "";

        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "LCO Details";
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                Session["pagenos"] = "0";

                operid = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                //catid = "3";

                FillLcoDetails();
            }
        }

        protected void FillLcoDetails()
        {
            string str = "";

            // Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
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

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,var_lcomst_code lcocode, num_lcomst_operid||'#'||var_lcomst_code opid ";  //num_lcomst_operid
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
                    ddlLco.DataValueField = "opid";

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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchOperators(string prefixText, int count, string contextKey)
        {
            string Str = prefixText.Trim();
            double Num;
            bool isNum = double.TryParse(Str, out Num);
            //if (!isNum)
            //{
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";

            string catid = "";
            string operid = "";
            if (HttpContext.Current.Session["category"] != null && HttpContext.Current.Session["operator_id"] != null)
            {
                catid = HttpContext.Current.Session["category"].ToString();
                operid = HttpContext.Current.Session["operator_id"].ToString();
            }

            str = "SELECT lconame, lcoid, lcocode, awailbal  ";
            str += " FROM view_lcoall_det_summ ";

            if (contextKey == "0")
            {
                str += " where upper(lcocode) like upper('" + prefixText.ToString() + "%')";
            }
            else if (contextKey == "1")
            {
                str += " where upper(lconame) like  upper('" + prefixText.ToString() + "%')";
            }
            if (catid == "2")
            {
                str += "  and PARENTID= '" + operid + "' ";
            }
            else if (catid == "5")
            {
                str += "  and DISTID= '" + operid + "' ";
            }
            else if (catid == "3")
            {
                str += "  and OPERID= '" + operid + "' ";
            }
            else if (catid == "10")
            {
                str += "  and hoid= '" + operid + "' ";
            }
            else
            {
            }

            OracleCommand cmd = new OracleCommand(str, con);


            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                if (contextKey == "0")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["lcocode"].ToString(), dr["lcocode"].ToString());
                    Operators.Add(item);
                }
                else if (contextKey == "1")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                            dr["lconame"].ToString(), dr["lconame"].ToString());
                    Operators.Add(item);
                }
            }
            con.Close();
            con.Dispose();
            return Operators;
        }

        private Hashtable getLcoAllParamsData()
        {
            string lcd = "", lnm = "", txtsear = "";
            txtsear = ddlLco.SelectedValue.Split('#')[1].ToString();

            //switch (rdolstSubsSearch.SelectedIndex)
            //{
            //    case 0:
            //        lcd = rdolstSubsSearch.SelectedIndex.ToString();
            //        break;
            //    case 1:
            //        lnm = rdolstSubsSearch.SelectedIndex.ToString();
            //        break;
            //    default:
            //        break;
            //}



            Hashtable htLcoAllParams = new Hashtable();
            htLcoAllParams.Add("lcd", "0");
            htLcoAllParams.Add("lnm", lnm);
            htLcoAllParams.Add("txtsear", txtsear);
            return htLcoAllParams;
        }

        protected void BindDataLco()
        {
            Hashtable htLcoAllParams = getLcoAllParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();//Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptLcoAll objTran = new Cls_Business_RptLcoAll();
            Hashtable htResponse = objTran.GetTransationsLcoDet(htLcoAllParams, username, catid, Session["operatorid"].ToString());

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            //showing parameters
            string strParams = htResponse["ParamStr"].ToString();
            if (!String.IsNullOrEmpty(strParams))
            {
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>Top-up Parameters : </b>" + strParams);
                //lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);
            }

            if (dt.Rows.Count == 0)
            {
                //grdLcoDet.Visible = false;
                //lbllcodet.Text = "No data found";
                //btngrnExel.Visible = true;
            }
            else
            {
                //btngrnExel.Visible = true;
                //grdLcoDet.Visible = true;
                //lbllcodet.Text = "";
                ViewState["searched_trans"] = dt;
                //grdLcoDet.DataSource = dt;
                //grdLcoDet.DataBind();
            }
        }

        public void BindDataLast()
        {
            Hashtable htLcoAllParams = getLcoAllParamsData();
            string username, catid, operator_id;

            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();

                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();//Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptLcoAll objTran = new Cls_Business_RptLcoAll();
            Hashtable htResponse = objTran.GetTransationsLastF(htLcoAllParams, username, catid, Session["operatorid"].ToString());

            DataTable dt = null; //check for exception
            if (htResponse["htResponse1"] != null)
            {
                dt = (DataTable)htResponse["htResponse1"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            //showing parameters
            string strParams = htResponse["ParamStr1"].ToString();
            if (!String.IsNullOrEmpty(strParams))
            {
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>LCO Party Ledger Parameters : </b>" + strParams);
                //lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt.Rows.Count == 0)
            {
                //btngrnExel.Visible = true;
                grdLastFive.Visible = false;
                //LastFiveAccordion.Visible = false;
                lbllastfive.Text = "No data found";
            }
            else
            {
                //btngrnExel.Visible = true;
                grdLastFive.Visible = true;
                //LastFiveAccordion.Visible = true;
                //LastFiveAccordion.SelectedIndex = -1;
                lbllastfive.Text = "";
                ViewState["searched_trans1"] = dt;
                grdLastFive.DataSource = dt;
                grdLastFive.DataBind();

                //showing result count
                ////lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        public void BindDataTopup()
        {
            Hashtable htLcoAllParams = getLcoAllParamsData();
            string username, catid, operator_id;

            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();

                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();//Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptLcoAll objTran = new Cls_Business_RptLcoAll();
            Hashtable htResponse = objTran.GetTransationsTop(htLcoAllParams, username, catid, Session["operatorid"].ToString());

            DataTable dt = null; //check for exception
            if (htResponse["htResponse2"] != null)
            {
                dt = (DataTable)htResponse["htResponse2"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            //showing parameters
            string strParams = htResponse["ParamStr2"].ToString();
            if (!String.IsNullOrEmpty(strParams))
            {
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>LCO Party Ledger Parameters : </b>" + strParams);
                //lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt.Rows.Count == 0)
            {
                //btngrnExel.Visible = true;
                grdTopup.Visible = false;
                //TopupAccordion.Visible = false;
                lbltop.Text = "No data found";
            }
            else
            {
                //btngrnExel.Visible = true;
                grdTopup.Visible = true;
                //TopupAccordion.Visible = true;
                //TopupAccordion.SelectedIndex = -1;
                lbltop.Text = "";
                ViewState["searched_trans"] = dt;
                grdTopup.DataSource = dt;
                grdTopup.DataBind();

            }
        }

        public void BindDataRevers()
        {
            Hashtable htLcoAllParams = getLcoAllParamsData();
            string username, catid, operator_id;

            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();//Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptLcoAll objTran = new Cls_Business_RptLcoAll();
            Hashtable htResponse = objTran.GetTransationsRevrs(htLcoAllParams, username, catid, Session["operatorid"].ToString());

            DataTable dt = null; //check for exception
            if (htResponse["htResponse3"] != null)
            {
                dt = (DataTable)htResponse["htResponse3"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            //showing parameters
            string strParams = htResponse["ParamStr3"].ToString();
            if (!String.IsNullOrEmpty(strParams))
            {
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>LCO Party Ledger Parameters : </b>" + strParams);
                //lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);
            }

            if (dt.Rows.Count == 0)
            {
                //btngrnExel.Visible = true;
                grdReversal.Visible = false;
                //ReversalAccordion.Visible = false;
                lblrevrs.Text = "No data found";
            }
            else
            {
                //btngrnExel.Visible = true;
                grdReversal.Visible = true;
                //ReversalAccordion.Visible = true;
                //ReversalAccordion.SelectedIndex = -1;
                lblrevrs.Text = "";
                ViewState["searched_trans"] = dt;
                grdReversal.DataSource = dt;
                grdReversal.DataBind();

                //showing result count
                ////lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        public void BindLedgerData()
        {
            string lcoid = hdnLcoId.Value;
            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();//Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            //ledger grid 1
            Cls_Business_RptLcoAll objTran = new Cls_Business_RptLcoAll();
            DataTable dt = objTran.GetLedgerData(lcoid, username, catid, Session["operatorid"].ToString());

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                grdPartyLed.Visible = false;
                lblPartyLed.Text = "No data found";
            }
            else
            {
                grdPartyLed.Visible = true;
                lblPartyLed.Text = "";
                grdPartyLed.DataSource = dt;
                grdPartyLed.DataBind();
            }
            //ledger grid 2
            Cls_Business_RptLcoAll objTran2 = new Cls_Business_RptLcoAll();
            DataTable dt2 = objTran.GetLedgerDet(lcoid, username);

            if (dt2 == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            if (dt2.Rows.Count == 0)
            {
                grdPartyLedDet.Visible = false;
            }
            else
            {
                grdPartyLedDet.Visible = true;
                grdPartyLedDet.DataSource = dt2;
                grdPartyLedDet.DataBind();
            }
        }

        public void BindServiceData()
        {
            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();//Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            Cls_Business_RptLcoAll objTran = new Cls_Business_RptLcoAll();
            DataTable dt = objTran.GetServiceData(username, catid, Session["operatorid"].ToString());
            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                grdactdact.Visible = false;
                lblServiceData.Text = "No data found";
            }
            else
            {
                grdactdact.Visible = true;
                lblServiceData.Text = "";
                grdactdact.DataSource = dt;
                grdactdact.DataBind();
            }
        }

        public void BindUserData()
        {
            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();//Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            Cls_Business_RptLcoAll objTran = new Cls_Business_RptLcoAll();
            DataTable dt = objTran.GetUserDet(username, catid, Session["operatorid"].ToString());
            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                grdUserDet.Visible = false;
                lblUserDet.Text = "No data found";
            }
            else
            {
                grdUserDet.Visible = true;
                lblUserDet.Text = "";
                grdUserDet.DataSource = dt;
                grdUserDet.DataBind();
            }
        }

        public void msgbox(string message, Control ctrl)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
            ctrl.Focus();
        }

        protected void getLcoBalanceDetails()
        {
            Cls_Bussiness_TransHwayUserCreditLimit balObj = new Cls_Bussiness_TransHwayUserCreditLimit();
            string username = "";
            string category_id = "";
            string user_id = "";
            string operator_id = "";
            if (Session["username"] != null && Session["category"] != null && Session["user_id"] != null && Session["operatorid"] != null)
            {
                username = Session["username"].ToString();
                category_id = Session["category"].ToString();
                user_id = Session["user_id"].ToString();
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();//Session["operatorid"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            string[] avail_bal = balObj.GetAvailBal(username, category_id, user_id, operator_id);
            if (avail_bal.Length != 0)
            {
                lblUnallocatedBal.Text = avail_bal[0].Trim();
                // lbltotalbalance.Text = avail_bal[1].Trim();
                lblAllocatedBal.Text = avail_bal[2].Trim();
            }
            else
            {
                lblUnallocatedBal.Text = "0";
                lblAllocatedBal.Text = "0";
            }

        }

        public void ShowLCODetails()
        {
            if (ddlLco.SelectedValue.Split('#')[1].ToString() != "")
            {
                Cls_Business_RptLcoAll obj = new Cls_Business_RptLcoAll();
                string[] responseStr = obj.getLcoDatadetails(username, ddlLco.SelectedValue.Split('#')[1].ToString(), "0", operid, catid);
                if (responseStr.Length != 0)
                {
                    lblLCONo.Text = responseStr[0].Trim();
                    lblLCOName.Text = responseStr[1].Trim();
                    lblLCOAddr.Text = responseStr[2].Trim();
                    lblmobno.Text = responseStr[3].Trim();
                    lblEmail.Text = responseStr[4].Trim();
                    lblCurrBalance.Text = responseStr[5].Trim();
                    hdnLcoId.Value = responseStr[6].Trim();
                    Session["operatorid"] = responseStr[7].Trim();
                    lblstate.Text = responseStr[8].Trim();
                    lblcity.Text = responseStr[9].Trim();
                    lbldist.Text = responseStr[10].Trim();
                    lblsubdist.Text = responseStr[11].Trim();
                    lbljvno.Text = responseStr[12].Trim();
                    lblerpaccno.Text = responseStr[13].Trim();
                    if (responseStr[14].Trim() != "")
                    {
                        if (responseStr[14].Trim() == "Y")
                        {
                            lblecsstatus.Text = "Active";
                        }
                        else
                        {
                            lblecsstatus.Text = "Inactive";
                        }
                    }
                    lblAreaM.Text = responseStr[15].Trim();
                    lblPtexdt.Text = responseStr[16].Trim();
                    lblIntagreedt.Text = responseStr[17].Trim();
                    lblExec.Text = responseStr[18].Trim();


                    //divLcoDet.Visible = true;
                    LCOAccordion.Visible = true;
                    getLcoBalanceDetails();
                    divLcoAllSearch.Visible = true;
                }
                else
                {
                    msgbox("No Such LCO Found", ddlLco);
                    LCOAccordion.Visible = false;
                    return;
                }
            }
            else
            {
                msgbox("Please Select LCO by Code or Name", ddlLco);
                LCOAccordion.Visible = false;
                return;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Label1.Visible = true;
            //Label2.Visible = true;
            //Label3.Visible = true;
            //Label4.Visible = true;
            ShowLCODetails();
            BindDataLco();
            BindDataLast();
            BindDataTopup();
            BindDataRevers();
            BindLedgerData();
            BindServiceData();
            BindUserData();
            //DetailsTab.Visible = true;
        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            Hashtable htLcoAllParams = getLcoAllParamsData();

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

            Cls_Business_RptLcoAll objTran = new Cls_Business_RptLcoAll();
            Hashtable htResponse = objTran.GetTransationsLcoDet(htLcoAllParams, username, catid, operator_id);


            DateTime dd = DateTime.Now;
            string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

            StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "LcoAllDet_" + datetime + ".xls");
            try
            {
                DataTable dt = null; //check for exception
                if (htResponse["htResponse"] != null)
                {
                    dt = (DataTable)htResponse["htResponse"];

                    //-----------------------------------------------------LCO Details :-------------------------------------------------------

                    int i = 0;
                    String strheader0 = "LCO Details :";
                    String strheader = "Sr.No." + Convert.ToChar(9) + "LCO Code" + Convert.ToChar(9) + "LCO Name" + Convert.ToChar(9) + "Awailable Balance" + Convert.ToChar(9);

                    while (i < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader0);
                        sw.WriteLine(strheader);
                        for (int ii = 0; ii < dt.Rows.Count; ii++)
                        {
                            i = i + 1;
                            string strrow = i.ToString() + Convert.ToChar(9) + dt.Rows[ii]["lcocode"].ToString() + Convert.ToChar(9) + dt.Rows[ii]["lconame"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[ii]["awailbal"].ToString() + Convert.ToChar(9);
                            sw.WriteLine(strrow);
                        }
                    }
                }
                if (dt == null)
                {
                }
                //}
                /*if (dt == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }*/
                //-----------------------------------------------------Customer Last Five Transaction :-------------------------------------------------------

                DataTable dt1 = null; //check for exception

                Cls_Business_RptLcoAll objTran1 = new Cls_Business_RptLcoAll();
                Hashtable htResponse1 = objTran1.GetTransationsLastF(htLcoAllParams, username, catid, operator_id);
                if (htResponse1["htResponse1"] != null)
                {
                    dt1 = (DataTable)htResponse1["htResponse1"];
                    int j = 0;
                    String strheader011 = "Customer Last Five Transaction :";
                    String strheader1 = "Sr.No." + Convert.ToChar(9) + "Customer ID" + Convert.ToChar(9) + "Customer Name" + Convert.ToChar(9) + "Customer Address" + Convert.ToChar(9)
                        + "VC" + Convert.ToChar(9) + "Plan Name" + Convert.ToChar(9) + "Plan Type" + Convert.ToChar(9);
                    strheader1 += "User ID" + Convert.ToChar(9) + "User Name" + Convert.ToChar(9) + "'" + "Transaction Date & Time" + Convert.ToChar(9) + "Amount deducted" + Convert.ToChar(9) + "'" + "Expiry date" + Convert.ToChar(9);
                    strheader1 += "Pay Term" + Convert.ToChar(9) + "Balance" + Convert.ToChar(9) + "LCO Code" + Convert.ToChar(9) + "LCO Name" + Convert.ToChar(9) + "JV Name" + Convert.ToChar(9);
                    strheader1 += "ERP LCO A/C" + Convert.ToChar(9) + "Distributor" + Convert.ToChar(9) + "Sub distributor" + Convert.ToChar(9);
                    strheader1 += "City" + Convert.ToChar(9) + "State" + Convert.ToChar(9);

                    while (j < dt1.Rows.Count)
                    {
                        sw.WriteLine();
                        sw.WriteLine(strheader011);
                        sw.WriteLine(strheader1);

                        for (int jj = 0; jj < dt1.Rows.Count; jj++)
                        {
                            j = j + 1;
                            string strrow1 = j.ToString() + Convert.ToChar(9) + "'" + dt1.Rows[jj]["custid"].ToString() + Convert.ToChar(9)
                                + dt1.Rows[jj]["custname"].ToString() + Convert.ToChar(9)
                                + dt1.Rows[jj]["custaddr"].ToString() + Convert.ToChar(9)
                                + dt1.Rows[jj]["vc"].ToString() + Convert.ToChar(9);
                            strrow1 += dt1.Rows[jj]["plnname"].ToString() + Convert.ToChar(9) + dt1.Rows[jj]["plntyp"].ToString() + Convert.ToChar(9);
                            strrow1 += dt1.Rows[jj]["uname"].ToString() + Convert.ToChar(9) + dt1.Rows[jj]["userowner"].ToString() + Convert.ToChar(9) + "'" + dt1.Rows[jj]["tdt"].ToString() + Convert.ToChar(9);
                            strrow1 += dt1.Rows[jj]["amtdd"].ToString() + Convert.ToChar(9) + "'" + dt1.Rows[jj]["expdt"].ToString() + Convert.ToChar(9);
                            strrow1 += dt1.Rows[jj]["payterm"].ToString() + Convert.ToChar(9) + dt1.Rows[jj]["bal"].ToString() + Convert.ToChar(9);
                            strrow1 += dt1.Rows[jj]["lcocode"].ToString() + Convert.ToChar(9) + dt1.Rows[jj]["lconame"].ToString() + Convert.ToChar(9);
                            strrow1 += dt1.Rows[jj]["jvname"].ToString() + Convert.ToChar(9) + dt1.Rows[jj]["erplco_ac"].ToString() + Convert.ToChar(9);
                            strrow1 += dt1.Rows[jj]["distname"].ToString() + Convert.ToChar(9) + dt1.Rows[jj]["subdist"].ToString() + Convert.ToChar(9);
                            strrow1 += dt1.Rows[jj]["city"].ToString() + Convert.ToChar(9) + dt1.Rows[jj]["state"].ToString() + Convert.ToChar(9);
                            sw.WriteLine(strrow1);

                        }
                    }
                    //}
                }
                if (dt == null)
                {
                }
                /*if (dt1 == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }*/
                //-----------------------------------------------------Receipt Entry Details :-------------------------------------------------------
                DataTable dt2 = null; //check for exception
                //if (htResponse2["htResponse2"] != null)
                //{
                Cls_Business_RptLcoAll objTran2 = new Cls_Business_RptLcoAll();
                Hashtable htResponse2 = objTran2.GetTransationsTop(htLcoAllParams, username, catid, operator_id);
                if (htResponse2["htResponse2"] != null)
                {
                    dt2 = (DataTable)htResponse2["htResponse2"];
                    int k = 0;
                    String strheader022 = "Receipt Entry Details :";
                    String strheader2 = "Sr.No." + Convert.ToChar(9) + "Date & Time" + Convert.ToChar(9) + "Amount" + Convert.ToChar(9) + "Mode of payment" + Convert.ToChar(9) + "ERP Receipt No." + Convert.ToChar(9);
                    strheader2 += "UPASS Transaction ID" + Convert.ToChar(9) + "Finance user id" + Convert.ToChar(9) + "Finance user name" + Convert.ToChar(9) + "Action" + Convert.ToChar(9);
                    strheader2 += "LCO Code" + Convert.ToChar(9) + "LCO Name" + Convert.ToChar(9) + "JV Name" + Convert.ToChar(9) + "ERP LCO A/C" + Convert.ToChar(9) + "Distributor" + Convert.ToChar(9);
                    strheader2 += "Sub distributor" + Convert.ToChar(9) + "City" + Convert.ToChar(9) + "State" + Convert.ToChar(9);

                    while (k < dt2.Rows.Count)
                    {
                        sw.WriteLine();
                        sw.WriteLine(strheader022);
                        sw.WriteLine(strheader2);

                        for (int kk = 0; kk < dt2.Rows.Count; kk++)
                        {
                            k = k + 1;
                            string strrow2 = k.ToString() + Convert.ToChar(9) + "'" + dt2.Rows[kk]["dtttime"].ToString() + Convert.ToChar(9) + dt2.Rows[kk]["amt"].ToString() + Convert.ToChar(9);
                            strrow2 += dt2.Rows[kk]["paymode"].ToString() + Convert.ToChar(9) + dt2.Rows[kk]["erprcptno"].ToString() + Convert.ToChar(9);
                            strrow2 += dt2.Rows[kk]["rcptno"].ToString() + Convert.ToChar(9) + dt2.Rows[kk]["finuid"].ToString() + Convert.ToChar(9);
                            strrow2 += dt2.Rows[kk]["fiuname"].ToString() + Convert.ToChar(9) + dt2.Rows[kk]["action"].ToString() + Convert.ToChar(9);
                            strrow2 += dt2.Rows[kk]["lcocode"].ToString() + Convert.ToChar(9) + dt2.Rows[kk]["lconame"].ToString() + Convert.ToChar(9);
                            strrow2 += dt2.Rows[kk]["jvname"].ToString() + Convert.ToChar(9) + dt2.Rows[kk]["erplco_ac"].ToString() + Convert.ToChar(9);
                            strrow2 += dt2.Rows[kk]["distname"].ToString() + Convert.ToChar(9) + dt2.Rows[kk]["subdist"].ToString() + Convert.ToChar(9);
                            strrow2 += dt2.Rows[kk]["city"].ToString() + Convert.ToChar(9) + dt2.Rows[kk]["state"].ToString() + Convert.ToChar(9);
                            sw.WriteLine(strrow2);

                        }
                    }
                }
                if (dt2 == null)
                {
                }
                //}
                /*if (dt2 == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }*/
                //-----------------------------------------------------Receipt Entry Reversal :-------------------------------------------------------
                DataTable dt3 = null; //check for exception
                //if (htResponse3["htResponse3"] != null)
                //{

                Cls_Business_RptLcoAll objTran3 = new Cls_Business_RptLcoAll();
                Hashtable htResponse3 = objTran3.GetTransationsRevrs(htLcoAllParams, username, catid, operator_id);
                if (htResponse3["htResponse3"] != null)
                {
                    dt3 = (DataTable)htResponse3["htResponse3"];
                    int l = 0;
                    String strheader033 = "Receipt Entry Reversal :";
                    String strheader3 = "Sr.No." + Convert.ToChar(9) + "LCO Code" + Convert.ToChar(9) + "Company Code" + Convert.ToChar(9) + "Voucher NO." + Convert.ToChar(9) + "Amount" + Convert.ToChar(9);
                    strheader3 += "Reason" + Convert.ToChar(9) + "Remark" + Convert.ToChar(9) + "Inserted By" + Convert.ToChar(9) + "Inserted Date";

                    while (l < dt3.Rows.Count)
                    {
                        sw.WriteLine();
                        sw.WriteLine(strheader033);
                        sw.WriteLine(strheader3);

                        for (int ll = 0; ll < dt3.Rows.Count; ll++)
                        {
                            l = l + 1;
                            string strrow4 = l.ToString() + Convert.ToChar(9) + "'" + dt3.Rows[ll]["lcocode"].ToString() + Convert.ToChar(9) + dt3.Rows[ll]["companycode"].ToString() + Convert.ToChar(9);
                            strrow4 += dt3.Rows[ll]["voucherno"].ToString() + Convert.ToChar(9) + dt3.Rows[ll]["amount"].ToString() + Convert.ToChar(9);
                            strrow4 += dt3.Rows[ll]["reasonname"].ToString() + Convert.ToChar(9) + dt3.Rows[ll]["lcopayremark"].ToString() + Convert.ToChar(9);
                            strrow4 += dt3.Rows[ll]["insby"].ToString() + Convert.ToChar(9) + dt3.Rows[ll]["date1"].ToString() + Convert.ToChar(9);
                            sw.WriteLine(strrow4);

                        }
                    }
                }
                if (dt3 == null)
                {
                }
                //}
                //}
                //}
                //}
                /*if (dt3 == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }*/
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
            Response.Redirect("../MyExcelFile/" + "LcoAllDet_" + datetime + ".xls");
        }

        protected void grdLcoDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
                {
                    //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
                    awailbal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "awailbal"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Text = "" + awailbal;
                    //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
                    //e.Item.Cells[8].Text = "" + Total;
                }
            }
        }

        protected void grdLcoDet_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable;
            string sort_direction;
            if (ViewState[e.SortExpression.ToString()] == null)
            {
                ViewState[e.SortExpression.ToString()] = Convert.ToString(e.SortDirection);
                sort_direction = Convert.ToString(e.SortDirection);
            }
            else
            {
                if (ViewState[e.SortExpression.ToString()].ToString() == "Ascending")
                {
                    ViewState[e.SortExpression.ToString()] = "Descending";
                    sort_direction = "Descending";
                }
                else
                {
                    ViewState[e.SortExpression.ToString()] = "Ascending";
                    sort_direction = "Ascending";
                }
            }

            if (ViewState["searched_trans"] != null)
            {
                dataTable = (DataTable)ViewState["searched_trans"];
            }
            else
            {
                return;
            }
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + SetSortDirection(sort_direction);
                //grdLcoDet.DataSource = dataTable;
                //grdLcoDet.DataBind();
            }
        }

        protected void grdLastFive_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
                {
                    amtdd += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "amtdd"));
                    bal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "bal"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[9].Text = "" + amtdd;
                    e.Row.Cells[10].Text = "" + bal;
                }
            }
        }

        protected void grdLastFive_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable;
            string sort_direction;
            if (ViewState[e.SortExpression.ToString()] == null)
            {
                ViewState[e.SortExpression.ToString()] = Convert.ToString(e.SortDirection);
                sort_direction = Convert.ToString(e.SortDirection);
            }
            else
            {
                if (ViewState[e.SortExpression.ToString()].ToString() == "Ascending")
                {
                    ViewState[e.SortExpression.ToString()] = "Descending";
                    sort_direction = "Descending";
                }
                else
                {
                    ViewState[e.SortExpression.ToString()] = "Ascending";
                    sort_direction = "Ascending";
                }
            }

            if (ViewState["searched_trans"] != null)
            {
                dataTable = (DataTable)ViewState["searched_trans"];
            }
            else
            {
                return;
            }
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + SetSortDirection(sort_direction);
                grdLastFive.DataSource = dataTable;
                grdLastFive.DataBind();
            }
        }

        protected void grdTopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
                {
                    //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
                    amt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "amt"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = "" + amt;
                    //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
                    //e.Item.Cells[8].Text = "" + Total;
                }
            }
        }

        protected void grdTopup_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable;
            string sort_direction;
            if (ViewState[e.SortExpression.ToString()] == null)
            {
                ViewState[e.SortExpression.ToString()] = Convert.ToString(e.SortDirection);
                sort_direction = Convert.ToString(e.SortDirection);
            }
            else
            {
                if (ViewState[e.SortExpression.ToString()].ToString() == "Ascending")
                {
                    ViewState[e.SortExpression.ToString()] = "Descending";
                    sort_direction = "Descending";
                }
                else
                {
                    ViewState[e.SortExpression.ToString()] = "Ascending";
                    sort_direction = "Ascending";
                }
            }

            if (ViewState["searched_trans"] != null)
            {
                dataTable = (DataTable)ViewState["searched_trans"];
            }
            else
            {
                return;
            }
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + SetSortDirection(sort_direction);
                grdTopup.DataSource = dataTable;
                grdTopup.DataBind();
            }
        }

        protected void grdReversal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
                {
                    revamt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "amount"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = "" + revamt;
                }
            }
        }

        protected void grdReversal_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable;
            string sort_direction;
            if (ViewState[e.SortExpression.ToString()] == null)
            {
                ViewState[e.SortExpression.ToString()] = Convert.ToString(e.SortDirection);
                sort_direction = Convert.ToString(e.SortDirection);
            }
            else
            {
                if (ViewState[e.SortExpression.ToString()].ToString() == "Ascending")
                {
                    ViewState[e.SortExpression.ToString()] = "Descending";
                    sort_direction = "Descending";
                }
                else
                {
                    ViewState[e.SortExpression.ToString()] = "Ascending";
                    sort_direction = "Ascending";
                }
            }

            if (ViewState["searched_trans"] != null)
            {
                dataTable = (DataTable)ViewState["searched_trans"];
            }
            else
            {
                return;
            }
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + SetSortDirection(sort_direction);
                grdReversal.DataSource = dataTable;
                grdReversal.DataBind();
            }
        }

        protected string SetSortDirection(string sortDirection)
        {
            if (sortDirection == "Ascending")
            {
                return "DESC";
            }
            else
            {
                return "ASC";
            }
        }

        protected void rdolstSubsSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdPartyLed_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
                {
                    led_opnbal1 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "openinbal"));
                    led_debit1 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "drlimit"));
                    led_credit1 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "crlimit"));
                    led_closebal1 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "closingbal"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Text = "" + led_opnbal1;
                    e.Row.Cells[4].Text = "" + led_debit1;
                    e.Row.Cells[5].Text = "" + led_credit1;
                    e.Row.Cells[6].Text = "" + led_closebal1;
                }
            }
        }

        protected void grdPartyLedDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
                {
                    led_debit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "drlimit"));
                    led_credit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "crlimit"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[4].Text = "" + led_debit;
                    e.Row.Cells[5].Text = "" + led_credit;
                }
            }
        }


    }
}