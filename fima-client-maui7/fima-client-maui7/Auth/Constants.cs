namespace fima_client_maui7.Auth
{
    public static class Constants
    {
        public static readonly string ClientId = "b128319e-3798-4650-ae00-f46274d7cb60";
        public static readonly string TenantId = "d2f389b3-db11-452f-b7bb-f1e9148478c6";
        public static readonly string[] Scopes = "openid profile offline_access api://9bf99f11-ec08-42dc-84dd-82b24ef99fe7/user_impersonation".Split(' ');

        // The next code to add B2C
        public static readonly string TenantName = "YOUR_TENANT_NAME";
        public static readonly string SignInPolicy = "B2C_1_Client";
        public static readonly string AuthorityBase = $"https://login.microsoftonline.com/d2f389b3-db11-452f-b7bb-f1e9148478c6/v2.0";
        public static readonly string AuthoritySignIn = $"{AuthorityBase}{SignInPolicy}";
        public static readonly string IosKeychainSecurityGroups = "com.fluxys.fima.dev";
    }
}
