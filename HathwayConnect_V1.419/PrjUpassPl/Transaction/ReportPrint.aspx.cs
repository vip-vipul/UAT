using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;
using System.Text;
using System.IO;
using System.Resources;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;

namespace PrjUpassPl.Transaction
{
    public partial class ReportPrint : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Button butClose;
       // protected CrystalReportViewer CrystalReportViewer1;

        protected void Page_Load(object sender, EventArgs e)
        {
            //CrystalReportViewer1.HasPageNavigationButtons = true;
            //CrystalReportViewer1.HasGotoPageButton = true;
            //CrystalReportViewer1.HasDrillUpButton = true;
            //CrystalReportViewer1.HasRefreshButton = true;
            //CrystalReportViewer1.HasSearchButton = true;
            
            //CrystalReportViewer1.HasZoomFactorList = true;
            //CrystalReportViewer CrystalReportViewer1 = null; 
            ReportData("D");
        }

        private void ReportData(string flag)
        {
            try
            {
                    Recieptgeneral myreceiptreport = new Recieptgeneral();
                    string ReportPath = Server.MapPath("Recieptgeneral.rpt");
                    myreceiptreport.Load(ReportPath);
                    //    ds.Tables[0] = TblAdmitCard;
                    //    rpt.SetDataSource(ds.Tables["TblAdmitCard"]);

                    //rpt.SetParameterValue("Semester", ddlsem.SelectedItem.Text);
                    //rpt.SetParameterValue("RptHead", RptHead);

                    myreceiptreport.SetParameterValue("par_rcptno", Session["rcptno1"].ToString());
                    myreceiptreport.SetParameterValue("par_rcptdt", Session["date1"].ToString());
                    myreceiptreport.SetParameterValue("par_lcocd", Session["lcocd1"].ToString());
                    myreceiptreport.SetParameterValue("par_lconm", Session["lconm1"].ToString());
                    myreceiptreport.SetParameterValue("par_cashier", Session["cashiername"].ToString());
                    myreceiptreport.SetParameterValue("par_amt", Session["amt1"].ToString());
                    myreceiptreport.SetParameterValue("par_paymode", Session["paymode1"].ToString());
                    if (Session["paymode1"].ToString() == "Cheque" || Session["paymode1"].ToString() == "DD")
                    {
                        myreceiptreport.SetParameterValue("par_cheqno", Session["cheqno1"].ToString());
                        myreceiptreport.SetParameterValue("par_bnknm", Session["bnknm1"].ToString());

                    }
                    else if (Session["paymode1"].ToString() == "Cash")
                    {
                        myreceiptreport.SetParameterValue("par_cheqno", "N/A");
                        myreceiptreport.SetParameterValue("par_bnknm", "N/A");
                    }
                    myreceiptreport.SetParameterValue("par_premark", Session["premark1"].ToString());
                    myreceiptreport.SetParameterValue("par_company", Session["company"].ToString());
                    myreceiptreport.SetParameterValue("par_address", Session["address"].ToString());
                    if (Session["paymode1"].ToString() == "Cheque")
                    {
                        myreceiptreport.SetParameterValue("par_chq", "cheque is subject to clearance");
                    }
                    else
                    {
                        myreceiptreport.SetParameterValue("par_chq", ".");
                    }
                    String ExportPath = Server.MapPath("..\\MyExcelFile\\") + Session["rcptno1"].ToString() + " myrecpt " + DateTime.Now.ToString("dd-MM-yy hh mm ss") + ".pdf";
                    myreceiptreport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, ExportPath);

                    FileStream fs = null;
                    fs = File.Open(ExportPath, FileMode.Open);
                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    Response.AddHeader("Content-disposition", "attachment; filename=" + Session["rcptno1"].ToString() + " myrecpt " + DateTime.Now.ToString("dd-MM-yy hh mm ss") + ".pdf");
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(btFile);
                    Response.End();
                    //CrystalReportViewer1.ReportSource = myreceiptreport;  ..shri
                    //CrystalReportViewer1.DataBind();                   
                //if (flag == "P")
                    //{
                    //    myreceiptreport.PrintToPrinter(1, false, 0, 0);
                    //}
                //myreceiptreport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"C:\Temp\PDF\a.pdf");

                //myreceiptreport.Dispose();
                //myreceiptreport.Close();

                //FileStream fs = null;
                //fs = File.Open(@"C:\Temp\PDF\\a.pdf", System.IO.FileMode.Open);
                //byte[] btFile = new byte[fs.Length];
                //fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                //fs.Close();
                //Response.AddHeader("Content-disposition", "attachment; filename=a.pdf");
                //Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(btFile);
                //Response.End();
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Transaction/TransHwayLcoPayment.aspx");
        }

        //protected void btnPrint_Click(object sender, EventArgs e)
        //{
        //    ReportData("P");
        //}
    }
}