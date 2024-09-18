using SplitKaro.Models;

namespace SplitKaro.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
