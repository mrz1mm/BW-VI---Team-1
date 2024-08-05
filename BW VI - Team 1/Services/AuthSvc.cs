using BW_VI___Team_1.Models.DTO;
using BW_VI___Team_1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class AuthSvc : IAuthSvc
    {
        private readonly LifePetDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthSvc(LifePetDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Register(RegisterDTO model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                return false;
            }

            var user = new User
            {
                Username = model.Username,
                Password = model.Password,
                Role = (Role)Enum.Parse(typeof(Role), model.Role)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Login(LoginDTO model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return true;
        }

        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
