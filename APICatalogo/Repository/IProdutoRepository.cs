using APICatalogo.Models;

namespace APICatalogo.Repository
{
    interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosByPreco();

    }
}
