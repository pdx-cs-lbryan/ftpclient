using System;
using System.IO;
namespace FtpClientApp
{
    public class RenameLocal
    {
        public String RenameFileLocal(String file, String newName)
        {
            String localFile = '/' + file;
            String NewlocalFile = '/' + newName;
            if (File.Exists(localFile))
            {
                System.IO.File.Move(@localFile, @NewlocalFile);
                return "success";
            }
            else
            {
                return "File does not exist";
            }

        }
    }
}
