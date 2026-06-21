using BlazorWeatherApp.MLModel;

namespace BlazorWeatherApp.Services;

public interface ISentimentPredictor
{
    ModelOutput Predict(ModelInput input);
}
