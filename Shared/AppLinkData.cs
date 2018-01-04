namespace Zebble
{
    public class AppLinkData
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public AppLinkData(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
