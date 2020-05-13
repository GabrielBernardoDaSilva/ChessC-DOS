using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xadrez.tabuleiro;

namespace Xadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            Posicao p = new Posicao(3,4);
            Tabuleiro t = new Tabuleiro(8,8);

            Console.WriteLine($"Posição = {p}");
            Console.ReadKey();

        }
    }
}
