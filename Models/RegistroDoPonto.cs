using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroDoPonto.Models;

public class RegistroDoPonto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UsuarioId { get; set; }
    public DateTime DataHora { get; set; }
    public TipoRegistro Tipo { get; set; }

    // Propriedade de navegação
    public Usuario Usuario { get; set; }
}
