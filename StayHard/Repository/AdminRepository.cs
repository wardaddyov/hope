using StayHard.Data;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Repository;

public class AdminRepository : IAdminRepository
{
    private readonly DataContext _context;

    public AdminRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Admin> GetAdmins()
    {
        return _context.Admins.ToList();
    }

    public Admin GetAdmin(int adminId)
    {
        return _context.Admins.Where(a => a.Id == adminId).FirstOrDefault();
    }

    public Admin GetAdmin(string adminUsername)
    {
        return _context.Admins.Where(a => a.Username == adminUsername).FirstOrDefault();
    }

    public bool AdminExists(int adminId)
    {
        return _context.Admins.Any(a => a.Id == adminId);
    }

    public bool AdminExists(string adminUsername)
    {
        return _context.Admins.Any(a => a.Username == adminUsername);
    }

    public Admin CreateAdminObject(AdminCreateDto adminCreateDto)
    {
        var admin = new Admin()
        {
            Username = adminCreateDto.Username,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(adminCreateDto.password),
            AccessLevel = adminCreateDto.AccessLevel
        };

        return admin;
    }

    public bool CreateAdmin(Admin admin)
    {
        _context.Add(admin);
        return Save();
    }

    public bool DeleteAdmin(Admin admin)
    {
        _context.Remove(admin);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}