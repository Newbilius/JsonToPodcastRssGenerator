using HeyRed.MarkdownSharp;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using JsonToRssGenerator.Data;
using static System.String;

namespace JsonToRssGenerator
{
    public class RssGenerator
    {
        private const string ItunesUri = "http://www.itunes.com/dtds/podcast-1.0.dtd";
        private const string ContentUri = "http://purl.org/rss/1.0/modules/content/";

        private readonly XmlWriterSettings xmlWriterSettings;
        private XmlWriter writer;
        private readonly Markdown markdown;

        public RssGenerator()
        {
            markdown = new Markdown();
            xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = true
            };
        }

        public string Generate(ChannelData channel)
        {
            Validate(channel);

            using (var stringWriter = new Utf8StringWriter())
            {
                using (writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    writer.WriteStartDocument();
                    WriteBegin("rss");
                    writer.WriteAttributeString("xmlns", "itunes", null, "http://www.itunes.com/dtds/podcast-1.0.dtd");
                    writer.WriteAttributeString("xmlns", "content", null, "http://purl.org/rss/1.0/modules/content/");
                    writer.WriteAttributeString("version", null, "2.0");

                    //channel (START)
                    WriteBegin("channel");
                    WriteChannelInformation(channel);

                    foreach (var episode in channel.Episodes)
                        WriteEpisode(episode, channel);

                    WriteEnd();
                    //channel (END)

                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                }

                return stringWriter.ToString();
            }
        }

        private void Validate(ChannelData channel)
        {
            var errors = new StringBuilder();

            if (IsNullOrEmpty(channel.Title))
                errors.AppendLine("Не указано название подкаста");
            if (IsNullOrEmpty(channel.AuthorEmail))
                errors.AppendLine("Не указан email автора");
            if (IsNullOrEmpty(channel.AuthorName))
                errors.AppendLine("Не указано имя автора");
            if (IsNullOrEmpty(channel.ImageUrl))
                errors.AppendLine("Не указана ссылка на обложку подкаста");
            if (IsNullOrEmpty(channel.Description))
                errors.AppendLine("Не указано описание подкаста");
            if (IsNullOrEmpty(channel.Language))
                errors.AppendLine("Не указан язык подкаста");
            if (channel.Categories.IsEmpty())
                errors.AppendLine("Не указаны категории подкаста");

            var index = 0;
            if (channel.Episodes.IsEmpty())
                errors.AppendLine("Нет ни одного эпизода");
            else
                foreach (var episode in channel.Episodes)
                {
                    index++;
                    if (IsNullOrEmpty(episode.Title))
                        errors.AppendLine($"Не указано название эпизода № {index}");
                    if (IsNullOrEmpty(episode.MP3FileUrl))
                        errors.AppendLine($"Не указана ссылка на MP3 в эпизоде № {index}");
                    if (episode.MP3FileSizeBytes < 1)
                        errors.AppendLine($"Не указан размер MP3-файла в эпизоде № {index}");
                    if (IsNullOrEmpty(episode.Description))
                        errors.AppendLine($"Не указано описание эпизода № {index}");
                    if (episode.DurationHours < 1
                        && episode.DurationMinutes < 1)
                        errors.AppendLine($"Не указана продолжительность эпизода № {index}");
                    if (IsNullOrEmpty(episode.Id))
                        errors.AppendLine($"Не указан уникальный ID эпизода № {index}");
                    if (episode.Date.Year < 1990)
                        errors.AppendLine($"Не указана дата эпизода № {index}");
                    if (episode.EpisodeNumber < 1)
                        errors.AppendLine($"Не указан номер эпизода № {index}");
                }

            var errorMessage = errors.ToString();
            if (errorMessage.IsFilled())
                throw new ValidationException(errorMessage);
        }

