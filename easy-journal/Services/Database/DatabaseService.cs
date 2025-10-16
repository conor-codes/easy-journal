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
                    await _database.CreateTableAsync<DailyQuoteCache>();
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

            var cached = await _database.Table<DailyQuoteCache>()
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

            var cache = new DailyQuoteCache
            {
                Date = date,
                QuoteContent = quote.Content,
                QuoteAuthor = quote.Author,
                FetchedAt = DateTime.Now
            };

            await _database.InsertOrReplaceAsync(cache);
        }

        public Task ClearAllData()
        {
            throw new NotImplementedException();
        }
    }
}
