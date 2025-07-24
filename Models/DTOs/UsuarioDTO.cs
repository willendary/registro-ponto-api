using System.ComponentModel.DataAnnotations;

namespace RegistroDoPonto.Models.DTOs;

public class UsuarioDTO
{
    [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O email do usuário é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.", MinimumLength = 6)]
    public string? Password { get; set; } // Opcional para edição, obrigatório para criação

    public string? Role { get; set; } // Para atribuição de role na criação/edição
}

