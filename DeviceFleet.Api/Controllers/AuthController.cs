using System.ComponentModel.DataAnnotations;
using DeviceFleet.Api.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DeviceFleet.Api.Controllers;

public record LoginRequest([Required] string Username, [Required] string Password);
public record LoginResponse(string Token, string Role);

[ApiController]
[Route("api/[controller]")]
public class AuthController(TokenService tokens) : ControllerBase
{
    // DEMO: w prawdziwej aplikacji użytkownicy i ZAHASZOWANE hasła trzymane są w bazie.
    // Tutaj uproszczone na potrzeby portfolio.
    private static readonly Dictionary<string, (string Password, string Role)> Users = new()
    {
        ["admin"] = ("admin123", "Admin"),
        ["operator"] = ("operator123", "Operator")
    };

    /// <summary>Loguje użytkownika i zwraca token JWT.</summary>
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        if (!Users.TryGetValue(request.Username, out var user) || user.Password != request.Password)
            return Unauthorized("Nieprawidłowa nazwa użytkownika lub hasło.");

        var token = tokens.CreateToken(request.Username, user.Role);
        return Ok(new LoginResponse(token, user.Role));
    }
}
