using System;
using System.IO;

namespace FtpClientApp
{

    /*
     * Class for logging the user's activities on client
     */
    public class LogFiles
    {
        public static string MyDateTime = DateTime.Now.ToString("yyyyMMddHHmm");
        public static string LogBaseName = "FTPLog_" + MyDateTime + ".log";
        public static string DefaultLogDir = Directory.GetCurrentDirectory();
        public static string ftpLogName = DefaultLogDir + "/" + LogBaseName;
        public static StreamWriter ftpLog = File.AppendText(ftpLogName);

        /*
         * Starts up the log file
         */
        public void StartLog()
        {
            try
            {
                TextWriter w = ftpLog;
                w.WriteLine("========================================================");
                w.WriteLine("Begin Log " + DateTime.Now.ToString("yyyyMMdd-HH:mm:ss"));
            } catch (Exception e)
            {
                Console.WriteLine("Error writing to log file: " + e.Message);
            }
            

        }

        /*
         * Writes message to log file
         */
        public void WriteLog(string logMessage)
        {
            try
            {
                TextWriter w = ftpLog;
                w.WriteLine(DateTime.Now.ToString("yyyyMMdd-HH:mm:ss") + " " + logMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing to log file: " + e.Message);
            }

        }

        /*
         * Closes the log file
         */
        public void EndLog()
        {
            try
            {
                TextWriter w = ftpLog;
                w.WriteLine("End Log " + DateTime.Now.ToString("yyyyMMdd-HH:mm:ss"));
                w.WriteLine("========================================================");

                w.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing to log file: " + e.Message);
            }

        }

    }
}
