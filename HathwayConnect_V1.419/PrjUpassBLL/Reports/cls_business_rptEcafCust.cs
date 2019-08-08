using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Helper;

namespace PrjUpassBLL.Reports
{
    public class cls_business_rptEcafCust
    {

        public Hashtable getCrfdata(string username, Hashtable ht)
        {
            string whereString = "";
            string from = ht["from"].ToString();
            string to = ht["to"].ToString();
            string searchParamStr = "";

            if (from != null && from != "")
            {
                whereString += "  trunc(dt) >=  '" + from + "' ";
                searchParamStr += " <b>Transaction From : </b> " + from;
            }
            if (to != null && to != "")
            {
                whereString += " and trunc(dt) <=  '" + to + "' ";
                searchParamStr += " <b>Transaction To : </b> " + to;
            }
            whereString += " and  a.owner='" + username + "' ";

            string _getdata = "select a.owner , count(*) total  from view_rptcrf_details a where " + whereString + " group by a.owner";
            Cls_Helper obj = new Cls_Helper();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("data", obj.GetDataTable(_getdata));
            htResponse.Add("ParamStr", searchParamStr);

            return htResponse;

        }



        public Hashtable getCrfVC(string username, string vc)
        {
            string whereString = "";

            whereString += " and  a.owner='" + username + "' ";
            string searchParamStr = "By VC" + vc;

            string _getdata = "select a.owner , count(*) total  from view_rptcrf_details a  where a.vcid = '" + vc + "' group by a.owner";
            Cls_Helper obj = new Cls_Helper();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("data", obj.GetDataTable(_getdata));
            htResponse.Add("ParamStr", searchParamStr);

            return htResponse;

        }



        public Hashtable getReport(string username, Hashtable ht)
        {
            string whereString = "";

            string from = ht["from"].ToString();
            string to = ht["to"].ToString();
            string searchParamStr = "";

            if (from != null && from != "")
            {
                whereString += "  trunc(dt) >=  '" + from + "' ";
                searchParamStr += " <b>Transaction From : </b> " + from;
            }
            if (to != null && to != "")
            {
                whereString += " and trunc(dt) <=  '" + to + "' ";
                searchParamStr += " <b>Transaction To : </b> " + to;
            }

            whereString += " and  a.owner='" + username + "' ";

            string _getdata = "select *  from view_rptcrf_details a where " + whereString + " ";
            Cls_Helper obj = new Cls_Helper();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("data", obj.GetDataTable(_getdata));
            htResponse.Add("ParamStr", searchParamStr);

            return htResponse;
        }



        public Hashtable getReportVC(string username, string vc)
        {
            // string whereString = "";
            string searchParamStr = "";
            // whereString += " and  a.owner='" + username + "' ";

            string _getdata = "select *  from view_rptcrf_details a  where a.vcid = '" + vc + "' ";
            Cls_Helper obj = new Cls_Helper();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("data", obj.GetDataTable(_getdata));
            htResponse.Add("ParamStr", searchParamStr);

            return htResponse;
        }

    }



}
