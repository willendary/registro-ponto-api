using System.ComponentModel.DataAnnotations;
using RegistroDoPonto.Models.Enums;

namespace RegistroDoPonto.Models.DTOs;
public class RegistroDoPontoDTO
{
    public int Id { get; set; } // Adicionado para incluir o ID do registro
    [Required]
    public string UsuarioId { get; set; }
    [Required]
    public DateTime DataHora { get; set; }
    [Required]
    public TipoRegistro Tipo { get; set; }
}