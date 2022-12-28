using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.IO.Errata
{
    public static class FileInfoArrayExtensions
    {

        public static void SortLikeExplorer(this FileInfo[] files)
        {
            var comparer = new StringLogicalComparer();
            Array.Sort(files, (x, y) => comparer.Compare(x.Name, y.Name));

        }


        public static byte[] Bytes(this IEnumerable<FileInfo> files)
        {
            using (var output = new MemoryStream())
            {
                foreach (var file in files)
                {
                    using (var input = file.OpenRead())
                    {
                        input.CopyTo(output);
                    }
                }
                var bytes = output.ToArray();
                return bytes;
            }
        }


        private class StringLogicalComparer : IComparer, IComparer<string>
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern int StrCmpLogicalW(string x, string y);

            public int Compare(object x, object y)
            {
                return StrCmpLogicalW(x.ToString(), y.ToString());
            }

            public int Compare(string x, string y)
            {
                return StrCmpLogicalW(x, y);
            }
        }

        

    }
}
