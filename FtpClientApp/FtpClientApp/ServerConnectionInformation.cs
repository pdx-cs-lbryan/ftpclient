using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FtpClientApp
{
    public class ServerConnectionInformation
    {
        #region Private Member Variables
        private string userName;
        private string passWord;
        private string serverName;
        //obviously shouldn't be called salt but is for readability
        private readonly string salt = "JEcv4Wqii5t";
        private const string directoryName = "ServerInformation";
        private const string userFile = "user.txt";
        private const string passFile = "pass.txt";
        private const string serverFile = "server.txt";
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

        //BaseDir gets where the three info files are stored
        private string BaseDirectory()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), directoryName);
        }

        private string UserFile()
        {
            return Path.Combine(BaseDirectory(), userFile);
        }

        private string ServerFile()
        {
            return Path.Combine(BaseDirectory(), serverFile);
        }

        private string PassFile()
        {
            return Path.Combine(BaseDirectory(), passFile);
        }

        public ServerConnectionInformation(String user, String pass, String server)
        {
            this.userName = user;
            this.passWord = pass;
            this.serverName = server;
        }

        public void Save()
        {
            string dirPath = BaseDirectory();
            string usrPath = UserFile();
            string pasPath = PassFile();
            string serPath = ServerFile();

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(usrPath, UserName);
            File.WriteAllText(pasPath, PassWord);
            File.WriteAllText(serPath, ServerName);
        }

        //unused hash function
        private string Hash(string str)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] passBytes = encoding.GetBytes(str);
            byte[] saltBytes = encoding.GetBytes(salt);

            byte[] combinedBytes = new byte[passBytes.Length + saltBytes.Length];

            for (int i = 0; i < passBytes.Length; i++)
            {
                combinedBytes[i] = passBytes[i];
            }
            for (int i = 0; i < saltBytes.Length; i++)
            {
                combinedBytes[passBytes.Length + i] = saltBytes[i];
            }
            byte[] final;
            using (SHA256 algorithm = SHA256.Create())
            {
                final = algorithm.ComputeHash(combinedBytes);
            }

            return encoding.GetString(final);
        }
    }
    #endregion
}

