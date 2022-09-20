using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Repository.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("[controller]")]
    [ApiController]
    //[EnableCors("PermitirApiRequest")]
    [EnableCors]
    public class CategoriasController : Controller
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public CategoriasController(IUnitOfWork context, IMapper mapper, ILogger<CategoriasController> logger)
        {
            _uof = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet(Name = "ObterCategoria")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
           // var categoria = await _context.categorias.Take(10).AsNoTracking().ToListAsync();
           try
            {
                var categoria = await _uof.CategoriaRepository.Get().ToListAsync();
                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);

                _logger.LogInformation($"=============== GET /categorias");

                return categoriaDto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpGet("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<CategoriaDTO>> GetById(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria == null) return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Categoria não encontrada" });

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                _logger.LogInformation($"=============== GET /categorias/id = {id}");

                return categoriaDto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro de consulta" });
            }
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetProdutoByCategoria()
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetCategoriasProdutos();

                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);

                _logger.LogInformation($"=============== GET /categorias/produtos");


                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                _logger.LogInformation($"=============== POST /categorias");

                return new CreatedAtRouteResult("ObterCategoria", new { Id = categoriaDTO.CategoriaId }, categoriaDTO);
            }
            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro ao criar" });
            }
        }

        [HttpPut("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (categoriaDto.CategoriaId != id) return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "ID Diferentes" });

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                _logger.LogInformation($"=============== PUT /categorias/id = {id}");

                return Ok(categoriaDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Categoria não encontrada" });
            }
            
        }

        [HttpDelete("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (!ModelState.IsValid || categoria is null) return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Categoria não encontrada" });

                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

                _logger.LogInformation($"=============== DELETE /categorias/id = {id}");

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
