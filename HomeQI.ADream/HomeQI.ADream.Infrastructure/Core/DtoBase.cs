using System;

namespace HomeQI.ADream.Infrastructure.Core
{
    public class DtoBase
    {
        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        public DateTime RequestTime { get; set; }

        public DateTime ResponseTime { get; set; }
    }

}
