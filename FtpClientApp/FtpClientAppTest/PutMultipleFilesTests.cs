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
    public class PutMultipleFilesTests
    {
        public PutMultipleFilesTests()
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
            PutMultipleFiles putmultiple = new PutMultipleFiles(info);
            String resp = putmultiple.create(server);
            Console.WriteLine(resp);
            Assert.IsTrue(resp.Equals("The remote server returned success (file found)"));

        }

        [TestMethod]
        public void MultipleFilesSuccessfullyCopiedreturnssuccess()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var server = A.Fake<FTPTestWrapperAbstract>();

            info.UserName = "un";
            info.PassWord = "pw";
            info.ServerName = "ftp://localhost";
            FtpWebResponse a = null;

            A.CallTo(() => server.getResp()).Returns(a);
            PutMultipleFiles putmultiple = new PutMultipleFiles(info);
            String resp = putmultiple.create(server);
            Assert.IsTrue(resp.Equals("success"));
        }
    }
}