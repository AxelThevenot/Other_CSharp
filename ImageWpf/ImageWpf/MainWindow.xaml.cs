using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace ImageWpf
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MonImage image;

        /// <summary>
        /// Initialiser l'interface
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            image = new MonImage("C:\\Users\\Axel\\Documents\\Visual Studio 2015\\Projects\\ImageWpf\\ImageWpf\\bin\\Debug\\lena.bmp");
            Display(image);
        }

        /// <summary>
        /// Afficher l'image changée
        /// </summary>
        /// <param name="image">Image Changée</param>
        public void Display(MonImage image)
        {
            WriteableBitmap bmi2 = new WriteableBitmap(
                image.Pixel.GetLength(1), image.Pixel.GetLength(0), 11811, 11811, PixelFormats.Bgr24, null);
            // Copy the data into a one-dimensional array.
            byte[] pixels1d = new byte[image.Pixel.Length * 3];
            int index = 0;
            for (int i = 0; i < image.Pixel.GetLength(0); i++)
            {
                for (int j = 0; j < image.Pixel.GetLength(1); j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        pixels1d[index++] = image.Pixel[i, j, k];
                    }
                }
            }


            // Update writeable bitmap with the colorArray to the image.
            Int32Rect rect = new Int32Rect(0, 0, image.Pixel.GetLength(1), image.Pixel.GetLength(0));
            int stride = 3 * image.Pixel.GetLength(1);
            bmi2.WritePixels(rect, pixels1d, stride, 0);

            ImageChangee.Source = bmi2;
        }
        /// <summary>
        /// Superposition de deux images
        /// </summary>
        /// <param name="_image1"></param>
        /// <param name="_image2"></param>
        public void Superposer(MonImage _image1, MonImage _image2)
        {
            /*MonImage imageMax = new MonImage(_image1, true);
            imageMax.HeaderInfo = _image2.HeaderInfo;
            int hauteur = Math.Max(_image1.Pixel.GetLength(0), _image2.Pixel.GetLength(0));
            int largeur = Math.Max(_image1.Pixel.GetLength(1), _image2.Pixel.GetLength(1));
            WriteableBitmap bmi2 = new WriteableBitmap(
                            hauteur, largeur, 11811, 11811, PixelFormats.Bgr24, null);
            // Copy the data into a one-dimensional array.
            byte[] pixels1d = new byte[hauteur * largeur * 3];
            int index = 0;
            for (int i = 0; i < image.Pixel.GetLength(0); i++)
            {
                for (int j = 0; j < image.Pixel.GetLength(1); j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        pixels1d[index++] = 0;
                    }
                }
            }
            index = 0;
            for (int i = 0; i < _image1.Pixel.GetLength(0); i++)
            {
                for (int j = 0; j < _image1.Pixel.GetLength(1); j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        pixels1d[index++] = _image1.Pixel[i, j, k];
                    }
                }
            }
            index = 0;
            for (int i = 0; i < _image2.Pixel.GetLength(0); i++)
            {
                for (int j = 0; j < _image2.Pixel.GetLength(1); j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        pixels1d[index++] = _image2.Pixel[i, j, k];
                    }
                }
            }
            // Update writeable bitmap with the colorArray to the image.
            Int32Rect rect = new Int32Rect(0, 0, image.Pixel.GetLength(0), image.Pixel.GetLength(1));
            int stride = 3 * image.Pixel.GetLength(0);
            bmi2.WritePixels(rect, pixels1d, stride, 0);

            ImageChangee.Source = bmi2;*/
            //L'image de retour sera une nouvelle image en partant de _image1
            MonImage imageTotale = new MonImage(_image1, true);
            imageTotale.HeaderInfo = _image2.HeaderInfo;
            int hauteur = Math.Max(_image1.Pixel.GetLength(0), _image2.Pixel.GetLength(0));
            int largeur = Math.Max(_image1.Pixel.GetLength(1), _image2.Pixel.GetLength(1));
            int hauteurMin = _image1.Pixel.GetLength(0);
            int largeurMin = _image1.Pixel.GetLength(1);
            MonImage imageMin = new MonImage(_image1, false);
            MonImage imageMax = new MonImage(_image2, false);
            if (_image1.Pixel.Length > _image2.Pixel.Length)
            {
                hauteurMin = _image2.Pixel.GetLength(0);
                largeurMin = _image2.Pixel.GetLength(1);
                imageMin = new MonImage(_image2, false);
                imageMax = new MonImage(_image1, false);
            }
            //imageMax.ToCompare(_image2);
            //On attribue les bonnes dimensions à l'image 1 même si inchangée (moins de "if" à faire)
            byte[] Hauteur = MonImage.IntToEndian(hauteur);
            byte[] Largeur = MonImage.IntToEndian(largeur);
            for (int i = 0; i < 4; i++)
            {
                imageTotale.HeaderInfo[4 + i] = Hauteur[i];
                imageTotale.HeaderInfo[8 + i] = Largeur[i];
            }
            //On place l'image 
            for (int i = 0; i < hauteur; i++)
                for (int j = 0; j < largeur; j++)
                    for (int k = 0; k < 3; k++)
                        if (i < hauteurMin && j < largeurMin)
                        {
                            imageTotale.Pixel[i, j, k] = imageMin.Pixel[i, j, k];
                        }
                        else if(i< imageMax.Pixel.GetLength(0))
                        {
                            imageTotale.Pixel[i, j, k] = imageMax.Pixel[i, j, k];
                        }
            _image1 = imageTotale;
            Display(imageTotale);

        }

        #region Boutons
        /// <summary>
        /// Bouton Télécharger pour importer une image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            if (op.ShowDialog() == true)
            {
                image = new MonImage(op.FileName);
                Display(image);
                WriteableBitmap bmi2 = new WriteableBitmap(new BitmapImage(new Uri(op.FileName, UriKind.RelativeOrAbsolute)));
                ImageOriginale.Source = new BitmapImage(new Uri(op.FileName, UriKind.RelativeOrAbsolute));

            }
        }
        /// <summary>
        /// Bouton pour sauvegarder l'image sous le nom "save.bmp"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSauvegarder_Click(object sender, RoutedEventArgs e)
        {
            image.Sauvegarder("save.bmp");
        }
        /// <summary>
        /// Bouton rotation +90°
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonp90_Click(object sender, RoutedEventArgs e)
        {

            image = Traitement.Rotation(image);
            Display(image);
        }
        /// <summary>
        /// Bouton rotation -90°
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonm90_Click(object sender, RoutedEventArgs e)
        {
            image = Traitement.Rotation(image);
            image = Traitement.Rotation(image);
            image = Traitement.Rotation(image);
            Display(image);
        }
        /// <summary>
        /// Bouton pour nuance de gris
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGris_Click(object sender, RoutedEventArgs e)
        {
            image = Traitement.NuanceDeGris(image, (int)sliderNuanceDeGris.Value);
            Display(image);
        }
        /// <summary>
        /// Bouton pour superposition images
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSuperposition_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            if (op.ShowDialog() == true)
            {
                MonImage image2 = new MonImage(op.FileName);
                Superposer(image, image2);
                //Display(image);
            }
        }
        /// <summary>
        /// Bouton pour détection des contours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonContours_Click(object sender, RoutedEventArgs e)
        {
            image = DetectionDesContours.NouvelleImage(image);
            Display(image);
        }
        /// <summary>
        /// Bouton pour flou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFlou_Click(object sender, RoutedEventArgs e)
        {
            image = Flou.NouvelleImage(image);
            Display(image);
        }
        /// <summary>
        /// Bouton pour repoussage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRepoussage_Click(object sender, RoutedEventArgs e)
        {
            image = Repoussage.NouvelleImage(image);
            Display(image);
        }
        /// <summary>
        /// Bouton pour Renforcement des bords
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBords_Click(object sender, RoutedEventArgs e)
        {
            image = RenforcementDesBords.NouvelleImage(image);
            Display(image);
        }
        /// <summary>
        /// Bouton pour créer histogramme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHisto_Click(object sender, RoutedEventArgs e)
        {
            int col = 0;
            if (G.IsSelected) { col = 1; }
            else if (R.IsSelected) { col = 2; }
            byte[,,] pix = new byte[image.Pixel.GetLength(0), image.Pixel.GetLength(1), 3];
            for (int j = 0; j < image.Pixel.GetLength(1); j++)
            {

                for (int i = 0; i < image.Pixel.GetLength(0); i++)
                {
                    pix[i, j, 0] = 255;
                    pix[i, j, 1] = 255;
                    pix[i, j, 2] = 255;
                }

            }

            for (int j = 0; j < image.Pixel.GetLength(1); j++)
            {
                long sum = 0;
                long sumcol = 0;
                for (int i = 0; i < image.Pixel.GetLength(0); i++)
                {
                    sum += image.Pixel[i, j, 0] + image.Pixel[i, j, 1] + image.Pixel[i, j, 2];
                    sumcol += image.Pixel[i, j, col];
                }
                for (int i = 0; i < image.Pixel.GetLength(0) * sumcol / sum; i++)
                {
                    pix[i, j, (col + 1) % 3] = 0;
                    pix[i, j, (col + 2) % 3] = 0;
                }
            }
            image.Pixel = pix;
            Display(image);
        }
        /// <summary>
        /// Bouton pour créer une figure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFigure_Click(object sender, RoutedEventArgs e)
        {
            const int width = 512;
            const int height = 512;
            WriteableBitmap wbitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);
            byte[,,] pixels = new byte[height, width, 3];
            // On met tout en blanc
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    for (int i = 0; i < 3; i++)
                        pixels[row, col, i] = 255;
                }
            }
            if (Triangle.IsSelected)
            {
                // On fera un triangle bleu
                for (int row = height / 2 - 40; row < height / 2 + 40; row++)
                {
                    for (int col = height / 2 - 40; col <= row; col++)
                    {
                        pixels[row, col, 1] = 0;
                        pixels[row, col, 2] = 0;
                    }
                }
            }
            if (Carré.IsSelected)
            {

                // On fera un carré magenta
                for (int row = height / 2 - 40; row < height / 2 + 40; row++)
                {
                    for (int col = width / 2 - 40; col < width / 2 + 40; col++)
                    {
                        pixels[row, col, 1] = 0;
                    }
                }
            }

            // Copy the data into a one-dimensional array.
            byte[] pixels1d = new byte[height * width * 3];
            int index = 0;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    for (int i = 0; i < 3; i++)
                        pixels1d[index++] = pixels[row, col, i];
                }
            }

            // Update writeable bitmap with the colorArray to the image.
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            int stride = 3 * width;
            wbitmap.WritePixels(rect, pixels1d, stride, 0);


            //Set the Image source.
            ImageChangee.Source = wbitmap;

        }
        #endregion
    }
}
