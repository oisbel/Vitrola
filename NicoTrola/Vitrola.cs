using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.MessageBox;

namespace NicoTrola
{
    /// <summary>
    /// Contexto visual de generos-artistas-temas
    /// </summary>
    public class Vitrola : INotifyPropertyChanged
    {
        //una salveda para poder deseleccionar las dos filas iguales con selecionindex
        #region Propiedades
        /// <summary>
        /// Volumen a que se reproduciran los videos(escala 1-10)
        /// </summary>
        public int Volumen { get; set; }
        /// <summary>
        ///Extensiones de archivos que se van a reproducir
        /// </summary>
        public List<string> Filters { get; set; }
        /// <summary>
        /// Valor  para reproducir una canción
        /// </summary>
        public double ValueTrack { get; set; }
        /// <summary>
        /// Diccionario de Géneros-Artistas-Track
        /// </summary>
        public SortedList<string, SortedList<string, Tuple<List<string>, List<string>,string,string>>> GenArtTrack { get; set; }
        /// <summary>
        /// Direcciones de los archivos a reproducir
        /// </summary>
        public List<string> DirTracks { get; set; }
        public string CurrentDirectory { get; set; }
        /// <summary>
        /// Dirección del archivo con la colección serializada
        /// </summary>
        public string FileDirCollection { get; set; }
        /// <summary>
        /// Direccion de la foto del artista por default
        /// </summary>
        public string DirImage { get; set; }
        /// <summary>
        /// Direccion donde se alojan de las imagenes de fondo de los géneros
        /// (archivos con los nombres de los generos y ninguna extension) y del archivo(background) con los colores n caso de que los tengan:genre:color
        /// </summary>
        public string DirBackGround { get; set; }
        /// <summary>
        /// Direcion del archivo de configuracion
        /// </summary>
        public string FileDirConfiguration { get; set; }
        /// <summary>
        /// Direccion del archivo de las trazas
        /// </summary>
        public string FileDirTrazas { get; set; }
        /// <summary>
        /// Establece si se va a ver la vista en miniatura
        /// </summary>
        public bool ViewMiniature { get; set; }
        /// <summary>
        /// Establece si se va a proyectar video en la pantalla secundaria durante la inactividad
        /// </summary>
        public bool ViewVideo { get; set; }
        /// <summary>
        /// Establece si se van a reproducir publicidades entre videos
        /// </summary>
        public bool ViewPublicities { get; set; }
        /// <summary>
        /// Altura de los lisbox asociados a los generos,artistas,tracks
        /// </summary>
        public double HeightListBox { get; set; }
        /// <summary>
        /// Indice del elemento que cae en el medio
        /// </summary>
        public double MiddleGenre { get; set; }
        /// <summary>
        /// Indice del elemento que cae en el medio
        /// </summary>
        public double MiddleArtist { get; set; }
        /// <summary>
        /// Indice del elemento que cae en el medio
        /// </summary>
        public double MiddleTrack { get; set; }
        public enum TypesList { Genre,Artist,Track};
        private string _source;
        /// <summary>
        /// Retraso en segundos de programa al cargar
        /// </summary>
        public int DelaySeg { get; set; }
        /// <summary>
        /// Retraso en minutos de programa al cargar
        /// </summary>
        public int DelayMin { get; set; }

        private List<string> publicities; 
        /// <summary>
        /// Publicidad entre canciones
        /// </summary>
        public List<string> Publicities
        {
            get { return publicities; } 
            set 
            {
                publicities = value;
            }
        }
        /// <summary>
        /// Direccion del archivo de multimedia para el que se requiere una vista previa
        /// </summary>
        public string Source
        {
            get { return _source; }
            set
            {
                if(_source!=value)
                {
                    _source = value;
                    NotifyChanged("Source");
                }
            }
        }
        public string CountGenre 
        {
            get
            {
                int count = Genres != null ? Genres.Count : 0;
                return "Géneros Musicales ("+count.ToString()+ ")";
            }
        }
        public string CountArtist
        {
            get
            {
                int count = Artists != null ? Artists.Count : 0;
                return "Artistas (" + count.ToString() + ")";
            }
        }
        /// <summary>
        /// Fondos del programa(uno por cada genero)
        /// </summary>
        public SortedList<string, Brush> BackGrounds { get; set; }
        /// <summary>
        /// Direcciones de los fondos del programa
        /// </summary>
        public SortedList<string, string> BackGroundsStrings { get; set; }
        /// <summary>
        /// Bitmas correspondientes a los fondos de la vitrola
        /// </summary>
        public List<BitmapImage> BitmapImages { get; set; } 
        /// <summary>
        /// Imagenes que ya no seran utilizadas como fondo y tienen nombre de algun genero
        /// </summary>
        public List<string> DeleteImages { get; set; } 
        /// <summary>
        /// Direcciones de las imagenes de los fondos de los generos 
        /// que han sido agregadas a las que existian hasta que se inicio la vitrola,para luegohacer los cambios en disco
        /// </summary>
        public SortedList<string, string> DirBackGroundImages { get; set; } 
        private string _dirScreenSaver;
        /// <summary>
        /// Direccion del video de fondo del pantalla secundaria(ruta relativa)
        /// </summary>
        public string DirScreenSaver
        {
            get
            {
                var temp = CurrentDirectory + "\\" + _dirScreenSaver;
                if(File.Exists(temp))
                    return temp;
                return "";
            }
            set { _dirScreenSaver = value; }
        }
      
