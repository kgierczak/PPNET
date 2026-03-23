using Lab_3.Services;
using SixLabors.ImageSharp.PixelFormats;
using ImageSharp = SixLabors.ImageSharp;
using MauiImage = Microsoft.Maui.Controls.Image;

namespace Lab_3
{
    public partial class MainPage : ContentPage
    {
        private ImageSharp.Image<Rgba32>? _currentImage;
        private byte[]? _imageData;

        public MainPage()
        {
            InitializeComponent();

            ThreadSlider.ValueChanged += (s, e) =>
            {
                ThreadCountLabel.Text = ((int)ThreadSlider.Value).ToString();
            };
        }

        private async void OnLoadImageClicked(object? sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result == null)
                {
                    await DisplayAlert("Info", "No image selected", "OK");
                    return;
                }

                using (var stream = await result.OpenReadAsync())
                {
                    using (var ms = new MemoryStream())
                    {
                        await stream.CopyToAsync(ms);
                        _imageData = ms.ToArray();
                    }
                }

                _currentImage = ImageProcessor.LoadImageFromBytes(_imageData);

                if (_currentImage == null)
                {
                    await DisplayAlert("Error", "Could not load image", "OK");
                    return;
                }

                OriginalImage.Source = ImageSource.FromStream(() => new MemoryStream(_imageData));
                StatusLabel.Text = $"Image loaded: {_currentImage.Width}x{_currentImage.Height}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load image: {ex.Message}", "OK");
            }
        }

        private async void OnProcessClicked(object? sender, EventArgs e)
        {
            if (_currentImage == null)
            {
                await DisplayAlert("Error", "Please load an image first", "OK");
                return;
            }

            try
            {
                ProcessBtn.IsEnabled = false;
                StatusLabel.Text = "Processing...";
                ResultsContainer.Clear();

                int threadCount = (int)ThreadSlider.Value;

                // Run on thread pool to avoid blocking UI
                var results = await Task.Run(() =>
                    ImageProcessor.ApplyFiltersParallel(_currentImage.Clone(), threadCount, 6));

                // Update UI on main thread
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DisplayResults(results);
                    ProcessBtn.IsEnabled = true;
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Processing failed: {ex.Message}", "OK");
                ProcessBtn.IsEnabled = true;
            }
        }

        private void DisplayResults(List<ImageProcessor.FilterResult> results)
        {
            ResultsContainer.Clear();

            long totalTime = 0;
            int successCount = 0;

            foreach (var result in results)
            {
                if (result.Success)
                {
                    totalTime += result.TimeMs;
                    successCount++;

                    var frame = new Frame
                    {
                        BorderColor = Colors.LightGray,
                        CornerRadius = 10,
                        Padding = 10,
                        Margin = 5,
                        Content = new VerticalStackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Label { Text = $"{result.Name} ({result.TimeMs}ms)", FontSize = 14, FontAttributes = FontAttributes.Bold },
                                new MauiImage 
                                { 
                                    Source = ImageSource.FromStream(() => new MemoryStream(ImageProcessor.ImageToBytes(result.Image!))),
                                    Aspect = Aspect.AspectFit,
                                    HeightRequest = 150
                                }
                            }
                        }
                    };

                    ResultsContainer.Add(frame);
                }
                else
                {
                    var errorLabel = new Label
                    {
                        Text = $"❌ {result.Name}: Failed",
                        TextColor = Colors.Red,
                        FontSize = 12
                    };
                    ResultsContainer.Add(errorLabel);
                }
            }

            var summaryLabel = new Label
            {
                Text = $"\n📊 Summary: {successCount} filters processed in {totalTime}ms total",
                FontSize = 12,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Green,
                Margin = 10
            };
            ResultsContainer.Add(summaryLabel);

            StatusLabel.Text = $"Processing complete: {successCount} filters in {totalTime}ms";
        }
    }
}

