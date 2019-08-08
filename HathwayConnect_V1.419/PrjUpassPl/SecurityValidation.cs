using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;


namespace PrjUpassPl
{
    public static class SecurityValidation
    {
        private static Boolean chkValidation = false;

        public static bool IsNumeric(this string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        public static string chkData(string ctrlType, string ctrlValue)
        {
            string[] strReserveWords = { "select", "update", "delete", "drop", "truncate","script","alert" };
            string[] strReserveChars = { "*", ";", "-", "'", "=","\"" };

            int cntvalue = 0;
            if (ctrlType == "N")
            {
                if (ctrlValue != "")
                    if (!ctrlValue.IsNumeric())
                        cntvalue = 1;

            }
            else if (ctrlType == "T")
            {
                for (int i = 0; i < strReserveWords.Length; i++)
                {
                    if (ctrlValue.ToLower().Contains(strReserveWords[i]))
                        cntvalue = 1;
                }

                /*if (strReserveWords.Contains(ctrlValue.ToLower()))
                    cntvalue = 1;*/


                for (int i = 0; i < strReserveChars.Length; i++)
                {
                    if (ctrlValue.Contains(strReserveChars[i]))
                        cntvalue = 1;
                }
            }

            if (cntvalue == 1)
                return "Incorrect values entered";
            else
                return "";
            //return chkValidation;
        }

        public static bool SizeUploadValidation(FileUpload fileName)
        {
            
           // System.Drawing.Image img = System.Drawing.Image.FromStream(fileName.PostedFile.InputStream);
            //int height = img.Height;
            //int width = img.Width;
            decimal size = (Math.Round(((decimal)fileName.PostedFile.ContentLength / (decimal)1024), 2)) / 1024;

            if (size <= 5)
                return true;
            else
                return false;

            //  return size;
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Size: " + size + "KB\\nHeight: " + height + "\\nWidth: " + width + "');", true);
        }

    }
}