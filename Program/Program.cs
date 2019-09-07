using System;
using System.Globalization;
using System.IO;
using JsonToRssGenerator.Data;
using Newtonsoft.Json;

namespace JsonToRssGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Примеры использования приложения:\n" +
                                  "JsonToRssGenerator.exe c:\\1.json c:\\1.xml\n" +
                                  "JsonToRssGenerator.exe c:\\1.json\n");
                return;
            }

            try
            {
                var inputFileName = args[0];
                var outputFileName = GetOutputFileName(inputFileName, args);

                using (var file = File.OpenText(inputFileName))
                {
                    var serializer = new JsonSerializer
                    {
                        Culture = new CultureInfo("EN-US")
                    };
                    var channelData = (ChannelData) serializer.Deserialize(file, typeof(ChannelData));
                    var result = new RssGenerator().Generate(channelData);
                    File.WriteAllText(outputFileName, result);
                }

                Console.WriteLine("Файл с RSS успешно создан");
            }
            catch (ValidationException exception)
            {
                Console.WriteLine("ОШИБКА");
                Console.WriteLine(exception.Message);
            }
        }

        private static string GetOutputFileName(string inputFileName, string[] args)
        {
            if (args.Length > 1)
                return args[1];

            var directory = Path.GetDirectoryName(inputFileName);
            if (directory.IsFilled())
                return directory + Path.DirectorySeparatorChar
                                 + Path.GetFileNameWithoutExtension(inputFileName)
                                 + ".xml";

            return Path.GetFileNameWithoutExtension(inputFileName)
                   + ".xml";
        }
    }
}