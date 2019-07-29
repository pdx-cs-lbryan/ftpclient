using System;
using System.Collections.Generic;
using System.Text;

namespace FtpClientApp
{
    class ChangePermissions
    {
        private ServerConnectionInformation connection;

        public ChangePermissions(ServerConnectionInformation conn)
        {
            this.connection = conn;
        }

        public String getDir()
        {
            Console.WriteLine("\nEnter Path from Root of File to Change Permissions For : ");
            String dir = Console.ReadLine();
            return dir;
        }

        public int getPerms()
        {
            Console.WriteLine("\nPlease enter the 3 digit numeric value of the permissions to set.");
            Console.WriteLine("Accept values for each digit are 0-7.\nThe leftmost digit is for the user.\nThe center digit is for the group.\nThe right most digit is for Others.");
            Console.WriteLine("Values are: 0 - No Permissions, 1 - Execute only, 2 - Write Only, 3 - Write and Execute\n4 - Read Only, 5 - Read and Execute, 6 - Read and Write, 7 - Read, Write, Execute");
            Console.WriteLine("Enter Permissions to Set : ");
            bool ready = false;
            String value = "";
            while(ready != true)
            {
                ready = true;
                value = Console.ReadLine();
                if (value.Length != 3)
                {
                    Console.WriteLine("\nPlease enter a 3 digit value");
                    ready = false;
                    continue;
                }

                Char[] digits = value.ToCharArray();

                ready = true;
                for(int i = 0; i < digits.Length; i++)
                {
                    if(digits[i] != '0' && digits[i] != '1' && digits[i] != '2' && digits[i] != '3' && digits[i] != '4' && digits[i] != '5' && digits[i] != '6' && digits[i] != '7')
                    {
                        ready = false;
                    }
                }

                if(ready == false)
                {
                    Console.WriteLine("\nPlease use values 0-7 for each digit");
                }
            }

            return int.Parse(value);
        }

        public String change(String dir, int perms)
        {
            try
            {
                FluentFTP.FtpClient client = new FluentFTP.FtpClient(this.connection.ServerName);
                client.Credentials = new System.Net.NetworkCredential(this.connection.UserName, this.connection.PassWord);
                client.Chmod(dir, perms);
                return "success";
            } catch(FluentFTP.FtpAuthenticationException)
            {
                return "Server Validation Failed\n";
            } catch (ArgumentException)
            {
                return "The Path was Blank";
            } catch (FluentFTP.FtpCommandException e)
            {
                if(e.Message.Equals("Command not implemented for that parameter"))
                {
                    return "504 Error from Server: This Server Has Not Implemented the CHMOD command.\n";
                }
                return e.Message;
            }
        }
    }
}
