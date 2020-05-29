using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Shoko.Jellyfin.Models.Group;

namespace Shoko.Jellyfin
{
    public class ShokoImageProvider : IRemoteImageProvider
    {
        private readonly IApplicationPaths _appPaths;
        private readonly IHttpClient _httpClient;

        public ShokoImageProvider(IHttpClient httpClient, IApplicationPaths appPaths)
        {
            _httpClient = httpClient;
            _appPaths = appPaths;
        }

        public string Name => Constants.ShokoGroup;

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            return _httpClient.GetResponse(new HttpRequestOptions {
                CancellationToken = cancellationToken,
                Url = url
            });
        }

        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
        {
            var series = item as Series;

            var apiResult = (await Plugin.Instance
                .GetHttpAsync<GroupResult[]>($"group?id={series.GetProviderId(Constants.ShokoGroup)}")
                .ConfigureAwait(false))[0];
            var list = new List<RemoteImageInfo>();

            apiResult.Art.Banner.ForEach(art =>
            {
                list.Add(new RemoteImageInfo {
                    Url = Plugin.Instance.Configuration.Url + art.Url,
                    ProviderName = Name,
                    Type = ImageType.Backdrop
                });
            });
            apiResult.Art.Fanart.ForEach(art =>
            {
                list.Add(new RemoteImageInfo {
                    Url = Plugin.Instance.Configuration.Url + art.Url,
                    ProviderName = Name,
                    Type = ImageType.Primary
                });
            });
            apiResult.Art.Thumb.ForEach(art =>
            {
                list.Add(new RemoteImageInfo {
                    Url = Plugin.Instance.Configuration.Url + art.Url,
                    ProviderName = Name,
                    Type = ImageType.Thumb
                });
            });

            return list;
        }

        public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
        {
            return (ImageType[]) Enum.GetValues(typeof(ImageType));
        }

        public bool Supports(BaseItem item)
        {
            return item is Series;
        }
    }
}