using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;
using System.Collections;
using PrjUpassBLL.Master;
using System.Net;
using System.Text;
using System.IO;

namespace PrjUpassPl.Master
{
    public partial class mstecafstbtransfer : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            Session["RightsKey"] = "N";
            Master.PageHeading = "STB Transfer";

            if (!IsPostBack)
            {
                try
                {
                    DataTable dt = new DataTable();
                    string GetLcodetails = "select  num_crlimit_actuallimit actuallimit,num_lcomst_cityid cityid,var_lcomst_dasarea DAS from aoup_lcopre_crlimit_det left outer join aoup_lcopre_lco_det on var_crlimit_lcocode=var_lcomst_code";
                    GetLcodetails += " where var_crlimit_lcocode='" + Session["username"].ToString() + "'";
                    lbllcocode.Text = Session["username"].ToString();

                    OracleCommand cmd = new OracleCommand(GetLcodetails, con);
                    OracleDataAdapter DaObj = new OracleDataAdapter(cmd);


                    DaObj.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        lbllcobal.Text = dt.Rows[0]["actuallimit"].ToString();

                        string getSTBRate = "";

                        getSTBRate += " select coalesce(max(rate1),max(rate2),max(rate3))rate from ( select case when num_stbrateconfig_city=" + dt.Rows[0]["cityid"].ToString();
                        getSTBRate += " and var_stbrateconfig_das='" + dt.Rows[0]["DAS"].ToString() + "' and var_stbrateconfig_lco='" + Session["username"].ToString() + "'";
                        getSTBRate += " then num_stbrateconfig_rate end rate1, case when num_stbrateconfig_city=" + dt.Rows[0]["cityid"].ToString() + " and var_stbrateconfig_das='" + dt.Rows[0]["DAS"].ToString() + "'";
                        getSTBRate += " and var_stbrateconfig_lco is null then num_stbrateconfig_rate end rate2, case when num_stbrateconfig_city=" + dt.Rows[0]["cityid"].ToString() + " and ";
                        getSTBRate += "  var_stbrateconfig_das is null and var_stbrateconfig_lco is null then num_stbrateconfig_rate end rate3 from AOUP_LCOPRE_STBRATECONFIG_MST) ";

                        OracleCommand cmdSTBRate = new OracleCommand(getSTBRate, con);
                        OracleDataAdapter DaObjSTBRate = new OracleDataAdapter(cmdSTBRate);
                        DataTable dtSTBRate = new DataTable();
                        DaObjSTBRate.Fill(dtSTBRate);

                        if (dtSTBRate.Rows.Count > 0)
                        {
                            lblstbrate.Text = dtSTBRate.Rows[0]["rate"].ToString();
                            ViewState["STBrate"] = dtSTBRate.Rows[0]["rate"].ToString();
                        }
                        else
                        {
                            lblstbrate.Text = "0";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblResponseMsg.Text = ex.Message;
                }
            }

        }



