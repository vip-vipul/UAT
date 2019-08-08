using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Transaction;

namespace PrjUpassBLL.Transaction
{
  public  class cls_business_rptleafletdetails
    {
      public DataTable getleaflet(string username , string state , string area)
      {
          cls_data_rptleafletDetails ob = new cls_data_rptleafletDetails();
          return ob.getLeaflet(username , state , area);
      
      }


      public void GetCity(string username, out string city, out string state, out string area)
      {
          city = "";
          area = "";
          state = "";
          cls_data_rptleafletDetails obj = new cls_data_rptleafletDetails();
          obj.GetCity(username, out city, out state, out area);

      }


    }
}
