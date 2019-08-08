using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;

namespace PrjUpassDAL.Reports
{
   public  class Cls_Data_rptInvUnusedSTBVC
    {
       public DataTable GetDetails(Hashtable HT,string username)
       { 
           try
           {
               string Type = HT["Type"].ToString();
               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;
               StrQry = "";

               if (Type == "VC")
               {
                   StrQry = "select * from view_lcopre_InvunusedVC where  LcoCode='"+username+"'";
               }
               else
               {
                   StrQry = "select * from view_lcopre_Invunusedstb where  LcoCode='" + username + "'";
               }
           return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptExpiry.cs-GetDetails");
                return null;
            }
       }
    }
}
