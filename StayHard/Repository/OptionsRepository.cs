using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StayHard.Data;
using StayHard.Interfaces;

namespace StayHard.Repository;

public class OptionsRepository : IOptionsRepository
{
    private readonly DataContext _context;

    public OptionsRepository(DataContext context)
    {
        _context = context;
    }
    public bool GetDatabaseConnectionStatus()
    {
        return _context.Database.CanConnect();
    }

    public bool Migrate()
    {
        try
        {
            _context.Database.Migrate();
        }
        catch(SqlException e)
        {
            Trace.WriteLine(e.Message);
            return false;
        }
        return true;
    }
}