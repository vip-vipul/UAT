using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using PrjUpassDAL.Helper;
using System.Web.UI;

namespace PrjUpassPl
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Cache.Insert("Pages", DateTime.Now, null,
           System.DateTime.MaxValue, System.TimeSpan.Zero,
           System.Web.Caching.CacheItemPriority.NotRemovable,
           null);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            //string fullOrigionalpath = Request.Url.ToString();

            //try
            //{

            //    Context.RewritePath(System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath) + "?" + DateTime.Now.Ticks.ToString());
                
            //}
            //catch { }
           

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            /*
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                var page = HttpContext.Current.Request.Url.AbsolutePath;
                Cls_Security objSecurity = new Cls_Security();
                string username;
                if (Session["username"] != null)
                {
                    username = Session["username"].ToString();
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }
                Exception err = Server.GetLastError();
                string errMessage = err.Message.ToString();
                objSecurity.InsertIntoDb(username, errMessage, page);
                Response.Redirect("~/ErrorPage.aspx");
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            */
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
        }
    }
}