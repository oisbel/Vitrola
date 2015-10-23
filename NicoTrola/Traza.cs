using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace NicoTrola
{
    /// <summary>
    /// Representa las trazas de un dia(contador de canciones y de monedas)
    /// </summary>
    public class Traza
    {
        /// <summary>
        /// Fecha de la que se tiene almacenada la informacion de los reportes
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Fecha corta de la que se tiene almacenada la informacion de los reportes
        /// </summary>
        public string DateShort { get; set; }
        /// <summary>
        /// Identifica a la traza por un numero en caso de que la fecha dada por el ordenador este mal
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Lista de canciones tocadas
        /// </summary>
        public List<string> Tracks { get; set; }
        /// <summary>
        /// Cantidad de canciones tocadas
        /// </summary>
        public int CountC
        {
            get { return Tracks.Count; }
        }
        /// <summary>
        /// Cantidad de canciones erroneas
        /// </summary>
        public int CountE
        {
            get { return Wrongs.Count; }
        }
        /// <summary>
        /// Cantidad de monedas ingresadas
        /// </summary>
        public int CountM { get; set; }
        /// <summary>
        /// Cantidad de dinero ingresado
        /// </summary>
        public double CountMoney { get; set; }
        /// <summary>
        /// Especifica las canciones que no se pudieron reproducir
        /// </summary>
        public List<string> Wrongs { get; set; }
        /// <summary>
        /// Crea una nueva traza para el dia:date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="number"></param>
        public Traza(DateTime date,int number)
        {
            Date = date;
            Tracks=new List<string>();
            Wrongs=new List<string>();
            Number = number;
            DateShort = Date.ToShortDateString();
        }
        /// <summary>
        ///Crea una traza pasada cargando los datos de la traza de un dia desde un texto
        /// </summary>
        /// <param name="data"></param>
        public Traza(string data)
        {
            Tracks = new List<string>();
            Wrongs = new List<string>();
            //12/04/2014     --->fecha
            // 1             --->numero de la traza
            //90              --->cantidad de monedas ingresadas
            //45              --->Dinero ingresado
            //6              --->cantidad de temas tocados
            //pop/britney/oh baby          --->cada uno de los nombres de los temas   
            //perro
            //barby
            //oisbel
            //adis
            //ani
            //3              --->cantidad de temas que dieron problemas
            //pop/britney/oh baby          --->cada uno de los nombres de los temas   
            //perro
            //barby
            
            try
            {
                var cad = data.Split(new string[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
                var d = int.Parse(cad[0].Substring(0, 2));
                var m = int.Parse(cad[0].Substring(3, 2));
                var y = int.Parse(cad[0].Substring(6, 4));
                Date = new DateTime(y, m, d);
                DateShort = Date.ToShortDateString();
                Number = int.Parse(cad[1]);
                CountM = int.Parse(cad[2]);
                CountMoney = double.Parse(cad[3]);
                var count = int.Parse(cad[4]);
                if (count > 0)
                    for (int i = 5; i < count + 5; i++)
                    {
                        Tracks.Add(cad[i]);
                    }
                count = int.Parse(cad[5+count]);
                if (count > 0)
                    for (int i = 6+Tracks.Count; i < count + 6+Tracks.Count; i++)
                    {
                        Wrongs.Add(cad[i]);
                    }

            }
            catch
            {
                Date = DateTime.Now;
                Tracks = new List<string>();
            }
        }
        /// <summary>
        /// Agrega un nuevo tema con el precio asociado
        /// </summary>
        /// <param name="track"></param>
        /// <param name="money"></param>
        public void Add(string track,double money)
        {
            Tracks.Add(track);
            CountM++;
            CountMoney += money;
        }

        /// <summary>
        /// Devuelve los datos de la traza para que sean guardados en archivo
        /// </summary>
        /// <returns></returns>
        public string StringTraza { 
            get
            {
                var result = "";
                result += Date.ToShortDateString() + "\n";
                result += Number.ToString() + "\n";
                result += CountM + "\n";
                result += CountMoney + "\n";
                result += CountC + "\n";
                foreach (var track in Tracks)
                {
                    result += track + "\n";
                }
                result += CountE + "\n";
                foreach (var wrong in Wrongs)
                {
                    result += wrong + "\n";
                }
                return result;
            } 
        }
        /// <summary>
        /// Lista de temas tocados en forma de cadena
        /// </summary>
        /// <returns></returns>
        public string TracksOk
        {
            get
            {
                var result = "";

                foreach (var track in Tracks)
                {
                    result += track + "\n";
                }
                return result;
            }
        }
        /// <summary>
        /// Lista de temas que fallaron al tocar en forma de cadena
        /// </summary>
        /// <returns></returns>
        public string TracksFail
        {
            get
            {
                var result = "";
                foreach (var track in Wrongs)
                {
                    result += track + "\n";
                }
                return result;
            }
        }
        /// <summary>
        /// Elimina la cancion que dio error y la pasa para Wrongs
        /// </summary>
        /// <param name="track"></param>
        public void AddWrong(string track)
        {
            if(Tracks.Contains(track))
            {
                Tracks.Remove(track);
                Wrongs.Add(track);
            }
        }
    }
}
