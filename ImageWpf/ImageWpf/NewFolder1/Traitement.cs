using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWpf
{
    /* On créer une class Traitement                             *
     * pour séparer les méthodes de traitement d'image du reste  *
     * quesion d'organisation et de lisibilité                   */

    class Traitement
    {
        /// <summary>
        /// Redimensionner l'image selon une coefficient multiplicateur
        /// </summary>
        /// <param name="_image">Image à redimensionner</param>
        /// <param name="coefficient">Coefficient de redimentionnement</param>
        /// <returns></returns>
        public static MonImage Redimensionner(MonImage _image, double coefficient)
        {
            MonImage image = new MonImage(_image, true);
            //On redimensionne l'image dans le Header
            //On récupère la dimension initiale
            int hauteur = MonImage.EndianToInt(new byte[] { _image.HeaderInfo[4], _image.HeaderInfo[5], _image.HeaderInfo[6], _image.HeaderInfo[7] });
            int largeur = MonImage.EndianToInt(new byte[] { _image.HeaderInfo[8], _image.HeaderInfo[9], _image.HeaderInfo[10], _image.HeaderInfo[11] });
            //On multiplie par le coefficient demandé
            hauteur = (int)(hauteur * coefficient);
            largeur = (int)(largeur * coefficient);
            //On réajuste afin que la largeur soit un multiple de 4
            largeur += (4 - (largeur % 4)) % 4;
            //On implemente les nouvelles dimensons dans le header
            byte[] Hauteur = MonImage.IntToEndian(hauteur);
            byte[] Largeur = MonImage.IntToEndian(largeur);
            for (int i = 0; i < 4; i++)
            {
                image.HeaderInfo[4 + i] = Hauteur[i];
                image.HeaderInfo[8 + i] = Largeur[i];
            }
            image.Pixel = new byte[hauteur, largeur, 3];
            //On peut maintenant recréer l'image sous les nouvelles dimensions

            for (double i = 0; i < _image.Pixel.GetLength(0); i += 1 / coefficient)
                for (double j = 0; j < _image.Pixel.GetLength(1); j += 1 / coefficient)
                    for (int k = 0; k < 3; k++)
                        image.Pixel[(int)(i * coefficient), (int)(j * coefficient), k] = _image.Pixel[(int)(i), (int)j, k];

            return image;
        }
        /// <summary>
        /// Rotation de l'image
        /// </summary>
        /// <param name="_image">Image à faire tourner</param>
        /// <returns></returns>
        public static MonImage Rotation(MonImage _image)//Don't work
        {
            MonImage image = new MonImage(_image, true);
            //On inverse la hauteur et la largeur de l'image
            //Donc on récupère les données 
            byte[] largeur = new byte[] { _image.HeaderInfo[4],
                                          _image.HeaderInfo[5],
                                          _image.HeaderInfo[6],
                                          _image.HeaderInfo[7]};
            byte[] hauteur = new byte[] { _image.HeaderInfo[8],
                                          _image.HeaderInfo[9],
                                          _image.HeaderInfo[10],
                                          _image.HeaderInfo[11]};
            //On inverse aussi la résolution horizontale et verticale
            byte[] resolutionHorizontale = new byte[] { _image.HeaderInfo[24],
                                                        _image.HeaderInfo[25],
                                                        _image.HeaderInfo[26],
                                                        _image.HeaderInfo[27]};
            byte[] resolutionVerticale = new byte[] { _image.HeaderInfo[28],
                                                      _image.HeaderInfo[29],
                                                      _image.HeaderInfo[30],
                                                      _image.HeaderInfo[31]};
            //puis on les implémentee les données dans la nouvelle image
            for (int i = 0; i < 4; i++)
            {
                image.HeaderInfo[4 + i] = hauteur[i];
                image.HeaderInfo[8 + i] = largeur[i];
                image.HeaderInfo[24 + i] = resolutionHorizontale[i];
                image.HeaderInfo[28 + i] = resolutionVerticale[i];
            }




            image.Pixel = new byte[MonImage.EndianToInt(new byte[] { hauteur[0],
                                                                     hauteur[1],
                                                                     hauteur[2],
                                                                     hauteur[3] }),
                                   MonImage.EndianToInt(new byte[] { largeur[0],
                                                                     largeur[1],
                                                                     largeur[2],
                                                                     largeur[3] }),
                                   3];
            //On peut maintenant traiter l'image
            byte[,,] newPixels = new byte[image.Pixel.GetLength(1), image.Pixel.GetLength(0), 3];
            for (int i = 0; i < newPixels.GetLength(0); i++)
                for (int j = 0; j < newPixels.GetLength(1); j++)
                    for (int k = 0; k < 3; k++)
                        newPixels[i, j, k] = _image.Pixel[j, newPixels.GetLength(0) - i - 1, k];

            image.Pixel = newPixels;
            return image;
        }
        /// <summary>
        /// Retourne une image en 255 nuances de gris
        /// </summary>
        /// <param name="_image">Image d'entrée</param>
        /// <returns>Image en nuances de gris associée</returns>
        public static MonImage NuanceDeGris(MonImage _image)
        {
            MonImage image = new MonImage(_image, false);
            for (int i = 0; i < image.Pixel.GetLength(0); i++)
                for (int j = 0; j < image.Pixel.GetLength(1); j++)
                    for (int k = 0; k < image.Pixel.GetLength(2); k++)
                    {
                        //Valeur trouvée sur internet
                        byte gris = (byte)(.299 * image.Pixel[i, j, 0] + .587 * image.Pixel[i, j, 1] + .114 * image.Pixel[i, j, 2]);
                        image.Pixel[i, j, 0] = gris;
                        image.Pixel[i, j, 1] = gris;
                        image.Pixel[i, j, 2] = gris;
                    }
            return image;
        }
        /// <summary>
        /// Retourne une image en nuances de gris
        /// </summary>
        /// <param name="_image">Image en entrée</param>
        /// <param name="nombreDeNuances">Nombre de nuance</param>
        /// <returns>Image en nuances de gris associée</returns>
        public static MonImage NuanceDeGris(MonImage _image, int nombreDeNuances)
        {
            MonImage image = new MonImage(_image, false);
            if (nombreDeNuances < 257 && nombreDeNuances > 1)
            {
                double tailleNuance = 1 / nombreDeNuances;
                for (int i = 0; i < image.Pixel.GetLength(0); i++)
                    for (int j = 0; j < image.Pixel.GetLength(1); j++)
                        for (int k = 0; k < image.Pixel.GetLength(2); k++)
                        {
                            //Valeur trouvée sur internet
                            byte gris = (byte)(.299 * image.Pixel[i, j, 0] + .587 * image.Pixel[i, j, 1] + .114 * image.Pixel[i, j, 2]);
                            image.Pixel[i, j, 0] = (byte)(gris + (gris % (255 / (nombreDeNuances - 1))) % 256);
                            image.Pixel[i, j, 1] = (byte)(gris + (gris % (255 / (nombreDeNuances - 1))) % 256);
                            image.Pixel[i, j, 2] = (byte)(gris + (gris % (255 / (nombreDeNuances - 1))) % 256);
                        }
            }

            return image;
        }
        /// <summary>
        /// Retourne une image en noir et blanc
        /// </summary>
        /// <param name="_image">Image d'entrée</param>
        /// <returns>Image en noir et blanc associée</returns>
        public static MonImage NoirEtBlanc(MonImage _image)
        {
            MonImage image = new MonImage(_image, false);
            for (int i = 0; i < image.Pixel.GetLength(0); i++)
                for (int j = 0; j < image.Pixel.GetLength(1); j++)
                    for (int k = 0; k < image.Pixel.GetLength(2); k++)
                    {
                        //Valeur trouvée sur internet
                        if ((byte)(.299 * image.Pixel[i, j, 0] + .587 * image.Pixel[i, j, 1] + .114 * image.Pixel[i, j, 2]) < 128)
                        {
                            image.Pixel[i, j, 0] = 0;
                            image.Pixel[i, j, 1] = 0;
                            image.Pixel[i, j, 2] = 0;
                        }
                        else
                        {
                            image.Pixel[i, j, 0] = 255;
                            image.Pixel[i, j, 1] = 255;
                            image.Pixel[i, j, 2] = 255;
                        }
                    }
            return image;
        }
        /// <summary>
        /// Retourne une image de la superposition de deux autres
        /// </summary>
        /// <param name="_image1"></param>
        /// <param name="_image2"></param>
        /// <param name="_x">décalage bas en pixel</param>
        /// <param name="_y">décalage droite en pixel</param>
        /// <returns>Image de superposition associée</returns>
        public static MonImage Superposition(MonImage _image1, MonImage _image2, int _x, int _y)//don't work
        {
            //L'image de retour sera une nouvelle image en partant de _image1
            MonImage imageMax = new MonImage(_image1, true);
            imageMax.HeaderInfo = _image2.HeaderInfo;
            int hauteur = _image2.Pixel.GetLength(0);
            int largeur = _image2.Pixel.GetLength(1);
            //imageMax.ToCompare(_image2);
            //On vérifie _image1 pourra contenir _image2 plus le décalage, sinon on change la taille
            if (_image2.Pixel.GetLength(0) + _x > _image1.Pixel.GetLength(0))
            {
                hauteur += _image2.Pixel.GetLength(0) + _x - _image2.Pixel.GetLength(0);
            }
            if (_x < 0)
            {
                hauteur += _x * (-1);
            }
            if (_image2.Pixel.GetLength(1) + _y > _image1.Pixel.GetLength(1))
            {
                largeur += _image2.Pixel.GetLength(1) + _y - _image2.Pixel.GetLength(1);
            }
            if (_x < 0)
            {
                largeur += _y * (-1);
            }
            //On attribue les bonnes dimensions à l'image 1 même si inchangée (moins de "if" à faire)
            byte[] Hauteur = MonImage.IntToEndian(hauteur);
            byte[] Largeur = MonImage.IntToEndian(largeur);
            for (int i = 0; i < 4; i++)
            {
                imageMax.HeaderInfo[4 + i] = Hauteur[i];
                imageMax.HeaderInfo[8 + i] = Largeur[i];
            }

            //Ne pas oublier chaque ligne multiple de 4
            imageMax.Pixel = new byte[hauteur, largeur + (4 - largeur % 4) % 4, 3];
            //On place d'abord l'image 1
            int min_x = Math.Min(_x, 0);
            int min_y = Math.Min(_y, 0);
            int max_x = Math.Max(_x, 0);
            int max_y = Math.Max(_y, 0);
            for (int i = min_x; i < min_x + _image1.Pixel.GetLength(0); i++)
                for (int j = min_y; j < min_y + _image1.Pixel.GetLength(1); j++)
                    for (int k = 0; k < 3; k++)
                        imageMax.Pixel[max_x, max_x, k] = _image1.Pixel[i - min_y, j - min_y, k];
            //Ensuite on place l'image 2 sachant que le point haute gauche de _image1 est au pixel (m_x, m_y)
            for (int i = min_x + _x; i < min_x + _x + _image2.Pixel.GetLength(0); i++)
                for (int j = min_y + _y; j < min_y + _y + _image2.Pixel.GetLength(1); j++)
                    for (int k = 0; k < 3; k++)
                        imageMax.Pixel[i, j, k] = _image2.Pixel[i - min_x - _x, j - min_y - _y, k];
            return imageMax;
        }
        /// <summary>
        /// Etendre les bordures d'une image après application d'un filtre
        /// </summary>
        /// <param name="_image">Image soumise au traitement</param>
        public static void EtendreBordures(MonImage _image)
        {
            //On donne les valeurs des pixels aux bords gauche et droite par celles de son voisin horizontal
            for (int i = 0; i < _image.Pixel.GetLength(0); i++)
                for (int k = 0; k < 3; k++)
                {
                    _image.Pixel[i, 0, k] = _image.Pixel[i, 1, k];
                    _image.Pixel[i, _image.Pixel.GetLength(1) - 1, k] = _image.Pixel[i, _image.Pixel.GetLength(1) - 2, k];
                }
            //On donne les valeurs des pixels aux bords haut et bas par celles de son voisin vertical
            for (int i = 0; i < _image.Pixel.GetLength(1); i++)
                for (int k = 0; k < 3; k++)
                {
                    _image.Pixel[0, i, k] = _image.Pixel[1, i, k];
                    _image.Pixel[_image.Pixel.GetLength(0) - 1, i, k] = _image.Pixel[_image.Pixel.GetLength(0) - 2, i, k];
                }
        }
    }
}
