//Requests — це що надсилає клієнт (React) у запиті.

//Responses — це що сервер віддає назад клієнту у відповіді.

using System.ComponentModel.DataAnnotations;
using Api.Etc;
using Api.Models.Dtos.Requests;
using Api.Models.Dtos.Responses;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Api.Services;

public interface IAuthService
{
    //тут я перевірю меіл і логін
    //
    AuthUserInfo Authenticate(LoginRequest request);
    //тут я створю нового користувача
    Task<AuthUserInfo> Register(RegisterRequest request);
}

public class AuthService(
    ILogger<AuthService> _logger,//logger
    IPasswordHasher<User> _passwordHasher,//password hasher
    IRepository<User> _userRepository//user repository
    ):IAuthService//here we implement interface
{
  
    
    public AuthUserInfo Authenticate(LoginRequest request)//here we check login
    {
        try
        {
            
        
        // _userRepository — об’єкт, який дає доступ до таблиці користувачів через код;
        //Single тільки треба один користувач
      var  user = _userRepository.Query().Single(u=>u.Email==request.Email);//try to find user by email in db
      var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);//verify password
      if (result == PasswordVerificationResult.Success)//if password is correct
      {
          return new AuthUserInfo(user.Id, user.UserName, user.Role);//return user info but without password
      }
      
        }catch(Exception e)
        {
            _logger.LogError(e.Message);
        }
        throw new AuthenticationError();
    }
    

    public async Task<AuthUserInfo> Register(RegisterRequest request)//here we create new user
    {
        if(_userRepository.Query().Any(u=>u.Email==request.Email))//if user with this email already exists
        {
            throw new ValidationException("Email already in use");//throw exception
        }

        var user = new User()//create new user
        {
            Email = request.Email,
            UserName = request.UserName,
        };
       user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);//hash password
       await _userRepository.Add(user);//add user to db
       return new AuthUserInfo(user.Id, user.UserName, user.Role);//return user info but without password
       
    }
}