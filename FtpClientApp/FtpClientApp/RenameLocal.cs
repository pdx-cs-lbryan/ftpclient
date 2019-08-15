using System;
using System.IO;
namespace FtpClientApp
{
    /*
     * Class for renaming a local file
     */
    public class RenameLocal
    {
        /*
         * Takes in a string File as the old path and a string newName to set as the new path
         */
        public String RenameFileLocal(String file, String newName)
        {
           
            String localFile = file;
            String NewlocalFile = newName;
            //check if file exists
            if (File.Exists(localFile))
            {
                //check if new directory exists
                if(Directory.Exists(newName) == false)
                {
                    return "New directory does not exist";
                }

                //try to set the new name
                try { 
                System.IO.File.Move(@localFile, @NewlocalFile);
                } catch (Exception e) {
                    return e.Message;

                }
                return "success";
            }
            else
            {
                return "File does not exist";
            }

        }
    }
}
