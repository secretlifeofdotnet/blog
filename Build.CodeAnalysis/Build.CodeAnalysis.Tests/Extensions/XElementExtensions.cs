namespace Build.CodeAnalysis.Tests.Extensions
{
    using System.Xml.Linq;

    public static class XElementExtensions
    {
        public static bool Attribute(this XElement element, string name, string value)
        {
            return element.HasAttributes && element.Attribute(name).Value == value;
        }
    }
}