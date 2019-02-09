using MediaBrowser.Common.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Serialization;
using System.Net.Http;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;

namespace Shoko.Emby
{
    public class Plugin : BasePlugin<PluginConfiguration>
    {
        public const string ShokoProvider = "Shoko";
        public override string Name => "Shoko for Emby";

        public override string Description => base.Description;

        public override Guid Id => new Guid("9C75CC0F-5174-4836-8901-7CA38B98DC52");
        public static Plugin Instance { get; private set; }

        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, IJsonSerializer jsonSerializer, IHttpClient httpClient) : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
            this.JsonSerializer = jsonSerializer;
            _httpClient = httpClient;
        }

        public IJsonSerializer JsonSerializer { get; set; }
        private readonly IHttpClient _httpClient;

        public async Task<T> GetHttpAsync<T>(string endpoint)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"{Configuration.Url}api/{endpoint}");
            message.Headers.Accept.Clear();
            message.Headers.Accept.ParseAdd("application/json");
            message.Headers.Add("apikey", Configuration.GetApiKey());

            var options = new HttpRequestOptions() {
                Url = $"{Configuration.Url}api/{endpoint}",
                AcceptHeader = "application/json",
                LogRequestAsDebug = true
            };
            options.RequestHeaders.Add("apikey", Configuration.GetApiKey());


            return await JsonSerializer.DeserializeFromStreamAsync<T>(
                (await _httpClient.SendAsync(options, "GET").ConfigureAwait(false)).Content)
                .ConfigureAwait(false);
        }

        public T GetHttp<T>(string endpoint) => GetHttpAsync<T>(endpoint).GetAwaiter().GetResult();
    }
}
