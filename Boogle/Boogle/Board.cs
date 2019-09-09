using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boogle
{
    class Board
    {
        public Dice[,] Dices { get; private set; }



        public bool TestBoard(char[] word)
        {
            int[,] diceUsed = new int[16, 2];
            for (int i = 0; i < this.Dices.GetLength(0); i++)
            {
                for (int j = 0; j < this.Dices.GetLength(1); j++)
                {
                    if (word[0] == this.Dices[i, j].LetterVisible)
                    {
                        diceUsed[0, 0] = i;
                        diceUsed[0, 0] = j;
                        return letterFollows(word, 0, i, j, diceUsed);
                    }
                }
            }
            return false;
        }

        public bool letterFollows(char[] word, int index, int iPos, int jPos, int[,] diceUsed)
        {
            for (int i = Math.Max(0, iPos - 1); i < Math.Min(4, iPos + 2); i++)
            {
                for (int j = Math.Max(0, jPos - 1); i < Math.Min(4, jPos + 2); j++)
                {
                    if (this.Dices[i, j].LetterVisible == word[index + 1])
                    {
                        //On verifie que ce n'est pas utilisé
                        bool isletterUsed = false;
                        for (int k = 0; k < index + 1; k++)
                        {
                            if (i == diceUsed[k, 0] && j == diceUsed[k, 1])
                            {
                                isletterUsed = true;
                            }
                        }
                        if (!isletterUsed)
                        {
                            diceUsed[index + 1, 0] = i;
                            diceUsed[index + 1, 0] = j;
                            if(index +1 == word.Length)
                            {
                                return true;
                            }
                            else
                            {
                                return letterFollows(word, index + 1, i, j, diceUsed);
                            }
                        }

                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < this.Dices.GetLength(0); i++)
            {
                for (int j = 0; j < this.Dices.GetLength(1); j++)
                {
                    str += this.Dices[i, j].LetterVisible;
                }
                str += '\n';
            }
            return str;
        }

    }
}
