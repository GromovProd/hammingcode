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
    /// Interaction logic for Coder.xaml
    /// </summary>
    public partial class CoderPage : Page
    {

        private CoderDevice Coder
        {
            get
            {
                return (CoderDevice)Resources["CoderDevice"];
            }
        }

        public CoderPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Coder.isInitialized)
            {
                var inputStr = inputBox.Text;

                if (inputStr.Length != 4)
                {
                    MessageBox.Show("Длина входного символа должна быть равна 4!", "Ошибка");
                }

                for (int i = 0; i < inputStr.Length; i++)
                {
                    if (!(inputStr[i] == '0' || inputStr[i] == '1'))
                    {
                        MessageBox.Show("Код должен задаваться символами '0' или '1'!", "Ошибка");
                        return;
                    }
                }

                bool[] input = new bool[4];

                for (int i = 0; i < inputStr.Length; i++)
                {
                    if (inputStr[i] == '0') input[i] = false;
                    else input[i] = true;
                }

                Coder.Initialize(input);
            }


            Coder.MakeTick();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Coder.Clear();
        }


    }
}
