using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using Button = System.Windows.Forms.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para Configuration.xaml
    /// </summary>
    public partial class Configuration : INotifyPropertyChanged
    {
        private readonly Vitrola _vitrola;
        private SortedList<string, SortedList<string, Tuple<List<string>, List<string>, string,string>>> GenArtTrack { get; set; }

        public string CountGenre
        {
            get
            {
                int count = Genres != null ? Genres.Count : 0;
                return "Géneros Musicales (" + count.ToString() + ")";
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
        ///Imagenes a eliminar(en archivo) 
        /// </summary>
        private List<string> DeleteImages { get; set; } 

        /// <summary>
        /// Asociacion de colores de fondo de cada genero
        /// </summary>
        public SortedList<string, SolidColorBrush> ColorsBackGround { get; set; }

        /// <summary>
        /// Asociacion de imagenes de fondo de cada genero
        /// </summary>
        public SortedList<string, string> ImagesBackGround { get; set; }

        public ObservableCollection<string> Filters { get; set; }
       
        /// <summary>
        /// Direccion de la imagen a previsualiazr
        /// </summary>
        public string SelectImage { get; set; }
        
        public Configuration(Vitrola vitrola)
        {
            InitializeComponent();
            _vitrola = vitrola;
            viewMiniatureCB.IsChecked = _vitrola.ViewMiniature;
            viewScreenSaverVideoCB.IsChecked = _vitrola.ViewVideo;
            fontsizeLListBox.Text = _vitrola.FontsizeLListBox.ToString();
            DataContext = this;
            DeleteImages=new List<string>();
            Filters = new ObservableCollection<string>(vitrola.Filters);
            tempFilters = new List<string>(vitrola.Filters);
            valuePlayVideoTB.Text = vitrola.ValueTrack.ToString();
            delayMin.Text = vitrola.DelayMin.ToString();
            delaySeg.Text = vitrola.DelaySeg.ToString();

            if (vitrola.Genres == null)
                return;
            Genres = new ObservableCollection<string>(vitrola.Genres);
            if (Genres != null && Genres.Count > 0)
            {
                SelectedGenre = Genres.First();
            }
            Artists = new ObservableCollection<string>(vitrola.Artists);
            if (Artists != null && Artists.Count > 0)
                SelectedArtist = Artists.First();

            if (vitrola.GenArtTrack == null)
                GenArtTrack =
                new SortedList<string, SortedList<string, Tuple<List<string>, List<string>, string,string>>>();
            else
                GenArtTrack =
                new SortedList<string, SortedList<string, Tuple<List<string>, List<string>, string,string>>>(_vitrola.GenArtTrack);

            InicializeImagenGenre();
           
            dataGrid.ItemsSource = vitrola.Trazas;
        }

        public Configuration(Vitrola vitrola, bool filter)
            : this(vitrola)
        {
            tabControl1.SelectedIndex = 3;
        }
        /// <summary>
        /// Inicializa los fondos de los generos(se invoca al inicio)
        /// </summary>
        private void InicializeImagenGenre()
        {
            if (Genres == null || Genres.Count == 0)
            {
                ImagesBackGround = new SortedList<string, string>();
                ColorsBackGround = new SortedList<string, SolidColorBrush>();
                SelectImage = "";
                SelectColor = Brushes.Transparent;
                return;
            }
            ImagesBackGround = new SortedList<string, string>(Genres.Count);
            ColorsBackGround = new SortedList<string, SolidColorBrush>(ImagesBackGround.Count);

            foreach (string g in Genres)
            {
                ColorsBackGround.Add(g, Brushes.Transparent);
            }
            ImagesBackGround = new SortedList<string, string>(_vitrola.BackGroundsStrings);
            SortedList<string, byte[]> temp =
                 Util.DeserializeBrushes(_vitrola.CurrentDirectory + _vitrola.DirBackGround + "background");
            if (temp != null)
                ColorsBackGround = Util.StringToBrush(temp);

            SelectedGenreImage = SelectedGenre; //dentro se llama al Setimage()
        }
        #region Edición de Géneros

        /// <summary>
        /// Actualiza la coleccion de GenreArtistTrack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateColletionClick(object sender, RoutedEventArgs e)
        {
            string temp = "";
            try
            {
                temp = GenArtTrack.First().Value.First().Value.Item2.First();
                var temp1 = temp.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
                temp = temp1.Take(temp1.Length - 3).Aggregate("", (current, s) => current + (s + "\\"));
            }
            catch
            {
                AddCollectionClick(sender, e);
                return;
            }
            AddColletionAux(temp);
        }

        /// <summary>
        /// Agrega la coleccion entera a la vitrola
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCollectionClick(object sender, RoutedEventArgs e)
        {
            var fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddColletionAux(fd.SelectedPath);
            }
        }

        private void AddColletionAux(string dir)
        {
            GenArtTrack = Util.DoCollection(dir);
            if (GenArtTrack == null || GenArtTrack.Count == 0)
                return;
            Genres = new ObservableCollection<string>(GenArtTrack.Keys);
            if (Genres != null && Genres.Count != 0)
                SelectedGenre = Genres.First();
            Artists = new ObservableCollection<string>(GenArtTrack.First().Value.Keys);
            if (Artists != null && Artists.Count != 0)
                SelectedArtist = Artists.First();
            NotifyChanged("Genres");
            if(ColorsBackGround==null)
                ColorsBackGround=new SortedList<string, SolidColorBrush>();
            ColorsBackGround.Clear();
            foreach (string g in Genres)
                ColorsBackGround.Add(g, Brushes.Transparent);
            if (ImagesBackGround == null)
                ImagesBackGround=new SortedList<string, string>();
            foreach (var genre in Genres)
            {
                if (!ImagesBackGround.ContainsKey(genre))
                    ImagesBackGround.Remove(genre);
            }
        }

        /// <summary>
        /// Agrega un nuevo género a la coleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddGenreClick(object sender, RoutedEventArgs e)
        {
            var addgenre = new AddGenre();
            addgenre.ShowDialog();
            if (addgenre.NameGenre != null && !GenArtTrack.ContainsKey(addgenre.NameGenre))
            {
                try
                {
                    GenArtTrack.Add(addgenre.NameGenre,
                                    new SortedList<string, Tuple<List<string>, List<string>, string,string>>());
                    Genres = new ObservableCollection<string>(GenArtTrack.Keys);
                    SelectedGenre = addgenre.NameGenre;
                }
                catch
                {
                    return;
                }
                NotifyChanged("Genres");
                NotifyChanged("CountGenre");
                NotifyChanged("CountArtist");
                ColorsBackGround.Add(addgenre.NameGenre,Brushes.Transparent);
            }
        }

        /// <summary>
        /// Elimina un género de la coleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteGenreClick(object sender, RoutedEventArgs e)
        {
            if (listBoxGenre.SelectedValue == null)
                return;
            var temp = SelectedGenre;
            Genres.Remove(temp);
            GenArtTrack.Remove(temp);
            if (Genres.Count > 0)
                SelectedGenre = Genres.First();
            else Artists.Clear();
            NotifyChanged("Genres");
            NotifyChanged("Artists");
            NotifyChanged("CountGenre");
            NotifyChanged("CountArtist");
            ColorsBackGround.Remove(temp);
            if (ImagesBackGround.ContainsKey(temp))
                ImagesBackGround.Remove(temp);
        }

        /// <summary>
        /// Elimina todos los géneros de la coleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllGenreClick(object sender, RoutedEventArgs e)
        {
            Genres.Clear();
            Artists.Clear();
            GenArtTrack.Clear();
            NotifyChanged("Genres");
            NotifyChanged("Artists");
            NotifyChanged("CountGenre");
            NotifyChanged("CountArtist");
            ColorsBackGround.Clear();
            ImagesBackGround.Clear();
        }

        #endregion

        #region Edición de Artistas

        /// <summary>
        /// Agrega un nuevo artista a la coleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddArtistClick(object sender, RoutedEventArgs e)
        {
            if (Genres.Count == 0)
                return;
            if (SelectedGenre == null)
                SelectedGenre = Genres.First();
            var fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Util.AddArtists(GenArtTrack, SelectedGenre, fd.SelectedPath);
                    var temp = GenArtTrack[SelectedGenre];
                    Artists = new ObservableCollection<string>(temp.Keys);
                    SelectedArtist = Artists.Last();
                }
                catch
                { 
                   return;
                }
            }
            NotifyChanged("Artists");
            NotifyChanged("CountGenre");
            NotifyChanged("CountArtist");
        }

        /// <summary>
        /// Elimina un artista de la coleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteArtistClick(object sender, RoutedEventArgs e)
        {
            if (listBoxArtist.SelectedValue == null)
                return;
            var temp = SelectedArtist;
            Artists.Remove(temp);
            GenArtTrack[_selectedGenre].Remove(temp);
            if (Artists.Count > 0)
                SelectedArtist = Artists.First();
            NotifyChanged("Genres");
            NotifyChanged("Artists");
            NotifyChanged("CountGenre");
            NotifyChanged("CountArtist");

        }

        /// <summary>
        /// Elimina todos los artistas de la coleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllArtistClick(object sender, RoutedEventArgs e)
        {
            if (SelectedGenre == null)
                return;
            Artists.Clear();
            if (GenArtTrack.ContainsKey(SelectedGenre))
                GenArtTrack[SelectedGenre].Clear();
            NotifyChanged("Artists");
            NotifyChanged("CountGenre");
            NotifyChanged("CountArtist");
        }

        #endregion

        #region Notificaciones

        public ObservableCollection<string> Genres { get; set; }
        private string _selectedGenre;

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
                    try
                    {
                        var temp = GenArtTrack[value];
                        Artists = new ObservableCollection<string>(temp.Keys);
                        NotifyChanged("Artists");
                    }
                    catch
                    {
                        return;
                    }
                    NotifyChanged("SelectedGenre");
                }
            }
        }

        private string _selectedGenreImage;

        /// <summary>
        /// Género que se está explorando para establecer wallspaper
        /// </summary>
        public string SelectedGenreImage
        {
            get { return _selectedGenreImage; }
            set
            {
                if (_selectedGenreImage != value)
                {
                    _selectedGenreImage = value;

                    if (_selectedGenreImage != null && ImagesBackGround.ContainsKey(_selectedGenreImage))
                    {
                        SelectImage = ImagesBackGround[_selectedGenreImage];
                        SetImage();
                    }
                    else
                    {
                        if (_selectedGenreImage != null && ColorsBackGround.ContainsKey(_selectedGenreImage))
                            SelectColor = ColorsBackGround[_selectedGenreImage];
                        else SelectColor = Brushes.Transparent;
                        //imageBackGround.Visibility = Visibility.Hidden;
                    }
                    NotifyChanged("SelectedGenreImage");
                }
            }
        }

        public ObservableCollection<string> Artists { get; set; }
        private string _selectedArtist;

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

        private SolidColorBrush _selectColor;

        /// <summary>
        /// Color del fondo del genero a mostrar
        /// </summary>
        public SolidColorBrush SelectColor
        {
            get { return _selectColor; }
            set
            {
                if (_selectColor != value)
                {
                    _selectColor = value;
                    
                    NotifyChanged("SelectColor");
                }
                imageBackGround.Visibility = Visibility.Hidden;
            }
        }

        public void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Guarda la coleccion GenArtTrack y las configuraciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _vitrola.GenArtTrack = GenArtTrack;
                _vitrola.Filters = new List<string>(tempFilters);
                _vitrola.DeleteImages.AddRange(DeleteImages);
                Util.Serialize(GenArtTrack,_vitrola.CurrentDirectory+"\\"+ _vitrola.FileDirCollection);
                _vitrola.UpdateCollection();
                double tempValueTrack;
                if (double.TryParse(valuePlayVideoTB.Text, out tempValueTrack))
                {
                    _vitrola.ValueTrack = tempValueTrack;
                    _vitrola.NotifyChanged("ValueTrack");
                }
                _vitrola.ViewMiniature = viewMiniatureCB.IsChecked.Value;
                _vitrola.ViewVideo = viewScreenSaverVideoCB.IsChecked.Value;
                _vitrola.Volumen = (int) slider1.Value;
                double tempfontsizeLListBox;
                if (double.TryParse(fontsizeLListBox.Text, out tempfontsizeLListBox))
                {
                    _vitrola.FontsizeLListBox = tempfontsizeLListBox;
                }
                int delayM;
                if (int.TryParse(delayMin.Text, out delayM))
                {
                    _vitrola.DelayMin = delayM;
                }
                int delayS;
                if (int.TryParse(delaySeg.Text, out delayS))
                {
                    _vitrola.DelaySeg = delayS;
                }
                _vitrola.UpdateConfiguration();
            }
            catch (Exception exe)
            {
                System.Windows.MessageBox.Show("Ha ocurrido un error al guardar configuracion, se recomienda reiniciar la vitrola:"+exe.Message);
            }
            Util.SerializeBrushes(Util.BrushToString(ColorsBackGround), _vitrola.CurrentDirectory + _vitrola.DirBackGround + "background");
            UpdateBackGroundVitrola();

            //guardando los cambios de fondos de generos
            foreach (var tempDirBackGroundImage in tempDirBackGroundImages)
            {
                if (_vitrola.DirBackGroundImages.ContainsKey(tempDirBackGroundImage.Key))
                    _vitrola.DirBackGroundImages[tempDirBackGroundImage.Key] = tempDirBackGroundImage.Value;
                else _vitrola.DirBackGroundImages.Add(tempDirBackGroundImage.Key, tempDirBackGroundImage.Value);
            }

            Close();
        }
        /// <summary>
        /// Actualiza los fondos de generos de la vitrola
        /// </summary>
        private void UpdateBackGroundVitrola()
        {
            var result = new SortedList<string, Brush>();
            _vitrola.BackGroundsStrings =new SortedList<string, string>(ImagesBackGround);

            foreach (var colorBrush in ColorsBackGround)
            {
                if (!result.ContainsKey(colorBrush.Key))
                {
                    result.Add(colorBrush.Key,colorBrush.Value);
                }
            }
            foreach (var image in ImagesBackGround)
            {
                if(File.Exists(image.Value))
                {
                    var imageDrawing = new ImageDrawing
                    {
                        Rect = new Rect(0, 0, 150, 100),
                        ImageSource = new BitmapImage(new Uri(image.Value))
                    };
                    if (result.ContainsKey(image.Key))
                        result[image.Key] = new DrawingBrush(imageDrawing);
                    else
                        result.Add(image.Key, new DrawingBrush(imageDrawing));
                }
            }
            _vitrola.BackGrounds = result;
            if(_vitrola.Genres==null || _vitrola.Genres.Count==0)
                return;
            _vitrola.SelectedGenre = _vitrola.Genres.First();
            _vitrola.BackGround = _vitrola.BackGrounds[_vitrola.SelectedGenre];
        }
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            cancelEvent();
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("EvaPlay (version 1.0)");
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                cancelEvent();
            }
        }
        void cancelEvent()
        {
            _vitrola.UpdateCollection();
            Close();
        }
        private void listBoxGenre_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteGenreClick(null, null);
            }
        }

        private void listBoxArtist_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteArtistClick(null, null);
            }
        }

        /// <summary>
        /// Agrega filtros al coleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddClick(object sender, RoutedEventArgs e)
        {
            if (textBoxFilter.Text != "")
            {
                var temp = textBoxFilter.Text;
                if (temp.StartsWith("."))
                    temp.Remove(0, 1);
                if (temp.Length > 0)
                {
                    Filters.Add(temp);
                    tempFilters.Add(temp);
                }
            }
            textBoxFilter.Text = "";
            NotifyChanged("Filters");
            if (listBoxFilters.Items.Count > 0)
                listBoxFilters.ScrollIntoView(listBoxFilters.Items[listBoxFilters.Items.Count - 1]);

        }

        /// <summary>
        /// Para controlar los filtros que son deseleccionados y seleccionados
        /// </summary>
        private List<string> tempFilters;

        private void checkBoxFilter_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = (System.Windows.Controls.CheckBox) sender;
            if (!tempFilters.Contains(checkBox.Content.ToString()))
                tempFilters.Add(checkBox.Content.ToString());
        }

        private void checkBoxFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = (System.Windows.Controls.CheckBox) sender;
            if (tempFilters.Contains(checkBox.Content.ToString()))
                tempFilters.Remove(checkBox.Content.ToString());
        }
        SortedList<string,string> tempDirBackGroundImages=new SortedList<string, string>(); 
        private void seachPicture_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            var fd = new OpenFileDialog();
            fd.Filter = "Imágenes(*.BMP;*.JPG;*PNG)|*.BMP;*.JPG;*.PNG";
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var name = fd.FileName.Split(new char[] {'\\'}).Last();
                var dir = _vitrola.CurrentDirectory + _vitrola.DirBackGround;
                Directory.CreateDirectory(dir);
                
                //verificando que el archivo de imagen no ya no se halla seleccionado(se crea otro)
                if (File.Exists(dir + name))
                {
                    while (File.Exists(dir + name))
                    {
                        name="1"+name;
                    }
                }
                if (imageBackGround.Source is BitmapImage)
                {
                    ((BitmapImage) imageBackGround.Source).StreamSource.Dispose();
                }
                int count = 0;
                while (true)
                {
                    try
                    {
                        File.Copy(fd.FileName, dir +name , true);
                        break;
                    }
                    catch
                    {
                        if (count >= 20)
                        {
                            System.Windows.MessageBox.Show(
                                "Problema al cargar la imagen.Si no fué satisfactorio inténtelo de nuevo por favor.");
                            break;
                        }
                        count++;
                        continue;
                    }
                }
                if (ImagesBackGround.ContainsKey(SelectedGenreImage))
                {
                    DeleteImages.Remove(ImagesBackGround[SelectedGenreImage]);
                    ImagesBackGround[SelectedGenreImage] = dir + name;
                }
                else
                    ImagesBackGround.Add(SelectedGenreImage, dir + name);

                if (tempDirBackGroundImages.ContainsKey(SelectedGenreImage))
                    tempDirBackGroundImages[SelectedGenreImage] = dir + name;
                else tempDirBackGroundImages.Add(SelectedGenreImage, dir + name);

                SelectImage = dir + name;
                SetImage();
            }
        }

        /// <summary>
        /// Establece dir al UriSource del bitmapimage de imageBackGround
        /// </summary>
        private void SetImage()
        {
            BitmapImage image = null;
            if (!File.Exists(SelectImage))
            {
                if (ColorsBackGround.ContainsKey(SelectedGenreImage))
                    SelectColor = ColorsBackGround[SelectedGenreImage];
                else 
                    SelectColor = Brushes.Transparent;
                imageBackGround.Visibility = Visibility.Hidden;
                return;
            }
            imageBackGround.Visibility = Visibility.Visible;
            _selectColor = Brushes.Transparent;
            NotifyChanged("SelectColor");
            image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new FileStream(SelectImage, FileMode.Open, FileAccess.Read);
            image.EndInit();
            //Pongo la imagen como fuente
            imageBackGround.Source = image;
        }

        /// <summary>
        /// Establece un color para el fondo del genero seleccionado
        ///  y añade a una lista la imagen para mas tarde eliminarla, si existiera
        /// </summary>
        private void SetColor()
        {
            if (SelectedGenreImage==null)
                return;
            if (ImagesBackGround.ContainsKey(SelectedGenreImage))
            {
                if(ImagesBackGround[SelectedGenreImage].EndsWith(SelectedGenreImage))
                    DeleteImages.Add(ImagesBackGround[SelectedGenreImage]);
                ImagesBackGround.Remove(SelectedGenreImage);
            }
            if (tempDirBackGroundImages.ContainsKey(SelectedGenreImage))
                tempDirBackGroundImages.Remove(SelectedGenreImage);

            if (ColorsBackGround.ContainsKey(SelectedGenreImage))
                SelectColor=ColorsBackGround[SelectedGenreImage];
        }

        private void groupBoxImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            seachPicture_Click(null, null);
        }

        /// <summary>
        /// Boton ..mas para explorar mas colores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            var color = new ColorDialog();
            if (color.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ColorsBackGround[SelectedGenreImage] = new SolidColorBrush(Color.FromArgb
                                                                               (color.Color.A, color.Color.R,
                                                                                color.Color.G, color.Color.B));
                SetColor();
            }

        }

        private void MediumOrchid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            SetColortransparent();
            MediumOrchid.BorderBrush = Brushes.Tomato;
            ColorsBackGround[SelectedGenreImage] = Brushes.MediumOrchid;
            SetColor();
        }

        private void Black_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            SetColortransparent();
            Black.BorderBrush = Brushes.Tomato;
            ColorsBackGround[SelectedGenreImage] = Brushes.Black;
            SetColor();
        }

        private void MistyRose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            SetColortransparent();
            MistyRose.BorderBrush = Brushes.Tomato;
            ColorsBackGround[SelectedGenreImage] = Brushes.MistyRose;
            SetColor();
        }

        private void DeepPink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            SetColortransparent();
            DeepPink.BorderBrush = Brushes.Tomato;
            ColorsBackGround[SelectedGenreImage] = Brushes.DeepPink;
            SetColor();
        }

        private void YellowGreen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            SetColortransparent();
            YellowGreen.BorderBrush = Brushes.Tomato;
            ColorsBackGround[SelectedGenreImage] = Brushes.YellowGreen;
            SetColor();
        }

        private void MediumPurple_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            SetColortransparent();
            MediumPurple.BorderBrush = Brushes.Tomato;
            ColorsBackGround[SelectedGenreImage] = Brushes.MediumPurple;
            SetColor();
        }

        private void MediumBlue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            SetColortransparent();
            MediumBlue.BorderBrush = Brushes.Tomato;
            ColorsBackGround[SelectedGenreImage] = Brushes.MediumBlue;
            SetColor();
        }

        private void Tomato_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedGenreImage == null)
                return;
            SetColortransparent();
            Tomato.BorderBrush = Brushes.Tomato;
            ColorsBackGround[SelectedGenreImage] = Brushes.Tomato;
            SetColor();
        }

        private void SetColortransparent()
        {
            MediumOrchid.BorderBrush = Brushes.Transparent;
            Black.BorderBrush = Brushes.Transparent;
            MistyRose.BorderBrush = Brushes.Transparent;
            DeepPink.BorderBrush = Brushes.Transparent;
            YellowGreen.BorderBrush = Brushes.Transparent;
            MediumPurple.BorderBrush = Brushes.Transparent;
            MediumBlue.BorderBrush = Brushes.Transparent;
            Tomato.BorderBrush = Brushes.Transparent;

        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetColortransparent();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }
        private void DataGrid_Expanded(object sender, RoutedEventArgs e)
        {
            dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }
        private void DataGrid_Collapse(object sender, RoutedEventArgs e)
        {
            dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }
        /// <summary>
        /// Elimina las ultimas trazas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var count = 7;
            try
            {
                count = int.Parse(countTrazasDel.Text);
            }
            catch{}
            _vitrola.DeleteTrazas(count);
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = _vitrola.Trazas;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }

}
