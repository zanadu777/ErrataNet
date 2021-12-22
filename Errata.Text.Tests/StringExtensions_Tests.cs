using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Errata.Text.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        

        [Test]
        public void Test1()
        {
            var text = TestUtils.ReadTestData( "Books.xml") ;
            Debug.WriteLine(text);

           // Assert.Pass();
        }
    }
}