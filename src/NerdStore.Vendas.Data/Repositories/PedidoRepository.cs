using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Vendas.Domain.Entidades;
using NerdStore.Vendas.Domain.Enums;
using NerdStore.Vendas.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly VendasContext _context;

        public PedidoRepository(VendasContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Pedido> ObterPorId(Guid id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId)
        {
            return await _context.Pedidos.AsNoTracking().Where(p => p.IdCliente == clienteId).ToListAsync();
        }

        public async Task<Pedido> ObterPedidoRascunhoPorClienteId(Guid clienteId)
        {
            //populando o pedido
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdCliente == clienteId && p.PedidoStatus == EPedidoStatus.Rascunho);
            if (pedido == null) return null;

            //com o pedido populado estou carregando tbm os pedidos itens 
            await _context.Entry(pedido)
                .Collection(i => i.ItensPedido).LoadAsync();

            if (pedido.IdVoucher != null)
            {
                //com o pedido populado e caso ele tenha voucher estou carregando o voucher
                await _context.Entry(pedido)
                    .Reference(i => i.Voucher).LoadAsync();
            }

            return pedido;
        }

        public void Adicionar(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public void Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }


        public async Task<ItemPedido> ObterItemPorId(Guid id)
        {
            return await _context.PedidoItems.FindAsync(id);
        }

        public async Task<ItemPedido> ObterItemPorPedido(Guid pedidoId, Guid produtoId)
        {
            return await _context.PedidoItems.FirstOrDefaultAsync(p => p.IdProduto == produtoId && p.IdPedido == pedidoId);
        }

        public void AdicionarItem(ItemPedido pedidoItem)
        {
            _context.PedidoItems.Add(pedidoItem);
        }

        public void AtualizarItem(ItemPedido pedidoItem)
        {
            _context.PedidoItems.Update(pedidoItem);
        }

        public void RemoverItem(ItemPedido pedidoItem)
        {
            _context.PedidoItems.Remove(pedidoItem);
        }

        public async Task<Voucher> ObterVoucherPorCodigo(string codigo)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
