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
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "ObterProdutos")]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.produtos.ToList();
            return produtos;
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetProdutoById(int id)
        {
            var produto = _context.produtos.Where(p => p.ProdutoId == id).FirstOrDefault();
            if (produto is null) return NotFound();

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto == null) return BadRequest();

            _context.produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProdutos", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult<Produto> Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId) return BadRequest("Id diferentes");

            try
            {
                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();
            }

            catch(DbUpdateConcurrencyException)
            {
                return BadRequest("Erro de edição");
            }

            return Ok("Produto editado");
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Remove(int id)
        {
            var produto = _context.produtos.Where(p => p.ProdutoId == id).FirstOrDefault();
            if (produto is null) return BadRequest("Produto inexistente");

            _context.produtos.Remove(produto);
            _context.SaveChanges();

            return Ok("Produto removido");
        }

    }
}
