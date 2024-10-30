namespace FimaClient
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            try
            {
                // Request location permissions if needed
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    LocationLabel.Text = "Location permission denied.";
                    return;
                }

                // Get current location
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });

                if (location != null)
                {
                    LocationLabel.Text = $"Latitude: {location.Latitude:0.000}, Longitude: {location.Longitude:0.000}";
                }
                else
                {
                    LocationLabel.Text = "Unable to retrieve location.";
                }
            }
            catch (Exception ex)
            {
                LocationLabel.Text = $"Error: {ex.Message}";
            }
        }
    }

}
