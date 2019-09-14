using System.IO;
using System.Text;

namespace JsonToRssGenerator
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => new UTF8Encoding(false);
    }
}