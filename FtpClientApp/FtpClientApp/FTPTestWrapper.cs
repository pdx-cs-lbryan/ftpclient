using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FtpClientApp
{
    //Make a mock of this with your tests
    abstract public class FTPTestWrapperAbstract
    {
        abstract public FtpWebResponse getResp();
        
    }


    //Use This wrapper to make your request
    //You'll need to set the request first with setRequest, then invoke getResp
    public class FtpTestWrapper : FTPTestWrapperAbstract
    {
        FtpWebRequest req = null;

        public void setRequest(FtpWebRequest request)
        {
            this.req = request;
        }

        override
        public FtpWebResponse getResp()
        {
            try
            {

                return (FtpWebResponse)this.req.GetResponse();
            }
            catch (Exception e)
            {
                throw e;

            } 
        }
    }
}
