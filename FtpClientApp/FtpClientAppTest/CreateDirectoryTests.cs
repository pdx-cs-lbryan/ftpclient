using System;
using System.Collections.Generic;
using System.Text;

namespace FtpClientAppTest
{
    using FakeItEasy;
    using FtpClientApp;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Net;

    [TestClass]
    public class CreateDirectoryTests
    {
        public CreateDirectoryTests()
        {

        }

        [TestMethod]
        public void directoryAlreadyExistsReturnsMessageSayingItExists()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var server = A.Fake <FTPTestWrapperAbstract> ();
            
            info.UserName = "un";
            info.PassWord = "pw";
            info.ServerName = "ftp://localhost";
            WebException ex = new WebException(
                "The remote server returned an error: (550) File unavailable (e.g., file not found, no access).",
                WebExceptionStatus.ProtocolError);
            

            Console.WriteLine(ex.Message);
            A.CallTo(() => server.getResp()).Throws(ex);
            CreateRemoteDirectory crd = new CreateRemoteDirectory(info);
            String resp = crd.create(server);
            Console.WriteLine(resp);
            Assert.IsTrue(resp.Equals("The server sent an error code of 550. The directory may already exist or the file was unavailable due to a lack of access."));

        }

        [TestMethod]
        public void directoryNotExistsNoError()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var server = A.Fake<FTPTestWrapperAbstract>();

            info.UserName = "un";
            info.PassWord = "pw";
            info.ServerName = "ftp://localhost";
            FtpWebResponse a = null;


            A.CallTo(() => server.getResp()).Returns(a);
            CreateRemoteDirectory crd = new CreateRemoteDirectory(info);
            String resp = crd.create(server);
            Assert.IsTrue(resp.Equals("success"));
        }

        [TestMethod]
        public void invalidCharReturnsInvalidDir()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var server = A.Fake<FTPTestWrapperAbstract>();

            info.UserName = "un";
            info.PassWord = "pw";
            info.ServerName = "ftp://localhost";
            FtpWebResponse a = null;

            CreateRemoteDirectory crd = new CreateRemoteDirectory(info);
            bool test = crd.validDir("hello,this|is<not>valid");
            Assert.IsFalse(test);
        }
        
    }
}
