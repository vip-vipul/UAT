using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;
using System.IO;

namespace PrjUpassDAL.Transaction
{
    public class Cls_data_TransHwayBulkRenewal
    {
        public string BulkRenewMoveData(string username, string uploadid, string filename, string fromdt, string todt, string plan, string plan_type, string vcid)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_renewal2_emp", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 100);
                Cmd.Parameters["in_username"].Value = username;

                DateTime fromDt = DateTime.ParseExact(fromdt, "dd-MMM-yyyy", null);
                DateTime toDt = DateTime.ParseExact(todt, "dd-MMM-yyyy", null);

                Cmd.Parameters.Add("in_fromdt", OracleType.DateTime);
                Cmd.Parameters["in_fromdt"].Value = fromDt;

                Cmd.Parameters.Add("in_todate", OracleType.DateTime);
                Cmd.Parameters["in_todate"].Value = toDt;

                Cmd.Parameters.Add("in_useruniquekey", OracleType.VarChar, 100);
                Cmd.Parameters["in_useruniquekey"].Value = uploadid;

                Cmd.Parameters.Add("in_filename", OracleType.VarChar, 100);
                Cmd.Parameters["in_filename"].Value = filename;

                Cmd.Parameters.Add("in_plan", OracleType.VarChar, 1000);
                Cmd.Parameters["in_plan"].Value = plan;

                Cmd.Parameters.Add("in_plantype", OracleType.VarChar, 1000);
                Cmd.Parameters["in_plantype"].Value = plan_type;

                Cmd.Parameters.Add("in_vcid", OracleType.VarChar, 1000);
                Cmd.Parameters["in_vcid"].Value = vcid;

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
                    Str = "9999$renewal data moved successfully...";
                }
                else
                {
                    Str = "0$" + Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_data_TransHwayBulkRenewal-BulkRenewMoveData");
                return "ex_occured  - $ " + ex.Message.ToString();
            }

            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public DataTable getUploadedRenewalData(string username, string upload_id)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry = "";
                StrQry += "SELECT a.var_lcopre_bulk_custid account_no, a.var_lcopre_bulk_vcid vc, a.var_lcopre_bulk_lcocode lco_code, a.var_lcopre_bulk_planname planname, a.var_lcopre_bulk_action action, ";
                StrQry += " a.var_lcopre_bulk_insby insby, a.var_lcopre_bulk_useruniqueid upload_id, a.var_lcopre_bulk_date blk_dt, a.var_lcopre_bulk_brmpoid brmpoid ,";
                StrQry += " a.var_lcopre_bulk_ecs_flag AutoRenew ";
                StrQry += " FROM aoup_lcopre_bulk_temp a ";
                StrQry += " where a.var_lcopre_bulk_useruniqueid='" + upload_id + "'";
                StrQry += " and TRUNC (TO_DATE (a.var_lcopre_bulk_date,'DD-Mon-YYYY hh24:mi:ss')) = TRUNC (SYSDATE)";
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_data_TransHwayBulkRenewal-getUploadedRenewalData");
                return null;
            }
        }

        public DataTable GetExpDetails(Hashtable htAddPlanParams, string username, string operid, string catid, string type, string planname)
        {
            try
            {
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string plan_name = "ALL";
                string vcid = "";
                if (htAddPlanParams["plan_name"] != null)
                { //in case of expiry report, this will be null
                    plan_name = htAddPlanParams["plan_name"].ToString();
                }
                if (htAddPlanParams["vcid"].ToString() != "" || htAddPlanParams["vcid"] != null)
                {
                    vcid = htAddPlanParams["vcid"].ToString();
                }
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "SELECT a.account_no, a.fullname, a.address, a.vc, a.mobile, a.lco_code, a.lco_name, a.planname, a.plantype, to_char(a.enddate, 'dd-Mon-yyyy') enddate, a.account_poid," +
                         " a.product_poid, a.service_poid, a.purchase_poid,a.cityname, a.brmpoid, trunc(a.prodenddate) prodenddate" +
                         " FROM view_lcopre_expiry a " +
                         " where (a.enddate >='" + from + "' " +
                         " and a.enddate <='" + to + "' or " + "  a.prodenddate >='" + from + "' " +
                         " and a.prodenddate <='" + to + "')";
                if (plan_name.Trim() != "ALL")
                {
                    StrQry += " and upper(trim(a.planname)) = upper(trim('" + plan_name + "'))";
                }
                if (vcid != "")
                {
                    StrQry += " and a.vc='" + vcid + "' ";
                }
                //StrQry += " and freeflag='N' ";
                if (type != "ALL")
                {
                    string plan_type = "Al-la-carte";
                    if (type == "AD")
                    {
                        plan_type = "Addon";
                    }
                    else if (type == "B,HSP")
                    {
                        plan_type = "'Basic','HSP'";
                    }
                    StrQry += " and trim(a.plantype) in (" + plan_type + ")";
                }
                if (catid == "3" || catid=="11")
                {
                    StrQry += " and a.operid = '" + operid + "'";
                }
                else if (catid == "10")
                {
                    StrQry += " and a.hoid = '" + operid + "'";
                }

                 FileLogText("On Submit : ", "", "", StrQry, "" );

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_data_TransHwayBulkRenewal.cs-GetExpDetails");
                return null;
            }
        }

        public DataTable FillplanCombo(Hashtable htAddPlanParams, string username, string operid, string catid, string type, string lcoCode)
        {
            try
            {
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string plan_name = "ALL";
                if (htAddPlanParams["plan_name"] != null)
                { //in case of expiry report, this will be null
                    plan_name = htAddPlanParams["plan_name"].ToString();
                };
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "SELECT a.plan_name FROM view_lcopre_bulkrenew_flt a " +
                         " where a.expiry >='" + from + "' " +
                         " and a.expiry <='" + to + "'";
              /*  if (plan_name.Trim() != "ALL")
                {
                    StrQry += " and upper(trim(a.plan_name)) = upper(trim('" + plan_name + "'))";
                }
               */

                //StrQry += " and freeflag='N' ";

                if (type.Trim() != "ALL")
                {
                    StrQry += " and trim(a.plan_type) in trim(" + type + ")";
                }
                if (catid == "3")
                {
                    StrQry += " and a.lcocode = '" + lcoCode + "'";
                }
                else if (catid == "10")
                {
                    StrQry += " group by a.plan_name"; //" and a.hoid = '" + operid + "'";
                }

                FileLogText("Fill COmbo : ", from, to, StrQry, "");

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_data_TransHwayBulkRenewal.cs-FillplanCombo");
                return null;
            }
        }

        private void FileLogText(String Str, String sender, String strRequest, String strResponse, string browser_no)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\BULKRENEWDEV_TEST_" + browser_no + "_" + filename + ".txt", true);
                sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(strRequest.Trim());
                sw.WriteLine(strResponse.Trim());
                sw.WriteLine("************************************************************************************************************************");
            }
            catch (Exception ex)
            {
                //Response.Write("Error while writing logs : " + ex.Message.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
    }
}
