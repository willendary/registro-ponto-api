using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroDoPonto.Models;
using RegistroDoPonto.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace RegistroDoPonto.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;

    public UsuariosController(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    // GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDTOResponse>>> GetUsuarios()
    {
        var usuarios = await _userManager.Users.ToListAsync();
        var usuariosDtoResponse = usuarios.Select(u => new UsuarioDTOResponse
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email
        });
        return Ok(usuariosDtoResponse);
    }

    // GET: api/Usuarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDTO>> GetUsuario(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        var usuarioResponseDto = new UsuarioDTOResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };

        return Ok(usuarioResponseDto);
    }



    // PUT: api/Usuarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(string id, UsuarioDTO usuarioDto)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        usuario.Nome = usuarioDto.Nome;
        usuario.Email = usuarioDto.Email;
        usuario.UserName = usuarioDto.Email; // Sincronizar UserName com Email

        var result = await _userManager.UpdateAsync(usuario);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        return NoContent();
    }

    // DELETE: api/Usuarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(usuario);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}
