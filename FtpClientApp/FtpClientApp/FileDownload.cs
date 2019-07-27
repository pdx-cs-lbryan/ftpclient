using System;
using System.IO;
using System.Net;

namespace FtpClientApp
{
    class FileDownload
    {
        public FileDownload(ServerConnectionInformation myConnection)
        {   //for test
            //myConnection.ServerName = "ftp://speedtest.tele2.net";
            //myConnection.UserName = "anonymous";
            //myConnection.PassWord = "anonymous";

            string FileName = "1KB.zip";
            FileName = getFileName(myConnection);

            string DownloadDirectory = "c:/download";
            DownloadDirectory = getDownLoadPath(FileName);

            int bytesRead = 0;
            byte[] buffer = new byte[2048];

            FtpWebRequest myServerConnectionRequest = (FtpWebRequest)WebRequest.Create(myConnection.ServerName);
            myServerConnectionRequest.Credentials = new NetworkCredential(myConnection.UserName, myConnection.PassWord);
            myServerConnectionRequest.Proxy = null;
            myServerConnectionRequest.UsePassive = true;
            myServerConnectionRequest.UseBinary = true;
            myServerConnectionRequest.KeepAlive = true;

            myServerConnectionRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            Stream StreamReader = myServerConnectionRequest.GetResponse().GetResponseStream();
            FileStream myFileStream = new FileStream(DownloadDirectory, FileMode.Create);

            while (true)
            {
                bytesRead = StreamReader.Read(buffer, 0, buffer.Length);

                if (bytesRead == 0) { break;  }

                myFileStream.Write(buffer, 0, bytesRead);
            }
            myFileStream.Close();
            StreamReader.Close();
            Console.WriteLine($"\nDownload complete \n\n ");

        } // end FileDownload()


        public string getFileName(ServerConnectionInformation myConnection)
        {
            bool LetsContinueLoop = true;
            string myAnswer = "";
            string FileName = "";

            Console.WriteLine("\nPlease enter the File to Download: ");
 
            do
            {
                FileName = Console.ReadLine();
                while (FileName == "")
                {
                    Console.WriteLine("\nFile Name Entered is blank, Please try again ");
                    FileName = Console.ReadLine();
                }
                myConnection.ServerName = myConnection.ServerName + '/' + FileName;

                Console.WriteLine($"\nPlease verify the download Source: {myConnection.ServerName}");
                Console.WriteLine($"\n Y or y to accept and continue. ");
                myAnswer = Console.ReadLine();
                if (myAnswer == "Y" || myAnswer == "y") { LetsContinueLoop = false; }

            } while (LetsContinueLoop);

            return FileName;
        } // end getFileName()

        public string getDownLoadPath(string fileName)
        {
            bool LetsContinueLoop = true;
            string myAnswer = "";
            string DownLoadPath = "";

            Console.WriteLine("\nPlease enter the Download Location: ");
            Console.WriteLine("\nexample: c:\\download  location must exist");

            do
            {
                DownLoadPath = Console.ReadLine();
                while (DownLoadPath == "")
                {
                    Console.WriteLine("\nLocal File Path Entered is blank, Please try again ");
                    DownLoadPath = Console.ReadLine();
                }
                DownLoadPath = DownLoadPath + '/' + fileName;

                Console.WriteLine($"\nPlease Verify the Destination: {DownLoadPath}");
                Console.WriteLine($"\n Y or y to accept and continue. ");
                myAnswer = Console.ReadLine();
                if (myAnswer == "Y" || myAnswer == "y") { LetsContinueLoop = false; }

            } while (LetsContinueLoop);

            return DownLoadPath;
        } // end getLocalFilePath()

    } // end class FileDownload
} // end namespace FtpClientApp
