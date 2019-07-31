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
    public class FileUploadTests
    {
        public FileUploadTests()
        {

        }

        [TestMethod]
        public void FileAlreadyExistsReturnsappropriatemessage()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var server = A.Fake<FTPTestWrapperAbstract>();

            info.UserName = "un";
            info.PassWord = "pw";
            info.ServerName = "ftp://localhost";
            WebException ex = new WebException(
                "The remote server returned success (file found)",
                WebExceptionStatus.ProtocolError);


            Console.WriteLine(ex.Message);
            A.CallTo(() => server.getResp()).Throws(ex);
            FileUpload fileupload = new FileUpload(info);
            String resp = fileupload.create(server);
            Console.WriteLine(resp);
            Assert.IsTrue(resp.Equals("The remote server returned success (file found)"));

        }

        [TestMethod]
        public void FileSuccessfullyUploadedreturnssuccess()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var server = A.Fake<FTPTestWrapperAbstract>();

            info.UserName = "un";
            info.PassWord = "pw";
            info.ServerName = "ftp://localhost";
            FtpWebResponse a = null;

            A.CallTo(() => server.getResp()).Returns(a);
            FileUpload fileupload = new FileUpload(info);
            String resp = fileupload.create(server);
            Assert.IsTrue(resp.Equals("success"));
        }
    }
}