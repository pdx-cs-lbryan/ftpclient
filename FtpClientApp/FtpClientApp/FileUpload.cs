using System;
using System.IO;
using System.Net;
using System.Text;

namespace FtpClientApp
{
    class FileUpload
    {
        public FileUpload(ServerConnectionInformation myConnection)
        {   //for test
            myConnection.ServerName = "tekprd05:/u/prodctl/kintana/upload";
            myConnection.UserName = "kintana";
            myConnection.PassWord = "";
            Console.WriteLine("\n ServerName:  " + myConnection.ServerName);
            Console.WriteLine("\n UserName:  " + myConnection.UserName);
            Console.WriteLine("\n Password:  " + myConnection.PassWord);

            string FileName = "c:/download/daffodils.jpg";

            //FtpWebRequest myServerConnectionRequest = (FtpWebRequest)WebRequest.Create(myConnection.ServerName);
            FtpWebRequest myServerConnectionRequest = (FtpWebRequest)WebRequest.Create(new Uri(myConnection.ServerName));
            myServerConnectionRequest.Credentials = new NetworkCredential(myConnection.UserName, myConnection.PassWord);
            myServerConnectionRequest.Proxy = null;
            myServerConnectionRequest.UsePassive = true;
            myServerConnectionRequest.UseBinary = true;
            myServerConnectionRequest.KeepAlive = true;

            myServerConnectionRequest.Method = WebRequestMethods.Ftp.UploadFile;

            byte[] upLoadFileContents;
            using (StreamReader sourceStream = new StreamReader(FileName))
            {
                upLoadFileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            }

            myServerConnectionRequest.ContentLength = upLoadFileContents.Length;

            using (Stream UploadStream = myServerConnectionRequest.GetRequestStream())
            {
                UploadStream.Write(upLoadFileContents, 0, upLoadFileContents.Length);
            }
            using (FtpWebResponse resultsResponse = (FtpWebResponse)myServerConnectionRequest.GetResponse())
            {
                Console.WriteLine($"File upload to destination status: {resultsResponse.StatusDescription}");
            }

        } // end FileUPload()

    } // end class FileUpload
} // end namespace FtpClientApp
