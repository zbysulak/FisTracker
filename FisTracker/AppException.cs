using System;
using System.Net;

namespace FisTracker
{
    public class AppException : Exception
    {
        public HttpStatusCode ResultCode { get; set; }
    }
}
