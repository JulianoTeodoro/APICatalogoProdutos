using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : Controller
    {
        private readonly IUnitOfWork _uof;
        public CategoriasController(IUnitOfWork context)
        {
            _uof = context;
        }

        [HttpGet(Name = "ObterCategoria")]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
           // var categoria = await _context.categorias.Take(10).AsNoTracking().ToListAsync();
           try
            {
                return _uof.CategoriaRepository.Get().ToList();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpGet("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Categoria> GetById(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

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
        public ActionResult<IEnumerable<Categoria>> GetProdutoByCategoria()
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpPost]
        public ActionResult<Categoria> Post([FromBody] Categoria categoria)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();

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

                _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();

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
                var categoria = _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (!ModelState.IsValid || categoria is null) return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Categoria não encontrada" });

                _uof.CategoriaRepository.Delete(categoria);
                _uof.Commit();

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