        protected void btncnfmBlck_Click(object sender, EventArgs e)
        {
            try
            {
                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection con = new OracleConnection(strCon);



                if (lblstbrate.Text == "0")
                {
                    PopMsgBoxErr.Show();
                    lblInformation.Text = "STB Rate can not be 0";
                    return;
                }
                if (rblType.SelectedValue == "S")
                {
                    string cur_time = DateTime.Now.ToString("dd-MMM-yyyy_hhmmss");
                    Random random = new Random();
                    string RefId = "REF" + "_" + cur_time + "_" + random.Next(1000, 9999);
                    if (txtstbno.Text == "")
                    {

                        PopMsgBoxErr.Show();
                        lblInformation.Text = "Please enter STB No.";
                        return;
                    }

                    string Getstb = "select account_no,lco_code from VIEW_HWCAS_BRM_CUST_MASTER  where stb='" + txtstbno.Text.Trim() + "' and connection_type='CHILD' ";

                    OracleCommand cmd = new OracleCommand(Getstb, con);
                    OracleDataAdapter DaObj = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    DaObj.Fill(dt);


                    if (dt.Rows.Count == 0)
                    {
                        PopMsgBoxErr.Show();
                        lblInformation.Text = "No data found";
                        return;

                    }


                    string in_translcoCode = dt.Rows[0]["lco_code"].ToString();

                    if(in_translcoCode==Session["username"].ToString())
                    {
                         PopMsgBoxErr.Show();
                        lblInformation.Text = "STb belongs to same LCO";
                        return;
                    }
                    string in_translcocity = "";
                    string in_translcoDAS = "";
                    Hashtable ht = new Hashtable();
                    ht.Add("in_stb", txtstbno.Text.Trim());
                    ht.Add("in_lcoCode", Session["username"].ToString());
                    ht.Add("in_translcoCode", in_translcoCode);
                    ht.Add("in_translcocity", in_translcocity);
                    ht.Add("in_translcoDAS", in_translcoDAS);
                    ht.Add("in_amount", lblstbrate.Text);
                    ht.Add("in_RefId", RefId);
                    ht.Add("in_operid", Session["operator_id"].ToString());
                    Cls_BLL_ecafstbtransfer obj = new Cls_BLL_ecafstbtransfer();
                    string response = obj.InsertDetails(Session["username"].ToString(), ht);
                    string[] responseArr = response.Split('$');
                    if (responseArr[0].ToString() == "9999")
                    {
                        ViewState["Erorcode"] = "9999";
                    }
                    PopMsgBoxErr.Show();
                    lblInformation.Text = responseArr[1].ToString();
                    return;

                }
                else
                {
                    string cur_time = DateTime.Now.ToString("dd-MMM-yyyy_hhmmss");
                    Random random = new Random();
                    string RefId = "REF" + "_" + cur_time + "_" + random.Next(1000, 9999);

                    if (FileUpload1.HasFile == true)
                    {
                        DateTime dd = DateTime.Now;
                        string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;
                        String Path = Server.MapPath("~//ImageGarbage") + "\\" + datetime + "STbTransfer";

                        if (FileUpload1.HasFile)
                        {
                            FileUpload1.SaveAs(Path + ".txt");
                        }

                        string[] lines = System.IO.File.ReadAllLines(Path + ".txt");
                        if (lines.Length > 1000)
                        {
                            lblResponseMsg.Text = "Records should not be greater than 1000";
                            return;
                        }

                        DataTable dtResponse = new DataTable();
                        foreach (string STBNo in lines)
                        {
                            if (dtResponse.Columns.Contains("STBNo"))
                            {
                            }
                            else
                            {
                                dtResponse.Columns.Add("STBNo");
                            }

                            dtResponse.Rows.Add(STBNo);
                        }

                        if (dtResponse.Rows.Count > 0)
                        {
                            DataTable distinct = dtResponse.DefaultView.ToTable(true, "STBNo");
                            if (distinct.Rows.Count == dtResponse.Rows.Count)
                            {
                                //there are no duplicates
                            }
                            else
                            {
                                PopMsgBoxErr.Show();
                                lblInformation.Text = "Duplicate STB not allowed";
                                return;

                            }
                        }
                        foreach (string STBNo in lines)
                        {
                            string Getstb = " select account_no,lco_code from VIEW_HWCAS_BRM_CUST_MASTER  where stb='" + STBNo + "'";


                            OracleCommand cmd = new OracleCommand(Getstb, con);
                            OracleDataAdapter DaObj = new OracleDataAdapter(cmd);
                            DataTable dt = new DataTable();

                            DaObj.Fill(dt);


                            if (dt.Rows.Count == 0)
                            {
                                PopMsgBoxErr.Show();
                                lblInformation.Text = "No data found";
                                return;
                            }

                            else
                            {

                                string in_translcoCode = dt.Rows[0]["lco_code"].ToString();

                                if (in_translcoCode == Session["username"].ToString())
                                {
                                    PopMsgBoxErr.Show();
                                    lblInformation.Text = "LCO belongs to same City";
                                    return;
                                }
                                string in_translcocity = "";
                                string in_translcoDAS = "";


                                Hashtable ht = new Hashtable();
                                ht.Add("in_stb", STBNo);
                                ht.Add("in_lcoCode", Session["username"].ToString());
                                ht.Add("in_translcoCode", in_translcoCode);
                                ht.Add("in_translcocity", in_translcocity);
                                ht.Add("in_translcoDAS", in_translcoDAS);
                                ht.Add("in_amount", lblstbrate.Text);
                                ht.Add("in_RefId", RefId);
                                ht.Add("in_operid", Session["operator_id"].ToString());
                                Cls_BLL_ecafstbtransfer obj = new Cls_BLL_ecafstbtransfer();
                                string response = obj.InsertDetails(Session["username"].ToString(), ht);
                                string[] responseArr = response.Split('$');
                                PopMsgBoxErr.Show();
                                lblInformation.Text = responseArr[1].ToString();
                            }
                        }
                    }
                    else
                    {
                        PopMsgBoxErr.Show();
                        lblInformation.Text = "Please Upload File";
                        return;
                    }
                }



            }
            catch (Exception ex)
            {
                lblResponseMsg.Text = ex.Message;
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "S")
            {
                if (txtstbno.Text.Trim() == "")
                {
                    PopMsgBoxErr.Show();
                    lblInformation.Text = "Please enter STB No.";
                    return;
                }
            }

            if (rblType.SelectedValue == "B")
            {
                if (FileUpload1.HasFile == false)
                {
                    PopMsgBoxErr.Show();
                    lblInformation.Text = "Please Upload File";
                    return;
                }

            }

            if (ViewState["STBrate"] == null)
            {
                ViewState["STBrate"] = "0";
            }
            popMsgBox.Show();
            lblMsg.Text = "Are You Sure, Total STB Rate :" + ViewState["STBrate"].ToString();


        }


        //
        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (ViewState["Erorcode"] != null)
            {
                if (ViewState["Erorcode"].ToString() == "9999")
                {
                    Response.Redirect("../Reports/EcafPages.aspx");
                }
                else
                {
                    PopMsgBoxErr.Hide();
                }
            }
            else
            {
                PopMsgBoxErr.Hide();
            }
        }
        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "S")
            {
                trsingle.Visible = true;
                trbulk.Visible = false;
            }
            else
            {
                trsingle.Visible = false;
                trbulk.Visible = true;
            }
        }
    }
}