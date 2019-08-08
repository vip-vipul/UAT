using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;


namespace PrjUpassPl.Transaction
{
    public partial class transRupassDistLimit : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                
                //string strConn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                //OracleConnection conn = new OracleConnection(strConn);
                //if (conn.State != ConnectionState.Open)
                //    conn.Open();
                //string str = "select num_oper_id,var_oper_opername from aoup_Operator_def where num_oper_category='3' and var_oper_compcode in ('BUCCS','HCCS','TESTRUPASS')";
                //OracleCommand cmdoper = new OracleCommand(str, conn);
                //OracleDataAdapter da = new OracleDataAdapter(cmdoper);
                //DataSet ds = new DataSet();
                //da.Fill(ds);

                //ddlDist.DataSource = ds;


                //ddlDist.DataTextField = "var_oper_opername";
                //ddlDist.DataValueField = "num_oper_id";

                //ddlDist.DataBind();
                //distlimitlco();
                //binddatagrid();

                //txtCashAmt.Text = "";
                //txtbranchnm.Text = "";
                //txtReferenceNo.Text = "";
                //txtBankName.Text = "";
                //ddlBankName.SelectedIndex = 0;
            }
            btnSubmit.Attributes.Add("onclick", "javascript:return dovalid1()");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] SearchOperators(string prefixText, int count)
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = " select a.var_oper_opername from aoup_operator_def a" +
            " where upper(a.var_oper_opername) like upper('" + prefixText + "%')";
            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Operators.Add(dr["var_oper_opername"].ToString());
            }
            string[] prefixTextArray = Operators.ToArray<string>();
            con.Close();
            return prefixTextArray;


            //List<string> Operators = new List<string>();
            //Operators.Add("ranjan singh");
            //Operators.Add("ranjit mahajan");
            //Operators.Add("rinky sharma");
            //Operators.Add("rukhsana shaikh");
            //Operators.Add("ryan oberoy");
            
            //string[] prefixTextArray = Operators.ToArray<string>();
            //return prefixTextArray;

        }

        
        protected void radCash_CheckedChanged(object sender, EventArgs e)
        {
            lblmsg.Text = "";
        }

        protected void radCheque_CheckedChanged(object sender, EventArgs e)
        {
            lblmsg.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("transRupassDistLimit.aspx");
        }



    }
}
