using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroDoPonto.Models;
using RegistroDoPonto.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace RegistroDoPonto.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class UsuariosController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsuariosController(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // POST: api/Usuarios
    [HttpPost]
    public async Task<ActionResult<UsuarioDTOResponse>> PostUsuario([FromBody] RegistroUsuarioDTO registroDto)
    {
        var user = new Usuario { UserName = registroDto.Email, Email = registroDto.Email, Nome = registroDto.Nome };
        var result = await _userManager.CreateAsync(user, registroDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        if (!string.IsNullOrEmpty(registroDto.Role))
        {
            await _userManager.AddToRoleAsync(user, registroDto.Role);
        }
        else
        {
            // Adiciona a role "User" por padrão se nenhuma for especificada
            await _userManager.AddToRoleAsync(user, "User");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var userResponseDto = new UsuarioDTOResponse
        {
            Id = user.Id,
            Nome = user.Nome,
            Email = user.Email,
            Roles = roles
        };

        return CreatedAtAction(nameof(GetUsuario), new { id = user.Id }, userResponseDto);
    }

    // GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDTOResponse>>> GetUsuarios()
    {
        var usuarios = await _userManager.Users.ToListAsync();
        var usuariosDtoResponse = new List<UsuarioDTOResponse>();

        foreach (var u in usuarios)
        {
            var roles = await _userManager.GetRolesAsync(u);
            usuariosDtoResponse.Add(new UsuarioDTOResponse
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Roles = roles
            });
        }
        return Ok(usuariosDtoResponse);
    }

    // GET: api/Usuarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDTOResponse>> GetUsuario(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(usuario);
        var usuarioResponseDto = new UsuarioDTOResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Roles = roles
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

        // Atualizar roles
        var currentRoles = await _userManager.GetRolesAsync(usuario);
        if (!string.IsNullOrEmpty(usuarioDto.Role) && !currentRoles.Contains(usuarioDto.Role))
        {
            await _userManager.RemoveFromRolesAsync(usuario, currentRoles);
            await _userManager.AddToRoleAsync(usuario, usuarioDto.Role);
        }

        return NoContent();
    }

    // GET: api/Usuarios/roles
    [HttpGet("roles")]
    public ActionResult<IEnumerable<string>> GetRoles()
    {
        return Ok(_roleManager.Roles.Select(r => r.Name).ToList());
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
