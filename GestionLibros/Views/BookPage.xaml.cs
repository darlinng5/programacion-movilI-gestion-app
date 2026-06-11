using GestionLibros.ViewModels;

namespace GestionLibros.Views;

public partial class BookPage : ContentPage
{
    private readonly BookViewModel viewModel;
    public BookPage(BookViewModel viewModel)	
	{
		InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await viewModel.LoadBooksCommand.ExecuteAsync(null);
    }
}