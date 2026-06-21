using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;
using System.Linq;

string dataPath = "../BlazorWeatherApp/MLModel/training_data.csv";
string modelPath = "../BlazorWeatherApp/MLModel/sentiment.zip";

Console.WriteLine("Rozpoczynam trening modelu ML.NET dla analizy sentymentu...\n");

if (!File.Exists(dataPath))
{
    Console.WriteLine($"Błąd: Nie znaleziono pliku treningowego: {dataPath}");
    Console.WriteLine($"Szukam w: {Path.GetFullPath(dataPath)}");
    return;
}

var mlContext = new MLContext();

try
{
    Console.WriteLine("Wczytywanie danych treningowych...");
    var data = mlContext.Data.LoadFromTextFile<SentimentData>(dataPath, hasHeader: true, separatorChar: ',');

    var trainTestSplit = mlContext.Data.TrainTestSplit(data, testFraction: 0.2, seed: 1);
    var trainingData = trainTestSplit.TrainSet;
    var testData = trainTestSplit.TestSet;

    Console.WriteLine("Dane wczytane\n");

    Console.WriteLine("Budowanie pipeline'u ML...");
    var pipeline = mlContext.Transforms.Conversion.MapValueToKey(
        outputColumnName: "LabelKey",
        inputColumnName: nameof(SentimentData.Label))
        .Append(mlContext.Transforms.Text.FeaturizeText(
            outputColumnName: "Features",
            inputColumnName: nameof(SentimentData.Text)))
        .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
            labelColumnName: "LabelKey",
            featureColumnName: "Features"))
        .Append(mlContext.Transforms.Conversion.MapKeyToValue(
            outputColumnName: "PredictedLabel",
            inputColumnName: "PredictedLabel"));

    Console.WriteLine("Pipeline zbudowany\n");

    Console.WriteLine("Rozpoczęcie treningu modelu...\n");
    var startTime = DateTime.Now;
    var model = pipeline.Fit(trainingData);
    var trainingTime = DateTime.Now - startTime;

    Console.WriteLine($"Trening ukończony w {trainingTime.TotalSeconds:F1} sekund\n");

    Console.WriteLine("Ewaluacja modelu na zbiorze testowym...\n");
    var predictions = model.Transform(testData);
    var metrics = mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName: "LabelKey");

    Console.WriteLine($"Dokładność (Accuracy):        {metrics.MicroAccuracy:P2}");
    Console.WriteLine($"Przeciętna Log-Loss:          {metrics.LogLoss:F3}");
    Console.WriteLine($"Log-Loss redukcja:            {metrics.LogLossReduction:F3}\n");

    Console.WriteLine("Testowanie modelu na przykładach:\n");

    var predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
    var testExamples = new[]
    {
        "I love this, it's amazing!",
        "This is terrible and awful",
        "It's okay, nothing special",
        "Czuję się super, życie jest piękne!",
        "To jest okropne i straszne",
        "I don't feel good",
        "Nie jestem zadowolony"
    };

    foreach (var text in testExamples)
    {
        var input = new SentimentData { Text = text };
        var output = predictionEngine.Predict(input);
        var maxScore = output.Score?.Length > 0 ? output.Score.Max() : 0f;
        Console.WriteLine($"Tekst: \"{text}\"");
        Console.WriteLine($"Przewidywanie: {output.Prediction} (pewność: {maxScore:P2})\n");
    }

    Console.WriteLine("Zapisywanie modelu do pliku...");
    mlContext.Model.Save(model, trainingData.Schema, modelPath);
    Console.WriteLine($"Model zapisany: {Path.GetFullPath(modelPath)}\n");

    FileInfo fileInfo = new FileInfo(modelPath);
    Console.WriteLine($"Rozmiar modelu: {fileInfo.Length / 1024.0:F2} KB");
    Console.WriteLine("\nTrening modelu ukończony pomyślnie!");
    Console.WriteLine("Możesz teraz uruchomić aplikację Blazor - będzie używać wytrenowanego modelu.");
}
catch (Exception ex)
{
    Console.WriteLine($"\nBłąd podczas treningu: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}

public class SentimentData
{
    [LoadColumn(0)]
    public string? Text { get; set; }

    [LoadColumn(1)]
    public string? Label { get; set; }
}

public class SentimentPrediction
{
    [ColumnName("PredictedLabel")]
    public string? Prediction { get; set; }

    public float[]? Score { get; set; }
}

