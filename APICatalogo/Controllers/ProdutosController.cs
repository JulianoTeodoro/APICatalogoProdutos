using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Context;
using APICatalogo.Models;
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

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            List<Produto> produtos = _context.Produtos.ToList();
            if (produtos is null)
            {
                return NotFound();
            }
            return produtos;
        }

        [HttpGet]
        [Route("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> GetProdutoById(int id)
        {
            var produto = _context.Produtos.Where(p => p.ProdutoId == id).FirstOrDefault();
            if(produto is null)
            {
                return NotFound("Produto não existe");
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null) return BadRequest();
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId) return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok("Produto editado");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto is null) return NotFound("Produto inexistente!");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok("Produto removido");
        }

    }
}
