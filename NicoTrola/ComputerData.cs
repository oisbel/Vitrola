using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
//using System.Text;
//using System;
using System.Management;
using System.Windows;

namespace NicoTrola
{
    public class ComputerData
    {
        public List<string> L;
        private Security security;
        public ComputerData()
        {
            L = new List<string>();
            FillL();
            security = new Security();
            //var list = new List<string>();
            //list.Add(GetProcessorID()); list.Add("*");

            //list.Add(UserName()); list.Add("*");
            //list.Add(NumberHdd()); list.Add("*");
            //list.AddRange(OtherNumberHdd()); list.Add("*");
            //list.AddRange(MacsAddres());

            //File.WriteAllLines("caracteristicas de maquina del trabajo", list);
        }
        private void FillL()
        {
            L.Add("02");L.Add("86");L.Add("39");L.Add("24");L.Add("61");L.Add("54");
            L.Add("66");L.Add("16");L.Add("16");L.Add("64");L.Add("22");L.Add("73");
            L.Add("27");L.Add("73");L.Add("70");L.Add("26");L.Add("40");L.Add("77");
            L.Add("79");L.Add("08");L.Add("00");

        }
        /// <summary>
        /// Metodo que verifica si el software puede correr en la pc
        /// </summary>
        /// <param name="skeyp"></param>
        /// <returns></returns>
        public bool IsOkComputer(string skeyp,string currentDir)
        {
            var skey = skeyp.Replace('/', '3');
            if (!File.Exists(currentDir+"\\lic.lot"))
                return false;
           
            try
            {
                security = new Security();
                GCHandle gch = GCHandle.Alloc(skey, GCHandleType.Pinned);
                security.DecryptFile(currentDir+"\\lic.lot",currentDir+
                   "\\tempL",
                   skey);

                gch.Free();

                var lista = File.ReadAllText(currentDir+"\\tempL").Split(new char[] { '*' },
                                                             StringSplitOptions.RemoveEmptyEntries);
                File.Delete(currentDir+"\\tempL");

                var user = lista[0];
                var number = lista[1].Remove(0, 2);
                var otherNumbers = lista[2].Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var mac = new List<string>();
                if (lista.Length == 4)
                    mac = lista[3].Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                //empezamos a verificar

                var vUser = UserName();
                var vNumberHdd = NumberHdd();
                var vOtherNumberHdd = OtherNumberHdd();
                var vMacsAddres = MacsAddres();//puede no estar habilitada

                if (otherNumbers.Count == 0 && vNumberHdd == number)
                {
                    var list = new List<string>();
                    //list.Add(GetProcessorID()); list.Add("*");

                    list.Add(vUser + "*");
                    list.Add(vNumberHdd + "*");
                    list.AddRange(vOtherNumberHdd);
                    list.Add("*");
                    list.AddRange(vMacsAddres);

                    File.WriteAllLines("temp3", list);
                    skey = skey.Replace('/', '3');
                    GCHandle gch1 = GCHandle.Alloc(skey, GCHandleType.Pinned);

                    security.EncryptFile("temp3",
                       "lic.lot", skey);

                    gch1.Free();
                    File.Delete("temp3");

                    return true;
                }

                //if (number != vNumberHdd)
                //    MessageBox.Show("Upss...Tenga en cuenta que el programa " +
                //                    "se ha ejecutado en una dirección diferente y necesita " +
                //                    "que su archivo de licencia esté en la misma dirección!");

                foreach (var otherNumber in otherNumbers)
                {
                    if (vOtherNumberHdd.Contains(otherNumber))
                    {
                        var list = new List<string>();
                        //list.Add(GetProcessorID()); list.Add("*");

                        list.Add(vUser + "*");
                        list.Add(vNumberHdd + "*");
                        list.AddRange(vOtherNumberHdd);
                        list.Add("*");
                        list.AddRange(vMacsAddres);

                        File.WriteAllLines(currentDir+"\\temp3", list);
                        skey = skey.Replace('/', '3');
                        GCHandle gch1 = GCHandle.Alloc(skey, GCHandleType.Pinned);

                        security.EncryptFile(currentDir+"\\temp3",
                           currentDir+"\\lic.lot", skey);

                        gch1.Free();
                        File.Delete(currentDir+"\\temp3");

                        return true; //es el mismo disco duro
                    }
                }
                if (vMacsAddres.Count != 0)
                {
                    foreach (var m in mac)
                    {
                        if (vMacsAddres.Contains(m))
                        {
                            var list = new List<string>();
                            //list.Add(GetProcessorID()); list.Add("*");

                            list.Add(vUser + "*");
                            list.Add(vNumberHdd + "*");
                            list.AddRange(vOtherNumberHdd);
                            list.Add("*");
                            list.AddRange(vMacsAddres);

                            File.WriteAllLines(currentDir+"\\temp3", list);
                            skey = skey.Replace('/', '3');
                            GCHandle gch2 = GCHandle.Alloc(skey, GCHandleType.Pinned);

                            security.EncryptFile(currentDir+"\\temp3",
                               currentDir + "\\lic.lot", skey);

                            gch2.Free();
                            File.Delete(currentDir+"\\temp3");

                            return true; //tiene la misma tarjeta de red
                        }
                    }
                }
                if (vUser == user)//es el mismo usuario porque ha cambiado de pc
                {
                    MessageBox.Show("Ha copiado el programa en otra PC." +
                                    " Contactar a: 53392127, Email: oisvelsv@libero.it/osv.softeam@gmail.com/@yahoo.com/@hotmail.com" +
                                    " para que reciba otra licencia");
                    Random random = new Random();
                    var insertlicence = new Licence(random.Next(10000, 99999).ToString(), false);
                    insertlicence.ShowDialog();
                    if (!InsertLicence(insertlicence.GiveMeRandom(), insertlicence.GiveMeLicence(), skeyp, false))
                    {
                        MessageBox.Show("No está autorizado!");
                        SameUser = true;
                        return false;
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }


            return false;
        }
        /// <summary>
        /// Especifica que es el mismo usuario pero ha fallado la entrada de licencia
        /// </summary>
        public bool SameUser { get; set; }
        /// <summary>
        /// Procesa la licencia, y si es valida se vincula al PC
        /// </summary>
        /// <param name="prelicence"></param>
        /// <param name="licence"></param>
        /// <param name="skey"></param>
        /// <param name="without"></param>
        /// <returns></returns>
        public bool InsertLicence(string prelicence, string licence, string skey, bool without)
        {
            try
            {
                int key = int.Parse(skey.Remove(2, 1).Remove(4, 1));
                int pret = int.Parse(prelicence[2].ToString()) * 3 + int.Parse(prelicence[3].ToString()) *
                    int.Parse(prelicence[3].ToString()) + int.Parse(prelicence[0].ToString()) *
                    int.Parse(prelicence[1].ToString());
                int pre = int.Parse(prelicence[4].ToString() + pret.ToString());
                int result = pre * key;
                var w = result.ToString() + prelicence[2] + prelicence[0] + L[2*int.Parse(prelicence[0].ToString())];
                if (w == licence || without)
                {
                    var list = new List<string>();
                    //list.Add(GetProcessorID()); list.Add("*");

                    list.Add(UserName() + "*");
                    list.Add(NumberHdd() + "*");
                    list.AddRange(OtherNumberHdd());
                    list.Add("*");
                    list.AddRange(MacsAddres());

                    File.WriteAllLines("temp3", list);
                    skey = skey.Replace('/', '3');
                    GCHandle gch = GCHandle.Alloc(skey, GCHandleType.Pinned);

                    security.EncryptFile("temp3",
                       "lic.lot", skey);

                    gch.Free();
                    File.Delete("temp3");

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Devuelve el # del procesador
        /// </summary>
        /// <returns></returns>
        public string GetProcessorID()
        {

            string sProcessorID = "";

            string sQuery = "SELECT ProcessorId FROM Win32_Processor";

            try
            {
                var oManagementObjectSearcher = new ManagementObjectSearcher(sQuery);

                ManagementObjectCollection oCollection = oManagementObjectSearcher.Get();

                foreach (ManagementObject oManagementObject in oCollection)
                    sProcessorID = (string)oManagementObject["ProcessorId"];
                return sProcessorID;
            }
            catch
            {
                try
                {
                    ManagementObjectCollection mbsList = null;
                    var mbs = new ManagementObjectSearcher("Select * From Win32_processor");
                    mbsList = mbs.Get();
                    string id = "";
                    foreach (ManagementObject mo in mbsList)
                    {
                        id = mo["ProcessorID"].ToString();
                    }
                    return id;
                }
                catch
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// Devuelve el numero del motherboard
        /// </summary>
        /// <returns></returns>
        public string MotherBoardNumber()
        {
            return "";
            //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            //ManagementObjectCollection moc = mos.Get();
            //string motherBoard = "";
            //foreach (ManagementObject mo in moc)
            //{
            //    motherBoard = (string)mo["SerialNumber"];
            //}
            //if(motherBoard!=null)
            //    foreach (var VARIABLE in motherBoard)
            //    {
            //        if (VARIABLE != ' ')
            //            return motherBoard;
            //    }

            //string mbInfo = String.Empty;
            //ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
            //scope.Connect();
            //ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

            //foreach (PropertyData propData in wmiClass.Properties)
            //{
            //    if (propData.Name == "SerialNumber")
            //        mbInfo = String.Format("{0}", Convert.ToString(propData.Value));
            //}
            //if(mbInfo!=motherBoard)
            //    return mbInfo;
            //string serial = "";
            //ManagementObjectSearcher mos1 = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
            //ManagementObjectCollection moc1 = mos1.Get();

            //foreach (ManagementObject mo in moc1)
            //{
            //    serial = mo["SerialNumber"].ToString();
            //}
            //return serial;

        }
        /// <summary>
        /// Devuelve el tamanno del disco duro
        /// </summary>
        /// <returns></returns>
        public long SizeHardDrive()
        {
            long result = 0;
            //Referir al namespace \\root\cimv2
            ManagementScope scope = new ManagementScope("\\root\\cimv2");
            //Crear un objeto para consultar una tabla del namespace
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_LogicalDisk where drivetype=3");
            //Ejecutar el query
            ManagementObjectSearcher mos = new ManagementObjectSearcher(scope, query);

            //Iterar en los resultados del query
            foreach (ManagementObject item in mos.Get())
            {
                long hddSizeBytes = Int64.Parse(item["Size"].ToString());
                result = hddSizeBytes / 1024 / 1024 / 1024;
            }
            return result;
        }
        /// <summary>
        /// Devuelve los numeros de series de la parte del disco donde se esta ejecutando
        /// </summary>
        /// <returns></returns>
        public string NumberHdd()
        {
            var result = "";
            int count = 0;

            DirectoryInfo currentDir = new DirectoryInfo(Environment.CurrentDirectory);
            string path = string.Format("win32_logicaldisk.deviceid=\"{0}\"",
              currentDir.Root.Name.Replace("\\", ""));
            ManagementObject disk = new ManagementObject(path);
            disk.Get();

            foreach (PropertyData property in disk.Properties)
            {
                string name = property.Name.PadRight(25);
                if (name.Contains("SerialNumber"))
                    result = (property.Value ?? string.Empty).ToString();
            }
            return result;
        }
        /// <summary>
        /// Metodo alternativo para devolver los numeros de series de los discos duros
        /// </summary>
        /// <returns></returns>
        public List<string> OtherNumberHdd()
        {
            var result = new List<string>();
            //DriveInfo[] drives = DriveInfo.GetDrives();
            //foreach (DriveInfo drive in drives)
            //{
            //    if(drive.DriveType==DriveType.Fixed)
            //        result.Add(drive.Name);
            //}
            try
            {
                ManagementObjectSearcher searcher =
                  new ManagementObjectSearcher("root\\CIMV2",
                  "SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string interfaceType = "";
                    string mediaType = "";

                    try
                    {
                        interfaceType = queryObj["InterfaceType"].ToString();
                        mediaType = queryObj["MediaType"].ToString();
                    }
                    catch
                    {
                        mediaType = queryObj["MediaType"].ToString();
                    }
                    if (interfaceType.Contains("USB") || mediaType.Contains("Removable"))
                        continue;
                    if (interfaceType.Contains("IDE") || mediaType.Contains("Fixed hard disk"))
                        result.Add(queryObj["SerialNumber"].ToString().Replace(" ", ""));

                }
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// Da las direcciones mac de las tarjetas de red activas
        /// </summary>
        /// <returns></returns>
        public List<string> MacsAddres()
        {
            ManagementClass management = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection adapters = management.GetInstances();

            List<string> macs = new List<string>();
            foreach (ManagementObject adapter in adapters)
            {
                bool isIpEnabled = (bool)(adapter["IPEnabled"] ?? false);
                if (isIpEnabled)
                    macs.Add(adapter["MacAddress"] as string);
                adapter.Dispose();
            }
            return macs;
        }
        /// <summary>
        /// Obtiene el nombre de usuario
        /// </summary>
        /// <returns></returns>
        public string UserName()
        {
            var result = "";
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            result = user.Name;
            return result;
        }
    }
}
