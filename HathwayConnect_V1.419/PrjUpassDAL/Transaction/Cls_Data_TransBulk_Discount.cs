using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Configuration;
using PrjUpassDAL.Helper;
using System.Collections;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_TransBulk_Discount
    {
        public string bulkUploadTemp(string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_discount_temp", ConObj);
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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransBulk_Discount-bulkUploadTemp");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string bulkUploadMst(string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_discount_mst", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("IN_UserName", OracleType.VarChar, 50);
                Cmd.Parameters["IN_UserName"].Value = username;

                Cmd.Parameters.Add("IN_Flag", OracleType.VarChar, 50);
                Cmd.Parameters["IN_Flag"].Value = "SP";

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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransBulk_Discount-bulkUploadMst");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }


        //--- Plan Share Procedure Calling here 
        public string bulkPlanUpload(string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_lcoshare_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode ;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransBulk_Discount-bulkUploadMst");
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
