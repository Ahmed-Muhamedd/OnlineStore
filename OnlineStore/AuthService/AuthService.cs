using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Common;
using OnlineStore.Data;
using OnlineStore.Dtos;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthService( AppDbContext context ,UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task<(bool IsSuccess, AuthDTO AuthData)> LoginAsync(AuthLoginDto Login)
        {
            ApplicationUser? User = await _userManager.FindByNameAsync(Login.Username);
            if (User == null || !await _userManager.CheckPasswordAsync(User, Login.Password))
                return (false, new AuthDTO { Message = new string[] { "Email or Password is incorrect!" } });

           return  await GenerateAuthAsync(User);
        }

        public async Task<(bool IsSuccess, AuthDTO AuthData)> RegisterAsync(AuthRegisterDto Register)
        {
            if (await _userManager.FindByNameAsync(Register.Username) is not null)
                return (false , new AuthDTO { Message =  new string[]{ "Username is already registered!" } });

            if (await _userManager.FindByEmailAsync(Register.Email) is not null)
                return (false, new AuthDTO { Message = new string[] { "Email is already registered!" } });

            ApplicationUser newUser = new ApplicationUser
            {
                UserName = Register.Username,
                Email = Register.Email,
                PhoneNumber = Register.PhoneNumber,
                FirstName = Register.FirstName,
                SecondName = Register.SecondName,
            };

           using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await _userManager.CreateAsync(newUser, Register.Password);
                if (!result.Succeeded)
                {
                    return (false, new AuthDTO { Message = result.Errors.Select(err => err.Description) });
                }

                if (Register.IsCustomer)
                {
                    await CreateCustomerAndCreateRecordAsync(newUser);
                }
                else
                {
                   await AssignRoleToAdminAsync(newUser);
                }


                await transaction.CommitAsync();

                return await GenerateAuthAsync(newUser);

            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();

                return (false, new AuthDTO { Message = new string[] { $"Registration failed: {ex.Message}" } });
            }
        }


        private async Task CreateCustomerAndCreateRecordAsync(ApplicationUser user)
        {
            await _userManager.AddToRoleAsync(user, "customer");
            this._context.Customers.Add(new Core.Models.Customer { UserID = user.Id });
            await this._context.SaveChangesAsync();
        }


        private async Task AssignRoleToAdminAsync(ApplicationUser user)
        {
            await _userManager.AddToRoleAsync(user, "admin");

        }

        private async Task<(bool IsSuccess, AuthDTO AuthData)> GenerateAuthAsync(ApplicationUser user)
        {

            var jwtSecurityToken = await CreateJWTTokenAsync(user);

            var roles = jwtSecurityToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            return (true, new AuthDTO
            {
                Email = user.Email!,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = roles,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName!,
                Name = $"{user.FirstName} {user.SecondName}"
            });
        }
        private async Task<JwtSecurityToken> CreateJWTTokenAsync(ApplicationUser user)
        {

            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            string FullName = $"{user.FirstName} {user.SecondName}";
            userClaims.Add(new Claim(ClaimTypes.Name, FullName));


            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("amf[aismfi[3MMin7KOAWMO363537@$^@!%#!FAFJAW#qq#)qkq( k_(jt(tqj(t"));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                audience: "SecureApi",
                issuer: "SecureApiUser",
                expires: DateTime.Now.AddMinutes(5),
                claims: userClaims,
                signingCredentials: signingCredentials
                );

            return token;
        }
    }
}
