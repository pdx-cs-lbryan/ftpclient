using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace FtpClientApp
{
   public class RenameFileRemote
   {
       private ServerConnectionInformation connection;

       private FtpWebRequest request;

       private FtpTestWrapper wrapper;

       public RenameFileRemote(ServerConnectionInformation toUse)
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

       public String RenameFileOnRemoteServer(String file, String newName)
        {
            String remoteFile = this.connection.ServerName + '/' + file;
            try{
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteFile);
                request.Credentials = new NetworkCredential(this.connection.UserName, this.connection.PassWord);
                request.Method = WebRequestMethods.Ftp.Rename;
                request.RenameTo = newName;
 
                FtpWebResponse response = (FtpWebResponse) request.GetResponse();
                response.Close();
                return "success";
            }
            catch(WebException e){
                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                {
                    return "The server sent an error code of 550. The file may not exist.";
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
                        return "Error - could not rename file";
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
