using System.Text;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Errata;
using FluentAssertions;

namespace Errata.IO.Tests
{
  [TestFixture]
  public class FileInfoExtensionsTests
  {

    [Test]
    public void ReadAllTextTest()
    {
      var filePath = @"c:\temp2\tempfile.txt";
      var fileContents = @"test text";
      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");
      mockFileSystem.AddFile(filePath, fileContents);

      var fi = new FileInfo(filePath);
      var readText = fi.ReadAllText(mockFileSystem);
      readText.Should().Be(fileContents);
    }


    [Test]
    public void ReadAllLines()
    {
      var filePath = @"c:\temp2\tempfile.txt";
      var fileContents = new  []{ "Line 1", "Line 2" };
      var sb = new StringBuilder();
      foreach (var line in fileContents)
        sb.AppendLine(line);

      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");

      mockFileSystem.AddFile(filePath,sb.ToString());

      var fi = new FileInfo(filePath);
      var readLines = fi.ReadAllLines(mockFileSystem);
      readLines.Should().BeEquivalentTo(fileContents);
    }

    [Test]
    public void WriteAllBytesTests()
    {
      var binaryFilePath = @"c:\temp2\tempfile.bin";
      var binaryFileContents = new byte[] { 0xFF, 0x34, 0x56, 0xD2 };
     
      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");

      var fi = new FileInfo(binaryFilePath);

      mockFileSystem.FileExists(binaryFilePath).Should().BeFalse();
      fi.WriteAllBytes(binaryFileContents, mockFileSystem);

      mockFileSystem.FileExists(binaryFilePath).Should().BeTrue();
    }

    [Test]
    public void ReadAllBtyesTests()
    {
      var binaryFilePath = @"c:\temp2\tempfile.bin";
      var binaryFileContents = new byte[] { 0xFF, 0x34, 0x56, 0xD2 };
      var mockFileData = new MockFileData(binaryFileContents);

      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");
      mockFileSystem.AddFile(binaryFilePath, mockFileData);

      var fi = new FileInfo(binaryFilePath);

      fi.ReadAllBytes(mockFileSystem).Should().BeEquivalentTo(binaryFileContents);
    }

    [Test]
    public void WriteAllTextTest()
    {
      var filePath = @"c:\temp2\tempfile.txt";
      var fileContents = @"test text";

      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");
      var fi = new FileInfo(filePath);
      fi.WriteAllText(fileContents, mockFileSystem);

      mockFileSystem.FileExists(filePath).Should().BeTrue();
      mockFileSystem.GetFile(filePath).TextContents.Should().Be(fileContents);
    }


    [Test]
    public void WriteAllLinesTests()
    {
      var filePath = @"c:\temp2\tempfile.txt";
      var fileContents = new List<string>{"Line 1", "Line 1" };
      var sb = new StringBuilder();
      foreach (var line in fileContents)
        sb.AppendLine(line);

      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");

      var fi = new FileInfo(filePath);
      fi.WriteAllLines(fileContents, mockFileSystem);

      mockFileSystem.FileExists(filePath).Should().BeTrue();
      mockFileSystem.GetFile(filePath).TextContents.Should().Be(sb.ToString());
    }

    [Test]
    public void NameWithoutExtensionTests()
    {
      var filePath = @"c:\temp2\tempfile.txt";
      var fi = new FileInfo(filePath);
      fi.NameWithoutExtension().Should().Be("tempfile");
    }

    [Test]
    public void AppendLine()
    {
      var filePath = @"c:\temp2\tempfile.txt";

      var lines = new List<string> { "Line 1", "Line 2" };
      var sb = new StringBuilder();
      foreach (var line in lines)
        sb.AppendLine(line);

      var initialFileContents = sb.ToString();

      sb.AppendLine("Line 3");
      var endFileContents = sb.ToString();

      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");
      mockFileSystem.AddFile(filePath, initialFileContents);


      var fi = new FileInfo(filePath);
      fi.AppendLine("Line 3", mockFileSystem);
      mockFileSystem.GetFile(filePath).TextContents.Should().Be(endFileContents);
    }


