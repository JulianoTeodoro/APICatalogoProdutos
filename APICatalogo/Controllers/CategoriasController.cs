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
           try
            {
                var categoria = await _context.categorias.ToListAsync();
                return categoria;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpGet("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<Categoria>> GetById(int id)
        {
            try
            {
                var categoria = await _context.categorias.Where(p => p.CategoriaId == id).FirstOrDefaultAsync();

                if (categoria == null) return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Categoria não encontrada" });

                return categoria;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro de consulta" });
            }
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetProdutoByCategoria()
        {
            try
            {
                var categoria = await _context.categorias.Include(p => p.produtos).ToListAsync();
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria categoria)
        {
            try
            {
                if (categoria is null) return NotFound();

                await _context.categorias.AddAsync(categoria);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterCategoria", new { Id = categoria.CategoriaId }, categoria);
            }
            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro ao criar" });
            }
        }

        [HttpPut("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Categoria> Put(int id, Categoria categoria)
        {
            try
            {
                if (categoria.CategoriaId != id) return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "ID Diferentes" });

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(categoria);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Categoria não encontrada" });
            }
            
        }

        [HttpDelete("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var categoria = _context.categorias.Where(p => p.CategoriaId == id).FirstOrDefault();

                if (categoria is null) return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Categoria não encontrada" });

                _context.categorias.Remove(categoria);
                _context.SaveChanges();

                return Ok( new { message = "Categoria removida" } );   
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro de exclusão" });
            }

        }

    }
}
