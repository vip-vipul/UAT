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
    public partial class rptLcowiseAwailBalDet : System.Web.UI.Page
    {
        decimal actuallim = 0;
        DateTime dtime = DateTime.Now;
         string type;
         string operid;
         string username;
         string catid;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Available Balance Report(Lco wise)";
            if (!IsPostBack)
            {
                Session["pagenos"] = "1";
                Session["RightsKey"] = null;
             //   grdAwailBal.PageIndex = 0;
                //setting page heading
                
                operid = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
              
                FillLcoDetails();
               // BindData();
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

            str = "SELECT lcocode, lconame, actuallim ";
            str += " FROM view_lco_Awailbal_det ";

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
            //string[] prefixTextArray = Operators.ToArray<string>();
            con.Close();
            con.Dispose();
            return Operators;
            ////////////
            //List<string> Operators = new List<string>();
            //Operators.Add("ranjan singh");
            //Operators.Add("ranjit mahajan");
            //Operators.Add("rinky sharma");
            //Operators.Add("rukhsana shaikh");
            //Operators.Add("ryan oberoy");

            //string[] prefixTextArray = Operators.ToArray<string>();
            //return prefixTextArray;
            //}
            //else
            //    return null;
        }

        private Hashtable getTopupParamsData()
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

            Hashtable htTopupParams = new Hashtable();
            htTopupParams.Add("lcd", "0");
           // htTopupParams.Add("lnm", lnm);
            htTopupParams.Add("txtsear", txtsear);
            return htTopupParams;
        }

        protected void BindData()
        {

            Hashtable htTopupParams = getTopupParamsData();

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

            Cls_Business_RptAwailBal objTran = new Cls_Business_RptAwailBal();
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
                //lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt.Rows.Count == 0)
            {
                grdAwailBal.Visible = false;
                lblSearchMsg.Text = "No data found";
                btngrnExel.Visible = false;
            }
            else
            {
                btngrnExel.Visible = true;
                grdAwailBal.Visible = true;
                btngrnExel.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdAwailBal.DataSource = dt;
                grdAwailBal.DataBind();



                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdAwailBal.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");
                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            Hashtable htTopupParams = getTopupParamsData();

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

            Cls_Business_RptAwailBal objTran = new Cls_Business_RptAwailBal();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "AwailBal_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "Total Balance" + Convert.ToChar(9)
                        + "Allocated Balance" + Convert.ToChar(9)
                        + "Unallocated Balance" + Convert.ToChar(9)
                        + "Last Transaction Date" + Convert.ToChar(9)
                        + "Company Name" + Convert.ToChar(9)
                        + "Distributor" + Convert.ToChar(9)
                        + "Sub Distributor" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9)
                        + "DAS Area" + Convert.ToChar(9);


                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + "'"
                                + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["actuallim"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["allocatedlimit"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["availablelimit"].ToString() + Convert.ToChar(9)
                                + "'"
                                + dt.Rows[i]["last_transdt"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["companyname"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["distributor"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["subdistributor"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["statename"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["cityname"].ToString() + Convert.ToChar(9)
                             + dt.Rows[i]["DASAREA"].ToString() + Convert.ToChar(9);


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
                Response.Redirect("../MyExcelFile/" + "AwailBal_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
        }

        protected void grdAwailBal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            //    {
            //        //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
            //        actuallim += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "actuallim"));
            //    }
            //    else if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        e.Row.Cells[3].Text = "" + actuallim;
            //        //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
            //        //e.Item.Cells[8].Text = "" + Total;
            //    }
            //}
        }

        protected void grdAwailBal_Sorting(object sender, GridViewSortEventArgs e)
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
                grdAwailBal.DataSource = dataTable;
                grdAwailBal.DataBind();
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

     

        protected void grdAwailBal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAwailBal.PageIndex = e.NewPageIndex;
            BindData();
        }
    }
}