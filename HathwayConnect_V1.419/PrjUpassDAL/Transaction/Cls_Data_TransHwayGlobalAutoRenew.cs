using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_TransHwayGlobalAutoRenew
    {
        public DataTable LcoPaymentDetails(string ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("select * from aoup_lco_lcocode_search where var_lcomst_code='" + ht + "' and rownum = 1", ConObj);
                OracleDataAdapter od = new OracleDataAdapter(Cmd);
                DataTable dt = new DataTable();
                od.Fill(dt);

                Cmd.ExecuteNonQuery();
                ConObj.Close();



                string Str;

                ConObj.Dispose();
                return dt;
            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable();
                return dt;
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string GARstatuschange(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lco_gauto_renewal", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = ht["User"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar, 100);
                Cmd.Parameters["in_flag"].Value = ht["flag"];

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 50);
                Cmd.Parameters["in_lcocode"].Value = ht["lcocode"];

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 2000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;


                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);

                string Str;


                if (exeResult == 9999)
                {
                    Str = "9999";
                    Str += ",Global Auto Renew Status Changed successfully...";

                }
                else
                {
                    Str = exeResult.ToString();
                    Str += "," + Convert.ToString(Cmd.Parameters["out_errortext"].Value);

                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["User"].ToString(), ex.Message.ToString(), "Cls_Data_TransHwayGlobalAutoRenew.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
    }
}
