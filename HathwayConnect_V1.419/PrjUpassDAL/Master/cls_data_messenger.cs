using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Configuration;
using System.Data.OracleClient;
using System.Collections;

namespace PrjUpassDAL.Master
{
    public class cls_data_messenger
    {

        public void GetCity(string username, out string city, out string State, out string Userlevel, out string cityuser)
        {
            city = "";
            Userlevel = "";
            State = "";
            cityuser = "";
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str = " select nvl(VAR_USERMST_MSGCITYSTR,num_usermst_cityid) cityid,VAR_USERMST_MSGLEVEL,num_usermst_stateid,num_usermst_cityid from aoup_lcopre_user_det where var_usermst_username= '" + username + "'";
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    city = dr["cityid"].ToString();
                    Userlevel = dr["VAR_USERMST_MSGLEVEL"].ToString();
                    State = dr["num_usermst_stateid"].ToString();
                    cityuser = dr["num_usermst_cityid"].ToString();
                }
                conObj.Close();
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_data_messenger-GetCity");

            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public DataTable fillinbox(string username, string city, string Userlevel, string state, string from, string subject)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string _getIndata = "select * from view_lcopre_inbox_messsage ";
                _getIndata += "   where lco_code like ('%" + username + "%') and mfrom not like ('%" + username + "%') and INBOXDELETE not like ('%" + username + ",%')   ";
                if (Userlevel == "ST")
                {
                    _getIndata += " and state_id=" + state;
                }

                if (from != "" && subject != "")
                {
                    _getIndata += " and upper(mfrom) like '%" + from.ToUpper() + "%' and upper(msub) like '%" + subject.ToUpper() + "%'";
                }
                else if (from != "")
                {
                    _getIndata += " and upper(mfrom) like '%" + from.ToUpper() + "%'";
                }
                else if (subject != "")
                {
                    _getIndata += " and upper(msub) like '%" + subject.ToUpper() + "%'";
                }
                else
                {
                    _getIndata += " ";
                }

                _getIndata += " order by mdate desc";

                Cls_Helper ob = new Cls_Helper();

                return ob.GetDataTable(_getIndata);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_data_messenger.cs-fillinbox");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }

        }
        public DataTable fillsentMsgs(string username, string from, string subject)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string _getIndata = "select  a.num_messenger_id mid , a.num_msgto_subconvstnid msubid, a.var_messenger_subject msub, UTL_RAW.CAST_TO_VARCHAR2(a.var_messenger_message) mmsg, " +
" a.var_messenger_from mfrom, " +
" Case when var_messenger_toall='CN' then 'All Country LCO'  " +
" when var_messenger_toall='ST' then 'All '|| var_state_name ||'State LCO' " +
" when var_messenger_toall='CT' then 'All '|| var_city_name ||'City LCO' else  a.var_messenger_to end mto , " +
 " a.var_messenger_file mfile, a.dat_messenger_date mdate, a.var_messenger_insby mins, " +
 " a.dat_messenger_insdate minsdate, a.var_messenger_updby mupby, a.var_messenger_upddate mudate,  a.var_messenger_type mtype, " +  //UTL_RAW.CAST_TO_VARCHAR2(var_messenger_readby) readby,
 " var_messenger_toall msgtoall,var_messenger_toid msgtoallid,var_messenger_sentdelete sentdelete " +
 " from Aoup_Lcopre_Messenger a " +
 " left join  aoup_lcopre_state_def b on a.var_messenger_toid=b.num_state_id " +
 " left join aoup_lcopre_city_def c on a.var_messenger_toid=c.num_city_id " +
 " where a.var_messenger_from ='" + username + "' and nvl(var_messenger_sentdelete,',') not like ('%" + username + ",%') ";

                if (from != "" && subject != "")
                {
                    _getIndata += " and upper(a.var_messenger_to) like '%" + from.ToUpper() + "%' and upper(a.var_messenger_to) like '%" + from.ToUpper() + "%'";
                }
                else if (from != "")
                {
                    _getIndata += " and upper(a.var_messenger_to) like '%" + from.ToUpper() + "%'";
                }
                else if (subject != "")
                {
                    _getIndata += " and upper(a.var_messenger_subject) like '%" + subject.ToUpper() + "%'";
                }
                else
                {
                    _getIndata += " ";
                }

                _getIndata += "  order by a.dat_messenger_date desc ";

                Cls_Helper ob = new Cls_Helper();

