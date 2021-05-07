using MediatR;
using NerdStore.Core.Interfaces;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain.Entidades;
using NerdStore.Vendas.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Handlers
{
    public class PedidoCommandHandler :
        IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatorHandler mediatorHandler)
        {
            _pedidoRepository = pedidoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.IdCliente);
            var itemPedido = new ItemPedido(message.IdProduto, message.Nome, message.Quantidade, message.ValorUnitario);

            if(pedido == null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.IdCliente);
                pedido.AdicionarItem(itemPedido);

                _pedidoRepository.Adicionar(pedido);
                pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.IdCliente, message.IdProduto));
            }
            else
            {
                var pedidoItemExistente = pedido.ItemPedidoExistente(itemPedido);
                pedido.AdicionarItem(itemPedido);

                if (pedidoItemExistente)
                {
                    _pedidoRepository.AtualizarItem(pedido.ItensPedido.FirstOrDefault(p => p.IdProduto == itemPedido.IdProduto));
                }
                else
                {
                    _pedidoRepository.AdicionarItem(itemPedido);
                }

                pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.IdCliente, pedido.Id, pedido.ValorTotal));
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.IdCliente, pedido.Id, message.IdProduto, message.ValorUnitario, message.Quantidade, message.Nome));
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command message)
        {
            if (message.EhValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
