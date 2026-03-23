namespace PPNET_1
{
    internal class Result
    {
        public List<int> SelectedItems { get; set; }
        public int TotalValue { get; set; }
        public int TotalWeight { get; set; }

        public Result()
        {
            SelectedItems = new List<int>();
            TotalValue = 0;
            TotalWeight = 0;
        }

        public override string ToString()
        {
            if (SelectedItems.Count == 0)
            {
                return "Nie wybrano żadnych przedmiotów.\nŁączna wartość: 0\nŁączna waga: 0";
            }

            string items = string.Join(", ", SelectedItems);
            return $"Wybrane przedmioty: {items}\nŁączna wartość: {TotalValue}\nŁączna waga: {TotalWeight}";
        }
    }
}
