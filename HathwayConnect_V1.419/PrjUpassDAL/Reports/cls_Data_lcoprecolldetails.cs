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
    public class cls_Data_lcoprecolldetails
    {
        public DataTable lcoprecolldetails(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            try
            {
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string search_type = htAddPlanParams["search"].ToString().Trim();
                string txtsearch = htAddPlanParams["txtsearch"].ToString().Trim();
                
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                 StrQry = "SELECT a.account_no, a.customer_name, a.entity_code, a.lco_name, a.city, " +
                          " a.state, a.area, a.receipt_no, a.amount, a.receipt_date, " +
                          " a.reversal_date, a.created_by_username, a.description, " +
                          " a.customer_type, a.payment_mode, a.cheque_no, a.cheque_date," +
                          " a.bank_name, a.branch_name, a.bank_code, a.payment_channel,  " +
                          " a.upass_reciept_no, a.reversal_status, a.jv, a.distributer, " +
                          " a.SUB_DISTRIBUTER, a.company, a.report_date, a.from_date, a.to_date,a.vc" +
                          " FROM view_splcoprecolldetails a " +
                          " where Trunc(a.date1) >='" + from + "'" +
                          " and  Trunc(a.date1) <='" + to + "'";

              
                if (catid == "3")
                {
                    StrQry += " and a.entity_code = '" + username + "'";
                }

                if (txtsearch.Length > 0)
                {
                    if (search_type == "0")
                    {
                        StrQry += @" and upper(""Account No"")= upper('" + txtsearch + "')";
                    }
                    else if (search_type == "1")
                    {
                        StrQry += @" and upper(a.a.vc)= upper('" + txtsearch + "')";
                    }
                    else if (search_type == "2")
                    {
                        StrQry += @" and upper(""Lco Code"")= upper('" + txtsearch + "')";
                    }
                }
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_Data_lcoprecolldetails.cs-lcoprecolldetails");
                return null;
            }
        }
    }
}
