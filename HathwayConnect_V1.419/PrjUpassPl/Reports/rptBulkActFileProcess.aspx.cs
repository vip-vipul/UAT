using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Text;
using System.IO;
using System.Drawing;

namespace PrjUpassPl.Reports
{
    public partial class rptBulkActFileProcess : System.Web.UI.Page
    {
        decimal amt = 0;
        DateTime dtime = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Bulk Scheduler File Process Status";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;



                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();

                FillLcoDetails();

            }
        }

        protected void FillLcoDetails()
        {
            string str = "";
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

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,var_lcomst_code lcocode ";
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

                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
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

        private Hashtable getTopupParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            Hashtable htTopupParams = new Hashtable();
            htTopupParams.Add("from", from);
            htTopupParams.Add("to", to);
            return htTopupParams;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            binddata();

        }

        protected void grdBulkProc_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("Count"))
            {

                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    string uniqueid = ((Label)clickedRow.FindControl("lbluid1")).Text;
                    string processdt = ((Label)clickedRow.FindControl("lblprocessdt")).Text;
                    DateTime dtprocess = Convert.ToDateTime(processdt);
                    string filePath = "";

                    if (uniqueid != "" || processdt != "")
                    {
                        Session["uniqueid"] = uniqueid;
                        Session["processdt"] = dtprocess.ToString("dd/MMM/yyyy");
                        Session["flag"] = RadSearchby.SelectedValue.ToString();
                        Response.Redirect("../Reports/rptBulkActFileProcessRemove.aspx");

                    }



                }
                catch (Exception ex)
                {

                    Response.Write("Error : " + ex.Message.Trim());
                    return;
                }
            }

        }




        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }


        protected void binddata()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            DateTime fromDt;
            DateTime toDt;
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
            {
                fromDt = new DateTime();
                toDt = new DateTime();
                fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);

                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";

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

                }
            }

            Hashtable htTopupParams = getTopupParamsData();

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

            if (RadSearchby.SelectedValue == "E")
            {
                str = " select a.uploadid,a.filename,a.insdt,a.Total,var_lcopre_bulk_process_flag status,a.processdate,null,null from view_lcopre_bulkact_file_proc a " +
                     " where trunc(a.insdt) >='" + from + "' " +
                      " and trunc(a.insdt) <='" + to + "' and a.lcocode= '" + ddlLco.SelectedValue.ToString().Trim() + "' ";
            }
            else
            {
                str = " select a.uploadid,a.filename,a.insdt,a.Total,var_lcopre_bulk_process_flag status,a.processdate,delby,deldate from VIEW_BULKACTDEL_FILE_PROC a " +
                       " where trunc(a.insdt) >='" + from + "' " +
                        " and trunc(a.insdt) <='" + to + "' and a.lcocode= '" + ddlLco.SelectedValue.ToString().Trim() + "' ";
            }


            OracleCommand cmd = new OracleCommand(str, con);
            OracleDataAdapter DaObj = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();

            DaObj.Fill(dt);


            if (dt.Rows.Count == 0)
            {
                grdBulkProc.Visible = false;
                lblSearchMsg.Text = "No data found";

            }

            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["status"].ToString() == "Y")
                    {
                        dt.Rows[i]["status"] = "Complete";
                    }
                    else
                    {
                        dt.Rows[i]["status"] = "Pending";
                    }
                }
                grdBulkProc.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;

                if (RadSearchby.SelectedValue == "E") 
                {
                    grdBulkProc.Columns[7].Visible = false;
                    grdBulkProc.Columns[8].Visible = false;
                  
                   
                }
                else
                {
                    grdBulkProc.Columns[7].Visible = true;
                    grdBulkProc.Columns[8].Visible = true;
                  
                }
                grdBulkProc.DataSource = dt;
                grdBulkProc.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdBulkProc.ClientID + "', 400, 1200 , 37 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");
            }



        }

        protected void grdBulkProc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBulkProc.PageIndex = e.NewPageIndex;
            binddata();
        }

    }
}