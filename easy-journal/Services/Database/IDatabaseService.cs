using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easy_journal.Services.Database
{
    public interface IDatabaseService
    {
        // Entry Cache Operations
        Task<Models.Entry> GetEntryForDate(string date);
        Task SaveEntryForDate(string date, Models.Entry entry);

        // Quote Cache Operations
        Task<Models.Quote> GetQuoteForDate(string date);
        Task SaveQuoteForDate(string date, Models.Quote quote);

        // Database Management
        Task EnsureDatabaseInitialized();
        Task ClearAllData();
    }
}
