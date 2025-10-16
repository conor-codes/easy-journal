using easy_journal.Extensions;
using easy_journal.Services.Database;
using easy_journal.Servicess.Http;
using easy_journal.Servicess.Quote.Models;

namespace easy_journal.Services.Quote
{
    public class QuoteService : IQuoteService
    {
        private readonly IHttpService _httpService;
        private readonly IDatabaseService _database;
        private const string BASE_URL = "https://api.quotable.io";

        private readonly static List<Models.Quote> _fallbacksQuotes = new()
        {
            new Models. Quote
            {
                Content = "The only way to do great work is to love what you do.",
                Author = "Steve Jobs"
            },
            new Models. Quote
            {
                Content = "Code is like humor. When you have to explain it, it's bad.",
                Author = "Cory House"
            },
            new Models.Quote
            {
                Content = "First, solve the problem. Then, write the code.",
                Author = "John Johnson"
            },
            new Models.Quote
            {
                Content = "Make it work, make it right, make it fast.",
                Author = "Kent Beck"
            },
            new Models.Quote
            {
                Content = "Simplicity is the soul of efficiency.",
                Author = "Austin Freeman"
            }
        };

        public QuoteService(IHttpService httpService, IDatabaseService database)
        {
            _httpService = httpService;
            _database = database;
        }

        public async Task<Models.Quote> GetQuoteOfTheDay()
        {
            var today = DateTime.Today.ToDatabaseDateString();

            // Check cache first
            var cached = await _database.GetQuoteForDate(today);
            if (cached != null)
                return cached;

            // Fetch new quote
            var quote = await GetRandomQuote();

            // Cache it
            await _database.SaveQuoteForDate(today, quote);

            return quote;
        }

        public async Task<Models.Quote> GetRandomQuote()
        {
            try
            {
                var response = await _httpService.GetAsync<QuotableResponse>($"{BASE_URL}/random");
                return MapToQuote(response);
            }
            catch (Exception ex)
            {
                // Log error / TODO: create ILogger
                System.Diagnostics.Debug.WriteLine($"Failed to fetch quote: {ex.Message}");
                return GetFallbackQuote();
            }
        }

        private Models.Quote MapToQuote(QuotableResponse response)
        {
            return new Models.Quote
            {
                Content = response.Content,
                Author = response.Author
            };
        }

        private Models.Quote GetFallbackQuote()
        {
            return _fallbacksQuotes[Random.Shared.Next(_fallbacksQuotes.Count)];
        }
    }
}
