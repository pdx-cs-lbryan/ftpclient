using System;
using System.IO;
using System.Net;

namespace FtpClientApp
{
    public class FileDownload
    {

        private ServerConnectionInformation connection;
        private FtpWebRequest request;

        private FtpTestWrapper wrapper;
    
        public FileDownload(ServerConnectionInformation toUse)
        {
            this.connection = toUse;
        }
        public void setRequest(FtpWebRequest req)
        {
            this.request = req;
        }

        public void setWrapper(FtpTestWrapper wrapper)
        {
            this.wrapper = wrapper;
        }
        public String FileDownloadFromRemote(ServerConnectionInformation myConnection, string FileName, string DownloadDirectory)
        {   //for test
            //myConnection.ServerName = "ftp://speedtest.tele2.net";
            //myConnection.UserName = "anonymous";
            //myConnection.PassWord = "anonymous";

            myConnection.ServerName = myConnection.ServerName + '/' + FileName;

            int bytesRead = 0;
            byte[] buffer = new byte[2048];
            try{
                FtpWebRequest myServerConnectionRequest = (FtpWebRequest)WebRequest.Create(myConnection.ServerName);
                myServerConnectionRequest.Credentials = new NetworkCredential(myConnection.UserName, myConnection.PassWord);
                myServerConnectionRequest.Proxy = null;
                myServerConnectionRequest.UsePassive = true;
                myServerConnectionRequest.UseBinary = true;
                myServerConnectionRequest.KeepAlive = true;

                myServerConnectionRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                Stream StreamReader = myServerConnectionRequest.GetResponse().GetResponseStream();
                FileStream myFileStream = new FileStream(DownloadDirectory+FileName, FileMode.Create);

                while (true)
                {
                    bytesRead = StreamReader.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0) { break;  }

                    myFileStream.Write(buffer, 0, bytesRead);
                }
            
                myFileStream.Close();
                StreamReader.Close();
                return "success";
                }
                catch(WebException e){
                    if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                    {
                        return "The server sent an error code of 550. The file may not exist.";
                    }
                    return e.Message.ToString();
                }

        } // end FileDownload()

        public String create(FTPTestWrapperAbstract wrapper)
        {
            //Handle response
            try
            {
                FtpWebResponse response = wrapper.getResp();
                if(response != null)
                {
                    if ((int)response.StatusCode >= 300)
                    {
                        return "Error - could not delete file";
                    }
                }
                
            }
            catch (WebException e)
            {
                
                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access).")) { 
                          return "The server sent an error code of 550. The directory may already exist or the file was unavailable due to a lack of access.";
                }
                return e.Message.ToString();
            }
            catch (System.UriFormatException e)
            {
                return "Poorly formatted URI. Please enter a valid file name";
            }

            return "success";

        }

    }
}
