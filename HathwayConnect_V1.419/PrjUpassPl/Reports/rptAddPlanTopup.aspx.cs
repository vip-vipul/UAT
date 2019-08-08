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

namespace PrjUpassPl.Reports
{
    public partial class rptAddPlanTopup : System.Web.UI.Page
    {
        decimal amt = 0;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Top-up Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                grdAddPlantopup.PageIndex = 0;
                //setting page heading

                Session["pagenos"] = "1";

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                btngrnExel.Visible = false;
                btnGenerateExcel.Visible = false;
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

        private Hashtable getTopupParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            Hashtable htTopupParams = new Hashtable();
            htTopupParams.Add("from", from);
            htTopupParams.Add("to", to);
            return htTopupParams;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            lblResultCount.Text = "";
            DateTime fromDt;
            DateTime toDt;
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
            {
                fromDt = new DateTime();
                toDt = new DateTime();
                fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    grdAddPlantopup.Visible = false;
                    lblSearchMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else if (Convert.ToDateTime(txtFrom.Text.ToString()) > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select date greater than current date!";
                    return;
                }
                else if (Convert.ToDateTime(txtTo.Text.ToString()) > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select date greater than current date!";
                    return;
                }
                else
                {
                    lblSearchMsg.Text = "";
                    grdAddPlantopup.Visible = true;
                }
            }

            Hashtable htTopupParams = getTopupParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();    //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptTopup objTran = new Cls_Business_RptTopup();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid, operator_id);

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
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt.Rows.Count == 0)
            {
                grdAddPlantopup.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                btnGenerateExcel.Visible = true;
                grdAddPlantopup.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdAddPlantopup.DataSource = dt;
                grdAddPlantopup.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdAddPlantopup.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");

                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }

