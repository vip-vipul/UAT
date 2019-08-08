using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Master;

namespace PrjUpassBLL.Master
{
    public class Cls_Business_MstLCOPreUDefine
    {
        public Hashtable GetTransationsLCo(string username, string catid, string operator_id)
        {
            string whereString = "";
            whereString += " a.var_usermst_username= '" + username + "' ";            
            Cls_Data_MstLCOPreUDefine objDataLCOPreUDefine = new Cls_Data_MstLCOPreUDefine();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataLCOPreUDefine.getLCODataLco(whereString, username,catid));
            //htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }

        public Hashtable GetTransations(Hashtable htLCOPreUDefineParams, string username, string catid, string operator_id)
        {
            string whereString = "";
            string lcocd = htLCOPreUDefineParams["lcocd"].ToString();
            string lconm = htLCOPreUDefineParams["lconm"].ToString();
            //string searchParamStr = "";

            if (lcocd != null && lcocd != "")
            {
                whereString += "  upper(lcocode)  like upper('" + lcocd + "%') ";
                //searchParamStr += " <b>Transaction From : </b> " + from;
            }
            else if (lconm != null && lconm != "")
            {
                whereString += "  upper(lconame) like upper('" + lconm + "%') ";
                //searchParamStr += " <b>Transaction To : </b> " + to;
            }
            if (catid == "2")
            {
                whereString += "  and PARENTID= '" + operator_id + "' ";
            }
            else if (catid == "5")
            {
                whereString += "  and DISTID= '" + operator_id + "' ";
            }
            else if (catid == "3")
            {
                whereString += "  and OPERID= '" + operator_id + "' ";
            }

            Cls_Data_MstLCOPreUDefine objDataLCOPreUDefine = new Cls_Data_MstLCOPreUDefine();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataLCOPreUDefine.getLCOData(whereString, username));
            //htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }

        public DataTable getUserDataBll(string whereClauseStr, string username)
        {
            Cls_Data_MstLCOPreUDefine obj = new Cls_Data_MstLCOPreUDefine();
            return obj.getLCOData(whereClauseStr, username);
        }

        public string setUserData(string username, Hashtable ht)
        {
            Cls_Data_MstLCOPreUDefine obj = new Cls_Data_MstLCOPreUDefine();
            return obj.setUserData(username, ht);
        }
        
    }
}
