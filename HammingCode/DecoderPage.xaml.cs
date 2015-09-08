using HammingCode.Model;
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
    /// Interaction logic for DecoderPage.xaml
    /// </summary>
    public partial class DecoderPage : Page
    {
        private DecoderDevice DeCoder
        {
            get
            {
                return (DecoderDevice)Resources["DecoderDevice"];
            }
        }
        public DecoderPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!DeCoder.IsInitialized)
            {
                var inputStr = inputBox.Text;

                if(inputStr.Length!=7)
                {
                    MessageBox.Show("Длина входного символа должна быть равна 7!","Ошибка");
                }

                for (int i = 0; i < inputStr.Length; i++)
                {
                    if (!(inputStr[i] == '0' || inputStr[i] == '1'))
                    {
                        MessageBox.Show("Код должен задаваться символами '0' или '1'!", "Ошибка");
                        return;
                    }
                }

                bool[] input = new bool[7];

                for (int i = 0; i < inputStr.Length; i++)
                {
                    if (inputStr[i] == '0') input[i] = false;
                    else input[i] = true;
                }

                DeCoder.Initialize(input);
            }


            DeCoder.MakeTick();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DeCoder.Clear();
        }
    }
}
