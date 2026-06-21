using BlazorMovieApp.MLModel;
using Microsoft.Extensions.ML;

namespace BlazorMovieApp.Services;

public class SentimentPredictionService : ISentimentPredictor
{
    private readonly PredictionEnginePool<ModelInput, ModelOutput>? _predictionPool;

    public SentimentPredictionService(PredictionEnginePool<ModelInput, ModelOutput>? predictionPool = null)
    {
        _predictionPool = predictionPool;
    }

    public ModelOutput Predict(ModelInput input)
    {
        if (_predictionPool == null)
        {
            throw new InvalidOperationException("ML.NET model is not loaded. Ensure sentiment.zip exists in MLModel folder.");
        }

        if (string.IsNullOrEmpty(input?.Text))
        {
            return new ModelOutput
            {
                PredictedLabel = "Neutral",
                Score = 0.5f
            };
        }

        return _predictionPool.Predict(input);
    }
}
