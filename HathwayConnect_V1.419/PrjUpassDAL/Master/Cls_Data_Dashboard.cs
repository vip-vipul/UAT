using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;

namespace PrjUpassDAL.Master
{
    public class Cls_Data_Dashboard
    {

        public string getDistData(string username, string oper_id)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                //fetching name
                string strQry = "select a.var_usermst_name username,s.var_state_name state,c.var_city_name city,l.var_lcomst_code lco_code " +
                " from aoup_lcopre_user_det a ,aoup_lcopre_state_def s ,aoup_lcopre_city_def c,aoup_lcopre_lco_det l " +
                " where a.num_usermst_stateid=s.num_state_id and a.num_usermst_cityid=c.num_city_id and " +
                 " a.var_usermst_username=l.var_lcomst_code and " +
                " num_usermst_operid= " + oper_id;
                string oper_name = "";
                string state = "";
                string City = "";
                string lcocode = "";
                OracleCommand Cmd = new OracleCommand(strQry, conObj);
                conObj.Open();
                OracleDataReader reader = Cmd.ExecuteReader();
                while (reader.Read())
                {
                    oper_name = reader["username"].ToString();
                    state = reader["state"].ToString();
                    City = reader["city"].ToString();
                    lcocode = reader["lco_code"].ToString();
                }
                return oper_name + "~" + state + "~" + City + "~" + lcocode;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_TxnAssignPlan.cs-getDistData");
                return "ex_occured";
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
        }


