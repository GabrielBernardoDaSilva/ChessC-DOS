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
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.terminada)
                {
                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tab);
                    Console.WriteLine("Posição da Peças Origem: ");
                    Posicao origem = Tela.lerPosicaoXadrez().toPosicao();


                    bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();



                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tab,posicoesPossiveis);

                    Console.WriteLine();
                    Console.WriteLine("Posição da Peças Destino: ");
                    Posicao destino = Tela.lerPosicaoXadrez().toPosicao();

                    partida.executaMovimento(origem, destino);

                }

                Tela.imprimirTabuleiro(partida.tab);

            }
            catch(TabuleiroException e)
            {
                Console.WriteLine($"Erro : {e.Message}");
            }

            Console.ReadKey();

        }
    }
}
