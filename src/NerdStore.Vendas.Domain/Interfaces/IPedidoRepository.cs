using NerdStore.Core.Data;
using NerdStore.Vendas.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> ObterPorId(Guid id);
        Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId);
        Task<Pedido> ObterPedidoRascunhoPorClienteId(Guid clienteId);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);

        Task<ItemPedido> ObterItemPorId(Guid id);
        Task<ItemPedido> ObterItemPorPedido(Guid pedidoId, Guid produtoId);
        void AdicionarItem(ItemPedido itemPedido);
        void AtualizarItem(ItemPedido itemPedido);
        void RemoverItem(ItemPedido itemPedido);

        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}
