using System;
using System.Collections.Generic;
using System.Data;
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
            return HttpUtility.HtmlEncode(strData).Replace(Environment.NewLine, "<br>");;
        }

        public static string DecodeData(string strData)
        {
            return HttpUtility.HtmlDecode(strData).Replace("<br>", Environment.NewLine);
        }
    }
}