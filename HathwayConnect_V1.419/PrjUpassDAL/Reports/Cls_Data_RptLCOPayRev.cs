using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_RptLCOPayRev
    {
        public DataTable GetPaymentRev(string whereClauseStr, string username, string operid, string cat)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT a.num_lcopay_transid, a.var_lcopay_voucherno, ";
                StrQry += "  a.num_lcopay_amount, a.var_reason_name, a.var_lcopay_remark, ";
                StrQry += " a.var_lcopay_companycode, a.var_lcopay_insby, a.date1, ";
                StrQry += " a.dat_lcopay_insdt, a.var_lcopay_lcocode, a.var_lcomst_name, ";
                StrQry += " a.dat_lcopay_chequebouncedt, a.num_oper_id, a.num_oper_parentid, ";
                StrQry += " a.num_oper_distid, a.hoid, a.cityname, a.statename, ";
                StrQry += " a.companyname, a.distributor, a.subdistributor, a.payment_dt, a.payment_mode, ";
                StrQry += " a.bank, a.branch, a.cashier, a.cheque_no ";
                StrQry += " FROM view_lco_payement_revoke a ";

                StrQry += " where " + whereClauseStr;


                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLCOPayRev.cs");
                return null;
            }
        }
    }
}
