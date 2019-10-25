using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    public class LogHelp
    {
        public static readonly ILog logInfo = LogManager.GetLogger("loginfo");
        public static readonly ILog logError = LogManager.GetLogger("logerror");

        public static void LogInfo(string info)
        {
            if (logInfo.IsInfoEnabled)
            {
                logInfo.Info(info);
            }
        }

        public static void LogError(string info, Exception ex)
        {
            if (logError.IsErrorEnabled)
            {
                logError.Error(info, ex);
            }
        }
    }
}
