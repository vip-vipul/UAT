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
    public partial class rptLcowiseUserdetails : System.Web.UI.Page
    {
        decimal usercount = 0;
         string catid;
         string operator_id;
         string type;
        string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "User Details Report(Lco wise)";
            
            DateTime dtime = DateTime.Now;
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                grdLcodet.PageIndex = 0;

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

                //setting page heading
                
                if (catid == "3")
                {
                    Session["TOperID"] = operator_id;
                    Response.Redirect("../Reports/rptUserdetailsLcowiseDet.aspx?showall=1");
                }
               // BindData();

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


        protected void BindData()
        {
           
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString(); //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            cls_Business_rptLcowiseUserdetails objTran = new cls_Business_rptLcowiseUserdetails();
            DataTable dt = objTran.getLcodetails(username, catid, operator_id);

            if (dt.Rows.Count == 0)
            {
                btnAll.Visible = false;
                grdLcodet.Visible = false;
                lblSearchMsg.Text = "No data found";
                btngrnExel.Visible = false;
            }
            else
            {
                btnAll.Visible = true;
                btngrnExel.Visible = true;
                grdLcodet.Visible = true;
                btngrnExel.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_Lco"] = dt;
                grdLcodet.DataSource = dt;
                grdLcodet.DataBind();

            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchOperators(string prefixText, int count, string contextKey)
        {
            string Str = prefixText.Trim();


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
            

                str = "SELECT a.msoid, a.msoname, a.distid, a.distname, a.lcoid, a.lcocode, a.lconame, a.transdt, a.usercnt" +
                            " FROM view_lcopre_userdet_det a";

                if (contextKey == "1")
                {
                    str += " where upper(a.lconame) like upper('" + prefixText + "%') ";
                }
                else if (contextKey == "0")
                {
                    str += " where upper(a.lcocode) like upper('" + prefixText + "%') ";
                }

                if (catid == "2")
                {
                    str += " and a.msoid ='" + operid + "'";
                }

                if (catid == "5")
                {
                    str += " and a.distid ='" + operid + "'";
                }

                if (catid == "3")
                {
                    str += " and a.lcoid ='" + operid + "'";
                }

                if (catid == "10")
                {
                    str += " and a.hoid ='" + operid + "'";
                }
                
                str += " order by a.transdt desc";

            
           
            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (contextKey == "1")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["lconame"].ToString(), dr["lconame"].ToString());

                    Operators.Add(item);
                }
                else if (contextKey == "0")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["lcocode"].ToString(), dr["lcocode"].ToString());
                    Operators.Add(item);
                }
            }
            //string[] prefixTextArray = Operators.ToArray<string>();
            con.Close();
            con.Dispose();
            return Operators;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString(); //Convert.ToString(Session["operator_id"]);
                Session["TOperID"] = operator_id.ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            cls_Business_rptLcowiseUserdetails objTran = new cls_Business_rptLcowiseUserdetails();
            DataTable dt = objTran.getLcoSearch(username, ddlLco.SelectedValue.Split('#')[1].ToString(), "0", catid, operator_id);

            if (dt.Rows.Count == 0)
            {
                btnAll.Visible = false;
                grdLcodet.Visible = false;
                lblSearchMsg.Text = "No data found";
                btngrnExel.Visible = false;
            }
            else
            {
                btnAll.Visible = true;
                btngrnExel.Visible = true;
                grdLcodet.Visible = true;
                btngrnExel.Visible = true;
                lblSearchMsg.Text = "";
                grdLcodet.DataSource = dt;
                grdLcodet.DataBind();

            }
        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString(); //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            cls_Business_rptLcowiseUserdetails objTran = new cls_Business_rptLcowiseUserdetails();
            DataTable dt = objTran.getLcodetails(username, catid, operator_id);

            if (dt.Rows.Count != 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "LcoDetails_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "Company Name" + Convert.ToChar(9)
                        + "Distributor" + Convert.ToChar(9)
                        + "Sub Distributor" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9) 
                        + "User Count" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9) + "'"
                                + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["companyname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["distributor"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["subdistributor"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["statename"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cityname"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["usercnt"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "LcoDetails_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
        }

        protected void grdLcodet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            //    {
            //        usercount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "usercnt"));
            //    }
            //    else if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        e.Row.Cells[8].Text = "" + usercount;
            //    }
            //}
        }

        protected void Linkuser_Click(object sender, EventArgs e)
        {
            int rindex = (((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer))).RowIndex;
            Session["Lcoid"] = ((Label)grdLcodet.Rows[rindex].FindControl("lblOperid1")).Text;
            Response.Redirect("../Reports/rptUserdetailsLcowiseDet.aspx?showall=1");
        }

        protected void rdolstSubsSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void btnAll_Click(object sender, EventArgs e)
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
            string Showall = "0";
            string lcoid = "";
            cls_Business_rptLcowiseUserdetails objTran = new cls_Business_rptLcowiseUserdetails();
            DataTable dt = objTran.getLcouserdetails(username, lcoid, Showall, catid, operator_id);
            if (dt.Rows.Count != 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "UserDetails_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9) + "User Id" + Convert.ToChar(9) + "User Name" + Convert.ToChar(9)
                        + "First Name" + Convert.ToChar(9) + "Middle Name" + Convert.ToChar(9) + "Last Name" + Convert.ToChar(9) + "Address" + Convert.ToChar(9)
                        + "Code" + Convert.ToChar(9) + "Brmpoid" + Convert.ToChar(9) + "state" + Convert.ToChar(9) + "City" + Convert.ToChar(9)
                        + "User Type" + Convert.ToChar(9)
                        + "Email" + Convert.ToChar(9) + "Mobile Number" + Convert.ToChar(9) + "Account number" + Convert.ToChar(9)
                        + "Balance" + Convert.ToChar(9)
                        + "Inserted By" + Convert.ToChar(9)
                        + "Date" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9) + "'" + dt.Rows[i]["username"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["userowner"].ToString() + Convert.ToChar(9) + dt.Rows[i]["fname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["mname"].ToString() + Convert.ToChar(9) + dt.Rows[i]["lname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["addr"].ToString() + Convert.ToChar(9) + dt.Rows[i]["code"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["brmpoid"].ToString() + Convert.ToChar(9) + dt.Rows[i]["ststeid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cityid"].ToString() + Convert.ToChar(9) + dt.Rows[i]["flag"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["email"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["mobno"].ToString() + Convert.ToChar(9) + dt.Rows[i]["accno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["balance"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["insby"].ToString() + Convert.ToChar(9) + dt.Rows[i]["insdt"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "UserDetails_" + datetime + ".xls");
            }
            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            
            
           
           // Response.Redirect("../Reports/rptUserdetailsLcowiseDet.aspx?showall=0");
        }

        protected void grdLcodet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLcodet.PageIndex = e.NewPageIndex;
            BindData();
        }

        //protected void grdLcodet_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Usercount")
        //    {
        //        try
        //        {
        //            int rindex = (((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer))).RowIndex;
        //            Session["Lcoid"] = (Label)grdLcodet.Rows[rindex].FindControl("lblOperid1");
        //        }
        //        catch (Exception ex)
        //        {
        //            Response.Redirect("../errorPage.aspx");
        //        }
        //        Response.Redirect("../Reports/rptUserdetailsLcowiseDet.aspx");
        //    }
        //}
    }
}