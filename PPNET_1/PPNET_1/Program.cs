namespace PPNET_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== Knapsack Problem ===\n");

                Console.Write("Podaj wartość ziarna (seed): ");
                int seed = int.Parse(Console.ReadLine());

                Console.Write("Podaj ilość przedmiotów: ");
                int numberOfItems = int.Parse(Console.ReadLine());

                Problem problem = new Problem(numberOfItems, seed);

                Console.WriteLine("\n" + problem.ToString());

                Console.Write("Podaj pojemność plecaka: ");
                int capacity = int.Parse(Console.ReadLine());

                Result result = problem.Solve(capacity);

                Console.WriteLine("\n=== Rozwiązanie ===");
                Console.WriteLine(result.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nWciśnij dowolny klawisz aby zakończyć...");
            Console.ReadKey();
        }
    }
}
