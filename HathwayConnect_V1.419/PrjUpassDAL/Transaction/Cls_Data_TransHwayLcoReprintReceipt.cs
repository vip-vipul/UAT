using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;

namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_TransHwayLcoReprintReceipt
    {
        public DataTable GetLcopaymentDetails(string username, string prefixText, string category, string operid, string type, string BankName)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            DataTable dt = new DataTable();
            Cls_Helper ObjHelper = new Cls_Helper();
            try
            {

                string str = "";
                str = " SELECT a.lcocode, a.lconame, a.username, a.address, a.mobno, a.email, a.amt,a.name, a.paymode, a.bnknm, a.branch, a.receiptno, a.bank, a.chqddno, a.paydt, a.remrk,";
                str += " a.chequedt, a.operid, a.opercategory, a.parentid, a.distid, a.hoid,a.isactive, a.company";
                str += " FROM view_lcopre_payment_reprint a";

                if (type == "0")
                {
                    str += " where a.receiptno='" + prefixText + "'";
                }
                else
                {
                    str += " where a.lcocode='" + prefixText + "'";
                    str += " and a.isactive='Y'";
                }
                if (category == "2")
                {
                    str += " and a.parentid='" + operid.ToString() + "'  ";
                    str += " and  a.username ='" + username + "'";
                }
                else if (category == "5")
                {
                    str += " and a.distid='" + operid.ToString() + "'  ";
                }
                else if (category == "10")
                {
                    str += " and a.hoid='" + operid.ToString() + "'  ";
                }
                str += " and rownum <= 5";
                dt = ObjHelper.GetDataTable(str);

                return dt;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TransHwayLcoReprintReceipt-GetLcopaymentDetails");
                return dt;
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }
    }
}
