using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PrjUpassBLL.Reports;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.Data.OracleClient;
using System.Drawing;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PrjUpassPl.Reports
{
    public partial class rptEcafCustDetails : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Today;
        string totcountSum;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Master.PageHeading = "ECAF Report";
            if (!IsPostBack)
            {

                // grd.PageIndex = 0;



                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                //  btngrnExel.Visible = false;
                btnGenerateExcel.Visible = false;

                try
                {
                    Label2.Text = "";
                    if (Request.QueryString["vc"].ToString().Trim() != "")
                    {  
                        pnldetail.Visible = false;
                        dIVtOTgRIDD.Visible = false;
                        cls_business_rptEcafCust ob = new cls_business_rptEcafCust();
                        DataTable dt = new DataTable();
                        Hashtable htCrf = getCrfParams();

                        Hashtable htResponse;
                        htResponse = ob.getReportVC(Session["username"].ToString(), Request.QueryString["vc"].ToString());

                        if (htResponse["data"] != null)
                        {
                            dt = (DataTable)htResponse["data"];
                        }

                        if (dt.Rows.Count > 0)
                        {
                            divData.Visible = true;
                            dIvGridReport.Visible = true;
                            grd.DataSource = dt;
                            grd.DataBind();
                        }
                        else
                        {
                            divData.Visible = true;
                            dIvGridReport.Visible = true;
                            Label2.Text = "No ECAF found for the customer";
                        }

                    }
                }
                catch { }
            }
        }

        private Hashtable getCrfParams()
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
            string from = txtFrom.Text;
            string to = txtTo.Text;
            //  lblResultCount.Text = "";
            Label1.Text = "";
            lblSearchMsg.Text = "";
            DateTime fromDt;
            DateTime toDt;
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
            {
                fromDt = new DateTime();
                toDt = new DateTime();
                fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    // grdAddPlantopup.Visible = false;
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
                    // grdAddPlantopup.Visible = true;
                }
            }

            Hashtable htCrf = getCrfParams();

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

            cls_business_rptEcafCust obj = new cls_business_rptEcafCust();

            DataTable dt = new DataTable();
            Hashtable htResponse;
            if (chkdt.Checked == true)
            {

                htResponse = obj.getCrfdata(username, htCrf);

            }

            else if (chkVC.Checked == true)
            {
                if (txtVCid.Text == "" || txtVCid.Text == null)
                {
                    Label1.Text = "VC Id Can't Be Blank";
                    return;
                }
                else
                {
                    string valid = SecurityValidation.chkData("T", txtVCid.Text);

                    if (valid == "")
                        htResponse = obj.getCrfVC(username, txtVCid.Text.Trim());
                    else
                    {
                        Label1.Text = valid.ToString();
                       
                        return;

                    }
                }


               // htResponse = obj.getCrfVC(username, txtVCid.Text.Trim());
            }

            else
            {
                Label1.Text = "Please search by any one of the choice!!";
                return;

            }

            if (htResponse["data"] != null)
            {
                dt = (DataTable)htResponse["data"];
            }


            string strParams = htResponse["ParamStr"].ToString();
            if (!String.IsNullOrEmpty(strParams))
            {
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>Top-up Parameters : </b>" + strParams);
                lblSearchMsg.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt == null)
            {

                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            else if (dt.Rows.Count == 0)
            {
                Label1.Text = "No Data Found";
                divData.Visible = false;

            }
            else
            {
                xDGr.DataSource = dt;
                xDGr.DataBind();

                divData.Visible = true;
                dIVtOTgRIDD.Visible = true;
                dIvGridReport.Visible = false;
            }
        }

        protected void xDGr_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                totcountSum += Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "total"));
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                e.Item.Cells[2].Text = "" + totcountSum;
            }
        }

        protected void xDGr_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            dIVtOTgRIDD.Visible = false;

            cls_business_rptEcafCust ob = new cls_business_rptEcafCust();
            DataTable dt = new DataTable();
            Hashtable htCrf = getCrfParams();

            Hashtable htResponse;

            if (chkdt.Checked == true)
            {

                htResponse = ob.getReport(Session["username"].ToString(), htCrf);

            }

            else
            {
                //htCrf = null;
                htResponse = ob.getReportVC(Session["username"].ToString(), txtVCid.Text.Trim());
            }



            if (htResponse["data"] != null)
            {
                dt = (DataTable)htResponse["data"];
            }


            string strParams = htResponse["ParamStr"].ToString();
            if (!String.IsNullOrEmpty(strParams))
            {
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>Top-up Parameters : </b>" + strParams);
                lblSearchMsg.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt == null)
            {

                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            else if (dt.Rows.Count == 0)
            {
                Label1.Text = "No Data Found";
                // divData.Visible = false;

            }
            else
            {
                grd.DataSource = dt;
                grd.DataBind();

                // divData.Visible = true;
                dIvGridReport.Visible = true;
            }

        }

        // Button btnEdit = (Button)sender;

        protected void lnkkyc_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int RowIndex = Convert.ToInt32(row.RowIndex);
            String VC = grd.Rows[RowIndex].Cells[11].Text;
            HiddenField hdnidproofpath = (HiddenField)grd.Rows[RowIndex].FindControl("hdnidproofpath");
            HiddenField hdnresiproofpath = (HiddenField)grd.Rows[RowIndex].FindControl("hdnresiproofpath");
            HiddenField hdndocumentproofpath = (HiddenField)grd.Rows[RowIndex].FindControl("hdndocumentproofpath");
            VC = VC.Replace(":", "_");
            String PDFNAME= "EcafDocumentProof_" + VC + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
            String ecafno = grd.Rows[RowIndex].Cells[1].Text;
            convertoonepdf(hdnidproofpath.Value.Trim().Replace("yesbank","yesbanktemp"), hdnresiproofpath.Value.Trim().Replace("yesbank","yesbanktemp"), hdndocumentproofpath.Value.Trim().Replace("yesbank","yesbanktemp"), VC, PDFNAME, ecafno);
            var creatfpdf = Server.MapPath("~/MyExcelFile/");
            var finalPDF = System.IO.Path.Combine(creatfpdf, PDFNAME);

            if (File.Exists(finalPDF))
            {
                Response.Write("<script type='text/javascript'>");
                Response.Write("window.open('ViewPdf.aspx?ID=" + PDFNAME + "','_blank');");
                Response.Write("</script>");
            }
            else
            {
                Response.Write("<script type='text/javascript'>");
                Response.Write("alert('PDF not created');");
                Response.Write("</script>");
            }
        }

        protected void lnkPdf_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                int RowIndex = Convert.ToInt32(row.RowIndex);

                String VC = grd.Rows[RowIndex].Cells[11].Text;
                // VC = VC.Replace(":", "_");
                String Stbno = grd.Rows[RowIndex].Cells[12].Text;
                HiddenField hdnaccno = (HiddenField)grd.Rows[RowIndex].FindControl("hdnaccno");
                //Response.Redirect("../Reports/rptcrfReport.aspx?CustNo=" + id);
                DataTable TblchildTV = new DataTable();
                DataTable TblDetail = new DataTable();
                GetChildVC(hdnaccno.Value.Trim(), TblchildTV);

                TblchildTV.Columns.Add("Signature", typeof(Byte[]));
                TblchildTV.Columns.Add("Photo", typeof(Byte[]));
                TblchildTV.Columns.Add("resiproof", typeof(Byte[]));
                TblchildTV.Columns.Add("idproof", typeof(Byte[]));
                TblchildTV.Columns.Add("logo", typeof(Byte[]));

                GetCustDetail(hdnaccno.Value.Trim(), VC, Stbno, TblDetail);

                if (TblDetail.Rows.Count > 0)
                {
                    String PhotoImagePath = TblDetail.Rows[0]["imagepath"].ToString().Replace("yesbank", "yesbanktemp");
                    String signaturePath = TblDetail.Rows[0]["signpath"].ToString().Replace("yesbank", "yesbanktemp");
                    String idproofPath = TblDetail.Rows[0]["idproofpath"].ToString().Replace("yesbank", "yesbanktemp");
                    String resiproofPath = TblDetail.Rows[0]["addressproofpath"].ToString().Replace("yesbank", "yesbanktemp");

                    Byte[] Photoimage = convertimgtobyte(PhotoImagePath, true);

                    Byte[] signatureimage = convertimgtobyte(signaturePath, false);

                    Byte[] idproofimage = new Byte[0]; //convertimgtobyte(idproofPath,true);


                    Byte[] resiproofimage = new Byte[0]; //convertimgtobyte(resiproofPath, true);
                    //String logopath = Server.MapPath("~/Images/HathwayLogo - Copy.png");
                    //Byte[] logoimage = convertimgtobyte(logopath);

                    foreach (DataRow dr in TblchildTV.Rows)
                    {
                        dr["Signature"] = signatureimage;
                        dr["Photo"] = Photoimage;
                        dr["resiproof"] = resiproofimage;
                        dr["idproof"] = idproofimage;
                    }

                    if (TblchildTV.Rows.Count == 0)
                    {
                        TblchildTV.Rows.Add(null, null, null, null, null, null, null, null, null, null, null);
                        foreach (DataRow dr in TblchildTV.Rows)
                        {
                            dr["Signature"] = signatureimage;
                            dr["Photo"] = Photoimage;
                            dr["resiproof"] = resiproofimage;
                            dr["idproof"] = idproofimage;
                        }
                    }

                    //TblchildTV.Rows[0]["logo"] = logoimage;

                    GenerateReportManual(TblchildTV, TblDetail);
                }
            }
            catch (Exception ex)
            {
                FileLogTextChange1(ex.ToString(), "lnkPDF_click :-", "", "");
            }


        }

        private byte[] convertimgtobyte(String Path,bool show)
        {
            byte[] bArr = new byte[0];
            if (Path != "")
            {
                try
                {
                    //System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(new WebClient().DownloadData(Path)));

                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(Path);
                    MemoryStream ms = new MemoryStream(bytes);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                    bArr = imgToByteArray(img);
                }
                catch
                {
                    if (show == true)
                    {
                        try
                        {
                            Path = "http://localhost/yesbank/MobImages/no-image-icon.jpg";
                            WebClient wc = new WebClient();
                            byte[] bytes = wc.DownloadData(Path);
                            MemoryStream ms = new MemoryStream(bytes);
                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);


                            bArr = imgToByteArray(img);
                        }
                        catch
                        { }
                    }
                }
            }
            else
            {
                if (show == true)
                {
                    Path = "http://localhost/yesbank/MobImages/no-image-icon.jpg";
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(Path);
                    MemoryStream ms = new MemoryStream(bytes);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);


                    bArr = imgToByteArray(img);
                }
            }
            return bArr;
        }

        public byte[] imgToByteArray(System.Drawing.Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        public void GetChildVC(String Accno, DataTable Tblchildtv)
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();

            String str = " select var_cust_child_accno accno,var_cust_child_childvc childvc,var_cust_child_childstb childstb,var_cust_child_insby insby," +
                "dat_cust_child_insdate insdate,var_cust_child_entitycode Entity " +
                    "from aoup_crf_cust_childtv_def where var_cust_child_accno= '" + Accno + "' order by dat_cust_child_insdate";


            OracleCommand Cmd = new OracleCommand(str, con);
            OracleDataAdapter AdpData = new OracleDataAdapter();
            AdpData.SelectCommand = Cmd;
            AdpData.Fill(Tblchildtv);

            con.Close();

        }

        public void GetCustDetail(String Accno, String VC, String Stbno, DataTable TblDetail)
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();

            String str = "select * from view_ecaf_customerdetail_repor " +
              "where   accno= '" + Accno + "' and stbno='" + Stbno + "' and vcid='" + VC + "' and updatedonform='Y'";



            OracleCommand Cmd = new OracleCommand(str, con);
            OracleDataAdapter AdpData = new OracleDataAdapter();
            AdpData.SelectCommand = Cmd;
            AdpData.Fill(TblDetail);

            con.Close();

        }

        protected void chkdt_CheckedChanged(object sender, EventArgs e)
        {
            chkVC.Checked = false;
        }

        protected void chkVC_CheckedChanged(object sender, EventArgs e)
        {
            chkdt.Checked = false;
        }

        public void GenerateReportManual(DataTable tblchildtv, DataTable Tbl)
        {
            try
            {
                String downloadpath = "Ecaf_Form_" + DateTime.Now.ToString("dd-MM-yyhhmmss") + ".pdf";

                String ReportPath = Server.MapPath("New_CrystalReportECAFpdf.rpt");
                String ExportPath = Server.MapPath("..\\MyExcelFile\\") + downloadpath;
                DataTable dtCompany = new DataTable();
                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection con = new OracleConnection(strCon);
                if (Tbl.Rows.Count > 0)
                {
                    FileLogTextChange1(Tbl.Rows[0]["state"].ToString(), "GenerateReportManual :-", "", "");
                    string str = "select var_StateName,var_ownername,var_Address from aoup_lcopre_ST_Company_det where Upper(var_StateName)='" + Tbl.Rows[0]["state"].ToString() + "' and var_adtype='SP' ";
                    FileLogTextChange1(str, "string :-", "", "");
                    OracleCommand Cmd = new OracleCommand(str, con);
                    OracleDataAdapter AdpData = new OracleDataAdapter();
                    AdpData.SelectCommand = Cmd;
                    AdpData.Fill(dtCompany);
                }

                ReportDocument rpt = new ReportDocument();


                DataSet DSet = new DataSet();

                DSet.Tables.Add(tblchildtv);

                tblchildtv.TableName = "tblchildtv";

                rpt.Load(ReportPath);
                rpt.SetDataSource(DSet);

                rpt.SetParameterValue("Lco_Name", Tbl.Rows[0]["lconame"].ToString());
                rpt.SetParameterValue("CIN_No", Tbl.Rows[0]["cinno"].ToString());
                rpt.SetParameterValue("Lco_Address", Tbl.Rows[0]["lcoaddress"].ToString());
                rpt.SetParameterValue("Reg_no", Tbl.Rows[0]["regno"].ToString());
                rpt.SetParameterValue("Date_of_issue", Tbl.Rows[0]["Date_of_issue"].ToString());
                rpt.SetParameterValue("Date_of_expiry", Tbl.Rows[0]["Date_of_expiry"].ToString());
                rpt.SetParameterValue("Service_tax_reg_no", Tbl.Rows[0]["Service_tax_reg_no"].ToString());
                rpt.SetParameterValue("E_Tax_No", Tbl.Rows[0]["E_Tax_No"].ToString());
                rpt.SetParameterValue("Pan_No", Tbl.Rows[0]["panno"].ToString());
                rpt.SetParameterValue("Tin_No", Tbl.Rows[0]["Tin_No"].ToString());
                rpt.SetParameterValue("Applicant_Name", Tbl.Rows[0]["CUSTNAME"].ToString());
                rpt.SetParameterValue("Contact_Person", Tbl.Rows[0]["CONTECT_PERSON"].ToString());
                rpt.SetParameterValue("Flat_No", Tbl.Rows[0]["PLOTNO"].ToString() + "," + Tbl.Rows[0]["Floor"].ToString());
                rpt.SetParameterValue("Floor", Tbl.Rows[0]["Floor"].ToString());
                rpt.SetParameterValue("Society_name", Tbl.Rows[0]["BULIDING"].ToString());
                rpt.SetParameterValue("Area", Tbl.Rows[0]["Area"].ToString());
                rpt.SetParameterValue("Street_name", Tbl.Rows[0]["STREET"].ToString()+"," + Tbl.Rows[0]["Area"].ToString());
                rpt.SetParameterValue("City", Tbl.Rows[0]["City"].ToString());
                rpt.SetParameterValue("State", Tbl.Rows[0]["State"].ToString());
                rpt.SetParameterValue("pincode", Tbl.Rows[0]["pincode"].ToString());
                rpt.SetParameterValue("telno", Tbl.Rows[0]["MOBILENO"].ToString());
                rpt.SetParameterValue("Emailid", Tbl.Rows[0]["EMAIL"].ToString());
                rpt.SetParameterValue("main_vcid", Tbl.Rows[0]["VCID"].ToString());
                rpt.SetParameterValue("main_stbno", Tbl.Rows[0]["STBNO"].ToString());
                rpt.SetParameterValue("cafno", Tbl.Rows[0]["cafno"].ToString());
                rpt.SetParameterValue("lcocode", Tbl.Rows[0]["LCOCODE"].ToString());
                rpt.SetParameterValue("TransDate", Tbl.Rows[0]["insdate"].ToString());
                rpt.SetParameterValue("ip", Tbl.Rows[0]["ip"].ToString());

                rpt.SetParameterValue("Billingflat", Tbl.Rows[0]["PLOTNO"].ToString() + "," + Tbl.Rows[0]["Floor"].ToString());
                rpt.SetParameterValue("billingstreetname", Tbl.Rows[0]["STREET"].ToString() + "," + Tbl.Rows[0]["Area"].ToString());
                rpt.SetParameterValue("billingsocietyname", Tbl.Rows[0]["BULIDING"].ToString());

                rpt.SetParameterValue("billingcity", Tbl.Rows[0]["City"].ToString());
                rpt.SetParameterValue("billingpincode", Tbl.Rows[0]["pincode"].ToString());
                rpt.SetParameterValue("contactno", "");
                rpt.SetParameterValue("Age", "");
                rpt.SetParameterValue("CCadd", dtCompany.Rows[0]["var_Address"].ToString());
                rpt.SetParameterValue("var_ownername", "");//COMPANY_LOCALADDRESS
                rpt.SetParameterValue("StateName", Tbl.Rows[0]["State"].ToString());
                rpt.SetParameterValue("LCO_MobileNo", Tbl.Rows[0]["LCO_MobileNo"].ToString());

                //-------
                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, ExportPath);
                rpt.Close();
                rpt.Dispose();
                Response.Write("<script type='text/javascript'>");
                Response.Write("window.open('ViewPdf.aspx?ID=" + downloadpath + "','_blank');");
                Response.Write("</script>");
            }
            catch (Exception ex)
            {
                FileLogTextChange1(ex.ToString(), "GenerateReportManual", "", "");
            }
            /*
            FileStream fs = null;
            fs = File.Open(ExportPath, FileMode.Open);
            byte[] btFile = new byte[fs.Length];
            fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
            fs.Close();


            Response.AddHeader("Content-disposition", "attachment; filename=" + downloadpath);
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(btFile);
            Response.End();*/
        }

        private Byte[] getheader_text()
        {
            using (var ms = new System.IO.MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.Open();
                var TableFont = FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD);
                doc.Add(new Paragraph("Document Details", TableFont));
                doc.Close();
                return ms.ToArray();
            }
        }

        public void convertoonepdf(String Idproofimage, String resiproofimage, string documentproof,String VC,string PDFName,String Ecafno)
        {
            try
            {
                var creatfpdf = Server.MapPath("~/MyExcelFile/");
             
                var finalPDF = System.IO.Path.Combine(creatfpdf, PDFName);
               
                var Header_text = getheader_text();

                //This variable will eventually hold our combined PDF as a byte array
                Byte[] finalFileBytes;

                //Write everything to a MemoryStream
                using (var finalFile = new System.IO.MemoryStream())
                {

                    //Create a generic Document
                    Document doc = new Document();


                    //Use PdfSmartCopy to intelligently merge files
                    PdfSmartCopy copy = new PdfSmartCopy(doc, finalFile);


                    //Open our document for writing
                    doc.Open();

                    //#1 - Import the SSRS report

                    //Bind a reader to our SSRS report
                    PdfReader.unethicalreading = true;
                    if (Idproofimage != "http://localhost/yesbank/MobImages/")
                    {
                        if (Idproofimage.ToUpper().Contains(".PDF"))
                        {
                            try
                            {
                                PdfReader reader1 = new PdfReader(Idproofimage);

                                for (var i = 1; i <= reader1.NumberOfPages; i++)
                                {
                                    copy.AddPage(copy.GetImportedPage(reader1, i));
                                }
                                reader1.Close();
                            }
                            catch
                            { }
                        }
                        else
                        {
                            try
                            {
                                iTextSharp.text.Rectangle pageSize = null;
                                WebClient wc = new WebClient();
                                byte[] bytes = wc.DownloadData(Idproofimage);
                                MemoryStream ms = new MemoryStream(bytes);
                                using (var srcImage = new Bitmap(ms))
                                {
                                    if (srcImage.Width < 580)
                                    {
                                        pageSize = new iTextSharp.text.Rectangle(0, 0, 580, srcImage.Height + 100);
                                    }
                                    else
                                    {
                                        pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height + 100);
                                    }
                                }

                                //Will eventually hold the PDF with the image as a byte array
                                Byte[] imageBytes;

                                //Simple image to PDF
                                using (var m = new MemoryStream())
                                {
                                    Document d = new Document(pageSize, 0, 0, 0, 0);

                                    PdfWriter w = PdfWriter.GetInstance(d, m);

                                    d.Open();
                                    d.Add(iTextSharp.text.Image.GetInstance(Idproofimage));
                                    d.Close();
                                    //Grab the bytes before closing out the stream
                                    imageBytes = m.ToArray();
                                }

                                //Now merge using the same merge code as #1
                                PdfReader reader1 = new PdfReader(imageBytes);

                                for (var i = 1; i <= reader1.NumberOfPages; i++)
                                {
                                    copy.AddPage(copy.GetImportedPage(reader1, i));
                                }
                                reader1.Close();
                                ms.Close();
                            }
                            catch
                            { }
                        }

                    }
                    //#3 - Merge additional PDF
                    if (resiproofimage != "http://localhost/yesbank/MobImages/")
                    {
                        if (resiproofimage.ToUpper().Contains(".PDF"))
                        {
                            try
                            {
                                PdfReader reader1 = new PdfReader(resiproofimage);
                                
                                for (var i = 1; i <= reader1.NumberOfPages; i++)
                                {
                                    copy.AddPage(copy.GetImportedPage(reader1, i));
                                }
                                reader1.Close();
                            }
                            catch
                            { }
                        }
                        else
                        {
                            try
                            {
                                iTextSharp.text.Rectangle pageSize = null;
                                WebClient wc = new WebClient();
                                byte[] bytes = wc.DownloadData(resiproofimage);
                                MemoryStream ms = new MemoryStream(bytes);
                                using (var srcImage = new Bitmap(ms))
                                {
                                    if (srcImage.Width < 580)
                                    {
                                        pageSize = new iTextSharp.text.Rectangle(0, 0, 580, srcImage.Height + 100);
                                    }
                                    else
                                    {
                                        pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height + 100);
                                    }
                                }

                                //Will eventually hold the PDF with the image as a byte array
                                Byte[] imageBytes;

                                //Simple image to PDF
                                using (var m = new MemoryStream())
                                {
                                    Document d = new Document(pageSize, 0, 0, 0, 0);

                                    PdfWriter w = PdfWriter.GetInstance(d, m);

                                    d.Open();
                                    d.Add(iTextSharp.text.Image.GetInstance(resiproofimage));
                                    d.Close();



                                    //Grab the bytes before closing out the stream
                                    imageBytes = m.ToArray();
                                }

                                //Now merge using the same merge code as #1
                                PdfReader reader1 = new PdfReader(imageBytes);

                                for (var i = 1; i <= reader1.NumberOfPages; i++)
                                {
                                    copy.AddPage(copy.GetImportedPage(reader1, i));
                                }
                                reader1.Close();
                                ms.Close();
                            }
                            catch
                            { }
                        }
                    }

                    if (documentproof != "http://localhost/yesbank/MobImages/")
                    {
                        try
                        {

                            PdfReader reader1 = new PdfReader(documentproof);

                            for (var i = 1; i <= reader1.NumberOfPages; i++)
                            {
                                copy.AddPage(copy.GetImportedPage(reader1, i));
                            }
                            reader1.Close();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    doc.Close();
                    copy.Close();

                    //Grab the bytes before closing the stream
                            finalFileBytes = finalFile.ToArray();

                            System.IO.File.WriteAllBytes(finalPDF, finalFileBytes);

                            byte[] bytesaa = File.ReadAllBytes(finalPDF);
                            iTextSharp.text.Font blackFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                            using (MemoryStream stream = new MemoryStream())
                            {
                                PdfReader reader = new PdfReader(bytesaa);
                                using (PdfStamper stamper = new PdfStamper(reader, stream))
                                {
                                    int pages = reader.NumberOfPages;
                                    for (int i = 1; i <= pages; i++)
                                    {
                                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("E-CAF No. : "+Ecafno, blackFont), 568f, 15f, 0);
                                    }
                                }
                                bytesaa = stream.ToArray();
                            }
                            File.WriteAllBytes(finalPDF, bytesaa);

                }
            }
            catch
            { }

        }

        private void FileLogTextChange1(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            if (!Directory.Exists(@"C:\temp\Logs\HwayChangeLog"))
            {
                Directory.CreateDirectory(@"C:\temp\Logs\HwayChangeLog");
            }
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayChangeLog\HwayObrm_SMS1_" + filename + ".txt", true);
                sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(strRequest.Trim());
                sw.WriteLine(strResponse.Trim());
                sw.WriteLine("************************************************************************************************************************");
            }
            catch (Exception ex)
            {
                // Response.Write("Error while writing logs : " + ex.Message.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

    }
}