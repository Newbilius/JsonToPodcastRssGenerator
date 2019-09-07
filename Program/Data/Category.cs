namespace JsonToRssGenerator.Data
{
    public class Category
    {
        public string Name { get; }
        public string SubCategory { get; }

        public Category(string name, string subCategory = null)
        {
            Name = name;
            SubCategory = subCategory;
        }
    }
}
