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
    public class Cls_Data_MstLCOUpdateDetails
    {
        public string UpdateLCOData(string username, string LCOCode, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_lcodata_update", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocode"].Value = LCOCode;



                Cmd.Parameters.Add("in_email", OracleType.VarChar, 100);
                Cmd.Parameters["in_email"].Value = ht["email"];

                Cmd.Parameters.Add("in_mobileno", OracleType.VarChar, 100);


                Cmd.Parameters["in_mobileno"].Value = ht["mobileno"];

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

                    Str = " LCO Details Update successfully...";

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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCOUpdateDetails.cs-UpdateLCOData");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();

            }
        }

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
                StrQry += " a.var_usermst_email, ";
                StrQry += " a.num_usermst_mobileno, a.var_lcomst_companycode, ";
                StrQry += " a.var_usermst_accno, a.var_usermst_insby, a.var_usermst_insdt, ";
                StrQry += " a.var_usermst_flag,a.state, a.city, a.lconame, a.lcocode, a.jvno, a.directno, a.distributor, a.subdistributor ";
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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCOUpdateDetails.cs-getUserData");
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
                StrQry = "SELECT a.mst_id, a.lcoid, a.lconame, a.fname, a.mname, a.lname, a.addr,";
                StrQry += " a.lcocode, a.stateid, a.cityid, a.email, a.mobileno, a.jvno, ";
                StrQry += "  a.directno, a.brmpoid, a.company, a.distname, a.subdist, ";
                StrQry += "   a.companycode, a.insby, a.insdt, a.insdt1, a.pin, a.operid, ";
                StrQry += "  a.opercategory, a.parentid, a.distid, a.state, a.city ";
                StrQry += "  FROM view_prelco_lco_det a ";
                StrQry += "  where " + whereClauseStr;

                /*dtLCO = objHelper.GetDataTable(strLCOQry);
                DataSet dsLCOData = new DataSet();
                dsLCOData.Tables.Add(dtLCO);
                return dsLCOData;*/
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCOUpdateDetails.cs-getUserData");
                return null;
            }
        }

        public string LCOAssignRights(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("AOUP_LCOPRE_ACCESS_CTRL_INST", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                if (ht["LCOCode"].ToString() != "" && ht["LCOCode"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_LCOCode", OracleType.VarChar, 100);
                    Cmd.Parameters["in_LCOCode"].Value = ht["LCOCode"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_LCOCode", OracleType.VarChar, 100);
                    Cmd.Parameters["in_LCOCode"].Value = DBNull.Value;
                }

                if (ht["UserAccMap"].ToString() != "" && ht["UserAccMap"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_UserAccMap", OracleType.VarChar, 100);
                    Cmd.Parameters["in_UserAccMap"].Value = ht["UserAccMap"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_UserAccMap", OracleType.VarChar, 100);
                    Cmd.Parameters["in_UserAccMap"].Value = DBNull.Value;
                }

                if (ht["Add"].ToString() != "" && ht["Add"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_Add", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Add"].Value = ht["Add"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_Add", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Add"].Value = DBNull.Value;
                }

                if (ht["Renew"].ToString() != "" && ht["Renew"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_renew", OracleType.VarChar, 100);
                    Cmd.Parameters["in_renew"].Value = ht["Renew"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_renew", OracleType.VarChar, 100);
                    Cmd.Parameters["in_renew"].Value = DBNull.Value;
                }
                if (ht["Change"].ToString() != "" && ht["Change"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_Change", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Change"].Value = ht["Change"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_Change", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Change"].Value = DBNull.Value;
                }
                if (ht["Cancel"].ToString() != "" && ht["Cancel"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_Cancel", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Cancel"].Value = ht["Cancel"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_Cancel", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Cancel"].Value = DBNull.Value;
                }
                if (ht["Discount"].ToString() != "" && ht["Discount"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_Discount", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Discount"].Value = ht["Discount"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_Discount", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Discount"].Value = DBNull.Value;
                }
                if (ht["Retrack"].ToString() != "" && ht["Retrack"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_Retrack", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Retrack"].Value = ht["Retrack"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_Retrack", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Retrack"].Value = DBNull.Value;
                }
                if (ht["CustModify"].ToString() != "" && ht["CustModify"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_CustModify", OracleType.VarChar, 100);
                    Cmd.Parameters["in_CustModify"].Value = ht["CustModify"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_CustModify", OracleType.VarChar, 100);
                    Cmd.Parameters["in_CustModify"].Value = DBNull.Value;
                }
                if (ht["STBSwap"].ToString() != "" && ht["STBSwap"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_STBSwap", OracleType.VarChar, 100);
                    Cmd.Parameters["in_STBSwap"].Value = ht["STBSwap"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_STBSwap", OracleType.VarChar, 100);
                    Cmd.Parameters["in_STBSwap"].Value = DBNull.Value;
                }
                if (ht["AutoRenew"].ToString() != "" && ht["AutoRenew"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_AutoRenew", OracleType.VarChar, 100);
                    Cmd.Parameters["in_AutoRenew"].Value = ht["AutoRenew"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_AutoRenew", OracleType.VarChar, 100);
                    Cmd.Parameters["in_AutoRenew"].Value = DBNull.Value;
                }

                if (ht["Deactivate"].ToString() != "" && ht["Deactivate"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_Deactivate", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Deactivate"].Value = ht["Deactivate"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_Deactivate", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Deactivate"].Value = DBNull.Value;
                }

                if (ht["Terminate"].ToString() != "" && ht["Terminate"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_Terminate", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Terminate"].Value = ht["Terminate"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_Terminate", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Terminate"].Value = DBNull.Value;
                }
                if (ht["FocPack"].ToString() != "" && ht["FocPack"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_FOCPack", OracleType.VarChar, 100);
                    Cmd.Parameters["in_FOCPack"].Value = ht["FocPack"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_FOCPack", OracleType.VarChar, 100);
                    Cmd.Parameters["in_FOCPack"].Value = DBNull.Value;
                }

                if (ht["Pages"].ToString() != "" && ht["Pages"].ToString() != null)
                {
                    Cmd.Parameters.Add("in_Page", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Page"].Value = ht["Pages"].ToString();
                }
                else
                {
                    Cmd.Parameters.Add("in_Page", OracleType.VarChar, 100);
                    Cmd.Parameters["in_Page"].Value = DBNull.Value;
                }

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

                    Str = " LCO Rights Submitted successfully...";

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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCOUpdateDetails.cs-LCOAssignRights");
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
