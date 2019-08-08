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
using System.Data.OracleClient;
using System.Configuration;

namespace PrjUpassPl.Reports
{
    public partial class rptPrePartyLedgerLCO : System.Web.UI.Page
    {
        decimal crlimit = 0;
        decimal opnbal = 0;
        decimal debit = 0;
        decimal credit = 0;
        decimal closebal = 0;
        DateTime dtime = DateTime.Now;



        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "LCO Party Ledger Report";
            if (!IsPostBack)
            {
                Session["pagenos"] = "1";

                Session["RightsKey"] = null;
                grdLcoPartyLedger.PageIndex = 0;
                //setting page heading
                

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();

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

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,num_lcomst_operid lcocode ";  //num_lcomst_operid
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
                    //if (category_id == "11")
                    //{
                    //    ddlLco.Items.Insert(0, new ListItem("Select LCO", "0"));
                    //}
                    //else if (category_id == "3")
                    //{
                    //    //ddllco_SelectedIndexChanged(null, null);
                    //}
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


        private Hashtable getLedgerParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            Session["fromdt"] = txtFrom.Text;
            Session["todt"] = txtTo.Text;

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            return htSearchParams;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string from = txtFrom.Text;
            string to = txtTo.Text;


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
                    grdLcoPartyLedger.Visible = false;
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
                    grdLcoPartyLedger.Visible = true;
                }
            }

            Hashtable htAddPlanParams = getLedgerParamsData();

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

            if (catid == "11")
            {
                Session["Soperatorid"] = operator_id;
            }

            Cls_Business_RptLedger objTran = new Cls_Business_RptLedger();
            Hashtable htResponse = objTran.GetTransations(htAddPlanParams, username, catid, operator_id);

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
                //lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>LCO Party Ledger Parameters : </b>" + strParams);
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (catid == "3")
            {
                if (dt.Rows.Count > 0)
                {
                    Session["lconame"] = dt.Rows[0]["lconame"].ToString();
                }
                else
                {
                    Session["lconame"] = "";
                }
                Response.Redirect("../Reports/rptPrePartyLedgerDET.aspx?showall=1");
            }


            if (dt.Rows.Count == 0)
            {
                btn_genExl.Visible = false;
                grdLcoPartyLedger.Visible = false;
                lblSearchMsg.Text = "No data found";
            }

            else
            {
                btn_genExl.Visible = true;
                grdLcoPartyLedger.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdLcoPartyLedger.DataSource = dt;
                grdLcoPartyLedger.DataBind();

                //showing result count
                ////lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        protected void grdLcoPartyLedger_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("LcoName1"))
            {
                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    //Session["showall"] = null;
                    Session["lcoid"] = ((Label)clickedRow.FindControl("lblOperid1")).Text;
                    Session["lconame"] = ((Label)clickedRow.FindControl("lblolconame")).Text;

                }
                catch (Exception ex)
                {
                    Response.Redirect("../errorPage.aspx");
                }
                Response.Redirect("../Reports/rptPrePartyLedgerDET.aspx?showall=1");
            }
        }

        protected void grdLcoPartyLedger_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            //    {

            //        crlimit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "crlimit"));
            //        opnbal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "openinbal"));
            //        debit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "drlimit"));
            //        credit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "crlimit"));
            //        closebal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "closingbal"));
            //    }
            //    else if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        e.Row.Cells[3].Text = "" + crlimit;
            //        e.Row.Cells[4].Text = "" + opnbal;
            //        e.Row.Cells[5].Text = "" + debit;
            //        e.Row.Cells[6].Text = "" + credit;
            //        e.Row.Cells[7].Text = "" + closebal;
            //        //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
            //        //e.Item.Cells[8].Text = "" + Total;
            //    }
            //}
        }

        protected void grdLcoPartyLedger_Sorting(object sender, GridViewSortEventArgs e)
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
                grdLcoPartyLedger.DataSource = dataTable;
                grdLcoPartyLedger.DataBind();
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

        protected void btnAll_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Reports/rptPrePartyLedgerDET.aspx?showall=0");
        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            Hashtable htAddPlanParams = getLedgerParamsData();
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



            Cls_Business_RptLedger objTran = new Cls_Business_RptLedger();
            Hashtable htResponse = objTran.GetTransations(htAddPlanParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PartyLedgerLCO_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "SrNo." + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        + "Opening Balance" + Convert.ToChar(9)
                        + "Debit" + Convert.ToChar(9)
                        + "Credit" + Convert.ToChar(9)
                        + "Closing Balance" + Convert.ToChar(9)
                        + "Company Name" + Convert.ToChar(9)
                        + "Distributor" + Convert.ToChar(9)
                        + "Sub Distributor" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9);


                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["openinbal"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["drlimit"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["crlimit"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["closingbal"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["companyname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["distributor"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["subdistributor"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["statename"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cityname"].ToString() + Convert.ToChar(9);
                            
                           


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
                Response.Redirect("../MyExcelFile/" + "PartyLedgerLCO_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
        }

        protected void grdLcoPartyLedger_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLcoPartyLedger.PageIndex = e.NewPageIndex;
            Hashtable htAddPlanParams = getLedgerParamsData();

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



            Cls_Business_RptLedger objTran = new Cls_Business_RptLedger();
            Hashtable htResponse = objTran.GetTransations(htAddPlanParams, username, catid, operator_id);

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

           

           


            if (dt.Rows.Count == 0)
            {
                btn_genExl.Visible = false;
                grdLcoPartyLedger.Visible = false;
                lblSearchMsg.Text = "No data found";
            }

            else
            {
                btn_genExl.Visible = true;
                grdLcoPartyLedger.Visible = true;
                lblSearchMsg.Text = "";
                //ViewState["searched_trans"] = dt;
                grdLcoPartyLedger.DataSource = dt;
                grdLcoPartyLedger.DataBind();

              
            }
        }
    }
}