using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_DownloadAgreement
    {
        public DataTable GetAgreementPath(string username)
        {
            try
            {
                Cls_Helper obj= new Cls_Helper();
                string StrQry;
                StrQry = "select var_agreement_sia_path,var_agreement_mia_path from aoup_lcopre_agreement a,aoup_lcopre_lco_det b ";
                StrQry += " where a.num_agreement_cityid=b.num_lcomst_cityid ";
                StrQry += " and var_lcomst_code='" + username + "'";
                return obj.GetDataTable(StrQry);
                
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_DownloadAgreement.cs-GetAgreementPath");
                return null;
            }
        }

        public DataTable MIAAgreement(string username)
        {
            try
            {
                Cls_Helper obj = new Cls_Helper();
                string StrQry;
                StrQry = "SELECT * FROM aoup_lcopre_lco_det a ,aoup_operator_def c,aoup_user_def u,aoup_lcopre_miadetails b ";
StrQry += " WHERE a.num_lcomst_operid = c.num_oper_id and  a.num_lcomst_operid=u.num_user_operid and a.var_lcomst_code=u.var_user_username";
StrQry += " and u.var_user_username=b.var_miadetails_lcocode ";
StrQry += " and num_lcomst_operid='" + username + "'";
                return obj.GetDataTable(StrQry);

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_DownloadAgreement.cs-GetAgreementPath");
                return null;
            }
        }
    }
}
