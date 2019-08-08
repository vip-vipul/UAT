using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace PrjUpassDAL.Transaction
{
   public class Cls_Data_UserAccountMap
    {
       public string UserAccountMap(string username, string IPAdd, string LcoCode, string AccountNo, string ActiveFlag, string Type)
       {
           string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
           OracleConnection ConObj = new OracleConnection(ConStr);
           try
           {
               ConObj.Open();
               OracleCommand Cmd = new OracleCommand("AOUP_LCOPRE_USER_ACC_MAP_INST", ConObj);
               Cmd.CommandType = CommandType.StoredProcedure;

               Cmd.Parameters.Add("IN_UserName", OracleType.VarChar, 50);
               Cmd.Parameters["IN_UserName"].Value = username;

               Cmd.Parameters.Add("IN_IP", OracleType.VarChar, 50);
               Cmd.Parameters["IN_IP"].Value = IPAdd;
               if (LcoCode != "" && LcoCode != null)
               {
                   Cmd.Parameters.Add("in_LcoCode", OracleType.VarChar, 100);
                   Cmd.Parameters["in_LcoCode"].Value = LcoCode;
               }
               else
               {
                   Cmd.Parameters.Add("in_LcoCode", OracleType.VarChar, 100);
                   Cmd.Parameters["in_LcoCode"].Value = DBNull.Value;
               }

               if (AccountNo != "" && AccountNo != null)
               {
                   Cmd.Parameters.Add("in_AccountNo", OracleType.VarChar, 100);
                   Cmd.Parameters["in_AccountNo"].Value = AccountNo;
               }
               else
               {
                   Cmd.Parameters.Add("in_AccountNo", OracleType.VarChar, 100);
                   Cmd.Parameters["in_AccountNo"].Value = DBNull.Value;
               }

               if (ActiveFlag != "" && ActiveFlag != null)
               {
                   Cmd.Parameters.Add("in_ActiveFlag", OracleType.VarChar, 100);
                   Cmd.Parameters["in_ActiveFlag"].Value = ActiveFlag;
               }
               else
               {
                   Cmd.Parameters.Add("in_ActiveFlag", OracleType.VarChar, 100);
                   Cmd.Parameters["in_ActiveFlag"].Value = DBNull.Value;
               }

               if (Type != "" && Type != null)
               {
                   Cmd.Parameters.Add("in_Type", OracleType.VarChar, 100);
                   Cmd.Parameters["in_Type"].Value = Type;
               }
               else
               {
                   Cmd.Parameters.Add("in_Type", OracleType.VarChar, 100);
                   Cmd.Parameters["in_Type"].Value = DBNull.Value;
               }
               Cmd.Parameters.Add("OUT_ERRTEXT", OracleType.LongVarChar, 400000);
               Cmd.Parameters["OUT_ERRTEXT"].Direction = ParameterDirection.Output;

               Cmd.Parameters.Add("OUT_ERRCODE", OracleType.Number);
               Cmd.Parameters["OUT_ERRCODE"].Direction = ParameterDirection.Output;

               Cmd.ExecuteNonQuery();
               ConObj.Close();

               string exeData = Convert.ToString(Cmd.Parameters["OUT_ERRTEXT"].Value);
               string exeCode = Convert.ToString(Cmd.Parameters["OUT_ERRCODE"].Value);
               return exeCode + "#" + exeData;
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_UserAccountMap-UserAccountMap");
               return "-300#" + ex.Message.ToString();
           }
           finally
           {
               ConObj.Close();
               ConObj.Dispose();
           }
       }

       public DataTable userAccMap_Det(string strQurey, string username)
       {
           try
           {
               DataTable dt = new DataTable();
               Cls_Helper ObjHelper = new Cls_Helper();
               return ObjHelper.GetDataTable(strQurey);

           }
           catch (Exception ex)
           {
               
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_UserAccountMap-userAccMap_Det");
               return null;
           }
       }
    }
}
