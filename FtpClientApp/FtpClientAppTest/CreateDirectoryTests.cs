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
            var crd = A.Fake<CreateRemoteDirectory>();
            var server = A.Fake <FtpWebRequest> ();

            info.UserName = "un";
            info.PassWord = "pw";
            info.ServerName = "ftp://localhost";

            WebException ex = new WebException(
                "testmsg", (WebExceptionStatus) FtpStatusCode.ActionNotTakenFileUnavailable);
          

            A.CallTo(() => crd.getDirectoryName()).Returns("testdir");
            A.CallTo(() => server.GetResponse()).Throws(ex);
            String resp = crd.create(crd.getDirectoryName());
            Assert.IsTrue(resp.Equals("The server sent an error code of 550. The directory may already exist or the file was unavailable due to a lack of access."));

        }
        
    }
}
