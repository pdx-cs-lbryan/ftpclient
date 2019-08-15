using System;
using System.IO;
using System.Net;

namespace FtpClientApp
{
    /*
     * Class for downloading a file from the remote server
     */
    public class FileDownload
    {

        private ServerConnectionInformation connection;
        private FtpWebRequest request;

        private FtpTestWrapper wrapper;
    
        /*
         * Constructor with a SCI to set
         */
        public FileDownload(ServerConnectionInformation toUse)
        {
            this.connection = toUse;
        }

        //sets the class' request to the given FtpWebRequest
        public void setRequest(FtpWebRequest req)
        {
            this.request = req;
        }

        //sets class' ftp wrapper to the one given
        public void setWrapper(FtpTestWrapper wrapper)
        {
            this.wrapper = wrapper;
        }

        //Method for downloading a file. Takes in a connection, a file name to get, and a directory to store the file in
        public String FileDownloadFromRemote(ServerConnectionInformation myConnection, string FileName, string DownloadDirectory)
        {   

            String rememberServer = myConnection.ServerName;
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
                if(Directory.Exists(Path.GetDirectoryName(DownloadDirectory + "/" + FileName)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(DownloadDirectory + "/" + FileName));
                }

                Stream StreamReader = myServerConnectionRequest.GetResponse().GetResponseStream();
                FileStream myFileStream = new FileStream(DownloadDirectory+"/"+FileName, FileMode.Create);

                while (true)
                {
                    bytesRead = StreamReader.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0) { break;  }

                    myFileStream.Write(buffer, 0, bytesRead);
                }
                myConnection.ServerName = rememberServer;
                myFileStream.Close();
                StreamReader.Close();
                return "success";
                }
                catch(WebException e){
                    myConnection.ServerName = rememberServer;
                    if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                    {
                        return "The server sent an error code of 550. The file may not exist.";
                    }
                    return e.Message.ToString();
                } catch(Exception e)
            {
                myConnection.ServerName = rememberServer;
                return e.Message.ToString();
            }

            

        } // end FileDownload()


        /*
         * Function to make FTP wrapper to make request to the server
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
