using System.Threading.Tasks;
using ITProcesses.Models;

namespace ITProcesses.Services;

public interface IUserService
{
    public Task<User> Login(string login, string password);
    
}