using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace FtpClientApp
{
    /*
     * Class to rename file on the remote server via FTP
     */
   public class RenameFileRemote
   {
       private ServerConnectionInformation connection;

       private FtpWebRequest request;

       private FtpTestWrapper wrapper;

        /*
         * Constructor with a SCI to set for the class
         */
       public RenameFileRemote(ServerConnectionInformation toUse)
       {
           this.connection = toUse;
       }

        /*
         * Sets the class' request to the one passed in
         */
       public void setRequest(FtpWebRequest req)
       {
           this.request = req;
       }

        /*
         * Sets the class' wrapper to the one passed in
         */
       public void setWrapper(FtpTestWrapper wrapper)
       {
           this.wrapper = wrapper;
       }


        /*
         * Takes in a file and a new name to set its name to for the remote server
         */
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

        /*
         * Returns the class' wrapper
         */
        public FtpTestWrapper getWrapper()
        {
            return this.wrapper;
        }

        /*
         * Makes the FTP request to the server via the wrapper
         */
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
