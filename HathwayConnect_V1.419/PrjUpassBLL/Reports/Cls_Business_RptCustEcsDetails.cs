using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
  public  class Cls_Business_RptCustEcsDetails
    {
        public DataTable getCustEcsDetails(string username, string catid, string operid,string Isactive,string hdnslctcols)
        {

            string StrQry = "select " + hdnslctcols + " from View_aoup_lcopre_ecs_det a where upper(a.lcocode)= upper('" + username + "') ";
            if (Isactive == "Y")
            {
                StrQry += " and is_active ='Active' ";
            }
                            //" FROM view_lcopre_userdet_det a
            cls_data_RptCustEcsDetails data = new cls_data_RptCustEcsDetails();

            DataTable dt = data.GetCustEcsDetails(username, StrQry);
            return dt;
        }
    }
}
