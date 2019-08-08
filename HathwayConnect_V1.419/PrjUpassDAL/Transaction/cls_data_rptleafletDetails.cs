using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;

namespace PrjUpassDAL.Transaction
{
   public class cls_data_rptleafletDetails
    {
       public DataTable getLeaflet(string username , string State , string area)
       {
           try
           {
               string _getfile = "select * from view_lcopre_leaflet where state='" + State + "' and area='" + area + "' ";
               Cls_Helper ob = new Cls_Helper();


               return ob.GetDataTable(_getfile);
           }
           catch (Exception ex)
           {
               Cls_Security ob = new Cls_Security();
               ob.InsertIntoDb(username , ex.ToString(), "cls_data_rptleafletDetails");

               return null;
           }
       
       }

       public void GetCity(string username, out string city, out string State, out string area)
       {
           city = "";
           area = "";
           State = "";
           string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
           OracleConnection conObj = new OracleConnection(ConStr);
           try
           {
              
               string strQry = "select b.num_usermst_cityid cityid,a.var_lcomst_dasarea dasarea,a.num_lcomst_operid operid , num_lcomst_stateid state from aoup_lcopre_lco_det a,aoup_lcopre_user_det b ";
               strQry += " where a.num_lcomst_operid=b.num_usermst_operid ";
               strQry += " and b.var_usermst_username='" + username + "'";

               OracleCommand cmd = new OracleCommand(strQry, conObj);
               conObj.Open();

               OracleDataReader dr = cmd.ExecuteReader();

               while (dr.Read())
               {
                   city = dr["cityid"].ToString();
                   area = dr["dasarea"].ToString();
                   State = dr["state"].ToString();
               }
               conObj.Close();
           }
           
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_TransHwayLcoPayment-getTaxDetails");

           }
           finally
           {
               conObj.Close();
               conObj.Dispose();
           }
       }
    }
}
