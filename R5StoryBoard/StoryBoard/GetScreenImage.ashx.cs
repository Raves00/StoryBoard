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
            context.Response.ContentType = "image/jpeg";
            if (context.Request.QueryString["Id"] != null)
            {
                int imgId = 0;
                imgId = Convert.ToInt16(context.Request.QueryString["Id"]);
                KeyValuePair<string,byte[]> imagedata = DataMaster.GetImageFromDB(imgId);
                if (imagedata.Value != null)
                {
                    MemoryStream memoryStream = new MemoryStream(imagedata.Value, false);
                    System.Drawing.Image imgFromDataBase = System.Drawing.Image.FromStream(memoryStream);
                    if(imagedata.Key.ToLower().EndsWith(".jpg") || imagedata.Key.ToLower().EndsWith(".jpeg"))
                    imgFromDataBase.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    else if(imagedata.Key.ToLower().EndsWith(".png"))
                        imgFromDataBase.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
                    else if (imagedata.Key.ToLower().EndsWith(".bmp"))
                        imgFromDataBase.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Bmp);
                    else if (imagedata.Key.ToLower().EndsWith(".gif"))
                        imgFromDataBase.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);


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