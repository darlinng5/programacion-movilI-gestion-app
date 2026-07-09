using System.Diagnostics;
using GestionLibros.ViewModels;

namespace GestionLibros.Views;

public partial class StatsPage : ContentPage
{
    private readonly StatsViewModel viewModel;
    public StatsPage(StatsViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await viewModel.LoadStatsCommand.ExecuteAsync(null);
    }

    private async void OnGetLocationClicked(object sender, EventArgs e)
    {
        Location? location = await Geolocation.Default.GetLocationAsync();
        Debug.WriteLine($"GPS -> Latitud: {location?.Latitude}, Longitud: {location?.Longitude}");
    }
}
