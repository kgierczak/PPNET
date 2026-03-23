namespace PPNET_1
{
    internal class Problem
    {
        public int NumberOfItems { get; private set; }
        public List<Item> Items { get; private set; }

        public Problem(int n, int seed)
        {
            NumberOfItems = n;
            Items = new List<Item>();

            Random random = new Random(seed);

            for (int i = 0; i < n; i++)
            {
                int value = random.Next(1, 11);
                int weight = random.Next(1, 11);
                Items.Add(new Item(i, value, weight));
            }
        }

        public Result Solve(int capacity)
        {
            Result result = new Result();

            var sortedItems = Items.OrderByDescending(item => item.GetRatio()).ToList();

            int remainingCapacity = capacity;

            foreach (var item in sortedItems)
            {
                if (item.Weight <= remainingCapacity)
                {
                    result.SelectedItems.Add(item.Index);
                    result.TotalValue += item.Value;
                    result.TotalWeight += item.Weight;
                    remainingCapacity -= item.Weight;
                }
            }

            result.SelectedItems.Sort();

            return result;
        }

        public override string ToString()
        {
            string output = $"Knapsack Problem:\nLiczba Przedmiotów: {NumberOfItems}\n\nPrzedmioty:\n";

            foreach (var item in Items)
            {
                output += item.ToString() + "\n";
            }

            return output;
        }
    }
}
