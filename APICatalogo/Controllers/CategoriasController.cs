using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Repository.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : Controller
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet(Name = "ObterCategoria")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
           // var categoria = await _context.categorias.Take(10).AsNoTracking().ToListAsync();
           try
            {
                var categoria = _uof.CategoriaRepository.Get().ToList();
                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);
                return categoriaDto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpGet("{id:int:min(1):maxlength(5)}")]
        public ActionResult<CategoriaDTO> GetById(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria == null) return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Categoria não encontrada" });

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro de consulta" });
            }
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetProdutoByCategoria()
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();

                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);

                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria", new { Id = categoriaDTO.CategoriaId }, categoriaDTO);
            }
            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro ao criar" });
            }
        }

        [HttpPut("{id:int:min(1):maxlength(5)}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (categoriaDto.CategoriaId != id) return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "ID Diferentes" });

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Categoria não encontrada" });
            }
            
        }

        [HttpDelete("{id:int:min(1):maxlength(5)}")]
        public ActionResult<CategoriaDTO> Delete(int id)
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
