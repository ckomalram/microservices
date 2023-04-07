namespace Servicios.api.Seguridad.Core.Entities.JwtLogic;


public class UserSesion : IUserSesion
{
    private readonly IHttpContextAccessor httpcontextaccesor;

    public UserSesion(IHttpContextAccessor _httpcontextaccesor)
    {
        httpcontextaccesor = _httpcontextaccesor;
    }
    public string GetUserSession()
    {
        var username = httpcontextaccesor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == "username")?.Value;
        return username;
    }
}



public interface IUserSesion
{
    string GetUserSession();

}

