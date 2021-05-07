using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoAtualizadoEvent : Event
    {
        public PedidoAtualizadoEvent(Guid idCliente, Guid idPedido, decimal valorTotal)
        {
            AggregateId = idPedido;
            IdCliente = idCliente;
            IdPedido = idPedido;
            ValorTotal = valorTotal;
        }

        public Guid IdCliente { get; private set; }
        public Guid IdPedido { get; private set; }
        public decimal ValorTotal { get; private set; }
    }
}
