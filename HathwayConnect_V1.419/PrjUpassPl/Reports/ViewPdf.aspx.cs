using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace PrjUpassPl.Reports
{
    public partial class ViewPdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            String ExportPath = Server.MapPath("..\\MyExcelFile\\");
            Byte[] buffer = client.DownloadData(ExportPath + Request.QueryString["ID"]);

            if (buffer != null)
            {
                /*
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
                */
                FileStream fs = null;
                fs = File.Open(ExportPath + Request.QueryString["ID"], FileMode.Open);
                byte[] btFile = new byte[fs.Length];
                fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
                Response.End();

            }
        }
    }
}