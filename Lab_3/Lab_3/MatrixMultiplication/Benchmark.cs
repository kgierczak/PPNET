using System.Diagnostics;

namespace MatrixMultiplication;

public class Benchmark
{
    public static BenchmarkResult MeasureSequential(Matrix a, Matrix b)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var result = Matrix.MultiplySequential(a, b);
        sw.Stop();
        return new BenchmarkResult("Sequential", 1, sw.ElapsedMilliseconds, result);
    }

    public static BenchmarkResult MeasureParallel(Matrix a, Matrix b, int threads)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var result = Matrix.MultiplyParallel(a, b, threads);
        sw.Stop();
        return new BenchmarkResult("Parallel.For", threads, sw.ElapsedMilliseconds, result);
    }

    public static BenchmarkResult MeasureThreads(Matrix a, Matrix b, int threadCount)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var result = Matrix.MultiplyWithThreads(a, b, threadCount);
        sw.Stop();
        return new BenchmarkResult("Thread", threadCount, sw.ElapsedMilliseconds, result);
    }

    public static void RunTests()
    {
        int[] sizes = { 100, 200, 500 };
        int[] threadCounts = { 1, 2, 4, 8, 14 };
        int iterations = 5;

        Console.WriteLine("=== Matrix Multiplication Benchmark ===\n");

        foreach (int size in sizes)
        {
            Console.WriteLine($"\n--- Matrix Size: {size}x{size} ---");

            Random rand = new(42);
            Matrix a = new(size, rand);
            Matrix b = new(size, rand);

            // Sequential (baseline)
            long seqTotal = 0;
            for (int i = 0; i < iterations; i++)
            {
                var result = MeasureSequential(a, b);
                seqTotal += result.TimeMs;
            }
            long seqAvg = seqTotal / iterations;
            Console.WriteLine($"Sequential (1 thread): {seqAvg} ms (avg)");

            // Parallel.For
            foreach (int threads in threadCounts)
            {
                long parTotal = 0;
                for (int i = 0; i < iterations; i++)
                {
                    var result = MeasureParallel(a, b, threads);
                    parTotal += result.TimeMs;
                }
                long parAvg = parTotal / iterations;
                double speedup = (double)seqAvg / parAvg;
                Console.WriteLine($"Parallel.For ({threads} threads): {parAvg} ms (avg), Speedup: {speedup:F2}x");
            }

            // Thread with different thread counts
            Console.WriteLine("\nManual Thread approach:");
            foreach (int threads in threadCounts)
            {
                long thrTotal = 0;
                for (int i = 0; i < iterations; i++)
                {
                    var result = MeasureThreads(a, b, threads);
                    thrTotal += result.TimeMs;
                }
                long thrAvg = thrTotal / iterations;
                double speedup = (double)seqAvg / thrAvg;
                Console.WriteLine($"Thread ({threads} threads): {thrAvg} ms (avg), Speedup: {speedup:F2}x");
            }

            // Verify correctness
            Console.WriteLine("\nVerifying correctness...");
            var sequential = Matrix.MultiplySequential(a, b);
            var parallel4 = Matrix.MultiplyParallel(a, b, 4);
            var threads4 = Matrix.MultiplyWithThreads(a, b, 4);

            bool seqParallel = sequential.Equals(parallel4);
            bool seqThreads = sequential.Equals(threads4);

            Console.WriteLine($"Sequential vs Parallel.For: {(seqParallel ? " CORRECT" : " INCORRECT")}");
            Console.WriteLine($"Sequential vs Thread: {(seqThreads ? " CORRECT" : " INCORRECT")}");
        }
    }
}

public record BenchmarkResult(string Method, int Threads, long TimeMs, Matrix ResultMatrix);
