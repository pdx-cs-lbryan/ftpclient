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
    public class ChangePermissionsTests
    {
        public ChangePermissionsTests()
        {

        }

        [TestMethod]
        public void errorOn504()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var server = A.Fake<FTPTestWrapperAbstract>();

            info.UserName = "un";
            info.PassWord = "pw";
            info.ServerName = "ftp://localhost";
            FluentFTP.FtpCommandException e = new FluentFTP.FtpCommandException("504", "Command not implemented for that parameter");
            A.CallTo(() => server.runFluent(0)).Throws(e);

            ChangePermissions p = new ChangePermissions(info);
            String resp = p.change(server);

            Console.WriteLine(resp);
            Assert.IsTrue(resp.Equals("504 Error from Server: This Server Has Not Implemented the CHMOD command.\n"));
        }

        [TestMethod]
        public void noErrorSuccess()
        {
            var info = A.Fake<ServerConnectionInformation>();
            var server = A.Fake<FTPTestWrapperAbstract>();

            info.UserName = "un";
            info.PassWord = "pw";
            info.ServerName = "ftp://localhost";
            FluentFTP.FtpCommandException e = new FluentFTP.FtpCommandException("504", "Command not implemented for that parameter");
            A.CallTo(() => server.runFluent(0));

            ChangePermissions p = new ChangePermissions(info);
            String resp = p.change(server);

            Console.WriteLine(resp);
            Assert.IsTrue(resp.Equals("success"));
        }
    }
}
