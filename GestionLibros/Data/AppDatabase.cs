using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionLibros.Models;
using SQLite;

namespace GestionLibros.Data
{
    public class AppDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public AppDatabase()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "gestionlibros.db3");

            _database = new SQLiteAsyncConnection(dbPath);

            _database.CreateTableAsync<Book>().Wait();
            _database.CreateTableAsync<Category>().Wait();

            SeedDataAsync().Wait();
        }

        private async Task SeedDataAsync()
        {
            if (await _database.Table<Category>().CountAsync().ConfigureAwait(false) == 0)
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Ficción" },
                    new Category { Name = "Ciencia" },
                    new Category { Name = "Historia" },
                    new Category { Name = "Biografía" },
                };
                foreach (var category in categories)
                {
                    await _database.InsertAsync(category).ConfigureAwait(false);
                }
            }

            if (await _database.Table<Book>().CountAsync().ConfigureAwait(false) == 0)
            {
                var categoryIds = (await _database.Table<Category>().ToListAsync().ConfigureAwait(false))
                    .Select(c => c.Id)
                    .ToList();

                var books = new List<Book>
                {
                    new Book { Name = "Cien Años de Soledad", Author = "Gabriel García Márquez", Description = "La historia de la familia Buendía en Macondo.", CategoryId = categoryIds[0 % categoryIds.Count], Status = ReadingStatus.Leido, Rating = 5, Latitude = 40.4168, Longitude = -3.7038 },
                    new Book { Name = "Una Breve Historia del Tiempo", Author = "Stephen Hawking", Description = "Un recorrido por la cosmología moderna.", CategoryId = categoryIds[1 % categoryIds.Count], Status = ReadingStatus.Leyendo, Rating = 4, Latitude = 40.7532, Longitude = -73.9822 },
                    new Book { Name = "Sapiens", Author = "Yuval Noah Harari", Description = "Una breve historia de la humanidad.", CategoryId = categoryIds[2 % categoryIds.Count], Status = ReadingStatus.PorLeer, Rating = 0, Latitude = 51.5299, Longitude = -0.1268 },
                    new Book { Name = "El Diario de Ana Frank", Author = "Ana Frank", Description = "El testimonio de una joven durante el Holocausto.", CategoryId = categoryIds[3 % categoryIds.Count], Status = ReadingStatus.Leido, Rating = 5, Latitude = 19.4459, Longitude = -99.1509 },
                    new Book { Name = "1984", Author = "George Orwell", Description = "Una distopía sobre el totalitarismo.", CategoryId = categoryIds[0 % categoryIds.Count], Status = ReadingStatus.Leido, Rating = 5, Latitude = -37.8102, Longitude = 144.9628 },
                    new Book { Name = "Una Corta Historia de Casi Todo", Author = "Bill Bryson", Description = "Un vistazo a la ciencia y el universo.", CategoryId = categoryIds[1 % categoryIds.Count], Status = ReadingStatus.PorLeer, Rating = 0, Latitude = 39.9339, Longitude = 116.3378 },
                    new Book { Name = "Los Pilares de la Tierra", Author = "Ken Follett", Description = "La construcción de una catedral en la Inglaterra medieval.", CategoryId = categoryIds[2 % categoryIds.Count], Status = ReadingStatus.Leyendo, Rating = 4, Latitude = 31.2089, Longitude = 29.9092 },
                    new Book { Name = "Steve Jobs", Author = "Walter Isaacson", Description = "La biografía del cofundador de Apple.", CategoryId = categoryIds[3 % categoryIds.Count], Status = ReadingStatus.Leido, Rating = 4, Latitude = -34.5875, Longitude = -58.3974 },
                    new Book { Name = "El Nombre del Viento", Author = "Patrick Rothfuss", Description = "Las aventuras de un joven mago narradas por él mismo.", CategoryId = categoryIds[0 % categoryIds.Count], Status = ReadingStatus.PorLeer, Rating = 0, Latitude = 53.3438, Longitude = -6.2546 },
                    new Book { Name = "Cosmos", Author = "Carl Sagan", Description = "Un viaje por el universo y la ciencia.", CategoryId = categoryIds[1 % categoryIds.Count], Status = ReadingStatus.Leyendo, Rating = 5, Latitude = 35.6762, Longitude = 139.6503 },
                };

                foreach (var book in books)
                {
                    await _database.InsertAsync(book).ConfigureAwait(false);
                }
            }
        }

        public Task<int> SaveBookAsync(Book book)
        {
            return _database.InsertAsync(book);
        }

        public Task<List<Book>> GetBooksAsync()
        {
            return _database.Table<Book>().ToListAsync();
        }

        public Task<int> UpdateBookAsync(Book book)
        {
            return _database.UpdateAsync(book);
        }

        public Task<int> DeleteBookAsync(Book book)
        {
            return _database.DeleteAsync(book);
        }

        public Task<int> SaveCategoryAsync(Category category)
        {
            return _database.InsertAsync(category);
        }

        public Task<List<Category>> GetCategoriesAsync()
        {
            return _database.Table<Category>().ToListAsync();
        }

        public Task<int> UpdateCategoryAsync(Category category)
        {
            return _database.UpdateAsync(category);
        }

        public Task<int> DeleteCategoryAsync(Category category)
        {
            return _database.DeleteAsync(category);
        }

    }

    }
