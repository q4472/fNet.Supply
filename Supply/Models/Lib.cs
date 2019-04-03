using System;
using System.Data;

namespace FNet.Supply.Models
{
    public class Lib
    {
        public class ТаблицаДанных
        {
            private DataTable dt;
            private DataView dv;
            public ТаблицаДанных(DataTable dt)
            {
                if (dt != null)
                {
                    this.dt = dt;
                    dv = dt.DefaultView;
                }
            }
            public Int32 RowsCount
            {
                get => (dv == null) ? 0 : dv.Count;
            }
            public СтрокаДанных this[Int32 index]
            {
                get
                {
                    СтрокаДанных row = null;
                    if (dv != null && index >= 0 && index < dv.Count)
                    {
                        row = new СтрокаДанных(dv[index]);
                    }
                    return row;
                }
            }
            public String Sort
            {
                set
                {
                    dv.Sort = value;
                }
            }
            public String RowFilter
            {
                set
                {
                    dv.RowFilter = value;
                }
            }
        }
        public class СтрокаДанных
        {
            private DataRowView drv;
            public СтрокаДанных(DataRowView drv) { this.drv = drv; }
            public String this[String fieldName]
            {
                get
                {
                    String v = String.Empty;
                    if (!String.IsNullOrWhiteSpace(fieldName) && drv != null && drv.DataView.Table.Columns.Contains(fieldName))
                    {
                        v = ConvertToString(drv[fieldName]);
                    }
                    return v;
                }
            }

        }
        public static String ConvertToString(Object v)
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
                    case "System.Decimal":
                        s = ((Decimal)v).ToString("n3");
                        break;
                    case "System.DateTime":
                        s = ((DateTime)v).ToString("dd.MM.yy");
                        break;
                    default:
                        s = "FNet.Supply.Models.Lib.ConvertToString() result: " + tfn;
                        break;
                }
            }
            return s;
        }
    }
}