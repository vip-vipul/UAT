using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.IO;

namespace PrjUpassDAL.Reports
{
   public class Cls_Data_RptAddPlan_JV
    {
        public DataTable GetTransations(string whereClauseStr, string username)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT lconame, lcoid, lcocode, sum(amt)amt, sum(cnt)cnt,sum(amtdd) amtdd " +
                                " FROM view_jvAddPlan_Trans_summ " +
                " where " + whereClauseStr +
                " group by lconame, lcoid, lcocode ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptAddPlan.cs");
                return null;
            }
        }

        public DataTable GetTransationsMSO(string whereClauseStr, string username)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT sum(amt)amt, sum(cnt)cnt,sum(amtdd) amtdd,  ";
                StrQry += " a.msoname,a.MSOID ";
                StrQry += " FROM view_jvaddplan_trans_summ_mso a ";
                StrQry += " where " + whereClauseStr;
                StrQry += " group by a.msoname,a.MSOID ";

                //FileLogText(StrQry, "GetTransationsMSO", "view_addplan_trans_summ_mso", StrQry);
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptAddPlan.cs");
                return null;
            }
        }

        public DataTable GetTransationsU(string whereClauseStr1, string username1)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT lconame, uname, usrid, userowner, sum(amt)amt, sum(cnt)cnt,sum(amtdd) amtdd " +
                                " FROM view_JVaddplan_trans_summ " +
                " where " + whereClauseStr1 +
                " group by lconame, uname, usrid, userowner ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username1, ex.Message.ToString(), "Cls_Data_RptAddPlan.cs");
                return null;
            }
        }

        public DataTable GetTransationsDet(string whereClauseStr2, string username2)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT * FROM VIEW_JVPLAN_TRANS_DET " +
                " where " + whereClauseStr2;

                FileLogText(StrQry, "GetTransationsDet", "view_AddPlan_Trans_det", StrQry);


                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username2, ex.Message.ToString(), "Cls_Data_RptAddPlan.cs");
                return null;
            }
        }

        public void FileLogText(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\Hway\logquery_" + filename + ".txt", true);
                sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(strRequest.Trim());
                sw.WriteLine(strResponse.Trim());
                sw.WriteLine("************************************************************************************************************************");
            }
            catch (Exception ex)
            {
                //Response.Write("Error while writing logs : " + ex.Message.ToString());
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
