using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.Data.OracleClient;
using System.Collections;
using System.Configuration;

namespace PrjUpassDAL.Master
{
    public class Cls_Data_MstLcoAutoEcsConfiguration
    {
        public DataTable getLCOData(string whereClauseStr, string username)
        {
            try
            {
                //DataTable dtLCO = new DataTable("LCO");
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "SELECT a.var_lcomst_code,a.var_lcomst_name,a.var_lcomst_firstname, ";
                StrQry += " a.var_lcomst_middlename,a.var_lcomst_lastname,a.num_lcomst_directno,";
                StrQry += " a.num_lcomst_jvno,a.var_lcomst_ecsstatus ";
                StrQry += " from aoup_lcopre_lco_det a ";
                StrQry += " where " + whereClauseStr;

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCOPreUDefine.cs-getUserData");
                return null;
            }
        }

        public string setLCOData(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_Lco_Ecs_AutoConfig", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 10);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 300);
                Cmd.Parameters["in_lcocode"].Value = ht["Lcocode"];

                Cmd.Parameters.Add("in_lconame", OracleType.VarChar, 300);
                Cmd.Parameters["in_lconame"].Value = ht["Lconame"];

                Cmd.Parameters.Add("in_Ecs_flag", OracleType.VarChar, 100);
                Cmd.Parameters["in_Ecs_flag"].Value = ht["ecssattus"];

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;

                if (exeResult == 9999)
                {
                    Str = " Auto Ecs Configuration updated successfully...";
                }
                else
                {
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstHwaymsgBrdr-SetBrodcastermsg");
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
