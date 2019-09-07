using System;
using NUnit.Framework;
using JsonToRssGenerator;
using JsonToRssGenerator.Data;
using Shouldly;

namespace Tests
{
    //todo добавить проверку на удаление всех лишних MD-тегов

    public class Content
    {
        [Test]
        public void HtmlDescription()
        {
            var data = new ChannelData
            {
                Title = "tmp",
                AuthorEmail = "tmp",
                AuthorName = "tmp",
                ImageUrl = "tmp",
                Description = "tmp",
                Language = "tmp",
                Categories = new[]
                {
                    new Category("tmp")
                },
                Episodes = new[]
                {
                    new Episode
                    {
                        Title = "tmp",
                        MP3FileUrl = "tmp",
                        MP3FileSizeBytes = 1,
                        Description = @"Здесь будет довольно длинное описание этого эпизода

# Заголовок первого уровня

## Заголовок второго уровня

### Заголовок третьего уровня

#### Заголовок четвёртого уровня

##### Заголовок пятого уровня

##### Заголовок шестого уровня

Тут в тексте будет [ссылка на файл](http://www.hosting.ru/file.zip) где-то в интернетах.

А ещё не забудем про **жирный** и *курсив*, ага?",
                        DurationMinutes = 10,
                        Id = "tmp",
                        Date = new DateTime(2019, 05, 12),
                        EpisodeNumber = 1
                    }
                }
            };

            var result = new RssGenerator().Generate(data);

            result.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-16""?>
<rss xmlns:itunes=""http://www.itunes.com/dtds/podcast-1.0.dtd"" xmlns:content=""http://purl.org/rss/1.0/modules/content/""
	version=""2.0"">
	<channel>
		<title>tmp</title>
		<description>tmp</description>
		<itunes:summary>tmp</itunes:summary>
		<language>tmp</language>
		<itunes:image
			href=""tmp"" />
		<itunes:owner>
			<itunes:name>tmp</itunes:name>
			<itunes:email>tmp</itunes:email>
		</itunes:owner>
		<itunes:explicit>No</itunes:explicit>
		<itunes:author>tmp</itunes:author>
		<link />
		<itunes:category
			text=""tmp"" />
		<item>
			<title>tmp</title>
			<itunes:title>tmp</itunes:title>
			<itunes:author>tmp</itunes:author>
			<pubDate>Sun, 12 May 2019 00:00:00 GMT</pubDate>
			<itunes:duration>10:</itunes:duration>
			<itunes:episode>1</itunes:episode>
			<guid>tmp</guid>
			<itunes:explicit>No</itunes:explicit>
			<enclosure
				url=""tmp""
				length=""1""
				type=""audio/mpeg"" />
			<description>Здесь будет довольно длинное описание этого эпизода

# Заголовок первого уровня

# Заголовок второго уровня

# Заголовок третьего уровня

# Заголовок четвёртого уровня

# Заголовок пятого уровня

# Заголовок шестого уровня

Тут в тексте будет ссылка на файл (http://www.hosting.ru/file.zip) где-то в интернетах.

А ещё не забудем про *жирный* и *курсив*, ага?</description>
			<content:encoded><![CDATA[<p>Здесь будет довольно длинное описание этого эпизода</p>

<br><h1>Заголовок первого уровня</h1>

<br><h2>Заголовок второго уровня</h2>

<br><h3>Заголовок третьего уровня</h3>

<br><h4>Заголовок четвёртого уровня</h4>

<br><h5>Заголовок пятого уровня</h5>

<br><h5>Заголовок шестого уровня</h5>

<p>Тут в тексте будет <a href=""http://www.hosting.ru/file.zip"">ссылка на файл</a> где-то в интернетах.</p>

<p>А ещё не забудем про <strong>жирный</strong> и <em>курсив</em>, ага?</p>]]></content:encoded>
		</item>
	</channel>
</rss>");
        }

        [Test]
        public void OnlyRequiredData()
        {
            var data = new ChannelData
            {
                Title = "Заголовок подкаста",
                AuthorEmail = "Имя автора",
                AuthorName = "email@email.ru",
                ImageUrl = "http://www.test.ru/image.jpg",
                Description = "Краткое описание",
                Language = "RU",
                Categories = new[]
                {
                    new Category("категория 1"),
                },
                Episodes = new[]
                {
                    new Episode
                    {
                        Title = "Эпизод первый",
                        Description = "Краткое описание первой серии",
                        DurationMinutes = 10,
                        Date = new DateTime(2019, 05, 06),
                        Id = "E1",
                        EpisodeNumber = 1,
                        MP3FileSizeBytes = 23456,
                        MP3FileUrl = "http://www.test.ru/e1.mp3",
                    },
                    new Episode
                    {
                        Title = "Эпизод внезапно третий сезона",
                        Description = "Краткое описание третьей сериисерии",
                        DurationMinutes = 10,
                        DurationHours = 2,
                        Date = new DateTime(2019, 05, 07),
                        Id = "E3",
                        EpisodeNumber = 3,
                        MP3FileSizeBytes = 7777,
                        MP3FileUrl = "http://www.test.ru/e2.mp3",
                    }
                }
            };

            var result = new RssGenerator().Generate(data);

            result.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-16""?>
<rss xmlns:itunes=""http://www.itunes.com/dtds/podcast-1.0.dtd"" xmlns:content=""http://purl.org/rss/1.0/modules/content/""
	version=""2.0"">
	<channel>
		<title>Заголовок подкаста</title>
		<description>Краткое описание</description>
		<itunes:summary>Краткое описание</itunes:summary>
		<language>RU</language>
		<itunes:image
			href=""http://www.test.ru/image.jpg"" />
		<itunes:owner>
			<itunes:name>email@email.ru</itunes:name>
			<itunes:email>Имя автора</itunes:email>
		</itunes:owner>
		<itunes:explicit>No</itunes:explicit>
		<itunes:author>email@email.ru</itunes:author>
		<link />
		<itunes:category
			text=""категория 1"" />
		<item>
			<title>Эпизод первый</title>
			<itunes:title>Эпизод первый</itunes:title>
			<itunes:author>email@email.ru</itunes:author>
			<pubDate>Mon, 06 May 2019 00:00:00 GMT</pubDate>
			<itunes:duration>10:</itunes:duration>
			<itunes:episode>1</itunes:episode>
			<guid>E1</guid>
			<itunes:explicit>No</itunes:explicit>
			<enclosure
				url=""http://www.test.ru/e1.mp3""
				length=""23456""
				type=""audio/mpeg"" />
			<description>Краткое описание первой серии</description>
			<content:encoded><![CDATA[<p>Краткое описание первой серии</p>]]></content:encoded>
		</item>
		<item>
			<title>Эпизод внезапно третий сезона</title>
			<itunes:title>Эпизод внезапно третий сезона</itunes:title>
			<itunes:author>email@email.ru</itunes:author>
			<pubDate>Tue, 07 May 2019 00:00:00 GMT</pubDate>
			<itunes:duration>2:10:</itunes:duration>
			<itunes:episode>3</itunes:episode>
			<guid>E3</guid>
			<itunes:explicit>No</itunes:explicit>
			<enclosure
				url=""http://www.test.ru/e2.mp3""
				length=""7777""
				type=""audio/mpeg"" />
			<description>Краткое описание третьей сериисерии</description>
			<content:encoded><![CDATA[<p>Краткое описание третьей сериисерии</p>]]></content:encoded>
		</item>
	</channel>
</rss>");
        }

        [Test]
        public void Simple()
        {
            var data = new ChannelData
            {
                Title = "Название",
                Subtitle = "Подзаголовок",
                AuthorEmail = "email@email.ru",
                AuthorName = "Автор",
                ImageUrl = "http://www.test.ru/image.jpg",
                Description = "Краткое описание",
                Language = "RU",
                Categories = new[]
                {
                    new Category("категория 1", "подкатегория 1"),
                    new Category("Категория 2"),
                },
                Explicit = true,
                Link = "http://www.test.ru",
                Episodes = new[]
                {
                    new Episode
                    {
                        Title = "Эпизод первый",
                        Subtitle = "Подзаголовок первого эпизода",
                        Link = "http://www.test.ru/episodes/1",
                        Description = "Краткое описание первой серии",
                        DurationHours = 1,
                        DurationMinutes = 10,
                        DurationSeconds = 25,
                        Date = new DateTime(2019, 05, 06),
                        Id = "E1",
                        ImageUrl = "http://www.test.ru/image_e1.jpg",
                        Explicit = true,
                        EpisodeNumber = 1,
                        SeasonNumber = 2,
                        MP3FileSizeBytes = 23456,
                        MP3FileUrl = "http://www.test.ru/e1.mp3",
                    }
                }
            };

            var result = new RssGenerator().Generate(data);

            result.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-16""?>
<rss xmlns:itunes=""http://www.itunes.com/dtds/podcast-1.0.dtd"" xmlns:content=""http://purl.org/rss/1.0/modules/content/""
	version=""2.0"">
	<channel>
		<title>Название</title>
		<itunes:subtitle>Подзаголовок</itunes:subtitle>
		<description>Краткое описание</description>
		<itunes:summary>Краткое описание</itunes:summary>
		<language>RU</language>
		<itunes:image
			href=""http://www.test.ru/image.jpg"" />
		<itunes:owner>
			<itunes:name>Автор</itunes:name>
			<itunes:email>email@email.ru</itunes:email>
		</itunes:owner>
		<itunes:explicit>Yes</itunes:explicit>
		<itunes:author>Автор</itunes:author>
		<link>http://www.test.ru</link>
		<itunes:category
			text=""категория 1"">
			<itunes:category
				text=""подкатегория 1"" />
		</itunes:category>
		<itunes:category
			text=""Категория 2"" />
		<item>
			<title>Эпизод первый</title>
			<itunes:title>Эпизод первый</itunes:title>
			<subtitle>Подзаголовок первого эпизода</subtitle>
			<itunes:subtitle>Подзаголовок первого эпизода</itunes:subtitle>
			<itunes:author>Автор</itunes:author>
			<itunes:image
				href=""http://www.test.ru/image_e1.jpg"" />
			<link>http://www.test.ru/episodes/1</link>
			<pubDate>Mon, 06 May 2019 00:00:00 GMT</pubDate>
			<itunes:duration>1:10:25</itunes:duration>
			<itunes:episode>1</itunes:episode>
			<itunes:season>2</itunes:season>
			<guid>E1</guid>
			<itunes:explicit>Yes</itunes:explicit>
			<enclosure
				url=""http://www.test.ru/e1.mp3""
				length=""23456""
				type=""audio/mpeg"" />
			<description>Краткое описание первой серии</description>
			<content:encoded><![CDATA[<p>Краткое описание первой серии</p>]]></content:encoded>
		</item>
	</channel>
</rss>");
        }
    }
}