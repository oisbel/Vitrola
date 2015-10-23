//using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private readonly Vitrola _vitrola;
        /// <summary>
        /// Indica si se ha de cerrar el programa
        /// </summary>
        public bool CloseProgram { get; set; }
        /// <summary>
        /// Indica que se va a apagar la maguina
        /// </summary>
        public bool CloseWindows { get; set; }
        public Menu(Vitrola vitrola)
        {
            InitializeComponent();
            _vitrola = vitrola;
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
            _vitrola.UpdateCollection();
               Close();
            }
        }

        private void ShutDownClick(object sender, RoutedEventArgs e)
        {
            CloseWindows = true;
            Close();
        }

        private void GeneralConfigurationClick(object sender, RoutedEventArgs e)
        {
            var conf = new Configuration(_vitrola);
            conf.ShowDialog();
        }

        private void CloseProgramClick(object sender, RoutedEventArgs e)
        {
            CloseProgram = true;
            _vitrola.UpdateCollection();
            Close();
        }

        private void PublicityClick(object sender, RoutedEventArgs e)
        {
            var addPublicity = new AddPublicity(_vitrola.Publicity,_vitrola.Publicities,_vitrola.ViewPublicities);
            addPublicity.ShowDialog();
            if(_vitrola.Publicity!=addPublicity.Publicity || addPublicity.ChangePublicities)
            {
                _vitrola.Publicity = addPublicity.Publicity;
                _vitrola.Publicities = addPublicity.Publicities;
                _vitrola.ViewPublicities = addPublicity.ActivatePublicities;
                _vitrola.UpdateConfiguration();
            }


        }

        private void FilterClick(object sender, RoutedEventArgs e)
        {
            var conf = new Configuration(_vitrola, true);
            conf.ShowDialog();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            _vitrola.UpdateCollection();
            Close();
        }
    }
}
