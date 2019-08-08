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
    public partial class rptLastFive : System.Web.UI.Page
    {
        static string operid;
        static string username;
        static string catid;

        decimal amtdd = 0;
        decimal bal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Last Five Transaction Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                
                Session["pagenos"] = "1";


                operid = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);

                btn_genExl.Visible = false;

                if (catid == "3")
                {
                    tbldet.Visible = false;


                    Hashtable htAddPlanParams = getUserPara();

                    Cls_Business_RptLastFiveGridBind cls = new Cls_Business_RptLastFiveGridBind();
                    Hashtable htResponse = cls.GetLastFiveTransaction(htAddPlanParams, username, catid, operid);

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
                        grdTransDet.Visible = false;
                        lblSearchMsg.Text = "No data found";
                        btn_genExl.Visible = false;
                    }

                    else
                    {
                        btn_genExl.Visible = true;
                        grdTransDet.Visible = true;
                        lblSearchMsg.Text = "";
                        ViewState["searched_trans"] = dt;
                        grdTransDet.DataSource = dt;
                        grdTransDet.DataBind();
                    }
                }
                else
                {
                    tbldet.Visible = true;
                    FillLcoDetails();
                  //  FillLco();
                }


            }
        }

        protected void FillLcoDetails()
        {
            string str = "";
            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
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

        //protected void FillLco()
        //{
        //    if (Session["username"] != null)
        //    {
        //        Cls_Business_RptLastFive obj = new Cls_Business_RptLastFive();
        //        string username = Convert.ToString(Session["username"]);

        //        DataSet ds = obj.GetLcoUserDetails(username, catid, operid);
        //        if (ds != null)
        //        {
        //            ddlLco.DataSource = ds.Tables[0];
        //            ddlLco.DataTextField = "var_oper_opername";
        //            ddlLco.DataValueField = "num_oper_id";
        //            ddlLco.DataBind();
        //            ddlLco.Items.Insert(0, new ListItem("Select LCO", "0"));



        //        }
        //        else
        //        {
        //            //exception occured
        //            Response.Redirect("~/ErrorPage.aspx");
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        Session.Abandon();
        //        Response.Redirect("~/Login.aspx");
        //    }

        //}

        //protected void ddlLco_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string lcoid = ddlLco.SelectedValue.ToString();
        //    if (lcoid != "0")
        //    {
        //        string where_str = " WHERE num_user_operid=" + lcoid;
        //        DataSet ds = Cls_Helper.Comboupdate("aoup_user_def " + where_str + " ORDER BY var_user_username", "num_user_userid", "var_user_username");
        //        ddlUser.DataSource = ds;
        //        ddlUser.DataTextField = "var_user_username";
        //        ddlUser.DataValueField = "num_user_userid";
        //        ddlUser.DataBind();
        //        ddlUser.Dispose();
        //    }
        //    else
        //    {
        //        ddlUser.Items.Clear();
        //    }
        //    ddlUser.Items.Insert(0, new ListItem("Select User", "0"));
        //}

        private Hashtable getUserPara()
        {
            string user = "";
            //if (catid != "3")
            //{
            //    user = ddlUser.SelectedItem.Text.ToString();
            //}

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("user", user);

            return htSearchParams;
        }

        public void msgbox(string message, Control ctrl)
        {
            string msg = "<script type=\"text/javascript\">alert(\"" + message + "\");</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", msg);
            ctrl.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {


            //if (ddlUser.SelectedIndex == 0)
            //{
            //    msgbox("Please Select User", ddlUser);
            //    return;

            //}

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(ddlLco.SelectedValue); //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Hashtable htAddPlanParams = getUserPara();

            Cls_Business_RptLastFiveGridBind cls = new Cls_Business_RptLastFiveGridBind();
            Hashtable htResponse = cls.GetLastFiveTransaction(htAddPlanParams, username, catid, operator_id);

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
                grdTransDet.Visible = false;
                lblSearchMsg.Text = "No data found";
                btn_genExl.Visible = false;
            }

            else
            {
                btn_genExl.Visible = true;
                grdTransDet.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdTransDet.DataSource = dt;
                grdTransDet.DataBind();

                //showing result count
                ////lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        protected void grdTransDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            //    {
            //        //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
            //        amtdd += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "amtdd"));
            //        bal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "bal"));
            //    }
            //    else if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        e.Row.Cells[8].Text = "" + amtdd;
            //        e.Row.Cells[11].Text = "" + bal;
            //        //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
            //        //e.Item.Cells[8].Text = "" + Total;
            //    }
            //}
        }

        protected void grdTransDet_Sorting(object sender, GridViewSortEventArgs e)
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
                grdTransDet.DataSource = dataTable;
                grdTransDet.DataBind();
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


            string username;
            string catid, operator_id;
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


            Hashtable htAddPlanParams = getUserPara();

            Cls_Business_RptLastFiveGridBind cls = new Cls_Business_RptLastFiveGridBind();
            Hashtable htResponse = cls.GetLastFiveTransaction(htAddPlanParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "LastFiveTrans_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Customer ID" + Convert.ToChar(9) + "VC" + Convert.ToChar(9) + "Plan Name" + Convert.ToChar(9) + "Plan Type" + Convert.ToChar(9);
                    strheader += " Transaction Type" + Convert.ToChar(9) + "Reason" + Convert.ToChar(9);
                    strheader += "User ID" + Convert.ToChar(9) + "User Name" + Convert.ToChar(9)
                        + "'" + "Transaction Date & Time" + Convert.ToChar(9)
                        + "MRP" + Convert.ToChar(9)
                        + "Amount deducted" + Convert.ToChar(9) + "'" + "Expiry date" + Convert.ToChar(9);
                    strheader += "Pay Term" + Convert.ToChar(9) + "Balance" + Convert.ToChar(9) + "LCO Code" + Convert.ToChar(9) + "LCO Name" + Convert.ToChar(9) + "JV Name" + Convert.ToChar(9);
                    strheader += "ERP LCO A/C" + Convert.ToChar(9) + "Distributor" + Convert.ToChar(9) + "Sub distributor" + Convert.ToChar(9);
                    strheader += "City" + Convert.ToChar(9) + "State" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + dt.Rows[i]["custid"].ToString() + Convert.ToChar(9) + dt.Rows[i]["vc"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["plnname"].ToString() + Convert.ToChar(9) + dt.Rows[i]["plntyp"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["flag"].ToString() + Convert.ToChar(9) + dt.Rows[i]["reason"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["uname"].ToString() + Convert.ToChar(9) + dt.Rows[i]["userowner"].ToString() + Convert.ToChar(9) + "'" + dt.Rows[i]["tdt"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["custprice"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["amtdd"].ToString() + Convert.ToChar(9) + "'" + dt.Rows[i]["expdt"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["payterm"].ToString() + Convert.ToChar(9) + dt.Rows[i]["bal"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9) + dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["jvname"].ToString() + Convert.ToChar(9) + dt.Rows[i]["erplco_ac"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["distname"].ToString() + Convert.ToChar(9) + dt.Rows[i]["subdist"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["city"].ToString() + Convert.ToChar(9) + dt.Rows[i]["state"].ToString() + Convert.ToChar(9);


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
                Response.Redirect("../MyExcelFile/" + "LastFiveTrans_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                btn_genExl.Visible = false;
                grdTransDet.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btn_genExl.Visible = true;
                grdTransDet.Visible = true;

            }
        }

    }



}