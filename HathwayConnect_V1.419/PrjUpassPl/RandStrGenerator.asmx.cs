using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;

namespace PrjUpassPl
{
    /// <summary>
    /// Summary description for RandStrGenerator
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class RandStrGenerator : System.Web.Services.WebService
    {

        [WebMethod]
        public string GenerateRandStr(string Name)
        {
            string strConn = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(strConn);
            conn.Open();

            OracleCommand oracmd = new OracleCommand("aoup_yesbanksec_randomstr", conn);
            oracmd.CommandType = CommandType.StoredProcedure;

            oracmd.Parameters.Add("in_username", OracleType.VarChar, 50);
            oracmd.Parameters["in_username"].Value = Name.ToString().Trim();

            oracmd.Parameters.Add("out_errcode", OracleType.Number);
            oracmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

            oracmd.Parameters.Add("out_radstr", OracleType.VarChar, 20);
            oracmd.Parameters["out_radstr"].Direction = ParameterDirection.Output;


            oracmd.ExecuteNonQuery();
            conn.Close();

            int errcode = Convert.ToInt32(oracmd.Parameters["out_errcode"].Value);
            string random = oracmd.Parameters["out_radstr"].Value.ToString();
            return random.ToString();

        }
    }
}
