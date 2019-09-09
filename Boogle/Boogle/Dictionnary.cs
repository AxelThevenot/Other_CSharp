using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boogle
{
    class Dictionnary
    {
        public string[,] Words { get; private set; }

        public bool DichoSearch(string word, int a, int b)
        {
            bool exist = false;
            if (a == b)
            {
                if (this.Words[word.Length - 1, a] == word) { return true; }
                else { return false; }
            }
            return exist;
        }
    }
}
