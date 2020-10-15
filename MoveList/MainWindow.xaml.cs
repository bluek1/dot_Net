using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Win32;
using System;
using System.Windows;

namespace MoveList {
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window {
        

        public MainWindow() {
            InitializeComponent();

            Unosquare.FFME.Library.FFmpegDirectory = @"C:\ffmpeg";
        }

        private void Window_Initialized(object sender, EventArgs e) {
            Dictionary<String, String> doc = new Dictionary<string, string>();
            doc.Add("1", "1");
            doc.Add("2", "2");
            doc.Add("3", "3");
            doc.Add("4", "4");
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog() { DefaultExt = ".avi", Filter = "All files (*.*)|*.*", Multiselect = false };

            if (dlg.ShowDialog() == true) { Media.Source = new Uri(dlg.FileName); Media.Volume = 50; Media.SpeedRatio = 1; Media.Play(); }

        }
    }
}

