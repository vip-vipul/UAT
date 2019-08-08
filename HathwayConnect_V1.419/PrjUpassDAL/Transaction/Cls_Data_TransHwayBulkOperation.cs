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
    public class Cls_Data_TransHwayBulkOperation
    {
        public string bulkUploadTemp(string username, string upload_id)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                //OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_upload_temp2", ConObj);

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_upload_check", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_useruniqueid", OracleType.VarChar, 100);
                Cmd.Parameters["in_useruniqueid"].Value = upload_id;

                Cmd.Parameters.Add("out_data", OracleType.LongVarChar, 400000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "#" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation-bulkUploadTemp");
                return "-300#"+ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
        public string bulkprocessupdt(string username, string upload_id)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_process_updt", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_useruniqueid", OracleType.VarChar, 100);
                Cmd.Parameters["in_useruniqueid"].Value = upload_id;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 4000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "#" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation-bulkValidate");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
        public DataTable getUploadedData(string username, string upload_id)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry = "";
                StrQry += "SELECT a.var_lcopre_bulk_custid, a.var_lcopre_bulk_vcid, a.var_lcopre_bulk_lcocode, a.var_lcopre_bulk_planname, a.var_lcopre_bulk_action, ";
                StrQry += " a.var_lcopre_bulk_insby, a.var_lcopre_bulk_useruniqueid, a.var_lcopre_bulk_date, a.var_lcopre_bulk_brmpoid,a.var_lcopre_bulk_ecs_flag ";
                StrQry += " FROM aoup_lcopre_bulk_temp a";
                StrQry += " where a.var_lcopre_bulk_useruniqueid='" + upload_id + "'";
                StrQry += " and TRUNC (TO_DATE (a.var_lcopre_bulk_date,'DD-Mon-YYYY hh24:mi:ss')) = TRUNC (SYSDATE)";
                StrQry += " and var_lcopre_bulk_process_flag='N' ";
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation-getUploadedData");
                return null;
            }
        }

        public string bulkValidate(string username, string upload_id, string cust_data, string service_data, string action, string plan_name, string lco_code) {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_validation", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_useruniqueid", OracleType.VarChar, 100);
                Cmd.Parameters["in_useruniqueid"].Value = upload_id;

                Cmd.Parameters.Add("in_custstr", OracleType.VarChar, 4000);
                Cmd.Parameters["in_custstr"].Value = cust_data;

                Cmd.Parameters.Add("in_planstr", OracleType.VarChar, 5000);
                Cmd.Parameters["in_planstr"].Value = service_data;

                Cmd.Parameters.Add("in_action", OracleType.VarChar, 1);
                Cmd.Parameters["in_action"].Value = action;

                Cmd.Parameters.Add("in_planname", OracleType.VarChar, 1000);
                Cmd.Parameters["in_planname"].Value = plan_name;

                //Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 1000);
                //Cmd.Parameters["in_lcocode"].Value = lco_code;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 4000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "#" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation-bulkValidate");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string validateBulkTrans(Hashtable htTransData)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_provi_valid", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = htTransData["username"];

                Cmd.Parameters.Add("in_lcoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcoid"].Value = htTransData["lcoid"];

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocode"].Value = htTransData["lco_code"];

                Cmd.Parameters.Add("in_custid", OracleType.VarChar, 100);
                Cmd.Parameters["in_custid"].Value = htTransData["custid"];

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar, 100);
                Cmd.Parameters["in_vcid"].Value = htTransData["vcid"];

                Cmd.Parameters.Add("in_custname", OracleType.VarChar, 1000);
                Cmd.Parameters["in_custname"].Value = htTransData["custname"];

                Cmd.Parameters.Add("in_planid", OracleType.VarChar, 100);
                Cmd.Parameters["in_planid"].Value = htTransData["planid"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar, 10);
                Cmd.Parameters["in_flag"].Value = htTransData["flag"];

                Cmd.Parameters.Add("in_expdate", OracleType.VarChar, 50);
                Cmd.Parameters["in_expdate"].Value = htTransData["expdate"];

                Cmd.Parameters.Add("in_actidate", OracleType.VarChar, 50);
                Cmd.Parameters["in_actidate"].Value = htTransData["actidate"];

                Cmd.Parameters.Add("in_request", OracleType.VarChar, 1000);
                Cmd.Parameters["in_request"].Value = htTransData["request"];

                Cmd.Parameters.Add("in_IP", OracleType.VarChar, 1000);
                Cmd.Parameters["in_IP"].Value = htTransData["IP"];

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 1000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "$" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(htTransData["username"].ToString(), ex.Message.ToString(), "Cl_Data_TransHwayBulkOperation-validateBulkTrans");
                return "-300$ex_occured";

            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string bulkTransIns(Hashtable ht)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_provision_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = ht["username"];

                Cmd.Parameters.Add("in_lcoid", OracleType.VarChar);
                Cmd.Parameters["in_lcoid"].Value = ht["lcoid"];

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocode"].Value = ht["lco_code"];

                Cmd.Parameters.Add("in_custid", OracleType.VarChar);
                Cmd.Parameters["in_custid"].Value = ht["custid"];

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar);
                Cmd.Parameters["in_vcid"].Value = ht["vcid"];

                Cmd.Parameters.Add("in_custname", OracleType.VarChar);
                Cmd.Parameters["in_custname"].Value = ht["custname"];

                Cmd.Parameters.Add("in_planid", OracleType.VarChar);
                Cmd.Parameters["in_planid"].Value = ht["planid"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar);
                Cmd.Parameters["in_flag"].Value = ht["flag"];

                Cmd.Parameters.Add("in_expdate", OracleType.VarChar);
                Cmd.Parameters["in_expdate"].Value = ht["expdate"];

                Cmd.Parameters.Add("in_actidate", OracleType.VarChar);
                Cmd.Parameters["in_actidate"].Value = ht["actidate"];

                Cmd.Parameters.Add("in_obrmsts", OracleType.VarChar);
                Cmd.Parameters["in_obrmsts"].Value = ht["obrmsts"];

                Cmd.Parameters.Add("in_reqid", OracleType.Number);
                Cmd.Parameters["in_reqid"].Value = ht["request_id"];

                Cmd.Parameters.Add("in_response", OracleType.VarChar);
                Cmd.Parameters["in_response"].Value = ht["response"];

                Cmd.Parameters.Add("in_address", OracleType.VarChar);
                Cmd.Parameters["in_address"].Value = ht["cust_addr"];

                Cmd.Parameters.Add("in_reasonid", OracleType.VarChar);
                Cmd.Parameters["in_reasonid"].Value = ht["reason_id"];

                Cmd.Parameters.Add("in_IP", OracleType.VarChar);
                Cmd.Parameters["in_IP"].Value = ht["IP"];

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
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cl_Data_TransHwayBulkOperation-bulkTransIns");
                return "-310$ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public void bulkStatusUpdt(Hashtable ht)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_status_updt", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = ht["username"];

                Cmd.Parameters.Add("in_useruniqueid", OracleType.VarChar);
                Cmd.Parameters["in_useruniqueid"].Value = ht["upload_id"];

                Cmd.Parameters.Add("in_custid", OracleType.VarChar);
                Cmd.Parameters["in_custid"].Value = ht["cust_no"];

                Cmd.Parameters.Add("in_errorcode", OracleType.Number);
                Cmd.Parameters["in_errorcode"].Value = ht["err_code"];

                Cmd.Parameters.Add("in_errormsg", OracleType.VarChar);
                Cmd.Parameters["in_errormsg"].Value = ht["err_msg"];

                Cmd.Parameters.Add("in_action", OracleType.VarChar);
                Cmd.Parameters["in_action"].Value = ht["action"];

                Cmd.Parameters.Add("in_planname", OracleType.VarChar);
                Cmd.Parameters["in_planname"].Value = ht["plan_name"];

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar);
                Cmd.Parameters["in_vcid"].Value = ht["vcid"];

                //Cmd.Parameters.Add("in_lcocode", OracleType.VarChar);
                //Cmd.Parameters["in_lcocode"].Value = ht["lco_code"];

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 4000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string ExeResult = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string ExeResultMsg = Convert.ToString(Cmd.Parameters["out_data"].Value);

                ConObj.Dispose();
                //return ExeResult + "$" + ExeResultMsg;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cl_Data_TranHwayBulkOperation-bulkStatusUpdt");
                //return "-310$ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string masterMovement(string username, string upload_id)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_upload", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_useruniqueid", OracleType.VarChar);
                Cmd.Parameters["in_useruniqueid"].Value = upload_id;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 4000);
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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cl_Data_TranHwayBulkOperation-masterMovement");
                return "-310$" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string ProvECS(Hashtable ht)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_ecs_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = ht["username"];

                Cmd.Parameters.Add("in_lcoid", OracleType.Number);
                Cmd.Parameters["in_lcoid"].Value = ht["lcoid"];

                Cmd.Parameters.Add("in_custid", OracleType.VarChar);
                Cmd.Parameters["in_custid"].Value = ht["custid"];

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar);
                Cmd.Parameters["in_vcid"].Value = ht["vcid"];

                Cmd.Parameters.Add("in_custname", OracleType.VarChar);
                Cmd.Parameters["in_custname"].Value = ht["custname"];

                Cmd.Parameters.Add("in_planid", OracleType.VarChar);
                Cmd.Parameters["in_planid"].Value = ht["planid"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar);
                Cmd.Parameters["in_flag"].Value = ht["flag"];

                Cmd.Parameters.Add("in_expdate", OracleType.VarChar);
                Cmd.Parameters["in_expdate"].Value = ht["expdate"];

                Cmd.Parameters.Add("in_actidate", OracleType.VarChar);
                Cmd.Parameters["in_actidate"].Value = ht["actidate"];

                Cmd.Parameters.Add("in_obrmsts", OracleType.VarChar);
                Cmd.Parameters["in_obrmsts"].Value = ht["obrmsts"];

                Cmd.Parameters.Add("in_address", OracleType.VarChar);
                Cmd.Parameters["in_address"].Value = ht["cust_addr"];

                Cmd.Parameters.Add("in_IP", OracleType.VarChar);
                Cmd.Parameters["in_IP"].Value = ht["IP"];

                Cmd.Parameters.Add("in_autorenew", OracleType.VarChar);
                Cmd.Parameters["in_autorenew"].Value = ht["autorenew"];

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
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation.cs-ProvECS");
                return "-310$ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string bulkUploadDuplicate(string username, string upload_id)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                //OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_upload_temp2", ConObj);
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_check_duplict", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_useruniqueid", OracleType.VarChar, 100);
                Cmd.Parameters["in_useruniqueid"].Value = upload_id;

                Cmd.Parameters.Add("out_data", OracleType.LongVarChar, 400000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "#" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation-bulkUploadTemp");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public void GetRequToday(string LCO_Code, out string RequToday)
        {
            RequToday = "";
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str = " select nvl(SUM(num_plan_lcoprice),0) TodayRequed from REPORTS.hwcas_brm_cust_master@CASLIVE_new a,view_lcopre_lcoplan_def b ";
                str += " where b.var_plan_name=a.productname and a.lco_code = b.var_lcomst_code";
                str += " and enddate = trunc(sysdate) and status = 'ACTIVE' and lco_code = '" + LCO_Code + "'";
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    RequToday = dr["TodayRequed"].ToString();
                }
                conObj.Close();
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(LCO_Code, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation.cs-GetRequToday");

            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        public void GetRequTomor(string LCO_Code, out string RequTomor)
        {
            RequTomor = "";
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str = " select nvl(SUM(num_plan_lcoprice),0) TomorRequed from REPORTS.hwcas_brm_cust_master@CASLIVE_new a,view_lcopre_lcoplan_def b ";
                str += " where b.var_plan_name=a.productname and a.lco_code = b.var_lcomst_code";
                str += " and enddate = trunc(sysdate + 1) and status = 'ACTIVE' and lco_code = '" + LCO_Code + "'";
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    RequTomor = dr["TomorRequed"].ToString();
                }
                conObj.Close();
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(LCO_Code, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation.cs-GetRequTomor");

            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public void GetAvalBalance(string LCO_Code, out string AvalBalance)
        {
            AvalBalance = "";
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str = "  select num_partled_bal from (";
                str += " select num_partled_bal from aoup_lcopre_lco_partyled_det,aoup_lcopre_lco_det where ";
                str += " num_partled_lcoid = num_lcomst_id and var_lcomst_code = '" + LCO_Code + "' order by dat_partled_date desc";
                str += " ) where rownum = 1";
                /*str = " select SUM(num_plan_lcoprice) TodayRequed from REPORTS.hwcas_brm_cust_master@CASLIVE a,view_lcopre_lcoplan_def b ";
                str += " where b.var_plan_name=a.productname and a.lco_code = b.var_lcomst_code";
                str += " and enddate = trunc(sysdate) and status = 'ACTIVE' and lco_code = " + LCO_Code + "'";*/
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    AvalBalance = dr["num_partled_bal"].ToString();
                }
                conObj.Close();
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(LCO_Code, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation.cs-GetAvalBalance");

            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string bulkUploadActTemp(string username, string upload_id)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                //OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_upload_temp2", ConObj);

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulkact_upd_check", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_useruniqueid", OracleType.VarChar, 100);
                Cmd.Parameters["in_useruniqueid"].Value = upload_id;

                Cmd.Parameters.Add("out_data", OracleType.LongVarChar, 400000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "#" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation-bulkUploadTemp");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string bulkUploadActTempRemove(string lblbulkid,string upload_id, string processdt,string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                //OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_upload_temp2", ConObj);

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulkact_upd_del", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_useruniqueid", OracleType.VarChar, 100);
                Cmd.Parameters["in_useruniqueid"].Value = upload_id;

                Cmd.Parameters.Add("in_processdt", OracleType.VarChar, 100);
                Cmd.Parameters["in_processdt"].Value = processdt;

                Cmd.Parameters.Add("in_bulkid", OracleType.VarChar, 100);
                Cmd.Parameters["in_bulkid"].Value = lblbulkid;

                Cmd.Parameters.Add("out_data", OracleType.LongVarChar, 400000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "#" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation-bulkUploadTemp");
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
