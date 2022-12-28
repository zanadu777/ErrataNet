using System.Text.RegularExpressions;

namespace System.Text.Errata
{
    public  class XmlSubstring:TextSubstring
    {
         
        public XmlSubstring(string text) :base(text)
        {
             
        }

        public bool FilterElement(string element)
        {
            var rxElement = new Regex($@"<{element}(>|\s.*?>).*?</{element}>", RegexOptions.Singleline);
            return base.RegexFilter(rxElement);
        }

        public bool FilterElement(string element, Predicate<Match> matchChooser)
        {
            var rxElement = new Regex($@"(<{element}(>|\s.*?>).*?>(?<value>.*?)</{element}>|<{element}\s+.*?/>)", RegexOptions.Singleline);
            return base.RegexFilter(rxElement,matchChooser);
        }


        public bool FilterElementValue(string element)
        {
            var rxElement = new Regex($@"<{element}(>|\s.*?>)(?<value>.*?)</{element}>", RegexOptions.Singleline);
            return base.RegexFilter(rxElement, "value");
        }





        public bool FilterAttribute(string attribute)
        {
            var rxElement = new Regex($@"\s{attribute}=""(?<value>.*?)""(>|\s|/>)", RegexOptions.Singleline);
            return base.RegexFilter(rxElement, "value");
        }
    }
}
