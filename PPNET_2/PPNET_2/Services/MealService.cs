using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PPNET_2.Services
{
    public class MealService
    {
        private HttpClient client;

        public MealService()
        {
            client = new HttpClient();
        }

        public async Task<List<MealDto>> GetMealsAsync(string query)
        {
            string call = $"https://www.themealdb.com/api/json/v1/1/search.php?s={query}";
            string response = await client.GetStringAsync(call);
            MealApiResponse apiResponse = JsonSerializer.Deserialize<MealApiResponse>(response);
            return apiResponse?.meals ?? new List<MealDto>();
        }
    }

    public class MealApiResponse
    {
        public List<MealDto> meals { get; set; }
    }

    public class MealDto
    {
        public string idMeal { get; set; }
        public string strMeal { get; set; }
        public string strCategory { get; set; }
        public string strArea { get; set; }

        public override string ToString()
        {
            return $"ID: {idMeal}, Posilek: {strMeal}, Kategoria: {strCategory}, Obszar: {strArea}";
        }
    }
}