        private void WriteEpisode(Episode episode,
            ChannelData channel)
        {
            WriteBegin("item");

            WriteValue("title", episode.Title);
            WriteItunesValue("title", episode.Title);

            if (episode.Subtitle.IsFilled())
            {
                WriteValue("subtitle", episode.Subtitle);
                WriteItunesValue("subtitle", episode.Subtitle);
            }

            WriteItunesValue("author", channel.AuthorName);

            if (episode.ImageUrl.IsFilled())
                WriteImage(episode.ImageUrl);

            if (episode.Link.IsFilled())
                WriteValue("link", episode.Link);

            WriteValue("pubDate", episode.Date.ToStringRFC2822());

            WriteItunesValue("duration",
                $"{(episode.DurationHours > 0 ? episode.DurationHours + ":" : "")}" +
                $"{(episode.DurationMinutes > 0 ? episode.DurationMinutes + ":" : "")}" +
                $"{(episode.DurationSeconds > 0 ? episode.DurationSeconds.ToString() : "")}");

            WriteItunesValue("episode", episode.EpisodeNumber.ToString());
            if (episode.SeasonNumber > 0)
                WriteItunesValue("season", episode.SeasonNumber.ToString());

            WriteValue("guid", episode.Id);
            WriteItunesValue("explicit", episode.Explicit ? "Yes" : "No");

            // файл
            WriteBegin("enclosure");
            writer.WriteAttributeString("url", episode.MP3FileUrl);
            writer.WriteAttributeString("length", episode.MP3FileSizeBytes.ToString());
            writer.WriteAttributeString("type", "audio/mpeg");
            WriteEnd();
            //

            //дополнительные отступы для iTunes
            var description = episode.Description + (channel.EpisodeDescriptionFooter ?? "");
            var descriptionHtml = markdown.Transform(description);
            descriptionHtml = descriptionHtml
                .Replace("<h1>", "<br><h1>")
                .Replace("<h2>", "<br><h2>")
                .Replace("<h3>", "<br><h3>")
                .Replace("<h4>", "<br><h4>")
                .Replace("<h5>", "<br><h5>")
                .Replace("<h6>", "<br><h6>");

            //удаляем сохраняем только часть оформления
            var descriptionText = MarkdownToTextConverter.Convert(description);

            //текстовое описание
            WriteValue("description", descriptionText);

            //HTML-описание
            WriteBegin("encoded", ContentUri);
            writer.WriteCData(descriptionHtml);
            WriteEnd();

            WriteEnd(); //item
        }

        private void WriteChannelInformation(ChannelData channel)
        {
            WriteValue("title", channel.Title);

            if (channel.Subtitle.IsFilled())
                WriteItunesValue("subtitle", channel.Subtitle);

            WriteValue("description", channel.Description);
            WriteItunesValue("summary", channel.Description);

            WriteValue("language", channel.Language);

            WriteImage(channel.ImageUrl);

            WriteBegin("owner", ItunesUri);
            writer.WriteElementString("name", ItunesUri, channel.AuthorName);
            writer.WriteElementString("email", ItunesUri, channel.AuthorEmail);
            WriteEnd();

            WriteItunesValue("explicit", channel.Explicit ? "Yes" : "No");
            WriteItunesValue("author", channel.AuthorName);
            WriteValue("link", channel.Link);

            foreach (var category in channel.Categories)
            {
                WriteBegin("category", ItunesUri);
                writer.WriteAttributeString("text", category.Name);

                if (category.SubCategory.IsFilled())
                {
                    WriteElementWithAttribute("category",
                        "text",
                        category.SubCategory);
                }

                WriteEnd();
            }
        }

        private void WriteBegin(string value, string ns = null)
        {
            if (ns != null)
                writer.WriteStartElement(value, ns);
            else
                writer.WriteStartElement(value);
        }

        private void WriteEnd()
        {
            writer.WriteEndElement();
        }

        private void WriteElementWithAttribute(string element,
            string attributeName,
            string value)
        {
            WriteBegin(element, ItunesUri);
            writer.WriteAttributeString(attributeName, value);
            WriteEnd();
        }

        private void WriteImage(string href)
        {
            WriteElementWithAttribute("image", "href", href);
        }

        private void WriteValue(string name, string value)
        {
            WriteBegin(name);
            writer.WriteValue(value);
            WriteEnd();
        }

        private void WriteItunesValue(string elementName,
            string value)
        {
            writer.WriteElementString(elementName, ItunesUri, value);
        }
    }
}