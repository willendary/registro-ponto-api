using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroDoPonto.Models;

public class Usuario : IdentityUser
{
    [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; }

    // O Email já é fornecido por IdentityUser, mas podemos adicionar validações
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public override string Email { get; set; }

    // Propriedade de navegação para os registros de ponto
    public virtual ICollection<RegistroDoPonto> Registros { get; set; } = new List<RegistroDoPonto>();

    [Required]
    public bool IsAtivo { get; set; } = true;
}