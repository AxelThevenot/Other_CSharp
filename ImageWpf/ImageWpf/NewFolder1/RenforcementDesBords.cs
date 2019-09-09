using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWpf
{
    class RenforcementDesBords
    {
        //Matrice de convolution associée au filtre
        static int[,] matrice = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
        /// <summary>
        /// Traitement direct du filtre sur l'image
        /// </summary>
        /// <param name="_image">Image soumise au filtre</param>
        public static void Appliquer(MonImage _image)
        {
            _image = NouvelleImage(_image);
        }
        /// <summary>
        /// Retourne l'image associée au filtre appliqué
        /// </summary>
        /// <param name="_image">Image soumise au filtre</param>
        /// <returns>Image associée</returns>
        public static MonImage NouvelleImage(MonImage _image)
        {
            MonImage image = new MonImage(_image, false);
            for (int i = 1; i < _image.Pixel.GetLength(0) - 1; i++)
                for (int j = 1; j < _image.Pixel.GetLength(1) - 1; j++)
                    for (int k = 0; k < 3; k++)
                        image.Pixel[i, j, k] = (byte)((_image.Pixel[i + 0, j - 1, k] * matrice[1, 0]
                                                     + _image.Pixel[i + 0, j + 0, k] * matrice[1, 1]));
            Traitement.EtendreBordures(_image);
            return image;
        }
    }
}
