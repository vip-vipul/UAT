using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;

namespace PrjUpassDAL.Master
{
    public class Cls_Data_mstComplaintRegistration
    {
        public string[] GetLcoDetails(string username, string prefixText, string type, string operid, string catid)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                if (type == "1")
                {

                    str = "  SELECT a.lcoid, a.distid, a.msoid, a.hoid, a.lcomstcode, a.lcomstname, ";
                    str += "  a.lcomstaddress, a.lcomstmobileno, a.lcomstemail, ";
                    str += "  a.currentcreditlimit,companyname ";
                    str += "  FROM veiw_lcopre_paylco_search a ";
                    str += " where upper(a.LCOMSTNAME) like upper('" + prefixText + "%') ";
                    //    str = " select a.var_lcomst_code, a.var_lcomst_name, a.var_lcomst_address, a.num_lcomst_mobileno, a.var_lcomst_email " +
                    //" from aoup_lcopre_lco_det a where upper(a.var_lcomst_name) like upper('" + prefixText + "%') " +
                    //" and rownum = '1'";
                }
                else if (type == "0")
                {
                    str = "  SELECT a.lcoid, a.distid, a.msoid, a.hoid, a.lcomstcode, a.lcomstname, ";
                    str += "  a.lcomstaddress, a.lcomstmobileno, a.lcomstemail, ";
                    str += "  a.currentcreditlimit,companyname ";
                    str += "  FROM veiw_lcopre_paylco_search a ";
                    str += " where a.LCOMSTCODE='" + prefixText + "'";

                    //str = " select a.var_lcomst_code, a.var_lcomst_name, a.var_lcomst_address, a.num_lcomst_mobileno, a.var_lcomst_email " +
                    //               " from aoup_lcopre_lco_det a where upper(a.var_lcomst_code) = '" + prefixText + "'" +
                    //               " and rownum = '1'";
                }
                if (catid == "2")
                {
                    str += " and a.msoid = " + operid;
                }
                else if (catid == "5")
                {
                    str += " and a.distid = " + operid;
                }
                else if (catid == "10")
                {
                    str += " and a.HOID = " + operid;
                }

                OracleCommand cmd = new OracleCommand(str, conObj);

                conObj.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(dr["LCOMSTCODE"].ToString());
                    Operators.Add(dr["LCOMSTNAME"].ToString());
                    Operators.Add(dr["lcomstaddress"].ToString());
                    Operators.Add(dr["LCOMSTMOBILENO"].ToString());
                    Operators.Add(dr["LCOMSTEMAIL"].ToString());
                    Operators.Add(dr["CURRENTCREDITLIMIT"].ToString());
                    Operators.Add(dr["companyname"].ToString());
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

        public string InsertComplaint(Hashtable ht,string username)
        {
            string strreturn = "";
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand com = new OracleCommand("crm.aomcm_newcomplaintmst_ins", ConObj);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new OracleParameter("in_UserId", OracleType.VarChar));
                com.Parameters["in_UserId"].Value = ht["in_UserId"];
                com.Parameters.Add(new OracleParameter("in_CompltMasID", OracleType.Number));
                com.Parameters["in_CompltMasID"].Value = ht["in_CompltMasID"];
                com.Parameters.Add(new OracleParameter("in_ComplNo", OracleType.VarChar));
                com.Parameters["in_ComplNo"].Value = ht["in_ComplNo"];
                com.Parameters.Add(new OracleParameter("in_CmpltID", OracleType.Number));
                com.Parameters["in_CmpltID"].Value = ht["in_CmpltID"];
                com.Parameters.Add(new OracleParameter("in_DeptID", OracleType.Number));
                com.Parameters["in_DeptID"].Value = ht["in_DeptID"];
                com.Parameters.Add(new OracleParameter("in_AuthorizeUserId", OracleType.Number));
                com.Parameters["in_AuthorizeUserId"].Value = ht["in_AuthorizeUserId"];
                com.Parameters.Add(new OracleParameter("in_ComplTypeid", OracleType.Number));
                com.Parameters["in_ComplTypeid"].Value = ht["in_ComplTypeid"];
                com.Parameters.Add(new OracleParameter("in_ComplSubTyp", OracleType.Number));
                com.Parameters["in_ComplSubTyp"].Value = ht["in_ComplSubTyp"];
                com.Parameters.Add(new OracleParameter("in_CustomeNM", OracleType.VarChar));
                com.Parameters["in_CustomeNM"].Value = ht["in_CustomeNM"];
                com.Parameters.Add(new OracleParameter("in_CustCntNo", OracleType.Number));
                com.Parameters["in_CustCntNo"].Value = ht["in_CustCntNo"];
                com.Parameters.Add(new OracleParameter("in_Complaint", OracleType.VarChar));
                com.Parameters["in_Complaint"].Value = ht["in_Complaint"];
                com.Parameters.Add(new OracleParameter("in_CurrentStst", OracleType.VarChar));
                com.Parameters["in_CurrentStst"].Value = ht["in_CurrentStst"];
                com.Parameters.Add(new OracleParameter("in_cmplRegDate", OracleType.DateTime));
                com.Parameters["in_cmplRegDate"].Value = ht["in_cmplRegDate"];
                com.Parameters.Add(new OracleParameter("in_CmplRefId", OracleType.Number));
                com.Parameters["in_CmplRefId"].Value = ht["in_CmplRefId"];
                com.Parameters.Add(new OracleParameter("in_ComEntryType", OracleType.VarChar));
                com.Parameters["in_ComEntryType"].Value = ht["in_ComEntryType"];
                //com.Parameters.Add(new OracleParameter("in_SmsCmplID", OracleType.Number));
                //com.Parameters["in_SmsCmplID"].Value = ht["in_SmsCmplID"];
                com.Parameters.Add(new OracleParameter("in_Mode", OracleType.Number));
                com.Parameters["in_Mode"].Value = 1;
                com.Parameters.Add(new OracleParameter("in_Address", OracleType.VarChar));
                com.Parameters["in_Address"].Value =ht["in_Address"];

