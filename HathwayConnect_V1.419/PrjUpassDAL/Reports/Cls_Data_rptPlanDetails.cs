using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_rptPlanDetails
    {
        public DataTable getPlanData()
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = " SELECT a.var_plan_name, a.var_plan_plantype, a.var_plan_planpoid, " +
                         " a.var_plan_dealpoid, a.var_plan_productpoid, " +
                         " a.num_plan_custprice, a.num_plan_lcoprice, a.var_city_name, a.AREA " +
                         " FROM view_lcopre_plan_det a ";
                    /*
                    " SELECT a.var_plan_name, (case when a.var_plan_plantype='B' then 'Basic' " +
                         " when a.var_plan_plantype='AL' then 'A-La-Carte' when a.var_plan_plantype='AD' then 'Addon' end) var_plan_plantype," +
                         " a.var_plan_planpoid, a.var_plan_dealpoid, a.var_plan_productpoid, " +
                         " a.num_plan_custprice, a.num_plan_lcoprice,b.var_city_name " +
                         " FROM aoup_lcopre_plan_def a,aoup_lcopre_city_def b " +
                         " WHERE a.num_plan_cityid=b.num_city_id " +
                         " ORDER by b.var_city_name,a.var_plan_name";
                    */
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb("admin_ho", ex.Message.ToString(), "Cls_Data_rptPlanDetails-getPlanData");
                return null;
            }
        }

    }
}
