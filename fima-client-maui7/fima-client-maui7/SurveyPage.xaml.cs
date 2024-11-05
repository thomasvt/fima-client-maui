using System.Reflection;
using System.Text.Json;

namespace fima_client_maui7;

public partial class SurveyPage : ContentPage
{
	public SurveyPage()
	{
		InitializeComponent();
        LoadSurvey();

        SurveyWebView.Navigating += SurveyWebViewNavigating;
    }

    private void SurveyWebViewNavigating(object sender, WebNavigatingEventArgs e)
    {
        if (e.Url.Contains("/api/"))
        {
            var dataId = e.Url[(e.Url.IndexOf("/api/") + 5)..];
            e.Cancel = true;
            Task.Run(() => {
                Dispatcher.Dispatch(async () => {
                    var data = await SurveyWebView.EvaluateJavaScriptAsync($"getData({dataId})");
                    var json = data;
                    // for some reason we get the json encoded as json string, but a variable amount of times, so let's fix that:
                    while (json.Contains("\\\"dataId"))
                    {
                        if (!json.StartsWith('\"')) json = $"\"{json}\"";
                        json = JsonSerializer.Deserialize<string>(json);
                    }

                    var csData = JsonSerializer.Deserialize<CsData>(json);

                    if (csData != null)
                    {
                        switch (csData.command)
                        {
                            case "Log":
                                await Application.Current.MainPage.DisplayAlert("JavaScript says:", csData.dataString, "OK");

                                //Just for the Show...
                                await SurveyWebView.EvaluateJavaScriptAsync($"log('{csData.command} with dataId {csData.dataId} was executed...')");
                                break;
                        }
                    }
                });
            });
        }
    }

    private async void LoadSurvey()
    {
        // // Load the survey HTML file from resources
        // var htmlSource = new HtmlWebViewSource
        // {
        //     Html = await LoadSurveyHtml(),
        //     BaseUrl = "fima_client_maui7.Resources.Raw"
        // };
        // SurveyWebView.Source = htmlSource;
    }

    private async Task<string> LoadSurveyHtml()
    {
        // resource files are in the project's ```Resources/Raw``` folder
        var html = await GetResourceFileContent("test-survey.html");

        var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();

        // var surveyCss = await GetResourceFileContent("defaultV2.min.css");
        // var surveyCoreScript = await GetResourceFileContent("survey.core.min.js");
        // var surveyUIScript = await GetResourceFileContent("survey-js-ui.min.js");
        // 
        // html = html.Replace("<link href=\"https://unpkg.com/survey-core/defaultV2.min.css\" type=\"text/css\" rel=\"stylesheet\">", $"<style>{surveyCss}</style>");
        // html = html.Replace("<script type=\"text/javascript\" src=\"https://unpkg.com/survey-core/survey.core.min.js\"></script>", $"<script type=\"text/javascript\">{surveyCoreScript}</script>");
        // html = html.Replace("<script type=\"text/javascript\" src=\"https://unpkg.com/survey-js-ui/survey-js-ui.min.js\"></script>", $"<script>{surveyUIScript}</script>");

        return html;
    }

    private async Task<string> GetResourceFileContent(string filename)
    {
        await using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}

internal class CsData
{
    public int dataId { get; set; }
    public string command { get; set; }
    public string dataString { get; set; }
}