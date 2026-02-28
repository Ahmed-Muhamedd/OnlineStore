using OnlineStore.Common;
using OnlineStore.Dtos;

namespace OnlineStore.AuthService
{
    public interface IAuthService
    {
        Task<(bool IsSuccess, AuthDTO AuthData)> RegisterAsync(AuthRegisterDto Register);


        Task<(bool IsSuccess, AuthDTO AuthData)> LoginAsync(AuthLoginDto Login);

    }

    
}
