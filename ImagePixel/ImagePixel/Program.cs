using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ImagePixel
{
    class Program
    {
        static void Main(string[] args)
        {
            string nomFichier = "coco.bmp";
            MonImage image = new MonImage(nomFichier);
            foreach (byte b in image.Header) { Console.Write(b + " "); }
            Console.WriteLine();
            foreach (byte b in image.HeaderInfo) { Console.Write(b + " "); }
            Console.WriteLine();
            image.Rotation();
            image.Sauvegarder("save.bmp");



            Process.Start("save.bmp");
            Console.ReadKey();
        }
    }
}
