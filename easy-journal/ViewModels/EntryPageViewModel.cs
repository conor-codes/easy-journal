using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using easy_journal.Models;
using easy_journal.Services.Database;
using easy_journal.Services.Quote;

namespace easy_journal.ViewModels
{
    public partial class EntryPageViewModel : ObservableObject
    {
        private readonly IQuoteService _quoteService;
        private readonly IDatabaseService _database;

        [ObservableProperty]
        private Quote dailyQuote;

        [ObservableProperty]
        private bool isLoadingQuote;

        [ObservableProperty]
        private DateTime currentDate;

        public EntryPageViewModel(IQuoteService quoteService, IDatabaseService database)
        {
            _quoteService = quoteService;
            _database = database;

            CurrentDate = DateTime.Now;
        }

        public async Task Initialize()
        {
            await LoadQuote(); 
        }

        private async Task LoadQuote()
        {
            IsLoadingQuote = true;

            try
            {
                DailyQuote = await _quoteService.GetQuoteOfTheDay();
            }
            catch (Exception ex)
            {
                // Handle error - maybe show a message
                DailyQuote = new Quote
                {
                    Content = "Unable to load quote",
                    Author = "Error"
                };
            }
            finally
            {
                IsLoadingQuote = false;
            }
        }

        [RelayCommand]
        private async Task RefreshQuote()
        {
            IsLoadingQuote = true;

            try
            {
                DailyQuote = await _quoteService.GetRandomQuote();
            }
            finally
            {
                IsLoadingQuote = false;
            }
        }
    }
}
