using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoryBoard
{
    public class ErrorLogger
    {
        private static log4net.ILog logger { get; set; }

        static ErrorLogger()
        {
            logger = log4net.LogManager.GetLogger(typeof(ErrorLogger));
        }

        public static void LogError(string strPagename, Exception ex)
        {
            logger.Error(strPagename, ex);
        }

        public static void LogError(string strPagename, string exception)
        {
            logger.Error(string.Format("Source:{0} Error: {1}", strPagename, exception));
        }
    }
}