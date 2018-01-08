namespace Zebble
{
    using System.Collections.Generic;

    public class MetaAttribute
    {
        public MetaAttributeName Name { get; set; }
        public string Value { get; set; }

        public MetaAttribute(MetaAttributeName name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class MetaAttributeResult
    {
        public List<MetaAttribute> MetaAttributes { get; set; }
        public NavigationResult NavigationResult { get; set; }

        public MetaAttributeResult(List<MetaAttribute> metaAttributes, NavigationResult navigationResult)
        {
            MetaAttributes = metaAttributes;
            NavigationResult = navigationResult;
        }
    }

    public enum MetaAttributeName
    {
        Url = 0,
        Package = 1,
        AppName = 2,
        WebUrl = 3,
        WebFallBack = 4
    }
}
