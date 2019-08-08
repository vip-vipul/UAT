using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.OracleClient;
using PrjUpassDAL.Helper;
using System.Data;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_TransHome
    {
        /*
        public string isFirstLogin(string username) {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string strQuery = "SELECT a.num_user_userid FROM aoup_user_def a WHERE a.var_user_username='" + username.Trim() + "' and a.dat_user_lastlogindt is null";
                OracleCommand cmd = new OracleCommand(strQuery, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows) {
                    return "1";
                }else{
                    return "0";
                }
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHome-isFirstLogin");
                return "1";
            }
            finally {
                conObj.Close();
                conObj.Dispose();
            }
        }
         */
 
        public string updatePassword(string username, string cur_pass, string new_pass, string IP)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_change_password", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_oldpassword", OracleType.VarChar, 100);
                Cmd.Parameters["in_oldpassword"].Value = cur_pass;

                Cmd.Parameters.Add("in_newpassword", OracleType.VarChar, 100);
                Cmd.Parameters["in_newpassword"].Value = new_pass;

                Cmd.Parameters.Add("in_IP", OracleType.VarChar, 100);
                Cmd.Parameters["in_IP"].Value = IP;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                return exeCode + "$" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHome-updatePassword");
                return "-1000$ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string MIASTATUSINS(string username, string status, string IP)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_miadetails_ins", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_status", OracleType.VarChar, 100);
                Cmd.Parameters["in_status"].Value = status;

                Cmd.Parameters.Add("in_IP", OracleType.VarChar, 100);
                Cmd.Parameters["in_IP"].Value = IP;

                Cmd.Parameters.Add("out_errmsg", OracleType.VarChar, 500);
                Cmd.Parameters["out_errmsg"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string exeData = Convert.ToString(Cmd.Parameters["out_errmsg"].Value);
                return exeCode + "$" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHome-updatePassword");
                return "-1000$ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        //---- Added by RP on 12.12.2017
        public void GetInventryBalance(string UserName, out string Balance,out string InBalance)
        {
            Balance = "";
            InBalance = "";
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str = " select nvl(num_user_curcrlimit,0) Bal, nvl(num_lcomst_invbal,0) invbal from aoup_user_def where var_user_username='" + UserName + "' ";
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Balance = dr["bal"].ToString();
                    InBalance = dr["invbal"].ToString();
                }
                conObj.Close();
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(UserName, ex.Message.ToString(), "Cls_Data_TransHome-GetInventryBalance");

            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        //--------------
    
    }
}
