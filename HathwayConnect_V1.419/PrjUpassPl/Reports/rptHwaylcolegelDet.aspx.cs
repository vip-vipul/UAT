using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Reports;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Configuration;
using PrjUpassBLL.Master;

namespace PrjUpassPl.Reports
{
    public partial class rptHwaylcolegelDet : System.Web.UI.Page
    {
        string username = "";
        string operator_id = "";
        string category_id = "";
        string user_id = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null && Session["user_id"] != null)
            {
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

            Master.PageHeading = "Legal Content";

            Session["RightsKey"] = null;

            DataTable dt = new DataTable(); // Added by Vivek 18-Apr-2016
            dt = DownloadAgreement();


            if (dt.Rows.Count > 0)
            {
                trMIA.Visible = false;
                trSIA.Visible = true;
            }
            else
            {
                trMIA.Visible = false;
                trSIA.Visible = false;
            }

            DataTable dtMia = new DataTable(); // Added by Vivek 18-Apr-2016
            dtMia = MIAAgreement();


            if (dtMia.Rows.Count > 0)
            {
                trMIAagree.Visible = true;
            }
            else
            {
                trMIAagree.Visible = false;
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

        public DataTable MIAAgreement()
        {
            try
            {
                DataTable dt = new DataTable();
                Cls_Business_RptDownloadAgreement obj = new Cls_Business_RptDownloadAgreement();
                dt = obj.MIAAgreement(Session["operator_id"].ToString());

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        protected void lnkmiaagreement_Click(object sender, EventArgs e)
        {
            DataTable miaData = new DataTable();


            string miamsoarea = "";
            string miamsoregisdate = "";
            string miamsoregisno = "";

            try
            {
                miamsoarea = ConfigurationSettings.AppSettings["miamsoarea"].ToString().Trim();
            }
            catch { miamsoarea = ""; }

            try
            {
                miamsoregisdate = ConfigurationSettings.AppSettings["miamsoregisdate"].ToString().Trim();
            }
            catch { miamsoregisdate = ""; }

            try
            {
                miamsoregisno = ConfigurationSettings.AppSettings["miamsoregisno"].ToString().Trim().Replace('*', '&');
            }
            catch { miamsoregisno = ""; }

            Cls_Business_MstHwaymsgBrodcaster ob = new Cls_Business_MstHwaymsgBrodcaster();
            miaData = ob.getmiadata(username, username);
            if (miaData.Rows.Count > 0)
            {
                if (miaData.Rows[0]["STATUS"].ToString() == "A")
                {
                    DateTime DT = Convert.ToDateTime(miaData.Rows[0]["ACCPTDATE"].ToString());

                    string Curday = DT.ToString("dd");
                    string curMonth = DT.ToString("MMM-yyyy");
                    string curDate = DT.ToString("dd-MMM-yyyy");

                    MIADOC2._lbldatetime ="Date of Acceptance : " +DT.ToString("dd-MM-yyyy hh:mm:ss tt");

                    if (Curday != "")
                    {
                        MIADOC2._lblMIAday = Curday;
                    }
                    if (curMonth != "")
                    {
                        MIADOC2._lblMIAmonth = curMonth;
                    }
                    if (miaData.Rows[0]["LCONAME"].ToString() != "")
                    {
                        MIADOC2._lblMIAlcoName = miaData.Rows[0]["LCONAME"].ToString();
                        MIADOC2._lbllconame = miaData.Rows[0]["LCONAME"].ToString();
                        MIADOC2._lblMIAlcoName2 = miaData.Rows[0]["LCONAME"].ToString();
                    }

                    if (miaData.Rows[0]["ip"].ToString() != "")
                    {
                        MIADOC2._lblip ="IP Address : "+ miaData.Rows[0]["ip"].ToString();
                    }
                    if (miaData.Rows[0]["ip"].ToString() != "")
                    {
                        MIADOC2._lblMIAip = "IP Address : " + miaData.Rows[0]["ip"].ToString();
                    }
                    if (miaData.Rows[0]["ADDRESS"].ToString() != "")
                    {
                        MIADOC2._lblMIAlcoaddress = miaData.Rows[0]["ADDRESS"].ToString();
                    }

                    if (miaData.Rows[0]["REGISNO"].ToString() != "")
                    {
                        MIADOC2._lbllcoregisno = miaData.Rows[0]["REGISNO"].ToString();
                    }

                    if (miaData.Rows[0]["REGISDATE"].ToString() != "")
                    {
                        MIADOC2._lblMIAlcodate = miaData.Rows[0]["REGISDATE"].ToString();
                    }
                    if (miaData.Rows[0]["REGISDATE"].ToString() != "")
                    {
                        MIADOC2._lblMIAdatetime ="Date : "+ miaData.Rows[0]["REGISDATE"].ToString();
                    }
                    if (miaData.Rows[0]["HEADOFF"].ToString() != "")
                    {
                        MIADOC2._lblMIAlcoheadOffice = miaData.Rows[0]["HEADOFF"].ToString();
                    }

                    if (miaData.Rows[0]["TERRITORY"].ToString() != "")
                    {
                        MIADOC2._lbllcoterritory = miaData.Rows[0]["TERRITORY"].ToString();
                    }

                    if (miaData.Rows[0]["AREA"].ToString() != "")
                    {
                        MIADOC2._lblMIAlcoarea = miaData.Rows[0]["AREA"].ToString();
                    }
                    if (miaData.Rows[0]["stateaddress"].ToString() != "")
                    {
                        MIADOC2._lblMIAStateAddress = miaData.Rows[0]["stateaddress"].ToString();
                    }
                    if (miaData.Rows[0]["company"].ToString() != "")
                    {
                        MIADOC2._lblcompanyname = miaData.Rows[0]["company"].ToString();
                    }
                    if (miaData.Rows[0]["company"].ToString() != "")
                    {
                        MIADOC2._lblMIAcompanyname = miaData.Rows[0]["company"].ToString();
                    }
                    

                    if (miamsoarea != "")
                    {
                        MIADOC2._lblmiamsoarea = miamsoarea;
                    }

                    if (miamsoregisdate != "")
                    {
                        MIADOC2._lblmiamsoregisdate = miamsoregisdate;
                    }

                    if (miamsoregisno != "")
                    {
                        MIADOC2._lblmiamsoregisno = miamsoregisno;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Accept MIA Terms')", true);
                    return;
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found')", true);
                return;
            }

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=MIA_Agreement.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Panel1.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }


    }
}