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
    public class Cls_Data_rptExpired
    {
        public DataTable GetDetails(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            try
            {
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string plan_name = "All";
                string accvctxt = "";
                string accvc = "";
                if (htAddPlanParams["accvctxt"] != null)
                { //in case of expiry report, this will be null
                    accvctxt = htAddPlanParams["accvctxt"].ToString();
                    accvc = htAddPlanParams["accvc"].ToString();
                };
                if (htAddPlanParams["plan_name"] != null)
                { 
                
                }
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "SELECT a.account_no, a.fullname, a.address, a.vc, a.mobile, a.lco_code, a.lco_name, a.planname, a.plantype, to_char(a.enddate, 'dd-Mon-yyyy') enddate, a.account_poid," +
                         "  a.cityname " +
                         " FROM view_lcopre_expired_rpt_new a  " +
                         " where a.enddate >='" + from + "'" +
                         " and a.enddate <='" + to + "'";
                if (plan_name.Trim() != "All")
                {
                    StrQry += " and trim(a.planname) = trim('" + plan_name + "')";
                }
                if (accvctxt != "")
                {
                    if (accvc == "ACC")
                    {
                        StrQry += " and ACCOUNT_NO='" + accvctxt.ToString() + "'";  
                    }
                    else if (accvc == "VC")
                    {
                        StrQry += " and vc='" + accvctxt.ToString() + "'";
                    }
                }
                if (catid == "3" || catid == "11")
                {
                    StrQry += " and a.LCO_CODE = '" + operid + "'";
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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptExpired.cs-GetDetails");
                return null;
            }
        }
    }
}
