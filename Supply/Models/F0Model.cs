using Nskd;
using System;
using System.Collections;
using System.Data;
using static FNet.Supply.Models.Lib;

namespace FNet.Supply.Models
{
    public class F0Model
    {
        private RequestPackage rqp;
        public FilterData Filter;
        public FilteredData Data;
        public DataTable Поставщики;
        public DataTable СостоянияЗаказа;

        public F0Model(RequestPackage rqp)
        {
            this.rqp = rqp;
        }

        public class FilterData
        {
            public String все = "False"; // фильтр по полю "обработано". По умолчанию обработанные строки не показываем.
            public String дата_min = "";
            public String дата_max = "";
            public String менеджер = "";
            public String спецификация_номер = "";
            public String аукцион_номер = "";

            public FilterData(F0Model m)
            {
                if (m.rqp != null)
                {
                    все = (m.rqp["все"] == null) ? "False" : ((Boolean)m.rqp["все"]).ToString();
                    дата_min = (m.rqp["дата_min"] == null) ? "" : (String)m.rqp["дата_min"];
                    дата_max = (m.rqp["дата_max"] == null) ? "" : (String)m.rqp["дата_max"];
                    менеджер = (m.rqp["менеджер"] == null) ? "" : (String)m.rqp["менеджер"];
                    спецификация_номер = (m.rqp["спецификация_номер"] == null) ? "" : (String)m.rqp["спецификация_номер"];
                    аукцион_номер = (m.rqp["аукцион_номер"] == null) ? "" : (String)m.rqp["аукцион_номер"];
                }
            }
        }
        public class FilteredData
        {
            private DataSet ds;
            public ТаблицаДанных Шапка;
            public ТаблицаДанных Таблица;
            public FilteredData(F0Model m)
            {
                if (m.rqp != null && m.rqp.SessionId != null)
                {
                    RequestPackage rqp1 = new RequestPackage()
                    {
                        SessionId = m.rqp.SessionId,
                        Command = "Supply.dbo.заказы_у_поставщиков__получить",
                        Parameters = new RequestParameter[]
                        {
                            new RequestParameter() { Name = "session_id", Value = m.rqp.SessionId },
                            new RequestParameter() { Name = "все", Value = m.Filter.все }
                        }
                    };
                    if (!String.IsNullOrWhiteSpace(m.Filter.дата_min)) rqp1["дата_min"] = m.Filter.дата_min;
                    if (!String.IsNullOrWhiteSpace(m.Filter.дата_max)) rqp1["дата_max"] = m.Filter.дата_max;
                    if (!String.IsNullOrWhiteSpace(m.Filter.менеджер)) rqp1["менеджер"] = m.Filter.менеджер;
                    if (!String.IsNullOrWhiteSpace(m.Filter.спецификация_номер)) rqp1["спецификация_номер"] = m.Filter.спецификация_номер;
                    if (!String.IsNullOrWhiteSpace(m.Filter.аукцион_номер)) rqp1["аукцион_номер"] = m.Filter.аукцион_номер;
                    ResponsePackage rsp1 = rqp1.GetResponse("http://127.0.0.1:11012");
                    if (rsp1 != null)
                    {
                        ds = rsp1.Data;
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                Шапка = new ТаблицаДанных(ds.Tables[0]);
                                Шапка.Sort = "[id] desc";
                                if (ds.Tables.Count > 1)
                                {
                                    Таблица = new ТаблицаДанных(ds.Tables[1]);
                                    Таблица.RowFilter = "[parent_uid] is null";
                                }
                            }
                        }
                    }
                }
            }
        }
        public class ЗаказыУПоставщиковТаблица
        {
            private DataTable заказыУПоставщиковТаблица;
            private DataTable заказыУПоставщиковТаблицаЦены;
            public Int32 RowsCount { get => (заказыУПоставщиковТаблица == null) ? 0 : заказыУПоставщиковТаблица.Rows.Count; }
            public class ItemArray
            {
                public String uid;
                public String id;
                public String parent_uid;
                //public String обработано;
                public String товар;
                public String примечание;

                //public String uid;
                //public String id;
                //public String parent_uid;
                public String цена1;
                public String цена2;
                public String цена3;
                public String цена4;
                public String количество1;
                public String количество2;
                public String количество3;
                public String количество4;
                public String срок_годности1;
                public String срок_годности2;
                public String срок_годности3;
                public String срок_годности4;


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
                    ItemArray items = new ItemArray();
                    if (заказыУПоставщиковТаблица != null && index >= 0 && index < заказыУПоставщиковТаблица.Rows.Count)
                    {
                        DataRow dr = заказыУПоставщиковТаблица.Rows[index];
                        items.uid = ConvertToString(dr["uid"]);
                        items.id = ConvertToString(dr["id"]);
                        items.parent_uid = ConvertToString(dr["parent_uid"]);
                        //items.обработано = ConvertToString(dr["обработано"]);
                        items.товар = ConvertToString(dr["товар"]);
                        items.примечание = ConvertToString(dr["примечание"]);
                    }
                    if (заказыУПоставщиковТаблицаЦены != null && index >= 0 && index < заказыУПоставщиковТаблицаЦены.Rows.Count)
                    {
                        DataRow dr = заказыУПоставщиковТаблицаЦены.Rows[index];
                        //items.uid = ConvertToString(dr["uid"]),
                        //items.id = ConvertToString(dr["id"]),
                        //items.parent_uid = ConvertToString(dr["parent_uid"]),
                        items.цена1 = ConvertToString(dr["цена1"]);
                        items.цена2 = ConvertToString(dr["цена2"]);
                        items.цена3 = ConvertToString(dr["цена3"]);
                        items.цена4 = ConvertToString(dr["цена4"]);
                        items.количество1 = ConvertToString(dr["количество1"]);
                        items.количество2 = ConvertToString(dr["количество2"]);
                        items.количество3 = ConvertToString(dr["количество3"]);
                        items.количество4 = ConvertToString(dr["количество4"]);
                        items.срок_годности1 = ConvertToString(dr["срок_годности1"]);
                        items.срок_годности2 = ConvertToString(dr["срок_годности2"]);
                        items.срок_годности3 = ConvertToString(dr["срок_годности3"]);
                        items.срок_годности4 = ConvertToString(dr["срок_годности4"]);
                        if (items.цена1.Length > 4) { items.цена1 = items.цена1.Substring(0, items.цена1.Length - 1); }
                        if (items.цена2.Length > 4) { items.цена2 = items.цена2.Substring(0, items.цена2.Length - 1); }
                        if (items.цена3.Length > 4) { items.цена3 = items.цена3.Substring(0, items.цена3.Length - 1); }
                        if (items.цена4.Length > 4) { items.цена4 = items.цена4.Substring(0, items.цена4.Length - 1); }
                        if (items.количество1.Length > 4) { items.количество1 = items.количество1.Substring(0, items.количество1.Length - 4); }
                        if (items.количество2.Length > 4) { items.количество2 = items.количество2.Substring(0, items.количество2.Length - 4); }
                        if (items.количество3.Length > 4) { items.количество3 = items.количество3.Substring(0, items.количество3.Length - 4); }
                        if (items.количество4.Length > 4) { items.количество4 = items.количество4.Substring(0, items.количество4.Length - 4); }
                        if (items.срок_годности1.Length == 8) { items.срок_годности1 = items.срок_годности1.Substring(3, 5); }
                        if (items.срок_годности2.Length == 8) { items.срок_годности2 = items.срок_годности2.Substring(3, 5); }
                        if (items.срок_годности3.Length == 8) { items.срок_годности3 = items.срок_годности3.Substring(3, 5); }
                        if (items.срок_годности4.Length == 8) { items.срок_годности4 = items.срок_годности4.Substring(3, 5); }
                    }
                    return items;
                }
            }
        }

        private ТаблицаДанных ПолучитьЗаказыУПоставщиковШапка(Guid sessionId, Guid order_uid)
        {
            ТаблицаДанных table = null;
            RequestPackage rqp1 = new RequestPackage();
            rqp1.SessionId = sessionId;
            rqp1.Command = "Supply.dbo.заказы_у_поставщиков_шапка__получить";
            rqp1.Parameters = new RequestParameter[]
            {
                        new RequestParameter() { Name = "session_id", Value = sessionId },
                        new RequestParameter() { Name = "uid", Value = order_uid }
            };
            ResponsePackage rsp1 = rqp1.GetResponse("http://127.0.0.1:11012");
            if (rsp1 != null)
            {
                table = new ТаблицаДанных(rsp1.GetFirstTable());
            }
            return table;
        }
        private DataTable ПолучитьСписокПоставщиков(F0Model m)
        {
            DataTable dt = null;
            RequestPackage rqp = new RequestPackage
            {
                SessionId = m.rqp.SessionId,
                Command = "Supply.dbo.поставщики__получить",
                Parameters = new RequestParameter[]
                {
                    new RequestParameter() { Name = "session_id", Value = m.rqp.SessionId }
                }
            };
            ResponsePackage rsp = rqp.GetResponse("http://127.0.0.1:11012");
            if (rsp != null)
            {
                dt = rsp.GetFirstTable();
            }
            return dt;
        }
        private DataTable ПолучитьСписокСостоянийЗаказа(F0Model m)
        {
            DataTable dt = null;
            RequestPackage rqp = new RequestPackage
            {
                SessionId = m.rqp.SessionId,
                Command = "Supply.dbo.состояния_заказа__получить",
                Parameters = new RequestParameter[]
                {
                    new RequestParameter() { Name = "session_id", Value = m.rqp.SessionId }
                }
            };
            ResponsePackage rsp = rqp.GetResponse("http://127.0.0.1:11012");
            if (rsp != null)
            {
                dt = rsp.GetFirstTable();
            }
            return dt;
        }
        public СтрокаДанных GetHeadDetail()
        {
            СтрокаДанных row = null;
            if (rqp != null && rqp.SessionId != null)
            {
                Guid.TryParse(rqp["uid"] as String, out Guid uid);
                RequestPackage rqp1 = new RequestPackage() {
                    SessionId = rqp.SessionId,
                    Command = "Supply.dbo.заказы_у_поставщиков_шапка__получить",
                    Parameters = new RequestParameter[]
                    {
                        new RequestParameter() { Name = "session_id", Value = rqp.SessionId },
                        new RequestParameter() { Name = "uid", Value = uid }
                    }
                };
                ResponsePackage rsp1 = rqp1.GetResponse("http://127.0.0.1:11012");
                if (rsp1 != null)
                {
                    ТаблицаДанных table = new ТаблицаДанных(rsp1.GetFirstTable());
                    if (table != null && table.RowsCount > 0)
                    {
                        row = table[0];
                    }
                }
            }
            return row;
        }
        public СтрокаДанных GetTableDetail()
        {
            СтрокаДанных row = null;
            if (rqp != null && rqp.SessionId != null)
            {
                Guid.TryParse(rqp["uid"] as String, out Guid uid);
                RequestPackage rqp1 = new RequestPackage()
                {
                    SessionId = rqp.SessionId,
                    Command = "Supply.dbo.заказы_у_поставщиков_таблица__получить",
                    Parameters = new RequestParameter[]
                    {
                        new RequestParameter() { Name = "session_id", Value = rqp.SessionId },
                        new RequestParameter() { Name = "uid", Value = uid }
                    }
                };
                ResponsePackage rsp1 = rqp1.GetResponse("http://127.0.0.1:11012");
                if (rsp1 != null)
                {
                    ТаблицаДанных table = new ТаблицаДанных(rsp1.GetFirstTable());
                    if (table != null && table.RowsCount > 0)
                    {
                        row = table[0];
                    }
                }
            }
            return row;
        }
        public void SetSupplier()
        {
            Hashtable setSupplierValue = (Hashtable)rqp["SetSupplier"];
            Guid.TryParse(setSupplierValue["supplier_uid"] as String, out Guid supplierUid);
            String supplierName = setSupplierValue["supplier_name"] as String;
            String uids = setSupplierValue["uids"] as String;
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
                        new RequestParameter() { Name = "uids", Value = uids }
            };
            ResponsePackage rsp = rqp1.GetResponse("http://127.0.0.1:11012");
        }
        public void OrderHeadUpdate()
        {
            if (rqp != null && rqp.Parameters != null && rqp.Parameters.Length > 0)
            {
                Guid sessionId = rqp.SessionId;
                foreach (RequestParameter p in rqp.Parameters)
                {
                    if (p.Value is Hashtable v && v.ContainsKey("order_uid") && Guid.TryParse(v["order_uid"] as String, out Guid orderUid))
                    {
                        RequestPackage rqp1 = new RequestPackage()
                        {
                            SessionId = sessionId,
                            Command = "Supply.dbo.заказы_у_поставщиков_шапка__обновить_поле"
                        };
                        switch (p.Name)
                        {
                            case "SetOrderSupplier":
                                Guid.TryParse(v["supplier_uid"] as String, out Guid supplierUid);
                                String supplierName = v["supplier_name"] as String;
                                rqp1.Parameters = new RequestParameter[]
                                {
                                    new RequestParameter() { Name = "session_id", Value = sessionId },
                                    new RequestParameter() { Name = "order_uid", Value = orderUid },
                                    new RequestParameter() { Name = "field", Value = "поставщик" },
                                    new RequestParameter() { Name = "supplier_uid", Value = supplierUid },
                                    new RequestParameter() { Name = "supplier_name", Value = supplierName }
                                };
                                break;
                            case "SetOrderState":
                                Guid.TryParse(v["state_uid"] as String, out Guid stateUid);
                                String stateName = v["state_name"] as String;
                                rqp1.Parameters = new RequestParameter[]
                                {
                                    new RequestParameter() { Name = "session_id", Value = sessionId },
                                    new RequestParameter() { Name = "order_uid", Value = orderUid },
                                    new RequestParameter() { Name = "field", Value = "состояние" },
                                    new RequestParameter() { Name = "state_uid", Value = stateUid },
                                    new RequestParameter() { Name = "state_name", Value = stateName }
                                };
                                break;
                            case "SetOrderComment":
                                String примечание = v["примечание"] as String;
                                rqp1.Parameters = new RequestParameter[]
                                {
                                    new RequestParameter() { Name = "session_id", Value = sessionId },
                                    new RequestParameter() { Name = "order_uid", Value = orderUid },
                                    new RequestParameter() { Name = "field", Value = "примечание" },
                                    new RequestParameter() { Name = "примечание", Value = примечание }
                                };
                                break;
                            case "SetOrderNum":
                                String номер = v["номер"] as String;
                                rqp1.Parameters = new RequestParameter[]
                                {
                                    new RequestParameter() { Name = "session_id", Value = sessionId },
                                    new RequestParameter() { Name = "order_uid", Value = orderUid },
                                    new RequestParameter() { Name = "field", Value = "номер" },
                                    new RequestParameter() { Name = "номер", Value = номер }
                                };
                                break;
                            case "SetOrderSDate":
                                String дата_поставки = v["дата_поставки"] as String;
                                if (дата_поставки != null && дата_поставки.Length == 8)
                                {
                                    дата_поставки = $"20{дата_поставки.Substring(6, 2)}-{дата_поставки.Substring(3, 2)}-{дата_поставки.Substring(0, 2)}";
                                    rqp1.Parameters = new RequestParameter[]
                                    {
                                        new RequestParameter() { Name = "session_id", Value = sessionId },
                                        new RequestParameter() { Name = "order_uid", Value = orderUid },
                                        new RequestParameter() { Name = "field", Value = "дата_поставки" },
                                        new RequestParameter() { Name = "дата_поставки", Value = дата_поставки }
                                    };
                                }
                                break;
                            case "SetX":
                                rqp1.Parameters = new RequestParameter[]
                                {
                                        new RequestParameter() { Name = "session_id", Value = sessionId },
                                        new RequestParameter() { Name = "order_uid", Value = orderUid },
                                        new RequestParameter() { Name = "field", Value = "обработано" },
                                        new RequestParameter() { Name = "обработано", Value = v["x"] }
                                };
                                break;
                            default:
                                break;
                        }
                        if (rqp1.Parameters != null && rqp1.Parameters.Length > 0)
                        {
                            ResponsePackage rsp = rqp1.GetResponse("http://127.0.0.1:11012");
                        }
                    }
                }
            }
        }
        public void OrderTableUpdate()
        {
            if (rqp != null && rqp.Parameters != null && rqp.Parameters.Length > 0)
            {
                rqp.Command = "Supply.dbo.заказы_у_поставщиков_таблица__обновить";
                ResponsePackage rsp = rqp.GetResponse("http://127.0.0.1:11012");
            }
        }
        public void ApplyFilter()
        {
            Filter = new FilterData(this);
            Data = new FilteredData(this);
            Поставщики = ПолучитьСписокПоставщиков(this);
            СостоянияЗаказа = ПолучитьСписокСостоянийЗаказа(this);
        }
        public void SetAsFree()
        {
            RequestPackage rqp1 = new RequestPackage()
            {
                SessionId = rqp.SessionId,
                Command = "[Supply].[dbo].[заказы_у_поставщиков_таблица__перенести_в_свободные]",
                Parameters = new RequestParameter[]
                {
                    new RequestParameter(){ Name = "session_id", Value = rqp.SessionId },
                    new RequestParameter(){ Name = "uids", Value = rqp["uids"] },
                }
            };
            ResponsePackage rsp = rqp1.GetResponse("http://127.0.0.1:11012");
        }
        public void SplitRow()
        {
            RequestPackage rqp1 = new RequestPackage()
            {
                SessionId = rqp.SessionId,
                Command = "[Supply].[dbo].[заказы_у_поставщиков_таблица__разделить_строку]",
                Parameters = new RequestParameter[]
                {
                    new RequestParameter(){ Name = "session_id", Value = rqp.SessionId },
                    new RequestParameter(){ Name = "uid", Value = rqp["uid"] },
                    new RequestParameter(){ Name = "qty", Value = rqp["qty"] },
                }
            };
            ResponsePackage rsp = rqp1.GetResponse("http://127.0.0.1:11012");
        }
    }
}
