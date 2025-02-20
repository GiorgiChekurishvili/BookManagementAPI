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

        // Authenticates a user based on email and password.
        // it also uses private methods VerifyPasswordHash() and CreateToken()
        // returns A JWT token if authentication is successful, otherwise null.
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

        // Registers a new user with an email and hashed password.
        //it also uses private method CreatePasswordHash()
        // returnsThe ID of the newly created user if successful, or -1 if the email is already in use.
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

        // Creates a password hash using HMACSHA512, it is implemeted in Register() method.
        private void CreatePasswordHash(string password , out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // Verifies a password against its stored hash and salt, it is implemented in Login() method.
        // returns True if the password matches the hash, otherwise false.
        private bool VerifyPasswordHash(string password,byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // Generates a JWT token for a given user, it is implemented in Login() method.
        // returns A JWT token as a string.
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
