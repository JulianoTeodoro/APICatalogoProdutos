using System.ComponentModel.DataAnnotations;
namespace APICatalogo.Repository.DTOs
{
    public class UsuarioDTO
    {
        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
