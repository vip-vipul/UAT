using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Master;
using System.Collections;

namespace PrjUpassBLL.Master
{
    public class Cls_Business_MstLCORegistration
    {
        public DataSet getCompDataBll(string username,string catid,string operid,string companyname) {
            Cls_Data_MstLCORegistration obj = new Cls_Data_MstLCORegistration();
            return obj.getCompanyData(username,catid,operid,companyname);
        }

        public string setLCOData(string username, Hashtable ht)
        {
            Cls_Data_MstLCORegistration obj = new Cls_Data_MstLCORegistration();
            return obj.setLCOData(username, ht);
        }

        public Hashtable GetTransations(Hashtable htLCOPreUDefineParams, string username)
        {
            string whereString = "";
            string lcocd = htLCOPreUDefineParams["lcocd"].ToString();
            string lconm = htLCOPreUDefineParams["lconm"].ToString();
            //string searchParamStr = "";

            if (lcocd != null && lcocd != "")
            {
                whereString += "  upper(a.var_hway_lco_code)  like upper('" + lcocd + "%') ";
                //searchParamStr += " <b>Transaction From : </b> " + from;
            }
            else if (lconm != null && lconm != "")
            {
                whereString += "  upper(a.var_hway_lco_name) like upper('" + lconm + "%') ";
                //searchParamStr += " <b>Transaction To : </b> " + to;
            }
           

            Cls_Data_MstLCORegistration objDataLCOPreUDefine = new Cls_Data_MstLCORegistration();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataLCOPreUDefine.getLCOData(whereString, username));
            //htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }
    }
}
