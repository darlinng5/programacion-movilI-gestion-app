using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionLibros.Data;
using GestionLibros.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionLibros.ViewModels
{
    public partial class BookViewModel : ObservableObject
    {
        private readonly AppDatabase database;

        public BookViewModel(AppDatabase database)
        {
            this.database = database;
        }

        [ObservableProperty]
        private string name = string.Empty;
        [ObservableProperty]
        private string description = string.Empty;
        //Ocupamos una lista para guardar a los libros que vayamos creando
        [ObservableProperty]
        private ObservableCollection<Book> books = new ObservableCollection<Book>();
        
        [ObservableProperty]
        private Book selectedBook = new Book();
        [RelayCommand]
        private void Save()
        {
            Book book = new Book();
            book.Name = Name;
            book.Description = Description;
            database.SaveBookAsync(book);
            Books.Add(book);
            Console.WriteLine("Tamano de Lista: " + Books.Count);
        }
        [RelayCommand]
        private async Task Update() {

            if (SelectedBook == null)
            {
                Console.WriteLine("Debe seleccionar un libro");
                return;
            }

            SelectedBook.Name = Name;
            SelectedBook.Description = Description;

            await database.UpdateBookAsync(SelectedBook);

            await LoadBooks();

            Name = string.Empty;
            Description = string.Empty;
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (SelectedBook == null)
            {
                Console.WriteLine("Debe seleccionar un libro");
                return;
            }

            await database.DeleteBookAsync(SelectedBook);

            Books.Remove(SelectedBook);

            Name = string.Empty;
            Description = string.Empty;
            SelectedBook = new Book();
        }

        [RelayCommand]
        private async Task LoadBooks()
        {
            var list = await database.GetBooksAsync();

            Books.Clear();

            foreach (var book in list)
            {
                Books.Add(book);
            }
        }

        partial void OnSelectedBookChanged(Book? value) {
            if (value == null) return; 
            Name = value.Name; 
            Description = value.Description; 
        }
    }
}
