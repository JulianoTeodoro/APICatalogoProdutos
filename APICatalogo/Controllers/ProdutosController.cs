using APICatalogo.Filters;
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
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet(Name = "ObterProdutos")]
       // [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            try
            {
                var produtos = _uof.ProdutoRepository.Get().ToList();
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
        public ActionResult<ProdutoDTO> GetProdutoById(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
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
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutoPreco()
        {
            var produto = _uof.ProdutoRepository.GetProdutosByPreco().ToList();
            return _mapper.Map<List<ProdutoDTO>>(produto);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post([FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                if (!ModelState.IsValid || produtoDto == null) return BadRequest(ModelState);
                var produtos = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Add(produtos);
                _uof.Commit();

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
        public ActionResult<ProdutoDTO> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                if (!ModelState.IsValid || produtoDto.ProdutoId != id) return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro de edição" });

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Update(produto);
                _uof.Commit();

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
        public ActionResult<ProdutoDTO> Remove(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.CategoriaId == id);
                if (produto is null) return BadRequest("Produto inexistente");

                _uof.ProdutoRepository.Delete(produto);
                _uof.Commit();

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
