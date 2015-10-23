using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
//using System.Windows.Media;
using System.Windows.Media.Animation;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.MessageBox;
//using ListViewItem = System.Windows.Controls.ListViewItem;
//using MessageBox = System.Windows.MessageBox;
using ProgressBar = System.Windows.Controls.ProgressBar;

namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para BigScreen.xaml
    /// </summary>
    public partial class BigScreen : Window
    {
        #region Propiedades
        /// <summary>
        /// Nombre del autor del tema que se está tocando
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Tema que se está tocando
        /// </summary>
        public string Track { get; set; }
        
        private int missingTrack;
        /// <summary>
        /// Cantidad de temas que quedan por tocar
        /// </summary>
        public int MissingTrack
        {
            get { return missingTrack; }
            set 
            {
                missingTrack = value;
                missingTrackTextBlock.Text = missingTrack.ToString();
            }
        }
        /// <summary>
        /// Mensaje de publicidad de la pantalla secundaria
        /// </summary>
        public string SmsWelcome { get; set; }
        /// <summary>
        /// Volumen del reproductor
        /// </summary>
        public double Volumen { get; set; }
        /// <summary>
        /// Lista de reproduccion
        /// </summary>
        private List<string> PlayList { get; set; }
        /// <summary>
        /// ListBox de reproduccion pasado en el contructor 
        /// </summary>
        private ListBox ListBoxReproduction { get; set; }
        /// <summary>
        /// Indice de el video de publicidad que toca luego de terminada la cancion
        /// </summary>
        private int IndexPublicity { get; set; }
        /// <summary>
        /// Direccion del video a reproducir en pantalla secundaria en tiempo de inactividad.
        /// </summary>
        private string dirScreenSaver;
        private Vitrola _vitrola;
        private MainWindow _mainWindow;
        private ProgressBar _progressBar;

        private Storyboard _storyboard;
        #endregion
        public BigScreen(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            dirScreenSaver = mainWindow._vitrola.DirScreenSaver;
            if (mainWindow._vitrola.ViewVideo && dirScreenSaver!="")
            {
                mediaElement.Visibility = Visibility.Visible;
                imageLogo.Visibility = Visibility.Hidden;
                try
                {
                    mediaElement.Source = new Uri(dirScreenSaver);
                    mediaElement.Play();
                }
                catch
                {
                    mediaElement.Visibility = Visibility.Hidden;
                    imageLogo.Visibility = Visibility.Visible;
                    dirScreenSaver = "";
                }
            }
            else
            {
                mediaElement.Visibility = Visibility.Hidden;
                imageLogo.Visibility = Visibility.Visible;
                dirScreenSaver = "";
            }

            SmsWelcome = "ESCOJA SU TEMA FAVORITO";
            tbmarquee.Text = SmsWelcome;
            MissingTrack = 0;
            Author = SmsWelcome;
            Track = "";
            ListBoxReproduction =mainWindow.listBoxReproduction;
            _storyboard = mainWindow.storyBoard;
            _progressBar = mainWindow.progressBar1;
            _vitrola = mainWindow._vitrola;

            PlayList = new List<string>();
            //Publicities = _vitrola.Publicities;
            Screen tempScreen = Screen.PrimaryScreen;
            foreach (var screen in Screen.AllScreens)
            {
                if (!screen.Primary)
                    tempScreen = screen;
            }
            Left= tempScreen.Bounds.Left;
            Top = tempScreen.Bounds.Top;
            Width = tempScreen.Bounds.Width;
            Height = tempScreen.Bounds.Height;

            stackPanel.Width = Width;
            ScaleVideo();
            //mediaElement.Width = Width;
            //WindowState = WindowState.Maximized;
            Activate();
        }
        #region Métodos
        /// <summary>
        /// Reproduce un archivo de multimedia
        /// </summary>
        /// <param name="url">direccion de archivo</param>
        public void PlayTrack(string url)
        {
            try
            {
                mediaElement.Visibility = Visibility.Visible;
                imageLogo.Visibility = Visibility.Hidden;

                mediaElement.Source = new Uri(url);
                mediaElement.Play();

                var temp = url.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                Track = temp.Last();
                Author = temp[temp.Length - 2];
                var list = Track.Split(new char[] {'.'},
                                       StringSplitOptions.RemoveEmptyEntries).ToList();
                list.RemoveAt(list.Count - 1);
                var name = "";
                foreach (var l in list)
                {
                    name += l;
                }
                tbmarquee.Text = Author + "  :  " +name;
                PublicityMove();
                lastPublicity = false;
            }
            catch
            {
                _vitrola.Money += _vitrola.ValueTrack;
                return;
            }
            //if (ListBoxReproduction.Items.Count > 0)
            //{
            //    ListBoxReproduction.SelectedValue = ListBoxReproduction.Items[0].ToString();
            //    var lbi = (ListViewItem)ListBoxReproduction.ItemContainerGenerator.ContainerFromIndex(0);
            //    lbi.FontSize = lbi.FontSize + 4;
            //    lbi.Foreground = Brushes.Gold;
            //}
        }
        /// <summary>
        /// Agrega un archivo a la lista de reproduccion
        /// </summary>
        /// <param name="url">direccion del archivo</param>
        /// <param name="name">nombre del archivo</param>
        public void AddTrack(string url, string name)
        {
            ListBoxReproduction.Items.Add(name);
            PlayList.Add(url);
            _vitrola.AddTrack(url);
            if (PlayList.Count == 1)
                PlayTrack(url);
            else
            {
                MissingTrack += 1;
            }
        }
        ///// <summary>
        ///// Agrega la lista de publicidad entre canciones
        ///// </summary>
        ///// <param name="publicities"></param>
        //public void AddPublicities(List<string> publicities)
        //{
        //    Publicities = publicities;
        //    IndexPublicity = 0;
        //}
        /// <summary>
        /// indica si la ultima reproduccion fue una publicidad
        /// </summary>
        private bool lastPublicity;
        /// <summary>
        /// Pasa a la siguiente pista de la lista de reproducción
        /// </summary>
        public void NextTrack()
        {
            if (PlayList.Count == 0)
            {
                if (dirScreenSaver!="")
                {
                    mediaElement.Visibility = Visibility.Visible;
                    imageLogo.Visibility = Visibility.Hidden;
                    if(mediaElement.Source.LocalPath==dirScreenSaver)
                    {
                        mediaElement.Stop();
                        mediaElement.Play();
                    }
                    else
                    {
                        mediaElement.Source = new Uri(dirScreenSaver);
                        mediaElement.Play();
                    }
                }
                return;
            }

            if(!lastPublicity && _vitrola.Publicities.Count>0 &&_vitrola.ViewPublicities)
            {
                try
                {
                    if (IndexPublicity >= _vitrola.Publicities.Count)
                        IndexPublicity = 0;
                    lastPublicity = true;
                    mediaElement.Source = new Uri(_vitrola.Publicities[IndexPublicity++]);
                    mediaElement.Play();
                    tbmarquee.Visibility = Visibility.Hidden;
                }
                catch
                {
                    
                }
                
                return;
            }
            PlayList.RemoveAt(0);
            ListBoxReproduction.Items.RemoveAt(0);
            MissingTrack -= 1;
            if (PlayList.Count == 0)
            {
                Author = SmsWelcome;
                Track = "";
                mediaElement.Close();
                tbmarquee.Text = SmsWelcome;
                PublicityMove();
                MissingTrack = 0;

                if (dirScreenSaver != "")
                {
                    mediaElement.Visibility = Visibility.Visible;
                    imageLogo.Visibility = Visibility.Hidden;
                    if (mediaElement.Source.LocalPath == dirScreenSaver)
                    {
                        mediaElement.Stop();
                        mediaElement.Play();
                    }
                    else
                    {
                        mediaElement.Source = new Uri(dirScreenSaver);
                        mediaElement.Play();
                    }
                }
                else
                {
                    mediaElement.Visibility = Visibility.Hidden;
                    imageLogo.Visibility = Visibility.Visible;
                }

                return;
            }
            lastPublicity = false;
            PlayTrack(PlayList.First());
            
        }
        /// <summary>
        /// Establece el volumen del reproductor(de cero a 1
        /// </summary>
        /// <param name="volumen"></param>
        public void SetVolume(double volumen)
        {
            Volumen = volumen;
            mediaElement.Volume = Volumen;
        }

        #endregion

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
           NextTrack();
        }
        /// <summary>
        /// Lista de publicidades que no se pueden reproducir
        /// </summary>
        List<string> failedPublicities=new List<string>(); 
        private void mediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (lastPublicity && _vitrola.Publicities.Count > 0)
            {
                if (IndexPublicity == 0)
                    failedPublicities.Add(_vitrola.Publicities[_vitrola.Publicities.Count - 1]);
                else failedPublicities.Add(_vitrola.Publicities[IndexPublicity - 1]);

                if (failedPublicities.Count >= _vitrola.Publicities.Count)
                    lastPublicity = true;
                else lastPublicity = false;
                NextTrack();
                return;
            }
            var temptrack = "";
            if(PlayList.Count>0)
                 temptrack= PlayList.First();

            NextTrack();
           _vitrola.MessageError="TEMA INCOMPATIBLE";
            _vitrola.Money += _vitrola.ValueTrack;
            _vitrola.AddWrong(temptrack);
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source.LocalPath == dirScreenSaver)
                return;
            ScaleVideo();
            //mediaElement.Height = mediaElement.Height;
            //mediaElement.Width = Width;
            _vitrola.DurationmElement = mediaElement.NaturalDuration;
            _progressBar.BeginStoryboard(_storyboard);

        }
        
        /// <summary>
        /// Escala el ancho y el alto de un media element
        /// </summary>
        private void ScaleVideo()
        {
            var difWidth = Width - mediaElement.NaturalVideoWidth;
            var difHeight = Height - mediaElement.NaturalVideoHeight;
            if(Math.Abs(difWidth - 0) < 1 || difWidth<0)
                return;
           
            stackPanel.Height = mediaElement.NaturalVideoHeight +
                                Math.Min(difWidth, difHeight);
            //mediaElement.Height = Height;
            //mediaElement.Height = Math.Min(mediaElement.NaturalVideoHeight + difWidth,
            //    mediaElement.NaturalVideoHeight + difHeight);
            //mediaElement.Width = Width;
        }
        private void PublicityMove()
        {
            tbmarquee.Visibility = Visibility.Visible;
            double height = canMain.ActualHeight - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            //doubleAnimation.From = -tbmarquee.ActualWidth;
            //doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.From = -tbmarquee.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.AutoReverse = false;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(24));
            tbmarquee.BeginAnimation(Canvas.RightProperty, doubleAnimation);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           PublicityMove();
        }
        private void window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            _mainWindow.Activate();
            _mainWindow.Focus();
        }
        
    }
}
