using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoryBoard
{
    public partial class AjaxDataProcessor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string SearchDataElements(string ElementName)
        {
            try
            {
                return SBHelper.DataTableToJSON(DataMaster.SearchElements(ElementName, "-1"));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}