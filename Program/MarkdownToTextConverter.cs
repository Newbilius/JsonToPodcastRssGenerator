using System.Text.RegularExpressions;

namespace JsonToRssGenerator
{
    //на основе https://gist.github.com/dennisslimmers/4b63db37e640d74acb29d4e1f24e9acd

    public static class MarkdownToTextConverter
    {
        public static string Convert(string content)
        {
            //удаляем части оформления
            content = StrikethroughRegex.Replace(content, "");
            content = StrikethroughRegex.Replace(content, "");
            content = CodeblocksRegex.Replace(content, "");
            content = HTMLTagsRegex.Replace(content, "");
            content = Footnotes1Regex.Replace(content, "");
            content = Footnotes2Regex.Replace(content, "");
            content = ImagesRegex.Replace(content, "");

            //заменяем ссылки на сочетание "текст ссылки (урл ссылки)"
            content = LinksRegex.Replace(content, "$1 ($2)");

            //заменяем вложеные заголовки на одиночные
            while (content.Contains("##"))
                content = content.Replace("##", "#");

            //заменяем жирный/курсив на идентичное выделение
            while (content.Contains("**"))
                content = content.Replace("**", "*");

            return content;
        }

        private static readonly Regex
            LinksRegex = new Regex(@"\[([^]]*)\]\(([^\s^\)]*)[\s\)]", RegexOptions.Compiled);

        private static readonly Regex
            StrikethroughRegex = new Regex(@"/~~/g", RegexOptions.Compiled);

        private static readonly Regex
            CodeblocksRegex = new Regex(@"/`{3}.*\n/g", RegexOptions.Compiled);

        private static readonly Regex
            HTMLTagsRegex = new Regex(@"/<[^>]*>/g", RegexOptions.Compiled);

        private static readonly Regex
            Footnotes1Regex = new Regex(@"/\[\^.+?\](\: .*?$)?/g", RegexOptions.Compiled);

        private static readonly Regex
            Footnotes2Regex = new Regex(@"/\s{0,2}\[.*?\]: .*?$/g", RegexOptions.Compiled);

        private static readonly Regex
            ImagesRegex = new Regex(@"/\!\[.*?\][\[\(].*?[\]\)]/g", RegexOptions.Compiled);
    }
}