using Api.Models.Dtos.Requests;
using Api.Models.Dtos.Responses;
using Api.Security;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService service,ITokenService tokenService ) : ControllerBase
{
    [HttpPost] // 🔹 Атрибут: метод буде обробляти HTTP POST-запити
    [Route("login")] // 🔹 Вказує маршрут: /login
    public async Task<LoginResponse> Login([FromBody] LoginRequest request)
    {
        // 🔸 1️⃣ Отримуємо дані користувача (AuthUserInfo) після перевірки логіну/пароля
        //     service.Authenticate(request) — це метод, який перевіряє, чи існує користувач і чи правильний пароль
        var userInfo = service.Authenticate(request);

        // 🔸 2️⃣ Створюємо JWT-токен для цього користувача
        //     tokenService — це наш ITokenService (JwtService), який генерує підписаний токен
        var token = tokenService.CreateToken(userInfo);

        // 🔸 3️⃣ Формуємо відповідь (LoginResponse), в яку вкладаємо згенерований токен
        //     Цей токен потім отримає клієнт (React, мобільний додаток тощо)
        return new LoginResponse(token);
    }


    [HttpPost]
    [Route("register")]
    public async Task<RegisterResponse> Register([FromBody] RegisterRequest request)
    {
        var userInfo = await service.Register(request);
        return new RegisterResponse(UserName: userInfo.UserName);
    }

    [HttpPost]
    [Route("logout")]
    public async Task<IResult> Logout()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("userinfo")]
    public async Task<AuthUserInfo?> UserInfo()
    {
        return service.GetUserInfo(User);
    }
}