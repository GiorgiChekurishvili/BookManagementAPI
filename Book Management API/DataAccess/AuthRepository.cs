using Book_Management_API.Entities.Context;
using Book_Management_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Book_Management_API.DataAccess
{
    public class AuthRepository : IAuthRepisotory
    {
        private readonly BookManagementDbContext _context;
        private readonly IConfiguration _config;
        public AuthRepository(BookManagementDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<string> Login(string email, string password)
        {
            var userData = await _context.Users.Where(x=>x.Email == email).FirstOrDefaultAsync();
            if (userData == null)
            {
                return null;
            }
            if(VerifyPasswordHash(password, userData!.PasswordHash!, userData.PasswordSalt!))
            {
                var token = CreateToken(userData);
                return token;
            }
            return null;
        }

        public async Task<int> Register(string email, string password)
        {
            var ifExists =  await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (ifExists != null)
            {
                return -1;
            }
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            User user = new User { Email = email, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;

        }
        private void CreatePasswordHash(string password , out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password,byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken
                (
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(3),
                    signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
