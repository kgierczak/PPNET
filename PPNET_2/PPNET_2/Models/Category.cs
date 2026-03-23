using System.Collections.Generic;

namespace PPNET_2.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Meal> Meals { get; set; } = new List<Meal>();
    }
}