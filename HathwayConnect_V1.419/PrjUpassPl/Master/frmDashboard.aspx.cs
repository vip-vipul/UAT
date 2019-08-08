using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Master;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;
using System.Web.UI.DataVisualization.Charting;
using PrjUpassBLL.Transaction;
using System.Drawing;
using System.Globalization;

namespace PrjUpassPl.Master
{
    public partial class frmDashboard : System.Web.UI.Page
    {
        string oper_id = "";
        string username = "";
        string catid = "";
        int flag = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            Master.PageHeading = "Dashboard";
            Session["RightsKey"] = null;
            // bindChart();
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {
                //oper_id = Convert.ToString(Session["operator_id"]);
                // Session["lcoid"] = Session["operator_id"];
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {

                lblSummaryason.Text = getrefreshdate(); //DateTime.Now.ToString("dd-MMM-yyyy");

                lblexpsumm.Text = getrefreshdate();//DateTime.Now.ToString("dd-MMM-yyyy");


                FillLcoDetails();
                ddllco_SelectedIndexChanged(null, null);
            }

        }

        public String getrefreshdate()
        {
            try
            {
                String Date = "";
                String str = "select * from VIEW_DASHBOARD_REFRSHDATE where trunc(insdt)=trunc(sysdate) and ROWNUM=1";

                DataTable tbldate = GetResult(str);

                if (tbldate.Rows.Count == 0)
                {
                    str = "select * from VIEW_DASHBOARD_REFRSHDATE where trunc(insdt)=trunc(sysdate-1) and ROWNUM=1";

                    tbldate = GetResult(str);
                    if (tbldate.Rows.Count == 0)
                    {
                        Date = DateTime.Now.ToString("dd-MMM-yyyy hh:mm TT");
                    }
                    else
                    {
                        Date = Convert.ToDateTime(tbldate.Rows[0]["insdt"].ToString()).ToString("dd-MMM-yyyy hh:mm tt");
                    }
                }
                else
                {
                    Date = Convert.ToDateTime(tbldate.Rows[0]["insdt"].ToString()).ToString("dd-MMM-yyyy hh:mm tt");
                }

                return Date;
            }
            catch
            {
                return DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            }
        }

     

        protected void ddllco_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["DASHOperid"] = ddllco.SelectedValue.Split('$')[0].ToString();
            Session["DASHCatid"] = ddllco.SelectedValue.Split('$')[1].ToString();
            Overview();
            Subscriber();
            bindChart();
            Expiry();
            Retention();
        }

        protected void FillLcoDetails()
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

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,num_lcomst_operid||'$'||u.num_user_opercatid lcocode ";
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
                    ddllco.DataTextField = "name";
                    ddllco.DataValueField = "lcocode";

                    ddllco.DataSource = tbllco;
                    ddllco.DataBind();
                    //if (category_id == "11")
                    //{
                    //    ddllco.Items.Insert(0, new ListItem("Select LCO", "0"));
                    //}
                    //else if (category_id == "3")
                    //{
                    ////ddllco_SelectedIndexChanged(null, null);
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

