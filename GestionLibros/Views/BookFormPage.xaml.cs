using GestionLibros.ViewModels;

namespace GestionLibros.Views;

public partial class BookFormPage : ContentPage
{
    private readonly BookViewModel viewModel;

    public BookFormPage(BookViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
        Title = viewModel.IsEditing ? "Editar libro" : "Nuevo libro";
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

    private async void OnAddPhotoClicked(object sender, EventArgs e)
    {
        if (!MediaPicker.Default.IsCaptureSupported) return;

        FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();
        if (photo == null) return;

        Stream stream = await photo.OpenReadAsync();
        PhotoPreview.Source = ImageSource.FromStream(() => stream);
    }
}
