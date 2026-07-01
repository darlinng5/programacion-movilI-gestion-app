using GestionLibros.Models;
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

    private async void OnNewBookClicked(object sender, EventArgs e)
    {
        viewModel.ClearFormCommand.Execute(null);
        await Navigation.PushModalAsync(new BookFormPage(viewModel));
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var book = (Book)((Button)sender).CommandParameter;
        viewModel.SelectedBook = book;
        await Navigation.PushModalAsync(new BookFormPage(viewModel));
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var book = (Book)((Button)sender).CommandParameter;
        bool confirm = await DisplayAlert("Eliminar libro", $"¿Seguro que deseas eliminar \"{book.Name}\"?", "Eliminar", "Cancelar");
        if (!confirm) return;

        await viewModel.DeleteBookCommand.ExecuteAsync(book);
    }
}
