//using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para AddPublicity.xaml
    /// </summary>
    public partial class AddPublicity : Window
    {
        /// <summary>
        /// Publicidad de tipo texo
        /// </summary>
        public string Publicity { get; set; }
        /// <summary>
        /// Publicidad entre canciones
        /// </summary>
        public List<string> Publicities { get; set; }
        /// <summary>
        /// Determina si cambio alguna publicidad
        /// </summary>
        public bool ChangePublicities { get; set; }
        /// <summary>
        /// Determina si se reproduce publicidad entre canciones
        /// </summary>
        public bool ActivatePublicities { get; set; }
        public AddPublicity(string publicity,List<string> publicities,bool activatePublicities )
        {
            InitializeComponent();
            beforePub = publicity;
            Publicity = publicity;
            publicitiesCB.IsChecked = !activatePublicities;
            ActivatePublicities = activatePublicities; ;
            Publicities =new List<string>(publicities);
            this.publicity.Text = Publicity;
            listPublicity.ItemsSource = Publicities;
            if(Publicities.Count>0)
            {
                var temp = Publicities.First().Split(new char[] { '\\' });
                publicityTB.Text = temp[temp.Length-2];
            }
        }

        private string beforePub = "";
        private void accept_Click(object sender, RoutedEventArgs e)
        {
            Publicity = publicity.Text;
            Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            ChangePublicities = false;
            Publicity = beforePub;
            Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Publicity = beforePub;
                ChangePublicities = false;
                Close();
            }
        }

        private void AddPublicity_Click(object sender, RoutedEventArgs e)
        {
            var fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && Directory.Exists(fd.SelectedPath))
            {
                Publicities.Clear();
                var files = Directory.GetFiles(fd.SelectedPath);
                foreach (var file in files)
                {
                    Publicities.Add(file);
                    
                }
                ChangePublicities = true;
                if (Publicities.Count > 0)
                {
                    var temp = Publicities.First().Split(new char[] { '\\' });
                    publicityTB.Text = temp[temp.Length - 2];
                }
                listPublicity.ItemsSource = null;
                listPublicity.ItemsSource = Publicities;
                //listPublicity.BringIntoView();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            listPublicity.IsEnabled = false;
            border.IsEnabled = false;
            ActivatePublicities = false;
            ChangePublicities = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            listPublicity.IsEnabled = true;
            border.IsEnabled = true;
            ActivatePublicities = true;
            ChangePublicities = true;
        }
    }
}
