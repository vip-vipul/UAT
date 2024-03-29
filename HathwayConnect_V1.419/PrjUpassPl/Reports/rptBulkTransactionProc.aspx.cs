﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Master;
using System.Threading;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Reports
{
    public partial class rptBulkTransactionProc : System.Web.UI.Page
    {
        static int intNum;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["RightsKey"] = "N";
            Master.PageHeading = "Bulk Upload Process";
            if (!IsPostBack)
            {
                // Page.PreviousPage.ResolveClientUrl
                if (Request.QueryString["uniqueid"] != null)
                {
                    string uniqueid = Request.QueryString["uniqueid"].ToString();
                    ViewState["uniqueid"] = uniqueid;
                    if (uniqueid != "")
                    {
                        txtUniqueid.Attributes.Add("style", "display:none");
                        divtextbox.Attributes.Add("style", "display:none");
                        //  txtUniqueid.Visible = false;
                        // divtextbox.Visible = false;
                        Thread.Sleep(700);
                        SearchbindData(uniqueid);
                        DivShowValue.Visible = true;
                        DivGried.Visible = true;
                    }


                }
                else
                {
                    DivShowValue.Visible = false;
                    DivGried.Visible = false;
                }



            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "javascript:ShowPopup();", true);
        }


        protected void SearchbindData(string Unique_id)
        {
            //try
            //{
            lbSearchMsg.Text = "";

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
            DataTable dt = new DataTable();


            Cls_Business_RptBulkTransProces objTran = new Cls_Business_RptBulkTransProces();
            dt = objTran.GetDetails(username, operator_id, catid, Unique_id);

            if (dt == null)
            {
                lblResponseMsg.Text = "No Data Found";
            }
            else if (dt.Rows.Count == 0)
            {
                lblResponseMsg.Text = "No Data Found";
                DivShowValue.Visible = false;
                //SearchbindData(Unique_id);


            }


            else
            {
                DivShowValue.Visible = true;
                lblSumFile.Text = dt.Rows[0]["uploadid"].ToString();
                lblSumTotal.Text = dt.Rows[0]["total"].ToString();
                lblSumSuccess.Text = dt.Rows[0]["success"].ToString();
                lblSumFailure.Text = dt.Rows[0]["failed"].ToString();
                lblRemaing.Text = dt.Rows[0]["remaing"].ToString();


            }

            //}
            //catch (Exception ex)
            //{
            //    Response.Redirect("../errorPage.aspx");
            //}



        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                lbSearchMsg.Text = "";
                string Unique_id = "";
                if (txtUniqueid.Text == "" && ViewState["uniqueid"] == null && Convert.ToString(ViewState["uniqueid"]) == "")
                {

                    lblerrormsg.Text = "Please Eneter unique  Id....";
                    return;
                }
                else
                {
                    if (txtUniqueid.Text != "")
                    {
                        string valid = SecurityValidation.chkData("T", txtUniqueid.Text);
                        Unique_id = (valid == "") ? txtUniqueid.Text.Trim().ToString() : "";
                        // Unique_id = txtUniqueid.Text.Trim().ToString();
                    }
                    else if (ViewState["uniqueid"].ToString() != "")
                    {
                        Unique_id = ViewState["uniqueid"].ToString();
                    }
                }
                SearchbindData(Unique_id);
            }
            catch (Exception ex)
            {
                Response.Redirect("../errorPage.aspx");
            }

        }

        protected void lblSumTotal_Click(object sender, EventArgs e)
        {
            string upload_id = lblSumFile.Text;
            intNum = 1;
            binddata(upload_id, intNum);

        }

        protected void lblSumSuccess_Click(object sender, EventArgs e)
        {
            string upload_id = lblSumFile.Text;
            intNum = 2;
            binddata(upload_id, intNum);
        }

        protected void lblSumFailure_Click(object sender, EventArgs e)
        {
            string upload_id = lblSumFile.Text;
            intNum = 3;
            binddata(upload_id, intNum);

        }

        protected void lblRemaing_Click(object sender, EventArgs e)
        {
            string upload_id = lblSumFile.Text;
            intNum = 4;
            binddata(upload_id, intNum);

        }

        protected void binddata(string upload_id, int IntBulkStatus)
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
            DataTable dt = new DataTable();

            Cls_Business_RptBulkTransProces objTran = new Cls_Business_RptBulkTransProces();
            dt = objTran.GetBulkStatusDetails(username, upload_id, IntBulkStatus);

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }



            if (dt.Rows.Count == 0)
            {

                grdBulkstatus.Visible = false;
                lbSearchMsg.Text = "No data found";
            }
            else
                //{
                //    btnGenerateExcel.Visible = true;
                DivGried.Visible = true;
            grdBulkstatus.Visible = true;

            grdBulkstatus.DataSource = dt;
            grdBulkstatus.DataBind();
        }
        protected void grdBulkstatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBulkstatus.PageIndex = e.NewPageIndex;
            binddata(lblSumFile.Text, intNum);
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                lbSearchMsg.Text = "";
                string Unique_id = "";
                if (txtUniqueid.Text == "" && ViewState["uniqueid"] == null && Convert.ToString(ViewState["uniqueid"]) == "")
                {

                    lblerrormsg.Text = "Please Eneter unique  Id....";
                    return;
                }
                else
                {
                    if (txtUniqueid.Text != "")
                    {
                        Unique_id = txtUniqueid.Text.Trim().ToString();
                    }
                    else if (ViewState["uniqueid"].ToString() != "")
                    {
                        Unique_id = ViewState["uniqueid"].ToString();
                    }

                }

                SearchbindData(Unique_id);



            }
            catch (Exception ex)
            {
                Response.Redirect("../errorPage.aspx");
            }

        }


    }

}


