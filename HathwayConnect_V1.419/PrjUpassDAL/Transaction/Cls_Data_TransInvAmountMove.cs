using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;
using PrjUpassDAL.Helper;
namespace PrjUpassDAL.Transaction
{
   public class Cls_Data_TransInvAmountMove
    {
       public string UpdateAmount(string sp, Hashtable HT)
       {
           string result = "";
           try
           {
               string strcon = ConfigurationSettings.AppSettings["ConString"].ToString();
               OracleConnection con = new OracleConnection(strcon);
               OracleCommand cmd = new OracleCommand(sp, con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add("in_username", OracleType.VarChar);
               cmd.Parameters.Add("in_lcocode", OracleType.VarChar);
               cmd.Parameters.Add("in_Amount", OracleType.Number);
               cmd.Parameters.Add("in_reason", OracleType.VarChar);
               cmd.Parameters.Add("out_errcode", OracleType.Number, 100);
               cmd.Parameters["in_username"].Value = HT["LCOcode"].ToString();
               cmd.Parameters["in_lcocode"].Value = HT["LCOcode"].ToString();
               cmd.Parameters["in_Amount"].Value = Convert.ToInt32( HT["Amount"]);
               cmd.Parameters["in_reason"].Value = HT["Remark"].ToString();
               cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;
               con.Open();
               cmd.ExecuteNonQuery();
               int exeResult = Convert.ToInt32(cmd.Parameters["out_errcode"].Value);
               con.Dispose();
               return result = Convert.ToString(exeResult);
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(HT["LCOcode"].ToString(), ex.Message.ToString(), "Cls_Data_TransInvAmountMove.cs");
                return "ex_occured";
           }
       }
    }
}
