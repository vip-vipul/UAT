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
    public class Cls_Data_MstLCOPreMSOUDefine
    {
        public DataTable getMSOData(string whereClauseStr, string username)
        {
            try
            {

                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = " SELECT a.num_comp_companyid, a.var_comp_companyname,a.var_comp_state,a.var_comp_city, a.num_comp_distid, ";
                StrQry += " a.var_comp_distname, a.num_comp_subdistid, ";
                StrQry += " a.var_comp_subdistname, a.var_comp_insby, a.dat_comp_insdt, ";
                StrQry += " a.var_comp_updby, a.dat_comp_upddt, a.num_comp_operid ";
                StrQry += " FROM view_lcopre_company_det a ";
                StrQry += " where " + whereClauseStr;


                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_MstLCOPreMSOUDefine.cs-getMSOData");
                return null;
            }
        }

        public string setUserData(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_msouser_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_companyname", OracleType.VarChar, 100);
                Cmd.Parameters["in_companyname"].Value = ht["companyname"];

                Cmd.Parameters.Add("in_loginid", OracleType.VarChar, 100);
                Cmd.Parameters["in_loginid"].Value = ht["loginid"];

                Cmd.Parameters.Add("in_firstname", OracleType.VarChar, 100);
                Cmd.Parameters["in_firstname"].Value = ht["fname"];

                Cmd.Parameters.Add("in_middlename", OracleType.VarChar, 500);
                Cmd.Parameters["in_middlename"].Value = ht["mname"];

                Cmd.Parameters.Add("in_lastname", OracleType.VarChar, 100);
                Cmd.Parameters["in_lastname"].Value = ht["lname"];

                Cmd.Parameters.Add("in_directno", OracleType.VarChar, 100);
                Cmd.Parameters["in_directno"].Value = ht["direct"];

                Cmd.Parameters.Add("in_jvno", OracleType.VarChar, 100);
                Cmd.Parameters["in_jvno"].Value = ht["jv"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar, 10);
                Cmd.Parameters["in_flag"].Value = ht["flag"];

                Cmd.Parameters.Add("in_brmpoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_brmpoid"].Value = ht["brmpoid"];

                Cmd.Parameters.Add("in_address", OracleType.VarChar, 100);
                Cmd.Parameters["in_address"].Value = ht["addr"];

                Cmd.Parameters.Add("in_stateid", OracleType.VarChar, 50);
                Cmd.Parameters["in_stateid"].Value = ht["state"];

                Cmd.Parameters.Add("in_cityid", OracleType.VarChar, 50);
                Cmd.Parameters["in_cityid"].Value = ht["city"];

                Cmd.Parameters.Add("in_pincode", OracleType.Number);
                Cmd.Parameters["in_pincode"].Value = Convert.ToInt64(ht["pincode"]);


                Cmd.Parameters.Add("in_mobno", OracleType.Number);
                Cmd.Parameters["in_mobno"].Value = Convert.ToInt64(ht["mobileno"]);


                Cmd.Parameters.Add("in_email", OracleType.VarChar, 100);
                Cmd.Parameters["in_email"].Value = ht["email"];


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

                    Str = " USER registered successfully...";

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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_MstLCOPreMSOUDefine.cs-setUserData");
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
