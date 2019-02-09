using System;
using System.Collections.Generic;
using System.Text;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Shoko.Emby
{
    public class ShokoSeriesExternalId : IExternalId
    {
        public bool Supports(IHasProviderIds item) => item is Season;

        public string Name { get; } = Constants.ShokoSeries;
        public string Key { get; } = "Shoko.Emby.Series";
        public string UrlFormatString { get; } = $"{Plugin.Instance.Configuration.Url}api/serie.json?id={0}";
    }

    public class ShokoGroupExternalId : IExternalId
    {
        public bool Supports(IHasProviderIds item) => item is Series;

        public string Name { get; } = Constants.ShokoGroup;
        public string Key { get; } = "Shoko.Emby.Group";
        public string UrlFormatString { get; } = $"{Plugin.Instance.Configuration.Url}api/group.json?id={0}";
    }

    public class ShokoEpisodeExternalId : IExternalId
    {
        public bool Supports(IHasProviderIds item) => item is Series;

        public string Name { get; } = Constants.ShokoEpisode;
        public string Key { get; } = "Shoko.Emby.Episode";
        public string UrlFormatString { get; } = $"{Plugin.Instance.Configuration.Url}api/ep.json?id={0}";
    }
}