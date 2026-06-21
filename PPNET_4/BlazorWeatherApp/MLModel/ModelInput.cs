namespace BlazorWeatherApp.MLModel;

public class ModelInput
{
    [Microsoft.ML.Data.ColumnName("Text")]
    public string? Text { get; set; }
}
