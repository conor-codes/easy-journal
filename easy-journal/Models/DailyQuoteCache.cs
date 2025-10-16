using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easy_journal.Models
{
    public class DailyQuoteCache
    {
        [PrimaryKey]
        public string Date { get; set; } // "2025-01-15"

        public string QuoteContent { get; set; }

        public string QuoteAuthor { get; set; }

        public DateTime FetchedAt { get; set; }
    }
}
