using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Configuration;
using System.Data.OracleClient;
using System.Collections;

namespace PrjUpassPl.Helper
{
    public class Cls_Presentation_Helper
    {
        public string GetDatabaseDateSource()
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection con = new OracleConnection(ConStr);
            String a = con.ConnectionString;
            String b = con.DataSource;

            return b;
        }

        public string GetDatabaseUserId()
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection con = new OracleConnection(ConStr);
            String UserId = "";
            ArrayList arr = new ArrayList(con.ConnectionString.Split(';'));

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].ToString() != "")
                {
                    ArrayList arr1 = new ArrayList(arr[i].ToString().Split('='));

                    if (arr1[0].ToString().ToLower() == "user id")
                    {
                        UserId = arr1[1].ToString();
                    }
                }
            }

            return UserId;
        }

        public string GetDatabasePassword()
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection con = new OracleConnection(ConStr);
            String Password = "";
            ArrayList arr = new ArrayList(con.ConnectionString.Split(';'));

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].ToString() != "")
                {
                    ArrayList arr1 = new ArrayList(arr[i].ToString().Split('='));

                    if (arr1[0].ToString().ToLower() == "password")
                    {
                        Password = arr1[1].ToString();
                    }
                }
            }

            return Password;
        }

        public string GenerateXls(string path, DataGrid grid)
        {
            StringWriter sw = null;
            HtmlTextWriter hw = null;
            StreamWriter strmWriter = null;
            try
            {
                string excelPath = "";
                sw = new StringWriter();
                hw = new HtmlTextWriter(sw);
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderStyle.BackColor = System.Drawing.Color.Maroon;
                grid.HeaderStyle.ForeColor = System.Drawing.Color.White;
                grid.BorderColor = System.Drawing.Color.DarkBlue;
                grid.RenderControl(hw);
                excelPath = HttpContext.Current.Server.MapPath(path);
                strmWriter = new StreamWriter(excelPath);
                strmWriter.Write(sw);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (strmWriter != null)
                    strmWriter.Close();
                if (hw != null)
                    hw.Close();
                if (sw != null)
                    sw.Close();
            }
        }

        public string cDataUpload(string filepath, string ctlfilenamepath, string logfilepath, string tablecol, string dbsid, string dbuser, string dbpwd, string tablename, string upload_id)
        {
            string returnval = "";
            string tablecolumn = "";
            string strConn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection oraConn = new OracleConnection(strConn);
            oraConn.Open();
            if (tablecol == "")
            {
                if (tablename == "")
                {
                    return returnval = "";
                }
                else
                {
                    string str = "select column_name abc from user_tab_columns  where upper(table_name)=upper('" + tablename + "') order by column_id ";
                    OracleCommand cmd = new OracleCommand(str, oraConn);
                    OracleDataReader droper = cmd.ExecuteReader();
                    
                    bool a = droper.HasRows;
                    
                    while (droper.Read())
                    {
                        tablecolumn += droper["abc"].ToString() + ",";
                    }

                    droper.Close();
                    tablecolumn = "(" + tablecolumn.ToString().TrimEnd(',') + ")";
                }
            }
            oraConn.Close();
            try
            {
                string file = "";
                string a = "LOAD DATA";
                string b = "INFILE '" + filepath + "'";
                string c = "APPEND INTO  TABLE " + tablename + " ";
                string d = "";
                string g = "FIELDS TERMINATED BY ";
                string h = "" + "\"" + "	" + "\"";
                string e = "TRAILING NULLCOLS";
                
                if (tablecol == "")
                {
                    file = a + Environment.NewLine + b + Environment.NewLine + c + Environment.NewLine + d + Environment.NewLine + g + h + Environment.NewLine + e + Environment.NewLine + tablecolumn;
                }
                else
                {
                    file = a + Environment.NewLine + b + Environment.NewLine + c + Environment.NewLine + d + Environment.NewLine + g + h + Environment.NewLine + e + Environment.NewLine + tablecol;
                }
                StreamWriter sw = new StreamWriter(ctlfilenamepath);
                sw.WriteLine(file);
                sw.Flush();
                sw.Close();
                
                string ctlfilename = ctlfilenamepath.Substring(ctlfilenamepath.LastIndexOf('\\') + 1);
                string dir = ctlfilenamepath.Substring(0, ctlfilenamepath.LastIndexOf('\\'));
                string logfilename = logfilepath.Substring(logfilepath.LastIndexOf('\\') + 1);

                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                psi.Arguments = @"/c sqlldr " + dbuser + "/" + dbpwd + "@" + dbsid + " control=" + ctlfilename + " log=" + logfilename + " ";
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardError = true;
                psi.WorkingDirectory = dir;

                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);
                System.IO.StreamReader sOut = proc.StandardOutput;
                System.IO.StreamWriter sIn = proc.StandardInput;

                string results = sOut.ReadToEnd().Trim();

                proc.WaitForExit();

                if (proc.ExitCode == 0)
                {
                    returnval = "0";
                    //xlblmsg.Text = "Uploaded Successfully";
                }
                else
                {
                    System.IO.StreamReader sError = proc.StandardError;
                    returnval = sError.ReadToEnd().Trim();
                    //xlblmsg.Text = error.ToString().Trim();
                }
            }
            catch (Exception e)
            {
                returnval = e.Message.ToString();
            }

            return returnval;
        }
    }
}