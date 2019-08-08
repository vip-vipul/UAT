using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;

namespace PrjUpassDAL.Reports
{
   public class Cls_Data_rptNewActivation
    {
       public DataTable GetNewActivation(Hashtable htAddPlanParams, string username)
       {
           try
           {
               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;

               string whereString = "";
               string from = htAddPlanParams["from"].ToString();
               string to = htAddPlanParams["to"].ToString();
               string State = htAddPlanParams["State"].ToString();
               string City = htAddPlanParams["City"].ToString();
               string JV = htAddPlanParams["JV"].ToString();
               string LCOCode = htAddPlanParams["LCOCode"].ToString();
               if (from != null && from != "")
               {
                   whereString += " and dt >=  '" + from + "' ";
               }
               if (to != null && to != "")
               {
                   whereString += " and dt <=  '" + to + "' ";
               }
               if (State != "All")
               {
                   whereString += " and c.var_state_name= '" + State + "' ";
               }
               if (City != "All" && City != "0")
               {
                   whereString += "  and a.City= '" + City + "' ";
               }
               if (JV != "0")
               {
                   whereString += "  and b.var_lcomst_company= '" + JV + "' ";
               }
               if (LCOCode != "" && LCOCode != null)
               {
                   whereString += "  and LCOCode = '" + LCOCode + "' ";
               }

               StrQry = " select a.cafno,a.owner,a.accno,a.name,a.mobile,a.landline,a.zip,a.vcid,a.stb,dt,b.var_lcomst_company JVNAME,a.city,c.var_state_name state,b.var_lcomst_dasarea Das from ";
               StrQry += " view_rptcrf_details a,aoup_lcopre_lco_det b,view_lcopre_pan_def c where a.owner=b.var_lcomst_code";
               StrQry += " and a.city=c.var_city_name";
               StrQry += " and custflag='WEB'";
               StrQry += " " + whereString;
               return ObjHelper.GetDataTable(StrQry);
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptNewActivation-GetNewActivation");
               return null;
           }
       }

       public DataTable GetSTBSWAPDet(Hashtable htAddPlanParams, string username)
       {
           try
           {
               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;

               string whereString = "";
               string from = htAddPlanParams["from"].ToString();
               string to = htAddPlanParams["to"].ToString();
               string OperID = htAddPlanParams["OperID"].ToString();
               if (from != null && from != "")
               {
                   whereString += "  trunc(INSDATE) >=  '" + from + "' ";
               }
               if (to != null && to != "")
               {
                   whereString += " and trunc(INSDATE) <=  '" + to + "' ";
               }
               if (OperID != "" && OperID != null)
               {
                   whereString += "  and OperID = '" + OperID + "' ";
               }

               StrQry = " select * from view_STB_SWAP_DET where";
               StrQry += " " + whereString;
               StrQry += " order by INSDATE desc"; 
               return ObjHelper.GetDataTable(StrQry);
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptNewActivation-GetSTBSWAPDet");
               return null;
           }
       }

    }
}
