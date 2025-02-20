using Book_Management_API.Models;

namespace Book_Management_API.DataAccess
{
    public interface IAuthRepisotory
    {
        Task<string> Login(string email, string password);
        Task<int> Register(string email, string password);
    }
}
