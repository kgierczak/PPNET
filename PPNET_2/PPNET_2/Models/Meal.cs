namespace PPNET_2.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public string ApiId { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public override string ToString()
        {
            return $"ID: {ApiId ?? Id.ToString()}, Posiłek: {Name}, Kategoria: {Category?.Name ?? "Brak"}, Obszar: {Area ?? "Brak"}";
        }
    }
}