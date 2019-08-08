using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace PrjUpassPl
{
    public class ClsGenerateExcel
    {
        public void ExportToExcel(DataTable dtExcel, String XlsName, System.Web.UI.Page objPage)
        {
            string contentType = "application/msexcel";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = contentType;
            string attachment = string.Format("attachment; filename=\"{0}\"", XlsName + ".xls");
            HttpContext.Current.Response.AddHeader("Content-Disposition", attachment);
            byte[] preamble = Encoding.UTF8.GetPreamble();
            HttpContext.Current.Response.BinaryWrite(preamble);

            string strFontSize = "13";

            HttpContext.Current.Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" \r\n xmlns:x=\"urn:schemas-microsoft-com:office:excel\" \r\n xmlns=\"http://www.w3.org/TR/REC-html40\">");
            HttpContext.Current.Response.Write(@"<head>");
            HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">");
            HttpContext.Current.Response.Write("<meta name=ProgId content=Excel.Sheet>");
            HttpContext.Current.Response.Write("<meta name=Generator content=\"Microsoft Excel 11\">");
            HttpContext.Current.Response.Write(@"</head>");
            HttpContext.Current.Response.Write(@"<body>");

            HttpContext.Current.Response.Write(@"<table style='widtExcelh: 100%; font-size:" + strFontSize + ";' border='1'>");

            foreach (DataColumn column in dtExcel.Columns)
            {
                HttpContext.Current.Response.Write(@"<td><b>" + column.ColumnName + "</b></td>");
            }
            HttpContext.Current.Response.Write(@"</tr>");


            string strCell = "";
            foreach (DataRow row in dtExcel.Rows)
            {
                HttpContext.Current.Response.Write(@"<tr>");
                foreach (DataColumn column in dtExcel.Columns)
                {

                    strCell = row.ItemArray[dtExcel.Columns[column.ColumnName].Ordinal].ToString().Trim();
                    HttpContext.Current.Response.Write(@"<td style ='  mso-number-format:\@;'>" + strCell + "</td>");
                }
                HttpContext.Current.Response.Write(@"</tr>");
            }
            HttpContext.Current.Response.Write(@"</table>");
            HttpContext.Current.Response.Write(@"</body>");
            HttpContext.Current.Response.Write(@"</html>");
            HttpContext.Current.Response.End();
        }
    }
}