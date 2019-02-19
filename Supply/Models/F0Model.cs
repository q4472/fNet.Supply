using Nskd;
using System;
using System.Collections;
using System.Data;
using System.Text;

namespace FNet.Supply.Models
{
    public class F0Model
    {
        public RequestPackage Rqp;
        public FilterData Filter;
        public FilteredData Data;
        public DataTable Поставщики;
        public DataTable СостоянияЗаказа;

        public F0Model(RequestPackage rqp)
        {
            Rqp = rqp;
            Filter = new FilterData(this);
            Data = new FilteredData(this);
            Поставщики = ПолучитьСписокПоставщиков(this);
            СостоянияЗаказа = ПолучитьСписокСостоянийЗаказа(this);
        }

        public class FilterData
        {
            public String все = "False"; // фильтр по полю "обработано". По умолчанию обработанные строки не показываем.
            public String дата_min = "";
            public String дата_max = "";
            public String менеджер = "";

            public FilterData(F0Model m)
            {
                if (m.Rqp != null)
                {
                    все = (m.Rqp["все"] == null) ? "False" : ((Boolean)m.Rqp["все"]).ToString();
                    дата_min = (m.Rqp["дата_min"] == null) ? "" : (String)m.Rqp["дата_min"];
                    дата_max = (m.Rqp["дата_max"] == null) ? "" : (String)m.Rqp["дата_max"];
                    менеджер = (m.Rqp["менеджер"] == null) ? "" : (String)m.Rqp["менеджер"];
                }
            }
        }
        public class FilteredData
        {
            private DataTable dt;
            public Int32 RowsCount { get => (dt == null) ? 0 : dt.Rows.Count; }
            public class ItemArray
            {
                public String заказы_у_поставщиков_таблица__uid;
                public String заказ;
                public String заказ_номер;
                public String заказ_обработано;
                public String заказ_поставщик;
                public String заказ_поставщик_наименование;
                public String заказ_состояние;
                public String заказ_состояние_наименование;
                public String заказ_примечание;
                public String заказ_номер_их;
                public String товар;
                public String товар_описание;
                public String товар_примечание;
                public String товар_спецификация;
                public String товар_спецификация_номер;
                public String товар_спецификация_менеджер;

                public String this[String fieldName]
                {
                    get
                    {
                        String s = null;
                        var field = typeof(ItemArray).GetField(fieldName);
                        if (field != null)
                        {
                            s = (String)field.GetValue(this);
                        }
                        return s;
                    }
                }
            }
            public FilteredData(F0Model m)
            {
                if (m.Rqp != null && m.Rqp.SessionId != null)
                {
                    RequestPackage rqp = new RequestPackage();
                    rqp.SessionId = m.Rqp.SessionId;
                    rqp.Command = "Supply.dbo.заказы_у_поставщиков__получить";
                    rqp.Parameters = new RequestParameter[]
                    {
                        new RequestParameter() { Name = "session_id", Value = m.Rqp.SessionId },
                        new RequestParameter() { Name = "все", Value = m.Filter.все }
                    };
                    if (!String.IsNullOrWhiteSpace(m.Filter.дата_min)) rqp["дата_min"] = m.Filter.дата_min;
                    if (!String.IsNullOrWhiteSpace(m.Filter.дата_max)) rqp["дата_max"] = m.Filter.дата_max;
                    if (!String.IsNullOrWhiteSpace(m.Filter.менеджер)) rqp["менеджер"] = m.Filter.менеджер;
                    ResponsePackage rsp = rqp.GetResponse("http://127.0.0.1:11012");
                    if (rsp != null)
                    {
                        dt = rsp.GetFirstTable();
                    }
                }
            }
            public ItemArray this[Int32 index]
            {
                get
                {
                    ItemArray items = null;
                    if (dt != null && index >= 0 && index < dt.Rows.Count)
                    {
                        DataRow dr = dt.Rows[index];
                        items = new ItemArray
                        {
                            заказы_у_поставщиков_таблица__uid = ConvertToString(dr["заказы_у_поставщиков_таблица__uid"]),
                            заказ = ConvertToString(dr["заказ"]),
                            заказ_номер = ConvertToString(dr["заказ_номер"]),
                            заказ_обработано = ConvertToString(dr["заказ_обработано"]),
                            заказ_поставщик = ConvertToString(dr["заказ_поставщик"]),
                            заказ_поставщик_наименование = ConvertToString(dr["заказ_поставщик_наименование"]),
                            заказ_состояние = ConvertToString(dr["заказ_состояние"]),
                            заказ_состояние_наименование = ConvertToString(dr["заказ_состояние_наименование"]),
                            заказ_примечание = ConvertToString(dr["заказ_примечание"]),
                            заказ_номер_их = ConvertToString(dr["заказ_номер_их"]),
                            товар = ConvertToString(dr["товар"]),
                            товар_описание = ConvertToString(dr["товар_описание"]),
                            товар_примечание = ConvertToString(dr["товар_примечание"]),
                            товар_спецификация = ConvertToString(dr["товар_спецификация"]),
                            товар_спецификация_номер = ConvertToString(dr["товар_спецификация_номер"]),
                            товар_спецификация_менеджер = ConvertToString(dr["товар_спецификация_менеджер"])
                        };
                    }
                    return items;
                }
            }
            private String ConvertToString(Object v)
            {
                String s = String.Empty;
                if (v != null && v != DBNull.Value)
                {
                    String tfn = v.GetType().FullName;
                    switch (tfn)
                    {
                        case "System.Guid":
                            s = ((Guid)v).ToString();
                            break;
                        case "System.Int32":
                            s = ((Int32)v).ToString();
                            break;
                        case "System.Boolean":
                            s = ((Boolean)v).ToString();
                            break;
                        case "System.String":
                            s = (String)v;
                            break;
                        default:
                            s = tfn;
                            break;
                    }
                }
                return s;
            }
        }
        public class ЗаказыУПоставщиковШапка
        {
            private DataTable dt;
            public ЗаказыУПоставщиковШапка(Guid sessionId, Guid order_uid)
            {
                    RequestPackage rqp = new RequestPackage();
                    rqp.SessionId = sessionId;
                    rqp.Command = "Supply.dbo.заказы_у_поставщиков_шапка__получить";
                    rqp.Parameters = new RequestParameter[]
                    {
                        new RequestParameter() { Name = "session_id", Value = sessionId },
                        new RequestParameter() { Name = "order_uid", Value = order_uid }
                    };
                    ResponsePackage rsp = rqp.GetResponse("http://127.0.0.1:11012");
                    if (rsp != null)
                    {
                        dt = rsp.GetFirstTable();
                    }
            }
            public Int32 RowsCount { get => (dt == null) ? 0 : dt.Rows.Count; }
            public class ItemArray
            {
                public String uid;
                public String id;
                public String обработано;
                public String поставщик;
                public String поставщик_наименование;
                public String состояние;
                public String состояние_наименование;
                public String примечание;
                public String номер; // их

