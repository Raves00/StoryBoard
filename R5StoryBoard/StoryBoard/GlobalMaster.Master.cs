using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StoryBoard
{
    public partial class GlobalMaster : System.Web.UI.MasterPage
    {
        public int PageID
        {
            get
            {
                if (HttpContext.Current.Session["PageId"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["PageId"]);
                }
                return -1;
            }
            set { HttpContext.Current.Session["PageId"] = value; }
        }

        /// <summary>
        /// 1- SSP
        /// 2- WP
        /// </summary>
        public int ModuleType
        {
            get
            {
                if (HttpContext.Current.Session["ModuleType"] != null)
                {
                    return Convert.ToInt32( HttpContext.Current.Session["ModuleType"]);
                }
                return -1;
            }
            set { HttpContext.Current.Session["ModuleType"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string strUrl = Request.Url.Segments[Request.Url.Segments.Count() - 1];
            switch (strUrl)
            {
                case "AddPageDetails.aspx":
                    m1.Attributes.Add("class", "active");
                    m2.Attributes.Remove("class");
                    m3.Attributes.Remove("class");
                    m4.Attributes.Remove("class");
                    break;
                case "AddDataElements.aspx":
                    m2.Attributes.Add("class", "active");
                    m1.Attributes.Remove("class");
                    m3.Attributes.Remove("class");
                    m4.Attributes.Remove("class");
                    break;
                case "ScreenImages.aspx":
                    m3.Attributes.Add("class", "active");
                    m4.Attributes.Remove("class");
                    m1.Attributes.Remove("class");
                    m2.Attributes.Remove("class");
                    break;
                case "SearchElements.aspx":
                    m4.Attributes.Add("class", "active");
                    m1.Attributes.Remove("class");
                    m2.Attributes.Remove("class");
                    m3.Attributes.Remove("class");
                    break;
            }

            if (!IsPostBack)
            {

            }
        }
    }
}