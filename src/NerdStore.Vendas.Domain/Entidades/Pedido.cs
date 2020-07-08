using NerdStore.Core.DomainObjects;
using NerdStore.Vendas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain.Entidades
{
    public class Pedido : Entidade, IAggregateRoot
    {
        public Pedido(Guid idCliente, bool voucherUtilizado, decimal desconto, decimal valorTotal)
        {
            IdCliente = idCliente;
            VoucherUtilizado = voucherUtilizado;
            Desconto = desconto;
            ValorTotal = valorTotal;
            _itensPedido = new List<ItemPedido>();
        }

        protected Pedido() { _itensPedido = new List<ItemPedido>(); }

        public int Codigo { get; private set; }
        public Guid IdCliente { get; private set; }
        public Guid? IdVoucher { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public EPedidoStatus PedidoStatus { get; private set; }

        // Relacionamento para o EF
        public virtual Voucher Voucher { get; private set; }

        private readonly List<ItemPedido> _itensPedido;
        public IReadOnlyCollection<ItemPedido> ItensPedido => _itensPedido;

        public void CalcularValorPedido()
        {
            ValorTotal = ItensPedido.Sum(p => p.CalcularValor());
            CalcularValorTotalDesconto();
        }

        public void AplicarVoucher(Voucher voucher)
        {
            Voucher = voucher;
            VoucherUtilizado = true;
            CalcularValorPedido();
        }

        public bool ItemPedidoExistente(ItemPedido item)
        {
            return _itensPedido.Any(p => p.IdProduto == item.IdProduto);
        }

        public void AdicionarItem(ItemPedido item)
        {
            if (!item.EhValido()) return;

            item.AssociarPedido(Id);

            if (ItemPedidoExistente(item))
            {
                var itemExistente = _itensPedido.FirstOrDefault(p => p.IdProduto == item.IdProduto);
                itemExistente.AdicionarUnidades(item.Quantidade);
                item = itemExistente;

                _itensPedido.Remove(itemExistente);
            }

            item.CalcularValor();
            _itensPedido.Add(item);

            CalcularValorPedido();
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if(Voucher.TipoDescontoVoucher == ETipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }

        public void RemoverItem(ItemPedido item)
        {
            if (!item.EhValido()) return;

            var itemExistente = ItensPedido.FirstOrDefault(p => p.IdProduto == item.IdProduto);

            if (itemExistente == null) throw new DomainException("O item não pertence ao pedido");
            _itensPedido.Remove(itemExistente);

            CalcularValorPedido();
        }

        public void AtualizarItem(ItemPedido item)
        {
            if (!item.EhValido()) return;
            item.AssociarPedido(Id);

            var itemExistente = ItensPedido.FirstOrDefault(p => p.IdProduto == item.IdProduto);

            if (itemExistente == null) throw new DomainException("O item não pertence ao pedido");

            _itensPedido.Remove(itemExistente);
            _itensPedido.Add(item);

            CalcularValorPedido();
        }

        public void AtualizarUnidades(ItemPedido item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        public void TornarRascunho()
        {
            PedidoStatus = EPedidoStatus.Rascunho;
        }

        public void IniciarPedido()
        {
            PedidoStatus = EPedidoStatus.Iniciado;
        }

        public void FinalizarPedido()
        {
            PedidoStatus = EPedidoStatus.Pago;
        }

        public void CancelarPedido()
        {
            PedidoStatus = EPedidoStatus.Cancelado;
        }

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid idCliente)
            {
                var pedido = new Pedido
                {
                    IdCliente = idCliente,
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }
    }
}
