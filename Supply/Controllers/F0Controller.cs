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
            Object v = null;
            RequestPackage rqp = null;
            StreamReader reader = new StreamReader(Request.InputStream, Request.ContentEncoding);
            String body = reader.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(body))
            {
                if (body[0] == '{')
                {
                    rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
                }
                else if (body[0] == 's' && body.Length == 46)
                {
                    if (Guid.TryParse(body.Substring(10, 36), out Guid sessionId))
                    {
                        rqp = new RequestPackage
                        {
                            SessionId = sessionId
                        };
                    }
                }
            }
            if (rqp == null)
            {
                rqp = new RequestPackage
                {
                    SessionId = new Guid()
                };
            }
            F0Model m = new F0Model(rqp);
            v = PartialView("~/Views/F0/Index.cshtml", m);
            return v;
        }
        public Object ApplyFilter()
        {
            Object v = null;
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            F0Model m = new F0Model(rqp);
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        public Object GetOrderDetail()
        {
            Object v = "FNet.Supply.Controllers.F0Controller.GetOrderDetail()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            v = PartialView("~/Views/F0/АтрибутыЗаказа.cshtml", F0Model.GetOrderDetail(rqp));
            return v;
        }
        public Object GetPriceDetail()
        {
            Object v = "FNet.Supply.Controllers.F0Controller.GetPriceDetail()";
            RequestPackage rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            v = PartialView("~/Views/F0/АтрибутыЦены.cshtml", F0Model.GetPriceDetail(rqp));
            return v;
        }
    }
}