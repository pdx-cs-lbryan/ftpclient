using System;
using System.Linq;
using System.Net;

namespace FtpClientApp
{
    //Class for creating a directory on the remote FTP server with its necessary functionality and data.
    //Works off of the functions provided in Program.cs
    public class CreateRemoteDirectory
    {
        //A MainMenu variable to keep track of the user and server for the use of an instance of this class.
        private ServerConnectionInformation connection;
        private FtpTestWrapper wrapper;
        private FtpWebRequest request;

        //A constructor for the class which takes in a ServerconnectionInformation to set up for its use.
        public CreateRemoteDirectory(ServerConnectionInformation toUse)
        {
            this.connection = toUse;

        }

        public void setWrapper(FtpTestWrapper wrapper)
        {
            this.wrapper = wrapper;
        }

        public void setRequest(FtpWebRequest req)
        {
            this.request = req;
        }

        //A function to get a name of a directory to create from the user. Returns the string that the user enters as the name.
        public String getDirectoryName()
        {
            Console.WriteLine("\nName of Directory to Create : ");
            String dir = Console.ReadLine();
            return dir;
        }

        public String setup(String dir)
        {
            bool testValid = validDir(dir);
            if(testValid == false)
            {
                return "Directory name contained invalid character";
            }

            //make request
            String remoteDir = this.connection.ServerName + '/' + dir;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteDir);
                request.Credentials = new NetworkCredential(this.connection.UserName, this.connection.PassWord);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                this.wrapper.setRequest(request);
                return "success";
            }
            catch (WebException e)
            {

                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                {
                    return "The server sent an error code of 550. The directory may already exist or the file was unavailable due to a lack of access.";
                }
                return e.Message.ToString();
            }
            catch (System.UriFormatException e)
            {
                return "Poorly formatted URI. Please enter a valid directory name";
            }
        }

        public bool validDir(String dir)
        {
            //check for invalid characters
            char[] invalid = new char[] { ',', '<', '>', '|', '.' };

            for (int i = 0; i < invalid.Length; i++)
            {
                if (dir.Contains(invalid[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public FtpTestWrapper getWrapper()
        {
            return this.wrapper;
        }

        //create() is the main function of the class. It takes in a string to add as a directory to the remote server
        //and returns a String which will be 'success' in the case that the directory was successfully created.
        //Otherwise the string will contain a relevant error message.
        public String create(FTPTestWrapperAbstract wrapper)
        {
            //Handle response
            try
            {
                FtpWebResponse response = wrapper.getResp();
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
                return "Poorly formatted URI. Please enter a valid directory name";
            }

            return "success";

        }
    }
}
