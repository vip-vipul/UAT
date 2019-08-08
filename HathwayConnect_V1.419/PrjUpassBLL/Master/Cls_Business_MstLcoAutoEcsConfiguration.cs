using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PrjUpassDAL.Master;

namespace PrjUpassBLL.Master
{
  public  class Cls_Business_MstLcoAutoEcsConfiguration
    {
        public Hashtable GetTransations(Hashtable htLCOPreUDefineParams, string username)
        {
            


            string whereString = "";
            string lcocd = htLCOPreUDefineParams["lcocd"].ToString();
            string lconm = htLCOPreUDefineParams["lconm"].ToString();
            //string searchParamStr = "";

            if (lcocd != null && lcocd != "")
            {
                whereString += "  upper(a.var_lcomst_code)  like upper('" + lcocd + "%') ";
                //searchParamStr += " <b>Transaction From : </b> " + from;
            }
            else if (lconm != null && lconm != "")
            {
                whereString += "  upper(a.var_lcomst_name) like upper('" + lconm + "%') ";
                //searchParamStr += " <b>Transaction To : </b> " + to;
            }


            Cls_Data_MstLcoAutoEcsConfiguration objDataLCOPreUDefine = new Cls_Data_MstLcoAutoEcsConfiguration();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataLCOPreUDefine.getLCOData(whereString, username));
            //htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }

        public string setLCOData(string username, Hashtable ht)
        {
            Cls_Data_MstLcoAutoEcsConfiguration obj = new Cls_Data_MstLcoAutoEcsConfiguration();
            return obj.setLCOData(username, ht);
        }
    }

}
