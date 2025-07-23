using System.ComponentModel.DataAnnotations;

namespace RegistroDoPonto.Models.DTOs;

public class UsuarioDTOResponse
{
    public string Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}

