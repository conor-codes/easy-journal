using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easy_journal.Models
{
    public partial class Entry : ObservableObject
    {
        [ObservableProperty]
        private string content = string.Empty;

        [ObservableProperty]
        private DateTime entryDate = DateTime.Today;

        public int CharacterCount => Content?.Length ?? 0;

        public int WordCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Content))
                    return 0;

                return Content.Split(new[] { ' ', '\n', '\r', '\t' },
                    StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }

        // Notify when Content changes that counts also changed
        partial void OnContentChanged(string value)
        {
            OnPropertyChanged(nameof(CharacterCount));
            OnPropertyChanged(nameof(WordCount));
        }
    }
}
