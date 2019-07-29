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
using System.Threading;  // password
using FtpClientApp;

namespace FtpClient
{
    public class FtpClientMain
    {
        static void Main(string[] args)
        {
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("  Welcome to FTPClient \n");

            int running = 1;
            String username = "";
            String password = "";
            String server = "";

            while (running == 1)
            {
                Console.WriteLine("Press 1 to log in, Press 2 to exit");
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

                    bool timeout = false;
                    /**
                     * TO IMPLEMENT: LOG - IN
                     * Use the above info to try making a request to the server
                     * The request should be something like dirsize where only read permissions are necessary
                     * Only matters if the response code indicates success, if so proceed.
                     *
                     *
                     * TO IMPLEMENT: Time out
                     * Repeat similar to the above with the request to the server. If the response code
                     * doesn't indicate success, set timeout to true.
                     *
                     *
                     * TO IMPLEMENT: Log out
                     * if the input option is set to the log out option, set timeout to false
                     */

                    while (timeout == false)
                    {
                        DisplayMenu();
                        timeout = GetResponce(username, password, server);

                        //TIMEOUT -- probably want to do timeout check here
                    }
                    //On Logout, clear all user credentials stored
                   username = "";
                   password = "";
                   server = "";
                }
                else if (response == "2")
                {
                    running = 0;
                }
                else
                {
                    continue;
                }
            }

        } // end Main()


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
            Console.WriteLine("9) Logout from Server \n");

        } // end DisplayMenu()

        public static bool GetResponce(String username, String password, String server)
        {
            string getAnswer = "";
            bool MyAnswer = false;
            getAnswer = Console.ReadLine();
            ServerConnectionInformation conn = new ServerConnectionInformation(username, password, server);
            switch (getAnswer)
            {
                case "9":
                    Console.Clear();
                    Console.Write(username);
                    Console.WriteLine(" Logged out from server! \n");
                    Console.WriteLine(" ########################################### \n");
                    //Set response to 'true' to logout
                    MyAnswer = true;
                    break;
                case "8":
                    ListDirectoryLocal list = new ListDirectoryLocal();
                    list.ListDirectory();
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
                    Console.WriteLine(" Not Implemented Yet  \n");
                    //Change file permissions
                    MyAnswer = false;
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
                    FtpTestWrapper wrapper = new FtpTestWrapper();
                    String directory = createRemDir.getDirectoryName();
                    createRemDir.setWrapper(wrapper);
                    createRemDir.setup(directory);
                    String response = createRemDir.create(createRemDir.getWrapper());
                    if (response == "success")
                    {
                        Console.Write("Directory Created\n");
                    }
                    else if (response == "disconnect")
                    {
                        //If lost connection to server, log out
                        MyAnswer = true;
                        break;
                    }
                    else
                    {
                        Console.Write("Could not create directory due to an error.\n" + response + "\n");
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
                    Console.WriteLine(" Not Implemented Yet \n");
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
