using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xadrez.tabuleiro;
using Xadrez.tabuleiro.exception;
using Xadrez.xadrez;

namespace Xadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                  Tabuleiro tab = new Tabuleiro(8, 8);
                  tab.colocarPeca(new Torre(Cor.Preta, tab), new Posicao(0, 0));
     
                  tab.colocarPeca(new Torre(Cor.Preta, tab), new Posicao(1, 3));
                  tab.colocarPeca(new Rei(Cor.Preta, tab), new Posicao(2, 4));
                  tab.colocarPeca(new Rei(Cor.Branca, tab), new Posicao(3, 4));
                  tab.colocarPeca(new Rei(Cor.Branca, tab), new Posicao(6, 7));
                  tab.colocarPeca(new Rei(Cor.Branca, tab), new Posicao(1, 6));

                Tela.imprimirTabuleiro(tab);

            }
            catch(TabuleiroException e)
            {
                Console.WriteLine($"Erro : {e.Message}");
            }

            Console.ReadKey();

        }
    }
}
