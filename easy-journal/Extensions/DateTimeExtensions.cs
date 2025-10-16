using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easy_journal.Extensions
{
    public static class DateTimeExtensions
    {
        private const string LongDateFormat = "dddd, MMMM dd, yyyy";
        private const string ShortDateFormat = "MMM dd, yyyy";
        private const string DatabaseFormat = "yyyy-MM-dd";
        private const string WrittenAtFormat ="h:mm tt";

        /// <summary>
        /// Formats date as "Monday, January 15, 2025"
        /// </summary>
        public static string ToLongerDateString(this DateTime date)
        {
            return date.ToString(LongDateFormat);
        }

        /// <summary>
        /// Formats date as "Jan 15, 2025"
        /// </summary>
        public static string ToShortishDateString(this DateTime date)
        {
            return date.ToString(ShortDateFormat);
        }

        /// <summary>
        /// Formats as "2025-01-15" for database storage
        /// </summary>
        public static string ToDatabaseDateString(this DateTime date)
        {
            return date.ToString(DatabaseFormat);
        }

        /// <summary>
        /// Formats as "Written at 9:45 PM"
        /// </summary>
        public static string ToWrittenAtString(this DateTime date)
        {
            return $"Written at {date.ToString(WrittenAtFormat)}";
        }
    }
}
