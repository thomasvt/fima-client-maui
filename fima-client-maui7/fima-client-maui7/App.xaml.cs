namespace fima_client_maui7
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            // receive oauth2 callback
            if (uri.Scheme == "fima")
            {
                var response = uri.AbsoluteUri; // Get the full URI
            }

            base.OnAppLinkRequestReceived(uri);
        }
    }
}
