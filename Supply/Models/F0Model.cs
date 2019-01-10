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

        public F0Model(RequestPackage rqp)
        {
            Rqp = rqp;
            Filter = new FilterData(this);
            Data = new FilteredData(this);
        }

        public class FilterData
        {
            private F0Model m;

            public String все;
            public String дата_min;
            public String дата_max;
            public String менеджер;

            public FilterData(F0Model model)
            {
                // todo: Filter - загрузить или создать по умолчанию.
                m = model;

                все = "False"; // фильтр по полю "обработано". По умолчанию обработанные строки не показываем.
                дата_min = "";
                дата_max = "";
                менеджер = "";
            }
        }
        public class FilteredData
        {
            // todo: Data - загрузить или создать по умолчанию.
            private F0Model m;
            private DataTable dt;

            public Int32 RowsCount { get => (dt == null) ? 0 : dt.Rows.Count; }
            public class ItemArray
            {
                public String uid;
                public String id;
                public String обработано;
                public String менеджер;
            }

            public FilteredData(F0Model model)
            {
                m = model;

                if (m.Rqp != null && m.Rqp.SessionId != null)
                {
                    m.Rqp.Command = "Supply.dbo.заказы_у_поставщиков__получить";
                    m.Rqp.AddSessionIdToParameters();
                    ResponsePackage rsp = m.Rqp.GetResponse("http://127.0.0.1:11012");
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
                            uid = ((Guid)dr["uid"]).ToString(),
                            id = ((Int32)dr["id"]).ToString(),
                            обработано = ((Boolean)dr["обработано"]).ToString(),
                            менеджер = (dr["менеджер"] == DBNull.Value) ? "" : (String)dr["менеджер"]
                        };
                    }
                    return items;
                }
            }
        }
    }
}
