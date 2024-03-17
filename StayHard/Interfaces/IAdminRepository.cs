using StayHard.Dto;
using StayHard.Models;

namespace StayHard.Interfaces;

public interface IAdminRepository
{
    ICollection<Admin> GetAdmins();
    Admin GetAdmin(int adminId);
    Admin GetAdmin(string adminUsername);
    bool AdminExists(int adminId);
    bool AdminExists(string adminUsername);
    Admin CreateAdminObject(AdminCreateDto adminCreateDto);
    bool CreateAdmin(Admin admin);
    bool DeleteAdmin(Admin admin);
    bool Save();
}