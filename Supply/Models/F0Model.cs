using System;

namespace FNet.Supply.Models
{
    public class F0Model
    {
        public Guid SessionId;
        public FilterData Filter;
        public FilteredData Data;

        public F0Model(Guid sessionId)
        {
            SessionId = sessionId;
            Filter = new FilterData(); // зависит от SessionId
            Data = new FilteredData();// зависит от Filter
        }
        public class FilterData
        {
            public String все;
            // ather fields ...
            public FilterData()
            {
                // todo: Filter - загрузить или создать по умолчанию.
                все = "False"; // фильтр по полю "обработано". По умолчанию обработанные строки не показываем.
                // ather field defaults ...
            }
        }
        public class FilteredData
        {
            // todo: Data - загрузить или создать по умолчанию.
            //public fields ...
            public FilteredData()
            {
                // field defaults ...
            }
        }
    }
}
