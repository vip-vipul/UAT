using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace PrjUpassDAL.Transaction
{
    public class cls_data_rcptpis
    {
        public string getrcptData(string username, string transid)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_fetchtransdetails", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_UserId", OracleType.VarChar, 100);
                Cmd.Parameters["in_UserId"].Value = username;

                Cmd.Parameters.Add("in_transid", OracleType.VarChar);
                Cmd.Parameters["in_transid"].Value = transid;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 1000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_ErrorCode", OracleType.Number);
                Cmd.Parameters["out_ErrorCode"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_ErrorMsg", OracleType.VarChar, 100);
                Cmd.Parameters["out_ErrorMsg"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();

                string data = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string code = Convert.ToString(Cmd.Parameters["out_ErrorCode"].Value);
                string msg = Convert.ToString(Cmd.Parameters["out_ErrorMsg"].Value);


                return data + "~" + code;
            }
            catch (Exception ex)
            {
                Cls_Security ob = new Cls_Security();
                ob.InsertIntoDb(username, ex.ToString(), "cls_data_rcptpis.cs");

                return "-300";
            }


        }
    }
}
