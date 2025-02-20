using Book_Management_API.Models;

namespace Book_Management_API.DataAccess
{
    public interface IAuthRepisotory
    {
        Task<string> Login(User user);
        Task Register(User user);
    }
}
