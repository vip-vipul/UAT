using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_TransHwayLcoPayment
    {
        public string[] SearchOperators(string username, string prefixText, string type)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                if (type == "1")
                {
                    str = " select a.var_lcomst_name from aoup_lcopre_lco_det a" +
                    " where upper(a.var_lcomst_name) like upper('" + prefixText + "%')";
                }
                else
                {
                    return null;
                }
                OracleCommand cmd = new OracleCommand(str, conObj);

                conObj.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(dr["var_oper_opername"].ToString());
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayLcoPayment.cs-SearchOperators");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string[] GetLcopaymentDetails(string username, string prefixText, string category, string operid, string type, string BankName)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {

                string str = "";

                str = "  select VAR_LCOMST_CODE, VAR_LCOMST_NAME, VAR_LCOMST_ADDRESS, NUM_LCOMST_MOBILENO, ";
                str += " VAR_LCOMST_EMAIL, NUM_LCOPAY_AMOUNT, VAR_LCOPAY_PAYMODE, BNKNM, VAR_LCOPAY_BRANCH,var_lcopay_chqddno,dat_lcopay_chequedt,";
                str += " VAR_LCOPAY_RECEIPTNO, OPERID, OPERCATEGORY, PARENTID, DISTID from view_lcopre_payment_rev";
                if (type == "0")
                {
                    str += " where VAR_LCOPAY_RECEIPTNO='" + prefixText + "'";
                }
                else
                {
                    str += " where var_lcopay_bank='" + BankName.ToString() + "' and var_lcopay_chqddno='" + prefixText + "' ";
                }

                if (category == "2")
                {
                    str += " and parentid='" + operid.ToString() + "'  ";
                }
                if (category == "5")
                {
                    str += " and distid='" + operid.ToString() + "'  ";
                }
                else if (category == "10")
                {

                    str += " and HOID='" + operid.ToString() + "'  ";
                }


                OracleCommand cmd = new OracleCommand(str, conObj);

                conObj.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(dr["var_lcomst_code"].ToString());
                    Operators.Add(dr["var_lcomst_name"].ToString());
                    Operators.Add(dr["var_lcomst_address"].ToString());
                    Operators.Add(dr["num_lcomst_mobileno"].ToString());
                    Operators.Add(dr["var_lcomst_email"].ToString());
                    Operators.Add(dr["num_lcopay_amount"].ToString());
                    Operators.Add(dr["var_lcopay_paymode"].ToString());
                    Operators.Add(dr["bnknm"].ToString());
                    Operators.Add(dr["var_lcopay_branch"].ToString());
                    Operators.Add(dr["var_lcopay_chqddno"].ToString());
                    Operators.Add(dr["dat_lcopay_chequedt"].ToString());
                    Operators.Add(dr["VAR_LCOPAY_RECEIPTNO"].ToString());
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHwayLcoPayment-GetLcopaymentDetails");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string[] GetLcoCashpaymentDetails(string username, string prefixText, string category, string operid)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {

                string str = "";

                str = "  select a.var_lcomst_code, a.var_lcomst_name, a.var_lcomst_address, ";
                str += " a.num_lcomst_mobileno, a.var_lcomst_email, a.num_lcopay_amount,";
                str += " a.var_lcopay_paymode, a.var_lcopay_receiptno";
                str += " from view_lcopre_cash_payment_rev a";
                str += " where a.var_lcopay_receiptno='" + prefixText + "' ";

                if (category == "2")
                {
                    str += " and parentid='" + operid.ToString() + "'  ";
                }
                if (category == "5")
                {
                    str += " and distid='" + operid.ToString() + "'  ";
                }
                else if (category == "10")
                {
                    str += " and HOID='" + operid.ToString() + "'  ";
                }


                OracleCommand cmd = new OracleCommand(str, conObj);

                conObj.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(dr["var_lcomst_code"].ToString());
                    Operators.Add(dr["var_lcomst_name"].ToString());
                    Operators.Add(dr["var_lcomst_address"].ToString());
                    Operators.Add(dr["num_lcomst_mobileno"].ToString());
                    Operators.Add(dr["var_lcomst_email"].ToString());
                    Operators.Add(dr["num_lcopay_amount"].ToString());
                    Operators.Add(dr["var_lcopay_paymode"].ToString());
                    Operators.Add(dr["VAR_LCOPAY_RECEIPTNO"].ToString());
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHwayLcoPayment-GetLcoCashpaymentDetail");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

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
                    str += "  a.currentcreditlimit ";
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
                    str += "  a.currentcreditlimit ";
                    str += "  FROM veiw_lcopre_paylco_search a ";
                    str += " where upper(a.LCOMSTCODE) like upper('" + prefixText + "%')";

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
                    Operators.Add(dr["LCOMSTEMAIL"].ToString());
                    Operators.Add(dr["LCOMSTMOBILENO"].ToString());
                    Operators.Add(dr["LCOMSTEMAIL"].ToString());
                    Operators.Add(dr["CURRENTCREDITLIMIT"].ToString());
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

        public string[] GetplanDetails(string username, string prefixText, string plantype, string city, string type, string operid)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                if (type == "1")
                {
                    str = " select a.var_lcomst_code, a.var_lcomst_name, a.var_lcomst_address, a.num_lcomst_mobileno, a.var_lcomst_email " +
                " from aoup_lcopre_lco_det a where upper(a.var_lcomst_name) like upper('" + prefixText + "%') " +
                " and rownum = '1'";
                }
                else if (type == "0")
                {
                    str = " select a.var_lcomst_code, a.var_lcomst_name, a.var_lcomst_address, a.num_lcomst_mobileno, a.var_lcomst_email " +
                                   " from aoup_lcopre_lco_det a where upper(a.var_lcomst_code) = '" + prefixText + "'" +
                                   " and rownum = '1'";
                }

                OracleCommand cmd = new OracleCommand(str, conObj);

                conObj.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(dr["var_lcomst_code"].ToString());
                    Operators.Add(dr["var_lcomst_name"].ToString());
                    Operators.Add(dr["var_lcomst_address"].ToString());
                    Operators.Add(dr["num_lcomst_mobileno"].ToString());
                    Operators.Add(dr["var_lcomst_email"].ToString());
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

        public string LcoPaymentDetails(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_payment_ins", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = ht["User"];

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 50);
                Cmd.Parameters["in_lcocode"].Value = ht["CustCode"];

                Cmd.Parameters.Add("in_amount", OracleType.Number);
                Cmd.Parameters["in_amount"].Value = ht["Amount"];

                Cmd.Parameters.Add("in_paymode", OracleType.VarChar, 100);
                Cmd.Parameters["in_paymode"].Value = ht["PayMode"];

                Cmd.Parameters.Add("in_chequeddno", OracleType.VarChar, 100);
                Cmd.Parameters["in_chequeddno"].Value = ht["chequeddno"];

                Cmd.Parameters.Add("in_chequedt", OracleType.DateTime);

                if (ht["CheckDate"].ToString() != "")
                {
                    Cmd.Parameters["in_chequedt"].Value = ht["CheckDate"];
                }

                else
                {
                    Cmd.Parameters["in_chequedt"].Value = DBNull.Value;
                }

                Cmd.Parameters.Add("in_bankname", OracleType.VarChar, 100);
                Cmd.Parameters["in_bankname"].Value = ht["BankName"];

                Cmd.Parameters.Add("in_branchname", OracleType.VarChar, 100);
                Cmd.Parameters["in_branchname"].Value = ht["Branch"];

                Cmd.Parameters.Add("in_remark", OracleType.VarChar, 100);
                Cmd.Parameters["in_remark"].Value = ht["Remark"];

                Cmd.Parameters.Add("in_erpreceiptno", OracleType.VarChar, 100);
                Cmd.Parameters["in_erpreceiptno"].Value = ht["ReceiptNo"];

                Cmd.Parameters.Add("in_IP", OracleType.VarChar, 100);
                Cmd.Parameters["in_IP"].Value = ht["IP"];

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 250);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_receiptno", OracleType.VarChar, 250);
                Cmd.Parameters["out_receiptno"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_recdt", OracleType.VarChar, 250);
                Cmd.Parameters["out_recdt"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_cashier", OracleType.VarChar, 250);
                Cmd.Parameters["out_cashier"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_address", OracleType.VarChar, 250);
                Cmd.Parameters["out_address"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_company", OracleType.VarChar, 250);
                Cmd.Parameters["out_company"].Direction = ParameterDirection.Output;

                /*    Cmd.Parameters.Add("out_BASEamt", OracleType.Double);
                    Cmd.Parameters["out_BASEamt"].Direction = ParameterDirection.Output;

                    Cmd.Parameters.Add("out_ET", OracleType.Double);
                    Cmd.Parameters["out_ET"].Direction = ParameterDirection.Output;

                    Cmd.Parameters.Add("out_ETamt", OracleType.Double);
                    Cmd.Parameters["out_ETamt"].Direction = ParameterDirection.Output;

                    Cmd.Parameters.Add("out_ST", OracleType.Double);
                    Cmd.Parameters["out_ST"].Direction = ParameterDirection.Output;

                    Cmd.Parameters.Add("out_STamt", OracleType.Double);
                    Cmd.Parameters["out_STamt"].Direction = ParameterDirection.Output;

                    Cmd.Parameters.Add("out_EC", OracleType.Double);
                    Cmd.Parameters["out_EC"].Direction = ParameterDirection.Output;

                    Cmd.Parameters.Add("out_ECamt", OracleType.Double);
                    Cmd.Parameters["out_ECamt"].Direction = ParameterDirection.Output;

                    Cmd.Parameters.Add("out_HEC", OracleType.Double);
                    Cmd.Parameters["out_HEC"].Direction = ParameterDirection.Output;

                    Cmd.Parameters.Add("out_HECamt", OracleType.Double);
                    Cmd.Parameters["out_HECamt"].Direction = ParameterDirection.Output;
                */
                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;
                string out_receiptno = Convert.ToString(Cmd.Parameters["out_receiptno"].Value);
                string out_recdt = Convert.ToString(Cmd.Parameters["out_recdt"].Value);
                string out_cashier = Convert.ToString(Cmd.Parameters["out_cashier"].Value);
                string out_address = Convert.ToString(Cmd.Parameters["out_address"].Value);
                string out_company = Convert.ToString(Cmd.Parameters["out_company"].Value);

                /*
                string out_BASEamt = Convert.ToString(Cmd.Parameters["out_BASEamt"].Value);
                string out_ET = Convert.ToString(Cmd.Parameters["out_ET"].Value);
                string out_ETamt = Convert.ToString(Cmd.Parameters["out_ETamt"].Value);
                string out_ST = Convert.ToString(Cmd.Parameters["out_ST"].Value);
                string out_STamt = Convert.ToString(Cmd.Parameters["out_STamt"].Value);
                string out_EC = Convert.ToString(Cmd.Parameters["out_EC"].Value);
                string out_ECamt = Convert.ToString(Cmd.Parameters["out_ECamt"].Value);
                string out_HEC = Convert.ToString(Cmd.Parameters["out_HEC"].Value);
                string out_HECamt = Convert.ToString(Cmd.Parameters["out_HECamt"].Value);
                */
                if (exeResult == 9999)
                {
                    Str = "9999";
                    Str += ",Payment done successfully...";
                    Str += "," + out_receiptno.ToString();
                    Str += "," + out_recdt.ToString();
                    Str += "," + out_cashier.ToString();
                    Str += "," + out_address.ToString();
                    Str += "," + out_company.ToString();
                    /*  Str += "," + out_BASEamt.ToString();
                      Str += "," + out_ET.ToString();
                      Str += "," + out_ETamt.ToString();
                      Str += "," + out_ST.ToString();
                      Str += "," + out_STamt.ToString();
                      Str += "," + out_EC.ToString();
                      Str += "," + out_ECamt.ToString();
                      Str += "," + out_HEC.ToString();
                      Str += "," + out_HECamt.ToString(); */
                }
                else
                {
                    Str = exeResult.ToString();
                    Str += "," + Convert.ToString(Cmd.Parameters["out_data"].Value);
                    //Str += "," + out_receiptno.ToString();
                    //Str += "," + out_recdt.ToString();
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["User"].ToString(), ex.Message.ToString(), "Cls_Data_TransHwayLcoPayment.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string LcoPaymentRevarsal(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_LCOPRE_PAYMENT_REVOKE", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = ht["User"];

                Cmd.Parameters.Add("in_checkbouncedt", OracleType.DateTime);
                Cmd.Parameters["in_checkbouncedt"].Value = ht["ChequeBounceDate"];

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocode"].Value = ht["LcoCode"];

                Cmd.Parameters.Add("in_receiptno", OracleType.VarChar, 100);
                Cmd.Parameters["in_receiptno"].Value = ht["ReceiptNo"];

                Cmd.Parameters.Add("in_amount", OracleType.VarChar, 100);
                Cmd.Parameters["in_amount"].Value = ht["Amount"];

                Cmd.Parameters.Add("in_remark", OracleType.VarChar, 1000);
                Cmd.Parameters["in_remark"].Value = ht["Remark"];

                Cmd.Parameters.Add("in_reason", OracleType.VarChar, 100);
                Cmd.Parameters["in_reason"].Value = ht["Reason"];

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
                    Str = "Payment reversal done successfully...";
                }
                else
                {
                    //Str = "Payment reversal done successfully...";
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["user"].ToString(), ex.Message.ToString(), "Cls_Data_TransHwayLcoPayment.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string LcoCashPaymentRevarsal(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_cash_paymentrevoke", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = ht["User"];

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocode"].Value = ht["LcoCode"];

                Cmd.Parameters.Add("in_receiptno", OracleType.VarChar, 100);
                Cmd.Parameters["in_receiptno"].Value = ht["ReceiptNo"];

                Cmd.Parameters.Add("in_amount", OracleType.VarChar, 100);
                Cmd.Parameters["in_amount"].Value = ht["Amount"];

                Cmd.Parameters.Add("in_remark", OracleType.VarChar, 1000);
                Cmd.Parameters["in_remark"].Value = ht["Remark"];

                Cmd.Parameters.Add("in_reason", OracleType.VarChar, 100);
                Cmd.Parameters["in_reason"].Value = ht["Reason"];

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
                    Str = "Payment reversal done successfully...";
                }
                else
                {
                    //Str = "Payment reversal done successfully...";
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                    if (Str == "")
                    {
                        Str = "Payment reversal failed...";
                    }
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["user"].ToString(), ex.Message.ToString(), "Cls_Data_TransHwayLcoPayment.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string[] getTaxDetails(string username, string rcpt_no)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str = " SELECT a.num_lcopay_taxet, a.num_lcopay_taxst,  " +
                      " a.num_lcopay_taxec, a.num_lcopay_taxhec, a.num_lcopay_taxetamt,  " +
                      " a.num_lcopay_taxstamt, a.num_lcopay_taxecamt, a.num_lcopay_taxhecamt, " +
                      " a.num_lcopay_baseamt "  +
                      " FROM aoup_lcopre_lco_payment_det a "  +
                      " WHERE a.var_lcopay_receiptno = '" + rcpt_no + "'";
                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(dr["num_lcopay_taxet"].ToString());
                    Operators.Add(dr["num_lcopay_taxetamt"].ToString());
                    Operators.Add(dr["num_lcopay_taxst"].ToString());
                    Operators.Add(dr["num_lcopay_taxstamt"].ToString());
                    Operators.Add(dr["num_lcopay_taxec"].ToString());
                    Operators.Add(dr["num_lcopay_taxecamt"].ToString());
                    Operators.Add(dr["num_lcopay_taxhec"].ToString());
                    Operators.Add(dr["num_lcopay_taxhecamt"].ToString());
                    Operators.Add(dr["num_lcopay_baseamt"].ToString());
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHwayLcoPayment-getTaxDetails");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
        public Hashtable LcoOnlinePaymentTransID(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_online_payment", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = ht["User"];

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 50);
                Cmd.Parameters["in_lcocode"].Value = ht["CustCode"];

                Cmd.Parameters.Add("in_amount", OracleType.Number);
                Cmd.Parameters["in_amount"].Value = ht["Amount"];

                Cmd.Parameters.Add("in_paymode", OracleType.VarChar, 100);
                Cmd.Parameters["in_paymode"].Value = ht["PayMode"];

                Cmd.Parameters.Add("in_chequeddno", OracleType.VarChar, 100);
                Cmd.Parameters["in_chequeddno"].Value = ht["chequeddno"];

                Cmd.Parameters.Add("in_chequedt", OracleType.DateTime);
                Cmd.Parameters["in_chequedt"].Value = ht["CheckDate"];

                Cmd.Parameters.Add("in_bankname", OracleType.VarChar, 100);
                Cmd.Parameters["in_bankname"].Value = ht["BankName"];

                Cmd.Parameters.Add("in_branchname", OracleType.VarChar, 100);
                Cmd.Parameters["in_branchname"].Value = ht["Branch"];


                Cmd.Parameters.Add("in_remark", OracleType.VarChar, 100);
                Cmd.Parameters["in_remark"].Value = ht["Remark"];

                Cmd.Parameters.Add("in_erpreceiptno", OracleType.VarChar, 100);
                Cmd.Parameters["in_erpreceiptno"].Value = ht["ReceiptNo"];

                Cmd.Parameters.Add("in_user_id", OracleType.VarChar, 100);
                Cmd.Parameters["in_user_id"].Value = ht["user_id"];

                Cmd.Parameters.Add("in_user_brmpoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_user_brmpoid"].Value = ht["user_brmpoid"];

                Cmd.Parameters.Add("in_operator_id", OracleType.VarChar, 100);
                Cmd.Parameters["in_operator_id"].Value = ht["operator_id"];

                Cmd.Parameters.Add("in_category", OracleType.VarChar, 100);
                Cmd.Parameters["in_category"].Value = ht["category"];

                Cmd.Parameters.Add("in_name", OracleType.VarChar, 100);
                Cmd.Parameters["in_name"].Value = ht["name"];

                Cmd.Parameters.Add("in_last_login", OracleType.VarChar, 100);
                Cmd.Parameters["in_last_login"].Value = ht["last_login"];


                Cmd.Parameters.Add("in_login_flag", OracleType.VarChar, 100);
                Cmd.Parameters["in_login_flag"].Value = ht["login_flag"];

                Cmd.Parameters.Add("in_Session", OracleType.VarChar, 100);
                Cmd.Parameters["in_Session"].Value = ht["Session"];

                Cmd.Parameters.Add("in_identifier", OracleType.VarChar, 100);
                Cmd.Parameters["in_identifier"].Value = ht["Identifier"];

                Cmd.Parameters.Add("in_crlmttype", OracleType.VarChar, 100);
                Cmd.Parameters["in_crlmttype"].Value = ht["CrLimittype"];

                Cmd.Parameters.Add("out_transid", OracleType.Number);
                Cmd.Parameters["out_transid"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_receipt", OracleType.VarChar, 100);
                Cmd.Parameters["out_receipt"].Direction = ParameterDirection.Output;
                
                Cmd.Parameters.Add("out_data", OracleType.VarChar, 250);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;


                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();


                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;
                int Transid;
                string receipt;

                if (exeResult == 9999)
                {

                    Transid = Convert.ToInt32(Cmd.Parameters["out_transid"].Value);
                    receipt = Convert.ToString(Cmd.Parameters["out_receipt"].Value);
                    ht.Add("Transid", Transid);
                    ht.Add("receipt", receipt);
                    Str = "Online Payment  id generate  successfully...";
                }
                else
                {
                    //Str = "Payment reversal done successfully...";
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                    Str = "Online Payment  id generate  Unsuccessfully...";
                    Transid = 0;
                }
                ConObj.Dispose();
                return ht;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["user"].ToString(), ex.Message.ToString(), "LcoOnlinePaymentTransID.cs");
                return null;
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string LcoOnlinPayment(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_get_online_pay_det", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

     

                Cmd.Parameters.Add("in_online_trans_id",  OracleType.Number);
                Cmd.Parameters["in_online_trans_id"].Value = ht["TransId"];

                Cmd.Parameters.Add("in_ubilldesk_order_no",  OracleType.Number);
                Cmd.Parameters["in_ubilldesk_order_no"].Value = ht["UbilldeskOrderNo"];

                Cmd.Parameters.Add("in_billdesk_Ref_no", OracleType.VarChar, 100);
                Cmd.Parameters["in_billdesk_Ref_no"].Value = ht["BillDeskRef"];

                Cmd.Parameters.Add("in_billdesk_autho_status", OracleType.Number);
                Cmd.Parameters["in_billdesk_autho_status"].Value = ht["BillDeskAuthoStatus"];

                Cmd.Parameters.Add("in_billdesk_autho_message", OracleType.VarChar, 100);
                Cmd.Parameters["in_billdesk_autho_message"].Value = ht["BillDeskAuthoMessage"];

                Cmd.Parameters.Add("in_identifier", OracleType.VarChar, 100);
                Cmd.Parameters["in_identifier"].Value = ht["Indentifier"];

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 250);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;
                 Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;
                //string out_receiptno = Convert.ToString(Cmd.Parameters["out_data"].Value);
                //string[] stroutdata=
                    //out_receiptno
            
                if (exeResult == 9999)
                {
                    string out_receiptno = Convert.ToString(Cmd.Parameters["out_data"].Value);
                    string[] stroutdata = out_receiptno.Split('$');

                    Str = "successfully" + "$" + stroutdata[1] + "$" + stroutdata[2];
                }
                else
                {

                    Str =  Convert.ToString(Cmd.Parameters["out_data"].Value);
                    //Str = exeResult.ToString();
                    //Str += "," + Convert.ionToString(Cmd.Parameters["out_data"].Value);
                    ////Str += "," + out_receiptno.ToString();
                    ////Str += "," + out_recdt.ToString();
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["User"].ToString(), ex.Message.ToString(), "Cls_Data_TransHwayLcoPayment.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
        public string[] getPanDetails(string username, string company)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str += " SELECT a.var_compconfig_cin, a.var_compconfig_pan, a.var_compconfig_stno,b.var_comp_companyname,d.var_usermst_address ";
                str += " FROM   aoup_lcopre_compconfig_det a, aoup_lcopre_company_def b,aoup_lcopre_lco_det c,aoup_lcopre_user_det d ";
                str += " where  a.var_compconfig_company = b.var_comp_companyname and var_lcomst_company=var_compconfig_company ";
                str += "  and d.var_usermst_username=c.var_lcomst_code and c.var_lcomst_code='" + username + "'";
                str += " group by a.var_compconfig_cin, a.var_compconfig_pan, a.var_compconfig_stno,b.var_comp_companyname,d.var_usermst_address";

                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(Convert.ToString(dr["var_compconfig_cin"]));
                    Operators.Add(Convert.ToString(dr["var_compconfig_pan"]));
                    Operators.Add(Convert.ToString(dr["var_compconfig_stno"]));
                    Operators.Add(Convert.ToString(dr["var_comp_companyname"]));
                    Operators.Add(Convert.ToString(dr["var_usermst_address"]));
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHwayLcoPayment-getPanDetails");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        //------- Added By RP on 28.07.2017
        public Hashtable InventryOnlinePaymentTransID(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {



                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_online_payment", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = ht["User"];

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 50);
                Cmd.Parameters["in_lcocode"].Value = ht["CustCode"];

                Cmd.Parameters.Add("in_amount", OracleType.Number);
                Cmd.Parameters["in_amount"].Value = ht["Amount"];

                Cmd.Parameters.Add("in_paymode", OracleType.VarChar, 100);
                Cmd.Parameters["in_paymode"].Value = ht["PayMode"];

                Cmd.Parameters.Add("in_chequeddno", OracleType.VarChar, 100);
                Cmd.Parameters["in_chequeddno"].Value = ht["chequeddno"];

                Cmd.Parameters.Add("in_chequedt", OracleType.DateTime);
                Cmd.Parameters["in_chequedt"].Value = ht["CheckDate"];

                Cmd.Parameters.Add("in_bankname", OracleType.VarChar, 100);
                Cmd.Parameters["in_bankname"].Value = ht["BankName"];

                Cmd.Parameters.Add("in_branchname", OracleType.VarChar, 100);
                Cmd.Parameters["in_branchname"].Value = ht["Branch"];


                Cmd.Parameters.Add("in_remark", OracleType.VarChar, 100);
                Cmd.Parameters["in_remark"].Value = ht["Remark"];

                Cmd.Parameters.Add("in_erpreceiptno", OracleType.VarChar, 100);
                Cmd.Parameters["in_erpreceiptno"].Value = ht["ReceiptNo"];

                Cmd.Parameters.Add("in_user_id", OracleType.VarChar, 100);
                Cmd.Parameters["in_user_id"].Value = ht["user_id"];

                Cmd.Parameters.Add("in_user_brmpoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_user_brmpoid"].Value = ht["user_brmpoid"];

                Cmd.Parameters.Add("in_operator_id", OracleType.VarChar, 100);
                Cmd.Parameters["in_operator_id"].Value = ht["operator_id"];

                Cmd.Parameters.Add("in_category", OracleType.VarChar, 100);
                Cmd.Parameters["in_category"].Value = ht["category"];

                Cmd.Parameters.Add("in_name", OracleType.VarChar, 100);
                Cmd.Parameters["in_name"].Value = ht["name"];

                Cmd.Parameters.Add("in_last_login", OracleType.VarChar, 100);
                Cmd.Parameters["in_last_login"].Value = ht["last_login"];


                Cmd.Parameters.Add("in_login_flag", OracleType.VarChar, 100);
                Cmd.Parameters["in_login_flag"].Value = ht["login_flag"];

                Cmd.Parameters.Add("in_Session", OracleType.VarChar, 100);
                Cmd.Parameters["in_Session"].Value = ht["Session"];

                Cmd.Parameters.Add("in_identifier", OracleType.VarChar, 100);
                Cmd.Parameters["in_identifier"].Value = ht["Identifier"];

                Cmd.Parameters.Add("in_crlmttype", OracleType.VarChar, 100);
                Cmd.Parameters["in_crlmttype"].Value = ht["CrLimittype"];

                Cmd.Parameters.Add("out_transid", OracleType.Number);
                Cmd.Parameters["out_transid"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_receipt", OracleType.VarChar, 100);
                Cmd.Parameters["out_receipt"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 250);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;


                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();


                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;
                int Transid;
                string receipt;

                if (exeResult == 9999)
                {

                    Transid = Convert.ToInt32(Cmd.Parameters["out_transid"].Value);
                    receipt = Convert.ToString(Cmd.Parameters["out_receipt"].Value);
                    ht.Add("Transid", Transid);
                    ht.Add("receipt", receipt);
                    Str = "Online Payment  id generate  successfully...";
                }
                else
                {
                    //Str = "Payment reversal done successfully...";
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                    Str = "Online Payment  id generate  Unsuccessfully...";
                    Transid = 0;
                }
                ConObj.Dispose();
                return ht;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["user"].ToString(), ex.Message.ToString(), "LcoOnlinePaymentTransID.cs");
                return null;
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public string InventryOnlinPayment(Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_get_online_pay_det", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;



                Cmd.Parameters.Add("in_online_trans_id", OracleType.Number);
                Cmd.Parameters["in_online_trans_id"].Value = ht["TransId"];

                Cmd.Parameters.Add("in_ubilldesk_order_no", OracleType.Number);
                Cmd.Parameters["in_ubilldesk_order_no"].Value = ht["UbilldeskOrderNo"];

                Cmd.Parameters.Add("in_billdesk_Ref_no", OracleType.VarChar, 100);
                Cmd.Parameters["in_billdesk_Ref_no"].Value = ht["BillDeskRef"];

                Cmd.Parameters.Add("in_billdesk_autho_status", OracleType.Number);
                Cmd.Parameters["in_billdesk_autho_status"].Value = ht["BillDeskAuthoStatus"];

                Cmd.Parameters.Add("in_billdesk_autho_message", OracleType.VarChar, 100);
                Cmd.Parameters["in_billdesk_autho_message"].Value = ht["BillDeskAuthoMessage"];

                Cmd.Parameters.Add("in_identifier", OracleType.VarChar, 100);
                Cmd.Parameters["in_identifier"].Value = ht["Indentifier"];

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 250);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;
                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;
                //string out_receiptno = Convert.ToString(Cmd.Parameters["out_data"].Value);
                //string[] stroutdata=
                //out_receiptno

                if (exeResult == 9999)
                {
                    string out_receiptno = Convert.ToString(Cmd.Parameters["out_data"].Value);
                    string[] stroutdata = out_receiptno.Split('$');

                    Str = "successfully" + "$" + stroutdata[1] + "$" + stroutdata[2];
                }
                else
                {

                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                    //Str = exeResult.ToString();
                    //Str += "," + Convert.ionToString(Cmd.Parameters["out_data"].Value);
                    ////Str += "," + out_receiptno.ToString();
                    ////Str += "," + out_recdt.ToString();
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["User"].ToString(), ex.Message.ToString(), "Cls_Data_TransHwayLcoPayment.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
        //---------
        //-------- Added By RP on 11.12.2017
        public string[] getPanDetailsPIS(string username, string company)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str += " SELECT a.var_compconfig_cin, a.var_compconfig_pan, a.var_compconfig_stno,a.var_compconfig_company,a.var_compconfig_localaddress,a.var_compconfig_regiaddress ";
                str += " FROM   aoup_lcopre_compconfig_det a,aoup_lcopre_user_det d,aoup_lcopre_company_det c ";
                str += " where d.num_usermst_operid=c.num_comp_operid and   c.var_comp_company=var_compconfig_company and d.var_usermst_username='" + username + "'";
                str += " group by a.var_compconfig_cin, a.var_compconfig_pan, a.var_compconfig_stno,a.var_compconfig_company,a.var_compconfig_localaddress,a.var_compconfig_regiaddress";

                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(Convert.ToString(dr["var_compconfig_cin"]));
                    Operators.Add(Convert.ToString(dr["var_compconfig_pan"]));
                    Operators.Add(Convert.ToString(dr["var_compconfig_stno"]));
                    Operators.Add(Convert.ToString(dr["var_compconfig_company"]));
                    Operators.Add(Convert.ToString(dr["var_compconfig_localaddress"]));
                    Operators.Add(Convert.ToString(dr["var_compconfig_regiaddress"]));
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHwayLcoPayment-getPanDetails");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        //---------- 
    }
}
