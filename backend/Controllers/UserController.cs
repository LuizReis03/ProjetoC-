using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace backend.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{

    [HttpGet("{nome}/{senha}")]
    public object Login(string nome, string senha)
    {
        Access<Usuario> access = new Access<Usuario>();
        var user = access.All
            .FirstOrDefault(u => u.Nome == nome);
        if (user == null)
        {
            return new {
                status = "Erro",
                message = "Não existe usuário com este nome."
            };
        }

        if (user.Senha != senha)
        {
            return new {
                status = "Erro",
                message = "Senha incorreta."
            };
        }

        var key = TokenSystem.NewToken(user);

        return new {
            status = "Sucesso",
            token = key
        };
    }
}