            /*DataTable dt = new DataTable("Pager");
            dt.Columns.Add("dtttime");
            dt.Columns.Add("amt");
            dt.Columns.Add("paymode");
            dt.Columns.Add("erprcptno");
            dt.Columns.Add("rcptno");
            dt.Columns.Add("finuid");
            dt.Columns.Add("fiuname");
            dt.Columns.Add("action");
            dt.Rows.Add();
            dt.Rows[0]["dtttime"] = "24-Nov-2014 10:20:04 pm";
            dt.Rows[0]["amt"] = "1000";
            dt.Rows[0]["paymode"] = "Cash";
            dt.Rows[0]["erprcptno"] = "1112";
            dt.Rows[0]["rcptno"] = "5501";
            dt.Rows[0]["finuid"] = "0600001";
            dt.Rows[0]["fiuname"] = "ADMIN";
            dt.Rows[0]["action"] = "Refund";
            dt.Rows.Add();
            dt.Rows[1]["dtttime"] = "23-Nov-2014 11:03:33 pm";
            dt.Rows[1]["amt"] = "2000";
            dt.Rows[1]["paymode"] = "Cheque";
            dt.Rows[1]["erprcptno"] = "4567";
            dt.Rows[1]["rcptno"] = "5572";
            dt.Rows[1]["finuid"] = "0600101";
            dt.Rows[1]["fiuname"] = "UPASS";
            dt.Rows[1]["action"] = "Topup";
            dt.Rows.Add();
            dt.Rows[2]["dtttime"] = "22-Nov-2014 08:11:00 pm";
            dt.Rows[2]["amt"] = "3000";
            dt.Rows[2]["paymode"] = "Cash";
            dt.Rows[2]["erprcptno"] = "98788";
            dt.Rows[2]["rcptno"] = "5598";
            dt.Rows[2]["finuid"] = "060010001";
            dt.Rows[2]["fiuname"] = "UPASS";
            dt.Rows[2]["action"] = "Reversal";
            

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdAddPlantopup.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdAddPlantopup.Visible = true;
                lblSearchMsg.Text = "";
                grdAddPlantopup.DataSource = dt;
                grdAddPlantopup.DataBind();
            }*/
        }

        protected void grdAddPlantopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            //    {
            //        //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
            //        amt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "amt"));
            //    }
            //    else if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        e.Row.Cells[2].Text = "" + amt;
            //        //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
            //        //e.Item.Cells[8].Text = "" + Total;
            //    }
            //}            
        }

        protected void grdAddPlantopup_Sorting(object sender, GridViewSortEventArgs e)
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
                grdAddPlantopup.DataSource = dataTable;
                grdAddPlantopup.DataBind();
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

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            lblResultCount.Text = "";

            Hashtable htTopupParams = getTopupParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();  //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptTopup objTran = new Cls_Business_RptTopup();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "Topup_" + datetime + ".csv");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + ","
                        + "Date & Time" + ","
                        + "Amount" + ","
                        + "Mode of payment" + ","
                        + "Bank Name" + ","
                        + "Branch Name" + ","
                        + "Cheque No." + ","
                        + "Cheque Date" + ","
                        + "ERP Receipt No." + ","
                    + "UPASS Transaction ID" + ","
                        + "Finance user id" + ","
                        + "Finance user name" + ","
                        + "Action" + ","
                    + "LCO Code" + ","
                        + "LCO Name" + ","
                        + "JV Name" + ","
                        + "ERP LCO A/C" + ","
                        + "Distributor" + ","
                    + "Sub distributor" + ","
                        + "City" + ","
                        + "State" + ","
                         + "DAS Area" + ","
                           + "R.R. No." + ","
                             + "Auth No." + ","
                               + "MPOS UserID" + ","

                        + "Billdesk Ref No." + ","     
                        + "Source"
                           + "Payment Type" ;

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + ","
                                + "'" + dt.Rows[i]["dtttime"].ToString() + ","
                                + dt.Rows[i]["amt"].ToString() + ","
                            + dt.Rows[i]["paymode"].ToString() + ","
                                + dt.Rows[i]["BANKNAME"].ToString() + ","
                                + dt.Rows[i]["BRANCHNAME"].ToString() + ","
                                + dt.Rows[i]["CHEQUEDDNO"].ToString() + ","
                                + dt.Rows[i]["CHEQUEDT"].ToString() + ","
                                + dt.Rows[i]["erprcptno"].ToString() + ","
                            + dt.Rows[i]["rcptno"].ToString() + ","
                            + dt.Rows[i]["finuid"].ToString() + ","
                            + dt.Rows[i]["fiuname"].ToString() + ","
                            + dt.Rows[i]["action"].ToString() + ","
                            + dt.Rows[i]["lcocode"].ToString() + ","
                            + dt.Rows[i]["lconame"].ToString() + ","
                            + dt.Rows[i]["jvname"].ToString() + ","
                            + dt.Rows[i]["erplco_ac"].ToString() + ","
                            + dt.Rows[i]["distname"].ToString() + ","
                            + dt.Rows[i]["subdist"].ToString() + ","
                            + dt.Rows[i]["city"].ToString() + ","
                            + dt.Rows[i]["state"].ToString() + ","
                             + dt.Rows[i]["AREA"].ToString() + ","
                              + dt.Rows[i]["rrno"].ToString() + ","
                               + dt.Rows[i]["authno"].ToString() + ","
                                + dt.Rows[i]["mposuserid"].ToString() + ","


                              + dt.Rows[i]["billdesk_ref"].ToString() + ","   
                              + dt.Rows[i]["sflag"].ToString()
                            +dt.Rows[i]["identifier"].ToString();
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
                Response.AddHeader("Content-disposition", "attachment; filename=Topup_" + datetime + ".csv");
                Response.ContentType = "text/csv";
                Response.Redirect("../MyExcelFile/" + "Topup_" + datetime + ".csv");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdAddPlantopup.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdAddPlantopup.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdAddPlantopup.DataSource = dt;
                grdAddPlantopup.DataBind();

                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        protected void grdAddPlantopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAddPlantopup.PageIndex = e.NewPageIndex;
            Hashtable htTopupParams = getTopupParamsData();

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

            Cls_Business_RptTopup objTran = new Cls_Business_RptTopup();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid, operator_id);

            DataTable dt = null;
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }




            if (dt.Rows.Count == 0)
            {
                grdAddPlantopup.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                btnGenerateExcel.Visible = true;
                grdAddPlantopup.Visible = true;
                lblSearchMsg.Text = "";
                //ViewState["searched_trans"] = dt;
                grdAddPlantopup.DataSource = dt;
                grdAddPlantopup.DataBind();



            }
        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            lblResultCount.Text = "";

            Hashtable htTopupParams = getTopupParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();  //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptTopup objTran = new Cls_Business_RptTopup();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "Topup_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "Date & Time" + Convert.ToChar(9)
                        + "Amount" + Convert.ToChar(9)
                        + "Mode of payment" + Convert.ToChar(9)
                        + "Bank Name" + Convert.ToChar(9)
                        + "Branch Name" + Convert.ToChar(9)
                        + "Cheque No." + Convert.ToChar(9)
                        + "Cheque Date" + Convert.ToChar(9)
                        + "ERP Receipt No." + Convert.ToChar(9)
                    + "UPASS Transaction ID" + Convert.ToChar(9)
                        + "Finance user id" + Convert.ToChar(9)
                        + "Finance user name" + Convert.ToChar(9)
                        + "Action" + Convert.ToChar(9)
                    + "LCO Code" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "JV Name" + Convert.ToChar(9)
                        + "ERP LCO A/C" + Convert.ToChar(9)
                        + "Distributor" + Convert.ToChar(9)
                    + "Sub distributor" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                         + "DAS Area" + Convert.ToChar(9)
                          + "R.R. No." + Convert.ToChar(9)
                           + "Auth No." + Convert.ToChar(9)
                            + "MPOS UserId" + Convert.ToChar(9)

                        + "Billdesk Ref No." + Convert.ToChar(9)   
                        + "Source" + Convert.ToChar(9)
                          + "Payment Type" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + "'" + dt.Rows[i]["dtttime"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["amt"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["paymode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["BANKNAME"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["BRANCHNAME"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["CHEQUEDDNO"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["CHEQUEDT"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["erprcptno"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["rcptno"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["finuid"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["fiuname"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["action"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["jvname"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["erplco_ac"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["distname"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["subdist"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["city"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["state"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["AREA"].ToString() + Convert.ToChar(9)
                             + dt.Rows[i]["rrno"].ToString() + Convert.ToChar(9)
                              + dt.Rows[i]["authno"].ToString() + Convert.ToChar(9)
                               + dt.Rows[i]["mposuserid"].ToString() + Convert.ToChar(9)

                             + dt.Rows[i]["billdesk_ref"].ToString() + Convert.ToChar(9)  ////rrno  authno  mposuserid
                             + dt.Rows[i]["sflag"].ToString() + Convert.ToChar(9)
                            +dt.Rows[i]["identifier"].ToString() + Convert.ToChar(9);

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
                Response.Redirect("../MyExcelFile/" + "Topup_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdAddPlantopup.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdAddPlantopup.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdAddPlantopup.DataSource = dt;
                grdAddPlantopup.DataBind();

                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        protected void lnkGenerateReceipt_Click(object sender, EventArgs e)
        {
            int indexno = (((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer))).RowIndex;
            int rindex = Convert.ToInt32(indexno.ToString());


            HiddenField hdndtttime = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdndtttime");
            string tempDateTimeValue = hdndtttime.Value;
            string[] DateTimeValue = tempDateTimeValue.Split(',');
            string DateTime = DateTimeValue[0].ToString();


            HiddenField hdnamt = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnamt");
            string tempAmount = hdnamt.Value;
            string[] tempAmountValue = tempAmount.Split(',');
            string Amount = tempAmountValue[0].ToString();

            HiddenField hdnrcptno = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnerprcptno");
            string tempReceiptonlineno = hdnrcptno.Value;
            string[] temptransactionIDValue = tempReceiptonlineno.Split(',');
            string Receiptonlineno = temptransactionIDValue[0].ToString();

            HiddenField hdnerprcptno = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnrcptno");
            string tempreceiptno = hdnerprcptno.Value;
            string[] tempreceiptnovalue = tempreceiptno.Split(',');
            string ReceiptNo = tempreceiptnovalue[0].ToString();

            HiddenField hdnlcocode = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnlcocode");
            string templcocode = hdnlcocode.Value;
            string[] templcocodevalue = templcocode.Split(',');
            string Lcocode = templcocodevalue[0].ToString();

            HiddenField hdnlconame = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnlconame");
            string templconame = hdnlconame.Value;
            string[] templconamevalue = templconame.Split(',');
            string Lconame = templconamevalue[0].ToString();

            HiddenField hdnpaymode = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnpaymode");
            string temphdnpaymode = hdnpaymode.Value;
            string[] temphdnpaymodevalue = temphdnpaymode.Split(',');
            string Paymentmode = temphdnpaymodevalue[0].ToString();

            HiddenField hdncity = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdncity");
            string temphdncity = hdncity.Value;
            string[] temphdncityvalue = temphdncity.Split(',');
            string City = temphdncityvalue[0].ToString();

            HiddenField hdnBANKNAME = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnBANKNAME");
            string temphdnBANKNAME = hdnBANKNAME.Value;
            string[] temptemphdnBANKNAMEvalue = temphdnBANKNAME.Split(',');
            string Bankname = temptemphdnBANKNAMEvalue[0].ToString();

            HiddenField hdnCHEQUEDDNO = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnCHEQUEDDNO");
            string temphdnCHEQUEDDNO = hdnCHEQUEDDNO.Value;
            string[] temphdnCHEQUEDDNOval = temphdnCHEQUEDDNO.Split(',');
            string ChequeNo = temphdnCHEQUEDDNOval[0].ToString();


            HiddenField hdnrr = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnrr");
            string temphdnCHEQUEDDNOhdnrr = hdnrr.Value;
            string[] temphdnCHEQUEDDNOvalhdnrr = temphdnCHEQUEDDNOhdnrr.Split(',');
            string Chdnrr = temphdnCHEQUEDDNOvalhdnrr[0].ToString();

            HiddenField hdnauthno = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnauthno");
            string temphdnhdnauthno = hdnauthno.Value;
            string[] temptemphdnhdnauthno = temphdnhdnauthno.Split(',');
            string Ctemptemphdnhdnauthno = temptemphdnhdnauthno[0].ToString();

            HiddenField hdnmpos = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnmpos");
            string temphhdnmpos = hdnmpos.Value;
            string[] temphdnmpos = temphhdnmpos.Split(',');
            string Ctemphdnmpos = temphdnmpos[0].ToString();

            HiddenField hdnremark = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnremark");
            string temphdnremark = hdnremark.Value;
            string[] temphdntemphdnremark = temphdnremark.Split(',');
            string Ctemphdntemphdnremark = temphdntemphdnremark[0].ToString();
            //hdnremark

            HiddenField hdnfinuid = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnfinuid");
            string temphdnfiuname = hdnfinuid.Value;
            string[] temphdnfiunameF = temphdnfiuname.Split(',');
            string CtemphdnfiunameF = temphdnfiunameF[0].ToString();

            HiddenField hdnlco_paymode = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnlco_paymode");
            string temphdnlco_paymode = hdnlco_paymode.Value;
            string[] temphdnlco_paymodeF = temphdnlco_paymode.Split(',');
            string Clco_paymodF = temphdnlco_paymodeF[0].ToString();

            Session["rcpt_pt_datetime"] = DateTime;
            Session["rcpt_pt_receiptno"] = ReceiptNo;
            Session["rcpt_pt_Lcocode"] = Lcocode;
            Session["rcpt_pt_Lconame"] = Lconame;
            Session["rcpt_pt_Amount"] = Amount;
            Session["rcpt_pt_Paymentmode"] = Clco_paymodF;

            if (Paymentmode == "Q")
            {
                Session["data"] = "**Credit in account will be given subject to clearance of Cheque.";
            }
            if (Paymentmode == "DD")
            {
                Session["data"] = "**Credit in account will be given subject to clearance of Demant Draft.";
            }


            Session["rcpt_pt_City"] = City;
            Session["rcpt_pt_Bankname"] = Bankname;
            Session["rcpt_pt_ChequeNo"] = ChequeNo;
            Session["rcpt_pt_receiptonlineno"] = Receiptonlineno;

            Session["rcpt_rr"] = Chdnrr;
            Session["rcpt_auth"] = Ctemptemphdnhdnauthno;
            Session["rcpt_mpos"] = Ctemphdnmpos;
            Session["rcpt_pt_premark1"] = Ctemphdntemphdnremark;
            Session["rcpt_pt_cashiername"] = CtemphdnfiunameF;


            ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('../Reports/rcptPaymentReceiptInvoice.aspx');", true);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdAddPlantopup.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
            DivRoot.Style.Add("display", "block");
            //Response.Write("<script language='javascript'> window.open('../Reports/rcptPaymentReceiptInvoice.aspx', 'Print_Receipt','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes,location=no,status=no');</script>");

            //   ClientScript.RegisterStartupScript(typeof(Page), "alertMessage", "<script type='text/javascript'>alert('Payment Done Successfully');window.location.replace('../Transaction/rcptPaymentReceiptInvoice.aspx?lcoid=" + Request.QueryString["lcoid"].ToString().Trim() + "&msoname=" + Request.QueryString["msoname"].ToString().Trim() + "&lcoadd=" + Request.QueryString["lcoadd"].ToString().Trim() + "&lcocont=" + Request.QueryString["lcocont"].ToString().Trim() + "&lconame=" + Request.QueryString["lconame"].ToString().Trim() + "');</script>");
        }


    }
}