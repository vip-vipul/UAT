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
using System.Configuration;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayOsdBmailFlagMap : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                Master.PageHeading = "BMail Deactivation";

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();

                Child.Visible = false;
                lblSave.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                Parent.Visible = true;
                fillCombo();
            }

        }

        protected void fillCombo()
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

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,num_lcomst_operid lcocode ";
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

        protected void btnSave_Click(object sender, EventArgs e)
        {

            string data = getAllValueToSubmit();
            callprocedureMst(data);

        }
        private void callprocedureMst(string data)
        {
            Cls_BLL_TransOsdBmailNotification obj = new Cls_BLL_TransOsdBmailNotification();

            string username = Convert.ToString(Session["username"]);
            string reference = "";
            if (ViewState["reference"] != null)
            {
                reference = ViewState["reference"].ToString();
            }
            string pro_output = "";
            pro_output = obj.SaveOsdBMailActivation(username, reference, data);
            if (pro_output.Split('$')[0] == "9999")
            {
                lblSave.Text = pro_output.Split('$')[1].ToString();

            }
            else
            {
                lblSave.Text = pro_output.Split('$')[1].ToString();
            }
        }

        private string getAllValueToSubmit()
        {

            string strdata = "";

            try
            {


                for (int i = 0; i < grddt.Rows.Count; i++)
                {
                    string tempstr = "";
                    string flag = "";
                    string flagdelete = "";
                    string TransId = "";

                    CheckBox chk = (CheckBox)grddt.Rows[i].Cells[12].FindControl("cbRenew");
                    CheckBox chkdelete = (CheckBox)grddt.Rows[i].Cells[12].FindControl("cbdelete");
                    TransId = ((HiddenField)grddt.Rows[i].Cells[12].FindControl("hdntransid")).Value.ToString(); ;

                    if (chk.Checked)
                    {
                        flag = "Y";
                    }
                    else
                    {
                        flag = "N";
                    }
                    if (chkdelete.Checked)
                    {
                        flagdelete = "Y";
                    }
                    else
                    {
                        flagdelete = "N";
                    }
                    tempstr = TransId + "$" + flag + "$" + flagdelete;

                    if (strdata.Length == 0)
                    {
                        strdata = tempstr;
                    }
                    else
                    {
                        strdata += "#" + tempstr;
                    }
                }

                return strdata;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Child.Visible = false;
            lblSave.Visible = false;
            btnCancel.Visible = false;
            btnSave.Visible = false;
            Parent.Visible = true;
            parentgrid.Visible = true;
            lblSave.Text = "";
            btnSubmit_Click(null, null);
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            binddata(getSummaryQuery(), grdExpiry);
        }

        protected void binddata(string qry, GridView gr)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

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
            DataSet dt = new DataSet();


            dt = Cls_Helper.fnGetdataset(qry);

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }


            if (dt.Tables[0].Rows.Count == 0)
            {

                lblSearchMsg.Text = "No data found";
                gr.Visible = false;
            }
            else
            {
                gr.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                gr.DataSource = dt;
                gr.DataBind();
            }
        }
        protected void grdExpiry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("referenceid"))
            {
                GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                int rowindex = clickedRow.RowIndex;

                string reference = ((LinkButton)clickedRow.FindControl("lnkreferenceid")).Text;
                ViewState["rowindex"] = reference;
                binddata("select * from view_lcopre_notif_flag_details where referenceid='" + reference + "'", grddt);
                ViewState["reference"] = reference;
                Child.Visible = true;
                lblSave.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;

            }
            parentgrid.Visible = false;
            Parent.Visible = false;
        }
        private string getSummaryQuery()
        {
            try
            {
                string from = txtFrom.Text;
                string to = txtTo.Text;
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
                    return "";
                }


                string str = " select * from view_lcopre_notif_flag_summary ";
                str += " where TRUNC(dt) >= ('" + from + "') AND TRUNC(dt) <= ('" + to + "') ";
                str += " and lcoid=" + operator_id;
                str += " and referenceid like '%" + ddlType.SelectedValue + "%'";

                return str;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        protected void grdExpiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Child.Visible = false;
                lblSave.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                Parent.Visible = true;
                parentgrid.Visible = true;
                grdExpiry.PageIndex = e.NewPageIndex;
                binddata(getSummaryQuery(), grdExpiry);
            }
            catch (Exception ex)
            {

            }
        }
        protected void grddt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Child.Visible = true;
                lblSave.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                Parent.Visible = false;
                parentgrid.Visible = false;
                grdExpiry.PageIndex = e.NewPageIndex;
                binddata("select * from view_lcopre_notif_flag_details where referenceid='" + ViewState["rowindex"].ToString() + "'", grdExpiry);
            }
            catch (Exception ex)
            {
            }
        }

        protected void grddt_DataBinding(object sender, EventArgs e)
        {

        }

        protected void grddt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Int32 count = ((System.Data.DataSet)(grddt.DataSource)).Tables[0].Rows.Count;
                Int32 truecount = ((System.Data.DataSet)(grddt.DataSource)).Tables[0].Select("activeflag='Y'").Length;
                Int32 truecountdelete = ((System.Data.DataSet)(grddt.DataSource)).Tables[0].Select("deleteflag='Y'").Length;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (truecount == count)
                    {
                        CheckBox chk = (CheckBox)e.Row.Cells[12].FindControl("mainCB");
                        chk.Checked = true;
                    }
                    if (truecountdelete == count)
                    {
                        CheckBox chk = (CheckBox)e.Row.Cells[12].FindControl("maindelete");
                        chk.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}