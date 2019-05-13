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
        F0Model m;

        private Object TestForUpdate()
        {
            v += $"TestForUpdate({Nskd.Json.ToString(rqp)})\n";
            try
            {
                v = m.TestForUpdate();
            }
            catch (Exception e) { v += e.Message; }
            return v;
        }
        private Object AddRowsToOrder()
        {
            //v += $"AddRowsToOrder({Nskd.Json.ToString(rqp)})\n";
            m.AddRowsToOrder();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object SplitRow()
        {
            //v += $"SplitRow({Nskd.Json.ToString(rqp)})\n";
            m.SplitRow();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object SetAsFree()
        {
            //v += $"SetAsFree({Nskd.Json.ToString(rqp)})\n";
            m.SetAsFree();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object ApplyFilter()
        {
            //v += $"ApplyFilter({Nskd.Json.ToString(rqp)})\n";
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object GetTableDetail()
        {
            //v += $"GetTableDetail({Nskd.Json.ToString(rqp)})\n";
            var items = m.GetTableDetail();
            v = PartialView("~/Views/F0/АтрибутыТаблицы.cshtml", items);
            return v;
        }
        private Object OrderTableUpdate()
        {
            //v += $"OrderTableUpdate({Nskd.Json.ToString(rqp)})\n";
            m.OrderTableUpdate();
            //v = PartialView("~/Views/F0/АтрибутыТаблицы.cshtml", items);
            return v;
        }
        private Object SetSupplier()
        {
            //v += $"SetSupplier({Nskd.Json.ToString(rqp)})\n";
            m.SetSupplier();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }
        private Object GetHeadDetail()
        {
            //v += $"GetHeadDetail({Nskd.Json.ToString(rqp)})\n";
            var items = m.GetHeadDetail();
            v = PartialView("~/Views/F0/АтрибутыШапки.cshtml", items);
            return v;
        }
        private Object OrderHeadUpdate()
        {
            v += $"OrderHeadUpdate({Nskd.Json.ToString(rqp)})\n";
            m.OrderHeadUpdate();
            m.ApplyFilter();
            v = PartialView("~/Views/F0/Table.cshtml", m);
            return v;
        }

        public Object Index()
        {
            v = "FNet.Supply.Controllers.F0Controller.Index()\n";
            if (Request.HttpMethod == "GET" && Request.Path.Length == 32 && Request.Path.Substring(0, 24) == "/supply/f0/filedownload/")
            {
                // Path = "/supply/f0/filedownload/01cc6f8b"
                String fileId = Request.Path.Substring(24, 8);
                var (FileName, FileContents) = F0Model.FileDownload(fileId);
                FileContentResult fileContentResult = new FileContentResult(FileContents, "application/octet-stream");
                fileContentResult.FileDownloadName = FileName;
                return fileContentResult;
            }
            rqp = RequestPackage.ParseRequest(Request.InputStream, Request.ContentEncoding);
            if (rqp != null)
            {
                m = new F0Model(rqp);
                switch (rqp.Command)
                {
                    case "Supply.F0.TestForUpdate":
                        v = TestForUpdate();
                        break;
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