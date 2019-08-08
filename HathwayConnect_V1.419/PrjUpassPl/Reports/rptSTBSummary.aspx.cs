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
    public partial class rptSTBSummary : System.Web.UI.Page
    {
        decimal amt = 0;
        DateTime dtime = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "STB Summary Detail :";
            Session["RightsKey"] = "N";
            if (!IsPostBack)
            {
                


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
                    //if (category_id == "11")
                    //{
                    //    ddlLco.Items.Insert(0, new ListItem("Select LCO", "0"));
                    //}
                    //else if (category_id == "3")
                    //{
                    //    //ddllco_SelectedIndexChanged(null, null);
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
            String val = "";
            if (e.CommandSource as LinkButton != null)
            {
                int rowIndex = ((e.CommandSource as LinkButton).NamingContainer as GridViewRow).RowIndex;
       

                val = (string)this.grdBulkProc.DataKeys[rowIndex]["stb_id"].ToString();


                GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                String Count = "";
                if (e.CommandName.Equals("stb_faulty"))
                {
                    Count = ((Label)clickedRow.FindControl("lbSTBFaulty")).Text + "_stb_faulty";
                }
                if (e.CommandName.Equals("vc_faulty"))
                {
                    Count = ((Label)clickedRow.FindControl("lblvcfaulty")).Text + "_vc_faulty";
                }
                if (e.CommandName.Equals("stb_good"))
                {
                    Count = ((Label)clickedRow.FindControl("lblStbGood")).Text + "_stb_good";
                }
                if (e.CommandName.Equals("vc_good"))
                {
                    Count = ((Label)clickedRow.FindControl("lblVDGood")).Text + "_vc_good";
                }
                if (e.CommandName.Equals("lnkSTBUndelivered"))
                {
                    Count = ((Label)clickedRow.FindControl("lblSTBUndelivered")).Text + "_lnkSTBUndelivered";
                }
                if (e.CommandName.Equals("vc_undelivered"))
                {
                    Count = ((Label)clickedRow.FindControl("lblVCUndelivered")).Text + "_vc_undelivered";
                }

                BindPopup(val, Count);
            }
            else
            {
                binddata();
            }
                    }

        protected void grdBulkstatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBulkstatus.PageIndex = e.NewPageIndex;
        }

        private void BindPopup(string trans_id, string Query2Part)
        {
            String QueryPart = "";
            String Type = "";
            if (Query2Part.Contains("vc"))
            {
                Type = "VC";
            }
            else { Type = "STB"; }

            if (Query2Part.Contains("faulty")) { QueryPart = "confirmstatus IN ('HF','WF','FF','F')"; }
            if (Query2Part.Contains("good")) { QueryPart = "confirmstatus='G'"; }
            if (Query2Part.Contains("unde")) { QueryPart = "confirmstatus IN ('HU','WU','FU','U','LT')"; }

            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);

           // Cls_Business_RptBulkTransProces objTran = new Cls_Business_RptBulkTransProces();
            // dt = objTran.GetDetails(username, operator_id, catid, Unique_id);
            String Query = "select * from view_STB_summary_det where trans_id='" + trans_id + "' and type='" + Type + "' and ";
            Query += QueryPart;
            OracleCommand cmd = new OracleCommand(Query, con);
            OracleDataAdapter DaObj = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();

            DaObj.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                lblPDCMsg.Text = "No Data Found";



            }


            else
            {
                grdBulkstatus.DataSource = dt;
                grdBulkstatus.DataBind();

                popCheques.Show();


            }
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

            str = " select a.stb_id,a.receiptno,a.total,a.stb_faulty,a.vc_faulty,a.stb_good,a.vc_good,a.stb_undelivered,a.vc_undelivered,a.stbpendingcount";
          str+=",a.vcpendingcount,a.scheme_name,a.scheme_type from view_STB_summary_mst a" ;

          if (txtRecptNo.Text.ToString() != "" && from != "" && to != "")
          {
              str += " where trunc(a.insdate) >='" + from + "' ";
              str += " and trunc(a.insdate) <='" + to + "' and a.lco_code= '" + ddlLco.SelectedValue.ToString().Trim() + "' ";
              str += "' and a.receiptno='" + txtRecptNo.Text.ToString() + "'";
          } else
              if (txtRecptNo.Text.ToString() == "" && from != "" && to != "") {
                  str += " where trunc(a.insdate) >='" + from + "' ";
                  str += " and trunc(a.insdate) <='" + to + "' and a.lco_code= '" + ddlLco.SelectedValue.ToString().Trim() + "' ";
              }

          //str += " where trunc(a.insdate) >='" + from + "' ";
          //str += " and trunc(a.insdate) <='" + to + "' and a.lco_code= '" + ddlLco.SelectedValue.ToString().Trim() + "' ";
          //str += "Or  a.lco_code= '" + ddlLco.SelectedValue.ToString().Trim() + "' and a.receiptno='" + txtRecptNo.Text.ToString() + "'";



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

                grdBulkProc.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdBulkProc.DataSource = dt;
                grdBulkProc.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdBulkProc.ClientID + "', 400, 1200 , 37 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");
            }



        }

        protected void grdBulkProc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBulkProc.PageIndex = e.NewPageIndex;
          //  grdPISTransDet.PageIndex = e.NewPageIndex;
            grdBulkProc.DataBind();
            if (ViewState["searched_trans"] != null)
            {
                grdBulkProc.DataSource = ViewState["searched_trans"];
                grdBulkProc.DataBind();
            }
        }

    }
}