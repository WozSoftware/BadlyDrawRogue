using System.Xml;
using System.Xml.Linq;
using Functional.Maybe;

namespace Woz.Linq.Xml
{
    public static class AttributeHelpers
    {
        public static XAttribute 
            RequiredAttribute(this XElement element, string name)
        {
            return element
                .MaybeAttribute(name)
                .OrElse(
                    () => new XmlException(
                        string.Format(
                            "Attribute {0} missing from Element {1}",
                            name, element.Name)));
        }

        public static Maybe<XAttribute> 
            MaybeAttribute(this XElement element, string name)
        {
            return element
                .Attribute(name)
                .ToMaybe();
        }
    }
}