using GestionLibros.Models;
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

    private async void OnNewCategoryClicked(object sender, EventArgs e)
    {
        viewModel.ClearFormCommand.Execute(null);
        await Navigation.PushModalAsync(new CategoryFormPage(viewModel));
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var category = (Category)((Button)sender).CommandParameter;
        viewModel.SelectedCategory = category;
        await Navigation.PushModalAsync(new CategoryFormPage(viewModel));
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var category = (Category)((Button)sender).CommandParameter;
        bool confirm = await DisplayAlert("Eliminar categoría", $"¿Seguro que deseas eliminar \"{category.Name}\"?", "Eliminar", "Cancelar");
        if (!confirm) return;

        await viewModel.DeleteCategoryCommand.ExecuteAsync(category);
    }
}
