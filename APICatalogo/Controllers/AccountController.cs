using APICatalogo.Repository.DTOs;
using APICatalogo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"AutorizaController acessado em: {DateTime.Now.ToLongDateString()}";
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDTO>> Register([FromBody] UsuarioDTO usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Erro de registro!");
            }

            var user = new IdentityUser
            {
                UserName = usuarioDto.Email,
                Email = usuarioDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            
            var tokenService = new TokenService(_config);

            return Ok(tokenService.GeraToken(usuarioDto));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDTO>> Login([FromBody] UsuarioDTO usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Erro de login!");
            }

            var result = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Password,
                isPersistent: false, lockoutOnFailure: false);

            var tokenService = new TokenService(_config);

            if (result.Succeeded)
            {
                return Ok(tokenService.GeraToken(usuario));
            } else
            {
                ModelState.AddModelError(string.Empty, "Login Invalido");
                return BadRequest(ModelState);
            }

        }

    }
}
