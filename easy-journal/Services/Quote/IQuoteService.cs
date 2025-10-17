namespace easy_journal.Services.Quote
{
    public interface IQuoteService
    {
        Task<easy_journal.Models.Quote> GetRandomQuote();
        Task<easy_journal.Models.Quote> GetQuoteOfTheDay();
    }
}
