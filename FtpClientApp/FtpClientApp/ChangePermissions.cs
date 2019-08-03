using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FtpClientApp
{
    /*
     * Class which handles user permissions.
     * A Note on usage and testing:
     * Windows does not support CHMOD, and this program will notify
     * the user that a 504 error was received if a windows server is attempted to be
     * used. For testing, I was unable to confirm it working in an Ubuntu environment.
     * Several hours were spent attempting to get the program to compile and build, but
     * I could not find the tools necessary to do so. Significant time also went into 
     * attempting to connect to a server running inside a linux VM on a windows machine.
     * Unfortunately, I have only been able to confirm that windows will acknowledge the command and give a 504 error,
     * along the functionality of my code via unit tests.
     */
    public class ChangePermissions
    {
        private ServerConnectionInformation connection;

        /*
         *Constructor which sets connection to the passed in SCI
         */
        public ChangePermissions(ServerConnectionInformation conn)
        {
            this.connection = conn;
        }


        /*
         * A function which gets a path to use for changing permissions from the user
         * and sets it as the passed in wrapper's path.
         */
        public void setDir(FluentWrapper wrapper)
        {
            Console.WriteLine("\nEnter Path from Root of File to Change Permissions For : ");
            String dir = Console.ReadLine();
            wrapper.setPath(dir);
        }

        /**
         * This function is used to get permissions from the user to set.
         * The console prints information to the user on the allowed format
         * get the user's input, verifies it for correctness, and then
         * sets it as the passed in wrapper's permissions.
         */
        public void setPerms(FluentWrapper wrapper)
        {
            Console.WriteLine("\nPlease enter the 3 digit numeric value of the permissions to set.");
            Console.WriteLine("Accept values for each digit are 0-7.\nThe leftmost digit is for the user.\nThe center digit is for the group.\nThe right most digit is for Others.");
            Console.WriteLine("Values are: 0 - No Permissions, 1 - Execute only, 2 - Write Only, 3 - Write and Execute\n4 - Read Only, 5 - Read and Execute, 6 - Read and Write, 7 - Read, Write, Execute");
            Console.WriteLine("Enter Permissions to Set : ");
            bool ready = false;
            String value = "";
            while(ready != true)
            {

                //Check if length requirement is met
                ready = true;
                value = Console.ReadLine();
                ready = checkInput(value);

                if(ready == false)
                {
                    Console.WriteLine("\nPlease three digits with values 0-7 for each digit");
                }
            }

            wrapper.setPermission(int.Parse(value));
        }

        public bool checkInput(String value)
        {
            bool ready = true;
            if (value.Length != 3)
            {
                Console.WriteLine("\nPlease enter a 3 digit value");
                ready = false;
            }

            Char[] digits = value.ToCharArray();


            //check if each char is an allowed digit
            ready = true;
            for (int i = 0; i < digits.Length; i++)
            {
                if (digits[i] != '0' && digits[i] != '1' && digits[i] != '2' && digits[i] != '3' && digits[i] != '4' && digits[i] != '5' && digits[i] != '6' && digits[i] != '7')
                {
                    ready = false;
                }
            }

            return ready;
        }

        //Main class to change the permissions. Invokes the wrapper's run method
        //and returns success. If an exception arises, returns an appropriate error message.
        public String change(FTPTestWrapperAbstract wrapper)
        {
            //Note: This uses FluentFTP as the Microsoft library does not enable the use of SITE commands.
            //When I discussed this with the team, people wanted to use a combination of both libraries than switch
            //Exclusively to FluentFTP.
            try
            {
                //Open connection and make change
                wrapper.runFluent(0);
                return "success";

                //Handle Exceptions thrown by FluentFTP
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
            }catch (System.Net.Sockets.SocketException)
            {
                return "Error when attempting to connect - Socket connection refused.";
            }
            catch (WebException e)
            {

                if (e.Message.ToString().Equals("The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."))
                {
                    return "The server sent an error code of 550. The file may not exist or file was unavailable due to a lack of access.";
                }
                return e.Message.ToString();
            }
            catch (System.UriFormatException e)
            {
                return "Poorly formatted URI. Please enter a valid path.";
            }
        }
    }
}
