using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace backend.Controllers;

[ApiController]
[Route("register")]

public class RegisterController : ControllerBase
{

    [HttpGet("{name}/{email}/{password}/{passwordConfirm}")]

    public object Register(string name, string email, string password, string passwordConfirm)
    {
        
        Access<Usuario> access = new Access<Usuario>();

        var novoUser = access.All
            .FirstOrDefault(u => u.Email == email);
        if (novoUser != null)
        {
            return new {
                status = "Erro",
                message = "Já existe um usuário com este email."
            };
        }

        if (passwordConfirm != password)
        {
            return new {
                status = "Erro",
                message = "Senhas incompatíveis!"
            };
        }

        var usuario = new Usuario(); 
        usuario.Nome = name;
        usuario.Senha = password;
        usuario.Email = email;

        access.Add(usuario);

        return new {
                status = "Sucesso",
                message = "Registrado com sucesso!"
            };

    }
}