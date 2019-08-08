using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Configuration;
using PrjUpassDAL.Helper;
using System.Collections;
using PrjUpassDAL.Transaction;
namespace PrjUpassDAL.Transaction
{
    public class Cls_DLL_Bulk_Cust_Ecaf
    {
        public string Bulk_Cust_Ecaf_temp(string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_ecs_temp", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("IN_UserName", OracleType.VarChar, 50);
                Cmd.Parameters["IN_UserName"].Value = username;

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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Business_TransBulk_AutoRenewal-bulkUploadTemp");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string Bulk_Cust_Ecaf_mst(string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_ecs_mst", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("IN_UserName", OracleType.VarChar, 50);
                Cmd.Parameters["IN_UserName"].Value = username;

                //Cmd.Parameters.Add("IN_Flag", OracleType.VarChar, 50);
                //Cmd.Parameters["IN_Flag"].Value = "SP";

                Cmd.Parameters.Add("OUT_ERRTEXT", OracleType.LongVarChar, 400000);
                Cmd.Parameters["OUT_ERRTEXT"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("OUT_ERRCODE", OracleType.Number);
                Cmd.Parameters["OUT_ERRCODE"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["OUT_ERRTEXT"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "#" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Business_TransBulk_AutoRenewal-bulkUploadMst");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }


        public string Bulk_Cust_Ecaf_inst(string username,string Uniquno)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_eacf_inst", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("IN_UserName", OracleType.VarChar, 50);
                Cmd.Parameters["IN_UserName"].Value = username;

                Cmd.Parameters.Add("IN_uniquno", OracleType.VarChar, 150);
                Cmd.Parameters["IN_uniquno"].Value = Uniquno;

                Cmd.Parameters.Add("OUT_ERRCODE", OracleType.Number);
                Cmd.Parameters["OUT_ERRCODE"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("OUT_ERRTEXT", OracleType.LongVarChar, 400000);
                Cmd.Parameters["OUT_ERRTEXT"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["OUT_ERRTEXT"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "#" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Business_TransBulk_AutoRenewal-Bulk_Cust_Ecaf_inst");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

    }
}
