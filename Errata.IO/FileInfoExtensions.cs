using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO.Abstractions;
using System.Reflection;

namespace System.IO.Errata
{
  public static partial class FileInfoExtensions
  {
    static FileInfoExtensions()
    {
      concreteFileSystem = new FileSystem();
    }

    private static IFileSystem concreteFileSystem;

    public static Byte[] ReadAllBytes(this FileInfo fileInfo, IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;
      return fileSystem.File.ReadAllBytes(fileInfo.FullName);
    }

    public static string ReadAllText(this FileInfo fileInfo, IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;
      return fileSystem.File.ReadAllText(fileInfo.FullName);
    }

    public static string[] ReadAllLines(this FileInfo fileInfo, IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;
      return fileSystem.File.ReadAllLines(fileInfo.FullName);
    }


    public static FileInfo WriteAllBytes(this FileInfo fileInfo, Byte[] bytes, IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;
      fileSystem.File.WriteAllBytes(fileInfo.FullName, bytes);
      return fileInfo;
    }

    public static FileInfo WriteAllText(this FileInfo fileInfo, string text, IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;

      fileSystem.File.WriteAllText(fileInfo.FullName, text);
      return fileInfo;
    }

    public static FileInfo WriteAllLines(this FileInfo fileInfo, IEnumerable<string> lines, IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;

      var l = lines.ToArray();
      fileSystem.File.WriteAllLines(fileInfo.FullName, l);
      return fileInfo;
    }


    public static string NameWithoutExtension(this FileInfo fileInfo)
    {
      return Path.GetFileNameWithoutExtension(fileInfo.FullName);
    }

    public static FileInfo AppendLine(this FileInfo fileinfo, string text, IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;

      var lines = new string[] { text };
      fileSystem.File.AppendAllLines(fileinfo.FullName, lines);

      return fileinfo;
    }


    public static FileInfo Increment(this FileInfo fileInfo)
    {
      var filename = fileInfo.NameWithoutExtension();

      var numberFinder = "(?<zeros>0*)(?<number>[123456789]*)$";
      var rx = new Regex(numberFinder);
      var m = rx.Match(filename);

      string updatedFileName;

      if (string.IsNullOrEmpty(m.Value)) // No Existing number
        updatedFileName = $"{filename}2";
      else
      {
        int originalNumber = int.Parse(m.Groups["number"].Value);
        var zeroText = m.Groups["zeros"].Value;
        int zeros = zeroText.Length;
        string fileNameBeforeNumbers = filename.Substring(0, m.Index);

        if (zeros == 0) //no padded zeros 
        {
          updatedFileName = $"{fileNameBeforeNumbers}{originalNumber + 1}";
        }
        else //padded zeros 
        {
          var paddedZeros = ((originalNumber + 1) / 10 - originalNumber / 10 == 0) ? zeroText : zeroText.Substring(1);
          updatedFileName = $"{fileNameBeforeNumbers}{paddedZeros}{originalNumber + 1}";
        }
      }

      var updatedPath = UpdateFilePath(fileInfo.FullName, null,updatedFileName);
      return new FileInfo(updatedPath);

    }



    private static string  UpdateFilePath(string originalPath, string updatedDirectory= null, string updatedFileName = null, string updatedExtension = null)
    {
      if (updatedDirectory == null && updatedFileName == null && updatedExtension == null)
        return originalPath;

      var directory = updatedDirectory ?? Path.GetDirectoryName(originalPath);
      var fileName = updatedFileName ?? Path.GetFileNameWithoutExtension(originalPath);
      var extension = updatedExtension?? Path.GetExtension(originalPath);

      return CreateFilePath(directory, fileName, extension);
    }

    private static string CreateFilePath (   string directory,  string fileName, string extension)
    {
      var updatedPath = $@"{directory.TrimEnd('\\')}\{fileName.TrimStart('\\')}{extension}";
      return updatedPath;
    }

    public static FileInfo Rename(this FileInfo fileinfo, string updatedFileName, IFileSystem fileSystem = null)
    {
      return Rename(fileinfo, updatedFileName, null, fileSystem);
    }

     public static FileInfo Rename(this FileInfo fileinfo, string updatedFileName, string updatedExtension, IFileSystem fileSystem = null)
     {
       if (fileSystem == null) fileSystem = concreteFileSystem;
       var updatedPath = UpdateFilePath(fileinfo.FullName, fileinfo.DirectoryName, updatedFileName, updatedExtension);

       if (!fileSystem.File.Exists(fileinfo.FullName))
         return new FileInfo(updatedPath);

       fileSystem.File.Move(fileinfo.FullName, updatedPath);
       return new FileInfo(updatedPath);
    }

