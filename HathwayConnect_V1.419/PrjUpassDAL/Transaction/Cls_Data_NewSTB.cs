using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_NewSTB
    {

        public string[] GetLcoDetails(string username, string prefixText, string type, string operid, string catid)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "select var_user_username username,var_lcomst_name name, num_lcomst_mobileno mobileno,var_lcomst_email email, ";
                str += "a.var_lcomst_address address, nvl(b.num_lcomst_invbal, 0) invbal,num_user_curcrlimit LCOBalance ";
                str += "from aoup_lcopre_lco_det a, aoup_user_def b ";
                str += "where a.var_lcomst_code=b.var_user_username and b.var_user_username = '" + prefixText + "' and num_user_opercatid= " + catid;

                OracleCommand cmd = new OracleCommand(str, conObj);

                conObj.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(dr["username"].ToString());
                    Operators.Add(dr["name"].ToString());
                    Operators.Add(dr["address"].ToString());
                    Operators.Add(dr["mobileno"].ToString());
                    Operators.Add(dr["email"].ToString());
                    Operators.Add(dr["invbal"].ToString());
                    Operators.Add(dr["LCOBalance"].ToString());
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayLcoPayment.cs-GetLcoDetails");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        /*--------------------------RP-------------------------------*/
        public string[] GetSchemDetails(string SchemeID, string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str = " select max_stb_quanity_allowed stbcount,var_user_boxtype BoxType,nvl(num_scheme_value,0) RateSTB,nvl(num_stb_discount,0) DisSTB,nvl(num_stb_netamount,0) NetSTB,nvl(num_subscription_lcorate,0) RateLCO,nvl(subscription_discount_lcorate,0) DisLCO,nvl(subscription_net_lcorate,0) NetLCO,software_hardware,var_allowpdc allowpdc,  ";
                str += " nvl(num_pdc_tenure,'0') Tenure,nvl(num_lco_upfront,'0') LCOUpfront,nvl(num_stb_upfront,'0') STBUpfront from aoup_lcopre_scheme_master where num_scheme_id='" + SchemeID + "'";
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    Operators.Add(dr["RateSTB"].ToString());
                    Operators.Add(dr["DisSTB"].ToString());
                    Operators.Add(dr["NetSTB"].ToString());
                    Operators.Add(dr["RateLCO"].ToString());
                    Operators.Add(dr["DisLCO"].ToString());
                    Operators.Add(dr["NetLCO"].ToString());
                    Operators.Add(dr["BoxType"].ToString());
                    Operators.Add(dr["software_hardware"].ToString());
                    Operators.Add(dr["stbcount"].ToString());
                    Operators.Add(dr["allowpdc"].ToString());
                    Operators.Add(dr["Tenure"].ToString());
                    Operators.Add(dr["LCOUpfront"].ToString());
                    Operators.Add(dr["STBUpfront"].ToString());
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayLcoPayment.cs-GetLcoDetails");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        /*----------------------PP----------------------*/
        public string InsertNewSTBPP(string sp, Hashtable ht)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            int returnValue = 0;
            try
            {
                OracleCommand cmd = new OracleCommand(sp, ConObj);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add("IN_SNEWSTB_STBNO", OracleType.VarChar);
                cmd.Parameters["IN_SNEWSTB_STBNO"].Value = ht["STBSRNo"];

                if (ht["Accno"] == "" || ht["Accno"] == null)
                {
                    cmd.Parameters.Add("IN_NEWSTB_ACCNO", OracleType.VarChar);
                    cmd.Parameters["IN_NEWSTB_ACCNO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_NEWSTB_ACCNO", OracleType.VarChar);
                    cmd.Parameters["IN_NEWSTB_ACCNO"].Value = ht["Accno"];
                }

                if (ht["vcno"] == "" || ht["vcno"] == null)
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_VCNO", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_VCNO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_VCNO", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_VCNO"].Value = ht["vcno"];
                }

                if (ht["custname"] == "" || ht["custname"] == null)
                {
                    cmd.Parameters.Add("IN_PISNEWSRB_CUSTNAME", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSRB_CUSTNAME"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISNEWSRB_CUSTNAME", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSRB_CUSTNAME"].Value = ht["custname"];
                }
                if (ht["boxType"] == "" || ht["boxType"] == null)
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_BOXTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_BOXTYPE"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_BOXTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_BOXTYPE"].Value = ht["boxType"];
                }

                if (ht["SchemeID"] == "" || ht["SchemeID"] == null)
                {
                    cmd.Parameters.Add("IN_SCHEME_ID", OracleType.Number);
                    cmd.Parameters["IN_SCHEME_ID"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("IN_SCHEME_ID", OracleType.Number);
                    cmd.Parameters["IN_SCHEME_ID"].Value = ht["SchemeID"];
                }

                if (ht["PayMode"] == "" || ht["PayMode"] == null)
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_PAYMODE", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_PAYMODE"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_PAYMODE", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_PAYMODE"].Value = ht["PayMode"];
                }

                if (ht["Remark"] == "" || ht["Remark"] == null)
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_REMARK", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_REMARK"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_REMARK", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_REMARK"].Value = ht["Remark"];
                }

                if (ht["ChequeNo"] == "" || ht["ChequeNo"] == null)
                {
                    cmd.Parameters.Add("IN_PISCHEQUE_NO", OracleType.VarChar);
                    cmd.Parameters["IN_PISCHEQUE_NO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISCHEQUE_NO", OracleType.VarChar);
                    cmd.Parameters["IN_PISCHEQUE_NO"].Value = ht["ChequeNo"];
                }

                if (ht["BankId"] == "" || ht["BankId"] == null)
                {
                    cmd.Parameters.Add("IN_PISBANK_ID", OracleType.Number);
                    cmd.Parameters["IN_PISBANK_ID"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISBANK_ID", OracleType.Number);
                    cmd.Parameters["IN_PISBANK_ID"].Value = ht["BankId"];
                }

                if (ht["Bankbranch"] == "" || ht["Bankbranch"] == null)
                {
                    cmd.Parameters.Add("IN_PISBANKBRANCH", OracleType.VarChar);
                    cmd.Parameters["IN_PISBANKBRANCH"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISBANKBRANCH", OracleType.VarChar);
                    cmd.Parameters["IN_PISBANKBRANCH"].Value = ht["Bankbranch"];
                }


                if (ht["RRNO"] == "" || ht["RRNO"] == null)
                {
                    cmd.Parameters.Add("IN_PISRR_NO", OracleType.VarChar);
                    cmd.Parameters["IN_PISRR_NO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISRR_NO", OracleType.VarChar);
                    cmd.Parameters["IN_PISRR_NO"].Value = ht["RRNO"];
                }

                if (ht["mposid"] == "" || ht["mposid"] == null)
                {
                    cmd.Parameters.Add("IN_PISMPOS_USERID", OracleType.VarChar);
                    cmd.Parameters["IN_PISMPOS_USERID"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISMPOS_USERID", OracleType.VarChar);
                    cmd.Parameters["IN_PISMPOS_USERID"].Value = ht["mposid"];
                }

                if (ht["AuthCode"] == "" || ht["AuthCode"] == null)
                {
                    cmd.Parameters.Add("IN_PISMPOS_AUTHNUMBER", OracleType.VarChar);
                    cmd.Parameters["IN_PISMPOS_AUTHNUMBER"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISMPOS_AUTHNUMBER", OracleType.VarChar);
                    cmd.Parameters["IN_PISMPOS_AUTHNUMBER"].Value = ht["AuthCode"];
                }

                if (ht["insertBy"] == "" || ht["insertBy"] == null)
                {
                    cmd.Parameters.Add("IN_PISTRANS_INSBY", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_INSBY"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_INSBY", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_INSBY"].Value = ht["insertBy"];
                }

                if (ht["transtype"] == "" || ht["transtype"] == null)
                {
                    cmd.Parameters.Add("IN_PISTRANS_TRANSTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_TRANSTYPE"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_TRANSTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_TRANSTYPE"].Value = DBNull.Value;
                }

                if (ht["transubtype"] == "" || ht["transubtype"] == null)
                {
                    cmd.Parameters.Add("IN_PISTRANS_TRANSSUBTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_TRANSSUBTYPE"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_TRANSSUBTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_TRANSSUBTYPE"].Value = ht["transubtype"];
                }

                if (ht["City"] == "" || ht["City"] == null)
                {
                    cmd.Parameters.Add("IN_PISTRANS_CITY", OracleType.Number);
                    cmd.Parameters["IN_PISTRANS_CITY"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_CITY", OracleType.Number);
                    cmd.Parameters["IN_PISTRANS_CITY"].Value = ht["City"];

                }
                if (ht["City"] == "" || ht["City"] == null)
                {
                    cmd.Parameters.Add("IN_PISTRANS_STATE", OracleType.Number);
                    cmd.Parameters["IN_PISTRANS_STATE"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_STATE", OracleType.Number);
                    cmd.Parameters["IN_PISTRANS_STATE"].Value = ht["State"];
                }


                cmd.Parameters.Add("IN_Mode", OracleType.VarChar);
                cmd.Parameters["IN_Mode"].Value = ht["mode"];
                cmd.Parameters.Add("in_ChqDate", OracleType.DateTime);
                cmd.Parameters["in_ChqDate"].Value = ht["ChequeDate"];
                cmd.Parameters.Add("IN_userid", OracleType.VarChar);
                cmd.Parameters["IN_userid"].Value = ht["userid"];

                if (ht["Mobileno"] == "" || ht["Mobileno"] == null)
                {
                    cmd.Parameters.Add("IN_MOBILENO", OracleType.Number);
                    cmd.Parameters["IN_MOBILENO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_MOBILENO", OracleType.Number);
                    cmd.Parameters["IN_MOBILENO"].Value = ht["Mobileno"];
                }
                if (ht["RateSTB"] == "" || ht["RateSTB"] == null)
                {
                    cmd.Parameters.Add("In_RateSTB", OracleType.Number);
                    cmd.Parameters["In_RateSTB"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("In_RateSTB", OracleType.Number);
                    cmd.Parameters["In_RateSTB"].Value = ht["RateSTB"];
                }
                if (ht["DiscountSTB"] == "" || ht["DiscountSTB"] == null)
                {
                    cmd.Parameters.Add("In_DiscountSTB", OracleType.Number);
                    cmd.Parameters["In_DiscountSTB"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("In_DiscountSTB", OracleType.Number);
                    cmd.Parameters["In_DiscountSTB"].Value = ht["DiscountSTB"];
                }
                if (ht["NetSTB"] == "" || ht["NetSTB"] == null)
                {
                    cmd.Parameters.Add("In_NetSTB", OracleType.Number);
                    cmd.Parameters["In_NetSTB"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("In_NetSTB", OracleType.Number);
                    cmd.Parameters["In_NetSTB"].Value = ht["NetSTB"];
                }
                if (ht["RateLCO"] == "" || ht["RateLCO"] == null)
                {
                    cmd.Parameters.Add("In_RateLCO", OracleType.Number);
                    cmd.Parameters["In_RateLCO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("In_RateLCO", OracleType.Number);
                    cmd.Parameters["In_RateLCO"].Value = ht["RateLCO"];
                }
                if (ht["DiscountLCO"] == "" || ht["DiscountLCO"] == null)
                {
                    cmd.Parameters.Add("In_DiscountLCO", OracleType.Number);
                    cmd.Parameters["In_DiscountLCO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("In_DiscountLCO", OracleType.Number);
                    cmd.Parameters["In_DiscountLCO"].Value = ht["DiscountLCO"];
                }
                if (ht["NetLCO"] == "" || ht["NetLCO"] == null)
                {
                    cmd.Parameters.Add("In_NetLCO", OracleType.Number);
                    cmd.Parameters["In_NetLCO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("In_NetLCO", OracleType.Number);
                    cmd.Parameters["In_NetLCO"].Value = ht["NetLCO"];
                }
                if (ht["TotalNet"] == "" || ht["TotalNet"] == null)
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_AMOUNT", OracleType.Number);
                    cmd.Parameters["IN_PISNEWSTB_AMOUNT"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_AMOUNT", OracleType.Number);
                    cmd.Parameters["IN_PISNEWSTB_AMOUNT"].Value = ht["TotalNet"];
                }

                cmd.Parameters.Add("Out_ErrorCode", OracleType.Number);
                cmd.Parameters["Out_ErrorCode"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("Out_ErrorMsg", OracleType.VarChar, 1000);
                cmd.Parameters["Out_ErrorMsg"].Direction = ParameterDirection.Output;

                ConObj.Open();
                cmd.ExecuteNonQuery();

                int exeResult = Convert.ToInt32(cmd.Parameters["Out_ErrorCode"].Value);
                string ExeResultMsg = Convert.ToString(cmd.Parameters["out_ErrorMsg"].Value);
                string Str;

                return exeResult + "$" + ExeResultMsg;

                ConObj.Dispose();
                return Str;

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["UserName"].ToString(), ex.Message.ToString(), "frmNewSTB.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public int insertQry(String sb)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            int returnValue = 0;
            try
            {
                OracleCommand cmd = new OracleCommand(sb, ConObj);
                ConObj.Open();
                returnValue = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return returnValue;
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
            return returnValue;
        }

        /*----------------------------SP-------------------*/

        public string InsertNewSTBSP(string sp, Hashtable ht)
        {

            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            int returnValue = 0;
            try
            {
                OracleCommand cmd = new OracleCommand(sp, ConObj);
                cmd.CommandType = CommandType.StoredProcedure;

                if (ht["STBCount"] == null || ht["STBCount"] == "")
                {
                    cmd.Parameters.Add("IN_SNEWSTB_STBCOUNT", OracleType.Number);
                    cmd.Parameters["IN_SNEWSTB_STBCOUNT"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("IN_SNEWSTB_STBCOUNT", OracleType.Number);
                    cmd.Parameters["IN_SNEWSTB_STBCOUNT"].Value = ht["STBCount"];
                }

                if (ht["STBLCO"] == null || ht["STBLCO"] == "")
                {
                    cmd.Parameters.Add("IN_SNEWSTB_STBLCO", OracleType.VarChar);
                    cmd.Parameters["IN_SNEWSTB_STBLCO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_SNEWSTB_STBLCO", OracleType.VarChar);
                    cmd.Parameters["IN_SNEWSTB_STBLCO"].Value = ht["STBLCO"];
                }

                if (ht["Remark"] == null || ht["Remark"] == "")
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_REMARK", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_REMARK"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_REMARK", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_REMARK"].Value = ht["Remark"];
                }

                if (ht["SchemeID"] == null || ht["SchemeID"] == "")
                {
                    cmd.Parameters.Add("IN_SCHEME_ID", OracleType.Number);
                    cmd.Parameters["IN_SCHEME_ID"].Value = 0;

                }
                else
                {
                    cmd.Parameters.Add("IN_SCHEME_ID", OracleType.Number);
                    cmd.Parameters["IN_SCHEME_ID"].Value = Convert.ToInt64(ht["SchemeID"]);
                }


                if (ht["STBSRNo"] == null || ht["STBSRNo"] == "")
                {
                    cmd.Parameters.Add("IN_SNEWSTB_STBNO", OracleType.VarChar);
                    cmd.Parameters["IN_SNEWSTB_STBNO"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_SNEWSTB_STBNO", OracleType.VarChar);
                    cmd.Parameters["IN_SNEWSTB_STBNO"].Value = ht["STBSRNo"];
                }

                if (ht["ChequeNo"] == null || ht["ChequeNo"] == "")
                {
                    cmd.Parameters.Add("IN_PISCHEQUE_NO", OracleType.VarChar);
                    cmd.Parameters["IN_PISCHEQUE_NO"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISCHEQUE_NO", OracleType.VarChar);
                    cmd.Parameters["IN_PISCHEQUE_NO"].Value = ht["ChequeNo"];

                }

                if (ht["BankId"] == null || ht["BankId"] == "")
                {
                    cmd.Parameters.Add("IN_PISBANK_ID", OracleType.Number);
                    cmd.Parameters["IN_PISBANK_ID"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISBANK_ID", OracleType.Number);
                    cmd.Parameters["IN_PISBANK_ID"].Value = Convert.ToInt64(ht["BankId"]);

                }

                if (ht["Branch"] == null || ht["Branch"] == "")
                {
                    cmd.Parameters.Add("IN_PISBANK_BRANCH", OracleType.VarChar);
                    cmd.Parameters["IN_PISBANK_BRANCH"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISBANK_BRANCH", OracleType.VarChar);
                    cmd.Parameters["IN_PISBANK_BRANCH"].Value = ht["Branch"];

                }

                if (ht["RRNo"] == null || ht["RRNo"] == "")
                {
                    cmd.Parameters.Add("IN_PISRR_NO", OracleType.VarChar);
                    cmd.Parameters["IN_PISRR_NO"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISRR_NO", OracleType.VarChar);
                    cmd.Parameters["IN_PISRR_NO"].Value = ht["RRNo"];

                }

                if (ht["mposid"] == null || ht["mposid"] == "")
                {
                    cmd.Parameters.Add("IN_PISMPOS_USERID", OracleType.VarChar);

                    cmd.Parameters["IN_PISMPOS_USERID"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISMPOS_USERID", OracleType.VarChar);
                    cmd.Parameters["IN_PISMPOS_USERID"].Value = ht["mposid"];
                }

                if (ht["AuthCode"] == null || ht["AuthCode"] == "")
                {
                    cmd.Parameters.Add("IN_PISMPOS_AUTHNUMBER", OracleType.VarChar);
                    cmd.Parameters["IN_PISMPOS_AUTHNUMBER"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISMPOS_AUTHNUMBER", OracleType.VarChar);
                    cmd.Parameters["IN_PISMPOS_AUTHNUMBER"].Value = ht["AuthCode"];

                }

                if (ht["transtype"] == null || ht["transtype"] == "")
                {
                    cmd.Parameters.Add("IN_PISTRANS_TRANSTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_TRANSTYPE"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_TRANSTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_TRANSTYPE"].Value = ht["transtype"];
                }

                if (ht["transsubtype"] == null || ht["transsubtype"] == "")
                {
                    cmd.Parameters.Add("IN_PISTRANS_TRANSSUBTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_TRANSSUBTYPE"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_TRANSSUBTYPE", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_TRANSSUBTYPE"].Value = ht["transsubtype"];

                }

                if (ht["PayMode"] == null || ht["PayMode"] == "")
                {

                    cmd.Parameters.Add("IN_PISNEWSTB_PAYMODE", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_PAYMODE"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISNEWSTB_PAYMODE", OracleType.VarChar);
                    cmd.Parameters["IN_PISNEWSTB_PAYMODE"].Value = ht["PayMode"];

                }

                if (ht["City"] == null || ht["City"] == "")
                {

                    cmd.Parameters.Add("IN_PISTRANS_CITY", OracleType.Number);
                    cmd.Parameters["IN_PISTRANS_CITY"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_CITY", OracleType.Number);
                    cmd.Parameters["IN_PISTRANS_CITY"].Value = ht["City"];

                }

                if (ht["State"] == null || ht["State"] == "")
                {

                    cmd.Parameters.Add("IN_PISTRANS_STATE", OracleType.Number);
                    cmd.Parameters["IN_PISTRANS_STATE"].Value = 0;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_STATE", OracleType.Number);
                    cmd.Parameters["IN_PISTRANS_STATE"].Value = ht["State"];

                }

                if (ht["RateSTB"] == null || ht["RateSTB"] == "")
                {

                    cmd.Parameters.Add("In_RateSTB", OracleType.Number);
                    cmd.Parameters["In_RateSTB"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("In_RateSTB", OracleType.Number);
                    cmd.Parameters["In_RateSTB"].Value = ht["RateSTB"];

                }

                if (ht["DiscountSTB"] == null || ht["DiscountSTB"] == "")
                {

                    cmd.Parameters.Add("In_DiscountSTB", OracleType.Number);
                    cmd.Parameters["In_DiscountSTB"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("In_DiscountSTB", OracleType.Number);
                    cmd.Parameters["In_DiscountSTB"].Value = ht["DiscountSTB"];
                }

                if (ht["NetSTB"] == null || ht["NetSTB"] == "")
                {

                    cmd.Parameters.Add("In_NetSTB", OracleType.Number);
                    cmd.Parameters["In_NetSTB"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("In_NetSTB", OracleType.Number);
                    cmd.Parameters["In_NetSTB"].Value = ht["NetSTB"];
                }

                if (ht["RateLCO"] == null || ht["RateLCO"] == "")
                {

                    cmd.Parameters.Add("In_RateLCO", OracleType.Number);
                    cmd.Parameters["In_RateLCO"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("In_RateLCO", OracleType.Number);
                    cmd.Parameters["In_RateLCO"].Value = ht["RateLCO"];
                }

                if (ht["DiscountLCO"] == null || ht["DiscountLCO"] == "")
                {

                    cmd.Parameters.Add("In_DiscountLCO", OracleType.Number);
                    cmd.Parameters["In_DiscountLCO"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("In_DiscountLCO", OracleType.Number);
                    cmd.Parameters["In_DiscountLCO"].Value = ht["DiscountLCO"];
                }

                if (ht["NetLCO"] == null || ht["NetLCO"] == "")
                {

                    cmd.Parameters.Add("In_NetLCO", OracleType.Number);
                    cmd.Parameters["In_NetLCO"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("In_NetLCO", OracleType.Number);
                    cmd.Parameters["In_NetLCO"].Value = ht["NetLCO"];
                }

                if (ht["TotalNet"] == null || ht["TotalNet"] == "")
                {

                    cmd.Parameters.Add("In_TotalNet", OracleType.Number);
                    cmd.Parameters["In_TotalNet"].Value = 0;
                }
                else
                {
                    cmd.Parameters.Add("In_TotalNet", OracleType.Number);
                    cmd.Parameters["In_TotalNet"].Value = ht["TotalNet"];
                }

                if (ht["insertBy"] == null || ht["insertBy"] == "")
                {
                    cmd.Parameters.Add("IN_PISTRANS_INSBY", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_INSBY"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_PISTRANS_INSBY", OracleType.VarChar);
                    cmd.Parameters["IN_PISTRANS_INSBY"].Value = ht["insertBy"];

                }

                if (ht["fauttype"] == null || ht["fauttype"] == "")
                {
                    cmd.Parameters.Add("in_faulttype", OracleType.VarChar);
                    cmd.Parameters["in_faulttype"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("in_faulttype", OracleType.VarChar);
                    cmd.Parameters["in_faulttype"].Value = ht["fauttype"];

                }

                cmd.Parameters.Add("in_boxtype", OracleType.VarChar);
                cmd.Parameters["in_boxtype"].Value = ht["boxtype"];
                cmd.Parameters.Add("in_type", OracleType.VarChar);
                cmd.Parameters["in_type"].Value = ht["type"];
                if (ht["MobileNo"] == null || ht["MobileNo"] == "")
                {
                    cmd.Parameters.Add("in_mobileno", OracleType.VarChar);
                    cmd.Parameters["in_mobileno"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("in_mobileno", OracleType.VarChar);
                    cmd.Parameters["in_mobileno"].Value = ht["MobileNo"];
                }

                cmd.Parameters.Add("in_ChqDate", OracleType.DateTime);
                cmd.Parameters["in_ChqDate"].Value = ht["ChequeDate"];

                if (ht["AccessString"] == null || ht["AccessString"] == "")
                {
                    cmd.Parameters.Add("IN_Accessstr", OracleType.VarChar);
                    cmd.Parameters["IN_Accessstr"].Value = DBNull.Value;

                }
                else
                {
                    cmd.Parameters.Add("IN_Accessstr", OracleType.VarChar);
                    cmd.Parameters["IN_Accessstr"].Value = ht["AccessString"];

                }

                cmd.Parameters.Add("Out_ErrorCode", OracleType.Number);
                cmd.Parameters["Out_ErrorCode"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("Out_ErrorMsg", OracleType.VarChar, 1000);
                cmd.Parameters["Out_ErrorMsg"].Direction = ParameterDirection.Output;

                ConObj.Open();
                cmd.ExecuteNonQuery();

                int exeResult = Convert.ToInt32(cmd.Parameters["Out_ErrorCode"].Value);
                string ExeResultMsg = Convert.ToString(cmd.Parameters["out_ErrorMsg"].Value);
                return exeResult + "$" + ExeResultMsg;

                ConObj.Dispose();

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["UserName"].ToString(), ex.Message.ToString(), "frmNewSTB.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }


        }

        /*-------- RP on 07.09.2017 -------- */
        public string[] BindSchemDetails(string SchemeID, string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str += " SELECT num_scheme_id, var_scheme_name SchemeName, var_scheme_description Scheme_Description,case when termination_allowed ='Y' then 'Yes' ";
                str += " when termination_allowed='N' then 'No'  end termination_allowed,penalty_on_foreclosure penalty, ";
                str += " case when plan_change_allowed ='Y' then 'Yes' when plan_change_allowed='N' then 'No' end plan_change_allowed, ";
                str += " case when activation_allowed ='Y' then 'Yes' when activation_allowed='N' then 'No' end activation_allowed,";
                str += " case when num_subscription_payterm ='1' then '1 Month' when num_subscription_payterm='3' then '3 Month' when num_subscription_payterm='6' then '6 Month' ";
                str += " when num_subscription_payterm='12' then '12 Month' end  num_subscription_payterm, var_subscription_plan,";
                str += " num_subscription_lcorate LcoRate, subscription_discount_lcorate LcoDiscount,    subscription_net_lcorate LcoNet, max_stb_quanity_allowed StbCount, var_stb_status StbStatus,";
                str += " var_user_boxtype BoxType, var_stb_makemodel Model, num_scheme_value StbRate,num_stb_discount StbDiscount, num_stb_netamount stbNet FROM  aoup_lcopre_scheme_master";
                str += " where num_scheme_id='" + SchemeID + "'";
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    Operators.Add(dr["SchemeName"].ToString());
                    Operators.Add(dr["Scheme_Description"].ToString());
                    Operators.Add(dr["termination_allowed"].ToString());
                    Operators.Add(dr["penalty"].ToString());
                    Operators.Add(dr["plan_change_allowed"].ToString());
                    Operators.Add(dr["activation_allowed"].ToString());
                    Operators.Add(dr["num_subscription_payterm"].ToString());
                    Operators.Add(dr["var_subscription_plan"].ToString());
                    Operators.Add(dr["lcorate"].ToString());
                    Operators.Add(dr["lcodiscount"].ToString());
                    Operators.Add(dr["lconet"].ToString());
                    Operators.Add(dr["stbcount"].ToString());
                    Operators.Add(dr["stbstatus"].ToString());
                    Operators.Add(dr["boxtype"].ToString());
                    Operators.Add(dr["model"].ToString());
                    Operators.Add(dr["stbrate"].ToString());
                    Operators.Add(dr["stbdiscount"].ToString());
                    Operators.Add(dr["stbnet"].ToString());
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayLcoPayment.cs-GetLcoDetails");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

    }
}
