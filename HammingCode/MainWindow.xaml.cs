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

namespace HammingCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frame.Navigate(new CoderPage());
            
        }

        private void Coder_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new CoderPage());
        }

        private void Decoder_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new DecoderPage());
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new AboutPage());
        }
    }
}
