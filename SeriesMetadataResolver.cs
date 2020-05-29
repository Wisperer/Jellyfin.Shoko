using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Shoko.Jellyfin.Models;
using Shoko.Jellyfin.Models.Group;

namespace Shoko.Jellyfin
{
    public class SeriesMetadataResolver : IRemoteMetadataProvider<Series, SeriesInfo>
    {
        private readonly IHttpClient _httpClient;

        public SeriesMetadataResolver(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeriesInfo info,
            CancellationToken cancellationToken)
        {
            TitleType[] types = {TitleType.Main, TitleType.Official, TitleType.Short};

            var results = await Plugin.Instance.GetHttpAsync<List<SeriesCollection>>($"group/search?query={info.Name}")
                .ConfigureAwait(false);
            return results.Select(s => new RemoteSearchResult {
                Name = s.Titles.First(x => types.Any(a => a == x.Type) || x.Language == info.MetadataCountryCode).Title,
                Overview = s.Summary,
                PremiereDate = s.Air.LocalDateTime,
                ProductionYear = s.Air.Year,
                SearchProviderName = Name,
                ProviderIds = new Dictionary<string, string> {{Constants.ShokoGroup, s.Id.ToString()}}
            });
        }

        public async Task<MetadataResult<Series>> GetMetadata(SeriesInfo info, CancellationToken cancellationToken)
        {
            var apiResult =
                await Plugin.Instance.GetHttpAsync<GroupResult>($"group?id={info.ProviderIds[Constants.ShokoGroup]}");
            var result = new MetadataResult<Series>();
            var series = result.Item ?? (result.Item = new Series());

            result.HasMetadata = true;
            result.Provider = Constants.ShokoGroup;
            apiResult.Roles.ForEach(role =>
            {
                result.AddPerson(new PersonInfo {
                    ImageUrl = (role.CharacterImage ?? role.StaffImage) != null
                        ? Plugin.Instance.Configuration.Url + (role.CharacterImage ?? role.StaffImage)
                        : string.Empty,
                    Name = role.Staff,
                    Role = role.Character,
                    Type = PersonType.Actor
                });
            });

            series.Name = apiResult.Name;
            series.PremiereDate = apiResult.Air.DateTime.ToUniversalTime();
            series.Overview = apiResult.Summary;
            series.Genres = apiResult.Tags.ToArray();
            series.OfficialRating = apiResult.Rating;

            return result;
        }

        public string Name { get; } = "Shoko Series";

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            return _httpClient.GetResponse(new HttpRequestOptions {
                CancellationToken = cancellationToken,
                Url = Plugin.Instance.Configuration.Url + url
            });
        }
    }
}