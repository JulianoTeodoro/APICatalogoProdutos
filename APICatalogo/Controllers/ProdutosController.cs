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

        [HttpGet("{id}")]
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

            _context.produtos.Add(produto);
            _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterProdutos", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Produto>> Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId) return BadRequest("Id diferentes");

            try
            {
                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChangesAsync();
            }

            catch(DbUpdateConcurrencyException)
            {
                return BadRequest("Erro de edição");
            }

            return Ok("Produto editado");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Produto>> Remove(int id)
        {
            var produto = await _context.produtos.Where(p => p.ProdutoId == id).FirstOrDefaultAsync();
            if (produto is null) return BadRequest("Produto inexistente");

            _context.produtos.Remove(produto);
            _context.SaveChangesAsync();

            return Ok("Produto removido");
        }

    }
}