                public String this[String fieldName]
                {
                    get
                    {
                        String s = null;
                        var field = typeof(ItemArray).GetField(fieldName);
                        if (field != null)
                        {
                            s = (String)field.GetValue(this);
                        }
                        return s;
                    }
                }
            }
            public ItemArray this[Int32 index]
            {
                get
                {
                    ItemArray items = null;
                    if (dt != null && index >= 0 && index < dt.Rows.Count)
                    {
                        DataRow dr = dt.Rows[index];
                        items = new ItemArray
                        {
                            uid = ConvertToString(dr["uid"]),
                            id = ConvertToString(dr["id"]),
                            обработано = ConvertToString(dr["обработано"]),
                            поставщик = ConvertToString(dr["поставщик"]),
                            поставщик_наименование = ConvertToString(dr["поставщик_наименование"]),
                            состояние = ConvertToString(dr["состояние"]),
                            состояние_наименование = ConvertToString(dr["состояние_наименование"]),
                            примечание = ConvertToString(dr["примечание"]),
                            номер = ConvertToString(dr["номер"])
                        };
                    }
                    return items;
                }
            }
            private String ConvertToString(Object v)
            {
                String s = String.Empty;
                if (v != null && v != DBNull.Value)
                {
                    String tfn = v.GetType().FullName;
                    switch (tfn)
                    {
                        case "System.Guid":
                            s = ((Guid)v).ToString();
                            break;
                        case "System.Int32":
                            s = ((Int32)v).ToString();
                            break;
                        case "System.Boolean":
                            s = ((Boolean)v).ToString();
                            break;
                        case "System.String":
                            s = (String)v;
                            break;
                        default:
                            s = tfn;
                            break;
                    }
                }
                return s;
            }
        }
        public DataTable ПолучитьСписокПоставщиков(F0Model m)
        {
            DataTable dt = null;
            RequestPackage rqp = new RequestPackage
            {
                SessionId = m.Rqp.SessionId,
                Command = "Supply.dbo.поставщики__получить",
                Parameters = new RequestParameter[]
                {
                    new RequestParameter() { Name = "session_id", Value = m.Rqp.SessionId }
                }
            };
            ResponsePackage rsp = rqp.GetResponse("http://127.0.0.1:11012");
            if (rsp != null)
            {
                dt = rsp.GetFirstTable();
            }
            return dt;
        }
        public DataTable ПолучитьСписокСостоянийЗаказа(F0Model m)
        {
            DataTable dt = null;
            RequestPackage rqp = new RequestPackage
            {
                SessionId = m.Rqp.SessionId,
                Command = "Supply.dbo.состояния_заказа__получить",
                Parameters = new RequestParameter[]
                {
                    new RequestParameter() { Name = "session_id", Value = m.Rqp.SessionId }
                }
            };
            ResponsePackage rsp = rqp.GetResponse("http://127.0.0.1:11012");
            if (rsp != null)
            {
                dt = rsp.GetFirstTable();
            }
            return dt;
        }

