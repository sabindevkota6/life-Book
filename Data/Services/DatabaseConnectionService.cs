using life_Book.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace life_Book.Data.Services;

public class DatabaseConnectionService
{
    private readonly DatabaseSchema _db;

    public DatabaseConnectionService(DatabaseSchema db)
    {
  _db = db;
    }

    // Initialize database connection and create tables
    public async Task<bool> InitializeAsync()
    {
   try
 {
await _db.Database.EnsureCreatedAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database initialization failed: {ex.Message}");
    return false;
        }
    }

    // Test database connection
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            return await _db.Database.CanConnectAsync();
     }
        catch
   {
   return false;
        }
 }

    // Get database path
    public string GetDatabasePath()
    {
     return _db.Database.GetConnectionString() ?? "Not configured";
    }

    // Close database connection
    public async Task CloseConnectionAsync()
    {
        await _db.Database.CloseConnectionAsync();
    }

 // Delete database (for testing/reset)
    public async Task<bool> DeleteDatabaseAsync()
    {
      try
        {
            return await _db.Database.EnsureDeletedAsync();
   }
        catch
   {
return false;
   }
    }
}
