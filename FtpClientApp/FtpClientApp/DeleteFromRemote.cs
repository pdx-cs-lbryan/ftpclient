using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace FtpClientApp
{
    /*
     * The DeleteFromRemote class is used to make FTP requests to a server to delete a file.
     */
    public class DeleteFromRemote
    {

        private ServerConnectionInformation connection;
        private FtpWebRequest request;

        private FtpTestWrapper wrapper;
    
        /*
         * Constructor to set the class' connection to the one passed in
         */
        public DeleteFromRemote(ServerConnectionInformation toUse)
        {
            this.connection = toUse;
        }

        /*
         * Class to set the class' request to the one passed in
         */
        public void setRequest(FtpWebRequest req)
        {
            this.request = req;
        }

        /*
         * Class to set the class' FTP wrapper to the one passed in
         */
        public void setWrapper(FtpTestWrapper wrapper)
        {
            this.wrapper = wrapper;
        }

        /*
         * Takes in a string to determine the file to delete and makes an FTP request to the server to delete that file.
         */
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
                    return "The server sent an error code of 550. The file may not exist.";
                }
                return e.Message.ToString();
            }
           
        }

        /*
         * Returns the class' FTP wrapper
         */
        public FtpTestWrapper getWrapper()
        {
            return this.wrapper;
        }


        /*
         * Class to make a request to the server via the wrapper.
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
