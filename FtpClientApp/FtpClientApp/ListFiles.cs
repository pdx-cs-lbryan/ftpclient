﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;

namespace FtpClientApp
{
    class ListFiles
    {
        public ListFiles(ServerConnectionInformation myConnection)
        {
            Console.WriteLine("\n ServerName:  " + myConnection.ServerName);
            Console.WriteLine("\n UserName:  " + myConnection.UserName);
            Console.WriteLine("\n Password:  " + myConnection.PassWord);

        } // end ListFiles
   
        
    } // end class ListFiles
} // end namespace FtpClientApp