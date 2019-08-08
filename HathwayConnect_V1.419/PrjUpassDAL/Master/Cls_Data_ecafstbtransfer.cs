using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Collections;
using System.Configuration;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Master
{
    public class Cls_Data_ecafstbtransfer
    {
        public string InsertDetails(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_stbtrans_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 10);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_stb", OracleType.VarChar, 4000);
                Cmd.Parameters["in_stb"].Value = ht["in_stb"];

                Cmd.Parameters.Add("in_lcoCode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcoCode"].Value = ht["in_lcoCode"];

                Cmd.Parameters.Add("in_translcoCode", OracleType.VarChar, 4000);
                Cmd.Parameters["in_translcoCode"].Value = ht["in_translcoCode"];

                //Cmd.Parameters.Add("in_translcocity", OracleType.VarChar, 4000);
                //Cmd.Parameters["in_translcocity"].Value = ht["in_translcocity"];

                //Cmd.Parameters.Add("in_translcoDAS", OracleType.VarChar, 4000);
                //Cmd.Parameters["in_translcoDAS"].Value = ht["in_translcoDAS"];

                Cmd.Parameters.Add("in_amount", OracleType.Number);
                Cmd.Parameters["in_amount"].Value = ht["in_amount"];

                Cmd.Parameters.Add("in_RefId", OracleType.VarChar, 4000);
                Cmd.Parameters["in_RefId"].Value = ht["in_RefId"];

                Cmd.Parameters.Add("in_operid", OracleType.Number);
                Cmd.Parameters["in_operid"].Value = ht["in_operid"];

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
                    Str = " STB Transfer Details Inserted successfully...";
                }
                else
                {
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                ConObj.Dispose();
                return exeResult + "$" + Str;
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

        public string InsertLCOTransfer(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_stbtranslcoacc_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 10);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_AccessString", OracleType.VarChar, 4000);
                Cmd.Parameters["in_AccessString"].Value = ht["in_AccessString"];

                Cmd.Parameters.Add("in_remark", OracleType.VarChar, 4000);
                Cmd.Parameters["in_remark"].Value = ht["in_remark"];

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
                    Str = " STB Transfer Status Updated Successfully";
                }
                else
                {
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                ConObj.Dispose();
                return exeResult + "$" + Str;
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

        public string InsertAdminTransfer(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_stbtransadmacc_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 10);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_AccessString", OracleType.VarChar, 4000);
                Cmd.Parameters["in_AccessString"].Value = ht["in_AccessString"];

                Cmd.Parameters.Add("in_remark", OracleType.VarChar, 4000);
                Cmd.Parameters["in_remark"].Value = ht["in_remark"];

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
                    Str = " STB Transfer Status Updated Successfully";
                }
                else
                {
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                ConObj.Dispose();
                return exeResult + "$" + Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstHwaymsgBrdr-InsertAdminTransfer");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string InsertLCOConfig(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_lcoconfig_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 10);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_state", OracleType.VarChar, 4000);
                Cmd.Parameters["in_state"].Value = ht["in_state"];

                Cmd.Parameters.Add("in_city", OracleType.VarChar, 4000);
                Cmd.Parameters["in_city"].Value = ht["in_city"];

                Cmd.Parameters.Add("in_DAS", OracleType.VarChar, 4000);
                Cmd.Parameters["in_DAS"].Value = ht["in_DAS"];

                Cmd.Parameters.Add("in_LCO", OracleType.VarChar, 4000);
                Cmd.Parameters["in_LCO"].Value = ht["in_LCO"];

                Cmd.Parameters.Add("in_adminlevel", OracleType.VarChar, 4000);
                Cmd.Parameters["in_adminlevel"].Value = ht["in_adminlevel"];


                Cmd.Parameters.Add("in_stateid", OracleType.VarChar, 4000);
                Cmd.Parameters["in_stateid"].Value = ht["in_stateid"];

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
                    Str = " Configuration Details Added successfully...";
                }
                else
                {
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                ConObj.Dispose();
                return exeResult + "$" + Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstHwaymsgBrdr-InsertAdminTransfer");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public DataTable GetAdminRejectedList(string username, Hashtable htAddPlanParams)
        {
            try
            {
                string from = htAddPlanParams["fromDt"].ToString();
                string to = htAddPlanParams["ToDt"].ToString(); //package
                string operid = htAddPlanParams["operid"].ToString(); //package
                Cls_Helper ObjHelper = new Cls_Helper();

                string StrQry = " ";
                StrQry += " select var_stbtransfer_refid refid,var_stbtransfer_stbno stbno,var_stbtransfer_reason adminremark,var_stbtransfer_insby insby, ";
                StrQry += " date_stbtransfer_insdate insdate,var_stbtransfer_approvalby adminapprovlby,";
                StrQry += " date_stbtransfer_approvaldate adminappdate from AOUP_LCOPRE_STBTRANSFER_MST where var_stbtransfer_status='C' and trunc(date_stbtransfer_insdate)>='" + from + "' and ";
                StrQry += " trunc(date_stbtransfer_insdate)<='" + to + "' and num_stbtransfer_operid=" + operid;
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "GetAdminRejectedList.cs-GetDetails");
                return null;
            }
        }

        public DataTable STBTransferList(string username, Hashtable htAddPlanParams)
        {
            try
            {
                string from = htAddPlanParams["fromDt"].ToString();
                string to = htAddPlanParams["ToDt"].ToString(); //package
                string operid = htAddPlanParams["operid"].ToString(); //package
                Cls_Helper ObjHelper = new Cls_Helper();

                string StrQry = " ";
                StrQry += " select * from vw_stbtransfer where  ";
                StrQry += " trunc(insdate)>='" + from + "' and trunc(insdate)<='" + to + "' and  ";
                StrQry += " operid='" + operid + "' order  by insdate desc";
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "GetAdminRejectedList.cs-GetDetails");
                return null;
            }
        }
    }
}
