using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Errata.Text
{
    public  class TextSubstring
    {
        public TextSubstring(string text)
        {
            this.text = text;
         
          

            Start = 0;
            Length = text.Length;
        }

        public TextSubstring(string text, int start, int length)
        {
            this.text = text;
            Start = start;
            Length = length;
        }


        private string text;
        private readonly List<int> start = new();
        private readonly List<int> length= new();

        public string Text => text;

        public string Substring
        {
            get
            {
                return text.Substring(Start,Length);
            }

            set
            {
                var updated = text.Substring(0, Start) + value + Text.Substring(Start + Length );
                text = updated;
                Length = value.Length;
            }
        }


        public int Start
        {
            get => start.Last(); 
            set =>   start.Add(value);
        }

        public int Length
        {
            get => length.Last();
            set => length.Add(value) ;
        }

        public void ClearAll()
        {   start.Clear();
            length.Clear();

            Start = 0;
            Length = text.Length;
        }


        public void ClearLast(int numberToClear)
        {
            if (numberToClear < 0)
                return;

            if (start.Count == 1)
                return;

            if (numberToClear > length.Count)
                ClearAll();

            start.RemoveRange( start.Count - numberToClear , numberToClear);
            length.RemoveRange(length.Count - numberToClear , numberToClear);


        }


        public bool Filter(Regex regex)
        {
            var match = regex.Match(Substring);
            if (match.Success)
            {
                Start += match.Index;
                Length=  match.Length;
                return true;
            }
            return false;
        }


        public bool Filter(Regex regex,  Predicate<Match> matchChooser)
        {
            var matches = regex.Matches(Substring);

            if (!matches.Any())
                return false;

            foreach (Match match in matches)
            {
                if (!matchChooser(match))
                    continue;

                Start += match.Index;
                Length = match.Length;
                return true;
            }

            return false;

        }



        public bool Filter(Regex regex, int pos)
        {
            var matches = regex.Matches(Substring);

            if (!matches.Any())
                return false;

            if (matches.Count < pos)
                return false;


            Start +=  matches[pos].Index;
            Length = matches[pos].Length;
                
            

            return false;

        }

        public bool Filter(Regex regex, string group)
        {
            var match = regex.Match(Substring);
            if (match.Success)
            {
               
                Start += match.Groups[group].Index;
                Length = match.Groups[group].Length;
                return true;
            }
            return false;
        }


        public bool Filter(Regex regex, string group , Predicate<Match> matchChooser)
        {
            var matches = regex.Matches(Substring);

            if (!matches.Any()) 
                return false;

            foreach (Match match in matches)
            {
                if (!matchChooser(match)) 
                    continue;

                Start += match.Groups[@group].Index;
                Length = match.Groups[@group].Length;
                return true;
            }

            return false;
          
        }


    }
}
