using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Master;
using System.Data;
using System.Collections;

namespace PrjUpassBLL.Master
{
   public class Cls_Business_mstComplaintRegistration
    {
        public string[] getLcodetails(string username, string LcoName, string type, string operid, string catid)
        {
            Cls_Data_mstComplaintRegistration obj = new Cls_Data_mstComplaintRegistration();
            return obj.GetLcoDetails(username, LcoName, type, operid, catid);
        }

        public string InsertComplaint(Hashtable ht, string username)
        {
            Cls_Data_mstComplaintRegistration obj = new Cls_Data_mstComplaintRegistration();
            return obj.InsertComplaint(ht, username);
        }
       

    }
}
