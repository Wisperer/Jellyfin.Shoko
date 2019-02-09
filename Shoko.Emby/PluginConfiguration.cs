using MediaBrowser.Model.Plugins;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Shoko.Emby
{
    public enum TitlePreferenceType
    {
        /// <summary>
        /// Use titles in the local metadata language.
        /// </summary>
        Localized,

        /// <summary>
        /// Use titles in Japanese.
        /// </summary>
        Japanese,

        /// <summary>
        /// Use titles in Japanese romaji.
        /// </summary>
        JapaneseRomaji
    }

    public class PluginConfiguration : BasePluginConfiguration
    {
        public TitlePreferenceType TitlePreference { get; set; } = TitlePreferenceType.Localized;
        public string Url { get; set; } = "http://192.168.1.3:8111/";
        public string Username { get; set; } = "Default";
        public string Password { get; set; } = "";

        public string GetApiKey()
        {
            var message = new HttpRequestMessage(HttpMethod.Post, $"{Plugin.Instance.Configuration.Url}api/auth");
            message.Headers.Accept.Clear();
            message.Headers.Accept.ParseAdd("application/json");

            var auth = Plugin.Instance.JsonSerializer.SerializeToString(new
            {
                user = Username,
                pass = Password,
                device = "Shoko Emby Plugin"
            });

            message.Content = new StringContent(auth, Encoding.UTF8, "application/json");

            return Plugin.Instance.JsonSerializer.DeserializeFromString<ApiKeyResult>(new HttpClient().SendAsync(message).ConfigureAwait(false).GetAwaiter().GetResult().Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult()).apikey;
        }
    }
    
    public class ApiKeyResult
    {
        public string apikey { get; set; }
    }
}
