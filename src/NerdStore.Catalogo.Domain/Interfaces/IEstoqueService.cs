using System;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Interfaces
{
    public interface IEstoqueService : IDisposable
    {
        Task<bool> ReporEstoque(Guid idProduto, int quantidade);
        Task<bool> DebitarEstoque(Guid idProduto, int quantidade);
    }
}
