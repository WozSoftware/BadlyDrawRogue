using System.Xml;
using System.Xml.Linq;
using Functional.Maybe;

namespace Woz.Linq.Xml
{
    public static class ElementHelpers
    {
        public static XElement 
            ElementOrDefault(this XElement element, string name)
        {
            return element
                .MaybeElement(name)
                .OrElse(new XElement(name));
        }

        public static XElement 
            RequiredElement(this XElement element, string name)
        {
            return element
                .MaybeElement(name)
                .OrElse(
                    () => new XmlException(
                        string.Format(
                            "Element {0} missing from Element {1}",
                            name, element.Name)));
        }

        public static Maybe<XElement> 
            MaybeElement(this XElement element, string name)
        {
            return element.Element(name).ToMaybe();
        }
    }
}