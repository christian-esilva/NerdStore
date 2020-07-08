using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Vendas.Domain.Enums
{
    public enum EPedidoStatus
    {
        Rascunho = 0,
        Iniciado = 1,
        Pago = 4,
        Entregue = 5,
        Cancelado = 6
    }
}
