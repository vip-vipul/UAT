using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Configuration;
using System.Collections;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_TransHwayUserCreditLimit
    {
        public string[] GetUserDetails(string username, string category, string type, string operid, string searchId)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str += " SELECT a.userid, a.username, a.userowner, a.curentcreditlimit, ";
                str += "  a.mobileno, a.useroperid, a.operid, a.opercategory, a.parentid, ";
                str += " a.distid ";
                str += " FROM view_hway_userdetails a ";
                if (type == "0")
                {
                    str += " where upper(a.username) like upper('" + searchId.ToString() + "%')";
                }
                else if (type == "1")
                {
                    str += " where upper(a.userowner) like  upper('" + searchId.ToString() + "%')";
                }
                if (category == "2")
                {
                    str += " and a.parentid='" + operid.ToString() + "'  ";
                }
                else if (category == "5")
                {
                    str += " and a.distid='" + operid.ToString() + "'  ";
                }
                else if (category == "3")
                {
                    str += " and a.operid ='" + operid.ToString() + "'";
                }



                OracleCommand cmd = new OracleCommand(str, conObj);

                conObj.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(dr["username"].ToString());
                    Operators.Add(dr["userowner"].ToString());
                    Operators.Add(dr["mobileno"].ToString());
                    Operators.Add(dr["curentcreditlimit"].ToString());

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

        public string UserLimitRevarsal(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_usercrlimit_new", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = ht["UserName"];

                Cmd.Parameters.Add("in_lcoid", OracleType.Number);
                Cmd.Parameters["in_lcoid"].Value = ht["LcoId"];

                Cmd.Parameters.Add("in_userid", OracleType.VarChar, 100);
                Cmd.Parameters["in_userid"].Value = ht["UserId"];

                Cmd.Parameters.Add("in_amount", OracleType.Number);
                Cmd.Parameters["in_amount"].Value = ht["Amount"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar, 100);
                Cmd.Parameters["in_flag"].Value = ht["Flag"];

                Cmd.Parameters.Add("in_remark", OracleType.VarChar, 100);
                Cmd.Parameters["in_remark"].Value = ht["Remark"];

                Cmd.Parameters.Add("in_IP", OracleType.VarChar, 100);
                Cmd.Parameters["in_IP"].Value = ht["IP"];

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 250);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;

                if (exeResult == 9999)
                {
                    //Str = "USER credit limit assigning successfully...";
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                else
                {
                    //Str = "USER credit limit assigning successfully...";
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["user"].ToString(), ex.Message.ToString(), "Cls_Data_TransHwayUserCreditLimit.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }

        }

        //----------------------new page---------------------------

        public DataTable GetAllUsers(string username, string category, string operid)
        {
            try
            {
                Cls_Helper helperObj = new Cls_Helper();
                string str = "";
                str += " SELECT a.userid, a.username, a.userowner, a.curentcreditlimit, ";
                str += "  a.mobileno, a.useroperid, a.operid, a.opercategory, a.parentid, ";
                str += " a.distid , a.userblck ";
                str += " FROM view_hway_userdetails a ";
                str += " WHERE a.userid is not null ";
                if (category == "2")
                {
                    str += " and a.parentid='" + operid.ToString() + "'  ";
                }
                else if (category == "5")
                {
                    str += " and a.distid='" + operid.ToString() + "'  ";
                }
                else if (category == "3")
                {
                    str += " and a.operid ='" + operid.ToString() + "'";
                }
                else if (category == "11")
                {
                    str += " and a.operid ='" + operid.ToString() + "'";
                }
                DataTable dtUsers = helperObj.GetDataTable(str);
                return dtUsers;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHayUserCreditLimt-getAllUsrs");
                return null;
            }
        }
        public string[] GetAvailBal(string username, string category, string userid, string operator_id)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            string avail_bal = "0";
            string allocatedbalance = "0";
            string totalbalance = "0";
            string[] str = new string[1];
            str[0] = "ex_occured";
            try
            {
                string strQuery = " SELECT (NVL(a.num_crlimit_actuallimit,0) - NVL(a.num_crlimit_exelimit,0)) avail_bal,NVL(a.num_crlimit_actuallimit,0) totalbalance, NVL(a.num_crlimit_exelimit,0) allocatedbalance,NVL(a.num_crlimit_lco_newlimit,0) new_limit" +
                                  " FROM aoup_lcopre_crlimit_det a, aoup_lcopre_lco_det b " +
                                  " WHERE a.var_crlimit_lcocode=b.var_lcomst_code and b.num_lcomst_operid='" + operator_id + "' ";
                OracleCommand cmd = new OracleCommand(strQuery, conObj);
                conObj.Open();
                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //if (dr["avail_bal"].ToString() != "")
                    //{
                         Operators.Add(dr["avail_bal"].ToString());
                    //}
                    //else
                    //{
                    //    avail_bal = "0";
                    //}

                    Operators.Add(dr["totalbalance"].ToString());
                    Operators.Add(dr["allocatedbalance"].ToString());
                    Operators.Add(dr["new_limit"].ToString()); 

                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
                
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHayUserCreditLimt-GetAvailBal");
                return str;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string SetLimits(string username, string inpu_str, string oper_id, string Ip)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_user_crlimit_updt", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_str", OracleType.VarChar, 100);
                Cmd.Parameters["in_str"].Value = inpu_str.Trim();

                Cmd.Parameters.Add("in_operid", OracleType.VarChar, 100);
                Cmd.Parameters["in_operid"].Value = oper_id;

                Cmd.Parameters.Add("in_IP", OracleType.VarChar, 100);
                Cmd.Parameters["in_IP"].Value = Ip;

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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayUserCreditLimit-SetLimits");
                return "-300$Setting credit limit failed";

            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }


        public string userBlockUnblock(string username, Hashtable ht)   //aoup_lcopre_userblock_ins
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);

            try
            {
                ConObj.Open();
                OracleCommand cmd = new OracleCommand("aoup_lcopre_userblock_ins", ConObj);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.Add("in_userid", OracleType.VarChar, 100);
                cmd.Parameters["in_userid"].Value = ht["userid"];

                cmd.Parameters.Add("in_name", OracleType.VarChar, 100);
                cmd.Parameters["in_name"].Value = ht["username"];

                cmd.Parameters.Add("in_ip", OracleType.VarChar, 20);
                cmd.Parameters["in_ip"].Value = ht["ip"];

                cmd.Parameters.Add("in_status", OracleType.VarChar, 2);
                cmd.Parameters["in_status"].Value = ht["status"];

                cmd.Parameters.Add("in_Username", OracleType.VarChar, 100);
                cmd.Parameters["in_Username"].Value = username;

                cmd.Parameters.Add("out_errcode", OracleType.Number);
                cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("out_errdata", OracleType.VarChar, 100);
                cmd.Parameters["out_errdata"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                string _outdata = Convert.ToString(cmd.Parameters["out_errcode"].Value);

                return _outdata;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayUserCreditLimit-userBlockUnblock");
                return "exoccured -300";
            }

            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }



        }
    }
}
