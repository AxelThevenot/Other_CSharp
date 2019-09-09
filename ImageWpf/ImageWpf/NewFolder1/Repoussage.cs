using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWpf
{
    class Repoussage
    {
        static int[,] matrice = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
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
                        image.Pixel[i, j, k] = (byte)((_image.Pixel[i - 1, j - 1, k] * matrice[0, 0]
                                                     + _image.Pixel[i - 1, j + 0, k] * matrice[0, 1]
                                                     + _image.Pixel[i - 1, j + 1, k] * matrice[0, 2]
                                                     + _image.Pixel[i + 0, j - 1, k] * matrice[1, 0]
                                                     + _image.Pixel[i + 0, j + 0, k] * matrice[1, 1]
                                                     + _image.Pixel[i + 0, j + 1, k] * matrice[1, 2]
                                                     + _image.Pixel[i + 1, j - 1, k] * matrice[2, 0]
                                                     + _image.Pixel[i + 1, j + 0, k] * matrice[2, 1]
                                                     + _image.Pixel[i + 1, j + 1, k] * matrice[2, 2]));
            Traitement.EtendreBordures(_image);
            return image;
        }
    }
}