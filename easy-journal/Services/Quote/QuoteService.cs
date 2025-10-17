using easy_journal.Extensions;
using easy_journal.Services.Database;
using easy_journal.Services.Http;
using easy_journal.Services.Quote.Models;

namespace easy_journal.Services.Quote
{
    public class QuoteService : IQuoteService
    {
        private readonly IHttpService _httpService;
        private readonly IDatabaseService _database;
        private const string BASE_URL = "https://api.quotable.io";

        private readonly static List<easy_journal.Models.Quote> _fallbacksQuotes = new()
        {
            new easy_journal.Models.Quote
            {
                Content = "The only way to do great work is to love what you do.",
                Author = "Steve Jobs"
            },
            new easy_journal.Models.Quote
            {
                Content = "Code is like humor. When you have to explain it, it's bad.",
                Author = "Cory House"
            },
            new easy_journal.Models.Quote
            {
                Content = "First, solve the problem. Then, write the code.",
                Author = "John Johnson"
            },
            new easy_journal.Models.Quote
            {
                Content = "Make it work, make it right, make it fast.",
                Author = "Kent Beck"
            },
            new easy_journal.Models.Quote
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

        public async Task<easy_journal.Models.Quote> GetQuoteOfTheDay()
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

        public async Task<easy_journal.Models.Quote> GetRandomQuote()
        {
            try
            {
                var response = await _httpService.GetAsync<QuotableResponse>($"{BASE_URL}/random");

                // Validate response
                if (response == null || string.IsNullOrEmpty(response.Content))
                {
                    System.Diagnostics.Debug.WriteLine("API returned invalid response");
                    return GetFallbackQuote();
                }

                return MapToQuote(response);

            }
            catch (Exception ex)
            {
                // Log error / TODO: create ILogger
                System.Diagnostics.Debug.WriteLine($"Failed to fetch quote: {ex.Message}");
                return GetFallbackQuote();
            }
        }

        private easy_journal.Models.Quote MapToQuote(QuotableResponse response)
        {
            return new easy_journal.Models.Quote
            {
                Content = response.Content,
                Author = response.Author
            };
        }

        private easy_journal.Models.Quote GetFallbackQuote()
        {
            return _fallbacksQuotes[Random.Shared.Next(_fallbacksQuotes.Count)];
        }
    }
}
