using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;

namespace ImageWpf
{
    public class MonImage
    {
        #region Propriétés
        public byte[] Header { get; set; }
        public byte[] HeaderInfo { get; set; }
        public byte[,,] Pixel { get; set; }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur d'image à partir de l'adresse du fichier
        /// </summary>
        /// <param name="path">Adresse du fichier</param>
        public MonImage(string path)
        {
            //On lit le fichier 
            byte[] data = File.ReadAllBytes(path);
            //Si le fichier est bien un Bitmap on peut l'utiliser
            if (data != null && data.Length > 1 && data[0] == 66 && data[1] == 77)
            {
                this.Header = new byte[14];
                this.HeaderInfo = new byte[40];

                //Infos de l'image
                for (int i = 0; i < 14; i++)
                    this.Header[i] = data[i];
                for (int i = 14; i < 54; i++)
                    this.HeaderInfo[i - 14] = data[i];

                //Taille de l'image
                this.Pixel = new byte[EndianToInt(new byte[] { data[22], data[23], data[24], data[25] }), EndianToInt(new byte[] { data[18], data[19], data[20], data[21] }), 3];
                for (int i = 0; i < this.Pixel.GetLength(0); i++)
                {
                    //Pixels de l'image
                    for (int j = 0; j < this.Pixel.GetLength(1); j++)
                    {

                        this.Pixel[i, j, 0] = data[i * this.Pixel.GetLength(1) * 3 + j * 3 + 54];
                        this.Pixel[i, j, 1] = data[i * this.Pixel.GetLength(1) * 3 + j * 3 + 55];
                        this.Pixel[i, j, 2] = data[i * this.Pixel.GetLength(1) * 3 + j * 3 + 56];
                    }
                }
            }
        }
        /// <summary>
        /// Constructeur de copie
        /// </summary>
        /// <param name="_image">MonImage à copier</param>
        /// <param name="HeaderSeulement">Header seulement ou non</param>
        public MonImage(MonImage _image, bool HeaderSeulement)
        {
            //On défini la taille des tableaux de byte
            this.Header = new byte[_image.Header.Length];
            this.HeaderInfo = new byte[_image.HeaderInfo.Length];
            this.Pixel = new byte[_image.Pixel.GetLength(0), _image.Pixel.GetLength(1), 3];

            //On donne les même valeur à la MonImage de copie
            for (int i = 0; i < this.Header.Length; i++)
                this.Header[i] = _image.Header[i];
            for (int i = 0; i < this.HeaderInfo.Length; i++)
                this.HeaderInfo[i] = _image.HeaderInfo[i];
            //Sauf si on ne veut pas lui redonner aussi ses pixels (Pour gagner du temps d'execution)
            if (!HeaderSeulement)
            {
                for (int i = 0; i < this.Pixel.GetLength(0); i++)
                    for (int j = 0; j < this.Pixel.GetLength(1); j++)
                        for (int k = 0; k < this.Pixel.GetLength(2); k++)
                            this.Pixel[i, j, k] = _image.Pixel[i, j, k];
            }
        }
        #endregion

        #region Convertir
        /// <summary>
        /// Converti un tableau de byte Endian en un entier
        /// </summary>
        /// <param name="tableau">Tableau à convertir</param>
        /// <returns>Entier correspondant</returns>
        public static int EndianToInt(byte[] tableau)
        {
            int entier = 0;
            if (tableau != null && tableau.Length != 0)
            {
                for (int i = 0; i < tableau.Length; i++)
                {
                    entier += tableau[i] * (int)Math.Pow(256, i);
                }
            }
            return entier;
        }
        /// <summary>
        /// Converti un entier en un tableau de byte Endian
        /// </summary>
        /// <param name="valeur">Valeur à convertir</param>
        /// <returns>Tableau de byte Endian correspondant</returns>
        public static byte[] IntToEndian(int valeur)
        {
            byte[] tableau = new byte[4];
            int i = tableau.Length - 1;
            while (i > 0)
            {
                if (valeur >= Math.Pow(256, i)) { tableau[i]++; valeur -= (int)Math.Pow(256, i); }
                else { i--; }
            }
            tableau[0] = (byte)valeur;
            return tableau;
        }
        #endregion

        /// <summary>
        /// Sauvegarder les fichier sous format .bmp
        /// </summary>
        /// <param name="filename"></param>
        public void Sauvegarder(string filename)
        {
            //On s'assure que le string sois bien sous format .bmp
            if (filename.Substring(filename.Length - 4) != ".bmp")
            {
                filename += ".bmp";
            }

            //Puis on sauvegarde l'image

            byte[] data = new byte[14 + 40 + this.Pixel.GetLength(0) * this.Pixel.GetLength(1) * 3];
            for (int i = 0; i < 14; i++)
                data[i] = this.Header[i];
            for (int i = 14; i < 54; i++)
                data[i] = this.HeaderInfo[i - 14];
            for (int i = 0; i < this.Pixel.GetLength(0); i++)
                for (int j = 0; j < this.Pixel.GetLength(1); j++)
                    for (int k = 0; k < 3; k++)
                        data[(i * this.Pixel.GetLength(1) + j) * 3 + k + 54] = this.Pixel[i, j, k];

            File.WriteAllBytes(filename, data);


        }
    }
}
