using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.IO;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_RptAddPlan
    {
        public DataTable GetTransations(string whereClauseStr, string username, string TableName)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = " SELECT lconame, lcoid, lcocode, sum(amt)amt, sum(cnt)cnt,sum(amtdd) amtdd " +
                         " FROM " +
                         "( select b.num_lcomst_operid lcoid, b.var_lcomst_code lcocode, b.var_lcomst_name lconame ,c.num_usermst_id usrid, c.var_usermst_username uname, c.var_usermst_name userowner," +
                         " trunc(a.dat_custpay_transdt) dt,count(a.num_custpay_transid) cnt, sum(nvl(a.num_custpay_balance,0))amt,sum(nvl(a.num_custpay_lcoprice,0))amtdd" +
                         " , xx.num_oper_id OPERID, xx.num_oper_distid distid, xx.num_oper_parentid PARENTID, yy.num_oper_parentid hoid,a.var_custpay_vcid VCMAC_Id,a.var_custpay_custid Account_No,xx.num_oper_clust_id clustid," +
                         " (case when a.var_custpay_plantype = 'AD' then 'Addon' when a.var_custpay_plantype = 'AL' then 'A-La-Carte'" +
                         " when a.var_custpay_plantype = 'B' then 'Basic' else a.var_custpay_plantype end) PlanType,(CASE WHEN a.var_custpay_flag = 'R' THEN 'Renewal' WHEN a.var_custpay_flag = 'A' THEN 'Activation' WHEN a.var_custpay_flag = 'C' THEN 'Cancellation'" +
                         " WHEN a.var_custpay_flag = 'RR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'AR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'CR' THEN 'Failure Refund'" +
                         " WHEN a.var_custpay_flag = 'CHR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'CH' THEN 'Cancellation' END) planflag,a.var_custpay_payterm payterm" +
                         " from " + TableName + " a, aoup_lcopre_lco_det b, aoup_lcopre_user_det c, aoup_operator_def xx,aoup_operator_def yy where a.var_custpay_insby=b.num_lcomst_operid" +
                         " and a.var_custpay_user=c.var_usermst_username and c.num_usermst_operid=xx.num_oper_id and yy.num_oper_id=xx.num_oper_parentid" +
                         " group by b.num_lcomst_operid , b.var_lcomst_code , b.var_lcomst_name  ,c.num_usermst_id , c.var_usermst_username , c.var_usermst_name ,a.var_custpay_vcid,a.var_custpay_custid," +
                         " trunc(a.dat_custpay_transdt), xx.num_oper_id , xx.num_oper_distid , xx.num_oper_parentid, yy.num_oper_parentid,xx.num_oper_clust_id,a.var_custpay_plantype,a.var_custpay_flag,a.var_custpay_payterm )" +
                         " where " + whereClauseStr +
                         " group by lconame, lcoid, lcocode ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptAddPlan.cs");
                return null;
            }
        }

        public DataTable GetTransationsMSO(string whereClauseStr, string username, string tableName)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                /*StrQry = "SELECT sum(amt)amt, sum(cnt)cnt,sum(amtdd) amtdd,  ";
                StrQry += " a.msoname,a.MSOID ";
                StrQry += " FROM view_addplan_trans_summ_mso a ";
                StrQry += " where " + whereClauseStr;
                StrQry += " group by a.msoname,a.MSOID ";*/

                StrQry = " SELECT sum(amt)amt, sum(cnt)cnt,sum(amtdd) amtdd,a.msoname,a.MSOID  from (";
                StrQry += " select c.num_usermst_id usrid, c.var_usermst_username uname, c.var_usermst_name userowner,trunc(a.dat_custpay_transdt) dt,";
                StrQry += " count(a.num_custpay_transid) cnt, sum(nvl(a.num_custpay_balance,0))amt,sum(nvl(a.num_custpay_lcoprice,0))amtdd";
                StrQry += " , yy.num_oper_id OPERID, xx.num_oper_distid distid, yy.num_oper_id msoid,yy.var_oper_opername msoname, yy.num_oper_parentid hoid,a.var_custpay_custid accno,a.var_custpay_vcid vcid,";
                StrQry += " b.num_lcomst_stateid State , b.num_lcomst_cityid city , b.num_lcomst_jvno JV,";
                StrQry += " (case when a.var_custpay_plantype = 'AD' then 'Addon' when a.var_custpay_plantype = 'AL' then 'A-La-Carte'";
                StrQry += " when a.var_custpay_plantype = 'B' then 'Basic' else a.var_custpay_plantype end) PlanType,(CASE WHEN a.var_custpay_flag = 'R' THEN 'Renewal' WHEN a.var_custpay_flag = 'A' THEN 'Activation' WHEN a.var_custpay_flag = 'C' THEN 'Cancellation'";
                StrQry += " WHEN a.var_custpay_flag = 'RR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'AR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'CR' THEN 'Failure Refund'";
                StrQry += " WHEN a.var_custpay_flag = 'CHR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'CH' THEN 'Cancellation' END) planflag, a.var_custpay_payterm payterm";
                StrQry += " from " + tableName + " a, aoup_lcopre_lco_det b, aoup_lcopre_user_det c, aoup_operator_def xx,aoup_operator_def yy where a.var_custpay_insby=b.num_lcomst_operid and a.var_custpay_user=c.var_usermst_username";
                StrQry += " and c.num_usermst_operid=xx.num_oper_id and yy.num_oper_id=xx.num_oper_parentid group by  yy.var_oper_opername,yy.num_oper_id,";
                StrQry += " c.num_usermst_id , c.var_usermst_username , c.var_usermst_name ,trunc(a.dat_custpay_transdt)";
                StrQry += " , xx.num_oper_id , xx.num_oper_distid , xx.num_oper_parentid, yy.num_oper_parentid,yy.var_oper_opername,yy.num_oper_id,a.var_custpay_custid,a.var_custpay_vcid,";
                StrQry += " b.num_lcomst_stateid  , b.num_lcomst_cityid , b.num_lcomst_jvno,a.var_custpay_plantype,a.var_custpay_flag,a.var_custpay_payterm )";
                StrQry += " where " + whereClauseStr;
                StrQry += " group by a.msoname,a.MSOID ";


                //FileLogText(StrQry, "GetTransationsMSO", "view_addplan_trans_summ_mso", StrQry);
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptAddPlan.cs");
                return null;
            }
        }

        public DataTable GetTransationsU(string whereClauseStr1, string username1, string TableName)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT lconame, uname, usrid, userowner, sum(amt)amt, sum(cnt)cnt,sum(amtdd) amtdd from " +
                                "( select b.num_lcomst_operid lcoid, b.var_lcomst_code lcocode, b.var_lcomst_name lconame ,c.num_usermst_id usrid, c.var_usermst_username uname, c.var_usermst_name userowner," +
                            " trunc(a.dat_custpay_transdt) dt,count(a.num_custpay_transid) cnt, sum(nvl(a.num_custpay_balance,0))amt,sum(nvl(a.num_custpay_lcoprice,0))amtdd" +
                            " , xx.num_oper_id OPERID, xx.num_oper_distid distid, xx.num_oper_parentid PARENTID, yy.num_oper_parentid hoid,a.var_custpay_vcid VCMAC_Id,a.var_custpay_custid Account_No,xx.num_oper_clust_id clustid," +
                            " (case when a.var_custpay_plantype = 'AD' then 'Addon' when a.var_custpay_plantype = 'AL' then 'A-La-Carte'" +
                            " when a.var_custpay_plantype = 'B' then 'Basic' else a.var_custpay_plantype end) PlanType,(CASE WHEN a.var_custpay_flag = 'R' THEN 'Renewal' WHEN a.var_custpay_flag = 'A' THEN 'Activation' WHEN a.var_custpay_flag = 'C' THEN 'Cancellation'" +
                            " WHEN a.var_custpay_flag = 'RR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'AR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'CR' THEN 'Failure Refund'" +
                            " WHEN a.var_custpay_flag = 'CHR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'CH' THEN 'Cancellation' END) planflag,a.var_custpay_payterm payterm" +
                            " from " + TableName + " a, aoup_lcopre_lco_det b, aoup_lcopre_user_det c, aoup_operator_def xx,aoup_operator_def yy where a.var_custpay_insby=b.num_lcomst_operid" +
                            " and a.var_custpay_user=c.var_usermst_username and c.num_usermst_operid=xx.num_oper_id and yy.num_oper_id=xx.num_oper_parentid" +
                            " group by b.num_lcomst_operid , b.var_lcomst_code , b.var_lcomst_name  ,c.num_usermst_id , c.var_usermst_username , c.var_usermst_name ,a.var_custpay_vcid,a.var_custpay_custid," +
                            " trunc(a.dat_custpay_transdt), xx.num_oper_id , xx.num_oper_distid , xx.num_oper_parentid, yy.num_oper_parentid,xx.num_oper_clust_id,a.var_custpay_plantype,a.var_custpay_flag,a.var_custpay_payterm )" +
                " where " + whereClauseStr1 +
                " group by lconame, uname, usrid, userowner ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username1, ex.Message.ToString(), "Cls_Data_RptAddPlan.cs");
                return null;
            }
        }

        public DataTable GetTransationsDet(string whereClauseStr2, string username2, string TableName)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT * FROM  " +
                    " (SELECT a.num_custpay_transid transid, a.var_custpay_receiptno receiptno, " +
