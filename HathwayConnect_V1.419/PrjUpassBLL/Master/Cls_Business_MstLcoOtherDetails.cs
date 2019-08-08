using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Master;

namespace PrjUpassBLL.Master
{
    public class Cls_Business_MstLcoOtherDetails
    {
        public string setLcoData(string username, Hashtable ht)
        {
            Cls_Data_MstLcoOtherDetails obj = new Cls_Data_MstLcoOtherDetails();
            return obj.setLcoData(username, ht);
        }
    }
}
