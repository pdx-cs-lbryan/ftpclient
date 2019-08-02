using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FtpClientApp
{
    public class ServerConnectionInformation
    {
        #region Private Member Variables
        private string userName;
        private string passWord;
        private string serverName;
        private readonly string salt = "AppleSauce";
        #endregion

        #region Public Declarations
        #endregion

        #region Public Properties
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        public ServerConnectionInformation()
        {
            this.userName = null;
            this.passWord = null;
            this.serverName = null;
        }

        public ServerConnectionInformation(String user, String pass, String server)
        {
            this.userName = user;
            this.passWord = pass;
            this.serverName = server;
        }

        public void Save()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            string path = Directory.GetCurrentDirectory() + "connectionInfo";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string hash1 = Hash("password");
            string hash2 = Hash("password");
            Console.Write("First Hash: " + hash1 + "\nHash2: " + hash2);
            Console.WriteLine(hash1 == hash2);
        }

        public string Hash(string password)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] passBytes = encoding.GetBytes(password);
            byte[] saltBytes = encoding.GetBytes(salt);

            byte[] combinedBytes = new byte[passBytes.Length + saltBytes.Length];

            for (int i = 0; i < passBytes.Length; i++)
            {
                combinedBytes[i] = passBytes[i];
            }
            for(int i = 0; i < saltBytes.Length; i++)
            {
                combinedBytes[passBytes.Length + i] = saltBytes[i];
            }
            byte[] final; 
            using(SHA256 algorithm = SHA256.Create())
            {
                final = algorithm.ComputeHash(combinedBytes);
            }
            
            return encoding.GetString(final);
        }
        #endregion
    }




}

