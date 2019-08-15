using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FtpClientApp
{
    
    /*
     * Class for downloading multiple files from the server
     */
    class FileDownloadMultiple
    {
        private ServerConnectionInformation connection;

        //constructor that pulls in the connection info and assigns it
        public FileDownloadMultiple(ServerConnectionInformation conn)
        {
            connection = conn;
        }

        //main method to call to start the download
        public String Download()
        {
            Console.Clear();

            bool done = false;
            int numFiles = 0;
            List<string> fileList = new List<string>();

            //loop asking for files until the user says they are done
            while (!done)
            {
                string newFile = "";
                if (numFiles == 0)
                {
                    Console.WriteLine("Please enter the first file");
                }
                else
                {
                    Console.WriteLine("Please enter the next file");
                }
                newFile = Console.ReadLine();

                if (string.IsNullOrEmpty(newFile))
                {
                    Console.WriteLine("File cannot be empty");
                }
                else
                {
                    numFiles += 1;
                    fileList.Add(newFile);
                }

                //check if the user wants to keep entering files
                Console.WriteLine("Press Enter to enter another file or Q to quit");
                string ans = Console.ReadLine();
                if (ans == "Q" || ans == "q")
                {
                    done = true;
                }

                Console.Clear();
            }

            //output the files that are going to be downloaded
            Console.WriteLine("Downloading these files:");
            foreach(var file in fileList)
            {
                Console.WriteLine(file);
            }

            //get download path
            string downloadLocation = "";
            downloadLocation = getDownloadPath();

            //count of how many files completed
            int count = 0;

            //make sure at least one file is entered
            if (fileList.Count > 0)
            {
                FileDownload fd = new FileDownload(connection);
                foreach (var file in fileList)
                {
                    string response = fd.FileDownloadFromRemote(connection, file, downloadLocation);
                    if (response == "success") { count += 1; }
                }
            }
            else
            {
                return "disconnect";
            }

            //check all files were downloaded
            if (count == fileList.Count)
            {
                return "success";
            }
            else
            {
                Console.Write($"{count}");
                if (count == 1)
                {
                    Console.Write(" file");
                }
                else
                {
                    Console.Write(" files");
                }
                Console.WriteLine(" successfully downloaded");
                System.Threading.Thread.Sleep(3000);
                return "disconnect";
            }
        }

        /*
         * Class for getting input from the user on download path to use
         */
        public string getDownloadPath()
        {
            string path = "";
            bool found = false;

            Console.WriteLine("Please enter Download Location on Local Machine: ");
            Console.WriteLine("Example: c:\\Download: \nLocation must exist");

            while(!found)
            {
                path = Console.ReadLine();
                if (path == "")
                {
                    Console.WriteLine("Local File Path Entered is blank, Please try again ");
                    Console.WriteLine("Enter N to enter a new location OR Enter Q to disconnect");
                    string user = Console.ReadLine();
                    if (user == "N")
                    {
                        Console.WriteLine("Please enter your down location: \nExample: c:\\Downloads");
                    }
                    else if (user == "Q")
                    {
                        return "";
                    }
                    else
                    {
                        Console.WriteLine("Value Not Understood.");
                        Console.WriteLine("Please enter your down location: \nExample: c:\\Downloads");
                    }
                    
                }
                else if (!Directory.Exists(path))
                {
                    Console.WriteLine("Directory does not exsist.");
                    Console.WriteLine("Enter N to enter a new location OR Enter Q to disconnect");
                    string user = Console.ReadLine();
                    if (user == "N")
                    {
                        Console.WriteLine("Please enter your down location: \nExample: c:\\Downloads");
                    }
                    else if (user == "Q")
                    {
                        return "";
                    }
                    else
                    {
                        Console.WriteLine("Value Not Understood.");
                        Console.WriteLine("Please enter your down location: \nExample: c:\\Downloads");
                    }
                }
                else
                {
                    found = true;
                }  
            }
            return path;
        }

        /*
         * Method to get input from the user on continuing
         */
        public void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadLine();
        }
    }
}
