using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using Errata.Functional;
using FluentAssertions;

namespace Errata.Functional.Tests
{
  public class Tests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void PipelineTests()
    {
      Func<string, string> fi1 = x => x + " Suffix";
      Func<string, string> fi2 = x => "Prefix " + x  ;

      var pipeline = (new [] {fi1,fi2}).Pipeline();
      ;
     var pipelined =  pipeline("test");
     var composed = fi2(fi1("test"));

     pipelined.Should().Be(composed);

    }
  }
}