using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Errata;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Errata.Text.Tests
{
    public class TextSubstring_Tests
    {
        [SetUp]
        public void Setup()
        {

        }



        [Test]
        public void CreationTests()
        {
            var sub = new TextSubstring("Test");
            sub.Text.Should().Be("Test");
            sub.Start.Should().Be(0);
            sub.Length.Should().Be(4);
            sub.Substring.Should().Be("Test");

        }


        [Test]
        public void FilterRegexTests()
        {

            var sub = new TextSubstring("Test123Test");
            sub.RegexFilter(new Regex(@"\d+"));
            sub.Substring.Should().Be("123");

        }



        [Test]
        public void ReplaceTests()
        {
            var sub = new TextSubstring("Test123Test");
            sub.RegexFilter(new Regex(@"\d+"));
            sub.Substring = "123";
            sub.Substring.Should().Be("123");
            sub.Text.Should().Be("Test123Test");

            sub.ClearAll();

            sub.RegexFilter(new Regex(@"\d+"));
            sub.Substring = "1234";
            sub.Substring.Should().Be("1234");
            sub.Text.Should().Be("Test1234Test");

            sub.ClearAll();
            sub.RegexFilter(new Regex(@"\d+"));
            sub.Substring = "12";
            sub.Substring.Should().Be("12");
            sub.Text.Should().Be("Test12Test");

        }

        [Test]
        public void BookTest()
        {
            var book = TestUtils.ReadTestData("Books.xml");
            var sub = new TextSubstring(book);
            sub.RegexFilter(new Regex(@"<book.*?</book>", RegexOptions.Singleline));

            sub.RegexFilter(new Regex(@"<price>(?<p>.*?)</price>"), "p");

            sub.Substring.Should().Be("44.95");

            sub.ClearAll();

            var regex = new Regex(
                @"<book.*?<price>(?<price>.*?)</price>.*?<publish_date>(?<publish>.*?)</publish_date>.*?</book>",
                RegexOptions.Singleline);

            sub.RegexFilter(regex, "price" ,m=> m.Groups["publish"].Value == "2000-09-02");

            sub.Substring.Should().Be("4.95");



        }

        [Test]
        public void ClearLastTest()
        {

            var book = TestUtils.ReadTestData("Books.xml");
            var sub = new TextSubstring(book);
            sub.RegexFilter(new Regex(@"<book.*?</book>", RegexOptions.Singleline));

            sub.RegexFilter(new Regex(@"<price>(?<p>.*?)</price>"), "p");

            

            sub.ClearLast(1);

            sub.RegexFilter(new Regex(@"<genre>(?<p>.*?)</genre>"), "p");

            sub.Substring.Should().Be("Computer");

        }


        [Test]
        public void FilterPosTexts()
        {
            var book = TestUtils.ReadTestData("Books.xml");
            var sub = new TextSubstring(book);
            sub.RegexFilter(new Regex(@"<book.*?</book>", RegexOptions.Singleline),2);
            sub.RegexFilter(new Regex(@"<price>(?<p>.*?)</price>"), "p");
            sub.Substring.Should().Be("5.95");
        }


      
      
    }
}
