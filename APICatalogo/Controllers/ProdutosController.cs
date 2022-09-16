using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Repository.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public ProdutosController(IUnitOfWork context, IMapper mapper, ILogger<ProdutosController> logger)
        {
            _uof = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet(Name = "ObterProdutos")]
       // [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            try
            {
                _logger.LogInformation(" ============ GET /produtos ===============");
                var produtos = await _uof.ProdutoRepository.Get().ToListAsync();
                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDTO;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpGet("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<ProdutoDTO>> GetProdutoById(int id)
        {
            try
            {
                _logger.LogInformation($" ============ GET BY ID /produtos/id = {id} ===============");

                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                var produtosDTO = _mapper.Map<ProdutoDTO>(produto);
                if (produto == null) return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Produto não encontrado" });

                return produtosDTO;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, 
                    new { message = "Erro ao consultar!" });
            }
        }


        [HttpGet("precos")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutoPreco()
        {
            _logger.LogInformation(" ============ GET BY PRECO /produtos/precos ===============");

            var produto = await _uof.ProdutoRepository.GetProdutosByPreco();
            return _mapper.Map<List<ProdutoDTO>>(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Post([FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                if (!ModelState.IsValid || produtoDto == null) return BadRequest(ModelState);
                _logger.LogInformation(" ============ POST /produtos ===============");

                var produtos = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Add(produtos);
                await _uof.Commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produtos);

                return new CreatedAtRouteResult("ObterProdutos", new { id = produtoDTO.ProdutoId }, produtoDTO);

            }
            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao criar produto" });
            }
        }

        [HttpPut("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<ProdutoDTO>> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                if (!ModelState.IsValid || produtoDto.ProdutoId != id) return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro de edição" });

                _logger.LogInformation($" ============ PUT /produtos/id = {id} ===============");

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Update(produto);
                await _uof.Commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                //return Ok("Produto editado");
                return produtoDTO;
            }

            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Produto inexistente" });
            }
        }

        [HttpDelete("{id:int:min(1):maxlength(5)}")]
        public async Task<ActionResult<ProdutoDTO>> Remove(int id)
        {
            try
            {
                var produto = await _uof.ProdutoRepository.GetById(p => p.CategoriaId == id);
                if (produto is null) return BadRequest("Produto inexistente");

                _logger.LogInformation($" ============ REMOVE /produtos/id = {id} ===============");


                _uof.ProdutoRepository.Delete(produto);
                await _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                //return Ok(new { message = "Produto removido" });
                return produtoDto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro de exclusão" });
            }
        }

    }
}
