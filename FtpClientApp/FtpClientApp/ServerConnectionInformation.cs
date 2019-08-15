using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FtpClientApp
{

    /*
     * Class for holding connection information, using it, and storing it.
     */
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

        /*
         * Functions for getting and setting the username
         */
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /*
         * Functions for getting and setting the password
         */
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        /*
         * Functions for getting and setting the servername
         */
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        //Constructor to set everything to null
        public ServerConnectionInformation()
        {
            this.userName = null;
            this.passWord = null;
            this.serverName = null;
        }

        //Tests if the client has saved connection info to access
        public bool load_saved_info()
        {
            String dirPath = BaseDirectory();
            String path = GetPath();
            
            //return false if dir or file don't exist
            if (!Directory.Exists(dirPath))
            {
                return false;
            } else if(!File.Exists(path))
            {
                return false;
            } else
            {
                String info = "";
                try
                {
                    //get text from file
                    System.IO.StreamReader saved = new System.IO.StreamReader(path);

                    info = saved.ReadToEnd();
                    saved.Close();
                } catch(ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }  catch(FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                } catch(DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                } catch(IOException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                
                if(info == "")
                {
                    //return false if empty
                    return false;
                }
                else
                {
                    //Get different parts of info
                    String[] parts = info.Split('\n');
                    if(parts.Length != 3)
                    {
                        return false;
                    } else
                    {
                        try
                        {
                            //decrypt
                            String se = parts[0];
                            String ue = parts[1];
                            String pe = parts[2];
                            byte[] sbe = Convert.FromBase64String(se);
                            byte[] ube = Convert.FromBase64String(ue);
                            byte[] pbe = Convert.FromBase64String(pe);

                            this.ServerName = Decrypt(sbe, this.k, this.v);
                            this.UserName = Decrypt(ube, this.k, this.v);
                            this.PassWord = Decrypt(pbe, this.k, this.v);
                            return true;

                        } catch (ArgumentNullException e)
                        {
                            Console.WriteLine("Invalid Number of Arguments in Saved Info File");
                            return false;
                        } catch (FormatException e)
                        {
                            Console.WriteLine("Wrong format in saved info file");
                            return false;
                        }
                        
                    }
                }

                    
            }
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
            return this.serverName;
        }

        //BaseDir gets where the three info files are stored
        private string BaseDirectory()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), d);
        }

        //Returns path of file
        private string GetPath()
        {
            return Path.Combine(BaseDirectory(), f);
        }

        //Constructor with parameters for name, pass, and server given
        public ServerConnectionInformation(String user, String pass, String server)
        {
            this.userName = user;
            this.passWord = pass;
            this.serverName = server;
        }

        /*
         * Saves user information to a file in Base64String from encrypted bytes
         */
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

        /*
         * Encrypt a text with a key and IV byte arrays.
         * Based on documentation from Microsoft:https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netframework-4.8
         */
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


        /*
         * Function for decrypting text as bytes from a key and IV byte array
         * Based on documentation from Microsoft: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netframework-4.8
         */
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

