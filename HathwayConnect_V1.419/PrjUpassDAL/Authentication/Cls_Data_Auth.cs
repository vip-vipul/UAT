using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.OracleClient;
using System.Configuration;
using System.Data;
using PrjUpassDAL.Helper;
namespace PrjUpassDAL.Authentication
{
    public class Cls_Data_Auth
    {
        public Hashtable Authentication(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
               OracleCommand Cmd = new OracleCommand("aoup_lcopre_login", ConObj);
                
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = ht["username"];

                Cmd.Parameters.Add("in_password", OracleType.VarChar);
                Cmd.Parameters["in_password"].Value = ht["password"];

                Cmd.Parameters.Add("in_IP", OracleType.VarChar);
                Cmd.Parameters["in_IP"].Value = ht["IP"];

                Cmd.Parameters.Add("in_siteflag", OracleType.VarChar);
                Cmd.Parameters["in_siteflag"].Value = "Y";

                Cmd.Parameters.Add("in_loginflag", OracleType.VarChar);
                Cmd.Parameters["in_loginflag"].Value = "Y";

                Cmd.Parameters.Add("out_userid", OracleType.Number);
                Cmd.Parameters["out_userid"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_useroperid", OracleType.Number);
                Cmd.Parameters["out_useroperid"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_useropercat", OracleType.Number);
                Cmd.Parameters["out_useropercat"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_useropername", OracleType.VarChar, 250);
                Cmd.Parameters["out_useropername"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_userbrmpoid", OracleType.VarChar, 250);
                Cmd.Parameters["out_userbrmpoid"].Direction = ParameterDirection.Output;

                // not present in actual login procedure
                Cmd.Parameters.Add("out_last_login", OracleType.VarChar, 250);
                Cmd.Parameters["out_last_login"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_loginflag", OracleType.VarChar, 250);
                Cmd.Parameters["out_loginflag"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errmsg", OracleType.VarChar, 300);
                Cmd.Parameters["out_errmsg"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string user_id = Cmd.Parameters["out_userid"].Value.ToString();
                string user_brmpoid = Cmd.Parameters["out_userbrmpoid"].Value.ToString();
                string user_operid = Cmd.Parameters["out_useroperid"].Value.ToString();
                string user_opercat = Cmd.Parameters["out_useropercat"].Value.ToString();
                string user_name = Cmd.Parameters["out_useropername"].Value.ToString();
                string last_login = Cmd.Parameters["out_last_login"].Value.ToString();
                string login_flag = Cmd.Parameters["out_loginflag"].Value.ToString();
                string errcode = Cmd.Parameters["out_errcode"].Value.ToString();
                string errmsg = Cmd.Parameters["out_errmsg"].Value.ToString();

                Hashtable AuthResponce = new Hashtable();
                AuthResponce["user_id"] = user_id;
                AuthResponce["user_brmpoid"] = user_brmpoid;
                AuthResponce["operator_id"] = user_operid;
                AuthResponce["user_operator_category"] = user_opercat;
                AuthResponce["user_name"] = user_name;
                AuthResponce["last_login"] = last_login;
                AuthResponce["login_flag"] = login_flag;
                AuthResponce["response_code"] = errcode;
                AuthResponce["response_msg"] = errmsg;
                AuthResponce["MIAflag"] = errmsg;

                ConObj.Dispose();
                return AuthResponce;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cls_Data_Auth.cs-Authentication");

                Hashtable AuthResponce = new Hashtable();
                AuthResponce["ex_ocuured"] = ex.Message.ToString();
                return AuthResponce;
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string SessionLog(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_sessionlog_ins", ConObj);

                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = ht["username"];

                Cmd.Parameters.Add("in_sessionid", OracleType.VarChar);
                Cmd.Parameters["in_sessionid"].Value = ht["sessionid"];

                Cmd.Parameters.Add("in_pagename", OracleType.VarChar);
                Cmd.Parameters["in_pagename"].Value = ht["pagename"];

                Cmd.Parameters.Add("in_name", OracleType.VarChar);
                Cmd.Parameters["in_name"].Value = ht["name"];

                Cmd.Parameters.Add("in_ip", OracleType.VarChar);
                Cmd.Parameters["in_ip"].Value = ht["IP"];


                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string errcode = Cmd.Parameters["out_errcode"].Value.ToString();


                ConObj.Dispose();
                return errcode;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["username"].ToString(), ex.Message.ToString(), "Cls_Data_Auth.cs-sessionlog");

                return "";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
        /*
        public Hashtable Authentication(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            ConObj.Open();
            OracleCommand Cmd = new OracleCommand("aoup_login_ins_new_hash", ConObj);
            Cmd.CommandType = CommandType.StoredProcedure;

            Cmd.Parameters.Add("in_username", OracleType.VarChar);
            Cmd.Parameters["in_username"].Value = ht["username"];

            Cmd.Parameters.Add("in_password", OracleType.VarChar);
            Cmd.Parameters["in_password"].Value = ht["password"];

            Cmd.Parameters.Add("in_siteflag", OracleType.VarChar);
            Cmd.Parameters["in_siteflag"].Value = "Y";

            Cmd.Parameters.Add("out_userid", OracleType.Number);
            Cmd.Parameters["out_userid"].Direction = ParameterDirection.Output;

            Cmd.Parameters.Add("out_useroperid", OracleType.Number);
            Cmd.Parameters["out_useroperid"].Direction = ParameterDirection.Output;

            Cmd.Parameters.Add("out_useropercat", OracleType.Number);
            Cmd.Parameters["out_useropercat"].Direction = ParameterDirection.Output;

            Cmd.Parameters.Add("out_useropername", OracleType.VarChar);
            Cmd.Parameters["out_useropername"].Direction = ParameterDirection.Output;

            Cmd.Parameters.Add("out_errcode", OracleType.Number);
            Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

            Cmd.ExecuteNonQuery();
            ConObj.Close();

            string user_id = Cmd.Parameters["out_userid"].Value.ToString();
            string user_operid = Cmd.Parameters["out_useroperid"].Value.ToString();
            string user_opercat = Cmd.Parameters["out_useropercat"].Value.ToString();
            string user_name = Cmd.Parameters["out_useropername"].Value.ToString();
            string out_errcode = Cmd.Parameters["out_errcode"].Value.ToString();

            Hashtable AuthResponce = new Hashtable();
            AuthResponce["user_id"] = user_id;
            AuthResponce["operator_id"] = user_operid;
            AuthResponce["user_operator_category"] = user_opercat;
            AuthResponce["user_name"] = user_name;
            AuthResponce["response_code"] = out_errcode;

            ConObj.Dispose();
            return AuthResponce;
        }
        */

        public string getbroadcastmsg()
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_broadmsg_display", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 1000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string errmsg = Cmd.Parameters["out_data"].Value.ToString();

                ConObj.Dispose();
                return errmsg;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb("admin", ex.Message.ToString(), "Cls_Data_Auth.cs-getbroadcastmsg");

                Hashtable AuthResponce = new Hashtable();
                AuthResponce["ex_ocuured"] = ex.Message.ToString();
                return "";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string GetIPAddress(HttpRequest request)
        {
            string ip;
            try
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    if (ip.IndexOf(",") > 0)
                    {
                        string[] ipRange = ip.Split(',');
                        int le = ipRange.Length - 1;
                        ip = ipRange[le];
                    }
                }
                else
                {
                    ip = request.UserHostAddress;
                }
            }
            catch { ip = ""; }

            return ip;
        }

        public bool checkRights(string userid, string uname, string page)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            ConObj.Open();
            try
            {
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_rightscheck", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = uname.Trim();

                Cmd.Parameters.Add("in_userid", OracleType.Number, 20);
                Cmd.Parameters["in_userid"].Value = userid.Trim();

                Cmd.Parameters.Add("in_page", OracleType.VarChar, 100);
                Cmd.Parameters["in_page"].Value = page.Trim();

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();

                int errcode = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);

                if (errcode == 9999)
                {
                    return true; //user has access
                }
                else
                {
                    return false; //user has does not have access
                }
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(uname, ex.Message.ToString(), "Cls_Data_Auth-checkRights");
                return false;
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string SendAndroidUrl(string UserName,string NewContact)
        {
            string strConn = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection oraConn = new OracleConnection(strConn);
            try
            {
                oraConn.Open();
                OracleCommand cmd1 = new OracleCommand("aoup_lcopre_send_android_apk", oraConn);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.Add("in_username", OracleType.VarChar);
                cmd1.Parameters["in_username"].Value = UserName;
                if (NewContact != "")
                {
                    cmd1.Parameters.Add("in_ContactNo", OracleType.VarChar);
                    cmd1.Parameters["in_ContactNo"].Value = NewContact;
                }
                else
                {
                    cmd1.Parameters.Add("in_ContactNo", OracleType.VarChar);
                    cmd1.Parameters["in_ContactNo"].Value = DBNull.Value;
                }
                cmd1.Parameters.Add("out_errcode", OracleType.Number);
                cmd1.Parameters["out_errcode"].Value = ParameterDirection.Output;

                cmd1.Parameters.Add("out_data", OracleType.VarChar, 1000);
                cmd1.Parameters["out_data"].Direction = ParameterDirection.Output;


                cmd1.ExecuteNonQuery();
                oraConn.Close();

                string errcode = cmd1.Parameters["out_errcode"].Value.ToString() + "$" + cmd1.Parameters["out_data"].Value.ToString();
                return errcode;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(UserName, ex.Message.ToString(), "Cls_Data_Auth-SendAndroidUrl");
                return "-100$Error while sending android app";
            }
            finally
            {
                oraConn.Close();
                oraConn.Dispose();
            }

        }
        //---- Added By RP on 28.06.2017
        public void GetGSTNo(string LCOCODE, out string GSTNO)
        {
            GSTNO = "";
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
               // str = " select gstin from reports.lco_gstn_master@caslive_new where account_no= '" + LCOCODE + "'";
                str = "select * from reports.lco_gstn_master@caslive_new where account_no in(";
                str += "select var_usermst_username from aoup_lcopre_user_det where num_usermst_operid='" + LCOCODE + "' and var_usermst_flag='M')";
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    GSTNO = dr["gstin"].ToString();
                }
                conObj.Close();
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();

                objSecurity.InsertIntoDb(LCOCODE, ex.Message.ToString(), "Cls_Data_Auth-GetGSTNo");
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        public void GetAccepGST(string LCOCODE, out string lconame)
        {
            lconame = "";
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str = " select var_gst_lconame from aoup_lcopre_gst_acceptance left join aoup_user_def on num_user_operid=var_gst_operid where var_user_username='" + LCOCODE + "'";
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lconame = dr["var_gst_lconame"].ToString();
                }
                conObj.Close();
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();

                objSecurity.InsertIntoDb(LCOCODE, ex.Message.ToString(), "Cls_Data_Auth-GetAccepGST");
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        public string InsertLCAccept(string LcoCode, string  IPAdd)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_gstacceptance_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar).Direction = ParameterDirection.Input;
                Cmd.Parameters.Add("in_IP", OracleType.VarChar).Direction = ParameterDirection.Input;
                Cmd.Parameters.Add("out_errcode", OracleType.Number, 10).Direction = ParameterDirection.Output;
                Cmd.Parameters.Add("out_errmsg", OracleType.VarChar, 500).Direction = ParameterDirection.Output;

                Cmd.Parameters["in_username"].Value = LcoCode;
                Cmd.Parameters["in_IP"].Value = IPAdd;
                
                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string ExeResult = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string ExeResultMsg = Convert.ToString(Cmd.Parameters["out_errMsg"].Value);

                ConObj.Dispose();

                return ExeResult + "$" + ExeResultMsg;
            }

            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(LcoCode, ex.Message.ToString(), "Cls_Data_Auth-.cs");
                return "-310$ex_occured";
            }

            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
        public DataTable GetLCODetails(string username)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "        SELECT var_lcomst_code,var_lcomst_name,var_lcomst_address,var_lcomst_company,var_compconfig_localaddress" +
                "  FROM aoup_lcopre_lco_det,aoup_lcopre_compconfig_det WHERE var_compconfig_company = var_lcomst_company " +
        "AND var_lcomst_code = '" + username + "' ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_Auth.cs");
                return null;
            }
        }
      //----Added By RP on 28.09.2017
        public string LCOContactNo(string UserName)
        {
            try
            {
                string LCOContactNo = "";
                string str = " select to_char(num_lcomst_mobileno)  lcomobno from aoup_lcopre_lco_det a inner join aoup_user_def b on  num_user_operid=a.num_lcomst_operid where b.var_user_username='" + UserName + "'";
                string strConn = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
                OracleConnection oraConn = new OracleConnection(strConn);
                OracleCommand cmd = new OracleCommand(str, oraConn);
                oraConn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    LCOContactNo = dr["lcomobno"].ToString();
                }
                oraConn.Close();
                return LCOContactNo;
            }
            catch (Exception ex)
            {
                
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(UserName, ex.Message.ToString(), "Cls_Data_Auth-SendAndroidUrl");
                return ex.Message.ToString();
            }
        }

        //----
    }
}
