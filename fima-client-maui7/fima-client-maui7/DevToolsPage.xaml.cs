using System.Text;
using System.Text.Json;
using fima_client_maui7.Api;
using fima_client_maui7.Auth;
using fima_client_maui7.LocalStorage;
using Microsoft.Identity.Client;

namespace fima_client_maui7;

public partial class DevToolsPage : ContentPage
{
    private readonly IAuthService _authService;
    private readonly ApiClient _apiClient;
    private readonly LocalStore _localStore;
    private string _userInfo;

    public DevToolsPage(IAuthService authService, ApiClient apiClient, LocalStore localStore)
    {
        _authService = authService;
        _apiClient = apiClient;
        _localStore = localStore;
        InitializeComponent();

        InitializeConnectivityTracking();
        ShowDeviceType();
    }

    private void InitializeConnectivityTracking()
    {
        OnlineCheckBox.IsChecked = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        Connectivity.Current.ConnectivityChanged += (sender, e) =>
        {
            OnlineCheckBox.IsChecked = e.NetworkAccess == NetworkAccess.Internet;
        };
    }

    private void ShowDeviceType()
    {
        var device = DeviceInfo.Current;
        DeviceTypeLabel.Text = JsonSerializer.Serialize(new
        {
            Platform = device.Platform.ToString(), 
            Name = device.Name,
            Type = device.DeviceType.ToString(),
            Idiom = device.Idiom.ToString(),
            Model = device.Model,
            Manufacturer = device.Manufacturer,
            Version = device.VersionString

        });
    }

    private async Task GetResult(AuthenticationResult result)
    {
        var claims = result?.ClaimsPrincipal.Claims;
        if (claims != null)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Name: {claims.FirstOrDefault(x => x.Type.Equals("name"))?.Value}");
            await Application.Current.MainPage.DisplayAlert("Title", stringBuilder.ToString(), "OK");
        }
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await _authService.LogIn(CancellationToken.None);
            await GetResult(result);
        }
        catch (MsalClientException ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK :(");
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await _authService.LogoutAsync(CancellationToken.None);
    }

    private async void OnGetUserInfoClicked(object sender, EventArgs e)
    {
        _userInfo = await _apiClient.GetUserInfo();
        UserInfoLabel.Text = _userInfo;
    }

    private async void SaveUserInfoButton_OnClicked(object sender, EventArgs e)
    {
        await _localStore.Set("user", _userInfo);
    }

    private async void LoadUserButton_OnClicked(object sender, EventArgs e)
    {
        UserInfoLabel.Text = await _localStore.Get("user") ?? "<?>";
    }

    private async void ClearLocalDbButton_OnClicked(object sender, EventArgs e)
    {
        await _localStore.CreateSchema(true);
    }
}