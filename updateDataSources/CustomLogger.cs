using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace updateDataSources
{
    public static class CustomLogger
    {
        private static string logPath = Environment.ExpandEnvironmentVariables("%USERPROFILE%") + @"\Desktop\iTouch\dataSourceCheck\logs.txt";
        public static void Trace(string text)
        {
            using (StreamWriter log = new StreamWriter(logPath, true))
            {
                text = string.Format("{0}: {1}", DateTime.Now, text);
                log.WriteLine(text);
            }
        }
    }
}
