using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Vendas.Domain.Entidades
{
    public class ItemPedido : Entidade
    {
        public ItemPedido(Guid idProduto, string nomeProduto, int quantidade, decimal valorUnitario)
        {
            IdProduto = idProduto;
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        protected ItemPedido() { }

        public Guid IdPedido { get; private set; }
        public Guid IdProduto { get; private set; }
        public string NomeProduto { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        // Relacionamento para o EF
        public Pedido Pedido { get; set; }

        internal void AssociarPedido(Guid idPedido)
        {
            IdPedido = idPedido;
        }

        public decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }

        internal void AdicionarUnidades(int unidades)
        {
            Quantidade += unidades;
        }

        internal void AtualizarUnidades(int unidades)
        {
            Quantidade = unidades;
        }

        public override bool EhValido()
        {
            return true;
        }
    }
}
