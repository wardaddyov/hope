using StayHard.Models;

namespace StayHard.Interfaces;

public interface IAuthRepository
{
    string CreateAdminToken(Admin admin);
}