namespace PPNET_1.Tests;

[TestClass]
public sealed class KnapsackTests
{
    // Test 1: Sprawdza, czy wygenerowanie problemu z n elementami powoduje utworzenie dokładnie n elementów.
    [TestMethod]
    public void Problem_GeneratesCorrectNumberOfItems()
    {
        int numberOfItems = 10;
        Problem problem = new Problem(numberOfItems, 42);

        Assert.AreEqual(numberOfItems, problem.NumberOfItems);
        Assert.AreEqual(numberOfItems, problem.Items.Count);
    }

    // Test 2: Sprawdza, czy jeśli przynajmniej jeden przedmiot spełnia ograniczenia to zwrócono co najmniej jeden element.
    [TestMethod]
    public void Solve_WithSufficientCapacity_SelectsAtLeastOneItem()
    {
        Problem problem = new Problem(5, 123);
        int capacity = 100; 

        Result result = problem.Solve(capacity);

        Assert.IsTrue(result.SelectedItems.Count > 0, " Przynajmniej jeden element powinien zostać wybrany");
        Assert.IsTrue(result.TotalValue > 0, "Wartość całkowita powinna być większa od 0");
    }

    // Test 3: Sprawdza, czy jeśli żaden przedmiot nie spełnia ograniczenia to zwrócono puste rozwiązanie.
    [TestMethod]
    public void Solve_WithZeroCapacity_SelectsNoItems()
    {
        Problem problem = new Problem(5, 456);
        int capacity = 0;

        Result result = problem.Solve(capacity);

        Assert.AreEqual(0, result.SelectedItems.Count);
        Assert.AreEqual(0, result.TotalValue);
        Assert.AreEqual(0, result.TotalWeight);
    }

    // Test 4: Weryfikuje poprawność dla konkretnej instancji.
    [TestMethod]
    public void Solve_WithKnownValues_ReturnsExpectedResult()
    {
        Problem problem = new Problem(5, 100);
        int capacity = 15;

        Result result = problem.Solve(capacity);

        Assert.IsTrue(result.TotalWeight <= capacity, "Waga całkowita nie powinna przekraczać pojemności");

        int calculatedValue = 0;
        int calculatedWeight = 0;
        foreach (int index in result.SelectedItems)
        {
            calculatedValue += problem.Items[index].Value;
            calculatedWeight += problem.Items[index].Weight;
        }

        Assert.AreEqual(calculatedValue, result.TotalValue, "Wartość całkowita powinna odpowiadać sumie wybranych elementów");
        Assert.AreEqual(calculatedWeight, result.TotalWeight, "Całkowita waga powinna odpowiadać sumie wybranych elementów");
    }

    // Test 5: Sprawdza, czy algorytm greedy respektuje ograniczenia pojemności.
    [TestMethod]
    public void Solve_AlwaysRespectsCapacityConstraint()
    {
        Problem problem = new Problem(20, 789);
        int capacity = 25;

        Result result = problem.Solve(capacity);

        Assert.IsTrue(result.TotalWeight <= capacity, 
            $"Waga całkowita {result.TotalWeight} nie powinna przekraczać pojemności {capacity}");
    }

    // Test 6: Sprawdza, czy elementy mają prawidłowe właściwości.
    [TestMethod]
    public void Problem_GeneratesItemsWithValidProperties()
    {
        Problem problem = new Problem(15, 999);

        foreach (var item in problem.Items)
        {
            Assert.IsTrue(item.Value >= 1 && item.Value <= 10, 
                "Wartośc przedmiotu powinna znajdować się między 1 a 10");
            Assert.IsTrue(item.Weight >= 1 && item.Weight <= 10, 
                "Waga przedmiotu powinna znajdować się między 1 a 10");
            Assert.IsTrue(item.Index >= 0 && item.Index < problem.NumberOfItems,
                "Indeks elementu powinien być prawidłowy");
        }
    }
}

