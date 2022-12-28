using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace System.Text.Errata
{
  public static class StringExtensions
  {
    public static string Remove(this string text, string toBeRemoved)
    {
      return text.Replace(toBeRemoved, "");
    }

    public static string RemoveRegex(this string text, Regex rx)
    {
      return rx.Replace(text, "");
    }

    public static bool HasValue(this string text)
    {
      return !string.IsNullOrWhiteSpace(text);
    }

    public static string Repeat(this string text, int repetitions)
    {
      if (repetitions > 1)
      {
        var sb = new StringBuilder(text);
        for (int i = 0; i < repetitions; i++)
        {
          sb.Append(text);
        }
        return sb.ToString();
      }

      return text;

    }


    public static List<string> Lines(this string text)
    {
      var lines = new List<string>();


      using (var sr = new StringReader(text))
      {
        string line;
        while ((line = sr.ReadLine()) != null)
          lines.Add(line);
      }
      return lines;
    }

    public static int LineCount(this string text)
    {
      var counter = 0;
      using (var sr = new StringReader(text))
      {
        while ((sr.ReadLine()) != null)
          counter++;
      }
      return counter;
    }

    public static string FirstToLower(this string text)
    {
      return text.Substring(0, 1).ToLower() + text.Substring((1));
    }

    public static string FirstToUpper(this string text)
    {
      return text.Substring(0, 1).ToUpper() + text.Substring((1));
    }


    private static HashSet<string> numericNames = new HashSet<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    public static string Value(this string text, Regex regex)
    {
      var match = regex.Match(text);
      return match.Value;
    }


    public static string GroupValue(this string text, Regex regex, string groupName)
    {
      var match = regex.Match(text);
      return match.Groups[groupName].Value;
    }


    public static string GroupValue(this string text, Regex regex, int index)
    {
      var match = regex.Match(text);
      return match.Groups[index].Value;
    }

    public static string GroupValue(this string text, Regex regex)
    {
      var match = regex.Match(text);
      var names = regex.GetGroupNames();

      foreach (var name in names)
      {
        if (numericNames.Contains(name))
          continue;
        int unused;
        if (int.TryParse(name, out unused))
          continue;

        return match.Groups[name].Value;
      }
      return match.Groups[0].Value;
    }


    public static bool IsMatch(this string text, Regex regex)
    {
      return regex.IsMatch(text);
    }


  }
}
