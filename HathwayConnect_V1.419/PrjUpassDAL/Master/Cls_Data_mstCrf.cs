using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;

namespace PrjUpassDAL.Master
{
    public class Cls_Data_mstCrf
    {
        public String GetLcoCode(string Operid)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                String Lcocode = "";
                string str = " select var_lcomst_code from aoup_lcopre_lco_det a where a.num_lcomst_operid= " + Operid;

                OracleCommand cmd = new OracleCommand(str, conObj);

                conObj.Open();

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Lcocode = dr["var_lcomst_code"].ToString(); ;
                }
                conObj.Close();
                return Lcocode;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string FetchCustomerDetail(String username, String Accno, String SearchFlag)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());

            try
            {

                ConObj.Open();


                OracleCommand cmd = new OracleCommand("aoup_lcopre_mob_crf_cust", ConObj);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("in_username", OracleType.VarChar);
                cmd.Parameters.Add("in_accountno", OracleType.VarChar);
                cmd.Parameters.Add("in_SearchFlag", OracleType.VarChar);
                cmd.Parameters.Add("out_data", OracleType.VarChar, 2000);
                cmd.Parameters["out_data"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("out_errcode", OracleType.Number);
                cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                cmd.Parameters["in_username"].Value = username;
                cmd.Parameters["in_accountno"].Value = Accno;
                cmd.Parameters["in_SearchFlag"].Value = SearchFlag;


                cmd.ExecuteNonQuery();
                ConObj.Close();

                string Str;
                Str = Convert.ToString(cmd.Parameters["out_errcode"].Value) + "$" + cmd.Parameters["out_data"].Value;


                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_Frmhwaydefentry-callprocedurefrm");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string getDocDetails(string username)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_crf_idproof_fetch", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;


                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;
                Str = Convert.ToString(Cmd.Parameters["out_data"].Value);

                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_ECAFCustdet");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();

            }
        }

        public string GetAdddeatils(string username, String selectionfalg, string Entity)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);

            try
            {
                conObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_hwcrf_entity_web_fetch", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;


                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_selectionflag", OracleType.VarChar);
                Cmd.Parameters["in_selectionflag"].Value = selectionfalg;

                Cmd.Parameters.Add("in_entity", OracleType.VarChar);
                Cmd.Parameters["in_entity"].Value = Entity;


                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 32767);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_state", OracleType.VarChar, 32767);
                 Cmd.Parameters["out_state"].Direction = ParameterDirection.Output;
                

                Cmd.ExecuteNonQuery();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                String State = Convert.ToString(Cmd.Parameters["out_state"].Value);
                return exeData + "*" + State;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_ECAFCustdet");
                return "-1000$ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string CheckBasicPlanType(string username, String planpoid)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);

            try
            {
                conObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_hwcrf_check_basetype", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;


                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_planpoid", OracleType.VarChar);
                Cmd.Parameters["in_planpoid"].Value = planpoid;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 32767);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;


                Cmd.ExecuteNonQuery();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                return exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_ECAFCustdet");
                return "-1000$ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string genOTPNEwCust(string username, string Mobile)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_hwcrf_otp_gen_newcust", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_mobile", OracleType.VarChar, 100);
                Cmd.Parameters["in_mobile"].Value = Mobile;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string StrotpId;
                StrotpId = Convert.ToString(Cmd.Parameters["out_data"].Value);


                ConObj.Dispose();
                return StrotpId;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_ECAFCustdet");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();

            }
        }

        public string ValidateOTPNEwCust(string username, string otpid, string otp)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_hwcrf_otp_valid_newcust", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_otpid", OracleType.VarChar, 100);
                Cmd.Parameters["in_otpid"].Value = otpid;

                Cmd.Parameters.Add("in_OTP", OracleType.VarChar, 100);
                Cmd.Parameters["in_OTP"].Value = otp;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeResult = Convert.ToString(Cmd.Parameters["out_errcode"].Value); //9999 validated
                string exedatat = Convert.ToString(Cmd.Parameters["out_data"].Value); //9999 validated

                ConObj.Dispose();
                return exeResult+"$"+exedatat;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_ECAFCustdet");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();

            }
        }

        public string GetPlanDetails(string username,String TVType,String SDHD,int Payterm,string VCNO,string STBNO)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_crf_pkg_web_fetch_new", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_TVType", OracleType.VarChar);
                Cmd.Parameters["in_TVType"].Value = TVType;

                Cmd.Parameters.Add("in_Payterm", OracleType.Number);
                Cmd.Parameters["in_Payterm"].Value = Payterm;

                Cmd.Parameters.Add("in_SDHD", OracleType.VarChar);
                Cmd.Parameters["in_SDHD"].Value = SDHD;

                Cmd.Parameters.Add("in_STBNO", OracleType.VarChar);
                Cmd.Parameters["in_STBNO"].Value = STBNO;

                if (VCNO == "")
                {
                    Cmd.Parameters.Add("in_VCID", OracleType.VarChar);
                    Cmd.Parameters["in_VCID"].Value = DBNull.Value;
                }
                else
                {
                    Cmd.Parameters.Add("in_VCID", OracleType.VarChar);
                    Cmd.Parameters["in_VCID"].Value = VCNO;
                }

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 20000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;
                Str = Convert.ToString(Cmd.Parameters["out_data"].Value);

                if (exeResult == 9999)
                {

                   Str = Convert.ToString(Cmd.Parameters["out_data"].Value);

                }
                else
                {
                    Str = "";
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_ECAFCustdet");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();

            }
        }

        public string InsertCRFData(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_ecaf_cust_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_custtype", OracleType.VarChar);
                Cmd.Parameters["in_custtype"].Value = ht["custtype"];

                Cmd.Parameters.Add("in_appicanttit", OracleType.VarChar);
                Cmd.Parameters["in_appicanttit"].Value = ht["appicanttit"];

                Cmd.Parameters.Add("in_fname", OracleType.VarChar);
                Cmd.Parameters["in_fname"].Value = ht["fname"];

                Cmd.Parameters.Add("in_lname", OracleType.VarChar);
                Cmd.Parameters["in_lname"].Value = ht["lname"];

                Cmd.Parameters.Add("in_mobno", OracleType.VarChar);
                Cmd.Parameters["in_mobno"].Value = ht["mob"];

                Cmd.Parameters.Add("in_landline", OracleType.VarChar);
                Cmd.Parameters["in_landline"].Value = ht["landline"];

                Cmd.Parameters.Add("in_emailid", OracleType.VarChar);
                Cmd.Parameters["in_emailid"].Value = ht["email"];

                Cmd.Parameters.Add("in_pin", OracleType.VarChar);
                Cmd.Parameters["in_pin"].Value = ht["pin"];

                Cmd.Parameters.Add("in_street", OracleType.VarChar);
                Cmd.Parameters["in_street"].Value = ht["street"];

                Cmd.Parameters.Add("in_city", OracleType.VarChar);
                Cmd.Parameters["in_city"].Value = ht["city"];

                Cmd.Parameters.Add("in_location", OracleType.VarChar);
                Cmd.Parameters["in_location"].Value = ht["location"];

                Cmd.Parameters.Add("in_area", OracleType.VarChar);
                Cmd.Parameters["in_area"].Value = ht["area"];

                Cmd.Parameters.Add("in_building", OracleType.VarChar);
                Cmd.Parameters["in_building"].Value = ht["building"];

                Cmd.Parameters.Add("in_flatno", OracleType.VarChar);
                Cmd.Parameters["in_flatno"].Value = ht["flatno"];

                Cmd.Parameters.Add("in_stbno", OracleType.VarChar);
                Cmd.Parameters["in_stbno"].Value = ht["stbno"];


                Cmd.Parameters.Add("in_vcno", OracleType.VarChar);
                Cmd.Parameters["in_vcno"].Value = ht["vcno"];

                Cmd.Parameters.Add("in_accno", OracleType.VarChar);
                Cmd.Parameters["in_accno"].Value = ht["accno"];

                Cmd.Parameters.Add("in_add", OracleType.VarChar);
                Cmd.Parameters["in_add"].Value = ht["add"];

                Cmd.Parameters.Add("in_idproofimage", OracleType.VarChar);
                Cmd.Parameters["in_idproofimage"].Value = ht["idproofimage"];

                Cmd.Parameters.Add("in_idvalue", OracleType.VarChar, 100);
                Cmd.Parameters["in_idvalue"].Value = ht["idvalue"];

                Cmd.Parameters.Add("in_macid", OracleType.VarChar, 100);
                Cmd.Parameters["in_macid"].Value = ht["macid"];

                Cmd.Parameters.Add("in_photoimage", OracleType.VarChar, 100);
                Cmd.Parameters["in_photoimage"].Value = ht["photoimage"];

                Cmd.Parameters.Add("in_resivalue", OracleType.VarChar, 100);
                Cmd.Parameters["in_resivalue"].Value = ht["resivalue"];

                Cmd.Parameters.Add("in_resiproof", OracleType.VarChar, 100);
                Cmd.Parameters["in_resiproof"].Value = ht["resiproof"];

                Cmd.Parameters.Add("in_signatureimage", OracleType.VarChar, 100);
                Cmd.Parameters["in_signatureimage"].Value = ht["signatureimage"];

                Cmd.Parameters.Add("in_state", OracleType.VarChar, 100);
                Cmd.Parameters["in_state"].Value = ht["state"];

                Cmd.Parameters.Add("in_Child", OracleType.VarChar, 100);
                Cmd.Parameters["in_Child"].Value = ht["Child"];

                Cmd.Parameters.Add("in_resiproofvalue", OracleType.VarChar, 100);
                Cmd.Parameters["in_resiproofvalue"].Value = ht["resiproofvalue"];

                Cmd.Parameters.Add("in_idproofvalue", OracleType.VarChar, 100);
                Cmd.Parameters["in_idproofvalue"].Value = ht["idproofvalue"];

                Cmd.Parameters.Add("in_cafno", OracleType.VarChar, 100);
                Cmd.Parameters["in_cafno"].Value = ht["cafno"];

                Cmd.Parameters.Add("in_ip", OracleType.VarChar, 100);
                Cmd.Parameters["in_ip"].Value = ht["ip"];
                
                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;
                Str = Convert.ToString(Cmd.Parameters["out_data"].Value);

                if (exeResult == 9999)
                {

                    //Str = "Success";

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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_ECAFCustdet");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();

            }
        }

        public string Validatenewcust(string username, string STB, string VC)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_crf_validate_new_cust", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_vc", OracleType.VarChar, 100);
                Cmd.Parameters["in_vc"].Value = VC;

                Cmd.Parameters.Add("in_stb", OracleType.VarChar, 100);
                Cmd.Parameters["in_stb"].Value = STB;


                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeResult = Convert.ToString(Cmd.Parameters["out_errcode"].Value); //9999 validated
                string exedatat = Convert.ToString(Cmd.Parameters["out_data"].Value); //9999 validated

                ConObj.Dispose();
                return exeResult + "$" + exedatat;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_ECAFCustdet");
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
