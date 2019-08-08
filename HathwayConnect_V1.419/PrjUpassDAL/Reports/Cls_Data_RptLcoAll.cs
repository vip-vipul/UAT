using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_RptLcoAll
    {
        public string[] GetLcoDetails(string username, string prefixText, string type, string operid, string catid)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";


                str = "  SELECT a.operid, a.lcoid, a.distid, a.parentid, a.hoid, a.lcocode, a.lconame, ";
                str += "  a.var_lcomst_address, a.num_lcomst_mobileno, a.var_lcomst_email, ";
                str += "  a.awailbal, a.state, a.city, a.dist, a.subdist, a.jvno, a.erpaccno, a.ecsstatus, ";
                str += "  a.AREAMANAGER, a.PTEXPIRYDT, a.INTAGREEEXPDT, a.EXECUTIVE ";
                str += "  FROM view_lcoall_det_summ a ";

                if (type == "1")
                {
                    str += " where upper(a.lconame) like upper('" + prefixText + "') ";
                }
                else if (type == "0")
                {
                    str += " where upper(a.lcocode) like upper('" + prefixText + "')";
                }
                if (catid == "2")
                {
                    str += " and a.parentid = " + operid;
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

                    Operators.Add(dr["lcocode"].ToString());
                    Operators.Add(dr["lconame"].ToString());
                    Operators.Add(dr["var_lcomst_address"].ToString());
                    Operators.Add(dr["num_lcomst_mobileno"].ToString());
                    Operators.Add(dr["var_lcomst_email"].ToString());
                    Operators.Add(dr["awailbal"].ToString());
                    Operators.Add(dr["lcoid"].ToString());
                    Operators.Add(dr["operid"].ToString());
                    Operators.Add(dr["state"].ToString());
                    Operators.Add(dr["city"].ToString());
                    Operators.Add(dr["dist"].ToString());
                    Operators.Add(dr["subdist"].ToString());
                    Operators.Add(dr["jvno"].ToString());
                    Operators.Add(dr["erpaccno"].ToString());
                    Operators.Add(dr["ecsstatus"].ToString());
                    Operators.Add(dr["AREAMANAGER"].ToString());
                    Operators.Add(dr["PTEXPIRYDT"].ToString());
                    Operators.Add(dr["INTAGREEEXPDT"].ToString());
                    Operators.Add(dr["EXECUTIVE"].ToString());



                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLcoAll-GetLcoDetails");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public string[] GetLcoDataById(string username, string operid)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string str = "";
                str = "  SELECT a.lcoid, a.distid, a.parentid, a.hoid, a.lcocode, a.lconame, ";
                str += "  a.var_lcomst_address, a.num_lcomst_mobileno, a.var_lcomst_email, ";
                str += "  a.awailbal ";
                str += "  FROM view_lcoall_det_summ a ";
                str += " where a.OPERID = " + operid;

                OracleCommand cmd = new OracleCommand(str, conObj);
                conObj.Open();
                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Operators.Add(dr["lcocode"].ToString());
                    Operators.Add(dr["lconame"].ToString());
                    Operators.Add(dr["var_lcomst_address"].ToString());
                    Operators.Add(dr["num_lcomst_mobileno"].ToString());
                    Operators.Add(dr["var_lcomst_email"].ToString());
                    Operators.Add(dr["awailbal"].ToString());
                    Operators.Add(dr["lcoid"].ToString());
                }
                string[] prefixTextArray = Operators.ToArray<string>();
                conObj.Close();
                return prefixTextArray;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLcoAll-GetLcoDataById");
                return null;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }

        public DataTable GetTransationsLcoDets(string whereClauseStr, string username)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT lconame, lcoid, lcocode, awailbal  " +
                                " FROM view_lcoall_det_summ " +
                " where " + whereClauseStr;
                //" group by lconame, lcoid, lcocode ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLcoAll-GetTransationsLcoDets");
                return null;
            }
        }

        public DataTable GetTransationsLastFDets(string whereClauseStr2, string username2)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                //StrQry = "SELECT a.transid, a.receiptno, a.custid, a.vc, a.custname, a.custaddr, a.planid, ";
                //StrQry += "  a.plnname, a.plntyp, a.amtdd, a.lcoprice, a.expdt, a.payterm, ";
                //StrQry += "   a.bal, a.companycode, a.flag, a.insby, a.dt, a.tdt, a.uname, ";
                //StrQry += "   a.userowner, a.usrid, a.lcocode, a.lconame, a.jvname, ";
                //StrQry += "   a.erplco_ac, a.distname, a.subdist, a.city, a.state,a.custprice,a.amtdd,a.flag  ";
                //StrQry += "  FROM view_lcoall_last_trans_det a  ";
                //StrQry += "   where " + whereClauseStr2;

                StrQry = "SELECT a.transid, a.receiptno, a.custid, a.vc, a.custname, a.planid,a.addr,  ";
                StrQry += " a.plnname, a.plntyp, a.amtdd, a.custprice, a.expdt, a.payterm, ";
                StrQry += " a.bal, a.companycode, a.flag, a.insby, a.dt, a.tdt, a.uname,  ";
                StrQry += "  a.userowner, a.usrid, a.lcocode, a.lconame, a.jvname,  ";
                StrQry += "  a.erplco_ac, a.distname, a.subdist, a.city, a.state, a.reason   ";
                StrQry += " FROM view_lcopre_user_trans_det a    ";
                StrQry += "   where " + whereClauseStr2;
                StrQry += " and rownum <=5 ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username2, ex.Message.ToString(), "Cls_Data_RptLcoAll-GetTransationsLastFDets");
                return null;
            }
        }

        public DataTable GetTransationsTopDets(string whereClauseStr3, string username3)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT dtttime, amt, paymode, rcptno, erprcptno, finuid," +
                        "fiuname, action, lcocode, lconame, jvname, erplco_ac, distname, subdist, city, state " +
                        " FROM view_lcoovervw_Topup_det " +
                        " where " + whereClauseStr3 +
                        " and rownum <=5 ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username3, ex.Message.ToString(), "Cls_Data_RptLcoAll-GetTransationsTopDets");
                return null;
            }
        }

        public DataTable GetTransationsReversDets(string whereClauseStr4, string username4)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT  transid,  voucherno, ";
                StrQry += "  amount,  reasonname,  lcopayremark, ";
                StrQry += "  companycode,  insby, a.date1,  ";
                StrQry += "  insdt,  lcocode,";
                StrQry += "  chequebouncedt, a.operid, a.parentid,";
                StrQry += "  a.distid FROM view_lcoovervw_revrs_det a ";
                StrQry += " where " + whereClauseStr4;
                StrQry += " and rownum <=5 ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username4, ex.Message.ToString(), "Cls_Data_RptLcoAll-GetTransationsReversDets");
                return null;
            }
        }

        public DataTable GetPartyLedger(string whereClauseStr, string username)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT lconame, lcocode, openinbal, drlimit, crlimit, closingbal  " +
                                " FROM view_LCO_partyledgr_summ " +
                " WHERE " + whereClauseStr;

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLcoAll-GetPartyLedger");
                return null;
            }
        }

        public DataTable GetPartyLedgerDet(string whereClauseStr2, string username2)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT drlimit, crlimit, ltype, dt1, premark,balance " +
                                " FROM view_LCO_partyledgr_det " +
                " where " + whereClauseStr2;

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username2, ex.Message.ToString(), "Cls_Data_RptLcoAll-GetPartyLedgerDet");
                return null;
            }
        }

        public DataTable GetServiceData(string username, string catid, string operator_id)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                string whereString = "";
                string from = DateTime.Today.ToString("dd-MMM-yyyy");
                string to = DateTime.Today.ToString("dd-MMM-yyyy");

                if (from != null && from != "")
                {
                    whereString += " and trandt >=  '" + from + "' ";
                }
                if (to != null && to != "")
                {
                    whereString += " and trandt <=  '" + to + "' ";
                }
                if (catid == "3")
                {
                    whereString += " and lcode = '" + username + "' ";
                }
                //if (catid == "2")
                //{
                //    whereString += " and MSOID= '" + operator_id + "' ";
                //}
                //else if (catid == "5")
                //{
                //    whereString += "  and DISTID= '" + operator_id + "' ";
                //}
                //else if (catid == "3")
                //{
                //    whereString += "  and LCOID= '" + operator_id + "' ";
                //}
                //else if (catid == "10")
                //{
                //    whereString += "  and hoid = '" + operator_id + "' ";
                //}

                StrQry = "SELECT a.msoid, a.msoname, a.distid, a.distname, a.lcoid, a.lconame, a.stbno, a.custno, a.addr, a.accpoid, a.svcpoid, a.vcid," +
                         " a.status, a.transby, a.transdt, a.orderid, a.reasonname" +
                         " FROM view_lcopre_service_actdact a " +
                         " where 1=1 " +
                         " " + whereString;

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLCOAll-GetServiceData");
                return null;
            }
        }

        public DataTable GetUserDet(string username, string oper_id)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry = "SELECT a.userid, a.operid, a.username, a.userowner, a.fname, a.mname, a.lname, a.addr, a.code, a.brmpoid, a.ststeid, a.cityid, a.email, " +
                            " a.mobno, a.compcode, a.accno, a.insby, a.insdt, a.flag, a.balance " +
                            " FROM view_Lcopre_user_det a" +
                            " where a.operid ='" + oper_id + "'";
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLCOAll-GetUserDet");
                return null;
            }
        }

    }
}