        public DataTable GetSubscriber(string operid, string Catid)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = " select stake_holder,title,priority, sum(a.col1)as col1,sum(a.col2)as col2,sum(a.col3)as col3 ";
                StrQry += " FROM ";
                StrQry += " (SELECT case when 10=" + Catid + " then  ho_name  when 2=" + Catid + "  then   mso_name   when 5=" + Catid + "  then dist_name ";
                StrQry += " else lconame  end as stake_holder,a.title, a.priority, col1,col2,col3 ";
                StrQry += " FROM view_lcopre_dshbrd_subs a ";
                StrQry += " where case when 10=" + Catid + "  then ho_id when 2=" + Catid + "  then mso when 5=" + Catid + "  then distri else lcoid  end =" + operid + " )a group by stake_holder,title,priority ";
                StrQry += " order by stake_holder,priority asc ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(operid, ex.Message.ToString(), "GetDeatilOverview.cs");
                return null;
            }
        }

        public DataTable getExpiryData(string username, string OpID, string CatId)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            DataTable dt = new DataTable();
            try
            {

                conObj.Open();

                string query = " select stake_holder,title,priority, sum(a.col1)as col1,sum(a.col2)as col2,sum(a.col3)as col3,sum(a.col4)as col4,sum(a.col5)as col5,sum(a.col6)as col6, " +
                " sum(a.col7)as col7,sum(a.col8)as col8,sum(a.col9)as col9, sum(a.col10)as col10,sum(a.col11)as col11,sum(a.col12)as col12, sum(a.col13)as col13,sum(a.col14)as col14, " +
               " sum(a.col15)as col15,sum(a.col16)as col16,sum(a.col17)as col17,sum(a.col18)as col18 FROM (SELECT case when 10='" + CatId + "' then ho_name when 2='" + CatId + "'  then mso_name " +
                " when 5='" + CatId + "'  then dist_name else lconame  end as stake_holder,a.title, a.priority, col1, col2, col3, col4, col5, col6, col7, col8, col9, col10, " +
                  "   col11, col12, col13, col14, col15,col16,col17,col18 FROM view_lcopre_dshbrd_exp a where case when 10='" + CatId + "'   then ho_id " +
                     " when 2='" + CatId + "'  then  mso  when 5='" + CatId + "'  then distri else lcoid  end ='" + OpID + "'" +
                         " ) a group by stake_holder,title,priority order by stake_holder,priority asc ,title asc ";

                Cls_Helper obDt = new Cls_Helper();
                dt = obDt.GetDataTable(query);
                return dt;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_Data_frmdashboard-GetExpiryDetails");
                return dt;

            }
            finally
            {

                conObj.Close();
                conObj.Dispose();
            }
        }


        public DataTable getchartData(string username, string OpID, string CatId)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            DataTable dtChart = new DataTable();
            try
            {

                conObj.Open();

                string query = "   select stake_holder,title,priority, " +
   " sum(a.col1)as col1,sum(a.col2)as col2,sum(a.col3)as col3 FROM " +
   " (SELECT case when 10='" + CatId + "' then ho_name when 2='" + CatId + "'  then mso_name  when 5='" + CatId + "'  then dist_name else lconame  end as stake_holder,a.title,  " +
                " a.priority,col1,col2,col3 FROM view_lcopre_dshbrd_exp a where case when 10='" + CatId + "'  then ho_id  when 2='" + CatId + "'  then mso  " +
    " when 5='" + CatId + "'  then distri else lcoid  end ='" + OpID + "'  and (priority not BETWEEN 7 and 20 and priority<>0) )a group by stake_holder,title,priority " +
    " order by stake_holder,priority asc ,title asc ";



                /*
                string query = " select stake_holder,title,priority, sum(a.col1)as col1,sum(a.col2)as col2,sum(a.col3)as col3, sum(a.col4)as col4,sum(a.col5)as col5, " +
              "  sum(a.col6)as col6, sum(a.col7)as col7,sum(a.col8)as col8,sum(a.col9)as col9, sum(a.col10)as col10,sum(a.col11)as col11,sum(a.col12)as col12, " +
" sum(a.col13)as col13,sum(a.col14)as col14,sum(a.col15)as col15 FROM (SELECT case when 10='" + CatId + "' then ho_name  " +
 " when 2='" + CatId + "'  then mso_name  when 5='" + CatId + "' then dist_name  " +
 " else lconame  end as stake_holder,a.title, a.priority, col1, col2, col3, col4, col5, col6, col7, col8, col9, col10, col11, col12, col13, col14, col15   " +
"  FROM view_lcopre_dshbrd_exp a where   case when 10='" + CatId + "'  then  ho_id  when 2='" + CatId + "'  then   " +
 " mso  when 5='" + CatId + "'  then distri  else lcoid  end ='" + OpID + "'  and priority=1   " +
 " )a group by stake_holder,title,priority order by stake_holder,priority asc ,title asc ";*/

                Cls_Helper obDt = new Cls_Helper();
                dtChart = obDt.GetDataTable(query);
                return dtChart;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_Data_frmdashboard-getChart");
                return dtChart;

            }
            finally
            {

                conObj.Close();
                conObj.Dispose();
            }
        }



        public DataTable getRetentiondata(string username, string OpID, string CatId)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            DataTable dtretention = new DataTable();
            try
            {

                conObj.Open();

                string query = " select stake_holder,title,subpri,priority, sum(a.col1)as col1,sum(a.col2)as col2,sum(a.col3)as col3,sum(a.col4)as col4,sum(a.col5)as col5, " +
              "  sum(a.col6)as col6 FROM (SELECT case when 10='" + CatId + "' then ho_name  when 2='" + CatId + "' then " +
 " mso_name  when 5='" + CatId + "' then dist_name  else lconame  end as stake_holder,a.title,a.subpri, a.priority, col1, col2, col3, col4, col5, col6 " +
 " FROM view_lcopre_dshbrd_ret a where   case when 10='" + CatId + "'  then ho_id   when 2='" + CatId + "'  then " +
 " mso  when 5='" + CatId + "'  then distri else lcoid  end ='" + OpID + "'  )a  group by stake_holder,title,priority,subpri " +
 " order by stake_holder,subpri asc ,priority asc ,title asc ";

                Cls_Helper obDt = new Cls_Helper();
                dtretention = obDt.GetDataTable(query);
                return dtretention;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_Data_frmdashboard-getretention");
                return dtretention;

            }
            finally
            {

                conObj.Close();
                conObj.Dispose();
            }
        }

        public DataTable getOverviewdata(string username, string OpID, string CatId)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            DataTable dtOverview = new DataTable();
            try
            {

                conObj.Open();

                string query = "  select stake_holder,title,priority, sum(col1)as col1,sum(col2)as col2,sum(col3)as col3 FROM (SELECT case when 10='" + CatId + "' then " +
                     " ho_name when 2='" + CatId + "'  then mso_name  when 5='" + CatId + "' then dist_name else lconame  end as stake_holder,a.title, a.priority, col1,col2,col3 " +
                    " FROM view_lcopre_dshbrd_overview a where case when 10='" + CatId + "' then  ho_id when 2='" + CatId + "' then mso when 5='" + CatId + "'  then " +
                    " distri else lcoid  end ='" + OpID + "') group by stake_holder,title,priority order by priority ";

                Cls_Helper obDt = new Cls_Helper();

                dtOverview = obDt.GetDataTable(query);

                return dtOverview;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_Data_frmdashboard-getOverview");
                return dtOverview;

            }
            finally
            {

                conObj.Close();
                conObj.Dispose();
            }
        }
    }
}
