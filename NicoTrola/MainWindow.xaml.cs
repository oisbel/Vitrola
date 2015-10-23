using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
//using System.Security.AccessControl;
//using System.Security.Permissions;
//using System.Threading;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
using System.Windows.Interop;
using System.Windows.Media;
using System.Media;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using Brush = System.Windows.Media.Brush;
//using ListView = System.Windows.Controls.ListView;
//using ListViewItem = System.Windows.Controls.ListViewItem;
using MessageBox = System.Windows.MessageBox;
//using Microsoft.VisualBasic.ApplicationServices;
//using Windows.UI.Notifications;
//using Windows.Data.Xml.Dom;
namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly Vitrola _vitrola;
        //private FormTrola form;
        private SoundPlayer soundPlayerCoin;
        private SoundPlayer soundPlayerPraise;
        //private MediaPlayer player;
        /// <summary>
        /// Pantalla secundaria
        /// </summary>
        private BigScreen bigScreen;
        private Brush BorderBrushListBox;
        /// <summary>
        /// Color del borde de los listBox
        /// </summary>
        private SolidColorBrush ColorBrush;
        /// <summary>
        /// Color de degradado de los triangulos
        /// </summary>
        private LinearGradientBrush linearGBTriangle;

        public MainWindow()
        {
            if (IsExecutingApplication())
                Close();
            
            FocusManager.SetIsFocusScope(this, true);
            _vitrola = new Vitrola();
           
            _vitrola.LoadVitrola();
            _vitrola.LoadTrazas();

            StartMove();

            Thread.Sleep(_vitrola.DelaySeg*1000);

            try
            {
                var dirWindows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                var dirWindowsFonts = dirWindows + "\\Fonts";
                var fontsDesde = _vitrola.CurrentDirectory + "\\Fonts";
                
                //Copiar las fuentes a su destino si es que no existen
                if (Directory.Exists(fontsDesde) && Directory.Exists(dirWindowsFonts))
                {
                    foreach (var file in Directory.GetFiles(fontsDesde))
                    {
                        var temp = file.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        var name = temp[temp.Length - 1];
                        if (!File.Exists(dirWindowsFonts + "\\" + name))
                            File.Copy(file, dirWindowsFonts + "\\" + name);
                    }
                }
            }
            catch{}

            InitializeComponent();

            DataContext = _vitrola;
            _vitrola.NotifyChanged("Genres");

            listBoxGenre.Width = _vitrola.Width;
            listBoxArtist.Width = _vitrola.Width;
            listBoxTrack.Width = _vitrola.Width;
            BorderBrushListBox = listBoxTrack.BorderBrush;
            ColorBrush = new SolidColorBrush(Colors.Gold);

            bigScreen = new BigScreen(this);
            bigScreen.SetVolume(_vitrola.Volumen);
            

            if (_vitrola.Genres != null && _vitrola.Genres.Count > 0 && _vitrola.Artists.Count > 0
                && _vitrola.Tracks.Count > 0)
            {
                if (_vitrola.ViewMiniature)
                {
                    _vitrola.Source = _vitrola.DirTracks[0];
                    mediaElement.Play();
                }
            }

            soundPlayerCoin = new SoundPlayer {SoundLocation = _vitrola.CurrentDirectory + @"\Configuration\coin.wav"};
            soundPlayerPraise = new SoundPlayer
                                    {SoundLocation = _vitrola.CurrentDirectory + @"\Configuration\praise.wav"};

            /////////////////Poner video de Fondo/////////////////////////

            //player = new MediaPlayer();
            //player.Volume = 0;
            //VideoDrawing videoDrawing = new VideoDrawing();
            //videoDrawing.Rect = new Rect(0, 0, 150, 100);
            //videoDrawing.Player = player;
            //DrawingBrush brush = new DrawingBrush(videoDrawing);
            //player.Open(new Uri(@"D:\videos\videos clips\Baladas\ALEX BUENO\02.- AMIGO.mpg"));

            //Background = brush;
            //player.Play();

            //creando el color de degradao de los triangulos
            linearGBTriangle = new LinearGradientBrush();
            linearGBTriangle.EndPoint = new Point(1, 0.5);
            linearGBTriangle.StartPoint = new Point(0, 0.5);

            var gradientStop = new GradientStop();
            gradientStop.Offset = 0;
            gradientStop.Color = Colors.Black;
            var gradientStop1 = new GradientStop();
            gradientStop1.Offset = 1;
            gradientStop1.Color = Colors.Red;

            linearGBTriangle.GradientStops =
                new GradientStopCollection(new List<GradientStop>() {gradientStop, gradientStop1});

            //Verify();
            CopyRegister();
            HideTaskBar();
            Activate();
            Focus();
            Keyboard.Focus(this);

            WindowInteropHelper interopHelper = new WindowInteropHelper(this);
            IntPtr windowHandle = interopHelper.Handle;
            //IntPtr windowHandle = new WindowInteropHelper(System.Windows.Application.Current.MainWindow).Handle;
            WinAPI.SiempreEncima(windowHandle.ToInt32());
           
        }
        /// <summary>
        /// Ejecución de la carga demorada del programa
        /// </summary>
        void StartMove()
        {
            var start = new Start(_vitrola.DelayMin, _vitrola.DelaySeg);
            try
            {
                start.ShowDialog();
            }
            catch { }
        }
        /// <summary>
        /// Copia las fuentes a al carpeta Fonts de Windows(Si no existen)
        /// </summary>
        private void CopyFonts()
        {
            var startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            if (Directory.Exists(startup))
            {
                using (StreamWriter writer = new StreamWriter(startup + "\\NicoTrola" + ".url"))
                {
                    string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    writer.WriteLine("[InternetShortcut]");
                    writer.WriteLine("URL=file:///" + app);
                    writer.WriteLine("IconIndex=0");
                    string icon = app.Replace('\\', '/');
                    writer.WriteLine("IconFile=" + icon);
                    writer.Flush();
                }
            }
        }

        private void Verify()
        {
            try
            {
                var computerData = new ComputerData();
                var skey = "10/06/20";
                if (!computerData.IsOkComputer(skey,_vitrola.CurrentDirectory))
                {
                    if (computerData.SameUser)
                        Close();
                    Random random = new Random();
                    var insertlicence = new Licence(random.Next(10000, 99999).ToString(), false);
                    insertlicence.ShowDialog();
                    if (!computerData.InsertLicence(insertlicence.GiveMeRandom(),
                                                    insertlicence.GiveMeLicence(), skey, false))
                    {
                        MessageBox.Show(
                            "Contactar al Proveedor",
                            "Licencia Inválida");
                        Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Problema al identificar el origen del programa", "Licencia Inválida");
                Close();
            }
        }

        //Constantes para ShowWindow  
        private const int SW_HIDE = 0;
        private const int SW_NORMAL = 1;
        //Busca el handle de una ventana hija a partir de su Hwnd Parent y el nombre de clase
        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, int hWnd2, string lpsz1, string lpsz2);

        //Busca el HWND de la ventana, en este caso la de la barra de tareas de windows ( mediante el nombre de clase Shell_TrayWnd )  
        [DllImport("user32")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //Oculta y muestra una ventana a partir de su HWND  
        [DllImport("user32")]
        private static extern void ShowWindow(IntPtr hwnd, int nCmdShow);

        private IntPtr HWND_TaskBar;
        private IntPtr HWND_TrayNotify;
        private IntPtr HWND_Iconos;
        private IntPtr HWND_Reloj;
        private IntPtr HWND_Start;
        /// <summary>
        /// Oculta barra de tarea y area de notificacion
        /// </summary>
        private void HideTaskBar()
        {
            try
            {
                HWND_TaskBar = FindWindow("Shell_TrayWnd", null);
                HWND_TrayNotify = FindWindowEx(HWND_TaskBar, 0, "TrayNotifyWnd", null);
                HWND_Iconos = FindWindowEx(HWND_TrayNotify, 0, "Syspager", null);
                HWND_Reloj = FindWindowEx(HWND_TrayNotify, 0, "TrayClockWClass", null);
                HWND_Start = FindWindowEx(HWND_Start, 0, "BUTTON", null);

                ShowWindow(HWND_Start, SW_HIDE);
                ShowWindow(HWND_TrayNotify, SW_HIDE);
                ShowWindow(HWND_Iconos, SW_HIDE);
                ShowWindow(HWND_Reloj, SW_HIDE);
                ShowWindow(HWND_TaskBar, SW_HIDE);
            }
            catch 
            {
               
            }
        }
        /// <summary>
        /// Desoculta barra de tarea y area de notificacion
        /// </summary>
        private void ShowTaskBar()
        {
            try
            {
                HWND_TaskBar = FindWindow("Shell_TrayWnd", null);
                HWND_TrayNotify = FindWindowEx(HWND_TaskBar, 0, "TrayNotifyWnd", null);
                HWND_Iconos = FindWindowEx(HWND_TrayNotify, 0, "Syspager", null);
                HWND_Reloj = FindWindowEx(HWND_TrayNotify, 0, "TrayClockWClass", null);
                HWND_Start = FindWindowEx(HWND_Start, 0, "BUTTON", null);

                ShowWindow(HWND_TaskBar, SW_NORMAL);
                ShowWindow(HWND_TrayNotify, SW_NORMAL);
                ShowWindow(HWND_Iconos, SW_NORMAL);
                ShowWindow(HWND_Reloj, SW_NORMAL);
                ShowWindow(HWND_Start, SW_NORMAL);
            }
            catch
            {

            }
        }

        private void CopyRegister()
        {
            try
            {
                var objRegistro = Registry.CurrentUser;
                objRegistro = objRegistro.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\", true);
                var dir = _vitrola.CurrentDirectory + "\\NicoTrola.exe";
                if (objRegistro == null)
                    return;
                foreach (var reg in objRegistro.GetValueNames())
                {
                    if(reg=="EvaPlay" && objRegistro.GetValue(reg).ToString()==dir)
                        return;
                }
                if (objRegistro != null)
                {
                    objRegistro.SetValue("EvaPlay", dir, RegistryValueKind.String);
                }
            }
            catch (UnauthorizedAccessException exAccess)
            {

            }
        }

        private bool IsExecutingApplication()
        {
            // Proceso actual
            Process currentProcess = Process.GetCurrentProcess();

            // Matriz de procesos
            Process[] processes = Process.GetProcesses();

            // Recorremos los procesos en ejecución
            foreach (Process p in processes)
            {
                if (p.Id != currentProcess.Id)
                {
                    if (p.ProcessName == currentProcess.ProcessName)
                    {
                        return true;
                    }
                }
            }
            return false;
        } 
        /// <summary>
        /// Método que se ejecuta al hechar una moneda
        /// </summary>
        private void CoinIn()
        {
            _vitrola.Money += _vitrola.ValueTrack;
            if (File.Exists(_vitrola.CurrentDirectory + @"\Configuration\coin.wav"))
            {
               soundPlayerCoin.Play();
            }
            else
            {
                SystemSounds.Exclamation.Play();
            }
        }

        /// <summary>
        /// Método que se ejecuta al seleccionar una tema,para sustraer el dinero y hacer el sonido predeterminado
        /// </summary>
        private void SelectTrack()
        {
            _vitrola.Money -= _vitrola.ValueTrack;
            soundPlayerPraise.Play();
        }

        /// <summary>
        /// Reproduce la vista enminiatura del tema seleccionado
        /// </summary>
        private void PlayMiniature()
        {
            if (!_vitrola.ViewMiniature || _vitrola.DirTracks==null ||listBoxTrack.SelectedIndex==-1||
                _vitrola.DirTracks.Count==0)
                return;
            try
            {
                _vitrola.Source = _vitrola.DirTracks[listBoxTrack.SelectedIndex];
                mediaElement.Play();
            }
            catch
            {
                return;
            }
        }

        public void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.C:
                    {
                        CoinIn();
                        break;
                    }
                case Key.Escape:
                    {
                        Close();

                        break;
                    }
                case Key.M:
                    {
                        var menu = new Menu(_vitrola);
                        menu.ShowDialog();
                        if (menu.CloseWindows)
                        {
                            var p = new ProcessStartInfo("cmd", "/c " + "shutdown -p") { CreateNoWindow = true };
                            var a = new Process { StartInfo = p };
                            a.Start();
                            Close();
                        }
                        if (menu.CloseProgram)
                        {
                            Close();
                        } 
                        _vitrola.UpdateCollectionMiddle(Vitrola.TypesList.Genre);
                        _vitrola.UpdateCollectionMiddle(Vitrola.TypesList.Artist);
                        _vitrola.UpdateCollectionMiddle(Vitrola.TypesList.Track);
                        _vitrola.NotifyChanged("SelectedGenre");//actualiza las dependencias de seleccion
                        _vitrola.NotifyChanged("SelectedArtist");

                        bigScreen.SetVolume(_vitrola.Volumen);
                        PublicityMove();

                        if (!_vitrola.ViewMiniature)
                            mediaElement.Stop();
                        else if (listBoxTrack.SelectedIndex != -1)
                        {
                            PlayMiniature();
                        }
                        //listBoxArtist.ApplyTemplate();
                        //listBoxArtist.InvalidateArrange();
                        //listBoxArtist.InvalidateVisual();
                        //InitializeComponent();
                        break;
                    }
                case Key.Enter:
                    {
                        if (listBoxTrack.SelectedIndex == -1)
                            break;
                        if (_vitrola.Money < _vitrola.ValueTrack)
                        {
                            textBlockMoney.Background = Brushes.Red;
                            labelMoney.Background = Brushes.Red;
                            break;
                        }

                        listBoxTrack.BorderBrush = ColorBrush;
                        //var lbi = (ListBoxItem)listBoxTrack.ItemContainerGenerator.ContainerFromItem(
                        //     listBoxTrack.SelectedValue);
                        var lbi = (ListBoxItem)listBoxTrack.ItemContainerGenerator.ContainerFromIndex(
                             listBoxTrack.SelectedIndex);
                        
                        lbi.BorderThickness = new Thickness(10, 1, 1, 1);
                        lbi.Opacity = 0.5;
                        int c = listBoxTrack.Items.Count < 10 ? listBoxTrack.Items.Count : 10;
                        for (int i = 0; i < c; i++)
                        {
                            var lb = (ListBoxItem)listBoxTrack.ItemContainerGenerator.ContainerFromIndex(i);
                            lb.IsEnabled = false;
                        }
                        lbi.IsEnabled = true;
                        if (bigScreen != null && listBoxTrack.SelectedValue != null)
                        {
                            if (listBoxReproduction.Items.Count > 0 &&
                                listBoxTrack.SelectedValue == listBoxReproduction.Items[listBoxReproduction.Items.Count - 1])
                                break;
                            var dir = _vitrola.DirTracks[listBoxTrack.SelectedIndex];
                            var name = _vitrola.Tracks[listBoxTrack.SelectedIndex];
                            bigScreen.AddTrack(dir, name);
                            
                            SelectTrack();
                        }
                    }
                    break;
                case Key.Z:
                    {
                        bigScreen.NextTrack();
                    }
                    break;
                case Key.E: //arriba de Género
                    {
                        if (listBoxGenre.SelectedIndex == -1)
                            break;
                        UpGenreDown();
                        break;
                    }
                case Key.D: //abajo de Género
                    {
                        if (listBoxGenre.SelectedIndex == -1)
                            break;
                        DownGenreDown();
                        break;
                    }
                case Key.R: //arriba de Artista
                    {
                        if (listBoxArtist.SelectedIndex == -1)
                            break;
                        UpArtistDown();
                        break;
                    }
                case Key.F: //abajo de Artista
                    {
                        if (listBoxArtist.SelectedIndex == -1)
                            break;
                        DownArtistDown();
                        break;
                    }
                case Key.T: //arriba de Track
                    {
                        if (listBoxTrack.SelectedIndex == -1)
                            break;
                        UpTrackDown();
                        break;
                    }
                case Key.G: //abajo de Track
                    {
                        if (listBoxTrack.SelectedIndex == -1)
                            break;
                        DownTrackDown();
                        break;
                    }
            }
        }
        
        #region DownsEvents

        /// <summary>
        /// Seleccionar hacia arriba en la lista de géneros en el evento keydown y thouchdown
        /// </summary>
        void UpGenreDown()
        {
            genreUp.Fill = Brushes.Red;
            //genreDown.Stroke = Brushes.CadetBlue;
           
            Rotate(_vitrola.Genres, true);
            _vitrola.SelectedGenre = _vitrola.Genres[(int)_vitrola.MiddleGenre];
            
                        
        }
        /// <summary>
        /// Seleccionar hacia abajo en la lista de géneros en el evento keydown y thouchdown
        /// </summary>
        void DownGenreDown()
        {
            
            genreDown.Fill = Brushes.Red;
            //genreUp.Stroke = Brushes.CadetBlue;
            
            Rotate(_vitrola.Genres, false);
            _vitrola.SelectedGenre = _vitrola.Genres[(int)_vitrola.MiddleGenre];
                
        }
        /// <summary>
        /// Seleccionar hacia arriba en la lista de artistas en el evento keydown y thouchdown
        /// </summary>
        void UpArtistDown()
        {

            artistUp.Fill = Brushes.Red;
            //artistDown.Stroke = Brushes.CadetBlue;
            Rotate(_vitrola.Artists, true);
            _vitrola.SelectedArtist = _vitrola.Artists[(int)_vitrola.MiddleArtist];

        }
        /// <summary>
        /// Seleccionar hacia abajo en la lista de artistas en el evento keydown y thouchdown
        /// </summary>
        void DownArtistDown()
        {
            
            artistDown.Fill = Brushes.Red;
            //artistUp.Stroke = Brushes.CadetBlue;
            Rotate(_vitrola.Artists, false);
            _vitrola.SelectedArtist = _vitrola.Artists[(int)_vitrola.MiddleArtist];
                       
        }
        /// <summary>
        /// Seleccionar hacia arriba en la lista de tracks en el evento keydown y thouchdown
        /// </summary>
        void UpTrackDown()
        {
            trackUp.Fill = Brushes.Red;
            //trackDown.Stroke = Brushes.CadetBlue;
            Rotate(_vitrola.Tracks, true);
            Rotate1(_vitrola.DirTracks, true);
            //_vitrola.SelectedTrack = _vitrola.Tracks[(int)_vitrola.MiddleTrack];
            _vitrola.SelectedIndexTrack = (int)_vitrola.MiddleTrack;
                       
        }
        /// <summary>
        /// Seleccionar hacia abajo en la lista de tracks en el evento keydown y thouchdown
        /// </summary>
        void DownTrackDown()
        {
            
            trackDown.Fill = Brushes.Red;
            //trackUp.Stroke = Brushes.CadetBlue;

            Rotate(_vitrola.Tracks, false);
            Rotate1(_vitrola.DirTracks, false);
            //_vitrola.SelectedTrack = _vitrola.Tracks[(int)_vitrola.MiddleTrack];
            _vitrola.SelectedIndexTrack = (int)_vitrola.MiddleTrack;
        }

        #endregion
       
        public void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        if (listBoxTrack.SelectedIndex == -1)
                            break;
                        //var color = new Color();
                        //color.A = 255;
                        //color.R = 183;
                        //color.G = 167;
                        //color.B = 167;
                        textBlockMoney.Background =Brushes.Black;
                        labelMoney.Background = Brushes.Transparent;

                        listBoxTrack.BorderBrush = BorderBrushListBox;
                        //var lbi = (ListBoxItem)listBoxTrack.ItemContainerGenerator.ContainerFromItem(
                        //    listBoxTrack.SelectedValue);
                        var lbi = (ListBoxItem)listBoxTrack.ItemContainerGenerator.ContainerFromIndex(
                           listBoxTrack.SelectedIndex);
                        lbi.BorderThickness = new Thickness(1);
                        lbi.Opacity = 1;
                        int c = listBoxTrack.Items.Count < 10 ? listBoxTrack.Items.Count : 10;
                        for (int i = 0; i < c; i++)
                        {
                            var lb = (ListBoxItem)listBoxTrack.ItemContainerGenerator.ContainerFromIndex(i);
                            lb.IsEnabled = true;
                        }
                        break;
                    }
                case Key.E: //arriba de Género
                    {
                        UpGenreUp();
                        break;
                    }
                case Key.D: //abajo de Género
                    {
                        DownGenreUp();
                        break;
                    }
                case Key.R: //arriba de Artista
                    {
                        UpArtistUp();
                        break;
                    }
                case Key.F: //abajo de Artista
                    {
                        DownArtistUp();
                        break;
                    }
                case Key.T: //arriba de Track
                    {
                        UpTrackUp();
                        break;
                    }
                case Key.G: //abajo de Track
                    {
                        DownTrackUp();
                        break;
                    }
            }
        }

        #region UpsEvents

        /// <summary>
        /// Seleccionar hacia arriba en la lista de géneros en el evento keyUp y thouchUp
        /// </summary>
        void UpGenreUp()
        {
            genreUp.Fill = linearGBTriangle;
            //genreUp.Stroke = Brushes.DarkGreen;
            if (_vitrola.Tracks != null )
            {
                _vitrola.DoAfterSelectGenre();
                _vitrola.DoAfterSelectArtist();
                PlayMiniature();
            }
        }
        /// <summary>
        /// Seleccionar hacia abajo en la lista de géneros en el evento keyUp y thouchUp
        /// </summary>
        void DownGenreUp()
        {
            genreDown.Fill = linearGBTriangle;
            //genreDown.Stroke = Brushes.DarkGreen;
            if (_vitrola.Tracks!=null)
            {
                _vitrola.DoAfterSelectGenre();
                _vitrola.DoAfterSelectArtist();
                PlayMiniature();
            }
        }
        /// <summary>
        /// Seleccionar hacia arriba en la lista de artistas en el evento keyUp y thouchUp
        /// </summary>
        void UpArtistUp()
        {
            artistUp.Fill = linearGBTriangle;
            //artistUp.Stroke = Brushes.DarkGreen;
            if (_vitrola.Tracks != null)
            {
                _vitrola.DoAfterSelectArtist();
                PlayMiniature();
            }
        }
        /// <summary>
        /// Seleccionar hacia abajo en la lista de artistas en el evento keyUp y thouchUp
        /// </summary>
        void DownArtistUp()
        {
            artistDown.Fill = linearGBTriangle;
            //artistDown.Stroke = Brushes.DarkGreen;
            if (_vitrola.Tracks != null )
            {
                _vitrola.DoAfterSelectArtist();
                PlayMiniature();
            }
        }
        /// <summary>
        /// Seleccionar hacia arriba en la lista de tracks en el evento keyUp y thouchUp
        /// </summary>
        void UpTrackUp()
        {
            trackUp.Fill = linearGBTriangle;
            //trackUp.Stroke = Brushes.DarkGreen;
            if (_vitrola.Tracks != null)
            {
                PlayMiniature();
            }
        }
        /// <summary>
        /// Seleccionar hacia abajo en la lista de tracks en el evento keyUp y thouchUp
        /// </summary>
        void DownTrackUp()
        {
            trackDown.Fill = linearGBTriangle;
            //trackDown.Stroke = Brushes.DarkGreen;
            if (_vitrola.Tracks != null )
            {
                PlayMiniature();
            }
        }

        #endregion 

        /// <summary>
        /// Rota los elementos de las colecciones arriba si up=true else abajo.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="up"></param>
        private void Rotate(ObservableCollection<string> collection, bool up)
        {
            if (collection.Count <= 1)
                return;
            if (up)
            {
                var temp = collection[collection.Count - 1];
                collection.RemoveAt(collection.Count - 1);
                //verificar que con el concat sea mas-menos eficiente
                collection.Insert(0, temp);
            }
            else
            {
                var temp = collection[0];
                collection.RemoveAt(0);
                collection.Add(temp);
            }
        }

        /// <summary>
        /// Rota los elementos de las colecciones arriba si up=true else abajo.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="up"></param>
        private void Rotate1(List<string> collection, bool up)
        {
            if (collection.Count <= 1)
                return;
            if (up)
            {
                var temp = collection[collection.Count - 1];
                collection.RemoveAt(collection.Count - 1);
                //verificar que con el concat sea mas-menos eficiente
                collection.Insert(0, temp);
            }
            else
            {
                var temp = collection[0];
                collection.RemoveAt(0);
                collection.Add(temp);
            }
        }

        private bool first;

        private void listBoxGenre_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!first)
            {
                first = true;
                return;
            }
            _vitrola.HeightListBox = listBoxGenre.ActualHeight;
            _vitrola.UpdateCollectionMiddle(Vitrola.TypesList.Genre);
            _vitrola.UpdateCollectionMiddle(Vitrola.TypesList.Artist);
            _vitrola.UpdateCollectionMiddle(Vitrola.TypesList.Track);
            first = false;
        }

        private void wind_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Screen.AllScreens.Length > 1)
            {
                Keyboard.Focus(this);
            }
        }
        /// <summary>
        /// Inicializa el movimiento del texto de publicidad
        /// </summary>
        private void PublicityMove()
        {
            double height = canMain.ActualHeight - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(0, height/2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            //doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(16));
            tbmarquee.BeginAnimation(Canvas.RightProperty, doubleAnimation);
        }

        private void wind_Loaded(object sender, RoutedEventArgs e)
        {
            PublicityMove();
            bigScreen.Show();
        }

        private void mediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            return;
        }

        private void wind_Closed(object sender, EventArgs e)
        {
            ShowTaskBar();
            bigScreen.Close();
            _vitrola.DisposeBackGrounds();
            Util.SerializeList(_vitrola.DeleteImages, _vitrola.CurrentDirectory + "\\tempdelete");
            Util.SerializeList(_vitrola.DirBackGroundImages, _vitrola.CurrentDirectory + "\\tempdirback");
            //_vitrola.SaveBackGround();
            //_vitrola.CleanBackGround();
        }

        private void genreUp_TouchDown(object sender, TouchEventArgs e)
        {
            UpGenreDown();
        }

        private void genreUp_TouchUp(object sender, TouchEventArgs e)
        {
            UpGenreUp();
        }

        private void artistUp_TouchDown(object sender, TouchEventArgs e)
        {
            UpArtistDown();
        }

        private void artistUp_TouchUp(object sender, TouchEventArgs e)
        {
            UpArtistUp();
        }

        private void trackUp_TouchDown(object sender, TouchEventArgs e)
        {
            UpTrackDown();
        }

        private void trackUp_TouchUp(object sender, TouchEventArgs e)
        {
            UpTrackUp();
        }

        private void genreDown_TouchDown(object sender, TouchEventArgs e)
        {
            DownGenreDown();
        }

        private void genreDown_TouchUp(object sender, TouchEventArgs e)
        {
            DownGenreUp();
        }

        private void artistDown_TouchDown(object sender, TouchEventArgs e)
        {
            DownArtistDown();
        }

        private void artistDown_TouchUp(object sender, TouchEventArgs e)
        {
            DownArtistUp();
        }

        private void trackDown_TouchDown(object sender, TouchEventArgs e)
        {
            DownTrackDown();
        }

        private void trackDown_TouchUp(object sender, TouchEventArgs e)
        {
            DownTrackUp();
        }

        private void listBoxTrack_TouchDown(object sender, TouchEventArgs e)
        {
            if (listBoxTrack.SelectedIndex == -1)
                return;
            if (_vitrola.Money < _vitrola.ValueTrack)
            {
                textBlockMoney.Background = Brushes.Brown;
                labelMoney.Background = Brushes.Red;
               return;
            }

            listBoxTrack.BorderBrush = ColorBrush;
            var lbi = (ListBoxItem)listBoxTrack.ItemContainerGenerator.ContainerFromItem(
                 listBoxTrack.SelectedValue);
            lbi.BorderThickness = new Thickness(10, 1, 1, 1);
            lbi.Opacity = 0.5;
            int c = listBoxTrack.Items.Count < 10 ? listBoxTrack.Items.Count : 10;
            for (int i = 0; i < c; i++)
            {
                var lb = (ListBoxItem)listBoxTrack.ItemContainerGenerator.ContainerFromIndex(i);
                lb.IsEnabled = false;
            }
            lbi.IsEnabled = true;
            if (bigScreen != null && listBoxTrack.SelectedValue != null)
            {
                if (listBoxReproduction.Items.Count > 0
                    &&
                    listBoxTrack.SelectedValue ==
                    listBoxReproduction.Items[listBoxReproduction.Items.Count - 1])
                    return;
                var dir = _vitrola.DirTracks[listBoxTrack.SelectedIndex];
                var name = _vitrola.Tracks[listBoxTrack.SelectedIndex];
                bigScreen.AddTrack(dir, name);
                SelectTrack();
            }
        }
       
        //public class ListViewItemStyleSelector : StyleSelector
        //{
        //    public override Style SelectStyle(object item,
        //        DependencyObject container)
        //    {
        //        var st = new Style();
        //        st.TargetType = typeof(ListViewItem);
        //        var backGroundSetter = new Setter();
        //        backGroundSetter.Property = ListViewItem.BackgroundProperty;
        //        var listView =
        //            ItemsControl.ItemsControlFromItemContainer(container)
        //              as ListView;
        //        int index =
        //            listView.ItemContainerGenerator.IndexFromContainer(container);
        //        if (index % 2 == 0)
        //        {
        //            backGroundSetter.Value = Brushes.LightBlue;
        //        }
        //        else
        //        {
        //            backGroundSetter.Value = Brushes.Beige;
        //        }
        //        st.Setters.Add(backGroundSetter);
        //        return st;
        //    }
        //}    
    }

}
