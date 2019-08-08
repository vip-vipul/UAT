using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_TxnAssignPlan
    {
        string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();



        public string getDistBalance(string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                //fetching available credits
                string available_credits = "";
                string strQry2 = "SELECT a.num_user_curcrlimit FROM aoup_user_def a WHERE a.var_user_username = '" + username + "'";
                OracleCommand Cmd2 = new OracleCommand(strQry2, conObj);
                conObj.Open();
                OracleDataReader reader = Cmd2.ExecuteReader();
                while (reader.Read())
                {
                    available_credits = reader["num_user_curcrlimit"].ToString();
                }
                if (available_credits == null || available_credits == "")
                {
                    available_credits = "0";
                }
                return available_credits;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TxnAssignPlan-getDistBalance");
                return "ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string getDistData(string username, string oper_id)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                //fetching name
                string strQry = "SELECT a.var_oper_opername FROM aoup_operator_def a WHERE a.num_oper_id = " + oper_id;
                string oper_name = "";
                string available_credits = "";
                OracleCommand Cmd = new OracleCommand(strQry, conObj);
                conObj.Open();
                OracleDataReader reader = Cmd.ExecuteReader();
                while (reader.Read())
                {
                    oper_name = reader["var_oper_opername"].ToString();
                }
                available_credits = getDistBalance(username);
                if (available_credits == "ex_occured")
                {
                    available_credits = "0";
                }
                return oper_name + "~" + available_credits + "~" + username;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-getDistData");
                return "ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public DataTable GetPackagedef(string username)
        {
            Cls_Helper objHelper = new Cls_Helper();
            try
            {
                string strQry = "SELECT a.plan_id, a.plan_name, a.plan_type, a.plan_poid, a.deal_poid, " +
                                " a.product_poid, a.cust_price, a.lco_price, a.payterm, a.cityid, a.city_name, " +
                                " a.company_code, a.insby, a.insdt " +
                                " FROM view_lcopre_plan_fetch a";
                return objHelper.GetDataTable(strQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-GetPackagedef");
                return null;
            }
        }

        public string ValidateProvTrans(Hashtable htTransData)
        {
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                // OracleCommand Cmd = new OracleCommand("paw_lcopre_provi_valid_emp", ConObj);
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_provi_valid_emp", ConObj);//
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = htTransData["username"];

                Cmd.Parameters.Add("in_lcoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcoid"].Value = htTransData["lcoid"];

                Cmd.Parameters.Add("in_custid", OracleType.VarChar, 100);
                Cmd.Parameters["in_custid"].Value = htTransData["custid"];

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar, 100);
                Cmd.Parameters["in_vcid"].Value = htTransData["vcid"];

                if (htTransData["STBNO"] != null)
                {
                    Cmd.Parameters.Add("in_stbno", OracleType.VarChar, 1000);
                    Cmd.Parameters["in_stbno"].Value = htTransData["STBNO"];

                }
                else
                {
                    Cmd.Parameters.Add("in_stbno", OracleType.VarChar, 1000);
                    Cmd.Parameters["in_stbno"].Value =DBNull.Value;

                }
                
                Cmd.Parameters.Add("in_custname", OracleType.VarChar, 100);
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

                Cmd.Parameters.Add("in_MainTV_Status", OracleType.VarChar, 1000);
                Cmd.Parameters["in_MainTV_Status"].Value = htTransData["MainTVStatus"];

                Cmd.Parameters.Add("in_TV_Type", OracleType.VarChar, 1000);
                Cmd.Parameters["in_TV_Type"].Value = htTransData["TVType"];

                Cmd.Parameters.Add("in_foc_count", OracleType.Number);
                Cmd.Parameters["in_foc_count"].Value = Convert.ToInt32(htTransData["FOCCount"]);

               

                Cmd.Parameters.Add("in_Device_Type", OracleType.VarChar, 1000);
                Cmd.Parameters["in_Device_Type"].Value = htTransData["DeviceType"];

                Cmd.Parameters.Add("in_Basic_poid", OracleType.VarChar, 1000);
                Cmd.Parameters["in_Basic_poid"].Value = htTransData["BasicPoid"];

                if (htTransData["Speacial"] != null)
                {
                    Cmd.Parameters.Add("in_Special", OracleType.VarChar, 1000);
                    Cmd.Parameters["in_Special"].Value = htTransData["Speacial"];
                }

                if (htTransData["Foc_Changeflag"] != null)
                {
                    Cmd.Parameters.Add("in_Foc_Changeflag", OracleType.VarChar);
                    Cmd.Parameters["in_Foc_Changeflag"].Value = htTransData["Foc_Changeflag"];
                }
                else
                {
                    Cmd.Parameters.Add("in_Foc_Changeflag", OracleType.VarChar);
                    Cmd.Parameters["in_Foc_Changeflag"].Value = "";
                }

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
                objSecurity.InsertIntoDb(htTransData["username"].ToString(), ex.Message.ToString(), "Cl_Data_TxnAssignPlan-ValidateProvTrans");
                return "-300$ex_occured";

            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string ValidateProvTransFoc2(Hashtable htTransData)
        {
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_provi_foc2_valid", ConObj);
                //OracleCommand Cmd = new OracleCommand("paw_lcopre_provi_foc2_valid", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = htTransData["username"];

                Cmd.Parameters.Add("in_lcoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcoid"].Value = htTransData["lcoid"];

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocode"].Value = htTransData["lcoid"];

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


                //added by pankaj on 25-jun-2016 for validation

                Cmd.Parameters.Add("in_MainTV_Status", OracleType.VarChar, 1000);
                Cmd.Parameters["in_MainTV_Status"].Value = htTransData["MainTVStatus"];

                Cmd.Parameters.Add("in_TV_Type", OracleType.VarChar, 1000);
                Cmd.Parameters["in_TV_Type"].Value = htTransData["TVType"];

                Cmd.Parameters.Add("in_foc_count", OracleType.Int32);
                Cmd.Parameters["in_foc_count"].Value = Convert.ToInt32(htTransData["FOCCount"]);

                Cmd.Parameters.Add("in_Device_Type", OracleType.VarChar, 10);
                Cmd.Parameters["in_Device_Type"].Value = htTransData["DeviceType"];

                Cmd.Parameters.Add("in_Basic_poid", OracleType.VarChar, 1000);
                Cmd.Parameters["in_Basic_poid"].Value = htTransData["BasicPoid"];

                Cmd.Parameters.Add("in_Foc_Language", OracleType.VarChar, 1000);
                Cmd.Parameters["in_Foc_Language"].Value = htTransData["Foc_Language"];

                Cmd.Parameters.Add("in_mrp", OracleType.Int32);
                Cmd.Parameters["in_mrp"].Value = htTransData["mrp"];

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 1000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Int32);
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
                objSecurity.InsertIntoDb(htTransData["username"].ToString(), ex.Message.ToString(), "Cl_Data_TxnAssignPlan-ValidateProvTrans");
                return "-300$ex_occured";

            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string ProvTransResEcaf(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_provision_ins_ecaf", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = ht["username"];

                Cmd.Parameters.Add("in_lcoid", OracleType.Number);
                Cmd.Parameters["in_lcoid"].Value = ht["lcoid"];

                Cmd.Parameters.Add("in_custid", OracleType.VarChar);
                Cmd.Parameters["in_custid"].Value = ht["custid"];

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar);
                Cmd.Parameters["in_vcid"].Value = ht["vcid"];

                Cmd.Parameters.Add("in_stbno", OracleType.VarChar);
                Cmd.Parameters["in_stbno"].Value = ht["STBNO"];

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

                Cmd.Parameters.Add("in_reqid", OracleType.VarChar);
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
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-ProvTransRes");
                return "-310$ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        //public string ValidateProvTrans(Hashtable htTransData)
        //{
        //    OracleConnection ConObj = new OracleConnection(ConStr);
        //    try
        //    {
        //        ConObj.Open();
        //        OracleCommand Cmd = new OracleCommand("aoup_lcopre_provi_valid_emp", ConObj);
        //        Cmd.CommandType = CommandType.StoredProcedure;

        //        Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
        //        Cmd.Parameters["in_username"].Value = htTransData["username"];

        //        Cmd.Parameters.Add("in_lcoid", OracleType.VarChar, 100);
        //        Cmd.Parameters["in_lcoid"].Value = htTransData["lcoid"];

        //        Cmd.Parameters.Add("in_custid", OracleType.VarChar, 100);
        //        Cmd.Parameters["in_custid"].Value = htTransData["custid"];

        //        Cmd.Parameters.Add("in_vcid", OracleType.VarChar, 100);
        //        Cmd.Parameters["in_vcid"].Value = htTransData["vcid"];

        //        Cmd.Parameters.Add("in_custname", OracleType.VarChar, 100);
        //        Cmd.Parameters["in_custname"].Value = htTransData["custname"];

        //        Cmd.Parameters.Add("in_planid", OracleType.VarChar, 100);
        //        Cmd.Parameters["in_planid"].Value = htTransData["planid"];

        //        Cmd.Parameters.Add("in_flag", OracleType.VarChar, 10);
        //        Cmd.Parameters["in_flag"].Value = htTransData["flag"];

        //        Cmd.Parameters.Add("in_expdate", OracleType.VarChar, 50);
        //        Cmd.Parameters["in_expdate"].Value = htTransData["expdate"];

        //        Cmd.Parameters.Add("in_actidate", OracleType.VarChar, 50);
        //        Cmd.Parameters["in_actidate"].Value = htTransData["actidate"];

        //        Cmd.Parameters.Add("in_request", OracleType.VarChar, 1000);
        //        Cmd.Parameters["in_request"].Value = htTransData["request"];

        //        Cmd.Parameters.Add("in_IP", OracleType.VarChar, 1000);
        //        Cmd.Parameters["in_IP"].Value = htTransData["IP"];

        //        Cmd.Parameters.Add("out_data", OracleType.VarChar, 1000);
        //        Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

        //        Cmd.Parameters.Add("out_errcode", OracleType.Number);
        //        Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

        //        Cmd.ExecuteNonQuery();
        //        ConObj.Close();

        //        string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
        //        string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
        //        return exeCode + "$" + exeData;
        //    }
        //    catch (Exception ex)
        //    {
        //        Cls_Security objSecurity = new Cls_Security();
        //        objSecurity.InsertIntoDb(htTransData["username"].ToString(), ex.Message.ToString(), "Cl_Data_TxnAssignPlan-ValidateProvTrans");
        //        return "-300$ex_occured";

        //    }
        //    finally
        //    {
        //        ConObj.Close();
        //        ConObj.Dispose();
        //    }
        //}

        public string ProvTransRes(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_provision_ins_emp", ConObj);//need to remove
               // OracleCommand Cmd = new OracleCommand("paw_lcopre_provision_ins_emp", ConObj);
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

                Cmd.Parameters.Add("in_reqid", OracleType.VarChar);
                Cmd.Parameters["in_reqid"].Value = ht["request_id"];

                Cmd.Parameters.Add("in_response", OracleType.VarChar);
                Cmd.Parameters["in_response"].Value = ht["response"];

                Cmd.Parameters.Add("in_address", OracleType.VarChar);
                Cmd.Parameters["in_address"].Value = ht["cust_addr"];

                Cmd.Parameters.Add("in_reasonid", OracleType.VarChar);
                Cmd.Parameters["in_reasonid"].Value = ht["reason_id"];

                Cmd.Parameters.Add("in_IP", OracleType.VarChar);
                Cmd.Parameters["in_IP"].Value = ht["IP"];

                if (ht["flag"].ToString() == "CH")
                {
                    Cmd.Parameters.Add("in_newplanid", OracleType.VarChar);
                    Cmd.Parameters["in_newplanid"].Value = ht["Newplanid"];

                }
                else
                {
                    Cmd.Parameters.Add("in_newplanid", OracleType.VarChar);
                    Cmd.Parameters["in_newplanid"].Value = DBNull.Value;
                }

                Cmd.Parameters.Add("in_newplanid", OracleType.VarChar);
                Cmd.Parameters["in_newplanid"].Value = DBNull.Value;
                Cmd.Parameters.Add("in_basicpoid", OracleType.VarChar);
                Cmd.Parameters["in_basicpoid"].Value = ht["BasicPoid"].ToString().Replace("'", "");

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
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-ProvTransRes");
                return "-310$ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string ProvECS(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_ecs_ins_new", ConObj);
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

                Cmd.Parameters.Add("in_Status", OracleType.VarChar);
                Cmd.Parameters["in_Status"].Value = ht["Status"];

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
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-ProvECS");
                return "-310$ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string ProvECSSingle(Hashtable ht)
        {
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
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-ProvECS");
                return "-310$ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string serviceStatusUpdateDAL(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_service_status_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = ht["username"];

                Cmd.Parameters.Add("in_stbno", OracleType.VarChar, 50);
                Cmd.Parameters["in_stbno"].Value = ht["stb_no"];

                Cmd.Parameters.Add("in_custno", OracleType.VarChar, 50);
                Cmd.Parameters["in_custno"].Value = ht["cust_no"];

                Cmd.Parameters.Add("in_custadd", OracleType.VarChar, 50);
                Cmd.Parameters["in_custadd"].Value = ht["cust_addr"];

                Cmd.Parameters.Add("in_accountpoid", OracleType.VarChar, 50);
                Cmd.Parameters["in_accountpoid"].Value = ht["account_poid"];

                Cmd.Parameters.Add("in_servicepoid", OracleType.VarChar, 50);
                Cmd.Parameters["in_servicepoid"].Value = ht["service_poid"];

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar, 50);
                Cmd.Parameters["in_vcid"].Value = ht["vc_id"];

                Cmd.Parameters.Add("in_status", OracleType.VarChar, 50);
                Cmd.Parameters["in_status"].Value = ht["status"];

                Cmd.Parameters.Add("in_orderid", OracleType.VarChar, 50);
                Cmd.Parameters["in_orderid"].Value = ht["orderid"];

                Cmd.Parameters.Add("in_reason_id", OracleType.Number);
                Cmd.Parameters["in_reason_id"].Value = ht["reason_id"];

                Cmd.Parameters.Add("in_IP", OracleType.VarChar, 50);
                Cmd.Parameters["in_IP"].Value = ht["IP"];

                Cmd.Parameters.Add("in_Type", OracleType.VarChar);
                Cmd.Parameters["in_Type"].Value = DBNull.Value;

                Cmd.Parameters.Add("in_reason", OracleType.VarChar);
                Cmd.Parameters["in_reason"].Value = DBNull.Value;

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
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Data_TxnAssignPlan-serviceStatusUpdate");
                return "-300$ex_occured";

            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public void GetuserCity(string username, out String cityid, out String dasarea, out String operid)
        {
            OracleConnection conObj = new OracleConnection(ConStr);
            OracleDataReader dr = null;
            cityid = "";
            dasarea = "";
            operid = "";
            try
            {

                string strQry = "select b.num_usermst_cityid cityid,a.var_lcomst_dasarea dasarea,a.num_lcomst_operid operid from aoup_lcopre_lco_det a,aoup_lcopre_user_det b ";
                strQry += " where a.num_lcomst_operid=b.num_usermst_operid ";
                strQry += " and b.var_usermst_username='" + username + "'";
                OracleCommand Cmd = new OracleCommand(strQry, conObj);
                conObj.Open();

                dr = Cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cityid = dr["cityid"].ToString();
                    dasarea = dr["dasarea"].ToString();
                    operid = dr["operid"].ToString();
                }

                //return cityid;//objHelper.GetDataTable(strQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cl_Data_TxnAssignPlan-GetuserCity");
                //return null;
            }
            finally
            {
                dr.Dispose();
                conObj.Close();
                conObj.Dispose();

            }
        }

        //public void GetuserCity(string username, out String cityid, out String dasarea)
        //{
        //    OracleConnection conObj = new OracleConnection(ConStr);
        //    OracleDataReader dr = null;
        //    cityid = "";
        //    dasarea = "";
        //    try
        //    {

        //        string strQry = "select b.num_usermst_cityid cityid,a.var_lcomst_dasarea dasarea from aoup_lcopre_lco_det a,aoup_lcopre_user_det b ";
        //        strQry += " where a.num_lcomst_operid=b.num_usermst_operid ";
        //        strQry += " and b.var_usermst_username='" + username + "'";

        //        OracleCommand Cmd = new OracleCommand(strQry, conObj);
        //        conObj.Open();

        //        dr = Cmd.ExecuteReader();
        //        dr.Read();
        //        if (dr.HasRows)
        //        {
        //            cityid = dr["cityid"].ToString();
        //            dasarea = dr["dasarea"].ToString();
        //        }

        //        //return cityid;//objHelper.GetDataTable(strQry);
        //    }
        //    catch (Exception ex)
        //    {
        //        Cls_Security objSecurity = new Cls_Security();
        //        objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cl_Data_TxnAssignPlan-GetuserCity");
        //        //return null;
        //    }
        //    finally
        //    {
        //        dr.Dispose();
        //        conObj.Close();
        //        conObj.Dispose();

        //    }
        //}

        public string GetCityFromBrmPoid(string username, string lcobrmpoid)
        {
            lcobrmpoid = lcobrmpoid.Trim().Split(' ')[2]; //  0.0.0.1 /account 34904356 7
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string cityid = "";
                string strQry = "SELECT a.num_lcomst_cityid FROM aoup_lcopre_lco_det a WHERE trim(a.var_lcomst_brmpoid) = trim('" + lcobrmpoid + "')";

                OracleCommand Cmd = new OracleCommand(strQry, conObj);
                conObj.Open();
                cityid = Convert.ToString(Cmd.ExecuteOracleScalar());
                return cityid;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cl_Data_TxnAssignPlan-GetCityFromBrmPoid");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        //-------------------------------------------------------------------------------
        public DataTable getPlanData(string username, List<string> plan_ids)
        {
            Cls_Helper objHelper = new Cls_Helper();
            try
            {
                string strQry = "SELECT a.num_hway_package_id, a.var_hway_package_code," +
                            " a.var_hway_package_name, a.num_hway_package_distprice," +
                            " a.num_hway_package_custprice, a.var_hway_package_insby," +
                            " a.dat_hway_package_insdt,a.var_hway_plan_poid " +
                            " FROM aoup_hway_package_def a" +
                            " WHERE ROWNUM <=3";
                if (plan_ids != null)
                {
                    string id_str = String.Join(",", plan_ids.ToArray());
                    id_str = id_str.Replace(",", "','");
                    strQry = " SELECT a.num_hway_package_id, a.var_hway_package_code," +
                            " a.var_hway_package_name, a.num_hway_package_distprice," +
                            " a.num_hway_package_custprice, a.var_hway_package_insby," +
                            " a.dat_hway_package_insdt,a.var_hway_plan_poid " +
                            " FROM aoup_hway_package_def a" +
                            " WHERE a.var_hway_plan_poid not in ('" + id_str + "') and ROWNUM <=3";
                }

                return objHelper.GetDataTable(strQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-getPlanData");
                return null;
            }
        }

        public DataTable getExistingPlanData(string username, List<string> plan_ids)
        {
            try
            {
                string id_str = String.Join(",", plan_ids.ToArray());
                if (id_str != "")
                {
                    id_str = id_str.Replace(",", "','");
                    string strQry = "SELECT a.num_hway_package_id, a.var_hway_package_code," +
                            "a.var_hway_package_name, a.num_hway_package_distprice," +
                            "a.num_hway_package_custprice, a.var_hway_package_insby," +
                            "a.dat_hway_package_insdt,a.var_hway_plan_poid " +
                            " FROM aoup_hway_package_def a" +
                            " WHERE a.var_hway_plan_poid in ('" + id_str + "') and ROWNUM <=1";
                    Cls_Helper objHelper = new Cls_Helper();
                    return objHelper.GetDataTable(strQry);
                }
                else
                {
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-getExistingPlanData");
                return null;
            }
        }

        public string SetPlans(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_hway_pkg_trans_ins_new", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = ht["username"];
                Cmd.Parameters.Add("in_accountno", OracleType.Number);
                Cmd.Parameters["in_accountno"].Value = ht["acc_no"];
                Cmd.Parameters.Add("in_custname", OracleType.VarChar);
                Cmd.Parameters["in_custname"].Value = ht["name"];
                Cmd.Parameters.Add("in_custadd", OracleType.VarChar);
                Cmd.Parameters["in_custadd"].Value = ht["address"];
                Cmd.Parameters.Add("in_planstr", OracleType.VarChar);
                Cmd.Parameters["in_planstr"].Value = ht["planstr"];

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;


                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int ExeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string ExeResultMsg;
                if (ExeResult == 9999)
                {
                    ExeResultMsg = "Plans added successfully";
                }
                else
                {
                    ExeResultMsg = Cmd.Parameters["out_data"].Value.ToString();
                }
                ConObj.Dispose();
                return ExeResultMsg;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-SetPlans");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string getServiceDataDL(string username, string city_id, string service_str, string AccNo)
        {
            service_str = service_str.Replace('|', '$');
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();
               // OracleCommand Cmd = new OracleCommand("aoup_lcopre_plandet_fetch", conObj);
                //OracleCommand Cmd = new OracleCommand("paw_lcopre_plandet_fetch_hway", conObj); //changed by pawan
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_plandet_fetch_hway", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_str", OracleType.VarChar);
                Cmd.Parameters["in_str"].Value = service_str;

                Cmd.Parameters.Add("in_city", OracleType.VarChar);
                Cmd.Parameters["in_city"].Value = city_id;

                Cmd.Parameters.Add("in_AccountNo", OracleType.VarChar);
                Cmd.Parameters["in_AccountNo"].Value = AccNo;

                Cmd.Parameters.Add("out_bsdata", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_bsdata"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_adondata", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_adondata"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_adondatareg", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_adondatareg"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_aldata", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_aldata"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_hwayspecial", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_hwayspecial"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_aldatafree", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_aldatafree"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string basicData = Convert.ToString(Cmd.Parameters["out_bsdata"].Value);
                string addonData = Convert.ToString(Cmd.Parameters["out_adondata"].Value);
                string out_adondatareg = Convert.ToString(Cmd.Parameters["out_adondatareg"].Value);
                string alacartaData = Convert.ToString(Cmd.Parameters["out_aldata"].Value);
                string hathwayspecial = Convert.ToString(Cmd.Parameters["out_hwayspecial"].Value);
                string alacartafreeData = Convert.ToString(Cmd.Parameters["out_aldatafree"].Value);

                return exeCode + "#" + basicData + "#" + addonData + "#" + out_adondatareg + "#" + alacartaData + "#" + hathwayspecial + "#" + alacartafreeData;

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TxnAssignPlan-getServiceData");
                return "-1000" + "#" + "ex_occured" + "#" + "ex_occured" + "#" + "ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        //--------------------------------------------------------------------------------
        public DataTable getProviReasons(string username, string type_flag)
        {
            try
            {
                string strQry = "SELECT a.num_reason_id, a.var_reason_name, a.var_reason_flag,a.var_reason_companycode" +
                                " FROM aoup_lcopre_provireason_det a" +
                                " WHERE a.var_reason_flag = '" + type_flag + "'";
                Cls_Helper objHelper = new Cls_Helper();
                return objHelper.GetDataTable(strQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TxnAssignPlan-getProviReasons");
                return new DataTable();
            }
        }
        
        public string getProviConfirm(Hashtable htData) {
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_provi_confirm_emp", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = htData["username"];

                Cmd.Parameters.Add("in_lcoid", OracleType.VarChar);
                Cmd.Parameters["in_lcoid"].Value = htData["lco_id"];

                Cmd.Parameters.Add("in_custid", OracleType.VarChar);
                Cmd.Parameters["in_custid"].Value = htData["cust_no"];

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar);
                Cmd.Parameters["in_vcid"].Value = htData["vc_id"];

                Cmd.Parameters.Add("in_custname", OracleType.VarChar);
                Cmd.Parameters["in_custname"].Value = htData["cust_name"];

                Cmd.Parameters.Add("in_planid", OracleType.VarChar);
                Cmd.Parameters["in_planid"].Value = htData["plan_id"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar);
                Cmd.Parameters["in_flag"].Value = htData["flag"];

                Cmd.Parameters.Add("in_expdate", OracleType.VarChar);
                Cmd.Parameters["in_expdate"].Value = htData["expiry"];

                Cmd.Parameters.Add("in_actidate", OracleType.VarChar);
                Cmd.Parameters["in_actidate"].Value = htData["activation"];

                Cmd.Parameters.Add("in_request", OracleType.VarChar);
                Cmd.Parameters["in_request"].Value = "";

                if (htData["existing_poids"] != null && Convert.ToString(htData["existing_poids"]) != "")
                {
                    Cmd.Parameters.Add("in_poid", OracleType.VarChar);
                    Cmd.Parameters["in_poid"].Value = htData["existing_poids"];
                }
                else {
                    Cmd.Parameters.Add("in_poid", OracleType.VarChar);
                    Cmd.Parameters["in_poid"].Value = "";
                }
                

                Cmd.Parameters.Add("in_IP", OracleType.VarChar);
                Cmd.Parameters["in_IP"].Value = htData["IP"];

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 2000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();
                
                string res_code = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string res_data = Convert.ToString(Cmd.Parameters["out_data"].Value);
                return res_code + "#" + res_data;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(htData["username"].ToString(), ex.Message.ToString(), "Data_TxnAssignPlan-getProviConfirm");
                return "-1000" + "#" + "Something went wrong...";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public DataTable getCustLastTrans(string username, string customer_no, string rownum) {
            try
            {
                string strQry = " SELECT a.transid, a.receiptno, a.custid, a.vc, a.custname, a.planid, " +
                                " a.plnname, a.addr, a.plntyp, a.amtdd, a.lcoprice, a.expdt, " +
                                " a.payterm, a.bal, a.companycode, a.flag, a.reason, a.insby, a.dt, " +
                                " a.tdt, a.uname, a.userowner, a.usrid, a.operid, a.lcocode, " +
                                " a.lconame, a.jvname, a.erplco_ac, a.distname, a.subdist, a.city, " +
                                " a.state, a.parentid, a.distid, a.hoid, a.custprice " +
                                " FROM view_lcopre_user_trans_det a " +
                                " WHERE a.custid = '"+ customer_no +"' " +
                                " and rownum <= " + rownum;
                Cls_Helper objHelper = new Cls_Helper();
                return objHelper.GetDataTable(strQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TxnAssignPlan-getCustLastTrans");
                return new DataTable();
            }
        }
        public string autorenewstatuslco(string username)
        {
            OracleConnection ConObj = new OracleConnection(ConStr);
            string status = "N";
            try
            {
                ConObj.Open();
                string strQry = " select flag from view_lcopre_autorenew_config where lcocode='" + username + "'";

                OracleCommand cmd = new OracleCommand(strQry, ConObj);
                OracleDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    status = dr["flag"].ToString();
                }
                return status;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TxnAssignPlan-autorenewstatuslco");
                return "N";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }



        public string autorenewstatus(string username, string vcid, string customerid, string planname)
        {
            string status = "";
            status = autorenewstatuslco(username);
            if (status == "Y")
            {
                //return status;
            }
            OracleConnection ConObj = new OracleConnection(ConStr);
            
            try
            {
                ConObj.Open();
                string strQry = " SELECT var_ecs_isactive ";
                strQry += " FROM aoup_lcopre_ecs_det a ";
                strQry += " where a.var_ecs_vcid='" + vcid + "'";
                strQry += " and a.var_ecs_custid ='" + customerid + "'";
                strQry += " and a.var_ecs_planid ='" + planname + "'";

                OracleCommand cmd = new OracleCommand(strQry, ConObj);
                OracleDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    status = dr["var_ecs_isactive"].ToString();
                }
                return status;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TxnAssignPlan-autorenewstatus");
                return "";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }

        }

        public string getRefund(Hashtable htData)
        {
            /*
            
             aoup_lcopre_refund_confirm (
   in_username IN VARCHAR2,
   in_lcoid IN VARCHAR2,
   in_planid IN VARCHAR2,   
   in_expdate IN VARCHAR2,
   out_data   OUT VARCHAR2,
   out_errcode   OUT NUMBER */
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_refund_confirm", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = htData["username"];

                Cmd.Parameters.Add("in_lcoid", OracleType.VarChar);
                Cmd.Parameters["in_lcoid"].Value = htData["lcoid"];

                Cmd.Parameters.Add("in_planid", OracleType.VarChar);
                Cmd.Parameters["in_planid"].Value = htData["planid"];

                Cmd.Parameters.Add("in_expdate", OracleType.VarChar);
                Cmd.Parameters["in_expdate"].Value = htData["expirydt"];

                Cmd.Parameters.Add("in_actdate", OracleType.VarChar);
                Cmd.Parameters["in_actdate"].Value = htData["actdt"];

                 Cmd.Parameters.Add("in_custid", OracleType.VarChar);
                Cmd.Parameters["in_custid"].Value = htData["in_custid"];

                 Cmd.Parameters.Add("in_vcid", OracleType.VarChar);
                Cmd.Parameters["in_vcid"].Value = htData["in_vcid"];

               Cmd.Parameters.Add("in_flag", OracleType.VarChar, 100);
                Cmd.Parameters["in_flag"].Value = htData["in_flag"];//need to remove
            
                Cmd.Parameters.Add("out_data", OracleType.VarChar, 2000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string res_code = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string res_data = Convert.ToString(Cmd.Parameters["out_data"].Value);
                return res_code + "#" + res_data;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(htData["username"].ToString(), ex.Message.ToString(), "Data_TxnAssignPlan-getRefund");
                return "-1000" + "#" + "Something went wrong...";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string getFreePlanDetails2(string username, string BasicPlanPoidId, string FocPackPoId, string lang)  //
        {

            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_foc2_plan_fetch", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;


                Cmd.Parameters.Add("in_basicplan_id", OracleType.VarChar);
                Cmd.Parameters["in_basicplan_id"].Value = BasicPlanPoidId;

                Cmd.Parameters.Add("in_focplan_id", OracleType.VarChar);
                Cmd.Parameters["in_focplan_id"].Value = FocPackPoId;

                Cmd.Parameters.Add("in_lang", OracleType.VarChar);
                Cmd.Parameters["in_lang"].Value = lang;

                Cmd.Parameters.Add("out_plandata", OracleType.VarChar, 4000);
                Cmd.Parameters["out_plandata"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string basicData = Convert.ToString(Cmd.Parameters["out_plandata"].Value);


                return exeCode + "#" + basicData;

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TxnAssignPlan-getFreePlanDetails");
                return "-1000" + "#" + "ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string getFreePlanDetails(string username, string BasicPlanPoidId, string FocPackPoId)
        {

            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_foc_plan_fetch", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;


                Cmd.Parameters.Add("in_basicplan_id", OracleType.VarChar);
                Cmd.Parameters["in_basicplan_id"].Value = BasicPlanPoidId;

                Cmd.Parameters.Add("in_focplan_id", OracleType.VarChar);
                Cmd.Parameters["in_focplan_id"].Value = FocPackPoId;


                Cmd.Parameters.Add("out_plandata", OracleType.VarChar, 4000);
                Cmd.Parameters["out_plandata"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string basicData = Convert.ToString(Cmd.Parameters["out_plandata"].Value);


                return exeCode + "#" + basicData;

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TxnAssignPlan-getFreePlanDetails");
                return "-1000" + "#" + "ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string Funaddplanbalancecheck(string username, string oper_id, string Newplan_Poid)
        {
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_provi_change_valid", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_lcoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcoid"].Value = oper_id;

                Cmd.Parameters.Add("in_planid", OracleType.VarChar, 100);
                Cmd.Parameters["in_planid"].Value = Newplan_Poid;

                Cmd.Parameters.Add("out_PlanName", OracleType.VarChar, 1000);
                Cmd.Parameters["out_PlanName"].Direction = ParameterDirection.Output;


                Cmd.Parameters.Add("out_data", OracleType.VarChar, 1000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string planname = Convert.ToString(Cmd.Parameters["out_PlanName"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "$" + planname + "$" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cl_Data_TxnAssignPlan-ValidateProvTrans");
                return "-300$ex_occured";

            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
        public string saveModifiesCust(string username, string account, string firstname, string middlename, string lastname, string email, string mobile, string address, string term, string lcocode, string firstname_old, string middlename_old, string lastname_old, string email_old, string mobile_old, string address_old)
        {

            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_cust_Modify_ins", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_account", OracleType.VarChar);
                Cmd.Parameters["in_account"].Value = account;

                Cmd.Parameters.Add("in_firstname", OracleType.VarChar);
                Cmd.Parameters["in_firstname"].Value = firstname;

                Cmd.Parameters.Add("in_middlename", OracleType.VarChar);
                Cmd.Parameters["in_middlename"].Value = middlename;

                Cmd.Parameters.Add("in_lastname", OracleType.VarChar);
                Cmd.Parameters["in_lastname"].Value = lastname;

                Cmd.Parameters.Add("in_email", OracleType.VarChar);
                Cmd.Parameters["in_email"].Value = email;

                Cmd.Parameters.Add("in_mobile", OracleType.VarChar);
                Cmd.Parameters["in_mobile"].Value = mobile;

                Cmd.Parameters.Add("in_address", OracleType.VarChar);
                Cmd.Parameters["in_address"].Value = address;

                Cmd.Parameters.Add("in_firstname_old", OracleType.VarChar);
                Cmd.Parameters["in_firstname_old"].Value = firstname_old;

                Cmd.Parameters.Add("in_middlename_old", OracleType.VarChar);
                Cmd.Parameters["in_middlename_old"].Value = middlename_old;

                Cmd.Parameters.Add("in_lastname_old", OracleType.VarChar);
                Cmd.Parameters["in_lastname_old"].Value = lastname_old;

                Cmd.Parameters.Add("in_email_old", OracleType.VarChar);
                Cmd.Parameters["in_email_old"].Value = email_old;

                Cmd.Parameters.Add("in_mobile_old", OracleType.VarChar);
                Cmd.Parameters["in_mobile_old"].Value = mobile_old;

                Cmd.Parameters.Add("in_address_old", OracleType.VarChar);
                Cmd.Parameters["in_address_old"].Value = address_old;

                Cmd.Parameters.Add("in_terms", OracleType.VarChar);
                Cmd.Parameters["in_terms"].Value = term;

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar);
                Cmd.Parameters["in_lcocode"].Value = lcocode;


                Cmd.Parameters.Add("out_data", OracleType.VarChar, 4000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string basicData = Convert.ToString(Cmd.Parameters["out_data"].Value);


                return exeCode + "#" + basicData;

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TxnAssignPlan-saveModifiesCust");
                return "-1000" + "#" + "ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string getvcwise_planstr(string username, string service_str, string planpoid) //Added by Vivek Singh on 12-Jul-2016
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            service_str = service_str.Replace('|', '$');
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_fetch_cancel_plan", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_str", OracleType.VarChar);
                Cmd.Parameters["in_str"].Value = service_str;

                Cmd.Parameters.Add("in_planpoid", OracleType.VarChar);
                Cmd.Parameters["in_planpoid"].Value = planpoid;

                Cmd.Parameters.Add("out_bsdata", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_bsdata"].Direction = ParameterDirection.Output;


                Cmd.Parameters.Add("out_addata", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_addata"].Direction = ParameterDirection.Output;


                Cmd.Parameters.Add("out_aldata", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_aldata"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Int32);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string basicData = Convert.ToString(Cmd.Parameters["out_bsdata"].Value);
                string add_onData = Convert.ToString(Cmd.Parameters["out_addata"].Value);
                string ala_data = Convert.ToString(Cmd.Parameters["out_aldata"].Value);


                return exeCode + "#" + add_onData + "~" + ala_data + "~" + basicData;

            }
            catch (Exception ex)
            {

                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "AutoBulkTrans-getchildvc_planstr");
                return "-1000" + "#" + "ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string getvcwise_planstr_specialplan(string username, string service_str) //Added by Vivek Singh on 14-Jul-2016
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            service_str = service_str.Replace('|', '$');
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_fetch_can_spl_plan", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                /* in_username    IN   VARCHAR2,
 in_str IN   VARCHAR2, 
 out_bsdata       OUT  varchar2,
 out_addata       OUT  varchar2,
 out_aldata       OUT  varchar2,
 out_errcode    OUT  NUMBER*/


                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_str", OracleType.VarChar);
                Cmd.Parameters["in_str"].Value = service_str;

                Cmd.Parameters.Add("out_addata", OracleType.LongVarChar, 40000);
                Cmd.Parameters["out_addata"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Int32);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string add_onData = Convert.ToString(Cmd.Parameters["out_addata"].Value);


                return exeCode + "#" + add_onData;

            }
            catch (Exception ex)
            {

                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "AutoBulkTrans-getvcwise_planstr_specialplan");
                return "-1000" + "#" + "ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string InsertDiscount(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);

            try
            {
                conObj.Open();


                OracleCommand Cmd = new OracleCommand("aoup_lcopre_discount_ins_new", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = htAddPlanParams["username"];

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar, 100);
                Cmd.Parameters["in_vcid"].Value = htAddPlanParams["vcid"];

                Cmd.Parameters.Add("in_amount", OracleType.Number);
                Cmd.Parameters["in_amount"].Value = htAddPlanParams["amount"];

                Cmd.Parameters.Add("in_discounttype", OracleType.VarChar, 100);
                Cmd.Parameters["in_discounttype"].Value = htAddPlanParams["distype"];

                Cmd.Parameters.Add("in_reason", OracleType.VarChar, 100);
                Cmd.Parameters["in_reason"].Value = htAddPlanParams["reason"];

                Cmd.Parameters.Add("in_Expirydate", OracleType.VarChar, 100);
                Cmd.Parameters["in_Expirydate"].Value = htAddPlanParams["expdt"];



                Cmd.Parameters.Add("out_errordata", OracleType.VarChar, 100);
                Cmd.Parameters["out_errordata"].Direction = ParameterDirection.Output;


                Cmd.Parameters.Add("out_errorcode", OracleType.Number);
                Cmd.Parameters["out_errorcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errorcode"].Value);
                string exeMsg = Convert.ToString(Cmd.Parameters["out_errordata"].Value);
                return exeCode + "$" + exeMsg;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_onebyonediscount-GetDetails");
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