    [Test]
    public void IncrementTests()
    {

      //does not end with a number 
      new FileInfo(@"c:\temp2\tempfile.txt").Increment().FullName.Should().Be(@"c:\temp2\tempfile2.txt");

      //ends with a number, but no padding 
      new FileInfo(@"c:\temp2\tempfile3.txt").Increment().FullName.Should().Be(@"c:\temp2\tempfile4.txt");
      new FileInfo(@"c:\temp2\tempfile99.txt").Increment().FullName.Should().Be(@"c:\temp2\tempfile100.txt");

      //ends with a padded number
      new FileInfo(@"c:\temp2\tempfile008.txt").Increment().FullName.Should().Be(@"c:\temp2\tempfile009.txt");
      new FileInfo(@"c:\temp2\tempfile009.txt").Increment().FullName.Should().Be(@"c:\temp2\tempfile010.txt");
      new FileInfo(@"c:\temp2\tempfile099.txt").Increment().FullName.Should().Be(@"c:\temp2\tempfile100.txt");
    }


    [Test]
    public void MoveToTests()
    { 
      
      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");
      mockFileSystem.AddDirectory(@"c:\temp3");
      var originalFilePath = @"c:\temp2\tempfile.txt";
      var destinationFilePath = @"c:\temp3\tempfile.txt";
      var destinationDirectoryPath = @"c:\temp3";
      var destinationDirectory = new DirectoryInfo(destinationDirectoryPath);
      var fileContents = @"test text";
      mockFileSystem.AddFile(originalFilePath, fileContents);

      var fileInfo = new FileInfo(originalFilePath);
      fileInfo.MoveTo(destinationDirectory,false, null, null,mockFileSystem);

      mockFileSystem.FileExists(destinationFilePath).Should().BeTrue();
      mockFileSystem.FileExists(originalFilePath).Should().BeFalse();
      mockFileSystem.GetFile(destinationFilePath).TextContents.Should().Be(fileContents);
    }



    [Test]
    public void CopyToTests()
    {

      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");
      mockFileSystem.AddDirectory(@"c:\temp3");
      var originalFilePath = @"c:\temp2\tempfile.txt";
      var destinationFilePath = @"c:\temp3\tempfile.txt";
      var destinationDirectoryPath = @"c:\temp3";
      var destinationDirectory = new DirectoryInfo(destinationDirectoryPath);
      var fileContents = @"test text";
      mockFileSystem.AddFile(originalFilePath, fileContents);

      var fileInfo = new FileInfo(originalFilePath);
      fileInfo.CopyTo(destinationDirectory, false, null, null, mockFileSystem);

      mockFileSystem.FileExists(destinationFilePath).Should().BeTrue();
      mockFileSystem.FileExists(originalFilePath).Should().BeTrue();
      mockFileSystem.GetFile(destinationFilePath).TextContents.Should().Be(fileContents);
    }

    [Test]
    public void RenameTests()
    {
      var mockFileSystem = new MockFileSystem();
      mockFileSystem.AddDirectory(@"c:\temp2");
      var originalFilePath = @"c:\temp2\tempfile.txt";

      var fileContents = @"test text";
      mockFileSystem.AddFile(originalFilePath, fileContents);

      var fi = new FileInfo(originalFilePath);

      var fi2 = fi.Rename("tempfile2", mockFileSystem);
      mockFileSystem.FileExists(@"c:\temp2\tempfile2.txt").Should().BeTrue();
      mockFileSystem.FileExists(originalFilePath).Should().BeFalse();


      var fi3 = fi2.Rename("tempfile3", ".text", mockFileSystem);
      mockFileSystem.FileExists(@"c:\temp2\tempfile2.txt").Should().BeFalse();
      mockFileSystem.FileExists(@"c:\temp2\tempfile3.text").Should().BeTrue();


     var fi4 =  fi3.ChangeExtension(".txt", mockFileSystem);
      mockFileSystem.FileExists(@"c:\temp2\tempfile3.txt").Should().BeTrue();

      fi4.Rename(n => n + "stuff", mockFileSystem);
      mockFileSystem.FileExists(@"c:\temp2\tempfile3stuff.txt").Should().BeTrue();

    }


    [Test]
    public void IsOfExtensionTests()
    {
      var fi = new FileInfo(@"c:\temp2\tempfile.txt");
      fi.IsOfExtension(".txt").Should().BeTrue();
    }

  }
}
