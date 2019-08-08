using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassDAL.Helper;
using System.Data;
using PrjUpassBLL.Reports;
using System.Collections;
using System.IO;

namespace PrjUpassPl.Reports
{
    public partial class rptNew_Activation : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                grdNewActivation.PageIndex = 0;
                //setting page heading
                Master.PageHeading = "New Activation Report";

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                bindState();
                bndJVDDl();
            }
        }

        public void bindState()
        {
            string _getState = "select num_state_id,var_state_name  from aoup_lcopre_state_def order by var_state_name";
            Cls_Helper ob = new Cls_Helper();


            DataTable dt = new DataTable();
            dt = ob.GetDataTable(_getState);
            if (dt.Rows.Count > 0)
            {

                //  ddlState.Visible = true;

                ddlState.DataSource = dt;
                ddlState.DataTextField = "var_state_name";
                ddlState.DataValueField = "num_state_id";
                ddlState.DataBind();

                ddlState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));

            }

        }

        public void bindCity()
        {
            DataTable dt = new DataTable();


            string _getdata = "select a.num_city_id, a.var_city_name from aoup_lcopre_city_def a where a.num_city_stateid='" + ddlState.SelectedValue.Trim() + "' order by var_city_name";
            Cls_Helper ob = new Cls_Helper();



            dt = ob.GetDataTable(_getdata);
            if (dt.Rows.Count > 0)
            {

                // ddlCity.Visible = true;

                ddlCity.DataSource = dt;
                ddlCity.DataTextField = "var_city_name";
                ddlCity.DataValueField = "num_city_id";
                ddlCity.DataBind();

                ddlCity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));

            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindCity();
        }
        public void bndJVDDl()
        {
            string _getPlan = "select  DISTINCT var_comp_company,var_comp_company  from aoup_lcopre_company_det ";

            Cls_Helper obp = new Cls_Helper();

            DataTable dtp = obp.GetDataTable(_getPlan);

            if (dtp == null)
            {
                return;
            }
            if (dtp.Rows.Count == 0)
            {

                return;
            }
            if (dtp.Rows.Count > 0)
            {

                ddlJV.DataSource = dtp;
                ddlJV.DataTextField = "var_comp_company";
                ddlJV.DataValueField = "var_comp_company";
                ddlJV.DataBind();

                ddlJV.Items.Insert(0, new ListItem("All", "0"));

            }
            else
            {
                ddlJV.Items.Clear();
                ddlJV.Items.Insert(0, new ListItem("All", "0"));
                ddlJV.DataBind();
            }
        }

        private Hashtable getParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            Session["fromdt"] = txtFrom.Text;
            Session["todt"] = txtTo.Text;


            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("State", ddlState.SelectedItem.ToString());
            if (ddlState.SelectedValue != "0")
            {
                htSearchParams.Add("City", ddlCity.SelectedItem.ToString());
            }
            else
            {
                htSearchParams.Add("City", "0");

            }
            htSearchParams.Add("JV", ddlJV.SelectedValue);


            if (txtLCOCode.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("T", txtLCOCode.Text);

                if (valid == "")
                {
                    htSearchParams.Add("LCOCode", txtLCOCode.Text);
                    return htSearchParams;
                }
                else
                {
                    lblSearchMsg.Text = valid.ToString();
                    return null;

                }

            }
            else
            {
                htSearchParams.Add("LCOCode", "");
                return htSearchParams;
            }

            //htSearchParams.Add("LCOCode", txtLCOCode.Text);
            //return htSearchParams;
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
                    grdNewActivation.Visible = false;
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
                    grdNewActivation.Visible = true;
                }
            }
            Binddata();
        }

        public void Binddata()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
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

            Hashtable htAddPlanParams = getParamsData();

            if (htAddPlanParams != null)
            {
                Hashtable htResponse = new Hashtable();
                Cls_Business_rptNew_Activation objTran = new Cls_Business_rptNew_Activation();
                htResponse = objTran.GetNewActivation(htAddPlanParams, username);
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

                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b><b>Transaction From : </b>" + from + "<b>Transaction To : </b>" + to);


                if (dt.Rows.Count == 0)
                {
                    btngrnExel.Visible = false;
                    grdNewActivation.Visible = false;
                    lblSearchMsg.Text = "No data found";
                }
                else
                {
                    btngrnExel.Visible = true;
                    grdNewActivation.Visible = true;
                    lblSearchMsg.Text = "";
                    ViewState["searched_trans"] = dt;
                    grdNewActivation.DataSource = dt;
                    grdNewActivation.DataBind();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdNewActivation.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                    DivRoot.Style.Add("display", "block");

                }
            }
            else
            {
                return;
            }
        }
        protected void btngrnExel_Click(object sender, EventArgs e)
        {
            Hashtable htAddPlanParams = getParamsData();

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

            DataTable dt = null; //check for exception
            if (ViewState["searched_trans"] != null)
            {
                dt = (DataTable)ViewState["searched_trans"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count != 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "New_Activation_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "Receipt No." + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        + "Account No" + Convert.ToChar(9)
                        + "Name" + Convert.ToChar(9)
                        + "Mobile No." + Convert.ToChar(9)
                        + "Lindline" + Convert.ToChar(9)
                        + "Zip Code" + Convert.ToChar(9)
                        + "VC Id" + Convert.ToChar(9)
                        + "STB No." + Convert.ToChar(9)
                        + "Activation Date & Time" + Convert.ToChar(9)
                        + "JV Name" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9)
                        + "DAS Area" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9) + "'";


                            strrow +=
                                 dt.Rows[i]["cafno"].ToString() + Convert.ToChar(9)
                                 + dt.Rows[i]["owner"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["accno"].ToString() + Convert.ToChar(9)
                                + "'"
                                + dt.Rows[i]["name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["mobile"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["landline"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Zip"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["VCID"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["stb"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["dt"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["JVNAME"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["state"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["city"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["das"].ToString() + Convert.ToChar(9);


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
                Response.Redirect("../MyExcelFile/" + "New_Activation_" + datetime + ".xls");
            }
        }

        protected void grdactdact_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdNewActivation.PageIndex = e.NewPageIndex;
            Hashtable htAddPlanParams = getParamsData();

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

            Cls_Business_rptNew_Activation objTran = new Cls_Business_rptNew_Activation();
            Hashtable htResponse = objTran.GetNewActivation(htAddPlanParams, username);

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
                btngrnExel.Visible = false;
                grdNewActivation.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                grdNewActivation.Visible = true;
                lblSearchMsg.Text = "";
                grdNewActivation.DataSource = dt;
                grdNewActivation.DataBind();
            }
        }


    }
}