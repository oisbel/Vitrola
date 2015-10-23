//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para AddGenre.xaml
    /// </summary>
    public partial class AddGenre : Window
    {
        public string NameGenre { get; set; }
        public AddGenre()
        {
            InitializeComponent();
        }

        private void accept_Click(object sender, RoutedEventArgs e)
        {
            NameGenre = nameGenreTB.Text;
            Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Escape)
                Close();
        }
    }
}
