using System.Net.Http;
using System.Text;
using MediaBrowser.Model.Plugins;

namespace Shoko.Jellyfin
{
    public enum TitlePreferenceType
    {
        /// <summary>
        ///     Use titles in the local metadata language.
        /// </summary>
        Localized,

        /// <summary>
        ///     Use titles in Japanese.
        /// </summary>
        Japanese,

        /// <summary>
        ///     Use titles in Japanese romaji.
        /// </summary>
        JapaneseRomaji
    }

    public class PluginConfiguration : BasePluginConfiguration
    {
        public TitlePreferenceType TitlePreference { get; set; } = TitlePreferenceType.Localized;
        public string Url { get; set; } = "http://localhost";
        public string Port { get; set; } = "8111/";
        public string Username { get; set; } = "Default";
        public string Password { get; set; } = "";

        public string GetApiKey()
        {
            var message = new HttpRequestMessage(HttpMethod.Post, $"{Plugin.Instance.Configuration.Url}:{Plugin.Instance.Configuration.Port}api/auth/");
            message.Headers.Accept.Clear();
            message.Headers.Accept.ParseAdd("application/json");

            var auth = Plugin.Instance.JsonSerializer.SerializeToString(new {
                user = Username,
                pass = Password,
                device = "Shoko Jellyfin Plugin"
            });

            message.Content = new StringContent(auth, Encoding.UTF8, "application/json");

            return Plugin.Instance.JsonSerializer.DeserializeFromString<ApiKeyResult>(new HttpClient()
                .SendAsync(message).ConfigureAwait(false).GetAwaiter().GetResult().Content.ReadAsStringAsync()
                .ConfigureAwait(false).GetAwaiter().GetResult()).apikey;
        }
    }

    public class ApiKeyResult
    {
        public string apikey { get; set; }
    }
}