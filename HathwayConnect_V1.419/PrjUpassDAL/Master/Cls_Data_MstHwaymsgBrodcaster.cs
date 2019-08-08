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
  public  class Cls_Data_MstHwaymsgBrodcaster
    {
        public string SetBrodcastermsg(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_broadmsg_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 10);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_message", OracleType.VarChar, 4000);
                Cmd.Parameters["in_message"].Value = ht["Brodcastermsg"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar, 100);
                Cmd.Parameters["in_flag"].Value = ht["msgFlag"];

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
                    Str = " Brodcaster Message updated successfully...";
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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstHwaymsgBrdr-SetBrodcastermsg");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string GetLCOBrodcasterMsg(string username, string oper_id)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_lcowsmsg_fetch", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_operid", OracleType.VarChar, 100);
                Cmd.Parameters["in_operid"].Value = oper_id;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 4000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeResult = Convert.ToString(Cmd.Parameters["out_data"].Value);

                ConObj.Dispose();
                return exeResult.Trim();
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstHwaymsgBrdr-GetLCOBrodcasterMsg");
                return "";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string SetLCOBrodcasterMsg(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_lcowsmsg_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 10);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_message", OracleType.VarChar, 4000);
                Cmd.Parameters["in_message"].Value = ht["message"];

                Cmd.Parameters.Add("in_flag", OracleType.VarChar, 100);
                Cmd.Parameters["in_flag"].Value = ht["flag"];

                Cmd.Parameters.Add("in_operid", OracleType.Number);
                Cmd.Parameters["in_operid"].Value = ht["operator"];

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
                    Str = " Brodcaster Message updated successfully...";
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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstHwaymsgBrdr-SetLCOBrodcasterMsg");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public DataTable getMIAdata(string username, string lcocode)
        {
            DataTable dtdata = new DataTable();
            try
            {

                string _getdata = "select * from VIEW_LCO_MIA_DET where lcocode= '" + lcocode + "'";
                Cls_Helper ob = new Cls_Helper();
                return dtdata = ob.GetDataTable(_getdata);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "getMIAdata");
                return dtdata;

            }
        }
    }
}
