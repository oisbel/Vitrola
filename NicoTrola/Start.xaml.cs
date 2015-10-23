using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para Start.xaml
    /// </summary>
    public partial class Start : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Retraso en segundos de programa al cargar
        /// </summary>
        public int DelaySeg { get; set; }
        /// <summary>
        /// Retraso en minutos de programa al cargar
        /// </summary>
        public int DelayMin { get; set; }
        public Start(int min, int seg)
        {
            InitializeComponent();

            DelayMin = min;
            DelaySeg = seg;
        }
     
        private void dAmElementBs_Completed(object sender, EventArgs e)
        {
          Close();
        }
        private Duration durationmElement;
        /// <summary>
        /// Duracion del retraso de la aplicacion
        /// </summary>
        public Duration DurationmElement
        {
            get { return durationmElement; }
            set
            {
                if (durationmElement != value)
                {
                    durationmElement = value;
                    NotifyChanged("DurationmElement");
                }
            }
        }
        public void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double height = canMain.ActualHeight - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.Duration = new Duration(new TimeSpan(0,DelayMin,DelaySeg));
            doubleAnimation.Completed += new EventHandler(DoubleAnimatioCompleted);
            tbmarquee.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
            //DurationmElement = new Duration(new TimeSpan(0,DelayMin,DelaySeg));
            //progressBar1.BeginStoryboard(storyBoard);
        }
        private void DoubleAnimatioCompleted(object sender, EventArgs e)
        {
            Close();
        }
    }
}
