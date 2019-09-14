namespace JsonToRssGenerator.Data
{
    public class ChannelData
    {
        public string Title;
        public string Link;
        public string Subtitle;
        public string Language;

        public string AuthorEmail;
        public string AuthorName;

        public bool Explicit;

        public string ImageUrl;

        public string Description;
        public string EpisodeDescriptionFooter;

        public Category[] Categories;
        public Episode[] Episodes;
    }
}