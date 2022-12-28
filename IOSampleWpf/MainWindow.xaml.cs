using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Errata;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IOSampleWpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void btBackup_Click(object sender, RoutedEventArgs e)
    {
    //var swatch = Stopwatch.StartNew();
      var d = new DirectoryInfo(@"\\Ix\Plex2\TV Shows\Star Blazers 2199\Season 1");
       var backupLocation = new DirectoryInfo(@"D:\temp\Backup");
      //d.CopyFilesTo(backupLocation);

      var transformations = new List<Func<string,string>>();
      transformations.Add(x => x.Replace("[Golumpa] ", ""));
      transformations.Add( x=> x.Replace("END", ""));

      foreach (var file in backupLocation.GetFiles())
      {
        file.Rename(x => x.Replace("[Golumpa] ", ""));
        file.Rename(x => x.Replace("END", ""));
        file.Rename(x => x.Replace(@"(Uchuu Senkan Yamato 2199) [FuniDub 1080p x264 AAC] " , ""));
        var rxBrace = new Regex(@"\[.*?\]");
        file.Rename(x => rxBrace.Replace(x, ""));

        file.Rename(x => x.Replace(" - ", " - S01E"));

        file.Rename(x => x.Trim());
      }
    }


     
  }
}
