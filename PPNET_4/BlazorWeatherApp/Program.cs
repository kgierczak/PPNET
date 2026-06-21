using BlazorWeatherApp.Components;
using BlazorWeatherApp.MLModel;
using BlazorWeatherApp.Services;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.Extensions.ML;
using Microsoft.ML;

namespace BlazorWeatherApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
            .AddCertificate();

        // ML.NET Configuration
        var mlContext = new MLContext();
        builder.Services.AddSingleton(mlContext);

        // Configure PredictionEnginePool for sentiment analysis
        string modelPath = Path.Combine(AppContext.BaseDirectory, "MLModel", "sentiment.zip");
        if (File.Exists(modelPath))
        {
            builder.Services.AddPredictionEnginePool<ModelInput, ModelOutput>()
                .FromFile(modelPath);
        }

        // Register sentiment prediction service
        builder.Services.AddScoped<ISentimentPredictor, SentimentPredictionService>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
