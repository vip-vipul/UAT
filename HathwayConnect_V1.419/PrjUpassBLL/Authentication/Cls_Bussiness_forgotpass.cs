using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PrjUpassDAL.Authentication;

namespace PrjUpassBLL.Authentication
{
    public class Cls_Bussiness_forgotpass
    {
        public string ForgotDetails(Hashtable ht)
        {
            Cls_Data_forgotpass obj = new Cls_Data_forgotpass();
            return obj.ForgotDetails(ht);
        }
    }
}
