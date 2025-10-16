using easy_journal.Models;
using SQLite;

namespace easy_journal.Services.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        private bool _isInitialized = false;
        private readonly SemaphoreSlim _initLock = new SemaphoreSlim(1, 1);

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task EnsureDatabaseInitialized()
        {
            if (_isInitialized)
                return;

            await _initLock.WaitAsync();
            try
            {
                if (!_isInitialized)
                {
                    await _database.CreateTableAsync<QuoteCache>();
                    await _database.CreateTableAsync<EntryCache>();

                    _isInitialized = true;
                }
            }
            finally
            {
                _initLock.Release();
            }
        }

        //Uses lazy loading to initalize, to prevent the deadlock in the constuctor
        public async Task<Models.Quote> GetQuoteForDate(string date)
        {
            await EnsureDatabaseInitialized();

            var cached = await _database.Table<QuoteCache>()
                  .Where(q => q.Date == date)
                  .FirstOrDefaultAsync();

            if (cached == null)
                return null;

            return new Models.Quote
            {
                Content = cached.QuoteContent,
                Author = cached.QuoteAuthor
            };
        }

        public async Task SaveQuoteForDate(string date, Models.Quote quote)
        {
            await EnsureDatabaseInitialized();

            var cache = new QuoteCache
            {
                Date = date,
                QuoteContent = quote.Content,
                QuoteAuthor = quote.Author,
                FetchedAt = DateTime.Now
            };

            await _database.InsertOrReplaceAsync(cache);
        }

        public async Task<Models.Entry> GetEntryForDate(string date)
        {
            await EnsureDatabaseInitialized();

            var cached = await _database.Table<EntryCache>()
                .Where(q => q.Date == date)
                .FirstOrDefaultAsync();

            if (cached == null)
                return null;

            return new Models.Entry
            {
                Content = cached.Content,
                EntryDate = DateTime.Parse(date),
            };
        }

        public async Task SaveEntryForDate(string date, Models.Entry entry)
        {
            await EnsureDatabaseInitialized();

            var cache = new EntryCache
            {
                Date = date,
                Content = entry.Content,
            };

            await _database.InsertOrReplaceAsync(cache);
        }

        public Task ClearAllData()
        {
            throw new NotImplementedException();
        }
    }
}
