namespace fima_client_maui7;

public partial class PhotoPage : ContentPage
{
	public PhotoPage()
	{
		InitializeComponent();
	}

    private async void OnTakePictureButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Check if the device's camera is supported
            if (MediaPicker.Default.IsCaptureSupported)
            {
                // Capture the photo
                var photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take a picture of your observation"
                });

                if (photo != null)
                {
                    // Save the photo to a local file
                    var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, photo.FileName);

                    using var stream = await photo.OpenReadAsync();
                    using var fileStream = File.OpenWrite(filePath);
                    await stream.CopyToAsync(fileStream);

                    // Display the image
                    CapturedImage.Source = ImageSource.FromFile(filePath);
                }
                else
                {
                    await DisplayAlert("Error", "Camera capture failed.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Camera capture is not supported on this device.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while capturing the photo: {ex.Message}", "OK");
        }
    }
}