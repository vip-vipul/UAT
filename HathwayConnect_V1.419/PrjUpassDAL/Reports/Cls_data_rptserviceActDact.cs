using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;

namespace PrjUpassDAL.Reports
{
    public class Cls_data_rptserviceActDact
    {
        public DataTable Getactdactstat(Hashtable htAddPlanParams, string username, string catid, string operator_id)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                string whereString = "";
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string status = htAddPlanParams["status"].ToString();

                if (from != null && from != "")
                {
                    whereString += " and trandt >=  '" + from + "' ";
                }
                if (to != null && to != "")
                {
                    whereString += " and trandt <=  '" + to + "' ";
                }
                if (catid == "2")
                {
                    whereString += " and MSOID= '" + operator_id + "' ";
                }
                else if (catid == "5")
                {
                    whereString += "  and DISTID= '" + operator_id + "' ";
                }
                else if (catid == "3")
                {
                    whereString += "  and LCOID= '" + operator_id + "' ";
                }
                else if (catid == "10")
                {
                    whereString += "  and hoid = '" + operator_id + "' ";
                }

                StrQry = " SELECT a.msoid, a.msoname, a.distid, a.distname, a.lcoid, a.lconame, ";
                StrQry += "  a.hoid, a.stbno, a.custno, a.addr, a.accpoid, a.svcpoid, a.vcid, ";
                StrQry += "  a.stat, a.status, a.transby, a.trandt, a.transdt, a.orderid, ";
                StrQry += "  a.cityname, a.statename, a.companyname, a.distributor, ";
                StrQry += "  a.subdistributor, a.lcode, a.lnaame, a.reason ";
                StrQry += "   FROM view_lcopre_service_actdact a ";
                if (status.ToString() == "ALL")
                {
                    StrQry += "   where a.stat in ('A','D','T')   ";
                }
                else
                {
                    StrQry += "   where a.stat = '" + status + "'   ";
                }
                StrQry += " " + whereString;

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_data_rptserviceActDact-Getactdactstat");
                return null;
            }
        }



        public DataTable GetBulkSCHstatus(Hashtable htAddPlanParams, string username,  string operator_id)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                string whereString = "";
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string status = htAddPlanParams["status"].ToString();

                if (from != null && from != "")
                {
                    whereString += " and TRUNC(INSDT) >=  '" + from + "' ";
                }
                if (to != null && to != "")
                {
                    whereString += " and TRUNC(INSDT) <=  '" + to + "' ";
                }
                    whereString += " and OperID= '" + operator_id + "' ";

                    StrQry = " SELECT a.BulkID,a.ACCNO, a.VCID, a.FILENAME, a.ACTION, a.STATUS, a.PROCESSFLAG, a.INSBY, a.INSDT, a.SCHDATE, a.REASON, a.OPERID,a.lcocode,a.DISABLBY,a.DISABLDATE ";
                StrQry += "   FROM view_BUlk_ACTDCT_SCHD_DET a ";
                if (status.ToString() == "ALL")
                {
                    StrQry += "   where a.status in ('A','D')   ";
                }
                else
                {
                    StrQry += "   where a.status = '" + status + "'   ";
                }
                StrQry += " " + whereString;

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_data_rptserviceActDact-GetBulkSCHstatus");
                return null;
            }
        }


        public DataTable GetBulkSCHDISstatus(Hashtable htAddPlanParams, string username, string operator_id)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                string whereString = "";
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string status = htAddPlanParams["status"].ToString();

                if (from != null && from != "")
                {
                    whereString += " and TRUNC(INSDT) >=  '" + from + "' ";
                }
                if (to != null && to != "")
                {
                    whereString += " and TRUNC(INSDT) <=  '" + to + "' ";
                }
                whereString += " and OperID= '" + operator_id + "' ";

                StrQry = " SELECT a.BulkID,a.ACCNO, a.VCID, a.FILENAME, a.ACTION, a.STATUS, a.PROCESSFLAG, a.INSBY, a.INSDT, a.SCHDATE, a.REASON, a.OPERID,a.lcocode,a.DISABLBY,a.DISABLDATE ";
                StrQry += "   FROM view_BUlk_ACTDCT_SCHD_HTSR a ";
                if (status.ToString() == "ALL")
                {
                    StrQry += "   where a.status in ('A','D')   ";
                }
                else
                {
                    StrQry += "   where a.status = '" + status + "'   ";
                }
                StrQry += " " + whereString;

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_data_rptserviceActDact-GetBulkSCHDISstatus");
                return null;
            }
        }


        public string bulkUploadActTempRemove(string lblbulkid,  string username)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                ConObj.Open();
                //OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulk_upload_temp2", ConObj);

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_bulkactdct_del", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_bulkid", OracleType.VarChar, 100);
                Cmd.Parameters["in_bulkid"].Value = lblbulkid;

                Cmd.Parameters.Add("out_data", OracleType.LongVarChar, 400000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);
                return exeCode + "#" + exeData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayBulkOperation-bulkUploadTemp");
                return "-300#" + ex.Message.ToString();
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        }
}
