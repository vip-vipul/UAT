using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.OracleClient;
using PrjUpassDAL.Helper;
using System.Data;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_TransOsdBmailNotification
    {
        public string SaveOsdBMail(string username, string data, string dtfrom, string dtto)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_notification_map", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

           

                Cmd.Parameters.Add("in_data", OracleType.VarChar);
                Cmd.Parameters["in_data"].Value = data;


                Cmd.Parameters.Add("in_from", OracleType.VarChar);
                Cmd.Parameters["in_from"].Value = dtfrom;


                Cmd.Parameters.Add("in_to", OracleType.VarChar);
                Cmd.Parameters["in_to"].Value =dtto;


                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                return exeCode + "$" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransOsdBmailNotification-SaveOsdBMail");
                return "-1000$ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        public string SaveOsdBMailActivation(string username, string reference, string data)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_notif_Flag_updt", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_referenceid", OracleType.VarChar, 100);
                Cmd.Parameters["in_referenceid"].Value = reference;

                Cmd.Parameters.Add("in_data", OracleType.VarChar);
                Cmd.Parameters["in_data"].Value = data;


                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                return exeCode + "$" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransOsdBmailNotification-SaveOsdBMail");
                return "-1000$ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string SaveOsdBMailbULK(string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                conObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_notification_bulk", conObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();

                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                return exeCode + "$" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransOsdBmailNotification-SaveOsdBMailbULK");
                return "-1000$ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        
        public DataTable getMessage(string username, string lco, string type)
        {
            Cls_Helper objHelper = new Cls_Helper();
            try
            {
                string strQry = "SELECT * FROM view_lcopre_notification a WHERE a.lcoid = " + lco + " and type='" + type + "'";
                return objHelper.GetDataTable(strQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransOsdBmailNotification.cs-getMessage");
                return null;
            }
        }
    }
}
