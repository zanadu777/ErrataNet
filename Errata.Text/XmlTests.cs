using System.Text.RegularExpressions;

namespace System.Text.Errata
{
    public static class XmlTests
    {
        public static Predicate<Match> AtrributeOf(string attributeName, string attributeValue)
        {

            Predicate<Match> pred = m =>
            {
                var rx = new Regex($@"\s{attributeName}\s*=\s*""{attributeValue}""(>|\s)");
                return rx.IsMatch(m.Value);
            };
            return pred;
        }


        public static Predicate<Match> ElementOf(string elementName, string elementValue)
        {
            Predicate<Match> pred = m =>
            {
                var rx = new Regex($@"<{elementName}.*?>(?<value>.*?)</{elementName}>");
               Match match = rx.Match(m.Value);
               if (!match.Success)
                   return false;

               return match.Groups["value"].Value == elementValue;
            };
            return pred;

        }
    }
}
