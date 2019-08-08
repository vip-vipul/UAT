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
    public class Cls_Data_MstLcoOtherDetails
    {
        public string setLcoData(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_otherlco_updt", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocode"].Value = ht["lcocode"];

                Cmd.Parameters.Add("in_areamanager", OracleType.VarChar, 100);
                Cmd.Parameters["in_areamanager"].Value = ht["areamanager"];

                Cmd.Parameters.Add("in_ptexpirydt", OracleType.DateTime);
                Cmd.Parameters["in_ptexpirydt"].Value = ht["ptexdt"];

                Cmd.Parameters.Add("in_intagreexpirydt", OracleType.DateTime);
                Cmd.Parameters["in_intagreexpirydt"].Value = ht["intagreeexpdt"];

                Cmd.Parameters.Add("in_executive", OracleType.VarChar, 100);
                Cmd.Parameters["in_executive"].Value = ht["executive"];

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
                        Str = " Lco Details Updated successfully...";                    
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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLcoOtherDetails.cs-setLcoData");
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
