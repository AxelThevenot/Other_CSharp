using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boogle
{
 
    class Player
    {
        public string Name { get; private set; }
        public int Score { get; set; }
        public string[] WordsFound { get; private set; }

        public Player(string name)
        {
            this.Name = name;
            this.WordsFound = new string[0];
        }

        public bool Contain(string word)
        {
            bool isAlreadyFound = false;
            for(int i =0; i < WordsFound.Length; i++)
            {
                if(this.WordsFound[i] == word) { isAlreadyFound = true; }
            }
            return isAlreadyFound;
        }

        public void AddWord(string word)
        {
            string[] newWordsFound = new string[this.WordsFound.Length + 1];
            for (int i = 0; i < WordsFound.Length; i++)
            {
                newWordsFound[i] = this.WordsFound[i];
            }
            newWordsFound[newWordsFound.Length - 1] = word;
            this.WordsFound = newWordsFound;
        }
    }
}