    public static FileInfo ChangeExtension(this FileInfo fileInfo ,string updatedExtension, IFileSystem fileSystem = null)
    {
      return Rename(fileInfo, null, updatedExtension, fileSystem);
    }


    public static FileInfo Rename(this FileInfo fileinfo, Func<string, string> transformation,  IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;
      var updatedPath = UpdateFilePath(fileinfo.FullName, fileinfo.DirectoryName, transformation( fileinfo.NameWithoutExtension()), fileinfo.Extension);

      if (!fileSystem.File.Exists(fileinfo.FullName))
        return new FileInfo(updatedPath);

      fileSystem.File.Move(fileinfo.FullName, updatedPath);
      return new FileInfo(updatedPath);
    }

    public static FileInfo MoveTo(this FileInfo fileinfo, DirectoryInfo directory, bool overWriteExisting = false, string updatedFileName = null, string updatedExtension = null,IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;
      var destination = UpdateFilePath(fileinfo.FullName , directory.FullName, updatedFileName, updatedExtension);

      if (fileSystem.File.Exists(destination) && !overWriteExisting)
        return fileinfo;

      fileSystem.File.Move(fileinfo.FullName, destination) ;
      return new FileInfo(destination);
    }

    public static FileInfo CopyTo(this FileInfo fileinfo, DirectoryInfo directory, bool overWriteExisting = false, string updatedFileName  = null, string updatedExtension = null, IFileSystem fileSystem = null)
    {
      if (fileSystem == null) fileSystem = concreteFileSystem;
      var destination = UpdateFilePath(fileinfo.FullName, directory.FullName, updatedFileName, updatedExtension);

      if (fileSystem.File.Exists(destination) && !overWriteExisting)
        return fileinfo;

      fileSystem.File.Copy(fileinfo.FullName, destination);
      return new FileInfo(destination);
    }


    //public static string RelativePath(this FileInfo fileinfo, DirectoryInfo directory)
    //{
    //  var root = directory.FullName;
    //  var path = fileinfo.FullName;

    //  if (!path.StartsWith(root)) return path;

    //  var length = (root.EndsWith(@"\")) ? root.Length : root.Length + 1;
    //  return path.Substring(length);
    //}

    //#region Hashing
    //public static string Md5Hash(this FileInfo fileInfo)
    //{
    //    return fileInfo.OpenRead().CryptoHash<MD5CryptoServiceProvider>();
    //}

    //public static string Sha1Hash(this FileInfo fileInfo)
    //{
    //    return fileInfo.OpenRead().CryptoHash<SHA1CryptoServiceProvider>();
    //}


    //public static string Sha256Hash(this FileInfo fileInfo)
    //{
    //    return fileInfo.OpenRead().CryptoHash<SHA256CryptoServiceProvider>();
    //}
    //public static string Sha384Hash(this FileInfo fileInfo)
    //{
    //    return fileInfo.OpenRead().CryptoHash<SHA384CryptoServiceProvider>();
    //}
    //public static string Sha512Hash(this FileInfo fileInfo)
    //{
    //    return fileInfo.OpenRead().CryptoHash<SHA512CryptoServiceProvider>();
    //}



    //#endregion

    //private static bool IsSameAs(this FileInfo fileInfo, FileInfo otherFileInfo, EHashCode hashCode, bool performByteByByte)
    //{
    //    if (fileInfo.Length != otherFileInfo.Length)
    //        return false;

    //    if (fileInfo.Hash(hashCode) != otherFileInfo.Hash(hashCode))
    //        return true;

    //    //from https://support.microsoft.com/en-us/kb/320348 
    //    int file1byte;
    //    int file2byte;

    //    var fs1 = new FileStream(fileInfo.FullName, FileMode.Open);
    //    var fs2 = new FileStream(otherFileInfo.FullName, FileMode.Open);
    //    do
    //    {
    //        // Read one byte from each file.
    //        file1byte = fs1.ReadByte();
    //        file2byte = fs2.ReadByte();
    //    }
    //    while ((file1byte == file2byte) && (file1byte != -1));
    //    return ((file1byte - file2byte) == 0);
    //}


    public static bool IsOfExtension(this FileInfo fileInfo, params string[] extensions)
    {
      return extensions.Any(extension => string.Equals(fileInfo.Extension, extension));
    }
  }
}
