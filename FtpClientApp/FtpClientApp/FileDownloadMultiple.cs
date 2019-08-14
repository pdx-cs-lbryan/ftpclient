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

            string direcoryName = "";
            Console.WriteLine("Please enter the directory name on the server:");
            direcoryName = Console.ReadLine();
          
            String directoryPath = Path.Combine(connection.ServerName, direcoryName);
            Console.WriteLine(connection.ServerName);
            Console.WriteLine(directoryPath);
            List<string> files = new List<string>();

            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(connection.ServerName);
                ftpRequest.Credentials = new NetworkCredential(connection.UserName, connection.PassWord);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                //populate file list
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    //Console.WriteLine(line); //output for testing
                    files.Add(line);
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("\n" + e.Message.ToString());
                System.Threading.Thread.Sleep(5000);
                Console.Clear();
                return "disconnect";
            }
            catch (Exception e)
            {
                Console.WriteLine("\n"+e.Message.ToString());
                System.Threading.Thread.Sleep(5000);
                Console.Clear();
                return "disconnect";
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(connection.UserName, connection.PassWord);

                    foreach (var file in files)
                    {
                        if (file.Contains("."))
                        {
                            String path = Path.Combine(connection.ServerName, file.ToString());
                            String location = Path.Combine(connection.ServerName, file.ToString());
                            client.DownloadFile(path, location);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return "disconnect";
            }
            return "success";
        }

        public string getDownloadPath()
        {
            bool done = false;
            string path = "";
            string answer = "";

            Console.WriteLine("Please enter your download location: ");
            Console.WriteLine("Example: c:\\Download: \nLocation must exist");

            bool found = false;
            while(!found)
            {
                path = Console.ReadLine();
                if (path == "")
                {
                    Console.WriteLine("Local File Path Entered is blank, Please try again ");
                }
                else if (!Directory.Exists(path))
                {
                    Console.WriteLine("Directory does not exsist.");
                }
                else
                {
                    found = true;
                }
                    
            }
            
            return path;
        }
    }
}
