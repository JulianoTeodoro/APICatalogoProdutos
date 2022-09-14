using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IQueryable<Produto> GetProdutosByPreco();

    }
}
