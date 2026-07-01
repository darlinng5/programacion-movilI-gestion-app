using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionLibros.Converters;
using GestionLibros.Data;
using GestionLibros.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        [RelayCommand]
        private async Task Save()
        {
            Book book = new Book();
            book.Name = Name;
            book.Author = Author;
            book.Description = Description;
            book.CategoryId = SelectedCategory?.Id ?? 0;
            book.Status = SelectedStatus;
            book.Rating = Rating;
            await database.SaveBookAsync(book);

            await LoadBooks();

            Name = string.Empty;
            Author = string.Empty;
            Description = string.Empty;
            SelectedCategory = null;
            SelectedStatus = ReadingStatus.PorLeer;
            Rating = 0;
        }
        [RelayCommand]
        private async Task Update() {

            if (SelectedBook == null)
            {
                return;
            }

            SelectedBook.Name = Name;
            SelectedBook.Author = Author;
            SelectedBook.Description = Description;
            SelectedBook.CategoryId = SelectedCategory?.Id ?? 0;
            SelectedBook.Status = SelectedStatus;
            SelectedBook.Rating = Rating;

            await database.UpdateBookAsync(SelectedBook);

            await LoadBooks();

            Name = string.Empty;
            Author = string.Empty;
            Description = string.Empty;
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (SelectedBook == null)
            {
                return;
            }

            await database.DeleteBookAsync(SelectedBook);

            await LoadBooks();

            Name = string.Empty;
            Author = string.Empty;
            Description = string.Empty;
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

            Books.Clear();
            foreach (var book in filtered)
            {
                Books.Add(book);
            }
        }

        partial void OnSelectedBookChanged(Book? value) {
            if (value == null) return;
            Name = value.Name;
            Author = value.Author;
            Description = value.Description;
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == value.CategoryId);
            SelectedStatus = value.Status;
            Rating = value.Rating;
        }
    }
}
