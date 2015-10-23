using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para Licence.xaml
    /// </summary>
    public partial class Licence : Window
    {
        private string prelicence;
        public Licence(string prelicence, bool key)
        {
            InitializeComponent();
            this.prelicence = prelicence;
            prelicencetextBlock.Text = prelicence + "-";
            if (key)
                prelicencetextBlock.Visibility = Visibility.Hidden;
        }
        public Licence()
        {
            
        }
        public string GiveMeLicence()
        {
            return licenceTextBox.Text;
        }
        public string GiveMeRandom()
        {
            return prelicence;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            licenceTextBox.Text = "";
            Close();
        }
        public void CloseW()
        {
            Close();
        }
    }
}
