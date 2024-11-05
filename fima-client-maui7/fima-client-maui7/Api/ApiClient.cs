using fima_client_maui7.Auth;

namespace fima_client_maui7.Api
{
    public class ApiClient
    {
        private readonly IAuthService _authService;
        private readonly HttpClient _httpClient;

        public ApiClient(IAuthService authService, HttpClient httpClient)
        {
            _authService = authService;
            _httpClient = httpClient;
        }

        public async Task<string> GetUserInfo()
        {
            await SetCurrentAccessToken();

            var response = await _httpClient.GetAsync("https://fima-clientgateway-mobile-fima.dev.azure.fluxys.net/api/v1/userinfo/all");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new Exception($"GET UserInfo failed: {response.StatusCode} {response.ReasonPhrase}");
        }

        private async Task SetCurrentAccessToken()
        {
            var result = await _authService.GetOrAskAuthentication(CancellationToken.None);
            if (result?.AccessToken == null)
                throw new Exception("Authentication failed.");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
        }
    }
}
