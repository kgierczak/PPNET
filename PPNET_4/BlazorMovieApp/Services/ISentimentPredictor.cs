using BlazorMovieApp.MLModel;

namespace BlazorMovieApp.Services;

public interface ISentimentPredictor
{
    ModelOutput Predict(ModelInput input);
}
