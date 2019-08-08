using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class cls_Business_rptLcowiseUserdetails
    {
        public DataTable getLcodetails(string username, string catid, string operid)
        {

            string StrQry = "SELECT a.msoid, a.msoname, a.distid, a.distname, a.lcoid, a.lcocode, a.lconame, a.transdt, a.usercnt, a.cityname, a.statename,a.companyname, a.distributor, a.subdistributor" +
                            " FROM view_lcopre_userdet_det a";
            if (catid == "2")
            {
                StrQry += " where a.msoid ='" + operid + "'";
            }

            if (catid == "5")
            {
                StrQry += " where a.distid ='" + operid + "'";
            }

            if (catid == "3" || catid == "11")
            {
                StrQry += " where a.lcoid ='" + operid + "'";
            }

            if (catid == "10")
            {
                StrQry += " where a.hoid ='" + operid + "'";
            }

            StrQry += " order by a.transdt desc";

            cls_data_rptUserdetailsLcowise data = new cls_data_rptUserdetailsLcowise();

            DataTable dt = data.GetLcodet(username, StrQry);
            return dt;
        }

        public DataTable getLcouserdetails(string username, string operid,string Showall,string catid,string operatorid)
        {
            string StrQry = "";

            StrQry = "SELECT a.userid, a.operid, a.username, a.userowner, a.fname, a.mname, a.lname, a.addr, a.code, a.brmpoid, a.ststeid, a.cityid, a.email, " +
                               " a.mobno, a.compcode, a.accno, a.insby, a.insdt, a.flag, a.balance, a.lcocode, a.DASAREA " +
                               " FROM view_Lcopre_user_det a";
            if (Showall == "1")
            {

                if (catid == "3" || catid == "11")
                {

                    StrQry += " where a.operid ='" + operatorid + "'";
                }
                else
                {
                    StrQry += " where a.operid ='" + operid + "'";
                }


            }
            else
            {


                if (catid == "2")
                {
                    StrQry += " where a.parentid='" + operatorid + "'";
                }
                else if (catid == "5")
                {
                    StrQry += " where a.distid='" + operatorid + "'";
                }
                else if (catid == "10")
                {
                    StrQry += " where  a.hoid='" + operatorid + "'";
                }
            }

            cls_data_rptUserdetailsLcowise data = new cls_data_rptUserdetailsLcowise();

            DataTable dt = data.GetLcodet(username, StrQry);
            return dt;
        }

        public DataTable getLcoSearch(string username, string search, string flag, string catid, string operid)
        {

            string StrQry = "SELECT a.msoid, a.msoname, a.distid, a.distname, a.lcoid, a.lcocode, a.lconame, a.transdt, a.usercnt, a.cityname, a.statename,a.companyname, a.distributor, a.subdistributor" +
                           " FROM view_lcopre_userdet_det a";
            if (catid == "2")
            {
                StrQry += " where a.msoid ='" + operid + "'";
                if (flag == "0")
                {
                    StrQry += " and a.lcocode ='" + search + "'";
                }
                else if (flag == "1")
                {
                    StrQry += " and upper(a.lconame) like upper('" + search + "%')";
                }
            }

            if (catid == "5")
            {
                StrQry += " where a.distid ='" + operid + "'";
                if (flag == "0")
                {
                    StrQry += " and a.lcocode ='" + search + "'";
                }
                else if (flag == "1")
                {
                    StrQry += " and upper(a.lconame) like upper('" + search + "%')";
                }
            }

            if (catid == "3" || catid == "11")
            {
                StrQry += " where a.lcoid ='" + operid + "'";
                if (flag == "0")
                {
                    StrQry += " and a.lcocode ='" + search + "'";
                }
                else if (flag == "1")
                {
                    StrQry += " and upper(a.lconame) like upper('" + search + "%')";
                }
            }

            if (catid == "10")
            {
                StrQry += " where a.hoid ='" + operid + "'";
                if (flag == "0")
                {
                    StrQry += " and a.lcocode ='" + search + "'";
                }
                else if (flag == "1")
                {
                    StrQry += " and upper(a.lconame) like upper('" + search + "%')";
                }
            }


            StrQry += " order by a.transdt desc";

            cls_data_rptUserdetailsLcowise data = new cls_data_rptUserdetailsLcowise();

            DataTable dt = data.GetLcodet(username, StrQry);
            return dt;
        }

        //------- Added By RP on 25.12.2017 from LCO Sub User Details
        public DataTable getLcoSubDetails(string username,string catid,string operid)
        {
            string StrQry = "select username,fname||' '||mname||' '||lname Name,addr,email,mobno from view_Lcopre_user_det where operid='" + operid + "' and flag='Executive' ";

            cls_data_rptUserdetailsLcowise data = new cls_data_rptUserdetailsLcowise();

            DataTable dt = data.GetLcodet(username, StrQry);
            return dt;
        }

        // ------  Added By RP on 25.12.2017 from LCO Sub User Details Dropdown
        public DataTable getLcoSub(string username, string catid, string operid)
        {
            string StrQry = "select username,fname||' '||mname||' '||lname Name from view_Lcopre_user_det where operid='" + operid + "' and flag='Executive' ";

            cls_data_rptUserdetailsLcowise data = new cls_data_rptUserdetailsLcowise();

            DataTable dt = data.GetLcodet(username, StrQry);
            return dt;
        }

        public DataTable getLcoSubRight_Details(string username, string catid, string operid)
        {
            string StrQry = "select * FROM view_UserBtn_Right where var_access_username='"+username+"' ";

            cls_data_rptUserdetailsLcowise data = new cls_data_rptUserdetailsLcowise();

            DataTable dt = data.GetLcodet(username, StrQry);
            return dt;
        }
    }
}
