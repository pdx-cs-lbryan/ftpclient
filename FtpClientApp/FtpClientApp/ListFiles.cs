using System;
using System.IO;
using System.Net;

namespace FtpClientApp
{
    class ListFiles
    {
        public ListFiles(ServerConnectionInformation myConnection)
        {   // defaults used for testing
            //myConnection.ServerName = "ftp://speedtest.tele2.net";
            //myConnection.UserName = "anonymous";
            //myConnection.PassWord = "anonymous";

            FtpWebRequest myServerConnectionRequest = (FtpWebRequest)WebRequest.Create(myConnection.ServerName);
            myServerConnectionRequest.Credentials = new NetworkCredential(myConnection.UserName, myConnection.PassWord);

            myServerConnectionRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            FtpWebResponse ServerResponse = (FtpWebResponse)myServerConnectionRequest.GetResponse();

            Stream ServerResponseStream = ServerResponse.GetResponseStream();
            StreamReader readerObj = new StreamReader(ServerResponseStream);
            Console.WriteLine(readerObj.ReadToEnd());

            readerObj.Close();
            ServerResponse.Close();

        } // end ListFiles
           
    } // end class ListFiles
} // end namespace FtpClientApp
