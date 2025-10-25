namespace Api.Models.Dtos.Responses;

public record RegisterResponse(string UserName);

// 🔹 LoginResponse — це простий DTO (Data Transfer Object)
//    Використовується для повернення результату логіну користувачу
//    Після успішної автентифікації сервер надсилає клієнту JWT-токен,
//    який клієнт потім використовує для доступу до захищених ресурсів.

public record LoginResponse(string Jwt); 
// 🔸 'record' — це тип у C#, який зручний для передачі даних (immutable object)
// 🔸 'Jwt' — властивість, що містить сам токен у вигляді рядка
//     приклад JSON-відповіді з цим record:
//
//     {
//       "jwt": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
//     }


public record AuthUserInfo(string Id, string UserName, string Role);
