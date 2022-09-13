using Microsoft.AspNetCore.Mvc;
using APICatalogo.Context;
using APICatalogo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Filters;

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
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            try
            {
                var produtos = await _context.produtos.ToListAsync();
                return produtos;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpGet("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<Produto>> GetProdutoById(int id)
        {
            try
            {
                var produto = await _context.produtos.Where(p => p.ProdutoId == id).FirstOrDefaultAsync();
                if(produto == null) return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Produto não encontrado" });

                return produto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, 
                    new { message = "Erro ao consultar!" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post([FromBody] Produto produto)
        {
            try
            {
                if (produto == null) return BadRequest();

                await _context.produtos.AddAsync(produto);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterProdutos", new { id = produto.ProdutoId }, produto);

            }
            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao criar produto" });
            }
        }

        [HttpPut("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Produto> Put(int id, [FromBody] Produto produto)
        {
            try
            {

                if (id != produto.ProdutoId) return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "ID Diferentes" });

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok("Produto editado");
            }

            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status404NotFound,
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

                return Ok(new { message = "Produto removido" });
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro de exclusão" });
            }
        }

    }
}
