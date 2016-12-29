using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetablePlus_API.Utility
{
    public class DatetimeUtil
    {
        public static long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return epoch;
        }
    }
}
