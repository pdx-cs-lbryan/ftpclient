/**
 * CS 410/510 Agile Developement Summer 2019
 * Team #7
 * Ftp Client Project
 *
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//

using System.IO;
using System.Net;
using System.Net.WebSockets; // ftp library
using System.Configuration;
using System.Reflection;
using System.Security.Principal;
using System.Net.Sockets;
using System.Threading;  // passwordusing System.Timers;
using System.Timers; //Timer
using FtpClientApp;

namespace FtpClient
{
    public class FtpClientMain
    {
        private static System.Timers.Timer aTimer;
        private static bool timeout;
        private static String username;
        private static String password;
        private static String server;
        static void Main()
        {
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("  Welcome to FTPClient \n");

            int running = 1;
            username = "";
            password = "";
            server = "";

            while (running == 1)
            {
                Console.WriteLine("Press 1 to log in, Press 2 to log in with saved information, 3 to exit");
                String response = Console.ReadLine();
                if (response == "1")
                {
                    //get credentials for user
                    Console.WriteLine("Enter your FTP server (localhost, an IP address, etc.) excluding 'ftp://'\n");
                    server = Console.ReadLine();
                    server = "ftp://" + server;
                    Console.WriteLine("Enter your FTP server username\n");
                    username = Console.ReadLine();
                    Console.WriteLine("Enter your FTP server password\n");
                    password = Console.ReadLine();

                    timeout = false;

                    System.Timers.Timer aTimer = new System.Timers.Timer();
                    aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    aTimer.Interval = 300000; // 5 min
                    aTimer.Enabled = true;

                    while (timeout == false) 
                    {   
                        DisplayMenu();
                        timeout = GetResponce(username, password, server, "");

                    }
                    //On Logout, clear all user credentials stored
                   username = "";
                   password = "";
                   server = "";
                }
                else if (response == "3")
                {
                    running = 0;
                } else if (response == "2")
                {
                    //start up a SCI to access its file and decryption
                    ServerConnectionInformation conn = new ServerConnectionInformation();

                    //load the old info if it's there
                    bool test_use = conn.load_saved_info();
                    if(test_use == true)
                    {
                        Console.WriteLine("Logging in as User: " + conn.getUser() + " on Server: " + conn.getServer());
                        //if it was, set it to the program.cs info
                        username = conn.getUser();
                        password = conn.getPass();
                        server = conn.getServer();

                        timeout = false;

                        System.Timers.Timer aTimer = new System.Timers.Timer();
                        aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                        aTimer.Interval = 300000; // 5 min
                        aTimer.Enabled = true;

                        while (timeout == false)
                        {
                            DisplayMenu();
                            timeout = GetResponce(username, password, server, "");

                        }
                        //On Logout, clear all user credentials stored
                        username = "";
                        password = "";
                        server = "";
                    } else
                    {
                        Console.WriteLine("There was no valid saved connection information");
                    }
                }
                else
                {
                    continue;
                }
            }

        } // end Main()

        private static void OnTimedEvent(object source, ElapsedEventArgs e)//Reference: https://docs.microsoft.com/en-us/dotnet/api/system.timers.timer?view=netframework-4.8
        {
            
            ServerConnectionInformation conn = new ServerConnectionInformation(username, password, server);
            Console.WriteLine(" You chose 3, List Files In Directory:  \n");
            ListFiles listFiles = new ListFiles(conn);
            String response3 = listFiles.ListFilesOnRemoteServer();
            if (response3 == "success")
            {
                Console.Write("Success: Still connected to server \n");
                timeout = false;
                DisplayMenu();
                return;
            }
            else
            {
                Console.Write("Error: not connected to server");
                System.Environment.Exit(1);
            }
            
        }


        public static void DisplayMenu()
        {
            Console.WriteLine("Please Select Action");
            Console.WriteLine("1) Get File From Remote Server");
            Console.WriteLine("2) Put File on Remote Server");
            Console.WriteLine("3) List Files on Remote Server");
            Console.WriteLine("4) Create Directory on Remote Server");
            Console.WriteLine("5) Delete file on Remote Server");
            Console.WriteLine("6) Change file permission on Remote Server");
            Console.WriteLine("7) Rename file on Remote Server");
            Console.WriteLine("8) List Files Local");
            Console.WriteLine("9) Rename local file");
            Console.WriteLine("10) Copy Directory (files & subdirectory) to Remote Server");
            Console.WriteLine("11) Put multiple files on Remote Server");
            Console.WriteLine("12) Get all files on Remote Server");
            Console.WriteLine("13) Save Connection Information");
            Console.WriteLine("14) Logout from Server \n");

            
        } // end DisplayMenu()

        public static bool GetResponce(String username, String password, String server, String getAnswer)
        {
            //string getAnswer = "";
            bool MyAnswer = false;
            getAnswer = Console.ReadLine();
            ServerConnectionInformation conn = new ServerConnectionInformation(username, password, server);

            switch (getAnswer)
            {
                case "14":
                    Console.Clear();
                    Console.Write(username);
                    Console.WriteLine(" Logged out from server! \n");
                    Console.WriteLine(" ########################################### \n");
                    //Set response to 'true' to logout
                    MyAnswer = true;
                    break;
                case "13":
                    Console.Clear();
                    conn.Save();
                    Console.WriteLine("Connection Info Saved.");
                    System.Threading.Thread.Sleep(2000);
                    Console.Clear();
                    MyAnswer = false;
                    break;
                case "12":
                    Console.Clear();
                    FileDownloadMultiple fdm = new FileDownloadMultiple(conn);
                    string r = fdm.Download();
                    if (r == "disconnect")
                    {
                        Console.WriteLine("Download Files Failed\n");
                    }
                    else if(r == "success")
                    {
                        Console.WriteLine("Download Files Successful\n");
                    }
                    MyAnswer = false;
                    break;
                case "11":
                    //Put Multiple files on remote server
                    PutMultipleFiles PutFilesToRemote = new PutMultipleFiles(conn);
                    Console.WriteLine(" \n ** Specify local directory path of files to be uploaded \n (Mention absolute path in this format, for ex: C:/xyz/test/ ** \n");
                    String sourcefile_m = PutFilesToRemote.getFileName();
                    Console.WriteLine(" \n ** Specify files to be uploaded from aforementioned path. (Mention each file name separated by a comma. for ex: abc.txt, xyz.png, 123.txt). Filetypes accepted: .txt, .jpg, .png ** \n");
                    String inputfilenames_m = PutFilesToRemote.getFileName();
                    Console.WriteLine(" \n ** Specify directory name on server where source directory is to be copied (for ex: test). \n");
                    String destinationfileonserver_m = PutFilesToRemote.getFileName();
                    String res_m = PutFilesToRemote.CopyFiles(sourcefile_m, destinationfileonserver_m, inputfilenames_m);
                    if (res_m == "success")
                    {
                        Console.Write("\n **** Uploade files complete. Please check messages for further info **** \n \n");
                    }
                    else
                    {
                        Console.Write("\n **** Could not upload files due to an error: \n" + res_m + "\n");
                    }
                    MyAnswer = false;
                    break;
                case "10":
                    //Copy Directory (including all files and subdirectory)
                    CopyDirectory CopyDirectoryToRemote = new CopyDirectory(conn);
                    Console.WriteLine(" ** Specify Directory to be copied to remote server \n (Mention absolute path in this format, for ex: C:/xyz/test/. Filetypes accepted: .txt, .jpg, .png ** \n");
                    String sourcefile = CopyDirectoryToRemote.getFileName();
                    Console.WriteLine(" \n ** Specify directory name on server where source directory is to be copied (for ex: test). \n");
                    String destinationfileonserver = CopyDirectoryToRemote.getFileName();
                    String res = CopyDirectoryToRemote.CopyDirectoryAndSubDirectories(sourcefile, destinationfileonserver);
                    if (res == "success")
                    {
                        Console.Write("\n **** Successfully copied directory **** \n \n");
                    }
                    else
                    {
                        Console.Write("**** Could not copy directory due to an error.\n" + res + "\n");
                    }
                    MyAnswer = false;
                    break;
                case "9":
                    //Rename local file
                    RenameLocal localRename = new RenameLocal();
                    Console.WriteLine("Enter the directory path followed by the file you wish to rename: ");
                    Console.WriteLine("(i.e. C:\\xyz\\Desktop\\file.txt");
                    String fileLocal = Console.ReadLine();
                    Console.WriteLine("Enter the directory path followed by the new name for the file: ");
                    Console.WriteLine("(i.e. C:\\xyz\\Desktop\\Newfile.txt");
                    String newFileLocal = Console.ReadLine();
                    String response10 = localRename.RenameFileLocal(fileLocal, newFileLocal);
                    if (response10 == "success")
                    {
                        Console.Write("File renamed\n\n");
                    }
                    else
                    {
                        Console.Write("Could not rename file due to an error.\n" + response10 + "\n\n");
                    }
                    MyAnswer = false;
                    break;
                case "8":
                    Console.Clear();
                    ListDirectoryLocal list = new ListDirectoryLocal();
                    //get user input
                    Console.WriteLine("Enter an absolute path to directory:");
                    string Dir = Console.ReadLine();
                    bool result = list.ListDirectory(Dir);
                    MyAnswer = false;
                    break;
                case "7":
                    //Rename remote file
                    RenameFileRemote renameRemote = new RenameFileRemote(conn);
                    Console.WriteLine("Enter the file you wish to rename: \n");
                    String fileRename;
                    fileRename = Console.ReadLine();
                    Console.WriteLine("Enter the name which you wish to rename the file with: \n");
                    String newName;
                    newName = Console.ReadLine();
                    String response2 = renameRemote.RenameFileOnRemoteServer(fileRename,newName);
                    if (response2 == "success")
                    {
                        Console.Write("File renamed\n");
                    }
                    else
                    {
                        Console.Write("Could not rename file due to an error.\n" + response2 + "\n");
                    }

                    MyAnswer = false;
                    break;
                case "6":
                    Console.WriteLine("You chose - 6: Change Permissions. Please Note that Changing Permissions Requires an *NIX Server supporting 'SITE CHMOD'. Windows Servers are not supported.");
                    //Change file permissions
                    ChangePermissions perms = new ChangePermissions(conn);
                    FluentWrapper permwrapper = new FluentWrapper();
                    permwrapper.setConn(conn);
                    perms.setDir(permwrapper);
                    perms.setPerms(permwrapper);
                    String permresponse = perms.change(permwrapper);

                    if (permresponse.Equals("Server Validation Failed\n") == true) {
                        Console.WriteLine("\nServer Validation Failed - Logging out");
                        MyAnswer = true;
                        
                    } else
                    {
                        if(permresponse.Equals("success"))
                        {
                            Console.WriteLine("Permissions Changed");
                        } else
                        {
                            Console.WriteLine("Could Not Change Permissions due to Error:");
                            Console.WriteLine(permresponse);
                        }
                        
                        MyAnswer = false;
                    }
                    break;
                case "5":
                    Console.WriteLine(" You chose 5, Delete File From Remote:  \n");
                    //Delete file on remote server
                    DeleteFromRemote deleteRemote = new DeleteFromRemote(conn);
                    Console.WriteLine("Enter the file you wish to delete: \n");
                    String file;
                    file = Console.ReadLine();
                    String response1 = deleteRemote.DeleteFileOnRemoteServer(file);
                    if (response1 == "success")
                    {
                        Console.Write("File deleted\n");
                    }
                    else
                    {
                        Console.Write("Could not delete file due to an error.\n" + response1 + "\n");
                    }

                    MyAnswer = false;
                    break;
                case "4":
                    Console.WriteLine(" You chose 4, Create Directory:  \n");
                    //create remote directory

                    CreateRemoteDirectory createRemDir = new CreateRemoteDirectory(conn);
                    FtpTestWrapper newdirwrapper = new FtpTestWrapper();
                    String directory = createRemDir.getDirectoryName();
                    createRemDir.setWrapper(newdirwrapper);
                    createRemDir.setup(directory);
                    String changeresponse = createRemDir.create(createRemDir.getWrapper());
                    if (changeresponse == "success")
                    {
                        Console.Write("Directory Created\n");
                    }
                    else if (changeresponse == "disconnect")
                    {
                        //If lost connection to server, log out
                        MyAnswer = true;
                        break;
                    }
                    else
                    {
                        Console.Write("Could not create directory due to an error.\n" + changeresponse + "\n");
                    }
                    MyAnswer = false;
                    break;
                case "3":
                    Console.WriteLine(" You chose 3, List Files In Directory:  \n");
                    ListFiles listFiles = new ListFiles(conn);
                    String response3 = listFiles.ListFilesOnRemoteServer();
                    if (response3 == "success")
                    {
                        Console.Write("Success: These are the files in the directory: \n");
                    }
                    else
                    {
                        Console.Write("Error: Can not list files in current directory. \n" + response3 + "\n");
                    }

                    MyAnswer = false;
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine(" You chose: Upload file to Remote server  \n");
                    //File upload
                    FileUpload uploadfile = new FileUpload(conn);
                    FtpTestWrapper wrapper_file = new FtpTestWrapper();
                    Console.WriteLine(" ** Specify file to be uploaded \n (Mention absolute path in this format, for ex: C:/xyz/rst/abc.filetype. Filetypes accepted: .txt, .jpg, .png ** \n");
                    String filetobeuploaded = uploadfile.getFileName();
                    Console.WriteLine(" \n ** Specify valid directory on server where file is to be uploaded (for ex: Enter TEST for ftp://localhost/TEST) \n");
                    String locationonserver = uploadfile.getFileName();
                    String response_file = uploadfile.setup(filetobeuploaded, locationonserver);

                    if (response_file == "success")
                    {
                        Console.Write("** File successfully uploaded **\n \n");
                    }
                    else
                    {
                        Console.Write("Could not Upload file.\n" + response_file + "\n");
                        MyAnswer = true;
                        break;
                    }
                    MyAnswer = false;
                    break;
                case "1":
                      //File download
                    Console.WriteLine(" You choose 1, download File, in devl \n");
                    FileDownload RemoteFileDownload = new FileDownload(conn);
                    String response4 = RemoteFileDownload.FileDownloadFromRemote(conn);
                    if (response4 == "success")
                    {
                        Console.Write("File downloaded!\n");
                    }
                    else
                    {
                        Console.Write("Error: Could not download file.\n" + response4 + "\n");
                    }

                    MyAnswer = false;
                    //File upload
                    break;
                default:
                    Console.WriteLine("\n That was not a valid input, Please try again \n");
                    MyAnswer = false;
                    //File download
                    break;
            }
            return MyAnswer;
        } // end getResponce()
    } // end class MainMenu
} // end namespace ftpClientConApp
