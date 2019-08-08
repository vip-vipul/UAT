using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_Faulty_Swap
    {
        public void Get_Faulty_Swap(string whereClauseStr, string username, out string OutStatus)
        {
            OutStatus = "";
            try
            {
                string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
                OracleConnection conObj = new OracleConnection(ConStr);

                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "select * from  reports.stb_dump_final@caslive_new ";
                StrQry += " where " + whereClauseStr;
                OracleCommand cmd = new OracleCommand(StrQry, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    OutStatus = dr["nds_no"].ToString();
                }
                conObj.Close();
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_Faulty_Swap.cs");

            }
        }

        public void GetPlanTo_Swap(string username, string strMainTV, string strChildTV, string strType, out string OutStatus)
        {
            OutStatus = "";
            try
            {
                string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
                OracleConnection conObj = new OracleConnection(ConStr);
                OracleConnection ConObj = new OracleConnection(ConStr);
                try
                {
                    ConObj.Open();
                    OracleCommand Cmd = new OracleCommand("aoup_lcopre_inv_mainchild_swap", ConObj);
                    Cmd.CommandType = CommandType.StoredProcedure;

                    Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                    Cmd.Parameters["in_username"].Value = username;

                    if (strMainTV != "" || strMainTV != null)
                    {
                        Cmd.Parameters.Add("in_mainTV_str", OracleType.VarChar, 150);
                        Cmd.Parameters["in_mainTV_str"].Value = strMainTV;
                    }
                    else
                    {
                        Cmd.Parameters.Add("in_mainTV_str", OracleType.VarChar, 150);
                        Cmd.Parameters["in_mainTV_str"].Value = DBNull.Value;
                    }
                    if (strChildTV != "" || strChildTV != null)
                    {
                        Cmd.Parameters.Add("in_childTV_str", OracleType.VarChar, 150);
                        Cmd.Parameters["in_childTV_str"].Value = strChildTV;
                    }
                    else
                    {
                        Cmd.Parameters.Add("in_childTV_str", OracleType.VarChar, 150);
                        Cmd.Parameters["in_childTV_str"].Value = DBNull.Value;
                    }

                    if (strType != "" || strType != null)
                    {
                        Cmd.Parameters.Add("in_Type_str", OracleType.VarChar, 150);
                        Cmd.Parameters["in_Type_str"].Value = strType;
                    }
                    else
                    {
                        Cmd.Parameters.Add("in_Type_str", OracleType.VarChar, 150);
                        Cmd.Parameters["in_Type_str"].Value = DBNull.Value;
                    }

                    Cmd.Parameters.Add("out_errorcode", OracleType.VarChar, 100);
                    Cmd.Parameters["out_errorcode"].Direction = ParameterDirection.Output;

                    Cmd.Parameters.Add("out_errormsg", OracleType.VarChar, 150);
                    Cmd.Parameters["out_errormsg"].Direction = ParameterDirection.Output;


                    Cmd.ExecuteNonQuery();
                    ConObj.Close();

                    string res_code = Convert.ToString(Cmd.Parameters["out_errorcode"].Value);
                    string res_data = Convert.ToString(Cmd.Parameters["out_errormsg"].Value);
                    OutStatus = res_code + "$" + res_data;

                }
                catch (Exception ex)
                {
                    Cls_Security objSecurity = new Cls_Security();
                    objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_Faulty_Swap-GetPlanTo_Swap");
                    //return "-1000" + "#" + "Something went wrong...";
                }
                finally
                {
                    ConObj.Close();
                    ConObj.Dispose();
                }
            }
            catch (Exception)
            {

            }


        }

        public string getSTB_Swap(Hashtable htData)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_STB_SWAP_INST", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = htData["username"];

                Cmd.Parameters.Add("in_AccountNo", OracleType.VarChar);
                Cmd.Parameters["in_AccountNo"].Value = htData["AccountNo"];
                if (htData["FromVC"].ToString() != "" || htData["FromVC"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_FromVC", OracleType.VarChar);
                    Cmd.Parameters["in_FromVC"].Value = htData["FromVC"];
                }
                else
                {
                    Cmd.Parameters.Add("in_FromVC", OracleType.VarChar);
                    Cmd.Parameters["in_FromVC"].Value = DBNull.Value;
                }
                if (htData["FromSTB"].ToString() != "" || htData["FromSTB"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_FromSTB", OracleType.VarChar);
                    Cmd.Parameters["in_FromSTB"].Value = htData["FromSTB"];
                }
                else
                {
                    Cmd.Parameters.Add("in_FromSTB", OracleType.VarChar);
                    Cmd.Parameters["in_FromSTB"].Value = DBNull.Value;
                }
                if (htData["TOVC"].ToString() != "" || htData["TOVC"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_TOVC", OracleType.VarChar);
                    Cmd.Parameters["in_TOVC"].Value = htData["TOVC"];
                }
                else
                {
                    Cmd.Parameters.Add("in_TOVC", OracleType.VarChar);
                    Cmd.Parameters["in_TOVC"].Value = DBNull.Value;
                }
                if (htData["TOSTB"].ToString() != "" || htData["TOSTB"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_TOSTB", OracleType.VarChar);
                    Cmd.Parameters["in_TOSTB"].Value = htData["TOSTB"];
                }
                else
                {
                    Cmd.Parameters.Add("in_TOSTB", OracleType.VarChar);
                    Cmd.Parameters["in_TOSTB"].Value = DBNull.Value;
                }
                Cmd.Parameters.Add("in_Reason", OracleType.VarChar);
                Cmd.Parameters["in_Reason"].Value = htData["Reason"];

                Cmd.Parameters.Add("in_Type", OracleType.VarChar, 100);
                Cmd.Parameters["in_Type"].Value = htData["Type"];

                Cmd.Parameters.Add("in_Status", OracleType.VarChar);
                Cmd.Parameters["in_Status"].Value = htData["Status"];
                if (htData["CustomerName"].ToString() != "" || htData["CustomerName"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_CustomerName", OracleType.VarChar);
                    Cmd.Parameters["in_CustomerName"].Value = htData["CustomerName"];
                }
                else
                {
                    Cmd.Parameters.Add("in_CustomerName", OracleType.VarChar);
                    Cmd.Parameters["in_CustomerName"].Value = DBNull.Value;
                }
                if (htData["MobileNo"].ToString() != "" || htData["MobileNo"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_MobileNo", OracleType.VarChar);
                    Cmd.Parameters["in_MobileNo"].Value = htData["MobileNo"];
                }
                else
                {
                    Cmd.Parameters.Add("in_MobileNo", OracleType.VarChar);
                    Cmd.Parameters["in_MobileNo"].Value = DBNull.Value;
                }
                if (htData["EmailID"].ToString() != "" || htData["EmailID"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_EmailID", OracleType.VarChar);
                    Cmd.Parameters["in_EmailID"].Value = htData["EmailID"];
                }
                else
                {
                    Cmd.Parameters.Add("in_EmailID", OracleType.VarChar);
                    Cmd.Parameters["in_EmailID"].Value = DBNull.Value;
                }
                if (htData["CustomerAdd"].ToString() != "" || htData["CustomerAdd"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_CustomerAdd", OracleType.VarChar, 100);
                    Cmd.Parameters["in_CustomerAdd"].Value = htData["CustomerAdd"];
                }
                else
                {
                    Cmd.Parameters.Add("in_CustomerAdd", OracleType.VarChar, 100);
                    Cmd.Parameters["in_CustomerAdd"].Value = DBNull.Value;
                }
                if (htData["OperID"].ToString() != "" || htData["OperID"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_OperID", OracleType.VarChar, 100);
                    Cmd.Parameters["in_OperID"].Value = htData["OperID"];
                }
                else
                {
                    Cmd.Parameters.Add("in_OperID", OracleType.VarChar, 100);
                    Cmd.Parameters["in_OperID"].Value = DBNull.Value;
                }
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
                objSecurity.InsertIntoDb(htData["username"].ToString(), ex.Message.ToString(), "Data_TxnAssignPlan-getSTB_Swap");
                return "-1000" + "#" + "Something went wrong...";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

    }
}