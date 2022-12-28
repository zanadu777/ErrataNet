using System.IO.Abstractions.TestingHelpers;
using System.IO.Errata;
using Errata.Text.Tests;
using FluentAssertions;

namespace Errata.IO.Tests
{
  public class DirectoryInfoExtensionsTests
  {
    [SetUp]
    public void Setup()
    {
    }

     

    [Test]
    public void CreateSubdirectoryTests()
    {
      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");

      var dir = new DirectoryInfo(@"c:\temp2");
      dir.CreateSubdirectory("test", mockFileSystem);

      mockFileSystem.FileExists(@"c:\temp2\test");
    }


    [Test]
    public void IsEmptyTest()
    {
      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\empty");
      mockFileSystem.AddDirectory(@"c:\emptyWithSub\empty");
      mockFileSystem.AddDirectory(@"c:\notEmpty");
      mockFileSystem.AddDirectory(@"c:\empty2");
      mockFileSystem.AddDirectory(@"c:\notEmptyWithSub\notEmpty");
      mockFileSystem.AddFile(@"c:\notEmpty\normalFile.txt", "stuff");
      mockFileSystem.AddFile(@"c:\empty2\thumbs.db", "stuff");

      DirectoryInfo dir = new DirectoryInfo(@"c:\empty");
      dir.IsEmpty(mockFileSystem).Should().BeTrue();

      dir = new DirectoryInfo(@"c:\notEmpty");
      dir.IsEmpty(mockFileSystem).Should().BeFalse();

      dir = new DirectoryInfo(@"c:\empty2");
      dir.IsEmpty(mockFileSystem).Should().BeTrue();
    }

    [Test]
    public void EmptyDirectoriesTest()
    {
      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\empty");
      mockFileSystem.AddDirectory(@"c:\emptyWithSub\empty");
      mockFileSystem.AddDirectory(@"c:\notEmpty");
      mockFileSystem.AddDirectory(@"c:\empty2");
      mockFileSystem.AddDirectory(@"c:\notEmptyWithSub\notEmpty");
      mockFileSystem.AddFile(@"c:\notEmpty\normalFile.txt", "stuff");
      mockFileSystem.AddFile(@"c:\empty2\thumbs.db", "stuff");

      var dir = new DirectoryInfo(@"C:\");

      var emptyDirs = dir.EmptyDirectories(mockFileSystem);
      emptyDirs.Count.Should().Be(5);
    }
  }
}
