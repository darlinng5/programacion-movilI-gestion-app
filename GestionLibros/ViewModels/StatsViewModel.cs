using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionLibros.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionLibros.ViewModels
{
    public class StatusCountItem
    {
        public string StatusName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class CategoryCountItem
    {
        public string CategoryName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public partial class StatsViewModel : ObservableObject
    {
        private readonly AppDatabase database;

        public StatsViewModel(AppDatabase database)
        {
            this.database = database;
        }

        [ObservableProperty]
        private int totalBooks;

        [ObservableProperty]
        private string averageRating = "Sin calificaciones";

        [ObservableProperty]
        private ObservableCollection<StatusCountItem> statusCounts = new ObservableCollection<StatusCountItem>();

        [ObservableProperty]
        private ObservableCollection<CategoryCountItem> categoryCounts = new ObservableCollection<CategoryCountItem>();

        [RelayCommand]
        private async Task LoadStats()
        {
            var books = await database.GetBooksAsync();
            var categories = await database.GetCategoriesAsync();

            TotalBooks = books.Count;

            var rated = books.Where(b => b.Rating > 0).ToList();
            AverageRating = rated.Count > 0
                ? $"{rated.Average(b => b.Rating):0.0} / 5"
                : "Sin calificaciones";

            StatusCounts.Clear();
            foreach (var group in books.GroupBy(b => b.Status))
            {
                StatusCounts.Add(new StatusCountItem { StatusName = group.Key.ToString(), Count = group.Count() });
            }

            CategoryCounts.Clear();
            foreach (var category in categories)
            {
                var count = books.Count(b => b.CategoryId == category.Id);
                CategoryCounts.Add(new CategoryCountItem { CategoryName = category.Name, Count = count });
            }

            var uncategorized = books.Count(b => !categories.Any(c => c.Id == b.CategoryId));
            if (uncategorized > 0)
            {
                CategoryCounts.Add(new CategoryCountItem { CategoryName = "Sin categoría", Count = uncategorized });
            }
        }
    }
}
