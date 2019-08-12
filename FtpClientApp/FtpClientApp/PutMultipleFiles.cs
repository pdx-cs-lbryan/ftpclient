using System;
using System.IO;
using System.Linq;
using System.Net;

namespace FtpClientApp
{
    //Class for uploading a file on the remote FTP server 
    //Works off of the functions provided in Program.cs
    public class PutMultipleFiles
    {
        //A MainMenu variable to keep track of the user and server for the use of an instance of this class.
        private ServerConnectionInformation connection;
        private FtpWebRequest request;
        //private WebClient request;
        private FtpTestWrapper wrapper;

        //A constructor for the class which takes in a ServerconnectionInformation to set up for its use.
        public PutMultipleFiles(ServerConnectionInformation toUse)
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

        public String getFileName()
        {
            String filepath = Console.ReadLine();
            return filepath;
        }

        public string CopyFiles(
            string sourceDirName, string destDirName, string inputfilenames)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);

                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                if (!Directory.Exists(destDirName))
                {
                    // If the destination directory doesn't exist, create it.
                    CreateRemoteDirectory createRemDir = new CreateRemoteDirectory(this.connection);
                    FtpTestWrapper wrapper = new FtpTestWrapper();
                    createRemDir.setWrapper(wrapper);
                    createRemDir.setup(destDirName);
                    String response = createRemDir.create(createRemDir.getWrapper());
                    if (response == "success")
                    {
                        Console.WriteLine("\n ** Directory doesn't exists on remote server \n Creating a new directory: {0} on remote server **\n", destDirName);
                    }
                }

                string[] inputfilenames_list = inputfilenames.Split(',');
                for (int i = 0; i < inputfilenames_list.Length; i++)
                {
                    inputfilenames_list[i] = inputfilenames_list[i].Trim();
                    bool fileexists = false;
                    
                    String extension = Path.GetExtension(inputfilenames_list[i]);
                    
                    if ((extension != ".txt") && (extension != ".jpg") && (extension != ".png"))
                    {
                        Console.WriteLine("Please enter one of the following file formats only :.txt, .jpg, .png");
                        return "disconnect";
                    }

                    // Get the files in the current directory and copy them to the new location.
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        if (inputfilenames_list[i] == file.Name)
                        {
                            fileexists = true;
                            String serverdirpath = this.connection.ServerName + "/" + destDirName;
                            string temppath = Path.Combine(serverdirpath, file.Name);
                            String filetobeuploaded = Path.Combine(sourceDirName, file.Name);

                            WebClient request1 = new WebClient();
                            request1.Credentials = new NetworkCredential(this.connection.UserName, this.connection.PassWord);
                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this.connection.ServerName);
                            request.Credentials = new NetworkCredential(this.connection.UserName, this.connection.PassWord);
                            byte[] responseArray = request1.UploadFile(temppath, filetobeuploaded);                            
                        }
                    }

                    if (!fileexists)
                    {
                        Console.WriteLine("\n This file doesn't exist in the source directory: {0} \n", inputfilenames_list[i]);
                    }
                    else
                    {
                        Console.WriteLine("\n ** Uploaded {0} to remote server **\n", inputfilenames_list[i]);
                    }
                }                               
            }
            catch (Exception e)
            {
                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                {
                    Console.WriteLine("The server sent an error code of 550 \n The directory may not exist on the Server or \n Please check local file path and provide in Drive:xyz/abc.txt format \n");
                    return "The server sent an error code of 550. The directory may not exist on the Server";
                }
                else
                {
                    Console.WriteLine(e.Message.ToString());
                    return "disconnect";
                }
            }
            return "success";
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
                        return "Error - could not copy file";
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