                com.Parameters.Add(new OracleParameter("in_ComplaintSource", OracleType.VarChar));
                com.Parameters["in_ComplaintSource"].Value = ht["in_ComplaintSource"];

                com.Parameters.Add(new OracleParameter("in_Email", OracleType.VarChar));
                com.Parameters["in_Email"].Value = ht["in_Email"];

                /*com.Parameters.Add(new OracleParameter("in_Location", OracleType.VarChar));

                if (ht["in_Location"] == null)
                {
                    com.Parameters["in_Location"].Value = DBNull.Value;
                }
                else
                {
                    com.Parameters["in_Location"].Value = ht["in_Location"];
                }*/

                com.Parameters.Add(new OracleParameter("in_complnt_flag", OracleType.VarChar));
                com.Parameters["in_complnt_flag"].Value = ht["in_complnt_flag"];

                com.Parameters.Add(new OracleParameter("in_lcocode", OracleType.VarChar));
                com.Parameters["in_lcocode"].Value = ht["in_lcocode"];


                com.Parameters.Add(new OracleParameter("in_callerName", OracleType.VarChar));
                com.Parameters["in_callerName"].Value = ht["in_callername"];

                com.Parameters.Add(new OracleParameter("in_callerNo", OracleType.VarChar));
                com.Parameters["in_callerNo"].Value = ht["in_callerno"];

                com.Parameters.Add(new OracleParameter("in_companyname", OracleType.VarChar));
                com.Parameters["in_companyname"].Value = ht["in_companyName"];

                com.Parameters.Add(new OracleParameter("in_Alternateno", OracleType.VarChar));
                com.Parameters["in_Alternateno"].Value = ht["in_Alternateno"];

                com.Parameters.Add(new OracleParameter("out_uniqueId", OracleType.Number, 1000));
                com.Parameters["out_uniqueId"].Direction = ParameterDirection.Output;

                com.Parameters.Add(new OracleParameter("out_ErrorCode", OracleType.Number, 1000));
                com.Parameters["out_ErrorCode"].Direction = ParameterDirection.Output;

                com.Parameters.Add(new OracleParameter("out_ErrorMsg", OracleType.VarChar, 3000));
                com.Parameters["out_ErrorMsg"].Direction = ParameterDirection.Output;
                com.ExecuteNonQuery();

                string strErrcode = com.Parameters["out_ErrorCode"].Value.ToString();
                
                string strErrorMsg = com.Parameters["out_ErrorMsg"].Value.ToString();

                string str_uniqueId = com.Parameters["out_uniqueId"].Value.ToString();
                
                return strErrcode + "$" + strErrorMsg + "$" + str_uniqueId;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCORegistration.cs-setLCOData");
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
