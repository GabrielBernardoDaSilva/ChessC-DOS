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
            
            Tabuleiro t = new Tabuleiro(8,8);
            Tela.imprimirTabuleiro(t);

            Console.ReadKey();

        }
    }
}
