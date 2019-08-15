using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FtpClientApp
{
    

    class FileDownloadMultiple
    {
        private ServerConnectionInformation connection;

        public FileDownloadMultiple(ServerConnectionInformation conn)
        {
            connection = conn;
        }

        public String Download()
        {
            string downloadLocation = "";
            downloadLocation = getDownloadPath();
            if (downloadLocation == "")
            {
                Console.Clear();
                return "disconnect";
            }

            List<string> files = new List<string>();
            Console.WriteLine("Downloading from "+connection.ServerName);

            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(connection.ServerName);
                ftpRequest.Credentials = new NetworkCredential(connection.UserName, connection.PassWord);
                ftpRequest.KeepAlive = false;
                ftpRequest.UsePassive = true;
                ftpRequest.Proxy = null;
                ftpRequest.UseBinary = true;
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                //populate file list
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    files.Add(line);
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("\n" + e.Message.ToString());
                PressAnyKeyToContinue();
                Console.Clear();
                return "disconnect";
            }
            catch (Exception e)
            {
                Console.WriteLine("\n"+e.Message.ToString());
                PressAnyKeyToContinue();
                Console.Clear();
                return "disconnect";
            }

            byte[] buffer = new byte[2048];
            int bytesRead = 0;
            try
            {
                FtpWebRequest myServerConnectionRequest = (FtpWebRequest)WebRequest.Create(connection.ServerName);
                myServerConnectionRequest.Credentials = new NetworkCredential(connection.UserName, connection.PassWord);
                myServerConnectionRequest.Proxy = null;
                myServerConnectionRequest.UsePassive = true;
                myServerConnectionRequest.UseBinary = true;
                myServerConnectionRequest.KeepAlive = true;

                foreach (var file in files)
                {
                    if (file.Contains("."))
                    {
                        String location = Path.Combine(downloadLocation, file.ToString());
                        Stream StreamReader = myServerConnectionRequest.GetResponse().GetResponseStream();
                        FileStream myFileStream = new FileStream(location, FileMode.Create);

                        while (true)
                        {
                            bytesRead = StreamReader.Read(buffer, 0, buffer.Length);

                            if (bytesRead == 0)
                            {
                                break;
                            }
                            myFileStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                PressAnyKeyToContinue();
                Console.Clear();
                return "disconnect";
            }
            Console.Clear();
            return "success";
        }

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
        public void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadLine();
        }
    }
}
