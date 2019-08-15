using System;
using System.IO;

namespace FtpClientApp
{

    /*
     * Class for logging the user's activities on client
     */
    public class LogFiles
    {

        public string MyDateTime;
        public string LogBaseName;
        public string DefaultLogDir;
        public string ftpLogName;
        public StreamWriter ftpLog;
        private bool operate;

        public LogFiles()
        {
            try
            {
                MyDateTime = DateTime.Now.ToString("yyyyMMddHHmm");
                LogBaseName = "FTPLog_" + MyDateTime + ".log";
                DefaultLogDir = Directory.GetCurrentDirectory();
                ftpLogName = DefaultLogDir + "/" + LogBaseName;
                         ftpLog = File.AppendText(ftpLogName);
                operate = true;

            } catch (Exception e)
            {
                operate = false;
                Console.WriteLine("Could not start the log file: " + e.Message);
            }

        }

        /*
         * Starts up the log file
         */
        public void StartLog()
        {
            if(operate == true)
            {
                try
                {
                    TextWriter w = ftpLog;
                    w.WriteLine("========================================================");
                    w.WriteLine("Begin Log " + DateTime.Now.ToString("yyyyMMdd-HH:mm:ss"));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error writing to log file: " + e.Message);
                }
            }

        }

        /*
         * Writes message to log file
         */
        public void WriteLog(string logMessage)
        {
            if(operate == true)
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
           
        }

        /*
         * Closes the log file
         */
        public void EndLog()
        {
            if(operate == true)
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
}
