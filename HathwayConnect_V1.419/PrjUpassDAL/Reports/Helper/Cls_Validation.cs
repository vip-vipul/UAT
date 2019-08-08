using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;

namespace PrjUpassDAL.Helper
{
    public class Cls_Validation
    {
        //SELECT a.lco_poid, a.lco_id, a.lco_operid, a.lco_name FROM view_lcopre_lcopoid a
        string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
        public bool CheckBalance(string username, double planamt, double availBal)
        {
            if (planamt > availBal)
                return false;
            else
                return true;
        }

        public bool MaxPackvalid(string username, double NumberofPack)
        {
            if (NumberofPack > 100)
                return false;
            else
                return true;
        }

        public string CustDataAccess(string username, string operator_id, string lco_poid, string OperCat)
        {
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {

                //string strQry = "SELECT a.var_lcomst_brmpoid FROM aoup_lcopre_lco_det a WHERE a.num_lcomst_operid = " + operator_id;


                string strQry = "SELECT a.num_lcomst_operid||'$'||a.num_lcomst_cityid||'$'||var_lcomst_code||'$'||u.num_user_curcrlimit||'$'||a.var_lcomst_name||'$'||a.var_lcomst_dasarea  lcoid FROM aoup_lcopre_lco_det a ,aoup_operator_def c,aoup_user_def u ";
                strQry += "WHERE a.num_lcomst_operid = c.num_oper_id and  a.num_lcomst_operid=u.num_user_operid and a.var_lcomst_code=u.var_user_username";
                strQry += " and instr('" + lco_poid + "',var_lcomst_brmpoid)>0";
                if (OperCat == "11")
                {
                    strQry += " and c.num_oper_clust_id = " + operator_id + "";
                }
                else
                {
                    strQry += "and a.num_lcomst_operid =  " + operator_id + "";
                }

                OracleCommand Cmd = new OracleCommand(strQry, conObj);
                conObj.Open();
                string lcocity = Convert.ToString(Cmd.ExecuteScalar());
                if (lcocity == null)
                {
                    lcocity = "";
                }
                return lcocity;


            }
            catch (Exception ex)
            {
                FileLogText("ex", "", username + "_" + operator_id + "_" + lco_poid + "_" + ex.Message + "_" + ex.StackTrace, "");
                return "";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }

        }

        public string lcoTransCode(string username)
        {
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string strQry = "select  var_lcomst_onlineidentifier from aoup_lcopre_lco_det where var_lcomst_code='" + username + "'";

                OracleCommand Cmd = new OracleCommand(strQry, conObj);
                conObj.Open();
                string lcoTransCode = Convert.ToString(Cmd.ExecuteScalar());
                if (lcoTransCode == null)
                {
                    lcoTransCode = "";
                }
                return lcoTransCode;


            }
            catch (Exception ex)
            {
                FileLogText("ex", "", username + "_" + ex.Message + "_" + ex.StackTrace, "");
                return "";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }

        }

        private void FileLogText(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\1_" + filename + ".txt", true);
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
