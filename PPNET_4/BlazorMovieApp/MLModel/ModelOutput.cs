namespace BlazorMovieApp.MLModel;

public class ModelOutput
{
    [Microsoft.ML.Data.ColumnName("PredictedLabel")]
    public string? PredictedLabel { get; set; }

    public float Score { get; set; }
}