" a.var_custpay_custid custid, a.var_custpay_vcid vc,REPLACE( replace(replace(a.var_custpay_custname, ',', ' '),chr(9),' ' ), CHR(13) || CHR(10))custname, " +
" REPLACE( replace(replace(a.var_custpay_address,',',' '),chr(9),' '), CHR(13) || CHR(10) )  custaddr,a.var_custpay_planid planid, a.var_custpay_planname plnname, " +
" (case when a.var_custpay_plantype = 'AD' then 'Broadcaster Bouquet' when a.var_custpay_plantype = 'AL' then 'A-La-Carte' when a.var_custpay_plantype='B' then 'Basic'  when a.var_custpay_plantype='HSP' then 'Hathway Bouquet'  else a.var_custpay_plantype end) plntyp, " +
" a.num_custpay_custprice custprice, a.num_custpay_custprice lcoprice,a.num_custpay_lcoprice amtdd, to_char(a.dat_custpay_expirydt, 'dd-Mon-yyyy')expdt,a.var_custpay_payterm payterm, nvl(a.num_custpay_balance, 0) bal, " +
" a.var_custpay_companycode companycode,(CASE WHEN a.var_custpay_flag = 'R' THEN 'Renewal' WHEN a.var_custpay_flag = 'A' THEN 'Activation' WHEN a.var_custpay_flag = 'C' THEN 'Cancellation' " +
" WHEN a.var_custpay_flag = 'RR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'AR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'CR' THEN 'Failure Refund' " +
" WHEN a.var_custpay_flag = 'CHR' THEN 'Failure Refund' WHEN a.var_custpay_flag = 'CH' THEN 'Cancellation' END) flag,a.var_custpay_reasontxt reason, " +
" z.var_user_username insby, trunc(a.dat_custpay_transdt)dt, to_char(a.dat_custpay_transdt, 'dd-Mon-yyyy hh:mi:ss AM')tdt,a.dat_custpay_transdt tdt1,x.var_user_username uname ,trim(z.var_user_userowner) userowner ,z.num_user_userid usrid , " +
" f.var_lcomst_code lcocode , replace(f.var_lcomst_name, ',',' ') lconame, replace(a.var_custpay_company, ',', ' ')||'('||d.var_city_name||')' jvname,f.var_lcomst_erpaccno  erplco_ac, " +
" f.var_lcomst_distbutor distname, f.var_lcomst_subdist subdist, d.var_city_name city,d.num_city_id city_id, e.var_state_name state, f.num_lcomst_operid loperid,y.num_oper_parentid parentid,'Success'obrmstatus, " +
" (CASE WHEN a.var_custpay_source_flag = 'HC' THEN 'WEB' WHEN a.var_custpay_source_flag = 'HCBULK' THEN 'BULK' WHEN a.var_custpay_source_flag = 'HCMOB' THEN 'MOBILE' " +
" WHEN a.var_custpay_source_flag = 'HSMS' THEN 'WEB' else a.var_custpay_source_flag END) Sflag,a.var_custpay_das_area AREA,a.num_custpay_lco_shares LSHARE,(case when a.var_custpay_shares_type = 'C' then 'City Share' " +
" when a.var_custpay_shares_type = 'L' then 'LCO Share' end) LSHARETYPE,a.num_custpay_discount DISCOUNT,case when num_state_id=11 and a.var_custpay_flag in ('A','R') and trunc(a.dat_custpay_transdt)>='01-jul-2017' then round(((a.num_custpay_custprice*1.18)/2)-a.num_custpay_lcoprice,2) else 0 end lcodiscount, " +
" case when num_state_id=11 and a.var_custpay_flag in ('A','R') and trunc(a.dat_custpay_transdt)>='01-jul-2017'  then round(((a.num_custpay_custprice*1.18)/2),2) else num_custpay_lcoprice end netlcoprice ," +
" case when num_state_id=11 and a.var_custpay_flag in ('A','R') and trunc(a.dat_custpay_transdt)>='01-jul-2017'  then '31-DEC-2017 12:00:00 AM'  else 'N/A' end discountenddate FROM " + TableName + " a, " +
" aoup_lcopre_city_def d, AOUP_LCOPRE_STATE_DEF e,aoup_lcopre_lco_det f, aoup_user_def x, aoup_operator_def y,aoup_user_def z where a.var_custpay_insby=f.num_lcomst_operid and z.var_user_username=a.var_custpay_user " +
" and x.var_user_username=a.var_custpay_user and d.num_city_stateid=e.num_state_id and f.num_lcomst_cityid=d.num_city_id and y.num_oper_id=x.num_user_operid order by a.num_custpay_transid  desc )" +
                " where " + whereClauseStr2;

                //FileLogText(StrQry, "GetTransationsDet", "view_AddPlan_Trans_det", StrQry);


                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username2, ex.Message.ToString(), "Cls_Data_RptAddPlan.cs");
                return null;
            }
        }

        public void FileLogText(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\Hway\logquery_" + filename + ".txt", true);
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
