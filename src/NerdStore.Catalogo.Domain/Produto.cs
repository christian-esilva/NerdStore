using NerdStore.Catalogo.Domain.ValueObjects;
using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Catalogo.Domain
{
    public class Produto : Entidade, IAggregateRoot
    {
        protected Produto() { }

        public Produto(string nome, string descricao, bool ativo, decimal valor, Guid idCategoria, DateTime dataCadastro, string imagem, Dimensoes dimensoes)
        {
            IdCategoria = idCategoria;
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            DataCadastro = dataCadastro;
            Imagem = imagem;
            Dimensoes = dimensoes;

            Validar();
        }

        public Guid IdCategoria { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Imagem { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public Dimensoes Dimensoes { get; private set; }
        public Categoria Categoria { get; private set; }

        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            IdCategoria = categoria.Id;
        }

        public void AlterarDescricao(string descricao)
        {
            Descricao = descricao;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            if (!PossuiEstoque(quantidade)) throw new DomainException("Estoque insuficiente!");
            QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoque(int quantidade)
        {
            QuantidadeEstoque += quantidade;
        }

        public bool PossuiEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome do Produto não pode ser vazio");
            Validacoes.ValidarSeVazio(Descricao, "O campo Descrição do Produto não pode ser vazio");
            Validacoes.ValidarSeIgual(IdCategoria, Guid.Empty, "O campo Id da Categoria não pode ser vazio");
            Validacoes.ValidarSeMenorQue(Valor, 1, "O campo Valor do Produto não pode ser menor ou igual a zero");
            Validacoes.ValidarSeVazio(Imagem, "O campo Imagem do Produto não pode ser vazio");
        }
    }
}