        private string _messageError;
        /// <summary>
        /// Mensaje de error cuando no se puede reproducir un archivo
        /// </summary>
        public string MessageError
        {
            get { return _messageError; }
            set
            {
                if(value!=null)
                {
                    _messageError = value;
                    var message = new Messages(_messageError);
                    message.Show();
                    message.Sleep();
                }
            }
        }
        /// <summary>
        /// Trazas que se cargan desde archivo "trazas"
        /// </summary>
        public List<Traza> Trazas { get; set; } 
        #endregion
        /// <summary>
        /// Maneja la configuracion general, la colleccion y la notificacion de su visualización
        /// </summary>
        public Vitrola()
        {
            Inicialize();
        }
        public void Inicialize()
        {
            DirBackGroundImages = new SortedList<string, string>();
            DeleteImages = new List<string>();
            BitmapImages = new List<BitmapImage>();
            Width = Screen.PrimaryScreen.Bounds.Width / 4 - 10;
            WidthBorder = Width - 20;
            Source = "";
            DurationmElement = new Duration(TimeSpan.FromDays(1));
            //ViewMiniature = true;
            
            CurrentDirectory = Application.StartupPath;
            if (!Directory.Exists(CurrentDirectory + "\\Configuration"))
            {
                CurrentDirectory = Directory.GetCurrentDirectory();
                if (!Directory.Exists(CurrentDirectory + "\\Configuration"))
                {
                    var temp = Assembly.GetCallingAssembly().Location.Split(
                        new char[] {'\\'}).ToList();
                    temp.RemoveAt(temp.Count-1);
                    var current = "";
                    foreach (var VARIABLE in temp)
                    {
                        current += VARIABLE;
                    }
                    CurrentDirectory = current;
                }
                
            }
            FileDirConfiguration = @"Configuration\config.dat";
            FileDirTrazas = @"Configuration\trazas.dat";
            LoadConfiguration();
            //MessageBox.Show("Direccion del archivo de configuracion: " + FileDirConfiguration);
            if (File.Exists(CurrentDirectory+ "\\" + FileDirCollection))
            {
                //MessageBox.Show("Direccion del archivo de coleccion: " + FileDirCollection);
                GenArtTrack = Util.Deserialize(CurrentDirectory + "\\" + FileDirCollection);
            }
            UpdateCollection();
            security = new Security();
            Trazas=new List<Traza>();
        }
        /// <summary>
        /// Metodo a llamar luego de crear la intancia para establecer los fondos
        /// </summary>
        public void LoadVitrola()
        {
            UpdateBackGround();
        }
        ~Vitrola()
        {
            try
            {
                if (!File.Exists(CurrentDirectory + "\\temptemporizador"))
                {
                    var dir = CurrentDirectory + "\\temptemporizador";
                    File.Create(dir);
                    DeleteImages = Util.DeserializeList(CurrentDirectory + "\\tempdelete");
                    File.Delete(CurrentDirectory + "\\tempdelete");
                    DirBackGroundImages = Util.DeserializeSortedList(CurrentDirectory + "\\tempdirback");
                    File.Delete(CurrentDirectory + "\\tempdirback");
                    SaveBackGround();
                    CleanBackGround();
                }
                else
                    File.Delete(CurrentDirectory+ "\\temptemporizador");
            }
            catch
            {
                return;
            }
        }

