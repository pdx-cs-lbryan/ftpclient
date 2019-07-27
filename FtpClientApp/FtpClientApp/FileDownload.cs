using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FtpClientApp
{
    class FileDownload
    {
        public void Download(ServerConnectionInformation connectionInfo)
        {
            string fileLocal = @"c:\";
            string fileServer = @"c:\";
            fileLocal = GetUserInputFile("Where would you like to put the file? Enter an absolute path:");
            fileServer = GetUserInputFile("Where is the file on the server? Enter an absolute path:");
            string filePath = "ftp://" + connectionInfo.ServerName + fileServer;
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential(connectionInfo.UserName, connectionInfo.PassWord);
            byte[] fileData = request.DownloadData(filePath);
            FileStream file = File.Create(fileLocal);
            file.Write(fileData, 0, fileData.Length);
            file.Close();
            Console.WriteLine("File Download Complete");
        }
        public string GetUserInputFile(string message)
        {
            string val = @"c:\";
            bool found = false;

            while (!found)
            {
                Console.WriteLine(message);
                val = Console.ReadLine();
                if (File.Exists(val))
                {
                    found = true;
                }
                else
                {
                    Console.WriteLine("Sorry that is not a file\n");
                }
            }
            return val;
        }
    }


}
