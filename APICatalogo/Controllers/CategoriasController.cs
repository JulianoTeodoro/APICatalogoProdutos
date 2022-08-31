using Microsoft.AspNetCore.Mvc;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;
        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "ObterCategoria")]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
           // var categoria = await _context.categorias.Take(10).AsNoTracking().ToListAsync();
            var categoria = await _context.categorias.AsNoTracking().ToListAsync();
            return categoria;
        }

        [HttpGet("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<Categoria>> GetById(int id)
        {
            Categoria? categoria = await _context.categorias.AsNoTracking().Where(p => p.CategoriaId == id).FirstOrDefaultAsync();
            if (categoria is null) return NotFound();
            return categoria;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetProdutoByCategoria()
        {
            var categoria = await _context.categorias.AsNoTracking().Include(p => p.produtos).ToListAsync();
            return categoria;
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria categoria)
        {
            if (categoria is null) return NotFound();

            await _context.categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterCategoria", new { Id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Categoria> Put(int id, Categoria categoria)
        {
            if (categoria.CategoriaId != id) return NotFound();

            try
            {
                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok(categoria);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("Categoria não encontrada");
            }
            
        }

        [HttpDelete("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var categoria = _context.categorias.Where(p => p.CategoriaId == id).FirstOrDefault();

                if (categoria is null) return NotFound("Não encontrado");

                _context.categorias.Remove(categoria);
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
