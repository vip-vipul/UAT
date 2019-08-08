using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
   public class Cls_Business_RptSelfCareDetails
    {
       public DataTable getSelfCareDetails(string FromDate, string ToDate, string LCOCode)
       {
           string StrQry = "select * from hw_payment_whatson ";
           StrQry += "where transaction_date between'"+FromDate+"' and '"+ToDate+"'";
           StrQry += "and lco_code ='" + LCOCode + "'";
           cls_data_RptSelfCareDetails data = new cls_data_RptSelfCareDetails();

           DataTable dt = data.GetSelfCareDetails(StrQry);
           return dt;
       }
    }
}
