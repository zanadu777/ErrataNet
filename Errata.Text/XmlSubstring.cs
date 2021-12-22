using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Errata.Text
{
    public  class XmlSubstring:TextSubstring
    {
         
        public XmlSubstring(string text) :base(text)
        {
             
        }

        public bool FilterElement(string element)
        {
            var rxElement = new Regex($@"<{element}.*?</{element}>", RegexOptions.Singleline);
            return base.Filter(rxElement);
        }

        public bool FilterElement(string element, Predicate<Match> matchChooser)
        {
            var rxElement = new Regex($@"<{element}.*?>(?<value>.*?)</{element}>", RegexOptions.Singleline);
            return base.Filter(rxElement,matchChooser);
        }


        public bool FilterElementValue(string element)
        {
            var rxElement = new Regex($@"<{element}.*?>(?<value>.*?)</{element}>", RegexOptions.Singleline);
            return base.Filter(rxElement, "value");
        }





        public bool FilterAttribute(string attribute)
        {
            var rxElement = new Regex($@"\s{attribute}=""(?<value>.*?)""(>|\s)", RegexOptions.Singleline);
            return base.Filter(rxElement, "value");
        }
    }
}