        protected void grdsubscriberBase4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text == "Disconnected Value (Rs)")
                {
                    e.Row.Cells[1].Text = Convert.ToDouble(e.Row.Cells[1].Text).ToString("0.0");
                    e.Row.Cells[2].Text = Convert.ToDouble(e.Row.Cells[2].Text).ToString("0.0");
                    e.Row.Cells[3].Text = Convert.ToDouble(e.Row.Cells[3].Text).ToString("0.0");
                }
            }
        }

        protected void grdOverview2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToDouble(e.Row.Cells[1].Text).ToString("0.0");
                e.Row.Cells[2].Text = Convert.ToDouble(e.Row.Cells[2].Text).ToString("0.0");
                e.Row.Cells[3].Text = Convert.ToDouble(e.Row.Cells[3].Text).ToString("0.0");
            }
        }
        protected void grdoverView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;

                if (e.Row.Cells[1].Text == "9")
                {
                    e.Row.Cells[2].Text = Convert.ToDouble(e.Row.Cells[2].Text).ToString("0.0");
                    e.Row.Cells[3].Text = Convert.ToDouble(e.Row.Cells[3].Text).ToString("0.0");
                    e.Row.Cells[4].Text = Convert.ToDouble(e.Row.Cells[4].Text).ToString("0.0");
                }


                if (e.Row.Cells[0].Text == "Opening Active STB")
                {
                    e.Row.Cells[0].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[1].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[2].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[3].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[4].Style.Add("background-color", "#e6b0aa");

                }


                if (e.Row.Cells[0].Text == "Closing Active STB")
                {

                    e.Row.Cells[0].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[1].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[2].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[3].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[4].Style.Add("background-color", "#e6b0aa");

                }

                if (e.Row.Cells[0].Text == "&nbsp;" && e.Row.Cells[1].Text == "0" && e.Row.Cells[2].Text == "0" && e.Row.Cells[3].Text == "0" && e.Row.Cells[4].Text == "0")
                {
                    e.Row.Cells[0].Style.Add("background-color", "#FFF");
                    e.Row.Cells[0].Style.Add("border", "none");
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[1].Style.Add("background-color", "#FFF");
                    e.Row.Cells[1].Style.Add("border", "none");
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Style.Add("background-color", "#FFF");
                    e.Row.Cells[2].Style.Add("border", "none");
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Style.Add("background-color", "#FFF");
                    e.Row.Cells[3].Style.Add("border", "none");
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Style.Add("background-color", "#FFF");
                    e.Row.Cells[4].Style.Add("border", "none");
                    e.Row.Cells[4].Text = "";
                }


            }

        }

        public void Overview()
        {
            Cls_BLL_Dashboard objAsPl = new Cls_BLL_Dashboard();
            DataTable Tbloverview = objAsPl.getOverVIew(username, Session["DASHOperid"].ToString(), Session["DASHCatid"].ToString());

            if (Tbloverview.Rows.Count > 0)
            {
                DataTable tblovereiw1 = new DataTable();
                DataTable tblovereiw2 = new DataTable();
                tblovereiw1 = Tbloverview.Clone();
                tblovereiw2 = Tbloverview.Clone();

                DataTable tblovereiw3 = new DataTable();
                tblovereiw3 = Tbloverview.Clone();
                for (int i = 0; i < Tbloverview.Rows.Count; i++)
                {
                    if (i < 6)
                    {
                        tblovereiw1.ImportRow(Tbloverview.Rows[i]);
                        //if (i == 5)
                        //{
                        //    tblovereiw1.Rows.Add("","",0,0,0,0);
                        //}
                    }
                    else if (i < 9)
                    {
                        tblovereiw3.ImportRow(Tbloverview.Rows[i]);
                    }
                    else
                    {
                        tblovereiw2.ImportRow(Tbloverview.Rows[i]);
                    }
                }

                grdoverView.DataSource = tblovereiw1;
                grdoverView.DataBind();

                grdbase1.DataSource = tblovereiw3;
                grdbase1.DataBind();
                
                //grdOverview2.DataSource = tblovereiw2;
                //grdOverview2.DataBind();
            }
        }

        public void Subscriber()
        {
            Cls_BLL_Dashboard objAsPl = new Cls_BLL_Dashboard();
            DataTable Tbloverview = objAsPl.GetSubscriber(Session["DASHOperid"].ToString(), Session["DASHCatid"].ToString());

            DataTable TblSubscriber = new DataTable();
            DataTable TblSubscriber1 = new DataTable();
            DataTable TblSubscriber2 = new DataTable();
            DataTable TblSubscriber3 = new DataTable();
            DataTable TblSubscriber4 = new DataTable();

            if (Tbloverview.Rows.Count > 0)
            {
                TblSubscriber = Tbloverview.Clone();
                TblSubscriber1 = Tbloverview.Clone();
                TblSubscriber2 = Tbloverview.Clone();
                TblSubscriber3 = Tbloverview.Clone();
                TblSubscriber4 = Tbloverview.Clone();

                for (int i = 0; i < Tbloverview.Rows.Count; i++)
                {
                    if (i < 5)
                    {
                        TblSubscriber.ImportRow(Tbloverview.Rows[i]);
                    }
                    //else if (i < 8)
                    //{
                    //    TblSubscriber1.ImportRow(Tbloverview.Rows[i]);
                    //}
                    else if (i < 9)
                    {
                        TblSubscriber2.ImportRow(Tbloverview.Rows[i]);
                    }
                    else
                    {
                        TblSubscriber4.ImportRow(Tbloverview.Rows[i]);
                    }


                }

                grdsubscriberBase.DataSource = TblSubscriber;
                grdsubscriberBase.DataBind();

                // grdsubscriberBase1.DataSource = TblSubscriber1;
                //  grdsubscriberBase1.DataBind();

                grdsubscriberBase2.DataSource = TblSubscriber2;
                grdsubscriberBase2.DataBind();

                //  grdsubscriberBase3.DataSource = TblSubscriber3;
                //   grdsubscriberBase3.DataBind();

                //grdsubscriberBase4.DataSource = TblSubscriber4;
                //grdsubscriberBase4.DataBind();

            }
        }

        public void Retention()
        {
            Cls_BLL_Dashboard objAsPl = new Cls_BLL_Dashboard();
            DataTable Tblretention = objAsPl.getRetention(username, Session["DASHOperid"].ToString(), Session["DASHCatid"].ToString());

            if (Tblretention.Rows.Count > 0)
            {
                DataTable Tblretention1 = new DataTable();
                DataTable Tblretention2 = new DataTable();
                DataTable Tblretention3 = new DataTable();
                Tblretention1 = Tblretention.Clone();
                Tblretention2 = Tblretention.Clone();
                Tblretention3 = Tblretention.Clone();
                for (int i = 0; i < Tblretention.Rows.Count; i++)
                {
                    if (i < 4)
                    {
                        Tblretention1.ImportRow(Tblretention.Rows[i]);
                    }
                    else if (i < 7)
                    {
                        Tblretention2.ImportRow(Tblretention.Rows[i]);
                    }
                    else
                    {
                        Tblretention3.ImportRow(Tblretention.Rows[i]);
                    }
                }

                if (Tblretention1.Rows.Count > 0)
                {
                    lblCurrentStb.Text = Tblretention1.Rows[0]["col1"].ToString();
                    lblLastSTB.Text = Tblretention1.Rows[0]["col2"].ToString();
                    lblLastAchievement.Text = Tblretention1.Rows[0]["col3"].ToString();
                    lblTotExpiry.Text = Tblretention1.Rows[1]["col1"].ToString();
                    lblLastReduction.Text = Tblretention1.Rows[1]["col2"].ToString();
                    lblRenewlMTD.Text = Tblretention1.Rows[2]["col1"].ToString();
                    lblLastExpiry.Text = Tblretention1.Rows[2]["col2"].ToString();
                    lblMTDAchieved.Text = Tblretention1.Rows[3]["col1"].ToString() + "%";
                    lblLastRenewal.Text = Tblretention1.Rows[3]["col2"].ToString();
                }

                grdretention1.DataSource = Tblretention2;
                grdretention1.DataBind();


                grdretention2.Columns[1].FooterText = Tblretention3.AsEnumerable().Select(x => x.Field<decimal>("col1")).Sum().ToString();
                grdretention2.Columns[2].FooterText = Tblretention3.AsEnumerable().Select(x => x.Field<decimal>("col2")).Sum().ToString();
                grdretention2.Columns[3].FooterText = Tblretention3.AsEnumerable().Select(x => x.Field<decimal>("col3")).Sum().ToString();
                grdretention2.Columns[4].FooterText = Tblretention3.AsEnumerable().Select(x => x.Field<decimal>("col4")).Sum().ToString();
                grdretention2.Columns[5].FooterText = Tblretention3.AsEnumerable().Select(x => x.Field<decimal>("col5")).Sum().ToString();
                grdretention2.Columns[6].FooterText = Tblretention3.AsEnumerable().Select(x => x.Field<decimal>("col6")).Sum().ToString();

                grdretention2.DataSource = Tblretention3;
                grdretention2.DataBind();


            }
        }

        protected void grdsubscriberBase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text == "&nbsp;" && e.Row.Cells[1].Text == "&nbsp;" && e.Row.Cells[2].Text == "&nbsp;")
                {
                    e.Row.Cells[0].Style.Add("background-color", "#094791");
                    e.Row.Cells[1].Style.Add("background-color", "#094791");
                    e.Row.Cells[2].Style.Add("background-color", "#094791");
                    e.Row.Cells[3].Style.Add("background-color", "#094791");
                }
            }
        }
        protected void grdExpiry_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[16].Visible = false;

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[16].Visible = false;
                if (e.Row.Cells[4].Text != "&nbsp;" && e.Row.Cells[5].Text != "&nbsp;" && e.Row.Cells[6].Text != "&nbsp;"
                    && e.Row.Cells[7].Text != "&nbsp;" && e.Row.Cells[8].Text != "&nbsp;")
                {
                    e.Row.Cells[4].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[5].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[6].Style.Add("background-color", "#e6b0aa");

                    e.Row.Cells[7].Style.Add("background-color", "#aed6f1");
                    e.Row.Cells[8].Style.Add("background-color", "#aed6f1");
                    e.Row.Cells[9].Style.Add("background-color", "#aed6f1");

                    e.Row.Cells[10].Style.Add("background-color", "#d7bde2");
                    e.Row.Cells[11].Style.Add("background-color", "#d7bde2");
                    e.Row.Cells[12].Style.Add("background-color", "#d7bde2");

                    e.Row.Cells[13].Style.Add("background-color", "#edbb99");
                    e.Row.Cells[14].Style.Add("background-color", "#edbb99");
                    e.Row.Cells[15].Style.Add("background-color", "#edbb99");
                }


                if (e.Row.Cells[16].Text == "7" || e.Row.Cells[16].Text == "10" || e.Row.Cells[16].Text == "18")
                {
                    e.Row.Cells[0].Style.Add("background-color", "#FFF");
                    e.Row.Cells[0].Style.Add("border", "none");
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[1].Style.Add("background-color", "#FFF");
                    e.Row.Cells[1].Style.Add("border", "none");
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Style.Add("background-color", "#FFF");
                    e.Row.Cells[2].Style.Add("border", "none");
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Style.Add("background-color", "#FFF");
                    e.Row.Cells[3].Style.Add("border", "none");
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Style.Add("background-color", "#FFF");
                    e.Row.Cells[4].Style.Add("border", "none");
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Style.Add("background-color", "#FFF");
                    e.Row.Cells[5].Style.Add("border", "none");
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Style.Add("background-color", "#FFF");
                    e.Row.Cells[6].Style.Add("border", "none");
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Style.Add("background-color", "#FFF");
                    e.Row.Cells[7].Style.Add("border", "none");
                    e.Row.Cells[7].Text = "";
                    e.Row.Cells[8].Style.Add("background-color", "#FFF");
                    e.Row.Cells[8].Style.Add("border", "none");
                    e.Row.Cells[8].Text = "";
                    e.Row.Cells[9].Style.Add("background-color", "#FFF");
                    e.Row.Cells[9].Style.Add("border", "none");
                    e.Row.Cells[9].Text = "";
                    e.Row.Cells[10].Style.Add("background-color", "#FFF");
                    e.Row.Cells[10].Style.Add("border", "none");
                    e.Row.Cells[10].Text = "";
                    e.Row.Cells[11].Style.Add("background-color", "#FFF");
                    e.Row.Cells[11].Style.Add("border", "none");
                    e.Row.Cells[11].Text = "";
                    e.Row.Cells[12].Style.Add("background-color", "#FFF");
                    e.Row.Cells[12].Style.Add("border", "none");
                    e.Row.Cells[12].Text = "";
                    e.Row.Cells[13].Style.Add("background-color", "#FFF");
                    e.Row.Cells[13].Style.Add("border", "none");
                    e.Row.Cells[13].Text = "";
                    e.Row.Cells[14].Style.Add("background-color", "#FFF");
                    e.Row.Cells[14].Style.Add("border", "none");
                    e.Row.Cells[14].Text = "";
                    e.Row.Cells[15].Style.Add("background-color", "#FFF");
                    e.Row.Cells[15].Style.Add("border", "none");
                    e.Row.Cells[15].Text = "";
                }

                if (e.Row.Cells[16].Text == "8" || e.Row.Cells[16].Text == "9" || e.Row.Cells[16].Text == "19" || e.Row.Cells[16].Text == "20")
                {
                    e.Row.Cells[0].Style.Add("background-color", "#EFF6FE");
                    e.Row.Cells[1].Style.Add("background-color", "#EFF6FE");
                    e.Row.Cells[2].Style.Add("background-color", "#EFF6FE");
                    e.Row.Cells[3].Style.Add("background-color", "#EFF6FE");
                }
                //Convert.ToDecimal(number).ToString("#,##0.00");

                if (e.Row.Cells[0].Text == "Pack Dispersion #")
                {

                    e.Row.Cells[0].Text = "Total";
                }
                if (e.Row.Cells[0].Text == "Pack Dispersion Value")
                {
                    flag = 1;

                    e.Row.Cells[0].Text = "Total";
                }
                else
                {
                    flag = 0;
                }

                if (flag == 1) 
                {
                    
                    e.Row.Cells[1].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col16")).ToString().Replace("$"," ");

                    e.Row.Cells[2].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col17")).ToString().Replace("$", " ");
                    e.Row.Cells[3].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col18")).ToString().Replace("$", " ");
                    e.Row.Cells[4].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col4")).ToString().Replace("$", " ");
                    e.Row.Cells[5].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col5")).ToString().Replace("$", " ");
                    e.Row.Cells[6].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col6")).ToString().Replace("$", " ");
                    e.Row.Cells[7].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col7")).ToString().Replace("$", " ");
                    e.Row.Cells[8].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col8")).ToString().Replace("$", " ");
                    e.Row.Cells[9].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col9")).ToString().Replace("$", " ");
                    e.Row.Cells[10].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col10")).ToString().Replace("$", " ");
                    e.Row.Cells[11].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col11")).ToString().Replace("$", " ");
                    e.Row.Cells[12].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col12")).ToString().Replace("$", " ");
                    e.Row.Cells[13].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col13")).ToString().Replace("$", " ");
                    e.Row.Cells[14].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col14")).ToString().Replace("$", " ");
                    e.Row.Cells[15].Text = String.Format("{0:C0}", DataBinder.Eval(e.Row.DataItem, "col15")).ToString().Replace("$", " ");
                }

            }

        }
        protected void grdExpiry_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell Cell_Header = new TableCell();
                Cell_Header.Text = " Particulars All Figures in nos";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 1;
                Cell_Header.RowSpan = 2;
                Cell_Header.BackColor = Color.FromName("#094791");
                Cell_Header.ForeColor = Color.FromName("#ffffff");
                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                Cell_Header.Text = "Total";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 1;
                Cell_Header.RowSpan = 2;
                Cell_Header.Font.Bold = true;
                Cell_Header.BackColor = Color.FromName("#094791");
                Cell_Header.ForeColor = Color.FromName("#ffffff");
                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                Cell_Header.Text = "Main TV";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 1;
                Cell_Header.RowSpan = 2;
                Cell_Header.Font.Bold = true;
                Cell_Header.BackColor = Color.FromName("#094791");
                Cell_Header.ForeColor = Color.FromName("#ffffff");
                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                Cell_Header.Text = "Child TV";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 1;
                Cell_Header.RowSpan = 2;
                Cell_Header.Font.Bold = true;
                Cell_Header.BackColor = Color.FromName("#094791");
                Cell_Header.ForeColor = Color.FromName("#ffffff");
                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                Cell_Header.Text = "Expiry overdue";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 3;
                Cell_Header.Font.Bold = true;
                Cell_Header.BackColor = Color.FromName("#094791");
                Cell_Header.ForeColor = Color.FromName("#ffffff");
                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                Cell_Header.Text = "Expiry due Today";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 3;
                Cell_Header.Font.Bold = true;
                Cell_Header.BackColor = Color.FromName("#094791");
                Cell_Header.ForeColor = Color.FromName("#ffffff");
                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                Cell_Header.Text = "Expiry due T+1";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 3;
                Cell_Header.Font.Bold = true;
                Cell_Header.BackColor = Color.FromName("#094791");
                Cell_Header.ForeColor = Color.FromName("#ffffff");
                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                Cell_Header.Text = "Expiry due T+2";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 3;
                Cell_Header.Font.Bold = true;
                Cell_Header.BackColor = Color.FromName("#094791");
                Cell_Header.ForeColor = Color.FromName("#ffffff");
                HeaderRow.Cells.Add(Cell_Header);

                grdExpiry.Controls[0].Controls.AddAt(0, HeaderRow);

            }
        }
        protected void grdExpiry1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text != "&nbsp;" && e.Row.Cells[5].Text != "&nbsp;" && e.Row.Cells[6].Text != "&nbsp;"
                    && e.Row.Cells[7].Text != "&nbsp;" && e.Row.Cells[8].Text != "&nbsp;")
                {
                    e.Row.Cells[4].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[5].Style.Add("background-color", "#e6b0aa");
                    e.Row.Cells[6].Style.Add("background-color", "#e6b0aa");

                    e.Row.Cells[7].Style.Add("background-color", "#aed6f1");
                    e.Row.Cells[8].Style.Add("background-color", "#aed6f1");
                    e.Row.Cells[9].Style.Add("background-color", "#aed6f1");

                    e.Row.Cells[10].Style.Add("background-color", "#d7bde2");
                    e.Row.Cells[11].Style.Add("background-color", "#d7bde2");
                    e.Row.Cells[12].Style.Add("background-color", "#d7bde2");

                    e.Row.Cells[13].Style.Add("background-color", "#edbb99");
                    e.Row.Cells[14].Style.Add("background-color", "#edbb99");
                    e.Row.Cells[15].Style.Add("background-color", "#edbb99");
                }
            }
        }

        public void Expiry()
        {
            Cls_BLL_Dashboard objAsPl = new Cls_BLL_Dashboard();
            DataTable Tblexpiry = objAsPl.getExpiry(username, Session["DASHOperid"].ToString(), Session["DASHCatid"].ToString());
            //DataTable TblExpiry1 = new DataTable();
            //DataTable TblExpiry2 = new DataTable();


            if (Tblexpiry.Rows.Count > 0)
            {
                //TblExpiry1 = Tblexpiry.Clone();
                //TblExpiry2 = Tblexpiry.Clone();

                //Double col1 = 0, col2 = 0, col3 = 0, col4 = 0, col5 = 0, col6 = 0, col7 = 0, col8 = 0, col9 = 0, col10 = 0,
                //    col11 = 0, col12 = 0, col13 = 0, col14 = 0, col15 = 0;
                //Double coll1 = 0, coll2 = 0, coll3 = 0, coll4 = 0, coll5 = 0, coll6 = 0, coll7 = 0, coll8 = 0, coll9 = 0,
                //    coll10 = 0, coll11 = 0, coll12 = 0, coll13 = 0, coll14 = 0, coll15 = 0;

                //int k = 0;
                //for (int i = 0; i < Tblexpiry.Rows.Count; i++)
                //{
                //    if (i < 7)
                //    {
                //        col1 += Convert.ToInt32(Tblexpiry.Rows[i]["col1"]);
                //        col2 += Convert.ToInt32(Tblexpiry.Rows[i]["col2"]);
                //        col3 += Convert.ToInt32(Tblexpiry.Rows[i]["col3"]);
                //        col4 += Convert.ToInt32(Tblexpiry.Rows[i]["col4"]);
                //        col5 += Convert.ToInt32(Tblexpiry.Rows[i]["col5"]);
                //        col6 += Convert.ToInt32(Tblexpiry.Rows[i]["col6"]);
                //        col7 += Convert.ToInt32(Tblexpiry.Rows[i]["col7"]);
                //        col8 += Convert.ToInt32(Tblexpiry.Rows[i]["col8"]);
                //        col9 += Convert.ToInt32(Tblexpiry.Rows[i]["col9"]);
                //        col10 += Convert.ToInt32(Tblexpiry.Rows[i]["col10"]);
                //        col11 += Convert.ToInt32(Tblexpiry.Rows[i]["col11"]);
                //        col12 += Convert.ToInt32(Tblexpiry.Rows[i]["col12"]);
                //        col13 += Convert.ToInt32(Tblexpiry.Rows[i]["col13"]);
                //        col14 += Convert.ToInt32(Tblexpiry.Rows[i]["col14"]);
                //        col15 += Convert.ToInt32(Tblexpiry.Rows[i]["col15"]);

                //    }
                //    else
                //    {
                //        coll1 += Convert.ToDouble(Tblexpiry.Rows[i]["col1"]);
                //        coll2 += Convert.ToDouble(Tblexpiry.Rows[i]["col2"]);
                //        coll3 += Convert.ToDouble(Tblexpiry.Rows[i]["col3"]);
                //        coll4 += Convert.ToDouble(Tblexpiry.Rows[i]["col4"]);
                //        coll5 += Convert.ToDouble(Tblexpiry.Rows[i]["col5"]);
                //        coll6 += Convert.ToDouble(Tblexpiry.Rows[i]["col6"]);
                //        coll7 += Convert.ToDouble(Tblexpiry.Rows[i]["col7"]);
                //        coll8 += Convert.ToDouble(Tblexpiry.Rows[i]["col8"]);
                //        coll9 += Convert.ToDouble(Tblexpiry.Rows[i]["col9"]);
                //        coll10 += Convert.ToDouble(Tblexpiry.Rows[i]["col10"]);
                //        coll11 += Convert.ToDouble(Tblexpiry.Rows[i]["col11"]);
                //        coll12 += Convert.ToDouble(Tblexpiry.Rows[i]["col12"]);
                //        coll13 += Convert.ToDouble(Tblexpiry.Rows[i]["col13"]);
                //        coll14 += Convert.ToDouble(Tblexpiry.Rows[i]["col14"]);
                //        coll15 += Convert.ToDouble(Tblexpiry.Rows[i]["col15"]);
                //    }
                //}







                //TblExpiry1.Rows.Add("", "Pack Dispersion", 0, col1, col2, col3, col4, col5, col6, col7, col8, col9, col10, col11, col12, col13, col14, col15);

                //for (int i = 0; i < Tblexpiry.Rows.Count; i++)
                //{
                //    if (i < 7)
                //    {
                //        TblExpiry1.ImportRow(Tblexpiry.Rows[i]);
                //    }
                //    else
                //    {
                //        if (k == 0)
                //        {
                //            TblExpiry1.Rows.Add("", "Pack Dispersion Value", 0, coll1, coll2, coll3, coll4, coll5, coll6, coll7, coll8, coll9, coll10, coll11, coll12, coll13, coll14, coll15);
                //        }
                //            TblExpiry1.ImportRow(Tblexpiry.Rows[i]);
                //        k++;
                //    }
                //}

                grdExpiry.DataSource = Tblexpiry;
                grdExpiry.DataBind();

                //GrdExpiry1.DataSource = TblExpiry2;
                //GrdExpiry1.DataBind();
            }
        }

        public void bindChart()
        {


            //DataTable dt = new DataTable();

            //Cls_Helper ob = new Cls_Helper();
            //dt = ob.GetDataTable(_query);

            DataTable TblChart = new DataTable();
            TblChart.Columns.Add("title", typeof(string));
            TblChart.Columns.Add("value", typeof(String));

            Cls_BLL_Dashboard objAsPl = new Cls_BLL_Dashboard();
            DataTable dt = objAsPl.getChart(username, Session["DASHOperid"].ToString(), Session["DASHCatid"].ToString());

            int Percentvalue = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["col1"].ToString() != "0")
                {
                    Percentvalue += Convert.ToInt32(dr["col1"].ToString());
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["col1"].ToString() != "0")
                {
                    int val = Convert.ToInt32(dr["col1"].ToString());
                    Double percent = (Double)val / Percentvalue;
                    int finalper = (int)Math.Round((double)(100 * val) / Percentvalue);
                    TblChart.Rows.Add(dr["title"].ToString() + " : " + Convert.ToInt32(dr["col1"].ToString()).ToString() + "(" + finalper + "%)", Convert.ToInt32(dr["col1"].ToString()));
                }
            }

            Chart1.DataSource = TblChart;
            Chart1.DataBind();
            Chart1.Series[0].ChartType = SeriesChartType.Pie;// for pie chart 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;// for pie chart 
            //   Chart1.Legends[0].Enabled = true;// for pie chart 



        }
    }
}

