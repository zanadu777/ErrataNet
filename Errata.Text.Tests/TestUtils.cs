using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errata.Text.Tests
{
    internal static class TestUtils
    {
        private static DirectoryInfo? testDataFolder;
        private static DirectoryInfo TestDataFolder
        {
            get
            {
                if (testDataFolder == null)
                {
                    var loc = @"D:\Dev\Programming 2022\ErrataNet6\Errata.Text.Tests\bin\Debug\net6.0\Errata.Text.Tests.dll";
                    var fi = new FileInfo(loc);
                    var dir = fi.Directory.Parent.Parent.Parent;

                    var dirpath = Path.Combine(dir.FullName, "TestData");
                    testDataFolder = new DirectoryInfo(dirpath);
                }

                return testDataFolder;
            }

        }

        public static string ReadTestData(string fileName)
        {
            var text = File.ReadAllText(Path.Combine(TestDataFolder.FullName, fileName));
            return text;
        }
    }
}
