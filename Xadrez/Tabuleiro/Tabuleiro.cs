﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xadrez.tabuleiro.exception;

namespace Xadrez.tabuleiro
{
    class Tabuleiro
    {
        public int linhas { get; set; }
        public int colunas { get; set; }

        private Peca[,] pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            this.linhas= linhas;
            this.colunas= colunas;
            pecas = new Peca[linhas, colunas];
        }

        public Peca peca(int linha, int coluna)
        {
            return pecas[linha, coluna];
        }

        public Peca peca(Posicao pos)
        {
            if (pos.linha > linhas || pos.coluna > colunas)
                throw new TabuleiroException("Posição invalida");
            return pecas[pos.linha, pos.coluna];
        }

        public void colocarPeca(Peca p,Posicao pos)
        {
            if (existePeca(pos))
                throw new TabuleiroException("Já Existe uma peça nesta posição");
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
        }

        public bool existePeca(Posicao pos)
        {
            validarPosicao(pos);
            return peca(pos) != null;
        }

        public Peca retirarPeca(Posicao pos)
        {
            if (peca(pos) == null)
                return null;

            Peca aux = peca(pos);
            aux.posicao = null;
            pecas[pos.linha, pos.coluna] = null;
            return aux;

        }

        public bool posicaoValida(Posicao pos)
        {
            if (pos.linha < 0 || pos.linha >= linhas || pos.coluna < 0 || pos.coluna >= colunas)
                return false;
            return true;
        }

        public void validarPosicao(Posicao pos)
        {
            if (!posicaoValida(pos))
                throw new TabuleiroException("Posição Invalida");
        }
    }
}
