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
        private readonly byte[] v = { 0x68, 0x35, 0x54, 0x72, 0x44, 0x65, 0x77, 0x76, 0x42, 0x31, 0x69, 0x55, 0x33, 0x55, 0x50, 0x71 };
        private readonly byte[] k = { 0x76, 0x43, 0x38, 0x58, 0x61, 0x70, 0x67, 0x33, 0x46, 0x69, 0x42, 0x49, 0x30, 0x70, 0x54, 0x74, 0x59, 0x55, 0x62, 0x76, 0x49, 0x72, 0x45, 0x64, 0x35, 0x56, 0x33, 0x33, 0x48, 0x37, 0x68, 0x62 };
        private const string d = "dat";
        private const string f = "dat.txt";
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

        //Tests if the client has saved connection info to access
        public bool load_saved_info()
        {
            return false;
        }

        //return pass for used saved info
        public String getPass()
        {
            return this.passWord;
        }

        //return user name for using saved info
        public String getUser()
        {
            return this.UserName;
        }

        //Return server name for loading saved info
        public String getServer()
        {
            return this.serverName
        }

        //BaseDir gets where the three info files are stored
        private string BaseDirectory()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), d);
        }

        private string GetPath()
        {
            return Path.Combine(BaseDirectory(), f);
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
            string path = GetPath();

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            
            byte[] temp1 = Encrypt(ServerName, k, v);
            byte[] temp2 = Encrypt(UserName, k, v);
            byte[] temp3 = Encrypt(PassWord, k, v);
            File.WriteAllText(path, $"{Convert.ToBase64String(temp1)}\n{Convert.ToBase64String(temp2)}\n{Convert.ToBase64String(temp3)}");
        }

        private static byte[] Encrypt(string text, byte[] Key, byte[] IV)
        {
            byte[] encryptedText;

            using(Aes aesAlgo = Aes.Create())
            {
                aesAlgo.Key = Key;
                aesAlgo.IV = IV;

                ICryptoTransform encryptor = aesAlgo.CreateEncryptor(aesAlgo.Key, aesAlgo.IV);

                using(MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor,CryptoStreamMode.Write))
                    {
                        using(StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data
                            swEncrypt.Write(text);
                        }
                        encryptedText = msEncrypt.ToArray();
                    }
                }
                
            }
            return encryptedText;
        }

        private static string Decrypt(byte[] text, byte[] Key, byte[] IV)
        {
            string plainText = string.Empty;

            using(Aes aesAlgo = Aes.Create())
            {
                aesAlgo.Key = Key;
                aesAlgo.IV = IV;

                ICryptoTransform decryptor = aesAlgo.CreateDecryptor(aesAlgo.Key, aesAlgo.IV);

                using(MemoryStream msDecrypt = new MemoryStream(text))
                {
                    using(CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader scDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = scDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }


    }
    #endregion
}

