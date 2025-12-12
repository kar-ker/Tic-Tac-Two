
using Microsoft.EntityFrameworkCore;

namespace DAL;

public static class DbHelper
{
    public static AppDbContext CreateCtx()
    {
        var connectionString = "Data Source=<%location%>tictactwo_db.db";
        connectionString = connectionString.Replace("<%location%>", FileHelper.BasePath);
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;
        var ctx = new AppDbContext(contextOptions);
        return ctx;
    }

}