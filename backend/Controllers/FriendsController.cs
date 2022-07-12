using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace backend.Controllers;

[ApiController]
[Route("friend")]
public class FriendsController : ControllerBase
{
    [HttpGet("{friendname}/{token}")]
    public object RequestFriend(string friendname, string token)
    {
        var user = TokenSystem.ValidateToken(token);
        
        if (user == null)
        {
            return new {
                status = "Erro",
                message = "Token Inválido"
            };
        }

        Access<Usuario> access = new Access<Usuario>();

        var todosUsuarios = access.All
            .FirstOrDefault(u => u.Nome == friendname);
        if (todosUsuarios == null)
        {
            return new {
                status = "Erro",
                message = "Não existe usuário com este nome."
            };
        }

        Access<Amizade> acessAmizade = new Access<Amizade>();
        var amizade = new Amizade();

        if (user.ID == todosUsuarios.ID)
        {
            return new {
                status = "Erro",
                message = "Erro ao fazer amizade."
            };
        }

        List<Amizade> todasAmizades = new List<Amizade>();
        todasAmizades = acessAmizade.All;

        foreach (var i in todasAmizades)
        {
            if (i.Usuario1 == user.ID && i.Usuario2 == todosUsuarios.ID)
            {
                return new {
                    status = "Erro",
                    message = "Usuários já possuem amizade!"
                };
            }
        }

        amizade.Usuario1 = user.ID;
        amizade.Usuario2 = todosUsuarios.ID;

        acessAmizade.Add(amizade);
        return new {
            status = "Sucesso",
            message = "Amizade feita!"
        };
    }
}