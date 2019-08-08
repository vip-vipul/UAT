using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Master;

namespace PrjUpassBLL.Master
{
    public class Cls_Business_MstLCOPreMSOUDefine
    {
        public Hashtable GetTransations(string companyname, string username)
        {
            string whereString = "";

            whereString += "  upper(var_comp_companyname)  like upper('" + companyname + "%') ";

            Cls_Data_MstLCOPreMSOUDefine objDataLCOPreUDefine = new Cls_Data_MstLCOPreMSOUDefine();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataLCOPreUDefine.getMSOData(whereString, username));

            return htResponse;
        }

        public string setUserData(string username, Hashtable ht)
        {
            Cls_Data_MstLCOPreMSOUDefine obj = new Cls_Data_MstLCOPreMSOUDefine();
            return obj.setUserData(username, ht);
        }
    }
}