        #region Métodos
        /// <summary>
        /// Inicializa los valores de configuracion desde el archivo
        /// </summary>
        void LoadConfiguration()
        {
            Filters=new List<string>();
            Publicities=new List<string>();
            try
            {
                //MessageBox.Show(CurrentDirectory + "\\" + FileDirConfiguration);
                string config = File.ReadAllText(CurrentDirectory + "\\" + FileDirConfiguration, Encoding.UTF8);
                var temp = config.Split(new [] { "<FileCollection>", "</FileCollection>", "\r\n" },
                    StringSplitOptions.RemoveEmptyEntries);
                FileDirCollection = temp.First();
                temp = config.Split(new [] { "<Filters>", "</Filters>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new []{"\r\n"},StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in temp)
                {
                    Filters.Add(s);
                }
                temp = config.Split(new [] { "<ValueTrack>", "</ValueTrack>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new []{"\r\n"},StringSplitOptions.RemoveEmptyEntries);
                ValueTrack =double.Parse(temp.First());

                temp=config.Split(new [] { "<Volumen>", "</Volumen>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new []{"\r\n"},StringSplitOptions.RemoveEmptyEntries);
                Volumen = int.Parse(temp.First());

                temp = config.Split(new [] { "<DirImage>", "</DirImage>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                DirImage = temp.First();
                SelectImage = CurrentDirectory + "\\" + DirImage;

                temp = config.Split(new [] { "<ScreenSaver>", "</ScreenSaver>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                DirScreenSaver = temp.First();

                temp = config.Split(new [] { "<DirBackGround>", "</DirBackGround>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                DirBackGround = temp.First();

                temp = config.Split(new [] { "<ViewMiniature>", "</ViewMiniature>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                ViewMiniature = temp.First()=="True";

                temp = config.Split(new [] { "<ViewVideo>", "</ViewVideo>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                ViewVideo = temp.First() == "True";

                temp = config.Split(new[] { "<ViewPublicities>", "</ViewPublicities>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                ViewPublicities = temp.First() == "True";

                temp = config.Split(new [] { "<Publicity>", "</Publicity>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                Publicity = temp.First();
                temp = config.Split(new[] { "<Publicities>", "</Publicities>" },
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(
                    new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in temp)
                {
                    Publicities.Add(s);
                }
                temp = config.Split(new[] { "<FontsizeLListBox>", "</FontsizeLListBox>" },
                   StringSplitOptions.RemoveEmptyEntries)[1].Split(
                   new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                FontsizeLListBox = double.Parse(temp.First());
                temp = config.Split(new[] { "<Delay>", "</Delay>" },
                   StringSplitOptions.RemoveEmptyEntries)[1].Split(
                   new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                DelayMin =int.Parse(temp[0]);
                DelaySeg = int.Parse(temp[1]);

            }
            catch
            {
                FileDirCollection = @"Configuration\collection.dat";
                Filters.Add("wmv");
                Filters.Add("avi");
                Filters.Add("mpg");
                Filters.Add("mpeg");
                Filters.Add("dat");
                Filters.Add("mp4");
                Filters.Add("vob");
                ValueTrack =0.5;
                Volumen = 50;
                SelectImage =CurrentDirectory + @"\\Configuration\artist.jpg";
                DirScreenSaver = @"Configuration\sc.mpg";
                DirBackGround = @"Images\";
                ViewMiniature = true;
                ViewVideo = true;
                ViewPublicities = true;
                Publicity = "Vitrolas Modernas";
                FontsizeLListBox = 22;
                FontsizeIListBoxSelect = 35;
                DelayMin = 0;
                DelaySeg = 30;
            }
            Util.Filters = Filters;
        }
        /// <summary>
        /// Actualiza la coleccion genres-artist-track desde la coleccion GenArtTrack.
        /// </summary>
        public void UpdateCollection()
        {
            if (GenArtTrack == null || GenArtTrack.Count == 0)
            {
                Genres = new ObservableCollection<string>();

                SelectedGenre = null;
                Artists = new ObservableCollection<string>();

                SelectedArtist = null;
                Tracks = new ObservableCollection<string>();
                DirTracks = new List<string>();

                //SelectedTrack = null;
                SelectedIndexTrack = -1;
            }
            else
            {
                try
                {
                    Genres = new ObservableCollection<string>(GenArtTrack.Keys);
                    if (Genres != null && Genres.Count > 0)
                        SelectedGenre = Genres.First();
                    Artists = new ObservableCollection<string>(GenArtTrack.First().Value.Keys);
                    if (Artists != null && Artists.Count > 0)
                        SelectedArtist = Artists.First();

                    foreach (var genArtTrack in GenArtTrack)
                    {
                        if (genArtTrack.Value.Count != 0)
                        {
                            Tracks = new ObservableCollection<string>(genArtTrack.Value.First().Value.Item1);
                            DirTracks = new List<string>(genArtTrack.Value.First().Value.Item2);
                            break;
                        }
                    }
                    if (Tracks != null && Tracks.Count > 0)
                    {
                        //SelectedTrack = Tracks.First();
                        SelectedIndexTrack = 0;
                    }
                }
                catch
                {
                    Genres = new ObservableCollection<string>();

                    SelectedGenre = null;
                    Artists = new ObservableCollection<string>();

                    SelectedArtist = null;
                    Tracks = new ObservableCollection<string>();
                    DirTracks = new List<string>();

                    //SelectedTrack = null;
                    SelectedIndexTrack = -1;
                }
            }
            NotifyChanged("Genres");
        }
        /// <summary>
        /// Pone la seleccion del lisbox en el medio.Es para configuraciones iniciales
        /// </summary>
        /// <param name="type"></param>
        public void UpdateCollectionMiddle(TypesList type )
        {
            //cantidad de elementos visibles en el listbox
            double countItem = 0;
            List<string> fin = null;
            string select = "";
            switch (type)
            {
                case TypesList.Genre:
                    if (Genres == null || Genres.Count <=0)
                    {
                        NotifyChanged("Genres");
                        break;
                    }
                    var t1 = Genres.Count*HeightIListBox + Genres.Count*7;
                    var t2 = HeightListBox/(HeightIListBox + 7);
                    countItem =  t1< HeightListBox
                        ? Genres.Count : t2;
                    fin = new List<string>();
                    select = Genres.First();
                    for (int i = 0; i < countItem/2-1; i++)
                    {
                        fin.Add(Genres[Genres.Count-1]);
                        Genres.RemoveAt(Genres.Count - 1);
                    }
                    fin.Reverse();
                    Genres=new ObservableCollection<string>(fin.Concat(Genres));
                    NotifyChanged("Genres");
                    SelectedGenre = select;
                    MiddleGenre = Genres.IndexOf(select);
                    break;
                case TypesList.Artist:
                    if (Artists == null || Artists.Count <= 0)
                    {
                        NotifyChanged("Artists");
                        break;
                    }
                    t1 = Artists.Count*HeightIListBox + Artists.Count*7;
                    t2 = HeightListBox/(HeightIListBox + 7);
                    countItem = t1< HeightListBox
                        ? Artists.Count :t2 ;
                    fin = new List<string>();
                    select = Artists.First();
                    for (int i = 0; i < countItem / 2 -1; i++)
                    {
                        fin.Add(Artists[Artists.Count - 1]);
                        Artists.RemoveAt(Artists.Count - 1);
                    }
                    fin.Reverse();
                    Artists = new ObservableCollection<string>(fin.Concat(Artists));
                    NotifyChanged("Artists");
                    SelectedArtist = select;
                    MiddleArtist = Artists.IndexOf(select);
                    break;
                case TypesList.Track:
                    if (Tracks == null || Tracks.Count <= 0)
                    {
                        NotifyChanged("Tracks");
                        MiddleTrack = -1;
                        SelectedIndexTrack = (int)MiddleTrack;
                        break;
                    }
                    t1 = Tracks.Count*HeightIListBox + Tracks.Count*7;
                    t2 = HeightListBox/(HeightIListBox + 7);
                    countItem = t1 < HeightListBox
                        ? Tracks.Count : t2;
                    fin = new List<string>();
                    var finDirTrack = new List<string>();
                    select = Tracks.First();
                    for (int i = 0; i < countItem / 2 -1; i++)
                    {
                        fin.Add(Tracks.Last());
                        Tracks.RemoveAt(Tracks.Count - 1);

                        finDirTrack.Add(DirTracks.Last());
                        DirTracks.RemoveAt(DirTracks.Count-1);
                    }
                    fin.Reverse();
                    finDirTrack.Reverse();
                    Tracks = new ObservableCollection<string>(fin.Concat(Tracks));
                    DirTracks = new List<string>(finDirTrack.Concat(DirTracks));
                    NotifyChanged("Tracks");
                    //SelectedTrack = select;
                    MiddleTrack = Tracks.IndexOf(select);
                    //MiddleTrack =countItem > 2 ? countItem % 2 == 0 ? countItem / 2 - 1: countItem / 2-0.5 : 0;
                    SelectedIndexTrack = (int)MiddleTrack;
                    break;
            }
        }
        /// <summary>
        /// Guarda los valores de ValueTrack, ViewMiniature,.., Publicity al archivo
        /// </summary>
        public void UpdateConfiguration()
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(CurrentDirectory+"\\"+ FileDirConfiguration,false,Encoding.UTF8);
                sw.WriteLine("<FileCollection>");
                sw.WriteLine(FileDirCollection);
                sw.WriteLine("</FileCollection>");
                
                sw.WriteLine("<Filters>");
                foreach (var filter in Filters)
                {
                    sw.WriteLine(filter);
                }
                sw.WriteLine("</Filters>");

                sw.WriteLine("<ValueTrack>");
                sw.WriteLine(ValueTrack);
                sw.WriteLine("</ValueTrack>");
               
                sw.WriteLine("<Volumen>");
                sw.WriteLine(Volumen);
                sw.WriteLine("</Volumen>");
                
                sw.WriteLine("<DirImage>");
                sw.WriteLine(DirImage);
                sw.WriteLine("</DirImage>");

                sw.WriteLine("<ScreenSaver>");
                sw.WriteLine(_dirScreenSaver);
                sw.WriteLine("</ScreenSaver>");

                sw.WriteLine("<DirBackGround>");
                sw.WriteLine(DirBackGround);
                sw.WriteLine("</DirBackGround>");

                sw.WriteLine("<ViewMiniature>");
                sw.WriteLine(ViewMiniature?"True":"False");
                sw.WriteLine("</ViewMiniature>");

                sw.WriteLine("<ViewVideo>");
                sw.WriteLine(ViewVideo ? "True" : "False");
                sw.WriteLine("</ViewVideo>");

                sw.WriteLine("<ViewPublicities>");
                sw.WriteLine(ViewPublicities ? "True" : "False");
                sw.WriteLine("</ViewPublicities>");

                sw.WriteLine("<Publicity>");
                sw.WriteLine(Publicity);
                sw.WriteLine("</Publicity>");

                sw.WriteLine("<Publicities>");
               
                foreach (var pub in Publicities)
                {
                    sw.WriteLine(pub);
                }
                sw.WriteLine("</Publicities>");

                sw.WriteLine("<FontsizeLListBox>");
                sw.WriteLine(FontsizeLListBox);
                sw.WriteLine("</FontsizeLListBox>");

                sw.WriteLine("<Delay>");
                sw.WriteLine(DelayMin);
                sw.WriteLine(DelaySeg);
                sw.WriteLine("</Delay>");

                sw.Close();
            }
            catch
            {
                if(sw!=null)
                    sw.Close();
            }
            Util.Filters = Filters;
        }
        /// <summary>
        /// Actualiza los fondos de los generos desde archivo(fotos y colores-archivo "background en carpeta:Images"-)
        /// </summary>
        public void UpdateBackGround()
        {
            BackGrounds = new SortedList<string, Brush>();
            BackGroundsStrings=new SortedList<string, string>();
            BackGround = Brushes.MediumOrchid;
            var dirbg = CurrentDirectory + DirBackGround + "background";
            if (File.Exists(dirbg))
            {
                SortedList<string, byte[]> temp = Util.DeserializeBrushes(dirbg);
                if (temp != null)
                {
                    BackGrounds = Util.ByteToBrush(temp);
                }
            }
            if ((Genres != null && Genres.Count != 0))
            {
                foreach (string t in Genres)
                {
                    var dir = CurrentDirectory + DirBackGround + t;
                    if (File.Exists(dir))
                    {
                        var imageDrawing = new ImageDrawing {Rect = new Rect(0, 0, 150, 100)};
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = new FileStream(dir, FileMode.Open, FileAccess.Read,FileShare.ReadWrite);
                        image.EndInit();
                        BitmapImages.Add(image);
                        imageDrawing.ImageSource = image;
                        if (BackGrounds.ContainsKey(t))
                        {
                            BackGrounds[t] = new DrawingBrush(imageDrawing);
                            BackGroundsStrings[t] = dir;
                        }
                        else
                        {
                            BackGrounds.Add(t, new DrawingBrush(imageDrawing));
                            BackGroundsStrings.Add(t,dir);
                        }
                    }
                }
                if (BackGrounds.ContainsKey(SelectedGenre))
                    BackGround = BackGrounds[SelectedGenre];
            }
        }
        /// <summary>
        /// Salva la imagenes de fondo de la vitrola al archivo
        /// </summary>
        public void SaveBackGround()
        {
            foreach (var deleteImage in DeleteImages)
            {
                while (true)
                {
                    try
                    {
                        File.Delete(deleteImage);
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            foreach (var dirImages in DirBackGroundImages)
            {
                    try
                    {
                        File.Move(dirImages.Value, CurrentDirectory + DirBackGround + dirImages.Key);
                        continue;
                    }
                    catch (Exception io)
                    {
                        if(io is IOException )//un archivo con el mismo nombre ya existe(hay que eliminarlo y luego renombrar)
                        {
                            int count = 0;
                            while (count<10)
                            {
                                try
                                {
                                    File.Delete(CurrentDirectory + DirBackGround + dirImages.Key);
                                    File.Move(dirImages.Value, CurrentDirectory + DirBackGround + dirImages.Key);
                                    break;
                                }
                                catch
                                {
                                    count++;
                                    continue;
                                } 
                            }
                           
                        }
                    }
            }
        }
        /// <summary>
        /// Libera los recursos de los fondos  de imagenes
        /// </summary>
        /// <returns></returns>
        public void DisposeBackGrounds()
        {
            BackGrounds.Clear();
            BackGround = null;
            if (BitmapImages==null)
                return;
            foreach (var bitmapImage in BitmapImages)
            {
                bitmapImage.StreamSource.Dispose();
            }
        }
        /// <summary>
        /// Elimina de las carpeta de los fondos de los generos todos los archivos que sobran
        /// (deja background y nombres de genero)
        /// </summary>
        public void CleanBackGround()
        {
            if(Genres==null ||Genres.Count==0 || !Directory.Exists(CurrentDirectory+DirBackGround))
                return;
            foreach (var file in Directory.GetFiles(CurrentDirectory+DirBackGround))
            {
                var temp = file.Split(new [] {'\\'}).Last();
                if(!Genres.Contains(temp) && temp!="background")
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        continue;
                    }
                    
            }
        }
        /// <summary>
        /// Agrega el valor de una moneda a "Money" acumulado
        /// </summary>
        /// <param name="money"></param>
        public void AddMoney(double money)
        {
            Money += money;
        }

        #region Trazas

        private Security security;
        private string skey;
        /// <summary>
        /// Carga la lista de trazas desde archivo
        /// </summary>
        public void LoadTrazas()
        {
            String st = Assembly.GetCallingAssembly().Location;
            FileInfo fi = new FileInfo(st);
            st = fi.Name;
            var caracteres = "Son10caracters";
            foreach (var VARIABLE in caracteres)
            {
                st += VARIABLE;
            }

            skey = System.IO.Path.GetFileName(st.Substring(0, 8)); 
            if(!File.Exists(CurrentDirectory+"\\"+FileDirTrazas))
            {
                Trazas.Add(new Traza(DateTime.Now,0));
                return;
            }
            
            GCHandle gch = GCHandle.Alloc(skey, GCHandleType.Pinned);
            
            security.DecryptFile(CurrentDirectory+"\\"+FileDirTrazas,
               CurrentDirectory + "\\" + "temp",
               skey);
            
            gch.Free();

            var list = File.ReadAllText(CurrentDirectory + "\\" + "temp").Split(new char[] { '*' },
                                                         StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in list)
            {
                if (s != "\r" && s != "\r\n")
                    Trazas.Add(new Traza(s));
            } 
            if(Trazas.Count>0 && (Trazas.Last().Date.Day!=DateTime.Now.Day
                || Trazas.Last().Date.Month != DateTime.Now.Month
                || Trazas.Last().Date.Year != DateTime.Now.Year))
            {
                var number = Trazas.Last().Number;
                Trazas.Add(new Traza(DateTime.Now,number+1));
            }
            if(Trazas.Count==0)
            {
                Trazas.Add(new Traza(DateTime.Now, 0));
            }
            File.Delete(CurrentDirectory + "\\" + "temp");
        }
        /// <summary>
        /// Salva las trazas a archivo
        /// </summary>
        public void SaveTrazas()
        {
            try
            {
                var file = new FileStream(CurrentDirectory + "\\" + "temp", FileMode.Create, FileAccess.Write);
                var writer = new StreamWriter(file);
                foreach (var traza in Trazas)
                {
                    writer.WriteLine(traza.StringTraza);
                    writer.WriteLine("*");
                }
                writer.Close();
                // Encrypt the file.        
                security.EncryptFile(CurrentDirectory + "\\" + "temp",
                   CurrentDirectory + "\\" + FileDirTrazas,
                   skey);
                File.Delete(CurrentDirectory + "\\" + "temp");
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// Agrega una cancion a la traza actual
        /// </summary>
        /// <param name="track"></param>
        public void AddTrack(string track)
        {
            if(Trazas.Count>0)
                Trazas.Last().Add(track,ValueTrack);
            SaveTrazas();
        }
        /// <summary>
        /// Agrega un tema que no se pudo reproducir y lo elimina de la lista correspondiente
        /// </summary>
        /// <param name="track"></param>
        public void AddWrong(string track)
        {
            if (Trazas.Count > 0)
                Trazas.Last().AddWrong(track);
            SaveTrazas();
        }
        /// <summary>
        /// Elimina las count dias de trazas mas viejas
        /// </summary>
        /// <param name="count">cantidad de trazas viejas a eliminar</param>
        public void DeleteTrazas(int count)
        {
            if(Trazas!=null)
            {
                if(count>Trazas.Count)
                    Trazas.Clear();
                else Trazas.RemoveRange(Trazas.Count-count,count);

                var number = 0;
                if(Trazas.Count>0)
                    number = Trazas.Last().Number+1;
                Trazas.Add(new Traza(DateTime.Now, number));
                SaveTrazas();
            }
        }
        #endregion

        #endregion
        #region Notificaciones

        private string _selectImage;
        /// <summary>
        /// Direccion de la imagen del artista a mostrar
        /// </summary>
        public string SelectImage
        {
            get { return _selectImage; }
            set
            {
                if(_selectImage!=value)
                {
                    _selectImage = value;
                    NotifyChanged("SelectImage");
                }
            }
        }
        private string _selectIcon;
        /// <summary>
        /// Direccion del icono de la bandera del artista
        /// </summary>
        public string SelectIcon
        {
            get {   return _selectIcon; }
            set
            {
                if (_selectIcon != value)
                {
                    _selectIcon = value;
                    NotifyChanged("SelectIcon");
                }
            }
        }
        private string _selectCountry;
        /// <summary>
        /// Nombre del pais del artista
        /// </summary>
        public string SelectCountry
        {
            get { return _selectCountry; }
            set
            {
                if (_selectCountry != value)
                {
                    _selectCountry = value;
                    NotifyChanged("SelectCountry");
                }
            }
        }
        private double _money;
        /// <summary>
        /// cantidad de dinero acumulado en la nicotrola
        /// </summary>
        public double Money
        {
            get { return _money; }
            set
            {
                if(_money!=value)
                {
                    _money = value;
                    NotifyChanged("Money");
                }
            }
        }

        public ObservableCollection<string> Genres { get; set; }
        string _selectedGenre;
        /// <summary>
        /// Género que se está explorando
        /// </summary>
        public string SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                if (_selectedGenre != value)
                {
                    _selectedGenre = value;
                   
                    NotifyChanged("SelectedGenre");
                }
            }
        }
        public void DoAfterSelectGenre()
        {
            try
            {
                if (_selectedGenre != null && BackGrounds.ContainsKey(_selectedGenre))
                    BackGround = BackGrounds[_selectedGenre];
                //else BackGround = Brushes.MediumOrchid;
                var temp = GenArtTrack[_selectedGenre];
                Artists = new ObservableCollection<string>(temp.Keys);
                UpdateCollectionMiddle(TypesList.Artist);
            }
            catch
            {
                return;
            }
        }
        public ObservableCollection<string> Artists { get; set; }
        string _selectedArtist;
        /// <summary>
        /// Artista que se está explorando
        /// </summary>
        public string SelectedArtist
        {
            get { return _selectedArtist; }
            set
            {
                if (_selectedArtist != value)
                {
                    _selectedArtist = value;
                   
                    NotifyChanged("SelectedArtist");
                }
            }
        }
        /// <summary>
        /// Actualiza la lista de track,foto-bandera-artista luego de seleccionar un artista
        /// </summary>
        public void DoAfterSelectArtist()
        {
            try
            {
                Tracks = new ObservableCollection<string>(GenArtTrack[_selectedGenre][_selectedArtist].Item1);
                DirTracks = new List<string>(GenArtTrack[_selectedGenre][_selectedArtist].Item2);

                UpdateCollectionMiddle(TypesList.Track);
                var temp = GenArtTrack[_selectedGenre][_selectedArtist].Item3;
                if (temp == "")
                    temp = CurrentDirectory + "\\" + DirImage;
                SelectImage = temp;
                temp = GenArtTrack[_selectedGenre][_selectedArtist].Item4;
                if (File.Exists(temp))
                {
                    SelectIcon = temp;
                    SelectCountry = temp.Split(new char[] { '\\' }).Last().Split(new char[] { '.' }).First();
                } //NotifyChanged("Tracks"); 
                NotifyChanged("SelectImage");
            }
            catch
            {
                return;
            }
        }
        public ObservableCollection<string> Tracks { get; set; }

        int _selectedIndexTrack;
        /// <summary>
        /// Track que se está explorando
        /// </summary>
        public int SelectedIndexTrack
        {
            get { return _selectedIndexTrack; }
            set
            {
                if (_selectedIndexTrack != value)
                {
                    _selectedIndexTrack = value;
                    NotifyChanged("SelectedIndexTrack");
                }
                
            }
        }

        private string _publicity;
        /// <summary>
        /// Publicidad del Programa
        /// </summary>
        public string Publicity
        {
            get { return _publicity; }
            set
            {
                if(_publicity!=value)
                {
                    _publicity = value;
                    NotifyChanged("Publicity");
                }
            }
        }

        private Duration durationmElement;
        /// <summary>
        /// Duracion del tema que se esta tocando
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
        private int _width;
        /// <summary>
        /// Ancho del listbox
        /// </summary>
        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    NotifyChanged("Width");
                }
            }
        }

        private double _heightLabel;
        ///// <summary>
        ///// Altura del espacio de los items de los listbox asociados a los generos,artistas,tracks
        ///// </summary>
        //public double HeightLabel
        //{
        //    get { return _heightLabel; }
        //    set
        //    {
        //        if(_heightLabel!=value)
        //        {
        //            _heightLabel = value;
        //            //NotifyChanged("HeightLabel");
        //        }
        //    }
        //}
        private double _fontsizeLListBox;
        /// <summary>
        /// Tamaño de letra de los item de los listbox, define cuantos elementos caben en la vista
        /// </summary>
        public double FontsizeLListBox
        {
            get { return _fontsizeLListBox; }
            set
            {
                if (_fontsizeLListBox != value)
                {
                    _fontsizeLListBox = value;
                    NotifyChanged("FontsizeLListBox");
                    FontsizeIListBoxSelect = _fontsizeLListBox + 15;
                    HeightIListBox = _fontsizeLListBox + 25;
                }
            }
        }
        private double _heightIListBox;
        /// <summary>
        /// Alto de los item seleccionados de los listboxs
        /// </summary>
        public double HeightIListBox
        {
            get { return _heightIListBox; }
            set
            {
                if (_heightIListBox != value)
                {
                    _heightIListBox = value;
                    NotifyChanged("HeightIListBox");
                }
            }
        }

        private double _fontsizeIListBoxSelect;
        /// <summary>
        /// Tamaño de letra de los item seleccionados de los listboxs
        /// </summary>
        public double FontsizeIListBoxSelect
        {
            get { return _fontsizeIListBoxSelect; }
            set
            {
                if (_fontsizeIListBoxSelect != value)
                {
                    _fontsizeIListBoxSelect = value;
                    NotifyChanged("FontsizeIListBoxSelect");
                    HeightIListBoxSelct = _fontsizeIListBoxSelect + 15;
                    //HeightLabel=_fontsizeIListBoxSelect+10;
                }
            }
        }
        private double _heightIListBoxSelct;
        /// <summary>
        /// Alto de los item seleccionados de los listboxs
        /// </summary>
        public double HeightIListBoxSelct
        {
            get { return _heightIListBoxSelct; }
            set
            {
                if (_heightIListBoxSelct != value)
                {
                    _heightIListBoxSelct = value;
                    NotifyChanged("HeightIListBoxSelct");
                }
            }
        }
        private int _widthBorder;
        /// <summary>
        /// Ancho del borde de los listbox
        /// </summary>
        public int WidthBorder
        {
            get { return _widthBorder; }
            set
            {
                if (_widthBorder != value)
                {
                    _widthBorder = value;
                    NotifyChanged("WidthBorder");
                }
            }
        }
        private Brush _background;
        /// <summary>
        /// Fondo del programa relativo a la seleccion del genero
        /// </summary>
        public Brush BackGround
        {
            get { return _background; }
            set
            {
                if (_background != value)
                {
                    _background = value;
                    NotifyChanged("BackGround");
                }
            }
        }
        public void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        
        #endregion 
    }
}
