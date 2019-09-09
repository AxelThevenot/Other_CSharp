using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePixel
{
    class PixelClass
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public PixelClass(byte _R, byte _G, byte _B)
        {
            this.R = _R;
            this.G = _G;
            this.B = _B;
        }
    }
}
