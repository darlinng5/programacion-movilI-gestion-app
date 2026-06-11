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

    }

    }
