namespace Shoko.Jellyfin.Models
{
    public class Episode
    {
        public string type { get; set; }
        public string season { get; set; }
        public int view { get; set; }
        public string eptype { get; set; }
        public int epnumber { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
        public string year { get; set; }
        public string air { get; set; }
        public int size { get; set; }
        public string rating { get; set; }
        public string votes { get; set; }
        public Art art { get; set; }
    }
}