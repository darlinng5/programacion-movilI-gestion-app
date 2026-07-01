using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionLibros.Data;
using GestionLibros.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionLibros.ViewModels
{
    public partial class CategoryViewModel : ObservableObject
    {
        private readonly AppDatabase database;

        public CategoryViewModel(AppDatabase database)
        {
            this.database = database;
        }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();

        [ObservableProperty]
        private Category selectedCategory = new Category();

        [RelayCommand]
        private async Task Save()
        {
            Category category = new Category();
            category.Name = Name;
            await database.SaveCategoryAsync(category);
            Categories.Add(category);
            ClearForm();
        }

        [RelayCommand]
        private async Task Update()
        {
            if (SelectedCategory == null)
            {
                return;
            }

            SelectedCategory.Name = Name;

            await database.UpdateCategoryAsync(SelectedCategory);

            await LoadCategories();

            ClearForm();
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (SelectedCategory == null)
            {
                return;
            }

            await database.DeleteCategoryAsync(SelectedCategory);

            Categories.Remove(SelectedCategory);

            ClearForm();
        }

        [RelayCommand]
        private void ClearForm()
        {
            Name = string.Empty;
            SelectedCategory = new Category();
        }

        [RelayCommand]
        private async Task LoadCategories()
        {
            var list = await database.GetCategoriesAsync();

            Categories.Clear();

            foreach (var category in list)
            {
                Categories.Add(category);
            }
        }

        partial void OnSelectedCategoryChanged(Category? value)
        {
            if (value == null) return;
            Name = value.Name;
        }
    }
}
