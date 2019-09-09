using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boogle
{
    class Dice
    {
        static Random rand = new Random();
        private char[] Letters { get; set; }
        public char LetterVisible { get; private set; }

        public Dice(char[] Letters)
        {
            this.Letters = Letters;
            this.Throw();
        }

        public void Throw()
        {
            this.LetterVisible = Letters[Dice.rand.Next(6)];
        }

        public override string ToString()
        {
            return this.LetterVisible.ToString();
        }
    }
}
