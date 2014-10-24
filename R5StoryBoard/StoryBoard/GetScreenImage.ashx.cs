using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StoryBoard
{
    /// <summary>
    /// Summary description for GetScreenImage
    /// </summary>
    public class GetScreenImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            //context.Response.ContentType = "image/jpeg";
            if (context.Request.QueryString["Id"] != null)
            {
                int imgId = 0;
                imgId = Convert.ToInt16(context.Request.QueryString["Id"]);
                KeyValuePair<string, byte[]> imagedata = DataMaster.GetImageFromDB(imgId);
                if (imagedata.Value != null)
                {
                    MemoryStream memoryStream = new MemoryStream(imagedata.Value, false);
                    System.Drawing.Image imgFromDataBase = System.Drawing.Image.FromStream(memoryStream);
                    string strExtn = Path.GetExtension(imagedata.Key.ToLower());
                    if (SBHelper.ImageFormats.ContainsKey(strExtn))
                        imgFromDataBase.Save(context.Response.OutputStream, SBHelper.ImageFormats[strExtn]);
                }
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}