using NerdStore.Core.Messages;
using System;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoItemAdicionadoEvent : Event
    {
        public PedidoItemAdicionadoEvent(Guid idCliente, Guid idPedido, Guid idProduto, decimal valorUnitario, int quantidade, string nomeProduto)
        {
            AggregateId = idPedido;
            IdCliente = idCliente;
            IdPedido = idPedido;
            IdProduto = idProduto;
            ValorUnitario = valorUnitario;
            Quantidade = quantidade;
            NomeProduto = nomeProduto;
        }

        public Guid IdCliente { get; private set; }
        public Guid IdPedido { get; private set; }
        public Guid IdProduto { get; private set; }
        public string NomeProduto { get; set; }
        public decimal ValorUnitario { get; private set; }
        public int Quantidade { get; private set; }
    }
}
