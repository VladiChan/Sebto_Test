using SebTest.Data;

namespace SebTest.Interfaces;

public interface ITokenService
{
    string BuildToken(User user);

}