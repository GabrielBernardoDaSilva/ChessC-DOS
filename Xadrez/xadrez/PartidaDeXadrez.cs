using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xadrez.tabuleiro;
using Xadrez.tabuleiro.exception;

namespace Xadrez.xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }

        public PartidaDeXadrez()
        {
            this.tab = new Tabuleiro(8,8);
            this.turno = 1;
            this.jogadorAtual = Cor.Branca;
            this.terminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            
            colocarPecas();
        }

        private Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimento();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
                capturadas.Add(pecaCapturada);


            // #jogadaespecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimento();
                tab.colocarPeca(T, destinoT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimento();
                tab.colocarPeca(T, destinoT);
            }






            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem,Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }
        }


        public void realizaJogada(Posicao origem,Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem,destino,pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }
            if (estaEmXeque(adversario(jogadorAtual)))
                xeque = true;
            else
                xeque = false;
            if (testeXequemate(adversario(jogadorAtual)))
                terminada = true;
            else
            {
                turno++;
                mudaJogador();
            }
        }

        public bool testeXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
                return false;
            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for(int i =0; i < tab.linhas; i++)
                {
                    for (int j = 0; j < tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino,pecaCapturada);
                            if (!testeXeque)
                                return false;
                        }
                    }
                }
            }
            return true;
        }


        private Cor adversario(Cor cor)
        {
            if (cor == Cor.Branca)
                return Cor.Preta;
            else
                return Cor.Branca;
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }


        public bool estaEmXeque(Cor cor)
        {
            Peca r = rei(cor);
            if (r == null)
                throw new TabuleiroException($"Não tem rei da cor do {cor} no tabuleiro");
            foreach (Peca x in pecasEmJogo(adversario(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[r.posicao.linha, r.posicao.coluna])
                    return true;
            }
            return false;
        }


        public void validarPosicaoOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
                throw new TabuleiroException("Não existe peça na posição atual! ");
            if (jogadorAtual != tab.peca(pos).cor)
                throw new TabuleiroException("A peça escolhida não é sua! ");
            if (!tab.peca(pos).existeMovimentosPossiveis())
                throw new TabuleiroException("Não movimentos possiveis para a peça escolhida! ");
        }

        public void validarPosicaoDestino(Posicao origem,Posicao destino)
        {
            if (!tab.peca(origem).movimentoPossivel(destino))
                throw new TabuleiroException("Posição de destino invalido");
        }

        private void mudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
                jogadorAtual = Cor.Preta;
            else
                jogadorAtual = Cor.Branca;
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach(Peca x in capturadas)
            {
                if (x.cor == cor)
                    aux.Add(x);
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                    aux.Add(x);
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }


        public void colocarNovaPeca(Peca peca,char coluna,int linha)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }


        private void colocarPecas()
        {
            colocarNovaPeca(new Torre(Cor.Branca, tab), 'a',1);
            colocarNovaPeca(new Cavalo(Cor.Branca, tab), 'b',1);
            colocarNovaPeca(new Bispo(Cor.Branca, tab), 'c', 1);
            colocarNovaPeca(new Dama(Cor.Branca, tab), 'd', 1);
            colocarNovaPeca(new Rei(Cor.Branca, tab,this), 'e', 1);
            colocarNovaPeca(new Bispo(Cor.Branca, tab), 'f', 1);
            colocarNovaPeca(new Cavalo(Cor.Branca, tab), 'g', 1);
            colocarNovaPeca(new Torre(Cor.Branca, tab), 'h', 1);
            colocarNovaPeca(new Peao(Cor.Branca, tab), 'a', 2);
            colocarNovaPeca(new Peao(Cor.Branca, tab), 'b', 2);
            colocarNovaPeca(new Peao(Cor.Branca, tab), 'c', 2);
            colocarNovaPeca(new Peao(Cor.Branca, tab), 'd', 2);
            colocarNovaPeca(new Peao(Cor.Branca, tab), 'e', 2);
            colocarNovaPeca(new Peao(Cor.Branca, tab), 'f', 2);
            colocarNovaPeca(new Peao(Cor.Branca, tab), 'g', 2);
            colocarNovaPeca(new Peao(Cor.Branca, tab), 'h', 2);


            colocarNovaPeca(new Torre(Cor.Preta, tab), 'a', 8);
            colocarNovaPeca(new Cavalo(Cor.Preta, tab), 'b', 8);
            colocarNovaPeca(new Bispo(Cor.Preta, tab), 'c', 8);
            colocarNovaPeca(new Dama(Cor.Preta, tab), 'd', 8);
            colocarNovaPeca(new Rei(Cor.Preta, tab,this), 'e', 8);
            colocarNovaPeca(new Bispo(Cor.Preta, tab), 'f', 8);
            colocarNovaPeca(new Cavalo(Cor.Preta, tab), 'g', 8);
            colocarNovaPeca(new Torre(Cor.Preta, tab), 'h', 8);
            colocarNovaPeca(new Peao(Cor.Preta, tab), 'a', 7);
            colocarNovaPeca(new Peao(Cor.Preta, tab), 'b', 7);
            colocarNovaPeca(new Peao(Cor.Preta, tab), 'c', 7);
            colocarNovaPeca(new Peao(Cor.Preta, tab), 'd', 7);
            colocarNovaPeca(new Peao(Cor.Preta, tab), 'e', 7);
            colocarNovaPeca(new Peao(Cor.Preta, tab), 'f', 7);
            colocarNovaPeca(new Peao(Cor.Preta, tab), 'g', 7);
            colocarNovaPeca(new Peao(Cor.Preta, tab), 'h', 7);

        }

    }
}
