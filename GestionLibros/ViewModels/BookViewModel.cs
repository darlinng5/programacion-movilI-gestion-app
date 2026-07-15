using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionLibros.Converters;
using GestionLibros.Data;
using GestionLibros.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionLibros.ViewModels
{
    public partial class BookViewModel : ObservableObject
    {
        private readonly AppDatabase database;
        private List<Book> allBooks = new List<Book>();

        public BookViewModel(AppDatabase database)
        {
            this.database = database;
        }

        [ObservableProperty]
        private string name = string.Empty;
        [ObservableProperty]
        private string author = string.Empty;
        [ObservableProperty]
        private string description = string.Empty;
        [ObservableProperty]
        private int rating;
        [ObservableProperty]
        private string searchText = string.Empty;
        [ObservableProperty]
        private string? photoPath;
        [ObservableProperty]
        private double? latitude;
        [ObservableProperty]
        private double? longitude;

        //Ocupamos una lista para guardar a los libros que vayamos creando
        [ObservableProperty]
        private ObservableCollection<Book> books = new ObservableCollection<Book>();

        [ObservableProperty]
        private Book selectedBook = new Book();

        [ObservableProperty]
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();

        [ObservableProperty]
        private Category? selectedCategory;

        [ObservableProperty]
        private ObservableCollection<ReadingStatus> statuses = new ObservableCollection<ReadingStatus>(
            Enum.GetValues<ReadingStatus>());

        [ObservableProperty]
        private ReadingStatus selectedStatus = ReadingStatus.PorLeer;

        [ObservableProperty]
        private bool isEditing;

        [ObservableProperty]
        private bool showMap;

        [ObservableProperty]
        private HtmlWebViewSource mapSource = new();

        [RelayCommand]
        private void ToggleMap()
        {
            ShowMap = !ShowMap;
        }

        private void BuildMapHtml()
        {
            var markers = new StringBuilder();
            foreach (var book in allBooks.Where(b => b.Latitude.HasValue && b.Longitude.HasValue))
            {
                var lat = book.Latitude!.Value.ToString(CultureInfo.InvariantCulture);
                var lng = book.Longitude!.Value.ToString(CultureInfo.InvariantCulture);
                var name = book.Name.Replace("\\", "\\\\").Replace("'", "\\'");
                markers.Append($"L.marker([{lat},{lng}]).addTo(map).bindPopup('{name}');");
            }

            var html = $@"<!DOCTYPE html><html><head>
<meta name='viewport' content='width=device-width, initial-scale=1.0'/>
<link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css'/>
<script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
<style>html,body,#map{{height:100%;margin:0}}</style>
</head><body><div id='map'></div><script>
var map = L.map('map').setView([20,0],2);
L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png').addTo(map);
{markers}
</script></body></html>";

            MapSource = new HtmlWebViewSource { Html = html };
        }

        [RelayCommand]
        private async Task Save()
        {
            if (SelectedBook.Id == 0)
            {
                Book book = new Book();
                book.Name = Name;
                book.Author = Author;
                book.Description = Description;
                book.CategoryId = SelectedCategory?.Id ?? 0;
                book.Status = SelectedStatus;
                book.Rating = Rating;
                book.PhotoPath = PhotoPath;
                book.Latitude = Latitude;
                book.Longitude = Longitude;
                await database.SaveBookAsync(book);
            }
            else
            {
                SelectedBook.Name = Name;
                SelectedBook.Author = Author;
                SelectedBook.Description = Description;
                SelectedBook.CategoryId = SelectedCategory?.Id ?? 0;
                SelectedBook.Status = SelectedStatus;
                SelectedBook.Rating = Rating;
                SelectedBook.PhotoPath = PhotoPath;
                SelectedBook.Latitude = Latitude;
                SelectedBook.Longitude = Longitude;
                await database.UpdateBookAsync(SelectedBook);
            }

            await LoadBooks();

            ClearForm();
        }

        [RelayCommand]
        private async Task DeleteBook(Book book)
        {
            await database.DeleteBookAsync(book);

            await LoadBooks();
        }

        [RelayCommand]
        private void ClearForm()
        {
            Name = string.Empty;
            Author = string.Empty;
            Description = string.Empty;
            SelectedCategory = null;
            SelectedStatus = ReadingStatus.PorLeer;
            Rating = 0;
            PhotoPath = null;
            Latitude = null;
            Longitude = null;
            SelectedBook = new Book();
        }

        [RelayCommand]
        private async Task LoadBooks()
        {
            allBooks = await database.GetBooksAsync();

            var categoryList = await database.GetCategoriesAsync();
            Categories.Clear();
            foreach (var category in categoryList)
            {
                Categories.Add(category);
            }
            CategoryIdToNameConverter.CategoryNames = categoryList.ToDictionary(c => c.Id, c => c.Name);

            ApplyFilter();
            BuildMapHtml();
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? allBooks
                : allBooks.Where(b =>
                    b.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                  .ToList();

            Books = new ObservableCollection<Book>(filtered);
        }

        partial void OnSelectedBookChanged(Book? value) {
            if (value == null) return;
            Name = value.Name;
            Author = value.Author;
            Description = value.Description;
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == value.CategoryId);
            SelectedStatus = value.Status;
            Rating = value.Rating;
            PhotoPath = value.PhotoPath;
            Latitude = value.Latitude;
            Longitude = value.Longitude;
            IsEditing = value.Id != 0;
        }
    }
}
