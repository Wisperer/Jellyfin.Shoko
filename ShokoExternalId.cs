using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Shoko.Jellyfin
{
    public class ShokoSeriesExternalId : IExternalId
    {
        public bool Supports(IHasProviderIds item)
        {
            return item is Season;
        }

        public string Name { get; } = Constants.ShokoSeries;
        public string Key { get; } = "Shoko.Jellyfin.Series";
        public string UrlFormatString { get; } = $"{Plugin.Instance.Configuration.Url}api/serie.json?id={0}";
    }

    public class ShokoGroupExternalId : IExternalId
    {
        public bool Supports(IHasProviderIds item)
        {
            return item is Series;
        }

        public string Name { get; } = Constants.ShokoGroup;
        public string Key { get; } = "Shoko.Jellyfin.Group";
        public string UrlFormatString { get; } = $"{Plugin.Instance.Configuration.Url}api/group.json?id={0}";
    }

    public class ShokoEpisodeExternalId : IExternalId
    {
        public bool Supports(IHasProviderIds item)
        {
            return item is Series;
        }

        public string Name { get; } = Constants.ShokoEpisode;
        public string Key { get; } = "Shoko.Jellyfin.Episode";
        public string UrlFormatString { get; } = $"{Plugin.Instance.Configuration.Url}api/ep.json?id={0}";
    }
}