using FNet.Supply.Models;
using Nskd;
using System;
using System.Web.Mvc;

namespace FNet.Supply.Controllers
{
    public class F0Controller : Controller
    {
        private Object v;
        private RequestPackage rqp;

        public Object Index()
        {
            v = "FNet.Supply.Controllers.F0Controller.Index()\n";
            rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            switch (rqp.Command)
            {
                case "Supply.F0.SetSupplier":
                    v = SetSupplier();
                    break;
                case "Supply.F0.GetOrderDetail":
                    v = GetOrderDetail();
                    break;
                case "Supply.F0.OrderHeadUpdate":
                    v = OrderHeadUpdate();
                    break;
                case "Supply.F0.GetPriceDetail":
                    v = GetPriceDetail();
                    break;
                default:
                    F0Model m = new F0Model(rqp);
                    v = PartialView("~/Views/F0/Index.cshtml", m);
                    break;
            }
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

        private Object GetPriceDetail()
        {
            v += $"GetPriceDetail({Nskd.Json.ToString(rqp)})\n";
            var items = F0Model.GetPriceDetail(rqp);
            v = PartialView("~/Views/F0/АтрибутыЦены.cshtml", items);
            return v;
        }
        private Object SetSupplier()
        {
            v += $"SetSupplier({Nskd.Json.ToString(rqp)})";
            F0Model.SetSupplier(rqp);
            F0Model m = new F0Model(rqp);
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object GetOrderDetail()
        {
            v += $"GetOrderDetail({Nskd.Json.ToString(rqp)})\n";
            var items = F0Model.GetOrderDetail(rqp);
            v = PartialView("~/Views/F0/АтрибутыЗаказа.cshtml", items);
            return v;
        }
        private Object OrderHeadUpdate()
        {
            v += $"OrderHeadUpdate({Nskd.Json.ToString(rqp)})\n";
            F0Model.OrderHeadUpdate(rqp);
            F0Model m = new F0Model(rqp);
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
    }
}