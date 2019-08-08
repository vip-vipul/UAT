using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.OracleClient;

namespace PrjUpassDAL.Helper
{
    public class Cls_Security
    {
       
        public void InsertIntoDb(string uname,string msg, string page)
        {
            
                string strConn = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
                OracleConnection oraConn = new OracleConnection(strConn);
                try
                {
                    
                    oraConn.Open();

                    OracleCommand oracmd = new OracleCommand("aoup_yesbanksec_excep", oraConn);
                    oracmd.CommandType = CommandType.StoredProcedure;

                    oracmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                    oracmd.Parameters["in_username"].Value = uname.ToString().Trim();

                    oracmd.Parameters.Add("in_msg", OracleType.VarChar, 1000);
                    oracmd.Parameters["in_msg"].Value = msg.ToString().Trim();

                    oracmd.Parameters.Add("in_page", OracleType.VarChar, 100);
                    oracmd.Parameters["in_page"].Value = page.ToString().Trim();

                    oracmd.Parameters.Add("out_errcode", OracleType.Number);
                    oracmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                    oracmd.Parameters.Add("out_errtext", OracleType.VarChar, 300);
                    oracmd.Parameters["out_errtext"].Direction = ParameterDirection.Output;

                    oracmd.ExecuteNonQuery();
                    oraConn.Close();

                    int errcode = Convert.ToInt32(oracmd.Parameters["out_errcode"].Value);
                }
                catch (Exception)
                {
                    
                }
                finally
                {
                    oraConn.Close();
                    oraConn.Dispose();
                }
            
        }
        public Boolean rightsCheck(int userid, string uname, string page)
        {
            string strConn = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection oraConn = new OracleConnection(strConn);
            try
            {

                oraConn.Open();

                OracleCommand oracmd = new OracleCommand("aoup_yesbanksec_rightschk", oraConn);
                oracmd.CommandType = CommandType.StoredProcedure;

                oracmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                oracmd.Parameters["in_username"].Value = uname.ToString().Trim();

                oracmd.Parameters.Add("in_userid", OracleType.Number, 20);
                oracmd.Parameters["in_userid"].Value = userid.ToString().Trim();

                oracmd.Parameters.Add("in_page", OracleType.VarChar, 100);
                oracmd.Parameters["in_page"].Value = page.ToString().Trim();

                oracmd.Parameters.Add("out_errcode", OracleType.Number);
                oracmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                oracmd.ExecuteNonQuery();
                oraConn.Close();

                int errcode = Convert.ToInt32(oracmd.Parameters["out_errcode"].Value);

                if (errcode == 9999)
                {
                    return true;
                }
                else if (errcode == -230)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return false;
            }
            finally
            {
                oraConn.Close();
                oraConn.Dispose();
            }
           
        }
    }
}
