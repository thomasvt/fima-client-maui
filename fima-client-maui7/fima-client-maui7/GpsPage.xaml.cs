namespace fima_client_maui7;

public partial class GpsPage : ContentPage
{
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isTracking = false;

    public GpsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        StartLocationUpdates();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        StopLocationUpdates();
    }

    private async void StartLocationUpdates()
    {
        _isTracking = true;
        _cancellationTokenSource = new CancellationTokenSource();

        while (_isTracking)
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium), _cancellationTokenSource.Token);
                if (location != null)
                {
                    LatitudeLabel.Text = $"Latitude: {location.Latitude}";
                    LongitudeLabel.Text = $"Longitude: {location.Longitude}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting location: {ex.Message}");
            }

            // Wait for 1 second before updating again
            await Task.Delay(1000);
        }
    }

    private void StopLocationUpdates()
    {
        _isTracking = false;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
    }
}