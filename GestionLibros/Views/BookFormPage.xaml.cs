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

        if (!string.IsNullOrEmpty(viewModel.PhotoPath))
            PhotoPreview.Source = ImageSource.FromFile(viewModel.PhotoPath);
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

        string destPath = Path.Combine(FileSystem.AppDataDirectory, $"{Guid.NewGuid()}.jpg");
        using (Stream sourceStream = await photo.OpenReadAsync())
        using (FileStream destStream = File.Create(destPath))
        {
            await sourceStream.CopyToAsync(destStream);
        }

        viewModel.PhotoPath = destPath;
        PhotoPreview.Source = ImageSource.FromFile(destPath);

        Location? location = await Geolocation.Default.GetLocationAsync();
        viewModel.Latitude = location?.Latitude;
        viewModel.Longitude = location?.Longitude;
    }
}
