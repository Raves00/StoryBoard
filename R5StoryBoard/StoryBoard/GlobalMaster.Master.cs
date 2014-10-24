using System;
using System.Collections.Generic;
using System.IO;
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
                    return Convert.ToInt32(HttpContext.Current.Session["ModuleType"]);
                }
                return -1;
            }
            set { HttpContext.Current.Session["ModuleType"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var items = lstMenu.Controls;
            string pageName = Path.GetFileNameWithoutExtension(Request.Path);
            foreach (var lstitem in items)
            {
                if (lstitem.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                {
                    var pageitem = ((System.Web.UI.HtmlControls.HtmlGenericControl)(lstitem));
                    var pagedetails = pageitem.InnerText;
                    if (pagedetails.IndexOf(pageName) != -1)
                    {
                        foreach (var itemsagain in items)
                        {
                            if (itemsagain.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                            {
                                ((System.Web.UI.HtmlControls.HtmlGenericControl)(itemsagain)).Attributes.Remove("class");
                            }
                        }
                        pageitem.Attributes.Add("class", "active");

                    }
                }
            }
            
            if (Session["User"] == null)
                Response.Redirect("Login.aspx");
            else
            {
                if (!IsPostBack)
                {
                    lblUserName.Text = (Session["User"] as User).FullName;
                }
            }
        }

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}