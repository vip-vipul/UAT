using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.OracleClient;
using System.Configuration;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Authentication
{
    public class Cls_Data_forgotpass
    {
        public string ForgotDetails(Hashtable ht)
        {
            OracleConnection conn = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                conn.Open();

                OracleCommand cmd = new OracleCommand("aoup_lcopre_password_reset", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("in_loginid", OracleType.VarChar);
                cmd.Parameters["in_loginid"].Value = ht["UserName"];

                cmd.Parameters.Add("in_IP", OracleType.VarChar);
                cmd.Parameters["in_IP"].Value = ht["IP"];

                cmd.Parameters.Add("out_data", OracleType.VarChar, 250);
                cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("out_errcode", OracleType.Number);
                cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                conn.Close();

                int exeResult = Convert.ToInt32(cmd.Parameters["out_errcode"].Value);
                string Str;

                if (exeResult == 9999)
                {
                    Str = "Password Reset Successfully";
                }
                else
                {
                    Str = Convert.ToString(cmd.Parameters["out_data"].Value);
                }
                conn.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["UserName"].ToString(), ex.Message.ToString(), "Cls_Data_forgotpass.cs");
                return "ex_occured";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
