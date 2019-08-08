using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Master;
using System.Collections;
using System.Data;

namespace PrjUpassBLL.Master
{
    public class Cls_BLL_Dashboard
    {

        public string getDistributorDetails(string username, string oper_id)
        {
            Cls_Data_Dashboard objAsPl = new Cls_Data_Dashboard();
            return objAsPl.getDistData(username, oper_id);
        }

        public DataTable GetSubscriber(string operid, string Catid)
        {
            Cls_Data_Dashboard objAsPl = new Cls_Data_Dashboard();
            return objAsPl.GetSubscriber(operid, Catid);
        }

        public DataTable getExpiry(string username, string operatorId, string caterId)
        {

            Cls_Data_Dashboard obexp = new Cls_Data_Dashboard();

            return obexp.getExpiryData(username, operatorId, caterId);
        }


        public DataTable getChart(string username, string operatorId, string caterId)
        {

            Cls_Data_Dashboard obChart = new Cls_Data_Dashboard();

            return obChart.getchartData(username, operatorId, caterId);
        }

        public DataTable getRetention(string username, string operatorId, string caterId)
        {

            Cls_Data_Dashboard obretention = new Cls_Data_Dashboard();

            return obretention.getRetentiondata(username, operatorId, caterId);
        }

        public DataTable getOverVIew(string username, string operatorId, string caterId)
        {

            Cls_Data_Dashboard obOverview = new Cls_Data_Dashboard();

            return obOverview.getOverviewdata(username, operatorId, caterId);
        }
    }
}
