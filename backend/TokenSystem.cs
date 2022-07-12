
using System;
using System.Text;
using System.Linq;

public static class TokenSystem
{
    public static string NewToken(Usuario user)
    {
        var key = generateKey(user.Nome);

        Access<Token> tokenaccess = new Access<Token>();
        Token token = new Token();
        token.Usuario = user.ID;
        token.Chave = key;
        tokenaccess.Add(token);

        return key;
    }

    private static string generateKey(string username)
    {
        int seed = DateTime.Now.Millisecond + 
            1000 * DateTime.Now.Day + 
            100000 * DateTime.Now.Second;
        Random rand = new Random(seed);
        StringBuilder sb = new StringBuilder();

        foreach (char c in username)
        {
            int num = c + rand.Next();
            sb.Append((char)('a' + num % 26));
        }
        while (sb.Length < 40)
        {
            int num = rand.Next();
            sb.Append((char)('a' + num % 26));
        }
        return sb.ToString();
    }

    public static Usuario ValidateToken(string token)
    {
        var searchedtoken = searchToken(token);
        if (searchedtoken == null)
            return null;
        var user = searchUser(searchedtoken.Usuario);
        return user;
    }

    private static Token searchToken(string token)
    {
        Access<Token> access = new Access<Token>();
        var result = access.All
            .FirstOrDefault(t => t.Chave == token);
        return result;
    }

    private static Usuario searchUser(int id)
    {
        Access<Usuario> access = new Access<Usuario>();
        foreach (var user in access.All)
        {
            if (user.ID == id)
                return user;
        }
        return null;
    }
}