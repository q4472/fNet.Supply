﻿using Nskd;
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
