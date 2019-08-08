using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Transaction
{
   public class Cls_Selfcare
    {
        // Added by Lokesh on 13-May-2019 -- Add Selfcare
        public string addEnableSelfcare(string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_Lco_selfcare_ins", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_lco_code", OracleType.VarChar, 100);
                Cmd.Parameters["in_lco_code"].Value = username;

                Cmd.Parameters.Add("out_errortext", OracleType.VarChar, 500);
                Cmd.Parameters["out_errortext"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string out_errortext = Convert.ToString(Cmd.Parameters["out_errortext"].Value);
                return exeCode + "$" + out_errortext;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Enable -Selfcare-Add");
                return "-1000$ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

    }
}
