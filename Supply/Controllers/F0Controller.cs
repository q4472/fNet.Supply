using FNet.Supply.Models;
using Nskd;
using System;
using System.IO;
using System.Web.Mvc;

namespace FNet.Supply.Controllers
{
    public class F0Controller : Controller
    {
        public Object Index()
        {
            Object v = "FNet.Supply.Controllers.F0Controller.Index()";
            Guid sessionId = new Guid();
            StreamReader reader = new StreamReader(Request.InputStream, Request.ContentEncoding);
            String body = reader.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(body))
            {
                if (body[0] == '{')
                {
                    RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
                    sessionId = rqp.SessionId;
                }
                else if (body[0] == 's' && body.Length == 46)
                {
                    Guid.TryParse(body.Substring(10, 36), out sessionId);
                }
            }
            F0Model m = new F0Model(sessionId);
            v = PartialView("~/Views/F0/Index.cshtml", m);
            return v;
        }
    }
}