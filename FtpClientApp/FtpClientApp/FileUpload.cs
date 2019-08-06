using System;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace FtpClientApp
{
    //Class for uploading a file on the remote FTP server 
    //Works off of the functions provided in Program.cs
    public class FileUpload
    {
        //A MainMenu variable to keep track of the user and server for the use of an instance of this class.
        private ServerConnectionInformation connection;

        private WebClient request;
        private FtpTestWrapper wrapper;

        //A constructor for the class which takes in a ServerconnectionInformation to set up for its use.
        public FileUpload(ServerConnectionInformation toUse)
        {
            this.connection = toUse;
        }

        public void setRequest(WebClient req)
        {
            this.request = req;
        }

        public void setWrapper(FtpTestWrapper wrapper)
        {
            this.wrapper = wrapper;
        }

        //A function to get File name to be uploaded & location where file is to eb stored. 
        public String getFileName()
        {
            String filepath = Console.ReadLine();
            return filepath;
        }

        //setup() function takes 2 strings as input: file to be uploaded and location on server 
        //returns a String which will be 'success' in the case File is successfully uploaded 
        //Otherwise the string will contain a relevant error message.
        public String setup(String filetobeuploaded, String locationonserver)
        {
            try
            {
                if (!File.Exists(filetobeuploaded))
                {
                    Console.WriteLine("File does not exist. Please enter valid file path");
                    return "disconnect";
                }
                String input = filetobeuploaded;
                String pattern = @"/";
                String[] elements = System.Text.RegularExpressions.Regex.Split(input, pattern);
                string lastItem = elements[elements.Length - 1];
                Console.WriteLine("Uploading " + lastItem + " to server\n");

                String serverpath = locationonserver + "/" + lastItem;

                String extension = Path.GetExtension(lastItem);

                if ((extension != ".txt") && (extension != ".jpg") && (extension != ".png"))
                {
                    Console.WriteLine("Please enter one of the following file formats only :.txt, .jpg, .png");
                    return "disconnect";
                }

                WebClient request1 = new WebClient();
                request1.Credentials = new NetworkCredential(this.connection.UserName, this.connection.PassWord);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this.connection.ServerName);
                request.Credentials = new NetworkCredential(this.connection.UserName, this.connection.PassWord);
                byte[] responseArray = request1.UploadFile(serverpath, filetobeuploaded);
            }
            catch (WebException e)
            {
                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                {
                    Console.WriteLine("The server sent an error code of 550 \n The directory may not exist on the Server or \n Please check local file path and provide in Drive:xyz/abc.txt format \n");
                    return "The server sent an error code of 550. The directory may not exist on the Server";
                }
                else
                {
                    Console.WriteLine("The server sent an error code of 550 \n The directory may not exist on the Server or \n Please check local file path and provide in Drive:xyz/abc.txt format \n ");
                    return "Please check local file path and provide in Drive:xyz/abc.txt path ";
                }
            }
            return "success";
        }

        public FtpTestWrapper getWrapper()
        {
            return this.wrapper;
        }

        public String create(FTPTestWrapperAbstract wrapper)
        {
            try
            {
                FtpWebResponse response = wrapper.getResp();
                if (response != null)
                {
                    if ((int)response.StatusCode >= 300)
                    {
                        return "Error - could not upload file";
                    }
                }

            }
            catch (WebException e)
            {

                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                {
                    return "The server sent an error code of 550. The file may already exist or the file was unavailable due to a lack of access.";
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

