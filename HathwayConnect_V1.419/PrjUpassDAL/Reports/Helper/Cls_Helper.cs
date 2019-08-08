using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Web;


namespace PrjUpassDAL.Helper
{
    public class Cls_Helper
    {

        string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();

        public DataTable GetDataTable(string QueryStr)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            DataTable DtObj = new DataTable();
            try
            {
                OracleCommand Cmd = new OracleCommand(QueryStr, ConObj);
                OracleDataAdapter DaObj = new OracleDataAdapter(Cmd);
                DaObj.Fill(DtObj);
                return DtObj;
            }
            catch (Exception ex)
            {
                return DtObj;
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public static DataSet fnGetdataset(string st)
        {

            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(ConStr);
            DataSet ds = new DataSet();
            try
            {
                OracleDataAdapter da = new OracleDataAdapter(st, conn);
                
                da.Fill(ds);
                da.Dispose();
                conn.Close();
                conn.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static DataSet Comboupdate(string tablename, string IDField, string DisplayField)
        {
            string st;
            //string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            //OracleConnection conn = new OracleConnection(ConStr);
            st = " SELECT DISTINCT " + IDField + "," + DisplayField + " FROM " + tablename;


            DataSet ds = fnGetdataset(st);

            return ds;

        }

        public int insertQry(String sb)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            int returnValue = 0;
            try
            {
                OracleCommand cmd = new OracleCommand(sb, ConObj);
                ConObj.Open();
                returnValue = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return returnValue;
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
            return returnValue;
        }

        public static string fnGetScalar(string st)
        {

            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(ConStr);
            string str = "";
            try
            {
                OracleCommand command = new OracleCommand(st, conn);
                command.Connection.Open();
                str = command.ExecuteScalar().ToString();


                return str;
            }
            catch (Exception ex)
            {
                return str;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //----
        public static object DropDownFill(System.Web.UI.WebControls.DropDownList Dropdown, String TableName, String DisplayField, String ValueField, String Condition, String SqlQuery)
        {
            OracleConnection connection = new OracleConnection();
            String query;
            String DISTINCT = "";

            if (ValueField != null && ValueField.ToUpper().Contains("DISTINCT"))
            {
                DISTINCT = "Y";
            }

            if (SqlQuery == "")
            {


                if (Condition == "" || Condition == null)
                {
                    query = "select " + DisplayField + ", " + ValueField + " from " + TableName;
                }
                else
                {
                    query = "select " + DisplayField + ", " + ValueField + " from " + TableName + " where " + Condition;
                }
            }

            else
            {
                query = SqlQuery;
            }
            connection = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());

            connection.Open();

            OracleCommand cmd = new OracleCommand(query, connection);
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("DisplayField", typeof(String));
            dt.Columns.Add("ValueField", typeof(String));
            dt.Rows.Add("-- Select Option --", 0);
            while (dr.Read())
            {
                dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
            }

            Dropdown.DataSource = dt;
            Dropdown.DataTextField = dt.Columns[0].ToString();
            Dropdown.DataValueField = dt.Columns[1].ToString();
            Dropdown.DataBind();
            connection.Close();
            return Dropdown;
        }

        public static object DropDownFillState(System.Web.UI.WebControls.DropDownList Dropdown)
        {
            OracleConnection connection = new OracleConnection();
            String query;
            String DISTINCT = "";

            query = "Select var_state_name,num_state_id from AOUP_LCOPRE_STATE_DEF order by var_state_name";

            connection = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());

            connection.Open();

            OracleCommand cmd = new OracleCommand(query, connection);
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("var_state_name", typeof(String));
            dt.Columns.Add("num_state_id", typeof(String));
            dt.Rows.Add("-- Select State --", 0);
            while (dr.Read())
            {
                dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
            }

            Dropdown.DataSource = dt;
            Dropdown.DataTextField = dt.Columns[0].ToString();
            Dropdown.DataValueField = dt.Columns[1].ToString();
            Dropdown.DataBind();
            connection.Close();
            return Dropdown;
        }

        public static object DropDownFillCity(System.Web.UI.WebControls.DropDownList Dropdown, string stateid)
        {
            OracleConnection connection = new OracleConnection();
            String query;
            String DISTINCT = "";

            query = "Select var_city_name,num_city_id from AOUP_LCOPRE_CITY_DEF where num_city_stateid = " + stateid + " order by var_city_name";

            connection = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());

            connection.Open();

            OracleCommand cmd = new OracleCommand(query, connection);
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("var_city_name", typeof(String));
            dt.Columns.Add("num_city_id", typeof(String));
            dt.Rows.Add("-- Select City --", 0);
            while (dr.Read())
            {
                dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
            }

            Dropdown.DataSource = dt;
            Dropdown.DataTextField = dt.Columns[0].ToString();
            Dropdown.DataValueField = dt.Columns[1].ToString();
            Dropdown.DataBind();
            connection.Close();
            return Dropdown;
        }
        public static int ExecuteScalarQuery(string sb)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            int returnValue = 0;
            try
            {
                OracleCommand cmd = new OracleCommand(sb, ConObj);
                ConObj.Open();
                returnValue = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return returnValue;
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
            return returnValue;
        }

        //----
    }
}
