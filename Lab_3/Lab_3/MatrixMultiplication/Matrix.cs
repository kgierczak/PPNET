using System.Threading;

namespace MatrixMultiplication;

public class Matrix
{
    private readonly double[][] _data;
    public int Size { get; }

    public Matrix(int size)
    {
        Size = size;
        _data = new double[size][];
        for (int i = 0; i < size; i++)
        {
            _data[i] = new double[size];
        }
    }

    public Matrix(int size, Random random) : this(size)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                _data[i][j] = random.NextDouble() * 100;
            }
        }
    }

    public double Get(int row, int col) => _data[row][col];
    public void Set(int row, int col, double value) => _data[row][col] = value;
    public double[] GetRow(int row) => _data[row];

    public static Matrix MultiplySequential(Matrix a, Matrix b)
    {
        int n = a.Size;
        Matrix result = new(n);

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                double sum = 0;
                for (int k = 0; k < n; k++)
                {
                    sum += a.Get(i, k) * b.Get(k, j);
                }
                result.Set(i, j, sum);
            }
        }

        return result;
    }

    public static Matrix MultiplyParallel(Matrix a, Matrix b, int threads)
    {
        int n = a.Size;
        Matrix result = new(n);
        object lockObj = new();

        var options = new ParallelOptions { MaxDegreeOfParallelism = threads };

        Parallel.For(0, n, options, i =>
        {
            for (int j = 0; j < n; j++)
            {
                double sum = 0;
                for (int k = 0; k < n; k++)
                {
                    sum += a.Get(i, k) * b.Get(k, j);
                }
                lock (lockObj)
                {
                    result.Set(i, j, sum);
                }
            }
        });

        return result;
    }

    public static Matrix MultiplyWithThreads(Matrix a, Matrix b, int threadCount)
    {
        int n = a.Size;
        Matrix result = new(n);
        object lockObj = new();

        int rowsPerThread = n / threadCount;
        int remainingRows = n % threadCount;
        Thread[] threads = new Thread[threadCount];

        for (int t = 0; t < threadCount; t++)
        {
            int threadId = t;
            int startRow = threadId * rowsPerThread + Math.Min(threadId, remainingRows);
            int endRow = startRow + rowsPerThread + (threadId < remainingRows ? 1 : 0);

            threads[t] = new Thread(() =>
            {
                for (int i = startRow; i < endRow; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < n; k++)
                        {
                            sum += a.Get(i, k) * b.Get(k, j);
                        }
                        lock (lockObj)
                        {
                            result.Set(i, j, sum);
                        }
                    }
                }
            });

            threads[t].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return result;
    }

    public void Print()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Console.Write($"{_data[i][j]:F2}\t");
            }
            Console.WriteLine();
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Matrix other) return false;
        if (Size != other.Size) return false;

        const double epsilon = 1e-10;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (Math.Abs(Get(i, j) - other.Get(i, j)) > epsilon)
                    return false;
            }
        }
        return true;
    }

    public override int GetHashCode() => Size.GetHashCode();
}
