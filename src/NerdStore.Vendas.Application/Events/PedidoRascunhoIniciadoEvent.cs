using NerdStore.Core.Messages;
using System;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoRascunhoIniciadoEvent : Event
    {
        public PedidoRascunhoIniciadoEvent(Guid idCliente, Guid idPedido)
        {
            AggregateId = idPedido;
            IdCliente = idCliente;
            IdPedido = idPedido;
        }

        public Guid IdCliente { get; private set; }
        public Guid IdPedido { get; private set; }

    }
}
