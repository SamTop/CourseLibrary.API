using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public static class DateTimeOffsetExtension
    {
        public static int GetCurrentAge(this DateTimeOffset DateTimeOffset)
        {
            var CurrentDate = DateTime.UtcNow;
            int Age = CurrentDate.Year - DateTimeOffset.Year;

            if(CurrentDate < DateTimeOffset.AddYears(Age))
            {
                Age--;
            }

            return Age;
        }
    }
}
