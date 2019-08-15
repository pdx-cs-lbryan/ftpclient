using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FtpClientApp
{

    /*
     * Class for listing the files of a local directory
     */
    public class ListDirectoryLocal
    {
        /*
         * Takes in a string for a directory to list. Returns true for success, false otherwise.
         */
        public bool ListDirectory(string Dir)
        {
            bool success=false;

            //check the dir exsists
            if (!Directory.Exists(Dir))
            {
                Console.WriteLine("Directory does not exsist.");
                return success;
            }

            //check again if somehow it got deleted since initial check
            if (Directory.Exists(Dir))
            {
                Console.WriteLine("Directories: \n");
                string[] directories = Directory.GetDirectories(Dir);
                foreach (string directory in directories)
                {
                    ListFile(directory);
                }
                Console.WriteLine("\nFiles: \n");
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

        /*
         * Gets file information from the file passed in and writes it out to the console.
         */
        public void ListFile(string file)
        {
            FileInfo info = new FileInfo(file);
            FileAttributes attributes = info.Attributes;
            DateTime creationTime = info.CreationTime;
            Console.WriteLine("{2:-15} {0:100}\n    {3:-15} {1:30}\n", file, creationTime.ToString("f"), "Name:", "Date Created:");
        }
    }
}
