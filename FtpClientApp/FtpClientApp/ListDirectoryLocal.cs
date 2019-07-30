using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FtpClientApp
{
    class ListDirectoryLocal
    {
        public bool ListDirectory()
        {
            string Dir = @"c:\";
            bool success=false;
            Console.Clear();

            //get user input
            Console.WriteLine("Enter an absolute path to directory:");
            Dir = Console.ReadLine();

            //check the dir exsists
            if (!Directory.Exists(Dir))
            {
                Console.WriteLine("Directory does not exsist.");
                return success;
            }

            Console.WriteLine("\n");

            //check again if somehow it got deleted since initial check
            if (Directory.Exists(Dir))
            {
                string[] files = Directory.GetFiles(Dir);
                foreach (string fileName in files)
                {
                    ListFile(fileName);
                }
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory", Dir);
                return success;
            }

            //if reached here it was successful
            success = true;
            return success;
        }
        public void ListFile(string file)
        {
            FileInfo info = new FileInfo(file);
            FileAttributes attributes = info.Attributes;
            DateTime creationTime = info.CreationTime;
            Console.WriteLine("{2:-15} {0:100}\n    {3:-15} {1:30}\n", file, creationTime.ToString("f"), "File:", "Date Created:");
        }
    }
}
