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

        private Object AddRowsToOrder()
        {
            //v += $"AddRowsToOrder({Nskd.Json.ToString(rqp)})";
            F0Model m = new F0Model(rqp);
            m.AddRowsToOrder();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object SplitRow()
        {
            //v += $"SplitRow({Nskd.Json.ToString(rqp)})";
            F0Model m = new F0Model(rqp);
            m.SplitRow();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object SetAsFree()
        {
            //v += $"SetAsFree({Nskd.Json.ToString(rqp)})";
            F0Model m = new F0Model(rqp);
            m.SetAsFree();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object ApplyFilter()
        {
            //v += $"ApplyFilter({Nskd.Json.ToString(rqp)})";
            F0Model m = new F0Model(rqp);
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object GetTableDetail()
        {
            //v += $"GetTableDetail({Nskd.Json.ToString(rqp)})\n";
            F0Model m = new F0Model(rqp);
            var items = m.GetTableDetail();
            v = PartialView("~/Views/F0/АтрибутыТаблицы.cshtml", items);
            return v;
        }
        private Object OrderTableUpdate()
        {
            //v += $"OrderTableUpdate({Nskd.Json.ToString(rqp)})\n";
            F0Model m = new F0Model(rqp);
            m.OrderTableUpdate();
            //v = PartialView("~/Views/F0/АтрибутыТаблицы.cshtml", items);
            return v;
        }
        private Object SetSupplier()
        {
            //v += $"SetSupplier({Nskd.Json.ToString(rqp)})";
            F0Model m = new F0Model(rqp);
            m.SetSupplier();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object GetHeadDetail()
        {
            //v += $"GetHeadDetail({Nskd.Json.ToString(rqp)})\n";
            F0Model m = new F0Model(rqp);
            var items = m.GetHeadDetail();
            v = PartialView("~/Views/F0/АтрибутыШапки.cshtml", items);
            return v;
        }
        private Object OrderHeadUpdate()
        {
            v += $"OrderHeadUpdate({Nskd.Json.ToString(rqp)})\n";
            F0Model m = new F0Model(rqp);
            m.OrderHeadUpdate();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }

        public Object Index()
        {
            v = "FNet.Supply.Controllers.F0Controller.Index()\n";
            rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            if (rqp != null)
            {
                switch (rqp.Command)
                {
                    case "Supply.F0.AddRowsToOrder":
                        v = AddRowsToOrder();
                        break;
                    case "Supply.F0.SplitRow":
                        v = SplitRow();
                        break;
                    case "Supply.F0.SetAsFree":
                        v = SetAsFree();
                        break;
                    case "Supply.F0.ApplyFilter":
                        v = ApplyFilter();
                        break;
                    case "Supply.F0.SetSupplier":
                        v = SetSupplier();
                        break;
                    case "Supply.F0.GetHeadDetail":
                        v = GetHeadDetail();
                        break;
                    case "Supply.F0.OrderHeadUpdate":
                        v = OrderHeadUpdate();
                        break;
                    case "Supply.F0.GetTableDetail":
                        v = GetTableDetail();
                        break;
                    case "Supply.F0.OrderTableUpdate":
                        v = OrderTableUpdate();
                        break;
                    default:
                        //v += $"Неизвестная команда: '{rqp.Command}'.\n";
                        F0Model m = new F0Model(rqp);
                        m.ApplyFilter();
                        v = PartialView("~/Views/F0/Index.cshtml", m);
                        break;
                }
            }
            else { v += "Отсутствует RequestPackage."; }
            return v;
        }
    }
}