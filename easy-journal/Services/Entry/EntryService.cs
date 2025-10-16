using easy_journal.Extensions;
using easy_journal.Models;
using easy_journal.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easy_journal.Services.Entry
{
    public class EntryService : IEntryService
    {
        private readonly IDatabaseService _database;

        public EntryService(IDatabaseService database) 
        {
            _database = database;
        }

        public async Task<Models.Entry> GetEntryAsync()
        {
            var today = DateTime.Today.ToDatabaseDateString();

            // Check cache first
            var cached = await _database.GetEntryForDate(today);
            if (cached != null)
                return cached;

            return new Models.Entry
            {
                Content = string.Empty,
                EntryDate = DateTime.Now,
            };
        }

        public async Task SaveEntryAsync(Models.Entry entry)
        {
            await _database.SaveEntryForDate(entry.EntryDate.ToDatabaseDateString(), entry); 
        }
        public Task DeleteEntryAsync(Models.Entry entry)
        {
            throw new NotImplementedException();
        }

    }
}
