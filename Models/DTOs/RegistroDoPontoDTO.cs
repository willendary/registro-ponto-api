using System.ComponentModel.DataAnnotations;

namespace RegistroDoPonto.Models.DTOs;
public class RegistroDoPontoDTO
{
    [Required]
    public string UsuarioId { get; set; }
    [Required]
    public DateTime DataHora { get; set; }
    [Required]
    public TipoRegistro Tipo { get; set; }
}