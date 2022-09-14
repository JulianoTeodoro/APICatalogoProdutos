using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {

        }

        public IQueryable<Produto> GetProdutosByPreco()
        {
            return Get().OrderBy(p => p.Preco);
        }
    }
}
