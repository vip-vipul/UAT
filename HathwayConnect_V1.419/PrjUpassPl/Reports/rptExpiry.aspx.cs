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
    public partial class rptExpiry : System.Web.UI.Page
    {
        decimal crlimit = 0;
        decimal opnbal = 0;
        decimal debit = 0;
        decimal credit = 0;
        decimal closebal = 0;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Plan Expiry Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                //setting page heading

                Session["pagenos"] = "1";


                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                //  RadSearchby.Items[1].Selected = false;

                FillLcoDetails();
            }

            //else
            //{

            //    btnSubmit.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSubmit, null) + ";");

            // }
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



        private Hashtable getLedgerParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            string search_type = RadSearchby.SelectedValue.ToString();


            Session["fromdt"] = txtFrom.Text;
            Session["todt"] = txtTo.Text;
            Session["search"] = search_type.ToString().Trim();

            string txtsearch = "";
            if (txtsearchpara.Text.Length > 0)
            {
                txtsearch = txtsearchpara.Text.ToString().Trim();
            }
            Session["txtsearch"] = txtsearch.ToString().Trim();

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("package", ddlPackage.SelectedValue.ToString());
            htSearchParams.Add("search", search_type);//added by Rushali
            htSearchParams.Add("txtsearch", txtsearch);//added by Rushali
            return htSearchParams;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            binddata();
        }

        protected void binddata()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            DateTime boundry_start = DateTime.Now.Date.AddDays(-15);
            DateTime boundry_end = DateTime.Now.Date.AddDays(15);
            string date_err_msg = "Please select From and To dates between the range of '" + boundry_start.Day + "-" + boundry_start.Month + "-" + boundry_start.Year + "' " +
                                    "& '" + boundry_end.Day + "-" + boundry_end.Month + "-" + boundry_end.Year + "'";
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
                    grdExpiry.Visible = false;
                    lblSearchMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                //else if (Convert.ToDateTime(txtFrom.Text.ToString()) < DateTime.Now.Date.AddDays(-15))
                //{
                //    lblSearchMsg.Text = date_err_msg;//"You can not select From date earlier than 15 days from current date!";
                //    return;
                //}
                else if (Convert.ToDateTime(txtFrom.Text.ToString()) < DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select From date earlier than current date!";//"You can not select From date later than 15 days from current date!";
                    return;
                }
                //else if (Convert.ToDateTime(txtTo.Text.ToString()) < DateTime.Now.Date.AddDays(-15))
                //{
                //    lblSearchMsg.Text = date_err_msg;// "You can not select To date earlier than 15 days from current date!";
                //    return;
                //}
                else if (Convert.ToDateTime(txtTo.Text.ToString()) > DateTime.Now.Date.AddDays(29))
                {
                    lblSearchMsg.Text = "You can not select To date later than 29 days from current date!";// "You can not select To date later than 15 days from current date!";
                    return;
                }
                else
                {
                    lblSearchMsg.Text = "";
                    grdExpiry.Visible = true;
                }
            }

            string txtsearch = "";
            if (txtsearchpara.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("T", txtsearchpara.Text);

                if (valid == "")
                    txtsearch = txtsearchpara.Text.ToString().Trim();
                else
                {
                    lblSearchMsg.Text = valid.ToString();
                    return;
                }

            }

            Hashtable htAddPlanParams = getLedgerParamsData();

            string username, catid, operator_id, search_type;
            

            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = ddlLco.SelectedValue.Split('#')[1].ToString();// Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
                search_type = Convert.ToString(Session["search"]);
                txtsearch = Convert.ToString(Session["txtsearch"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            DataTable dt = new DataTable();
            if (catid != "0" && catid != "10")
            {
                //if (catid == "3")
                //{
                Cls_Business_rptExpiry objTran = new Cls_Business_rptExpiry();
                dt = objTran.GetDetails(htAddPlanParams, username, operator_id, catid);
                //}
                //else
                //{
                //    lblSearchMsg.Text = "unauthorize user";
                //}
                if (dt == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }

                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b><b>Details From : </b>" + from + "<b> To : </b>" + to);

                if (dt.Rows.Count == 0)
                {
                    btnGenerateExcel.Visible = false;
                    grdExpiry.Visible = false;
                    lblSearchMsg.Text = "No data found";
                }
                else
                {
                    btnGenerateExcel.Visible = true;
                    grdExpiry.Visible = true;
                    lblSearchMsg.Text = "";
                    ViewState["searched_trans"] = dt;
                    grdExpiry.DataSource = dt;
                    grdExpiry.DataBind();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdExpiry.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                    DivRoot.Style.Add("display", "block");

                }
            }
            else if (catid == "0" || catid == "10")
            {
                ExportExcel(from, to);


            }
        }

        protected void grdExpiry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("vcid"))
            {
                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    //Session["showall"] = null;
                    int rowindex = clickedRow.RowIndex;
                    Session["vcid"] = ((LinkButton)clickedRow.FindControl("LBvc")).Text;
                    //Session["lconame"] = ((Label)clickedRow.FindControl("lblolconame")).Text;
                    lblAccNo.Text = grdExpiry.Rows[rowindex].Cells[1].Text;
                    lblVCNo.Text = ((LinkButton)clickedRow.FindControl("LBvc")).Text;
                    lbllcoName.Text = grdExpiry.Rows[rowindex].Cells[5].Text;
                    lblfullname.Text = ((HiddenField)clickedRow.FindControl("hdnfullname")).Value;
                    lbladdress.Text = ((HiddenField)clickedRow.FindControl("HdnAddress")).Value;
                    lblMobile.Text = grdExpiry.Rows[rowindex].Cells[7].Text;
                    lblplan.Text = grdExpiry.Rows[rowindex].Cells[8].Text;
                    lblplantype.Text = grdExpiry.Rows[rowindex].Cells[10].Text;
                    lblEnddate.Text = grdExpiry.Rows[rowindex].Cells[11].Text;
                    popExp.Show();
                }
                catch (Exception ex)
                {
                    Response.Redirect("../errorPage.aspx");
                }
                //Response.Redirect("../Reports/rptPrePartyLedgerDET.aspx");
            }
        }

        protected void grdExpiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdExpiry.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            ExportExcel(from, to);
        }
        protected void ExportExcel(string from, string to)
        {


            //string from = txtFrom.Text;
            //string to = txtTo.Text;

            DateTime boundry_start = DateTime.Now.Date.AddDays(-15);
            DateTime boundry_end = DateTime.Now.Date.AddDays(15);
            string date_err_msg = "Please select From and To dates between the range of '" + boundry_start.Day + "-" + boundry_start.Month + "-" + boundry_start.Year + "' " +
                                    "& '" + boundry_end.Day + "-" + boundry_end.Month + "-" + boundry_end.Year + "'";
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
                    grdExpiry.Visible = false;
                    lblSearchMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                //else if (Convert.ToDateTime(txtFrom.Text.ToString()) < DateTime.Now.Date.AddDays(-15))
                //{
                //    lblSearchMsg.Text = date_err_msg;//"You can not select From date earlier than 15 days from current date!";
                //    return;
                //}
                else if (Convert.ToDateTime(txtFrom.Text.ToString()) < DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select From date earlier than current date!";//"You can not select From date later than 15 days from current date!";
                    return;
                }
                //else if (Convert.ToDateTime(txtTo.Text.ToString()) < DateTime.Now.Date.AddDays(-15))
                //{
                //    lblSearchMsg.Text = date_err_msg;// "You can not select To date earlier than 15 days from current date!";
                //    return;
                //}
                else if (Convert.ToDateTime(txtTo.Text.ToString()) > DateTime.Now.Date.AddDays(29))
                {
                    lblSearchMsg.Text = "You can not select To date later than 29 days from current date!";// "You can not select To date later than 15 days from current date!";
                    return;
                }
                else
                {
                    lblSearchMsg.Text = "";

                }
            }

            Hashtable htAddPlanParams = getLedgerParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = ddlLco.SelectedValue.Split('#')[1].ToString(); //Session["username"].ToString();
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

            Cls_Business_rptExpiry objTran = new Cls_Business_rptExpiry();
            dt = objTran.GetDetails(htAddPlanParams, username, operator_id, catid);

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PlanExpiry_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                        + "Account Number" + Convert.ToChar(9)
                        + "VC Id" + Convert.ToChar(9)
                        + "STB No" + Convert.ToChar(9)
                        + "Customer Name" + Convert.ToChar(9)
                        + "Address" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "Lco code" + Convert.ToChar(9)
                        + "Mobile" + Convert.ToChar(9)
                        + "Package" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9)
                        + "Plan Type" + Convert.ToChar(9)
                        + "End Date" + Convert.ToChar(9)
                        + "Renewal Status" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["account_no"].ToString() + Convert.ToChar(9)
                                + "'" + dt.Rows[i]["vc"].ToString() + Convert.ToChar(9)
                                +"'"+ dt.Rows[i]["stb"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["fullname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["address"].ToString() + Convert.ToChar(9)

                                + dt.Rows[i]["lco_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lco_code"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["mobile"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["planname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cityname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["plantype"].ToString() + Convert.ToChar(9)
                                + "'"
                                + dt.Rows[i]["enddate"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["renewflagstatus"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "PlanExpiry_" + datetime + ".xls");
                if (catid == "0" || catid == "10")
                {
                    Response.Redirect("../Reports/rptExpiry.aspx");

                }

            }


            if (dt.Rows.Count == 0)
            {
                grdExpiry.Visible = false;
                lblSearchMsg.Text = "No data found";
            }

        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
            RadSearchby.Items[1].Selected = false;
            txtsearchpara.Enabled = false;
            txtsearchpara.Text = "";
            lblSearchMsg.Text = "";
        }

        protected void RadSearchby_SelectedIndexChanged1(object sender, EventArgs e)
        {

            txtsearchpara.Enabled = true;
        }

    }
}