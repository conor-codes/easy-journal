using easy_journal.ViewModels;

namespace easy_journal.Views;

public partial class EntryPage : ContentPage
{
	private readonly EntryPageViewModel _viewModel;
	public EntryPage(EntryPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
	}

    protected override async void OnAppearing()
    {
		await _viewModel.Initialize();
        base.OnAppearing();
    }
}