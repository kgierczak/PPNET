namespace PPNET_1
{
    internal class Item
    {
        public int Index { get; set; }
        public int Value { get; set; }
        public int Weight { get; set; }

        public Item(int index, int value, int weight)
        {
            Index = index;
            Value = value;
            Weight = weight;
        }

        public double GetRatio()
        {
            return (double)Value / Weight;
        }

        public override string ToString()
        {
            return $"Przedmiot {Index}: Wartość={Value}, Waga={Weight}, Ratio={GetRatio():F2}";
        }
    }
}
