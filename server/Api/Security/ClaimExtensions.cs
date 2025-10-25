using System.Security.Claims;
using Api.Models.Dtos.Responses;

namespace Api.Security;

// 🔹 Статичний клас з методами-розширеннями для роботи з Claims
public static class ClaimExtensions
{
    // 🔸 Отримує Id користувача з об’єкта ClaimsPrincipal
    public static string GetUserId(this ClaimsPrincipal claims) =>
        claims.FindFirst(ClaimTypes.NameIdentifier)!.Value;

    // 🔸 Перетворює наш об’єкт AuthUserInfo у набір claims (для JWT)
    public static IEnumerable<Claim> ToClaims(this AuthUserInfo user) =>
        [new("sub", user.Id.ToString()), new("role", user.Role)];

    // 🔸 Створює ClaimsPrincipal на основі AuthUserInfo
    public static ClaimsPrincipal ToPrincipal(this AuthUserInfo user) =>
        new ClaimsPrincipal(new ClaimsIdentity(user.ToClaims()));
}