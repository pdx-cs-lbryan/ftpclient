using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace FtpClientApp
{
    public class ListFiles
    {

        private ServerConnectionInformation connection;
        private FtpWebRequest request;

        private FtpTestWrapper wrapper;
    
        public ListFiles(ServerConnectionInformation toUse)
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

        public String ListFilesOnRemoteServer()
        {
            try{
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this.connection.ServerName);
                request.Credentials = new NetworkCredential(this.connection.UserName, this.connection.PassWord);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
 
                FtpWebResponse response = (FtpWebResponse) request.GetResponse();
                Stream ServerResponseStream = response.GetResponseStream();
                StreamReader readerObj = new StreamReader(ServerResponseStream);
                Console.WriteLine(readerObj.ReadToEnd());
                response.Close();
                return "success";
            }
            catch(WebException e){
                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                {
                    return "The server sent an error code of 550.";
                }
                return e.Message.ToString();
            }
           
        }

        public FtpTestWrapper getWrapper()
        {
            return this.wrapper;
        }

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
                        return "Error - could not list files";
                    }
                }
                
            }
            catch (WebException e)
            {
                
                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access).")) { 
                          return "The server sent an error code of 550.";
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
