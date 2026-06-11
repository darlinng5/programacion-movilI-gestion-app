using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            Books.Add(book);
            Console.WriteLine("Tamano de Lista: " + Books.Count);
        }
        [RelayCommand]
        private void Update() { 
            
            var index = Books.IndexOf(SelectedBook);
            if (index == -1){
                Console.WriteLine("Libro no encontrado");
                return;
            }
            Books[index] = new Book{ 
                                        Name = Name, 
                                        Description = Description 
                                   };
        }
        [RelayCommand]
        private void Delete() { 
            var index = Books.IndexOf(SelectedBook);
            if (index == -1){
                Console.WriteLine("Libro no encontrado");
                return;
            }
            Books.Remove(SelectedBook);
        }
        partial void OnSelectedBookChanged(Book? value) {
            if (value == null) return; 
            Name = value.Name; 
            Description = value.Description; 
        }
    }
}
