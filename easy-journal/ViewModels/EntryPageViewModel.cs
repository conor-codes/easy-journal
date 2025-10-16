using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using easy_journal.Models;
using easy_journal.Services.Database;
using easy_journal.Services.Entry;
using easy_journal.Services.Quote;

namespace easy_journal.ViewModels
{
    public partial class EntryPageViewModel : ObservableObject
    {
        private readonly IQuoteService _quoteService;
        private readonly IEntryService _entryService;
        private readonly IDatabaseService _database;

        [ObservableProperty]
        private Quote dailyQuote;

        [ObservableProperty]
        private Models.Entry dailyEntry;

        [ObservableProperty]
        private bool isLoadingQuote;

        [ObservableProperty]
        private bool isProcessingEntry;

        public EntryPageViewModel(IQuoteService quoteService, IDatabaseService database, IEntryService entryService)
        {
            _quoteService = quoteService;
            _entryService = entryService;
            _database = database;
        }

        public async Task Initialize()
        {
            await LoadQuote();
            await LoadEntry();
        }

        private async Task LoadEntry()
        {
            IsProcessingEntry = true;

            try
            {
                DailyEntry = await _entryService.GetEntryAsync();
            }
            catch (Exception ex)
            {
                // Handle error
                DailyEntry = new Models.Entry
                {
                    Content = string.Empty,
                    EntryDate = DateTime.Now,
                };
            }
            finally
            {
                IsProcessingEntry = false;
            }
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
                // Handle error
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
        private async Task SaveEntry() 
        { 
            IsProcessingEntry = true;

            if (string.IsNullOrEmpty(DailyEntry.Content))
                return;

            try
            {
                await _entryService.SaveEntryAsync(DailyEntry);
            }
            finally
            {
                IsProcessingEntry = false;
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
