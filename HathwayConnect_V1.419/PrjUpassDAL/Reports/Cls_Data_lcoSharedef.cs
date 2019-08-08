using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;

namespace PrjUpassDAL.Reports
{
   public class Cls_Data_lcoSharedef
    {
        public DataTable getPlanData(Hashtable htAddPlanParams , string Username, string catgr)
        {
            try
            {
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string city = htAddPlanParams["city"].ToString();
                string plan = htAddPlanParams["plan"].ToString();
                string chkPlan = htAddPlanParams["chkPlan"].ToString();
                string chkCity = htAddPlanParams["chkCity"].ToString();
                string chkDt = htAddPlanParams["chkDt"].ToString();
                string addmap = htAddPlanParams["addmap"].ToString();
                string abcmap = htAddPlanParams["abcmap"].ToString();
                string addbasiccity = htAddPlanParams["addbasiccity"].ToString();
                string addbasicplan = htAddPlanParams["addbasicplan"].ToString();

                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                /* StrQry = " SELECT a.var_plan_name, (case when a.var_plan_plantype='B' then 'Basic' " +
                          " when a.var_plan_plantype='AL' then 'A-La-Carte' when a.var_plan_plantype='AD' then 'Addon' end) var_plan_plantype," +
                          " a.var_plan_planpoid, a.var_plan_dealpoid, a.var_plan_productpoid, " +
                          " a.num_plan_custprice, a.num_plan_lcoprice,b.var_city_name " +
                          " FROM aoup_lcopre_plan_def a,aoup_lcopre_city_def b " +
                          " WHERE a.num_plan_cityid=b.num_city_id " +
                          " ORDER by b.var_city_name,a.var_plan_name";
                 */
                StrQry = "";

                if (addmap == "1")
                {
                    StrQry = " SELECT a.var_plan_name, 'Basic'var_plan_plantype, a.var_plan_planpoid, " +
                             " a.var_plan_dealpoid, a.var_plan_productpoid, " +
                             " a.num_plan_custprice, a.num_plan_lcoprice, a.var_city_name, a.LCO_Code, a.LCO_NAME " +
                             " FROM VIEW_LCO_SHARE_DEF a " +
                             " where 0=0 ";
                }
                else if (abcmap == "1")
                {
                    StrQry = " SELECT a.var_plan_name, 'Add On'var_plan_plantype, a.var_plan_planpoid, " +
                                                " a.var_plan_dealpoid, a.var_plan_productpoid, " +
                                                " a.num_plan_custprice, a.num_plan_lcoprice, a.var_city_name, a.LCO_Code, a.LCO_NAME " +
                                                " FROM VIEW_LCO_SHARE_DEF a,aoup_lcopre_lcoplan_def  b " +
                                                " where 0=0 and b.num_plan_plantypeid=a.num_plan_plantypeid" +
                                                " and a.num_city_id=b.num_plan_cityid ";
                }
                else if (plan.Trim() == "AD")
                {
                    StrQry = " SELECT a.var_plan_name, 'Add On'var_plan_plantype, a.var_plan_planpoid, " +
                                                " a.var_plan_dealpoid, a.var_plan_productpoid, " +
                                                " a.num_plan_custprice, a.num_plan_lcoprice, a.var_city_name, a.LCO_Code, a.LCO_NAME " +
                                                " FROM VIEW_LCO_SHARE_DEF a,aoup_lcopre_lcoplan_def  b " +
                                                " where 0=0 and b.num_plan_plantypeid=a.num_plan_plantypeid" +
                                                " and a.num_city_id=b.num_plan_cityid ";
                }
                else
                {
                    StrQry = " SELECT a.var_plan_name, a.var_plan_plantype, a.var_plan_planpoid, " +
                                                " a.var_plan_dealpoid, a.var_plan_productpoid, " +
                                                " a.num_plan_custprice, a.num_plan_lcoprice, a.var_city_name, a.LCO_Code, a.LCO_NAME " +
                                                " FROM VIEW_LCO_SHARE_DEF a " +
                                                " where 0=0 ";
                }

                if (chkDt == "1")
                {
                    StrQry += "  and a.dt >='" + from + "'";
                    StrQry += "  and a.dt <='" + to + "'";
                }


                if (addmap == "1")
                {
                    StrQry += " and a.num_city_id='" + addbasiccity.ToString() + "'";
                    StrQry += " and num_plan_plantypeid='" + addbasicplan + "'";
                    StrQry += " and a.plantype in('AD','GAD','RAD')";
                }
                else if (abcmap == "1")
                {
                    StrQry += " and a.num_city_id='" + addbasiccity.ToString() + "'";
                    StrQry += " and upper(b.var_plan_name) like upper('%" + addbasicplan + "%')";
                    StrQry += " and a.plantype='B'";
                }
                else
                {
                    if (chkPlan == "1")
                    {
                        if (plan.Trim() != "All")
                        {
                            if (plan.Trim() == "AD")
                            {
                                StrQry += " and a.plantype in('AD','GAD','RAD')";
                            }
                            else
                            {
                                StrQry += " and a.plantype='" + plan.ToString() + "'";
                            }

                        }

                    }
                    if (chkCity == "1")
                    {
                        if (city.Trim() != "All")
                        {
                            StrQry += " and a.num_city_id='" + city.ToString() + "'";
                        }
                    }

                    StrQry += "  and a.lco_code='"+Username.ToString()+"' ";
                    
                    StrQry += " group by a.var_plan_name, a.var_plan_plantype, a.var_plan_planpoid,";
                    StrQry += "  a.var_plan_dealpoid, a.var_plan_productpoid, a.num_plan_custprice,a.num_plan_lcoprice, a.var_city_name, a.LCO_Code, a.LCO_NAME order by a.var_plan_name";

                }

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb("admin_ho", ex.Message.ToString(), "Cls_Data_lcoSharedef-getPlanData");
                return null;
            }
        }
    }
}
