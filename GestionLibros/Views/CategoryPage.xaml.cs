using GestionLibros.ViewModels;

namespace GestionLibros.Views;

public partial class CategoryPage : ContentPage
{
    private readonly CategoryViewModel viewModel;
    public CategoryPage(CategoryViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await viewModel.LoadCategoriesCommand.ExecuteAsync(null);
    }
}
