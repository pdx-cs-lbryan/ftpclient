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

            //verify that encoding and decoding works
            //
            //string e = EncodeToBase64(PassWord);
            //string c = DecodeFrom64(e);
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine(PassWord);
            //sb.AppendLine(e);
            //sb.AppendLine(c);
            //Console.WriteLine(sb.ToString());

            File.WriteAllText(usrPath, EncodeToBase64(UserName));
            File.WriteAllText(pasPath, EncodeToBase64(PassWord));
            File.WriteAllText(serPath, EncodeToBase64(ServerName));
        }

        //encryption taken from https://www.c-sharpcorner.com/blogs/how-to-encrypt-or-decrypt-password-using-asp-net-with-c-sharp1
        public static string EncodeToBase64(string value)
        {
            try
            {
                byte[] encData_byte = new byte[value.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(value);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        //decryption taken from https://www.c-sharpcorner.com/blogs/how-to-encrypt-or-decrypt-password-using-asp-net-with-c-sharp1
        public string DecodeFrom64(string value)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(value);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
    #endregion
}

