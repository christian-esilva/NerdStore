using NerdStore.Core.DomainObjects;
using System.Collections;
using System.Collections.Generic;

namespace NerdStore.Catalogo.Domain
{
    public class Categoria : Entidade
    {
        protected Categoria() { }

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;

            Validar();
        }

        public string Nome { get; set; }
        public int Codigo { get; private set; }
        public ICollection<Produto> Produtos { get; set; }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome da Categoria não pode ser vazio");
            Validacoes.ValidarSeIgual(Codigo, 0, "O campo Código da Categoria não pode ser zero");
        }
    }
}
