using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easy_journal.Models
{
    public class EntryCache
    {
        [PrimaryKey]
        public string Date { get; set; }

        public string Content { get; set; }
    }
}
