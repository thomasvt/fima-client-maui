using System.Text;
using fima_client_maui7.Api;
using fima_client_maui7.Auth;
using Microsoft.Identity.Client;

namespace fima_client_maui7;

public partial class SecurityPage : ContentPage
{
    private readonly IAuthService _authService;
    private readonly ApiClient _apiClient;

    public SecurityPage()
	{
		InitializeComponent();
        _authService = Svc.BuildAuthService();
        _apiClient = new ApiClient(_authService);
    }

    private async Task GetResult(AuthenticationResult result)
    {
        var claims = result?.ClaimsPrincipal.Claims;
        if (claims != null)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Name: {claims.FirstOrDefault(x => x.Type.Equals("name"))?.Value}");
            await Application.Current.MainPage.DisplayAlert("Title", stringBuilder.ToString(), "OK");
            loginButton.IsVisible = false;
            logoutButton.IsVisible = true;
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
        loginButton.IsVisible = true;
        logoutButton.IsVisible = false;
    }

    private async void OnGetUserInfoClicked(object sender, EventArgs e)
    {
        var userInfo = await _apiClient.GetUserInfo();
        UserInfoLabel.Text = userInfo;
    }
}