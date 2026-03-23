using PPNET_2.Data;
using PPNET_2.Models;
using PPNET_2.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;

namespace PPNET_2.ViewModels
{
    public class MainViewModel : BindableObject
    {
        private readonly MealService _mealService;
        private readonly AppDbContext _dbContext;

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }

        private string _addMealName;
        public string AddMealName
        {
            get => _addMealName;
            set { _addMealName = value; OnPropertyChanged(); }
        }

        private string _addCategoryName;
        public string AddCategoryName
        {
            get => _addCategoryName;
            set { _addCategoryName = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Meal> Meals { get; set; } = new ObservableCollection<Meal>();

        public ICommand SearchCommand { get; }
        public ICommand LoadFromDbCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand AddManualCommand { get; }

        public MainViewModel()
        {
            _mealService = new MealService();
            _dbContext = new AppDbContext();

            SearchCommand = new Command(async () => await ExecuteSearch());
            LoadFromDbCommand = new Command(async () => await LoadFromDb());
            DeleteCommand = new Command<Meal>(async (meal) => await ExecuteDelete(meal));
            SortCommand = new Command(ExecuteSort);
            AddManualCommand = new Command(async () => await ExecuteAddManual());

            LoadFromDb().ConfigureAwait(false);
        }

        private async Task LoadFromDb()
        {
            var items = await _dbContext.Meals.Include(m => m.Category).ToListAsync();
            
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                Meals.Clear();
                foreach (var item in items)
                {
                    Meals.Add(item);
                }
            });
        }

        private async Task ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                return;

            string searchLower = SearchQuery.ToLower();

            var existingDbMeals = await _dbContext.Meals
                .Include(m => m.Category)
                .Where(m => m.Name.ToLower().Contains(searchLower))
                .ToListAsync();

            if (existingDbMeals.Any())
            {
                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
                {
                    Meals.Clear();
                    foreach (var meal in existingDbMeals)
                        Meals.Add(meal);
                });
                return;
            }

            var apiMeals = await _mealService.GetMealsAsync(SearchQuery);
            if (apiMeals == null || !apiMeals.Any())
                return;

            foreach (var dto in apiMeals)
            {
                if (await _dbContext.Meals.AnyAsync(m => m.ApiId == dto.idMeal))
                    continue;

                var catName = string.IsNullOrWhiteSpace(dto.strCategory) ? "Inne" : dto.strCategory;
                var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == catName);
                if (category == null)
                {
                    category = new Category { Name = catName };
                    _dbContext.Categories.Add(category);
                    await _dbContext.SaveChangesAsync();
                }

                var newMeal = new Meal
                {
                    ApiId = dto.idMeal,
                    Name = dto.strMeal,
                    Area = dto.strArea,
                    CategoryId = category.Id
                };

                _dbContext.Meals.Add(newMeal);
            }

            await _dbContext.SaveChangesAsync();

            await LoadFromDb();
        }

        private async Task ExecuteDelete(Meal meal)
        {
            if (meal == null) return;
            
            _dbContext.Meals.Remove(meal);
            await _dbContext.SaveChangesAsync();

            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                Meals.Remove(meal);
            });
        }

        private void ExecuteSort()
        {
            var sorted = Meals.OrderBy(m => m.Name).ToList();
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                Meals.Clear();
                foreach (var item in sorted)
                {
                    Meals.Add(item);
                }
            });
        }

        private async Task ExecuteAddManual()
        {
            if (string.IsNullOrWhiteSpace(AddMealName))
                return;

            string catName = string.IsNullOrWhiteSpace(AddCategoryName) ? "Własne" : AddCategoryName;
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == catName);
            if (category == null)
            {
                category = new Category { Name = catName };
                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();
            }

            var newMeal = new Meal
            {
                ApiId = "MANUAL_" + System.Guid.NewGuid().ToString().Substring(0, 8),
                Name = AddMealName,
                Area = "Nieznany",
                CategoryId = category.Id
            };

            _dbContext.Meals.Add(newMeal);
            await _dbContext.SaveChangesAsync();

            AddMealName = string.Empty;
            AddCategoryName = string.Empty;

            await LoadFromDb();
        }
    }
}