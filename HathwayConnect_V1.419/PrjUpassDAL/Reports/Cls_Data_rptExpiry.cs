using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.IO;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_rptExpiry
    {
        //public DataTable GetDetails(Hashtable htAddPlanParams, string username, string operid, string catid)
        //{
        //    try
        //    {
        //        string from = htAddPlanParams["from"].ToString();
        //        string to = htAddPlanParams["to"].ToString();
        //        string package = htAddPlanParams["package"].ToString();
        //        string plan_name = "All";
        //        string search_type = htAddPlanParams["search"].ToString().Trim();
        //        string txtsearch = htAddPlanParams["txtsearch"].ToString().Trim();
        //        if (htAddPlanParams["plan_name"] != null) { //in case of expiry report, this will be null
        //            plan_name = htAddPlanParams["plan_name"].ToString(); 
        //        };
        //        Cls_Helper ObjHelper = new Cls_Helper();
        //        string StrQry;
        //        StrQry = "SELECT a.account_no, a.fullname, a.address, a.vc, a.mobile, a.lco_code, a.lco_name, a.planname, a.plantype, to_char(a.enddate, 'dd-Mon-yyyy') enddate, a.account_poid," +
        //                 " a.product_poid, a.service_poid, a.purchase_poid,a.cityname, a.brmpoid , a.renewflagstatus " +
        //                 " FROM view_lcopre_expiry_rpt a " +
        //                 " where a.enddate >='" + from + "' " +
        //                 " and a.enddate <='" + to + "'";
        //        if (plan_name.Trim() != "All") {
        //            StrQry += " and trim(a.planname) = trim('" + plan_name + "')";
        //        }
        //        if (package != "All")
        //        {
        //            StrQry += " and PACKAGETYPE='"+package.ToString()+"'";
        //        }
        //        if (catid == "3")
        //        {
        //           StrQry += " and a.operid = '" + operid + "'";

        //        }
        //        else if (catid == "10")
        //        {
        //           StrQry += " and a.hoid = '" + operid + "'";
        //        }
        //        if (txtsearch.Length > 0)
        //        {
        //            if (search_type == "0")
        //            {
        //                StrQry += @" and upper(a.account_no)= upper('" + txtsearch + "')";
        //            }
        //            else if (search_type == "1")
        //            {
        //                StrQry += @" and upper(a.vc)= upper('" + txtsearch + "')";
        //            }
        //            else if (search_type == "2")
        //            {
        //                StrQry += @" and upper(a.lco_code)= upper('" + txtsearch + "')";
        //            }
        //        }

        //       // FileLogText("Expiry Query : ", "", StrQry, htAddPlanParams["plan_name"].ToString());

        //        return ObjHelper.GetDataTable(StrQry);
        //    }
        //    catch (Exception ex)
        //    {
        //        Cls_Security objSecurity = new Cls_Security();
        //        objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptExpiry.cs-GetDetails");
        //        return null;
        //    }
        //}


        public DataTable GetDetails(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            try
            {
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string package = htAddPlanParams["package"].ToString();
                string plan_name = "All";
                string search_type = htAddPlanParams["search"].ToString().Trim();
                string txtsearch = htAddPlanParams["txtsearch"].ToString().Trim();
                if (htAddPlanParams["plan_name"] != null)
                { //in case of expiry report, this will be null
                    plan_name = htAddPlanParams["plan_name"].ToString();
                };
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                //StrQry = "SELECT a.account_no, a.fullname, a.address, a.vc, a.mobile, a.lco_code, a.lco_name, a.planname, a.plantype, to_char(a.enddate, 'dd-Mon-yyyy') enddate, a.account_poid," +
                //         " a.product_poid, a.service_poid, a.purchase_poid,a.cityname, a.brmpoid , a.renewflagstatus " +
                //         " FROM view_lcopre_expiry_rpt a " +
                //         " where trunc(a.enddate) >='" + from + "' " +
                //         " and trunc(a.enddate) <='" + to + "'";
                //if (plan_name.Trim() != "All") {
                //    StrQry += " and trim(a.planname) = trim('" + plan_name + "')";
                //}
                //if (package != "All")
                //{
                //    StrQry += " and PACKAGETYPE='"+package.ToString()+"'";
                //}
                //if (catid == "3")
                //{
                //   StrQry += " and a.operid = '" + operid + "'";

                //}
                //else if (catid == "10")
                //{
                //   StrQry += " and a.hoid = '" + operid + "'";
                //}
                //if (txtsearch.Length > 0)
                //{
                //    if (search_type == "0")
                //    {
                //        StrQry += @" and upper(a.account_no)= upper('" + txtsearch + "')";
                //    }
                //    else if (search_type == "1")
                //    {
                //        StrQry += @" and upper(a.vc)= upper('" + txtsearch + "')";
                //    }
                //    else if (search_type == "2")
                //    {
                //        StrQry += @" and upper(a.lco_code)= upper('" + txtsearch + "')";
                //    }
                //}


                StrQry = "( select rownum as rnk,(a.account_no) account_no,6476 hoid,c.num_lcomst_operid operid,a.first_name||' '||a.middle_name||' '||a.last_name fullname, ";
                StrQry += " a.address address,a.vc vc,a.mobile mobile,a.lco_code lco_code,a.lco_name lco_name,a.name planname,(case when b.var_plan_plantype = 'B' then 'Basic' when b.var_plan_plantype = 'AL' then 'Al-la-carte' ";
                StrQry += " when b.var_plan_plantype = 'AD' then 'Addon' else b.var_plan_plantype end) plantype,var_plan_plantype packagetype,TO_CHAR(a.enddate,'DD/MM/YYYY')  enddate,a.account_poid account_poid, ";
                StrQry += " a.product_poid product_poid, a.service_poid service_poid,a.purchase_poid purchase_poid,a.city cityname,c.var_lcomst_brmpoid BRMPOID,a.var_brm_renewflag RenewFlagStatus,a.stb ";
                /*
                StrQry += " from (  select account_no,first_name,middle_name,last_name,";
                StrQry += " address,vc,mobile,lco_code,lco_name,name,enddate,account_poid,product_poid,";
                StrQry += " service_poid,purchase_poid,(select num_city_id from aoup_lcopre_city_def where var_city_name=city )city_id,city,";
                StrQry += " var_brm_renewflag from HWCAS_BRM_CUST_MASTER where enddate between (sysdate-15) and (sysdate+15)";
                StrQry += " )a, (select distinct var_plan_productpoid,num_plan_cityid,var_plan_plantype from aoup_lcopre_plan_def ";
                StrQry += " ) b,aoup_lcopre_lco_det c ";*/
                //Commented By Sandeep on 20_Jul_2016
                StrQry += " from (  select account_no,first_name,middle_name,last_name, ";
                StrQry += " address,vc,mobile,lco_code,lco_name,name,enddate,account_poid,product_poid, ";
                StrQry += " service_poid,purchase_poid,num_lcomst_cityid city_id,city, ";
                StrQry += " var_brm_renewflag,stb from HWCAS_BRM_CUST_MASTER a,aoup_lcopre_lco_Det b,aoup_lcopre_city_def c ";
                StrQry += " where trunc(enddate) between (sysdate-15) and (sysdate+15) ";
                StrQry += " and a.lco_code=b.var_lcomst_code and b.num_lcomst_cityid=c.num_city_id ";
                StrQry += " )a, (select distinct var_plan_productpoid,num_plan_cityid,var_plan_plantype from aoup_lcopre_plan_def   ";
                StrQry += " ) b,aoup_lcopre_lco_det c ";
                if (catid == "10")
                {
                    StrQry += " ,aoup_operator_def j,aoup_operator_def k";
                }
                StrQry += " where  a.product_poid = b.var_plan_productpoid and a.lco_code=c.var_lcomst_code and b.num_plan_cityid=a.city_id";
                if (catid == "10")
                {
                    StrQry += " and c.num_lcomst_operid=j.num_oper_id  and j.num_oper_parentid=k.num_oper_id";
                }
                StrQry += " and trunc(a.enddate) >='" + from + "'";
                StrQry += " and trunc(a.enddate) <='" + to + "'";
                if (plan_name.Trim() != "All")
                {
                    StrQry += " and trim(a.name) = trim('" + plan_name + "')";
                }
                if (package != "All")
                {
                    StrQry += " and b.var_plan_plantype='" + package.ToString() + "'";
                }
                if (catid == "3" || catid == "11")
                {
                    StrQry += " and c.num_lcomst_operid = '" + operid + "'";

                }
                else if (catid == "10")
                {
                    StrQry += " and k.num_oper_parentid = '6476'";
                }
                if (txtsearch.Length > 0)
                {
                    if (search_type == "0")
                    {
                        StrQry += @" and upper(a.account_no)= upper('" + txtsearch + "')";
                    }
                    else if (search_type == "1")
                    {
                        StrQry += @" and upper(a.vc)= upper('" + txtsearch + "')";
                    }
                    else if (search_type == "2")
                    {
                        StrQry += @" and a.lco_code= '" + txtsearch + "'";
                    }
                }
                StrQry += ")";








                // FileLogText("Expiry Query : ", "", StrQry, htAddPlanParams["plan_name"].ToString());

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptExpiry.cs-GetDetails");
                return null;
            }
        }

        public DataTable GetPlans(string from, string to, string username, string operid, string catid) {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "SELECT distinct a.planname" +
                         " FROM view_lcopre_expiry a " +
                         " where a.enddate >='" + from + "' " +
                         " and a.enddate <='" + to + "'";
                if (catid == "3" || catid == "11")
                {
                    StrQry += " and a.operid = '" + operid + "'";
                }
                else if (catid == "10")
                {
                    StrQry += " and a.hoid = '" + operid + "'";
                }

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptExpiry.cs-GetPlans");
                return null;
            }
        }
    }
}
