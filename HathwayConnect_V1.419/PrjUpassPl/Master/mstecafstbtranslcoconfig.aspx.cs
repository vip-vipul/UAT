using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;
using PrjUpassBLL.Master;

namespace PrjUpassPl.Master
{
    public partial class mstecafstbtranslcoconfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.PageHeading = "STB Transfer Configuration";
                loadState();
            }
        }

        protected void loadCity()
        {

            string where_str = " WHERE num_city_stateid='" + ViewState["stateid"].ToString() + "'";
            DataSet ds = Cls_Helper.Comboupdate("aoup_lcopre_city_def " + where_str + " ORDER BY var_city_name asc", "num_city_id", "var_city_name");
            ddlCity.DataSource = ds;
            ddlCity.DataTextField = "var_city_name";
            ddlCity.DataValueField = "num_city_id";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("All", ""));


        }

        protected void loadState()
        {
            DataSet dsStates = Cls_Helper.Comboupdate("AOUP_LCOPRE_STATE_DEF", "NUM_STATE_ID", "VAR_STATE_NAME");
            ddlState.DataSource = dsStates;
            ddlState.DataTextField = "VAR_STATE_NAME";
            ddlState.DataValueField = "NUM_STATE_ID";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("-- Select State --", ""));

        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlState.SelectedValue != "0" || ddlState.SelectedValue != "")
            {
                ViewState["stateid"] = ddlState.SelectedValue;
                loadCity();
            }
            else
            {
                ddlCity.DataSource = "";
                ddlCity.DataBind();
            }
        }
        protected void btnCloseInfo_Click(object sender, EventArgs e)
        {

            if (ViewState["ErrorInfo"] != null)
            {
                if (ViewState["ErrorInfo"].ToString() == "9999")
                {
                    Response.Redirect("../Reports/EcafPages.aspx");
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlState.SelectedValue == "0" || ddlState.SelectedValue == "")
                {

                    lblSearchMsg.Text = "Please select state";
                    return;

                }
              

                if (ddlDAS.SelectedValue == "")
                {
                    lblSearchMsg.Text = "Please select DAS Area";
                    return;

                }

                if (txtlco.Text == "")
                {
                    lblSearchMsg.Text = "Please enter LCO";
                    return;

                }

                Hashtable ht = new Hashtable();

                ht.Add("in_username", Session["username"].ToString());
                ht.Add("in_state", ddlState.SelectedItem.Text.ToUpper().ToString());
                ht.Add("in_city", ddlCity.SelectedItem.Text.ToUpper().ToString());
                ht.Add("in_DAS", ddlDAS.SelectedItem.Text.ToUpper().ToString());
                ht.Add("in_LCO", txtlco.Text.ToUpper().ToString());
                ht.Add("in_adminlevel", ddlAdminLevel.SelectedValue.ToString());
                ht.Add("in_stateid", ddlState.SelectedValue);
                Cls_BLL_ecafstbtransfer obj = new Cls_BLL_ecafstbtransfer();
                string response = obj.InsertLCOConfig(Session["username"].ToString(), ht);
                string[] responseArr = response.Split('$');



                lblSearchMsg.Text = responseArr[1].ToString();
                ViewState["ErrorInfo"] = "9999";
            }
            catch (Exception ex)
            {

                lblSearchMsg.Text = ex.Message;
            }

        }

    }
}