using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Context;
using APICatalogo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext? _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "ObterProdutos")]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.produtos.AsNoTracking().ToListAsync();
            return produtos;
        }

        [HttpGet("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<Produto>> GetProdutoById(int id)
        {
            var produto = await _context.produtos.Where(p => p.ProdutoId == id).AsNoTracking().FirstOrDefaultAsync();
            if (produto is null) return NotFound();

            return produto;
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post(Produto produto)
        {
            if (produto == null) return BadRequest();

            await _context.produtos.AddAsync(produto);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterProdutos", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Produto> Put(int id, Produto produto)
        {
            try
            {

                if (id != produto.ProdutoId) return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "ID Diferentes" });

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok("Produto editado");
            }

            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Produto inexistente" });
            }
        }

        [HttpDelete("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Produto> Remove(int id)
        {
            try
            {
                var produto = _context.produtos.Where(p => p.ProdutoId == id).FirstOrDefault();
                if (produto is null) return BadRequest("Produto inexistente");

                _context.produtos.Remove(produto);
                _context.SaveChanges();

                return Ok("Produto removido");
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro de exclusão" });
            }
        }

    }
}
