using GestionLibros.ViewModels;

namespace GestionLibros.Views;

public partial class CategoryFormPage : ContentPage
{
    private readonly CategoryViewModel viewModel;

    public CategoryFormPage(CategoryViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
        Title = viewModel.IsEditing ? "Editar categoría" : "Nueva categoría";
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await viewModel.SaveCommand.ExecuteAsync(null);
        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        viewModel.ClearFormCommand.Execute(null);
        await Navigation.PopModalAsync();
    }
}
