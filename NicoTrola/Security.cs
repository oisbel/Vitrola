//using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
//using System.Security;
using System.Security.Cryptography;
//using System.Windows;

namespace NicoTrola
{
    public class Security
    {
        public void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            var fsInput = new FileStream(sInputFilename,
                FileMode.Open,
                FileAccess.Read);

            var fsEncrypted = new FileStream(sOutputFilename,
                            FileMode.Create,
                            FileAccess.Write);
            var DES = new DESCryptoServiceProvider();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            ICryptoTransform desencrypt = DES.CreateEncryptor();
            var cryptostream = new CryptoStream(fsEncrypted,
                                desencrypt,
                                CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length - 1];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);

            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();

        }
        public void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename,
               FileMode.Open,
               FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread,
               desdecrypt,
               CryptoStreamMode.Read);
            //Print the contents of the decrypted file.
           
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            //int pos = 0;
            //var l = "";
            
            try
            {
                //var temp = new StreamReader(cryptostreamDecr);
                //while(!temp.EndOfStream)
                //{
                //    pos++;
                //    l+=temp.ReadLine()+"\n";    
                //}
                //fsDecrypted.Write(l);
                fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            }
            catch
            {
                //if(pos!=0)
                //    fsDecrypted.Write(l);
            }

            //fsread.Flush();
            fsread.Close();
            fsDecrypted.Flush();
            fsDecrypted.Close(); 
        }


    }
}
