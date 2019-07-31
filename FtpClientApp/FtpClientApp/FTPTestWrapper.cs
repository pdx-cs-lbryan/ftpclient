using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FtpClientApp
{
    //Make a mock of this with your tests
    abstract public class FTPTestWrapperAbstract
    {
        //Abstract classes for using the Windows library (FtpWebResponse getResp) and the FluentFTP library (runFluent)
        abstract public FtpWebResponse getResp();
        abstract public void runFluent(int command);
        
    }


    //Use This wrapper to make your request
    //You'll need to set the request first with setRequest, then invoke getResp
    public class FtpTestWrapper : FTPTestWrapperAbstract
    {
        FtpWebRequest req = null;

        //Function for setting the class' Ftp request.
        public void setRequest(FtpWebRequest request)
        {
            this.req = request;
        }
        
        //Function for requesting the response from the server.
        override
        public FtpWebResponse getResp()
        {
            try
            {

                return (FtpWebResponse)this.req.GetResponse();
            }
            catch (Exception e)
            {
                throw e;

            } 
        }

        /*
         * Implemented for abstract class. Do not call this.
         */
        public override void runFluent(int command)
        {
            Console.WriteLine("The Wrong Method Was Called");
        }
    }


    //A wrapper for connecting to the server with FluentFTP. See ChangePermissions.cs
    public class FluentWrapper : FTPTestWrapperAbstract
    {
        //If you need to use data, add it here and add a method to set it.
        //Use this.x in your method to access the data.
        int permission = 0;
        String path = "";
        ServerConnectionInformation connection;

        /*
         * Sets the connection to the given ServerConnectionInformation
         */
        public void setConn(ServerConnectionInformation incoming)
        {
            this.connection = incoming;
        }

        //To set permission
        public void setPermission(int val)
        {
            this.permission = val;
        }

        /*
         * Sets the wrapper's path to the given string
         */
        public void setPath(String path)
        {
            this.path = path;
        }


        /*
         * Concrete class to call FluentFTP
         */
        override
        public void runFluent(int command)
        {
            //0 = change permissions
            if(command == 0)
            { try
                {
                    FluentFTP.FtpClient client = new FluentFTP.FtpClient(this.connection.ServerName);
                    client.Credentials = new System.Net.NetworkCredential(this.connection.UserName, this.connection.PassWord);
                    client.Connect();
                    client.Chmod(this.path, this.permission);
                    client.Disconnect();
                }
                catch (Exception e)
                {
                    throw e;
                }

            } 
        }

        /*
         * Implemented for abstract class. Do not use this.
         */
         override
        public FtpWebResponse getResp()
        {
            return null;
        }
    }
}
