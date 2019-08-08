using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Reports;
using System.Data;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_RptDownloadAgreement
    {
        public DataTable GetAgreementPath(string username)
        {
            Cls_Data_DownloadAgreement obj = new Cls_Data_DownloadAgreement();
            
            return obj.GetAgreementPath(username);
        }

        public DataTable MIAAgreement(string username)
        {
            Cls_Data_DownloadAgreement obj = new Cls_Data_DownloadAgreement();

            return obj.MIAAgreement(username);
        }
    }
}
