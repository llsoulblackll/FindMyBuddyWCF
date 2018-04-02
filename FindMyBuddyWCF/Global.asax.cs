using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace FindMyBuddyWCF
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, DELETE, POST, PUT, OPTIONS");

                //ENABLE CONTENT-TYPE ACCESS FOR OPTIONS PREFLIGHT REQUEST TO ALLOW CONTENT-TYPE JSON HEADER SINCE IT IS NEEDED FOR WCF JSON DESERIALIZATION
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");//, Accept");
                //BY USING END (NOT CLOSE THAT CLOSES THE CONNECTION) WE SEND ALL THE DATA GATHERED SO FAR, SO SINCE WE ARE ON THE BEGIN REQUEST WE PREVENT THE OPTIONS PREFLIGHT REQUEST FROM GOING TO THE ENDPOINTS
                HttpContext.Current.Response.End();
                //HttpContext.Current.Response.Close();
            }
        }
    }
}