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
    class ListDirectoryLocalTest
    {
        [TestMethod]
        public void ListDirectorySuccess()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var obj = A.Fake<ListDirectoryLocal>();

            info.UserName = "";
            info.PassWord = "";
            info.ServerName = "";
            var dir = "c:\\";

            Assert.AreEqual(obj.ListDirectory(dir), true);
        }

        [TestMethod]
        public void ListDirectoryFailure()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var obj = A.Fake<ListDirectoryLocal>();

            info.UserName = "";
            info.PassWord = "";
            info.ServerName = "";
            var dir = "ThisShouldReturnFalse";

            Assert.AreEqual(obj.ListDirectory(dir), false);
        }
    }
}
