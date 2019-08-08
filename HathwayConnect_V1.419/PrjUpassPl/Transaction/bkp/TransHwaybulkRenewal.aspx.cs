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
using PrjUpassBLL.Transaction;
using System.IO;
using System.Text;
using System.Data.OracleClient;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwaybulkRenewal : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Bulk Renewal";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                //setting page heading
                

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();

                //trType.Visible = false;
                fillCombo();
               
            }
          
        }

        private Hashtable getLedgerParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            string plan_name = ddlPlanname.SelectedItem.Text;

            Session["fromdt"] = txtFrom.Text;
            Session["todt"] = txtTo.Text;

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("plan_name", plan_name);
            return htSearchParams;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            binddata();
          
            rbALL.Checked = true;
        }

        protected void binddata()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            DateTime boundry_start = DateTime.Now.Date;
            DateTime boundry_end = DateTime.Now.Date.AddDays(15);
            string date_err_msg = "Please select From and To dates between the range of '" + boundry_start.Day + "-" + boundry_start.Month + "-" + boundry_start.Year + "' " +
                                    "& '" + boundry_end.Day + "-" + boundry_end.Month + "-" + boundry_end.Year + "'";
            DateTime fromDt;
            DateTime toDt;
          //if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
          //     {
          //         fromDt = new DateTime();
          //         toDt = new DateTime();
          //         fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
          //         toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
          //         if (toDt.CompareTo(fromDt) < 0)
          //         {
          //             lblSearchMsg.Text = "To date must be later than From date";
          //             grdExpiry.Visible = false;
          //             lblSearchMsg.ForeColor = System.Drawing.Color.Red;
          //             return;
          //         }
          //         else if (Convert.ToDateTime(txtFrom.Text.ToString()) < DateTime.Now.Date)
          //         {
          //             lblSearchMsg.Text = date_err_msg;//"You can not select From date earlier than 15 days from current date!";
          //             return;
          //         }
          //         else if (Convert.ToDateTime(txtFrom.Text.ToString()) > DateTime.Now.Date.AddDays(15))
          //         {
          //             lblSearchMsg.Text = date_err_msg;//"You can not select From date later than 15 days from current date!";
          //             return;
          //         }
          //         else if (Convert.ToDateTime(txtTo.Text.ToString()) < DateTime.Now.Date)
          //         {
          //             lblSearchMsg.Text = date_err_msg;// "You can not select To date earlier than 15 days from current date!";
          //             return;
          //         }
          //         else if (Convert.ToDateTime(txtTo.Text.ToString()) > DateTime.Now.Date.AddDays(15))
          //         {
          //             lblSearchMsg.Text = date_err_msg;// "You can not select To date later than 15 days from current date!";
          //             return;
          //         }
          //         else
          //         {
          //             lblSearchMsg.Text = "";
          //             grdExpiry.Visible = true;
          //         }
          //     }
            //comment by vivek 20150627

            Hashtable htAddPlanParams = getLedgerParamsData();

            string type = "";
            if (rbALL.Checked)
            {
                type = "ALL";
            }
            else if (rbAD.Checked)
            {
                type = "AD";
            }
            else if (rbAL.Checked)
            {
                type = "AL";
            }

            else if (rbB.Checked)
            {
                type = "B";
            }
            string planname = "";
            if (ddlPlanname.SelectedIndex == 0)
            {
                planname = "ALL";
            }
            else
            {
                planname = ddlPlanname.SelectedValue.ToString();
            }

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
            //if (catid == "3")
            //{
            Cls_BLL_TransHwayBulkRenewal objTran = new Cls_BLL_TransHwayBulkRenewal();
            dt = objTran.GetExpDetails(htAddPlanParams, username, operator_id, catid, type, planname);
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
                //btnGenerateExcel.Visible = false;
                btnProceed.Visible = false;
                grdExpiry.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                //btnGenerateExcel.Visible = true;
                grdExpiry.Visible = true;
                btnProceed.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdExpiry.DataSource = dt;
                grdExpiry.DataBind();
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
                    lbllcoName.Text = grdExpiry.Rows[rowindex].Cells[3].Text;
                    lblfullname.Text = ((HiddenField)clickedRow.FindControl("hdnfullname")).Value;
                    lbladdress.Text = ((HiddenField)clickedRow.FindControl("HdnAddress")).Value;
                    lblMobile.Text = grdExpiry.Rows[rowindex].Cells[4].Text;
                    lblplan.Text = grdExpiry.Rows[rowindex].Cells[5].Text;
                    lblplantype.Text = grdExpiry.Rows[rowindex].Cells[6].Text;
                    lblEnddate.Text = grdExpiry.Rows[rowindex].Cells[7].Text;
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

        public void proceedRenewals()
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
            string file_name = "";
            string upload_id = "";
            DateTime date = DateTime.Now;
            string cur_timestamp = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
            string cur_time = DateTime.Now.ToString("dd-MMM-yyyy_hh:mm:ss");
            string cur_time_filename_part = DateTime.Now.ToString("dd-MMM-yyyy_hhmmss");
            Random random = new Random();
            upload_id = username + "_" + cur_time + "_" + random.Next(1000, 9999);
            file_name = "Renew_" + username + "_" + cur_time_filename_part;
            StringBuilder sb = new StringBuilder();
            if (((CheckBox)grdExpiry.HeaderRow.FindControl("cbAllrenew")).Checked)
            {
                //---------------------------------------------------------------
                string plan_type = "";
                if (rbAD.Checked)
                {
                    plan_type = "AD";
                }
                else if (rbAL.Checked)
                {
                    plan_type = "AL";
                }
                else if (rbB.Checked)
                {
                    plan_type = "B";
                }
                else
                {
                    plan_type = "ALL";
                }

                string plan_name = ddlPlanname.SelectedValue.ToString();
                //---------------------------------------------------------------


                Cls_BLL_TransHwayBulkRenewal objrenew = new Cls_BLL_TransHwayBulkRenewal();
                string returnval = objrenew.GetBulkRenewData(username, upload_id, file_name, txtFrom.Text, txtTo.Text, plan_name, plan_type);
                if (returnval.Split('$')[0] == "9999")
                {
                    Session["upload_id"] = upload_id;

                    Response.Redirect("../Transaction/TransHwayBulkRenewConf.aspx");
                }
                else
                {
                    lblSearchMsg.Text = returnval.Split('$')[1];
                    return;
                }
                //validated_data_arr = validated_data.Split('#');
            }
            else
            {
                int cnt = 0;
                string autorenwflag = "";
                foreach (GridViewRow item in grdExpiry.Rows)
                {

                    if (item.RowType == DataControlRowType.DataRow && (item.Cells[10].FindControl("cbRenew") as CheckBox).Checked)
                    {
                        //if (item.RowType == DataControlRowType.DataRow && (item.Cells[11].FindControl("cbAutoRenew") as CheckBox).Checked)
                        //CheckBox childchks = item.Cells[10].FindControl("cbRenew") as CheckBox;
                        CheckBox childchk = item.Cells[11].FindControl("cbAutoRenew") as CheckBox;
                        childchk.Attributes.Add("onclick", "Check_CbAuto(this);");
                         if (item.RowType == DataControlRowType.DataRow && (item.Cells[11].FindControl("cbAutoRenew") as CheckBox).Checked)
                        {
                            autorenwflag = "Y";
                        }
                        else if(HidChRenewalValue.Value == "N")
                        {
                            autorenwflag = "N";
                        
                        }
                        string str = "Insert into aoup_lcopre_bulk_temp values ('" + item.Cells[1].Text + "', '" + ((LinkButton)item.FindControl("LBvc")).Text + "', " +
                        " '" + item.Cells[6].Text + "','" + item.Cells[8].Text + "','R','" + username + "','" + upload_id + "','" + cur_timestamp + "','" + ((HiddenField)item.FindControl("hdnBrmpoid")).Value + "','','',SYSDATE,'N','" + file_name + "','N','"+autorenwflag+"')";
                        cnt++;
                        Cls_Helper obj = new Cls_Helper();
                        int returnval = obj.insertQry(str);

                    }
                }
                if (cnt == 0)
                {
                    string msg = "<script type=\"text/javascript\">alert(\"Please Select Atleast One Plan To Renew\");</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", msg);
                    return;
                }
                Session["upload_id"] = upload_id;
                Response.Redirect("../Transaction/TransHwayBulkRenewConf.aspx");

            }
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            CheckBox chkAll = (CheckBox)grdExpiry.HeaderRow.FindControl("cbAllrenew");
            string Message = "";
            if (chkAll.Checked)
            {
                Message = "Are you sure you want to renew all plans";
            }
            else
            {
                Message = "Are you sure you want to renew selected plans";
            }
            lblPopupFinalConfMsg.Text = Message;
            popFinalConf.Show();
        }

        protected void btnPopupConfYes_Click(object sender, EventArgs e)
        {
            proceedRenewals();
        }

        protected void fillCombo()
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
            Hashtable htAddPlanParams = getLedgerParamsData();

            string type = "";
            if (rbALL.Checked)
            {
                type = "ALL";
            }
            else if (rbAD.Checked)
            {
                type = "AD";
            }
            else if (rbAL.Checked)
            {
                type = "AL";
            }

            else if (rbB.Checked)
            {
                type = "B";
            }

            Cls_BLL_TransHwayBulkRenewal objTran = new Cls_BLL_TransHwayBulkRenewal();
            DataTable ds = objTran.FillPlanCombo(htAddPlanParams, username, operator_id, catid, type, username);
            ddlPlanname.DataSource = ds;
            ddlPlanname.DataTextField = "plan_name";
            ddlPlanname.DataValueField = "plan_name";
            ddlPlanname.DataBind();
            ds.Dispose();
            ddlPlanname.Items.Insert(0, "ALL");
        }

        protected void txtFrom_TextChanged(object sender, EventArgs e)
        {
            fillCombo();
        }

        protected void txtTo_TextChanged(object sender, EventArgs e)
        {

            fillCombo();

        }

        private void FileLogText(String Str, String sender, String strRequest, String strResponse, string browser_no)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\BULKRENEWDEV_TEST_" + browser_no + "_" + filename + ".txt", true);
            try
            {
                sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(strRequest.Trim());
                sw.WriteLine(strResponse.Trim());
                sw.WriteLine("************************************************************************************************************************");
            }
            catch (Exception ex)
            {
                //Response.Write("Error while writing logs : " + ex.Message.ToString());
            }
            finally
            {
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        protected void rbALL_CheckedChanged(object sender, EventArgs e)
        {
            fillCombo();
        }

        protected void rbB_CheckedChanged(object sender, EventArgs e)
        {
            fillCombo();
        }


        protected void rbAD_CheckedChanged(object sender, EventArgs e)
        {
            fillCombo();
        }

        protected void rbAL_CheckedChanged(object sender, EventArgs e)
        {
            fillCombo();
        }

        protected void ChckedChanged(object sender, EventArgs e)
        {
            

         /*   CheckBox chk = sender as CheckBox;
            if (chk.Checked == true)
            {
                GridViewRow row = (GridViewRow)chk.NamingContainer;
                CheckBox chkRow1 = (row.Cells[0].FindControl("cbAutoRenew") as CheckBox);
                chkRow1.Enabled = true;
            }
            else
            {
                GridViewRow row = (GridViewRow)chk.NamingContainer;
                CheckBox chkRow1 = (row.Cells[0].FindControl("cbAutoRenew") as CheckBox);
                chkRow1.Enabled = false;
            }*/
            foreach (GridViewRow row in grdExpiry.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("cbRenew") as CheckBox);
                    if (chkRow.Checked)
                    {

                        CheckBox chkRow1 = (row.Cells[0].FindControl("cbAutoRenew") as CheckBox);
                        chkRow1.Enabled = true;
                    }
                    else
                    {
                        CheckBox chkRow1 = (row.Cells[0].FindControl("cbAutoRenew") as CheckBox);
                        chkRow1.Enabled = false; ;
                    }
                }
            }
        }

        protected void CHCKEDCHANGEDALL(object SENDER, EventArgs E)
        {
            //GETSELECTEDROWS();
            //BINDSECONDGRID();


            foreach (GridViewRow row in grdExpiry.Rows)
            {

                if (row.RowType == DataControlRowType.Header)
                {
                    CheckBox CHKROW = (row.Cells[0].FindControl("CBALLRENEW") as CheckBox);
                    if (CHKROW.Checked)
                    {

                        CheckBox CHKROW1 = (row.Cells[0].FindControl("CBALLAUTORENEW") as CheckBox);
                        CHKROW1.Enabled = true;
                    }
                    else
                    {
                        CheckBox CHKROW1 = (row.Cells[0].FindControl("CBALLAUTORENEW") as CheckBox);
                        CHKROW1.Enabled = false;
                    }
                    
                }
            }
        }
    }
}