        public static ЗаказыУПоставщиковШапка.ItemArray GetOrderDetail(RequestPackage rqp)
        {
            ЗаказыУПоставщиковШапка.ItemArray items = null;
            if (rqp != null && rqp.SessionId != null)
            {
                Guid.TryParse(rqp["order_uid"] as String, out Guid orderUid);
                ЗаказыУПоставщиковШапка zh = new ЗаказыУПоставщиковШапка(rqp.SessionId, orderUid);
                if (zh.RowsCount > 0)
                {
                    items = zh[0];
                }
            }
            return items;
        }
        public static DataTable GetPriceDetail(RequestPackage rqp)
        {
            DataTable dt = null;
            if (rqp != null && rqp.SessionId != null)
            {
                Guid.TryParse(rqp["uid"] as String, out Guid uid);
                rqp.Command = "Supply.dbo.заказ_у_поставщика__получить_атрибуты_цены";
                rqp.Parameters = new RequestParameter[]
                {
                        new RequestParameter() { Name = "session_id", Value = rqp.SessionId },
                        new RequestParameter() { Name = "заказы_у_поставщиков_таблица__uid", Value = uid }
                };
                ResponsePackage rsp = rqp.GetResponse("http://127.0.0.1:11012");
                if (rsp != null)
                {
                    dt = rsp.GetFirstTable();
                }
            }
            return dt;
        }
        public static void SetSupplier(RequestPackage rqp)
        {
            Hashtable setSupplierValue = (Hashtable)rqp["SetSupplier"];
            Guid supplierUid = new Guid();
            String supplierName = null;
            StringBuilder uids = new StringBuilder();
            foreach (DictionaryEntry nvp in setSupplierValue)
            {
                if (nvp.Key as String == "supplier_uid")
                {
                    Guid.TryParse(nvp.Value as String, out supplierUid);
                }
                if (nvp.Key as String == "supplier_name")
                {
                    supplierName = nvp.Value as String;
                }
                if (nvp.Key as String == "uids")
                {
                    Object[] t = nvp.Value as Object[];
                    foreach (Object o in t)
                    {
                        uids.AppendFormat($"<a b=\"{o}\"/>");
                    }
                }
            }
            RequestPackage rqp1 = new RequestPackage()
            {
                SessionId = rqp.SessionId,
                Command = "Supply.dbo.заказы_у_поставщиков__установить_поставщика"
            };
            rqp1.Parameters = new RequestParameter[]
            {
                        new RequestParameter() { Name = "session_id", Value = rqp.SessionId },
                        new RequestParameter() { Name = "supplier_uid", Value = supplierUid },
                        new RequestParameter() { Name = "supplier_name", Value = supplierName },
                        new RequestParameter() { Name = "uids", Value = uids.ToString() }
            };
            ResponsePackage rsp = rqp1.GetResponse("http://127.0.0.1:11012");
        }
        public static void SetOrderAttr(RequestPackage rqp)
        {
            if (rqp != null && rqp.Parameters.Length > 0)
            {
                foreach (RequestParameter p in rqp.Parameters)
                {
                    if (p.Name == "SetOrderSupplier")
                    {
                        Hashtable v = p.Value as Hashtable;
                        Guid.TryParse(v["order_uid"] as String, out Guid orderUid);
                        Guid.TryParse(v["supplier_uid"] as String, out Guid supplierUid);
                        String supplierName = v["supplier_name"] as String;
                        RequestPackage rqp1 = new RequestPackage()
                        {
                            SessionId = rqp.SessionId,
                            Command = "Supply.dbo.заказы_у_поставщиков_шапка__установить_поставщика"
                        };
                        rqp1.Parameters = new RequestParameter[]
                        {
                        new RequestParameter() { Name = "session_id", Value = rqp.SessionId },
                        new RequestParameter() { Name = "order_uid", Value = orderUid },
                        new RequestParameter() { Name = "supplier_uid", Value = supplierUid },
                        new RequestParameter() { Name = "supplier_name", Value = supplierName }
                        };
                        ResponsePackage rsp = rqp1.GetResponse("http://127.0.0.1:11012");
                    }
                    if (p.Name == "SetOrderState")
                    {
                        Hashtable v = p.Value as Hashtable;
                        Guid.TryParse(v["order_uid"] as String, out Guid orderUid);
                        Guid.TryParse(v["state_uid"] as String, out Guid stateUid);
                        String stateName = v["state_name"] as String;
                        RequestPackage rqp1 = new RequestPackage()
                        {
                            SessionId = rqp.SessionId,
                            Command = "Supply.dbo.заказы_у_поставщиков_шапка__установить_статус"
                        };
                        rqp1.Parameters = new RequestParameter[]
                        {
                        new RequestParameter() { Name = "session_id", Value = rqp.SessionId },
                        new RequestParameter() { Name = "order_uid", Value = orderUid },
                        new RequestParameter() { Name = "state_uid", Value = stateUid },
                        new RequestParameter() { Name = "state_name", Value = stateName }
                        };
                        ResponsePackage rsp = rqp1.GetResponse("http://127.0.0.1:11012");
                    }
                }
            }
        }
    }
}
