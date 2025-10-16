namespace easy_journal.Services.Quote
{
    public interface IQuoteService
    {
        Task<Models.Quote> GetRandomQuote();
        Task<Models.Quote> GetQuoteOfTheDay();
    }
}
