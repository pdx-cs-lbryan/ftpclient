using System;
using System.IO;

namespace FtpClientApp
{
    public class LogFiles
    {
        //public string TheUserName = Process.Start(ProcessStartInfo.UserName)  .ToString.TheUserName ;
        public static string MyDateTime = DateTime.Now.ToString("yyyyMMddHHmm");
        public static string LogBaseName = "FTPLog_" + MyDateTime + ".log";
        public static string DefaultLogDir = "c:/tmp";  //C:/Users/(user-name)/tmp
        public static string ftpLogName = DefaultLogDir + "/" + LogBaseName;
        public static StreamWriter ftpLog = File.AppendText(ftpLogName);

        public void StartLog()
        {
            TextWriter w = ftpLog;
            w.WriteLine("=============================================================");
            w.WriteLine("Begin Log", DateTime.Now.ToString("yyyyMMdd-HH:mm:ss"));

        }

        public void WriteLog(string logMessage)
        {
            TextWriter w = ftpLog;
            w.WriteLine(DateTime.Now.ToString("yyyyMMdd-HH:mm:ss"), logMessage);

        }

        public void EndLog()
        {
            TextWriter w = ftpLog;
            w.WriteLine("End Log" + DateTime.Now.ToString("yyyyMMdd-HH:mm:ss"));
            w.WriteLine("=============================================================");

            w.Close();
        }

    }
}
