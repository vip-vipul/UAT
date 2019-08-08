using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_rptpartyledopenbal
    {
        public DataTable getpartyledDet(Hashtable htAddPlanParams)
        {
            try
            {
                string city = htAddPlanParams["city"].ToString();
                
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = " SELECT b.var_partled_lcocode,b.var_partled_lconame,b.num_partled_openinbal,(to_char(b.dat_partled_date,'dd-Mon-yyyy hh12:mi:ss AM'))partyleddate " +
                         " FROM   aoup_lcopre_lco_partyled_mst b, aoup_lcopre_lco_det c " +
                         " WHERE   dat_partled_date =(SELECT   MIN (dat_partled_date) " +
                         " FROM   aoup_lcopre_lco_partyled_mst a " +
                         " WHERE   a.var_partled_lcocode = b.var_partled_lcocode) " +
                         " AND c.var_lcomst_code = b.var_partled_lcocode ";

                        if (city.Trim() != "All")
                        {
                            StrQry += " and c.num_lcomst_cityid='" + city.ToString() + "'";
                        }
                        StrQry += " order by b.dat_partled_date";
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb("admin_ho", ex.Message.ToString(), "Cls_Data_rptpartyledopenbal-getpartyledDet");
                return null;
            }
        }

    }
}
