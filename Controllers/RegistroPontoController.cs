using Microsoft.AspNetCore.Mvc;
using RegistroDoPonto.Models;
using RegistroDoPonto.Models.DTOs;
using RegistroDoPonto.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RegistroPontoController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<Usuario> _userManager;

    public RegistroPontoController(AppDbContext context, UserManager<Usuario> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("ByUsuario/{userId}")]
    public async Task<ActionResult<IEnumerable<RegistroDoPontoDTO>>> GetRegistrosByUsuarioId(string userId)
    {
        var registros = await _context.Registros
                                .Where(r => r.UsuarioId == userId)
                                .ToListAsync();

        return Ok(registros.Select(r => new RegistroDoPontoDTO
        {
            UsuarioId = r.UsuarioId,
            DataHora = r.DataHora,
            Tipo = r.Tipo
        }));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegistroDoPontoDTO>>> GetRegistros()
    {
        var registros = await _context.Registros.ToListAsync();
        return Ok(registros.Select(r => new RegistroDoPontoDTO
        {
            UsuarioId = r.UsuarioId,
            DataHora = r.DataHora,
            Tipo = r.Tipo
        }));
    }
    [HttpPost]
    public async Task<ActionResult<RegistroDoPontoDTO>> RegistrarPonto([FromBody] RegistroDoPontoDTO registroDto)
    {
        var usuarioExiste = await _userManager.FindByIdAsync(registroDto.UsuarioId);
        if (usuarioExiste == null)
        {
            return BadRequest("Usuário não encontrado.");
        }

        var novoRegistro = new RegistroDoPonto.Models.RegistroDoPonto
        {
            UsuarioId = registroDto.UsuarioId,
            DataHora = registroDto.DataHora,
            Tipo = registroDto.Tipo
        };

        _context.Registros.Add(novoRegistro);
        await _context.SaveChangesAsync();

        var registroRetornoDto = new RegistroDoPontoDTO
        {
            UsuarioId = novoRegistro.UsuarioId,
            DataHora = novoRegistro.DataHora,
            Tipo = novoRegistro.Tipo
        };

        return CreatedAtAction(nameof(GetRegistro), new { id = novoRegistro.Id }, registroRetornoDto);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<RegistroDoPontoDTO>> GetRegistro(int id)
    {
        var registro = await _context.Registros.FirstOrDefaultAsync(r => r.Id == id);
        if (registro == null)
        {
            return NotFound();
        }

        var registroRetornoDto = new RegistroDoPontoDTO
        {
            UsuarioId = registro.UsuarioId,
            DataHora = registro.DataHora,
            Tipo = registro.Tipo
        };

        return registroRetornoDto;
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarRegistro(int id, [FromBody] RegistroDoPontoDTO registroDto)
    {
        var registroExistente = await _context.Registros.FirstOrDefaultAsync(r => r.Id == id);
        if (registroExistente == null)
        {
            return NotFound();
        }

        // Não permitir a alteração do UsuarioId em uma atualização
        // Se o UsuarioId precisar ser alterado, um novo registro deve ser criado.
        registroExistente.DataHora = registroDto.DataHora;
        registroExistente.Tipo = registroDto.Tipo;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Registros.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarRegistro(int id)
    {
        var registro = await _context.Registros.FirstOrDefaultAsync(r => r.Id == id);
        if (registro == null)
        {
            return NotFound();
        }
        _context.Registros.Remove(registro);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}