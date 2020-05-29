using System;
using System.Net.Http;
using System.Threading.Tasks;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;

namespace Shoko.Jellyfin
{
    public class Plugin : BasePlugin<PluginConfiguration>
    {
        public const string ShokoProvider = "Shoko";
        private readonly IHttpClient _httpClient;

        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, IJsonSerializer jsonSerializer,
            IHttpClient httpClient) : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
            JsonSerializer = jsonSerializer;
            _httpClient = httpClient;
        }

        public override string Name => "Shoko for Emby";

        public override string Description => base.Description;

        public override Guid Id => new Guid("9C75CC0F-5174-4836-8901-7CA38B98DC52");
        public static Plugin Instance { get; private set; }

        public IJsonSerializer JsonSerializer { get; set; }

        public async Task<T> GetHttpAsync<T>(string endpoint)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"{Configuration.Url}api/{endpoint}");
            message.Headers.Accept.Clear();
            message.Headers.Accept.ParseAdd("application/json");
            message.Headers.Add("apikey", Configuration.GetApiKey());

            var options = new HttpRequestOptions {
                Url = $"{Configuration.Url}api/{endpoint}",
                AcceptHeader = "application/json",
                LogErrorResponseBody = true
            };
            options.RequestHeaders.Add("apikey", Configuration.GetApiKey());


            return await JsonSerializer.DeserializeFromStreamAsync<T>(
                    (await _httpClient.SendAsync(options, "GET").ConfigureAwait(false)).Content)
                .ConfigureAwait(false);
        }

        public T GetHttp<T>(string endpoint)
        {
            return GetHttpAsync<T>(endpoint).GetAwaiter().GetResult();
        }
    }
}