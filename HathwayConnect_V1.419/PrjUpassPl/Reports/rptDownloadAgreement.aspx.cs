using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Reports;
using System.Data;
using System.Net;
using System.Threading;

namespace PrjUpassPl.Reports
{
    public partial class rptDownloadAgreement : System.Web.UI.Page
    {
        string username = "";
        string operator_id = "";
        string category_id = "";
        string user_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null && Session["user_id"] != null)
                {
                    Session["RightsKey"] = "N";
                    username = Convert.ToString(Session["username"]);
                    operator_id = Convert.ToString(Session["operator_id"]);
                    category_id = Convert.ToString(Session["category"]);
                    user_id = Convert.ToString(Session["user_id"]);
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                    return;
                }
                DataTable dt = new DataTable();
                dt = DownloadAgreement(); //(@"D:\JavaScript In 10 Simple Steps Or Less.pdf"); //get physical file path from server  


                if (dt.Rows.Count>0)
                {
                    string filename, filepath = "";
                    if (Request.QueryString["Flag"] != null)
                    {
                        if (Request.QueryString["Flag"] == "SIA")
                        {
                            filename = "Agreement_File_SIA.pdf";
                            filepath = dt.Rows[0]["var_agreement_sia_path"].ToString();
                        }
                        else
                        {
                            filename = "Agreement_File_MIA.pdf";
                            filepath = dt.Rows[0]["var_agreement_mia_path"].ToString();
                        }
                        Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
                        Response.ContentType = "Application/pdf";
                        Response.WriteFile(filepath.ToString());
                    }
                }
                else
                {

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alert",
                        "alert('Agreement file not available for this city');window.location ='../Reports/rptHwaylcolegelDet.aspx';", true);
                   

                }
            }

        }

        public DataTable DownloadAgreement()
        {
            try
            {
                DataTable dt = new DataTable();
                Cls_Business_RptDownloadAgreement obj = new Cls_Business_RptDownloadAgreement();
                dt = obj.GetAgreementPath(Session["username"].ToString());

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}