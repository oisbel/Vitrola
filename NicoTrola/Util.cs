using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization;
using System.Windows.Media;

namespace NicoTrola
{
    /// <summary>
    /// Utilidades para serializar colecciones para manejar el contenido de la vitrola
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Extensiones de archivos que se van a reproducir
        /// </summary>
        public static List<string> Filters;
        /// <summary>
        /// Serializa una lista de string
        /// </summary>
        /// <param name="dir"></param>
        public static void SerializeList(List<string> list, string dir)
        {
            try
            {
                using (FileStream file = File.Open(dir, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(file, list);
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Serializa una sortedlist de string
        /// </summary>
        /// <param name="dir"></param>
        public static void SerializeList(SortedList<string,string> list, string dir)
        {
            try
            {
                using (FileStream file = File.Open(dir, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(file, list);
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Serializa la coleccion de pares genero-color
        /// </summary>
        /// <param name="brushes"></param>
        /// <param name="fileDir">archivo donde se serializará</param>
        public static void SerializeBrushes(SortedList<string, byte[]> brushes, string fileDir)
        {
            try
            {
                using (FileStream file = File.Open(fileDir, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(file, brushes);
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Deserializa una lista de string
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static List<string> DeserializeList(string dir)
        {
            List<string> result = null;
            try
            {
                using (FileStream file = File.Open(dir, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    result = (List<string>)bf.Deserialize(file);
                }
            }
            catch
            {
                return null;
            }
            return result;
        }
        /// <summary>
        /// Deserializa una sortedlist de string
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static SortedList<string,string> DeserializeSortedList(string dir)
        {
            SortedList<string, string> result = null;
            try
            {
                using (FileStream file = File.Open(dir, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    result = (SortedList<string,string>)bf.Deserialize(file);
                }
            }
            catch
            {
                return null;
            }
            return result;
        }
        /// <summary>
        /// Deserializa la coleccion de genre-color
        /// </summary>
        /// <param name="fileDir"></param>
        /// <returns></returns>
        public static SortedList<string, byte[]> DeserializeBrushes(string fileDir)
        {
            if (!File.Exists(fileDir))
                return null;
            var text = File.ReadAllText(fileDir);
            if (text == "")
                return null;
            SortedList<string, byte[]> result = null;
            try
            {
                using (FileStream file = File.Open(fileDir, FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    result = (SortedList<string, byte[]>)bf.Deserialize(file);
                }
            }
            catch
            {
                return null;
            }
            return result;
        }
        /// <summary>
        /// Convierte las cadenas serialiadas de brochas a brochas
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static SortedList<string, Brush> ByteToBrush(SortedList<string, byte[]> s)
        {
            var result = new SortedList<string, Brush>();
            foreach (var bytese in s)
            {
                result.Add(bytese.Key, new SolidColorBrush(new Color() { A = bytese.Value[0], R = bytese.Value[1], G = bytese.Value[2], B = bytese.Value[3] }));
            }
            return result;
        }
        /// <summary>
        /// Convierte las cadenas serialiadas de brochas a brochas
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static SortedList<string, SolidColorBrush> StringToBrush(SortedList<string, byte[]> s)
        {
            var result = new SortedList<string, SolidColorBrush>();
            foreach (var bytese in s)
            {
                result.Add(bytese.Key, new SolidColorBrush(new Color() { A = bytese.Value[0], R = bytese.Value[1], G = bytese.Value[2], B = bytese.Value[3] }));
            }
            return result;
        }
        /// <summary>
        /// Convierte las brochas en su bytes
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SortedList<string, byte[]> BrushToString(SortedList<string, SolidColorBrush> colorsBackGround)
        {
            var result = new SortedList<string, byte[]>();
            foreach (var c in colorsBackGround)
            {
                result.Add(c.Key, new byte[] { c.Value.Color.A, c.Value.Color.R, c.Value.Color.G, c.Value.Color.B });
            }
            return result;
        }
        /// <summary>
        /// Serializa la colección(Guardar)
        /// </summary>
        /// <param name="list">coleccion que se guardará en el archivo</param>
        /// <param name="fileDir">archivo donde se serializará</param>
        public static void Serialize(SortedList<string, SortedList<string, Tuple<List<string>, List<string>,string,string>>> list, string fileDir)
        {
            try
            {
                using (FileStream file=File.Open(fileDir,FileMode.OpenOrCreate,FileAccess.Write))
                {
                    BinaryFormatter bf= new BinaryFormatter();
                    bf.Serialize(file,list);
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Deserializa una colección(Cargar)
        /// </summary>
        /// <param name="fileDir">Archivo donde está el objeto</param>
        /// <returns></returns>
        public static SortedList<string, SortedList<string, Tuple<List<string>, List<string>,string,string>>> Deserialize(string fileDir)
        {
            if(!File.Exists(fileDir))
                return null;
            var text = File.ReadAllText(fileDir);
            if(text=="")
                return null;
            SortedList<string, SortedList<string, Tuple<List<string>, List<string>,string,string>>> result = null;
            try
            {
                using(FileStream file=File.Open(fileDir,FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    result = (SortedList<string, SortedList<string, Tuple<List<string>, List<string>,string,string>>>)bf.Deserialize(file);
                }
            }
            catch
            {
                return null;
            }
            return result;
        }
        
        /// <summary>
        /// Agrega direcciones de canciones con sus respectivas subcarpetas de manera genero:artista:track,
        /// es el metodo mas general para crear la coleccion
        /// </summary>
        /// <param name="dir">direcion de la carpeta contenedora de los archivos de video</param>
        /// <returns>collecion:género-artista-track</returns>
        public static SortedList<string, SortedList<string, Tuple<List<string>,List<string>,string,string>>> DoCollection(string dir)
        {
            if(!Directory.Exists(dir))
            {
                return null;
            }
            var result = new SortedList<string, SortedList<string, Tuple<List<string>,List<string>,string,string>>>();
            
            DoCollectionAuxiliar(result,dir,1);
           
            return result;
        }
        /// <summary>
        /// Agrega informacion a la coleccion genero-artista-track segun el valor de dep
        /// </summary>
        /// <param name="result">coleccion</param>
        /// <param name="dir">direccion de la carpeta contenedora de los archivos de videoram</param>
        /// <param name="dep">1:genero,2:artista,3:tracks</param>
        static void DoCollectionAuxiliar(SortedList<string, SortedList<string, Tuple<List<string>,List<string>,string,string>>> result,
            string dir, int dep)
        {
            string genre = "", artist = "";
            if(dep>2)//agregando tracks
            {
                var temp = dir.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    genre = temp[temp.Length - 2];
                    artist = temp[temp.Length - 1];
                }
                catch
                {
                    return;
                }
                foreach (var file in Directory.GetFiles(dir,"*",SearchOption.AllDirectories))
                {
                    var s = file.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    var t = s.Last().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last();
                    if (!Filters.Contains(t.ToLower()))
                    {
                        continue;
                    }
                    try
                    {
                        result[genre][artist].Item1.Add(s.Last().Replace("."+t,""));
                        result[genre][artist].Item2.Add(file);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            else
            {
                foreach (var directory in Directory.GetDirectories(dir))
                {
                    try
                    {
                        switch (dep)
                        {
                            case 1://agregando géneros
                                genre = directory.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();
                                if (result == null)
                                    result = new SortedList<string, SortedList<string,Tuple<List<string>,List<string>,string,string>>>();
                                result.Add(genre, new SortedList<string,Tuple<List<string>,List<string>,string,string>>());
                                DoCollectionAuxiliar(result, directory, 2);
                                break;
                            default://agregando artistas
                                var temp = directory.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                                genre = temp[temp.Length - 2];
                                artist = temp.Last();
                                string photo = "";
                                string flag = "";
                                foreach (var file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
                                {
                                    var f = file.Substring(temp.Length);
                                    var t =
                                        f.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries).Last().ToLower();
                                    if (new List<string> {"jpg", "jpeg", "bmp", "png","gif"}.Contains(t))
                                    {
                                        photo = file;
                                    }
                                    if(t=="ico")
                                        flag = file;
                                }
                                result[genre].Add(artist, new Tuple<List<string>, List<string>,string,string>(new List<string>(),new List<string>(),photo,flag));
                                DoCollectionAuxiliar(result, directory, 3);
                                break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                } 
            }
        }
        /// <summary>
        /// Agrega un género a la coleccion,luego debe llamarce a AddArtists
        /// </summary>
        /// <param name="collection">coleccion genero-artista-track o vacia</param>
        /// <param name="nameGenre">nombre del género</param>
        public static void AddGenre(SortedList<string, SortedList<string, Tuple<List<string>,List<string>>>> collection,
            string nameGenre)
        {
            collection.Add(nameGenre,new SortedList<string, Tuple<List<string>, List<string>>>());
        }
        /// <summary>
        /// Agrega un género a la coleccion desde archivo
        /// </summary>
        /// <param name="collection">coleccion genero-artista-track o vacia</param>
        /// <param name="dir">direccion de la carpeta que contiene los artistas</param>
        public static void AddGenreFile(SortedList<string, SortedList<string, Tuple<List<string>, List<string>,string,string>>> collection,
            string dir)
        {
            if (!Directory.Exists(dir))
            {
                return ;
            }
            DoCollectionAuxiliar(collection,dir,2);
        }
        /// <summary>
        /// Agrega artistas a algun género de la coleccion (La coleccion debe tener algun genero)
        /// </summary>
        /// <param name="collection"> coleccion genero-artista-track</param>
        /// <param name="genre">genero al que se va a agregar el artista con sus track</param>
        /// <param name="dir">direccion de la carpeta</param>
        public static void AddArtists(SortedList<string, SortedList<string, Tuple<List<string>,List<string>,string,string>>> collection,
            string genre, string dir)
        {
            if (!Directory.Exists(dir) || collection==null ||collection.Count <= 0)
            {
                return;
            }
            var artist = dir.Split(new char[] {'\\'}).Last();
            var photo = "";
            var flag = "";
            var item1 = new List<string>();
            var item2 = new List<string>();
            foreach (var file in Directory.GetFiles(dir,"*",SearchOption.AllDirectories))
            {
                var f = file.Split(new char[] { '\\' }).Last();
                var t = f.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last().ToLower();
                var s = file.Split(new char[] { '\\', '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (!Filters.Contains(t))
                {
                    if (new List<string> { "jpg", "jpeg", "bmp", "png", "gif" }.Contains(t))
                    {
                        photo = file;
                    }
                    if(t=="ico")
                        flag = file;
                    continue;
                }
                try
                {
                    item1.Add(s[s.Length - 2]);
                    item2.Add(file);
                }
                catch
                {
                    continue;
                }
            }
            try
            {
                collection[genre].Add(artist, new Tuple<List<string>, List<string>, string,string>(item1, item2, photo,flag));
            }
            catch
            {
                return;
            }
            
        }
    }
}
