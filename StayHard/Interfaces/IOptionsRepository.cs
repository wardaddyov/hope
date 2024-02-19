namespace StayHard.Interfaces;

public interface IOptionsRepository
{
    bool GetDatabaseConnectionStatus();
    bool Migrate();
}