                return ob.GetDataTable(_getIndata);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_data_messenger.cs-fillsentMsgs");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }

        }

        public string sentMailDatains(string username, Hashtable htsent)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();
                OracleCommand cmd = new OracleCommand("aoup_lcopre_messenger_data", conObj);   //procedure ins Sent Mail data 
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("in_username", OracleType.VarChar, 100); //
                cmd.Parameters["in_username"].Value = username;

                if (htsent["msgto"].ToString() == "")
                {
                    cmd.Parameters.Add("in_msgto", OracleType.VarChar, 4000);
                    cmd.Parameters["in_msgto"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("in_msgto", OracleType.VarChar, 4000);
                    cmd.Parameters["in_msgto"].Value = htsent["msgto"];
                }

                if (htsent["msgtoall"].ToString() == "")
                {
                    cmd.Parameters.Add("in_msgtoall", OracleType.VarChar, 100);
                    cmd.Parameters["in_msgtoall"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("in_msgtoall", OracleType.VarChar, 100);
                    cmd.Parameters["in_msgtoall"].Value = htsent["msgtoall"];
                }

                if (htsent["msgtoallID"].ToString() == "")
                {
                    cmd.Parameters.Add("in_msgtoID", OracleType.VarChar, 100);
                    cmd.Parameters["in_msgtoID"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("in_msgtoID", OracleType.VarChar, 100);
                    cmd.Parameters["in_msgtoID"].Value = htsent["msgtoallID"];
                }

                cmd.Parameters.Add("in_msgsubject", OracleType.VarChar, 1000);
                cmd.Parameters["in_msgsubject"].Value = htsent["msgsubject"];

                cmd.Parameters.Add("in_msgfile", OracleType.VarChar, 500);
                cmd.Parameters["in_msgfile"].Value = htsent["msgfile"];

                cmd.Parameters.Add("in_msgType", OracleType.VarChar, 100);
                cmd.Parameters["in_msgType"].Value = htsent["msgType"];

                cmd.Parameters.Add("in_msgContecnt", OracleType.VarChar, 1000);
                cmd.Parameters["in_msgContecnt"].Value = htsent["msgContecnt"];  //NewMail

                cmd.Parameters.Add("in_newMail", OracleType.VarChar, 1000);
                cmd.Parameters["in_newMail"].Value = htsent["NewMail"];  //NewMail

                if (htsent["mainid"] != null)
                {
                    cmd.Parameters.Add("in_mainid", OracleType.Number, 20);
                    cmd.Parameters["in_mainid"].Value = Convert.ToInt32(htsent["mainid"]);
                }
                else
                {
                    cmd.Parameters.Add("in_mainid", OracleType.Number, 20);
                    cmd.Parameters["in_mainid"].Value = 0;
                }

                if (htsent["subid"] != null)
                {
                    cmd.Parameters.Add("in_subid", OracleType.Number, 20);
                    cmd.Parameters["in_subid"].Value = Convert.ToInt32(htsent["subid"]);
                }
                else
                {
                    cmd.Parameters.Add("in_subid", OracleType.Number, 20);
                    cmd.Parameters["in_subid"].Value = 0;
                }

                if (htsent["msgtocc"] != null)
                {
                    cmd.Parameters.Add("in_msgtoCC", OracleType.VarChar, 4000);
                    cmd.Parameters["in_msgtoCC"].Value = htsent["msgtocc"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("in_msgtoCC", OracleType.VarChar, 4000);
                    cmd.Parameters["in_msgtoCC"].Value = DBNull.Value;
                }


                cmd.Parameters.Add("in_deletetype", OracleType.VarChar, 20);
                cmd.Parameters["in_deletetype"].Value = Convert.ToString(htsent["deletetype"]);

                cmd.Parameters.Add("in_city", OracleType.Number, 20);
                cmd.Parameters["in_city"].Value = Convert.ToInt32(htsent["city"]);

                cmd.Parameters.Add("in_state", OracleType.Number, 20);
                cmd.Parameters["in_state"].Value = Convert.ToInt32(htsent["state"]);

                cmd.Parameters.Add("in_deleteby", OracleType.VarChar, 4000);
                cmd.Parameters["in_deleteby"].Value = htsent["delete"];

                if (htsent["repliedby"] != null)
                {
                    cmd.Parameters.Add("in_replyby", OracleType.VarChar, 4000);
                    cmd.Parameters["in_replyby"].Value = htsent["repliedby"];
                }
                else
                {
                    cmd.Parameters.Add("in_replyby", OracleType.VarChar, 4000);
                    cmd.Parameters["in_replyby"].Value = DBNull.Value;
                }

                cmd.Parameters.Add("out_data", OracleType.VarChar, 4000);
                cmd.Parameters["out_data"].Direction = ParameterDirection.Output;


                cmd.Parameters.Add("out_errcode", OracleType.Number, 100);
                cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                string cd = Convert.ToString(cmd.Parameters["out_data"].Value);
                string result = Convert.ToString(cmd.Parameters["out_errcode"].Value);



                conObj.Close();
                return result;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_data_messenger-sentmaildatains");
                return ex.Message;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string OpenMail(string username, String MainID, String SubId, String Readby)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();
                OracleCommand cmd = new OracleCommand("aoup_lcopre_messenger_open", conObj);   //procedure ins Sent Mail data 
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("in_username", OracleType.VarChar, 100); //
                cmd.Parameters["in_username"].Value = username;

                cmd.Parameters.Add("in_mainid", OracleType.Number, 20);
                cmd.Parameters["in_mainid"].Value = Convert.ToInt32(MainID);

                cmd.Parameters.Add("in_subid", OracleType.Number, 20);
                cmd.Parameters["in_subid"].Value = Convert.ToInt32(SubId);

                cmd.Parameters.Add("in_Readby", OracleType.VarChar, 4000);
                cmd.Parameters["in_Readby"].Value = Convert.ToString(Readby);

                cmd.Parameters.Add("out_data", OracleType.VarChar, 4000);
                cmd.Parameters["out_data"].Direction = ParameterDirection.Output;


                cmd.Parameters.Add("out_errcode", OracleType.Number, 100);
                cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                string cd = Convert.ToString(cmd.Parameters["out_data"].Value);
                string result = Convert.ToString(cmd.Parameters["out_errcode"].Value);



                conObj.Close();
                return result;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_data_messenger-sentmaildatains");
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
