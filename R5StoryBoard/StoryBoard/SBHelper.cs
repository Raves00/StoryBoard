using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace StoryBoard
{
    public class SBHelper
    {
        public static string DataTableToJSON(DataTable dtDataTable)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in dtDataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dtDataTable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        public static string EncodeData(string strData)
        {
            string sdata = "";
            if (!string.IsNullOrEmpty(strData))
                sdata = HttpUtility.HtmlEncode(strData.Replace(Environment.NewLine, "<br>").Replace("\n", "<br>")).Replace("&amp;", "&");
            return sdata;
        }

        public static string DecodeData(string strData)
        {
            if (string.IsNullOrEmpty(strData))
                return string.Empty;

            return HttpUtility.HtmlDecode(strData).Replace("&lt;", "<").Replace("&gt;", ">").Replace("<br>", Environment.NewLine).Replace("&#39;", "'");
        }

        public static Dictionary<string, ImageFormat> ImageFormats
        {
            get
            {
                return new Dictionary<string, ImageFormat>() { { ".jpeg", ImageFormat.Jpeg }, { ".jpg", ImageFormat.Jpeg }, { ".png", ImageFormat.Png }, { ".bmp", ImageFormat.Bmp }, { ".gif", ImageFormat.Gif } };

            }

        }
    }
}