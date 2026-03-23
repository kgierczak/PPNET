using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using Image = SixLabors.ImageSharp.Image;

namespace Lab_3.Services;

public class ImageProcessor
{
    public static class Filters
    {
        public static Image<Rgba32> Grayscale(Image<Rgba32> image)
        {
            var result = image.Clone(x => x.Grayscale());
            return result;
        }

        public static Image<Rgba32> Invert(Image<Rgba32> image)
        {
            var result = image.Clone(x => x.Invert());
            return result;
        }

        public static Image<Rgba32> Sepia(Image<Rgba32> image)
        {
            var result = image.Clone(x => x.Sepia());
            return result;
        }

        public static Image<Rgba32> EdgeDetection(Image<Rgba32> image)
        {
            var result = image.Clone(x => x.DetectEdges());
            return result;
        }

        public static Image<Rgba32> Blur(Image<Rgba32> image)
        {
            var result = image.Clone(x => x.GaussianBlur());
            return result;
        }

        public static Image<Rgba32> Brightness(Image<Rgba32> image)
        {
            var result = image.Clone(x => x.Brightness(1.3f));
            return result;
        }
    }

    public class FilterResult
    {
        public string Name { get; set; } = "";
        public Image<Rgba32>? Image { get; set; }
        public long TimeMs { get; set; }
        public bool Success { get; set; }
    }

    public class BenchmarkResult
    {
        public int FilterCount { get; set; }
        public int ThreadCount { get; set; }
        public long TimeMs { get; set; }
        public double Speedup { get; set; }
    }

    private static readonly Dictionary<string, Func<Image<Rgba32>, Image<Rgba32>>> AllFilters = new()
    {
        { "Grayscale", Filters.Grayscale },
        { "Invert", Filters.Invert },
        { "Sepia", Filters.Sepia },
        { "Edge Detection", Filters.EdgeDetection },
        { "Blur", Filters.Blur },
        { "Brightness", Filters.Brightness }
    };

    public static List<FilterResult> ApplyFiltersParallel(Image<Rgba32> sourceImage, int threadCount = 4, int filterCount = 6)
    {
        var filters = AllFilters.Take(Math.Min(filterCount, AllFilters.Count))
            .ToDictionary(x => x.Key, x => x.Value);

        var results = new List<FilterResult>();
        var options = new ParallelOptions { MaxDegreeOfParallelism = threadCount };
        object lockObj = new();

        Parallel.ForEach(filters, options, kvp =>
        {
            var filterName = kvp.Key;
            var filterFunc = kvp.Value;

            try
            {
                var sw = Stopwatch.StartNew();
                var processedImage = filterFunc(sourceImage.Clone());
                sw.Stop();

                var result = new FilterResult
                {
                    Name = filterName,
                    Image = processedImage,
                    TimeMs = sw.ElapsedMilliseconds,
                    Success = true
                };

                lock (lockObj)
                {
                    results.Add(result);
                }
            }
            catch
            {
                lock (lockObj)
                {
                    results.Add(new FilterResult
                    {
                        Name = filterName,
                        Success = false,
                        TimeMs = 0
                    });
                }
            }
        });

        return results.OrderBy(r => r.Name).ToList();
    }

    public static List<FilterResult> ApplyFiltersSequential(Image<Rgba32> sourceImage, int filterCount = 6)
    {
        var filters = AllFilters.Take(Math.Min(filterCount, AllFilters.Count))
            .ToDictionary(x => x.Key, x => x.Value);

        var results = new List<FilterResult>();

        foreach (var kvp in filters)
        {
            var filterName = kvp.Key;
            var filterFunc = kvp.Value;

            try
            {
                var sw = Stopwatch.StartNew();
                var processedImage = filterFunc(sourceImage.Clone());
                sw.Stop();

                results.Add(new FilterResult
                {
                    Name = filterName,
                    Image = processedImage,
                    TimeMs = sw.ElapsedMilliseconds,
                    Success = true
                });
            }
            catch
            {
                results.Add(new FilterResult
                {
                    Name = filterName,
                    Success = false,
                    TimeMs = 0
                });
            }
        }

        return results;
    }

    public static List<BenchmarkResult> BenchmarkFiltersWithDifferentThreads(Image<Rgba32> sourceImage, int[] threadCounts = null, int[] filterCounts = null, int iterations = 3)
    {
        threadCounts ??= new[] { 1, 2, 4, 8, 14 };
        filterCounts ??= new[] { 1, 2, 4, 6 };

        var results = new List<BenchmarkResult>();

        var baselineTimes = new Dictionary<int, long>();
        foreach (int filterCount in filterCounts)
        {
            long totalTime = 0;
            for (int i = 0; i < iterations; i++)
            {
                var sw = Stopwatch.StartNew();
                ApplyFiltersSequential(sourceImage.Clone(), filterCount);
                sw.Stop();
                totalTime += sw.ElapsedMilliseconds;
            }
            baselineTimes[filterCount] = totalTime / iterations;
        }

        foreach (int filterCount in filterCounts)
        {
            long baselineTime = baselineTimes[filterCount];

            foreach (int threadCount in threadCounts)
            {
                long totalTime = 0;
                for (int i = 0; i < iterations; i++)
                {
                    var sw = Stopwatch.StartNew();
                    ApplyFiltersParallel(sourceImage.Clone(), threadCount, filterCount);
                    sw.Stop();
                    totalTime += sw.ElapsedMilliseconds;
                }
                long avgTime = totalTime / iterations;
                double speedup = (double)baselineTime / avgTime;

                results.Add(new BenchmarkResult
                {
                    FilterCount = filterCount,
                    ThreadCount = threadCount,
                    TimeMs = avgTime,
                    Speedup = speedup
                });
            }
        }

        return results;
    }

    public static byte[] ImageToBytes(Image<Rgba32> image)
    {
        using (var ms = new MemoryStream())
        {
            image.SaveAsPng(ms);
            return ms.ToArray();
        }
    }

    public static Image<Rgba32>? LoadImageFromBytes(byte[] imageData)
    {
        try
        {
            using (var ms = new MemoryStream(imageData))
            {
                return Image.Load<Rgba32>(ms);
            }
        }
        catch
        {
            return null;
        }
    }
}
