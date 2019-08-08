using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;

namespace PrjUpassDAL.Master
{
    public class Cls_Data_MstLCOPreUDefine
    {
        public DataTable getLCODataLco(string whereClauseStr, string username, string category)
        {
            try
            {
                //DataTable dtLCO = new DataTable("LCO");
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = " SELECT a.num_usermst_id, a.lcoid, a.var_usermst_username, ";
                StrQry += " a.var_usermst_name, a.var_usermst_firstname, ";
                StrQry += " a.var_usermst_middlename, a.var_usermst_lastname, ";
                StrQry += " a.var_usermst_address, a.var_usermst_code, a.var_usermst_brmpoid, ";
                StrQry += " a.num_usermst_stateid, a.num_usermst_cityid, a.var_usermst_email, ";
                StrQry += " a.num_usermst_mobileno, a.var_lcomst_companycode, ";
                StrQry += " a.var_usermst_accno, a.var_usermst_insby, a.var_usermst_insdt, ";
                StrQry += " a.var_usermst_flag, a.lconame, a.lcocode,a.jvno,a.directno ";
                StrQry += " FROM view_prelco_lco_uddet a ";
                StrQry += " where " + whereClauseStr;


                /*dtLCO = objHelper.GetDataTable(strLCOQry);
                DataSet dsLCOData = new DataSet();
                dsLCOData.Tables.Add(dtLCO);
                return dsLCOData;*/
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCOPreUDefine.cs-getUserData");
                return null;
            }
        }

        public DataTable getLCOData(string whereClauseStr, string username)
        {
            try
            {
                //DataTable dtLCO = new DataTable("LCO");
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "SELECT lconame, lcoid, lcocode " +
                                " FROM view_preLCO_lco_det " +
                " where " + whereClauseStr;

                /*dtLCO = objHelper.GetDataTable(strLCOQry);
                DataSet dsLCOData = new DataSet();
                dsLCOData.Tables.Add(dtLCO);
                return dsLCOData;*/
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCOPreUDefine.cs-getUserData");
                return null;
            }
        }

        public string setUserData(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_user_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_userid", OracleType.VarChar, 100);
                Cmd.Parameters["in_userid"].Value = ht["userid12"];

                Cmd.Parameters.Add("in_userowner", OracleType.VarChar, 100);
                Cmd.Parameters["in_userowner"].Value = ht["userowner"];

                Cmd.Parameters.Add("in_password1", OracleType.VarChar, 100);
                Cmd.Parameters["in_password1"].Value = "a";

                Cmd.Parameters.Add("in_password2", OracleType.VarChar, 100);
                Cmd.Parameters["in_password2"].Value = "a";

                Cmd.Parameters.Add("in_firstname", OracleType.VarChar, 100);
                Cmd.Parameters["in_firstname"].Value = ht["fname"];

                Cmd.Parameters.Add("in_middlename", OracleType.VarChar, 500);
                Cmd.Parameters["in_middlename"].Value = ht["mname"];

                Cmd.Parameters.Add("in_lastname", OracleType.VarChar, 100);
                Cmd.Parameters["in_lastname"].Value = ht["lname"];

                Cmd.Parameters.Add("in_address", OracleType.VarChar, 100);
                Cmd.Parameters["in_address"].Value = ht["addr"];

                Cmd.Parameters.Add("in_pincode", OracleType.VarChar, 100);
                Cmd.Parameters["in_pincode"].Value = ht["pincode"];

                Cmd.Parameters.Add("in_brmpoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_brmpoid"].Value = ht["brmpoid"];

                Cmd.Parameters.Add("in_stateid", OracleType.Number);
                Cmd.Parameters["in_stateid"].Value = Convert.ToInt64(ht["state"]);

                Cmd.Parameters.Add("in_cityid", OracleType.Number);
                Cmd.Parameters["in_cityid"].Value = Convert.ToInt64(ht["city"]);

                Cmd.Parameters.Add("in_email", OracleType.VarChar, 100);
                Cmd.Parameters["in_email"].Value = ht["email"];

                Cmd.Parameters.Add("in_mobileno", OracleType.Number);
                Cmd.Parameters["in_mobileno"].Value = ht["mobileno"];

                Cmd.Parameters.Add("in_compcode", OracleType.VarChar, 100);
                Cmd.Parameters["in_compcode"].Value = ht["compcode"];

                Cmd.Parameters.Add("in_accno", OracleType.VarChar, 100);
                Cmd.Parameters["in_accno"].Value = ht["accno"];

                Cmd.Parameters.Add("in_lcoid", OracleType.Number);
                Cmd.Parameters["in_lcoid"].Value = ht["lcoid2"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar, 10);
                Cmd.Parameters["in_flag"].Value = ht["userlevel"];

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
                    if (Convert.ToInt32(ht["flag"]) == 0)
                    {
                        Str = " USER registered successfully...";
                    }
                    else
                    {
                        Str = " USER updated successfully...";
                    }
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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCOPreUDefeine.cs-setUserData");
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
