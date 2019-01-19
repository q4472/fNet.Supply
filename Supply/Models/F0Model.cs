using Nskd;
using System;
using System.Data;

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
                public String uid;
                public String id;
                public String обработано;
                public String товар;
                public String товар_описание;
                public String товар_менеджер;
                public String товар_спецификация_id;
                public String поставщик;
                public String поставщик_наименование;
                public String состояние_заказа;
                public String состояние_заказа_наименование;
                public String примечание;
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
                            uid = ConvertToString(dr["uid"]),
                            id = ConvertToString(dr["id"]),
                            обработано = ConvertToString(dr["обработано"]),
                            товар = ConvertToString(dr["товар"]),
                            товар_описание = ConvertToString(dr["товар_описание"]),
                            товар_менеджер = ConvertToString(dr["товар_менеджер"]),
                            товар_спецификация_id = ConvertToString(dr["товар_спецификация_id"]),
                            поставщик = ConvertToString(dr["поставщик"]),
                            поставщик_наименование = ConvertToString(dr["поставщик_наименование"]),
                            состояние_заказа = ConvertToString(dr["состояние_заказа"]),
                            состояние_заказа_наименование = ConvertToString(dr["состояние_заказа_наименование"]),
                            примечание = ConvertToString(dr["примечание"])
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
    }
}
