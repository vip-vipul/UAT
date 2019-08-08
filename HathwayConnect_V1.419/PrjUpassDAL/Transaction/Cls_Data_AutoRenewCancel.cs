using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;

namespace PrjUpassDAL.Transaction
{
    
     public class Cls_Data_AutoRenewCancel
    {
         string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
        public string AutoRenewCancel(string username, Hashtable ht)
        { 
         OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_ecs_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = ht["username"];

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string ExeResult = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string ExeResultMsg = Convert.ToString(Cmd.Parameters["out_data"].Value);

                ConObj.Dispose();
                return ExeResult + "$" + ExeResultMsg;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-ProvECS");
                return "-310$ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
    }
}
