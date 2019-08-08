using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Master;

namespace PrjUpassBLL.Master
{
    public class cls_BLL_messenger
    {

        public void GetCity(string username, out string city, out string state, out string Userlevel, out string cityuser)
        {
            city = "";
            Userlevel = "";
            state = "";
            cls_data_messenger obj = new cls_data_messenger();
            obj.GetCity(username, out city, out state, out Userlevel, out  cityuser);
        }

        public DataTable fillinbox(string username, string city, string Userlevel, string state, string from, string subject)
        {
            cls_data_messenger obj = new cls_data_messenger();

            return obj.fillinbox(username, city, Userlevel, state, from, subject);
        }

        public DataTable fillsentMsgs(string username, string from, string subject)
        {
            cls_data_messenger obj = new cls_data_messenger();

            return obj.fillsentMsgs(username, from, subject);
        }

        public string sentMailDatains(string username, Hashtable htdata)
        {
            cls_data_messenger obj = new cls_data_messenger();

            return obj.sentMailDatains(username, htdata);

        }

        public string OpenMail(string username, String MainID, String SubId, String Readby)
        {
            cls_data_messenger obj = new cls_data_messenger();

            return obj.OpenMail(username, MainID, SubId, Readby);

        }


    }
}
