using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }

        [HttpGet(Name = "ObterProdutos")]
       // [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                return _uof.ProdutoRepository.Get().ToList();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao consultar" });
            }
        }

        [HttpGet("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Produto> GetProdutoById(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null) return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Produto não encontrado" });

                return produto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, 
                    new { message = "Erro ao consultar!" });
            }
        }


        [HttpGet("precos")]
        public ActionResult<IEnumerable<Produto>> GetProdutoPreco()
        {
            return _uof.ProdutoRepository.GetProdutosByPreco().ToList();
        }

        [HttpPost]
        public ActionResult<Produto> Post([FromBody] Produto produto)
        {
            try
            {
                if (!ModelState.IsValid ||produto == null) return BadRequest(ModelState);

                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                return new CreatedAtRouteResult("ObterProdutos", new { id = produto.ProdutoId }, produto);

            }
            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro ao criar produto" });
            }
        }

        [HttpPut("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Produto> Put(int id, [FromBody] Produto produto)
        {
            try
            {
                var product = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (!ModelState.IsValid || produto.ProdutoId != id) return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Erro de edição" });

                _uof.ProdutoRepository.Update(produto);
                _uof.Commit();

                return Ok("Produto editado");
            }

            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = "Produto inexistente" });
            }
        }

        [HttpDelete("{id:int:min(1):maxlength(5)}")]
        public ActionResult<Produto> Remove(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.CategoriaId == id);
                if (produto is null) return BadRequest("Produto inexistente");

                _uof.ProdutoRepository.Delete(produto);
                _uof.Commit();

                return Ok(new { message = "Produto removido" });
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro de exclusão" });
            }
        }

    }
}
