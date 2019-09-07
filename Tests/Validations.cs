using System;
using NUnit.Framework;
using JsonToRssGenerator;
using JsonToRssGenerator.Data;
using Shouldly;

namespace Tests
{
    public class Validations
    {
        //todo проверка всех валидаций по отдельности

        [Test]
        public void ValidationDataAll()
        {
            var data = new ChannelData();

            ShouldBeExceptionWithMessage(
                () => new RssGenerator().Generate(data),
                @"Не указано название подкаста
Не указан email автора
Не указано имя автора
Не указана ссылка на обложку подкаста
Не указано описание подкаста
Не указан язык подкаста
Не указаны категории подкаста
Нет ни одного эпизода
");
        }

        [Test]
        public void ValidationsItemDataAll()
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
                        Description = "tmp",
                        DurationMinutes = 10,
                        Id = "tmp",
                        Date = DateTime.Now,
                        EpisodeNumber = 1
                    },
                    new Episode()
                }
            };

            ShouldBeExceptionWithMessage(
                () => new RssGenerator().Generate(data),
                @"Не указано название эпизода № 2
Не указана ссылка на MP3 в эпизоде № 2
Не указан размер MP3-файла в эпизоде № 2
Не указано описание эпизода № 2
Не указана продолжительность эпизода № 2
Не указан уникальный ID эпизода № 2
Не указана дата эпизода № 2
Не указан номер эпизода № 2
");
        }

        private static void ShouldBeExceptionWithMessage(Action action,
            string message)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                var exception = e as ValidationException;
                if (exception == null)
                    Assert.Fail("Не произошло экспешена");
                exception.Message.ShouldBe(message);
            }
        }
    }
}