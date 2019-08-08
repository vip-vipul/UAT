using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.Data.OracleClient;
using System.Collections;
using System.Configuration;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_RptINVLedger
    {
        string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();

        public DataTable GetTransations(string datestring, string whereClauseStr, string username, string dateStringparty)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;


                //StrQry = "select x.lconame,x.lcoid,x.lcocode,(select openinbal  from view_LCO_partyledgr_summ where dt=x.min_dt and lcocode=x.lcocode )openinbal," +
                //        " x.drlimit,x.crlimit, x.companyname, x.distributor,x.subdistributor, x.cityname, x.statename, " +
                //        " (select closingbal  from view_LCO_partyledgr_summ where dt=x.max_dt and lcocode=x.lcocode )closingbal" +
                //        " FRom (SELECT distid,PARENTID,hoid, dt,operid, lconame, lcoid, lcocode, sum(drlimit) drlimit , sum(crlimit) crlimit,  min(dt) min_dt,max(dt)max_dt, companyname, distributor,subdistributor, cityname, statename" +
                //        " FROM view_LCO_partyledgr_summ a " +
                //        " group by lconame, lcoid, lcocode, companyname, distributor,subdistributor, cityname, statename, dt,operid,distid,PARENTID,hoid)x " +
                //" where " + whereClauseStr;

                StrQry = " select x.lconame,x.lcoid,x.lcocode,(select num_partled_openinbal   from aoup_lcopre_lcoin_partyled_mst where trunc(dat_partled_date)=x.min_dt and var_partled_lcocode=x.lcocode )openinbal, ";
                StrQry += "   x.drlimit,x.crlimit, x.companyname, x.distributor,x.subdistributor, x.cityname, x.statename, ";
                StrQry += "    (select num_partled_closingbal   from aoup_lcopre_lcoin_partyled_mst where trunc(dat_partled_date)=x.min_dt and var_partled_lcocode=x.lcocode  )closingbal ";
                StrQry += "    FRom ";
                StrQry += "   (SELECT distid,PARENTID,hoid, operid, lconame, lcoid, lcocode, sum(drlimit) drlimit , sum(crlimit) crlimit,  min(dt) min_dt,max(dt)max_dt, companyname, ";
                StrQry += "   distributor,subdistributor, cityname, statename ";
                StrQry += "    FROM view_lco_invpartyledgr_summ a ";
                StrQry += "   where " + datestring + "";
                StrQry += "    group by lconame, lcoid, lcocode, companyname, distributor,subdistributor, cityname, statename, operid,distid,PARENTID,hoid)x ";
                if (whereClauseStr != "")
                {
                    StrQry += "    where " + whereClauseStr;
                }


                /*   StrQry = "  select   lconame, lcoid, lcocode, (select NVL(a.num_partled_openinbal,0) from aoup_lcopre_lco_partyled_mst a ";
                   StrQry += "   where dat_partled_date= ";
                   StrQry += "    (select min(dat_partled_date) from aoup_lcopre_lco_partyled_mst b where x.lcoid =b.var_partled_lcoid and " + dateStringparty + ") ";
                   StrQry += "   and x.lcoid=a.var_partled_lcoid)openinbal, ";
                   StrQry += "    (select NVL(a.num_partled_closingbal,0) from aoup_lcopre_lco_partyled_mst a ";
                   StrQry += "   where dat_partled_date= ";
                   StrQry += "   (select max(dat_partled_date) from aoup_lcopre_lco_partyled_mst b where x.lcoid =b.var_partled_lcoid and " + dateStringparty + " )  ";
                   StrQry += "   and x.lcoid=a.var_partled_lcoid) closingbal  ";
                   StrQry += "   ,sum(drlimit) drlimit , sum(crlimit) crlimit, ";
                   StrQry += "    companyname,distributor,subdistributor, cityname, statename       ";
                   StrQry += "    from view_LCO_partyledgr_summ x  ";
                   StrQry += " where " + datestring + " ";
                   StrQry += " and " + whereClauseStr + " ";
                   StrQry += "   group by  lconame, lcoid, lcocode, companyname,  distributor,subdistributor, cityname, statename ";*/



                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptINVLedger.cs-GetTransations");
                return null;
            }
        }

        public DataTable GetTransationsDet(string whereClauseStr2, string username2)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT drlimit, crlimit, ltype, dt1, premark,balance " +
                                " FROM view_lco_invpartyledgr_det " +
                " where " + whereClauseStr2;

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username2, ex.Message.ToString(), "Cls_Data_RptINVLedger.cs-GetTransationsDet");
                return null;
            }
        }

        public DataTable GetTransationsDetLCO(Hashtable ht, string whereClauseStr1, string username1)
        {
            OracleConnection ConObj = new OracleConnection(ConStr);
            try
            {
                //Cls_Helper ObjHelper = new Cls_Helper();
                //string StrQry;
                //StrQry = "SELECT lconame, lcocode, sum(openinbal) openinbal, sum(drlimit) drlimit, sum(crlimit) crlimit, sum(closingbal) closingbal  " +
                //                " FROM view_LCO_partyledgr_summ " +
                //" where " + whereClauseStr1 +
                //" group by lconame, lcocode ";
                //return ObjHelper.GetDataTable(StrQry);
                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_invprtyledsumm_dt", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 50);
                Cmd.Parameters["in_username"].Value = username1;

                Cmd.Parameters.Add("in_from_dt", OracleType.VarChar, 100);
                Cmd.Parameters["in_from_dt"].Value = ht["from"].ToString();

                Cmd.Parameters.Add("in_to_dt", OracleType.VarChar, 100);
                Cmd.Parameters["in_to_dt"].Value = ht["to"].ToString();

                Cmd.Parameters.Add("in_lcoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcoid"].Value = ht["lcoid"].ToString();

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 4000);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                ConObj.Close();

                string exeData = Convert.ToString(Cmd.Parameters["out_data"].Value);
                string exeCode = Convert.ToString(Cmd.Parameters["out_errcode"].Value);

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("lcocode"));
                dt.Columns.Add(new DataColumn("lconame"));
                dt.Columns.Add(new DataColumn("openinbal"));
                dt.Columns.Add(new DataColumn("drlimit"));
                dt.Columns.Add(new DataColumn("crlimit"));
                dt.Columns.Add(new DataColumn("closingbal"));

                if (exeData != "no_data_found")
                {
                    DataRow dr = dt.NewRow();
                    dr["lcocode"] = exeData.Split('$')[0];
                    dr["lconame"] = exeData.Split('$')[1];
                    dr["openinbal"] = exeData.Split('$')[2];
                    dr["crlimit"] = exeData.Split('$')[3];
                    dr["drlimit"] = exeData.Split('$')[4];
                    dr["closingbal"] = exeData.Split('$')[5];

                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username1, ex.Message.ToString() + '$' + ht["from"] + '$' + ht["to"] + '$' + ht["lcoid"], "Cls_Data_RptINVLedger.cs-GetTransationsDetLCO");
                return null;
            }
        }
    
    }
}
