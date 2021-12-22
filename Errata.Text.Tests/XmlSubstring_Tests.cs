using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Errata.Text.Tests
{
    public class XmlSubstring_Tests
    {
        [Test]
        public void BookTest()
        {
            var book = TestUtils.ReadTestData("Books.xml");
            var sub = new XmlSubstring(book);
            sub.FilterElement("book").Should().BeTrue();
            sub.FilterElementValue("genre").Should().BeTrue();
            sub.Substring.Should().Be("Computer");

            sub.ClearAll();


            sub.FilterElement("book", XmlTests.AtrributeOf("id", "bk109")).Should().BeTrue();
            sub.FilterAttribute("id").Should().BeTrue();
            sub.Substring.Should().Be("bk109");

            sub.ClearAll();



            sub.FilterElement("book", XmlTests.ElementOf("genre", "Fantasy")).Should().BeTrue();
            sub.FilterAttribute("id").Should().BeTrue();
            sub.Substring.Should().Be("bk102");

            sub.ClearAll();


            sub.FilterElement("book", m => XmlTests.ElementOf("genre", "Fantasy")(m)
                                           && XmlTests.ElementOf("publish_date", "2000-11-17")(m)).Should().BeTrue();
            sub.FilterAttribute("id").Should().BeTrue();
            sub.Substring.Should().Be("bk103");


        }

        [Test]
        public void LikeInfragisticsTest()
        {
            var xml = TestUtils.ReadTestData("LikeInfragistics.xml");
            var sub = new XmlSubstring(xml);
            sub.FilterElement("fields").Should().BeTrue();
            sub.FilterElement("field", XmlTests.AtrributeOf("id", "2")).Should().BeTrue(); ;
            sub.FilterAttribute("name").Should().BeTrue();
            sub.Substring.Should().Be("field2");
        }
    }
}