using System;

namespace JsonToRssGenerator.Data
{
    public class Episode
    {
        public string MP3FileUrl;
        public int MP3FileSizeBytes;

        public string Title;
        public string Subtitle;
        public string Id;
        public DateTime Date;
        public string Description;
        public bool Explicit;

        public string ImageUrl;

        public int DurationHours;
        public int DurationMinutes;
        public int DurationSeconds;

        public string Link;

        public int EpisodeNumber;
        public int SeasonNumber;
    }
}