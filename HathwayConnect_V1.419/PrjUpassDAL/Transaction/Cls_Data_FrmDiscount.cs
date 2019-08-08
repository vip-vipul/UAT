using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Collections;
using System.Configuration;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_FrmDiscount
    {
        OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());

        public string DiscountInsert(Hashtable ht)
        {

            ConObj.Open();
            try
            {
                OracleCommand cmd = new OracleCommand("aoup_lcopre_discount_ins", ConObj);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("in_account", OracleType.VarChar);
                cmd.Parameters["in_account"].Value = ht["AccNum"];

                cmd.Parameters.Add("in_vcid", OracleType.VarChar);
                cmd.Parameters["in_vcid"].Value = ht["vcid"];

                cmd.Parameters.Add("in_fname", OracleType.VarChar);
                cmd.Parameters["in_fname"].Value = ht["fname"];

                cmd.Parameters.Add("in_lname", OracleType.VarChar);
                cmd.Parameters["in_lname"].Value = ht["lname"];

                cmd.Parameters.Add("in_address", OracleType.VarChar);
                cmd.Parameters["in_address"].Value = ht["add"];//

                cmd.Parameters.Add("in_zip", OracleType.VarChar);
                cmd.Parameters["in_zip"].Value = ht["zip"];

                cmd.Parameters.Add("in_city", OracleType.VarChar);
                cmd.Parameters["in_city"].Value = ht["city"];


                cmd.Parameters.Add("in_state", OracleType.VarChar);
                cmd.Parameters["in_state"].Value = ht["state"];

                cmd.Parameters.Add("in_CustType", OracleType.VarChar);
                cmd.Parameters["in_CustType"].Value = ht["custtype"];

                cmd.Parameters.Add("in_ConnectionType", OracleType.VarChar);
                cmd.Parameters["in_ConnectionType"].Value = ht["contype"];
                
                cmd.Parameters.Add("in_Mobile", OracleType.VarChar);
                cmd.Parameters["in_Mobile"].Value = ht["mobile"];



                cmd.Parameters.Add("in_lcoCode", OracleType.VarChar);
                cmd.Parameters["in_lcoCode"].Value = ht["lcocode"];


                cmd.Parameters.Add("in_lcoName", OracleType.VarChar);
                cmd.Parameters["in_lcoName"].Value = ht["lconame"];

              
                cmd.Parameters.Add("in_amount", OracleType.VarChar);
                cmd.Parameters["in_amount"].Value = ht["amount"];

                //cmd.Parameters.Add("in_Expirydate", OracleType.VarChar);
                //cmd.Parameters["in_Expirydate"].Value =ht["expirydate"];

                cmd.Parameters.Add("in_req_by", OracleType.VarChar);
                cmd.Parameters["in_req_by"].Value = ht["reqBY"];

                cmd.Parameters.Add("in_reason", OracleType.VarChar);
                cmd.Parameters["in_reason"].Value = ht["Reason"];

                cmd.Parameters.Add("in_InsBy", OracleType.VarChar);
                cmd.Parameters["in_InsBy"].Value = ht["insBy"];

                //cmd.Parameters.Add("in_ToDayDate", OracleType.VarChar);
                //cmd.Parameters["in_ToDayDate"].Value = ht["Date"];

                cmd.Parameters.Add("in_upBy", OracleType.VarChar);
                cmd.Parameters["in_upBy"].Value = ht["UpdtBy"];

              
                //cmd.Parameters.Add("in_Update_date", OracleType.VarChar);
                //cmd.Parameters["in_Update_date"].Value = ht["UpdateDate"];

                cmd.Parameters.Add("in_discounttype", OracleType.VarChar);
                cmd.Parameters["in_discounttype"].Value = ht["discounttype"];

                cmd.Parameters.Add("out_errordata", OracleType.VarChar,4000);
                cmd.Parameters["out_errordata"].Direction = ParameterDirection.Output;


                cmd.Parameters.Add("out_errorcode", OracleType.Number,100);
                cmd.Parameters["out_errorcode"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                ConObj.Close();
                string cd = Convert.ToString(cmd.Parameters["out_errordata"].Value);
                string result = Convert.ToString(cmd.Parameters["out_errorcode"].Value);
                return result;
            }
            catch (Exception ex)
            {
              //  Cls_Security objSecurity = new Cls_Security();
               // objSecurity.InsertIntoDb(ht["AccNum"].ToString(), ex.Message.ToString(), "Cls_Data_FrmDiscount-DiscountInsert");
                return "-1000$ex_occured";

            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }


        }

    }
}
