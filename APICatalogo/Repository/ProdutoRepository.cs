using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Produto>> GetProdutosByPreco()
        {
            return await Get().OrderBy(p => p.Preco).ToListAsync();
        }
    }
}
