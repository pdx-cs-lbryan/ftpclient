using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace FtpClientApp
{
    public class DeleteFromRemote
    {

        private ServerConnectionInformation connection;
        private FtpWebRequest request;
    
        public DeleteFromRemote(ServerConnectionInformation toUse)
        {
            this.connection = toUse;
        }
        public void setRequest(FtpWebRequest req)
        {
            this.request = req;
        }

        public String DeleteFileOnRemoteServer(String file)
        {
            String remoteFile = this.connection.ServerName + '/' + file;
            try{
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteFile);
                request.Credentials = new NetworkCredential(this.connection.UserName, this.connection.PassWord);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
 
                FtpWebResponse response = (FtpWebResponse) request.GetResponse();
                response.Close();
                return "success";
            }
            catch(WebException e){
                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                {
                    return "The server sent an error code of 550. The file may no exist.";
                }
                return e.Message.ToString();
            }
            
           
        }

    }
}
